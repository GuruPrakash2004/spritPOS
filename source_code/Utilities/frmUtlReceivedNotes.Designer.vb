<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlReceivedNotes
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlReceivedNotes))
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.vsfdet = New C1.Win.C1FlexGrid.C1FlexGrid()
        CType(Me.vsfdet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnExport
        '
        Me.btnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExport.Location = New System.Drawing.Point(12, 393)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(89, 27)
        Me.btnExport.TabIndex = 0
        Me.btnExport.Text = "Export to Excel"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Location = New System.Drawing.Point(774, 393)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(89, 27)
        Me.btnClose.TabIndex = 1
        Me.btnClose.Text = "Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'vsfdet
        '
        Me.vsfdet.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfdet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfdet.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfdet.ExtendLastCol = True
        Me.vsfdet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfdet.Location = New System.Drawing.Point(12, 13)
        Me.vsfdet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfdet.Name = "vsfdet"
        Me.vsfdet.Rows.Count = 1
        Me.vsfdet.Rows.DefaultSize = 24
        Me.vsfdet.Rows.Fixed = 0
        Me.vsfdet.Size = New System.Drawing.Size(851, 374)
        Me.vsfdet.StyleInfo = resources.GetString("vsfdet.StyleInfo")
        Me.vsfdet.TabIndex = 2
        '
        'frmUtlReceivedNotes
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(875, 426)
        Me.Controls.Add(Me.vsfdet)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnExport)
        Me.MaximumSize = New System.Drawing.Size(891, 464)
        Me.Name = "frmUtlReceivedNotes"
        Me.ShowIcon = False
        Me.Text = "Reminder"
        CType(Me.vsfdet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnExport As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents vsfdet As C1.Win.C1FlexGrid.C1FlexGrid
End Class
