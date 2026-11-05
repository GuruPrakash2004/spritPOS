<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlMenuSequence
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlMenuSequence))
        Me.cboType = New System.Windows.Forms.ComboBox()
        Me.BtnLoad = New System.Windows.Forms.Button()
        Me.FileSystemWatcher1 = New System.IO.FileSystemWatcher()
        Me.vsfDET = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.FraImg = New System.Windows.Forms.Panel()
        Me.chkSelKey = New System.Windows.Forms.CheckBox()
        Me.pnlSCKeys = New System.Windows.Forms.Panel()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.btnApply = New System.Windows.Forms.Button()
        Me.cboKey = New System.Windows.Forms.ComboBox()
        Me.lblKey = New System.Windows.Forms.Label()
        Me.chkShift = New System.Windows.Forms.CheckBox()
        Me.chkAlt = New System.Windows.Forms.CheckBox()
        Me.chkCtrl = New System.Windows.Forms.CheckBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.lblHelp = New System.Windows.Forms.Label()
        Me.GrpImg = New System.Windows.Forms.GroupBox()
        Me.BtnRemove = New System.Windows.Forms.Button()
        Me.PicBox = New System.Windows.Forms.PictureBox()
        Me.BtnUpload = New System.Windows.Forms.Button()
        Me.btnAddGroup = New System.Windows.Forms.Button()
        Me.fraFooter = New System.Windows.Forms.Panel()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.BtnAddSeprator = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.FraGrpName = New System.Windows.Forms.Panel()
        Me.cboGrpName = New System.Windows.Forms.ComboBox()
        Me.BtnOk = New System.Windows.Forms.Button()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.lblAddGroup = New System.Windows.Forms.Label()
        Me.txtGrpName = New System.Windows.Forms.TextBox()
        Me.frame1 = New System.Windows.Forms.Panel()
        Me.MnuType = New System.Windows.Forms.Label()
        Me.PnlArrow = New System.Windows.Forms.Panel()
        Me.btnMvUP = New System.Windows.Forms.Button()
        Me.btnMvDwn = New System.Windows.Forms.Button()
        Me.Frame4 = New System.Windows.Forms.Panel()
        Me.OpenFileDialogPic = New System.Windows.Forms.OpenFileDialog()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.FileSystemWatcher1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfDET, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.FraImg.SuspendLayout()
        Me.pnlSCKeys.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GrpImg.SuspendLayout()
        CType(Me.PicBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraFooter.SuspendLayout()
        Me.FraGrpName.SuspendLayout()
        Me.frame1.SuspendLayout()
        Me.PnlArrow.SuspendLayout()
        Me.Frame4.SuspendLayout()
        Me.SuspendLayout()
        '
        'cboType
        '
        Me.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboType.FormattingEnabled = True
        Me.cboType.Items.AddRange(New Object() {"Master", "Transactions", "Reports", "Vouchers", "ACReports", "Tools", "Utilities"})
        Me.cboType.Location = New System.Drawing.Point(189, 9)
        Me.cboType.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cboType.Name = "cboType"
        Me.cboType.Size = New System.Drawing.Size(208, 33)
        Me.cboType.TabIndex = 1
        '
        'BtnLoad
        '
        Me.BtnLoad.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnLoad.Location = New System.Drawing.Point(580, 9)
        Me.BtnLoad.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.BtnLoad.Name = "BtnLoad"
        Me.BtnLoad.Size = New System.Drawing.Size(103, 34)
        Me.BtnLoad.TabIndex = 2
        Me.BtnLoad.Text = "&Load"
        Me.ToolTip1.SetToolTip(Me.BtnLoad, "Load : Alt + L")
        Me.BtnLoad.UseVisualStyleBackColor = True
        '
        'FileSystemWatcher1
        '
        Me.FileSystemWatcher1.EnableRaisingEvents = True
        Me.FileSystemWatcher1.SynchronizingObject = Me
        '
        'vsfDET
        '
        Me.vsfDET.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfDET.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDET.AutoResize = True
        Me.vsfDET.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDET.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfDET.ExtendLastCol = True
        Me.vsfDET.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.vsfDET.Location = New System.Drawing.Point(60, 53)
        Me.vsfDET.Margin = New System.Windows.Forms.Padding(7, 6, 7, 6)
        Me.vsfDET.Name = "vsfDET"
        Me.vsfDET.Rows.DefaultSize = 24
        Me.vsfDET.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.RowRange
        Me.vsfDET.Size = New System.Drawing.Size(625, 406)
        Me.vsfDET.TabIndex = 228
        '
        'FraImg
        '
        Me.FraImg.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FraImg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.FraImg.Controls.Add(Me.chkSelKey)
        Me.FraImg.Controls.Add(Me.pnlSCKeys)
        Me.FraImg.Controls.Add(Me.lblHelp)
        Me.FraImg.Controls.Add(Me.GrpImg)
        Me.FraImg.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FraImg.Location = New System.Drawing.Point(717, 14)
        Me.FraImg.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.FraImg.Name = "FraImg"
        Me.FraImg.Size = New System.Drawing.Size(397, 468)
        Me.FraImg.TabIndex = 229
        '
        'chkSelKey
        '
        Me.chkSelKey.AutoSize = True
        Me.chkSelKey.Location = New System.Drawing.Point(20, 110)
        Me.chkSelKey.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.chkSelKey.Name = "chkSelKey"
        Me.chkSelKey.Size = New System.Drawing.Size(152, 29)
        Me.chkSelKey.TabIndex = 5
        Me.chkSelKey.Text = "ShortCut Key"
        Me.chkSelKey.UseVisualStyleBackColor = True
        '
        'pnlSCKeys
        '
        Me.pnlSCKeys.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlSCKeys.Controls.Add(Me.btnClear)
        Me.pnlSCKeys.Controls.Add(Me.btnApply)
        Me.pnlSCKeys.Controls.Add(Me.cboKey)
        Me.pnlSCKeys.Controls.Add(Me.lblKey)
        Me.pnlSCKeys.Controls.Add(Me.chkShift)
        Me.pnlSCKeys.Controls.Add(Me.chkAlt)
        Me.pnlSCKeys.Controls.Add(Me.chkCtrl)
        Me.pnlSCKeys.Controls.Add(Me.PictureBox1)
        Me.pnlSCKeys.Location = New System.Drawing.Point(3, 121)
        Me.pnlSCKeys.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.pnlSCKeys.Name = "pnlSCKeys"
        Me.pnlSCKeys.Size = New System.Drawing.Size(389, 146)
        Me.pnlSCKeys.TabIndex = 4
        '
        'btnClear
        '
        Me.btnClear.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClear.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.btnClear.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClear.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClear.Location = New System.Drawing.Point(152, 106)
        Me.btnClear.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(120, 36)
        Me.btnClear.TabIndex = 231
        Me.btnClear.Text = "&Cancel"
        Me.btnClear.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClear, "Cancel : Alt + C")
        Me.btnClear.UseVisualStyleBackColor = False
        '
        'btnApply
        '
        Me.btnApply.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnApply.Location = New System.Drawing.Point(283, 106)
        Me.btnApply.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(100, 36)
        Me.btnApply.TabIndex = 7
        Me.btnApply.Text = "A&pply"
        Me.ToolTip1.SetToolTip(Me.btnApply, "Apply : Alt + P")
        Me.btnApply.UseVisualStyleBackColor = True
        '
        'cboKey
        '
        Me.cboKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboKey.FormattingEnabled = True
        Me.cboKey.Items.AddRange(New Object() {"", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12"})
        Me.cboKey.Location = New System.Drawing.Point(220, 48)
        Me.cboKey.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cboKey.Name = "cboKey"
        Me.cboKey.Size = New System.Drawing.Size(160, 33)
        Me.cboKey.TabIndex = 6
        '
        'lblKey
        '
        Me.lblKey.AutoSize = True
        Me.lblKey.Location = New System.Drawing.Point(165, 52)
        Me.lblKey.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblKey.Name = "lblKey"
        Me.lblKey.Size = New System.Drawing.Size(47, 25)
        Me.lblKey.TabIndex = 5
        Me.lblKey.Text = "Key"
        '
        'chkShift
        '
        Me.chkShift.AutoSize = True
        Me.chkShift.Location = New System.Drawing.Point(83, 80)
        Me.chkShift.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.chkShift.Name = "chkShift"
        Me.chkShift.Size = New System.Drawing.Size(73, 29)
        Me.chkShift.TabIndex = 4
        Me.chkShift.Text = "Shift"
        Me.chkShift.UseVisualStyleBackColor = True
        '
        'chkAlt
        '
        Me.chkAlt.AutoSize = True
        Me.chkAlt.Location = New System.Drawing.Point(83, 53)
        Me.chkAlt.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.chkAlt.Name = "chkAlt"
        Me.chkAlt.Size = New System.Drawing.Size(57, 29)
        Me.chkAlt.TabIndex = 3
        Me.chkAlt.Text = "Alt"
        Me.chkAlt.UseVisualStyleBackColor = True
        '
        'chkCtrl
        '
        Me.chkCtrl.AutoSize = True
        Me.chkCtrl.Location = New System.Drawing.Point(83, 26)
        Me.chkCtrl.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.chkCtrl.Name = "chkCtrl"
        Me.chkCtrl.Size = New System.Drawing.Size(64, 29)
        Me.chkCtrl.TabIndex = 2
        Me.chkCtrl.Text = "Ctrl"
        Me.chkCtrl.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.vLQMS.My.Resources.Resources.hotkey1
        Me.PictureBox1.Location = New System.Drawing.Point(16, 25)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(59, 50)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 1
        Me.PictureBox1.TabStop = False
        '
        'lblHelp
        '
        Me.lblHelp.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblHelp.ForeColor = System.Drawing.Color.Blue
        Me.lblHelp.Location = New System.Drawing.Point(11, 14)
        Me.lblHelp.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblHelp.Name = "lblHelp"
        Me.lblHelp.Size = New System.Drawing.Size(373, 91)
        Me.lblHelp.TabIndex = 3
        '
        'GrpImg
        '
        Me.GrpImg.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GrpImg.Controls.Add(Me.BtnRemove)
        Me.GrpImg.Controls.Add(Me.PicBox)
        Me.GrpImg.Controls.Add(Me.BtnUpload)
        Me.GrpImg.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.GrpImg.Location = New System.Drawing.Point(4, 277)
        Me.GrpImg.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GrpImg.Name = "GrpImg"
        Me.GrpImg.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GrpImg.Size = New System.Drawing.Size(380, 185)
        Me.GrpImg.TabIndex = 2
        Me.GrpImg.TabStop = False
        Me.GrpImg.Text = "Image"
        '
        'BtnRemove
        '
        Me.BtnRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnRemove.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnRemove.Image = CType(resources.GetObject("BtnRemove.Image"), System.Drawing.Image)
        Me.BtnRemove.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnRemove.Location = New System.Drawing.Point(324, 142)
        Me.BtnRemove.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.BtnRemove.Name = "BtnRemove"
        Me.BtnRemove.Size = New System.Drawing.Size(45, 34)
        Me.BtnRemove.TabIndex = 41
        Me.BtnRemove.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BtnRemove.UseVisualStyleBackColor = True
        Me.BtnRemove.Visible = False
        '
        'PicBox
        '
        Me.PicBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PicBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PicBox.InitialImage = Nothing
        Me.PicBox.Location = New System.Drawing.Point(7, 31)
        Me.PicBox.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.PicBox.Name = "PicBox"
        Me.PicBox.Size = New System.Drawing.Size(362, 103)
        Me.PicBox.TabIndex = 0
        Me.PicBox.TabStop = False
        '
        'BtnUpload
        '
        Me.BtnUpload.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.BtnUpload.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnUpload.Location = New System.Drawing.Point(8, 142)
        Me.BtnUpload.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.BtnUpload.Name = "BtnUpload"
        Me.BtnUpload.Size = New System.Drawing.Size(100, 36)
        Me.BtnUpload.TabIndex = 1
        Me.BtnUpload.Text = "&Upload"
        Me.ToolTip1.SetToolTip(Me.BtnUpload, "Upload : Alt + U")
        Me.BtnUpload.UseVisualStyleBackColor = True
        '
        'btnAddGroup
        '
        Me.btnAddGroup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAddGroup.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAddGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAddGroup.Location = New System.Drawing.Point(209, 9)
        Me.btnAddGroup.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnAddGroup.Name = "btnAddGroup"
        Me.btnAddGroup.Size = New System.Drawing.Size(137, 38)
        Me.btnAddGroup.TabIndex = 231
        Me.btnAddGroup.Text = "Add &Group"
        Me.ToolTip1.SetToolTip(Me.btnAddGroup, "Add Group : Alt + G")
        Me.btnAddGroup.UseVisualStyleBackColor = True
        '
        'fraFooter
        '
        Me.fraFooter.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fraFooter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.fraFooter.Controls.Add(Me.btnDelete)
        Me.fraFooter.Controls.Add(Me.BtnAddSeprator)
        Me.fraFooter.Controls.Add(Me.btnAddGroup)
        Me.fraFooter.Controls.Add(Me.btnClose)
        Me.fraFooter.Controls.Add(Me.btnSave)
        Me.fraFooter.Location = New System.Drawing.Point(9, 492)
        Me.fraFooter.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.fraFooter.Name = "fraFooter"
        Me.fraFooter.Size = New System.Drawing.Size(1111, 57)
        Me.fraFooter.TabIndex = 230
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDelete.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_DELETE
        Me.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDelete.Location = New System.Drawing.Point(355, 9)
        Me.btnDelete.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(137, 38)
        Me.btnDelete.TabIndex = 233
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnDelete, "Delete : Alt + D")
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'BtnAddSeprator
        '
        Me.BtnAddSeprator.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnAddSeprator.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnAddSeprator.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAddSeprator.Location = New System.Drawing.Point(12, 9)
        Me.BtnAddSeprator.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.BtnAddSeprator.Name = "BtnAddSeprator"
        Me.BtnAddSeprator.Size = New System.Drawing.Size(184, 38)
        Me.BtnAddSeprator.TabIndex = 232
        Me.BtnAddSeprator.Text = "&Add separator"
        Me.ToolTip1.SetToolTip(Me.BtnAddSeprator, "Add Seperator : Alt + A")
        Me.BtnAddSeprator.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackColor = System.Drawing.Color.Transparent
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Location = New System.Drawing.Point(983, 11)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(120, 36)
        Me.btnClose.TabIndex = 7
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close : Alt + O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackColor = System.Drawing.Color.Transparent
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Location = New System.Drawing.Point(855, 11)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(120, 36)
        Me.btnSave.TabIndex = 6
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Save : Alt + S")
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'FraGrpName
        '
        Me.FraGrpName.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.FraGrpName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.FraGrpName.Controls.Add(Me.cboGrpName)
        Me.FraGrpName.Controls.Add(Me.BtnOk)
        Me.FraGrpName.Controls.Add(Me.BtnCancel)
        Me.FraGrpName.Controls.Add(Me.lblAddGroup)
        Me.FraGrpName.Controls.Add(Me.txtGrpName)
        Me.FraGrpName.Location = New System.Drawing.Point(181, 188)
        Me.FraGrpName.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.FraGrpName.Name = "FraGrpName"
        Me.FraGrpName.Size = New System.Drawing.Size(381, 140)
        Me.FraGrpName.TabIndex = 232
        '
        'cboGrpName
        '
        Me.cboGrpName.FormattingEnabled = True
        Me.cboGrpName.Items.AddRange(New Object() {"Master", "Transactions", "Reports", "Vouchers", "ACReports", "Tools", "Utilities"})
        Me.cboGrpName.Location = New System.Drawing.Point(128, 21)
        Me.cboGrpName.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cboGrpName.MaxLength = 50
        Me.cboGrpName.Name = "cboGrpName"
        Me.cboGrpName.Size = New System.Drawing.Size(228, 33)
        Me.cboGrpName.TabIndex = 234
        '
        'BtnOk
        '
        Me.BtnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnOk.BackColor = System.Drawing.Color.Transparent
        Me.BtnOk.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.BtnOk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnOk.Location = New System.Drawing.Point(107, 87)
        Me.BtnOk.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.BtnOk.Name = "BtnOk"
        Me.BtnOk.Size = New System.Drawing.Size(120, 36)
        Me.BtnOk.TabIndex = 233
        Me.BtnOk.Text = "O&K"
        Me.ToolTip1.SetToolTip(Me.BtnOk, "Ok : Alt + K")
        Me.BtnOk.UseVisualStyleBackColor = False
        '
        'BtnCancel
        '
        Me.BtnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.BtnCancel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.BtnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnCancel.Location = New System.Drawing.Point(236, 87)
        Me.BtnCancel.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(120, 36)
        Me.BtnCancel.TabIndex = 230
        Me.BtnCancel.Text = "&Cancel"
        Me.BtnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.BtnCancel, "Cancel : Alt + C")
        Me.BtnCancel.UseVisualStyleBackColor = False
        '
        'lblAddGroup
        '
        Me.lblAddGroup.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblAddGroup.AutoSize = True
        Me.lblAddGroup.Location = New System.Drawing.Point(4, 26)
        Me.lblAddGroup.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblAddGroup.Name = "lblAddGroup"
        Me.lblAddGroup.Size = New System.Drawing.Size(107, 25)
        Me.lblAddGroup.TabIndex = 1
        Me.lblAddGroup.Text = "Add Group"
        '
        'txtGrpName
        '
        Me.txtGrpName.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtGrpName.Location = New System.Drawing.Point(128, 22)
        Me.txtGrpName.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtGrpName.Name = "txtGrpName"
        Me.txtGrpName.Size = New System.Drawing.Size(228, 30)
        Me.txtGrpName.TabIndex = 0
        Me.txtGrpName.Visible = False
        '
        'frame1
        '
        Me.frame1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.frame1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.frame1.Controls.Add(Me.MnuType)
        Me.frame1.Controls.Add(Me.PnlArrow)
        Me.frame1.Controls.Add(Me.BtnLoad)
        Me.frame1.Controls.Add(Me.FraGrpName)
        Me.frame1.Controls.Add(Me.cboType)
        Me.frame1.Controls.Add(Me.vsfDET)
        Me.frame1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.frame1.Location = New System.Drawing.Point(9, 15)
        Me.frame1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.frame1.Name = "frame1"
        Me.frame1.Size = New System.Drawing.Size(699, 469)
        Me.frame1.TabIndex = 233
        '
        'MnuType
        '
        Me.MnuType.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MnuType.AutoSize = True
        Me.MnuType.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MnuType.ForeColor = System.Drawing.Color.Red
        Me.MnuType.Location = New System.Drawing.Point(60, 14)
        Me.MnuType.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.MnuType.Name = "MnuType"
        Me.MnuType.Size = New System.Drawing.Size(112, 25)
        Me.MnuType.TabIndex = 251
        Me.MnuType.Tag = "M"
        Me.MnuType.Text = "Menu Type"
        '
        'PnlArrow
        '
        Me.PnlArrow.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PnlArrow.Controls.Add(Me.btnMvUP)
        Me.PnlArrow.Controls.Add(Me.btnMvDwn)
        Me.PnlArrow.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PnlArrow.Location = New System.Drawing.Point(-1, 49)
        Me.PnlArrow.Margin = New System.Windows.Forms.Padding(5, 6, 5, 6)
        Me.PnlArrow.Name = "PnlArrow"
        Me.PnlArrow.Size = New System.Drawing.Size(60, 416)
        Me.PnlArrow.TabIndex = 249
        '
        'btnMvUP
        '
        Me.btnMvUP.ForeColor = System.Drawing.Color.Black
        Me.btnMvUP.Image = CType(resources.GetObject("btnMvUP.Image"), System.Drawing.Image)
        Me.btnMvUP.Location = New System.Drawing.Point(5, 2)
        Me.btnMvUP.Margin = New System.Windows.Forms.Padding(5, 6, 5, 6)
        Me.btnMvUP.Name = "btnMvUP"
        Me.btnMvUP.Size = New System.Drawing.Size(52, 208)
        Me.btnMvUP.TabIndex = 9
        Me.btnMvUP.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnMvUP.UseVisualStyleBackColor = False
        '
        'btnMvDwn
        '
        Me.btnMvDwn.ForeColor = System.Drawing.Color.Black
        Me.btnMvDwn.Image = CType(resources.GetObject("btnMvDwn.Image"), System.Drawing.Image)
        Me.btnMvDwn.Location = New System.Drawing.Point(5, 209)
        Me.btnMvDwn.Margin = New System.Windows.Forms.Padding(5, 6, 5, 6)
        Me.btnMvDwn.Name = "btnMvDwn"
        Me.btnMvDwn.Size = New System.Drawing.Size(52, 201)
        Me.btnMvDwn.TabIndex = 10
        Me.btnMvDwn.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnMvDwn.UseVisualStyleBackColor = False
        '
        'Frame4
        '
        Me.Frame4.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Frame4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Frame4.Controls.Add(Me.frame1)
        Me.Frame4.Controls.Add(Me.fraFooter)
        Me.Frame4.Controls.Add(Me.FraImg)
        Me.Frame4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Frame4.Location = New System.Drawing.Point(4, 15)
        Me.Frame4.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Frame4.Name = "Frame4"
        Me.Frame4.Size = New System.Drawing.Size(1123, 565)
        Me.Frame4.TabIndex = 233
        '
        'OpenFileDialogPic
        '
        Me.OpenFileDialogPic.FileName = "OpenFileDialog1"
        '
        'frmUtlMenuSequence
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1149, 575)
        Me.Controls.Add(Me.Frame4)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.MinimumSize = New System.Drawing.Size(1031, 611)
        Me.Name = "frmUtlMenuSequence"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Menu Sequence"
        CType(Me.FileSystemWatcher1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfDET, System.ComponentModel.ISupportInitialize).EndInit()
        Me.FraImg.ResumeLayout(False)
        Me.FraImg.PerformLayout()
        Me.pnlSCKeys.ResumeLayout(False)
        Me.pnlSCKeys.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GrpImg.ResumeLayout(False)
        CType(Me.PicBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraFooter.ResumeLayout(False)
        Me.FraGrpName.ResumeLayout(False)
        Me.FraGrpName.PerformLayout()
        Me.frame1.ResumeLayout(False)
        Me.frame1.PerformLayout()
        Me.PnlArrow.ResumeLayout(False)
        Me.Frame4.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cboType As ComboBox
    Friend WithEvents BtnLoad As Button
    Friend WithEvents FileSystemWatcher1 As IO.FileSystemWatcher
    Friend WithEvents fraFooter As Panel
    Friend WithEvents FraImg As Panel
    Friend WithEvents vsfDET As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents BtnUpload As Button
    Friend WithEvents PicBox As PictureBox
    Friend WithEvents FraGrpName As Panel
    Friend WithEvents lblAddGroup As Label
    Friend WithEvents txtGrpName As TextBox
    Friend WithEvents frame1 As Panel
    Friend WithEvents Frame4 As Panel
    Friend WithEvents OpenFileDialogPic As OpenFileDialog
    Friend WithEvents GrpImg As GroupBox
    Friend WithEvents lblHelp As Label
    Friend WithEvents btnClose As Button
    Friend WithEvents btnSave As Button
    Friend WithEvents BtnCancel As Button
    Friend WithEvents btnAddGroup As Button
    Friend WithEvents BtnAddSeprator As Button
    Friend WithEvents BtnOk As Button
    Friend WithEvents pnlSCKeys As Panel
    Friend WithEvents btnApply As Button
    Friend WithEvents cboKey As ComboBox
    Friend WithEvents lblKey As Label
    Friend WithEvents chkShift As CheckBox
    Friend WithEvents chkAlt As CheckBox
    Friend WithEvents chkCtrl As CheckBox
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents chkSelKey As CheckBox
    Friend WithEvents btnClear As Button
    Friend WithEvents PnlArrow As Panel
    Friend WithEvents btnMvUP As Button
    Friend WithEvents btnMvDwn As Button
    Friend WithEvents BtnRemove As Button
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents MnuType As Label
    Friend WithEvents cboGrpName As ComboBox
    Friend WithEvents btnDelete As Button
End Class
