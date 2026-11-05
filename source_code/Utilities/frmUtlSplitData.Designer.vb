<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlSplitData
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlSplitData))
        Me.btnUpdate = New C1.Win.C1Input.C1Button()
        Me.btnCancel = New C1.Win.C1Input.C1Button()
        Me.txtADB = New System.Windows.Forms.TextBox()
        Me.txtCDB = New System.Windows.Forms.TextBox()
        Me.lblContactNo = New System.Windows.Forms.Label()
        Me.lblPartyName = New System.Windows.Forms.Label()
        Me.txtLastInvDate = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtLastPBDate = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblTips = New System.Windows.Forms.Label()
        Me.txtDBPfx = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'btnUpdate
        '
        Me.btnUpdate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnUpdate.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnUpdate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnUpdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUpdate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnUpdate.Location = New System.Drawing.Point(676, 414)
        Me.btnUpdate.Margin = New System.Windows.Forms.Padding(5)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(120, 38)
        Me.btnUpdate.TabIndex = 9
        Me.btnUpdate.Text = "SPLIT"
        Me.btnUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleRight
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
        Me.btnCancel.Location = New System.Drawing.Point(806, 414)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(5)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(120, 38)
        Me.btnCancel.TabIndex = 10
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'txtADB
        '
        Me.txtADB.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtADB.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.txtADB.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtADB.Location = New System.Drawing.Point(389, 202)
        Me.txtADB.Margin = New System.Windows.Forms.Padding(4)
        Me.txtADB.MaxLength = 20
        Me.txtADB.Name = "txtADB"
        Me.txtADB.Size = New System.Drawing.Size(526, 30)
        Me.txtADB.TabIndex = 7
        '
        'txtCDB
        '
        Me.txtCDB.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCDB.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.txtCDB.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCDB.Location = New System.Drawing.Point(238, 13)
        Me.txtCDB.Margin = New System.Windows.Forms.Padding(4)
        Me.txtCDB.MaxLength = 50
        Me.txtCDB.Name = "txtCDB"
        Me.txtCDB.Size = New System.Drawing.Size(664, 30)
        Me.txtCDB.TabIndex = 5
        '
        'lblContactNo
        '
        Me.lblContactNo.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblContactNo.AutoSize = True
        Me.lblContactNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblContactNo.Location = New System.Drawing.Point(61, 202)
        Me.lblContactNo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblContactNo.Name = "lblContactNo"
        Me.lblContactNo.Size = New System.Drawing.Size(329, 25)
        Me.lblContactNo.TabIndex = 8
        Me.lblContactNo.Tag = "M"
        Me.lblContactNo.Text = "Current Database will be archived to "
        '
        'lblPartyName
        '
        Me.lblPartyName.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPartyName.AutoSize = True
        Me.lblPartyName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPartyName.Location = New System.Drawing.Point(49, 13)
        Me.lblPartyName.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblPartyName.Name = "lblPartyName"
        Me.lblPartyName.Size = New System.Drawing.Size(166, 25)
        Me.lblPartyName.TabIndex = 6
        Me.lblPartyName.Tag = "M"
        Me.lblPartyName.Text = "Current Database"
        '
        'txtLastInvDate
        '
        Me.txtLastInvDate.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLastInvDate.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.txtLastInvDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLastInvDate.Location = New System.Drawing.Point(429, 51)
        Me.txtLastInvDate.Margin = New System.Windows.Forms.Padding(4)
        Me.txtLastInvDate.MaxLength = 20
        Me.txtLastInvDate.Name = "txtLastInvDate"
        Me.txtLastInvDate.Size = New System.Drawing.Size(473, 30)
        Me.txtLastInvDate.TabIndex = 11
        '
        'Label1
        '
        Me.Label1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(244, 56)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(162, 25)
        Me.Label1.TabIndex = 12
        Me.Label1.Tag = "M"
        Me.Label1.Text = "Last Invoice Date"
        '
        'txtLastPBDate
        '
        Me.txtLastPBDate.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLastPBDate.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.txtLastPBDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLastPBDate.Location = New System.Drawing.Point(429, 89)
        Me.txtLastPBDate.Margin = New System.Windows.Forms.Padding(4)
        Me.txtLastPBDate.MaxLength = 20
        Me.txtLastPBDate.Name = "txtLastPBDate"
        Me.txtLastPBDate.Size = New System.Drawing.Size(473, 30)
        Me.txtLastPBDate.TabIndex = 13
        '
        'Label2
        '
        Me.Label2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(244, 94)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(183, 25)
        Me.Label2.TabIndex = 14
        Me.Label2.Tag = "M"
        Me.Label2.Text = "Last Purchase Date"
        '
        'lblTips
        '
        Me.lblTips.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTips.BackColor = System.Drawing.Color.FromArgb(CType(CType(250, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(187, Byte), Integer))
        Me.lblTips.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTips.Location = New System.Drawing.Point(13, 310)
        Me.lblTips.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTips.Name = "lblTips"
        Me.lblTips.Size = New System.Drawing.Size(645, 206)
        Me.lblTips.TabIndex = 15
        Me.lblTips.Tag = "M"
        Me.lblTips.Text = "Current Database will be archived to "
        '
        'txtDBPfx
        '
        Me.txtDBPfx.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDBPfx.Font = New System.Drawing.Font("Microsoft Sans Serif", 13.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDBPfx.Location = New System.Drawing.Point(389, 164)
        Me.txtDBPfx.Margin = New System.Windows.Forms.Padding(4)
        Me.txtDBPfx.MaxLength = 20
        Me.txtDBPfx.Name = "txtDBPfx"
        Me.txtDBPfx.Size = New System.Drawing.Size(168, 34)
        Me.txtDBPfx.TabIndex = 16
        '
        'Label3
        '
        Me.Label3.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(90, 169)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(291, 25)
        Me.Label3.TabIndex = 17
        Me.Label3.Tag = "M"
        Me.Label3.Text = "DB Suffix for Archived Database"
        '
        'frmUtlSplitData
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(963, 525)
        Me.Controls.Add(Me.txtDBPfx)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lblTips)
        Me.Controls.Add(Me.txtLastPBDate)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtLastInvDate)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.txtADB)
        Me.Controls.Add(Me.txtCDB)
        Me.Controls.Add(Me.lblContactNo)
        Me.Controls.Add(Me.lblPartyName)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmUtlSplitData"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Split Data"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnUpdate As C1.Win.C1Input.C1Button
    Friend WithEvents btnCancel As C1.Win.C1Input.C1Button
    Public WithEvents txtADB As TextBox
    Public WithEvents txtCDB As TextBox
    Friend WithEvents lblContactNo As Label
    Friend WithEvents lblPartyName As Label
    Public WithEvents txtLastInvDate As TextBox
    Friend WithEvents Label1 As Label
    Public WithEvents txtLastPBDate As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents lblTips As Label
    Public WithEvents txtDBPfx As TextBox
    Friend WithEvents Label3 As Label
End Class
