<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlImportAnyFld
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlImportAnyFld))
        Me.vsfExcel = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.txtSelect = New System.Windows.Forms.TextBox()
        Me.lblSelectFile = New System.Windows.Forms.Label()
        Me.btnSelect = New System.Windows.Forms.Button()
        Me.btnImportData = New System.Windows.Forms.Button()
        Me.chkItemGroup = New System.Windows.Forms.CheckBox()
        Me.chkItem = New System.Windows.Forms.CheckBox()
        Me.chkPacking = New System.Windows.Forms.CheckBox()
        Me.vsfFlds = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.pnlExcel = New System.Windows.Forms.Panel()
        Me.btnExportToXL = New System.Windows.Forms.Button()
        Me.btnTemplate = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnLoadtoGrd = New System.Windows.Forms.Button()
        CType(Me.vsfExcel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfFlds, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlExcel.SuspendLayout()
        Me.SuspendLayout()
        '
        'vsfExcel
        '
        Me.vsfExcel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfExcel.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfExcel.ColumnInfo = "10,0,0,0,0,120,Columns:1{Width:285;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfExcel.ExtendLastCol = True
        Me.vsfExcel.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfExcel.Location = New System.Drawing.Point(13, 17)
        Me.vsfExcel.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.vsfExcel.Name = "vsfExcel"
        Me.vsfExcel.Rows.DefaultSize = 28
        Me.vsfExcel.Size = New System.Drawing.Size(705, 532)
        Me.vsfExcel.StyleInfo = resources.GetString("vsfExcel.StyleInfo")
        Me.vsfExcel.TabIndex = 243
        '
        'txtSelect
        '
        Me.txtSelect.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSelect.Location = New System.Drawing.Point(565, 11)
        Me.txtSelect.Margin = New System.Windows.Forms.Padding(4)
        Me.txtSelect.MaxLength = 100
        Me.txtSelect.Name = "txtSelect"
        Me.txtSelect.Size = New System.Drawing.Size(533, 30)
        Me.txtSelect.TabIndex = 242
        '
        'lblSelectFile
        '
        Me.lblSelectFile.AutoSize = True
        Me.lblSelectFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSelectFile.Location = New System.Drawing.Point(401, 14)
        Me.lblSelectFile.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblSelectFile.Name = "lblSelectFile"
        Me.lblSelectFile.Size = New System.Drawing.Size(156, 25)
        Me.lblSelectFile.TabIndex = 241
        Me.lblSelectFile.Text = "Select Excel File"
        '
        'btnSelect
        '
        Me.btnSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSelect.Location = New System.Drawing.Point(1106, 11)
        Me.btnSelect.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSelect.Name = "btnSelect"
        Me.btnSelect.Size = New System.Drawing.Size(47, 32)
        Me.btnSelect.TabIndex = 240
        Me.btnSelect.Text = "..."
        Me.btnSelect.UseVisualStyleBackColor = True
        '
        'btnImportData
        '
        Me.btnImportData.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnImportData.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnImportData.Location = New System.Drawing.Point(584, 553)
        Me.btnImportData.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnImportData.Name = "btnImportData"
        Me.btnImportData.Size = New System.Drawing.Size(134, 39)
        Me.btnImportData.TabIndex = 244
        Me.btnImportData.Text = "Import Data"
        Me.btnImportData.UseVisualStyleBackColor = True
        '
        'chkItemGroup
        '
        Me.chkItemGroup.AutoSize = True
        Me.chkItemGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkItemGroup.Location = New System.Drawing.Point(12, 14)
        Me.chkItemGroup.Margin = New System.Windows.Forms.Padding(5)
        Me.chkItemGroup.Name = "chkItemGroup"
        Me.chkItemGroup.Size = New System.Drawing.Size(187, 28)
        Me.chkItemGroup.TabIndex = 267
        Me.chkItemGroup.Text = "Select Item Group "
        Me.chkItemGroup.UseVisualStyleBackColor = True
        '
        'chkItem
        '
        Me.chkItem.AutoSize = True
        Me.chkItem.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkItem.Location = New System.Drawing.Point(12, 87)
        Me.chkItem.Margin = New System.Windows.Forms.Padding(7, 6, 7, 6)
        Me.chkItem.Name = "chkItem"
        Me.chkItem.Size = New System.Drawing.Size(124, 28)
        Me.chkItem.TabIndex = 268
        Me.chkItem.Text = "Select Item"
        Me.chkItem.UseVisualStyleBackColor = True
        '
        'chkPacking
        '
        Me.chkPacking.AutoSize = True
        Me.chkPacking.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPacking.Location = New System.Drawing.Point(12, 50)
        Me.chkPacking.Margin = New System.Windows.Forms.Padding(7, 6, 7, 6)
        Me.chkPacking.Name = "chkPacking"
        Me.chkPacking.Size = New System.Drawing.Size(196, 28)
        Me.chkPacking.TabIndex = 269
        Me.chkPacking.Text = "Select Item Packing"
        Me.chkPacking.UseVisualStyleBackColor = True
        '
        'vsfFlds
        '
        Me.vsfFlds.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.vsfFlds.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfFlds.ColumnInfo = "10,0,0,0,0,145,Columns:0{Width:182;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfFlds.ExtendLastCol = True
        Me.vsfFlds.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.vsfFlds.Location = New System.Drawing.Point(12, 161)
        Me.vsfFlds.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.vsfFlds.Name = "vsfFlds"
        Me.vsfFlds.Rows.DefaultSize = 29
        Me.vsfFlds.Size = New System.Drawing.Size(408, 385)
        Me.vsfFlds.StyleInfo = resources.GetString("vsfFlds.StyleInfo")
        Me.vsfFlds.TabIndex = 270
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(13, 134)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(177, 20)
        Me.Label1.TabIndex = 271
        Me.Label1.Text = "Select Fields to import"
        '
        'pnlExcel
        '
        Me.pnlExcel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlExcel.Controls.Add(Me.btnExportToXL)
        Me.pnlExcel.Controls.Add(Me.vsfExcel)
        Me.pnlExcel.Controls.Add(Me.btnImportData)
        Me.pnlExcel.Location = New System.Drawing.Point(441, 50)
        Me.pnlExcel.Name = "pnlExcel"
        Me.pnlExcel.Size = New System.Drawing.Size(737, 598)
        Me.pnlExcel.TabIndex = 272
        Me.pnlExcel.Visible = False
        '
        'btnExportToXL
        '
        Me.btnExportToXL.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExportToXL.Image = CType(resources.GetObject("btnExportToXL.Image"), System.Drawing.Image)
        Me.btnExportToXL.Location = New System.Drawing.Point(13, 553)
        Me.btnExportToXL.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnExportToXL.Name = "btnExportToXL"
        Me.btnExportToXL.Size = New System.Drawing.Size(37, 37)
        Me.btnExportToXL.TabIndex = 253
        Me.btnExportToXL.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnExportToXL.UseVisualStyleBackColor = False
        '
        'btnTemplate
        '
        Me.btnTemplate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnTemplate.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTemplate.Location = New System.Drawing.Point(12, 551)
        Me.btnTemplate.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnTemplate.Name = "btnTemplate"
        Me.btnTemplate.Size = New System.Drawing.Size(243, 39)
        Me.btnTemplate.TabIndex = 273
        Me.btnTemplate.Text = "Generate Excel Template"
        Me.btnTemplate.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnClose.Location = New System.Drawing.Point(324, 552)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(96, 36)
        Me.btnClose.TabIndex = 274
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnLoadtoGrd
        '
        Me.btnLoadtoGrd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnLoadtoGrd.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoadtoGrd.Location = New System.Drawing.Point(12, 594)
        Me.btnLoadtoGrd.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnLoadtoGrd.Name = "btnLoadtoGrd"
        Me.btnLoadtoGrd.Size = New System.Drawing.Size(243, 39)
        Me.btnLoadtoGrd.TabIndex = 275
        Me.btnLoadtoGrd.Text = "Load to Grid"
        Me.btnLoadtoGrd.UseVisualStyleBackColor = True
        '
        'frmUtlImportAnyFld
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1183, 660)
        Me.Controls.Add(Me.btnLoadtoGrd)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.txtSelect)
        Me.Controls.Add(Me.lblSelectFile)
        Me.Controls.Add(Me.btnTemplate)
        Me.Controls.Add(Me.btnSelect)
        Me.Controls.Add(Me.pnlExcel)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.vsfFlds)
        Me.Controls.Add(Me.chkPacking)
        Me.Controls.Add(Me.chkItem)
        Me.Controls.Add(Me.chkItemGroup)
        Me.Name = "frmUtlImportAnyFld"
        Me.Text = "Import Any Field to Item Master"
        CType(Me.vsfExcel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfFlds, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlExcel.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents vsfExcel As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents txtSelect As TextBox
    Friend WithEvents lblSelectFile As Label
    Friend WithEvents btnSelect As Button
    Friend WithEvents btnImportData As Button
    Friend WithEvents chkItemGroup As CheckBox
    Friend WithEvents chkItem As CheckBox
    Friend WithEvents chkPacking As CheckBox
    Private WithEvents vsfFlds As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents Label1 As Label
    Friend WithEvents pnlExcel As Panel
    Friend WithEvents btnTemplate As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents btnExportToXL As Button
    Friend WithEvents btnLoadtoGrd As Button
End Class
