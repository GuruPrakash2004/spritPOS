<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlAddParty
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
        Me.lblPartyName = New System.Windows.Forms.Label()
        Me.lblContactNo = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtPartyName = New System.Windows.Forms.TextBox()
        Me.txtContactNo = New System.Windows.Forms.TextBox()
        Me.txtRmks = New System.Windows.Forms.TextBox()
        Me.btnUpdate = New C1.Win.C1Input.C1Button()
        Me.btnCancel = New C1.Win.C1Input.C1Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.SuspendLayout()
        '
        'lblPartyName
        '
        Me.lblPartyName.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPartyName.AutoSize = True
        Me.lblPartyName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPartyName.Location = New System.Drawing.Point(12, 31)
        Me.lblPartyName.Name = "lblPartyName"
        Me.lblPartyName.Size = New System.Drawing.Size(91, 20)
        Me.lblPartyName.TabIndex = 0
        Me.lblPartyName.Tag = "M"
        Me.lblPartyName.Text = "Party Name"
        '
        'lblContactNo
        '
        Me.lblContactNo.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblContactNo.AutoSize = True
        Me.lblContactNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblContactNo.Location = New System.Drawing.Point(12, 81)
        Me.lblContactNo.Name = "lblContactNo"
        Me.lblContactNo.Size = New System.Drawing.Size(125, 20)
        Me.lblContactNo.TabIndex = 1
        Me.lblContactNo.Tag = "M"
        Me.lblContactNo.Text = "Contact Number"
        '
        'Label3
        '
        Me.Label3.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(12, 131)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(73, 20)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Remarks"
        '
        'txtPartyName
        '
        Me.txtPartyName.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPartyName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPartyName.Location = New System.Drawing.Point(179, 28)
        Me.txtPartyName.MaxLength = 50
        Me.txtPartyName.Name = "txtPartyName"
        Me.txtPartyName.Size = New System.Drawing.Size(377, 26)
        Me.txtPartyName.TabIndex = 0
        '
        'txtContactNo
        '
        Me.txtContactNo.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtContactNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtContactNo.Location = New System.Drawing.Point(179, 78)
        Me.txtContactNo.MaxLength = 20
        Me.txtContactNo.Name = "txtContactNo"
        Me.txtContactNo.Size = New System.Drawing.Size(377, 26)
        Me.txtContactNo.TabIndex = 1
        '
        'txtRmks
        '
        Me.txtRmks.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtRmks.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRmks.Location = New System.Drawing.Point(179, 128)
        Me.txtRmks.MaxLength = 250
        Me.txtRmks.Multiline = True
        Me.txtRmks.Name = "txtRmks"
        Me.txtRmks.Size = New System.Drawing.Size(377, 108)
        Me.txtRmks.TabIndex = 2
        '
        'btnUpdate
        '
        Me.btnUpdate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnUpdate.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnUpdate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnUpdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUpdate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnUpdate.Location = New System.Drawing.Point(368, 243)
        Me.btnUpdate.Margin = New System.Windows.Forms.Padding(4)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(90, 31)
        Me.btnUpdate.TabIndex = 3
        Me.btnUpdate.Text = "&Update"
        Me.btnUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnUpdate, "Update : Alt + U")
        Me.btnUpdate.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(466, 243)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(90, 31)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Cancel : Alt + C")
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'frmUtlAddParty
        '
        Me.AcceptButton = Me.btnUpdate
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(568, 287)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.txtRmks)
        Me.Controls.Add(Me.txtContactNo)
        Me.Controls.Add(Me.txtPartyName)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lblContactNo)
        Me.Controls.Add(Me.lblPartyName)
        Me.Name = "frmUtlAddParty"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Add Party"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblPartyName As Label
    Friend WithEvents lblContactNo As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents txtRmks As TextBox
    Friend WithEvents btnUpdate As C1.Win.C1Input.C1Button
    Friend WithEvents btnCancel As C1.Win.C1Input.C1Button
    Public WithEvents txtPartyName As TextBox
    Public WithEvents txtContactNo As TextBox
    Friend WithEvents ToolTip1 As ToolTip
End Class
