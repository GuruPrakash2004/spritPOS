<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMstPartyAlert
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMstPartyAlert))
        Me.btnSave = New C1.Win.C1Input.C1Button()
        Me.txtAID = New System.Windows.Forms.TextBox()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.txtDocId = New System.Windows.Forms.TextBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnOk = New C1.Win.C1Input.C1Button()
        Me.btnMsgCancel = New C1.Win.C1Input.C1Button()
        Me.btnList = New System.Windows.Forms.Button()
        Me.lblPartyName = New System.Windows.Forms.Label()
        Me.txtPartyCode = New System.Windows.Forms.TextBox()
        Me.lblPartyCode = New System.Windows.Forms.Label()
        Me.txtPartyId = New System.Windows.Forms.TextBox()
        Me.pnlMessage = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtAlertMsg = New System.Windows.Forms.TextBox()
        Me.txtTrnName = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.pnlBtns = New System.Windows.Forms.Panel()
        Me.lblNoofMessages = New System.Windows.Forms.Label()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlMessage.SuspendLayout()
        Me.pnlBtns.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Image = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.Location = New System.Drawing.Point(2, 2)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(90, 30)
        Me.btnSave.TabIndex = 8
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Shortcut : Alt + S")
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'txtAID
        '
        Me.txtAID.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtAID.Location = New System.Drawing.Point(299, 119)
        Me.txtAID.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtAID.Name = "txtAID"
        Me.txtAID.Size = New System.Drawing.Size(25, 25)
        Me.txtAID.TabIndex = 38
        Me.txtAID.Visible = False
        '
        'vsfDet
        '
        Me.vsfDet.AllowFiltering = True
        Me.vsfDet.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfDet.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfDet.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.AutoClipboard = True
        Me.vsfDet.AutoResize = True
        Me.vsfDet.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfDet.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfDet.Location = New System.Drawing.Point(19, 79)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.Rows.GlyphRow = 0
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(807, 303)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 36
        Me.vsfDet.Tag = ""
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Image = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.Location = New System.Drawing.Point(95, 2)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(90, 30)
        Me.btnClose.TabIndex = 42
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Shortcut : Alt + O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'txtDocId
        '
        Me.txtDocId.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDocId.Location = New System.Drawing.Point(569, 36)
        Me.txtDocId.Name = "txtDocId"
        Me.txtDocId.Size = New System.Drawing.Size(32, 26)
        Me.txtDocId.TabIndex = 210
        Me.txtDocId.Visible = False
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnOk.Location = New System.Drawing.Point(281, 237)
        Me.btnOk.Margin = New System.Windows.Forms.Padding(4)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(90, 30)
        Me.btnOk.TabIndex = 216
        Me.btnOk.Text = "O&K"
        Me.ToolTip1.SetToolTip(Me.btnOk, "Shortcut : Alt + K")
        Me.btnOk.UseVisualStyleBackColor = False
        '
        'btnMsgCancel
        '
        Me.btnMsgCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMsgCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMsgCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMsgCancel.Image = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnMsgCancel.Location = New System.Drawing.Point(376, 237)
        Me.btnMsgCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.btnMsgCancel.Name = "btnMsgCancel"
        Me.btnMsgCancel.Size = New System.Drawing.Size(90, 30)
        Me.btnMsgCancel.TabIndex = 217
        Me.btnMsgCancel.Text = "&Cancel"
        Me.btnMsgCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnMsgCancel, "Shortcut : Alt + C")
        Me.btnMsgCancel.UseVisualStyleBackColor = False
        '
        'btnList
        '
        Me.btnList.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnList.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_FIND
        Me.btnList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnList.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnList.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnList.Location = New System.Drawing.Point(604, 392)
        Me.btnList.Name = "btnList"
        Me.btnList.Size = New System.Drawing.Size(36, 29)
        Me.btnList.TabIndex = 218
        Me.ToolTip1.SetToolTip(Me.btnList, "View All Alert Messages")
        Me.btnList.UseVisualStyleBackColor = True
        '
        'lblPartyName
        '
        Me.lblPartyName.AutoSize = True
        Me.lblPartyName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPartyName.ForeColor = System.Drawing.Color.Black
        Me.lblPartyName.Location = New System.Drawing.Point(168, 45)
        Me.lblPartyName.Name = "lblPartyName"
        Me.lblPartyName.Size = New System.Drawing.Size(133, 20)
        Me.lblPartyName.TabIndex = 213
        Me.lblPartyName.Text = "Customer Code"
        '
        'txtPartyCode
        '
        Me.txtPartyCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPartyCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPartyCode.Location = New System.Drawing.Point(171, 13)
        Me.txtPartyCode.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtPartyCode.MaxLength = 150
        Me.txtPartyCode.Name = "txtPartyCode"
        Me.txtPartyCode.ReadOnly = True
        Me.txtPartyCode.Size = New System.Drawing.Size(331, 26)
        Me.txtPartyCode.TabIndex = 212
        '
        'lblPartyCode
        '
        Me.lblPartyCode.AutoSize = True
        Me.lblPartyCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPartyCode.ForeColor = System.Drawing.Color.Blue
        Me.lblPartyCode.Location = New System.Drawing.Point(29, 15)
        Me.lblPartyCode.Name = "lblPartyCode"
        Me.lblPartyCode.Size = New System.Drawing.Size(109, 20)
        Me.lblPartyCode.TabIndex = 211
        Me.lblPartyCode.Tag = "O"
        Me.lblPartyCode.Text = "Supplier Code"
        '
        'txtPartyId
        '
        Me.txtPartyId.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPartyId.Location = New System.Drawing.Point(531, 15)
        Me.txtPartyId.Name = "txtPartyId"
        Me.txtPartyId.Size = New System.Drawing.Size(32, 26)
        Me.txtPartyId.TabIndex = 214
        Me.txtPartyId.Visible = False
        '
        'pnlMessage
        '
        Me.pnlMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlMessage.Controls.Add(Me.btnMsgCancel)
        Me.pnlMessage.Controls.Add(Me.btnOk)
        Me.pnlMessage.Controls.Add(Me.Label2)
        Me.pnlMessage.Controls.Add(Me.txtAlertMsg)
        Me.pnlMessage.Controls.Add(Me.txtTrnName)
        Me.pnlMessage.Controls.Add(Me.Label1)
        Me.pnlMessage.Location = New System.Drawing.Point(276, 88)
        Me.pnlMessage.Name = "pnlMessage"
        Me.pnlMessage.Size = New System.Drawing.Size(483, 274)
        Me.pnlMessage.TabIndex = 215
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(13, 34)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(111, 20)
        Me.Label2.TabIndex = 215
        Me.Label2.Text = "Alert Message"
        '
        'txtAlertMsg
        '
        Me.txtAlertMsg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAlertMsg.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAlertMsg.Location = New System.Drawing.Point(13, 58)
        Me.txtAlertMsg.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtAlertMsg.MaxLength = 250
        Me.txtAlertMsg.Multiline = True
        Me.txtAlertMsg.Name = "txtAlertMsg"
        Me.txtAlertMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtAlertMsg.Size = New System.Drawing.Size(461, 171)
        Me.txtAlertMsg.TabIndex = 214
        '
        'txtTrnName
        '
        Me.txtTrnName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTrnName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTrnName.Location = New System.Drawing.Point(111, 8)
        Me.txtTrnName.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtTrnName.MaxLength = 150
        Me.txtTrnName.Name = "txtTrnName"
        Me.txtTrnName.ReadOnly = True
        Me.txtTrnName.Size = New System.Drawing.Size(196, 26)
        Me.txtTrnName.TabIndex = 213
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(13, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(92, 20)
        Me.Label1.TabIndex = 212
        Me.Label1.Tag = "O"
        Me.Label1.Text = "Transaction"
        '
        'pnlBtns
        '
        Me.pnlBtns.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlBtns.Controls.Add(Me.btnSave)
        Me.pnlBtns.Controls.Add(Me.btnClose)
        Me.pnlBtns.Location = New System.Drawing.Point(646, 389)
        Me.pnlBtns.Name = "pnlBtns"
        Me.pnlBtns.Size = New System.Drawing.Size(192, 36)
        Me.pnlBtns.TabIndex = 216
        '
        'lblNoofMessages
        '
        Me.lblNoofMessages.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblNoofMessages.AutoSize = True
        Me.lblNoofMessages.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNoofMessages.ForeColor = System.Drawing.Color.Blue
        Me.lblNoofMessages.Location = New System.Drawing.Point(16, 389)
        Me.lblNoofMessages.Name = "lblNoofMessages"
        Me.lblNoofMessages.Size = New System.Drawing.Size(124, 20)
        Me.lblNoofMessages.TabIndex = 217
        Me.lblNoofMessages.Text = "Customer Name"
        '
        'frmMstPartyAlert
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(838, 429)
        Me.Controls.Add(Me.btnList)
        Me.Controls.Add(Me.lblNoofMessages)
        Me.Controls.Add(Me.pnlBtns)
        Me.Controls.Add(Me.pnlMessage)
        Me.Controls.Add(Me.txtPartyId)
        Me.Controls.Add(Me.lblPartyName)
        Me.Controls.Add(Me.txtPartyCode)
        Me.Controls.Add(Me.lblPartyCode)
        Me.Controls.Add(Me.txtDocId)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.txtAID)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MinimumSize = New System.Drawing.Size(854, 467)
        Me.Name = "frmMstPartyAlert"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Contact Person(s)"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlMessage.ResumeLayout(False)
        Me.pnlMessage.PerformLayout()
        Me.pnlBtns.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtAID As System.Windows.Forms.TextBox
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnSave As C1.Win.C1Input.C1Button
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents txtDocId As System.Windows.Forms.TextBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents lblPartyName As Label
    Friend WithEvents txtPartyCode As TextBox
    Friend WithEvents lblPartyCode As Label
    Friend WithEvents txtPartyId As TextBox
    Friend WithEvents pnlMessage As Panel
    Friend WithEvents btnOk As C1.Win.C1Input.C1Button
    Friend WithEvents Label2 As Label
    Friend WithEvents txtAlertMsg As TextBox
    Friend WithEvents txtTrnName As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents btnMsgCancel As C1.Win.C1Input.C1Button
    Friend WithEvents pnlBtns As Panel
    Friend WithEvents lblNoofMessages As Label
    Friend WithEvents btnList As Button
End Class
