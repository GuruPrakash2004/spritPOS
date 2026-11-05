<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmRptReport
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRptReport))
        Dim CBlendItems1 As vLQMS.cBlendItems = New vLQMS.cBlendItems()
        Dim CBlendItems2 As vLQMS.cBlendItems = New vLQMS.cBlendItems()
        Dim CBlendItems3 As vLQMS.cBlendItems = New vLQMS.cBlendItems()
        Dim CBlendItems4 As vLQMS.cBlendItems = New vLQMS.cBlendItems()
        Me.vsfRpt = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfRights = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfUser = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.grpRights = New System.Windows.Forms.GroupBox()
        Me.btnSave = New vLQMS.CButton()
        Me.btnCancel = New vLQMS.CButton()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtid = New System.Windows.Forms.TextBox()
        Me.btnEnter = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnRights = New vLQMS.CButton()
        Me.btnClose = New vLQMS.CButton()
        CType(Me.vsfRpt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfRights, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfUser, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpRights.SuspendLayout()
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
        Me.vsfRpt.Location = New System.Drawing.Point(12, 51)
        Me.vsfRpt.Name = "vsfRpt"
        Me.vsfRpt.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.vsfRpt.Rows.Count = 4
        Me.vsfRpt.Rows.DefaultSize = 22
        Me.vsfRpt.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfRpt.Size = New System.Drawing.Size(863, 404)
        Me.vsfRpt.StyleInfo = resources.GetString("vsfRpt.StyleInfo")
        Me.vsfRpt.TabIndex = 231
        Me.vsfRpt.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2010Silver
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
        Me.vsfUser.Location = New System.Drawing.Point(6, 23)
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
        'grpRights
        '
        Me.grpRights.Controls.Add(Me.btnSave)
        Me.grpRights.Controls.Add(Me.btnCancel)
        Me.grpRights.Controls.Add(Me.vsfRights)
        Me.grpRights.Controls.Add(Me.vsfUser)
        Me.grpRights.Location = New System.Drawing.Point(28, 59)
        Me.grpRights.Name = "grpRights"
        Me.grpRights.Size = New System.Drawing.Size(745, 382)
        Me.grpRights.TabIndex = 233
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
        Me.ToolTip1.SetToolTip(Me.btnSave, "Save:Alt+S")
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
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Cancel:Alt+C")
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(12, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(315, 17)
        Me.Label1.TabIndex = 234
        Me.Label1.Text = "Double_Click or Use Enter option to view Reports."
        '
        'txtid
        '
        Me.txtid.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.txtid.BackColor = System.Drawing.Color.Orange
        Me.txtid.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtid.Location = New System.Drawing.Point(273, 460)
        Me.txtid.MaxLength = 3
        Me.txtid.Name = "txtid"
        Me.txtid.Size = New System.Drawing.Size(103, 20)
        Me.txtid.TabIndex = 0
        '
        'btnEnter
        '
        Me.btnEnter.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnEnter.Location = New System.Drawing.Point(379, 460)
        Me.btnEnter.Name = "btnEnter"
        Me.btnEnter.Size = New System.Drawing.Size(75, 23)
        Me.btnEnter.TabIndex = 237
        Me.btnEnter.Text = "&Load"
        Me.ToolTip1.SetToolTip(Me.btnEnter, "Load : Alt + L")
        Me.btnEnter.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(143, 463)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(124, 13)
        Me.Label2.TabIndex = 238
        Me.Label2.Text = "Enter Report No to view "
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
        Me.btnRights.Location = New System.Drawing.Point(12, 460)
        Me.btnRights.Name = "btnRights"
        Me.btnRights.Size = New System.Drawing.Size(122, 25)
        Me.btnRights.TabIndex = 232
        Me.btnRights.Text = "&Assign Rights"
        Me.ToolTip1.SetToolTip(Me.btnRights, "Assign Rights : Alt + A")
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
        Me.btnClose.Location = New System.Drawing.Point(816, 460)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(59, 25)
        Me.btnClose.TabIndex = 230
        Me.btnClose.Text = "Cl&ose"
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close:Alt+O")
        '
        'frmRptReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(887, 485)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnEnter)
        Me.Controls.Add(Me.txtid)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.grpRights)
        Me.Controls.Add(Me.btnRights)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.vsfRpt)
        Me.MinimumSize = New System.Drawing.Size(900, 499)
        Me.Name = "frmRptReport"
        Me.ShowIcon = False
        Me.Text = "Reports"
        CType(Me.vsfRpt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfRights, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfUser, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpRights.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnRights As CButton
    Friend WithEvents btnClose As CButton
    Friend WithEvents vsfRpt As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents vsfRights As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents vsfUser As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents grpRights As GroupBox
    Friend WithEvents btnSave As CButton
    Friend WithEvents btnCancel As CButton
    Friend WithEvents Label1 As Label
    Friend WithEvents txtid As TextBox
    Friend WithEvents btnEnter As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents ToolTip1 As ToolTip
End Class
