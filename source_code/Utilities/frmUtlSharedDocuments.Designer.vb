<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlSharedDocuments
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlSharedDocuments))
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.btnRemove = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.pnlAddSharedDocs = New System.Windows.Forms.Panel()
        Me.txtDocID = New System.Windows.Forms.TextBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.txtRemarks = New System.Windows.Forms.TextBox()
        Me.txtDoc = New System.Windows.Forms.TextBox()
        Me.cboGroup = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.fbd = New System.Windows.Forms.FolderBrowserDialog()
        Me.fsd = New System.Windows.Forms.OpenFileDialog()
        Me.lblfsize = New System.Windows.Forms.Label()
        Me.lblfname = New System.Windows.Forms.Label()
        Me.lblmod = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnExport = New C1.Win.C1Input.C1Button()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlAddSharedDocs.SuspendLayout()
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
        Me.vsfDet.Location = New System.Drawing.Point(23, 69)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 19
        Me.vsfDet.Rows.Fixed = 0
        Me.vsfDet.Rows.GlyphRow = 0
        Me.vsfDet.Size = New System.Drawing.Size(916, 377)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 0
        '
        'btnAdd
        '
        Me.btnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAdd.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_NEW
        Me.btnAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAdd.Location = New System.Drawing.Point(620, 564)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(75, 23)
        Me.btnAdd.TabIndex = 1
        Me.btnAdd.Text = "&Add"
        Me.btnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnAdd, "Add:Alt+A")
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnEdit
        '
        Me.btnEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEdit.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_EDIT
        Me.btnEdit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEdit.Location = New System.Drawing.Point(701, 564)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(75, 23)
        Me.btnEdit.TabIndex = 2
        Me.btnEdit.Text = "&Edit"
        Me.btnEdit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnEdit, "Edit:Alt+E")
        Me.btnEdit.UseVisualStyleBackColor = True
        '
        'btnRemove
        '
        Me.btnRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRemove.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnRemove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRemove.Location = New System.Drawing.Point(782, 564)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(75, 23)
        Me.btnRemove.TabIndex = 3
        Me.btnRemove.Text = "&Remove"
        Me.ToolTip1.SetToolTip(Me.btnRemove, "REmove:Alt+R")
        Me.btnRemove.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Location = New System.Drawing.Point(863, 564)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 5
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close : Alt+O")
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox1.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(434, 452)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(505, 105)
        Me.TextBox1.TabIndex = 8
        Me.TextBox1.Text = resources.GetString("TextBox1.Text")
        '
        'pnlAddSharedDocs
        '
        Me.pnlAddSharedDocs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlAddSharedDocs.Controls.Add(Me.txtDocID)
        Me.pnlAddSharedDocs.Controls.Add(Me.btnCancel)
        Me.pnlAddSharedDocs.Controls.Add(Me.btnOk)
        Me.pnlAddSharedDocs.Controls.Add(Me.btnBrowse)
        Me.pnlAddSharedDocs.Controls.Add(Me.txtRemarks)
        Me.pnlAddSharedDocs.Controls.Add(Me.txtDoc)
        Me.pnlAddSharedDocs.Controls.Add(Me.cboGroup)
        Me.pnlAddSharedDocs.Controls.Add(Me.Label4)
        Me.pnlAddSharedDocs.Controls.Add(Me.Label3)
        Me.pnlAddSharedDocs.Controls.Add(Me.Label2)
        Me.pnlAddSharedDocs.Controls.Add(Me.Label1)
        Me.pnlAddSharedDocs.Location = New System.Drawing.Point(274, 131)
        Me.pnlAddSharedDocs.Name = "pnlAddSharedDocs"
        Me.pnlAddSharedDocs.Size = New System.Drawing.Size(414, 232)
        Me.pnlAddSharedDocs.TabIndex = 11
        Me.pnlAddSharedDocs.Visible = False
        '
        'txtDocID
        '
        Me.txtDocID.Location = New System.Drawing.Point(309, 49)
        Me.txtDocID.Name = "txtDocID"
        Me.txtDocID.Size = New System.Drawing.Size(75, 20)
        Me.txtDocID.TabIndex = 1
        Me.txtDocID.Visible = False
        '
        'btnCancel
        '
        Me.btnCancel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Location = New System.Drawing.Point(309, 199)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 6
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Cancel:Alt+C")
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOk.Location = New System.Drawing.Point(232, 199)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(71, 23)
        Me.btnOk.TabIndex = 5
        Me.btnOk.Text = "O&k"
        Me.ToolTip1.SetToolTip(Me.btnOk, "Ok:Alt+K")
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnBrowse
        '
        Me.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBrowse.Location = New System.Drawing.Point(309, 72)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowse.TabIndex = 3
        Me.btnBrowse.Text = "&Browse"
        Me.ToolTip1.SetToolTip(Me.btnBrowse, "Browse:Alt+B")
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'txtRemarks
        '
        Me.txtRemarks.Location = New System.Drawing.Point(89, 100)
        Me.txtRemarks.MaxLength = 250
        Me.txtRemarks.Multiline = True
        Me.txtRemarks.Name = "txtRemarks"
        Me.txtRemarks.Size = New System.Drawing.Size(295, 93)
        Me.txtRemarks.TabIndex = 4
        '
        'txtDoc
        '
        Me.txtDoc.Enabled = False
        Me.txtDoc.Location = New System.Drawing.Point(89, 74)
        Me.txtDoc.Name = "txtDoc"
        Me.txtDoc.Size = New System.Drawing.Size(214, 20)
        Me.txtDoc.TabIndex = 2
        '
        'cboGroup
        '
        Me.cboGroup.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.cboGroup.FormattingEnabled = True
        Me.cboGroup.Location = New System.Drawing.Point(89, 48)
        Me.cboGroup.Name = "cboGroup"
        Me.cboGroup.Size = New System.Drawing.Size(214, 21)
        Me.cboGroup.TabIndex = 0
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(19, 100)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(57, 15)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Remarks"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.DarkOrchid
        Me.Label3.Location = New System.Drawing.Point(19, 70)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(64, 15)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Document"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.DarkOrchid
        Me.Label2.Location = New System.Drawing.Point(19, 48)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(41, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Group"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(19, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(185, 18)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Add Shared Documents"
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
        Me.lblfsize.Location = New System.Drawing.Point(12, 551)
        Me.lblfsize.Name = "lblfsize"
        Me.lblfsize.Size = New System.Drawing.Size(0, 16)
        Me.lblfsize.TabIndex = 13
        '
        'lblfname
        '
        Me.lblfname.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblfname.AutoSize = True
        Me.lblfname.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblfname.Location = New System.Drawing.Point(12, 529)
        Me.lblfname.Name = "lblfname"
        Me.lblfname.Size = New System.Drawing.Size(0, 16)
        Me.lblfname.TabIndex = 14
        '
        'lblmod
        '
        Me.lblmod.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblmod.AutoSize = True
        Me.lblmod.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblmod.Location = New System.Drawing.Point(12, 571)
        Me.lblmod.Name = "lblmod"
        Me.lblmod.Size = New System.Drawing.Size(0, 16)
        Me.lblmod.TabIndex = 15
        '
        'btnExport
        '
        Me.btnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExport.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnExport.Image = CType(resources.GetObject("btnExport.Image"), System.Drawing.Image)
        Me.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExport.Location = New System.Drawing.Point(23, 452)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(33, 31)
        Me.btnExport.TabIndex = 16
        Me.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnExport, "Export to XL : Alt + X")
        Me.btnExport.UseVisualStyleBackColor = False
        '
        'frmUtlSharedDocuments
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(951, 596)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.lblmod)
        Me.Controls.Add(Me.lblfname)
        Me.Controls.Add(Me.lblfsize)
        Me.Controls.Add(Me.pnlAddSharedDocs)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnRemove)
        Me.Controls.Add(Me.btnEdit)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.vsfDet)
        Me.KeyPreview = True
        Me.Name = "frmUtlSharedDocuments"
        Me.ShowIcon = False
        Me.Text = "Shared Documents"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlAddSharedDocs.ResumeLayout(False)
        Me.pnlAddSharedDocs.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnEdit As System.Windows.Forms.Button
    Friend WithEvents btnRemove As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents pnlAddSharedDocs As System.Windows.Forms.Panel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents txtRemarks As System.Windows.Forms.TextBox
    Friend WithEvents txtDoc As System.Windows.Forms.TextBox
    Friend WithEvents cboGroup As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents fbd As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents fsd As System.Windows.Forms.OpenFileDialog
    Friend WithEvents lblfsize As System.Windows.Forms.Label
    Friend WithEvents lblfname As System.Windows.Forms.Label
    Friend WithEvents lblmod As System.Windows.Forms.Label
    Friend WithEvents txtDocID As System.Windows.Forms.TextBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents btnExport As C1.Win.C1Input.C1Button
End Class
