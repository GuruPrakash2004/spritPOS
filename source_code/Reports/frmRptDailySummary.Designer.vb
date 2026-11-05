<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRptDailySummary
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRptDailySummary))
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnView = New System.Windows.Forms.Button()
        Me.dtpDay = New System.Windows.Forms.DateTimePicker()
        Me.lblForTheDay = New System.Windows.Forms.Label()
        Me.vsfView = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnWAMsg = New System.Windows.Forms.Button()
        Me.btnEmailPDF = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnExportToXL = New System.Windows.Forms.Button()
        Me.txtEmoji = New System.Windows.Forms.TextBox()
        Me.btnEmail = New System.Windows.Forms.Button()
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
        Me.chkNonLiqour = New System.Windows.Forms.CheckBox()
        Me.optSalesRate = New System.Windows.Forms.RadioButton()
        Me.optPurRate = New System.Windows.Forms.RadioButton()
        Me.pnlOptions = New System.Windows.Forms.Panel()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlCopyShortCut.SuspendLayout()
        Me.pnlOptions.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(1075, 20)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(100, 37)
        Me.btnClose.TabIndex = 7
        Me.btnClose.Text = "&Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnView
        '
        Me.btnView.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnView.Location = New System.Drawing.Point(288, 15)
        Me.btnView.Margin = New System.Windows.Forms.Padding(4)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(100, 37)
        Me.btnView.TabIndex = 6
        Me.btnView.Text = "&View"
        Me.btnView.UseVisualStyleBackColor = True
        '
        'dtpDay
        '
        Me.dtpDay.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpDay.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpDay.Location = New System.Drawing.Point(147, 18)
        Me.dtpDay.Margin = New System.Windows.Forms.Padding(4)
        Me.dtpDay.Name = "dtpDay"
        Me.dtpDay.Size = New System.Drawing.Size(132, 30)
        Me.dtpDay.TabIndex = 5
        '
        'lblForTheDay
        '
        Me.lblForTheDay.AutoSize = True
        Me.lblForTheDay.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblForTheDay.Location = New System.Drawing.Point(16, 21)
        Me.lblForTheDay.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblForTheDay.Name = "lblForTheDay"
        Me.lblForTheDay.Size = New System.Drawing.Size(113, 25)
        Me.lblForTheDay.TabIndex = 4
        Me.lblForTheDay.Text = "For the Day"
        '
        'vsfView
        '
        Me.vsfView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfView.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfView.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfView.Location = New System.Drawing.Point(16, 105)
        Me.vsfView.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.vsfView.Name = "vsfView"
        Me.vsfView.Rows.DefaultSize = 28
        Me.vsfView.Size = New System.Drawing.Size(1157, 451)
        Me.vsfView.StyleInfo = resources.GetString("vsfView.StyleInfo")
        Me.vsfView.TabIndex = 255
        '
        'btnWAMsg
        '
        Me.btnWAMsg.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnWAMsg.BackgroundImage = Global.vLQMS.My.Resources.Resources.whatsapp_icon2
        Me.btnWAMsg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnWAMsg.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnWAMsg.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnWAMsg.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnWAMsg.Location = New System.Drawing.Point(789, 20)
        Me.btnWAMsg.Margin = New System.Windows.Forms.Padding(4)
        Me.btnWAMsg.Name = "btnWAMsg"
        Me.btnWAMsg.Size = New System.Drawing.Size(53, 37)
        Me.btnWAMsg.TabIndex = 283
        Me.btnWAMsg.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnWAMsg.UseVisualStyleBackColor = False
        '
        'btnEmailPDF
        '
        Me.btnEmailPDF.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEmailPDF.BackColor = System.Drawing.SystemColors.Control
        Me.btnEmailPDF.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnEmailPDF.Image = CType(resources.GetObject("btnEmailPDF.Image"), System.Drawing.Image)
        Me.btnEmailPDF.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnEmailPDF.Location = New System.Drawing.Point(851, 20)
        Me.btnEmailPDF.Margin = New System.Windows.Forms.Padding(4)
        Me.btnEmailPDF.Name = "btnEmailPDF"
        Me.btnEmailPDF.Size = New System.Drawing.Size(108, 37)
        Me.btnEmailPDF.TabIndex = 282
        Me.btnEmailPDF.Text = "Preview"
        Me.btnEmailPDF.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnEmailPDF.UseVisualStyleBackColor = False
        '
        'btnPrint
        '
        Me.btnPrint.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPrint.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrint.Image = CType(resources.GetObject("btnPrint.Image"), System.Drawing.Image)
        Me.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPrint.Location = New System.Drawing.Point(967, 20)
        Me.btnPrint.Margin = New System.Windows.Forms.Padding(4)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 37)
        Me.btnPrint.TabIndex = 254
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnPrint.UseVisualStyleBackColor = False
        '
        'btnExportToXL
        '
        Me.btnExportToXL.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExportToXL.Image = CType(resources.GetObject("btnExportToXL.Image"), System.Drawing.Image)
        Me.btnExportToXL.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExportToXL.Location = New System.Drawing.Point(677, 20)
        Me.btnExportToXL.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnExportToXL.Name = "btnExportToXL"
        Me.btnExportToXL.Size = New System.Drawing.Size(44, 37)
        Me.btnExportToXL.TabIndex = 252
        Me.btnExportToXL.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnExportToXL.UseVisualStyleBackColor = False
        '
        'txtEmoji
        '
        Me.txtEmoji.Location = New System.Drawing.Point(1025, 554)
        Me.txtEmoji.Margin = New System.Windows.Forms.Padding(4)
        Me.txtEmoji.Name = "txtEmoji"
        Me.txtEmoji.Size = New System.Drawing.Size(108, 22)
        Me.txtEmoji.TabIndex = 284
        Me.txtEmoji.Visible = False
        '
        'btnEmail
        '
        Me.btnEmail.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEmail.BackgroundImage = CType(resources.GetObject("btnEmail.BackgroundImage"), System.Drawing.Image)
        Me.btnEmail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnEmail.FlatAppearance.BorderSize = 0
        Me.btnEmail.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEmail.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEmail.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnEmail.Location = New System.Drawing.Point(728, 20)
        Me.btnEmail.Margin = New System.Windows.Forms.Padding(4)
        Me.btnEmail.Name = "btnEmail"
        Me.btnEmail.Size = New System.Drawing.Size(53, 37)
        Me.btnEmail.TabIndex = 285
        Me.btnEmail.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnEmail.UseVisualStyleBackColor = False
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
        Me.pnlCopyShortCut.Location = New System.Drawing.Point(304, 138)
        Me.pnlCopyShortCut.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlCopyShortCut.Name = "pnlCopyShortCut"
        Me.pnlCopyShortCut.Size = New System.Drawing.Size(439, 218)
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
        Me.btnCopy.Location = New System.Drawing.Point(215, 166)
        Me.btnCopy.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCopy.Name = "btnCopy"
        Me.btnCopy.Size = New System.Drawing.Size(93, 36)
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
        Me.pnlbtnClose.Location = New System.Drawing.Point(316, 166)
        Me.pnlbtnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlbtnClose.Name = "pnlbtnClose"
        Me.pnlbtnClose.Size = New System.Drawing.Size(93, 36)
        Me.pnlbtnClose.TabIndex = 22
        Me.pnlbtnClose.Text = "Cl&ose"
        Me.pnlbtnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.pnlbtnClose.UseVisualStyleBackColor = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(237, 134)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(123, 20)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "Ctrl + Shift + G"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(237, 97)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(108, 20)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Ctrl + Alt + C"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(237, 59)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(122, 20)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "Ctrl + Shift + C"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(237, 23)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(68, 20)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "Ctrl + C"
        '
        'RbnCopyGrid
        '
        Me.RbnCopyGrid.AutoSize = True
        Me.RbnCopyGrid.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RbnCopyGrid.Location = New System.Drawing.Point(21, 134)
        Me.RbnCopyGrid.Margin = New System.Windows.Forms.Padding(4)
        Me.RbnCopyGrid.Name = "RbnCopyGrid"
        Me.RbnCopyGrid.Size = New System.Drawing.Size(164, 24)
        Me.RbnCopyGrid.TabIndex = 3
        Me.RbnCopyGrid.TabStop = True
        Me.RbnCopyGrid.Text = "Copy Entire Grid  "
        Me.RbnCopyGrid.UseVisualStyleBackColor = True
        '
        'RbnCopyRow
        '
        Me.RbnCopyRow.AutoSize = True
        Me.RbnCopyRow.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RbnCopyRow.Location = New System.Drawing.Point(21, 95)
        Me.RbnCopyRow.Margin = New System.Windows.Forms.Padding(4)
        Me.RbnCopyRow.Name = "RbnCopyRow"
        Me.RbnCopyRow.Size = New System.Drawing.Size(160, 24)
        Me.RbnCopyRow.TabIndex = 2
        Me.RbnCopyRow.TabStop = True
        Me.RbnCopyRow.Text = "Copy Entire  Row"
        Me.RbnCopyRow.UseVisualStyleBackColor = True
        '
        'RbnCopyColumn
        '
        Me.RbnCopyColumn.AutoSize = True
        Me.RbnCopyColumn.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RbnCopyColumn.Location = New System.Drawing.Point(20, 57)
        Me.RbnCopyColumn.Margin = New System.Windows.Forms.Padding(4)
        Me.RbnCopyColumn.Name = "RbnCopyColumn"
        Me.RbnCopyColumn.Size = New System.Drawing.Size(179, 24)
        Me.RbnCopyColumn.TabIndex = 1
        Me.RbnCopyColumn.TabStop = True
        Me.RbnCopyColumn.Text = "Copy Entire Column"
        Me.RbnCopyColumn.UseVisualStyleBackColor = True
        '
        'RbnCopyCell
        '
        Me.RbnCopyCell.AutoSize = True
        Me.RbnCopyCell.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RbnCopyCell.Location = New System.Drawing.Point(20, 23)
        Me.RbnCopyCell.Margin = New System.Windows.Forms.Padding(4)
        Me.RbnCopyCell.Name = "RbnCopyCell"
        Me.RbnCopyCell.Size = New System.Drawing.Size(161, 24)
        Me.RbnCopyCell.TabIndex = 0
        Me.RbnCopyCell.TabStop = True
        Me.RbnCopyCell.Text = "Copy Data In Cell"
        Me.RbnCopyCell.UseVisualStyleBackColor = True
        '
        'chkNonLiqour
        '
        Me.chkNonLiqour.AutoSize = True
        Me.chkNonLiqour.Checked = True
        Me.chkNonLiqour.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkNonLiqour.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkNonLiqour.Location = New System.Drawing.Point(5, 5)
        Me.chkNonLiqour.Margin = New System.Windows.Forms.Padding(5)
        Me.chkNonLiqour.Name = "chkNonLiqour"
        Me.chkNonLiqour.Size = New System.Drawing.Size(234, 27)
        Me.chkNonLiqour.TabIndex = 299
        Me.chkNonLiqour.Text = "Include Non Liqour Items"
        Me.chkNonLiqour.UseVisualStyleBackColor = True
        '
        'optSalesRate
        '
        Me.optSalesRate.AutoSize = True
        Me.optSalesRate.Checked = True
        Me.optSalesRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optSalesRate.Location = New System.Drawing.Point(468, 4)
        Me.optSalesRate.Margin = New System.Windows.Forms.Padding(4)
        Me.optSalesRate.Name = "optSalesRate"
        Me.optSalesRate.Size = New System.Drawing.Size(128, 29)
        Me.optSalesRate.TabIndex = 303
        Me.optSalesRate.TabStop = True
        Me.optSalesRate.Text = "Sales Rate"
        Me.optSalesRate.UseVisualStyleBackColor = True
        '
        'optPurRate
        '
        Me.optPurRate.AutoSize = True
        Me.optPurRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optPurRate.Location = New System.Drawing.Point(243, 4)
        Me.optPurRate.Margin = New System.Windows.Forms.Padding(4)
        Me.optPurRate.Name = "optPurRate"
        Me.optPurRate.Size = New System.Drawing.Size(203, 29)
        Me.optPurRate.TabIndex = 302
        Me.optPurRate.Text = "Last Purchase Rate"
        Me.optPurRate.UseVisualStyleBackColor = True
        '
        'pnlOptions
        '
        Me.pnlOptions.Controls.Add(Me.chkNonLiqour)
        Me.pnlOptions.Controls.Add(Me.optSalesRate)
        Me.pnlOptions.Controls.Add(Me.optPurRate)
        Me.pnlOptions.Location = New System.Drawing.Point(16, 60)
        Me.pnlOptions.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlOptions.Name = "pnlOptions"
        Me.pnlOptions.Size = New System.Drawing.Size(609, 36)
        Me.pnlOptions.TabIndex = 299
        Me.pnlOptions.Visible = False
        '
        'frmRptDailySummary
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1191, 574)
        Me.Controls.Add(Me.pnlOptions)
        Me.Controls.Add(Me.pnlCopyShortCut)
        Me.Controls.Add(Me.btnEmail)
        Me.Controls.Add(Me.txtEmoji)
        Me.Controls.Add(Me.btnWAMsg)
        Me.Controls.Add(Me.btnEmailPDF)
        Me.Controls.Add(Me.vsfView)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.btnExportToXL)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnView)
        Me.Controls.Add(Me.dtpDay)
        Me.Controls.Add(Me.lblForTheDay)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmRptDailySummary"
        Me.ShowIcon = False
        Me.Text = "Daily Summary Report"
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlCopyShortCut.ResumeLayout(False)
        Me.pnlCopyShortCut.PerformLayout()
        Me.pnlOptions.ResumeLayout(False)
        Me.pnlOptions.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnClose As Button
    Friend WithEvents btnView As Button
    Friend WithEvents dtpDay As DateTimePicker
    Friend WithEvents lblForTheDay As Label
    Friend WithEvents btnExportToXL As Button
    Friend WithEvents btnPrint As Button
    Friend WithEvents vsfView As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnEmailPDF As Button
    Friend WithEvents btnWAMsg As Button
    Friend WithEvents txtEmoji As TextBox
    Friend WithEvents btnEmail As Button
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
    Friend WithEvents chkNonLiqour As CheckBox
    Friend WithEvents optSalesRate As RadioButton
    Friend WithEvents optPurRate As RadioButton
    Friend WithEvents pnlOptions As Panel
End Class
