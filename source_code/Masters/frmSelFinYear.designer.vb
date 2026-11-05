<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmSelFinYear
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmSelFinYear))
        Me.vsfFnlYr = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnCancel = New C1.Win.C1Input.C1Button()
        Me.btnSave = New C1.Win.C1Input.C1Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnSelFinyear = New C1.Win.C1Input.C1Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.lblFrm = New System.Windows.Forms.Label()
        Me.lblDblClick = New System.Windows.Forms.Label()
        CType(Me.vsfFnlYr, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'vsfFnlYr
        '
        Me.vsfFnlYr.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfFnlYr.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfFnlYr.ColumnInfo = resources.GetString("vsfFnlYr.ColumnInfo")
        Me.vsfFnlYr.ExtendLastCol = True
        Me.vsfFnlYr.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfFnlYr.Location = New System.Drawing.Point(-2, 37)
        Me.vsfFnlYr.Name = "vsfFnlYr"
        Me.vsfFnlYr.Rows.DefaultSize = 24
        Me.vsfFnlYr.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfFnlYr.Size = New System.Drawing.Size(623, 229)
        Me.vsfFnlYr.StyleInfo = resources.GetString("vsfFnlYr.StyleInfo")
        Me.vsfFnlYr.TabIndex = 0
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(538, 277)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(90, 31)
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "Cl&ose"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Close : Alt+O")
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Image = CType(resources.GetObject("btnSave.Image"), System.Drawing.Image)
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(389, 277)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(141, 31)
        Me.btnSave.TabIndex = 1
        Me.btnSave.Text = "Set As Ac&tive"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Alt + T")
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnSelFinyear
        '
        Me.btnSelFinyear.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSelFinyear.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSelFinyear.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSelFinyear.Image = CType(resources.GetObject("btnSelFinyear.Image"), System.Drawing.Image)
        Me.btnSelFinyear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSelFinyear.Location = New System.Drawing.Point(0, 277)
        Me.btnSelFinyear.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSelFinyear.Name = "btnSelFinyear"
        Me.btnSelFinyear.Size = New System.Drawing.Size(97, 31)
        Me.btnSelFinyear.TabIndex = 3
        Me.btnSelFinyear.Text = "Select"
        Me.btnSelFinyear.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSelFinyear, "Select Financial Year")
        Me.btnSelFinyear.UseVisualStyleBackColor = False
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(53, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(106, Byte), Integer))
        Me.Panel2.Controls.Add(Me.lblFrm)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(628, 35)
        Me.Panel2.TabIndex = 109
        '
        'lblFrm
        '
        Me.lblFrm.AutoSize = True
        Me.lblFrm.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lblFrm.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFrm.ForeColor = System.Drawing.Color.White
        Me.lblFrm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblFrm.Location = New System.Drawing.Point(13, 10)
        Me.lblFrm.Name = "lblFrm"
        Me.lblFrm.Size = New System.Drawing.Size(0, 25)
        Me.lblFrm.TabIndex = 1
        '
        'lblDblClick
        '
        Me.lblDblClick.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblDblClick.AutoSize = True
        Me.lblDblClick.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDblClick.ForeColor = System.Drawing.Color.Blue
        Me.lblDblClick.Location = New System.Drawing.Point(104, 285)
        Me.lblDblClick.Name = "lblDblClick"
        Me.lblDblClick.Size = New System.Drawing.Size(265, 20)
        Me.lblDblClick.TabIndex = 110
        Me.lblDblClick.Text = "Double Click to Select Financial year"
        '
        'FrmSelFinYear
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(628, 311)
        Me.Controls.Add(Me.lblDblClick)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.btnSelFinyear)
        Me.Controls.Add(Me.vsfFnlYr)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(0, 50)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MinimumSize = New System.Drawing.Size(596, 329)
        Me.Name = "FrmSelFinYear"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Select Financial Year"
        CType(Me.vsfFnlYr, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents vsfFnlYr As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnCancel As C1.Win.C1Input.C1Button
    Friend WithEvents btnSave As C1.Win.C1Input.C1Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents btnSelFinyear As C1.Win.C1Input.C1Button
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Public WithEvents lblFrm As Label
    Friend WithEvents lblDblClick As Label
End Class
