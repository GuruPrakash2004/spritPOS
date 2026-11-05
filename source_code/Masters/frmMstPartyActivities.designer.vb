<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMstPartyActivities
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMstPartyActivities))
        Me.fraEdit = New System.Windows.Forms.Panel()
        Me.DtpDate = New System.Windows.Forms.DateTimePicker()
        Me.lblDate = New System.Windows.Forms.Label()
        Me.btnSave = New C1.Win.C1Input.C1Button()
        Me.btnCancel = New C1.Win.C1Input.C1Button()
        Me.lblNotes = New System.Windows.Forms.Label()
        Me.txtNotes = New System.Windows.Forms.TextBox()
        Me.txtActID = New System.Windows.Forms.TextBox()
        Me.txtPartyID = New System.Windows.Forms.TextBox()
        Me.txtPartyCode = New System.Windows.Forms.TextBox()
        Me.lblPartyCode = New System.Windows.Forms.Label()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnAdd = New C1.Win.C1Input.C1Button()
        Me.lblPartyName = New System.Windows.Forms.Label()
        Me.lblNoofAddress = New System.Windows.Forms.Label()
        Me.txtDocId = New System.Windows.Forms.TextBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnRemove = New C1.Win.C1Input.C1Button()
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.btnList = New System.Windows.Forms.Button()
        Me.fraEdit.SuspendLayout()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'fraEdit
        '
        Me.fraEdit.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fraEdit.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.fraEdit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.fraEdit.Controls.Add(Me.DtpDate)
        Me.fraEdit.Controls.Add(Me.lblDate)
        Me.fraEdit.Controls.Add(Me.btnSave)
        Me.fraEdit.Controls.Add(Me.btnCancel)
        Me.fraEdit.Controls.Add(Me.lblNotes)
        Me.fraEdit.Controls.Add(Me.txtNotes)
        Me.fraEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraEdit.Location = New System.Drawing.Point(12, 69)
        Me.fraEdit.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.fraEdit.Name = "fraEdit"
        Me.fraEdit.Size = New System.Drawing.Size(718, 307)
        Me.fraEdit.TabIndex = 0
        '
        'DtpDate
        '
        Me.DtpDate.CustomFormat = "dd/MM/yyyy"
        Me.DtpDate.Enabled = False
        Me.DtpDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DtpDate.Location = New System.Drawing.Point(139, 15)
        Me.DtpDate.Name = "DtpDate"
        Me.DtpDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.DtpDate.Size = New System.Drawing.Size(118, 26)
        Me.DtpDate.TabIndex = 266
        '
        'lblDate
        '
        Me.lblDate.AutoSize = True
        Me.lblDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDate.ForeColor = System.Drawing.Color.Blue
        Me.lblDate.Location = New System.Drawing.Point(5, 20)
        Me.lblDate.Name = "lblDate"
        Me.lblDate.Size = New System.Drawing.Size(44, 20)
        Me.lblDate.TabIndex = 207
        Me.lblDate.Text = "Date"
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(524, 271)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(90, 30)
        Me.btnSave.TabIndex = 8
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Shortcut : Alt + S")
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(622, 271)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(90, 30)
        Me.btnCancel.TabIndex = 9
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Shortcut : Alt + C")
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'lblNotes
        '
        Me.lblNotes.AutoSize = True
        Me.lblNotes.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNotes.ForeColor = System.Drawing.Color.Red
        Me.lblNotes.Location = New System.Drawing.Point(5, 56)
        Me.lblNotes.Name = "lblNotes"
        Me.lblNotes.Size = New System.Drawing.Size(51, 20)
        Me.lblNotes.TabIndex = 41
        Me.lblNotes.Text = "Notes"
        '
        'txtNotes
        '
        Me.txtNotes.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtNotes.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNotes.Location = New System.Drawing.Point(139, 56)
        Me.txtNotes.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtNotes.MaxLength = 250
        Me.txtNotes.Multiline = True
        Me.txtNotes.Name = "txtNotes"
        Me.txtNotes.Size = New System.Drawing.Size(382, 164)
        Me.txtNotes.TabIndex = 7
        '
        'txtActID
        '
        Me.txtActID.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtActID.Location = New System.Drawing.Point(299, 119)
        Me.txtActID.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtActID.Name = "txtActID"
        Me.txtActID.Size = New System.Drawing.Size(25, 25)
        Me.txtActID.TabIndex = 38
        Me.txtActID.Visible = False
        '
        'txtPartyID
        '
        Me.txtPartyID.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtPartyID.Location = New System.Drawing.Point(443, 12)
        Me.txtPartyID.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtPartyID.Name = "txtPartyID"
        Me.txtPartyID.Size = New System.Drawing.Size(25, 25)
        Me.txtPartyID.TabIndex = 35
        Me.txtPartyID.Visible = False
        '
        'txtPartyCode
        '
        Me.txtPartyCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPartyCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPartyCode.Location = New System.Drawing.Point(151, 9)
        Me.txtPartyCode.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtPartyCode.MaxLength = 150
        Me.txtPartyCode.Name = "txtPartyCode"
        Me.txtPartyCode.Size = New System.Drawing.Size(331, 26)
        Me.txtPartyCode.TabIndex = 31
        '
        'lblPartyCode
        '
        Me.lblPartyCode.AutoSize = True
        Me.lblPartyCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPartyCode.ForeColor = System.Drawing.Color.Blue
        Me.lblPartyCode.Location = New System.Drawing.Point(17, 13)
        Me.lblPartyCode.Name = "lblPartyCode"
        Me.lblPartyCode.Size = New System.Drawing.Size(120, 20)
        Me.lblPartyCode.TabIndex = 30
        Me.lblPartyCode.Tag = "O"
        Me.lblPartyCode.Text = "Customer Code"
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
        Me.vsfDet.AutoResize = True
        Me.vsfDet.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfDet.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfDet.Location = New System.Drawing.Point(20, 85)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.Rows.GlyphRow = 0
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(710, 248)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 36
        Me.vsfDet.Tag = ""
        '
        'btnAdd
        '
        Me.btnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAdd.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_NEW
        Me.btnAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAdd.Location = New System.Drawing.Point(443, 341)
        Me.btnAdd.Margin = New System.Windows.Forms.Padding(4)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnAdd.Size = New System.Drawing.Size(90, 30)
        Me.btnAdd.TabIndex = 39
        Me.btnAdd.Text = "&Add"
        Me.btnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnAdd, "Shortcut : Alt + A")
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'lblPartyName
        '
        Me.lblPartyName.AutoSize = True
        Me.lblPartyName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPartyName.ForeColor = System.Drawing.Color.Black
        Me.lblPartyName.Location = New System.Drawing.Point(147, 42)
        Me.lblPartyName.Name = "lblPartyName"
        Me.lblPartyName.Size = New System.Drawing.Size(133, 20)
        Me.lblPartyName.TabIndex = 43
        Me.lblPartyName.Text = "Customer Code"
        '
        'lblNoofAddress
        '
        Me.lblNoofAddress.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblNoofAddress.AutoSize = True
        Me.lblNoofAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNoofAddress.ForeColor = System.Drawing.Color.Blue
        Me.lblNoofAddress.Location = New System.Drawing.Point(17, 341)
        Me.lblNoofAddress.Name = "lblNoofAddress"
        Me.lblNoofAddress.Size = New System.Drawing.Size(124, 20)
        Me.lblNoofAddress.TabIndex = 209
        Me.lblNoofAddress.Text = "Customer Name"
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
        'btnRemove
        '
        Me.btnRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRemove.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_DELETE
        Me.btnRemove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRemove.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRemove.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRemove.Location = New System.Drawing.Point(541, 341)
        Me.btnRemove.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(90, 30)
        Me.btnRemove.TabIndex = 42
        Me.btnRemove.Text = "&Remove"
        Me.btnRemove.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnRemove, "Shortcut : Alt + R")
        Me.btnRemove.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Image = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.Location = New System.Drawing.Point(639, 341)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(90, 30)
        Me.btnClose.TabIndex = 211
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Shortcut : Alt + O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnList
        '
        Me.btnList.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnList.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_FIND
        Me.btnList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnList.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnList.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnList.Location = New System.Drawing.Point(397, 341)
        Me.btnList.Name = "btnList"
        Me.btnList.Size = New System.Drawing.Size(36, 29)
        Me.btnList.TabIndex = 219
        Me.ToolTip1.SetToolTip(Me.btnList, "View All Activities")
        Me.btnList.UseVisualStyleBackColor = True
        '
        'frmMstPartyActivities
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(742, 389)
        Me.Controls.Add(Me.fraEdit)
        Me.Controls.Add(Me.txtDocId)
        Me.Controls.Add(Me.lblPartyName)
        Me.Controls.Add(Me.txtPartyCode)
        Me.Controls.Add(Me.txtPartyID)
        Me.Controls.Add(Me.lblPartyCode)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.txtActID)
        Me.Controls.Add(Me.btnRemove)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.lblNoofAddress)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnList)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "frmMstPartyActivities"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Contact Person(s)"
        Me.fraEdit.ResumeLayout(False)
        Me.fraEdit.PerformLayout()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents fraEdit As System.Windows.Forms.Panel
    Friend WithEvents txtActID As System.Windows.Forms.TextBox
    Friend WithEvents txtNotes As System.Windows.Forms.TextBox
    Friend WithEvents txtPartyID As System.Windows.Forms.TextBox
    Friend WithEvents txtPartyCode As System.Windows.Forms.TextBox
    Friend WithEvents lblPartyCode As System.Windows.Forms.Label
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents lblNotes As System.Windows.Forms.Label
    Friend WithEvents btnSave As C1.Win.C1Input.C1Button
    Friend WithEvents btnCancel As C1.Win.C1Input.C1Button
    Friend WithEvents btnAdd As C1.Win.C1Input.C1Button
    Friend WithEvents lblDate As System.Windows.Forms.Label
    Friend WithEvents lblPartyName As System.Windows.Forms.Label
    Friend WithEvents lblNoofAddress As System.Windows.Forms.Label
    Friend WithEvents txtDocId As System.Windows.Forms.TextBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents btnRemove As C1.Win.C1Input.C1Button
    Friend WithEvents DtpDate As DateTimePicker
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents btnList As Button
End Class
