<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmUtlTrnLblPrint
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmUtlTrnLblPrint))
        Me.fraCmdScr3 = New System.Windows.Forms.Panel()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnNewPrint = New System.Windows.Forms.Button()
        Me.btnListPrint = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.fraMain = New System.Windows.Forms.Panel()
        Me.lblTable = New System.Windows.Forms.Label()
        Me.cboTable = New System.Windows.Forms.ComboBox()
        Me.btnOpenFile = New System.Windows.Forms.Button()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.txtPrintLayout = New System.Windows.Forms.TextBox()
        Me.txtQuery = New System.Windows.Forms.TextBox()
        Me.txtPrintLblName = New System.Windows.Forms.TextBox()
        Me.lblLayout = New System.Windows.Forms.Label()
        Me.lblQuery = New System.Windows.Forms.Label()
        Me.lblPrintName = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.fdb = New System.Windows.Forms.OpenFileDialog()
        Me.txtID = New System.Windows.Forms.TextBox()
        Me.fraCmdScr3.SuspendLayout()
        Me.fraMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'fraCmdScr3
        '
        Me.fraCmdScr3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.fraCmdScr3.Controls.Add(Me.btnClose)
        Me.fraCmdScr3.Controls.Add(Me.btnCancel)
        Me.fraCmdScr3.Controls.Add(Me.btnSave)
        Me.fraCmdScr3.Controls.Add(Me.btnNewPrint)
        Me.fraCmdScr3.Controls.Add(Me.btnListPrint)
        Me.fraCmdScr3.Controls.Add(Me.btnDelete)
        Me.fraCmdScr3.Location = New System.Drawing.Point(10, 347)
        Me.fraCmdScr3.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.fraCmdScr3.Name = "fraCmdScr3"
        Me.fraCmdScr3.Size = New System.Drawing.Size(620, 48)
        Me.fraCmdScr3.TabIndex = 2
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vICAxon.My.Resources.Resources.T3_CLOSE
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(531, 9)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(71, 29)
        Me.btnClose.TabIndex = 5
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close : Alt + O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackgroundImage = Global.vICAxon.My.Resources.Resources.T3_CANCEL
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(450, 9)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 29)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Cancel : Alt + C")
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImage = Global.vICAxon.My.Resources.Resources.T3_SAVE
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(377, 9)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(67, 29)
        Me.btnSave.TabIndex = 3
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Save : Alt + S")
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnNewPrint
        '
        Me.btnNewPrint.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNewPrint.Image = CType(resources.GetObject("btnNewPrint.Image"), System.Drawing.Image)
        Me.btnNewPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnNewPrint.Location = New System.Drawing.Point(256, 9)
        Me.btnNewPrint.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnNewPrint.Name = "btnNewPrint"
        Me.btnNewPrint.Size = New System.Drawing.Size(115, 29)
        Me.btnNewPrint.TabIndex = 2
        Me.btnNewPrint.Text = "&New Label Print"
        Me.btnNewPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnNewPrint, "New Label Print : Alt + N")
        Me.btnNewPrint.UseVisualStyleBackColor = False
        '
        'btnListPrint
        '
        Me.btnListPrint.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnListPrint.Image = CType(resources.GetObject("btnListPrint.Image"), System.Drawing.Image)
        Me.btnListPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnListPrint.Location = New System.Drawing.Point(193, 9)
        Me.btnListPrint.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnListPrint.Name = "btnListPrint"
        Me.btnListPrint.Size = New System.Drawing.Size(57, 29)
        Me.btnListPrint.TabIndex = 1
        Me.btnListPrint.Text = "&List"
        Me.btnListPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnListPrint, "List : Alt + L")
        Me.btnListPrint.UseVisualStyleBackColor = False
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDelete.BackgroundImage = Global.vICAxon.My.Resources.Resources.T3_DELETE
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
        Me.ToolTip1.SetToolTip(Me.btnDelete, "Delete : Alt + D")
        Me.btnDelete.UseVisualStyleBackColor = False
        '
        'fraMain
        '
        Me.fraMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.fraMain.Controls.Add(Me.lblTable)
        Me.fraMain.Controls.Add(Me.cboTable)
        Me.fraMain.Controls.Add(Me.btnOpenFile)
        Me.fraMain.Controls.Add(Me.btnBrowse)
        Me.fraMain.Controls.Add(Me.txtPrintLayout)
        Me.fraMain.Controls.Add(Me.txtQuery)
        Me.fraMain.Controls.Add(Me.txtPrintLblName)
        Me.fraMain.Controls.Add(Me.lblLayout)
        Me.fraMain.Controls.Add(Me.lblQuery)
        Me.fraMain.Controls.Add(Me.lblPrintName)
        Me.fraMain.Location = New System.Drawing.Point(10, 13)
        Me.fraMain.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.fraMain.MinimumSize = New System.Drawing.Size(620, 321)
        Me.fraMain.Name = "fraMain"
        Me.fraMain.Size = New System.Drawing.Size(620, 321)
        Me.fraMain.TabIndex = 3
        '
        'lblTable
        '
        Me.lblTable.AutoSize = True
        Me.lblTable.ForeColor = System.Drawing.Color.Red
        Me.lblTable.Location = New System.Drawing.Point(6, 12)
        Me.lblTable.Name = "lblTable"
        Me.lblTable.Size = New System.Drawing.Size(65, 13)
        Me.lblTable.TabIndex = 9
        Me.lblTable.Text = "Table Name"
        '
        'cboTable
        '
        Me.cboTable.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTable.FormattingEnabled = True
        Me.cboTable.Location = New System.Drawing.Point(106, 7)
        Me.cboTable.Name = "cboTable"
        Me.cboTable.Size = New System.Drawing.Size(265, 23)
        Me.cboTable.TabIndex = 8
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
        'txtPrintLayout
        '
        Me.txtPrintLayout.Location = New System.Drawing.Point(106, 278)
        Me.txtPrintLayout.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtPrintLayout.MaxLength = 50
        Me.txtPrintLayout.Name = "txtPrintLayout"
        Me.txtPrintLayout.Size = New System.Drawing.Size(350, 20)
        Me.txtPrintLayout.TabIndex = 5
        '
        'txtQuery
        '
        Me.txtQuery.Location = New System.Drawing.Point(106, 82)
        Me.txtQuery.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtQuery.MaxLength = 1000
        Me.txtQuery.Multiline = True
        Me.txtQuery.Name = "txtQuery"
        Me.txtQuery.Size = New System.Drawing.Size(496, 172)
        Me.txtQuery.TabIndex = 4
        '
        'txtPrintLblName
        '
        Me.txtPrintLblName.Location = New System.Drawing.Point(106, 44)
        Me.txtPrintLblName.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtPrintLblName.MaxLength = 50
        Me.txtPrintLblName.Name = "txtPrintLblName"
        Me.txtPrintLblName.Size = New System.Drawing.Size(496, 20)
        Me.txtPrintLblName.TabIndex = 3
        '
        'lblLayout
        '
        Me.lblLayout.AutoSize = True
        Me.lblLayout.ForeColor = System.Drawing.Color.Red
        Me.lblLayout.Location = New System.Drawing.Point(6, 282)
        Me.lblLayout.Name = "lblLayout"
        Me.lblLayout.Size = New System.Drawing.Size(74, 13)
        Me.lblLayout.TabIndex = 2
        Me.lblLayout.Text = "Layout Format"
        '
        'lblQuery
        '
        Me.lblQuery.AutoSize = True
        Me.lblQuery.ForeColor = System.Drawing.Color.Red
        Me.lblQuery.Location = New System.Drawing.Point(6, 87)
        Me.lblQuery.Name = "lblQuery"
        Me.lblQuery.Size = New System.Drawing.Size(35, 13)
        Me.lblQuery.TabIndex = 1
        Me.lblQuery.Text = "Query"
        '
        'lblPrintName
        '
        Me.lblPrintName.AutoSize = True
        Me.lblPrintName.ForeColor = System.Drawing.Color.Red
        Me.lblPrintName.Location = New System.Drawing.Point(6, 48)
        Me.lblPrintName.Name = "lblPrintName"
        Me.lblPrintName.Size = New System.Drawing.Size(88, 13)
        Me.lblPrintName.TabIndex = 0
        Me.lblPrintName.Text = "Print Label Name"
        '
        'fdb
        '
        Me.fdb.FileName = "OpenFileDialog1"
        '
        'txtID
        '
        Me.txtID.Location = New System.Drawing.Point(1, 387)
        Me.txtID.Name = "txtID"
        Me.txtID.Size = New System.Drawing.Size(35, 20)
        Me.txtID.TabIndex = 4
        Me.txtID.Visible = False
        '
        'FrmUtlTrnLblPrint
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(640, 408)
        Me.Controls.Add(Me.fraMain)
        Me.Controls.Add(Me.fraCmdScr3)
        Me.Controls.Add(Me.txtID)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "FrmUtlTrnLblPrint"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Transaction Label Print"
        Me.fraCmdScr3.ResumeLayout(False)
        Me.fraMain.ResumeLayout(False)
        Me.fraMain.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents fraCmdScr3 As Panel
    Friend WithEvents btnClose As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnSave As Button
    Friend WithEvents btnNewPrint As Button
    Friend WithEvents btnListPrint As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents fraMain As Panel
    Friend WithEvents btnOpenFile As Button
    Friend WithEvents btnBrowse As Button
    Friend WithEvents txtPrintLayout As TextBox
    Friend WithEvents txtQuery As TextBox
    Friend WithEvents txtPrintLblName As TextBox
    Friend WithEvents lblLayout As Label
    Friend WithEvents lblQuery As Label
    Friend WithEvents lblPrintName As Label
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents fdb As OpenFileDialog
    Friend WithEvents lblTable As Label
    Friend WithEvents cboTable As ComboBox
    Friend WithEvents txtID As TextBox
End Class
