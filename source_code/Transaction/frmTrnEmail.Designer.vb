<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmTrnEmail
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
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtBody = New System.Windows.Forms.TextBox()
        Me.txtAttach = New System.Windows.Forms.TextBox()
        Me.txtSub = New System.Windows.Forms.TextBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.txtCC = New System.Windows.Forms.TextBox()
        Me.txtTo = New System.Windows.Forms.TextBox()
        Me.btnSend = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnAttch = New System.Windows.Forms.Button()
        Me.btnTemplateSettings = New System.Windows.Forms.Button()
        Me.BottomToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.TopToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.RightToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.LeftToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.ContentPanel = New System.Windows.Forms.ToolStripContentPanel()
        Me.txtTranID = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cboTemplate = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblCount = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'txtBody
        '
        Me.txtBody.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBody.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtBody.Location = New System.Drawing.Point(90, 168)
        Me.txtBody.MaxLength = 5000
        Me.txtBody.Multiline = True
        Me.txtBody.Name = "txtBody"
        Me.txtBody.Size = New System.Drawing.Size(481, 258)
        Me.txtBody.TabIndex = 7
        Me.ToolTip1.SetToolTip(Me.txtBody, "Receiver's Mobile No")
        '
        'txtAttach
        '
        Me.txtAttach.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAttach.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtAttach.Location = New System.Drawing.Point(90, 137)
        Me.txtAttach.MaxLength = 250
        Me.txtAttach.Name = "txtAttach"
        Me.txtAttach.Size = New System.Drawing.Size(448, 25)
        Me.txtAttach.TabIndex = 5
        Me.ToolTip1.SetToolTip(Me.txtAttach, "Receiver's Mobile No")
        '
        'txtSub
        '
        Me.txtSub.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSub.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtSub.Location = New System.Drawing.Point(90, 106)
        Me.txtSub.MaxLength = 250
        Me.txtSub.Name = "txtSub"
        Me.txtSub.Size = New System.Drawing.Size(481, 25)
        Me.txtSub.TabIndex = 4
        Me.ToolTip1.SetToolTip(Me.txtSub, "Receiver's Mobile No")
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackColor = System.Drawing.Color.Transparent
        Me.btnCancel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnCancel.ForeColor = System.Drawing.Color.Black
        Me.btnCancel.Location = New System.Drawing.Point(389, 432)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(88, 30)
        Me.btnCancel.TabIndex = 9
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Shortcut Alt+C")
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'txtCC
        '
        Me.txtCC.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCC.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtCC.Location = New System.Drawing.Point(90, 75)
        Me.txtCC.MaxLength = 500
        Me.txtCC.Name = "txtCC"
        Me.txtCC.Size = New System.Drawing.Size(481, 25)
        Me.txtCC.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.txtCC, "Receiver's Mobile No")
        '
        'txtTo
        '
        Me.txtTo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTo.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtTo.Location = New System.Drawing.Point(90, 44)
        Me.txtTo.MaxLength = 500
        Me.txtTo.Name = "txtTo"
        Me.txtTo.Size = New System.Drawing.Size(481, 25)
        Me.txtTo.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.txtTo, "Receiver's Mobile No")
        '
        'btnSend
        '
        Me.btnSend.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSend.BackColor = System.Drawing.Color.Transparent
        Me.btnSend.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSend.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnSend.ForeColor = System.Drawing.Color.Black
        Me.btnSend.Location = New System.Drawing.Point(283, 432)
        Me.btnSend.Name = "btnSend"
        Me.btnSend.Size = New System.Drawing.Size(95, 30)
        Me.btnSend.TabIndex = 8
        Me.btnSend.Text = "Se&nd (F6)"
        Me.ToolTip1.SetToolTip(Me.btnSend, "Shortcut Alt+N")
        Me.btnSend.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackColor = System.Drawing.Color.Transparent
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnClose.ForeColor = System.Drawing.Color.Black
        Me.btnClose.Location = New System.Drawing.Point(483, 432)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(88, 30)
        Me.btnClose.TabIndex = 10
        Me.btnClose.Text = "&Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Shortcut Alt+C")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnAttch
        '
        Me.btnAttch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAttch.Location = New System.Drawing.Point(544, 137)
        Me.btnAttch.Name = "btnAttch"
        Me.btnAttch.Size = New System.Drawing.Size(27, 25)
        Me.btnAttch.TabIndex = 6
        Me.btnAttch.Text = "..."
        Me.ToolTip1.SetToolTip(Me.btnAttch, "Load Attachment")
        Me.btnAttch.UseVisualStyleBackColor = True
        '
        'btnTemplateSettings
        '
        Me.btnTemplateSettings.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnTemplateSettings.Location = New System.Drawing.Point(321, 13)
        Me.btnTemplateSettings.Name = "btnTemplateSettings"
        Me.btnTemplateSettings.Size = New System.Drawing.Size(27, 25)
        Me.btnTemplateSettings.TabIndex = 1
        Me.btnTemplateSettings.Tag = "SMS Template Settings"
        Me.btnTemplateSettings.Text = "..."
        Me.ToolTip1.SetToolTip(Me.btnTemplateSettings, "Load Template")
        Me.btnTemplateSettings.UseVisualStyleBackColor = True
        '
        'BottomToolStripPanel
        '
        Me.BottomToolStripPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.BottomToolStripPanel.Location = New System.Drawing.Point(0, 175)
        Me.BottomToolStripPanel.Name = "BottomToolStripPanel"
        Me.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.BottomToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.BottomToolStripPanel.Size = New System.Drawing.Size(150, 0)
        '
        'TopToolStripPanel
        '
        Me.TopToolStripPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me.TopToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.TopToolStripPanel.Name = "TopToolStripPanel"
        Me.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.TopToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.TopToolStripPanel.Size = New System.Drawing.Size(150, 25)
        '
        'RightToolStripPanel
        '
        Me.RightToolStripPanel.Dock = System.Windows.Forms.DockStyle.Right
        Me.RightToolStripPanel.Location = New System.Drawing.Point(150, 25)
        Me.RightToolStripPanel.Name = "RightToolStripPanel"
        Me.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Vertical
        Me.RightToolStripPanel.RowMargin = New System.Windows.Forms.Padding(0, 3, 0, 0)
        Me.RightToolStripPanel.Size = New System.Drawing.Size(0, 150)
        '
        'LeftToolStripPanel
        '
        Me.LeftToolStripPanel.Dock = System.Windows.Forms.DockStyle.Left
        Me.LeftToolStripPanel.Location = New System.Drawing.Point(0, 25)
        Me.LeftToolStripPanel.Name = "LeftToolStripPanel"
        Me.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Vertical
        Me.LeftToolStripPanel.RowMargin = New System.Windows.Forms.Padding(0, 3, 0, 0)
        Me.LeftToolStripPanel.Size = New System.Drawing.Size(0, 150)
        '
        'ContentPanel
        '
        Me.ContentPanel.Size = New System.Drawing.Size(150, 150)
        '
        'txtTranID
        '
        Me.txtTranID.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTranID.Location = New System.Drawing.Point(361, 14)
        Me.txtTranID.Name = "txtTranID"
        Me.txtTranID.Size = New System.Drawing.Size(25, 25)
        Me.txtTranID.TabIndex = 35
        Me.txtTranID.Visible = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label7.ForeColor = System.Drawing.Color.Black
        Me.Label7.Location = New System.Drawing.Point(11, 171)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(37, 17)
        Me.Label7.TabIndex = 60
        Me.Label7.Text = "Body"
        '
        'Label6
        '
        Me.Label6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label6.ForeColor = System.Drawing.Color.Black
        Me.Label6.Location = New System.Drawing.Point(11, 140)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(73, 17)
        Me.Label6.TabIndex = 58
        Me.Label6.Text = "Attachment"
        '
        'Label4
        '
        Me.Label4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label4.ForeColor = System.Drawing.Color.Black
        Me.Label4.Location = New System.Drawing.Point(12, 109)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(50, 17)
        Me.Label4.TabIndex = 56
        Me.Label4.Text = "Subject"
        '
        'Label3
        '
        Me.Label3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label3.ForeColor = System.Drawing.Color.Black
        Me.Label3.Location = New System.Drawing.Point(12, 78)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(24, 17)
        Me.Label3.TabIndex = 53
        Me.Label3.Text = "CC"
        '
        'Label2
        '
        Me.Label2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(8, 18)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(61, 17)
        Me.Label2.TabIndex = 52
        Me.Label2.Text = "Template"
        '
        'cboTemplate
        '
        Me.cboTemplate.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboTemplate.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.cboTemplate.FormattingEnabled = True
        Me.cboTemplate.Location = New System.Drawing.Point(90, 13)
        Me.cboTemplate.Name = "cboTemplate"
        Me.cboTemplate.Size = New System.Drawing.Size(225, 25)
        Me.cboTemplate.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(12, 47)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(22, 17)
        Me.Label1.TabIndex = 47
        Me.Label1.Text = "To"
        '
        'lblCount
        '
        Me.lblCount.AutoSize = True
        Me.lblCount.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblCount.ForeColor = System.Drawing.Color.Black
        Me.lblCount.Location = New System.Drawing.Point(87, 432)
        Me.lblCount.Name = "lblCount"
        Me.lblCount.Size = New System.Drawing.Size(80, 17)
        Me.lblCount.TabIndex = 63
        Me.lblCount.Text = "Body Length"
        '
        'frmTrnEmail
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(583, 474)
        Me.Controls.Add(Me.lblCount)
        Me.Controls.Add(Me.btnAttch)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtBody)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtAttach)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtSub)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtCC)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cboTemplate)
        Me.Controls.Add(Me.btnTemplateSettings)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtTo)
        Me.Controls.Add(Me.btnSend)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.txtTranID)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmTrnEmail"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Send Email"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents BottomToolStripPanel As ToolStripPanel
    Friend WithEvents TopToolStripPanel As ToolStripPanel
    Friend WithEvents RightToolStripPanel As ToolStripPanel
    Friend WithEvents LeftToolStripPanel As ToolStripPanel
    Friend WithEvents ContentPanel As ToolStripContentPanel
    Friend WithEvents txtTranID As TextBox
    Friend WithEvents btnAttch As Button
    Friend WithEvents Label7 As Label
    Friend WithEvents txtBody As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents txtAttach As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents txtSub As TextBox
    Friend WithEvents btnCancel As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents txtCC As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents cboTemplate As ComboBox
    Friend WithEvents btnTemplateSettings As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents txtTo As TextBox
    Friend WithEvents btnSend As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents lblCount As Label
End Class
