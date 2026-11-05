<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlFldsTemplate
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlFldsTemplate))
        Me.lblTemplate = New System.Windows.Forms.Label()
        Me.txtFlds = New System.Windows.Forms.RichTextBox()
        Me.vsfFlds = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cboTemplate = New System.Windows.Forms.ComboBox()
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.btnSave = New C1.Win.C1Input.C1Button()
        Me.btnClear = New C1.Win.C1Input.C1Button()
        Me.btnDelTemplate = New System.Windows.Forms.Button()
        Me.tt = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnNewTem = New System.Windows.Forms.Button()
        CType(Me.vsfFlds, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblTemplate
        '
        Me.lblTemplate.AutoSize = True
        Me.lblTemplate.Location = New System.Drawing.Point(12, 20)
        Me.lblTemplate.Name = "lblTemplate"
        Me.lblTemplate.Size = New System.Drawing.Size(82, 13)
        Me.lblTemplate.TabIndex = 0
        Me.lblTemplate.Text = "Template Name"
        '
        'txtFlds
        '
        Me.txtFlds.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFlds.Location = New System.Drawing.Point(15, 43)
        Me.txtFlds.MaxLength = 150
        Me.txtFlds.Name = "txtFlds"
        Me.txtFlds.Size = New System.Drawing.Size(321, 315)
        Me.txtFlds.TabIndex = 4
        Me.txtFlds.Text = ""
        '
        'vsfFlds
        '
        Me.vsfFlds.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfFlds.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfFlds.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfFlds.ColumnInfo = "2,0,0,0,0,120,Columns:0{Visible:False;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfFlds.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfFlds.ExtendLastCol = True
        Me.vsfFlds.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfFlds.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfFlds.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfFlds.Location = New System.Drawing.Point(342, 17)
        Me.vsfFlds.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfFlds.Name = "vsfFlds"
        Me.vsfFlds.Rows.DefaultSize = 24
        Me.vsfFlds.Rows.GlyphRow = 0
        Me.vsfFlds.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfFlds.ShowCellLabels = True
        Me.vsfFlds.Size = New System.Drawing.Size(303, 341)
        Me.vsfFlds.StyleInfo = resources.GetString("vsfFlds.StyleInfo")
        Me.vsfFlds.TabIndex = 3
        Me.vsfFlds.Tag = ""
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.Blue
        Me.Label2.Location = New System.Drawing.Point(207, 392)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(203, 13)
        Me.Label2.TabIndex = 15
        Me.Label2.Text = "Double Click on the Fields, to be selected"
        '
        'cboTemplate
        '
        Me.cboTemplate.FormattingEnabled = True
        Me.cboTemplate.Location = New System.Drawing.Point(111, 17)
        Me.cboTemplate.Name = "cboTemplate"
        Me.cboTemplate.Size = New System.Drawing.Size(138, 21)
        Me.cboTemplate.TabIndex = 0
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(567, 366)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(78, 29)
        Me.btnClose.TabIndex = 7
        Me.btnClose.Text = "C&lose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tt.SetToolTip(Me.btnClose, "Close: Alt+L")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(481, 366)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(78, 29)
        Me.btnSave.TabIndex = 6
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tt.SetToolTip(Me.btnSave, "Save: Alt + S")
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnClear
        '
        Me.btnClear.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClear.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClear.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClear.Location = New System.Drawing.Point(15, 367)
        Me.btnClear.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(100, 27)
        Me.btnClear.TabIndex = 5
        Me.btnClear.Text = "Clear text"
        Me.btnClear.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClear.UseVisualStyleBackColor = False
        '
        'btnDelTemplate
        '
        Me.btnDelTemplate.BackgroundImage = CType(resources.GetObject("btnDelTemplate.BackgroundImage"), System.Drawing.Image)
        Me.btnDelTemplate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnDelTemplate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDelTemplate.Location = New System.Drawing.Point(297, 17)
        Me.btnDelTemplate.Name = "btnDelTemplate"
        Me.btnDelTemplate.Size = New System.Drawing.Size(36, 24)
        Me.btnDelTemplate.TabIndex = 2
        Me.btnDelTemplate.Tag = ""
        Me.tt.SetToolTip(Me.btnDelTemplate, "To delete the selected Template")
        Me.btnDelTemplate.UseVisualStyleBackColor = True
        '
        'btnNewTem
        '
        Me.btnNewTem.BackgroundImage = Global.vLQMS.My.Resources.Resources.D_NEW
        Me.btnNewTem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnNewTem.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNewTem.Location = New System.Drawing.Point(255, 17)
        Me.btnNewTem.Name = "btnNewTem"
        Me.btnNewTem.Size = New System.Drawing.Size(36, 24)
        Me.btnNewTem.TabIndex = 1
        Me.tt.SetToolTip(Me.btnNewTem, "New Template ")
        Me.btnNewTem.UseVisualStyleBackColor = True
        '
        'frmUtlFldsTemplate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(657, 414)
        Me.Controls.Add(Me.btnNewTem)
        Me.Controls.Add(Me.btnDelTemplate)
        Me.Controls.Add(Me.cboTemplate)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.vsfFlds)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.txtFlds)
        Me.Controls.Add(Me.lblTemplate)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmUtlFldsTemplate"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        CType(Me.vsfFlds, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblTemplate As Label
    Friend WithEvents txtFlds As RichTextBox
    Friend WithEvents btnClear As C1.Win.C1Input.C1Button
    Friend WithEvents vsfFlds As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnSave As C1.Win.C1Input.C1Button
    Friend WithEvents Label2 As Label
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents cboTemplate As ComboBox
    Friend WithEvents btnDelTemplate As Button
    Friend WithEvents tt As ToolTip
    Friend WithEvents btnNewTem As Button
End Class
