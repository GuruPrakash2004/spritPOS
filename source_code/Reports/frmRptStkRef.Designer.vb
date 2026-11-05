<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmRptStkRef
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRptStkRef))
        Me.lblItem = New System.Windows.Forms.Label()
        Me.pnlDate = New System.Windows.Forms.Panel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.dtpToDate = New System.Windows.Forms.DateTimePicker()
        Me.dtpFromDate = New System.Windows.Forms.DateTimePicker()
        Me.chkDate = New System.Windows.Forms.CheckBox()
        Me.txtItemCode = New System.Windows.Forms.TextBox()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.vsfView = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.txtNos = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cboTrn = New System.Windows.Forms.ComboBox()
        Me.txtDet = New System.Windows.Forms.TextBox()
        Me.txtQOH = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.vsfTrn = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.pnlCopyShortCut = New System.Windows.Forms.Panel()
        Me.btnCopy = New System.Windows.Forms.Button()
        Me.pnlbtnClose = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.RbnCopyGrid = New System.Windows.Forms.RadioButton()
        Me.RbnCopyRow = New System.Windows.Forms.RadioButton()
        Me.RbnCopyColumn = New System.Windows.Forms.RadioButton()
        Me.RbnCopyCell = New System.Windows.Forms.RadioButton()
        Me.pnlDate.SuspendLayout()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.vsfTrn, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlCopyShortCut.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblItem
        '
        Me.lblItem.AutoSize = True
        Me.lblItem.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItem.Location = New System.Drawing.Point(23, 28)
        Me.lblItem.Name = "lblItem"
        Me.lblItem.Size = New System.Drawing.Size(68, 17)
        Me.lblItem.TabIndex = 221
        Me.lblItem.Text = "Item Code"
        '
        'pnlDate
        '
        Me.pnlDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlDate.Controls.Add(Me.Label4)
        Me.pnlDate.Controls.Add(Me.Label3)
        Me.pnlDate.Controls.Add(Me.dtpToDate)
        Me.pnlDate.Controls.Add(Me.dtpFromDate)
        Me.pnlDate.Controls.Add(Me.chkDate)
        Me.pnlDate.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlDate.Location = New System.Drawing.Point(642, 288)
        Me.pnlDate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.pnlDate.Name = "pnlDate"
        Me.pnlDate.Size = New System.Drawing.Size(378, 55)
        Me.pnlDate.TabIndex = 219
        Me.pnlDate.Visible = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(172, 28)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(22, 17)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "To"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(3, 26)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(38, 17)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "From"
        '
        'dtpToDate
        '
        Me.dtpToDate.CustomFormat = "dd/MM/yyyy"
        Me.dtpToDate.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpToDate.Location = New System.Drawing.Point(199, 22)
        Me.dtpToDate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtpToDate.Name = "dtpToDate"
        Me.dtpToDate.Size = New System.Drawing.Size(112, 25)
        Me.dtpToDate.TabIndex = 4
        '
        'dtpFromDate
        '
        Me.dtpFromDate.CustomFormat = "dd/MM/yyyy"
        Me.dtpFromDate.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpFromDate.Location = New System.Drawing.Point(47, 22)
        Me.dtpFromDate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtpFromDate.Name = "dtpFromDate"
        Me.dtpFromDate.Size = New System.Drawing.Size(119, 25)
        Me.dtpFromDate.TabIndex = 3
        '
        'chkDate
        '
        Me.chkDate.AutoSize = True
        Me.chkDate.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDate.Location = New System.Drawing.Point(5, 2)
        Me.chkDate.Margin = New System.Windows.Forms.Padding(4)
        Me.chkDate.Name = "chkDate"
        Me.chkDate.Size = New System.Drawing.Size(95, 21)
        Me.chkDate.TabIndex = 228
        Me.chkDate.Text = "Date Range"
        Me.chkDate.UseVisualStyleBackColor = True
        '
        'txtItemCode
        '
        Me.txtItemCode.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtItemCode.Location = New System.Drawing.Point(97, 25)
        Me.txtItemCode.Name = "txtItemCode"
        Me.txtItemCode.Size = New System.Drawing.Size(365, 25)
        Me.txtItemCode.TabIndex = 229
        '
        'btnClose
        '
        Me.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Image = CType(resources.GetObject("btnClose.Image"), System.Drawing.Image)
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(1064, 22)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(77, 28)
        Me.btnClose.TabIndex = 223
        Me.btnClose.Text = "Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'vsfView
        '
        Me.vsfView.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfView.ColumnInfo = "10,0,0,0,0,120,Columns:3{Width:122;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfView.ExtendLastCol = True
        Me.vsfView.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfView.Location = New System.Drawing.Point(97, 57)
        Me.vsfView.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfView.Name = "vsfView"
        Me.vsfView.Rows.DefaultSize = 24
        Me.vsfView.Size = New System.Drawing.Size(560, 235)
        Me.vsfView.StyleInfo = resources.GetString("vsfView.StyleInfo")
        Me.vsfView.TabIndex = 240
        '
        'vsfDet
        '
        Me.vsfDet.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfDet.AllowFiltering = True
        Me.vsfDet.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfDet.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfDet.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.AutoResize = True
        Me.vsfDet.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfDet.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(10, 341)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.Count = 2
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.Rows.GlyphRow = 0
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.ShowCellLabels = True
        Me.vsfDet.Size = New System.Drawing.Size(1132, 234)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 249
        Me.vsfDet.Tag = "View"
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.btnLoad)
        Me.Panel1.Controls.Add(Me.txtNos)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.cboTrn)
        Me.Panel1.Location = New System.Drawing.Point(10, 305)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1132, 30)
        Me.Panel1.TabIndex = 249
        '
        'btnLoad
        '
        Me.btnLoad.Location = New System.Drawing.Point(585, 7)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(75, 23)
        Me.btnLoad.TabIndex = 249
        Me.btnLoad.Text = "Load"
        Me.btnLoad.UseVisualStyleBackColor = True
        '
        'txtNos
        '
        Me.txtNos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtNos.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNos.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.txtNos.Location = New System.Drawing.Point(430, 7)
        Me.txtNos.MaxLength = 4
        Me.txtNos.Name = "txtNos"
        Me.txtNos.Size = New System.Drawing.Size(54, 26)
        Me.txtNos.TabIndex = 247
        Me.txtNos.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(490, 10)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(78, 17)
        Me.Label6.TabIndex = 246
        Me.Label6.Text = "transactions"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(360, 10)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(64, 17)
        Me.Label5.TabIndex = 245
        Me.Label5.Text = "Load Last"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(3, 10)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(80, 17)
        Me.Label2.TabIndex = 244
        Me.Label2.Text = "Transactions"
        '
        'cboTrn
        '
        Me.cboTrn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTrn.FormattingEnabled = True
        Me.cboTrn.Location = New System.Drawing.Point(93, 8)
        Me.cboTrn.Name = "cboTrn"
        Me.cboTrn.Size = New System.Drawing.Size(238, 21)
        Me.cboTrn.TabIndex = 243
        '
        'txtDet
        '
        Me.txtDet.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(217, Byte), Integer))
        Me.txtDet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDet.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDet.Location = New System.Drawing.Point(97, 56)
        Me.txtDet.MaxLength = 75
        Me.txtDet.Multiline = True
        Me.txtDet.Name = "txtDet"
        Me.txtDet.Size = New System.Drawing.Size(487, 72)
        Me.txtDet.TabIndex = 570
        '
        'txtQOH
        '
        Me.txtQOH.BackColor = System.Drawing.Color.Lime
        Me.txtQOH.Font = New System.Drawing.Font("Bookman Old Style", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtQOH.Location = New System.Drawing.Point(634, 20)
        Me.txtQOH.Name = "txtQOH"
        Me.txtQOH.ReadOnly = True
        Me.txtQOH.Size = New System.Drawing.Size(365, 42)
        Me.txtQOH.TabIndex = 571
        Me.txtQOH.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(632, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(365, 17)
        Me.Label7.TabIndex = 572
        Me.Label7.Text = "Stock in Hand"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'vsfTrn
        '
        Me.vsfTrn.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfTrn.AllowFiltering = True
        Me.vsfTrn.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfTrn.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfTrn.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfTrn.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfTrn.AutoResize = True
        Me.vsfTrn.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfTrn.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfTrn.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfTrn.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfTrn.ExtendLastCol = True
        Me.vsfTrn.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfTrn.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfTrn.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfTrn.Location = New System.Drawing.Point(97, 57)
        Me.vsfTrn.Name = "vsfTrn"
        Me.vsfTrn.Rows.Count = 2
        Me.vsfTrn.Rows.DefaultSize = 24
        Me.vsfTrn.Rows.GlyphRow = 0
        Me.vsfTrn.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfTrn.ShowCellLabels = True
        Me.vsfTrn.Size = New System.Drawing.Size(1045, 216)
        Me.vsfTrn.StyleInfo = resources.GetString("vsfTrn.StyleInfo")
        Me.vsfTrn.TabIndex = 573
        Me.vsfTrn.Tag = "View"
        '
        'pnlCopyShortCut
        '
        Me.pnlCopyShortCut.Controls.Add(Me.btnCopy)
        Me.pnlCopyShortCut.Controls.Add(Me.pnlbtnClose)
        Me.pnlCopyShortCut.Controls.Add(Me.Label1)
        Me.pnlCopyShortCut.Controls.Add(Me.Label8)
        Me.pnlCopyShortCut.Controls.Add(Me.Label9)
        Me.pnlCopyShortCut.Controls.Add(Me.Label10)
        Me.pnlCopyShortCut.Controls.Add(Me.RbnCopyGrid)
        Me.pnlCopyShortCut.Controls.Add(Me.RbnCopyRow)
        Me.pnlCopyShortCut.Controls.Add(Me.RbnCopyColumn)
        Me.pnlCopyShortCut.Controls.Add(Me.RbnCopyCell)
        Me.pnlCopyShortCut.Location = New System.Drawing.Point(304, 73)
        Me.pnlCopyShortCut.Name = "pnlCopyShortCut"
        Me.pnlCopyShortCut.Size = New System.Drawing.Size(329, 177)
        Me.pnlCopyShortCut.TabIndex = 574
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
        Me.pnlbtnClose.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(178, 109)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(88, 16)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Ctrl + Shift + G"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(178, 79)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(77, 16)
        Me.Label8.TabIndex = 6
        Me.Label8.Text = "Ctrl + Alt + C"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(178, 48)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(87, 16)
        Me.Label9.TabIndex = 5
        Me.Label9.Text = "Ctrl + Shift + C"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(178, 19)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(49, 16)
        Me.Label10.TabIndex = 4
        Me.Label10.Text = "Ctrl + C"
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
        'frmRptStkRef
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.ClientSize = New System.Drawing.Size(1028, 609)
        Me.Controls.Add(Me.pnlCopyShortCut)
        Me.Controls.Add(Me.vsfView)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.vsfTrn)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtQOH)
        Me.Controls.Add(Me.txtDet)
        Me.Controls.Add(Me.lblItem)
        Me.Controls.Add(Me.txtItemCode)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.pnlDate)
        Me.KeyPreview = True
        Me.Name = "frmRptStkRef"
        Me.ShowIcon = False
        Me.Text = "Stock Referencer"
        Me.pnlDate.ResumeLayout(False)
        Me.pnlDate.PerformLayout()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.vsfTrn, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlCopyShortCut.ResumeLayout(False)
        Me.pnlCopyShortCut.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents lblItem As System.Windows.Forms.Label
    Friend WithEvents pnlDate As System.Windows.Forms.Panel
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents dtpToDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpFromDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents chkDate As System.Windows.Forms.CheckBox
    Friend WithEvents txtItemCode As System.Windows.Forms.TextBox
    Friend WithEvents vsfView As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents Panel1 As Panel
    Friend WithEvents btnLoad As Button
    Friend WithEvents txtNos As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents cboTrn As ComboBox
    Friend WithEvents txtDet As TextBox
    Friend WithEvents txtQOH As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents vsfTrn As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents pnlCopyShortCut As Panel
    Friend WithEvents btnCopy As Button
    Friend WithEvents pnlbtnClose As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents RbnCopyGrid As RadioButton
    Friend WithEvents RbnCopyRow As RadioButton
    Friend WithEvents RbnCopyColumn As RadioButton
    Friend WithEvents RbnCopyCell As RadioButton
End Class
