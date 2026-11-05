<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmStkRewrite
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmStkRewrite))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnReWrite = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.vsfQOH = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnExportToXL = New System.Windows.Forms.Button()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfQOH, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Yellow
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(0, 497)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(800, 32)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Please close the software in all the systems other than this one, before reWritin" &
    "g"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnReWrite
        '
        Me.btnReWrite.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnReWrite.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnReWrite.Font = New System.Drawing.Font("Microsoft Sans Serif", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnReWrite.Location = New System.Drawing.Point(7, 425)
        Me.btnReWrite.Margin = New System.Windows.Forms.Padding(2)
        Me.btnReWrite.Name = "btnReWrite"
        Me.btnReWrite.Size = New System.Drawing.Size(212, 52)
        Me.btnReWrite.TabIndex = 1
        Me.btnReWrite.Text = "Re-Write"
        Me.btnReWrite.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(654, 425)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(2)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(135, 52)
        Me.btnClose.TabIndex = 2
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
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
        Me.vsfDet.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(435, 36)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(354, 383)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 251
        Me.vsfDet.Tag = ""
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(432, 12)
        Me.Label2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(314, 18)
        Me.Label2.TabIndex = 252
        Me.Label2.Text = "Diff. in Transaction Qty  and System Stock Qty"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(6, 12)
        Me.Label3.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(208, 18)
        Me.Label3.TabIndex = 254
        Me.Label3.Text = "Diff. in Stock Ledger and QOH"
        '
        'vsfQOH
        '
        Me.vsfQOH.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfQOH.AllowFiltering = True
        Me.vsfQOH.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfQOH.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfQOH.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfQOH.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.vsfQOH.AutoResize = True
        Me.vsfQOH.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfQOH.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfQOH.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfQOH.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfQOH.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfQOH.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfQOH.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfQOH.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfQOH.Location = New System.Drawing.Point(7, 36)
        Me.vsfQOH.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfQOH.Name = "vsfQOH"
        Me.vsfQOH.Rows.DefaultSize = 24
        Me.vsfQOH.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfQOH.Size = New System.Drawing.Size(423, 383)
        Me.vsfQOH.StyleInfo = resources.GetString("vsfQOH.StyleInfo")
        Me.vsfQOH.TabIndex = 253
        Me.vsfQOH.Tag = ""
        '
        'btnExportToXL
        '
        Me.btnExportToXL.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExportToXL.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnExportToXL.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExportToXL.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnExportToXL.Image = CType(resources.GetObject("btnExportToXL.Image"), System.Drawing.Image)
        Me.btnExportToXL.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExportToXL.Location = New System.Drawing.Point(604, 439)
        Me.btnExportToXL.Name = "btnExportToXL"
        Me.btnExportToXL.Size = New System.Drawing.Size(35, 29)
        Me.btnExportToXL.TabIndex = 255
        Me.btnExportToXL.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnExportToXL.UseVisualStyleBackColor = False
        '
        'frmStkRewrite
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 529)
        Me.Controls.Add(Me.btnExportToXL)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.vsfQOH)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnReWrite)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(816, 567)
        Me.Name = "frmStkRewrite"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Re-Write Stock Balance"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfQOH, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents btnReWrite As Button
    Friend WithEvents btnClose As Button
    Public WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Public WithEvents vsfQOH As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnExportToXL As Button
End Class
