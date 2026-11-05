<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlTranspose
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlTranspose))
        Me.btnExpNotepad = New C1.Win.C1Input.C1Button()
        Me.btnpnlClose = New C1.Win.C1Input.C1Button()
        Me.btnExpExcel = New C1.Win.C1Input.C1Button()
        Me.vsfTranspose = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.vsfTranspose, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnExpNotepad
        '
        Me.btnExpNotepad.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExpNotepad.BackgroundImage = Global.vLQMS.My.Resources.Resources.clipboard
        Me.btnExpNotepad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnExpNotepad.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExpNotepad.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnExpNotepad.Location = New System.Drawing.Point(675, 422)
        Me.btnExpNotepad.Name = "btnExpNotepad"
        Me.btnExpNotepad.Size = New System.Drawing.Size(35, 31)
        Me.btnExpNotepad.TabIndex = 203
        Me.btnExpNotepad.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnExpNotepad, "Notepad : Alt + N")
        Me.btnExpNotepad.UseVisualStyleBackColor = False
        '
        'btnpnlClose
        '
        Me.btnpnlClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnpnlClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnpnlClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnpnlClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnpnlClose.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnpnlClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnpnlClose.Location = New System.Drawing.Point(757, 422)
        Me.btnpnlClose.Name = "btnpnlClose"
        Me.btnpnlClose.Size = New System.Drawing.Size(74, 31)
        Me.btnpnlClose.TabIndex = 201
        Me.btnpnlClose.Text = "Cl&ose"
        Me.btnpnlClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnpnlClose, "Close : Alt + O")
        Me.btnpnlClose.UseVisualStyleBackColor = False
        '
        'btnExpExcel
        '
        Me.btnExpExcel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExpExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExpExcel.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnExpExcel.Image = CType(resources.GetObject("btnExpExcel.Image"), System.Drawing.Image)
        Me.btnExpExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExpExcel.Location = New System.Drawing.Point(716, 422)
        Me.btnExpExcel.Name = "btnExpExcel"
        Me.btnExpExcel.Size = New System.Drawing.Size(35, 31)
        Me.btnExpExcel.TabIndex = 200
        Me.btnExpExcel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnExpExcel, "Export to Excel : Alt + E")
        Me.btnExpExcel.UseVisualStyleBackColor = False
        '
        'vsfTranspose
        '
        Me.vsfTranspose.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfTranspose.AllowFiltering = True
        Me.vsfTranspose.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfTranspose.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfTranspose.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfTranspose.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfTranspose.AutoResize = True
        Me.vsfTranspose.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfTranspose.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfTranspose.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfTranspose.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfTranspose.ExtendLastCol = True
        Me.vsfTranspose.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfTranspose.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfTranspose.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfTranspose.Location = New System.Drawing.Point(11, 12)
        Me.vsfTranspose.Name = "vsfTranspose"
        Me.vsfTranspose.Rows.DefaultSize = 24
        Me.vsfTranspose.Rows.GlyphRow = 0
        Me.vsfTranspose.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfTranspose.ShowCellLabels = True
        Me.vsfTranspose.Size = New System.Drawing.Size(820, 404)
        Me.vsfTranspose.StyleInfo = resources.GetString("vsfTranspose.StyleInfo")
        Me.vsfTranspose.TabIndex = 204
        Me.vsfTranspose.Tag = "View"
        Me.vsfTranspose.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2007Blue
        '
        'frmUtlTranspose
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(844, 465)
        Me.Controls.Add(Me.vsfTranspose)
        Me.Controls.Add(Me.btnExpNotepad)
        Me.Controls.Add(Me.btnpnlClose)
        Me.Controls.Add(Me.btnExpExcel)
        Me.KeyPreview = True
        Me.Name = "frmUtlTranspose"
        Me.ShowIcon = False
        Me.Text = "Transpose"
        CType(Me.vsfTranspose, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnpnlClose As C1.Win.C1Input.C1Button
    Friend WithEvents btnExpExcel As C1.Win.C1Input.C1Button
    Friend WithEvents btnExpNotepad As C1.Win.C1Input.C1Button
    Friend WithEvents vsfTranspose As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents ToolTip1 As ToolTip
End Class
