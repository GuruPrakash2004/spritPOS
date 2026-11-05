<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlUpdatefromTally
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlUpdatefromTally))
        Me.btnUpdateFromTally = New C1.Win.C1Input.C1Button()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnExport = New C1.Win.C1Input.C1Button()
        Me.btnClose = New C1.Win.C1Input.C1Button()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnUpdateFromTally
        '
        Me.btnUpdateFromTally.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnUpdateFromTally.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnUpdateFromTally.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnUpdateFromTally.Location = New System.Drawing.Point(12, 12)
        Me.btnUpdateFromTally.Name = "btnUpdateFromTally"
        Me.btnUpdateFromTally.Size = New System.Drawing.Size(250, 31)
        Me.btnUpdateFromTally.TabIndex = 208
        Me.btnUpdateFromTally.Text = "Update Closing balance from Tally"
        Me.btnUpdateFromTally.UseVisualStyleBackColor = False
        '
        'vsfDet
        '
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "2,0,0,0,0,120,Columns:0{Visible:False;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfDet.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfDet.Location = New System.Drawing.Point(12, 50)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.Count = 1
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.Rows.GlyphRow = 2
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.ShowCellLabels = True
        Me.vsfDet.Size = New System.Drawing.Size(385, 362)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 209
        Me.vsfDet.Tag = ""
        '
        'btnExport
        '
        Me.btnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExport.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnExport.Image = CType(resources.GetObject("btnExport.Image"), System.Drawing.Image)
        Me.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExport.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnExport.Location = New System.Drawing.Point(12, 419)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(140, 31)
        Me.btnExport.TabIndex = 210
        Me.btnExport.Text = "&Export to Excel"
        Me.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnExport.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnClose.Location = New System.Drawing.Point(297, 419)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(100, 31)
        Me.btnClose.TabIndex = 211
        Me.btnClose.Text = "&Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'frmUtlUpdatefromTally
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(409, 462)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.btnUpdateFromTally)
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(425, 500)
        Me.Name = "frmUtlUpdatefromTally"
        Me.ShowIcon = False
        Me.Text = "Update OutStanding from Tally"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnUpdateFromTally As C1.Win.C1Input.C1Button
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnExport As C1.Win.C1Input.C1Button
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
End Class
