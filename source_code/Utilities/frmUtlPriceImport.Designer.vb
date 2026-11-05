<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlPriceImport
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlPriceImport))
        Me.txtXLFilePath = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.btnTemplate = New System.Windows.Forms.Button()
        Me.btnImportFromXL = New System.Windows.Forms.Button()
        Me.btnRemErr = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnExportToXL = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.chkInvalidData = New System.Windows.Forms.CheckBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtNoOfRec = New System.Windows.Forms.TextBox()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtPCat = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtXLFilePath
        '
        Me.txtXLFilePath.Location = New System.Drawing.Point(116, 13)
        Me.txtXLFilePath.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtXLFilePath.Name = "txtXLFilePath"
        Me.txtXLFilePath.Size = New System.Drawing.Size(395, 25)
        Me.txtXLFilePath.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(98, 19)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "From Excel File"
        '
        'btnBrowse
        '
        Me.btnBrowse.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnBrowse.Location = New System.Drawing.Point(517, 11)
        Me.btnBrowse.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(40, 29)
        Me.btnBrowse.TabIndex = 2
        Me.btnBrowse.Text = "..."
        Me.btnBrowse.UseVisualStyleBackColor = False
        '
        'btnTemplate
        '
        Me.btnTemplate.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnTemplate.Image = CType(resources.GetObject("btnTemplate.Image"), System.Drawing.Image)
        Me.btnTemplate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnTemplate.Location = New System.Drawing.Point(708, 11)
        Me.btnTemplate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnTemplate.Name = "btnTemplate"
        Me.btnTemplate.Size = New System.Drawing.Size(99, 29)
        Me.btnTemplate.TabIndex = 3
        Me.btnTemplate.Text = "&Template"
        Me.btnTemplate.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnTemplate.UseVisualStyleBackColor = False
        '
        'btnImportFromXL
        '
        Me.btnImportFromXL.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnImportFromXL.Image = CType(resources.GetObject("btnImportFromXL.Image"), System.Drawing.Image)
        Me.btnImportFromXL.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnImportFromXL.Location = New System.Drawing.Point(563, 11)
        Me.btnImportFromXL.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnImportFromXL.Name = "btnImportFromXL"
        Me.btnImportFromXL.Size = New System.Drawing.Size(139, 29)
        Me.btnImportFromXL.TabIndex = 4
        Me.btnImportFromXL.Text = "&Load from Excel"
        Me.btnImportFromXL.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnImportFromXL.UseVisualStyleBackColor = False
        '
        'btnRemErr
        '
        Me.btnRemErr.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRemErr.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnRemErr.Image = CType(resources.GetObject("btnRemErr.Image"), System.Drawing.Image)
        Me.btnRemErr.ImageAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.btnRemErr.Location = New System.Drawing.Point(467, 534)
        Me.btnRemErr.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnRemErr.Name = "btnRemErr"
        Me.btnRemErr.Size = New System.Drawing.Size(203, 29)
        Me.btnRemErr.TabIndex = 5
        Me.btnRemErr.Text = "&Remove erroneous records"
        Me.btnRemErr.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRemErr.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(822, 534)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 29)
        Me.btnClose.TabIndex = 6
        Me.btnClose.Text = "&Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnSave.Image = CType(resources.GetObject("btnSave.Image"), System.Drawing.Image)
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(706, 497)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(191, 29)
        Me.btnSave.TabIndex = 7
        Me.btnSave.Text = "Import data to database"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnExportToXL
        '
        Me.btnExportToXL.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExportToXL.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnExportToXL.Image = CType(resources.GetObject("btnExportToXL.Image"), System.Drawing.Image)
        Me.btnExportToXL.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExportToXL.Location = New System.Drawing.Point(678, 534)
        Me.btnExportToXL.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnExportToXL.Name = "btnExportToXL"
        Me.btnExportToXL.Size = New System.Drawing.Size(136, 29)
        Me.btnExportToXL.TabIndex = 8
        Me.btnExportToXL.Text = "&Export to excel"
        Me.btnExportToXL.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnExportToXL.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(8, 502)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(354, 19)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Records displayed with        coloured font are erroneous."
        '
        'chkInvalidData
        '
        Me.chkInvalidData.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkInvalidData.AutoSize = True
        Me.chkInvalidData.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.chkInvalidData.Location = New System.Drawing.Point(12, 537)
        Me.chkInvalidData.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.chkInvalidData.Name = "chkInvalidData"
        Me.chkInvalidData.Size = New System.Drawing.Size(119, 23)
        Me.chkInvalidData.TabIndex = 11
        Me.chkInvalidData.Text = "Check if invalid"
        Me.chkInvalidData.UseVisualStyleBackColor = False
        Me.chkInvalidData.Visible = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(513, 62)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(176, 19)
        Me.Label3.TabIndex = 12
        Me.Label3.Text = "Number of records loaded: "
        '
        'txtNoOfRec
        '
        Me.txtNoOfRec.Location = New System.Drawing.Point(695, 59)
        Me.txtNoOfRec.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtNoOfRec.Name = "txtNoOfRec"
        Me.txtNoOfRec.ReadOnly = True
        Me.txtNoOfRec.Size = New System.Drawing.Size(77, 25)
        Me.txtNoOfRec.TabIndex = 13
        '
        'vsfDet
        '
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.ColumnInfo = resources.GetString("vsfDet.ColumnInfo")
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.Location = New System.Drawing.Point(16, 91)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(881, 406)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 14
        Me.vsfDet.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2010Silver
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.Label4.ForeColor = System.Drawing.Color.Red
        Me.Label4.Location = New System.Drawing.Point(151, 502)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(32, 19)
        Me.Label4.TabIndex = 16
        Me.Label4.Text = "red"
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.Label5.Location = New System.Drawing.Point(8, 539)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(370, 19)
        Me.Label5.TabIndex = 17
        Me.Label5.Text = "Importing of data is not allowed until all the data is correct."
        '
        'txtPCat
        '
        Me.txtPCat.Location = New System.Drawing.Point(155, 59)
        Me.txtPCat.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtPCat.Name = "txtPCat"
        Me.txtPCat.ReadOnly = True
        Me.txtPCat.Size = New System.Drawing.Size(352, 25)
        Me.txtPCat.TabIndex = 19
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.Label6.Location = New System.Drawing.Point(12, 62)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(138, 19)
        Me.Label6.TabIndex = 18
        Me.Label6.Text = "Price Category Name"
        '
        'frmUtlPriceImport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(909, 573)
        Me.Controls.Add(Me.txtPCat)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.txtNoOfRec)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.chkInvalidData)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnExportToXL)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnRemErr)
        Me.Controls.Add(Me.btnImportFromXL)
        Me.Controls.Add(Me.btnTemplate)
        Me.Controls.Add(Me.btnBrowse)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtXLFilePath)
        Me.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "frmUtlPriceImport"
        Me.ShowIcon = False
        Me.Text = "Import Price Data from XL"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtXLFilePath As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents btnTemplate As System.Windows.Forms.Button
    Friend WithEvents btnImportFromXL As System.Windows.Forms.Button
    Friend WithEvents btnRemErr As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnExportToXL As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chkInvalidData As System.Windows.Forms.CheckBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtNoOfRec As System.Windows.Forms.TextBox
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtPCat As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
End Class
