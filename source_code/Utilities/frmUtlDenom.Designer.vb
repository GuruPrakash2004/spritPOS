<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlDenom
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlDenom))
        Me.btnConfig = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnRemoveRow = New System.Windows.Forms.Button()
        Me.btnMoveDown = New System.Windows.Forms.Button()
        Me.btnMoveUp = New System.Windows.Forms.Button()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtAmt = New System.Windows.Forms.TextBox()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnConfig
        '
        Me.btnConfig.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnConfig.Location = New System.Drawing.Point(36, 369)
        Me.btnConfig.Name = "btnConfig"
        Me.btnConfig.Size = New System.Drawing.Size(80, 29)
        Me.btnConfig.TabIndex = 61
        Me.btnConfig.Text = "&Config ->"
        Me.ToolTip1.SetToolTip(Me.btnConfig, "Configuation of Denomination" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "                    (Alt + C)")
        Me.btnConfig.UseVisualStyleBackColor = True
        Me.btnConfig.Visible = False
        '
        'btnAdd
        '
        Me.btnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAdd.Image = Global.vLQMS.My.Resources.Resources.T3_NEW
        Me.btnAdd.Location = New System.Drawing.Point(169, 369)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(89, 29)
        Me.btnAdd.TabIndex = 62
        Me.btnAdd.Text = "&Add"
        Me.btnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnAdd, "To add new Denomination" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "                  (Alt + A)")
        Me.btnAdd.UseVisualStyleBackColor = True
        Me.btnAdd.Visible = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Image = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.Location = New System.Drawing.Point(264, 369)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(95, 29)
        Me.btnSave.TabIndex = 63
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Save (Alt + S)")
        Me.btnSave.UseVisualStyleBackColor = True
        Me.btnSave.Visible = False
        '
        'btnRemoveRow
        '
        Me.btnRemoveRow.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRemoveRow.Image = CType(resources.GetObject("btnRemoveRow.Image"), System.Drawing.Image)
        Me.btnRemoveRow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRemoveRow.Location = New System.Drawing.Point(365, 369)
        Me.btnRemoveRow.Name = "btnRemoveRow"
        Me.btnRemoveRow.Size = New System.Drawing.Size(102, 29)
        Me.btnRemoveRow.TabIndex = 64
        Me.btnRemoveRow.Text = "&Remove Row"
        Me.btnRemoveRow.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnRemoveRow, "          Remove Row (Alt + R)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Removes selected row from the Grid")
        Me.btnRemoveRow.UseVisualStyleBackColor = True
        Me.btnRemoveRow.Visible = False
        '
        'btnMoveDown
        '
        Me.btnMoveDown.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.btnMoveDown.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnMoveDown.Image = CType(resources.GetObject("btnMoveDown.Image"), System.Drawing.Image)
        Me.btnMoveDown.Location = New System.Drawing.Point(5, 152)
        Me.btnMoveDown.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnMoveDown.Name = "btnMoveDown"
        Me.btnMoveDown.Size = New System.Drawing.Size(25, 32)
        Me.btnMoveDown.TabIndex = 222
        Me.ToolTip1.SetToolTip(Me.btnMoveDown, "Move the row down")
        Me.btnMoveDown.UseVisualStyleBackColor = True
        Me.btnMoveDown.Visible = False
        '
        'btnMoveUp
        '
        Me.btnMoveUp.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.btnMoveUp.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnMoveUp.Image = CType(resources.GetObject("btnMoveUp.Image"), System.Drawing.Image)
        Me.btnMoveUp.Location = New System.Drawing.Point(5, 115)
        Me.btnMoveUp.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnMoveUp.Name = "btnMoveUp"
        Me.btnMoveUp.Size = New System.Drawing.Size(25, 32)
        Me.btnMoveUp.TabIndex = 221
        Me.ToolTip1.SetToolTip(Me.btnMoveUp, "Move the row up")
        Me.btnMoveUp.UseVisualStyleBackColor = True
        Me.btnMoveUp.Visible = False
        '
        'vsfDet
        '
        Me.vsfDet.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfDet.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.BackColor = System.Drawing.Color.Ivory
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "2,0,0,0,0,105,Columns:"
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.Font = New System.Drawing.Font("Times New Roman", 9.75!)
        Me.vsfDet.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(36, 4)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 21
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(435, 311)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 223
        Me.vsfDet.Tag = ""
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Britannic Bold", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(35, 334)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(109, 23)
        Me.Label1.TabIndex = 225
        Me.Label1.Text = "Total Amt :"
        '
        'txtAmt
        '
        Me.txtAmt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAmt.BackColor = System.Drawing.SystemColors.Control
        Me.txtAmt.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtAmt.Font = New System.Drawing.Font("Britannic Bold", 15.75!)
        Me.txtAmt.Location = New System.Drawing.Point(262, 333)
        Me.txtAmt.Name = "txtAmt"
        Me.txtAmt.Size = New System.Drawing.Size(202, 24)
        Me.txtAmt.TabIndex = 226
        Me.txtAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'frmUtlDenom
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(477, 404)
        Me.Controls.Add(Me.txtAmt)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.btnMoveDown)
        Me.Controls.Add(Me.btnMoveUp)
        Me.Controls.Add(Me.btnRemoveRow)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.btnConfig)
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(493, 442)
        Me.Name = "frmUtlDenom"
        Me.ShowIcon = False
        Me.Tag = "UtlDenom"
        Me.Text = "Denomination"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnConfig As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnRemoveRow As System.Windows.Forms.Button
    Friend WithEvents btnMoveDown As System.Windows.Forms.Button
    Friend WithEvents btnMoveUp As System.Windows.Forms.Button
    Private WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtAmt As System.Windows.Forms.TextBox
End Class
