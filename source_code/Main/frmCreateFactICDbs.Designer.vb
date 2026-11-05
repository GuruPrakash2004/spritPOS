<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCreateFactICDbs
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCreateFactICDbs))
        Me.txtGlobeDB = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtLoginName = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtDBName = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtServerName = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtPwd = New System.Windows.Forms.TextBox()
        Me.btnFactICDBs = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.btnGlobeBrowse = New System.Windows.Forms.Button()
        Me.Cdlg = New System.Windows.Forms.OpenFileDialog()
        Me.SuspendLayout()
        '
        'txtGlobeDB
        '
        Me.txtGlobeDB.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtGlobeDB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtGlobeDB.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGlobeDB.Location = New System.Drawing.Point(273, 83)
        Me.txtGlobeDB.Name = "txtGlobeDB"
        Me.txtGlobeDB.Size = New System.Drawing.Size(402, 30)
        Me.txtGlobeDB.TabIndex = 15
        '
        'Label9
        '
        Me.Label9.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label9.Location = New System.Drawing.Point(10, 87)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(268, 20)
        Me.Label9.TabIndex = 14
        Me.Label9.Text = "Global Database Backup Filename"
        '
        'txtLoginName
        '
        Me.txtLoginName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLoginName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtLoginName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLoginName.Location = New System.Drawing.Point(273, 112)
        Me.txtLoginName.Name = "txtLoginName"
        Me.txtLoginName.Size = New System.Drawing.Size(136, 30)
        Me.txtLoginName.TabIndex = 17
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(10, 112)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(133, 20)
        Me.Label4.TabIndex = 16
        Me.Label4.Text = "SQL User Name"
        '
        'txtDBName
        '
        Me.txtDBName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDBName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDBName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDBName.Location = New System.Drawing.Point(273, 52)
        Me.txtDBName.Name = "txtDBName"
        Me.txtDBName.Size = New System.Drawing.Size(402, 30)
        Me.txtDBName.TabIndex = 13
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(10, 58)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(265, 20)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "Main Database Backup File-Name"
        '
        'txtServerName
        '
        Me.txtServerName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtServerName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtServerName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtServerName.Location = New System.Drawing.Point(273, 21)
        Me.txtServerName.Name = "txtServerName"
        Me.txtServerName.Size = New System.Drawing.Size(439, 30)
        Me.txtServerName.TabIndex = 9
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(10, 27)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(107, 20)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Server Name"
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label5.Location = New System.Drawing.Point(10, 141)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(83, 20)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Password"
        '
        'txtPwd
        '
        Me.txtPwd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPwd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPwd.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPwd.Location = New System.Drawing.Point(273, 140)
        Me.txtPwd.Name = "txtPwd"
        Me.txtPwd.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPwd.Size = New System.Drawing.Size(136, 30)
        Me.txtPwd.TabIndex = 12
        '
        'btnFactICDBs
        '
        Me.btnFactICDBs.BackColor = System.Drawing.Color.Transparent
        Me.btnFactICDBs.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFactICDBs.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnFactICDBs.Location = New System.Drawing.Point(323, 192)
        Me.btnFactICDBs.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnFactICDBs.Name = "btnFactICDBs"
        Me.btnFactICDBs.Size = New System.Drawing.Size(247, 63)
        Me.btnFactICDBs.TabIndex = 18
        Me.btnFactICDBs.Text = "Create and Restore FactIC Databases"
        Me.btnFactICDBs.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.Color.Transparent
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(585, 192)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(140, 63)
        Me.btnClose.TabIndex = 19
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnBrowse
        '
        Me.btnBrowse.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowse.Image = CType(resources.GetObject("btnBrowse.Image"), System.Drawing.Image)
        Me.btnBrowse.Location = New System.Drawing.Point(685, 58)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(27, 29)
        Me.btnBrowse.TabIndex = 20
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'btnGlobeBrowse
        '
        Me.btnGlobeBrowse.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGlobeBrowse.Image = CType(resources.GetObject("btnGlobeBrowse.Image"), System.Drawing.Image)
        Me.btnGlobeBrowse.Location = New System.Drawing.Point(685, 88)
        Me.btnGlobeBrowse.Name = "btnGlobeBrowse"
        Me.btnGlobeBrowse.Size = New System.Drawing.Size(27, 29)
        Me.btnGlobeBrowse.TabIndex = 21
        Me.btnGlobeBrowse.UseVisualStyleBackColor = True
        '
        'Cdlg
        '
        Me.Cdlg.FileName = "Cdlg"
        '
        'frmCreateFactICDbs
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(774, 269)
        Me.Controls.Add(Me.btnGlobeBrowse)
        Me.Controls.Add(Me.btnBrowse)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnFactICDBs)
        Me.Controls.Add(Me.txtGlobeDB)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.txtLoginName)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtDBName)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtServerName)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtPwd)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmCreateFactICDbs"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Create and Restore FACTIC Databses (Main and Globe)"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtGlobeDB As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents txtLoginName As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents txtDBName As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtServerName As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents txtPwd As TextBox
    Friend WithEvents btnFactICDBs As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents btnBrowse As Button
    Friend WithEvents btnGlobeBrowse As Button
    Friend WithEvents Cdlg As OpenFileDialog
End Class
