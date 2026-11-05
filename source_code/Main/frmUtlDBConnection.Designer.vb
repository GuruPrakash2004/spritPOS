<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlDBConnection
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlDBConnection))
        Me.tabDB = New C1.Win.C1Command.C1DockingTab()
        Me.tabAccess = New C1.Win.C1Command.C1DockingTabPage()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtDataPath = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.tabSQL = New C1.Win.C1Command.C1DockingTabPage()
        Me.btnRptPath = New System.Windows.Forms.Button()
        Me.txtRptPath = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtLoginName = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtPwd = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtDBName = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtServerName = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.fbd = New System.Windows.Forms.FolderBrowserDialog()
        Me.PicBoxAccess = New System.Windows.Forms.PictureBox()
        Me.PicBoxSQL = New System.Windows.Forms.PictureBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnConnect = New System.Windows.Forms.Button()
        Me.btnInitDBs = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnHelpConn = New System.Windows.Forms.Button()
        Me.vsfdet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.txtClientName = New System.Windows.Forms.TextBox()
        CType(Me.tabDB, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabDB.SuspendLayout()
        Me.tabAccess.SuspendLayout()
        Me.tabSQL.SuspendLayout()
        CType(Me.PicBoxAccess, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PicBoxSQL, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfdet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tabDB
        '
        Me.tabDB.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabDB.Controls.Add(Me.tabAccess)
        Me.tabDB.Controls.Add(Me.tabSQL)
        Me.tabDB.Location = New System.Drawing.Point(154, 16)
        Me.tabDB.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.tabDB.Name = "tabDB"
        Me.tabDB.SelectedIndex = 2
        Me.tabDB.Size = New System.Drawing.Size(778, 288)
        Me.tabDB.TabIndex = 0
        Me.tabDB.TabStyle = C1.Win.C1Command.TabStyleEnum.Office2007
        Me.tabDB.VisualStyle = C1.Win.C1Command.VisualStyle.Office2007Black
        Me.tabDB.VisualStyleBase = C1.Win.C1Command.VisualStyle.Office2007Black
        '
        'tabAccess
        '
        Me.tabAccess.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabAccess.Controls.Add(Me.Label8)
        Me.tabAccess.Controls.Add(Me.Label7)
        Me.tabAccess.Controls.Add(Me.Label6)
        Me.tabAccess.Controls.Add(Me.txtDataPath)
        Me.tabAccess.Controls.Add(Me.Label2)
        Me.tabAccess.Controls.Add(Me.btnBrowse)
        Me.tabAccess.Location = New System.Drawing.Point(1, 29)
        Me.tabAccess.Name = "tabAccess"
        Me.tabAccess.Size = New System.Drawing.Size(776, 258)
        Me.tabAccess.TabIndex = 0
        Me.tabAccess.Text = "Connect to Access Database"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.FromArgb(CType(CType(227, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.Label8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label8.Location = New System.Drawing.Point(3, 108)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(154, 17)
        Me.Label8.TabIndex = 5
        Me.Label8.Text = "Ex : ""D:\vLQMS6.0\Data\"""
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.FromArgb(CType(CType(227, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.Label7.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Red
        Me.Label7.Location = New System.Drawing.Point(3, 45)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(380, 13)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "If you want to LINK to an Existing Database then click on this Button --->"
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.Color.FromArgb(CType(CType(227, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.Label6.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label6.Location = New System.Drawing.Point(3, 69)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(442, 40)
        Me.Label6.TabIndex = 3
        Me.Label6.Text = "Data Path can be set for maintaining  the details of a particular Company in any " &
    "specified Location where the Database resides. The Selected Folder Should contai" &
    "n GLOBE.MDB and C1.MDB." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'txtDataPath
        '
        Me.txtDataPath.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDataPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDataPath.Location = New System.Drawing.Point(401, 8)
        Me.txtDataPath.Name = "txtDataPath"
        Me.txtDataPath.Size = New System.Drawing.Size(360, 25)
        Me.txtDataPath.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(227, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(331, 12)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(64, 17)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Data Path"
        '
        'btnBrowse
        '
        Me.btnBrowse.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowse.Image = CType(resources.GetObject("btnBrowse.Image"), System.Drawing.Image)
        Me.btnBrowse.Location = New System.Drawing.Point(734, 36)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(27, 29)
        Me.btnBrowse.TabIndex = 2
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'tabSQL
        '
        Me.tabSQL.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabSQL.BackColor = System.Drawing.Color.FromArgb(CType(CType(227, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.tabSQL.Controls.Add(Me.btnRptPath)
        Me.tabSQL.Controls.Add(Me.txtRptPath)
        Me.tabSQL.Controls.Add(Me.Label9)
        Me.tabSQL.Controls.Add(Me.txtLoginName)
        Me.tabSQL.Controls.Add(Me.Label4)
        Me.tabSQL.Controls.Add(Me.txtPwd)
        Me.tabSQL.Controls.Add(Me.Label5)
        Me.tabSQL.Controls.Add(Me.txtDBName)
        Me.tabSQL.Controls.Add(Me.Label3)
        Me.tabSQL.Controls.Add(Me.txtServerName)
        Me.tabSQL.Controls.Add(Me.Label1)
        Me.tabSQL.ForeColor = System.Drawing.Color.Red
        Me.tabSQL.Location = New System.Drawing.Point(1, 29)
        Me.tabSQL.Name = "tabSQL"
        Me.tabSQL.Size = New System.Drawing.Size(776, 258)
        Me.tabSQL.TabIndex = 0
        Me.tabSQL.Text = "Connect to SQL Database"
        '
        'btnRptPath
        '
        Me.btnRptPath.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRptPath.BackgroundImage = Global.vLQMS.My.Resources.Resources.T5_FIND
        Me.btnRptPath.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnRptPath.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRptPath.ForeColor = System.Drawing.Color.Black
        Me.btnRptPath.Location = New System.Drawing.Point(739, 147)
        Me.btnRptPath.Name = "btnRptPath"
        Me.btnRptPath.Size = New System.Drawing.Size(34, 27)
        Me.btnRptPath.TabIndex = 9
        Me.ToolTip1.SetToolTip(Me.btnRptPath, "Click to Select Folder")
        Me.btnRptPath.UseVisualStyleBackColor = True
        '
        'txtRptPath
        '
        Me.txtRptPath.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtRptPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtRptPath.Location = New System.Drawing.Point(235, 131)
        Me.txtRptPath.Name = "txtRptPath"
        Me.txtRptPath.Size = New System.Drawing.Size(501, 25)
        Me.txtRptPath.TabIndex = 8
        '
        'Label9
        '
        Me.Label9.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.FromArgb(CType(CType(227, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.Label9.ForeColor = System.Drawing.Color.Black
        Me.Label9.Location = New System.Drawing.Point(94, 134)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(77, 17)
        Me.Label9.TabIndex = 7
        Me.Label9.Text = "Report Path"
        '
        'txtLoginName
        '
        Me.txtLoginName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLoginName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtLoginName.Location = New System.Drawing.Point(235, 69)
        Me.txtLoginName.Name = "txtLoginName"
        Me.txtLoginName.Size = New System.Drawing.Size(372, 25)
        Me.txtLoginName.TabIndex = 3
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(227, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(94, 69)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(101, 17)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "SQL User Name"
        '
        'txtPwd
        '
        Me.txtPwd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPwd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPwd.Location = New System.Drawing.Point(235, 100)
        Me.txtPwd.Name = "txtPwd"
        Me.txtPwd.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPwd.Size = New System.Drawing.Size(372, 25)
        Me.txtPwd.TabIndex = 4
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(227, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.Label5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label5.Location = New System.Drawing.Point(94, 103)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(64, 17)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "Password"
        '
        'txtDBName
        '
        Me.txtDBName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDBName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDBName.Location = New System.Drawing.Point(235, 38)
        Me.txtDBName.Name = "txtDBName"
        Me.txtDBName.Size = New System.Drawing.Size(519, 25)
        Me.txtDBName.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(227, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(94, 38)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(102, 17)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Database Name"
        '
        'txtServerName
        '
        Me.txtServerName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtServerName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtServerName.Location = New System.Drawing.Point(235, 7)
        Me.txtServerName.Name = "txtServerName"
        Me.txtServerName.Size = New System.Drawing.Size(519, 25)
        Me.txtServerName.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(227, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(233, Byte), Integer))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(94, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(84, 17)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Server Name"
        '
        'PicBoxAccess
        '
        Me.PicBoxAccess.Image = CType(resources.GetObject("PicBoxAccess.Image"), System.Drawing.Image)
        Me.PicBoxAccess.Location = New System.Drawing.Point(12, 16)
        Me.PicBoxAccess.Name = "PicBoxAccess"
        Me.PicBoxAccess.Size = New System.Drawing.Size(154, 288)
        Me.PicBoxAccess.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PicBoxAccess.TabIndex = 6
        Me.PicBoxAccess.TabStop = False
        '
        'PicBoxSQL
        '
        Me.PicBoxSQL.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PicBoxSQL.Image = CType(resources.GetObject("PicBoxSQL.Image"), System.Drawing.Image)
        Me.PicBoxSQL.Location = New System.Drawing.Point(12, 16)
        Me.PicBoxSQL.Name = "PicBoxSQL"
        Me.PicBoxSQL.Size = New System.Drawing.Size(154, 288)
        Me.PicBoxSQL.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PicBoxSQL.TabIndex = 5
        Me.PicBoxSQL.TabStop = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Image = CType(resources.GetObject("btnCancel.Image"), System.Drawing.Image)
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(849, 314)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(83, 30)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Ca&ncel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnConnect
        '
        Me.btnConnect.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnConnect.Image = CType(resources.GetObject("btnConnect.Image"), System.Drawing.Image)
        Me.btnConnect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnConnect.Location = New System.Drawing.Point(749, 314)
        Me.btnConnect.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnConnect.Name = "btnConnect"
        Me.btnConnect.Size = New System.Drawing.Size(94, 30)
        Me.btnConnect.TabIndex = 0
        Me.btnConnect.Text = "&Connect"
        Me.btnConnect.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnConnect.UseVisualStyleBackColor = True
        '
        'btnInitDBs
        '
        Me.btnInitDBs.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnInitDBs.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnInitDBs.Location = New System.Drawing.Point(335, 314)
        Me.btnInitDBs.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnInitDBs.Name = "btnInitDBs"
        Me.btnInitDBs.Size = New System.Drawing.Size(238, 30)
        Me.btnInitDBs.TabIndex = 2
        Me.btnInitDBs.Text = "Initialise FACTIC Databases"
        Me.btnInitDBs.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnInitDBs.UseVisualStyleBackColor = True
        '
        'btnHelpConn
        '
        Me.btnHelpConn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnHelpConn.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnHelpConn.Location = New System.Drawing.Point(12, 312)
        Me.btnHelpConn.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnHelpConn.Name = "btnHelpConn"
        Me.btnHelpConn.Size = New System.Drawing.Size(268, 32)
        Me.btnHelpConn.TabIndex = 7
        Me.btnHelpConn.Text = "Connectivity HelpDoc"
        Me.btnHelpConn.UseVisualStyleBackColor = False
        '
        'vsfdet
        '
        Me.vsfdet.AllowFiltering = True
        Me.vsfdet.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfdet.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfdet.AutoResize = True
        Me.vsfdet.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfdet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfdet.ColumnInfo = "2,0,0,0,0,120,Columns:0{Width:97;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfdet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfdet.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
        Me.vsfdet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfdet.Location = New System.Drawing.Point(12, 353)
        Me.vsfdet.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.vsfdet.Name = "vsfdet"
        Me.vsfdet.Rows.DefaultSize = 24
        Me.vsfdet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfdet.Size = New System.Drawing.Size(920, 240)
        Me.vsfdet.StyleInfo = resources.GetString("vsfdet.StyleInfo")
        Me.vsfdet.TabIndex = 18
        Me.vsfdet.Tag = ""
        Me.vsfdet.Visible = False
        '
        'btnLoad
        '
        Me.btnLoad.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLoad.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnLoad.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnLoad.Location = New System.Drawing.Point(661, 314)
        Me.btnLoad.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(82, 30)
        Me.btnLoad.TabIndex = 20
        Me.btnLoad.Text = "Load"
        Me.btnLoad.UseVisualStyleBackColor = True
        Me.btnLoad.Visible = False
        '
        'txtClientName
        '
        Me.txtClientName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtClientName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtClientName.Location = New System.Drawing.Point(286, 314)
        Me.txtClientName.Name = "txtClientName"
        Me.txtClientName.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtClientName.Size = New System.Drawing.Size(37, 25)
        Me.txtClientName.TabIndex = 21
        Me.txtClientName.Visible = False
        '
        'frmUtlDBConnection
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(944, 607)
        Me.Controls.Add(Me.txtClientName)
        Me.Controls.Add(Me.btnLoad)
        Me.Controls.Add(Me.vsfdet)
        Me.Controls.Add(Me.btnHelpConn)
        Me.Controls.Add(Me.btnInitDBs)
        Me.Controls.Add(Me.PicBoxAccess)
        Me.Controls.Add(Me.PicBoxSQL)
        Me.Controls.Add(Me.tabDB)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnConnect)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "frmUtlDBConnection"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Database Connectivity"
        CType(Me.tabDB, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabDB.ResumeLayout(False)
        Me.tabAccess.ResumeLayout(False)
        Me.tabAccess.PerformLayout()
        Me.tabSQL.ResumeLayout(False)
        Me.tabSQL.PerformLayout()
        CType(Me.PicBoxAccess, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PicBoxSQL, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfdet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnConnect As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents tabDB As C1.Win.C1Command.C1DockingTab
    Friend WithEvents tabAccess As C1.Win.C1Command.C1DockingTabPage
    Friend WithEvents tabSQL As C1.Win.C1Command.C1DockingTabPage
    Friend WithEvents txtDataPath As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents txtServerName As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtPwd As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtLoginName As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtDBName As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents PicBoxSQL As System.Windows.Forms.PictureBox
    Friend WithEvents fbd As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents PicBoxAccess As System.Windows.Forms.PictureBox
    Friend WithEvents btnInitDBs As Button
    Friend WithEvents txtRptPath As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents btnRptPath As Button
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents btnHelpConn As Button
    Public WithEvents vsfdet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnLoad As Button
    Friend WithEvents txtClientName As TextBox
End Class
