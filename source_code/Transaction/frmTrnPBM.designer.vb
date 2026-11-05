<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmTrnPBM
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTrnPBM))
        Me.vsfView = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.lblAuthorised = New System.Windows.Forms.Label()
        Me.lblCancelled = New System.Windows.Forms.Label()
        Me.vsfOptions = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.fraAmm = New System.Windows.Forms.Panel()
        Me.txtAmmNo = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.dtAmmDate = New System.Windows.Forms.DateTimePicker()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.chkAmmendment = New System.Windows.Forms.CheckBox()
        Me.cboPfx = New System.Windows.Forms.ComboBox()
        Me.txtNo = New System.Windows.Forms.TextBox()
        Me.txtDocNo = New System.Windows.Forms.TextBox()
        Me.lblDocNo = New System.Windows.Forms.Label()
        Me.dtDocDate = New System.Windows.Forms.DateTimePicker()
        Me.lblDocDate = New System.Windows.Forms.Label()
        Me.txtDocID = New System.Windows.Forms.TextBox()
        Me.fraMain = New System.Windows.Forms.Panel()
        Me.txtPrintNo = New System.Windows.Forms.TextBox()
        Me.btnCopyFrom = New System.Windows.Forms.Button()
        Me.fraAttach = New System.Windows.Forms.Panel()
        Me.btnAttachClose = New System.Windows.Forms.Button()
        Me.svtFileAttach = New vLQMS.svtFileCtrl()
        Me.tbAdnlFlds = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.vsfHdr = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.lblHdrTip = New System.Windows.Forms.Label()
        Me.btnHdrPin = New System.Windows.Forms.Button()
        Me.cntxt = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveThisToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tbAdnlFldsFtr = New System.Windows.Forms.TabControl()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.vsfFtr = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnFtrPin = New System.Windows.Forms.Button()
        Me.lblFtrTip = New System.Windows.Forms.Label()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cdlg = New System.Windows.Forms.OpenFileDialog()
        Me.lblFrm = New System.Windows.Forms.Label()
        Me.vsfTop = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnPendingDoc = New System.Windows.Forms.Button()
        Me.txtDummy = New System.Windows.Forms.TextBox()
        Me.txtDummy2 = New System.Windows.Forms.TextBox()
        Me.pnlBalance = New System.Windows.Forms.Panel()
        Me.lblPBValue = New System.Windows.Forms.Label()
        Me.txtBalance = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblTotTip = New System.Windows.Forms.Label()
        Me.pnlToolTip = New System.Windows.Forms.Panel()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.lblToolTip = New System.Windows.Forms.Label()
        Me.btnTotPin = New System.Windows.Forms.Button()
        Me.btnDetPin = New System.Windows.Forms.Button()
        Me.btnPB = New System.Windows.Forms.Button()
        Me.vsfTot = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.lblType = New System.Windows.Forms.Label()
        Me.vsfTemp = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.svtPCtrl = New vLQMS.svtPartyCtrl()
        Me.srcTransaction = New vLQMS.ucSearch()
        Me.ucBtnTools = New vLQMS.ucBtn()
        Me.svtBtnCtrl = New vLQMS.btnCtrl()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfOptions, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraAmm.SuspendLayout()
        Me.fraMain.SuspendLayout()
        Me.fraAttach.SuspendLayout()
        Me.tbAdnlFlds.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.vsfHdr, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cntxt.SuspendLayout()
        Me.tbAdnlFldsFtr.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        CType(Me.vsfFtr, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfTop, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlBalance.SuspendLayout()
        Me.pnlToolTip.SuspendLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfTot, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfTemp, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
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
        Me.vsfView.Size = New System.Drawing.Size(579, 214)
        Me.vsfView.StyleInfo = resources.GetString("vsfView.StyleInfo")
        Me.vsfView.TabIndex = 6
        '
        'lblAuthorised
        '
        Me.lblAuthorised.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblAuthorised.AutoSize = True
        Me.lblAuthorised.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lblAuthorised.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle))
        Me.lblAuthorised.ForeColor = System.Drawing.Color.Red
        Me.lblAuthorised.Location = New System.Drawing.Point(486, 517)
        Me.lblAuthorised.Name = "lblAuthorised"
        Me.lblAuthorised.Size = New System.Drawing.Size(107, 16)
        Me.lblAuthorised.TabIndex = 14
        Me.lblAuthorised.Text = "AUTHORISED"
        '
        'lblCancelled
        '
        Me.lblCancelled.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCancelled.AutoSize = True
        Me.lblCancelled.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold)
        Me.lblCancelled.ForeColor = System.Drawing.Color.Red
        Me.lblCancelled.Location = New System.Drawing.Point(477, 488)
        Me.lblCancelled.Name = "lblCancelled"
        Me.lblCancelled.Size = New System.Drawing.Size(147, 25)
        Me.lblCancelled.TabIndex = 13
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
        Me.vsfOptions.Size = New System.Drawing.Size(6, 64)
        Me.vsfOptions.StyleInfo = resources.GetString("vsfOptions.StyleInfo")
        Me.vsfOptions.TabIndex = 38
        Me.vsfOptions.Tag = ""
        Me.vsfOptions.Visible = False
        '
        'fraAmm
        '
        Me.fraAmm.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fraAmm.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.fraAmm.Controls.Add(Me.txtAmmNo)
        Me.fraAmm.Controls.Add(Me.Label3)
        Me.fraAmm.Controls.Add(Me.dtAmmDate)
        Me.fraAmm.Controls.Add(Me.Label2)
        Me.fraAmm.Controls.Add(Me.chkAmmendment)
        Me.fraAmm.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.fraAmm.Location = New System.Drawing.Point(628, 377)
        Me.fraAmm.Name = "fraAmm"
        Me.fraAmm.Size = New System.Drawing.Size(475, 33)
        Me.fraAmm.TabIndex = 8
        '
        'txtAmmNo
        '
        Me.txtAmmNo.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtAmmNo.Location = New System.Drawing.Point(355, 2)
        Me.txtAmmNo.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtAmmNo.MaxLength = 50
        Me.txtAmmNo.Name = "txtAmmNo"
        Me.txtAmmNo.ReadOnly = True
        Me.txtAmmNo.Size = New System.Drawing.Size(110, 25)
        Me.txtAmmNo.TabIndex = 2
        Me.txtAmmNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Label3.ForeColor = System.Drawing.Color.Blue
        Me.Label3.Location = New System.Drawing.Point(289, 7)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 16)
        Me.Label3.TabIndex = 41
        Me.Label3.Text = "Number"
        '
        'dtAmmDate
        '
        Me.dtAmmDate.CustomFormat = "dd/MM/yyyy"
        Me.dtAmmDate.Enabled = False
        Me.dtAmmDate.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.dtAmmDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtAmmDate.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.dtAmmDate.Location = New System.Drawing.Point(164, 2)
        Me.dtAmmDate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtAmmDate.Name = "dtAmmDate"
        Me.dtAmmDate.Size = New System.Drawing.Size(120, 25)
        Me.dtAmmDate.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Label2.ForeColor = System.Drawing.Color.Red
        Me.Label2.Location = New System.Drawing.Point(106, 7)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(37, 16)
        Me.Label2.TabIndex = 1
        Me.Label2.Tag = "M"
        Me.Label2.Text = "Date"
        '
        'chkAmmendment
        '
        Me.chkAmmendment.AutoSize = True
        Me.chkAmmendment.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.chkAmmendment.Location = New System.Drawing.Point(7, 7)
        Me.chkAmmendment.Name = "chkAmmendment"
        Me.chkAmmendment.Size = New System.Drawing.Size(99, 20)
        Me.chkAmmendment.TabIndex = 0
        Me.chkAmmendment.Text = "Amendment"
        Me.chkAmmendment.UseVisualStyleBackColor = True
        '
        'cboPfx
        '
        Me.cboPfx.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPfx.FormattingEnabled = True
        Me.cboPfx.Location = New System.Drawing.Point(112, 4)
        Me.cboPfx.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.cboPfx.Name = "cboPfx"
        Me.cboPfx.Size = New System.Drawing.Size(82, 21)
        Me.cboPfx.TabIndex = 0
        '
        'txtNo
        '
        Me.txtNo.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtNo.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNo.Location = New System.Drawing.Point(199, 5)
        Me.txtNo.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtNo.MaxLength = 12
        Me.txtNo.Multiline = True
        Me.txtNo.Name = "txtNo"
        Me.txtNo.Size = New System.Drawing.Size(82, 20)
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
        Me.txtDocNo.Size = New System.Drawing.Size(35, 18)
        Me.txtDocNo.TabIndex = 564
        Me.txtDocNo.TabStop = False
        Me.txtDocNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtDocNo.Visible = False
        '
        'lblDocNo
        '
        Me.lblDocNo.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDocNo.ForeColor = System.Drawing.Color.Blue
        Me.lblDocNo.Location = New System.Drawing.Point(4, 5)
        Me.lblDocNo.Name = "lblDocNo"
        Me.lblDocNo.Size = New System.Drawing.Size(102, 21)
        Me.lblDocNo.TabIndex = 36
        Me.lblDocNo.Tag = "R"
        Me.lblDocNo.Text = "Doc No"
        Me.lblDocNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dtDocDate
        '
        Me.dtDocDate.CalendarFont = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.dtDocDate.CustomFormat = "dd/MM/yyyy"
        Me.dtDocDate.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtDocDate.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.dtDocDate.Location = New System.Drawing.Point(329, 4)
        Me.dtDocDate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtDocDate.Name = "dtDocDate"
        Me.dtDocDate.Size = New System.Drawing.Size(115, 23)
        Me.dtDocDate.TabIndex = 3
        '
        'lblDocDate
        '
        Me.lblDocDate.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDocDate.ForeColor = System.Drawing.Color.Blue
        Me.lblDocDate.Location = New System.Drawing.Point(288, 6)
        Me.lblDocDate.Name = "lblDocDate"
        Me.lblDocDate.Size = New System.Drawing.Size(52, 20)
        Me.lblDocDate.TabIndex = 2
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
        Me.txtDocID.TabIndex = 3
        Me.txtDocID.Visible = False
        '
        'fraMain
        '
        Me.fraMain.BackColor = System.Drawing.Color.Transparent
        Me.fraMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.fraMain.Controls.Add(Me.dtDocDate)
        Me.fraMain.Controls.Add(Me.txtPrintNo)
        Me.fraMain.Controls.Add(Me.lblDocDate)
        Me.fraMain.Controls.Add(Me.txtDocID)
        Me.fraMain.Controls.Add(Me.lblDocNo)
        Me.fraMain.Controls.Add(Me.btnCopyFrom)
        Me.fraMain.Controls.Add(Me.txtDocNo)
        Me.fraMain.Controls.Add(Me.txtNo)
        Me.fraMain.Controls.Add(Me.cboPfx)
        Me.fraMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.fraMain.Location = New System.Drawing.Point(6, 33)
        Me.fraMain.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.fraMain.Name = "fraMain"
        Me.fraMain.Size = New System.Drawing.Size(631, 132)
        Me.fraMain.TabIndex = 2
        '
        'txtPrintNo
        '
        Me.txtPrintNo.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.txtPrintNo.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtPrintNo.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPrintNo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.txtPrintNo.Location = New System.Drawing.Point(450, 7)
        Me.txtPrintNo.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtPrintNo.Multiline = True
        Me.txtPrintNo.Name = "txtPrintNo"
        Me.txtPrintNo.ReadOnly = True
        Me.txtPrintNo.Size = New System.Drawing.Size(172, 18)
        Me.txtPrintNo.TabIndex = 3
        Me.txtPrintNo.TabStop = False
        Me.txtPrintNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnCopyFrom
        '
        Me.btnCopyFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnCopyFrom.Location = New System.Drawing.Point(257, 70)
        Me.btnCopyFrom.Name = "btnCopyFrom"
        Me.btnCopyFrom.Size = New System.Drawing.Size(65, 27)
        Me.btnCopyFrom.TabIndex = 272
        Me.btnCopyFrom.Text = "Copy From"
        Me.btnCopyFrom.UseVisualStyleBackColor = True
        Me.btnCopyFrom.Visible = False
        '
        'fraAttach
        '
        Me.fraAttach.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fraAttach.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.fraAttach.Controls.Add(Me.btnAttachClose)
        Me.fraAttach.Controls.Add(Me.svtFileAttach)
        Me.fraAttach.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.fraAttach.Location = New System.Drawing.Point(24, 568)
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
        Me.btnAttachClose.TabIndex = 0
        Me.btnAttachClose.Text = "&Close"
        Me.btnAttachClose.UseVisualStyleBackColor = False
        Me.btnAttachClose.Visible = False
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
        Me.tbAdnlFlds.Size = New System.Drawing.Size(463, 132)
        Me.tbAdnlFlds.TabIndex = 3
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage1.Controls.Add(Me.vsfHdr)
        Me.TabPage1.Controls.Add(Me.lblHdrTip)
        Me.TabPage1.Controls.Add(Me.btnHdrPin)
        Me.TabPage1.ForeColor = System.Drawing.Color.Black
        Me.TabPage1.Location = New System.Drawing.Point(4, 26)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(455, 102)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Header Info"
        '
        'vsfHdr
        '
        Me.vsfHdr.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfHdr.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromCursor
        Me.vsfHdr.BackColor = System.Drawing.Color.LightGray
        Me.vsfHdr.ColumnInfo = "2,0,0,0,0,130,Columns:1{Style:""Margins:1, 0, 0, 0;TextEffect:Flat;TextDirection:N" &
    "ormal;Trimming:None;"";}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfHdr.ExtendLastCol = True
        Me.vsfHdr.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.vsfHdr.ForeColor = System.Drawing.Color.White
        Me.vsfHdr.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus
        Me.vsfHdr.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
        Me.vsfHdr.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfHdr.Location = New System.Drawing.Point(0, -3)
        Me.vsfHdr.Margin = New System.Windows.Forms.Padding(4)
        Me.vsfHdr.Name = "vsfHdr"
        Me.vsfHdr.Rows.Count = 4
        Me.vsfHdr.Rows.DefaultSize = 26
        Me.vsfHdr.Size = New System.Drawing.Size(455, 81)
        Me.vsfHdr.StyleInfo = resources.GetString("vsfHdr.StyleInfo")
        Me.vsfHdr.TabIndex = 0
        Me.vsfHdr.Tag = "DSGR"
        '
        'lblHdrTip
        '
        Me.lblHdrTip.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblHdrTip.BackColor = System.Drawing.Color.Silver
        Me.lblHdrTip.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHdrTip.Image = Global.vLQMS.My.Resources.Resources.ToolTipInfo
        Me.lblHdrTip.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblHdrTip.Location = New System.Drawing.Point(-1, -18)
        Me.lblHdrTip.Name = "lblHdrTip"
        Me.lblHdrTip.Size = New System.Drawing.Size(436, 35)
        Me.lblHdrTip.TabIndex = 70
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
        Me.btnHdrPin.Location = New System.Drawing.Point(436, -255)
        Me.btnHdrPin.Name = "btnHdrPin"
        Me.btnHdrPin.Size = New System.Drawing.Size(15, 15)
        Me.btnHdrPin.TabIndex = 69
        Me.btnHdrPin.Text = "PIN"
        Me.btnHdrPin.UseVisualStyleBackColor = True
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
        'tbAdnlFldsFtr
        '
        Me.tbAdnlFldsFtr.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbAdnlFldsFtr.Controls.Add(Me.TabPage2)
        Me.tbAdnlFldsFtr.Location = New System.Drawing.Point(5, 417)
        Me.tbAdnlFldsFtr.Name = "tbAdnlFldsFtr"
        Me.tbAdnlFldsFtr.SelectedIndex = 0
        Me.tbAdnlFldsFtr.Size = New System.Drawing.Size(471, 139)
        Me.tbAdnlFldsFtr.TabIndex = 9
        '
        'TabPage2
        '
        Me.TabPage2.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage2.Controls.Add(Me.vsfFtr)
        Me.TabPage2.Controls.Add(Me.btnFtrPin)
        Me.TabPage2.Controls.Add(Me.lblFtrTip)
        Me.TabPage2.Location = New System.Drawing.Point(4, 26)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(463, 109)
        Me.TabPage2.TabIndex = 0
        Me.TabPage2.Text = "Footer Info"
        '
        'vsfFtr
        '
        Me.vsfFtr.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfFtr.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromCursor
        Me.vsfFtr.BackColor = System.Drawing.SystemColors.Control
        Me.vsfFtr.ColumnInfo = "2,0,0,0,0,130,Columns:1{Style:""Margins:1, 0, 0, 0;TextEffect:Flat;TextDirection:N" &
    "ormal;Trimming:None;"";}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfFtr.ExtendLastCol = True
        Me.vsfFtr.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.vsfFtr.ForeColor = System.Drawing.Color.White
        Me.vsfFtr.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus
        Me.vsfFtr.Location = New System.Drawing.Point(1, -29)
        Me.vsfFtr.Margin = New System.Windows.Forms.Padding(4)
        Me.vsfFtr.Name = "vsfFtr"
        Me.vsfFtr.Rows.Count = 4
        Me.vsfFtr.Rows.DefaultSize = 26
        Me.vsfFtr.Size = New System.Drawing.Size(463, 71)
        Me.vsfFtr.StyleInfo = resources.GetString("vsfFtr.StyleInfo")
        Me.vsfFtr.TabIndex = 585
        Me.vsfFtr.Tag = "DSGR"
        '
        'btnFtrPin
        '
        Me.btnFtrPin.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFtrPin.BackgroundImage = CType(resources.GetObject("btnFtrPin.BackgroundImage"), System.Drawing.Image)
        Me.btnFtrPin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnFtrPin.ContextMenuStrip = Me.cntxt
        Me.btnFtrPin.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnFtrPin.ForeColor = System.Drawing.Color.Transparent
        Me.btnFtrPin.Location = New System.Drawing.Point(445, -247)
        Me.btnFtrPin.Name = "btnFtrPin"
        Me.btnFtrPin.Size = New System.Drawing.Size(15, 17)
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
        Me.lblFtrTip.Location = New System.Drawing.Point(3, 33)
        Me.lblFtrTip.Name = "lblFtrTip"
        Me.lblFtrTip.Size = New System.Drawing.Size(444, 27)
        Me.lblFtrTip.TabIndex = 73
        Me.lblFtrTip.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblTitle
        '
        Me.lblTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(53, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(106, Byte), Integer))
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.ForeColor = System.Drawing.Color.White
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1114, 30)
        Me.lblTitle.TabIndex = 0
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
        'vsfTop
        '
        Me.vsfTop.AllowEditing = False
        Me.vsfTop.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfTop.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfTop.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.vsfTop.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None
        Me.vsfTop.ColumnInfo = "2,1,0,0,0,125,Columns:"
        Me.vsfTop.Font = New System.Drawing.Font("Segoe UI", 10.2!)
        Me.vsfTop.ForeColor = System.Drawing.Color.White
        Me.vsfTop.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
        Me.vsfTop.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfTop.Location = New System.Drawing.Point(6, 176)
        Me.vsfTop.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfTop.Name = "vsfTop"
        Me.vsfTop.Rows.DefaultSize = 25
        Me.vsfTop.Rows.Fixed = 0
        Me.vsfTop.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.vsfTop.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfTop.ShowButtons = C1.Win.C1FlexGrid.ShowButtonsEnum.Always
        Me.vsfTop.Size = New System.Drawing.Size(1097, 27)
        Me.vsfTop.StyleInfo = resources.GetString("vsfTop.StyleInfo")
        Me.vsfTop.TabIndex = 4
        Me.vsfTop.Tag = "NONSTYLE"
        '
        'btnPendingDoc
        '
        Me.btnPendingDoc.Location = New System.Drawing.Point(464, 65)
        Me.btnPendingDoc.Name = "btnPendingDoc"
        Me.btnPendingDoc.Size = New System.Drawing.Size(164, 27)
        Me.btnPendingDoc.TabIndex = 17
        Me.btnPendingDoc.Text = "Load Pending"
        Me.btnPendingDoc.UseVisualStyleBackColor = True
        Me.btnPendingDoc.Visible = False
        '
        'txtDummy
        '
        Me.txtDummy.Location = New System.Drawing.Point(957, 568)
        Me.txtDummy.Name = "txtDummy"
        Me.txtDummy.Size = New System.Drawing.Size(28, 25)
        Me.txtDummy.TabIndex = 579
        Me.txtDummy.Visible = False
        '
        'txtDummy2
        '
        Me.txtDummy2.Location = New System.Drawing.Point(991, 568)
        Me.txtDummy2.Name = "txtDummy2"
        Me.txtDummy2.Size = New System.Drawing.Size(28, 25)
        Me.txtDummy2.TabIndex = 580
        Me.txtDummy2.Visible = False
        '
        'pnlBalance
        '
        Me.pnlBalance.BackColor = System.Drawing.Color.Gray
        Me.pnlBalance.Controls.Add(Me.lblPBValue)
        Me.pnlBalance.Controls.Add(Me.txtBalance)
        Me.pnlBalance.Controls.Add(Me.Label1)
        Me.pnlBalance.ForeColor = System.Drawing.Color.Black
        Me.pnlBalance.Location = New System.Drawing.Point(434, 90)
        Me.pnlBalance.Name = "pnlBalance"
        Me.pnlBalance.Size = New System.Drawing.Size(200, 76)
        Me.pnlBalance.TabIndex = 18
        '
        'lblPBValue
        '
        Me.lblPBValue.AutoSize = True
        Me.lblPBValue.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPBValue.ForeColor = System.Drawing.Color.White
        Me.lblPBValue.Location = New System.Drawing.Point(72, 1)
        Me.lblPBValue.Name = "lblPBValue"
        Me.lblPBValue.Size = New System.Drawing.Size(62, 17)
        Me.lblPBValue.TabIndex = 0
        Me.lblPBValue.Tag = ""
        Me.lblPBValue.Text = "PB Value"
        '
        'txtBalance
        '
        Me.txtBalance.BackColor = System.Drawing.Color.Lime
        Me.txtBalance.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBalance.ForeColor = System.Drawing.Color.Red
        Me.txtBalance.Location = New System.Drawing.Point(3, 30)
        Me.txtBalance.Multiline = True
        Me.txtBalance.Name = "txtBalance"
        Me.txtBalance.ReadOnly = True
        Me.txtBalance.Size = New System.Drawing.Size(195, 44)
        Me.txtBalance.TabIndex = 2
        Me.txtBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(55, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(99, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Tag = ""
        Me.Label1.Text = "[Balance to Enter]"
        '
        'lblTotTip
        '
        Me.lblTotTip.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTotTip.BackColor = System.Drawing.Color.Silver
        Me.lblTotTip.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotTip.Image = Global.vLQMS.My.Resources.Resources.ToolTipInfo
        Me.lblTotTip.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblTotTip.Location = New System.Drawing.Point(630, 528)
        Me.lblTotTip.Name = "lblTotTip"
        Me.lblTotTip.Size = New System.Drawing.Size(454, 27)
        Me.lblTotTip.TabIndex = 11
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
        Me.pnlToolTip.TabIndex = 15
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
        Me.lblToolTip.TabIndex = 0
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
        Me.btnTotPin.Location = New System.Drawing.Point(1086, 543)
        Me.btnTotPin.Name = "btnTotPin"
        Me.btnTotPin.Size = New System.Drawing.Size(15, 13)
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
        Me.btnDetPin.Location = New System.Drawing.Point(1084, 356)
        Me.btnDetPin.Name = "btnDetPin"
        Me.btnDetPin.Size = New System.Drawing.Size(15, 15)
        Me.btnDetPin.TabIndex = 70
        Me.btnDetPin.Text = "PIN"
        Me.btnDetPin.UseVisualStyleBackColor = False
        '
        'btnPB
        '
        Me.btnPB.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPB.Location = New System.Drawing.Point(485, 413)
        Me.btnPB.Name = "btnPB"
        Me.btnPB.Size = New System.Drawing.Size(141, 27)
        Me.btnPB.TabIndex = 583
        Me.btnPB.Text = "Load Purchase Indent"
        Me.btnPB.UseVisualStyleBackColor = True
        '
        'vsfTot
        '
        Me.vsfTot.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfTot.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromCursor
        Me.vsfTot.BackColor = System.Drawing.SystemColors.Control
        Me.vsfTot.ColumnInfo = "2,0,0,0,0,130,Columns:1{Style:""Margins:1, 0, 0, 0;TextEffect:Flat;TextDirection:N" &
    "ormal;Trimming:None;"";}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfTot.ExtendLastCol = True
        Me.vsfTot.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.vsfTot.ForeColor = System.Drawing.Color.White
        Me.vsfTot.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus
        Me.vsfTot.Location = New System.Drawing.Point(630, 414)
        Me.vsfTot.Margin = New System.Windows.Forms.Padding(4)
        Me.vsfTot.Name = "vsfTot"
        Me.vsfTot.Rows.Count = 4
        Me.vsfTot.Rows.DefaultSize = 26
        Me.vsfTot.Size = New System.Drawing.Size(472, 110)
        Me.vsfTot.StyleInfo = resources.GetString("vsfTot.StyleInfo")
        Me.vsfTot.TabIndex = 10
        Me.vsfTot.Tag = "DSGR"
        '
        'vsfDet
        '
        Me.vsfDet.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.BackColor = System.Drawing.SystemColors.Control
        Me.vsfDet.ColumnInfo = "2,1,0,0,0,145,Columns:"
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 10.2!)
        Me.vsfDet.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(5, 201)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 25
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(1098, 167)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 5
        Me.vsfDet.Tag = ""
        '
        'lblType
        '
        Me.lblType.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblType.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.lblType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblType.ForeColor = System.Drawing.Color.Red
        Me.lblType.Location = New System.Drawing.Point(376, 378)
        Me.lblType.Name = "lblType"
        Me.lblType.Size = New System.Drawing.Size(246, 16)
        Me.lblType.TabIndex = 582
        Me.lblType.Text = "Press F4 to Add Single Item" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'vsfTemp
        '
        Me.vsfTemp.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfTemp.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfTemp.ExtendLastCol = True
        Me.vsfTemp.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfTemp.Location = New System.Drawing.Point(268, 189)
        Me.vsfTemp.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfTemp.Name = "vsfTemp"
        Me.vsfTemp.Rows.DefaultSize = 24
        Me.vsfTemp.Size = New System.Drawing.Size(109, 58)
        Me.vsfTemp.StyleInfo = resources.GetString("vsfTemp.StyleInfo")
        Me.vsfTemp.TabIndex = 584
        Me.vsfTemp.Visible = False
        '
        'svtPCtrl
        '
        Me.svtPCtrl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.svtPCtrl.BackColor = System.Drawing.Color.Transparent
        Me.svtPCtrl.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.svtPCtrl.ForeColor = System.Drawing.SystemColors.ControlText
        Me.svtPCtrl.Location = New System.Drawing.Point(10, 63)
        Me.svtPCtrl.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.svtPCtrl.Name = "svtPCtrl"
        Me.svtPCtrl.Size = New System.Drawing.Size(618, 95)
        Me.svtPCtrl.TabIndex = 16
        '
        'srcTransaction
        '
        Me.srcTransaction.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.srcTransaction.BackColor = System.Drawing.Color.FromArgb(CType(CType(53, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(106, Byte), Integer))
        Me.srcTransaction.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.srcTransaction.Location = New System.Drawing.Point(683, -1)
        Me.srcTransaction.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.srcTransaction.Name = "srcTransaction"
        Me.srcTransaction.Size = New System.Drawing.Size(420, 31)
        Me.srcTransaction.TabIndex = 1
        Me.srcTransaction.Visible = False
        '
        'ucBtnTools
        '
        Me.ucBtnTools.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ucBtnTools.BackColor = System.Drawing.Color.Transparent
        Me.ucBtnTools.Location = New System.Drawing.Point(6, 381)
        Me.ucBtnTools.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.ucBtnTools.Name = "ucBtnTools"
        Me.ucBtnTools.Size = New System.Drawing.Size(583, 29)
        Me.ucBtnTools.TabIndex = 7
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
        Me.svtBtnCtrl.TabIndex = 12
        '
        'frmTrnPBM
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1114, 592)
        Me.Controls.Add(Me.vsfTemp)
        Me.Controls.Add(Me.vsfView)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.vsfTot)
        Me.Controls.Add(Me.btnPB)
        Me.Controls.Add(Me.lblType)
        Me.Controls.Add(Me.pnlBalance)
        Me.Controls.Add(Me.btnPendingDoc)
        Me.Controls.Add(Me.txtDummy2)
        Me.Controls.Add(Me.txtDummy)
        Me.Controls.Add(Me.lblTotTip)
        Me.Controls.Add(Me.vsfTop)
        Me.Controls.Add(Me.svtPCtrl)
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
        Me.Controls.Add(Me.fraAmm)
        Me.Controls.Add(Me.tbAdnlFlds)
        Me.Controls.Add(Me.vsfOptions)
        Me.Controls.Add(Me.ucBtnTools)
        Me.Controls.Add(Me.svtBtnCtrl)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MinimumSize = New System.Drawing.Size(1130, 630)
        Me.Name = "frmTrnPBM"
        Me.ShowIcon = False
        Me.Text = "Purchase Bill"
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfOptions, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraAmm.ResumeLayout(False)
        Me.fraAmm.PerformLayout()
        Me.fraMain.ResumeLayout(False)
        Me.fraMain.PerformLayout()
        Me.fraAttach.ResumeLayout(False)
        Me.tbAdnlFlds.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        CType(Me.vsfHdr, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cntxt.ResumeLayout(False)
        Me.tbAdnlFldsFtr.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        CType(Me.vsfFtr, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfTop, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlBalance.ResumeLayout(False)
        Me.pnlBalance.PerformLayout()
        Me.pnlToolTip.ResumeLayout(False)
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfTot, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfTemp, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents vsfView As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents lblAuthorised As System.Windows.Forms.Label
    Friend WithEvents lblCancelled As System.Windows.Forms.Label
    Public WithEvents vsfOptions As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents tlToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents fraAmm As System.Windows.Forms.Panel
    Friend WithEvents chkAmmendment As System.Windows.Forms.CheckBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents txtAmmNo As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents dtAmmDate As System.Windows.Forms.DateTimePicker
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
    Public WithEvents tbAdnlFldsFtr As System.Windows.Forms.TabControl
    Friend WithEvents ucBtnTools As vLQMS.ucBtn
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Public WithEvents svtBtnCtrl As vLQMS.btnCtrl
    Friend WithEvents btnHdrPin As System.Windows.Forms.Button
    Friend WithEvents btnDetPin As System.Windows.Forms.Button
    Friend WithEvents btnTotPin As System.Windows.Forms.Button
    Friend WithEvents cntxt As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SaveThisToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pnlToolTip As System.Windows.Forms.Panel
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents lblToolTip As System.Windows.Forms.Label
    Friend WithEvents btnCopyFrom As Button
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents cdlg As OpenFileDialog
    Public WithEvents lblFrm As Label
    Public WithEvents lblTitle As Label
    Public WithEvents svtPCtrl As svtPartyCtrl
    Public WithEvents vsfTop As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents lblHdrTip As Label
    Friend WithEvents lblTotTip As Label
    Friend WithEvents btnPendingDoc As Button
    Public WithEvents txtDummy As TextBox
    Public WithEvents txtDummy2 As TextBox
    Public WithEvents srcTransaction As ucSearch
    Public WithEvents txtPrintNo As TextBox
    Friend WithEvents pnlBalance As Panel
    Friend WithEvents Label1 As Label
    Public WithEvents txtBalance As TextBox
    Friend WithEvents btnPB As Button
    Friend WithEvents vsfTot As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents btnFtrPin As Button
    Friend WithEvents lblFtrTip As Label
    Friend WithEvents lblPBValue As Label
    Friend WithEvents vsfFtr As C1.Win.C1FlexGrid.C1FlexGrid
    Public WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents lblType As Label
    Public WithEvents vsfHdr As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents vsfTemp As C1.Win.C1FlexGrid.C1FlexGrid
End Class
