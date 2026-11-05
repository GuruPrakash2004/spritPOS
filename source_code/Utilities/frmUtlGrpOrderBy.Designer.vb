<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlGrpOrderBy
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlGrpOrderBy))
        Me.btnExportToXL = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.PnlArrow = New System.Windows.Forms.Panel()
        Me.btnMvUP = New System.Windows.Forms.Button()
        Me.btnMvDwn = New System.Windows.Forms.Button()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PnlArrow.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnExportToXL
        '
        Me.btnExportToXL.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExportToXL.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnExportToXL.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExportToXL.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.btnExportToXL.Image = CType(resources.GetObject("btnExportToXL.Image"), System.Drawing.Image)
        Me.btnExportToXL.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExportToXL.Location = New System.Drawing.Point(12, 357)
        Me.btnExportToXL.Name = "btnExportToXL"
        Me.btnExportToXL.Size = New System.Drawing.Size(35, 29)
        Me.btnExportToXL.TabIndex = 262
        Me.btnExportToXL.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnExportToXL, "Export to Excel : Alt + E")
        Me.btnExportToXL.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(312, 357)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(81, 29)
        Me.btnClose.TabIndex = 263
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close : Alt + O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(231, 357)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 29)
        Me.btnSave.TabIndex = 261
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Save : Alt + S")
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'vsfDet
        '
        Me.vsfDet.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfDet.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "3,1,0,0,0,120,Columns:"
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.Location = New System.Drawing.Point(53, 12)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.Count = 2
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(340, 339)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 264
        '
        'PnlArrow
        '
        Me.PnlArrow.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PnlArrow.Controls.Add(Me.btnMvUP)
        Me.PnlArrow.Controls.Add(Me.btnMvDwn)
        Me.PnlArrow.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PnlArrow.Location = New System.Drawing.Point(12, 12)
        Me.PnlArrow.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.PnlArrow.Name = "PnlArrow"
        Me.PnlArrow.Size = New System.Drawing.Size(35, 339)
        Me.PnlArrow.TabIndex = 265
        '
        'btnMvUP
        '
        Me.btnMvUP.ForeColor = System.Drawing.Color.Black
        Me.btnMvUP.Image = CType(resources.GetObject("btnMvUP.Image"), System.Drawing.Image)
        Me.btnMvUP.Location = New System.Drawing.Point(2, 0)
        Me.btnMvUP.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnMvUP.Name = "btnMvUP"
        Me.btnMvUP.Size = New System.Drawing.Size(31, 173)
        Me.btnMvUP.TabIndex = 9
        Me.btnMvUP.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnMvUP.UseVisualStyleBackColor = False
        '
        'btnMvDwn
        '
        Me.btnMvDwn.ForeColor = System.Drawing.Color.Black
        Me.btnMvDwn.Image = CType(resources.GetObject("btnMvDwn.Image"), System.Drawing.Image)
        Me.btnMvDwn.Location = New System.Drawing.Point(2, 172)
        Me.btnMvDwn.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnMvDwn.Name = "btnMvDwn"
        Me.btnMvDwn.Size = New System.Drawing.Size(31, 165)
        Me.btnMvDwn.TabIndex = 10
        Me.btnMvDwn.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnMvDwn.UseVisualStyleBackColor = False
        '
        'frmUtlGrpOrderBy
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(405, 398)
        Me.Controls.Add(Me.PnlArrow)
        Me.Controls.Add(Me.btnExportToXL)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.vsfDet)
        Me.KeyPreview = True
        Me.Name = "frmUtlGrpOrderBy"
        Me.ShowIcon = False
        Me.Text = "Item Group Order By"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PnlArrow.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnExportToXL As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents btnSave As Button
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents PnlArrow As Panel
    Friend WithEvents btnMvUP As Button
    Friend WithEvents btnMvDwn As Button
End Class
