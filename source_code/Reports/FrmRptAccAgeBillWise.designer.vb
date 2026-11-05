<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmRptAccAgeBillWise
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmRptAccAgeBillWise))
        Me.vsfView = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnExportToXL = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnView = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.dtpFromDate = New System.Windows.Forms.DateTimePicker()
        Me.chkSelCust = New System.Windows.Forms.CheckBox()
        Me.txtDr = New System.Windows.Forms.TextBox()
        Me.lblDr = New System.Windows.Forms.Label()
        Me.lblCr = New System.Windows.Forms.Label()
        Me.txtCr = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.vsfRange = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.optSmry = New System.Windows.Forms.RadioButton()
        Me.optDet = New System.Windows.Forms.RadioButton()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.optReceivable = New System.Windows.Forms.RadioButton()
        Me.optPayable = New System.Windows.Forms.RadioButton()
        Me.pnlCopyShortCut = New System.Windows.Forms.Panel()
        Me.btnCopy = New System.Windows.Forms.Button()
        Me.pnlbtnClose = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.RbnCopyGrid = New System.Windows.Forms.RadioButton()
        Me.RbnCopyRow = New System.Windows.Forms.RadioButton()
        Me.RbnCopyColumn = New System.Windows.Forms.RadioButton()
        Me.RbnCopyCell = New System.Windows.Forms.RadioButton()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfRange, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.pnlCopyShortCut.SuspendLayout()
        Me.SuspendLayout()
        '
        'vsfView
        '
        Me.vsfView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfView.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfView.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfView.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfView.Location = New System.Drawing.Point(12, 178)
        Me.vsfView.Name = "vsfView"
        Me.vsfView.Rows.Count = 4
        Me.vsfView.Rows.DefaultSize = 24
        Me.vsfView.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfView.Size = New System.Drawing.Size(875, 285)
        Me.vsfView.StyleInfo = resources.GetString("vsfView.StyleInfo")
        Me.vsfView.TabIndex = 64
        '
        'btnExportToXL
        '
        Me.btnExportToXL.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExportToXL.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnExportToXL.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExportToXL.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.btnExportToXL.Image = CType(resources.GetObject("btnExportToXL.Image"), System.Drawing.Image)
        Me.btnExportToXL.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExportToXL.Location = New System.Drawing.Point(763, 469)
        Me.btnExportToXL.Name = "btnExportToXL"
        Me.btnExportToXL.Size = New System.Drawing.Size(34, 29)
        Me.btnExportToXL.TabIndex = 65
        Me.btnExportToXL.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnExportToXL, "Export to Excel : Alt + E")
        Me.btnExportToXL.UseVisualStyleBackColor = False
        Me.btnExportToXL.Visible = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(803, 469)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(84, 29)
        Me.btnClose.TabIndex = 21
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close : Alt + O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnView
        '
        Me.btnView.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnView.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnView.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnView.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.btnView.Image = CType(resources.GetObject("btnView.Image"), System.Drawing.Image)
        Me.btnView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnView.Location = New System.Drawing.Point(801, 143)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(86, 29)
        Me.btnView.TabIndex = 20
        Me.btnView.Text = "&View"
        Me.btnView.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnView, "View : Alt + V")
        Me.btnView.UseVisualStyleBackColor = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.Label3.Location = New System.Drawing.Point(21, 13)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(96, 20)
        Me.Label3.TabIndex = 66
        Me.Label3.Text = "As On Date "
        '
        'dtpFromDate
        '
        Me.dtpFromDate.CustomFormat = "dd/MM/yyyy"
        Me.dtpFromDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpFromDate.Location = New System.Drawing.Point(123, 8)
        Me.dtpFromDate.Name = "dtpFromDate"
        Me.dtpFromDate.Size = New System.Drawing.Size(107, 26)
        Me.dtpFromDate.TabIndex = 67
        '
        'chkSelCust
        '
        Me.chkSelCust.AutoSize = True
        Me.chkSelCust.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.chkSelCust.Location = New System.Drawing.Point(247, 141)
        Me.chkSelCust.Margin = New System.Windows.Forms.Padding(4)
        Me.chkSelCust.Name = "chkSelCust"
        Me.chkSelCust.Size = New System.Drawing.Size(146, 24)
        Me.chkSelCust.TabIndex = 81
        Me.chkSelCust.Text = "Select Customer"
        Me.chkSelCust.UseVisualStyleBackColor = True
        '
        'txtDr
        '
        Me.txtDr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDr.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.txtDr.Location = New System.Drawing.Point(311, 43)
        Me.txtDr.Name = "txtDr"
        Me.txtDr.Size = New System.Drawing.Size(100, 26)
        Me.txtDr.TabIndex = 82
        '
        'lblDr
        '
        Me.lblDr.AutoSize = True
        Me.lblDr.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.lblDr.Location = New System.Drawing.Point(280, 13)
        Me.lblDr.Name = "lblDr"
        Me.lblDr.Size = New System.Drawing.Size(131, 20)
        Me.lblDr.TabIndex = 83
        Me.lblDr.Text = "Show Bills having"
        '
        'lblCr
        '
        Me.lblCr.AutoSize = True
        Me.lblCr.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.lblCr.Location = New System.Drawing.Point(243, 70)
        Me.lblCr.Name = "lblCr"
        Me.lblCr.Size = New System.Drawing.Size(64, 20)
        Me.lblCr.TabIndex = 85
        Me.lblCr.Text = "Credit >"
        '
        'txtCr
        '
        Me.txtCr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCr.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.txtCr.Location = New System.Drawing.Point(311, 68)
        Me.txtCr.Name = "txtCr"
        Me.txtCr.Size = New System.Drawing.Size(100, 26)
        Me.txtCr.TabIndex = 84
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.Label1.Location = New System.Drawing.Point(244, 45)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 20)
        Me.Label1.TabIndex = 86
        Me.Label1.Text = "Debit >"
        '
        'vsfRange
        '
        Me.vsfRange.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfRange.ColumnInfo = "10,0,0,0,0,95,Columns:"
        Me.vsfRange.Location = New System.Drawing.Point(439, 5)
        Me.vsfRange.Name = "vsfRange"
        Me.vsfRange.Rows.DefaultSize = 19
        Me.vsfRange.Size = New System.Drawing.Size(228, 136)
        Me.vsfRange.TabIndex = 87
        '
        'btnClear
        '
        Me.btnClear.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClear.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClear.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.btnClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClear.Location = New System.Drawing.Point(671, 109)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(89, 32)
        Me.btnClear.TabIndex = 88
        Me.btnClear.Text = "&Clear"
        Me.ToolTip1.SetToolTip(Me.btnClear, "Clear : Alt + C")
        Me.btnClear.UseVisualStyleBackColor = False
        '
        'optSmry
        '
        Me.optSmry.AutoSize = True
        Me.optSmry.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.optSmry.Location = New System.Drawing.Point(3, 30)
        Me.optSmry.Name = "optSmry"
        Me.optSmry.Size = New System.Drawing.Size(94, 24)
        Me.optSmry.TabIndex = 89
        Me.optSmry.Text = "Partywise"
        Me.optSmry.UseVisualStyleBackColor = True
        '
        'optDet
        '
        Me.optDet.AutoSize = True
        Me.optDet.Checked = True
        Me.optDet.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.optDet.Location = New System.Drawing.Point(2, 5)
        Me.optDet.Name = "optDet"
        Me.optDet.Size = New System.Drawing.Size(78, 24)
        Me.optDet.TabIndex = 90
        Me.optDet.TabStop = True
        Me.optDet.Text = "Billwise"
        Me.optDet.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.optDet)
        Me.Panel1.Controls.Add(Me.optSmry)
        Me.Panel1.Location = New System.Drawing.Point(25, 40)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(205, 61)
        Me.Panel1.TabIndex = 91
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.optReceivable)
        Me.Panel2.Controls.Add(Me.optPayable)
        Me.Panel2.Location = New System.Drawing.Point(25, 107)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(205, 65)
        Me.Panel2.TabIndex = 91
        '
        'optReceivable
        '
        Me.optReceivable.AutoSize = True
        Me.optReceivable.Checked = True
        Me.optReceivable.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.optReceivable.Location = New System.Drawing.Point(3, 4)
        Me.optReceivable.Name = "optReceivable"
        Me.optReceivable.Size = New System.Drawing.Size(105, 24)
        Me.optReceivable.TabIndex = 92
        Me.optReceivable.TabStop = True
        Me.optReceivable.Text = "Receivable"
        Me.optReceivable.UseVisualStyleBackColor = True
        '
        'optPayable
        '
        Me.optPayable.AutoSize = True
        Me.optPayable.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.optPayable.Location = New System.Drawing.Point(2, 34)
        Me.optPayable.Name = "optPayable"
        Me.optPayable.Size = New System.Drawing.Size(83, 24)
        Me.optPayable.TabIndex = 91
        Me.optPayable.Text = "Payable"
        Me.optPayable.UseVisualStyleBackColor = True
        '
        'pnlCopyShortCut
        '
        Me.pnlCopyShortCut.Controls.Add(Me.btnCopy)
        Me.pnlCopyShortCut.Controls.Add(Me.pnlbtnClose)
        Me.pnlCopyShortCut.Controls.Add(Me.Label5)
        Me.pnlCopyShortCut.Controls.Add(Me.Label4)
        Me.pnlCopyShortCut.Controls.Add(Me.Label2)
        Me.pnlCopyShortCut.Controls.Add(Me.Label6)
        Me.pnlCopyShortCut.Controls.Add(Me.RbnCopyGrid)
        Me.pnlCopyShortCut.Controls.Add(Me.RbnCopyRow)
        Me.pnlCopyShortCut.Controls.Add(Me.RbnCopyColumn)
        Me.pnlCopyShortCut.Controls.Add(Me.RbnCopyCell)
        Me.pnlCopyShortCut.Location = New System.Drawing.Point(302, 217)
        Me.pnlCopyShortCut.Name = "pnlCopyShortCut"
        Me.pnlCopyShortCut.Size = New System.Drawing.Size(329, 177)
        Me.pnlCopyShortCut.TabIndex = 296
        Me.pnlCopyShortCut.Visible = False
        '
        'btnCopy
        '
        Me.btnCopy.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCopy.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnCopy.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_ok
        Me.btnCopy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCopy.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnCopy.Location = New System.Drawing.Point(161, 135)
        Me.btnCopy.Name = "btnCopy"
        Me.btnCopy.Size = New System.Drawing.Size(70, 29)
        Me.btnCopy.TabIndex = 23
        Me.btnCopy.Text = "Copy"
        Me.btnCopy.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCopy.UseVisualStyleBackColor = False
        '
        'pnlbtnClose
        '
        Me.pnlbtnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlbtnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.pnlbtnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.pnlbtnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.pnlbtnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.pnlbtnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.pnlbtnClose.Location = New System.Drawing.Point(237, 135)
        Me.pnlbtnClose.Name = "pnlbtnClose"
        Me.pnlbtnClose.Size = New System.Drawing.Size(70, 29)
        Me.pnlbtnClose.TabIndex = 22
        Me.pnlbtnClose.Text = "Cl&ose"
        Me.pnlbtnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.pnlbtnClose, "Close : Alt + O")
        Me.pnlbtnClose.UseVisualStyleBackColor = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(178, 109)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(88, 16)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "Ctrl + Shift + G"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(178, 79)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(77, 16)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Ctrl + Alt + C"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(178, 48)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(87, 16)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Ctrl + Shift + C"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(178, 19)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(49, 16)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "Ctrl + C"
        '
        'RbnCopyGrid
        '
        Me.RbnCopyGrid.AutoSize = True
        Me.RbnCopyGrid.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RbnCopyGrid.Location = New System.Drawing.Point(16, 109)
        Me.RbnCopyGrid.Name = "RbnCopyGrid"
        Me.RbnCopyGrid.Size = New System.Drawing.Size(129, 20)
        Me.RbnCopyGrid.TabIndex = 3
        Me.RbnCopyGrid.TabStop = True
        Me.RbnCopyGrid.Text = "Copy Entire Grid  "
        Me.RbnCopyGrid.UseVisualStyleBackColor = True
        '
        'RbnCopyRow
        '
        Me.RbnCopyRow.AutoSize = True
        Me.RbnCopyRow.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RbnCopyRow.Location = New System.Drawing.Point(16, 77)
        Me.RbnCopyRow.Name = "RbnCopyRow"
        Me.RbnCopyRow.Size = New System.Drawing.Size(128, 20)
        Me.RbnCopyRow.TabIndex = 2
        Me.RbnCopyRow.TabStop = True
        Me.RbnCopyRow.Text = "Copy Entire  Row"
        Me.RbnCopyRow.UseVisualStyleBackColor = True
        '
        'RbnCopyColumn
        '
        Me.RbnCopyColumn.AutoSize = True
        Me.RbnCopyColumn.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RbnCopyColumn.Location = New System.Drawing.Point(15, 46)
        Me.RbnCopyColumn.Name = "RbnCopyColumn"
        Me.RbnCopyColumn.Size = New System.Drawing.Size(143, 20)
        Me.RbnCopyColumn.TabIndex = 1
        Me.RbnCopyColumn.TabStop = True
        Me.RbnCopyColumn.Text = "Copy Entire Column"
        Me.RbnCopyColumn.UseVisualStyleBackColor = True
        '
        'RbnCopyCell
        '
        Me.RbnCopyCell.AutoSize = True
        Me.RbnCopyCell.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RbnCopyCell.Location = New System.Drawing.Point(15, 19)
        Me.RbnCopyCell.Name = "RbnCopyCell"
        Me.RbnCopyCell.Size = New System.Drawing.Size(129, 20)
        Me.RbnCopyCell.TabIndex = 0
        Me.RbnCopyCell.TabStop = True
        Me.RbnCopyCell.Text = "Copy Data In Cell"
        Me.RbnCopyCell.UseVisualStyleBackColor = True
        '
        'FrmRptAccAgeBillWise
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(899, 503)
        Me.Controls.Add(Me.pnlCopyShortCut)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.vsfRange)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblCr)
        Me.Controls.Add(Me.txtCr)
        Me.Controls.Add(Me.lblDr)
        Me.Controls.Add(Me.txtDr)
        Me.Controls.Add(Me.chkSelCust)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.dtpFromDate)
        Me.Controls.Add(Me.btnExportToXL)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnView)
        Me.Controls.Add(Me.vsfView)
        Me.MinimumSize = New System.Drawing.Size(888, 541)
        Me.Name = "FrmRptAccAgeBillWise"
        Me.ShowIcon = False
        Me.Text = "Bill wise Ageing"
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfRange, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.pnlCopyShortCut.ResumeLayout(False)
        Me.pnlCopyShortCut.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnView As System.Windows.Forms.Button
    Friend WithEvents vsfView As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnExportToXL As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents dtpFromDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents chkSelCust As System.Windows.Forms.CheckBox
    Friend WithEvents txtDr As System.Windows.Forms.TextBox
    Friend WithEvents lblDr As System.Windows.Forms.Label
    Friend WithEvents lblCr As System.Windows.Forms.Label
    Friend WithEvents txtCr As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents vsfRange As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents optSmry As System.Windows.Forms.RadioButton
    Friend WithEvents optDet As System.Windows.Forms.RadioButton
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents optReceivable As RadioButton
    Friend WithEvents optPayable As RadioButton
    Friend WithEvents pnlCopyShortCut As Panel
    Friend WithEvents btnCopy As Button
    Friend WithEvents pnlbtnClose As Button
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents RbnCopyGrid As RadioButton
    Friend WithEvents RbnCopyRow As RadioButton
    Friend WithEvents RbnCopyColumn As RadioButton
    Friend WithEvents RbnCopyCell As RadioButton
End Class
