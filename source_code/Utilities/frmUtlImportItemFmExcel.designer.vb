<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlImportItemFmExcel
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlImportItemFmExcel))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtXLFilePath = New System.Windows.Forms.TextBox()
        Me.txtNoOfInv = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.vsfTmp = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.lblError = New System.Windows.Forms.Label()
        Me.Cdlg = New System.Windows.Forms.OpenFileDialog()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.optAppnd = New System.Windows.Forms.RadioButton()
        Me.optDel = New System.Windows.Forms.RadioButton()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnImportFromXL = New System.Windows.Forms.Button()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.vsfExcel = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnExportToXL = New System.Windows.Forms.Button()
        Me.btnRemErr = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnImpClose = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.vsfTmp, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfExcel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(21, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(107, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Select Excel File"
        '
        'txtXLFilePath
        '
        Me.txtXLFilePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtXLFilePath.Enabled = False
        Me.txtXLFilePath.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtXLFilePath.Location = New System.Drawing.Point(125, 15)
        Me.txtXLFilePath.Name = "txtXLFilePath"
        Me.txtXLFilePath.Size = New System.Drawing.Size(316, 26)
        Me.txtXLFilePath.TabIndex = 1
        '
        'txtNoOfInv
        '
        Me.txtNoOfInv.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtNoOfInv.Enabled = False
        Me.txtNoOfInv.Location = New System.Drawing.Point(242, 77)
        Me.txtNoOfInv.Name = "txtNoOfInv"
        Me.txtNoOfInv.Size = New System.Drawing.Size(59, 20)
        Me.txtNoOfInv.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(21, 81)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(215, 16)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Total Number of Records Loaded :"
        '
        'vsfTmp
        '
        Me.vsfTmp.ColumnInfo = "10,1,0,0,0,95,Columns:"
        Me.vsfTmp.Location = New System.Drawing.Point(957, 12)
        Me.vsfTmp.Name = "vsfTmp"
        Me.vsfTmp.Rows.DefaultSize = 19
        Me.vsfTmp.Size = New System.Drawing.Size(58, 36)
        Me.vsfTmp.TabIndex = 9
        Me.vsfTmp.Visible = False
        '
        'lblError
        '
        Me.lblError.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblError.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblError.ForeColor = System.Drawing.Color.Red
        Me.lblError.Location = New System.Drawing.Point(31, 566)
        Me.lblError.Name = "lblError"
        Me.lblError.Size = New System.Drawing.Size(10, 13)
        Me.lblError.TabIndex = 10
        '
        'Cdlg
        '
        Me.Cdlg.FileName = "Cdlg"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Blue
        Me.Label4.Location = New System.Drawing.Point(123, 48)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(319, 15)
        Me.Label4.TabIndex = 16
        Me.Label4.Text = "Please select a excel file containing Item Details"
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.Color.Red
        Me.lblStatus.Location = New System.Drawing.Point(635, 88)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(0, 13)
        Me.lblStatus.TabIndex = 18
        '
        'optAppnd
        '
        Me.optAppnd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.optAppnd.AutoSize = True
        Me.optAppnd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optAppnd.ForeColor = System.Drawing.Color.Red
        Me.optAppnd.Location = New System.Drawing.Point(281, 80)
        Me.optAppnd.Name = "optAppnd"
        Me.optAppnd.Size = New System.Drawing.Size(152, 17)
        Me.optAppnd.TabIndex = 20
        Me.optAppnd.Text = "Append Previous Data"
        Me.optAppnd.UseVisualStyleBackColor = True
        '
        'optDel
        '
        Me.optDel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.optDel.AutoSize = True
        Me.optDel.Checked = True
        Me.optDel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optDel.ForeColor = System.Drawing.Color.Red
        Me.optDel.Location = New System.Drawing.Point(448, 80)
        Me.optDel.Name = "optDel"
        Me.optDel.Size = New System.Drawing.Size(146, 17)
        Me.optDel.TabIndex = 21
        Me.optDel.TabStop = True
        Me.optDel.Text = "Delete Previous Data"
        Me.optDel.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnExport.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExport.Image = CType(resources.GetObject("btnExport.Image"), System.Drawing.Image)
        Me.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExport.Location = New System.Drawing.Point(638, 14)
        Me.btnExport.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(98, 29)
        Me.btnExport.TabIndex = 26
        Me.btnExport.Text = "&Template"
        Me.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnExport.UseVisualStyleBackColor = False
        '
        'btnImportFromXL
        '
        Me.btnImportFromXL.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnImportFromXL.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnImportFromXL.Image = CType(resources.GetObject("btnImportFromXL.Image"), System.Drawing.Image)
        Me.btnImportFromXL.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnImportFromXL.Location = New System.Drawing.Point(493, 14)
        Me.btnImportFromXL.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnImportFromXL.Name = "btnImportFromXL"
        Me.btnImportFromXL.Size = New System.Drawing.Size(139, 29)
        Me.btnImportFromXL.TabIndex = 25
        Me.btnImportFromXL.Text = "&Load from Excel"
        Me.btnImportFromXL.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnImportFromXL.UseVisualStyleBackColor = False
        '
        'btnBrowse
        '
        Me.btnBrowse.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnBrowse.Location = New System.Drawing.Point(447, 13)
        Me.btnBrowse.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(40, 29)
        Me.btnBrowse.TabIndex = 24
        Me.btnBrowse.Text = "..."
        Me.btnBrowse.UseVisualStyleBackColor = False
        '
        'vsfExcel
        '
        Me.vsfExcel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfExcel.ColumnInfo = resources.GetString("vsfExcel.ColumnInfo")
        Me.vsfExcel.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfExcel.Location = New System.Drawing.Point(24, 108)
        Me.vsfExcel.Name = "vsfExcel"
        Me.vsfExcel.Rows.DefaultSize = 24
        Me.vsfExcel.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfExcel.Size = New System.Drawing.Size(838, 406)
        Me.vsfExcel.StyleInfo = resources.GetString("vsfExcel.StyleInfo")
        Me.vsfExcel.TabIndex = 27
        Me.vsfExcel.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2010Silver
        '
        'btnExportToXL
        '
        Me.btnExportToXL.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExportToXL.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnExportToXL.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExportToXL.Image = CType(resources.GetObject("btnExportToXL.Image"), System.Drawing.Image)
        Me.btnExportToXL.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExportToXL.Location = New System.Drawing.Point(645, 558)
        Me.btnExportToXL.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnExportToXL.Name = "btnExportToXL"
        Me.btnExportToXL.Size = New System.Drawing.Size(130, 29)
        Me.btnExportToXL.TabIndex = 30
        Me.btnExportToXL.Text = "&Export to excel"
        Me.btnExportToXL.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnExportToXL.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ToolTip1.SetToolTip(Me.btnExportToXL, "ExportToXL:Alt+E")
        Me.btnExportToXL.UseCompatibleTextRendering = True
        Me.btnExportToXL.UseVisualStyleBackColor = False
        '
        'btnRemErr
        '
        Me.btnRemErr.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRemErr.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnRemErr.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRemErr.Image = CType(resources.GetObject("btnRemErr.Image"), System.Drawing.Image)
        Me.btnRemErr.ImageAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.btnRemErr.Location = New System.Drawing.Point(436, 558)
        Me.btnRemErr.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnRemErr.Name = "btnRemErr"
        Me.btnRemErr.Size = New System.Drawing.Size(203, 29)
        Me.btnRemErr.TabIndex = 29
        Me.btnRemErr.Text = "&Remove erroneous records"
        Me.btnRemErr.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRemErr.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Image = CType(resources.GetObject("btnSave.Image"), System.Drawing.Image)
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(678, 523)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(184, 29)
        Me.btnSave.TabIndex = 28
        Me.btnSave.Text = "Transfer to Main Page"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnImpClose
        '
        Me.btnImpClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnImpClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnImpClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnImpClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnImpClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnImpClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnImpClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnImpClose.Location = New System.Drawing.Point(781, 558)
        Me.btnImpClose.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnImpClose.Name = "btnImpClose"
        Me.btnImpClose.Size = New System.Drawing.Size(81, 29)
        Me.btnImpClose.TabIndex = 31
        Me.btnImpClose.Text = "&Close"
        Me.btnImpClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnImpClose, "Close: Alt + O")
        Me.btnImpClose.UseVisualStyleBackColor = False
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(21, 566)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(345, 16)
        Me.Label5.TabIndex = 33
        Me.Label5.Text = "Importing of data is not allowed until all the data is correct."
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(21, 529)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(348, 16)
        Me.Label3.TabIndex = 32
        Me.Label3.Text = "Records displayed with           coloured font are erroneous."
        '
        'Label6
        '
        Me.Label6.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.Label6.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.Label6.ForeColor = System.Drawing.Color.Red
        Me.Label6.Location = New System.Drawing.Point(164, 527)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(32, 19)
        Me.Label6.TabIndex = 34
        Me.Label6.Text = "red"
        '
        'frmUtlImportItemFmExcel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(874, 598)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.btnImpClose)
        Me.Controls.Add(Me.btnExportToXL)
        Me.Controls.Add(Me.btnRemErr)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.vsfExcel)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.btnImportFromXL)
        Me.Controls.Add(Me.btnBrowse)
        Me.Controls.Add(Me.optDel)
        Me.Controls.Add(Me.optAppnd)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.lblError)
        Me.Controls.Add(Me.vsfTmp)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtNoOfInv)
        Me.Controls.Add(Me.txtXLFilePath)
        Me.Controls.Add(Me.Label1)
        Me.MinimumSize = New System.Drawing.Size(890, 636)
        Me.Name = "frmUtlImportItemFmExcel"
        Me.ShowIcon = False
        Me.Text = "Import Item Details"
        CType(Me.vsfTmp, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfExcel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtXLFilePath As System.Windows.Forms.TextBox
    Friend WithEvents txtNoOfInv As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents vsfTmp As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents lblError As System.Windows.Forms.Label
    Friend WithEvents Cdlg As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents optAppnd As System.Windows.Forms.RadioButton
    Friend WithEvents optDel As System.Windows.Forms.RadioButton
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnImportFromXL As System.Windows.Forms.Button
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents vsfExcel As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnExportToXL As System.Windows.Forms.Button
    Friend WithEvents btnRemErr As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnImpClose As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As ToolTip
End Class
