<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmTrnNonPartyNonItem
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTrnNonPartyNonItem))
        Me.vsfTot = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
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
        Me.btnCopyFrom = New System.Windows.Forms.Button()
        Me.fraAttach = New System.Windows.Forms.Panel()
        Me.btnAttachClose = New System.Windows.Forms.Button()
        Me.svtFileAttach = New vLQMS.svtFileCtrl()
        Me.tbAdnlFlds = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.btnHdrPin = New System.Windows.Forms.Button()
        Me.cntxt = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SaveThisToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.vsfHdr = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.tbAdnlFldsFtr = New System.Windows.Forms.TabControl()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.btnFtrPin = New System.Windows.Forms.Button()
        Me.vsfFtr = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.btnDetPin = New System.Windows.Forms.Button()
        Me.btnTotPin = New System.Windows.Forms.Button()
        Me.pnlToolTip = New System.Windows.Forms.Panel()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.lblToolTip = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnImpXL = New C1.Win.C1Input.C1SplitButton()
        Me.DropDownItem1 = New C1.Win.C1Input.DropDownItem()
        Me.DropDownItem2 = New C1.Win.C1Input.DropDownItem()
        Me.cdlg = New System.Windows.Forms.OpenFileDialog()
        Me.lblFrm = New System.Windows.Forms.Label()
        Me.srcTransaction = New vLQMS.ucSearch()
        Me.svtBtnCtrl = New vLQMS.btnCtrl()
        Me.ucBtnTools = New vLQMS.ucBtn()
        CType(Me.vsfTot, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfOptions, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraAmm.SuspendLayout()
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
        Me.vsfTot.Location = New System.Drawing.Point(647, 401)
        Me.vsfTot.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfTot.Name = "vsfTot"
        Me.vsfTot.Rows.DefaultSize = 24
        Me.vsfTot.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfTot.Size = New System.Drawing.Size(475, 138)
        Me.vsfTot.StyleInfo = resources.GetString("vsfTot.StyleInfo")
        Me.vsfTot.TabIndex = 9
        Me.vsfTot.Tag = "DSGR"
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
        Me.vsfDet.Location = New System.Drawing.Point(12, 183)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(1109, 178)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 2
        Me.vsfDet.Tag = ""
        '
        'vsfView
        '
        Me.vsfView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfView.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfView.ExtendLastCol = True
        Me.vsfView.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfView.Location = New System.Drawing.Point(52, 207)
        Me.vsfView.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfView.Name = "vsfView"
        Me.vsfView.Rows.DefaultSize = 24
        Me.vsfView.Size = New System.Drawing.Size(591, 168)
        Me.vsfView.StyleInfo = resources.GetString("vsfView.StyleInfo")
        Me.vsfView.TabIndex = 1
        '
        'lblAuthorised
        '
        Me.lblAuthorised.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblAuthorised.AutoSize = True
        Me.lblAuthorised.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lblAuthorised.Font = New System.Drawing.Font("Segoe UI", 9.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAuthorised.ForeColor = System.Drawing.Color.Red
        Me.lblAuthorised.Location = New System.Drawing.Point(528, 482)
        Me.lblAuthorised.Name = "lblAuthorised"
        Me.lblAuthorised.Size = New System.Drawing.Size(90, 17)
        Me.lblAuthorised.TabIndex = 36
        Me.lblAuthorised.Text = "AUTHORISED"
        '
        'lblCancelled
        '
        Me.lblCancelled.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCancelled.AutoSize = True
        Me.lblCancelled.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCancelled.ForeColor = System.Drawing.Color.Red
        Me.lblCancelled.Location = New System.Drawing.Point(514, 435)
        Me.lblCancelled.Name = "lblCancelled"
        Me.lblCancelled.Size = New System.Drawing.Size(130, 30)
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
        Me.vsfOptions.Location = New System.Drawing.Point(553, 250)
        Me.vsfOptions.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfOptions.Name = "vsfOptions"
        Me.vsfOptions.Rows.DefaultSize = 24
        Me.vsfOptions.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfOptions.Size = New System.Drawing.Size(25, 49)
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
        Me.fraAmm.Location = New System.Drawing.Point(647, 366)
        Me.fraAmm.Name = "fraAmm"
        Me.fraAmm.Size = New System.Drawing.Size(475, 31)
        Me.fraAmm.TabIndex = 46
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
        Me.txtAmmNo.TabIndex = 42
        Me.txtAmmNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label3.ForeColor = System.Drawing.Color.Blue
        Me.Label3.Location = New System.Drawing.Point(289, 7)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 17)
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
        Me.dtAmmDate.TabIndex = 40
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label2.ForeColor = System.Drawing.Color.Red
        Me.Label2.Location = New System.Drawing.Point(106, 7)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(35, 17)
        Me.Label2.TabIndex = 39
        Me.Label2.Tag = "M"
        Me.Label2.Text = "Date"
        '
        'chkAmmendment
        '
        Me.chkAmmendment.AutoSize = True
        Me.chkAmmendment.Location = New System.Drawing.Point(9, 7)
        Me.chkAmmendment.Name = "chkAmmendment"
        Me.chkAmmendment.Size = New System.Drawing.Size(97, 21)
        Me.chkAmmendment.TabIndex = 0
        Me.chkAmmendment.Text = "Amendment"
        Me.chkAmmendment.UseVisualStyleBackColor = True
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
        Me.txtDocNo.BackColor = System.Drawing.SystemColors.Control
        Me.txtDocNo.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtDocNo.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDocNo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtDocNo.Location = New System.Drawing.Point(459, 6)
        Me.txtDocNo.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtDocNo.Multiline = True
        Me.txtDocNo.Name = "txtDocNo"
        Me.txtDocNo.ReadOnly = True
        Me.txtDocNo.Size = New System.Drawing.Size(128, 23)
        Me.txtDocNo.TabIndex = 564
        Me.txtDocNo.TabStop = False
        Me.txtDocNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblDocNo
        '
        Me.lblDocNo.Font = New System.Drawing.Font("Segoe UI", 9.75!)
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
        Me.lblDocDate.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblDocDate.ForeColor = System.Drawing.Color.Blue
        Me.lblDocDate.Location = New System.Drawing.Point(301, 8)
        Me.lblDocDate.Name = "lblDocDate"
        Me.lblDocDate.Size = New System.Drawing.Size(35, 17)
        Me.lblDocDate.TabIndex = 38
        Me.lblDocDate.Tag = "R"
        Me.lblDocDate.Text = "Date"
        '
        'txtDocID
        '
        Me.txtDocID.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtDocID.Location = New System.Drawing.Point(447, 38)
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
        Me.fraMain.Controls.Add(Me.btnCopyFrom)
        Me.fraMain.Controls.Add(Me.txtDocNo)
        Me.fraMain.Controls.Add(Me.txtNo)
        Me.fraMain.Controls.Add(Me.cboPfx)
        Me.fraMain.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.fraMain.Location = New System.Drawing.Point(12, 40)
        Me.fraMain.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.fraMain.Name = "fraMain"
        Me.fraMain.Size = New System.Drawing.Size(595, 137)
        Me.fraMain.TabIndex = 1
        '
        'btnCopyFrom
        '
        Me.btnCopyFrom.Location = New System.Drawing.Point(496, 36)
        Me.btnCopyFrom.Name = "btnCopyFrom"
        Me.btnCopyFrom.Size = New System.Drawing.Size(95, 27)
        Me.btnCopyFrom.TabIndex = 272
        Me.btnCopyFrom.Text = "Copy From"
        Me.btnCopyFrom.UseVisualStyleBackColor = True
        '
        'fraAttach
        '
        Me.fraAttach.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fraAttach.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.fraAttach.Controls.Add(Me.btnAttachClose)
        Me.fraAttach.Controls.Add(Me.svtFileAttach)
        Me.fraAttach.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.fraAttach.Location = New System.Drawing.Point(685, 195)
        Me.fraAttach.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.fraAttach.Name = "fraAttach"
        Me.fraAttach.Size = New System.Drawing.Size(265, 157)
        Me.fraAttach.TabIndex = 48
        Me.fraAttach.Visible = False
        '
        'btnAttachClose
        '
        Me.btnAttachClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAttachClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnAttachClose.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnAttachClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAttachClose.Location = New System.Drawing.Point(2, 122)
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
        Me.svtFileAttach.Location = New System.Drawing.Point(4, 5)
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
        Me.tbAdnlFlds.Location = New System.Drawing.Point(613, 40)
        Me.tbAdnlFlds.Name = "tbAdnlFlds"
        Me.tbAdnlFlds.SelectedIndex = 0
        Me.tbAdnlFlds.Size = New System.Drawing.Size(508, 136)
        Me.tbAdnlFlds.TabIndex = 61
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.btnHdrPin)
        Me.TabPage1.Controls.Add(Me.vsfHdr)
        Me.TabPage1.Location = New System.Drawing.Point(4, 26)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(500, 106)
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
        Me.btnHdrPin.Location = New System.Drawing.Point(462, 37)
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
        Me.vsfHdr.Size = New System.Drawing.Size(494, 100)
        Me.vsfHdr.StyleInfo = resources.GetString("vsfHdr.StyleInfo")
        Me.vsfHdr.TabIndex = 2
        Me.vsfHdr.Tag = "DSGR"
        '
        'tbAdnlFldsFtr
        '
        Me.tbAdnlFldsFtr.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbAdnlFldsFtr.Controls.Add(Me.TabPage2)
        Me.tbAdnlFldsFtr.Location = New System.Drawing.Point(12, 398)
        Me.tbAdnlFldsFtr.Name = "tbAdnlFldsFtr"
        Me.tbAdnlFldsFtr.SelectedIndex = 0
        Me.tbAdnlFldsFtr.Size = New System.Drawing.Size(496, 138)
        Me.tbAdnlFldsFtr.TabIndex = 62
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.btnFtrPin)
        Me.TabPage2.Controls.Add(Me.vsfFtr)
        Me.TabPage2.Location = New System.Drawing.Point(4, 26)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(488, 108)
        Me.TabPage2.TabIndex = 0
        Me.TabPage2.Text = "Footer Info"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'btnFtrPin
        '
        Me.btnFtrPin.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFtrPin.BackgroundImage = Global.vLQMS.My.Resources.Resources.GridPin
        Me.btnFtrPin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnFtrPin.ContextMenuStrip = Me.cntxt
        Me.btnFtrPin.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnFtrPin.ForeColor = System.Drawing.Color.Transparent
        Me.btnFtrPin.Location = New System.Drawing.Point(445, 38)
        Me.btnFtrPin.Name = "btnFtrPin"
        Me.btnFtrPin.Size = New System.Drawing.Size(15, 15)
        Me.btnFtrPin.TabIndex = 72
        Me.btnFtrPin.Text = "PIN"
        Me.btnFtrPin.UseVisualStyleBackColor = True
        '
        'vsfFtr
        '
        Me.vsfFtr.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfFtr.AutoResize = True
        Me.vsfFtr.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfFtr.ColumnInfo = "2,1,0,0,0,120,Columns:"
        Me.vsfFtr.Dock = System.Windows.Forms.DockStyle.Fill
        Me.vsfFtr.ExtendLastCol = True
        Me.vsfFtr.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfFtr.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcrossOut
        Me.vsfFtr.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcrossOut
        Me.vsfFtr.Location = New System.Drawing.Point(3, 3)
        Me.vsfFtr.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfFtr.Name = "vsfFtr"
        Me.vsfFtr.Rows.DefaultSize = 24
        Me.vsfFtr.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfFtr.ShowCellLabels = True
        Me.vsfFtr.Size = New System.Drawing.Size(482, 102)
        Me.vsfFtr.StyleInfo = resources.GetString("vsfFtr.StyleInfo")
        Me.vsfFtr.TabIndex = 8
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
        Me.lblTitle.Size = New System.Drawing.Size(1133, 30)
        Me.lblTitle.TabIndex = 66
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Timer1
        '
        '
        'btnDetPin
        '
        Me.btnDetPin.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDetPin.BackColor = System.Drawing.Color.Transparent
        Me.btnDetPin.BackgroundImage = Global.vLQMS.My.Resources.Resources.GridPin
        Me.btnDetPin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnDetPin.ContextMenuStrip = Me.cntxt
        Me.btnDetPin.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDetPin.ForeColor = System.Drawing.Color.Transparent
        Me.btnDetPin.Location = New System.Drawing.Point(1083, 341)
        Me.btnDetPin.Name = "btnDetPin"
        Me.btnDetPin.Size = New System.Drawing.Size(15, 15)
        Me.btnDetPin.TabIndex = 70
        Me.btnDetPin.Text = "PIN"
        Me.btnDetPin.UseVisualStyleBackColor = False
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
        Me.btnTotPin.Location = New System.Drawing.Point(1083, 524)
        Me.btnTotPin.Name = "btnTotPin"
        Me.btnTotPin.Size = New System.Drawing.Size(15, 15)
        Me.btnTotPin.TabIndex = 71
        Me.btnTotPin.Text = "PIN"
        Me.btnTotPin.UseVisualStyleBackColor = False
        '
        'pnlToolTip
        '
        Me.pnlToolTip.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.pnlToolTip.BackgroundImage = CType(resources.GetObject("pnlToolTip.BackgroundImage"), System.Drawing.Image)
        Me.pnlToolTip.Controls.Add(Me.PictureBox3)
        Me.pnlToolTip.Controls.Add(Me.lblToolTip)
        Me.pnlToolTip.Location = New System.Drawing.Point(12, 540)
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
        Me.lblToolTip.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblToolTip.ForeColor = System.Drawing.Color.White
        Me.lblToolTip.Location = New System.Drawing.Point(44, 3)
        Me.lblToolTip.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblToolTip.Name = "lblToolTip"
        Me.lblToolTip.Size = New System.Drawing.Size(732, 30)
        Me.lblToolTip.TabIndex = 216
        Me.lblToolTip.Text = "Help Tips:"
        '
        'btnImpXL
        '
        Me.btnImpXL.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnImpXL.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnImpXL.Items.Add(Me.DropDownItem1)
        Me.btnImpXL.Items.Add(Me.DropDownItem2)
        Me.btnImpXL.Location = New System.Drawing.Point(624, 477)
        Me.btnImpXL.Name = "btnImpXL"
        Me.btnImpXL.Size = New System.Drawing.Size(133, 29)
        Me.btnImpXL.TabIndex = 574
        Me.btnImpXL.Text = "Import from XL"
        Me.ToolTip1.SetToolTip(Me.btnImpXL, "Hold Ctrl + Click for Mouser related")
        Me.btnImpXL.UseVisualStyleBackColor = True
        '
        'DropDownItem1
        '
        Me.DropDownItem1.Text = "Export Template"
        '
        'DropDownItem2
        '
        Me.DropDownItem2.Text = "Import from XL"
        '
        'cdlg
        '
        Me.cdlg.FileName = "OpenFileDialog1"
        '
        'lblFrm
        '
        Me.lblFrm.AutoSize = True
        Me.lblFrm.Font = New System.Drawing.Font("Bookman Old Style", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFrm.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.lblFrm.Location = New System.Drawing.Point(8, 5)
        Me.lblFrm.Name = "lblFrm"
        Me.lblFrm.Size = New System.Drawing.Size(0, 22)
        Me.lblFrm.TabIndex = 575
        '
        'srcTransaction
        '
        Me.srcTransaction.BackColor = System.Drawing.Color.FromArgb(CType(CType(53, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(106, Byte), Integer))
        Me.srcTransaction.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.srcTransaction.Location = New System.Drawing.Point(687, 0)
        Me.srcTransaction.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.srcTransaction.Name = "srcTransaction"
        Me.srcTransaction.Size = New System.Drawing.Size(408, 28)
        Me.srcTransaction.TabIndex = 64
        Me.srcTransaction.Visible = False
        '
        'svtBtnCtrl
        '
        Me.svtBtnCtrl.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.svtBtnCtrl.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.svtBtnCtrl.CausesValidation = False
        Me.svtBtnCtrl.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.svtBtnCtrl.LayoutStyle = vLQMS.svtModule.typeLayoutStyles.Horizontal
        Me.svtBtnCtrl.Location = New System.Drawing.Point(322, 540)
        Me.svtBtnCtrl.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.svtBtnCtrl.Name = "svtBtnCtrl"
        Me.svtBtnCtrl.Size = New System.Drawing.Size(801, 34)
        Me.svtBtnCtrl.TabIndex = 0
        '
        'ucBtnTools
        '
        Me.ucBtnTools.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ucBtnTools.Location = New System.Drawing.Point(12, 364)
        Me.ucBtnTools.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.ucBtnTools.Name = "ucBtnTools"
        Me.ucBtnTools.Size = New System.Drawing.Size(605, 31)
        Me.ucBtnTools.TabIndex = 65
        '
        'frmTrnNonPartyNonItem
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1133, 576)
        Me.Controls.Add(Me.lblFrm)
        Me.Controls.Add(Me.btnImpXL)
        Me.Controls.Add(Me.tbAdnlFldsFtr)
        Me.Controls.Add(Me.pnlToolTip)
        Me.Controls.Add(Me.vsfView)
        Me.Controls.Add(Me.btnTotPin)
        Me.Controls.Add(Me.btnDetPin)
        Me.Controls.Add(Me.srcTransaction)
        Me.Controls.Add(Me.fraAttach)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.lblCancelled)
        Me.Controls.Add(Me.lblAuthorised)
        Me.Controls.Add(Me.fraMain)
        Me.Controls.Add(Me.svtBtnCtrl)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.fraAmm)
        Me.Controls.Add(Me.vsfTot)
        Me.Controls.Add(Me.tbAdnlFlds)
        Me.Controls.Add(Me.vsfOptions)
        Me.Controls.Add(Me.ucBtnTools)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "frmTrnNonPartyNonItem"
        Me.ShowIcon = False
        Me.Text = "Non Party Non Item Detail"
        CType(Me.vsfTot, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfOptions, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraAmm.ResumeLayout(False)
        Me.fraAmm.PerformLayout()
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
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents vsfTot As C1.Win.C1FlexGrid.C1FlexGrid
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
    Friend WithEvents btnCopyFrom As Button
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents btnImpXL As C1.Win.C1Input.C1SplitButton
    Friend WithEvents cdlg As OpenFileDialog
    Friend WithEvents DropDownItem1 As C1.Win.C1Input.DropDownItem
    Friend WithEvents DropDownItem2 As C1.Win.C1Input.DropDownItem
    Public WithEvents lblFrm As Label
    Public WithEvents lblTitle As Label
End Class
