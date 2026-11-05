<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlShortclose
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlShortclose))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cboDocName = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnLoadDocNo = New System.Windows.Forms.Button()
        Me.chkSelItemCode = New System.Windows.Forms.CheckBox()
        Me.chkSelParty = New System.Windows.Forms.CheckBox()
        Me.pnlDate = New System.Windows.Forms.Panel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.dtpToDate = New System.Windows.Forms.DateTimePicker()
        Me.dtpFromDate = New System.Windows.Forms.DateTimePicker()
        Me.chkDate = New System.Windows.Forms.CheckBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.rbBoth = New System.Windows.Forms.RadioButton()
        Me.rbPending = New System.Windows.Forms.RadioButton()
        Me.rbShortClosed = New System.Windows.Forms.RadioButton()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.cboDocNo = New System.Windows.Forms.ComboBox()
        Me.vsfSearch = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnExport = New C1.Win.C1Input.C1Button()
        Me.pnlEdit = New System.Windows.Forms.Panel()
        Me.chkSel = New System.Windows.Forms.CheckBox()
        Me.btnSave = New C1.Win.C1Input.C1Button()
        Me.btnCancel = New C1.Win.C1Input.C1Button()
        Me.vsfShort = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.pnlHdr = New System.Windows.Forms.Panel()
        Me.lblHelp = New System.Windows.Forms.Label()
        Me.tt = New System.Windows.Forms.ToolTip(Me.components)
        Me.pnlDate.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlEdit.SuspendLayout()
        CType(Me.vsfShort, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlHdr.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(9, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(83, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Document"
        '
        'cboDocName
        '
        Me.cboDocName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.cboDocName.FormattingEnabled = True
        Me.cboDocName.Location = New System.Drawing.Point(113, 11)
        Me.cboDocName.Name = "cboDocName"
        Me.cboDocName.Size = New System.Drawing.Size(256, 23)
        Me.cboDocName.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(9, 54)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(62, 20)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Doc No"
        '
        'btnLoadDocNo
        '
        Me.btnLoadDocNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.btnLoadDocNo.Location = New System.Drawing.Point(330, 51)
        Me.btnLoadDocNo.Name = "btnLoadDocNo"
        Me.btnLoadDocNo.Size = New System.Drawing.Size(40, 24)
        Me.btnLoadDocNo.TabIndex = 2
        Me.btnLoadDocNo.Text = "..."
        Me.btnLoadDocNo.UseVisualStyleBackColor = True
        '
        'chkSelItemCode
        '
        Me.chkSelItemCode.AutoSize = True
        Me.chkSelItemCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSelItemCode.Location = New System.Drawing.Point(406, 98)
        Me.chkSelItemCode.Margin = New System.Windows.Forms.Padding(5)
        Me.chkSelItemCode.Name = "chkSelItemCode"
        Me.chkSelItemCode.Size = New System.Drawing.Size(151, 24)
        Me.chkSelItemCode.TabIndex = 4
        Me.chkSelItemCode.Text = "Select Item Code"
        Me.chkSelItemCode.UseVisualStyleBackColor = True
        '
        'chkSelParty
        '
        Me.chkSelParty.AutoSize = True
        Me.chkSelParty.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSelParty.Location = New System.Drawing.Point(406, 127)
        Me.chkSelParty.Margin = New System.Windows.Forms.Padding(5)
        Me.chkSelParty.Name = "chkSelParty"
        Me.chkSelParty.Size = New System.Drawing.Size(113, 24)
        Me.chkSelParty.TabIndex = 5
        Me.chkSelParty.Text = "Select Party"
        Me.chkSelParty.UseVisualStyleBackColor = True
        '
        'pnlDate
        '
        Me.pnlDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlDate.Controls.Add(Me.Label4)
        Me.pnlDate.Controls.Add(Me.Label3)
        Me.pnlDate.Controls.Add(Me.dtpToDate)
        Me.pnlDate.Controls.Add(Me.dtpFromDate)
        Me.pnlDate.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlDate.Location = New System.Drawing.Point(11, 93)
        Me.pnlDate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.pnlDate.Name = "pnlDate"
        Me.pnlDate.Size = New System.Drawing.Size(358, 55)
        Me.pnlDate.TabIndex = 228
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(201, 25)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(27, 20)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "To"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(23, 25)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(46, 20)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "From"
        '
        'dtpToDate
        '
        Me.dtpToDate.CustomFormat = "dd/MM/yyyy"
        Me.dtpToDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpToDate.Location = New System.Drawing.Point(228, 22)
        Me.dtpToDate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtpToDate.Name = "dtpToDate"
        Me.dtpToDate.Size = New System.Drawing.Size(112, 26)
        Me.dtpToDate.TabIndex = 4
        '
        'dtpFromDate
        '
        Me.dtpFromDate.CustomFormat = "dd/MM/yyyy"
        Me.dtpFromDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpFromDate.Location = New System.Drawing.Point(76, 22)
        Me.dtpFromDate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtpFromDate.Name = "dtpFromDate"
        Me.dtpFromDate.Size = New System.Drawing.Size(119, 26)
        Me.dtpFromDate.TabIndex = 3
        '
        'chkDate
        '
        Me.chkDate.AutoSize = True
        Me.chkDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDate.Location = New System.Drawing.Point(16, 83)
        Me.chkDate.Margin = New System.Windows.Forms.Padding(4)
        Me.chkDate.Name = "chkDate"
        Me.chkDate.Size = New System.Drawing.Size(115, 24)
        Me.chkDate.TabIndex = 3
        Me.chkDate.Text = "Date Range"
        Me.chkDate.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.rbBoth)
        Me.Panel1.Controls.Add(Me.rbPending)
        Me.Panel1.Controls.Add(Me.rbShortClosed)
        Me.Panel1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel1.Location = New System.Drawing.Point(391, 11)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(184, 78)
        Me.Panel1.TabIndex = 229
        '
        'rbBoth
        '
        Me.rbBoth.AutoSize = True
        Me.rbBoth.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbBoth.Location = New System.Drawing.Point(15, 49)
        Me.rbBoth.Name = "rbBoth"
        Me.rbBoth.Size = New System.Drawing.Size(61, 24)
        Me.rbBoth.TabIndex = 2
        Me.rbBoth.TabStop = True
        Me.rbBoth.Text = "Both"
        Me.rbBoth.UseVisualStyleBackColor = True
        '
        'rbPending
        '
        Me.rbPending.AutoSize = True
        Me.rbPending.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbPending.Location = New System.Drawing.Point(15, 26)
        Me.rbPending.Name = "rbPending"
        Me.rbPending.Size = New System.Drawing.Size(85, 24)
        Me.rbPending.TabIndex = 1
        Me.rbPending.TabStop = True
        Me.rbPending.Text = "Pending"
        Me.rbPending.UseVisualStyleBackColor = True
        '
        'rbShortClosed
        '
        Me.rbShortClosed.AutoSize = True
        Me.rbShortClosed.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbShortClosed.Location = New System.Drawing.Point(15, 3)
        Me.rbShortClosed.Name = "rbShortClosed"
        Me.rbShortClosed.Size = New System.Drawing.Size(115, 24)
        Me.rbShortClosed.TabIndex = 0
        Me.rbShortClosed.TabStop = True
        Me.rbShortClosed.Text = "ShortClosed"
        Me.rbShortClosed.UseVisualStyleBackColor = True
        '
        'vsfDet
        '
        Me.vsfDet.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "2,0,0,0,0,110,Columns:"
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.Location = New System.Drawing.Point(16, 197)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.Count = 2
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(1079, 286)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 1
        '
        'btnClose
        '
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Image = CType(resources.GetObject("btnClose.Image"), System.Drawing.Image)
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(668, 122)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(77, 28)
        Me.btnClose.TabIndex = 7
        Me.btnClose.Text = "&Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tt.SetToolTip(Me.btnClose, "Close : Alt + C")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnLoad
        '
        Me.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLoad.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoad.Image = CType(resources.GetObject("btnLoad.Image"), System.Drawing.Image)
        Me.btnLoad.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnLoad.Location = New System.Drawing.Point(580, 122)
        Me.btnLoad.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(77, 28)
        Me.btnLoad.TabIndex = 6
        Me.btnLoad.Text = "&Load"
        Me.btnLoad.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tt.SetToolTip(Me.btnLoad, "Load : Alt + L")
        Me.btnLoad.UseVisualStyleBackColor = False
        '
        'cboDocNo
        '
        Me.cboDocNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.cboDocNo.FormattingEnabled = True
        Me.cboDocNo.Location = New System.Drawing.Point(113, 51)
        Me.cboDocNo.Name = "cboDocNo"
        Me.cboDocNo.Size = New System.Drawing.Size(211, 23)
        Me.cboDocNo.TabIndex = 1
        '
        'vsfSearch
        '
        Me.vsfSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfSearch.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfSearch.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfSearch.ExtendLastCol = True
        Me.vsfSearch.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfSearch.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfSearch.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfSearch.Location = New System.Drawing.Point(16, 167)
        Me.vsfSearch.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfSearch.Name = "vsfSearch"
        Me.vsfSearch.Rows.Count = 1
        Me.vsfSearch.Rows.DefaultSize = 24
        Me.vsfSearch.Rows.Fixed = 0
        Me.vsfSearch.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.vsfSearch.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfSearch.ShowCellLabels = True
        Me.vsfSearch.Size = New System.Drawing.Size(1079, 26)
        Me.vsfSearch.StyleInfo = resources.GetString("vsfSearch.StyleInfo")
        Me.vsfSearch.TabIndex = 234
        Me.vsfSearch.Tag = ""
        Me.vsfSearch.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.System
        '
        'btnExport
        '
        Me.btnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExport.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnExport.Image = CType(resources.GetObject("btnExport.Image"), System.Drawing.Image)
        Me.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExport.Location = New System.Drawing.Point(16, 490)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(33, 31)
        Me.btnExport.TabIndex = 2
        Me.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tt.SetToolTip(Me.btnExport, "Export to XL : Alt + E")
        Me.btnExport.UseVisualStyleBackColor = False
        '
        'pnlEdit
        '
        Me.pnlEdit.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlEdit.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.pnlEdit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlEdit.Controls.Add(Me.chkSel)
        Me.pnlEdit.Controls.Add(Me.btnSave)
        Me.pnlEdit.Controls.Add(Me.btnCancel)
        Me.pnlEdit.Controls.Add(Me.vsfShort)
        Me.pnlEdit.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.pnlEdit.Location = New System.Drawing.Point(14, 169)
        Me.pnlEdit.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.pnlEdit.Name = "pnlEdit"
        Me.pnlEdit.Size = New System.Drawing.Size(1081, 317)
        Me.pnlEdit.TabIndex = 0
        Me.pnlEdit.Visible = False
        '
        'chkSel
        '
        Me.chkSel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkSel.AutoSize = True
        Me.chkSel.Location = New System.Drawing.Point(3, 282)
        Me.chkSel.Name = "chkSel"
        Me.chkSel.Size = New System.Drawing.Size(79, 21)
        Me.chkSel.TabIndex = 1
        Me.chkSel.Text = "Select All"
        Me.chkSel.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(876, 274)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(95, 35)
        Me.btnSave.TabIndex = 2
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tt.SetToolTip(Me.btnSave, "Save : Alt + S")
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(979, 274)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(95, 35)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "C&ancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tt.SetToolTip(Me.btnCancel, "Shortcut : Alt + A")
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'vsfShort
        '
        Me.vsfShort.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfShort.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfShort.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfShort.ColumnInfo = "2,0,0,0,0,110,Columns:"
        Me.vsfShort.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfShort.Location = New System.Drawing.Point(-1, 13)
        Me.vsfShort.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfShort.Name = "vsfShort"
        Me.vsfShort.Rows.Count = 2
        Me.vsfShort.Rows.DefaultSize = 24
        Me.vsfShort.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfShort.Size = New System.Drawing.Size(1073, 262)
        Me.vsfShort.StyleInfo = resources.GetString("vsfShort.StyleInfo")
        Me.vsfShort.TabIndex = 0
        '
        'pnlHdr
        '
        Me.pnlHdr.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlHdr.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.pnlHdr.Controls.Add(Me.chkDate)
        Me.pnlHdr.Controls.Add(Me.btnLoadDocNo)
        Me.pnlHdr.Controls.Add(Me.Label1)
        Me.pnlHdr.Controls.Add(Me.cboDocName)
        Me.pnlHdr.Controls.Add(Me.Label2)
        Me.pnlHdr.Controls.Add(Me.cboDocNo)
        Me.pnlHdr.Controls.Add(Me.chkSelItemCode)
        Me.pnlHdr.Controls.Add(Me.btnClose)
        Me.pnlHdr.Controls.Add(Me.chkSelParty)
        Me.pnlHdr.Controls.Add(Me.btnLoad)
        Me.pnlHdr.Controls.Add(Me.pnlDate)
        Me.pnlHdr.Controls.Add(Me.Panel1)
        Me.pnlHdr.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.pnlHdr.Location = New System.Drawing.Point(14, 6)
        Me.pnlHdr.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.pnlHdr.Name = "pnlHdr"
        Me.pnlHdr.Size = New System.Drawing.Size(1081, 155)
        Me.pnlHdr.TabIndex = 237
        '
        'lblHelp
        '
        Me.lblHelp.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblHelp.AutoSize = True
        Me.lblHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHelp.ForeColor = System.Drawing.Color.Blue
        Me.lblHelp.Location = New System.Drawing.Point(417, 490)
        Me.lblHelp.Name = "lblHelp"
        Me.lblHelp.Size = New System.Drawing.Size(285, 20)
        Me.lblHelp.TabIndex = 3
        Me.lblHelp.Text = "Double Click to Shortclose Documents."
        '
        'frmUtlShortclose
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1107, 547)
        Me.Controls.Add(Me.pnlHdr)
        Me.Controls.Add(Me.pnlEdit)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.vsfSearch)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.lblHelp)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.KeyPreview = True
        Me.Name = "frmUtlShortclose"
        Me.ShowIcon = False
        Me.Text = "Short Close Document"
        Me.pnlDate.ResumeLayout(False)
        Me.pnlDate.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlEdit.ResumeLayout(False)
        Me.pnlEdit.PerformLayout()
        CType(Me.vsfShort, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlHdr.ResumeLayout(False)
        Me.pnlHdr.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents cboDocName As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents btnLoadDocNo As Button
    Friend WithEvents chkSelItemCode As CheckBox
    Friend WithEvents chkSelParty As CheckBox
    Friend WithEvents pnlDate As Panel
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents dtpToDate As DateTimePicker
    Friend WithEvents dtpFromDate As DateTimePicker
    Friend WithEvents chkDate As CheckBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents rbBoth As RadioButton
    Friend WithEvents rbPending As RadioButton
    Friend WithEvents rbShortClosed As RadioButton
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnClose As Button
    Friend WithEvents btnLoad As Button
    Friend WithEvents cboDocNo As ComboBox
    Friend WithEvents vsfSearch As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnExport As C1.Win.C1Input.C1Button
    Friend WithEvents pnlEdit As Panel
    Friend WithEvents pnlHdr As Panel
    Friend WithEvents vsfShort As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnSave As C1.Win.C1Input.C1Button
    Friend WithEvents btnCancel As C1.Win.C1Input.C1Button
    Friend WithEvents lblHelp As Label
    Friend WithEvents tt As ToolTip
    Friend WithEvents chkSel As CheckBox
End Class
