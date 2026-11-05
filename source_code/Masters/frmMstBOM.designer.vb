<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMstBOM
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMstBOM))
        Me.vsfTot = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfView = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfOptions = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.cboPfx = New System.Windows.Forms.ComboBox()
        Me.txtNo = New System.Windows.Forms.TextBox()
        Me.txtDocNo = New System.Windows.Forms.TextBox()
        Me.lblDocNo = New System.Windows.Forms.Label()
        Me.dtDocDate = New System.Windows.Forms.DateTimePicker()
        Me.lblDocDate = New System.Windows.Forms.Label()
        Me.txtDocID = New System.Windows.Forms.TextBox()
        Me.fraMain = New System.Windows.Forms.Panel()
        Me.tbAdnlFlds = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.btnHdrPin = New System.Windows.Forms.Button()
        Me.cntxt = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveThisToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.vsfHdr = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.tbAdnlFldsFtr = New System.Windows.Forms.TabControl()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.vsfFtr = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnItemCode = New System.Windows.Forms.Button()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.cdlg = New System.Windows.Forms.OpenFileDialog()
        Me.vsfSearch = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.lblUOM = New System.Windows.Forms.Label()
        Me.lblItemName = New System.Windows.Forms.Label()
        Me.cboItemCode = New System.Windows.Forms.ComboBox()
        Me.lblGrp = New System.Windows.Forms.Label()
        Me.lblLatest = New System.Windows.Forms.Label()
        Me.txtBOMID = New System.Windows.Forms.TextBox()
        Me.txtItemID = New System.Windows.Forms.TextBox()
        Me.pnlSearch = New System.Windows.Forms.Panel()
        Me.lblSearchImage = New System.Windows.Forms.Label()
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.tvBOM = New System.Windows.Forms.TreeView()
        Me.pnlDet = New System.Windows.Forms.Panel()
        Me.txtDummy = New System.Windows.Forms.TextBox()
        Me.pnlTree = New System.Windows.Forms.Panel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.PnlCopyFrom = New System.Windows.Forms.Panel()
        Me.btnCopyFrom = New System.Windows.Forms.Button()
        Me.txtCopyItem = New System.Windows.Forms.TextBox()
        Me.lblfrm = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.lblApproved = New System.Windows.Forms.Label()
        Me.gbEff = New System.Windows.Forms.GroupBox()
        Me.dtEffFrom = New System.Windows.Forms.DateTimePicker()
        Me.dtEffTo = New System.Windows.Forms.DateTimePicker()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.vsfEff = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnMulti = New System.Windows.Forms.Button()
        Me.btnRevision = New System.Windows.Forms.Button()
        Me.pnlToolTip = New System.Windows.Forms.Panel()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.lblToolTip = New System.Windows.Forms.Label()
        Me.btnTotPin = New System.Windows.Forms.Button()
        Me.svtBtnCtrl = New vLQMS.btnCtrl()
        Me.ucBtnTools = New vLQMS.ucBtn()
        CType(Me.vsfTot, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfOptions, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraMain.SuspendLayout()
        Me.tbAdnlFlds.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.cntxt.SuspendLayout()
        CType(Me.vsfHdr, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tbAdnlFldsFtr.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        CType(Me.vsfFtr, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlSearch.SuspendLayout()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlDet.SuspendLayout()
        Me.pnlTree.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.PnlCopyFrom.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbEff.SuspendLayout()
        CType(Me.vsfEff, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlToolTip.SuspendLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'vsfTot
        '
        Me.vsfTot.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfTot.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfTot.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfTot.ColumnInfo = "2,1,0,0,0,125,Columns:"
        Me.vsfTot.ExtendLastCol = True
        Me.vsfTot.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.vsfTot.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfTot.Location = New System.Drawing.Point(647, 476)
        Me.vsfTot.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfTot.Name = "vsfTot"
        Me.vsfTot.Rows.DefaultSize = 25
        Me.vsfTot.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.vsfTot.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfTot.Size = New System.Drawing.Size(475, 23)
        Me.vsfTot.StyleInfo = resources.GetString("vsfTot.StyleInfo")
        Me.vsfTot.TabIndex = 10
        Me.vsfTot.Tag = "DSGR"
        '
        'vsfView
        '
        Me.vsfView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfView.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfView.ExtendLastCol = True
        Me.vsfView.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfView.Location = New System.Drawing.Point(52, 178)
        Me.vsfView.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfView.Name = "vsfView"
        Me.vsfView.Rows.DefaultSize = 24
        Me.vsfView.Size = New System.Drawing.Size(496, 149)
        Me.vsfView.StyleInfo = resources.GetString("vsfView.StyleInfo")
        Me.vsfView.TabIndex = 3
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
        Me.vsfOptions.Location = New System.Drawing.Point(264, 236)
        Me.vsfOptions.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfOptions.Name = "vsfOptions"
        Me.vsfOptions.Rows.DefaultSize = 24
        Me.vsfOptions.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfOptions.Size = New System.Drawing.Size(413, 30)
        Me.vsfOptions.StyleInfo = resources.GetString("vsfOptions.StyleInfo")
        Me.vsfOptions.TabIndex = 38
        Me.vsfOptions.Tag = ""
        Me.vsfOptions.Visible = False
        '
        'cboPfx
        '
        Me.cboPfx.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.cboPfx.FormattingEnabled = True
        Me.cboPfx.Location = New System.Drawing.Point(123, 4)
        Me.cboPfx.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.cboPfx.Name = "cboPfx"
        Me.cboPfx.Size = New System.Drawing.Size(70, 25)
        Me.cboPfx.TabIndex = 560
        '
        'txtNo
        '
        Me.txtNo.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtNo.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtNo.Location = New System.Drawing.Point(201, 5)
        Me.txtNo.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtNo.MaxLength = 12
        Me.txtNo.Multiline = True
        Me.txtNo.Name = "txtNo"
        Me.txtNo.Size = New System.Drawing.Size(99, 23)
        Me.txtNo.TabIndex = 561
        Me.txtNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtDocNo
        '
        Me.txtDocNo.BackColor = System.Drawing.Color.White
        Me.txtDocNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDocNo.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDocNo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtDocNo.Location = New System.Drawing.Point(474, 81)
        Me.txtDocNo.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtDocNo.Multiline = True
        Me.txtDocNo.Name = "txtDocNo"
        Me.txtDocNo.ReadOnly = True
        Me.txtDocNo.Size = New System.Drawing.Size(27, 26)
        Me.txtDocNo.TabIndex = 1
        Me.txtDocNo.TabStop = False
        Me.txtDocNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtDocNo.Visible = False
        '
        'lblDocNo
        '
        Me.lblDocNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.lblDocNo.ForeColor = System.Drawing.Color.Blue
        Me.lblDocNo.Location = New System.Drawing.Point(12, 8)
        Me.lblDocNo.Name = "lblDocNo"
        Me.lblDocNo.Size = New System.Drawing.Size(107, 21)
        Me.lblDocNo.TabIndex = 36
        Me.lblDocNo.Tag = "R"
        Me.lblDocNo.Text = "Doc No"
        Me.lblDocNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtDocDate
        '
        Me.dtDocDate.CustomFormat = "dd/MM/yyyy"
        Me.dtDocDate.Enabled = False
        Me.dtDocDate.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.dtDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtDocDate.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.dtDocDate.Location = New System.Drawing.Point(342, 4)
        Me.dtDocDate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtDocDate.Name = "dtDocDate"
        Me.dtDocDate.Size = New System.Drawing.Size(112, 25)
        Me.dtDocDate.TabIndex = 563
        '
        'lblDocDate
        '
        Me.lblDocDate.AutoSize = True
        Me.lblDocDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.lblDocDate.ForeColor = System.Drawing.Color.Blue
        Me.lblDocDate.Location = New System.Drawing.Point(301, 8)
        Me.lblDocDate.Name = "lblDocDate"
        Me.lblDocDate.Size = New System.Drawing.Size(37, 16)
        Me.lblDocDate.TabIndex = 38
        Me.lblDocDate.Tag = "R"
        Me.lblDocDate.Text = "Date"
        '
        'txtDocID
        '
        Me.txtDocID.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDocID.Location = New System.Drawing.Point(394, 82)
        Me.txtDocID.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtDocID.Name = "txtDocID"
        Me.txtDocID.Size = New System.Drawing.Size(32, 26)
        Me.txtDocID.TabIndex = 40
        Me.txtDocID.Visible = False
        '
        'fraMain
        '
        Me.fraMain.BackColor = System.Drawing.Color.Transparent
        Me.fraMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.fraMain.Controls.Add(Me.lblDocDate)
        Me.fraMain.Controls.Add(Me.dtDocDate)
        Me.fraMain.Controls.Add(Me.lblDocNo)
        Me.fraMain.Controls.Add(Me.txtNo)
        Me.fraMain.Controls.Add(Me.cboPfx)
        Me.fraMain.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.fraMain.Location = New System.Drawing.Point(12, 40)
        Me.fraMain.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.fraMain.Name = "fraMain"
        Me.fraMain.Size = New System.Drawing.Size(27, 27)
        Me.fraMain.TabIndex = 1
        Me.fraMain.Visible = False
        '
        'tbAdnlFlds
        '
        Me.tbAdnlFlds.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbAdnlFlds.Controls.Add(Me.TabPage1)
        Me.tbAdnlFlds.Location = New System.Drawing.Point(758, 36)
        Me.tbAdnlFlds.Name = "tbAdnlFlds"
        Me.tbAdnlFlds.SelectedIndex = 0
        Me.tbAdnlFlds.Size = New System.Drawing.Size(370, 85)
        Me.tbAdnlFlds.TabIndex = 5
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.btnHdrPin)
        Me.TabPage1.Controls.Add(Me.vsfHdr)
        Me.TabPage1.Location = New System.Drawing.Point(4, 26)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(362, 55)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Header Info"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'btnHdrPin
        '
        Me.btnHdrPin.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnHdrPin.BackgroundImage = Global.vLQMS.My.Resources.Resources.GridPin
        Me.btnHdrPin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnHdrPin.ContextMenuStrip = Me.cntxt
        Me.btnHdrPin.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnHdrPin.ForeColor = System.Drawing.Color.Transparent
        Me.btnHdrPin.Location = New System.Drawing.Point(324, -342)
        Me.btnHdrPin.Name = "btnHdrPin"
        Me.btnHdrPin.Size = New System.Drawing.Size(15, 15)
        Me.btnHdrPin.TabIndex = 69
        Me.btnHdrPin.Text = "PIN"
        Me.btnHdrPin.UseVisualStyleBackColor = True
        '
        'cntxt
        '
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
        'vsfHdr
        '
        Me.vsfHdr.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfHdr.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfHdr.ColumnInfo = "2,1,0,0,0,120,Columns:"
        Me.vsfHdr.Dock = System.Windows.Forms.DockStyle.Fill
        Me.vsfHdr.ExtendLastCol = True
        Me.vsfHdr.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfHdr.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfHdr.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfHdr.Location = New System.Drawing.Point(3, 3)
        Me.vsfHdr.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfHdr.Name = "vsfHdr"
        Me.vsfHdr.Rows.DefaultSize = 24
        Me.vsfHdr.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfHdr.Size = New System.Drawing.Size(356, 49)
        Me.vsfHdr.StyleInfo = resources.GetString("vsfHdr.StyleInfo")
        Me.vsfHdr.TabIndex = 0
        Me.vsfHdr.Tag = "DSGR"
        '
        'tbAdnlFldsFtr
        '
        Me.tbAdnlFldsFtr.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbAdnlFldsFtr.Controls.Add(Me.TabPage2)
        Me.tbAdnlFldsFtr.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbAdnlFldsFtr.Location = New System.Drawing.Point(12, 447)
        Me.tbAdnlFldsFtr.Name = "tbAdnlFldsFtr"
        Me.tbAdnlFldsFtr.SelectedIndex = 0
        Me.tbAdnlFldsFtr.Size = New System.Drawing.Size(496, 61)
        Me.tbAdnlFldsFtr.TabIndex = 9
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.vsfFtr)
        Me.TabPage2.Location = New System.Drawing.Point(4, 29)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(488, 28)
        Me.TabPage2.TabIndex = 0
        Me.TabPage2.Text = "Footer Info"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'vsfFtr
        '
        Me.vsfFtr.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfFtr.AutoResize = True
        Me.vsfFtr.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfFtr.ColumnInfo = "2,1,0,0,0,125,Columns:"
        Me.vsfFtr.Dock = System.Windows.Forms.DockStyle.Fill
        Me.vsfFtr.ExtendLastCol = True
        Me.vsfFtr.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.vsfFtr.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcrossOut
        Me.vsfFtr.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcrossOut
        Me.vsfFtr.Location = New System.Drawing.Point(3, 3)
        Me.vsfFtr.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfFtr.Name = "vsfFtr"
        Me.vsfFtr.Rows.DefaultSize = 25
        Me.vsfFtr.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfFtr.ShowCellLabels = True
        Me.vsfFtr.Size = New System.Drawing.Size(482, 22)
        Me.vsfFtr.StyleInfo = resources.GetString("vsfFtr.StyleInfo")
        Me.vsfFtr.TabIndex = 0
        Me.vsfFtr.Tag = "DSGR"
        '
        'Timer1
        '
        '
        'btnItemCode
        '
        Me.btnItemCode.BackColor = System.Drawing.Color.Gainsboro
        Me.btnItemCode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnItemCode.CausesValidation = False
        Me.btnItemCode.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnItemCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnItemCode.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.btnItemCode.Location = New System.Drawing.Point(356, 38)
        Me.btnItemCode.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnItemCode.Name = "btnItemCode"
        Me.btnItemCode.Size = New System.Drawing.Size(33, 28)
        Me.btnItemCode.TabIndex = 2
        Me.btnItemCode.Text = "..."
        Me.ToolTip1.SetToolTip(Me.btnItemCode, "Select Item From Master")
        Me.btnItemCode.UseVisualStyleBackColor = False
        '
        'btnLoad
        '
        Me.btnLoad.BackColor = System.Drawing.Color.Gainsboro
        Me.btnLoad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnLoad.CausesValidation = False
        Me.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLoad.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoad.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnLoad.Location = New System.Drawing.Point(394, 39)
        Me.btnLoad.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(144, 28)
        Me.btnLoad.TabIndex = 0
        Me.btnLoad.Text = "Load Child Items "
        Me.ToolTip1.SetToolTip(Me.btnLoad, "Load Child Items Into Grid")
        Me.btnLoad.UseVisualStyleBackColor = False
        '
        'cdlg
        '
        Me.cdlg.FileName = "OpenFileDialog1"
        '
        'vsfSearch
        '
        Me.vsfSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.vsfSearch.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfSearch.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfSearch.ExtendLastCol = True
        Me.vsfSearch.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfSearch.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfSearch.Location = New System.Drawing.Point(852, 36)
        Me.vsfSearch.Margin = New System.Windows.Forms.Padding(4)
        Me.vsfSearch.Name = "vsfSearch"
        Me.vsfSearch.Rows.DefaultSize = 24
        Me.vsfSearch.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.RowRange
        Me.vsfSearch.Size = New System.Drawing.Size(272, 85)
        Me.vsfSearch.StyleInfo = resources.GetString("vsfSearch.StyleInfo")
        Me.vsfSearch.TabIndex = 1
        Me.vsfSearch.Visible = False
        '
        'lblUOM
        '
        Me.lblUOM.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUOM.ForeColor = System.Drawing.Color.Blue
        Me.lblUOM.Location = New System.Drawing.Point(8, 98)
        Me.lblUOM.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblUOM.Name = "lblUOM"
        Me.lblUOM.Size = New System.Drawing.Size(374, 21)
        Me.lblUOM.TabIndex = 583
        Me.lblUOM.Text = "Nos"
        '
        'lblItemName
        '
        Me.lblItemName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItemName.ForeColor = System.Drawing.Color.Blue
        Me.lblItemName.Location = New System.Drawing.Point(76, 70)
        Me.lblItemName.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblItemName.Name = "lblItemName"
        Me.lblItemName.Size = New System.Drawing.Size(350, 24)
        Me.lblItemName.TabIndex = 580
        Me.lblItemName.Text = "Item Name"
        Me.lblItemName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboItemCode
        '
        Me.cboItemCode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cboItemCode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboItemCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboItemCode.FormattingEnabled = True
        Me.cboItemCode.Location = New System.Drawing.Point(95, 38)
        Me.cboItemCode.Margin = New System.Windows.Forms.Padding(4)
        Me.cboItemCode.Name = "cboItemCode"
        Me.cboItemCode.Size = New System.Drawing.Size(255, 28)
        Me.cboItemCode.TabIndex = 0
        '
        'lblGrp
        '
        Me.lblGrp.AutoSize = True
        Me.lblGrp.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGrp.ForeColor = System.Drawing.Color.Red
        Me.lblGrp.Location = New System.Drawing.Point(4, 38)
        Me.lblGrp.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblGrp.Name = "lblGrp"
        Me.lblGrp.Size = New System.Drawing.Size(83, 20)
        Me.lblGrp.TabIndex = 579
        Me.lblGrp.Tag = "M"
        Me.lblGrp.Text = "Item Code"
        '
        'lblLatest
        '
        Me.lblLatest.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblLatest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblLatest.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLatest.ForeColor = System.Drawing.Color.Black
        Me.lblLatest.Location = New System.Drawing.Point(291, 3)
        Me.lblLatest.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblLatest.Name = "lblLatest"
        Me.lblLatest.Size = New System.Drawing.Size(166, 30)
        Me.lblLatest.TabIndex = 586
        Me.lblLatest.Text = "Latest BOM"
        Me.lblLatest.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblLatest.Visible = False
        '
        'txtBOMID
        '
        Me.txtBOMID.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBOMID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtBOMID.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtBOMID.Location = New System.Drawing.Point(53, 241)
        Me.txtBOMID.Margin = New System.Windows.Forms.Padding(4)
        Me.txtBOMID.MaxLength = 250
        Me.txtBOMID.Multiline = True
        Me.txtBOMID.Name = "txtBOMID"
        Me.txtBOMID.Size = New System.Drawing.Size(501, 25)
        Me.txtBOMID.TabIndex = 587
        Me.txtBOMID.Visible = False
        '
        'txtItemID
        '
        Me.txtItemID.Location = New System.Drawing.Point(507, 81)
        Me.txtItemID.Name = "txtItemID"
        Me.txtItemID.Size = New System.Drawing.Size(41, 25)
        Me.txtItemID.TabIndex = 588
        Me.txtItemID.Visible = False
        '
        'pnlSearch
        '
        Me.pnlSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlSearch.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.pnlSearch.BackColor = System.Drawing.Color.FromArgb(CType(CType(53, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(106, Byte), Integer))
        Me.pnlSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.pnlSearch.Controls.Add(Me.lblSearchImage)
        Me.pnlSearch.Controls.Add(Me.lblSearch)
        Me.pnlSearch.Controls.Add(Me.txtSearch)
        Me.pnlSearch.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlSearch.Location = New System.Drawing.Point(688, 3)
        Me.pnlSearch.Name = "pnlSearch"
        Me.pnlSearch.Size = New System.Drawing.Size(437, 29)
        Me.pnlSearch.TabIndex = 6
        '
        'lblSearchImage
        '
        Me.lblSearchImage.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSearchImage.BackColor = System.Drawing.Color.White
        Me.lblSearchImage.ForeColor = System.Drawing.Color.Transparent
        Me.lblSearchImage.Image = Global.vLQMS.My.Resources.Resources.D_FIND
        Me.lblSearchImage.Location = New System.Drawing.Point(367, 4)
        Me.lblSearchImage.Name = "lblSearchImage"
        Me.lblSearchImage.Size = New System.Drawing.Size(31, 18)
        Me.lblSearchImage.TabIndex = 15
        '
        'lblSearch
        '
        Me.lblSearch.AutoSize = True
        Me.lblSearch.BackColor = System.Drawing.Color.FromArgb(CType(CType(53, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(106, Byte), Integer))
        Me.lblSearch.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSearch.ForeColor = System.Drawing.Color.White
        Me.lblSearch.Location = New System.Drawing.Point(71, 7)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(45, 15)
        Me.lblSearch.TabIndex = 2
        Me.lblSearch.Text = "Search"
        '
        'txtSearch
        '
        Me.txtSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSearch.BackColor = System.Drawing.Color.White
        Me.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSearch.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSearch.Location = New System.Drawing.Point(138, 2)
        Me.txtSearch.Margin = New System.Windows.Forms.Padding(4)
        Me.txtSearch.MaxLength = 25
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(261, 26)
        Me.txtSearch.TabIndex = 0
        '
        'vsfDet
        '
        Me.vsfDet.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "2,1,0,0,0,120,Columns:"
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(4, 4)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(736, 282)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 1
        Me.vsfDet.Tag = ""
        '
        'tvBOM
        '
        Me.tvBOM.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tvBOM.Location = New System.Drawing.Point(3, 3)
        Me.tvBOM.Name = "tvBOM"
        Me.tvBOM.Size = New System.Drawing.Size(364, 276)
        Me.tvBOM.TabIndex = 0
        Me.tvBOM.Visible = False
        '
        'pnlDet
        '
        Me.pnlDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlDet.Controls.Add(Me.vsfDet)
        Me.pnlDet.Controls.Add(Me.vsfOptions)
        Me.pnlDet.Controls.Add(Me.txtBOMID)
        Me.pnlDet.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlDet.Location = New System.Drawing.Point(8, 123)
        Me.pnlDet.Name = "pnlDet"
        Me.pnlDet.Size = New System.Drawing.Size(744, 290)
        Me.pnlDet.TabIndex = 3
        '
        'txtDummy
        '
        Me.txtDummy.Location = New System.Drawing.Point(429, 82)
        Me.txtDummy.Name = "txtDummy"
        Me.txtDummy.Size = New System.Drawing.Size(28, 25)
        Me.txtDummy.TabIndex = 598
        Me.txtDummy.Visible = False
        '
        'pnlTree
        '
        Me.pnlTree.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlTree.Controls.Add(Me.tvBOM)
        Me.pnlTree.Location = New System.Drawing.Point(758, 128)
        Me.pnlTree.Name = "pnlTree"
        Me.pnlTree.Size = New System.Drawing.Size(370, 285)
        Me.pnlTree.TabIndex = 12
        Me.pnlTree.Visible = False
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(53, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(106, Byte), Integer))
        Me.Panel1.Controls.Add(Me.PnlCopyFrom)
        Me.Panel1.Controls.Add(Me.lblfrm)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.PictureBox1)
        Me.Panel1.Controls.Add(Me.PictureBox2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1133, 35)
        Me.Panel1.TabIndex = 594
        '
        'PnlCopyFrom
        '
        Me.PnlCopyFrom.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PnlCopyFrom.BackColor = System.Drawing.Color.FromArgb(CType(CType(53, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(106, Byte), Integer))
        Me.PnlCopyFrom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PnlCopyFrom.Controls.Add(Me.btnCopyFrom)
        Me.PnlCopyFrom.Controls.Add(Me.txtCopyItem)
        Me.PnlCopyFrom.Location = New System.Drawing.Point(489, 3)
        Me.PnlCopyFrom.Name = "PnlCopyFrom"
        Me.PnlCopyFrom.Size = New System.Drawing.Size(154, 30)
        Me.PnlCopyFrom.TabIndex = 221
        '
        'btnCopyFrom
        '
        Me.btnCopyFrom.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.btnCopyFrom.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCopyFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCopyFrom.Location = New System.Drawing.Point(8, 2)
        Me.btnCopyFrom.Name = "btnCopyFrom"
        Me.btnCopyFrom.Size = New System.Drawing.Size(106, 28)
        Me.btnCopyFrom.TabIndex = 2
        Me.btnCopyFrom.Text = "Copy From"
        Me.btnCopyFrom.UseVisualStyleBackColor = False
        Me.btnCopyFrom.Visible = False
        '
        'txtCopyItem
        '
        Me.txtCopyItem.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCopyItem.BackColor = System.Drawing.Color.White
        Me.txtCopyItem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCopyItem.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCopyItem.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtCopyItem.Location = New System.Drawing.Point(128, 4)
        Me.txtCopyItem.Margin = New System.Windows.Forms.Padding(4)
        Me.txtCopyItem.Name = "txtCopyItem"
        Me.txtCopyItem.Size = New System.Drawing.Size(16, 25)
        Me.txtCopyItem.TabIndex = 1
        Me.txtCopyItem.Visible = False
        '
        'lblfrm
        '
        Me.lblfrm.AutoSize = True
        Me.lblfrm.BackColor = System.Drawing.Color.FromArgb(CType(CType(53, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(106, Byte), Integer))
        Me.lblfrm.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lblfrm.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblfrm.ForeColor = System.Drawing.Color.White
        Me.lblfrm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblfrm.Location = New System.Drawing.Point(12, 6)
        Me.lblfrm.Name = "lblfrm"
        Me.lblfrm.Size = New System.Drawing.Size(0, 25)
        Me.lblfrm.TabIndex = 220
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.White
        Me.Label6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label6.Location = New System.Drawing.Point(58, 8)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(0, 17)
        Me.Label6.TabIndex = 218
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New System.Drawing.Point(250, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(34, 33)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 77
        Me.PictureBox1.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(352, -46)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(21, 21)
        Me.PictureBox2.TabIndex = 85
        Me.PictureBox2.TabStop = False
        '
        'lblApproved
        '
        Me.lblApproved.BackColor = System.Drawing.Color.Transparent
        Me.lblApproved.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblApproved.ForeColor = System.Drawing.Color.Green
        Me.lblApproved.Location = New System.Drawing.Point(605, 41)
        Me.lblApproved.Name = "lblApproved"
        Me.lblApproved.Size = New System.Drawing.Size(136, 28)
        Me.lblApproved.TabIndex = 595
        Me.lblApproved.Text = "APPROVED"
        Me.lblApproved.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblApproved.Visible = False
        '
        'gbEff
        '
        Me.gbEff.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbEff.Controls.Add(Me.dtEffFrom)
        Me.gbEff.Controls.Add(Me.dtEffTo)
        Me.gbEff.Controls.Add(Me.Label4)
        Me.gbEff.Controls.Add(Me.vsfEff)
        Me.gbEff.Controls.Add(Me.Label3)
        Me.gbEff.Location = New System.Drawing.Point(758, 123)
        Me.gbEff.Name = "gbEff"
        Me.gbEff.Size = New System.Drawing.Size(370, 290)
        Me.gbEff.TabIndex = 11
        Me.gbEff.TabStop = False
        '
        'dtEffFrom
        '
        Me.dtEffFrom.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.dtEffFrom.CustomFormat = "dd/MM/yyyy"
        Me.dtEffFrom.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.dtEffFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtEffFrom.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.dtEffFrom.Location = New System.Drawing.Point(99, 230)
        Me.dtEffFrom.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtEffFrom.Name = "dtEffFrom"
        Me.dtEffFrom.Size = New System.Drawing.Size(112, 25)
        Me.dtEffFrom.TabIndex = 111
        '
        'dtEffTo
        '
        Me.dtEffTo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.dtEffTo.CustomFormat = "dd/MM/yyyy"
        Me.dtEffTo.Enabled = False
        Me.dtEffTo.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.dtEffTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtEffTo.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.dtEffTo.Location = New System.Drawing.Point(99, 258)
        Me.dtEffTo.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtEffTo.MinDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtEffTo.Name = "dtEffTo"
        Me.dtEffTo.Size = New System.Drawing.Size(112, 25)
        Me.dtEffTo.TabIndex = 114
        Me.dtEffTo.Value = New Date(9998, 12, 31, 0, 0, 0, 0)
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Label4.ForeColor = System.Drawing.Color.Blue
        Me.Label4.Location = New System.Drawing.Point(4, 262)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(79, 16)
        Me.Label4.TabIndex = 115
        Me.Label4.Text = "Effective To"
        '
        'vsfEff
        '
        Me.vsfEff.AllowResizing = C1.Win.C1FlexGrid.AllowResizingEnum.None
        Me.vsfEff.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfEff.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfEff.ColumnInfo = "2,1,0,0,0,120,Columns:"
        Me.vsfEff.ExtendLastCol = True
        Me.vsfEff.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfEff.Location = New System.Drawing.Point(6, 11)
        Me.vsfEff.Name = "vsfEff"
        Me.vsfEff.Rows.Count = 5
        Me.vsfEff.Rows.DefaultSize = 24
        Me.vsfEff.Size = New System.Drawing.Size(358, 214)
        Me.vsfEff.StyleInfo = resources.GetString("vsfEff.StyleInfo")
        Me.vsfEff.TabIndex = 13
        Me.vsfEff.Tag = ""
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Label3.ForeColor = System.Drawing.Color.Red
        Me.Label3.Location = New System.Drawing.Point(4, 234)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(93, 16)
        Me.Label3.TabIndex = 113
        Me.Label3.Text = "Effective From"
        '
        'btnMulti
        '
        Me.btnMulti.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMulti.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMulti.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMulti.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnMulti.Location = New System.Drawing.Point(898, 419)
        Me.btnMulti.Name = "btnMulti"
        Me.btnMulti.Size = New System.Drawing.Size(230, 33)
        Me.btnMulti.TabIndex = 8
        Me.btnMulti.Text = "Qty Change for this Item in All BOM"
        Me.btnMulti.UseVisualStyleBackColor = False
        '
        'btnRevision
        '
        Me.btnRevision.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRevision.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRevision.Image = CType(resources.GetObject("btnRevision.Image"), System.Drawing.Image)
        Me.btnRevision.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRevision.Location = New System.Drawing.Point(599, 81)
        Me.btnRevision.Name = "btnRevision"
        Me.btnRevision.Size = New System.Drawing.Size(153, 29)
        Me.btnRevision.TabIndex = 7
        Me.btnRevision.Text = "Show Revisions"
        Me.btnRevision.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRevision.UseVisualStyleBackColor = False
        '
        'pnlToolTip
        '
        Me.pnlToolTip.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.pnlToolTip.BackgroundImage = CType(resources.GetObject("pnlToolTip.BackgroundImage"), System.Drawing.Image)
        Me.pnlToolTip.Controls.Add(Me.PictureBox3)
        Me.pnlToolTip.Controls.Add(Me.lblToolTip)
        Me.pnlToolTip.Location = New System.Drawing.Point(12, 516)
        Me.pnlToolTip.Name = "pnlToolTip"
        Me.pnlToolTip.Size = New System.Drawing.Size(783, 39)
        Me.pnlToolTip.TabIndex = 271
        Me.pnlToolTip.Visible = False
        '
        'PictureBox3
        '
        Me.PictureBox3.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.PictureBox3.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox3.BackgroundImage = CType(resources.GetObject("PictureBox3.BackgroundImage"), System.Drawing.Image)
        Me.PictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.PictureBox3.Location = New System.Drawing.Point(0, -2)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(45, 40)
        Me.PictureBox3.TabIndex = 218
        Me.PictureBox3.TabStop = False
        '
        'lblToolTip
        '
        Me.lblToolTip.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblToolTip.BackColor = System.Drawing.Color.Transparent
        Me.lblToolTip.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.lblToolTip.ForeColor = System.Drawing.Color.White
        Me.lblToolTip.Location = New System.Drawing.Point(44, 3)
        Me.lblToolTip.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblToolTip.Name = "lblToolTip"
        Me.lblToolTip.Size = New System.Drawing.Size(732, 30)
        Me.lblToolTip.TabIndex = 216
        Me.lblToolTip.Text = "Help Tips:"
        '
        'btnTotPin
        '
        Me.btnTotPin.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnTotPin.BackColor = System.Drawing.Color.Transparent
        Me.btnTotPin.BackgroundImage = Global.vLQMS.My.Resources.Resources.GridPin
        Me.btnTotPin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnTotPin.ContextMenuStrip = Me.cntxt
        Me.btnTotPin.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTotPin.ForeColor = System.Drawing.Color.Transparent
        Me.btnTotPin.Location = New System.Drawing.Point(1083, 505)
        Me.btnTotPin.Name = "btnTotPin"
        Me.btnTotPin.Size = New System.Drawing.Size(15, 15)
        Me.btnTotPin.TabIndex = 4
        Me.btnTotPin.Text = "PIN"
        Me.btnTotPin.UseVisualStyleBackColor = False
        Me.btnTotPin.Visible = False
        '
        'svtBtnCtrl
        '
        Me.svtBtnCtrl.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.svtBtnCtrl.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.svtBtnCtrl.CausesValidation = False
        Me.svtBtnCtrl.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.svtBtnCtrl.LayoutStyle = vLQMS.svtModule.typeLayoutStyles.Horizontal
        Me.svtBtnCtrl.Location = New System.Drawing.Point(178, 521)
        Me.svtBtnCtrl.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.svtBtnCtrl.Name = "svtBtnCtrl"
        Me.svtBtnCtrl.Size = New System.Drawing.Size(943, 34)
        Me.svtBtnCtrl.TabIndex = 4
        '
        'ucBtnTools
        '
        Me.ucBtnTools.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ucBtnTools.Location = New System.Drawing.Point(8, 413)
        Me.ucBtnTools.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.ucBtnTools.Name = "ucBtnTools"
        Me.ucBtnTools.Size = New System.Drawing.Size(605, 27)
        Me.ucBtnTools.TabIndex = 65
        '
        'frmMstBOM
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1133, 557)
        Me.Controls.Add(Me.txtItemID)
        Me.Controls.Add(Me.txtDocNo)
        Me.Controls.Add(Me.txtDummy)
        Me.Controls.Add(Me.txtDocID)
        Me.Controls.Add(Me.btnLoad)
        Me.Controls.Add(Me.btnMulti)
        Me.Controls.Add(Me.lblLatest)
        Me.Controls.Add(Me.vsfSearch)
        Me.Controls.Add(Me.lblApproved)
        Me.Controls.Add(Me.btnRevision)
        Me.Controls.Add(Me.pnlSearch)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.pnlTree)
        Me.Controls.Add(Me.vsfView)
        Me.Controls.Add(Me.lblUOM)
        Me.Controls.Add(Me.btnItemCode)
        Me.Controls.Add(Me.lblItemName)
        Me.Controls.Add(Me.cboItemCode)
        Me.Controls.Add(Me.lblGrp)
        Me.Controls.Add(Me.tbAdnlFldsFtr)
        Me.Controls.Add(Me.pnlToolTip)
        Me.Controls.Add(Me.btnTotPin)
        Me.Controls.Add(Me.fraMain)
        Me.Controls.Add(Me.svtBtnCtrl)
        Me.Controls.Add(Me.vsfTot)
        Me.Controls.Add(Me.tbAdnlFlds)
        Me.Controls.Add(Me.ucBtnTools)
        Me.Controls.Add(Me.pnlDet)
        Me.Controls.Add(Me.gbEff)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "frmMstBOM"
        Me.ShowIcon = False
        Me.Text = "Bill of Material"
        CType(Me.vsfTot, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfOptions, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraMain.ResumeLayout(False)
        Me.fraMain.PerformLayout()
        Me.tbAdnlFlds.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.cntxt.ResumeLayout(False)
        CType(Me.vsfHdr, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tbAdnlFldsFtr.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        CType(Me.vsfFtr, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlSearch.ResumeLayout(False)
        Me.pnlSearch.PerformLayout()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlDet.ResumeLayout(False)
        Me.pnlDet.PerformLayout()
        Me.pnlTree.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.PnlCopyFrom.ResumeLayout(False)
        Me.PnlCopyFrom.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbEff.ResumeLayout(False)
        Me.gbEff.PerformLayout()
        CType(Me.vsfEff, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlToolTip.ResumeLayout(False)
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents vsfTot As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents vsfView As C1.Win.C1FlexGrid.C1FlexGrid
    Public WithEvents vsfOptions As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents tlToolTip As System.Windows.Forms.ToolTip
    Public WithEvents cboPfx As System.Windows.Forms.ComboBox
    Public WithEvents txtNo As System.Windows.Forms.TextBox
    Public WithEvents txtDocNo As System.Windows.Forms.TextBox
    Friend WithEvents lblDocNo As System.Windows.Forms.Label
    Public WithEvents dtDocDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblDocDate As System.Windows.Forms.Label
    Friend WithEvents txtDocID As System.Windows.Forms.TextBox
    Friend WithEvents fraMain As System.Windows.Forms.Panel
    Public WithEvents tbAdnlFlds As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents vsfHdr As C1.Win.C1FlexGrid.C1FlexGrid
    Public WithEvents tbAdnlFldsFtr As System.Windows.Forms.TabControl
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents ucBtnTools As vLQMS.ucBtn
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Public WithEvents vsfFtr As C1.Win.C1FlexGrid.C1FlexGrid
    Public WithEvents svtBtnCtrl As vLQMS.btnCtrl
    Friend WithEvents btnHdrPin As System.Windows.Forms.Button
    Friend WithEvents cntxt As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveThisToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pnlToolTip As System.Windows.Forms.Panel
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents lblToolTip As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents cdlg As OpenFileDialog
    Friend WithEvents vsfSearch As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents lblUOM As Label
    Friend WithEvents btnItemCode As Button
    Friend WithEvents lblItemName As Label
    Friend WithEvents cboItemCode As ComboBox
    Friend WithEvents lblGrp As Label
    Friend WithEvents btnRevision As Button
    Friend WithEvents lblLatest As Label
    Friend WithEvents txtBOMID As TextBox
    Friend WithEvents txtItemID As TextBox
    Friend WithEvents pnlSearch As Panel
    Friend WithEvents lblSearchImage As Label
    Friend WithEvents lblSearch As Label
    Public WithEvents txtSearch As TextBox
    Public WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Public WithEvents tvBOM As TreeView
    Friend WithEvents pnlDet As Panel
    Friend WithEvents pnlTree As Panel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label6 As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents PictureBox2 As PictureBox
    Public WithEvents lblfrm As Label
    Friend WithEvents PnlCopyFrom As Panel
    Friend WithEvents btnCopyFrom As Button
    Friend WithEvents txtCopyItem As TextBox
    Friend WithEvents lblApproved As Label
    Friend WithEvents btnTotPin As Button
    Friend WithEvents gbEff As GroupBox
    Public WithEvents dtEffTo As DateTimePicker
    Friend WithEvents Label4 As Label
    Friend WithEvents vsfEff As C1.Win.C1FlexGrid.C1FlexGrid
    Public WithEvents dtEffFrom As DateTimePicker
    Friend WithEvents Label3 As Label
    Friend WithEvents btnMulti As Button
    Friend WithEvents txtDummy As TextBox
    Friend WithEvents btnLoad As Button
End Class
