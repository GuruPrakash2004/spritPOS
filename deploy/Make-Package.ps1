<#
    Make-Package.ps1 - run on the DEVELOPMENT PC. Writes ONE zip with what a server needs:
    backend (app, migrations, tools, Rpts, requirements), the production frontend build, the installer and DEPLOYMENT.md.
    Left out: .venv, .env, data\, uploads\, logs\, tests, caches, node_modules, install-settings.json.

    The frontend must be built with no API address (the page then calls /api on the address it was opened from).
    An existing frontend\dist is used only if it is newer than the sources and has no development URL in it; otherwise
    the build is made into a temporary folder (your frontend\dist is not touched).  The package is refused if a
    development URL (localhost:8001, 127.0.0.1:8001, or the VITE_API_URL of frontend\.env) is in the build.

    Usage:  Make-Package.cmd            (or)   powershell -ExecutionPolicy Bypass -File Make-Package.ps1 [-Rebuild] [-OutDir <folder>]
    Windows PowerShell 5.1 compatible, ASCII only.
#>
[CmdletBinding()]
param(
    [switch]$Rebuild,
    [string]$OutDir
)
$ErrorActionPreference = 'Stop'
$Deploy = Split-Path -Parent $PSCommandPath
$Root = Split-Path -Parent $Deploy
if (-not $OutDir) { $OutDir = Join-Path $Deploy 'out' }

