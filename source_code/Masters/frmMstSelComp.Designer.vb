<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMstSelComp
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMstSelComp))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.btnok = New C1.Win.C1Input.C1Button()
        Me.btnSwitchDB = New C1.Win.C1Input.C1Button()
        Me.vsfComp = New C1.Win.C1FlexGrid.C1FlexGrid()
        CType(Me.vsfComp, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(182, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(314, 30)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Select a Company to work with : "
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.Label3.ForeColor = System.Drawing.Color.Blue
        Me.Label3.Location = New System.Drawing.Point(183, 353)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(502, 51)
        Me.Label3.TabIndex = 17
        Me.Label3.Text = "Select the company and Click on the button Select ------ or  Press Enter button t" &
    "o select the company to work with..."
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.btnClose.Image = Global.vLQMS.My.Resources.Resources.cancel
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(587, 314)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(98, 31)
        Me.btnClose.TabIndex = 16
        Me.btnClose.Text = "&Cancel"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnok
        '
        Me.btnok.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnok.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnok.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.btnok.Image = Global.vLQMS.My.Resources.Resources.D_EDIT
        Me.btnok.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnok.Location = New System.Drawing.Point(479, 314)
        Me.btnok.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnok.Name = "btnok"
        Me.btnok.Size = New System.Drawing.Size(98, 31)
        Me.btnok.TabIndex = 15
        Me.btnok.Text = "Se&lect" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.btnok.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnok.UseVisualStyleBackColor = False
        '
        'btnSwitchDB
        '
        Me.btnSwitchDB.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSwitchDB.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSwitchDB.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.btnSwitchDB.Image = Global.vLQMS.My.Resources.Resources.comp
        Me.btnSwitchDB.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSwitchDB.Location = New System.Drawing.Point(186, 314)
        Me.btnSwitchDB.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnSwitchDB.Name = "btnSwitchDB"
        Me.btnSwitchDB.Size = New System.Drawing.Size(112, 31)
        Me.btnSwitchDB.TabIndex = 14
        Me.btnSwitchDB.Text = "S&witch to Database"
        Me.btnSwitchDB.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSwitchDB.UseVisualStyleBackColor = False
        '
        'vsfComp
        '
        Me.vsfComp.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfComp.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfComp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.vsfComp.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes
        Me.vsfComp.ColumnInfo = "5,0,0,0,0,140,Columns:1{Style:""TextAlign:LeftCenter;"";StyleFixed:""TextAlign:LeftC" &
    "enter;"";}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfComp.ExtendLastCol = True
        Me.vsfComp.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.vsfComp.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
        Me.vsfComp.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfComp.Location = New System.Drawing.Point(186, 41)
        Me.vsfComp.Name = "vsfComp"
        Me.vsfComp.Rows.Count = 4
        Me.vsfComp.Rows.DefaultSize = 28
        Me.vsfComp.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfComp.Size = New System.Drawing.Size(499, 265)
        Me.vsfComp.StyleInfo = resources.GetString("vsfComp.StyleInfo")
        Me.vsfComp.TabIndex = 10
        Me.vsfComp.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2007Black
        '
        'frmMstSelComp
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(697, 407)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnok)
        Me.Controls.Add(Me.btnSwitchDB)
        Me.Controls.Add(Me.vsfComp)
        Me.Controls.Add(Me.Label1)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(713, 445)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(713, 445)
        Me.Name = "frmMstSelComp"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Select a Company to work"
        CType(Me.vsfComp, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents vsfComp As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnSwitchDB As C1.Win.C1Input.C1Button
    Friend WithEvents btnok As C1.Win.C1Input.C1Button
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
End Class
