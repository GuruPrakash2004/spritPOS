<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmTrnPSE
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTrnPSE))
        Me.btnExportToXL = New System.Windows.Forms.Button()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnFind = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnView = New System.Windows.Forms.Button()
        Me.chkItemGroup = New System.Windows.Forms.CheckBox()
        Me.chkPkg = New System.Windows.Forms.CheckBox()
        Me.txtDummy2 = New System.Windows.Forms.TextBox()
        Me.txtDummy = New System.Windows.Forms.TextBox()
        Me.txtDocID = New System.Windows.Forms.TextBox()
        Me.lblDocNo = New System.Windows.Forms.Label()
        Me.vsfTotal = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.chkItem = New System.Windows.Forms.CheckBox()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.fraSummary = New System.Windows.Forms.Panel()
        Me.lblSumm = New System.Windows.Forms.Label()
        Me.btnCloseSumm = New System.Windows.Forms.Button()
        Me.btnExpToXlSumm = New System.Windows.Forms.Button()
        Me.pnlDocNo = New System.Windows.Forms.Panel()
        Me.txtPSEid = New System.Windows.Forms.TextBox()
        Me.btnShowSumm = New System.Windows.Forms.Button()
        Me.chkSales = New System.Windows.Forms.CheckBox()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraSummary.SuspendLayout()
        Me.pnlDocNo.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnExportToXL
        '
        Me.btnExportToXL.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExportToXL.Image = CType(resources.GetObject("btnExportToXL.Image"), System.Drawing.Image)
        Me.btnExportToXL.Location = New System.Drawing.Point(633, 468)
        Me.btnExportToXL.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnExportToXL.Name = "btnExportToXL"
        Me.btnExportToXL.Size = New System.Drawing.Size(33, 32)
        Me.btnExportToXL.TabIndex = 252
        Me.btnExportToXL.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnExportToXL.UseVisualStyleBackColor = False
        Me.btnExportToXL.Visible = False
        '
        'vsfDet
        '
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
        Me.vsfDet.ColumnInfo = resources.GetString("vsfDet.ColumnInfo")
        Me.vsfDet.EditOptions = C1.Win.C1FlexGrid.EditFlags.ExitOnLeftRightKeys
        Me.vsfDet.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfDet.Location = New System.Drawing.Point(3, 54)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.Rows.GlyphRow = 0
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.ShowCellLabels = True
        Me.vsfDet.Size = New System.Drawing.Size(1072, 412)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 286
        Me.vsfDet.Tag = ""
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.AutoSize = True
        Me.btnClose.BackColor = System.Drawing.SystemColors.MenuBar
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnClose.ForeColor = System.Drawing.Color.Black
        Me.btnClose.Image = CType(resources.GetObject("btnClose.Image"), System.Drawing.Image)
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(984, 470)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(90, 30)
        Me.btnClose.TabIndex = 287
        Me.btnClose.Tag = "Close"
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnFind
        '
        Me.btnFind.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFind.AutoSize = True
        Me.btnFind.BackColor = System.Drawing.SystemColors.MenuBar
        Me.btnFind.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnFind.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnFind.ForeColor = System.Drawing.Color.Black
        Me.btnFind.Image = CType(resources.GetObject("btnFind.Image"), System.Drawing.Image)
        Me.btnFind.Location = New System.Drawing.Point(953, 470)
        Me.btnFind.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.btnFind.Name = "btnFind"
        Me.btnFind.Size = New System.Drawing.Size(28, 30)
        Me.btnFind.TabIndex = 288
        Me.btnFind.Tag = "Find"
        Me.btnFind.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnFind.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.AutoSize = True
        Me.btnSave.BackColor = System.Drawing.SystemColors.MenuBar
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnSave.ForeColor = System.Drawing.Color.Black
        Me.btnSave.Image = CType(resources.GetObject("btnSave.Image"), System.Drawing.Image)
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(669, 470)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(90, 30)
        Me.btnSave.TabIndex = 289
        Me.btnSave.Tag = "Save"
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = False
        Me.btnSave.Visible = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.AutoSize = True
        Me.btnCancel.BackColor = System.Drawing.SystemColors.MenuBar
        Me.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnCancel.CausesValidation = False
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnCancel.ForeColor = System.Drawing.Color.Black
        Me.btnCancel.Image = CType(resources.GetObject("btnCancel.Image"), System.Drawing.Image)
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(765, 470)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(90, 30)
        Me.btnCancel.TabIndex = 290
        Me.btnCancel.Tag = "Cancel"
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancel.UseVisualStyleBackColor = False
        Me.btnCancel.Visible = False
        '
        'btnView
        '
        Me.btnView.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnView.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnView.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnView.Image = Global.vLQMS.My.Resources.Resources.DownLoadImg
        Me.btnView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnView.Location = New System.Drawing.Point(295, 19)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(85, 28)
        Me.btnView.TabIndex = 291
        Me.btnView.Text = "&Load"
        Me.btnView.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnView.UseVisualStyleBackColor = False
        '
        'chkItemGroup
        '
        Me.chkItemGroup.AutoSize = True
        Me.chkItemGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkItemGroup.Location = New System.Drawing.Point(3, 5)
        Me.chkItemGroup.Name = "chkItemGroup"
        Me.chkItemGroup.Size = New System.Drawing.Size(121, 20)
        Me.chkItemGroup.TabIndex = 292
        Me.chkItemGroup.Text = "Selected Group"
        Me.chkItemGroup.UseVisualStyleBackColor = True
        '
        'chkPkg
        '
        Me.chkPkg.AutoSize = True
        Me.chkPkg.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPkg.Location = New System.Drawing.Point(3, 29)
        Me.chkPkg.Name = "chkPkg"
        Me.chkPkg.Size = New System.Drawing.Size(139, 20)
        Me.chkPkg.TabIndex = 293
        Me.chkPkg.Text = "Selected Package"
        Me.chkPkg.UseVisualStyleBackColor = True
        '
        'txtDummy2
        '
        Me.txtDummy2.Location = New System.Drawing.Point(410, 12)
        Me.txtDummy2.Name = "txtDummy2"
        Me.txtDummy2.Size = New System.Drawing.Size(28, 20)
        Me.txtDummy2.TabIndex = 582
        Me.txtDummy2.Visible = False
        '
        'txtDummy
        '
        Me.txtDummy.Location = New System.Drawing.Point(376, 12)
        Me.txtDummy.Name = "txtDummy"
        Me.txtDummy.Size = New System.Drawing.Size(28, 20)
        Me.txtDummy.TabIndex = 581
        Me.txtDummy.Visible = False
        '
        'txtDocID
        '
        Me.txtDocID.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDocID.BackColor = System.Drawing.Color.Khaki
        Me.txtDocID.Enabled = False
        Me.txtDocID.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDocID.Location = New System.Drawing.Point(80, 5)
        Me.txtDocID.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtDocID.Name = "txtDocID"
        Me.txtDocID.Size = New System.Drawing.Size(128, 33)
        Me.txtDocID.TabIndex = 583
        Me.txtDocID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblDocNo
        '
        Me.lblDocNo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDocNo.AutoSize = True
        Me.lblDocNo.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lblDocNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lblDocNo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblDocNo.Location = New System.Drawing.Point(14, 15)
        Me.lblDocNo.Name = "lblDocNo"
        Me.lblDocNo.Size = New System.Drawing.Size(60, 16)
        Me.lblDocNo.TabIndex = 584
        Me.lblDocNo.Text = "Doc No"
        '
        'vsfTotal
        '
        Me.vsfTotal.AllowFiltering = True
        Me.vsfTotal.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfTotal.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfTotal.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfTotal.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfTotal.AutoResize = True
        Me.vsfTotal.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfTotal.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfTotal.ColumnInfo = resources.GetString("vsfTotal.ColumnInfo")
        Me.vsfTotal.EditOptions = C1.Win.C1FlexGrid.EditFlags.ExitOnLeftRightKeys
        Me.vsfTotal.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfTotal.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfTotal.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfTotal.Location = New System.Drawing.Point(13, 32)
        Me.vsfTotal.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfTotal.Name = "vsfTotal"
        Me.vsfTotal.Rows.DefaultSize = 24
        Me.vsfTotal.Rows.GlyphRow = 0
        Me.vsfTotal.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfTotal.ShowCellLabels = True
        Me.vsfTotal.Size = New System.Drawing.Size(899, 349)
        Me.vsfTotal.StyleInfo = resources.GetString("vsfTotal.StyleInfo")
        Me.vsfTotal.TabIndex = 585
        Me.vsfTotal.Tag = ""
        '
        'chkItem
        '
        Me.chkItem.AutoSize = True
        Me.chkItem.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkItem.Location = New System.Drawing.Point(141, 29)
        Me.chkItem.Name = "chkItem"
        Me.chkItem.Size = New System.Drawing.Size(109, 20)
        Me.chkItem.TabIndex = 586
        Me.chkItem.Text = "Selected Item"
        Me.chkItem.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPrint.AutoSize = True
        Me.btnPrint.BackColor = System.Drawing.SystemColors.MenuBar
        Me.btnPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPrint.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnPrint.ForeColor = System.Drawing.Color.Black
        Me.btnPrint.Image = CType(resources.GetObject("btnPrint.Image"), System.Drawing.Image)
        Me.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPrint.Location = New System.Drawing.Point(859, 470)
        Me.btnPrint.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(90, 30)
        Me.btnPrint.TabIndex = 587
        Me.btnPrint.Tag = "Print"
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnPrint.UseVisualStyleBackColor = False
        Me.btnPrint.Visible = False
        '
        'btnUpdate
        '
        Me.btnUpdate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnUpdate.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnUpdate.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnUpdate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnUpdate.Location = New System.Drawing.Point(3, 470)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(159, 28)
        Me.btnUpdate.TabIndex = 588
        Me.btnUpdate.Text = "Clear Physical Stock"
        Me.btnUpdate.UseVisualStyleBackColor = False
        Me.btnUpdate.Visible = False
        '
        'fraSummary
        '
        Me.fraSummary.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fraSummary.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.fraSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.fraSummary.Controls.Add(Me.lblSumm)
        Me.fraSummary.Controls.Add(Me.btnCloseSumm)
        Me.fraSummary.Controls.Add(Me.btnExpToXlSumm)
        Me.fraSummary.Controls.Add(Me.vsfTotal)
        Me.fraSummary.Location = New System.Drawing.Point(48, 39)
        Me.fraSummary.Name = "fraSummary"
        Me.fraSummary.Size = New System.Drawing.Size(933, 428)
        Me.fraSummary.TabIndex = 589
        '
        'lblSumm
        '
        Me.lblSumm.AutoSize = True
        Me.lblSumm.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lblSumm.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblSumm.ForeColor = System.Drawing.Color.Black
        Me.lblSumm.Location = New System.Drawing.Point(13, 9)
        Me.lblSumm.Name = "lblSumm"
        Me.lblSumm.Size = New System.Drawing.Size(128, 17)
        Me.lblSumm.TabIndex = 588
        Me.lblSumm.Text = "Summary Report"
        '
        'btnCloseSumm
        '
        Me.btnCloseSumm.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCloseSumm.AutoSize = True
        Me.btnCloseSumm.BackColor = System.Drawing.SystemColors.MenuBar
        Me.btnCloseSumm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnCloseSumm.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCloseSumm.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnCloseSumm.ForeColor = System.Drawing.Color.Black
        Me.btnCloseSumm.Image = CType(resources.GetObject("btnCloseSumm.Image"), System.Drawing.Image)
        Me.btnCloseSumm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCloseSumm.Location = New System.Drawing.Point(850, 389)
        Me.btnCloseSumm.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.btnCloseSumm.Name = "btnCloseSumm"
        Me.btnCloseSumm.Size = New System.Drawing.Size(74, 32)
        Me.btnCloseSumm.TabIndex = 587
        Me.btnCloseSumm.Tag = "Close"
        Me.btnCloseSumm.Text = "Cl&ose"
        Me.btnCloseSumm.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCloseSumm.UseVisualStyleBackColor = False
        '
        'btnExpToXlSumm
        '
        Me.btnExpToXlSumm.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExpToXlSumm.Image = CType(resources.GetObject("btnExpToXlSumm.Image"), System.Drawing.Image)
        Me.btnExpToXlSumm.Location = New System.Drawing.Point(13, 389)
        Me.btnExpToXlSumm.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnExpToXlSumm.Name = "btnExpToXlSumm"
        Me.btnExpToXlSumm.Size = New System.Drawing.Size(31, 32)
        Me.btnExpToXlSumm.TabIndex = 586
        Me.btnExpToXlSumm.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnExpToXlSumm.UseVisualStyleBackColor = False
        '
        'pnlDocNo
        '
        Me.pnlDocNo.Controls.Add(Me.lblDocNo)
        Me.pnlDocNo.Controls.Add(Me.txtDocID)
        Me.pnlDocNo.Location = New System.Drawing.Point(750, 5)
        Me.pnlDocNo.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.pnlDocNo.Name = "pnlDocNo"
        Me.pnlDocNo.Size = New System.Drawing.Size(214, 42)
        Me.pnlDocNo.TabIndex = 590
        Me.pnlDocNo.Visible = False
        '
        'txtPSEid
        '
        Me.txtPSEid.Location = New System.Drawing.Point(443, 12)
        Me.txtPSEid.Name = "txtPSEid"
        Me.txtPSEid.Size = New System.Drawing.Size(28, 20)
        Me.txtPSEid.TabIndex = 591
        Me.txtPSEid.Visible = False
        '
        'btnShowSumm
        '
        Me.btnShowSumm.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnShowSumm.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnShowSumm.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnShowSumm.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnShowSumm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnShowSumm.Location = New System.Drawing.Point(168, 470)
        Me.btnShowSumm.Name = "btnShowSumm"
        Me.btnShowSumm.Size = New System.Drawing.Size(138, 28)
        Me.btnShowSumm.TabIndex = 592
        Me.btnShowSumm.Text = "Show Summary"
        Me.btnShowSumm.UseVisualStyleBackColor = False
        Me.btnShowSumm.Visible = False
        '
        'chkSales
        '
        Me.chkSales.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkSales.AutoSize = True
        Me.chkSales.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSales.ForeColor = System.Drawing.Color.Red
        Me.chkSales.Location = New System.Drawing.Point(342, 475)
        Me.chkSales.Name = "chkSales"
        Me.chkSales.Size = New System.Drawing.Size(198, 20)
        Me.chkSales.TabIndex = 593
        Me.chkSales.Text = "Show Reduced Qty as Sales"
        Me.chkSales.UseVisualStyleBackColor = True
        '
        'frmTrnPSE
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1077, 503)
        Me.Controls.Add(Me.chkSales)
        Me.Controls.Add(Me.fraSummary)
        Me.Controls.Add(Me.btnShowSumm)
        Me.Controls.Add(Me.txtPSEid)
        Me.Controls.Add(Me.pnlDocNo)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.chkItem)
        Me.Controls.Add(Me.txtDummy2)
        Me.Controls.Add(Me.txtDummy)
        Me.Controls.Add(Me.chkPkg)
        Me.Controls.Add(Me.chkItemGroup)
        Me.Controls.Add(Me.btnView)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnFind)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.btnExportToXL)
        Me.Name = "frmTrnPSE"
        Me.ShowIcon = False
        Me.Text = "Physical Stock Entry"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfTotal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraSummary.ResumeLayout(False)
        Me.fraSummary.PerformLayout()
        Me.pnlDocNo.ResumeLayout(False)
        Me.pnlDocNo.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnExportToXL As Button
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnClose As Button
    Friend WithEvents btnFind As Button
    Friend WithEvents btnSave As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnView As Button
    Friend WithEvents chkItemGroup As CheckBox
    Friend WithEvents chkPkg As CheckBox
    Public WithEvents txtDummy2 As TextBox
    Public WithEvents txtDummy As TextBox
    Friend WithEvents txtDocID As TextBox
    Friend WithEvents lblDocNo As Label
    Friend WithEvents vsfTotal As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents chkItem As CheckBox
    Friend WithEvents btnPrint As Button
    Friend WithEvents btnUpdate As Button
    Friend WithEvents fraSummary As Panel
    Friend WithEvents btnCloseSumm As Button
    Friend WithEvents btnExpToXlSumm As Button
    Friend WithEvents pnlDocNo As Panel
    Public WithEvents txtPSEid As TextBox
    Friend WithEvents btnShowSumm As Button
    Friend WithEvents lblSumm As Label
    Friend WithEvents chkSales As CheckBox
End Class