function Say([string]$m, [string]$c = 'Gray') { Write-Host $m -ForegroundColor $c }
function Fail([string]$m) { Say "FAIL  $m" 'Red'; exit 1 }
function Quote-Arg([string]$a) {
    if ($a.Length -gt 0 -and $a -notmatch '[\s"]') { return $a }
    $sb = New-Object System.Text.StringBuilder; [void]$sb.Append('"'); $bs = 0
    foreach ($ch in $a.ToCharArray()) {
        if ($ch -eq [char]'\') { $bs++; continue }
        if ($ch -eq [char]'"') { [void]$sb.Append('\' * ($bs * 2 + 1)); [void]$sb.Append('"'); $bs = 0; continue }
        if ($bs -gt 0) { [void]$sb.Append('\' * $bs); $bs = 0 }
        [void]$sb.Append($ch)
    }
    if ($bs -gt 0) { [void]$sb.Append('\' * ($bs * 2)) }
    [void]$sb.Append('"'); return $sb.ToString()
}
function Run([string]$file, [string[]]$a, [string]$wd, [hashtable]$envv) {
    $psi = New-Object System.Diagnostics.ProcessStartInfo
    $psi.FileName = $file; $psi.Arguments = (($a | ForEach-Object { Quote-Arg $_ }) -join ' ')
    $psi.WorkingDirectory = $wd; $psi.UseShellExecute = $false; $psi.CreateNoWindow = $true
    $psi.RedirectStandardOutput = $true; $psi.RedirectStandardError = $true; $psi.RedirectStandardInput = $true
    foreach ($k in $envv.Keys) { $psi.EnvironmentVariables[$k] = [string]$envv[$k] }
    $p = New-Object System.Diagnostics.Process; $p.StartInfo = $psi; [void]$p.Start(); $p.StandardInput.Close()
    $o = $p.StandardOutput.ReadToEndAsync(); $e = $p.StandardError.ReadToEndAsync()
    while (-not $p.WaitForExit(3000)) { Write-Host -NoNewline '.' }
    Write-Host ''
    $p.WaitForExit()
    return [pscustomobject]@{ ExitCode = $p.ExitCode; Out = $o.Result; Err = $e.Result }
}
function Find-DevUrls([string]$dist, [string[]]$extra) {
    $needles = @('localhost:8001', '127.0.0.1:8001') + $extra
    $bad = @()
    foreach ($f in @(Get-ChildItem -LiteralPath $dist -Recurse -File -Include *.js, *.html, *.css, *.json)) {
        $t = [IO.File]::ReadAllText($f.FullName)
        foreach ($n in $needles) { if ($n -and $t.Contains($n)) { $bad += ("{0} ({1})" -f $f.Name, $n) } }
    }
    return $bad
}

Say "Packaging vLQMS from $Root" 'Cyan'
foreach ($rel in @('backend\app\main.py', 'backend\requirements.txt', 'frontend\package.json', 'deploy\Install-vLQMS.ps1')) {
    if (-not (Test-Path -LiteralPath (Join-Path $Root $rel))) { Fail "missing $rel - run this from a full checkout" }
}

# The development API addresses that must not be in a production build.
$devUrls = @()
foreach ($e in @('.env', '.env.local', '.env.production')) {
    $ef = Join-Path $Root "frontend\$e"
    if (Test-Path -LiteralPath $ef) {
        foreach ($l in (Get-Content -LiteralPath $ef)) {
            if ($l -match '^\s*VITE_(CENTRAL_)?API_URL\s*=\s*(\S+)') { $devUrls += $Matches[2].Trim('"').Trim("'").TrimEnd('/') }
        }
    }
}
$devUrls = @($devUrls | Where-Object { $_ } | Select-Object -Unique)

# ---- the frontend build ---------------------------------------------------------------------------------------------
$feDist = Join-Path $Root 'frontend\dist'
$useExisting = $false
if (-not $Rebuild -and (Test-Path -LiteralPath (Join-Path $feDist 'index.html'))) {
    $built = (Get-Item -LiteralPath (Join-Path $feDist 'index.html')).LastWriteTime
    $newest = Get-ChildItem -LiteralPath (Join-Path $Root 'frontend') -Recurse -File -ErrorAction SilentlyContinue |
        Where-Object { $_.FullName -notmatch '\\(node_modules|dist)\\' -and $_.Name -notmatch '^\.env' } | Sort-Object LastWriteTime -Descending | Select-Object -First 1
    if ($newest.LastWriteTime -lt $built -and @(Find-DevUrls $feDist $devUrls).Count -eq 0) { $useExisting = $true }
}
$staging = Join-Path ([IO.Path]::GetTempPath()) ('vlqms-pkg-' + [guid]::NewGuid().ToString('N').Substring(0, 8))
$distSrc = $feDist
$tempBuild = $null
if ($useExisting) { Say "Using the existing frontend build ($feDist): newer than the sources, no development URLs." 'Green' }
else {
    $npm = Get-Command npm.cmd -ErrorAction SilentlyContinue
    if (-not $npm) { Fail 'npm (Node.js) is not installed on this PC, and the frontend build is missing, stale or has a development URL.' }
    $tempBuild = Join-Path ([IO.Path]::GetTempPath()) ('vlqms-fe-' + [guid]::NewGuid().ToString('N').Substring(0, 8))
    Say 'Building the frontend with no API address (into a temporary folder; frontend\dist is left alone)...' 'Cyan'
    # A single space (not an empty value) overrides frontend\.env: api.js trims it and falls back to /api.
    $r = Run 'cmd.exe' @('/c', 'npm.cmd', 'run', 'build', '--', '--outDir', $tempBuild, '--emptyOutDir') (Join-Path $Root 'frontend') @{ VITE_API_URL = ' '; VITE_CENTRAL_API_URL = ' ' }
    if ($r.ExitCode -ne 0) { Say $r.Out; Say $r.Err 'Red'; Fail "npm run build failed (exit $($r.ExitCode))" }
    $distSrc = $tempBuild
}
if (-not (Test-Path -LiteralPath (Join-Path $distSrc 'index.html'))) { Fail "no index.html in $distSrc" }
$bad = @(Find-DevUrls $distSrc $devUrls)
if ($bad.Count) {
    Say 'The frontend build contains a development API address:' 'Red'; $bad | Select-Object -First 8 | ForEach-Object { Say "   $_" 'Red' }
    Fail 'Refusing to package it: every PC but the server would call itself. Rebuild with no VITE_API_URL.'
}
Say 'OK    frontend build has no development URLs' 'Green'

# ---- stage the files ------------------------------------------------------------------------------------------------
New-Item -ItemType Directory -Path $staging -Force | Out-Null
function Copy-Tree([string]$src, [string]$dst, [string[]]$xd = @(), [string[]]$xf = @()) {
    if (-not (Test-Path -LiteralPath $src)) { return }
    $a = @($src, $dst, '/E', '/NFL', '/NDL', '/NJH', '/NJS', '/NP', '/R:1', '/W:1')
    if ($xd.Count) { $a += '/XD'; $a += $xd }
    if ($xf.Count) { $a += '/XF'; $a += $xf }
    $r = Run 'robocopy.exe' $a $Root @{}
    if ($r.ExitCode -ge 8) { Fail "robocopy failed copying $src (exit $($r.ExitCode))" }
}
$be = Join-Path $Root 'backend'
$xdirs = @('__pycache__', '.venv', 'venv', 'data', 'uploads', 'logs', 'tests', 'node_modules', '.pytest_cache', 'backtest_out', 'obj', '.git')
$xfiles = @('*.pyc', '*.pyo', '.env', '.env.*', '*.log', '*.pem', 'secret.key', 'install-settings.json')
foreach ($d in @('app', 'migrations', 'tools', 'Rpts', 'scripts')) { Copy-Tree (Join-Path $be $d) (Join-Path $staging "backend\$d") $xdirs $xfiles }
foreach ($f in @('requirements.txt', 'requirements-ai.txt', '.env.example')) {
    if (Test-Path -LiteralPath (Join-Path $be $f)) { New-Item -ItemType Directory -Path (Join-Path $staging 'backend') -Force | Out-Null; Copy-Item -LiteralPath (Join-Path $be $f) -Destination (Join-Path $staging "backend\$f") }
}
Copy-Tree $distSrc (Join-Path $staging 'frontend\dist') @() @()
New-Item -ItemType Directory -Path (Join-Path $staging 'deploy') -Force | Out-Null
foreach ($sub in @('installers', 'wheels')) { Copy-Tree (Join-Path $Deploy $sub) (Join-Path $staging "deploy\$sub") @() @() }   # offline files, when present
foreach ($f in @('Install-vLQMS.cmd', 'Install-vLQMS.ps1', 'README.md')) { Copy-Item -LiteralPath (Join-Path $Deploy $f) -Destination (Join-Path $staging "deploy\$f") }
if (Test-Path -LiteralPath (Join-Path $Root 'DEPLOYMENT.md')) { Copy-Item -LiteralPath (Join-Path $Root 'DEPLOYMENT.md') -Destination $staging }

# ---- safety scan of what is about to ship -----------------------------------------------------------------------------
$files = @(Get-ChildItem -LiteralPath $staging -Recurse -File)
$forbidden = @($files | Where-Object { $_.Name -match '^\.env$|^\.env\.(?!example)|^secret\.key$|\.pem$|^install-settings\.json$' -or $_.FullName -match '\\(\.venv|data|uploads)\\' })
if ($forbidden.Count) { $forbidden | ForEach-Object { Say "   $($_.FullName)" 'Red' }; Fail 'secret or runtime files ended up in the package' }
$hits = @()
foreach ($f in ($files | Where-Object { $_.Extension -match '^\.(py|md|json|txt|ini|example|ps1|cmd|js|html)$' -and $_.Length -lt 3MB })) {
    $isCode = ($f.Extension -eq '.py' -or $f.Extension -eq '.js')   # code assigns variables named like secrets; only config-style files are checked for values
    $t = [IO.File]::ReadAllText($f.FullName)
    if ($t -match '-----BEGIN [A-Z ]*PRIVATE KEY-----') { $hits += "private key block in $($f.Name)" }
    if (-not $isCode -and $t -match '(?m)^\s*(DB_PASSWORD_ENC|DB_PASSWORD|JWT_SECRET|SECRET_KEY|SMTP_PASSWORD)\s*=\s*(?!change-this|your_|\s*$|#)\S{12,}') { $hits += "a real-looking secret assignment in $($f.Name)" }
}
if ($hits.Count) { $hits | ForEach-Object { Say "   $_" 'Red' }; Fail 'possible secrets found in the package' }
$ips = @()
foreach ($f in ($files | Where-Object { $_.Extension -match '^\.(py|md|json|ini|example|ps1)$' -and $_.Length -lt 1MB })) {
    if ([IO.File]::ReadAllText($f.FullName) -match '\b(192\.168|10\.\d{1,3}|172\.(1[6-9]|2\d|3[01]))\.\d{1,3}\.\d{1,3}\b') { $ips += $f.Name }
}
if ($ips.Count) { Say ("WARN  private IP-looking text in: " + (($ips | Select-Object -Unique | Select-Object -First 8) -join ', ') + ' (check it is a placeholder)') 'Yellow' }
else { Say 'OK    no secrets, keys, .env, venv or runtime data in the package' 'Green' }

# ---- the zip --------------------------------------------------------------------------------------------------------
if (-not (Test-Path -LiteralPath $OutDir)) { New-Item -ItemType Directory -Path $OutDir -Force | Out-Null }
$zipPath = Join-Path $OutDir ('vLQMS-' + (Get-Date).ToString('yyyyMMdd-HHmm') + '.zip')
if (Test-Path -LiteralPath $zipPath) { Remove-Item -LiteralPath $zipPath -Force }
Add-Type -AssemblyName System.IO.Compression
Add-Type -AssemblyName System.IO.Compression.FileSystem
$zip = [IO.Compression.ZipFile]::Open($zipPath, 'Create')
try {
    $base = $staging.TrimEnd('\') + '\'
    foreach ($f in $files) {
        $entry = $f.FullName.Substring($base.Length).Replace('\', '/')
        [void][IO.Compression.ZipFileExtensions]::CreateEntryFromFile($zip, $f.FullName, $entry, [IO.Compression.CompressionLevel]::Optimal)
    }
} finally { $zip.Dispose() }
Remove-Item -LiteralPath $staging -Recurse -Force -ErrorAction SilentlyContinue
if ($tempBuild) { Remove-Item -LiteralPath $tempBuild -Recurse -Force -ErrorAction SilentlyContinue }

$z = [IO.Compression.ZipFile]::OpenRead($zipPath)
$n = $z.Entries.Count
$top = $z.Entries | Group-Object { $_.FullName.Split('/')[0] } | ForEach-Object { "{0} ({1} files)" -f $_.Name, $_.Count }
$z.Dispose()
Say ("DONE  {0}  ({1:N1} MB, {2} files)" -f $zipPath, ((Get-Item -LiteralPath $zipPath).Length / 1MB), $n) 'Green'
Say ("      contains: " + ($top -join ', ')) 'Gray'
Say 'Next: copy the zip to the server, extract it to a LOCAL disk (e.g. C:\vLQMS), run deploy\Install-vLQMS.cmd.' 'Cyan'
exit 0
