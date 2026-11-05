<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlAuth
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlAuth))
        Me.vsfAuth = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnAuth = New C1.Win.C1Input.C1Button()
        Me.BtnRefresh = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.vsfAuth, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'vsfAuth
        '
        Me.vsfAuth.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfAuth.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfAuth.AutoResize = True
        Me.vsfAuth.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfAuth.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None
        Me.vsfAuth.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfAuth.ExtendLastCol = True
        Me.vsfAuth.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.None
        Me.vsfAuth.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfAuth.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
        Me.vsfAuth.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfAuth.Location = New System.Drawing.Point(15, 19)
        Me.vsfAuth.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfAuth.Name = "vsfAuth"
        Me.vsfAuth.Rows.DefaultSize = 24
        Me.vsfAuth.Rows.MaxSize = 50
        Me.vsfAuth.Rows.MinSize = 40
        Me.vsfAuth.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.vsfAuth.ScrollOptions = C1.Win.C1FlexGrid.ScrollFlags.ShowScrollTips
        Me.vsfAuth.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfAuth.Size = New System.Drawing.Size(638, 388)
        Me.vsfAuth.StyleInfo = resources.GetString("vsfAuth.StyleInfo")
        Me.vsfAuth.TabIndex = 0
        Me.vsfAuth.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2010Silver
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.CausesValidation = False
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(570, 432)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(83, 29)
        Me.btnClose.TabIndex = 3
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close : Alt + O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(12, 437)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(329, 20)
        Me.Label1.TabIndex = 13
        Me.Label1.Text = "Double click to open pending for authorise list"
        '
        'btnAuth
        '
        Me.btnAuth.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAuth.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAuth.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAuth.Image = CType(resources.GetObject("btnAuth.Image"), System.Drawing.Image)
        Me.btnAuth.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAuth.Location = New System.Drawing.Point(417, 432)
        Me.btnAuth.Margin = New System.Windows.Forms.Padding(4)
        Me.btnAuth.Name = "btnAuth"
        Me.btnAuth.Size = New System.Drawing.Size(145, 29)
        Me.btnAuth.TabIndex = 2
        Me.btnAuth.Text = "Select Doc for Authorisation"
        Me.btnAuth.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnAuth.UseVisualStyleBackColor = False
        '
        'BtnRefresh
        '
        Me.BtnRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnRefresh.BackColor = System.Drawing.Color.Transparent
        Me.BtnRefresh.BackgroundImage = CType(resources.GetObject("BtnRefresh.BackgroundImage"), System.Drawing.Image)
        Me.BtnRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.BtnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnRefresh.Location = New System.Drawing.Point(16, 414)
        Me.BtnRefresh.Name = "BtnRefresh"
        Me.BtnRefresh.Size = New System.Drawing.Size(33, 24)
        Me.BtnRefresh.TabIndex = 1
        Me.BtnRefresh.Tag = ""
        Me.BtnRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ToolTip1.SetToolTip(Me.BtnRefresh, "Refresh Documents for Authorization")
        Me.BtnRefresh.UseVisualStyleBackColor = False
        '
        'frmUtlAuth
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(665, 474)
        Me.Controls.Add(Me.BtnRefresh)
        Me.Controls.Add(Me.btnAuth)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.vsfAuth)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MinimumSize = New System.Drawing.Size(434, 491)
        Me.Name = "frmUtlAuth"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Authorisation"
        CType(Me.vsfAuth, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents vsfAuth As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnAuth As C1.Win.C1Input.C1Button
    Friend WithEvents BtnRefresh As Button
    Friend WithEvents ToolTip1 As ToolTip
End Class
