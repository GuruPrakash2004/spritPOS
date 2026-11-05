<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlExplosion
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlExplosion))
        Me.vsfExp = New C1.Win.C1FlexGrid.C1FlexGrid()
        CType(Me.vsfExp, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'vsfExp
        '
        Me.vsfExp.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfExp.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfExp.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfExp.ColumnInfo = "10,1,0,0,0,120,Columns:"
        Me.vsfExp.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfExp.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfExp.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfExp.Location = New System.Drawing.Point(12, 4)
        Me.vsfExp.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfExp.Name = "vsfExp"
        Me.vsfExp.Rows.Count = 2
        Me.vsfExp.Rows.DefaultSize = 24
        Me.vsfExp.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfExp.Size = New System.Drawing.Size(736, 466)
        Me.vsfExp.StyleInfo = resources.GetString("vsfExp.StyleInfo")
        Me.vsfExp.TabIndex = 4
        Me.vsfExp.Tag = ""
        '
        'frmUtlExplosion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(760, 483)
        Me.Controls.Add(Me.vsfExp)
        Me.Name = "frmUtlExplosion"
        Me.ShowIcon = False
        Me.Text = "frmUtlExplosion"
        CType(Me.vsfExp, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents vsfExp As C1.Win.C1FlexGrid.C1FlexGrid
End Class
