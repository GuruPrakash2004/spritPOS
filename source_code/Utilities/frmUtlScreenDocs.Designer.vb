<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlScreenDocs
    Inherits System.Windows.Forms.Form
    'Inherits MetroFramework.Forms.MetroForm

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlScreenDocs))
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.pnlAddScreenDocs = New System.Windows.Forms.Panel()
        Me.txtSlNo = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btnDelete = New C1.Win.C1Input.C1Button()
        Me.txtGroupName = New System.Windows.Forms.TextBox()
        Me.txtDocID = New System.Windows.Forms.TextBox()
        Me.btnSave = New C1.Win.C1Input.C1Button()
        Me.btnCancel = New C1.Win.C1Input.C1Button()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.txtRemarks = New System.Windows.Forms.TextBox()
        Me.txtDoc = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.fbd = New System.Windows.Forms.FolderBrowserDialog()
        Me.fsd = New System.Windows.Forms.OpenFileDialog()
        Me.lblfsize = New System.Windows.Forms.Label()
        Me.lblfname = New System.Windows.Forms.Label()
        Me.lblmods = New System.Windows.Forms.Label()
        Me.Pnlfilesproperty = New System.Windows.Forms.Panel()
        Me.lblmod = New System.Windows.Forms.Label()
        Me.lblFileSize = New System.Windows.Forms.Label()
        Me.lblformcaption = New System.Windows.Forms.Label()
        Me.txtfrmCaption = New System.Windows.Forms.TextBox()
        Me.btnRemove = New C1.Win.C1Input.C1Button()
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.btnAdd = New C1.Win.C1Input.C1Button()
        Me.btnEdit = New C1.Win.C1Input.C1Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlAddScreenDocs.SuspendLayout()
        Me.Pnlfilesproperty.SuspendLayout()
        Me.SuspendLayout()
        '
        'vsfDet
        '
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "10,0,0,0,0,95,Columns:0{AllowMerging:True;Style:""TextAlign:LeftCenter;"";StyleFixe" &
    "d:""TextAlign:LeftCenter;"";}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfDet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.vsfDet.Location = New System.Drawing.Point(23, 37)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 19
        Me.vsfDet.Rows.GlyphRow = 0
        Me.vsfDet.Size = New System.Drawing.Size(916, 363)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 0
        '
        'pnlAddScreenDocs
        '
        Me.pnlAddScreenDocs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlAddScreenDocs.Controls.Add(Me.txtSlNo)
        Me.pnlAddScreenDocs.Controls.Add(Me.Label5)
        Me.pnlAddScreenDocs.Controls.Add(Me.btnDelete)
        Me.pnlAddScreenDocs.Controls.Add(Me.txtGroupName)
        Me.pnlAddScreenDocs.Controls.Add(Me.txtDocID)
        Me.pnlAddScreenDocs.Controls.Add(Me.btnSave)
        Me.pnlAddScreenDocs.Controls.Add(Me.btnCancel)
        Me.pnlAddScreenDocs.Controls.Add(Me.btnBrowse)
        Me.pnlAddScreenDocs.Controls.Add(Me.txtRemarks)
        Me.pnlAddScreenDocs.Controls.Add(Me.txtDoc)
        Me.pnlAddScreenDocs.Controls.Add(Me.Label4)
        Me.pnlAddScreenDocs.Controls.Add(Me.Label3)
        Me.pnlAddScreenDocs.Controls.Add(Me.Label2)
        Me.pnlAddScreenDocs.Controls.Add(Me.Label1)
        Me.pnlAddScreenDocs.Location = New System.Drawing.Point(265, 93)
        Me.pnlAddScreenDocs.Name = "pnlAddScreenDocs"
        Me.pnlAddScreenDocs.Size = New System.Drawing.Size(494, 288)
        Me.pnlAddScreenDocs.TabIndex = 11
        '
        'txtSlNo
        '
        Me.txtSlNo.Location = New System.Drawing.Point(430, 258)
        Me.txtSlNo.Name = "txtSlNo"
        Me.txtSlNo.Size = New System.Drawing.Size(31, 20)
        Me.txtSlNo.TabIndex = 27
        Me.txtSlNo.Visible = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label5.ForeColor = System.Drawing.Color.Black
        Me.Label5.Location = New System.Drawing.Point(23, 112)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(77, 17)
        Me.Label5.TabIndex = 26
        Me.Label5.Text = "Delete a file"
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDelete.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_DELETE
        Me.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDelete.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDelete.Location = New System.Drawing.Point(108, 105)
        Me.btnDelete.Margin = New System.Windows.Forms.Padding(4)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(82, 31)
        Me.btnDelete.TabIndex = 25
        Me.btnDelete.Text = "&Delete "
        Me.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnDelete, "Shortcut : Alt + D")
        Me.btnDelete.UseVisualStyleBackColor = False
        '
        'txtGroupName
        '
        Me.txtGroupName.Location = New System.Drawing.Point(106, 49)
        Me.txtGroupName.Name = "txtGroupName"
        Me.txtGroupName.Size = New System.Drawing.Size(198, 20)
        Me.txtGroupName.TabIndex = 21
        '
        'txtDocID
        '
        Me.txtDocID.Location = New System.Drawing.Point(326, 49)
        Me.txtDocID.Name = "txtDocID"
        Me.txtDocID.Size = New System.Drawing.Size(75, 20)
        Me.txtDocID.TabIndex = 16
        Me.txtDocID.Visible = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(230, 251)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(82, 31)
        Me.btnSave.TabIndex = 19
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Shortcut : Alt + S")
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(317, 251)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(82, 31)
        Me.btnCancel.TabIndex = 20
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Shortcut : Alt + C")
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnBrowse
        '
        Me.btnBrowse.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnBrowse.Location = New System.Drawing.Point(407, 70)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(75, 33)
        Me.btnBrowse.TabIndex = 7
        Me.btnBrowse.Text = "&Browse"
        Me.ToolTip1.SetToolTip(Me.btnBrowse, "Shortcut : Alt + B")
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'txtRemarks
        '
        Me.txtRemarks.Location = New System.Drawing.Point(106, 146)
        Me.txtRemarks.MaxLength = 250
        Me.txtRemarks.Multiline = True
        Me.txtRemarks.Name = "txtRemarks"
        Me.txtRemarks.Size = New System.Drawing.Size(295, 93)
        Me.txtRemarks.TabIndex = 6
        '
        'txtDoc
        '
        Me.txtDoc.Enabled = False
        Me.txtDoc.Location = New System.Drawing.Point(106, 74)
        Me.txtDoc.Name = "txtDoc"
        Me.txtDoc.Size = New System.Drawing.Size(295, 20)
        Me.txtDoc.TabIndex = 5
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label4.ForeColor = System.Drawing.Color.Black
        Me.Label4.Location = New System.Drawing.Point(42, 146)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(58, 17)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Remarks"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label3.ForeColor = System.Drawing.Color.Red
        Me.Label3.Location = New System.Drawing.Point(33, 70)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(67, 17)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Document"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label2.ForeColor = System.Drawing.Color.Red
        Me.Label2.Location = New System.Drawing.Point(19, 48)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(84, 17)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Group Name"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label1.Location = New System.Drawing.Point(175, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(129, 21)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Add  Document"
        '
        'fsd
        '
        Me.fsd.FileName = "OpenFileDialog1"
        '
        'lblfsize
        '
        Me.lblfsize.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblfsize.AutoSize = True
        Me.lblfsize.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblfsize.Location = New System.Drawing.Point(12, 417)
        Me.lblfsize.Name = "lblfsize"
        Me.lblfsize.Size = New System.Drawing.Size(0, 16)
        Me.lblfsize.TabIndex = 13
        '
        'lblfname
        '
        Me.lblfname.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblfname.AutoSize = True
        Me.lblfname.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblfname.Location = New System.Drawing.Point(12, 395)
        Me.lblfname.Name = "lblfname"
        Me.lblfname.Size = New System.Drawing.Size(0, 16)
        Me.lblfname.TabIndex = 14
        '
        'lblmods
        '
        Me.lblmods.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblmods.AutoSize = True
        Me.lblmods.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblmods.Location = New System.Drawing.Point(12, 437)
        Me.lblmods.Name = "lblmods"
        Me.lblmods.Size = New System.Drawing.Size(0, 16)
        Me.lblmods.TabIndex = 15
        '
        'Pnlfilesproperty
        '
        Me.Pnlfilesproperty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Pnlfilesproperty.Controls.Add(Me.lblmod)
        Me.Pnlfilesproperty.Controls.Add(Me.lblFileSize)
        Me.Pnlfilesproperty.Location = New System.Drawing.Point(23, 410)
        Me.Pnlfilesproperty.Name = "Pnlfilesproperty"
        Me.Pnlfilesproperty.Size = New System.Drawing.Size(479, 43)
        Me.Pnlfilesproperty.TabIndex = 18
        Me.Pnlfilesproperty.Visible = False
        '
        'lblmod
        '
        Me.lblmod.AutoSize = True
        Me.lblmod.Font = New System.Drawing.Font("Segoe UI Semibold", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblmod.Location = New System.Drawing.Point(178, 12)
        Me.lblmod.Name = "lblmod"
        Me.lblmod.Size = New System.Drawing.Size(101, 20)
        Me.lblmod.TabIndex = 19
        Me.lblmod.Text = "Last Modified"
        '
        'lblFileSize
        '
        Me.lblFileSize.AutoSize = True
        Me.lblFileSize.Font = New System.Drawing.Font("Segoe UI Semibold", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFileSize.Location = New System.Drawing.Point(24, 12)
        Me.lblFileSize.Name = "lblFileSize"
        Me.lblFileSize.Size = New System.Drawing.Size(68, 20)
        Me.lblFileSize.TabIndex = 18
        Me.lblFileSize.Text = "File Size:"
        '
        'lblformcaption
        '
        Me.lblformcaption.AutoSize = True
        Me.lblformcaption.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblformcaption.Location = New System.Drawing.Point(19, 9)
        Me.lblformcaption.Name = "lblformcaption"
        Me.lblformcaption.Size = New System.Drawing.Size(109, 21)
        Me.lblformcaption.TabIndex = 24
        Me.lblformcaption.Text = "Form Caption"
        '
        'txtfrmCaption
        '
        Me.txtfrmCaption.Location = New System.Drawing.Point(36, 426)
        Me.txtfrmCaption.Name = "txtfrmCaption"
        Me.txtfrmCaption.Size = New System.Drawing.Size(75, 20)
        Me.txtfrmCaption.TabIndex = 25
        Me.txtfrmCaption.Visible = False
        '
        'btnRemove
        '
        Me.btnRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRemove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRemove.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRemove.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRemove.Location = New System.Drawing.Point(799, 419)
        Me.btnRemove.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(70, 31)
        Me.btnRemove.TabIndex = 23
        Me.btnRemove.Text = "&Remove"
        Me.btnRemove.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnRemove, "Shortcut : Alt + R")
        Me.btnRemove.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.CausesValidation = False
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(873, 419)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(70, 31)
        Me.btnClose.TabIndex = 22
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Shortcut : Alt + O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnAdd
        '
        Me.btnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAdd.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_NEW
        Me.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAdd.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAdd.Location = New System.Drawing.Point(648, 419)
        Me.btnAdd.Margin = New System.Windows.Forms.Padding(4)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnAdd.Size = New System.Drawing.Size(70, 31)
        Me.btnAdd.TabIndex = 21
        Me.btnAdd.Text = "&Add"
        Me.btnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnAdd, "Shortcut : Alt + A")
        Me.btnAdd.UseVisualStyleBackColor = False
        '
        'btnEdit
        '
        Me.btnEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEdit.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_EDIT
        Me.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEdit.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnEdit.Location = New System.Drawing.Point(723, 419)
        Me.btnEdit.Margin = New System.Windows.Forms.Padding(4)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(70, 31)
        Me.btnEdit.TabIndex = 26
        Me.btnEdit.Text = "&Edit"
        Me.btnEdit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnEdit, "Shortcut : Alt + E")
        Me.btnEdit.UseVisualStyleBackColor = False
        '
        'frmUtlScreenDocs
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(951, 462)
        Me.Controls.Add(Me.btnEdit)
        Me.Controls.Add(Me.lblformcaption)
        Me.Controls.Add(Me.pnlAddScreenDocs)
        Me.Controls.Add(Me.btnRemove)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.Pnlfilesproperty)
        Me.Controls.Add(Me.lblmods)
        Me.Controls.Add(Me.lblfname)
        Me.Controls.Add(Me.lblfsize)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.txtfrmCaption)
        Me.Name = "frmUtlScreenDocs"
        Me.ShowIcon = False
        Me.Text = "Screen Documents"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlAddScreenDocs.ResumeLayout(False)
        Me.pnlAddScreenDocs.PerformLayout()
        Me.Pnlfilesproperty.ResumeLayout(False)
        Me.Pnlfilesproperty.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents pnlAddScreenDocs As System.Windows.Forms.Panel
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents txtRemarks As System.Windows.Forms.TextBox
    Friend WithEvents txtDoc As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents fbd As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents fsd As System.Windows.Forms.OpenFileDialog
    Friend WithEvents lblfsize As System.Windows.Forms.Label
    Friend WithEvents lblfname As System.Windows.Forms.Label
    Friend WithEvents lblmods As System.Windows.Forms.Label
    Friend WithEvents txtDocID As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Pnlfilesproperty As System.Windows.Forms.Panel
    Friend WithEvents lblmod As System.Windows.Forms.Label
    Friend WithEvents lblFileSize As System.Windows.Forms.Label
    Friend WithEvents btnSave As C1.Win.C1Input.C1Button
    Friend WithEvents btnCancel As C1.Win.C1Input.C1Button
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents btnAdd As C1.Win.C1Input.C1Button
    Friend WithEvents btnRemove As C1.Win.C1Input.C1Button
    Friend WithEvents lblformcaption As System.Windows.Forms.Label
    Friend WithEvents txtGroupName As System.Windows.Forms.TextBox
    Friend WithEvents txtfrmCaption As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnDelete As C1.Win.C1Input.C1Button
    Friend WithEvents btnEdit As C1.Win.C1Input.C1Button
    Friend WithEvents txtSlNo As System.Windows.Forms.TextBox
    Friend WithEvents ToolTip1 As ToolTip
End Class
