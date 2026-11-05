<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmTrnVch
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTrnVch))
        Me.vsfTot = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.lblAuthorised = New System.Windows.Forms.Label()
        Me.lblCancelled = New System.Windows.Forms.Label()
        Me.vsfOptions = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.cboPfx = New System.Windows.Forms.ComboBox()
        Me.txtNo = New System.Windows.Forms.TextBox()
        Me.txtDocNo = New System.Windows.Forms.TextBox()
        Me.lblDocNo = New System.Windows.Forms.Label()
        Me.dtDocDate = New System.Windows.Forms.DateTimePicker()
        Me.lblDocDate = New System.Windows.Forms.Label()
        Me.txtDocID = New System.Windows.Forms.TextBox()
        Me.fraMain = New System.Windows.Forms.Panel()
        Me.fraAttach = New System.Windows.Forms.Panel()
        Me.btnAttachClose = New System.Windows.Forms.Button()
        Me.svtFileAttach = New vLQMS.svtFileCtrl()
        Me.tbAdnlFlds = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.lblHdrTip = New System.Windows.Forms.Label()
        Me.btnHdrPin = New System.Windows.Forms.Button()
        Me.cntxt = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveThisToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.vsfHdr = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.tbAdnlFldsFtr = New System.Windows.Forms.TabControl()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.btnFtrPin = New System.Windows.Forms.Button()
        Me.lblFtrTip = New System.Windows.Forms.Label()
        Me.vsfFtr = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cdlg = New System.Windows.Forms.OpenFileDialog()
        Me.lblFrm = New System.Windows.Forms.Label()
        Me.lblTotTip = New System.Windows.Forms.Label()
        Me.pnlToolTip = New System.Windows.Forms.Panel()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.lblToolTip = New System.Windows.Forms.Label()
        Me.btnTotPin = New System.Windows.Forms.Button()
        Me.btnDetPin = New System.Windows.Forms.Button()
        Me.vsfVwDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfAdj = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.chkShowBills = New System.Windows.Forms.CheckBox()
        Me.vsfView = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblTotCrValue = New System.Windows.Forms.Label()
        Me.lblTotDrValue = New System.Windows.Forms.Label()
        Me.lblCreditTotal = New System.Windows.Forms.Label()
        Me.lblDebitTotal = New System.Windows.Forms.Label()
        Me.txtDummy2 = New System.Windows.Forms.TextBox()
        Me.txtDummy = New System.Windows.Forms.TextBox()
        Me.srcTransaction = New vLQMS.ucSearch()
        Me.svtBtnCtrl = New vLQMS.btnCtrl()
        Me.ucBtnTools = New vLQMS.ucBtn()
        CType(Me.vsfTot, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfOptions, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraMain.SuspendLayout()
        Me.fraAttach.SuspendLayout()
        Me.tbAdnlFlds.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.cntxt.SuspendLayout()
        CType(Me.vsfHdr, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tbAdnlFldsFtr.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        CType(Me.vsfFtr, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlToolTip.SuspendLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfVwDet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfAdj, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'vsfTot
        '
        Me.vsfTot.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfTot.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfTot.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfTot.ColumnInfo = "2,1,0,0,0,120,Columns:"
        Me.vsfTot.ExtendLastCol = True
        Me.vsfTot.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfTot.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfTot.Location = New System.Drawing.Point(630, 413)
        Me.vsfTot.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfTot.Name = "vsfTot"
        Me.vsfTot.Rows.DefaultSize = 24
        Me.vsfTot.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfTot.Size = New System.Drawing.Size(475, 120)
        Me.vsfTot.StyleInfo = resources.GetString("vsfTot.StyleInfo")
        Me.vsfTot.TabIndex = 3
        Me.vsfTot.Tag = "DSGR"
        '
        'vsfDet
        '
        Me.vsfDet.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "2,1,0,0,0,110,Columns:"
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.vsfDet.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(6, 212)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 22
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(1099, 162)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 2
        Me.vsfDet.Tag = ""
        '
        'lblAuthorised
        '
        Me.lblAuthorised.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblAuthorised.AutoSize = True
        Me.lblAuthorised.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lblAuthorised.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle))
        Me.lblAuthorised.ForeColor = System.Drawing.Color.Red
        Me.lblAuthorised.Location = New System.Drawing.Point(504, 516)
        Me.lblAuthorised.Name = "lblAuthorised"
        Me.lblAuthorised.Size = New System.Drawing.Size(107, 16)
        Me.lblAuthorised.TabIndex = 36
        Me.lblAuthorised.Text = "AUTHORISED"
        '
        'lblCancelled
        '
        Me.lblCancelled.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCancelled.AutoSize = True
        Me.lblCancelled.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold)
        Me.lblCancelled.ForeColor = System.Drawing.Color.Red
        Me.lblCancelled.Location = New System.Drawing.Point(479, 488)
        Me.lblCancelled.Name = "lblCancelled"
        Me.lblCancelled.Size = New System.Drawing.Size(147, 25)
        Me.lblCancelled.TabIndex = 37
        Me.lblCancelled.Text = "CANCELLED"
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
        Me.vsfOptions.Location = New System.Drawing.Point(620, 294)
        Me.vsfOptions.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfOptions.Name = "vsfOptions"
        Me.vsfOptions.Rows.DefaultSize = 24
        Me.vsfOptions.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfOptions.Size = New System.Drawing.Size(8, 64)
        Me.vsfOptions.StyleInfo = resources.GetString("vsfOptions.StyleInfo")
        Me.vsfOptions.TabIndex = 38
        Me.vsfOptions.Tag = ""
        Me.vsfOptions.Visible = False
        '
        'cboPfx
        '
        Me.cboPfx.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.cboPfx.FormattingEnabled = True
        Me.cboPfx.Location = New System.Drawing.Point(110, 7)
        Me.cboPfx.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.cboPfx.Name = "cboPfx"
        Me.cboPfx.Size = New System.Drawing.Size(70, 21)
        Me.cboPfx.TabIndex = 0
        '
        'txtNo
        '
        Me.txtNo.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold)
        Me.txtNo.Location = New System.Drawing.Point(186, 8)
        Me.txtNo.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtNo.MaxLength = 12
        Me.txtNo.Name = "txtNo"
        Me.txtNo.Size = New System.Drawing.Size(95, 19)
        Me.txtNo.TabIndex = 1
        Me.txtNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtDocNo
        '
        Me.txtDocNo.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.txtDocNo.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtDocNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold)
        Me.txtDocNo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.txtDocNo.Location = New System.Drawing.Point(433, 8)
        Me.txtDocNo.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtDocNo.Multiline = True
        Me.txtDocNo.Name = "txtDocNo"
        Me.txtDocNo.ReadOnly = True
        Me.txtDocNo.Size = New System.Drawing.Size(191, 18)
        Me.txtDocNo.TabIndex = 564
        Me.txtDocNo.TabStop = False
        Me.txtDocNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblDocNo
        '
        Me.lblDocNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.lblDocNo.ForeColor = System.Drawing.Color.Blue
        Me.lblDocNo.Location = New System.Drawing.Point(4, 8)
        Me.lblDocNo.Name = "lblDocNo"
        Me.lblDocNo.Size = New System.Drawing.Size(108, 21)
        Me.lblDocNo.TabIndex = 0
        Me.lblDocNo.Tag = "R"
        Me.lblDocNo.Text = "Doc No"
        Me.lblDocNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtDocDate
        '
        Me.dtDocDate.CalendarFont = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.dtDocDate.CustomFormat = "dd/MM/yyyy"
        Me.dtDocDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.dtDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtDocDate.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.dtDocDate.Location = New System.Drawing.Point(322, 4)
        Me.dtDocDate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtDocDate.Name = "dtDocDate"
        Me.dtDocDate.Size = New System.Drawing.Size(105, 26)
        Me.dtDocDate.TabIndex = 2
        '
        'lblDocDate
        '
        Me.lblDocDate.AutoSize = True
        Me.lblDocDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.lblDocDate.ForeColor = System.Drawing.Color.Blue
        Me.lblDocDate.Location = New System.Drawing.Point(288, 11)
        Me.lblDocDate.Name = "lblDocDate"
        Me.lblDocDate.Size = New System.Drawing.Size(30, 13)
        Me.lblDocDate.TabIndex = 38
        Me.lblDocDate.Tag = "R"
        Me.lblDocDate.Text = "Date"
        '
        'txtDocID
        '
        Me.txtDocID.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtDocID.Location = New System.Drawing.Point(282, 72)
        Me.txtDocID.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtDocID.Name = "txtDocID"
        Me.txtDocID.Size = New System.Drawing.Size(40, 25)
        Me.txtDocID.TabIndex = 40
        Me.txtDocID.Visible = False
        '
        'fraMain
        '
        Me.fraMain.BackColor = System.Drawing.Color.Transparent
        Me.fraMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.fraMain.Controls.Add(Me.lblDocDate)
        Me.fraMain.Controls.Add(Me.txtDocID)
        Me.fraMain.Controls.Add(Me.dtDocDate)
        Me.fraMain.Controls.Add(Me.lblDocNo)
        Me.fraMain.Controls.Add(Me.txtDocNo)
        Me.fraMain.Controls.Add(Me.txtNo)
        Me.fraMain.Controls.Add(Me.cboPfx)
        Me.fraMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.fraMain.Location = New System.Drawing.Point(6, 33)
        Me.fraMain.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.fraMain.Name = "fraMain"
        Me.fraMain.Size = New System.Drawing.Size(631, 44)
        Me.fraMain.TabIndex = 1
        '
        'fraAttach
        '
        Me.fraAttach.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fraAttach.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.fraAttach.Controls.Add(Me.btnAttachClose)
        Me.fraAttach.Controls.Add(Me.svtFileAttach)
        Me.fraAttach.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.fraAttach.Location = New System.Drawing.Point(26, 568)
        Me.fraAttach.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.fraAttach.Name = "fraAttach"
        Me.fraAttach.Size = New System.Drawing.Size(34, 24)
        Me.fraAttach.TabIndex = 48
        Me.fraAttach.Visible = False
        '
        'btnAttachClose
        '
        Me.btnAttachClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAttachClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnAttachClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnAttachClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAttachClose.Location = New System.Drawing.Point(2, -11)
        Me.btnAttachClose.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnAttachClose.Name = "btnAttachClose"
        Me.btnAttachClose.Size = New System.Drawing.Size(70, 30)
        Me.btnAttachClose.TabIndex = 1
        Me.btnAttachClose.Text = "&Close"
        Me.btnAttachClose.UseVisualStyleBackColor = False
        '
        'svtFileAttach
        '
        Me.svtFileAttach.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.svtFileAttach.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.svtFileAttach.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.svtFileAttach.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.svtFileAttach.Location = New System.Drawing.Point(-227, -117)
        Me.svtFileAttach.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.svtFileAttach.MaximumSize = New System.Drawing.Size(254, 110)
        Me.svtFileAttach.MinimumSize = New System.Drawing.Size(254, 110)
        Me.svtFileAttach.Name = "svtFileAttach"
        Me.svtFileAttach.ShowPrintSelection = False
        Me.svtFileAttach.Size = New System.Drawing.Size(254, 110)
        Me.svtFileAttach.TabIndex = 0
        Me.svtFileAttach.ViewMode = False
        '
        'tbAdnlFlds
        '
        Me.tbAdnlFlds.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbAdnlFlds.Controls.Add(Me.TabPage1)
        Me.tbAdnlFlds.Location = New System.Drawing.Point(640, 33)
        Me.tbAdnlFlds.Name = "tbAdnlFlds"
        Me.tbAdnlFlds.SelectedIndex = 0
        Me.tbAdnlFlds.Size = New System.Drawing.Size(465, 176)
        Me.tbAdnlFlds.TabIndex = 1
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.lblHdrTip)
        Me.TabPage1.Controls.Add(Me.btnHdrPin)
        Me.TabPage1.Controls.Add(Me.vsfHdr)
        Me.TabPage1.Location = New System.Drawing.Point(4, 26)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(457, 146)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Header Info"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'lblHdrTip
        '
        Me.lblHdrTip.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblHdrTip.BackColor = System.Drawing.Color.Silver
        Me.lblHdrTip.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHdrTip.Image = Global.vLQMS.My.Resources.Resources.ToolTipInfo
        Me.lblHdrTip.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblHdrTip.Location = New System.Drawing.Point(3, 43)
        Me.lblHdrTip.Name = "lblHdrTip"
        Me.lblHdrTip.Size = New System.Drawing.Size(431, 25)
        Me.lblHdrTip.TabIndex = 1
        Me.lblHdrTip.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnHdrPin
        '
        Me.btnHdrPin.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnHdrPin.BackgroundImage = CType(resources.GetObject("btnHdrPin.BackgroundImage"), System.Drawing.Image)
        Me.btnHdrPin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnHdrPin.ContextMenuStrip = Me.cntxt
        Me.btnHdrPin.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnHdrPin.ForeColor = System.Drawing.Color.Transparent
        Me.btnHdrPin.Location = New System.Drawing.Point(438, 45)
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
        Me.vsfHdr.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfHdr.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfHdr.ColumnInfo = "2,1,0,0,0,120,Columns:"
        Me.vsfHdr.ExtendLastCol = True
        Me.vsfHdr.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfHdr.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfHdr.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfHdr.Location = New System.Drawing.Point(3, 3)
        Me.vsfHdr.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfHdr.Name = "vsfHdr"
        Me.vsfHdr.Rows.DefaultSize = 24
        Me.vsfHdr.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfHdr.Size = New System.Drawing.Size(451, 36)
        Me.vsfHdr.StyleInfo = resources.GetString("vsfHdr.StyleInfo")
        Me.vsfHdr.TabIndex = 0
        Me.vsfHdr.Tag = "DSGR"
        '
        'tbAdnlFldsFtr
        '
        Me.tbAdnlFldsFtr.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbAdnlFldsFtr.Controls.Add(Me.TabPage2)
        Me.tbAdnlFldsFtr.Location = New System.Drawing.Point(6, 413)
        Me.tbAdnlFldsFtr.Name = "tbAdnlFldsFtr"
        Me.tbAdnlFldsFtr.SelectedIndex = 0
        Me.tbAdnlFldsFtr.Size = New System.Drawing.Size(476, 142)
        Me.tbAdnlFldsFtr.TabIndex = 2
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.btnFtrPin)
        Me.TabPage2.Controls.Add(Me.lblFtrTip)
        Me.TabPage2.Controls.Add(Me.vsfFtr)
        Me.TabPage2.Location = New System.Drawing.Point(4, 26)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(468, 112)
        Me.TabPage2.TabIndex = 0
        Me.TabPage2.Text = "Footer Info"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'btnFtrPin
        '
        Me.btnFtrPin.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFtrPin.BackgroundImage = CType(resources.GetObject("btnFtrPin.BackgroundImage"), System.Drawing.Image)
        Me.btnFtrPin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnFtrPin.ContextMenuStrip = Me.cntxt
        Me.btnFtrPin.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnFtrPin.ForeColor = System.Drawing.Color.Transparent
        Me.btnFtrPin.Location = New System.Drawing.Point(450, 3)
        Me.btnFtrPin.Name = "btnFtrPin"
        Me.btnFtrPin.Size = New System.Drawing.Size(15, 15)
        Me.btnFtrPin.TabIndex = 72
        Me.btnFtrPin.UseVisualStyleBackColor = True
        '
        'lblFtrTip
        '
        Me.lblFtrTip.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFtrTip.BackColor = System.Drawing.Color.Silver
        Me.lblFtrTip.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFtrTip.Image = Global.vLQMS.My.Resources.Resources.ToolTipInfo
        Me.lblFtrTip.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblFtrTip.Location = New System.Drawing.Point(1, 2)
        Me.lblFtrTip.Name = "lblFtrTip"
        Me.lblFtrTip.Size = New System.Drawing.Size(449, 22)
        Me.lblFtrTip.TabIndex = 73
        Me.lblFtrTip.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'vsfFtr
        '
        Me.vsfFtr.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfFtr.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfFtr.AutoResize = True
        Me.vsfFtr.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfFtr.ColumnInfo = "2,1,0,0,0,120,Columns:"
        Me.vsfFtr.ExtendLastCol = True
        Me.vsfFtr.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfFtr.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcrossOut
        Me.vsfFtr.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcrossOut
        Me.vsfFtr.Location = New System.Drawing.Point(4, 3)
        Me.vsfFtr.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfFtr.Name = "vsfFtr"
        Me.vsfFtr.Rows.DefaultSize = 24
        Me.vsfFtr.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfFtr.ShowCellLabels = True
        Me.vsfFtr.Size = New System.Drawing.Size(461, 0)
        Me.vsfFtr.StyleInfo = resources.GetString("vsfFtr.StyleInfo")
        Me.vsfFtr.TabIndex = 0
        Me.vsfFtr.Tag = "DSGR"
        '
        'lblTitle
        '
        Me.lblTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(53, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(106, Byte), Integer))
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.ForeColor = System.Drawing.Color.White
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1116, 30)
        Me.lblTitle.TabIndex = 66
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Timer1
        '
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
        Me.lblFrm.TabIndex = 575
        '
        'lblTotTip
        '
        Me.lblTotTip.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTotTip.BackColor = System.Drawing.Color.Silver
        Me.lblTotTip.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotTip.Image = Global.vLQMS.My.Resources.Resources.ToolTipInfo
        Me.lblTotTip.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblTotTip.Location = New System.Drawing.Point(632, 536)
        Me.lblTotTip.Name = "lblTotTip"
        Me.lblTotTip.Size = New System.Drawing.Size(454, 21)
        Me.lblTotTip.TabIndex = 577
        Me.lblTotTip.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'pnlToolTip
        '
        Me.pnlToolTip.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.pnlToolTip.BackgroundImage = CType(resources.GetObject("pnlToolTip.BackgroundImage"), System.Drawing.Image)
        Me.pnlToolTip.Controls.Add(Me.PictureBox3)
        Me.pnlToolTip.Controls.Add(Me.lblToolTip)
        Me.pnlToolTip.Location = New System.Drawing.Point(12, 582)
        Me.pnlToolTip.Name = "pnlToolTip"
        Me.pnlToolTip.Size = New System.Drawing.Size(22, 10)
        Me.pnlToolTip.TabIndex = 271
        Me.pnlToolTip.Visible = False
        '
        'PictureBox3
        '
        Me.PictureBox3.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.PictureBox3.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox3.BackgroundImage = CType(resources.GetObject("PictureBox3.BackgroundImage"), System.Drawing.Image)
        Me.PictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.PictureBox3.Location = New System.Drawing.Point(-380, -16)
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
        Me.lblToolTip.Size = New System.Drawing.Size(0, 1)
        Me.lblToolTip.TabIndex = 216
        Me.lblToolTip.Text = "Help Tips:"
        '
        'btnTotPin
        '
        Me.btnTotPin.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnTotPin.BackColor = System.Drawing.Color.Transparent
        Me.btnTotPin.BackgroundImage = CType(resources.GetObject("btnTotPin.BackgroundImage"), System.Drawing.Image)
        Me.btnTotPin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnTotPin.ContextMenuStrip = Me.cntxt
        Me.btnTotPin.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTotPin.ForeColor = System.Drawing.Color.Transparent
        Me.btnTotPin.Location = New System.Drawing.Point(1088, 536)
        Me.btnTotPin.Name = "btnTotPin"
        Me.btnTotPin.Size = New System.Drawing.Size(15, 15)
        Me.btnTotPin.TabIndex = 71
        Me.btnTotPin.Text = "PIN"
        Me.btnTotPin.UseVisualStyleBackColor = False
        '
        'btnDetPin
        '
        Me.btnDetPin.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDetPin.BackColor = System.Drawing.Color.Transparent
        Me.btnDetPin.BackgroundImage = CType(resources.GetObject("btnDetPin.BackgroundImage"), System.Drawing.Image)
        Me.btnDetPin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnDetPin.ContextMenuStrip = Me.cntxt
        Me.btnDetPin.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDetPin.ForeColor = System.Drawing.Color.Transparent
        Me.btnDetPin.Location = New System.Drawing.Point(1086, 356)
        Me.btnDetPin.Name = "btnDetPin"
        Me.btnDetPin.Size = New System.Drawing.Size(15, 15)
        Me.btnDetPin.TabIndex = 70
        Me.btnDetPin.Text = "PIN"
        Me.btnDetPin.UseVisualStyleBackColor = False
        '
        'vsfVwDet
        '
        Me.vsfVwDet.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfVwDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfVwDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfVwDet.ColumnInfo = "2,1,0,0,0,110,Columns:"
        Me.vsfVwDet.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.vsfVwDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfVwDet.Location = New System.Drawing.Point(6, 213)
        Me.vsfVwDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfVwDet.Name = "vsfVwDet"
        Me.vsfVwDet.Rows.DefaultSize = 22
        Me.vsfVwDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfVwDet.Size = New System.Drawing.Size(1099, 161)
        Me.vsfVwDet.StyleInfo = resources.GetString("vsfVwDet.StyleInfo")
        Me.vsfVwDet.TabIndex = 1
        Me.vsfVwDet.Tag = ""
        '
        'vsfAdj
        '
        Me.vsfAdj.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfAdj.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfAdj.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfAdj.ColumnInfo = "2,1,0,0,0,120,Columns:"
        Me.vsfAdj.ExtendLastCol = True
        Me.vsfAdj.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfAdj.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfAdj.Location = New System.Drawing.Point(27, 213)
        Me.vsfAdj.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfAdj.Name = "vsfAdj"
        Me.vsfAdj.Rows.DefaultSize = 24
        Me.vsfAdj.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfAdj.Size = New System.Drawing.Size(501, 57)
        Me.vsfAdj.StyleInfo = resources.GetString("vsfAdj.StyleInfo")
        Me.vsfAdj.TabIndex = 579
        Me.vsfAdj.Tag = ""
        Me.vsfAdj.Visible = False
        '
        'chkShowBills
        '
        Me.chkShowBills.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkShowBills.AutoSize = True
        Me.chkShowBills.Location = New System.Drawing.Point(5, 568)
        Me.chkShowBills.Name = "chkShowBills"
        Me.chkShowBills.Size = New System.Drawing.Size(121, 21)
        Me.chkShowBills.TabIndex = 580
        Me.chkShowBills.Text = "Show Bill Details"
        Me.chkShowBills.UseVisualStyleBackColor = True
        '
        'vsfView
        '
        Me.vsfView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfView.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfView.ExtendLastCol = True
        Me.vsfView.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfView.Location = New System.Drawing.Point(43, 255)
        Me.vsfView.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfView.Name = "vsfView"
        Me.vsfView.Rows.DefaultSize = 24
        Me.vsfView.Size = New System.Drawing.Size(573, 214)
        Me.vsfView.StyleInfo = resources.GetString("vsfView.StyleInfo")
        Me.vsfView.TabIndex = 5
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.DimGray
        Me.Panel1.Controls.Add(Me.lblTotCrValue)
        Me.Panel1.Controls.Add(Me.lblTotDrValue)
        Me.Panel1.Controls.Add(Me.lblCreditTotal)
        Me.Panel1.Controls.Add(Me.lblDebitTotal)
        Me.Panel1.Location = New System.Drawing.Point(6, 186)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(633, 23)
        Me.Panel1.TabIndex = 581
        '
        'lblTotCrValue
        '
        Me.lblTotCrValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblTotCrValue.ForeColor = System.Drawing.Color.White
        Me.lblTotCrValue.Location = New System.Drawing.Point(402, 1)
        Me.lblTotCrValue.Name = "lblTotCrValue"
        Me.lblTotCrValue.Size = New System.Drawing.Size(106, 21)
        Me.lblTotCrValue.TabIndex = 40
        Me.lblTotCrValue.Tag = ""
        Me.lblTotCrValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTotDrValue
        '
        Me.lblTotDrValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblTotDrValue.ForeColor = System.Drawing.Color.White
        Me.lblTotDrValue.Location = New System.Drawing.Point(86, 2)
        Me.lblTotDrValue.Name = "lblTotDrValue"
        Me.lblTotDrValue.Size = New System.Drawing.Size(107, 21)
        Me.lblTotDrValue.TabIndex = 39
        Me.lblTotDrValue.Tag = ""
        Me.lblTotDrValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblCreditTotal
        '
        Me.lblCreditTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblCreditTotal.ForeColor = System.Drawing.Color.White
        Me.lblCreditTotal.Location = New System.Drawing.Point(321, 1)
        Me.lblCreditTotal.Name = "lblCreditTotal"
        Me.lblCreditTotal.Size = New System.Drawing.Size(87, 21)
        Me.lblCreditTotal.TabIndex = 38
        Me.lblCreditTotal.Tag = ""
        Me.lblCreditTotal.Text = "Credit Total :"
        Me.lblCreditTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblDebitTotal
        '
        Me.lblDebitTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblDebitTotal.ForeColor = System.Drawing.Color.White
        Me.lblDebitTotal.Location = New System.Drawing.Point(6, 2)
        Me.lblDebitTotal.Name = "lblDebitTotal"
        Me.lblDebitTotal.Size = New System.Drawing.Size(87, 21)
        Me.lblDebitTotal.TabIndex = 37
        Me.lblDebitTotal.Tag = ""
        Me.lblDebitTotal.Text = "Debit Total :"
        Me.lblDebitTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtDummy2
        '
        Me.txtDummy2.Location = New System.Drawing.Point(583, 95)
        Me.txtDummy2.Name = "txtDummy2"
        Me.txtDummy2.Size = New System.Drawing.Size(28, 25)
        Me.txtDummy2.TabIndex = 1
        Me.txtDummy2.Visible = False
        '
        'txtDummy
        '
        Me.txtDummy.Location = New System.Drawing.Point(549, 95)
        Me.txtDummy.Name = "txtDummy"
        Me.txtDummy.Size = New System.Drawing.Size(28, 25)
        Me.txtDummy.TabIndex = 0
        Me.txtDummy.Visible = False
        '
        'srcTransaction
        '
        Me.srcTransaction.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.srcTransaction.BackColor = System.Drawing.Color.FromArgb(CType(CType(53, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(106, Byte), Integer))
        Me.srcTransaction.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.srcTransaction.Location = New System.Drawing.Point(685, -1)
        Me.srcTransaction.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.srcTransaction.Name = "srcTransaction"
        Me.srcTransaction.Size = New System.Drawing.Size(420, 28)
        Me.srcTransaction.TabIndex = 0
        Me.srcTransaction.Visible = False
        '
        'svtBtnCtrl
        '
        Me.svtBtnCtrl.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.svtBtnCtrl.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.svtBtnCtrl.CausesValidation = False
        Me.svtBtnCtrl.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.svtBtnCtrl.LayoutStyle = vLQMS.svtModule.typeLayoutStyles.Horizontal
        Me.svtBtnCtrl.Location = New System.Drawing.Point(6, 558)
        Me.svtBtnCtrl.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.svtBtnCtrl.Name = "svtBtnCtrl"
        Me.svtBtnCtrl.Size = New System.Drawing.Size(945, 34)
        Me.svtBtnCtrl.TabIndex = 4
        '
        'ucBtnTools
        '
        Me.ucBtnTools.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ucBtnTools.BackColor = System.Drawing.Color.Transparent
        Me.ucBtnTools.Location = New System.Drawing.Point(6, 381)
        Me.ucBtnTools.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.ucBtnTools.Name = "ucBtnTools"
        Me.ucBtnTools.Size = New System.Drawing.Size(612, 29)
        Me.ucBtnTools.TabIndex = 65
        '
        'frmTrnVch
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1116, 592)
        Me.Controls.Add(Me.txtDummy2)
        Me.Controls.Add(Me.txtDummy)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.chkShowBills)
        Me.Controls.Add(Me.vsfVwDet)
        Me.Controls.Add(Me.vsfAdj)
        Me.Controls.Add(Me.vsfView)
        Me.Controls.Add(Me.lblTotTip)
        Me.Controls.Add(Me.lblFrm)
        Me.Controls.Add(Me.tbAdnlFldsFtr)
        Me.Controls.Add(Me.pnlToolTip)
        Me.Controls.Add(Me.btnTotPin)
        Me.Controls.Add(Me.btnDetPin)
        Me.Controls.Add(Me.srcTransaction)
        Me.Controls.Add(Me.fraAttach)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.lblCancelled)
        Me.Controls.Add(Me.lblAuthorised)
        Me.Controls.Add(Me.fraMain)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.vsfTot)
        Me.Controls.Add(Me.tbAdnlFlds)
        Me.Controls.Add(Me.vsfOptions)
        Me.Controls.Add(Me.svtBtnCtrl)
        Me.Controls.Add(Me.ucBtnTools)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MinimumSize = New System.Drawing.Size(1130, 630)
        Me.Name = "frmTrnVch"
        Me.ShowIcon = False
        Me.Text = "frmTrnSO"
        CType(Me.vsfTot, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfOptions, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraMain.ResumeLayout(False)
        Me.fraMain.PerformLayout()
        Me.fraAttach.ResumeLayout(False)
        Me.tbAdnlFlds.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.cntxt.ResumeLayout(False)
        CType(Me.vsfHdr, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tbAdnlFldsFtr.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        CType(Me.vsfFtr, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlToolTip.ResumeLayout(False)
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfVwDet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfAdj, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents vsfTot As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents lblAuthorised As System.Windows.Forms.Label
    Friend WithEvents lblCancelled As System.Windows.Forms.Label
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
    Friend WithEvents fraAttach As System.Windows.Forms.Panel
    Friend WithEvents btnAttachClose As System.Windows.Forms.Button
    Friend WithEvents svtFileAttach As vLQMS.svtFileCtrl
    Public WithEvents tbAdnlFlds As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents vsfHdr As C1.Win.C1FlexGrid.C1FlexGrid
    Public WithEvents tbAdnlFldsFtr As System.Windows.Forms.TabControl
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents srcTransaction As vLQMS.ucSearch
    Friend WithEvents ucBtnTools As vLQMS.ucBtn
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Public WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Public WithEvents vsfFtr As C1.Win.C1FlexGrid.C1FlexGrid
    Public WithEvents svtBtnCtrl As vLQMS.btnCtrl
    Friend WithEvents btnHdrPin As System.Windows.Forms.Button
    Friend WithEvents btnFtrPin As System.Windows.Forms.Button
    Friend WithEvents btnDetPin As System.Windows.Forms.Button
    Friend WithEvents btnTotPin As System.Windows.Forms.Button
    Friend WithEvents cntxt As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveThisToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pnlToolTip As System.Windows.Forms.Panel
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents lblToolTip As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents cdlg As OpenFileDialog
    Public WithEvents lblFrm As Label
    Public WithEvents lblTitle As Label
    Friend WithEvents lblHdrTip As Label
    Friend WithEvents lblFtrTip As Label
    Friend WithEvents lblTotTip As Label
    Public WithEvents vsfVwDet As C1.Win.C1FlexGrid.C1FlexGrid
    Public WithEvents vsfAdj As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents chkShowBills As CheckBox
    Friend WithEvents vsfView As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents Panel1 As Panel
    Friend WithEvents lblCreditTotal As Label
    Friend WithEvents lblDebitTotal As Label
    Friend WithEvents lblTotCrValue As Label
    Friend WithEvents lblTotDrValue As Label
    Public WithEvents txtDummy2 As TextBox
    Public WithEvents txtDummy As TextBox
End Class
