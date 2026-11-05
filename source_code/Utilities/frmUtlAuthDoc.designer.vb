<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlAuthDoc
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlAuthDoc))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chkSelClr = New System.Windows.Forms.CheckBox()
        Me.btnAuth = New C1.Win.C1Input.C1Button()
        Me.btnCancel = New C1.Win.C1Input.C1Button()
        Me.btnSave = New C1.Win.C1Input.C1Button()
        Me.vsfAuth = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.vsfAuth, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label1.Location = New System.Drawing.Point(9, -50)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(363, 17)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Please check the fields on which the search should take place"
        '
        'chkSelClr
        '
        Me.chkSelClr.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkSelClr.AutoSize = True
        Me.chkSelClr.Location = New System.Drawing.Point(12, 502)
        Me.chkSelClr.Name = "chkSelClr"
        Me.chkSelClr.Size = New System.Drawing.Size(114, 21)
        Me.chkSelClr.TabIndex = 17
        Me.chkSelClr.Text = "Select/Clear All"
        Me.chkSelClr.UseVisualStyleBackColor = True
        '
        'btnAuth
        '
        Me.btnAuth.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAuth.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAuth.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAuth.Image = CType(resources.GetObject("btnAuth.Image"), System.Drawing.Image)
        Me.btnAuth.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAuth.Location = New System.Drawing.Point(520, 445)
        Me.btnAuth.Margin = New System.Windows.Forms.Padding(4)
        Me.btnAuth.Name = "btnAuth"
        Me.btnAuth.Size = New System.Drawing.Size(112, 31)
        Me.btnAuth.TabIndex = 18
        Me.btnAuth.Text = "Authorise"
        Me.btnAuth.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnAuth.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(639, 445)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(95, 31)
        Me.btnCancel.TabIndex = 16
        Me.btnCancel.Text = "Cl&ose"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Close : Alt + O")
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(418, 445)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(95, 31)
        Me.btnSave.TabIndex = 15
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Save : Alt + S")
        Me.btnSave.UseVisualStyleBackColor = False
        Me.btnSave.Visible = False
        '
        'vsfAuth
        '
        Me.vsfAuth.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfAuth.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfAuth.AutoResize = True
        Me.vsfAuth.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfAuth.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfAuth.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfAuth.ExtendLastCol = True
        Me.vsfAuth.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.None
        Me.vsfAuth.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfAuth.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
        Me.vsfAuth.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfAuth.Location = New System.Drawing.Point(12, 13)
        Me.vsfAuth.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfAuth.Name = "vsfAuth"
        Me.vsfAuth.Rows.DefaultSize = 24
        Me.vsfAuth.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.vsfAuth.ScrollOptions = C1.Win.C1FlexGrid.ScrollFlags.ShowScrollTips
        Me.vsfAuth.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfAuth.Size = New System.Drawing.Size(722, 424)
        Me.vsfAuth.StyleInfo = resources.GetString("vsfAuth.StyleInfo")
        Me.vsfAuth.TabIndex = 0
        Me.vsfAuth.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2010Silver
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Blue
        Me.Label2.Location = New System.Drawing.Point(99, 450)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(254, 20)
        Me.Label2.TabIndex = 20
        Me.Label2.Text = "Double click to open the document"
        '
        'frmUtlAuthDoc
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(746, 486)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.vsfAuth)
        Me.Controls.Add(Me.btnAuth)
        Me.Controls.Add(Me.chkSelClr)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MinimumSize = New System.Drawing.Size(762, 524)
        Me.Name = "frmUtlAuthDoc"
        Me.ShowIcon = False
        Me.Text = "Authorise"
        CType(Me.vsfAuth, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnCancel As C1.Win.C1Input.C1Button
    Friend WithEvents chkSelClr As System.Windows.Forms.CheckBox
    Friend WithEvents btnSave As C1.Win.C1Input.C1Button
    Friend WithEvents btnAuth As C1.Win.C1Input.C1Button
    Friend WithEvents vsfAuth As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As ToolTip
End Class
