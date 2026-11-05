<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlAlias
    Inherits System.Windows.Forms.Form
    'Inherits MetroFramework.Forms.MetroForm

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlAlias))
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.fbd = New System.Windows.Forms.FolderBrowserDialog()
        Me.fsd = New System.Windows.Forms.OpenFileDialog()
        Me.lblfsize = New System.Windows.Forms.Label()
        Me.lblfname = New System.Windows.Forms.Label()
        Me.lblmods = New System.Windows.Forms.Label()
        Me.lblformcaption = New System.Windows.Forms.Label()
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.btnSave = New C1.Win.C1Input.C1Button()
        Me.btnRemoveRow = New System.Windows.Forms.Button()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'vsfDet
        '
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "10,0,0,0,0,95,Columns:0{AllowMerging:True;Style:""TextAlign:LeftCenter;"";StyleFixe" & _
    "d:""TextAlign:LeftCenter;"";}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfDet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.vsfDet.Location = New System.Drawing.Point(12, 33)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 19
        Me.vsfDet.Rows.GlyphRow = 0
        Me.vsfDet.Size = New System.Drawing.Size(507, 377)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 0
        '
        'fsd
        '
        Me.fsd.FileName = "OpenFileDialog1"
        '
        'lblfsize
        '
        Me.lblfsize.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblfsize.AutoSize = True
        Me.lblfsize.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblfsize.Location = New System.Drawing.Point(12, 417)
        Me.lblfsize.Name = "lblfsize"
        Me.lblfsize.Size = New System.Drawing.Size(0, 16)
        Me.lblfsize.TabIndex = 13
        '
        'lblfname
        '
        Me.lblfname.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblfname.AutoSize = True
        Me.lblfname.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblfname.Location = New System.Drawing.Point(12, 395)
        Me.lblfname.Name = "lblfname"
        Me.lblfname.Size = New System.Drawing.Size(0, 16)
        Me.lblfname.TabIndex = 14
        '
        'lblmods
        '
        Me.lblmods.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblmods.AutoSize = True
        Me.lblmods.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblmods.Location = New System.Drawing.Point(12, 437)
        Me.lblmods.Name = "lblmods"
        Me.lblmods.Size = New System.Drawing.Size(0, 16)
        Me.lblmods.TabIndex = 15
        '
        'lblformcaption
        '
        Me.lblformcaption.AutoSize = True
        Me.lblformcaption.Font = New System.Drawing.Font("Consolas", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblformcaption.ForeColor = System.Drawing.Color.DarkSlateBlue
        Me.lblformcaption.Location = New System.Drawing.Point(12, 6)
        Me.lblformcaption.Name = "lblformcaption"
        Me.lblformcaption.Size = New System.Drawing.Size(154, 24)
        Me.lblformcaption.TabIndex = 24
        Me.lblformcaption.Text = "Form Caption"
        Me.lblformcaption.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.CausesValidation = False
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Image = CType(resources.GetObject("btnClose.Image"), System.Drawing.Image)
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(452, 417)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(66, 31)
        Me.btnClose.TabIndex = 3
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Image = CType(resources.GetObject("btnSave.Image"), System.Drawing.Image)
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(378, 417)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(66, 31)
        Me.btnSave.TabIndex = 2
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnRemoveRow
        '
        Me.btnRemoveRow.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRemoveRow.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnRemoveRow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRemoveRow.Location = New System.Drawing.Point(12, 417)
        Me.btnRemoveRow.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnRemoveRow.Name = "btnRemoveRow"
        Me.btnRemoveRow.Size = New System.Drawing.Size(101, 30)
        Me.btnRemoveRow.TabIndex = 1
        Me.btnRemoveRow.Text = "Remove &Row"
        Me.btnRemoveRow.UseVisualStyleBackColor = False
        '
        'frmUtlAlias
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(531, 462)
        Me.Controls.Add(Me.btnRemoveRow)
        Me.Controls.Add(Me.lblformcaption)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.lblmods)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.lblfname)
        Me.Controls.Add(Me.lblfsize)
        Me.Controls.Add(Me.vsfDet)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmUtlAlias"
        Me.Text = "Alias"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents fbd As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents fsd As System.Windows.Forms.OpenFileDialog
    Friend WithEvents lblfsize As System.Windows.Forms.Label
    Friend WithEvents lblfname As System.Windows.Forms.Label
    Friend WithEvents lblmods As System.Windows.Forms.Label
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents lblformcaption As System.Windows.Forms.Label
    Friend WithEvents btnSave As C1.Win.C1Input.C1Button
    Friend WithEvents btnRemoveRow As System.Windows.Forms.Button
End Class
