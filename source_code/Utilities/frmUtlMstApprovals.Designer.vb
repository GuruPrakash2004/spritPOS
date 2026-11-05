<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlMstApprovals
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlMstApprovals))
        Me.txtID = New System.Windows.Forms.TextBox()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.lblMsg = New System.Windows.Forms.Label()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtID
        '
        Me.txtID.Location = New System.Drawing.Point(2, 410)
        Me.txtID.Name = "txtID"
        Me.txtID.Size = New System.Drawing.Size(35, 20)
        Me.txtID.TabIndex = 0
        Me.txtID.Visible = False
        '
        'vsfDet
        '
        Me.vsfDet.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfDet.AllowFiltering = True
        Me.vsfDet.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfDet.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfDet.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.AutoResize = True
        Me.vsfDet.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfDet.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(12, 44)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.Rows.GlyphRow = 2
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.ShowCellLabels = True
        Me.vsfDet.Size = New System.Drawing.Size(437, 326)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 14
        Me.vsfDet.Tag = "View"
        Me.vsfDet.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2007Blue
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(365, 388)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(81, 29)
        Me.btnClose.TabIndex = 17
        Me.btnClose.Text = "&Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Location = New System.Drawing.Point(279, 388)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(81, 29)
        Me.btnSave.TabIndex = 18
        Me.btnSave.Text = " &Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'lblMsg
        '
        Me.lblMsg.AutoSize = True
        Me.lblMsg.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.lblMsg.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!)
        Me.lblMsg.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMsg.Location = New System.Drawing.Point(12, 15)
        Me.lblMsg.Name = "lblMsg"
        Me.lblMsg.Size = New System.Drawing.Size(327, 17)
        Me.lblMsg.TabIndex = 19
        Me.lblMsg.Text = "Check the selected Masters which needs approval."
        '
        'frmUtlMstApprovals
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.ClientSize = New System.Drawing.Size(458, 432)
        Me.Controls.Add(Me.lblMsg)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.txtID)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.Name = "frmUtlMstApprovals"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Master Approvals"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtID As TextBox
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents btnSave As Button
    Friend WithEvents lblMsg As Label
End Class
