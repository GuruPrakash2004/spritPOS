<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmTrnPosInv
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTrnPosInv))
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfOptions = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.txtDocID = New System.Windows.Forms.TextBox()
        Me.cntxt = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveThisToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.chkSMS = New System.Windows.Forms.CheckBox()
        Me.btnAddParty = New System.Windows.Forms.Button()
        Me.btnHold = New System.Windows.Forms.Button()
        Me.btnShowPts = New System.Windows.Forms.Button()
        Me.cdlg = New System.Windows.Forms.OpenFileDialog()
        Me.lblFrm = New System.Windows.Forms.Label()
        Me.txtDummy = New System.Windows.Forms.TextBox()
        Me.txtDummy2 = New System.Windows.Forms.TextBox()
        Me.txtCount = New System.Windows.Forms.TextBox()
        Me.txtSalesPerson = New System.Windows.Forms.TextBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Watermark1 = New vLQMS.Watermark()
        Me.txtHdrInvVal = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.lblItemName = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtScan = New System.Windows.Forms.TextBox()
        Me.txtQty = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.btnAddItem = New System.Windows.Forms.Button()
        Me.txtParty = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtPrintNo = New System.Windows.Forms.TextBox()
        Me.dtDocDate = New System.Windows.Forms.DateTimePicker()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.FlowLayoutPanel2 = New System.Windows.Forms.FlowLayoutPanel()
        Me.lblLmtBeer = New System.Windows.Forms.Label()
        Me.lblLmtWine = New System.Windows.Forms.Label()
        Me.lblLmtIML = New System.Windows.Forms.Label()
        Me.lblCancelled = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtFinalValue = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtTotalQty = New System.Windows.Forms.TextBox()
        Me.txtNoOfItems = New System.Windows.Forms.TextBox()
        Me.vsfMenu = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfTot = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnOptions = New C1.Win.C1Input.C1SplitButton()
        Me.DropDownItem4 = New C1.Win.C1Input.DropDownItem()
        Me.DropDownItem5 = New C1.Win.C1Input.DropDownItem()
        Me.btnAlias = New C1.Win.C1Input.DropDownItem()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnRePrint = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnLoadTemp = New C1.Win.C1Input.C1SplitButton()
        Me.btnClearItems = New System.Windows.Forms.Button()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.txtNo = New System.Windows.Forms.TextBox()
        Me.txtDocNo = New System.Windows.Forms.TextBox()
        Me.cboPfx = New System.Windows.Forms.TextBox()
        Me.lblPrevBill = New System.Windows.Forms.Label()
        Me.btnQuickSel = New C1.Win.C1Input.C1SplitButton()
        Me.DropDownItem1 = New C1.Win.C1Input.DropDownItem()
        Me.DropDownItem2 = New C1.Win.C1Input.DropDownItem()
        Me.DropDownItem3 = New C1.Win.C1Input.DropDownItem()
        Me.txtRcvdAmt = New System.Windows.Forms.TextBox()
        Me.lblCollAmt = New System.Windows.Forms.Label()
        Me.txtPayback = New System.Windows.Forms.TextBox()
        Me.vsfCust = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.scMain = New System.Windows.Forms.SplitContainer()
        Me.scTot = New System.Windows.Forms.SplitContainer()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.btnCurr = New System.Windows.Forms.Button()
        Me.txtNoteValQry = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.imgSoftvent = New System.Windows.Forms.PictureBox()
        Me.txtPoints = New System.Windows.Forms.TextBox()
        Me.lblPoints = New System.Windows.Forms.Label()
        Me.vsfTemp = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfSplit = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.btnTopTen = New System.Windows.Forms.Button()
        Me.btn10 = New System.Windows.Forms.Button()
        Me.btn9 = New System.Windows.Forms.Button()
        Me.btn8 = New System.Windows.Forms.Button()
        Me.btn7 = New System.Windows.Forms.Button()
        Me.btn1 = New System.Windows.Forms.Button()
        Me.btn6 = New System.Windows.Forms.Button()
        Me.btn5 = New System.Windows.Forms.Button()
        Me.btn4 = New System.Windows.Forms.Button()
        Me.btn3 = New System.Windows.Forms.Button()
        Me.btn2 = New System.Windows.Forms.Button()
        Me.btnleftArr = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.imgSalPer = New System.Windows.Forms.PictureBox()
        Me.lblSalPer = New System.Windows.Forms.Label()
        Me.Panel7 = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.lblCounter = New System.Windows.Forms.Label()
        Me.imgCounName = New System.Windows.Forms.PictureBox()
        Me.Label7 = New System.Windows.Forms.Label()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfOptions, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cntxt.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.FlowLayoutPanel2.SuspendLayout()
        CType(Me.vsfMenu, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfTot, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfCust, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scMain.Panel1.SuspendLayout()
        Me.scMain.Panel2.SuspendLayout()
        Me.scMain.SuspendLayout()
        CType(Me.scTot, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scTot.Panel1.SuspendLayout()
        Me.scTot.Panel2.SuspendLayout()
        Me.scTot.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.imgSoftvent, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfTemp, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfSplit, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel4.SuspendLayout()
        Me.Panel5.SuspendLayout()
        CType(Me.imgSalPer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel7.SuspendLayout()
        CType(Me.imgCounName, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'vsfDet
        '
        Me.vsfDet.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.BackColor = System.Drawing.Color.MidnightBlue
        Me.vsfDet.ColumnInfo = "2,1,0,0,0,145,Columns:"
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 10.2!)
        Me.vsfDet.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(0, 0)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 25
        Me.vsfDet.Rows.MinSize = 30
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(772, 496)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 8
        Me.vsfDet.Tag = "NOSTYLE"
        '
        'vsfOptions
        '
        Me.vsfOptions.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfOptions.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfOptions.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfOptions.ColumnInfo = "2,1,0,0,0,120,Columns:"
        Me.vsfOptions.ExtendLastCol = True
        Me.vsfOptions.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfOptions.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfOptions.Location = New System.Drawing.Point(117, 691)
        Me.vsfOptions.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfOptions.Name = "vsfOptions"
        Me.vsfOptions.Rows.DefaultSize = 24
        Me.vsfOptions.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfOptions.Size = New System.Drawing.Size(1100, 54)
        Me.vsfOptions.StyleInfo = resources.GetString("vsfOptions.StyleInfo")
        Me.vsfOptions.TabIndex = 11
        Me.vsfOptions.Tag = ""
        Me.vsfOptions.Visible = False
        '
        'txtDocID
        '
        Me.txtDocID.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtDocID.Location = New System.Drawing.Point(979, 535)
        Me.txtDocID.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtDocID.Name = "txtDocID"
        Me.txtDocID.Size = New System.Drawing.Size(40, 25)
        Me.txtDocID.TabIndex = 3
        Me.txtDocID.Visible = False
        '
        'cntxt
        '
        Me.cntxt.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.cntxt.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveThisToolStripMenuItem})
        Me.cntxt.Name = "cntxt"
        Me.cntxt.Size = New System.Drawing.Size(121, 26)
        '
        'SaveThisToolStripMenuItem
        '
        Me.SaveThisToolStripMenuItem.Name = "SaveThisToolStripMenuItem"
        Me.SaveThisToolStripMenuItem.Size = New System.Drawing.Size(120, 22)
        Me.SaveThisToolStripMenuItem.Text = "Save this"
        '
        'lblTitle
        '
        Me.lblTitle.BackColor = System.Drawing.Color.MidnightBlue
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.ForeColor = System.Drawing.Color.White
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1370, 53)
        Me.lblTitle.TabIndex = 22
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Timer1
        '
        '
        'chkSMS
        '
        Me.chkSMS.AutoSize = True
        Me.chkSMS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.chkSMS.ForeColor = System.Drawing.Color.White
        Me.chkSMS.Location = New System.Drawing.Point(320, 1)
        Me.chkSMS.Name = "chkSMS"
        Me.chkSMS.Size = New System.Drawing.Size(86, 21)
        Me.chkSMS.TabIndex = 4
        Me.chkSMS.Text = "Send SMS"
        Me.ToolTip1.SetToolTip(Me.chkSMS, "Right Click Will Open SMS Settings")
        Me.chkSMS.UseVisualStyleBackColor = True
        Me.chkSMS.Visible = False
        '
        'btnAddParty
        '
        Me.btnAddParty.BackgroundImage = Global.vLQMS.My.Resources.Resources.add
        Me.btnAddParty.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnAddParty.Location = New System.Drawing.Point(350, 34)
        Me.btnAddParty.Name = "btnAddParty"
        Me.btnAddParty.Size = New System.Drawing.Size(38, 28)
        Me.btnAddParty.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.btnAddParty, "Add a New Set for Quick Selection")
        Me.btnAddParty.UseVisualStyleBackColor = True
        Me.btnAddParty.Visible = False
        '
        'btnHold
        '
        Me.btnHold.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnHold.BackgroundImage = CType(resources.GetObject("btnHold.BackgroundImage"), System.Drawing.Image)
        Me.btnHold.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnHold.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnHold.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnHold.Location = New System.Drawing.Point(-304, 171)
        Me.btnHold.Margin = New System.Windows.Forms.Padding(4)
        Me.btnHold.Name = "btnHold"
        Me.btnHold.Size = New System.Drawing.Size(184, 38)
        Me.btnHold.TabIndex = 4
        Me.btnHold.Text = "F5 - Hold/New-Bill"
        Me.btnHold.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnHold, "Open New Tab for Fresh Billing")
        Me.btnHold.UseVisualStyleBackColor = True
        Me.btnHold.Visible = False
        '
        'btnShowPts
        '
        Me.btnShowPts.BackgroundImage = CType(resources.GetObject("btnShowPts.BackgroundImage"), System.Drawing.Image)
        Me.btnShowPts.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnShowPts.FlatAppearance.BorderSize = 0
        Me.btnShowPts.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnShowPts.Location = New System.Drawing.Point(426, 4)
        Me.btnShowPts.Name = "btnShowPts"
        Me.btnShowPts.Size = New System.Drawing.Size(36, 28)
        Me.btnShowPts.TabIndex = 599
        Me.ToolTip1.SetToolTip(Me.btnShowPts, "Show Points")
        Me.btnShowPts.UseVisualStyleBackColor = True
        Me.btnShowPts.Visible = False
        '
        'cdlg
        '
        Me.cdlg.FileName = "OpenFileDialog1"
        '
        'lblFrm
        '
        Me.lblFrm.AutoSize = True
        Me.lblFrm.BackColor = System.Drawing.Color.FromArgb(CType(CType(53, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(106, Byte), Integer))
        Me.lblFrm.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lblFrm.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lblFrm.ForeColor = System.Drawing.Color.White
        Me.lblFrm.Location = New System.Drawing.Point(8, 5)
        Me.lblFrm.Name = "lblFrm"
        Me.lblFrm.Size = New System.Drawing.Size(0, 24)
        Me.lblFrm.TabIndex = 17
        '
        'txtDummy
        '
        Me.txtDummy.Location = New System.Drawing.Point(957, 568)
        Me.txtDummy.Name = "txtDummy"
        Me.txtDummy.Size = New System.Drawing.Size(28, 25)
        Me.txtDummy.TabIndex = 30
        Me.txtDummy.Visible = False
        '
        'txtDummy2
        '
        Me.txtDummy2.Location = New System.Drawing.Point(991, 568)
        Me.txtDummy2.Name = "txtDummy2"
        Me.txtDummy2.Size = New System.Drawing.Size(28, 25)
        Me.txtDummy2.TabIndex = 31
        Me.txtDummy2.Visible = False
        '
        'txtCount
        '
        Me.txtCount.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCount.BackColor = System.Drawing.Color.White
        Me.txtCount.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtCount.Font = New System.Drawing.Font("Verdana", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCount.ForeColor = System.Drawing.Color.Black
        Me.txtCount.Location = New System.Drawing.Point(945, 12)
        Me.txtCount.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtCount.MaxLength = 12
        Me.txtCount.Multiline = True
        Me.txtCount.Name = "txtCount"
        Me.txtCount.ReadOnly = True
        Me.txtCount.Size = New System.Drawing.Size(71, 28)
        Me.txtCount.TabIndex = 8
        Me.txtCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtSalesPerson
        '
        Me.txtSalesPerson.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSalesPerson.BackColor = System.Drawing.Color.LightYellow
        Me.txtSalesPerson.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtSalesPerson.Font = New System.Drawing.Font("Verdana", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSalesPerson.ForeColor = System.Drawing.Color.Black
        Me.txtSalesPerson.Location = New System.Drawing.Point(654, 14)
        Me.txtSalesPerson.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtSalesPerson.MaxLength = 12
        Me.txtSalesPerson.Multiline = True
        Me.txtSalesPerson.Name = "txtSalesPerson"
        Me.txtSalesPerson.Size = New System.Drawing.Size(165, 26)
        Me.txtSalesPerson.TabIndex = 6
        Me.txtSalesPerson.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel1.Controls.Add(Me.Watermark1)
        Me.Panel1.Controls.Add(Me.txtHdrInvVal)
        Me.Panel1.Controls.Add(Me.Label12)
        Me.Panel1.Controls.Add(Me.lblItemName)
        Me.Panel1.Location = New System.Drawing.Point(-1, 125)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1458, 44)
        Me.Panel1.TabIndex = 0
        '
        'Watermark1
        '
        Me.Watermark1.Font = New System.Drawing.Font("Verdana", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Watermark1.Location = New System.Drawing.Point(197, 0)
        Me.Watermark1.Name = "Watermark1"
        Me.Watermark1.Size = New System.Drawing.Size(486, 37)
        Me.Watermark1.TabIndex = 9
        Me.Watermark1.WatermarkColor = System.Drawing.Color.Gray
        Me.Watermark1.WatermarkText = "Scan Bar Code"
        '
        'txtHdrInvVal
        '
        Me.txtHdrInvVal.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtHdrInvVal.BackColor = System.Drawing.Color.MidnightBlue
        Me.txtHdrInvVal.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtHdrInvVal.Font = New System.Drawing.Font("Microsoft Sans Serif", 26.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHdrInvVal.ForeColor = System.Drawing.Color.Lime
        Me.txtHdrInvVal.Location = New System.Drawing.Point(1212, 3)
        Me.txtHdrInvVal.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtHdrInvVal.Name = "txtHdrInvVal"
        Me.txtHdrInvVal.ReadOnly = True
        Me.txtHdrInvVal.Size = New System.Drawing.Size(152, 40)
        Me.txtHdrInvVal.TabIndex = 8
        Me.txtHdrInvVal.Text = "883771"
        '
        'Label12
        '
        Me.Label12.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.Color.White
        Me.Label12.Location = New System.Drawing.Point(1107, 12)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(100, 26)
        Me.Label12.TabIndex = 7
        Me.Label12.Tag = "NOCHANGE"
        Me.Label12.Text = "Inv Val: "
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblItemName
        '
        Me.lblItemName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItemName.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblItemName.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblItemName.Location = New System.Drawing.Point(689, 8)
        Me.lblItemName.Name = "lblItemName"
        Me.lblItemName.Size = New System.Drawing.Size(328, 40)
        Me.lblItemName.TabIndex = 5
        Me.lblItemName.Tag = ""
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.Label6.ForeColor = System.Drawing.Color.White
        Me.Label6.Location = New System.Drawing.Point(81, 65)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(255, 13)
        Me.Label6.TabIndex = 6
        Me.Label6.Tag = ""
        Me.Label6.Text = "[ Scan / Enter Itemcode / Press Down Arrow for list ]"
        '
        'txtScan
        '
        Me.txtScan.BackColor = System.Drawing.Color.FromArgb(CType(CType(207, Byte), Integer), CType(CType(207, Byte), Integer), CType(CType(207, Byte), Integer))
        Me.txtScan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtScan.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtScan.Location = New System.Drawing.Point(6, 70)
        Me.txtScan.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtScan.MaxLength = 1000
        Me.txtScan.Name = "txtScan"
        Me.txtScan.Size = New System.Drawing.Size(310, 35)
        Me.txtScan.TabIndex = 1
        Me.txtScan.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtScan.WordWrap = False
        '
        'txtQty
        '
        Me.txtQty.BackColor = System.Drawing.Color.FromArgb(CType(CType(207, Byte), Integer), CType(CType(207, Byte), Integer), CType(CType(207, Byte), Integer))
        Me.txtQty.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtQty.Location = New System.Drawing.Point(73, 67)
        Me.txtQty.Margin = New System.Windows.Forms.Padding(4)
        Me.txtQty.MaxLength = 10
        Me.txtQty.Name = "txtQty"
        Me.txtQty.Size = New System.Drawing.Size(76, 35)
        Me.txtQty.TabIndex = 2
        Me.txtQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtQty.Visible = False
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.White
        Me.Label11.Location = New System.Drawing.Point(13, 70)
        Me.Label11.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(52, 29)
        Me.Label11.TabIndex = 2
        Me.Label11.Text = "Qty"
        '
        'btnAddItem
        '
        Me.btnAddItem.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAddItem.Location = New System.Drawing.Point(157, 67)
        Me.btnAddItem.Margin = New System.Windows.Forms.Padding(4)
        Me.btnAddItem.Name = "btnAddItem"
        Me.btnAddItem.Size = New System.Drawing.Size(53, 35)
        Me.btnAddItem.TabIndex = 3
        Me.btnAddItem.Text = "OK"
        Me.btnAddItem.UseVisualStyleBackColor = True
        Me.btnAddItem.Visible = False
        '
        'txtParty
        '
        Me.txtParty.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtParty.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtParty.Location = New System.Drawing.Point(96, 40)
        Me.txtParty.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtParty.MaxLength = 12
        Me.txtParty.Name = "txtParty"
        Me.txtParty.Size = New System.Drawing.Size(254, 22)
        Me.txtParty.TabIndex = 2
        Me.txtParty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtParty.Visible = False
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold)
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(23, 42)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(52, 21)
        Me.Label1.TabIndex = 1
        Me.Label1.Tag = "R"
        Me.Label1.Text = "Party"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtPrintNo
        '
        Me.txtPrintNo.BackColor = System.Drawing.Color.White
        Me.txtPrintNo.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtPrintNo.Font = New System.Drawing.Font("Verdana", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPrintNo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.txtPrintNo.Location = New System.Drawing.Point(327, 14)
        Me.txtPrintNo.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtPrintNo.Multiline = True
        Me.txtPrintNo.Name = "txtPrintNo"
        Me.txtPrintNo.ReadOnly = True
        Me.txtPrintNo.Size = New System.Drawing.Size(156, 26)
        Me.txtPrintNo.TabIndex = 20
        Me.txtPrintNo.TabStop = False
        Me.txtPrintNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'dtDocDate
        '
        Me.dtDocDate.CalendarFont = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.dtDocDate.CustomFormat = "dd/MM/yyyy HH:mm:ss tt"
        Me.dtDocDate.Font = New System.Drawing.Font("Verdana", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtDocDate.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.dtDocDate.Location = New System.Drawing.Point(56, 13)
        Me.dtDocDate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtDocDate.Name = "dtDocDate"
        Me.dtDocDate.Size = New System.Drawing.Size(269, 27)
        Me.dtDocDate.TabIndex = 19
        '
        'Panel3
        '
        Me.Panel3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel3.BackColor = System.Drawing.Color.Black
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel3.Controls.Add(Me.FlowLayoutPanel2)
        Me.Panel3.Controls.Add(Me.lblCancelled)
        Me.Panel3.Controls.Add(Me.Label8)
        Me.Panel3.Controls.Add(Me.txtFinalValue)
        Me.Panel3.Controls.Add(Me.Label9)
        Me.Panel3.Controls.Add(Me.Label10)
        Me.Panel3.Controls.Add(Me.txtTotalQty)
        Me.Panel3.Controls.Add(Me.txtNoOfItems)
        Me.Panel3.Location = New System.Drawing.Point(2, 668)
        Me.Panel3.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1451, 37)
        Me.Panel3.TabIndex = 23
        '
        'FlowLayoutPanel2
        '
        Me.FlowLayoutPanel2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.FlowLayoutPanel2.Controls.Add(Me.lblLmtBeer)
        Me.FlowLayoutPanel2.Controls.Add(Me.lblLmtWine)
        Me.FlowLayoutPanel2.Controls.Add(Me.lblLmtIML)
        Me.FlowLayoutPanel2.Location = New System.Drawing.Point(0, 3)
        Me.FlowLayoutPanel2.Name = "FlowLayoutPanel2"
        Me.FlowLayoutPanel2.Size = New System.Drawing.Size(438, 30)
        Me.FlowLayoutPanel2.TabIndex = 7
        '
        'lblLmtBeer
        '
        Me.lblLmtBeer.AutoSize = True
        Me.lblLmtBeer.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLmtBeer.ForeColor = System.Drawing.Color.White
        Me.lblLmtBeer.Location = New System.Drawing.Point(3, 0)
        Me.lblLmtBeer.Name = "lblLmtBeer"
        Me.lblLmtBeer.Size = New System.Drawing.Size(41, 21)
        Me.lblLmtBeer.TabIndex = 0
        Me.lblLmtBeer.Text = "Beer"
        Me.lblLmtBeer.Visible = False
        '
        'lblLmtWine
        '
        Me.lblLmtWine.AutoSize = True
        Me.lblLmtWine.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLmtWine.ForeColor = System.Drawing.Color.White
        Me.lblLmtWine.Location = New System.Drawing.Point(50, 0)
        Me.lblLmtWine.Name = "lblLmtWine"
        Me.lblLmtWine.Size = New System.Drawing.Size(46, 21)
        Me.lblLmtWine.TabIndex = 1
        Me.lblLmtWine.Text = "Wine"
        Me.lblLmtWine.Visible = False
        '
        'lblLmtIML
        '
        Me.lblLmtIML.AutoSize = True
        Me.lblLmtIML.BackColor = System.Drawing.Color.Black
        Me.lblLmtIML.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLmtIML.ForeColor = System.Drawing.Color.White
        Me.lblLmtIML.Location = New System.Drawing.Point(102, 0)
        Me.lblLmtIML.Name = "lblLmtIML"
        Me.lblLmtIML.Size = New System.Drawing.Size(36, 21)
        Me.lblLmtIML.TabIndex = 2
        Me.lblLmtIML.Text = "IML"
        Me.lblLmtIML.Visible = False
        '
        'lblCancelled
        '
        Me.lblCancelled.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblCancelled.AutoSize = True
        Me.lblCancelled.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold)
        Me.lblCancelled.ForeColor = System.Drawing.Color.Red
        Me.lblCancelled.Location = New System.Drawing.Point(481, 4)
        Me.lblCancelled.Name = "lblCancelled"
        Me.lblCancelled.Size = New System.Drawing.Size(147, 25)
        Me.lblCancelled.TabIndex = 1
        Me.lblCancelled.Text = "CANCELLED"
        '
        'Label8
        '
        Me.Label8.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.White
        Me.Label8.Location = New System.Drawing.Point(607, 3)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(105, 26)
        Me.Label8.TabIndex = 0
        Me.Label8.Tag = "NOCHANGE"
        Me.Label8.Text = "# Items :"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtFinalValue
        '
        Me.txtFinalValue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFinalValue.BackColor = System.Drawing.Color.Black
        Me.txtFinalValue.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtFinalValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFinalValue.ForeColor = System.Drawing.Color.Lime
        Me.txtFinalValue.Location = New System.Drawing.Point(1191, 4)
        Me.txtFinalValue.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtFinalValue.Name = "txtFinalValue"
        Me.txtFinalValue.ReadOnly = True
        Me.txtFinalValue.Size = New System.Drawing.Size(162, 25)
        Me.txtFinalValue.TabIndex = 6
        Me.txtFinalValue.Text = "883771"
        '
        'Label9
        '
        Me.Label9.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.White
        Me.Label9.Location = New System.Drawing.Point(1024, 3)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(170, 26)
        Me.Label9.TabIndex = 5
        Me.Label9.Tag = "NOCHANGE"
        Me.Label9.Text = "Invoice Value: "
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label10
        '
        Me.Label10.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.White
        Me.Label10.Location = New System.Drawing.Point(805, 3)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(122, 26)
        Me.Label10.TabIndex = 3
        Me.Label10.Tag = "NOCHANGE"
        Me.Label10.Text = "Total Qty :"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtTotalQty
        '
        Me.txtTotalQty.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTotalQty.BackColor = System.Drawing.Color.Black
        Me.txtTotalQty.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtTotalQty.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTotalQty.ForeColor = System.Drawing.Color.White
        Me.txtTotalQty.Location = New System.Drawing.Point(928, 4)
        Me.txtTotalQty.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtTotalQty.Name = "txtTotalQty"
        Me.txtTotalQty.ReadOnly = True
        Me.txtTotalQty.Size = New System.Drawing.Size(103, 25)
        Me.txtTotalQty.TabIndex = 4
        Me.txtTotalQty.Text = "9812"
        '
        'txtNoOfItems
        '
        Me.txtNoOfItems.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNoOfItems.BackColor = System.Drawing.Color.Black
        Me.txtNoOfItems.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtNoOfItems.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNoOfItems.ForeColor = System.Drawing.Color.White
        Me.txtNoOfItems.Location = New System.Drawing.Point(743, 4)
        Me.txtNoOfItems.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtNoOfItems.Name = "txtNoOfItems"
        Me.txtNoOfItems.ReadOnly = True
        Me.txtNoOfItems.Size = New System.Drawing.Size(81, 25)
        Me.txtNoOfItems.TabIndex = 2
        Me.txtNoOfItems.Text = "99"
        '
        'vsfMenu
        '
        Me.vsfMenu.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromCursor
        Me.vsfMenu.BackColor = System.Drawing.Color.Black
        Me.vsfMenu.ColumnInfo = "2,0,0,0,0,130,Columns:"
        Me.vsfMenu.Dock = System.Windows.Forms.DockStyle.Fill
        Me.vsfMenu.ExtendLastCol = True
        Me.vsfMenu.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.vsfMenu.ForeColor = System.Drawing.Color.White
        Me.vsfMenu.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus
        Me.vsfMenu.Location = New System.Drawing.Point(0, 31)
        Me.vsfMenu.Margin = New System.Windows.Forms.Padding(4)
        Me.vsfMenu.Name = "vsfMenu"
        Me.vsfMenu.Rows.Count = 9
        Me.vsfMenu.Rows.DefaultSize = 26
        Me.vsfMenu.Rows.MinSize = 30
        Me.vsfMenu.Size = New System.Drawing.Size(195, 465)
        Me.vsfMenu.StyleInfo = resources.GetString("vsfMenu.StyleInfo")
        Me.vsfMenu.TabIndex = 12
        Me.vsfMenu.Tag = "NOSTYLE"
        '
        'vsfTot
        '
        Me.vsfTot.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfTot.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromCursor
        Me.vsfTot.BackColor = System.Drawing.Color.MidnightBlue
        Me.vsfTot.ColumnInfo = "2,0,0,0,0,130,Columns:0{Width:208;}" & Global.Microsoft.VisualBasic.ChrW(9) & "1{Style:""Margins:1, 0, 0, 0;TextEffect:Flat;T" &
    "extDirection:Normal;Trimming:None;"";}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfTot.ExtendLastCol = True
        Me.vsfTot.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.vsfTot.ForeColor = System.Drawing.Color.White
        Me.vsfTot.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus
        Me.vsfTot.Location = New System.Drawing.Point(0, 21)
        Me.vsfTot.Margin = New System.Windows.Forms.Padding(4)
        Me.vsfTot.Name = "vsfTot"
        Me.vsfTot.Rows.Count = 4
        Me.vsfTot.Rows.DefaultSize = 26
        Me.vsfTot.Size = New System.Drawing.Size(387, 355)
        Me.vsfTot.StyleInfo = resources.GetString("vsfTot.StyleInfo")
        Me.vsfTot.TabIndex = 9
        Me.vsfTot.Tag = "DSGR"
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.FlowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(0, 704)
        Me.FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(1)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(1370, 45)
        Me.FlowLayoutPanel1.TabIndex = 10
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(-113, 89)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(123, 38)
        Me.btnClose.TabIndex = 587
        Me.btnClose.Text = "F9 - Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = True
        Me.btnClose.Visible = False
        '
        'btnOptions
        '
        Me.btnOptions.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_OPTIONS
        Me.btnOptions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnOptions.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOptions.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOptions.Items.Add(Me.DropDownItem4)
        Me.btnOptions.Items.Add(Me.DropDownItem5)
        Me.btnOptions.Items.Add(Me.btnAlias)
        Me.btnOptions.Location = New System.Drawing.Point(210, 110)
        Me.btnOptions.Margin = New System.Windows.Forms.Padding(4)
        Me.btnOptions.Name = "btnOptions"
        Me.btnOptions.Size = New System.Drawing.Size(117, 38)
        Me.btnOptions.TabIndex = 6
        Me.btnOptions.Text = "Options"
        Me.btnOptions.UseVisualStyleBackColor = True
        Me.btnOptions.Visible = False
        '
        'DropDownItem4
        '
        Me.DropDownItem4.Text = "Re-Print Bill"
        '
        'DropDownItem5
        '
        Me.DropDownItem5.Text = "Customer / Member Dtls"
        '
        'btnAlias
        '
        Me.btnAlias.Text = "Add/Edit Form Alias"
        '
        'btnPrint
        '
        Me.btnPrint.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPrint.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPrint.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrint.Location = New System.Drawing.Point(-154, 203)
        Me.btnPrint.Margin = New System.Windows.Forms.Padding(4)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(148, 37)
        Me.btnPrint.TabIndex = 0
        Me.btnPrint.Text = "F8 - Print Bill"
        Me.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnPrint.UseVisualStyleBackColor = True
        Me.btnPrint.Visible = False
        '
        'btnRePrint
        '
        Me.btnRePrint.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRePrint.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_PRINT
        Me.btnRePrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnRePrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRePrint.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRePrint.Location = New System.Drawing.Point(-270, 137)
        Me.btnRePrint.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRePrint.Name = "btnRePrint"
        Me.btnRePrint.Size = New System.Drawing.Size(149, 38)
        Me.btnRePrint.TabIndex = 1
        Me.btnRePrint.Text = "F2 - Re-Print"
        Me.btnRePrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRePrint.UseVisualStyleBackColor = True
        Me.btnRePrint.Visible = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(-163, 171)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(134, 38)
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "F3 - Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancel.UseVisualStyleBackColor = True
        Me.btnCancel.Visible = False
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDelete.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_DELETE
        Me.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDelete.Location = New System.Drawing.Point(-397, 137)
        Me.btnDelete.Margin = New System.Windows.Forms.Padding(4)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(128, 38)
        Me.btnDelete.TabIndex = 3
        Me.btnDelete.Text = "F4 - Delete"
        Me.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnDelete.UseVisualStyleBackColor = True
        Me.btnDelete.Visible = False
        '
        'btnLoadTemp
        '
        Me.btnLoadTemp.BackgroundImage = CType(resources.GetObject("btnLoadTemp.BackgroundImage"), System.Drawing.Image)
        Me.btnLoadTemp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnLoadTemp.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLoadTemp.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoadTemp.Location = New System.Drawing.Point(6, 202)
        Me.btnLoadTemp.Margin = New System.Windows.Forms.Padding(4)
        Me.btnLoadTemp.Name = "btnLoadTemp"
        Me.btnLoadTemp.Size = New System.Drawing.Size(69, 38)
        Me.btnLoadTemp.TabIndex = 5
        Me.btnLoadTemp.Text = "F6 - Load Template"
        Me.btnLoadTemp.UseVisualStyleBackColor = True
        '
        'btnClearItems
        '
        Me.btnClearItems.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClearItems.BackgroundImage = CType(resources.GetObject("btnClearItems.BackgroundImage"), System.Drawing.Image)
        Me.btnClearItems.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClearItems.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClearItems.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClearItems.Location = New System.Drawing.Point(-352, 202)
        Me.btnClearItems.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClearItems.Name = "btnClearItems"
        Me.btnClearItems.Size = New System.Drawing.Size(189, 38)
        Me.btnClearItems.TabIndex = 582
        Me.btnClearItems.Text = "F7 - Clear Items"
        Me.btnClearItems.UseVisualStyleBackColor = True
        Me.btnClearItems.Visible = False
        '
        'btnSearch
        '
        Me.btnSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSearch.Location = New System.Drawing.Point(-201, 125)
        Me.btnSearch.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(155, 38)
        Me.btnSearch.TabIndex = 583
        Me.btnSearch.Text = "Search - Ctrl+F"
        Me.btnSearch.UseVisualStyleBackColor = True
        Me.btnSearch.Visible = False
        '
        'txtNo
        '
        Me.txtNo.Location = New System.Drawing.Point(426, 138)
        Me.txtNo.Name = "txtNo"
        Me.txtNo.Size = New System.Drawing.Size(28, 25)
        Me.txtNo.TabIndex = 584
        Me.txtNo.Visible = False
        '
        'txtDocNo
        '
        Me.txtDocNo.Location = New System.Drawing.Point(430, 163)
        Me.txtDocNo.Name = "txtDocNo"
        Me.txtDocNo.Size = New System.Drawing.Size(28, 25)
        Me.txtDocNo.TabIndex = 585
        Me.txtDocNo.Visible = False
        '
        'cboPfx
        '
        Me.cboPfx.Location = New System.Drawing.Point(414, 163)
        Me.cboPfx.Name = "cboPfx"
        Me.cboPfx.Size = New System.Drawing.Size(28, 25)
        Me.cboPfx.TabIndex = 586
        Me.cboPfx.Visible = False
        '
        'lblPrevBill
        '
        Me.lblPrevBill.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPrevBill.BackColor = System.Drawing.Color.Yellow
        Me.lblPrevBill.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblPrevBill.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPrevBill.ForeColor = System.Drawing.Color.Black
        Me.lblPrevBill.Location = New System.Drawing.Point(3, 1)
        Me.lblPrevBill.Name = "lblPrevBill"
        Me.lblPrevBill.Size = New System.Drawing.Size(0, 29)
        Me.lblPrevBill.TabIndex = 21
        Me.lblPrevBill.Tag = ""
        Me.lblPrevBill.Text = "Prev Bill. Time:   && Amount:"
        Me.lblPrevBill.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblPrevBill.Visible = False
        '
        'btnQuickSel
        '
        Me.btnQuickSel.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btnQuickSel.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnQuickSel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnQuickSel.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnQuickSel.ForeColor = System.Drawing.Color.White
        Me.btnQuickSel.Items.Add(Me.DropDownItem1)
        Me.btnQuickSel.Items.Add(Me.DropDownItem2)
        Me.btnQuickSel.Items.Add(Me.DropDownItem3)
        Me.btnQuickSel.Location = New System.Drawing.Point(0, 0)
        Me.btnQuickSel.Margin = New System.Windows.Forms.Padding(4)
        Me.btnQuickSel.Name = "btnQuickSel"
        Me.btnQuickSel.Size = New System.Drawing.Size(195, 31)
        Me.btnQuickSel.TabIndex = 13
        Me.btnQuickSel.Text = "Quick Selection"
        Me.btnQuickSel.UseVisualStyleBackColor = False
        '
        'DropDownItem1
        '
        Me.DropDownItem1.Text = "Re-Print Bill"
        '
        'DropDownItem2
        '
        Me.DropDownItem2.Text = "Customer / Member Dtls"
        '
        'DropDownItem3
        '
        Me.DropDownItem3.Text = "Add/Edit Form Alias"
        '
        'txtRcvdAmt
        '
        Me.txtRcvdAmt.Font = New System.Drawing.Font("Segoe UI", 13.5!)
        Me.txtRcvdAmt.Location = New System.Drawing.Point(1, 24)
        Me.txtRcvdAmt.Name = "txtRcvdAmt"
        Me.txtRcvdAmt.Size = New System.Drawing.Size(305, 31)
        Me.txtRcvdAmt.TabIndex = 28
        '
        'lblCollAmt
        '
        Me.lblCollAmt.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCollAmt.ForeColor = System.Drawing.Color.White
        Me.lblCollAmt.Location = New System.Drawing.Point(3, 3)
        Me.lblCollAmt.Name = "lblCollAmt"
        Me.lblCollAmt.Size = New System.Drawing.Size(303, 23)
        Me.lblCollAmt.TabIndex = 27
        Me.lblCollAmt.Tag = "NOCHANGE"
        Me.lblCollAmt.Text = "Collected Amount /Tender Change (F10)"
        Me.lblCollAmt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtPayback
        '
        Me.txtPayback.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.txtPayback.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtPayback.Font = New System.Drawing.Font("Microsoft Sans Serif", 13.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPayback.Location = New System.Drawing.Point(2, 59)
        Me.txtPayback.Multiline = True
        Me.txtPayback.Name = "txtPayback"
        Me.txtPayback.ReadOnly = True
        Me.txtPayback.Size = New System.Drawing.Size(341, 52)
        Me.txtPayback.TabIndex = 29
        Me.txtPayback.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'vsfCust
        '
        Me.vsfCust.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.vsfCust.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromCursor
        Me.vsfCust.BackColor = System.Drawing.Color.FromArgb(CType(CType(85, Byte), Integer), CType(CType(85, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.vsfCust.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfCust.ColumnInfo = "2,0,0,0,0,130,Columns:"
        Me.vsfCust.ExtendLastCol = True
        Me.vsfCust.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.vsfCust.ForeColor = System.Drawing.Color.White
        Me.vsfCust.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus
        Me.vsfCust.Location = New System.Drawing.Point(27, 4)
        Me.vsfCust.Margin = New System.Windows.Forms.Padding(4)
        Me.vsfCust.Name = "vsfCust"
        Me.vsfCust.Rows.Count = 9
        Me.vsfCust.Rows.DefaultSize = 26
        Me.vsfCust.Rows.MinSize = 30
        Me.vsfCust.Size = New System.Drawing.Size(431, 258)
        Me.vsfCust.StyleInfo = resources.GetString("vsfCust.StyleInfo")
        Me.vsfCust.TabIndex = 14
        Me.vsfCust.Tag = "NOSTYLE"
        Me.vsfCust.Visible = False
        '
        'scMain
        '
        Me.scMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scMain.Location = New System.Drawing.Point(4, 170)
        Me.scMain.Name = "scMain"
        '
        'scMain.Panel1
        '
        Me.scMain.Panel1.Controls.Add(Me.vsfMenu)
        Me.scMain.Panel1.Controls.Add(Me.btnQuickSel)
        '
        'scMain.Panel2
        '
        Me.scMain.Panel2.Controls.Add(Me.scTot)
        Me.scMain.Size = New System.Drawing.Size(1840, 496)
        Me.scMain.SplitterDistance = 195
        Me.scMain.TabIndex = 9
        '
        'scTot
        '
        Me.scTot.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.scTot.Location = New System.Drawing.Point(0, 0)
        Me.scTot.Name = "scTot"
        '
        'scTot.Panel1
        '
        Me.scTot.Panel1.Controls.Add(Me.vsfDet)
        Me.scTot.Panel1.Controls.Add(Me.vsfCust)
        '
        'scTot.Panel2
        '
        Me.scTot.Panel2.Controls.Add(Me.Panel2)
        Me.scTot.Panel2.Controls.Add(Me.vsfTot)
        Me.scTot.Size = New System.Drawing.Size(1251, 543)
        Me.scTot.SplitterDistance = 772
        Me.scTot.TabIndex = 0
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel2.Controls.Add(Me.btnCurr)
        Me.Panel2.Controls.Add(Me.lblCollAmt)
        Me.Panel2.Controls.Add(Me.txtRcvdAmt)
        Me.Panel2.Controls.Add(Me.txtPayback)
        Me.Panel2.Controls.Add(Me.txtNoteValQry)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 384)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(475, 159)
        Me.Panel2.TabIndex = 10
        '
        'btnCurr
        '
        Me.btnCurr.BackgroundImage = Global.vLQMS.My.Resources.Resources.INRr
        Me.btnCurr.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnCurr.FlatAppearance.BorderSize = 0
        Me.btnCurr.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnCurr.Location = New System.Drawing.Point(309, 24)
        Me.btnCurr.Name = "btnCurr"
        Me.btnCurr.Size = New System.Drawing.Size(35, 32)
        Me.btnCurr.TabIndex = 31
        Me.btnCurr.UseVisualStyleBackColor = True
        '
        'txtNoteValQry
        '
        Me.txtNoteValQry.Location = New System.Drawing.Point(6, 6)
        Me.txtNoteValQry.Name = "txtNoteValQry"
        Me.txtNoteValQry.Size = New System.Drawing.Size(102, 25)
        Me.txtNoteValQry.TabIndex = 30
        Me.txtNoteValQry.UseSystemPasswordChar = True
        Me.txtNoteValQry.Visible = False
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.BackColor = System.Drawing.Color.MidnightBlue
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(1231, 2)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(134, 50)
        Me.Label4.TabIndex = 596
        Me.Label4.Tag = "R"
        Me.Label4.Text = "9880606365  80-26794645 mail@softvent.com www.softvent.com" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'imgSoftvent
        '
        Me.imgSoftvent.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.imgSoftvent.BackColor = System.Drawing.Color.MidnightBlue
        Me.imgSoftvent.BackgroundImage = CType(resources.GetObject("imgSoftvent.BackgroundImage"), System.Drawing.Image)
        Me.imgSoftvent.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.imgSoftvent.Location = New System.Drawing.Point(1019, 0)
        Me.imgSoftvent.Margin = New System.Windows.Forms.Padding(4)
        Me.imgSoftvent.Name = "imgSoftvent"
        Me.imgSoftvent.Size = New System.Drawing.Size(204, 52)
        Me.imgSoftvent.TabIndex = 594
        Me.imgSoftvent.TabStop = False
        '
        'txtPoints
        '
        Me.txtPoints.BackColor = System.Drawing.Color.Yellow
        Me.txtPoints.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtPoints.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPoints.ForeColor = System.Drawing.Color.Black
        Me.txtPoints.Location = New System.Drawing.Point(394, 39)
        Me.txtPoints.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtPoints.MaxLength = 12
        Me.txtPoints.Name = "txtPoints"
        Me.txtPoints.Size = New System.Drawing.Size(76, 22)
        Me.txtPoints.TabIndex = 598
        Me.txtPoints.TabStop = False
        Me.txtPoints.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtPoints.Visible = False
        '
        'lblPoints
        '
        Me.lblPoints.AutoSize = True
        Me.lblPoints.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPoints.ForeColor = System.Drawing.Color.White
        Me.lblPoints.Location = New System.Drawing.Point(350, 65)
        Me.lblPoints.Name = "lblPoints"
        Me.lblPoints.Size = New System.Drawing.Size(120, 20)
        Me.lblPoints.TabIndex = 597
        Me.lblPoints.Tag = "R"
        Me.lblPoints.Text = "Loyalty Points"
        Me.lblPoints.Visible = False
        '
        'vsfTemp
        '
        Me.vsfTemp.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfTemp.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfTemp.ExtendLastCol = True
        Me.vsfTemp.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfTemp.Location = New System.Drawing.Point(584, 336)
        Me.vsfTemp.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfTemp.Name = "vsfTemp"
        Me.vsfTemp.Rows.DefaultSize = 24
        Me.vsfTemp.Size = New System.Drawing.Size(203, 77)
        Me.vsfTemp.StyleInfo = resources.GetString("vsfTemp.StyleInfo")
        Me.vsfTemp.TabIndex = 600
        Me.vsfTemp.Visible = False
        '
        'vsfSplit
        '
        Me.vsfSplit.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfSplit.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfSplit.ExtendLastCol = True
        Me.vsfSplit.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfSplit.Location = New System.Drawing.Point(592, 344)
        Me.vsfSplit.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfSplit.Name = "vsfSplit"
        Me.vsfSplit.Rows.DefaultSize = 24
        Me.vsfSplit.Size = New System.Drawing.Size(203, 77)
        Me.vsfSplit.StyleInfo = resources.GetString("vsfSplit.StyleInfo")
        Me.vsfSplit.TabIndex = 601
        Me.vsfSplit.Visible = False
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.txtNo)
        Me.Panel4.Controls.Add(Me.btnClose)
        Me.Panel4.Controls.Add(Me.txtDocNo)
        Me.Panel4.Controls.Add(Me.Label6)
        Me.Panel4.Controls.Add(Me.txtScan)
        Me.Panel4.Controls.Add(Me.lblPrevBill)
        Me.Panel4.Controls.Add(Me.cboPfx)
        Me.Panel4.Controls.Add(Me.btnOptions)
        Me.Panel4.Controls.Add(Me.chkSMS)
        Me.Panel4.Controls.Add(Me.btnPrint)
        Me.Panel4.Controls.Add(Me.btnShowPts)
        Me.Panel4.Controls.Add(Me.btnRePrint)
        Me.Panel4.Controls.Add(Me.btnAddItem)
        Me.Panel4.Controls.Add(Me.btnCancel)
        Me.Panel4.Controls.Add(Me.txtQty)
        Me.Panel4.Controls.Add(Me.btnDelete)
        Me.Panel4.Controls.Add(Me.txtPoints)
        Me.Panel4.Controls.Add(Me.btnSearch)
        Me.Panel4.Controls.Add(Me.btnHold)
        Me.Panel4.Controls.Add(Me.Label11)
        Me.Panel4.Controls.Add(Me.btnClearItems)
        Me.Panel4.Controls.Add(Me.btnLoadTemp)
        Me.Panel4.Controls.Add(Me.lblPoints)
        Me.Panel4.Controls.Add(Me.btnAddParty)
        Me.Panel4.Controls.Add(Me.txtParty)
        Me.Panel4.Controls.Add(Me.Label1)
        Me.Panel4.Location = New System.Drawing.Point(406, 173)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(25, 28)
        Me.Panel4.TabIndex = 11
        Me.Panel4.Visible = False
        '
        'btnTopTen
        '
        Me.btnTopTen.BackColor = System.Drawing.Color.RoyalBlue
        Me.btnTopTen.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTopTen.Image = CType(resources.GetObject("btnTopTen.Image"), System.Drawing.Image)
        Me.btnTopTen.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnTopTen.Location = New System.Drawing.Point(4, 62)
        Me.btnTopTen.Name = "btnTopTen"
        Me.btnTopTen.Size = New System.Drawing.Size(245, 52)
        Me.btnTopTen.TabIndex = 602
        Me.btnTopTen.Text = "Top 10 Items"
        Me.btnTopTen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnTopTen.UseVisualStyleBackColor = False
        '
        'btn10
        '
        Me.btn10.BackColor = System.Drawing.Color.Fuchsia
        Me.btn10.BackgroundImage = CType(resources.GetObject("btn10.BackgroundImage"), System.Drawing.Image)
        Me.btn10.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btn10.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn10.Location = New System.Drawing.Point(1136, 62)
        Me.btn10.Name = "btn10"
        Me.btn10.Size = New System.Drawing.Size(70, 52)
        Me.btn10.TabIndex = 32
        Me.btn10.Text = "Sprite"
        Me.btn10.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.btn10.UseVisualStyleBackColor = False
        '
        'btn9
        '
        Me.btn9.BackColor = System.Drawing.Color.Fuchsia
        Me.btn9.BackgroundImage = CType(resources.GetObject("btn9.BackgroundImage"), System.Drawing.Image)
        Me.btn9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btn9.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn9.Location = New System.Drawing.Point(1049, 62)
        Me.btn9.Name = "btn9"
        Me.btn9.Size = New System.Drawing.Size(70, 52)
        Me.btn9.TabIndex = 31
        Me.btn9.Text = "Kinley Soda"
        Me.btn9.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.btn9.UseVisualStyleBackColor = False
        '
        'btn8
        '
        Me.btn8.BackColor = System.Drawing.Color.Yellow
        Me.btn8.BackgroundImage = CType(resources.GetObject("btn8.BackgroundImage"), System.Drawing.Image)
        Me.btn8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btn8.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn8.Location = New System.Drawing.Point(962, 62)
        Me.btn8.Name = "btn8"
        Me.btn8.Size = New System.Drawing.Size(70, 52)
        Me.btn8.TabIndex = 29
        Me.btn8.Text = "Bacadi"
        Me.btn8.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.btn8.UseVisualStyleBackColor = False
        '
        'btn7
        '
        Me.btn7.BackColor = System.Drawing.Color.Yellow
        Me.btn7.BackgroundImage = CType(resources.GetObject("btn7.BackgroundImage"), System.Drawing.Image)
        Me.btn7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btn7.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn7.Location = New System.Drawing.Point(875, 62)
        Me.btn7.Name = "btn7"
        Me.btn7.Size = New System.Drawing.Size(70, 52)
        Me.btn7.TabIndex = 29
        Me.btn7.Text = "Water Bottles"
        Me.btn7.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.btn7.UseVisualStyleBackColor = False
        '
        'btn1
        '
        Me.btn1.BackColor = System.Drawing.Color.SpringGreen
        Me.btn1.BackgroundImage = CType(resources.GetObject("btn1.BackgroundImage"), System.Drawing.Image)
        Me.btn1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btn1.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btn1.FlatAppearance.BorderSize = 3
        Me.btn1.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn1.Location = New System.Drawing.Point(318, 62)
        Me.btn1.Name = "btn1"
        Me.btn1.Size = New System.Drawing.Size(74, 52)
        Me.btn1.TabIndex = 27
        Me.btn1.Text = "KF 650 ml"
        Me.btn1.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.btn1.UseVisualStyleBackColor = False
        '
        'btn6
        '
        Me.btn6.BackColor = System.Drawing.Color.Orange
        Me.btn6.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn6.Location = New System.Drawing.Point(788, 62)
        Me.btn6.Name = "btn6"
        Me.btn6.Size = New System.Drawing.Size(70, 52)
        Me.btn6.TabIndex = 29
        Me.btn6.Text = "RUM"
        Me.btn6.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.btn6.UseVisualStyleBackColor = False
        '
        'btn5
        '
        Me.btn5.BackColor = System.Drawing.Color.Turquoise
        Me.btn5.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn5.Location = New System.Drawing.Point(527, 62)
        Me.btn5.Name = "btn5"
        Me.btn5.Size = New System.Drawing.Size(70, 52)
        Me.btn5.TabIndex = 29
        Me.btn5.Text = "8 PM"
        Me.btn5.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.btn5.UseVisualStyleBackColor = False
        '
        'btn4
        '
        Me.btn4.BackColor = System.Drawing.Color.Orange
        Me.btn4.BackgroundImage = CType(resources.GetObject("btn4.BackgroundImage"), System.Drawing.Image)
        Me.btn4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btn4.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn4.Location = New System.Drawing.Point(701, 62)
        Me.btn4.Name = "btn4"
        Me.btn4.Size = New System.Drawing.Size(70, 52)
        Me.btn4.TabIndex = 30
        Me.btn4.Text = "Covers"
        Me.btn4.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.btn4.UseVisualStyleBackColor = False
        '
        'btn3
        '
        Me.btn3.BackColor = System.Drawing.Color.Turquoise
        Me.btn3.BackgroundImage = CType(resources.GetObject("btn3.BackgroundImage"), System.Drawing.Image)
        Me.btn3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.btn3.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn3.Location = New System.Drawing.Point(614, 62)
        Me.btn3.Name = "btn3"
        Me.btn3.Size = New System.Drawing.Size(70, 52)
        Me.btn3.TabIndex = 29
        Me.btn3.Text = "Snacks"
        Me.btn3.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.btn3.UseVisualStyleBackColor = False
        '
        'btn2
        '
        Me.btn2.BackColor = System.Drawing.Color.SpringGreen
        Me.btn2.BackgroundImage = CType(resources.GetObject("btn2.BackgroundImage"), System.Drawing.Image)
        Me.btn2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.btn2.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn2.Location = New System.Drawing.Point(440, 62)
        Me.btn2.Name = "btn2"
        Me.btn2.Size = New System.Drawing.Size(70, 52)
        Me.btn2.TabIndex = 28
        Me.btn2.Text = "Cigeratte"
        Me.btn2.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.btn2.UseVisualStyleBackColor = False
        '
        'btnleftArr
        '
        Me.btnleftArr.BackColor = System.Drawing.Color.MidnightBlue
        Me.btnleftArr.BackgroundImage = CType(resources.GetObject("btnleftArr.BackgroundImage"), System.Drawing.Image)
        Me.btnleftArr.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.btnleftArr.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnleftArr.FlatAppearance.BorderSize = 3
        Me.btnleftArr.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnleftArr.Location = New System.Drawing.Point(248, 62)
        Me.btnleftArr.Name = "btnleftArr"
        Me.btnleftArr.Size = New System.Drawing.Size(70, 52)
        Me.btnleftArr.TabIndex = 33
        Me.btnleftArr.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnleftArr.UseVisualStyleBackColor = False
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.MidnightBlue
        Me.Button1.BackgroundImage = CType(resources.GetObject("Button1.BackgroundImage"), System.Drawing.Image)
        Me.Button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.Button1.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.Button1.FlatAppearance.BorderSize = 3
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Location = New System.Drawing.Point(1223, 62)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(70, 52)
        Me.Button1.TabIndex = 604
        Me.Button1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.Label3)
        Me.Panel5.Controls.Add(Me.imgSalPer)
        Me.Panel5.Controls.Add(Me.lblSalPer)
        Me.Panel5.Location = New System.Drawing.Point(488, 9)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(163, 36)
        Me.Panel5.TabIndex = 605
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(206, 12)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(0, 17)
        Me.Label3.TabIndex = 601
        '
        'imgSalPer
        '
        Me.imgSalPer.BackgroundImage = CType(resources.GetObject("imgSalPer.BackgroundImage"), System.Drawing.Image)
        Me.imgSalPer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.imgSalPer.Location = New System.Drawing.Point(1, 2)
        Me.imgSalPer.Name = "imgSalPer"
        Me.imgSalPer.Size = New System.Drawing.Size(22, 31)
        Me.imgSalPer.TabIndex = 26
        Me.imgSalPer.TabStop = False
        '
        'lblSalPer
        '
        Me.lblSalPer.AutoSize = True
        Me.lblSalPer.Font = New System.Drawing.Font("Verdana", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSalPer.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.lblSalPer.Location = New System.Drawing.Point(33, 11)
        Me.lblSalPer.Name = "lblSalPer"
        Me.lblSalPer.Size = New System.Drawing.Size(129, 18)
        Me.lblSalPer.TabIndex = 600
        Me.lblSalPer.Text = "Sales Person-"
        '
        'Panel7
        '
        Me.Panel7.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel7.Controls.Add(Me.Label2)
        Me.Panel7.Controls.Add(Me.Label13)
        Me.Panel7.Controls.Add(Me.lblCounter)
        Me.Panel7.Controls.Add(Me.imgCounName)
        Me.Panel7.Location = New System.Drawing.Point(825, 8)
        Me.Panel7.Name = "Panel7"
        Me.Panel7.Size = New System.Drawing.Size(117, 36)
        Me.Panel7.TabIndex = 606
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label2.Location = New System.Drawing.Point(108, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(0, 16)
        Me.Label2.TabIndex = 602
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(206, 12)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(0, 17)
        Me.Label13.TabIndex = 601
        '
        'lblCounter
        '
        Me.lblCounter.AutoSize = True
        Me.lblCounter.Font = New System.Drawing.Font("Verdana", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCounter.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.lblCounter.Location = New System.Drawing.Point(31, 11)
        Me.lblCounter.Name = "lblCounter"
        Me.lblCounter.Size = New System.Drawing.Size(87, 18)
        Me.lblCounter.TabIndex = 599
        Me.lblCounter.Text = "Counter-"
        '
        'imgCounName
        '
        Me.imgCounName.BackgroundImage = CType(resources.GetObject("imgCounName.BackgroundImage"), System.Drawing.Image)
        Me.imgCounName.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.imgCounName.Location = New System.Drawing.Point(3, 3)
        Me.imgCounName.Name = "imgCounName"
        Me.imgCounName.Size = New System.Drawing.Size(22, 31)
        Me.imgCounName.TabIndex = 26
        Me.imgCounName.TabStop = False
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Verdana", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label7.Location = New System.Drawing.Point(2, 7)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(59, 36)
        Me.Label7.TabIndex = 607
        Me.Label7.Text = "Date && No"
        '
        'frmTrnPosInv
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.Color.MidnightBlue
        Me.ClientSize = New System.Drawing.Size(1370, 749)
        Me.Controls.Add(Me.btn10)
        Me.Controls.Add(Me.btn9)
        Me.Controls.Add(Me.dtDocDate)
        Me.Controls.Add(Me.btn8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.btn7)
        Me.Controls.Add(Me.Panel7)
        Me.Controls.Add(Me.btn1)
        Me.Controls.Add(Me.btn6)
        Me.Controls.Add(Me.Panel5)
        Me.Controls.Add(Me.btn5)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.btn4)
        Me.Controls.Add(Me.btnleftArr)
        Me.Controls.Add(Me.btn3)
        Me.Controls.Add(Me.btn2)
        Me.Controls.Add(Me.btnTopTen)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.vsfSplit)
        Me.Controls.Add(Me.vsfTemp)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.scMain)
        Me.Controls.Add(Me.FlowLayoutPanel1)
        Me.Controls.Add(Me.imgSoftvent)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.txtPrintNo)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.txtSalesPerson)
        Me.Controls.Add(Me.txtDummy2)
        Me.Controls.Add(Me.txtCount)
        Me.Controls.Add(Me.txtDocID)
        Me.Controls.Add(Me.txtDummy)
        Me.Controls.Add(Me.lblFrm)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.vsfOptions)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MinimumSize = New System.Drawing.Size(1130, 630)
        Me.Name = "frmTrnPosInv"
        Me.ShowIcon = False
        Me.Text = "frmTrnSO"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfOptions, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cntxt.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.FlowLayoutPanel2.ResumeLayout(False)
        Me.FlowLayoutPanel2.PerformLayout()
        CType(Me.vsfMenu, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfTot, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfCust, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scMain.Panel1.ResumeLayout(False)
        Me.scMain.Panel2.ResumeLayout(False)
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scMain.ResumeLayout(False)
        Me.scTot.Panel1.ResumeLayout(False)
        Me.scTot.Panel2.ResumeLayout(False)
        CType(Me.scTot, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scTot.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.imgSoftvent, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfTemp, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfSplit, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        CType(Me.imgSalPer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel7.ResumeLayout(False)
        Me.Panel7.PerformLayout()
        CType(Me.imgCounName, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents vsfOptions As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents tlToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents txtDocID As System.Windows.Forms.TextBox
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Public WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents cntxt As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveThisToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents cdlg As OpenFileDialog
    Public WithEvents lblFrm As Label
    Public WithEvents lblTitle As Label
    Friend WithEvents lblTotTip As Label
    Public WithEvents txtDummy As TextBox
    Public WithEvents txtDummy2 As TextBox
    Public WithEvents txtCount As TextBox
    Public WithEvents txtSalesPerson As TextBox
    Public WithEvents txtScan As TextBox
    Friend WithEvents Label6 As Label
    Public WithEvents txtParty As TextBox
    Friend WithEvents Label1 As Label
    Public WithEvents txtPrintNo As TextBox
    Public WithEvents dtDocDate As DateTimePicker
    Friend WithEvents Panel3 As Panel
    Friend WithEvents txtFinalValue As TextBox
    Friend WithEvents txtTotalQty As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents txtNoOfItems As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents vsfMenu As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents imgSoftvent As PictureBox
    Friend WithEvents vsfTot As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents txtQty As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents btnAddItem As Button
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents btnOptions As C1.Win.C1Input.C1SplitButton
    Friend WithEvents DropDownItem4 As C1.Win.C1Input.DropDownItem
    Friend WithEvents DropDownItem5 As C1.Win.C1Input.DropDownItem
    Friend WithEvents btnAlias As C1.Win.C1Input.DropDownItem
    Friend WithEvents btnPrint As Button
    Friend WithEvents lblCancelled As Label
    Friend WithEvents btnRePrint As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnHold As Button
    Friend WithEvents btnClearItems As Button
    Friend WithEvents btnSearch As Button
    Friend WithEvents lblPrevBill As Label
    Friend WithEvents btnAddParty As Button
    Friend WithEvents lblItemName As Label
    Public WithEvents txtNo As TextBox
    Public WithEvents txtDocNo As TextBox
    Public WithEvents cboPfx As TextBox
    Friend WithEvents btnQuickSel As C1.Win.C1Input.C1SplitButton
    Friend WithEvents DropDownItem1 As C1.Win.C1Input.DropDownItem
    Friend WithEvents DropDownItem2 As C1.Win.C1Input.DropDownItem
    Friend WithEvents DropDownItem3 As C1.Win.C1Input.DropDownItem
    Friend WithEvents btnLoadTemp As C1.Win.C1Input.C1SplitButton
    Friend WithEvents lblCollAmt As Label
    Friend WithEvents txtPayback As TextBox
    Friend WithEvents vsfCust As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnClose As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents scMain As SplitContainer
    Friend WithEvents scTot As SplitContainer
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Label4 As Label
    Friend WithEvents FlowLayoutPanel2 As FlowLayoutPanel
    Friend WithEvents lblLmtBeer As Label
    Friend WithEvents lblLmtWine As Label
    Friend WithEvents lblLmtIML As Label
    Friend WithEvents chkSMS As CheckBox
    Friend WithEvents txtNoteValQry As TextBox
    Friend WithEvents btnCurr As Button
    Public WithEvents txtRcvdAmt As TextBox
    Friend WithEvents Label12 As Label
    Friend WithEvents txtHdrInvVal As TextBox
    Public WithEvents txtPoints As TextBox
    Friend WithEvents lblPoints As Label
    Friend WithEvents btnShowPts As Button
    Friend WithEvents vsfTemp As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents vsfSplit As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents Panel4 As Panel
    Friend WithEvents btnTopTen As Button
    Friend WithEvents Watermark1 As Watermark
    Friend WithEvents btn10 As Button
    Friend WithEvents btn9 As Button
    Friend WithEvents btn8 As Button
    Friend WithEvents btn7 As Button
    Friend WithEvents btn1 As Button
    Friend WithEvents btn6 As Button
    Friend WithEvents btn5 As Button
    Friend WithEvents btn4 As Button
    Friend WithEvents btn3 As Button
    Friend WithEvents btn2 As Button
    Friend WithEvents btnleftArr As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents Panel5 As Panel
    Friend WithEvents Label3 As Label
    Friend WithEvents imgSalPer As PictureBox
    Friend WithEvents lblSalPer As Label
    Friend WithEvents Panel7 As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents lblCounter As Label
    Friend WithEvents imgCounName As PictureBox
    Friend WithEvents Label7 As Label
End Class
