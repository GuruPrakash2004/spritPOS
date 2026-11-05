<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlImportLegacyData
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlImportLegacyData))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cboType = New System.Windows.Forms.ComboBox()
        Me.btnImportData = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtLegDB = New System.Windows.Forms.TextBox()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(21, 13)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(95, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Legacy Data Type"
        '
        'cboType
        '
        Me.cboType.FormattingEnabled = True
        Me.cboType.Items.AddRange(New Object() {"Drops", "vbLQMS"})
        Me.cboType.Location = New System.Drawing.Point(23, 29)
        Me.cboType.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.cboType.Name = "cboType"
        Me.cboType.Size = New System.Drawing.Size(180, 21)
        Me.cboType.TabIndex = 1
        '
        'btnImportData
        '
        Me.btnImportData.Location = New System.Drawing.Point(23, 120)
        Me.btnImportData.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.btnImportData.Name = "btnImportData"
        Me.btnImportData.Size = New System.Drawing.Size(96, 24)
        Me.btnImportData.TabIndex = 2
        Me.btnImportData.Text = "Import Data"
        Me.btnImportData.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(21, 68)
        Me.Label2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(122, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Legacy Database Name"
        '
        'txtLegDB
        '
        Me.txtLegDB.Location = New System.Drawing.Point(23, 84)
        Me.txtLegDB.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.txtLegDB.Name = "txtLegDB"
        Me.txtLegDB.Size = New System.Drawing.Size(222, 20)
        Me.txtLegDB.TabIndex = 4
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(140, 120)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(96, 24)
        Me.btnClose.TabIndex = 5
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'vsfDet
        '
        Me.vsfDet.AllowFiltering = True
        Me.vsfDet.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfDet.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfDet.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfDet.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.AutoResize = True
        Me.vsfDet.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "10,0,0,0,0,140,Columns:4{Width:197;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfDet.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.vsfDet.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(342, 3)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 28
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(360, 237)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 23
        Me.vsfDet.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2007Blue
        '
        'frmUtlImportLegacyData
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(727, 250)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.txtLegDB)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnImportData)
        Me.Controls.Add(Me.cboType)
        Me.Controls.Add(Me.Label1)
        Me.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.Name = "frmUtlImportLegacyData"
        Me.ShowIcon = False
        Me.Text = "Import Legacy Data"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents cboType As ComboBox
    Friend WithEvents btnImportData As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents txtLegDB As TextBox
    Friend WithEvents btnClose As Button
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
End Class
