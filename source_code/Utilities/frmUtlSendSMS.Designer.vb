<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlSendSMS
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlSendSMS))
        Me.fraEntry = New System.Windows.Forms.Panel()
        Me.txtID = New System.Windows.Forms.TextBox()
        Me.chkDefault = New System.Windows.Forms.CheckBox()
        Me.txtDailyLimit = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtSMSURL = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.txtCode = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnTestSMS = New System.Windows.Forms.Button()
        Me.btnOpenHistory = New System.Windows.Forms.Button()
        Me.btnInvSMS = New System.Windows.Forms.Button()
        Me.pnlGrid = New System.Windows.Forms.Panel()
        Me.btnRemoveRowAUOM = New System.Windows.Forms.Button()
        Me.btnDetVertical = New System.Windows.Forms.Button()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.pnlTestSMS = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtMobile = New System.Windows.Forms.TextBox()
        Me.fraEntry.SuspendLayout()
        Me.pnlGrid.SuspendLayout()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlTestSMS.SuspendLayout()
        Me.SuspendLayout()
        '
        'fraEntry
        '
        Me.fraEntry.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fraEntry.Controls.Add(Me.txtID)
        Me.fraEntry.Controls.Add(Me.chkDefault)
        Me.fraEntry.Controls.Add(Me.txtDailyLimit)
        Me.fraEntry.Controls.Add(Me.Label6)
        Me.fraEntry.Controls.Add(Me.txtSMSURL)
        Me.fraEntry.Controls.Add(Me.Label8)
        Me.fraEntry.Controls.Add(Me.btnClose)
        Me.fraEntry.Controls.Add(Me.btnSave)
        Me.fraEntry.Controls.Add(Me.txtCode)
        Me.fraEntry.Controls.Add(Me.Label1)
        Me.fraEntry.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraEntry.Location = New System.Drawing.Point(0, 2)
        Me.fraEntry.Name = "fraEntry"
        Me.fraEntry.Size = New System.Drawing.Size(627, 155)
        Me.fraEntry.TabIndex = 0
        '
        'txtID
        '
        Me.txtID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtID.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtID.Location = New System.Drawing.Point(459, 13)
        Me.txtID.MaxLength = 150
        Me.txtID.Name = "txtID"
        Me.txtID.Size = New System.Drawing.Size(76, 26)
        Me.txtID.TabIndex = 41
        Me.txtID.Visible = False
        '
        'chkDefault
        '
        Me.chkDefault.AutoSize = True
        Me.chkDefault.Location = New System.Drawing.Point(342, 9)
        Me.chkDefault.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.chkDefault.Name = "chkDefault"
        Me.chkDefault.Size = New System.Drawing.Size(80, 24)
        Me.chkDefault.TabIndex = 40
        Me.chkDefault.Text = "Default"
        Me.chkDefault.UseVisualStyleBackColor = True
        '
        'txtDailyLimit
        '
        Me.txtDailyLimit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDailyLimit.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDailyLimit.Location = New System.Drawing.Point(90, 40)
        Me.txtDailyLimit.MaxLength = 150
        Me.txtDailyLimit.Name = "txtDailyLimit"
        Me.txtDailyLimit.Size = New System.Drawing.Size(76, 26)
        Me.txtDailyLimit.TabIndex = 31
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(7, 44)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(80, 20)
        Me.Label6.TabIndex = 39
        Me.Label6.Text = "Daily Limit"
        '
        'txtSMSURL
        '
        Me.txtSMSURL.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSMSURL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSMSURL.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSMSURL.Location = New System.Drawing.Point(90, 71)
        Me.txtSMSURL.MaxLength = 250
        Me.txtSMSURL.Multiline = True
        Me.txtSMSURL.Name = "txtSMSURL"
        Me.txtSMSURL.Size = New System.Drawing.Size(527, 79)
        Me.txtSMSURL.TabIndex = 33
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(7, 101)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(81, 20)
        Me.Label8.TabIndex = 37
        Me.Label8.Text = "SMS URL"
        '
        'btnClose
        '
        Me.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(516, 8)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(81, 35)
        Me.btnClose.TabIndex = 36
        Me.btnClose.Text = "Cancel"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close : Alt + O")
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(418, 8)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(81, 35)
        Me.btnSave.TabIndex = 35
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Save : Alt + S")
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'txtCode
        '
        Me.txtCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCode.Location = New System.Drawing.Point(90, 9)
        Me.txtCode.MaxLength = 150
        Me.txtCode.Name = "txtCode"
        Me.txtCode.Size = New System.Drawing.Size(247, 26)
        Me.txtCode.TabIndex = 26
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(7, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(66, 20)
        Me.Label1.TabIndex = 32
        Me.Label1.Text = "Provider"
        '
        'btnTestSMS
        '
        Me.btnTestSMS.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.btnTestSMS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnTestSMS.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTestSMS.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTestSMS.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnTestSMS.Location = New System.Drawing.Point(396, 5)
        Me.btnTestSMS.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnTestSMS.Name = "btnTestSMS"
        Me.btnTestSMS.Size = New System.Drawing.Size(176, 30)
        Me.btnTestSMS.TabIndex = 41
        Me.btnTestSMS.Text = "Send Test Message"
        Me.btnTestSMS.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnTestSMS, "Save : Alt + S")
        Me.btnTestSMS.UseVisualStyleBackColor = False
        '
        'btnOpenHistory
        '
        Me.btnOpenHistory.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnOpenHistory.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(242, Byte), Integer), CType(CType(191, Byte), Integer))
        Me.btnOpenHistory.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnOpenHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOpenHistory.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOpenHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnOpenHistory.Location = New System.Drawing.Point(75, 162)
        Me.btnOpenHistory.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnOpenHistory.Name = "btnOpenHistory"
        Me.btnOpenHistory.Size = New System.Drawing.Size(179, 25)
        Me.btnOpenHistory.TabIndex = 42
        Me.btnOpenHistory.Text = "Open SMS History"
        Me.btnOpenHistory.UseVisualStyleBackColor = False
        '
        'btnInvSMS
        '
        Me.btnInvSMS.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnInvSMS.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(242, Byte), Integer), CType(CType(191, Byte), Integer))
        Me.btnInvSMS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnInvSMS.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnInvSMS.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnInvSMS.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnInvSMS.Location = New System.Drawing.Point(269, 162)
        Me.btnInvSMS.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnInvSMS.Name = "btnInvSMS"
        Me.btnInvSMS.Size = New System.Drawing.Size(230, 25)
        Me.btnInvSMS.TabIndex = 589
        Me.btnInvSMS.Text = "Invoice - SMS Settings"
        Me.btnInvSMS.UseVisualStyleBackColor = False
        '
        'pnlGrid
        '
        Me.pnlGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlGrid.Controls.Add(Me.btnInvSMS)
        Me.pnlGrid.Controls.Add(Me.btnOpenHistory)
        Me.pnlGrid.Controls.Add(Me.btnRemoveRowAUOM)
        Me.pnlGrid.Controls.Add(Me.btnDetVertical)
        Me.pnlGrid.Controls.Add(Me.vsfDet)
        Me.pnlGrid.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlGrid.Location = New System.Drawing.Point(0, 200)
        Me.pnlGrid.Name = "pnlGrid"
        Me.pnlGrid.Size = New System.Drawing.Size(621, 188)
        Me.pnlGrid.TabIndex = 39
        '
        'btnRemoveRowAUOM
        '
        Me.btnRemoveRowAUOM.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRemoveRowAUOM.BackgroundImage = CType(resources.GetObject("btnRemoveRowAUOM.BackgroundImage"), System.Drawing.Image)
        Me.btnRemoveRowAUOM.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnRemoveRowAUOM.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRemoveRowAUOM.ForeColor = System.Drawing.Color.Red
        Me.btnRemoveRowAUOM.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRemoveRowAUOM.Location = New System.Drawing.Point(41, 161)
        Me.btnRemoveRowAUOM.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.btnRemoveRowAUOM.Name = "btnRemoveRowAUOM"
        Me.btnRemoveRowAUOM.Size = New System.Drawing.Size(29, 24)
        Me.btnRemoveRowAUOM.TabIndex = 588
        Me.btnRemoveRowAUOM.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnRemoveRowAUOM.UseVisualStyleBackColor = True
        '
        'btnDetVertical
        '
        Me.btnDetVertical.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDetVertical.BackColor = System.Drawing.Color.Transparent
        Me.btnDetVertical.BackgroundImage = Global.vLQMS.My.Resources.Resources.add
        Me.btnDetVertical.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnDetVertical.Font = New System.Drawing.Font("Microsoft Sans Serif", 13.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDetVertical.Location = New System.Drawing.Point(10, 161)
        Me.btnDetVertical.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.btnDetVertical.Name = "btnDetVertical"
        Me.btnDetVertical.Size = New System.Drawing.Size(26, 24)
        Me.btnDetVertical.TabIndex = 587
        Me.btnDetVertical.UseVisualStyleBackColor = False
        '
        'vsfDet
        '
        Me.vsfDet.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "1,0,0,0,0,125,Columns:"
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.vsfDet.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(3, 4)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 25
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(614, 153)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 22
        Me.vsfDet.Tag = ""
        '
        'pnlTestSMS
        '
        Me.pnlTestSMS.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlTestSMS.BackColor = System.Drawing.Color.MistyRose
        Me.pnlTestSMS.Controls.Add(Me.btnTestSMS)
        Me.pnlTestSMS.Controls.Add(Me.Label2)
        Me.pnlTestSMS.Controls.Add(Me.txtMobile)
        Me.pnlTestSMS.Location = New System.Drawing.Point(0, 158)
        Me.pnlTestSMS.Name = "pnlTestSMS"
        Me.pnlTestSMS.Size = New System.Drawing.Size(617, 39)
        Me.pnlTestSMS.TabIndex = 40
        Me.pnlTestSMS.Visible = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(7, 11)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(126, 20)
        Me.Label2.TabIndex = 40
        Me.Label2.Text = "Enter Mobile No."
        '
        'txtMobile
        '
        Me.txtMobile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMobile.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMobile.Location = New System.Drawing.Point(138, 7)
        Me.txtMobile.MaxLength = 10
        Me.txtMobile.Name = "txtMobile"
        Me.txtMobile.Size = New System.Drawing.Size(247, 26)
        Me.txtMobile.TabIndex = 32
        '
        'frmUtlSendSMS
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(629, 392)
        Me.Controls.Add(Me.pnlTestSMS)
        Me.Controls.Add(Me.pnlGrid)
        Me.Controls.Add(Me.fraEntry)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmUtlSendSMS"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SMS Settings"
        Me.fraEntry.ResumeLayout(False)
        Me.fraEntry.PerformLayout()
        Me.pnlGrid.ResumeLayout(False)
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlTestSMS.ResumeLayout(False)
        Me.pnlTestSMS.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents fraEntry As Panel
    Friend WithEvents txtDailyLimit As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents txtSMSURL As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents btnClose As Button
    Friend WithEvents btnSave As Button
    Friend WithEvents txtCode As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents chkDefault As CheckBox
    Friend WithEvents txtID As TextBox
    Friend WithEvents pnlGrid As Panel
    Friend WithEvents btnRemoveRowAUOM As Button
    Friend WithEvents btnDetVertical As Button
    Public WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents pnlTestSMS As Panel
    Friend WithEvents btnTestSMS As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents txtMobile As TextBox
    Friend WithEvents btnOpenHistory As Button
    Friend WithEvents btnInvSMS As Button
End Class
