<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlTrnFldsTemplate
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlTrnFldsTemplate))
        Me.lblTemplate = New System.Windows.Forms.Label()
        Me.txtFlds = New System.Windows.Forms.RichTextBox()
        Me.vsfFlds = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.cboTemplate = New System.Windows.Forms.ComboBox()
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.btnSave = New C1.Win.C1Input.C1Button()
        Me.btnClear = New C1.Win.C1Input.C1Button()
        Me.btnDelTemplate = New System.Windows.Forms.Button()
        Me.tt = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnNewTem = New System.Windows.Forms.Button()
        Me.vsfTbls = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtMobileNo = New System.Windows.Forms.TextBox()
        Me.optStdTbls = New System.Windows.Forms.RadioButton()
        Me.optQuery = New System.Windows.Forms.RadioButton()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.txtQueryText = New System.Windows.Forms.TextBox()
        Me.chkAutoSend = New System.Windows.Forms.CheckBox()
        Me.txtCond = New System.Windows.Forms.TextBox()
        Me.lblInfo1 = New System.Windows.Forms.Label()
        Me.lblInfo2 = New System.Windows.Forms.Label()
        Me.chkDef = New System.Windows.Forms.CheckBox()
        CType(Me.vsfFlds, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfTbls, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblTemplate
        '
        Me.lblTemplate.AutoSize = True
        Me.lblTemplate.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemplate.Location = New System.Drawing.Point(14, 26)
        Me.lblTemplate.Name = "lblTemplate"
        Me.lblTemplate.Size = New System.Drawing.Size(120, 21)
        Me.lblTemplate.TabIndex = 0
        Me.lblTemplate.Text = "Template Name"
        '
        'txtFlds
        '
        Me.txtFlds.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtFlds.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFlds.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFlds.Location = New System.Drawing.Point(17, 94)
        Me.txtFlds.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtFlds.MaxLength = 150
        Me.txtFlds.Name = "txtFlds"
        Me.txtFlds.Size = New System.Drawing.Size(374, 364)
        Me.txtFlds.TabIndex = 4
        Me.txtFlds.Text = ""
        '
        'vsfFlds
        '
        Me.vsfFlds.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
        Me.vsfFlds.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfFlds.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfFlds.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfFlds.ColumnInfo = "3,0,0,0,0,140,Columns:"
        Me.vsfFlds.ExtendLastCol = True
        Me.vsfFlds.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfFlds.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.vsfFlds.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfFlds.Location = New System.Drawing.Point(399, 75)
        Me.vsfFlds.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.vsfFlds.Name = "vsfFlds"
        Me.vsfFlds.Rows.DefaultSize = 28
        Me.vsfFlds.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfFlds.ShowCellLabels = True
        Me.vsfFlds.Size = New System.Drawing.Size(364, 383)
        Me.vsfFlds.StyleInfo = resources.GetString("vsfFlds.StyleInfo")
        Me.vsfFlds.TabIndex = 3
        Me.vsfFlds.Tag = ""
        '
        'lblInfo
        '
        Me.lblInfo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblInfo.AutoSize = True
        Me.lblInfo.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInfo.ForeColor = System.Drawing.Color.Blue
        Me.lblInfo.Location = New System.Drawing.Point(515, 463)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(248, 17)
        Me.lblInfo.TabIndex = 15
        Me.lblInfo.Text = "Double Click on the Fields, to be selected"
        '
        'cboTemplate
        '
        Me.cboTemplate.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTemplate.FormattingEnabled = True
        Me.cboTemplate.Location = New System.Drawing.Point(140, 22)
        Me.cboTemplate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.cboTemplate.Name = "cboTemplate"
        Me.cboTemplate.Size = New System.Drawing.Size(149, 29)
        Me.cboTemplate.TabIndex = 0
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(672, 509)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(5)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(91, 38)
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
        Me.btnSave.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(572, 509)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(5)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(91, 38)
        Me.btnSave.TabIndex = 6
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tt.SetToolTip(Me.btnSave, "Save: Alt + S")
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnClear
        '
        Me.btnClear.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnClear.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClear.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClear.Location = New System.Drawing.Point(18, 467)
        Me.btnClear.Margin = New System.Windows.Forms.Padding(5)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(116, 35)
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
        Me.btnDelTemplate.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDelTemplate.Location = New System.Drawing.Point(346, 22)
        Me.btnDelTemplate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnDelTemplate.Name = "btnDelTemplate"
        Me.btnDelTemplate.Size = New System.Drawing.Size(42, 31)
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
        Me.btnNewTem.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNewTem.Location = New System.Drawing.Point(297, 22)
        Me.btnNewTem.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnNewTem.Name = "btnNewTem"
        Me.btnNewTem.Size = New System.Drawing.Size(42, 31)
        Me.btnNewTem.TabIndex = 1
        Me.tt.SetToolTip(Me.btnNewTem, "New Template ")
        Me.btnNewTem.UseVisualStyleBackColor = True
        '
        'vsfTbls
        '
        Me.vsfTbls.AllowEditing = False
        Me.vsfTbls.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
        Me.vsfTbls.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfTbls.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfTbls.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfTbls.ColumnInfo = "1,0,0,0,0,95,Columns:"
        Me.vsfTbls.ExtendLastCol = True
        Me.vsfTbls.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Solid
        Me.vsfTbls.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.vsfTbls.Location = New System.Drawing.Point(110, 248)
        Me.vsfTbls.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfTbls.Name = "vsfTbls"
        Me.vsfTbls.Rows.Count = 4
        Me.vsfTbls.Rows.DefaultSize = 19
        Me.vsfTbls.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfTbls.Size = New System.Drawing.Size(135, 150)
        Me.vsfTbls.StyleInfo = resources.GetString("vsfTbls.StyleInfo")
        Me.vsfTbls.TabIndex = 207
        Me.vsfTbls.Tag = "DSGR"
        Me.vsfTbls.Visible = False
        '
        'txtSearch
        '
        Me.txtSearch.Location = New System.Drawing.Point(50, 292)
        Me.txtSearch.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtSearch.MaxLength = 500
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(219, 25)
        Me.txtSearch.TabIndex = 208
        Me.txtSearch.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(16, 64)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(126, 21)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Fixed Mobile No."
        '
        'txtMobileNo
        '
        Me.txtMobileNo.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMobileNo.Location = New System.Drawing.Point(140, 61)
        Me.txtMobileNo.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtMobileNo.Name = "txtMobileNo"
        Me.txtMobileNo.Size = New System.Drawing.Size(248, 29)
        Me.txtMobileNo.TabIndex = 209
        '
        'optStdTbls
        '
        Me.optStdTbls.AutoSize = True
        Me.optStdTbls.Checked = True
        Me.optStdTbls.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optStdTbls.Location = New System.Drawing.Point(77, 24)
        Me.optStdTbls.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.optStdTbls.Name = "optStdTbls"
        Me.optStdTbls.Size = New System.Drawing.Size(139, 25)
        Me.optStdTbls.TabIndex = 210
        Me.optStdTbls.TabStop = True
        Me.optStdTbls.Text = "Standard Tables"
        Me.optStdTbls.UseVisualStyleBackColor = True
        '
        'optQuery
        '
        Me.optQuery.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.optQuery.AutoSize = True
        Me.optQuery.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optQuery.Location = New System.Drawing.Point(238, 24)
        Me.optQuery.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.optQuery.Name = "optQuery"
        Me.optQuery.Size = New System.Drawing.Size(104, 25)
        Me.optQuery.TabIndex = 211
        Me.optQuery.Text = "SQL Query"
        Me.optQuery.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.optStdTbls)
        Me.GroupBox1.Controls.Add(Me.optQuery)
        Me.GroupBox1.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(399, 4)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.GroupBox1.Size = New System.Drawing.Size(364, 61)
        Me.GroupBox1.TabIndex = 212
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Field Selection"
        '
        'txtQueryText
        '
        Me.txtQueryText.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtQueryText.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtQueryText.Location = New System.Drawing.Point(399, 73)
        Me.txtQueryText.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtQueryText.Multiline = True
        Me.txtQueryText.Name = "txtQueryText"
        Me.txtQueryText.Size = New System.Drawing.Size(364, 385)
        Me.txtQueryText.TabIndex = 213
        Me.txtQueryText.Visible = False
        '
        'chkAutoSend
        '
        Me.chkAutoSend.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkAutoSend.AutoSize = True
        Me.chkAutoSend.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAutoSend.Location = New System.Drawing.Point(152, 469)
        Me.chkAutoSend.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.chkAutoSend.Name = "chkAutoSend"
        Me.chkAutoSend.Size = New System.Drawing.Size(101, 25)
        Me.chkAutoSend.TabIndex = 214
        Me.chkAutoSend.Text = "Auto Send"
        Me.chkAutoSend.UseVisualStyleBackColor = True
        '
        'txtCond
        '
        Me.txtCond.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCond.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCond.Location = New System.Drawing.Point(259, 467)
        Me.txtCond.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtCond.MaxLength = 500
        Me.txtCond.Multiline = True
        Me.txtCond.Name = "txtCond"
        Me.txtCond.Size = New System.Drawing.Size(250, 79)
        Me.txtCond.TabIndex = 215
        Me.txtCond.Visible = False
        '
        'lblInfo1
        '
        Me.lblInfo1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblInfo1.AutoSize = True
        Me.lblInfo1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInfo1.ForeColor = System.Drawing.Color.Blue
        Me.lblInfo1.Location = New System.Drawing.Point(17, 508)
        Me.lblInfo1.Name = "lblInfo1"
        Me.lblInfo1.Size = New System.Drawing.Size(155, 17)
        Me.lblInfo1.TabIndex = 216
        Me.lblInfo1.Text = "Write fields within << >>"
        '
        'lblInfo2
        '
        Me.lblInfo2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblInfo2.AutoSize = True
        Me.lblInfo2.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInfo2.ForeColor = System.Drawing.Color.Blue
        Me.lblInfo2.Location = New System.Drawing.Point(17, 530)
        Me.lblInfo2.Name = "lblInfo2"
        Me.lblInfo2.Size = New System.Drawing.Size(236, 17)
        Me.lblInfo2.TabIndex = 217
        Me.lblInfo2.Text = "Each conditions are separated by 'And'"
        '
        'chkDef
        '
        Me.chkDef.AutoSize = True
        Me.chkDef.Location = New System.Drawing.Point(515, 483)
        Me.chkDef.Name = "chkDef"
        Me.chkDef.Size = New System.Drawing.Size(241, 21)
        Me.chkDef.TabIndex = 218
        Me.chkDef.Text = "Default Template For this transaction"
        Me.chkDef.UseVisualStyleBackColor = True
        '
        'frmUtlTrnFldsTemplate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(777, 560)
        Me.Controls.Add(Me.chkDef)
        Me.Controls.Add(Me.lblInfo2)
        Me.Controls.Add(Me.lblInfo1)
        Me.Controls.Add(Me.txtCond)
        Me.Controls.Add(Me.chkAutoSend)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.txtMobileNo)
        Me.Controls.Add(Me.btnNewTem)
        Me.Controls.Add(Me.btnDelTemplate)
        Me.Controls.Add(Me.cboTemplate)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.lblInfo)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.txtFlds)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblTemplate)
        Me.Controls.Add(Me.txtSearch)
        Me.Controls.Add(Me.vsfTbls)
        Me.Controls.Add(Me.vsfFlds)
        Me.Controls.Add(Me.txtQueryText)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "frmUtlTrnFldsTemplate"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        CType(Me.vsfFlds, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfTbls, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblTemplate As Label
    Friend WithEvents txtFlds As RichTextBox
    Friend WithEvents btnClear As C1.Win.C1Input.C1Button
    Friend WithEvents vsfFlds As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnSave As C1.Win.C1Input.C1Button
    Friend WithEvents lblInfo As Label
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents cboTemplate As ComboBox
    Friend WithEvents btnDelTemplate As Button
    Friend WithEvents tt As ToolTip
    Friend WithEvents btnNewTem As Button
    Friend WithEvents txtSearch As TextBox
    Public WithEvents vsfTbls As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents Label1 As Label
    Friend WithEvents txtMobileNo As TextBox
    Friend WithEvents optStdTbls As RadioButton
    Friend WithEvents optQuery As RadioButton
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents txtQueryText As TextBox
    Friend WithEvents chkAutoSend As CheckBox
    Friend WithEvents txtCond As TextBox
    Friend WithEvents lblInfo1 As Label
    Friend WithEvents lblInfo2 As Label
    Friend WithEvents chkDef As CheckBox
End Class
