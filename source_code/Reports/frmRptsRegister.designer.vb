<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRptsRegister
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRptsRegister))
        Dim CBlendItems1 As vLQMS.cBlendItems = New vLQMS.cBlendItems()
        Dim CBlendItems2 As vLQMS.cBlendItems = New vLQMS.cBlendItems()
        Dim CBlendItems3 As vLQMS.cBlendItems = New vLQMS.cBlendItems()
        Dim CBlendItems4 As vLQMS.cBlendItems = New vLQMS.cBlendItems()
        Me.tt = New System.Windows.Forms.ToolTip(Me.components)
        Me.vsfRpt = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.grpRights = New System.Windows.Forms.GroupBox()
        Me.btnSave = New vLQMS.CButton()
        Me.btnCancel = New vLQMS.CButton()
        Me.vsfRights = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfUser = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnRights = New vLQMS.CButton()
        Me.btnClose = New vLQMS.CButton()
        CType(Me.vsfRpt, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpRights.SuspendLayout()
        CType(Me.vsfRights, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfUser, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'vsfRpt
        '
        Me.vsfRpt.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfRpt.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfRpt.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfRpt.ColumnInfo = "2,1,0,0,0,110,Columns:"
        Me.vsfRpt.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.vsfRpt.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfRpt.Location = New System.Drawing.Point(12, 35)
        Me.vsfRpt.Name = "vsfRpt"
        Me.vsfRpt.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.vsfRpt.Rows.Count = 4
        Me.vsfRpt.Rows.DefaultSize = 22
        Me.vsfRpt.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfRpt.Size = New System.Drawing.Size(863, 407)
        Me.vsfRpt.StyleInfo = resources.GetString("vsfRpt.StyleInfo")
        Me.vsfRpt.TabIndex = 227
        Me.vsfRpt.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2010Silver
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(315, 17)
        Me.Label1.TabIndex = 228
        Me.Label1.Text = "Double_Click or Use Enter option to view Reports."
        '
        'grpRights
        '
        Me.grpRights.Controls.Add(Me.btnSave)
        Me.grpRights.Controls.Add(Me.btnCancel)
        Me.grpRights.Controls.Add(Me.vsfRights)
        Me.grpRights.Controls.Add(Me.vsfUser)
        Me.grpRights.Location = New System.Drawing.Point(93, 12)
        Me.grpRights.Name = "grpRights"
        Me.grpRights.Size = New System.Drawing.Size(745, 382)
        Me.grpRights.TabIndex = 230
        Me.grpRights.TabStop = False
        Me.grpRights.Visible = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BorderColor = System.Drawing.Color.Black
        CBlendItems1.iColor = New System.Drawing.Color() {System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer)), System.Drawing.Color.White}
        CBlendItems1.iPoint = New Single() {0!, 1.0!}
        Me.btnSave.ColorFillBlend = CBlendItems1
        Me.btnSave.DesignerSelected = False
        Me.btnSave.ForeColor = System.Drawing.Color.Black
        Me.btnSave.ImageIndex = 0
        Me.btnSave.Location = New System.Drawing.Point(611, 339)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(59, 25)
        Me.btnSave.TabIndex = 231
        Me.btnSave.Text = "&Save"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BorderColor = System.Drawing.Color.Black
        CBlendItems2.iColor = New System.Drawing.Color() {System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer)), System.Drawing.Color.White}
        CBlendItems2.iPoint = New Single() {0!, 1.0!}
        Me.btnCancel.ColorFillBlend = CBlendItems2
        Me.btnCancel.DesignerSelected = False
        Me.btnCancel.ForeColor = System.Drawing.Color.Black
        Me.btnCancel.ImageIndex = 0
        Me.btnCancel.Location = New System.Drawing.Point(680, 339)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(59, 25)
        Me.btnCancel.TabIndex = 230
        Me.btnCancel.Text = "&Cancel"
        '
        'vsfRights
        '
        Me.vsfRights.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfRights.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfRights.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfRights.ColumnInfo = "2,1,0,0,0,110,Columns:"
        Me.vsfRights.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.vsfRights.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfRights.Location = New System.Drawing.Point(268, 22)
        Me.vsfRights.Name = "vsfRights"
        Me.vsfRights.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.vsfRights.Rows.Count = 4
        Me.vsfRights.Rows.DefaultSize = 22
        Me.vsfRights.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfRights.Size = New System.Drawing.Size(471, 311)
        Me.vsfRights.StyleInfo = resources.GetString("vsfRights.StyleInfo")
        Me.vsfRights.TabIndex = 229
        Me.vsfRights.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2010Silver
        '
        'vsfUser
        '
        Me.vsfUser.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfUser.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfUser.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfUser.ColumnInfo = "2,1,0,0,0,110,Columns:"
        Me.vsfUser.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.vsfUser.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfUser.Location = New System.Drawing.Point(6, 22)
        Me.vsfUser.Name = "vsfUser"
        Me.vsfUser.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.vsfUser.Rows.Count = 4
        Me.vsfUser.Rows.DefaultSize = 22
        Me.vsfUser.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfUser.Size = New System.Drawing.Size(252, 311)
        Me.vsfUser.StyleInfo = resources.GetString("vsfUser.StyleInfo")
        Me.vsfUser.TabIndex = 228
        Me.vsfUser.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2010Silver
        '
        'btnRights
        '
        Me.btnRights.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRights.BorderColor = System.Drawing.Color.Black
        CBlendItems3.iColor = New System.Drawing.Color() {System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer)), System.Drawing.Color.White}
        CBlendItems3.iPoint = New Single() {0!, 1.0!}
        Me.btnRights.ColorFillBlend = CBlendItems3
        Me.btnRights.DesignerSelected = False
        Me.btnRights.ForeColor = System.Drawing.Color.Black
        Me.btnRights.ImageIndex = 0
        Me.btnRights.Location = New System.Drawing.Point(12, 448)
        Me.btnRights.Name = "btnRights"
        Me.btnRights.Size = New System.Drawing.Size(122, 25)
        Me.btnRights.TabIndex = 229
        Me.btnRights.Text = "Assign Rights"
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BorderColor = System.Drawing.Color.Black
        CBlendItems4.iColor = New System.Drawing.Color() {System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer)), System.Drawing.Color.White}
        CBlendItems4.iPoint = New Single() {0!, 1.0!}
        Me.btnClose.ColorFillBlend = CBlendItems4
        Me.btnClose.DesignerSelected = False
        Me.btnClose.ForeColor = System.Drawing.Color.Black
        Me.btnClose.ImageIndex = 0
        Me.btnClose.Location = New System.Drawing.Point(816, 448)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(59, 25)
        Me.btnClose.TabIndex = 223
        Me.btnClose.Text = "C&lose"
        '
        'frmRptsRegister
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(887, 485)
        Me.Controls.Add(Me.grpRights)
        Me.Controls.Add(Me.btnRights)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.vsfRpt)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MinimumSize = New System.Drawing.Size(900, 500)
        Me.Name = "frmRptsRegister"
        Me.ShowIcon = False
        Me.Text = "Reports"
        CType(Me.vsfRpt, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpRights.ResumeLayout(False)
        CType(Me.vsfRights, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfUser, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents tt As System.Windows.Forms.ToolTip
    Friend WithEvents btnClose As vLQMS.CButton
    Friend WithEvents vsfRpt As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnRights As vLQMS.CButton
    Friend WithEvents grpRights As System.Windows.Forms.GroupBox
    Friend WithEvents btnSave As vLQMS.CButton
    Friend WithEvents btnCancel As vLQMS.CButton
    Friend WithEvents vsfRights As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents vsfUser As C1.Win.C1FlexGrid.C1FlexGrid
End Class
