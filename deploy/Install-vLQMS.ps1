<#
    Install-vLQMS.ps1 - one-click installer / updater for the e-vLQMS ERP backend (Windows service) on a PC or server.

    Menu (double-click Install-vLQMS.cmd):  1 Install / Repair   2 Update   3 Status   4 Uninstall
    Unattended:  -Mode Install -AppPath C:\vLQMS -Port 8001 -Unattended [-DbServer .. -DbName .. -DbUser .. (password: -DbPassword or env VLQMS_DB_PASSWORD)]
    Exit code: 0 = healthy, 1 = failed.   -DryRun only reports what it would do.

    The app itself is not changed. The database connection is bound by the app's own one-time "Database setup" page
    (it seals the password with the server's key); the DB questions here only CHECK that the details work.
    Windows PowerShell 5.1 compatible, ASCII only.
#>
[CmdletBinding()]
param(
    [ValidateSet('Menu', 'Install', 'Update', 'Status', 'Uninstall')][string]$Mode = 'Menu',
    [string]$AppPath,
    [int]$Port = 0,
    [string]$ServiceName = 'vLQMS',
    [string]$NssmPath = '',          # where nssm.exe lives or goes (default C:\nssm\nssm.exe)
    [string]$DbServer,
    [string]$DbPort,
    [string]$DbName,
    [string]$DbUser,
    [string]$DbPassword,
    [string]$ServiceUser,
    [string]$ServicePassword,
    [switch]$Offline,
    [switch]$WithAiModel,
    [switch]$Unattended,
    [switch]$DryRun,
    [switch]$LoadOnly
)

# Captured at SCRIPT scope: inside a function $PSBoundParameters would be that function's own (empty) set.
$script:Bound = @{} + $PSBoundParameters
$script:ScriptPath = $PSCommandPath
$script:DeployDir = Split-Path -Parent $PSCommandPath
$ErrorActionPreference = 'Stop'
$ProgressPreference = 'SilentlyContinue'

# ---- pinned downloads ---------------------------------------------------------------------------------------------
$script:PY_FILE = 'python-3.12.10-amd64.exe'
$script:PY_URLS = @('https://www.python.org/ftp/python/3.12.10/python-3.12.10-amd64.exe')
$script:PY_SHA = '67B5635E80EA51072B87941312D00EC8927C4DB9BA18938F7AD2D27B328B95FB'
$script:NSSM_FILE = 'nssm-2.24-101-g897c7ad.zip'
# nssm.cc is often down (and its /release/ path is gone): the CI path first, then the Internet Archive's raw copy.
$script:NSSM_URLS = @('https://nssm.cc/ci/nssm-2.24-101-g897c7ad.zip',
    'https://web.archive.org/web/2023id_/https://nssm.cc/ci/nssm-2.24-101-g897c7ad.zip')
$script:NSSM_SHA = '99F5045FFFBFFB745D67FE3A065A953C4A3D9C253B868892D9B685B0EE7D07B8'
$script:ODBC_FILE = 'msodbcsql.msi'
$script:ODBC_URLS = @('https://go.microsoft.com/fwlink/?linkid=2249006')   # ODBC Driver 18 x64; trusted by Microsoft signature

# ---- state ------------------------------------------------------------------------------------------------------
$script:DryRun = [bool]$DryRun
$script:Unattended = [bool]$Unattended
$script:Offline = [bool]$Offline
$script:WithAi = [bool]$WithAiModel
$script:SvcName = $ServiceName
$script:LogFile = $null
$script:LogBuf = New-Object System.Collections.ArrayList
$script:Secrets = New-Object System.Collections.ArrayList
$script:Tally = @{}
$script:Manual = New-Object System.Collections.ArrayList
$script:Nssm = $(if ($NssmPath) { $NssmPath } else { 'C:\nssm\nssm.exe' })
$script:Settings = @{}
$script:App = $null; $script:Backend = $null; $script:VenvPy = $null; $script:BasePy = $null; $script:PortNo = 0

# ===================================================================================================================
#  Output
# ===================================================================================================================
function Mask([string]$t) {
    foreach ($s in $script:Secrets) { if ($s -and $s.Length -ge 3) { $t = $t.Replace($s, '***') } }
    return $t
}
function Add-LogLine([string]$l) {
    $t = (Get-Date).ToString('yyyy-MM-dd HH:mm:ss') + ' ' + $l
    if ($script:LogFile) { try { [IO.File]::AppendAllText($script:LogFile, $t + "`r`n") } catch { $null = $_ } }
    else { [void]$script:LogBuf.Add($t) }
}
function Say([string]$m, [string]$color = 'Gray') {
    $line = Mask $m
    Write-Host $line -ForegroundColor $color
    Add-LogLine $line
}
function Set-LogFolder([string]$dir) {
    if ($script:DryRun) { return }   # a dry run writes nothing, not even a log
    try {
        if (-not (Test-Path -LiteralPath $dir)) { New-Item -ItemType Directory -Path $dir -Force | Out-Null }
        $script:LogFile = Join-Path $dir ('install-' + (Get-Date).ToString('yyyyMMdd-HHmmss') + '.log')
        foreach ($l in $script:LogBuf) { [IO.File]::AppendAllText($script:LogFile, $l + "`r`n") }
        $script:LogBuf.Clear()
    } catch { $script:LogFile = $null }
}
function Step([string]$status, [string]$name, [string]$detail = '') {
    $colors = @{ OK = 'Green'; DONE = 'Cyan'; WARN = 'Yellow'; FAIL = 'Red'; DRY = 'Magenta'; INFO = 'Gray' }
    $msg = ('[{0,-4}] {1}' -f $status, $name)
    if ($detail) { $msg += ' - ' + $detail }
    Say $msg $colors[$status]
    $script:Tally[$status] = 1 + [int]$script:Tally[$status]
}
function Stop-Install([string]$name, [string]$detail, [string]$hint = '') {
    Step FAIL $name $detail
    if ($hint) { Say ('       Hint: ' + $hint) 'Yellow' }
    throw (New-Object System.Exception 'VLQMS_FATAL')
}
function Change([string]$what) {
    # Every step that modifies the machine asks here first: in a dry run it only says what it would do.
    if ($script:DryRun) { Step DRY $what 'would be done'; return $false }
    return $true
}
function Add-Manual([string]$t) { if (-not $script:Manual.Contains($t)) { [void]$script:Manual.Add($t) } }

# ===================================================================================================================
#  Helpers: quoting, processes, prompts
# ===================================================================================================================
function Quote-Arg([string]$a) {
    # Quote one argument for a Windows command line (CommandLineToArgvW rules: spaces, quotes, trailing backslashes).
    if ($null -eq $a) { return '""' }
    if ($a.Length -gt 0 -and $a -notmatch '[\s"]') { return $a }
    $sb = New-Object System.Text.StringBuilder
    [void]$sb.Append('"')
    $bs = 0
    foreach ($ch in $a.ToCharArray()) {
        if ($ch -eq [char]'\') { $bs++; continue }
        if ($ch -eq [char]'"') { [void]$sb.Append('\' * ($bs * 2 + 1)); [void]$sb.Append('"'); $bs = 0; continue }
        if ($bs -gt 0) { [void]$sb.Append('\' * $bs); $bs = 0 }
        [void]$sb.Append($ch)
    }
    if ($bs -gt 0) { [void]$sb.Append('\' * ($bs * 2)) }
    [void]$sb.Append('"')
    return $sb.ToString()
}
function Strip-Nul([string]$s) { if ($null -eq $s) { return '' }; return ($s -replace "`0", '') }

function Invoke-Proc {
    # Run an executable with redirected streams (no ::new(), no shell). Dots show progress on long jobs.
    param([string]$File, [string[]]$ArgList = @(), [string]$WorkDir, [int]$TimeoutSec = 600, [hashtable]$EnvVars, [switch]$Dots)
    $psi = New-Object System.Diagnostics.ProcessStartInfo
    $psi.FileName = $File
    $psi.Arguments = (($ArgList | ForEach-Object { Quote-Arg $_ }) -join ' ')
    $psi.UseShellExecute = $false
    $psi.RedirectStandardOutput = $true
    $psi.RedirectStandardError = $true
    $psi.RedirectStandardInput = $true
    $psi.CreateNoWindow = $true
    if ($WorkDir) { $psi.WorkingDirectory = $WorkDir }
    if ($EnvVars) { foreach ($k in $EnvVars.Keys) { $psi.EnvironmentVariables[$k] = [string]$EnvVars[$k] } }
    $p = New-Object System.Diagnostics.Process
    $p.StartInfo = $psi
    try { [void]$p.Start() }
    catch { return [pscustomobject]@{ ExitCode = -1; Out = ''; Err = $_.Exception.Message; TimedOut = $false } }
    $p.StandardInput.Close()
    $o = $p.StandardOutput.ReadToEndAsync()
    $e = $p.StandardError.ReadToEndAsync()
    $sw = [Diagnostics.Stopwatch]::StartNew()
    $timed = $false; $ticks = 0
    while (-not $p.WaitForExit(3000)) {
        if ($Dots) { Write-Host -NoNewline '.'; $ticks++ }
        if ($sw.Elapsed.TotalSeconds -gt $TimeoutSec) { try { $p.Kill() } catch { $null = $_ }; $timed = $true; break }
    }
    if ($ticks) { Write-Host '' }
    try { $p.WaitForExit() } catch { $null = $_ }
    $out = ''; $err = ''
    try { if ($o.Wait(5000)) { $out = $o.Result } } catch { $null = $_ }
    try { if ($e.Wait(5000)) { $err = $e.Result } } catch { $null = $_ }
    return [pscustomobject]@{ ExitCode = $p.ExitCode; Out = (Strip-Nul $out); Err = (Strip-Nul $err); TimedOut = $timed }
}
function Last-Lines([string]$text, [int]$n = 15) {
    $l = @(($text -split "\r?\n") | Where-Object { $_.Trim() })
    if ($l.Count -gt $n) { $l = $l[($l.Count - $n)..($l.Count - 1)] }
    return ($l -join "`r`n")
}
function Ask([string]$prompt, [string]$default = '') {
    if ($script:Unattended) { return $default }
    $suffix = ''; if ($default) { $suffix = " [$default]" }
    $v = Read-Host ($prompt + $suffix)
    if ([string]::IsNullOrWhiteSpace($v)) { return $default }
    return $v.Trim()
}
function Ask-Secret([string]$prompt) {
    if ($script:Unattended) { return '' }
    $s = Read-Host $prompt -AsSecureString
    $b = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($s)
    try { $v = [Runtime.InteropServices.Marshal]::PtrToStringAuto($b) } finally { [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($b) }
    if ($v) { [void]$script:Secrets.Add($v) }
    return $v
}
function Ask-YesNo([string]$prompt, [bool]$default = $true) {
    if ($script:Unattended) { return $default }
    $d = 'Y/n'; if (-not $default) { $d = 'y/N' }
    $v = Read-Host "$prompt [$d]"
    if ([string]::IsNullOrWhiteSpace($v)) { return $default }
    return ($v.Trim().Substring(0, 1).ToLower() -eq 'y')
}
function Test-Admin {
    $id = [Security.Principal.WindowsIdentity]::GetCurrent()
    return (New-Object Security.Principal.WindowsPrincipal($id)).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
}
function New-Secret([int]$bytes = 48) {
    $b = New-Object byte[] $bytes
    $rng = [Security.Cryptography.RandomNumberGenerator]::Create()
    $rng.GetBytes($b); $rng.Dispose()
    return ([Convert]::ToBase64String($b)).TrimEnd('=').Replace('+', '-').Replace('/', '_')
}
function Refresh-Path {
    $m = [Environment]::GetEnvironmentVariable('Path', 'Machine'); $u = [Environment]::GetEnvironmentVariable('Path', 'User')
    $env:Path = (@($m, $u) | Where-Object { $_ }) -join ';'
}
function Write-Utf8([string]$path, [string]$text) {
    [IO.File]::WriteAllText($path, $text, (New-Object System.Text.UTF8Encoding($false)))
}

# ===================================================================================================================
#  Saved answers (never secrets)
# ===================================================================================================================
function Settings-Path { return (Join-Path $script:DeployDir 'install-settings.json') }
function Load-Settings {
    $script:Settings = @{}
    $p = Settings-Path
    if (Test-Path -LiteralPath $p) {
        try {
            $o = Get-Content -LiteralPath $p -Raw | ConvertFrom-Json
            foreach ($pr in $o.PSObject.Properties) { $script:Settings[$pr.Name] = $pr.Value }
        } catch { Step WARN 'Saved answers' 'install-settings.json is unreadable; ignoring it' }
    }
}
function Save-Settings {
    if ($script:DryRun) { return }
    try { Write-Utf8 (Settings-Path) ((New-Object PSObject -Property $script:Settings) | ConvertTo-Json) } catch { Step WARN 'Saved answers' ('not saved: ' + $_.Exception.Message) }
}

# ===================================================================================================================
#  Environment checks
# ===================================================================================================================
function Test-OS {
    $os = Get-CimInstance Win32_OperatingSystem
    if (-not [Environment]::Is64BitOperatingSystem) { Stop-Install 'Operating system' 'a 64-bit Windows is required' 'Install on a 64-bit Windows 8.1 / Server 2012 R2 or newer.' }
    if ([version]$os.Version -lt [version]'6.3') { Stop-Install 'Operating system' ("{0} is too old" -f $os.Caption) 'Windows 8.1 / Server 2012 R2 or newer is required.' }
    if ($PSVersionTable.PSVersion.Major -lt 5) { Stop-Install 'PowerShell' ('version ' + $PSVersionTable.PSVersion + ' is too old') 'Install Windows Management Framework 5.1.' }
    Step OK 'Operating system' ("{0} ({1}), 64-bit, PowerShell {2}" -f $os.Caption.Trim(), $os.Version, $PSVersionTable.PSVersion)
}

function Test-AppFolder([string]$p) {
    # Returns a list of problems (empty = usable as the application folder).
    $problems = @()
    try { $full = [IO.Path]::GetFullPath($p).TrimEnd('\') } catch { return @('this is not a valid path') }
    if ($full.StartsWith('\\')) { return @('a network (UNC) path: a Windows service cannot rely on it; use a local disk') }
    $root = [IO.Path]::GetPathRoot($full)
    if ($root.Length -ge 2 -and $root[1] -eq ':') {
        $dev = $root.Substring(0, 2).ToUpper()
        $disk = Get-CimInstance Win32_LogicalDisk -Filter "DeviceID='$dev'" -ErrorAction SilentlyContinue
        if (-not $disk) { $problems += "drive $dev is not visible to this administrator session (a mapped drive?); use a local disk" }
        elseif ($disk.DriveType -ne 3) {
            $kind = @{ 2 = 'removable'; 4 = 'network (mapped)'; 5 = 'optical'; 6 = 'RAM' }[[int]$disk.DriveType]
            $problems += "drive $dev is a $kind drive; a service needs a fixed local disk"
        } else {
            $need = 3GB; if ($script:WithAi) { $need = 8GB }
            if ($disk.FreeSpace -lt $need) { $problems += ("only {0:N1} GB free on {1}; need {2} GB" -f ($disk.FreeSpace / 1GB), $dev, [int]($need / 1GB)) }
        }
        $subst = (Invoke-Proc 'cmd.exe' @('/c', 'subst')).Out
        if ($subst -match ('(?im)^' + [regex]::Escape($dev) + '\\: =>')) { $problems += "drive $dev is a SUBST alias; a service cannot see it" }
    } else { $problems += 'the path has no drive letter' }
    if ($full.Length -gt 120) { $problems += ("the path is {0} characters; keep it under 120 (Windows path limit)" -f $full.Length) }
    if ($full -match '[^\x20-\x7E]') { $problems += 'the path has non-ASCII characters; use plain letters and digits' }
    if (-not (Test-Path -LiteralPath $full -PathType Container)) { $problems += 'the folder does not exist' }
    else {
        foreach ($rel in @('backend\app\main.py', 'backend\requirements.txt', 'frontend\dist\index.html')) {
            if (-not (Test-Path -LiteralPath (Join-Path $full $rel))) { $problems += "missing $rel" }
        }
    }
    return $problems
}
function Browse-Folder([string]$start) {
    try {
        Add-Type -AssemblyName System.Windows.Forms
        $d = New-Object System.Windows.Forms.FolderBrowserDialog
        $d.Description = 'Choose the vLQMS application folder (the one that contains backend and frontend)'
        if ($start -and (Test-Path -LiteralPath $start)) { $d.SelectedPath = $start }
        if ($d.ShowDialog() -eq [System.Windows.Forms.DialogResult]::OK) { return $d.SelectedPath }
    } catch { $null = $_ }
    return ''
}
function Resolve-AppFolder {
    $cand = $AppPath
    $fromUser = [bool]$AppPath
    if (-not $cand) { $cand = [string]$script:Settings['AppPath'] }
    if (-not $cand) { $cand = Split-Path -Parent $script:DeployDir }
    for ($i = 0; $i -lt 6; $i++) {
        $cand = $cand.Trim().Trim('"').Trim("'").TrimEnd('')
        if ($cand.Length -eq 2 -and $cand[1] -eq ':') { $cand += '' }
        $problems = @(Test-AppFolder $cand)
        if ($problems.Count -eq 0) {
            if ($script:Unattended -or $fromUser) { break }
            $v = Ask 'Application folder (Enter to accept, type a path, or ? to browse)' $cand
            if ($v -eq '?') { $v = Browse-Folder $cand }
            if (-not $v -or $v -eq $cand) { break }
            $cand = $v; $fromUser = $true
            continue
        }
        foreach ($pr in $problems) { Step WARN 'Application folder' $pr }
        if ($script:Unattended) { Stop-Install 'Application folder' ($problems -join '; ') 'Pass a valid -AppPath (a local folder holding backend\ and frontend\dist\).' }
        $v = Ask 'Application folder (type a path, or ? to browse)' $cand
        if ($v -eq '?') { $v = Browse-Folder $cand }
        if ($v) { $cand = $v }
        $fromUser = $true
    }
    $problems = @(Test-AppFolder $cand)
    if ($problems.Count -gt 0) { Stop-Install 'Application folder' ($problems -join '; ') 'Extract the package to a local disk, for example C:LQMS.' }
    $full = [IO.Path]::GetFullPath($cand).TrimEnd('')
    $script:App = $full
    $script:Backend = Join-Path $full 'backend'
    $script:VenvPy = Join-Path $script:Backend '.venv\Scripts\python.exe'
    $script:Settings['AppPath'] = $full
    Set-LogFolder (Join-Path $script:Backend 'logs')
    Step OK 'Application folder' $full
}

# ===================================================================================================================
#  Downloads and prerequisites
# ===================================================================================================================
function Init-Network {
    try {
        [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
        [Net.WebRequest]::DefaultWebProxy = [Net.WebRequest]::GetSystemWebProxy()
        [Net.WebRequest]::DefaultWebProxy.Credentials = [Net.CredentialCache]::DefaultCredentials
    } catch { $null = $_ }
}
function Download-File([string[]]$urls, [string]$dest) {
    $dir = Split-Path -Parent $dest
    if (-not (Test-Path -LiteralPath $dir)) { New-Item -ItemType Directory -Path $dir -Force | Out-Null }
    $part = $dest + '.part'
    $last = ''
    foreach ($u in $urls) {
        for ($try = 1; $try -le 3; $try++) {
            Say ("       downloading $u (try $try)") 'DarkGray'
            $wc = New-Object System.Net.WebClient
            try {
                $wc.Proxy = [Net.WebRequest]::GetSystemWebProxy()
                $wc.Proxy.Credentials = [Net.CredentialCache]::DefaultCredentials
                $wc.Headers['User-Agent'] = 'vLQMS-installer'
                $t = $wc.DownloadFileTaskAsync($u, $part)
                while (-not $t.Wait(3000)) { Write-Host -NoNewline '.' }
                Write-Host ''
                if ($t.IsFaulted) { throw $t.Exception.GetBaseException() }
                Move-Item -LiteralPath $part -Destination $dest -Force
                return $true
            } catch {
                Write-Host ''
                $last = $_.Exception.Message
                if (Test-Path -LiteralPath $part) { Remove-Item -LiteralPath $part -Force -ErrorAction SilentlyContinue }
                Start-Sleep -Seconds (2 * $try)
            } finally { $wc.Dispose() }
        }
    }
    Say ("       download failed: $last") 'Yellow'
    return $false
}
function Get-Installer([string]$file, [string[]]$urls, [string]$sha, [string]$publisher) {
    # Returns the local path of a verified installer (pinned SHA-256 and/or Authenticode publisher), or stops.
    $dir = Join-Path $script:DeployDir 'installers'
    $path = Join-Path $dir $file
    if (-not (Test-Path -LiteralPath $path)) {
        if ($script:DryRun) { Step DRY "Download $file" 'would be downloaded and verified'; return $null }
        if ($script:Offline) { Stop-Install "Installer $file" "not found in $dir (offline mode)" "Copy $file into deploy\installers on a PC with internet access." }
        if (-not (Download-File $urls $path)) { Stop-Install "Download $file" 'could not download it' 'Check the internet / proxy, or copy the file into deploy\installers and run again.' }
    }
    if ($sha) {
        $h = (Get-FileHash -LiteralPath $path -Algorithm SHA256).Hash
        if ($h -ne $sha) { Remove-Item -LiteralPath $path -Force -ErrorAction SilentlyContinue; Stop-Install "Verify $file" "SHA-256 mismatch ($h)" 'The file was corrupted or replaced; it was deleted. Run again.' }
    }
    if ($publisher) {
        $sig = Get-AuthenticodeSignature -LiteralPath $path
        if ($sig.Status -ne 'Valid' -or $sig.SignerCertificate.Subject -notmatch $publisher) {
            Remove-Item -LiteralPath $path -Force -ErrorAction SilentlyContinue
            Stop-Install "Verify $file" ("signature is '{0}' / {1}" -f $sig.Status, $sig.SignerCertificate.Subject) 'Not signed by the expected publisher; it was deleted.'
        }
    }
    return $path
}

function Get-MachinePython {
    # Only a machine-wide (all users) CPython 3.12 x64 counts: a per-user Python cannot be used by a service running as SYSTEM.
    foreach ($k in @('HKLM:\SOFTWARE\Python\PythonCore\3.12\InstallPath', 'HKLM:\SOFTWARE\WOW6432Node\Python\PythonCore\3.12\InstallPath')) {
        $i = Get-ItemProperty -LiteralPath $k -ErrorAction SilentlyContinue
        if (-not $i) { continue }
        $exe = $i.ExecutablePath
        if (-not $exe -and $i.'(default)') { $exe = Join-Path $i.'(default)' 'python.exe' }
        if (-not $exe -or -not (Test-Path -LiteralPath $exe)) { continue }
        if ($exe -match '\\Users\\|\\AppData\\') { continue }
        $r = Invoke-Proc $exe @('-I', '-c', 'import sys,struct;print(sys.version_info[0],sys.version_info[1],struct.calcsize("P")*8)') -TimeoutSec 60
        if ($r.ExitCode -eq 0 -and $r.Out.Trim() -eq '3 12 64') { return $exe }
    }
    return $null
}
function Ensure-Python {
    $py = Get-MachinePython
    if ($py) { Step OK 'Python 3.12 (all users)' $py; $script:BasePy = $py; return }
    $u = Get-Command python.exe -ErrorAction SilentlyContinue
    if ($u) { Step INFO 'Python on PATH is not usable' ("$($u.Source): only an all-users Python 3.12 works for a service; installing one") }
    $f = Get-Installer $script:PY_FILE $script:PY_URLS $script:PY_SHA 'Python Software Foundation'
    if (Change 'Install Python 3.12.10 for all users') {
        $r = Invoke-Proc $f @('/quiet', 'InstallAllUsers=1', 'PrependPath=1', 'Include_test=0', 'Shortcuts=0') -TimeoutSec 1200 -Dots
        if ($r.ExitCode -ne 0 -and $r.ExitCode -ne 3010) { Stop-Install 'Install Python' ("installer exit code $($r.ExitCode)") 'Run the installer in deploy\installers by hand to see its message; Windows 8.1 / 2012 R2 needs the latest Windows updates.' }
        Refresh-Path
        $py = Get-MachinePython
        if (-not $py) { Stop-Install 'Install Python' 'installed, but the all-users Python 3.12 was not found' 'Restart the PC and run the installer again.' }
        $script:BasePy = $py
        Step DONE 'Python 3.12 (all users)' $py
    }
}
function Test-Nssm {
    if (-not (Test-Path -LiteralPath $script:Nssm)) { return $false }
    $r = Invoke-Proc $script:Nssm @('version') -TimeoutSec 20
    # Older builds answer 'version' with their usage text and a non-zero exit code: any output that names NSSM proves it runs.
    return ($r.ExitCode -ne -1 -and -not $r.TimedOut -and ($r.Out + $r.Err) -match '(?i)nssm')
}
function Find-ExistingNssm {
    # A service already installed (for example by hand with C:\nssm\nssm.exe) keeps its own nssm.exe.
    $w = Get-CimInstance Win32_Service -Filter ("Name='" + $script:SvcName + "'") -ErrorAction SilentlyContinue
    if ($w -and $w.PathName) {
        $exe = ([string]$w.PathName).Trim().Trim('"')
        if ($exe -match '(?i)nssm\.exe$' -and (Test-Path -LiteralPath $exe)) { return $exe }
    }
    return $null
}
function Ensure-Nssm {
    $ex = Find-ExistingNssm
    if ($ex) { $ours = $script:Nssm; $script:Nssm = $ex; if (Test-Nssm) { Step OK 'NSSM service manager' ("existing $ex"); return }; $script:Nssm = $ours }
    if (Test-Nssm) { Step OK 'NSSM service manager' $script:Nssm; return }
    if ((Test-Path -LiteralPath $script:Nssm) -and (Get-Svc)) { Step WARN 'NSSM service manager' ("$($script:Nssm) did not answer the check but a service uses it; keeping it (not overwritten)"); return }
    $f = Get-Installer $script:NSSM_FILE $script:NSSM_URLS $script:NSSM_SHA ''
    if (Change 'Install NSSM 2.24-101') {
        $dir = Split-Path -Parent $script:Nssm
        if (-not (Test-Path -LiteralPath $dir)) { New-Item -ItemType Directory -Path $dir -Force | Out-Null }
        Add-Type -AssemblyName System.IO.Compression.FileSystem
        $z = [IO.Compression.ZipFile]::OpenRead($f)
        try {
            $e = $z.Entries | Where-Object { $_.FullName -like '*/win64/nssm.exe' } | Select-Object -First 1
            if (-not $e) { Stop-Install 'Install NSSM' 'win64\nssm.exe is not in the zip' }
            try { [IO.Compression.ZipFileExtensions]::ExtractToFile($e, $script:Nssm, $true) }
            catch [System.IO.IOException] {
                if (-not (Test-Path -LiteralPath $script:Nssm)) { throw }
                Step WARN 'NSSM service manager' ("$($script:Nssm) is in use by a running service; keeping the existing file")
                return
            }
        } finally { $z.Dispose() }
        if (-not (Test-Nssm)) { Stop-Install 'Install NSSM' 'nssm.exe does not run' 'Antivirus may have blocked it: allow C:\ProgramData\vLQMS\bin\nssm.exe.' }
        Step DONE 'NSSM service manager' $script:Nssm
    }
}
function Test-Odbc {
    foreach ($n in @('ODBC Driver 18 for SQL Server', 'ODBC Driver 17 for SQL Server')) {
        if (Test-Path -LiteralPath "HKLM:\SOFTWARE\ODBC\ODBCINST.INI\$n") { return $n }
    }
    return $null
}
function Ensure-Odbc {
    $d = Test-Odbc
    if ($d) { Step OK 'SQL Server ODBC driver' $d; return }
    try {
        $f = Get-Installer $script:ODBC_FILE $script:ODBC_URLS '' 'Microsoft Corporation'
        if (Change 'Install Microsoft ODBC Driver 18 for SQL Server') {
            $log = Join-Path $env:TEMP 'vlqms-odbc.log'
            $r = Invoke-Proc 'msiexec.exe' @('/i', $f, '/qn', '/norestart', 'IACCEPTMSODBCSQLLICENSETERMS=YES', '/l*v', $log) -TimeoutSec 900 -Dots
            if ($r.ExitCode -ne 0 -and $r.ExitCode -ne 3010) { throw "msiexec exit code $($r.ExitCode) (log: $log)" }
            Step DONE 'SQL Server ODBC driver' (Test-Odbc)
        }
    } catch {
        if ($_.Exception.Message -eq 'VLQMS_FATAL') { Step WARN 'SQL Server ODBC driver' 'not installed; the app falls back to pymssql (SQL logins work)' }
        else { Step WARN 'SQL Server ODBC driver' ('not installed (' + $_.Exception.Message + '); the app falls back to pymssql') }
        $script:Tally['FAIL'] = [Math]::Max(0, [int]$script:Tally['FAIL'] - 1)
    }
}
function Note-OptionalComponents {
    $rptExe = Join-Path $script:Backend 'tools\rptbridge\bin\RptBridge.exe'
    $rpts = @(Get-ChildItem -LiteralPath (Join-Path $script:Backend 'Rpts') -Filter *.rpt -ErrorAction SilentlyContinue).Count
    $crystal = (Test-Path 'HKLM:\SOFTWARE\WOW6432Node\SAP BusinessObjects') -or (Test-Path 'HKLM:\SOFTWARE\WOW6432Node\Business Objects')
    if ((Test-Path -LiteralPath $rptExe) -and $crystal) { Step OK 'Crystal Reports printing' ("bridge present, $rpts .rpt files") }
    else {
        Step INFO 'Crystal Reports printing (optional)' 'not complete; billing works, Crystal-format prints need it'
    }
}

# ===================================================================================================================
#  Service, processes, ports
# ===================================================================================================================
function Get-Svc { return (Get-Service -Name $script:SvcName -ErrorAction SilentlyContinue) }
function Get-SvcProcessId { $s = Get-CimInstance Win32_Service -Filter ("Name='" + $script:SvcName + "'") -ErrorAction SilentlyContinue; if ($s) { return [int]$s.ProcessId }; return 0 }
function Nssm-Run([string[]]$a) { return (Invoke-Proc $script:Nssm $a -TimeoutSec 60) }
function Nssm-Get([string[]]$names) {
    $r = Nssm-Run (@('get', $script:SvcName) + $names)
    if ($r.ExitCode -ne 0) { return $null }
    return (($r.Out -replace '[\r\n]+$', '').Trim())
}
function Stop-Tree([int]$procId) {
    $kids = @(Get-CimInstance Win32_Process -Filter "ParentProcessId=$procId" -ErrorAction SilentlyContinue)
    foreach ($k in $kids) { Stop-Tree ([int]$k.ProcessId) }
    Stop-Process -Id $procId -Force -ErrorAction SilentlyContinue
}
function Stop-AppService {
    $s = Get-Svc
    if (-not $s -or $s.Status -eq 'Stopped') { return }
    if (-not (Change "Stop service $($script:SvcName)")) { return }
    $procId = Get-SvcProcessId
    try { Stop-Service -Name $script:SvcName -Force -ErrorAction Stop; $s.WaitForStatus('Stopped', [TimeSpan]::FromSeconds(40)) }
    catch {
        Step WARN 'Stop service' ('did not stop cleanly (' + $_.Exception.Message + '); ending its processes')
        if ($procId -gt 0) { Stop-Tree $procId }
        Start-Sleep -Seconds 2
    }
    Step DONE 'Service stopped' $script:SvcName
}
function Get-SelfChain {
    $ids = @([int]$PID); $cur = [int]$PID
    for ($i = 0; $i -lt 8 -and $cur; $i++) {
        $p = Get-CimInstance Win32_Process -Filter "ProcessId=$cur" -ErrorAction SilentlyContinue
        if (-not $p) { break }
        $cur = [int]$p.ParentProcessId
        if ($cur) { $ids += $cur }
    }
    return $ids
}
function Find-AppProcesses {
    # python / bridge processes running from THIS app folder (e.g. a hand-started server that would lock files).
    $self = Get-SelfChain
    $res = @()
    foreach ($p in (Get-CimInstance Win32_Process -ErrorAction SilentlyContinue)) {
        if ($self -contains [int]$p.ProcessId) { continue }
        if ($p.Name -notmatch '^(python|pythonw|uvicorn|RptBridge)\.exe$') { continue }
        $exe = [string]$p.ExecutablePath; $cl = [string]$p.CommandLine
        if (($exe -and $exe.StartsWith($script:App, [StringComparison]::OrdinalIgnoreCase)) -or ($cl -and $cl.IndexOf($script:Backend, [StringComparison]::OrdinalIgnoreCase) -ge 0)) { $res += $p }
    }
    return $res
}
function Stop-AppProcesses {
    $found = @(Find-AppProcesses)
    if ($found.Count -eq 0) { return }
    if (-not (Change ("End $($found.Count) process(es) running from the app folder"))) { return }
    foreach ($p in $found) { Stop-Process -Id ([int]$p.ProcessId) -Force -ErrorAction SilentlyContinue }
    Start-Sleep -Seconds 1
    Step DONE 'Stopped leftover processes' (($found | ForEach-Object { "$($_.Name) $($_.ProcessId)" }) -join ', ')
}

function Parse-Netstat([string]$text, [int]$port) {
    foreach ($l in ($text -split "\r?\n")) {
        if ($l -match '^\s*TCP\s+\S+:(\d+)\s+\S+:0\s+\S+\s+(\d+)\s*$' -and [int]$Matches[1] -eq $port) { return [int]$Matches[2] }
    }
    return 0
}
function Parse-ExcludedRanges([string]$text) {
    $res = @()
    foreach ($l in ($text -split "\r?\n")) {
        if ($l -match '^\s*(\d+)\s+(\d+)\s*\*?\s*$') { $res += [pscustomobject]@{ Start = [int]$Matches[1]; End = [int]$Matches[2] } }
    }
    return $res
}
function Get-PortOwner([int]$port) {
    $ownerPid = 0
    try { $c = @(Get-NetTCPConnection -State Listen -LocalPort $port -ErrorAction Stop); if ($c.Count) { $ownerPid = [int]$c[0].OwningProcess } }
    catch { $ownerPid = Parse-Netstat ((Invoke-Proc 'netstat.exe' @('-ano', '-p', 'TCP')).Out) $port }
    if ($ownerPid -le 0) { return $null }
    $p = Get-CimInstance Win32_Process -Filter "ProcessId=$ownerPid" -ErrorAction SilentlyContinue
    $name = '(unknown)'; $cl = ''; $exe = ''
    if ($p) { $name = [string]$p.Name; $cl = [string]$p.CommandLine; $exe = [string]$p.ExecutablePath }
    return [pscustomobject]@{ Pid = $ownerPid; Name = $name; CommandLine = $cl; Exe = $exe }
}
function Get-ExcludedRange([int]$port) {
    $r = Invoke-Proc 'netsh.exe' @('interface', 'ipv4', 'show', 'excludedportrange', 'protocol=tcp') -TimeoutSec 30
    foreach ($x in (Parse-ExcludedRanges $r.Out)) { if ($port -ge $x.Start -and $port -le $x.End) { return $x } }
    return $null
}
function Test-PortBind([int]$port) {
    try { $l = New-Object System.Net.Sockets.TcpListener([System.Net.IPAddress]::Any, $port); $l.Start(); $l.Stop(); return '' }
    catch { return $_.Exception.Message }
}
function Get-PortProblem([int]$port) {
    $o = Get-PortOwner $port
    if ($o) {
        $mine = ($o.Name -match '^(python|pythonw|uvicorn)\.exe$') -and ($script:Backend) -and (($o.CommandLine -and $o.CommandLine.IndexOf($script:Backend, [StringComparison]::OrdinalIgnoreCase) -ge 0) -or ($o.Exe -and $o.Exe.StartsWith($script:App, [StringComparison]::OrdinalIgnoreCase)))
        if (-not $mine -and $o.Name -match '^(python|pythonw)\.exe$' -and $o.CommandLine -match 'app\.main:app') { $mine = $true }   # a server of this app started by hand from elsewhere
        return [pscustomobject]@{ Kind = $(if ($mine) { 'ownapp' } else { 'busy' }); Owner = $o; Text = ("port $port is in use by {0} (pid {1}) {2}" -f $o.Name, $o.Pid, $o.CommandLine) }
    }
    $x = Get-ExcludedRange $port
    if ($x) { return [pscustomobject]@{ Kind = 'reserved'; Owner = $null; Text = "port $port is inside a range Windows reserves ($($x.Start)-$($x.End), usually Hyper-V / WSL / Docker)" } }
    $b = Test-PortBind $port
    if ($b) { return [pscustomobject]@{ Kind = 'bind'; Owner = $null; Text = "port $port cannot be opened: $b" } }
    return $null
}
function Find-FreePort([int]$start = 8001) {
    for ($p = $start; $p -lt $start + 200; $p++) { if (-not (Get-PortProblem $p)) { return $p } }
    return 0
}
function Resolve-Port {
    $p = $Port
    if ($p -le 0) { $p = [int]$script:Settings['Port'] }
    if ($p -le 0) { $p = 8001 }
    for ($i = 0; $i -lt 10; $i++) {
        $prob = Get-PortProblem $p
        if (-not $prob) { break }
        Step WARN 'Port' $prob.Text
        if ($prob.Kind -eq 'ownapp') {
            if ($script:Unattended -or (Ask-YesNo 'This looks like a hand-started copy of this app. Stop it?' $true)) {
                if (Change "Stop the hand-started server (pid $($prob.Owner.Pid))") { Stop-Tree $prob.Owner.Pid; Start-Sleep -Seconds 2; continue }
                break
            }
        }
        if ($script:Unattended) { Stop-Install 'Port' $prob.Text 'Free the port or pass another -Port.' }
        if ($script:DryRun) { break }
        $sug = Find-FreePort ($p + 1)
        $v = Ask 'Use which port instead' ([string]$sug)
        if ($v -notmatch '^\d{4,5}$' -or [int]$v -gt 65535) { Say '       enter a number between 1024 and 65535' 'Yellow'; continue }
        $p = [int]$v
    }
    if (Get-PortProblem $p) { if (-not $script:DryRun) { Stop-Install 'Port' "no usable port found (last tried $p)" 'Pass a free -Port.' } }
    else { Step OK 'Port' "$p is free" }
    $script:PortNo = $p
    $script:Settings['Port'] = $p
}

function Ensure-Firewall {
    $name = "$($script:SvcName) $($script:PortNo)"
    $old = [string]$script:Settings['FirewallRule']
    if ($old -and $old -ne $name -and (Get-NetFirewallRule -DisplayName $old -ErrorAction SilentlyContinue)) {
        if (Change "Remove old firewall rule '$old'") { Remove-NetFirewallRule -DisplayName $old; Step DONE 'Old firewall rule removed' $old }
    }
    $r = Get-NetFirewallRule -DisplayName $name -ErrorAction SilentlyContinue
    if ($r) {
        $pf = $r | Get-NetFirewallPortFilter
        if ("$($r.Enabled)" -eq 'True' -and "$($r.Action)" -eq 'Allow' -and "$($r.Direction)" -eq 'Inbound' -and "$($pf.LocalPort)" -eq "$($script:PortNo)" -and "$($pf.Protocol)" -eq 'TCP') { Step OK 'Firewall rule' $name; $script:Settings['FirewallRule'] = $name; return }
        if (Change "Repair firewall rule '$name'") { Remove-NetFirewallRule -DisplayName $name }
    }
    if (Change "Create inbound firewall rule '$name'") {
        New-NetFirewallRule -DisplayName $name -Direction Inbound -Action Allow -Protocol TCP -LocalPort $script:PortNo -Profile Any | Out-Null
        Step DONE 'Firewall rule' $name
    }
    $script:Settings['FirewallRule'] = $name
}

# ===================================================================================================================
#  Virtual environment and packages
# ===================================================================================================================
function Test-Venv([string]$venv, [string]$basePy) {
    $vp = Join-Path $venv 'Scripts\python.exe'
    if (-not (Test-Path -LiteralPath $vp)) { return 'missing' }
    $r = Invoke-Proc $vp @('-I', '-c', 'import sys;print(sys.base_prefix)') -TimeoutSec 60
    if ($r.ExitCode -ne 0) { return 'broken' }
    $base = $r.Out.Trim().TrimEnd('\')
    if ($basePy) {
        $want = (Split-Path -Parent $basePy).TrimEnd('\')
        if ($base -ne $want -and -not $base.Equals($want, [StringComparison]::OrdinalIgnoreCase)) { return "built-for-another-python ($base)" }
    }
    return 'ok'
}
function Remove-Folder([string]$p) {
    for ($i = 0; $i -lt 3; $i++) {
        try { Remove-Item -LiteralPath $p -Recurse -Force -ErrorAction Stop; return } catch { Start-Sleep -Seconds 2 }
    }
    Invoke-Proc 'cmd.exe' @('/c', 'rmdir', '/s', '/q', $p) | Out-Null
    if (Test-Path -LiteralPath $p) { Stop-Install 'Remove old virtual environment' "$p is locked" 'Stop anything using it (service, console, antivirus) and run again.' }
}
function Ensure-Venv {
    $venv = Join-Path $script:Backend '.venv'
    $state = Test-Venv $venv $script:BasePy
    if ($state -eq 'ok') { Step OK 'Virtual environment' $venv; return }
    if ($state -ne 'missing') { Step WARN 'Virtual environment unusable' "$state; rebuilding it (a venv copied from another PC does not work)" }
    if (Change 'Create the virtual environment') {
        if (Test-Path -LiteralPath $venv) { Remove-Folder $venv }
        $r = Invoke-Proc $script:BasePy @('-m', 'venv', $venv) -TimeoutSec 600 -Dots
        if ($r.ExitCode -ne 0 -or -not (Test-Path -LiteralPath $script:VenvPy)) { Stop-Install 'Create virtual environment' (Last-Lines ($r.Out + $r.Err) 5) 'Check disk space and antivirus.' }
        Step DONE 'Virtual environment' $venv
    }
}
function Get-FilteredRequirements {
    $req = Join-Path $script:Backend 'requirements.txt'
    $lines = @(Get-Content -LiteralPath $req)
    if (-not $script:WithAi) { $lines = @($lines | Where-Object { $_ -notmatch '^\s*(torch|chronos-forecasting)\b' }) }
    return (($lines -join "`r`n") + "`r`n")
}
function Test-Imports {
    $mods = 'fastapi,uvicorn,jwt,cryptography,pymssql,httpx,jinja2,fitz,certifi'
    $r = Invoke-Proc $script:VenvPy @('-I', '-c', "import $mods") -TimeoutSec 120
    return $r
}
function Ensure-Packages {
    $text = Get-FilteredRequirements
    $stampFile = Join-Path $script:Backend '.venv\.req-sha256'
    $sha = [BitConverter]::ToString([Security.Cryptography.SHA256]::Create().ComputeHash([Text.Encoding]::UTF8.GetBytes($text + "|ai=$($script:WithAi)"))).Replace('-', '')
    $have = ''
    if (Test-Path -LiteralPath $stampFile) { $have = (Get-Content -LiteralPath $stampFile -Raw).Trim() }
    if ($have -eq $sha -and (Test-Path -LiteralPath $script:VenvPy) -and (Test-Imports).ExitCode -eq 0) { Step OK 'Python packages' 'already up to date'; return }
    if (-not (Change 'Install / refresh the Python packages (pip)')) { return }
    $tmp = Join-Path $env:TEMP ("vlqms-req-$PID.txt")
    Write-Utf8 $tmp $text
    $wheels = Join-Path $script:DeployDir 'wheels'
    $a = @('-m', 'pip', 'install', '--prefer-binary', '--retries', '5', '--default-timeout', '60', '--no-input', '--disable-pip-version-check', '-r', $tmp)
    if ($script:Offline) { $a += @('--no-index', '--find-links', $wheels) }
    elseif (Test-Path -LiteralPath $wheels) { $a += @('--find-links', $wheels) }
    if (-not $script:Offline) {
        $up = Invoke-Proc $script:VenvPy @('-m', 'pip', 'install', '--upgrade', 'pip', '--retries', '5', '--default-timeout', '60', '--no-input', '--disable-pip-version-check') -WorkDir $script:Backend -TimeoutSec 600 -Dots
        if ($up.ExitCode -ne 0) { Step WARN 'pip upgrade' 'could not upgrade pip; continuing with the bundled one' }
    }
    Say '       pip is installing (a few minutes on a first install) ' 'DarkGray'
    $r = Invoke-Proc $script:VenvPy $a -WorkDir $script:Backend -TimeoutSec 3600 -Dots
    Remove-Item -LiteralPath $tmp -Force -ErrorAction SilentlyContinue
    if ($r.ExitCode -ne 0) {
        Say (Last-Lines ($r.Out + "`r`n" + $r.Err) 15) 'Yellow'
        Stop-Install 'pip install' "exit code $($r.ExitCode)" 'No internet or proxy block? Use -Offline with wheels in deploy\wheels. A package without a Windows wheel needs the same Python version as the wheels.'
    }
    $c = Test-Imports
    if ($c.ExitCode -ne 0) { Stop-Install 'Package import check' (Last-Lines ($c.Out + $c.Err) 5) 'Delete backend\.venv and run Install again.' }
    $po = Invoke-Proc $script:VenvPy @('-I', '-c', 'import pyodbc') -TimeoutSec 60
    if ($po.ExitCode -ne 0) { Step WARN 'pyodbc' 'not importable; pymssql will be used' }
    Write-Utf8 $stampFile $sha
    Step DONE 'Python packages' 'installed and import-checked'
}
function Test-AppImport {
    if (-not (Change 'Import-check the application')) { return }
    $r = Invoke-Proc $script:VenvPy @('-c', "import sys;sys.path.insert(0,'.');import app.main") -WorkDir $script:Backend -TimeoutSec 180 -EnvVars @{ VLQMS_KEYS_IN_MEMORY = '1'; PYTHONUTF8 = '1' }
    if ($r.ExitCode -ne 0) {
        Say (Last-Lines ($r.Out + "`r`n" + $r.Err) 15) 'Yellow'
        Stop-Install 'Application import check' "app.main did not import (exit $($r.ExitCode))" 'See the lines above: a missing package or a damaged copy of backend\app.'
    }
    Step OK 'Application import check' 'app.main loads'
}
function Test-Dist {
    $dist = Join-Path $script:App 'frontend\dist'
    if (-not (Test-Path -LiteralPath (Join-Path $dist 'index.html'))) { Stop-Install 'Frontend build' "$dist\index.html is missing" 'Run Make-Package on the development PC and use its zip.' }
    $bad = @()
    foreach ($f in @(Get-ChildItem -LiteralPath $dist -Recurse -File -Include *.js, *.html -ErrorAction SilentlyContinue)) {
        $t = [IO.File]::ReadAllText($f.FullName)
        if ($t.Contains('localhost:8001') -or $t.Contains('127.0.0.1:8001')) { $bad += $f.Name }
    }
    if ($bad.Count) { Stop-Install 'Frontend build' ("a development API address is baked into: " + ($bad -join ', ')) 'Every other PC would call itself. Rebuild with Make-Package (it builds with no API address).' }
    Step OK 'Frontend build' 'present, no development URLs'
}

# ===================================================================================================================
#  Settings file (.env)
# ===================================================================================================================
function Get-EnvValue([string]$text, [string]$key) {
    $m = [regex]::Match($text, '(?m)^[ \t]*' + [regex]::Escape($key) + '[ \t]*=([^\r\n]*)')
    if (-not $m.Success) { return $null }
    return (($m.Groups[1].Value -replace '\s+#.*$', '').Trim())
}
function Set-EnvValue([string]$text, [string]$key, [string]$value) {
    $nl = "`r`n"; if ($text -and $text.Contains("`n") -and -not $text.Contains("`r`n")) { $nl = "`n" }
    $re = New-Object System.Text.RegularExpressions.Regex('(?m)^[ \t]*' + [regex]::Escape($key) + '[ \t]*=[^\r\n]*')
    $line = "$key=$value"
    if ($re.IsMatch($text)) { return $re.Replace($text, [System.Text.RegularExpressions.MatchEvaluator] { param($m) $line }, 1) }
    if ($text -and -not $text.EndsWith("`n")) { $text += $nl }
    return $text + $line + $nl
}
function Get-SidOf([string]$user) {
    $u = $user
    if ($u.StartsWith('.\')) { $u = $env:COMPUTERNAME + '\' + $u.Substring(2) }
    return (New-Object Security.Principal.NTAccount($u)).Translate([Security.Principal.SecurityIdentifier]).Value
}
function Lock-Path([string]$path, [bool]$isDir, [string]$extraSid) {
    # Owner-only style ACL by SID (locale-proof): SYSTEM + Administrators (+ the service account).
    $perm = '(F)'; if ($isDir) { $perm = '(OI)(CI)(F)' }
    $a = @($path, '/grant:r', ('*S-1-5-18:' + $perm), ('*S-1-5-32-544:' + $perm))
    if ($extraSid) { $rp = '(R)'; if ($isDir) { $rp = '(OI)(CI)(M)' }; $a += @('/grant:r', ('*' + $extraSid + ':' + $rp)) }
    $a += @('/inheritance:r')
    $r = Invoke-Proc 'icacls.exe' $a -TimeoutSec 60
    if ($r.ExitCode -ne 0) { Step WARN 'Permissions' ("icacls failed on $path : " + (Last-Lines $r.Out 2)) }
}
function Ensure-EnvFile {
    $envFile = Join-Path $script:Backend '.env'
    $example = Join-Path $script:Backend '.env.example'
    $fresh = -not (Test-Path -LiteralPath $envFile)
    if ($fresh -and -not (Change 'Create backend\.env from the template')) { return }
    $text = ''
    if (-not $fresh) { $text = [IO.File]::ReadAllText($envFile) }
    elseif (Test-Path -LiteralPath $example) { $text = [IO.File]::ReadAllText($example) }
    $orig = $text
    $notes = @()
    $jwt = Get-EnvValue $text 'JWT_SECRET'; $sk = Get-EnvValue $text 'SECRET_KEY'
    $weak = '^(|change-this.*|changeme.*|your_.*)$'
    if ($jwt -match $weak -or $sk -match $weak) {
        $sec = New-Secret
        if ($jwt -match $weak) { $text = Set-EnvValue $text 'JWT_SECRET' $sec }
        if ($sk -match $weak) { $text = Set-EnvValue $text 'SECRET_KEY' $sec }
        $notes += 'random JWT secret generated'
    }
    if ($fresh) {
        $text = Set-EnvValue $text 'APP_ENV' 'production'
        if ((Get-EnvValue $text 'SMTP_USER') -eq 'your_smtp_user') {
            foreach ($k in @('SMTP_HOST', 'SMTP_USER', 'SMTP_PASSWORD', 'SMTP_FROM')) { $text = Set-EnvValue $text $k '' }
            $notes += 'placeholder SMTP settings blanked (e-mail off)'
        }
        if ((Get-EnvValue $text 'DEFAULT_ADMIN_PASSWORD') -eq 'admin') { $text = Set-EnvValue $text 'DEFAULT_ADMIN_PASSWORD' ''; $notes += 'default admin password blanked' }
    }
    $text = Set-EnvValue $text 'API_PORT' ([string]$script:PortNo)
    if ($fresh -or $text -ne $orig) {
        if (Change 'Write backend\.env') {
            Write-Utf8 $envFile $text
            Step DONE 'Settings file (.env)' $(if ($fresh) { 'created' } else { 'updated' })
            foreach ($n in $notes) { Say "       $n" 'DarkGray' }
        }
    } else { Step OK 'Settings file (.env)' 'present' }
    if (-not $script:DryRun) {
        $sid = ''; if ($ServiceUser -and $ServiceUser -ne 'LocalSystem') { try { $sid = Get-SidOf $ServiceUser } catch { $null = $_ } }
        elseif ($script:Settings['ServiceUser']) { try { $sid = Get-SidOf ([string]$script:Settings['ServiceUser']) } catch { $null = $_ } }
        # The app's Database setup replaces .env through a temporary file, which resets this ACL: it is re-applied on every Install / Update.
        if (Test-Path -LiteralPath $envFile) { Lock-Path $envFile $false $sid }
        $data = Join-Path $script:Backend 'data'
        if (-not (Test-Path -LiteralPath $data)) { New-Item -ItemType Directory -Path $data -Force | Out-Null }
        Lock-Path $data $true $sid
        $who = 'SYSTEM, Administrators'; if ($sid) { $who += ' and the service account' }
        Step OK 'Permissions' (".env and data\ limited to " + $who)
    }
}

# ===================================================================================================================
#  Database check (the app's own Database setup does the real binding)
# ===================================================================================================================
function Check-Database {
    $server = $DbServer; $dbn = $DbName; $user = $DbUser; $dport = $DbPort
    $pw = $DbPassword; if (-not $pw) { $pw = $env:VLQMS_DB_PASSWORD }
    if ($pw) { [void]$script:Secrets.Add($pw) }
    if (-not $server) {
        if ($script:Unattended) { return }
        if (-not (Ask-YesNo 'Check the SQL Server connection details now? (optional; the site asks again in its Database setup)' $false)) { return }
    }
    $py = @'
import os, sys
host, port, db, user, pw = (os.environ.get(k, "") for k in ("CHK_HOST", "CHK_PORT", "CHK_DB", "CHK_USER", "CHK_PW"))
err = ""
try:
    import pymssql
    srv = host if not port else host + ":" + port
    c = pymssql.connect(server=srv, user=user, password=pw, database=db or "master", login_timeout=10)
    c.close()
    print("OK pymssql")
    sys.exit(0)
except Exception as e:
    err = str(e)
try:
    import pyodbc
    for drv in ("ODBC Driver 18 for SQL Server", "ODBC Driver 17 for SQL Server"):
        try:
            s = "DRIVER={%s};SERVER=%s;DATABASE=%s;UID=%s;PWD=%s;TrustServerCertificate=yes;Encrypt=no" % (drv, host + ("," + port if port else ""), db or "master", user, pw)
            pyodbc.connect(s, timeout=10).close()
            print("OK " + drv)
            sys.exit(0)
        except Exception as e2:
            err = str(e2)
except ImportError:
    pass
print(err[:300])
sys.exit(2)
'@
    $tmpPy = Join-Path $env:TEMP 'vlqms-dbcheck.py'
    for ($try = 0; $try -lt 5; $try++) {
        if (-not $server) { $server = Ask 'SQL Server (e.g. localhost\SQLEXPRESS or 192.168.1.5)' ''; if (-not $server) { return } }
        if (-not $dbn) { $dbn = Ask 'Database name' '' }
        if (-not $user) { $user = Ask 'SQL login' '' }
        if (-not $pw) { $pw = Ask-Secret 'Password (hidden)' }
        if (-not $dport -and $server -notmatch '\\') { $dport = Ask 'Port (blank = default 1433)' '' }
        if (-not (Change 'Check the SQL Server login')) { return }
        Write-Utf8 $tmpPy $py
        $r = Invoke-Proc $script:VenvPy @('-I', $tmpPy) -TimeoutSec 60 -EnvVars @{ CHK_HOST = $server; CHK_PORT = $dport; CHK_DB = $dbn; CHK_USER = $user; CHK_PW = $pw }
        Remove-Item -LiteralPath $tmpPy -Force -ErrorAction SilentlyContinue
        if ($r.ExitCode -eq 0) {
            Step OK 'SQL Server login' ("works ($($r.Out.Trim())); finish it in the site's Database setup")
            $script:Settings['DbServer'] = $server; $script:Settings['DbName'] = $dbn; $script:Settings['DbUser'] = $user
            return
        }
        $msg = Mask ($r.Out + ' ' + $r.Err).Trim()
        Step WARN 'SQL Server login' ($msg)
        if ($script:Unattended) { Stop-Install 'SQL Server login' 'the details given do not work' 'Check -DbServer / -DbPort / -DbName / -DbUser / password and that SQL Server accepts TCP/IP and SQL logins.' }
        if (-not (Ask-YesNo 'Re-enter the details?' $true)) { return }
        $server = Ask 'SQL Server' $server; $dbn = Ask 'Database name' $dbn; $user = Ask 'SQL login' $user; $dport = Ask 'Port' $dport; $pw = Ask-Secret 'Password (hidden)'
    }
}

# ===================================================================================================================
#  Windows service
# ===================================================================================================================
function Get-ServiceSpec {
    $logDir = Join-Path $script:Backend 'logs'
    $log = Join-Path $logDir 'service.log'
    return @(
        @{ N = 'Application'; V = @($script:VenvPy); Cmp = $true }
        @{ N = 'AppParameters'; V = @("-m uvicorn app.main:app --host 0.0.0.0 --port $($script:PortNo)"); Cmp = $true }
        @{ N = 'AppDirectory'; V = @($script:Backend); Cmp = $true }
        @{ N = 'DisplayName'; V = @('e-vLQMS ERP'); Cmp = $false }
        @{ N = 'Description'; V = @('e-vLQMS ERP web backend (FastAPI + frontend)'); Cmp = $false }
        @{ N = 'Start'; V = @('SERVICE_DELAYED_AUTO_START'); Cmp = $true }
        @{ N = 'AppStdout'; V = @($log); Cmp = $false }
        @{ N = 'AppStderr'; V = @(Join-Path $logDir 'service-error.log'); Cmp = $false }
        @{ N = 'AppRotateFiles'; V = @('1'); Cmp = $false }
        @{ N = 'AppRotateOnline'; V = @('1'); Cmp = $false }
        @{ N = 'AppRotateBytes'; V = @('5242880'); Cmp = $false }
        @{ N = 'AppRestartDelay'; V = @('5000'); Cmp = $false }
        @{ N = 'AppThrottle'; V = @('5000'); Cmp = $false }
        @{ N = 'AppStopMethodConsole'; V = @('15000'); Cmp = $false }
        @{ N = 'AppEnvironmentExtra'; V = @('PYTHONUTF8=1', 'PYTHONIOENCODING=utf-8'); Cmp = $false }
    )
}
function Sync-Service {
    $logDir = Join-Path $script:Backend 'logs'
    if (-not (Test-Path -LiteralPath $logDir)) { New-Item -ItemType Directory -Path $logDir -Force | Out-Null }
    $exists = [bool](Get-Svc)
    if (-not (Test-Path -LiteralPath $script:VenvPy) -and -not $script:DryRun) { Stop-Install 'Service' "$($script:VenvPy) does not exist" 'The virtual environment step failed.' }
    if (-not $exists) {
        if (Change "Install service $($script:SvcName)") {
            $r = Nssm-Run @('install', $script:SvcName, $script:VenvPy)
            if ($r.ExitCode -ne 0) { Stop-Install 'Install service' (Last-Lines ($r.Out + $r.Err) 5) 'Run as Administrator; check the name is not used by another service.' }
            Step DONE 'Service created' $script:SvcName
        } else { return }
    } else {
        # A service created by another nssm.exe (or one that was moved) is pointed back at ours.
        $w = Get-CimInstance Win32_Service -Filter ("Name='" + $script:SvcName + "'")
        $img = ([string]$w.PathName).Trim('"')
        if (-not $img.StartsWith($script:Nssm, [StringComparison]::OrdinalIgnoreCase)) {
            if (Change "Point the service at $($script:Nssm)") {
                Invoke-Proc 'sc.exe' @('config', $script:SvcName, 'binPath=', $script:Nssm) | Out-Null
                Step DONE 'Service executable repaired' "was $img"
            }
        }
    }
    if ($script:DryRun) { Step DRY 'Apply NSSM settings' 'would be (re)applied'; return }
    foreach ($s in (Get-ServiceSpec)) {
        if ($s.Cmp -and $exists) {
            $cur = Nssm-Get @($s.N)
            $want = $s.V -join ' '
            if ($null -ne $cur -and $cur -ne $want) { Step DONE ("Service setting " + $s.N + " repaired") ("was '" + $cur + "'") }
        }
        $r = Nssm-Run (@('set', $script:SvcName, $s.N) + $s.V)
        if ($r.ExitCode -ne 0) { Stop-Install ("nssm set " + $s.N) (Last-Lines ($r.Out + $r.Err) 3) }
    }
    $r = Nssm-Run @('set', $script:SvcName, 'AppExit', 'Default', 'Restart')
    if ($r.ExitCode -ne 0) { Step WARN 'nssm AppExit' (Last-Lines ($r.Out + $r.Err) 2) }
    # Run as: only when asked (-ServiceUser), otherwise whatever the service already uses (LocalSystem for a new one).
    if ($ServiceUser) {
        if ($ServiceUser -eq 'LocalSystem') { Nssm-Run @('set', $script:SvcName, 'ObjectName', 'LocalSystem') | Out-Null; Step DONE 'Service account' 'LocalSystem'; $script:Settings['ServiceUser'] = '' }
        else {
            $pw = $ServicePassword; if (-not $pw) { $pw = $env:VLQMS_SERVICE_PASSWORD }
            if (-not $pw) { $pw = Ask-Secret "Windows password of $ServiceUser (hidden)" }
            if (-not $pw) { Stop-Install 'Service account' 'no password for the service user' 'Pass -ServicePassword or set VLQMS_SERVICE_PASSWORD.' }
            [void]$script:Secrets.Add($pw)
            $sid = $null
            try { $sid = Get-SidOf $ServiceUser } catch { Stop-Install 'Service account' "unknown Windows user $ServiceUser" }
            Invoke-Proc 'icacls.exe' @($script:App, '/grant', ('*' + $sid + ':(OI)(CI)(M)')) -TimeoutSec 300 | Out-Null
            $r = Nssm-Run @('set', $script:SvcName, 'ObjectName', $ServiceUser, $pw)
            if ($r.ExitCode -ne 0) { Stop-Install 'Service account' (Mask (Last-Lines ($r.Out + $r.Err) 3)) }
            $script:Settings['ServiceUser'] = $ServiceUser
            Step DONE 'Service account' "$ServiceUser (read on .env, modify on the app folder)"
        }
    }
    Step OK 'Service settings' 'Application / AppParameters / AppDirectory / log rotation / restart-on-exit / delayed auto-start applied'
}

# ===================================================================================================================
#  Start and verify
# ===================================================================================================================
function Http-Get([string]$url, [string]$accept = '*/*', [int]$timeout = 8) {
    $req = [Net.HttpWebRequest][Net.WebRequest]::Create($url)
    $req.Proxy = $null; $req.Timeout = $timeout * 1000; $req.ReadWriteTimeout = $timeout * 1000; $req.Accept = $accept; $req.UserAgent = 'vLQMS-installer'
    try { $resp = $req.GetResponse() }
    catch [Net.WebException] {
        $resp = $_.Exception.Response
        if (-not $resp) { return [pscustomobject]@{ Status = 0; Body = ''; Error = $_.Exception.Message } }
    }
    try {
        $sr = New-Object IO.StreamReader($resp.GetResponseStream())
        $body = $sr.ReadToEnd(); $sr.Dispose()
        return [pscustomobject]@{ Status = [int]$resp.StatusCode; Body = $body; Error = '' }
    } finally { $resp.Close() }
}
function Read-OneLog([string]$f, [int]$n) {
    if (-not (Test-Path -LiteralPath $f)) { return '' }
    try {
        $fs = [IO.File]::Open($f, 'Open', 'Read', 'ReadWrite'); $sr = New-Object IO.StreamReader($fs)
        $t = $sr.ReadToEnd(); $sr.Dispose(); $fs.Dispose()
        return (Last-Lines (Strip-Nul $t) $n)
    } catch { return "(cannot read ${f}: $($_.Exception.Message))" }
}
function Read-LogTail([int]$n = 40) {
    # uvicorn logs to stderr, so the error log matters most; both are shown.
    $dir = Join-Path $script:Backend 'logs'
    $e = Read-OneLog (Join-Path $dir 'service-error.log') $n
    $o = Read-OneLog (Join-Path $dir 'service.log') $n
    $parts = @()
    if ($e) { $parts += "--- service-error.log ---`r`n$e" }
    if ($o) { $parts += "--- service.log ---`r`n$o" }
    if (-not $parts.Count) { return '(no service logs yet)' }
    return ($parts -join "`r`n")
}function Check-Health([int]$port) {
    $res = [pscustomobject]@{ Api = $false; Root = $false; Db = $false; Reason = '' }
    $a = Http-Get "http://127.0.0.1:$port/api/" 'application/json'
    if ($a.Status -eq 200) {
        try { $j = $a.Body | ConvertFrom-Json; if ($j.status -eq 'ok') { $res.Api = $true; $res.Db = [bool]$j.db.connected } else { $res.Reason = 'API answered but not "ok"' } }
        catch { $res.Reason = 'API answered with something that is not JSON' }
    } else { $res.Reason = if ($a.Status) { "API returned HTTP $($a.Status)" } else { "no answer on port $port ($($a.Error))" } }
    $r = Http-Get "http://127.0.0.1:$port/" 'text/html'
    if ($r.Status -eq 200 -and $r.Body -match '(?i)<html' -and $r.Body -match '(?i)<script') { $res.Root = $true }
    elseif (-not $res.Reason) { $res.Reason = 'the site root did not return the app page (frontend\dist missing?)' }
    return $res
}
function Wait-Healthy([int]$port, [int]$timeoutSec = 180) {
    $sw = [Diagnostics.Stopwatch]::StartNew(); $pausedSince = $null; $repaired = $false; $h = $null
    while ($sw.Elapsed.TotalSeconds -lt $timeoutSec) {
        $s = Get-Svc
        if (-not $s) { return [pscustomobject]@{ Ok = $false; Health = $null; Reason = 'the service does not exist' } }
        if ($s.Status -eq 'Paused') {
            if (-not $pausedSince) { $pausedSince = Get-Date }
            if (((Get-Date) - $pausedSince).TotalSeconds -gt 15 -and -not $repaired) {
                $repaired = $true
                Step WARN 'Service is Paused' 're-applying its settings and restarting (NSSM pauses when the program cannot start)'
                Stop-AppService; Sync-Service
                try { Start-Service -Name $script:SvcName } catch { $null = $_ }
                $pausedSince = $null
            }
        } else { $pausedSince = $null }
        if ($s.Status -eq 'Running') {
            $h = Check-Health $port
            if ($h.Api -and $h.Root) { return [pscustomobject]@{ Ok = $true; Health = $h; Reason = '' } }
        } elseif ($s.Status -eq 'Stopped' -and $sw.Elapsed.TotalSeconds -gt 8) {
            try { Start-Service -Name $script:SvcName } catch { $null = $_ }
        }
        Write-Host -NoNewline '.'
        Start-Sleep -Seconds 3
    }
    Write-Host ''
    $reason = 'timed out waiting for the site to answer'; if ($h -and $h.Reason) { $reason = $h.Reason }
    $s = Get-Svc; if ($s) { $reason += " (service state: $($s.Status))" }
    return [pscustomobject]@{ Ok = $false; Health = $h; Reason = $reason }
}
function Show-Diagnostics([int]$port) {
    Say '------ diagnostics ------' 'Yellow'
    $tail = Read-LogTail 40
    Say 'Last 40 lines of backend\logs\service-error.log / service.log:' 'Yellow'; Say $tail 'DarkGray'
    Say 'Stored service settings:' 'Yellow'
    foreach ($n in @('Application', 'AppParameters', 'AppDirectory', 'Start', 'ObjectName')) { Say ("  {0} = {1}" -f $n, (Nssm-Get @($n))) 'DarkGray' }
    $o = Get-PortOwner $port
    if ($o) { Say ("Port {0} owner: {1} (pid {2})" -f $port, $o.Name, $o.Pid) 'DarkGray' } else { Say "Port ${port}: nobody is listening" 'DarkGray' }
    $hint = 'Read the log lines above.'
    if ($tail -match 'No module named|ModuleNotFoundError') { $hint = 'A package is missing: run Install / Repair again (it rebuilds the virtual environment and reinstalls packages).' }
    elseif ($tail -match '10048|Address already in use|only one usage of each socket') { $hint = 'The port is taken by another program: run Install again and pick another port.' }
    elseif ($tail -match '10013|forbidden by its access permissions') { $hint = 'The port is reserved by Windows (Hyper-V / WSL / Docker): choose a different port.' }
    elseif ($tail -match "can't open file|No such file|cannot find the (file|path)") { $hint = 'The service points at a path that no longer exists: run Install / Repair to re-apply the service settings.' }
    elseif ($tail -match 'Access is denied|PermissionError') { $hint = 'The service account cannot read the app folder or .env: re-run Install (it fixes permissions).' }
    elseif (-not (Get-Svc)) { $hint = 'The service is not installed: run Install / Repair.' }
    Say ('Hint: ' + $hint) 'Yellow'
}
function Verify-Running {
    $port = $script:PortNo
    if (-not (Change "Start service and check http://127.0.0.1:$port/")) { return $true }
    Stop-AppService
    Say '       starting the service (the first start loads the database, please wait)' 'DarkGray'
    try { Start-Service -Name $script:SvcName -ErrorAction Stop } catch { Step WARN 'Start service' $_.Exception.Message }
    $w = Wait-Healthy $port
    if (-not $w.Ok) {
        Show-Diagnostics $port
        Stop-Install 'Health check' $w.Reason 'See the diagnostics above.'
    }
    $h = $w.Health
    Step OK 'Service' "$($script:SvcName) is Running"
    Step OK 'API' "http://127.0.0.1:$port/api/ answers"
    Step OK 'Site root' 'the app page loads'
    if ($h.Db) { Step OK 'Database' 'connected' }
    else { Step WARN 'Database' 'not connected yet - open the site and finish "Database setup" (back the shop database up first)'; Add-Manual 'Finish "Database setup" in the browser.' }
    $tail = Read-LogTail 300
    if ($tail -match 'Traceback') { Step WARN 'Service log' 'a Python traceback was logged; read backend\logs\service-error.log' }
    if ($tail -match '(?i)(bridge|crystal)[^\r\n]*(not found|unavailable|cannot|failed)') { Step WARN 'Crystal bridge' 'not available (optional)' }
    $o = Get-PortOwner $port
    if ($o) { Step OK "Port $port" ("listening: {0} (pid {1})" -f $o.Name, $o.Pid) } else { Step WARN "Port $port" 'nobody listening?' }
    return $true
}
function Show-Summary {
    Say '' ; Say '==================== Summary ====================' 'Cyan'
    Say ("Open the site at:") 'White'
    $ips = @()
    foreach ($n in @(Get-CimInstance Win32_NetworkAdapterConfiguration -Filter 'IPEnabled=True' -ErrorAction SilentlyContinue)) {
        foreach ($ip in @($n.IPAddress)) { if ($ip -match '^\d+\.\d+\.\d+\.\d+$' -and $ip -notmatch '^(127\.|169\.254\.)') { $ips += $ip } }
    }
    $p = $script:PortNo
    Say "   http://localhost:$p/" 'Green'
    Say "   http://$($env:COMPUTERNAME):$p/" 'Green'
    foreach ($ip in ($ips | Select-Object -Unique)) { Say "   http://${ip}:$p/" 'Green' }
    Say ("Logs:    " + (Join-Path $script:Backend 'logs')) 'Gray'
    Say ("Install log: " + $script:LogFile) 'Gray'
    if ($script:Manual.Count) { Say 'Still to do by hand:' 'Yellow'; foreach ($m in $script:Manual) { Say "   - $m" 'Yellow' } }
    $t = $script:Tally
    Say ("Result: OK {0}  DONE {1}  WARN {2}  FAIL {3}" -f [int]$t['OK'], [int]$t['DONE'], [int]$t['WARN'], [int]$t['FAIL']) 'Cyan'
}

# ===================================================================================================================
#  Modes
# ===================================================================================================================
function Get-SvcParam([string]$name) {
    # The service's own settings, straight from the registry: no nssm.exe needed, so it works whatever NSSM build installed it.
    $k = 'HKLM:\SYSTEM\CurrentControlSet\Services\' + $script:SvcName + '\Parameters'
    try { $v = (Get-ItemProperty -LiteralPath $k -ErrorAction Stop).$name; if ($null -ne $v) { return ([string]$v).Trim() } } catch { $null = $_ }
    return $null
}
function Get-InstalledPort {
    # The port the installed service really runs on (its AppParameters), or 0.
    if (-not (Get-Svc)) { return 0 }
    $par = Get-SvcParam 'AppParameters'
    if (-not $par -and (Test-Path -LiteralPath $script:Nssm)) { $par = Nssm-Get @('AppParameters') }
    if ($par -match '--port[ =]+(\d+)') { return [int]$Matches[1] }
    return 0
}
function Resolve-Installed {
    # Update / Status / Uninstall: find where it is installed and which port it uses from the service itself, never from a default.
    $ex = Find-ExistingNssm; if ($ex) { $script:Nssm = $ex }
    if (-not (Get-Svc)) { return }
    $dir = Get-SvcParam 'AppDirectory'
    if (-not $dir -and (Test-Path -LiteralPath $script:Nssm)) { $dir = Nssm-Get @('AppDirectory') }
    if ($dir -and (Test-Path -LiteralPath $dir) -and -not $AppPath) { $script:Settings['AppPath'] = (Split-Path -Parent $dir) }
    $p = Get-InstalledPort
    if ($p -gt 0 -and $Port -le 0) { $script:Settings['Port'] = $p }
}
function Adopt-ServicePort {
    # Install / Repair on a folder whose service already exists keeps that service's port unless -Port says otherwise.
    if ($Port -gt 0 -or -not (Get-Svc)) { return }
    $dir = Get-SvcParam 'AppDirectory'
    if ($dir -and $dir.TrimEnd('\').Equals($script:Backend.TrimEnd('\'), [StringComparison]::OrdinalIgnoreCase)) {
        $p = Get-InstalledPort
        if ($p -gt 0) { $script:Settings['Port'] = $p; Step INFO 'Port' "keeping the port of the installed service ($p)" }
    }
}
function Ask-Port {
    # Install and Update always ask which port to build on (Enter keeps the installed service's port, else 8001),
    # before anything is stopped or changed. -Port or -Unattended skips the question.
    if ($script:Unattended -or $Port -gt 0) { return }
    $cur = [int]$script:Settings['Port']
    if ($cur -le 0) { $cur = 8001 }
    for ($i = 0; $i -lt 5; $i++) {
        $ans = Ask 'Port for the site' ([string]$cur)
        if ($ans -match '^\d{4,5}$' -and [int]$ans -ge 1024 -and [int]$ans -le 65535) { $script:Settings['Port'] = [int]$ans; return }
        Say '       enter a number between 1024 and 65535' 'Yellow'
    }
    Stop-Install 'Port' 'no valid port entered'
}function Do-Install([bool]$update) {
    Test-OS
    if ($update) { Resolve-Installed }
    Resolve-AppFolder
    $script:IsUpdate = $update
    Adopt-ServicePort
    Ask-Port
    if ($update -and -not (Get-Svc) -and -not $script:Settings['FirewallRule']) { Stop-Install 'Update' "service $($script:SvcName) is not installed here" 'Use Install / Repair first.' }
    Init-Network
    Ensure-Nssm
    Stop-AppService
    Stop-AppProcesses
    Resolve-Port
    Ensure-Python
    Ensure-Odbc
    Test-Dist
    Ensure-Venv
    Ensure-Packages
    Test-AppImport
    Ensure-EnvFile
    if (-not $update) { Check-Database }
    Sync-Service
    Ensure-Firewall
    Note-OptionalComponents
    [void](Verify-Running)
    Save-Settings
    Show-Summary
}
function Do-Status {
    Test-OS
    Resolve-Installed
    if ($Port -gt 0) { $script:PortNo = $Port } else { $script:PortNo = [int]$script:Settings['Port'] }
    if ($script:PortNo -le 0) { $script:PortNo = 8001 }
    $ok = $true
    $s = Get-Svc
    if (-not $s) { Step FAIL 'Service' "$($script:SvcName) is not installed"; throw (New-Object System.Exception 'VLQMS_FATAL') }
    if ($s.Status -eq 'Running') { Step OK 'Service' 'Running' } else { Step FAIL 'Service' "state is $($s.Status)"; $ok = $false }
    if (Test-Nssm) {
        foreach ($n in @('Application', 'AppParameters', 'AppDirectory', 'Start', 'ObjectName')) { Step INFO "  $n" (Nssm-Get @($n)) }
        $app = Nssm-Get @('Application')
        if ($app -and -not (Test-Path -LiteralPath $app)) { Step FAIL 'Application path' "$app does not exist - run Install / Repair"; $ok = $false }
    }
    $o = Get-PortOwner $script:PortNo
    if ($o) { Step OK "Port $($script:PortNo)" ("{0} (pid {1})" -f $o.Name, $o.Pid) } else { Step FAIL "Port $($script:PortNo)" 'nobody is listening'; $ok = $false }
    $h = Check-Health $script:PortNo
    if ($h.Api) { Step OK 'API' 'answers' } else { Step FAIL 'API' $h.Reason; $ok = $false }
    if ($h.Root) { Step OK 'Site root' 'the app page loads' } else { Step FAIL 'Site root' 'not served'; $ok = $false }
    if ($h.Api) { if ($h.Db) { Step OK 'Database' 'connected' } else { Step WARN 'Database' 'not connected (Database setup pending?)' } }
    $name = "$($script:SvcName) $($script:PortNo)"
    $fr = Get-NetFirewallRule -DisplayName $name -ErrorAction SilentlyContinue
    if ($fr -and "$($fr.Enabled)" -eq 'True') { Step OK 'Firewall rule' $name } else { Step WARN 'Firewall rule' "'$name' is missing or disabled - run Install / Repair" }
    if (-not $ok) { throw (New-Object System.Exception 'VLQMS_FATAL') }
}
function Do-Uninstall {
    Test-OS
    Resolve-Installed
    $s = Get-Svc
    if ($s) {
        if (-not $script:Unattended -and -not (Ask-YesNo "Remove service $($script:SvcName) and its firewall rule? (files, .env and logs are kept)" $false)) { Say 'Cancelled.'; return }
        Stop-AppService
        if (Change 'Remove the service') {
            $r = Nssm-Run @('remove', $script:SvcName, 'confirm')
            if ($r.ExitCode -ne 0 -or (Get-Svc)) { Invoke-Proc 'sc.exe' @('delete', $script:SvcName) | Out-Null }
            Step DONE 'Service removed' $script:SvcName
        }
    } else { Step OK 'Service' 'not installed' }
    $rule = [string]$script:Settings['FirewallRule']
    if (-not $rule -and $Port -gt 0) { $rule = "$($script:SvcName) $Port" }
    if ($rule -and (Get-NetFirewallRule -DisplayName $rule -ErrorAction SilentlyContinue)) {
        if (Change "Remove firewall rule '$rule'") { Remove-NetFirewallRule -DisplayName $rule; Step DONE 'Firewall rule removed' $rule }
    }
    if (-not $script:DryRun) { $script:Settings.Remove('FirewallRule'); Save-Settings }
    Say 'Kept: application files, backend\.env, backend\data, logs. Delete the folder by hand to remove everything.' 'Gray'
}

function Show-Menu {
    Write-Host ''
    Write-Host '  e-vLQMS ERP  -  installer' -ForegroundColor Cyan
    Write-Host '  ---------------------------------'
    Write-Host '   1  Install / Repair'
    Write-Host '   2  Update (after copying a new package over this folder)'
    Write-Host '   3  Status'
    Write-Host '   4  Uninstall (keeps files, .env, logs)'
    Write-Host '   0  Exit'
    $c = Read-Host '  Choose'
    switch ($c.Trim()) { '1' { return 'Install' } '2' { return 'Update' } '3' { return 'Status' } '4' { return 'Uninstall' } default { return 'Exit' } }
}

# ===================================================================================================================
#  Main
# ===================================================================================================================
if ($LoadOnly) { return }

# 32-bit PowerShell on 64-bit Windows: switch to the real 64-bit one first (the registry and drivers differ).
if ([IntPtr]::Size -eq 4 -and $env:PROCESSOR_ARCHITEW6432) {
    $ps64 = Join-Path $env:windir 'sysnative\WindowsPowerShell\v1.0\powershell.exe'
    $a = @('-NoProfile', '-ExecutionPolicy', 'Bypass', '-File', $script:ScriptPath)
    foreach ($k in $script:Bound.Keys) { $v = $script:Bound[$k]; if ($v -is [System.Management.Automation.SwitchParameter]) { if ($v.IsPresent) { $a += "-$k" } } else { $a += "-$k"; $a += [string]$v } }
    $p = Start-Process -FilePath $ps64 -ArgumentList (($a | ForEach-Object { Quote-Arg $_ }) -join ' ') -Wait -PassThru -NoNewWindow
    exit $p.ExitCode
}

# Self-elevate, passing this script's own arguments on (a dry run only reads, so it needs no elevation).
if (-not $script:DryRun -and -not (Test-Admin)) {
    $a = @('-NoProfile', '-ExecutionPolicy', 'Bypass', '-File', $script:ScriptPath)
    foreach ($k in $script:Bound.Keys) {
        $v = $script:Bound[$k]
        if ($k -eq 'DbPassword') { $env:VLQMS_DB_PASSWORD = [string]$v; continue }          # secrets travel in the environment, never on the command line
        if ($k -eq 'ServicePassword') { $env:VLQMS_SERVICE_PASSWORD = [string]$v; continue }
        if ($v -is [System.Management.Automation.SwitchParameter]) { if ($v.IsPresent) { $a += "-$k" } }
        else { $a += "-$k"; $a += [string]$v }
    }
    Write-Host 'Administrator rights are needed - accept the Windows prompt...' -ForegroundColor Yellow
    try {
        $p = Start-Process -FilePath (Join-Path $env:windir 'System32\WindowsPowerShell\v1.0\powershell.exe') -ArgumentList (($a | ForEach-Object { Quote-Arg $_ }) -join ' ') -Verb RunAs -Wait -PassThru
        exit $p.ExitCode
    } catch { Write-Host 'Elevation was declined or failed: ' $_.Exception.Message -ForegroundColor Red; exit 1 }
}

$exit = 0
try {
    Load-Settings
    if ($script:Unattended -and $Mode -eq 'Menu') { $Mode = 'Install' }
    if ($Mode -eq 'Menu') { $Mode = Show-Menu }
    if ($Mode -eq 'Exit') { exit 0 }
    $pw0 = $DbPassword; if ($pw0) { [void]$script:Secrets.Add($pw0) }
    if ($env:VLQMS_DB_PASSWORD) { [void]$script:Secrets.Add($env:VLQMS_DB_PASSWORD) }
    if ($env:VLQMS_SERVICE_PASSWORD) { [void]$script:Secrets.Add($env:VLQMS_SERVICE_PASSWORD) }
    Say ("e-vLQMS installer - mode $Mode" + $(if ($script:DryRun) { ' (DRY RUN: nothing will be changed)' } else { '' })) 'Cyan'
    switch ($Mode) {
        'Install' { Do-Install $false }
        'Update' { Do-Install $true }
        'Status' { Do-Status }
        'Uninstall' { Do-Uninstall }
    }
} catch {
    $exit = 1
    if ($_.Exception.Message -ne 'VLQMS_FATAL') {
        Step FAIL 'Unexpected error' ($_.Exception.Message + ' at ' + $_.InvocationInfo.PositionMessage)
    }
    Say 'The installer stopped. Fix the problem above and run it again: every step is safe to repeat.' 'Red'
}
if ($script:LogFile) { Say ("Log: " + $script:LogFile) 'Gray' }
if (-not $script:Unattended -and $PSCommandPath -and $Host.Name -eq 'ConsoleHost') { Read-Host 'Press Enter to close' | Out-Null }
exit $exit
