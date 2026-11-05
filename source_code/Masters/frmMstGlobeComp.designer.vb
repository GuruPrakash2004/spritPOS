<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMstGlobeComp
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMstGlobeComp))
        Me.txtAID = New System.Windows.Forms.TextBox()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.txtDocId = New System.Windows.Forms.TextBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnDelete = New C1.Win.C1Input.C1Button()
        Me.btnAdd = New C1.Win.C1Input.C1Button()
        Me.btnEdit = New C1.Win.C1Input.C1Button()
        Me.btnSave = New C1.Win.C1Input.C1Button()
        Me.btnCancel = New C1.Win.C1Input.C1Button()
        Me.pnlBtns = New System.Windows.Forms.Panel()
        Me.pnlAdd = New System.Windows.Forms.Panel()
        Me.chkDef = New System.Windows.Forms.CheckBox()
        Me.btnbackup = New System.Windows.Forms.Button()
        Me.txtId = New System.Windows.Forms.TextBox()
        Me.lblCompTitle = New System.Windows.Forms.Label()
        Me.lblCreateFrmBackup = New System.Windows.Forms.Label()
        Me.txtBackup = New System.Windows.Forms.TextBox()
        Me.txtAddCompName = New System.Windows.Forms.TextBox()
        Me.lblCompanyName = New System.Windows.Forms.Label()
        Me.txtDBName = New System.Windows.Forms.TextBox()
        Me.lblDBName = New System.Windows.Forms.Label()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlBtns.SuspendLayout()
        Me.pnlAdd.SuspendLayout()
        Me.SuspendLayout()
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
        Me.vsfDet.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfDet.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfDet.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfDet.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.AutoClipboard = True
        Me.vsfDet.AutoResize = True
        Me.vsfDet.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "3,0,0,0,0,120,Columns:0{Visible:False;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfDet.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfDet.Location = New System.Drawing.Point(12, 13)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.Rows.GlyphRow = 0
        Me.vsfDet.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(660, 362)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 2
        Me.vsfDet.Tag = ""
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Image = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.Location = New System.Drawing.Point(301, 3)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(90, 30)
        Me.btnClose.TabIndex = 3
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
        'btnDelete
        '
        Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDelete.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_DELETE
        Me.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDelete.Location = New System.Drawing.Point(202, 3)
        Me.btnDelete.Margin = New System.Windows.Forms.Padding(4)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(90, 30)
        Me.btnDelete.TabIndex = 2
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnDelete, "Shortcut : Alt + D")
        Me.btnDelete.UseVisualStyleBackColor = False
        '
        'btnAdd
        '
        Me.btnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAdd.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_NEW
        Me.btnAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAdd.Location = New System.Drawing.Point(4, 3)
        Me.btnAdd.Margin = New System.Windows.Forms.Padding(4)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnAdd.Size = New System.Drawing.Size(90, 30)
        Me.btnAdd.TabIndex = 0
        Me.btnAdd.Text = "&Add"
        Me.btnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnAdd, "Shortcut : Alt + A")
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnEdit
        '
        Me.btnEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEdit.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_EDIT
        Me.btnEdit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnEdit.Location = New System.Drawing.Point(103, 3)
        Me.btnEdit.Margin = New System.Windows.Forms.Padding(4)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(90, 30)
        Me.btnEdit.TabIndex = 1
        Me.btnEdit.Text = "&Edit"
        Me.btnEdit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnEdit, "Shortcut : Alt + E")
        Me.btnEdit.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(445, 280)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(90, 30)
        Me.btnSave.TabIndex = 3
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
        Me.btnCancel.Location = New System.Drawing.Point(543, 280)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(90, 30)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Shortcut : Alt + C")
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'pnlBtns
        '
        Me.pnlBtns.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlBtns.AutoSize = True
        Me.pnlBtns.Controls.Add(Me.btnAdd)
        Me.pnlBtns.Controls.Add(Me.btnDelete)
        Me.pnlBtns.Controls.Add(Me.btnEdit)
        Me.pnlBtns.Controls.Add(Me.btnClose)
        Me.pnlBtns.Location = New System.Drawing.Point(277, 382)
        Me.pnlBtns.Name = "pnlBtns"
        Me.pnlBtns.Size = New System.Drawing.Size(395, 35)
        Me.pnlBtns.TabIndex = 216
        '
        'pnlAdd
        '
        Me.pnlAdd.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlAdd.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.pnlAdd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlAdd.Controls.Add(Me.chkDef)
        Me.pnlAdd.Controls.Add(Me.btnbackup)
        Me.pnlAdd.Controls.Add(Me.txtId)
        Me.pnlAdd.Controls.Add(Me.lblCompTitle)
        Me.pnlAdd.Controls.Add(Me.lblCreateFrmBackup)
        Me.pnlAdd.Controls.Add(Me.txtBackup)
        Me.pnlAdd.Controls.Add(Me.btnSave)
        Me.pnlAdd.Controls.Add(Me.btnCancel)
        Me.pnlAdd.Controls.Add(Me.txtAddCompName)
        Me.pnlAdd.Controls.Add(Me.lblCompanyName)
        Me.pnlAdd.Controls.Add(Me.txtDBName)
        Me.pnlAdd.Controls.Add(Me.lblDBName)
        Me.pnlAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlAdd.Location = New System.Drawing.Point(12, 36)
        Me.pnlAdd.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.pnlAdd.Name = "pnlAdd"
        Me.pnlAdd.Size = New System.Drawing.Size(660, 339)
        Me.pnlAdd.TabIndex = 1
        Me.pnlAdd.TabStop = True
        '
        'chkDef
        '
        Me.chkDef.AutoSize = True
        Me.chkDef.Location = New System.Drawing.Point(175, 218)
        Me.chkDef.Name = "chkDef"
        Me.chkDef.Size = New System.Drawing.Size(337, 24)
        Me.chkDef.TabIndex = 466
        Me.chkDef.Text = "Check this to open this Company as Default"
        Me.chkDef.UseVisualStyleBackColor = True
        '
        'btnbackup
        '
        Me.btnbackup.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnbackup.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnbackup.ForeColor = System.Drawing.Color.Black
        Me.btnbackup.Location = New System.Drawing.Point(608, 156)
        Me.btnbackup.Name = "btnbackup"
        Me.btnbackup.Size = New System.Drawing.Size(25, 27)
        Me.btnbackup.TabIndex = 465
        Me.btnbackup.Text = "..."
        Me.btnbackup.UseVisualStyleBackColor = True
        '
        'txtId
        '
        Me.txtId.Location = New System.Drawing.Point(518, 17)
        Me.txtId.Name = "txtId"
        Me.txtId.Size = New System.Drawing.Size(34, 26)
        Me.txtId.TabIndex = 215
        Me.txtId.Visible = False
        '
        'lblCompTitle
        '
        Me.lblCompTitle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCompTitle.AutoSize = True
        Me.lblCompTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCompTitle.ForeColor = System.Drawing.Color.Black
        Me.lblCompTitle.Location = New System.Drawing.Point(1, 17)
        Me.lblCompTitle.Name = "lblCompTitle"
        Me.lblCompTitle.Size = New System.Drawing.Size(258, 25)
        Me.lblCompTitle.TabIndex = 213
        Me.lblCompTitle.Tag = ""
        Me.lblCompTitle.Text = "Creating New Company"
        '
        'lblCreateFrmBackup
        '
        Me.lblCreateFrmBackup.AutoSize = True
        Me.lblCreateFrmBackup.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCreateFrmBackup.ForeColor = System.Drawing.Color.Black
        Me.lblCreateFrmBackup.Location = New System.Drawing.Point(-1, 158)
        Me.lblCreateFrmBackup.Name = "lblCreateFrmBackup"
        Me.lblCreateFrmBackup.Size = New System.Drawing.Size(156, 20)
        Me.lblCreateFrmBackup.TabIndex = 212
        Me.lblCreateFrmBackup.Text = "Create From Backup"
        '
        'txtBackup
        '
        Me.txtBackup.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBackup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtBackup.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBackup.Location = New System.Drawing.Point(155, 156)
        Me.txtBackup.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtBackup.MaxLength = 500
        Me.txtBackup.Name = "txtBackup"
        Me.txtBackup.Size = New System.Drawing.Size(447, 26)
        Me.txtBackup.TabIndex = 2
        '
        'txtAddCompName
        '
        Me.txtAddCompName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAddCompName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAddCompName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddCompName.Location = New System.Drawing.Point(155, 88)
        Me.txtAddCompName.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtAddCompName.MaxLength = 150
        Me.txtAddCompName.Name = "txtAddCompName"
        Me.txtAddCompName.Size = New System.Drawing.Size(478, 26)
        Me.txtAddCompName.TabIndex = 0
        '
        'lblCompanyName
        '
        Me.lblCompanyName.AutoSize = True
        Me.lblCompanyName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCompanyName.ForeColor = System.Drawing.Color.Red
        Me.lblCompanyName.Location = New System.Drawing.Point(-1, 88)
        Me.lblCompanyName.Name = "lblCompanyName"
        Me.lblCompanyName.Size = New System.Drawing.Size(122, 20)
        Me.lblCompanyName.TabIndex = 36
        Me.lblCompanyName.Tag = "M"
        Me.lblCompanyName.Text = "Company Name"
        '
        'txtDBName
        '
        Me.txtDBName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDBName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDBName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDBName.Location = New System.Drawing.Point(155, 122)
        Me.txtDBName.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtDBName.MaxLength = 50
        Me.txtDBName.Name = "txtDBName"
        Me.txtDBName.Size = New System.Drawing.Size(478, 26)
        Me.txtDBName.TabIndex = 1
        '
        'lblDBName
        '
        Me.lblDBName.AutoSize = True
        Me.lblDBName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDBName.ForeColor = System.Drawing.Color.Red
        Me.lblDBName.Location = New System.Drawing.Point(-1, 124)
        Me.lblDBName.Name = "lblDBName"
        Me.lblDBName.Size = New System.Drawing.Size(125, 20)
        Me.lblDBName.TabIndex = 29
        Me.lblDBName.Tag = "M"
        Me.lblDBName.Text = "Database Name"
        '
        'frmMstGlobeComp
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(684, 429)
        Me.Controls.Add(Me.pnlBtns)
        Me.Controls.Add(Me.txtDocId)
        Me.Controls.Add(Me.txtAID)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.pnlAdd)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MinimumSize = New System.Drawing.Size(440, 440)
        Me.Name = "frmMstGlobeComp"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Companies"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlBtns.ResumeLayout(False)
        Me.pnlAdd.ResumeLayout(False)
        Me.pnlAdd.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtAID As System.Windows.Forms.TextBox
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents txtDocId As System.Windows.Forms.TextBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents pnlBtns As Panel
    Friend WithEvents btnAdd As C1.Win.C1Input.C1Button
    Friend WithEvents btnDelete As C1.Win.C1Input.C1Button
    Friend WithEvents btnEdit As C1.Win.C1Input.C1Button
    Friend WithEvents pnlAdd As Panel
    Friend WithEvents lblCreateFrmBackup As Label
    Friend WithEvents txtBackup As TextBox
    Friend WithEvents btnSave As C1.Win.C1Input.C1Button
    Friend WithEvents btnCancel As C1.Win.C1Input.C1Button
    Friend WithEvents txtAddCompName As TextBox
    Friend WithEvents lblCompanyName As Label
    Friend WithEvents txtDBName As TextBox
    Friend WithEvents lblDBName As Label
    Friend WithEvents lblCompTitle As Label
    Friend WithEvents txtId As TextBox
    Friend WithEvents btnbackup As Button
    Friend WithEvents chkDef As CheckBox
End Class
