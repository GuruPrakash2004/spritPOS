<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlCompareStk
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlCompareStk))
        Me.vsfHdr1 = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfHdr2 = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfCompare = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnImpXL2 = New System.Windows.Forms.Button()
        Me.btnImpXL1 = New System.Windows.Forms.Button()
        Me.btnCompare = New System.Windows.Forms.Button()
        Me.btnImpClose = New System.Windows.Forms.Button()
        Me.btnExportToXL = New System.Windows.Forms.Button()
        Me.Cdlg = New System.Windows.Forms.OpenFileDialog()
        CType(Me.vsfHdr1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfHdr2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfCompare, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'vsfHdr1
        '
        Me.vsfHdr1.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfHdr1.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfHdr1.ColumnInfo = "2,1,0,0,0,120,Columns:"
        Me.vsfHdr1.ExtendLastCol = True
        Me.vsfHdr1.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfHdr1.Location = New System.Drawing.Point(12, 12)
        Me.vsfHdr1.Name = "vsfHdr1"
        Me.vsfHdr1.Rows.Count = 6
        Me.vsfHdr1.Rows.DefaultSize = 24
        Me.vsfHdr1.Size = New System.Drawing.Size(440, 249)
        Me.vsfHdr1.StyleInfo = resources.GetString("vsfHdr1.StyleInfo")
        Me.vsfHdr1.TabIndex = 130
        Me.vsfHdr1.Tag = ""
        '
        'vsfHdr2
        '
        Me.vsfHdr2.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfHdr2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfHdr2.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfHdr2.ColumnInfo = "2,1,0,0,0,120,Columns:"
        Me.vsfHdr2.ExtendLastCol = True
        Me.vsfHdr2.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfHdr2.Location = New System.Drawing.Point(458, 12)
        Me.vsfHdr2.Name = "vsfHdr2"
        Me.vsfHdr2.Rows.Count = 6
        Me.vsfHdr2.Rows.DefaultSize = 24
        Me.vsfHdr2.Size = New System.Drawing.Size(438, 249)
        Me.vsfHdr2.StyleInfo = resources.GetString("vsfHdr2.StyleInfo")
        Me.vsfHdr2.TabIndex = 131
        Me.vsfHdr2.Tag = ""
        '
        'vsfCompare
        '
        Me.vsfCompare.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfCompare.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfCompare.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfCompare.ColumnInfo = "2,1,0,0,0,120,Columns:"
        Me.vsfCompare.ExtendLastCol = True
        Me.vsfCompare.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfCompare.Location = New System.Drawing.Point(12, 299)
        Me.vsfCompare.Name = "vsfCompare"
        Me.vsfCompare.Rows.Count = 6
        Me.vsfCompare.Rows.DefaultSize = 24
        Me.vsfCompare.Size = New System.Drawing.Size(884, 160)
        Me.vsfCompare.StyleInfo = resources.GetString("vsfCompare.StyleInfo")
        Me.vsfCompare.TabIndex = 132
        Me.vsfCompare.Tag = ""
        '
        'btnImpXL2
        '
        Me.btnImpXL2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnImpXL2.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnImpXL2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnImpXL2.Image = CType(resources.GetObject("btnImpXL2.Image"), System.Drawing.Image)
        Me.btnImpXL2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnImpXL2.Location = New System.Drawing.Point(717, 263)
        Me.btnImpXL2.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnImpXL2.Name = "btnImpXL2"
        Me.btnImpXL2.Size = New System.Drawing.Size(179, 29)
        Me.btnImpXL2.TabIndex = 134
        Me.btnImpXL2.Text = "Import from Excel (New)"
        Me.btnImpXL2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnImpXL2.UseVisualStyleBackColor = False
        '
        'btnImpXL1
        '
        Me.btnImpXL1.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnImpXL1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnImpXL1.Image = CType(resources.GetObject("btnImpXL1.Image"), System.Drawing.Image)
        Me.btnImpXL1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnImpXL1.Location = New System.Drawing.Point(12, 263)
        Me.btnImpXL1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnImpXL1.Name = "btnImpXL1"
        Me.btnImpXL1.Size = New System.Drawing.Size(172, 29)
        Me.btnImpXL1.TabIndex = 135
        Me.btnImpXL1.Text = "Import from Excel (Old)"
        Me.btnImpXL1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnImpXL1.UseVisualStyleBackColor = False
        '
        'btnCompare
        '
        Me.btnCompare.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.btnCompare.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnCompare.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCompare.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCompare.Location = New System.Drawing.Point(390, 263)
        Me.btnCompare.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnCompare.Name = "btnCompare"
        Me.btnCompare.Size = New System.Drawing.Size(117, 29)
        Me.btnCompare.TabIndex = 136
        Me.btnCompare.Text = "Compare Stocks"
        Me.btnCompare.UseVisualStyleBackColor = False
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
        Me.btnImpClose.Location = New System.Drawing.Point(815, 466)
        Me.btnImpClose.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnImpClose.Name = "btnImpClose"
        Me.btnImpClose.Size = New System.Drawing.Size(81, 29)
        Me.btnImpClose.TabIndex = 138
        Me.btnImpClose.Text = "&Close"
        Me.btnImpClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnImpClose.UseVisualStyleBackColor = False
        '
        'btnExportToXL
        '
        Me.btnExportToXL.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExportToXL.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnExportToXL.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExportToXL.Image = CType(resources.GetObject("btnExportToXL.Image"), System.Drawing.Image)
        Me.btnExportToXL.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExportToXL.Location = New System.Drawing.Point(679, 466)
        Me.btnExportToXL.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnExportToXL.Name = "btnExportToXL"
        Me.btnExportToXL.Size = New System.Drawing.Size(130, 29)
        Me.btnExportToXL.TabIndex = 137
        Me.btnExportToXL.Text = "&Export to excel"
        Me.btnExportToXL.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnExportToXL.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnExportToXL.UseCompatibleTextRendering = True
        Me.btnExportToXL.UseVisualStyleBackColor = False
        '
        'Cdlg
        '
        Me.Cdlg.FileName = "Cdlg"
        '
        'frmUtlCompareStk
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(908, 499)
        Me.Controls.Add(Me.btnImpClose)
        Me.Controls.Add(Me.btnExportToXL)
        Me.Controls.Add(Me.btnCompare)
        Me.Controls.Add(Me.btnImpXL1)
        Me.Controls.Add(Me.btnImpXL2)
        Me.Controls.Add(Me.vsfCompare)
        Me.Controls.Add(Me.vsfHdr2)
        Me.Controls.Add(Me.vsfHdr1)
        Me.Name = "frmUtlCompareStk"
        Me.ShowIcon = False
        Me.Text = "Compare Stock"
        CType(Me.vsfHdr1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfHdr2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfCompare, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents vsfHdr1 As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents vsfHdr2 As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents vsfCompare As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnImpXL2 As Button
    Friend WithEvents btnImpXL1 As Button
    Friend WithEvents btnCompare As Button
    Friend WithEvents btnImpClose As Button
    Friend WithEvents btnExportToXL As Button
    Friend WithEvents Cdlg As OpenFileDialog
End Class
