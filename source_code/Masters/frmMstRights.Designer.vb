<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMstRights
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMstRights))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnTranspose = New C1.Win.C1Input.C1Button()
        Me.txtCode = New System.Windows.Forms.TextBox()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.btnCls = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnSave = New C1.Win.C1Input.C1Button()
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.btnPnlClose = New C1.Win.C1Input.C1Button()
        Me.btnPnlSave = New C1.Win.C1Input.C1Button()
        Me.btnCopyFrom = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.chkTAll = New System.Windows.Forms.CheckBox()
        Me.chkTPosInv = New System.Windows.Forms.CheckBox()
        Me.chkTMain = New System.Windows.Forms.CheckBox()
        Me.chkTPosPayRoll = New System.Windows.Forms.CheckBox()
        Me.chkTRestaurant = New System.Windows.Forms.CheckBox()
        Me.chkTPosFA = New System.Windows.Forms.CheckBox()
        Me.vsfMenu = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.optTMasters = New System.Windows.Forms.RadioButton()
        Me.optTTransactions = New System.Windows.Forms.RadioButton()
        Me.optTReports = New System.Windows.Forms.RadioButton()
        Me.optTAll = New System.Windows.Forms.RadioButton()
        Me.lblMnuCaption = New System.Windows.Forms.Label()
        Me.optTVouchers = New System.Windows.Forms.RadioButton()
        Me.pnlTranspose = New System.Windows.Forms.Panel()
        Me.optTUtilities = New System.Windows.Forms.RadioButton()
        Me.vsfUserDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.txtUserID = New System.Windows.Forms.TextBox()
        Me.fraCopyFrom = New System.Windows.Forms.GroupBox()
        Me.cboUserCopy = New System.Windows.Forms.ComboBox()
        Me.lblName = New System.Windows.Forms.Label()
        Me.lblCode = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblFilter = New System.Windows.Forms.Label()
        Me.cboFilterOn = New System.Windows.Forms.ComboBox()
        Me.Panel2.SuspendLayout()
        CType(Me.vsfMenu, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlTranspose.SuspendLayout()
        CType(Me.vsfUserDet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraCopyFrom.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnTranspose
        '
        Me.btnTranspose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnTranspose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTranspose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTranspose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnTranspose.Location = New System.Drawing.Point(6, 545)
        Me.btnTranspose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnTranspose.Name = "btnTranspose"
        Me.btnTranspose.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnTranspose.Size = New System.Drawing.Size(99, 29)
        Me.btnTranspose.TabIndex = 4
        Me.btnTranspose.Text = "&Transpose"
        Me.btnTranspose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnTranspose, "Shortcut: Alt + T")
        Me.btnTranspose.UseVisualStyleBackColor = False
        '
        'txtCode
        '
        Me.txtCode.BackColor = System.Drawing.Color.White
        Me.txtCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCode.ForeColor = System.Drawing.Color.Black
        Me.txtCode.Location = New System.Drawing.Point(147, 13)
        Me.txtCode.Margin = New System.Windows.Forms.Padding(5)
        Me.txtCode.MaxLength = 50
        Me.txtCode.Multiline = True
        Me.txtCode.Name = "txtCode"
        Me.txtCode.ReadOnly = True
        Me.txtCode.Size = New System.Drawing.Size(331, 28)
        Me.txtCode.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.txtCode, "Enter Code")
        '
        'txtName
        '
        Me.txtName.BackColor = System.Drawing.Color.White
        Me.txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtName.ForeColor = System.Drawing.Color.Black
        Me.txtName.Location = New System.Drawing.Point(147, 49)
        Me.txtName.Margin = New System.Windows.Forms.Padding(5)
        Me.txtName.MaxLength = 50
        Me.txtName.Multiline = True
        Me.txtName.Name = "txtName"
        Me.txtName.ReadOnly = True
        Me.txtName.Size = New System.Drawing.Size(331, 28)
        Me.txtName.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.txtName, "Enter Name")
        '
        'btnCls
        '
        Me.btnCls.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnCls.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCls.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCls.Location = New System.Drawing.Point(109, 62)
        Me.btnCls.Name = "btnCls"
        Me.btnCls.Size = New System.Drawing.Size(87, 31)
        Me.btnCls.TabIndex = 2
        Me.btnCls.Text = "C&lose"
        Me.btnCls.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCls, "Close:Alt+L")
        Me.btnCls.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOK.Location = New System.Drawing.Point(22, 62)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(81, 31)
        Me.btnOK.TabIndex = 1
        Me.btnOK.Text = "O&K"
        Me.ToolTip1.SetToolTip(Me.btnOK, "OK:Alt+k")
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(824, 542)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(81, 31)
        Me.btnSave.TabIndex = 7
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Save:Alt+S")
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(913, 542)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(81, 31)
        Me.btnClose.TabIndex = 8
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close:Alt+O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnPnlClose
        '
        Me.btnPnlClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPnlClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnPnlClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnPnlClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPnlClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPnlClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPnlClose.Location = New System.Drawing.Point(920, 546)
        Me.btnPnlClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnPnlClose.Name = "btnPnlClose"
        Me.btnPnlClose.Size = New System.Drawing.Size(79, 29)
        Me.btnPnlClose.TabIndex = 202
        Me.btnPnlClose.Text = "Cl&ose"
        Me.btnPnlClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnPnlClose, "Close : Alt + O")
        Me.btnPnlClose.UseVisualStyleBackColor = False
        '
        'btnPnlSave
        '
        Me.btnPnlSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPnlSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnPnlSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnPnlSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPnlSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPnlSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPnlSave.Location = New System.Drawing.Point(818, 548)
        Me.btnPnlSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnPnlSave.Name = "btnPnlSave"
        Me.btnPnlSave.Size = New System.Drawing.Size(84, 29)
        Me.btnPnlSave.TabIndex = 201
        Me.btnPnlSave.Text = "&Save"
        Me.btnPnlSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnPnlSave, "Save : Alt + S")
        Me.btnPnlSave.UseVisualStyleBackColor = False
        '
        'btnCopyFrom
        '
        Me.btnCopyFrom.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCopyFrom.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCopyFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCopyFrom.Image = CType(resources.GetObject("btnCopyFrom.Image"), System.Drawing.Image)
        Me.btnCopyFrom.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCopyFrom.Location = New System.Drawing.Point(107, 545)
        Me.btnCopyFrom.Name = "btnCopyFrom"
        Me.btnCopyFrom.Size = New System.Drawing.Size(122, 28)
        Me.btnCopyFrom.TabIndex = 5
        Me.btnCopyFrom.Text = "&Copy From"
        Me.btnCopyFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCopyFrom, "Copy From:Alt+C")
        Me.btnCopyFrom.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel2.BackColor = System.Drawing.SystemColors.Control
        Me.Panel2.Controls.Add(Me.chkTAll)
        Me.Panel2.Controls.Add(Me.chkTPosInv)
        Me.Panel2.Controls.Add(Me.chkTMain)
        Me.Panel2.Controls.Add(Me.chkTPosPayRoll)
        Me.Panel2.Controls.Add(Me.chkTRestaurant)
        Me.Panel2.Controls.Add(Me.chkTPosFA)
        Me.Panel2.Location = New System.Drawing.Point(478, 380)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(16, 23)
        Me.Panel2.TabIndex = 212
        Me.Panel2.Visible = False
        '
        'chkTAll
        '
        Me.chkTAll.AutoSize = True
        Me.chkTAll.BackColor = System.Drawing.Color.Transparent
        Me.chkTAll.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTAll.Location = New System.Drawing.Point(3, 3)
        Me.chkTAll.Name = "chkTAll"
        Me.chkTAll.Size = New System.Drawing.Size(78, 19)
        Me.chkTAll.TabIndex = 5
        Me.chkTAll.Tag = "R"
        Me.chkTAll.Text = "Select All"
        Me.chkTAll.UseVisualStyleBackColor = False
        '
        'chkTPosInv
        '
        Me.chkTPosInv.AutoSize = True
        Me.chkTPosInv.BackColor = System.Drawing.Color.Transparent
        Me.chkTPosInv.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTPosInv.Location = New System.Drawing.Point(331, 3)
        Me.chkTPosInv.Name = "chkTPosInv"
        Me.chkTPosInv.Size = New System.Drawing.Size(81, 19)
        Me.chkTPosInv.TabIndex = 3
        Me.chkTPosInv.Tag = "I"
        Me.chkTPosInv.Text = "Inventory"
        Me.chkTPosInv.UseVisualStyleBackColor = False
        '
        'chkTMain
        '
        Me.chkTMain.AutoSize = True
        Me.chkTMain.BackColor = System.Drawing.Color.Transparent
        Me.chkTMain.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTMain.Location = New System.Drawing.Point(87, 3)
        Me.chkTMain.Name = "chkTMain"
        Me.chkTMain.Size = New System.Drawing.Size(73, 19)
        Me.chkTMain.TabIndex = 0
        Me.chkTMain.Tag = "M"
        Me.chkTMain.Text = "Modules"
        Me.chkTMain.UseVisualStyleBackColor = False
        '
        'chkTPosPayRoll
        '
        Me.chkTPosPayRoll.AutoSize = True
        Me.chkTPosPayRoll.BackColor = System.Drawing.Color.Transparent
        Me.chkTPosPayRoll.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTPosPayRoll.Location = New System.Drawing.Point(259, 3)
        Me.chkTPosPayRoll.Name = "chkTPosPayRoll"
        Me.chkTPosPayRoll.Size = New System.Drawing.Size(66, 19)
        Me.chkTPosPayRoll.TabIndex = 2
        Me.chkTPosPayRoll.Tag = "P"
        Me.chkTPosPayRoll.Text = "PayRoll"
        Me.chkTPosPayRoll.UseVisualStyleBackColor = False
        '
        'chkTRestaurant
        '
        Me.chkTRestaurant.AutoSize = True
        Me.chkTRestaurant.BackColor = System.Drawing.Color.Transparent
        Me.chkTRestaurant.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTRestaurant.Location = New System.Drawing.Point(166, 3)
        Me.chkTRestaurant.Name = "chkTRestaurant"
        Me.chkTRestaurant.Size = New System.Drawing.Size(87, 19)
        Me.chkTRestaurant.TabIndex = 4
        Me.chkTRestaurant.Tag = "R"
        Me.chkTRestaurant.Text = "Restaurant"
        Me.chkTRestaurant.UseVisualStyleBackColor = False
        '
        'chkTPosFA
        '
        Me.chkTPosFA.AutoSize = True
        Me.chkTPosFA.BackColor = System.Drawing.Color.Transparent
        Me.chkTPosFA.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTPosFA.Location = New System.Drawing.Point(418, 3)
        Me.chkTPosFA.Name = "chkTPosFA"
        Me.chkTPosFA.Size = New System.Drawing.Size(94, 19)
        Me.chkTPosFA.TabIndex = 1
        Me.chkTPosFA.Tag = "A"
        Me.chkTPosFA.Text = "Fixed Assets"
        Me.chkTPosFA.UseVisualStyleBackColor = False
        '
        'vsfMenu
        '
        Me.vsfMenu.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfMenu.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfMenu.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.vsfMenu.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfMenu.ColumnInfo = "2,0,0,0,0,110,Columns:"
        Me.vsfMenu.ExtendLastCol = True
        Me.vsfMenu.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.vsfMenu.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfMenu.Location = New System.Drawing.Point(6, 5)
        Me.vsfMenu.Name = "vsfMenu"
        Me.vsfMenu.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.vsfMenu.Rows.Count = 4
        Me.vsfMenu.Rows.DefaultSize = 22
        Me.vsfMenu.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfMenu.Size = New System.Drawing.Size(409, 536)
        Me.vsfMenu.StyleInfo = resources.GetString("vsfMenu.StyleInfo")
        Me.vsfMenu.TabIndex = 194
        Me.vsfMenu.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2010Silver
        '
        'optTMasters
        '
        Me.optTMasters.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.optTMasters.AutoSize = True
        Me.optTMasters.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optTMasters.Location = New System.Drawing.Point(22, 547)
        Me.optTMasters.Name = "optTMasters"
        Me.optTMasters.Size = New System.Drawing.Size(91, 24)
        Me.optTMasters.TabIndex = 206
        Me.optTMasters.Tag = "MASTERS"
        Me.optTMasters.Text = "Masters"
        Me.optTMasters.UseVisualStyleBackColor = True
        '
        'optTTransactions
        '
        Me.optTTransactions.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.optTTransactions.AutoSize = True
        Me.optTTransactions.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optTTransactions.Location = New System.Drawing.Point(113, 547)
        Me.optTTransactions.Name = "optTTransactions"
        Me.optTTransactions.Size = New System.Drawing.Size(130, 24)
        Me.optTTransactions.TabIndex = 207
        Me.optTTransactions.Tag = "TRANSACTIONS"
        Me.optTTransactions.Text = "Transactions"
        Me.optTTransactions.UseVisualStyleBackColor = True
        '
        'optTReports
        '
        Me.optTReports.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.optTReports.AutoSize = True
        Me.optTReports.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optTReports.Location = New System.Drawing.Point(243, 547)
        Me.optTReports.Name = "optTReports"
        Me.optTReports.Size = New System.Drawing.Size(91, 24)
        Me.optTReports.TabIndex = 208
        Me.optTReports.Tag = "REPORTS"
        Me.optTReports.Text = "Reports"
        Me.optTReports.UseVisualStyleBackColor = True
        '
        'optTAll
        '
        Me.optTAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.optTAll.AutoSize = True
        Me.optTAll.Checked = True
        Me.optTAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optTAll.Location = New System.Drawing.Point(432, 547)
        Me.optTAll.Name = "optTAll"
        Me.optTAll.Size = New System.Drawing.Size(47, 24)
        Me.optTAll.TabIndex = 211
        Me.optTAll.TabStop = True
        Me.optTAll.Tag = "ALL"
        Me.optTAll.Text = "All"
        Me.optTAll.UseVisualStyleBackColor = True
        '
        'lblMnuCaption
        '
        Me.lblMnuCaption.AutoSize = True
        Me.lblMnuCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblMnuCaption.Location = New System.Drawing.Point(418, 0)
        Me.lblMnuCaption.Name = "lblMnuCaption"
        Me.lblMnuCaption.Size = New System.Drawing.Size(0, 17)
        Me.lblMnuCaption.TabIndex = 213
        '
        'optTVouchers
        '
        Me.optTVouchers.AutoSize = True
        Me.optTVouchers.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optTVouchers.Location = New System.Drawing.Point(547, 462)
        Me.optTVouchers.Name = "optTVouchers"
        Me.optTVouchers.Size = New System.Drawing.Size(103, 24)
        Me.optTVouchers.TabIndex = 212
        Me.optTVouchers.Tag = "VOUCHERS"
        Me.optTVouchers.Text = "Vouchers"
        Me.optTVouchers.UseVisualStyleBackColor = True
        Me.optTVouchers.Visible = False
        '
        'pnlTranspose
        '
        Me.pnlTranspose.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlTranspose.Controls.Add(Me.optTVouchers)
        Me.pnlTranspose.Controls.Add(Me.lblMnuCaption)
        Me.pnlTranspose.Controls.Add(Me.optTAll)
        Me.pnlTranspose.Controls.Add(Me.optTUtilities)
        Me.pnlTranspose.Controls.Add(Me.optTReports)
        Me.pnlTranspose.Controls.Add(Me.optTTransactions)
        Me.pnlTranspose.Controls.Add(Me.optTMasters)
        Me.pnlTranspose.Controls.Add(Me.btnPnlClose)
        Me.pnlTranspose.Controls.Add(Me.btnPnlSave)
        Me.pnlTranspose.Controls.Add(Me.vsfUserDet)
        Me.pnlTranspose.Controls.Add(Me.vsfMenu)
        Me.pnlTranspose.Controls.Add(Me.Panel2)
        Me.pnlTranspose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlTranspose.Location = New System.Drawing.Point(16, 12)
        Me.pnlTranspose.Name = "pnlTranspose"
        Me.pnlTranspose.Size = New System.Drawing.Size(1003, 579)
        Me.pnlTranspose.TabIndex = 194
        Me.pnlTranspose.Visible = False
        '
        'optTUtilities
        '
        Me.optTUtilities.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.optTUtilities.AutoSize = True
        Me.optTUtilities.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optTUtilities.Location = New System.Drawing.Point(340, 548)
        Me.optTUtilities.Name = "optTUtilities"
        Me.optTUtilities.Size = New System.Drawing.Size(87, 24)
        Me.optTUtilities.TabIndex = 209
        Me.optTUtilities.Tag = "UTILITIES"
        Me.optTUtilities.Text = "Utilities"
        Me.optTUtilities.UseVisualStyleBackColor = True
        '
        'vsfUserDet
        '
        Me.vsfUserDet.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfUserDet.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfUserDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfUserDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfUserDet.ColumnInfo = "2,1,0,0,0,110,Columns:"
        Me.vsfUserDet.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.vsfUserDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfUserDet.Location = New System.Drawing.Point(418, 26)
        Me.vsfUserDet.Name = "vsfUserDet"
        Me.vsfUserDet.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.vsfUserDet.Rows.Count = 4
        Me.vsfUserDet.Rows.DefaultSize = 22
        Me.vsfUserDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfUserDet.Size = New System.Drawing.Size(581, 513)
        Me.vsfUserDet.StyleInfo = resources.GetString("vsfUserDet.StyleInfo")
        Me.vsfUserDet.TabIndex = 195
        Me.vsfUserDet.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2010Silver
        '
        'vsfDet
        '
        Me.vsfDet.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "2,0,0,0,0,110,Columns:"
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfDet.Location = New System.Drawing.Point(9, 122)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.vsfDet.Rows.Count = 4
        Me.vsfDet.Rows.DefaultSize = 22
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Column
        Me.vsfDet.Size = New System.Drawing.Size(985, 404)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 3
        Me.vsfDet.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2010Silver
        '
        'txtUserID
        '
        Me.txtUserID.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.txtUserID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtUserID.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUserID.Location = New System.Drawing.Point(233, 546)
        Me.txtUserID.Name = "txtUserID"
        Me.txtUserID.Size = New System.Drawing.Size(62, 26)
        Me.txtUserID.TabIndex = 6
        Me.txtUserID.Visible = False
        '
        'fraCopyFrom
        '
        Me.fraCopyFrom.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.fraCopyFrom.Controls.Add(Me.btnCls)
        Me.fraCopyFrom.Controls.Add(Me.btnOK)
        Me.fraCopyFrom.Controls.Add(Me.cboUserCopy)
        Me.fraCopyFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraCopyFrom.Location = New System.Drawing.Point(179, 197)
        Me.fraCopyFrom.Name = "fraCopyFrom"
        Me.fraCopyFrom.Size = New System.Drawing.Size(220, 117)
        Me.fraCopyFrom.TabIndex = 213
        Me.fraCopyFrom.TabStop = False
        Me.fraCopyFrom.Text = "Copy From"
        Me.fraCopyFrom.Visible = False
        '
        'cboUserCopy
        '
        Me.cboUserCopy.FormattingEnabled = True
        Me.cboUserCopy.Location = New System.Drawing.Point(22, 22)
        Me.cboUserCopy.Name = "cboUserCopy"
        Me.cboUserCopy.Size = New System.Drawing.Size(174, 28)
        Me.cboUserCopy.TabIndex = 0
        '
        'lblName
        '
        Me.lblName.BackColor = System.Drawing.Color.Transparent
        Me.lblName.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.lblName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblName.ForeColor = System.Drawing.Color.Blue
        Me.lblName.Location = New System.Drawing.Point(10, 53)
        Me.lblName.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(127, 21)
        Me.lblName.TabIndex = 216
        Me.lblName.Tag = "M"
        Me.lblName.Text = "Name"
        '
        'lblCode
        '
        Me.lblCode.BackColor = System.Drawing.Color.Transparent
        Me.lblCode.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.lblCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCode.ForeColor = System.Drawing.Color.Blue
        Me.lblCode.Location = New System.Drawing.Point(10, 17)
        Me.lblCode.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lblCode.Name = "lblCode"
        Me.lblCode.Size = New System.Drawing.Size(127, 21)
        Me.lblCode.TabIndex = 215
        Me.lblCode.Tag = "M"
        Me.lblCode.Text = "Code"
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.lblFilter)
        Me.Panel1.Controls.Add(Me.cboFilterOn)
        Me.Panel1.Controls.Add(Me.btnSave)
        Me.Panel1.Controls.Add(Me.btnClose)
        Me.Panel1.Controls.Add(Me.txtName)
        Me.Panel1.Controls.Add(Me.txtCode)
        Me.Panel1.Controls.Add(Me.lblCode)
        Me.Panel1.Controls.Add(Me.lblName)
        Me.Panel1.Controls.Add(Me.btnCopyFrom)
        Me.Panel1.Controls.Add(Me.fraCopyFrom)
        Me.Panel1.Controls.Add(Me.btnTranspose)
        Me.Panel1.Controls.Add(Me.txtUserID)
        Me.Panel1.Controls.Add(Me.vsfDet)
        Me.Panel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel1.Location = New System.Drawing.Point(16, 12)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1004, 579)
        Me.Panel1.TabIndex = 192
        '
        'lblFilter
        '
        Me.lblFilter.BackColor = System.Drawing.Color.Transparent
        Me.lblFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.lblFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFilter.ForeColor = System.Drawing.Color.Black
        Me.lblFilter.Location = New System.Drawing.Point(10, 89)
        Me.lblFilter.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lblFilter.Name = "lblFilter"
        Me.lblFilter.Size = New System.Drawing.Size(77, 21)
        Me.lblFilter.TabIndex = 225
        Me.lblFilter.Tag = ""
        Me.lblFilter.Text = "Filter On"
        '
        'cboFilterOn
        '
        Me.cboFilterOn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFilterOn.FormattingEnabled = True
        Me.cboFilterOn.Items.AddRange(New Object() {"Masters", "Transactions", "Vouchers", "Reports", "Tools", "Utilities", "All"})
        Me.cboFilterOn.Location = New System.Drawing.Point(147, 86)
        Me.cboFilterOn.MaxDropDownItems = 7
        Me.cboFilterOn.Name = "cboFilterOn"
        Me.cboFilterOn.Size = New System.Drawing.Size(331, 28)
        Me.cboFilterOn.TabIndex = 2
        '
        'frmMstRights
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1032, 601)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.pnlTranspose)
        Me.Location = New System.Drawing.Point(100, 4)
        Me.MinimumSize = New System.Drawing.Size(1048, 554)
        Me.Name = "frmMstRights"
        Me.ShowIcon = False
        Me.Text = "User Master"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.vsfMenu, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlTranspose.ResumeLayout(False)
        Me.pnlTranspose.PerformLayout()
        CType(Me.vsfUserDet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraCopyFrom.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents Panel2 As Panel
    Friend WithEvents chkTAll As CheckBox
    Friend WithEvents chkTPosInv As CheckBox
    Friend WithEvents chkTMain As CheckBox
    Friend WithEvents chkTPosPayRoll As CheckBox
    Friend WithEvents chkTRestaurant As CheckBox
    Friend WithEvents chkTPosFA As CheckBox
    Friend WithEvents vsfMenu As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnPnlSave As C1.Win.C1Input.C1Button
    Friend WithEvents btnPnlClose As C1.Win.C1Input.C1Button
    Friend WithEvents optTMasters As RadioButton
    Friend WithEvents optTTransactions As RadioButton
    Friend WithEvents optTReports As RadioButton
    Friend WithEvents optTAll As RadioButton
    Friend WithEvents lblMnuCaption As Label
    Friend WithEvents optTVouchers As RadioButton
    Friend WithEvents pnlTranspose As Panel
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents vsfUserDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents txtUserID As TextBox
    Friend WithEvents btnTranspose As C1.Win.C1Input.C1Button
    Friend WithEvents fraCopyFrom As GroupBox
    Friend WithEvents btnCls As Button
    Friend WithEvents btnOK As Button
    Friend WithEvents cboUserCopy As ComboBox
    Friend WithEvents btnCopyFrom As Button
    Friend WithEvents lblName As Label
    Friend WithEvents lblCode As Label
    Friend WithEvents txtCode As TextBox
    Friend WithEvents txtName As TextBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents btnSave As C1.Win.C1Input.C1Button
    Friend WithEvents cboFilterOn As ComboBox
    Friend WithEvents lblFilter As Label
    Friend WithEvents optTUtilities As RadioButton
End Class
