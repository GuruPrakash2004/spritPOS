<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmRptBrandWise
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRptBrandWise))
        Me.btnExportToXL = New System.Windows.Forms.Button()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnView = New System.Windows.Forms.Button()
        Me.chkBrand = New System.Windows.Forms.CheckBox()
        Me.chkItemGroup = New System.Windows.Forms.CheckBox()
        Me.gbDateRange = New System.Windows.Forms.GroupBox()
        Me.lblTo = New System.Windows.Forms.Label()
        Me.lblFrom = New System.Windows.Forms.Label()
        Me.dtpTo = New System.Windows.Forms.DateTimePicker()
        Me.dtpFrom = New System.Windows.Forms.DateTimePicker()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.optPurSal = New System.Windows.Forms.RadioButton()
        Me.optSales = New System.Windows.Forms.RadioButton()
        Me.optPur = New System.Windows.Forms.RadioButton()
        Me.ChkWithVal = New System.Windows.Forms.CheckBox()
        Me.cboRateType = New System.Windows.Forms.ComboBox()
        Me.pnlCopyShortCut = New System.Windows.Forms.Panel()
        Me.btnCopy = New System.Windows.Forms.Button()
        Me.pnlbtnClose = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.RbnCopyGrid = New System.Windows.Forms.RadioButton()
        Me.RbnCopyRow = New System.Windows.Forms.RadioButton()
        Me.RbnCopyColumn = New System.Windows.Forms.RadioButton()
        Me.RbnCopyCell = New System.Windows.Forms.RadioButton()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbDateRange.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.pnlCopyShortCut.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnExportToXL
        '
        Me.btnExportToXL.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExportToXL.FlatAppearance.BorderSize = 0
        Me.btnExportToXL.Image = CType(resources.GetObject("btnExportToXL.Image"), System.Drawing.Image)
        Me.btnExportToXL.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExportToXL.Location = New System.Drawing.Point(12, 542)
        Me.btnExportToXL.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnExportToXL.Name = "btnExportToXL"
        Me.btnExportToXL.Size = New System.Drawing.Size(33, 29)
        Me.btnExportToXL.TabIndex = 81
        Me.btnExportToXL.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnExportToXL.UseVisualStyleBackColor = False
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
        Me.vsfDet.Location = New System.Drawing.Point(12, 68)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.Count = 4
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(1093, 466)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 80
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnClose.FlatAppearance.BorderSize = 0
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnClose.Image = CType(resources.GetObject("btnClose.Image"), System.Drawing.Image)
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(1035, 543)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(70, 29)
        Me.btnClose.TabIndex = 79
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnView
        '
        Me.btnView.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnView.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnView.FlatAppearance.BorderSize = 0
        Me.btnView.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnView.Image = CType(resources.GetObject("btnView.Image"), System.Drawing.Image)
        Me.btnView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnView.Location = New System.Drawing.Point(1035, 34)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(70, 28)
        Me.btnView.TabIndex = 78
        Me.btnView.Text = "&View"
        Me.btnView.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnView.UseVisualStyleBackColor = False
        '
        'chkBrand
        '
        Me.chkBrand.AutoSize = True
        Me.chkBrand.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkBrand.Location = New System.Drawing.Point(516, 12)
        Me.chkBrand.Name = "chkBrand"
        Me.chkBrand.Size = New System.Drawing.Size(120, 24)
        Me.chkBrand.TabIndex = 77
        Me.chkBrand.Text = "Select Brand"
        Me.chkBrand.UseVisualStyleBackColor = True
        '
        'chkItemGroup
        '
        Me.chkItemGroup.AutoSize = True
        Me.chkItemGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkItemGroup.Location = New System.Drawing.Point(516, 36)
        Me.chkItemGroup.Name = "chkItemGroup"
        Me.chkItemGroup.Size = New System.Drawing.Size(154, 24)
        Me.chkItemGroup.TabIndex = 82
        Me.chkItemGroup.Text = "Select ItemGroup"
        Me.chkItemGroup.UseVisualStyleBackColor = True
        '
        'gbDateRange
        '
        Me.gbDateRange.Controls.Add(Me.lblTo)
        Me.gbDateRange.Controls.Add(Me.lblFrom)
        Me.gbDateRange.Controls.Add(Me.dtpTo)
        Me.gbDateRange.Controls.Add(Me.dtpFrom)
        Me.gbDateRange.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbDateRange.Location = New System.Drawing.Point(12, 3)
        Me.gbDateRange.Name = "gbDateRange"
        Me.gbDateRange.Size = New System.Drawing.Size(343, 58)
        Me.gbDateRange.TabIndex = 260
        Me.gbDateRange.TabStop = False
        Me.gbDateRange.Text = "For The Period"
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTo.Location = New System.Drawing.Point(182, 27)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(27, 20)
        Me.lblTo.TabIndex = 15
        Me.lblTo.Text = "To"
        '
        'lblFrom
        '
        Me.lblFrom.AutoSize = True
        Me.lblFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFrom.Location = New System.Drawing.Point(6, 27)
        Me.lblFrom.Name = "lblFrom"
        Me.lblFrom.Size = New System.Drawing.Size(46, 20)
        Me.lblFrom.TabIndex = 14
        Me.lblFrom.Text = "From"
        '
        'dtpTo
        '
        Me.dtpTo.CustomFormat = "dd/MM/yyyy"
        Me.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpTo.Location = New System.Drawing.Point(215, 25)
        Me.dtpTo.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Size = New System.Drawing.Size(118, 26)
        Me.dtpTo.TabIndex = 13
        '
        'dtpFrom
        '
        Me.dtpFrom.CustomFormat = "dd/MM/yyyy"
        Me.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpFrom.Location = New System.Drawing.Point(58, 26)
        Me.dtpFrom.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Size = New System.Drawing.Size(118, 26)
        Me.dtpFrom.TabIndex = 12
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.optPurSal)
        Me.Panel2.Controls.Add(Me.optSales)
        Me.Panel2.Controls.Add(Me.optPur)
        Me.Panel2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Panel2.Location = New System.Drawing.Point(361, 12)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(149, 50)
        Me.Panel2.TabIndex = 286
        '
        'optPurSal
        '
        Me.optPurSal.AutoSize = True
        Me.optPurSal.Location = New System.Drawing.Point(5, 25)
        Me.optPurSal.Name = "optPurSal"
        Me.optPurSal.Size = New System.Drawing.Size(133, 20)
        Me.optPurSal.TabIndex = 6
        Me.optPurSal.TabStop = True
        Me.optPurSal.Text = "Purchase Vs Sale"
        Me.optPurSal.UseVisualStyleBackColor = True
        '
        'optSales
        '
        Me.optSales.AutoSize = True
        Me.optSales.Location = New System.Drawing.Point(94, 4)
        Me.optSales.Name = "optSales"
        Me.optSales.Size = New System.Drawing.Size(54, 20)
        Me.optSales.TabIndex = 5
        Me.optSales.TabStop = True
        Me.optSales.Text = "Sale"
        Me.optSales.UseVisualStyleBackColor = True
        '
        'optPur
        '
        Me.optPur.AutoSize = True
        Me.optPur.Checked = True
        Me.optPur.Location = New System.Drawing.Point(5, 4)
        Me.optPur.Name = "optPur"
        Me.optPur.Size = New System.Drawing.Size(83, 20)
        Me.optPur.TabIndex = 1
        Me.optPur.TabStop = True
        Me.optPur.Text = "Purchase"
        Me.optPur.UseVisualStyleBackColor = True
        '
        'ChkWithVal
        '
        Me.ChkWithVal.AutoSize = True
        Me.ChkWithVal.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkWithVal.Location = New System.Drawing.Point(676, 32)
        Me.ChkWithVal.Name = "ChkWithVal"
        Me.ChkWithVal.Size = New System.Drawing.Size(105, 24)
        Me.ChkWithVal.TabIndex = 287
        Me.ChkWithVal.Text = "With Value"
        Me.ChkWithVal.UseVisualStyleBackColor = True
        '
        'cboRateType
        '
        Me.cboRateType.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboRateType.FormattingEnabled = True
        Me.cboRateType.Location = New System.Drawing.Point(787, 31)
        Me.cboRateType.Name = "cboRateType"
        Me.cboRateType.Size = New System.Drawing.Size(91, 24)
        Me.cboRateType.TabIndex = 288
        Me.cboRateType.Visible = False
        '
        'pnlCopyShortCut
        '
        Me.pnlCopyShortCut.Controls.Add(Me.btnCopy)
        Me.pnlCopyShortCut.Controls.Add(Me.pnlbtnClose)
        Me.pnlCopyShortCut.Controls.Add(Me.Label5)
        Me.pnlCopyShortCut.Controls.Add(Me.Label4)
        Me.pnlCopyShortCut.Controls.Add(Me.Label6)
        Me.pnlCopyShortCut.Controls.Add(Me.Label7)
        Me.pnlCopyShortCut.Controls.Add(Me.RbnCopyGrid)
        Me.pnlCopyShortCut.Controls.Add(Me.RbnCopyRow)
        Me.pnlCopyShortCut.Controls.Add(Me.RbnCopyColumn)
        Me.pnlCopyShortCut.Controls.Add(Me.RbnCopyCell)
        Me.pnlCopyShortCut.Location = New System.Drawing.Point(399, 124)
        Me.pnlCopyShortCut.Name = "pnlCopyShortCut"
        Me.pnlCopyShortCut.Size = New System.Drawing.Size(329, 177)
        Me.pnlCopyShortCut.TabIndex = 298
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
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(178, 48)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(87, 16)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "Ctrl + Shift + C"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(178, 19)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(49, 16)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "Ctrl + C"
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
        'frmRptBrandWise
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1117, 584)
        Me.Controls.Add(Me.pnlCopyShortCut)
        Me.Controls.Add(Me.cboRateType)
        Me.Controls.Add(Me.ChkWithVal)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.gbDateRange)
        Me.Controls.Add(Me.chkItemGroup)
        Me.Controls.Add(Me.btnExportToXL)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnView)
        Me.Controls.Add(Me.chkBrand)
        Me.Name = "frmRptBrandWise"
        Me.ShowIcon = False
        Me.Text = "Brand Wise Report"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbDateRange.ResumeLayout(False)
        Me.gbDateRange.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.pnlCopyShortCut.ResumeLayout(False)
        Me.pnlCopyShortCut.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnExportToXL As Button
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnClose As Button
    Friend WithEvents btnView As Button
    Friend WithEvents chkBrand As CheckBox
    Friend WithEvents chkItemGroup As CheckBox
    Friend WithEvents gbDateRange As GroupBox
    Friend WithEvents lblTo As Label
    Friend WithEvents lblFrom As Label
    Friend WithEvents dtpTo As DateTimePicker
    Friend WithEvents dtpFrom As DateTimePicker
    Friend WithEvents Panel2 As Panel
    Friend WithEvents optSales As RadioButton
    Friend WithEvents optPur As RadioButton
    Friend WithEvents optPurSal As RadioButton
    Friend WithEvents ChkWithVal As CheckBox
    Friend WithEvents cboRateType As ComboBox
    Friend WithEvents pnlCopyShortCut As Panel
    Friend WithEvents btnCopy As Button
    Friend WithEvents pnlbtnClose As Button
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents RbnCopyGrid As RadioButton
    Friend WithEvents RbnCopyRow As RadioButton
    Friend WithEvents RbnCopyColumn As RadioButton
    Friend WithEvents RbnCopyCell As RadioButton
End Class
