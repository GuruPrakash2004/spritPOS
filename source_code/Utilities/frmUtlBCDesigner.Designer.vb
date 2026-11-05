<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlBCDesigner
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlBCDesigner))
        Me.fraMain = New System.Windows.Forms.Panel()
        Me.txtDummy2 = New System.Windows.Forms.TextBox()
        Me.txtDummy1 = New System.Windows.Forms.TextBox()
        Me.btnOpenFile = New System.Windows.Forms.Button()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.txtBCLayout = New System.Windows.Forms.TextBox()
        Me.txtQuery = New System.Windows.Forms.TextBox()
        Me.txtBCName = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.fraCmdScr3 = New System.Windows.Forms.Panel()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnNewBC = New System.Windows.Forms.Button()
        Me.btnBCList = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.fdb = New System.Windows.Forms.OpenFileDialog()
        Me.txtDummy = New System.Windows.Forms.TextBox()
        Me.fraMain.SuspendLayout()
        Me.fraCmdScr3.SuspendLayout()
        Me.SuspendLayout()
        '
        'fraMain
        '
        Me.fraMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.fraMain.Controls.Add(Me.txtDummy2)
        Me.fraMain.Controls.Add(Me.txtDummy1)
        Me.fraMain.Controls.Add(Me.btnOpenFile)
        Me.fraMain.Controls.Add(Me.btnBrowse)
        Me.fraMain.Controls.Add(Me.txtBCLayout)
        Me.fraMain.Controls.Add(Me.txtQuery)
        Me.fraMain.Controls.Add(Me.txtBCName)
        Me.fraMain.Controls.Add(Me.Label3)
        Me.fraMain.Controls.Add(Me.Label2)
        Me.fraMain.Controls.Add(Me.Label1)
        Me.fraMain.Location = New System.Drawing.Point(10, 12)
        Me.fraMain.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.fraMain.MinimumSize = New System.Drawing.Size(620, 321)
        Me.fraMain.Name = "fraMain"
        Me.fraMain.Size = New System.Drawing.Size(620, 321)
        Me.fraMain.TabIndex = 0
        '
        'txtDummy2
        '
        Me.txtDummy2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtDummy2.Location = New System.Drawing.Point(290, 380)
        Me.txtDummy2.Name = "txtDummy2"
        Me.txtDummy2.Size = New System.Drawing.Size(38, 25)
        Me.txtDummy2.TabIndex = 224
        Me.txtDummy2.Visible = False
        '
        'txtDummy1
        '
        Me.txtDummy1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtDummy1.Location = New System.Drawing.Point(290, 380)
        Me.txtDummy1.Name = "txtDummy1"
        Me.txtDummy1.Size = New System.Drawing.Size(38, 25)
        Me.txtDummy1.TabIndex = 223
        Me.txtDummy1.Visible = False
        '
        'btnOpenFile
        '
        Me.btnOpenFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOpenFile.Image = CType(resources.GetObject("btnOpenFile.Image"), System.Drawing.Image)
        Me.btnOpenFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnOpenFile.Location = New System.Drawing.Point(509, 276)
        Me.btnOpenFile.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnOpenFile.Name = "btnOpenFile"
        Me.btnOpenFile.Size = New System.Drawing.Size(93, 29)
        Me.btnOpenFile.TabIndex = 7
        Me.btnOpenFile.Text = "Open File"
        Me.btnOpenFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnOpenFile.UseVisualStyleBackColor = False
        '
        'btnBrowse
        '
        Me.btnBrowse.Location = New System.Drawing.Point(462, 276)
        Me.btnBrowse.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(39, 29)
        Me.btnBrowse.TabIndex = 6
        Me.btnBrowse.Text = "..."
        Me.btnBrowse.UseVisualStyleBackColor = False
        '
        'txtBCLayout
        '
        Me.txtBCLayout.Location = New System.Drawing.Point(106, 278)
        Me.txtBCLayout.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtBCLayout.Name = "txtBCLayout"
        Me.txtBCLayout.Size = New System.Drawing.Size(350, 25)
        Me.txtBCLayout.TabIndex = 5
        '
        'txtQuery
        '
        Me.txtQuery.Location = New System.Drawing.Point(106, 52)
        Me.txtQuery.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtQuery.Multiline = True
        Me.txtQuery.Name = "txtQuery"
        Me.txtQuery.Size = New System.Drawing.Size(433, 199)
        Me.txtQuery.TabIndex = 4
        '
        'txtBCName
        '
        Me.txtBCName.Location = New System.Drawing.Point(106, 14)
        Me.txtBCName.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtBCName.Name = "txtBCName"
        Me.txtBCName.Size = New System.Drawing.Size(433, 25)
        Me.txtBCName.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.Color.Red
        Me.Label3.Location = New System.Drawing.Point(6, 282)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(91, 17)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Layout Format"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.Red
        Me.Label2.Location = New System.Drawing.Point(24, 55)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(43, 17)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Query"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.Red
        Me.Label1.Location = New System.Drawing.Point(6, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(97, 17)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "BC Label Name"
        '
        'fraCmdScr3
        '
        Me.fraCmdScr3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.fraCmdScr3.Controls.Add(Me.btnClose)
        Me.fraCmdScr3.Controls.Add(Me.btnCancel)
        Me.fraCmdScr3.Controls.Add(Me.btnSave)
        Me.fraCmdScr3.Controls.Add(Me.btnNewBC)
        Me.fraCmdScr3.Controls.Add(Me.btnBCList)
        Me.fraCmdScr3.Controls.Add(Me.btnDelete)
        Me.fraCmdScr3.Location = New System.Drawing.Point(10, 347)
        Me.fraCmdScr3.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.fraCmdScr3.Name = "fraCmdScr3"
        Me.fraCmdScr3.Size = New System.Drawing.Size(620, 48)
        Me.fraCmdScr3.TabIndex = 1
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(531, 9)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(71, 29)
        Me.btnClose.TabIndex = 5
        Me.btnClose.Text = "&Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(450, 9)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 29)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(377, 9)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(67, 29)
        Me.btnSave.TabIndex = 3
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnNewBC
        '
        Me.btnNewBC.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNewBC.Image = CType(resources.GetObject("btnNewBC.Image"), System.Drawing.Image)
        Me.btnNewBC.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnNewBC.Location = New System.Drawing.Point(256, 9)
        Me.btnNewBC.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnNewBC.Name = "btnNewBC"
        Me.btnNewBC.Size = New System.Drawing.Size(115, 29)
        Me.btnNewBC.TabIndex = 2
        Me.btnNewBC.Text = "&New Barcode Label"
        Me.btnNewBC.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnNewBC.UseVisualStyleBackColor = False
        '
        'btnBCList
        '
        Me.btnBCList.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBCList.Image = CType(resources.GetObject("btnBCList.Image"), System.Drawing.Image)
        Me.btnBCList.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnBCList.Location = New System.Drawing.Point(193, 9)
        Me.btnBCList.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnBCList.Name = "btnBCList"
        Me.btnBCList.Size = New System.Drawing.Size(57, 29)
        Me.btnBCList.TabIndex = 1
        Me.btnBCList.Text = "&List"
        Me.btnBCList.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnBCList.UseVisualStyleBackColor = False
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDelete.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_DELETE
        Me.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDelete.Image = CType(resources.GetObject("btnDelete.Image"), System.Drawing.Image)
        Me.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDelete.Location = New System.Drawing.Point(106, 9)
        Me.btnDelete.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(81, 29)
        Me.btnDelete.TabIndex = 0
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnDelete.UseVisualStyleBackColor = False
        '
        'fdb
        '
        Me.fdb.FileName = "OpenFileDialog1"
        '
        'txtDummy
        '
        Me.txtDummy.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtDummy.Location = New System.Drawing.Point(590, 393)
        Me.txtDummy.Name = "txtDummy"
        Me.txtDummy.Size = New System.Drawing.Size(38, 25)
        Me.txtDummy.TabIndex = 222
        Me.txtDummy.Visible = False
        '
        'frmUtlBCDesigner
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(640, 415)
        Me.Controls.Add(Me.txtDummy)
        Me.Controls.Add(Me.fraCmdScr3)
        Me.Controls.Add(Me.fraMain)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MinimumSize = New System.Drawing.Size(656, 446)
        Me.Name = "frmUtlBCDesigner"
        Me.ShowIcon = False
        Me.Text = "Barcode Label Designer"
        Me.fraMain.ResumeLayout(False)
        Me.fraMain.PerformLayout()
        Me.fraCmdScr3.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents fraMain As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtBCName As System.Windows.Forms.TextBox
    Friend WithEvents txtQuery As System.Windows.Forms.TextBox
    Friend WithEvents txtBCLayout As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents fraCmdScr3 As System.Windows.Forms.Panel
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnNewBC As System.Windows.Forms.Button
    Friend WithEvents btnBCList As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents fdb As System.Windows.Forms.OpenFileDialog
    Friend WithEvents btnOpenFile As System.Windows.Forms.Button
    Friend WithEvents txtDummy As TextBox
    Friend WithEvents txtDummy2 As TextBox
    Friend WithEvents txtDummy1 As TextBox
End Class
