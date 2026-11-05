<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlFindFlds
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlFindFlds))
        Me.fraEdit = New System.Windows.Forms.Panel()
        Me.lblAlias5 = New System.Windows.Forms.Label()
        Me.lblAlias2 = New System.Windows.Forms.Label()
        Me.lblAlias3 = New System.Windows.Forms.Label()
        Me.lblAlias4 = New System.Windows.Forms.Label()
        Me.lblAlias1 = New System.Windows.Forms.Label()
        Me.txtAlias5 = New System.Windows.Forms.TextBox()
        Me.txtAlias4 = New System.Windows.Forms.TextBox()
        Me.txtAlias3 = New System.Windows.Forms.TextBox()
        Me.txtAlias2 = New System.Windows.Forms.TextBox()
        Me.txtAlias1 = New System.Windows.Forms.TextBox()
        Me.cboStrFld = New System.Windows.Forms.ComboBox()
        Me.cboTblNam = New System.Windows.Forms.ComboBox()
        Me.lblDispFld = New System.Windows.Forms.Label()
        Me.btnSave = New C1.Win.C1Input.C1Button()
        Me.btnCancel = New C1.Win.C1Input.C1Button()
        Me.txtDispFld = New System.Windows.Forms.TextBox()
        Me.lblTblnam = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.btnDelete = New C1.Win.C1Input.C1Button()
        Me.btnAdd = New C1.Win.C1Input.C1Button()
        Me.btnEdit = New C1.Win.C1Input.C1Button()
        Me.btnExport = New C1.Win.C1Input.C1Button()
        Me.btnAddList = New System.Windows.Forms.Button()
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
        Me.fraEdit.Controls.Add(Me.lblAlias5)
        Me.fraEdit.Controls.Add(Me.lblAlias2)
        Me.fraEdit.Controls.Add(Me.lblAlias3)
        Me.fraEdit.Controls.Add(Me.lblAlias4)
        Me.fraEdit.Controls.Add(Me.lblAlias1)
        Me.fraEdit.Controls.Add(Me.txtAlias5)
        Me.fraEdit.Controls.Add(Me.txtAlias4)
        Me.fraEdit.Controls.Add(Me.txtAlias3)
        Me.fraEdit.Controls.Add(Me.txtAlias2)
        Me.fraEdit.Controls.Add(Me.txtAlias1)
        Me.fraEdit.Controls.Add(Me.cboStrFld)
        Me.fraEdit.Controls.Add(Me.cboTblNam)
        Me.fraEdit.Controls.Add(Me.lblDispFld)
        Me.fraEdit.Controls.Add(Me.btnSave)
        Me.fraEdit.Controls.Add(Me.btnCancel)
        Me.fraEdit.Controls.Add(Me.txtDispFld)
        Me.fraEdit.Controls.Add(Me.lblTblnam)
        Me.fraEdit.Controls.Add(Me.Label7)
        Me.fraEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraEdit.Location = New System.Drawing.Point(1, 13)
        Me.fraEdit.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.fraEdit.Name = "fraEdit"
        Me.fraEdit.Size = New System.Drawing.Size(891, 510)
        Me.fraEdit.TabIndex = 4
        '
        'lblAlias5
        '
        Me.lblAlias5.AutoSize = True
        Me.lblAlias5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAlias5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAlias5.Location = New System.Drawing.Point(8, 283)
        Me.lblAlias5.Name = "lblAlias5"
        Me.lblAlias5.Size = New System.Drawing.Size(90, 20)
        Me.lblAlias5.TabIndex = 219
        Me.lblAlias5.Text = "Alias Field5"
        '
        'lblAlias2
        '
        Me.lblAlias2.AutoSize = True
        Me.lblAlias2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAlias2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAlias2.Location = New System.Drawing.Point(8, 181)
        Me.lblAlias2.Name = "lblAlias2"
        Me.lblAlias2.Size = New System.Drawing.Size(90, 20)
        Me.lblAlias2.TabIndex = 218
        Me.lblAlias2.Text = "Alias Field2"
        '
        'lblAlias3
        '
        Me.lblAlias3.AutoSize = True
        Me.lblAlias3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAlias3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAlias3.Location = New System.Drawing.Point(8, 215)
        Me.lblAlias3.Name = "lblAlias3"
        Me.lblAlias3.Size = New System.Drawing.Size(90, 20)
        Me.lblAlias3.TabIndex = 217
        Me.lblAlias3.Text = "Alias Field3"
        '
        'lblAlias4
        '
        Me.lblAlias4.AutoSize = True
        Me.lblAlias4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAlias4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAlias4.Location = New System.Drawing.Point(8, 249)
        Me.lblAlias4.Name = "lblAlias4"
        Me.lblAlias4.Size = New System.Drawing.Size(90, 20)
        Me.lblAlias4.TabIndex = 216
        Me.lblAlias4.Text = "Alias Field4"
        '
        'lblAlias1
        '
        Me.lblAlias1.AutoSize = True
        Me.lblAlias1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAlias1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAlias1.Location = New System.Drawing.Point(8, 147)
        Me.lblAlias1.Name = "lblAlias1"
        Me.lblAlias1.Size = New System.Drawing.Size(90, 20)
        Me.lblAlias1.TabIndex = 215
        Me.lblAlias1.Text = "Alias Field1"
        '
        'txtAlias5
        '
        Me.txtAlias5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAlias5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAlias5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAlias5.Location = New System.Drawing.Point(127, 281)
        Me.txtAlias5.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtAlias5.MaxLength = 50
        Me.txtAlias5.Name = "txtAlias5"
        Me.txtAlias5.Size = New System.Drawing.Size(737, 26)
        Me.txtAlias5.TabIndex = 12
        '
        'txtAlias4
        '
        Me.txtAlias4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAlias4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAlias4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAlias4.Location = New System.Drawing.Point(127, 247)
        Me.txtAlias4.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtAlias4.MaxLength = 50
        Me.txtAlias4.Name = "txtAlias4"
        Me.txtAlias4.Size = New System.Drawing.Size(737, 26)
        Me.txtAlias4.TabIndex = 11
        '
        'txtAlias3
        '
        Me.txtAlias3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAlias3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAlias3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAlias3.Location = New System.Drawing.Point(127, 213)
        Me.txtAlias3.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtAlias3.MaxLength = 50
        Me.txtAlias3.Name = "txtAlias3"
        Me.txtAlias3.Size = New System.Drawing.Size(737, 26)
        Me.txtAlias3.TabIndex = 10
        '
        'txtAlias2
        '
        Me.txtAlias2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAlias2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAlias2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAlias2.Location = New System.Drawing.Point(127, 179)
        Me.txtAlias2.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtAlias2.MaxLength = 50
        Me.txtAlias2.Name = "txtAlias2"
        Me.txtAlias2.Size = New System.Drawing.Size(737, 26)
        Me.txtAlias2.TabIndex = 9
        '
        'txtAlias1
        '
        Me.txtAlias1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAlias1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAlias1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAlias1.Location = New System.Drawing.Point(127, 145)
        Me.txtAlias1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtAlias1.MaxLength = 50
        Me.txtAlias1.Name = "txtAlias1"
        Me.txtAlias1.Size = New System.Drawing.Size(737, 26)
        Me.txtAlias1.TabIndex = 8
        '
        'cboStrFld
        '
        Me.cboStrFld.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cboStrFld.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboStrFld.FormattingEnabled = True
        Me.cboStrFld.Location = New System.Drawing.Point(127, 46)
        Me.cboStrFld.Margin = New System.Windows.Forms.Padding(4)
        Me.cboStrFld.Name = "cboStrFld"
        Me.cboStrFld.Size = New System.Drawing.Size(176, 28)
        Me.cboStrFld.TabIndex = 6
        Me.cboStrFld.Tag = ""
        '
        'cboTblNam
        '
        Me.cboTblNam.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cboTblNam.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTblNam.FormattingEnabled = True
        Me.cboTblNam.Location = New System.Drawing.Point(127, 8)
        Me.cboTblNam.Margin = New System.Windows.Forms.Padding(4)
        Me.cboTblNam.Name = "cboTblNam"
        Me.cboTblNam.Size = New System.Drawing.Size(176, 28)
        Me.cboTblNam.TabIndex = 5
        Me.cboTblNam.Tag = "M"
        '
        'lblDispFld
        '
        Me.lblDispFld.AutoSize = True
        Me.lblDispFld.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDispFld.ForeColor = System.Drawing.Color.Red
        Me.lblDispFld.Location = New System.Drawing.Point(8, 113)
        Me.lblDispFld.Name = "lblDispFld"
        Me.lblDispFld.Size = New System.Drawing.Size(98, 20)
        Me.lblDispFld.TabIndex = 209
        Me.lblDispFld.Tag = "M"
        Me.lblDispFld.Text = "Display Field"
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(676, 469)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(90, 30)
        Me.btnSave.TabIndex = 13
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
        Me.btnCancel.Location = New System.Drawing.Point(774, 469)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(90, 30)
        Me.btnCancel.TabIndex = 14
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Shortcut : Alt + C")
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'txtDispFld
        '
        Me.txtDispFld.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDispFld.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDispFld.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDispFld.Location = New System.Drawing.Point(127, 111)
        Me.txtDispFld.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtDispFld.MaxLength = 50
        Me.txtDispFld.Name = "txtDispFld"
        Me.txtDispFld.Size = New System.Drawing.Size(737, 26)
        Me.txtDispFld.TabIndex = 7
        '
        'lblTblnam
        '
        Me.lblTblnam.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTblnam.AutoSize = True
        Me.lblTblnam.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTblnam.ForeColor = System.Drawing.Color.Red
        Me.lblTblnam.Location = New System.Drawing.Point(8, 11)
        Me.lblTblnam.Name = "lblTblnam"
        Me.lblTblnam.Size = New System.Drawing.Size(94, 20)
        Me.lblTblnam.TabIndex = 36
        Me.lblTblnam.Tag = "M"
        Me.lblTblnam.Text = "Table Name"
        '
        'Label7
        '
        Me.Label7.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Red
        Me.Label7.Location = New System.Drawing.Point(8, 44)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(104, 20)
        Me.Label7.TabIndex = 29
        Me.Label7.Tag = "M"
        Me.Label7.Text = "Storage Field"
        '
        'vsfDet
        '
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "2,0,0,0,0,120,Columns:0{Visible:False;}" & Global.Microsoft.VisualBasic.ChrW(9)
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
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(876, 475)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 2
        Me.vsfDet.Tag = ""
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(802, 493)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(90, 30)
        Me.btnClose.TabIndex = 206
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Shortcut : Alt + O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDelete.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_DELETE
        Me.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDelete.Location = New System.Drawing.Point(704, 493)
        Me.btnDelete.Margin = New System.Windows.Forms.Padding(4)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(90, 30)
        Me.btnDelete.TabIndex = 204
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
        Me.btnAdd.Location = New System.Drawing.Point(512, 493)
        Me.btnAdd.Margin = New System.Windows.Forms.Padding(4)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnAdd.Size = New System.Drawing.Size(90, 30)
        Me.btnAdd.TabIndex = 203
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
        Me.btnEdit.Location = New System.Drawing.Point(608, 493)
        Me.btnEdit.Margin = New System.Windows.Forms.Padding(4)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(90, 30)
        Me.btnEdit.TabIndex = 205
        Me.btnEdit.Text = "&Edit"
        Me.btnEdit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnEdit, "Shortcut : Alt + E")
        Me.btnEdit.UseVisualStyleBackColor = False
        '
        'btnExport
        '
        Me.btnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExport.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExport.Image = Global.vLQMS.My.Resources.Resources.ucexport_to_XL
        Me.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExport.Location = New System.Drawing.Point(15, 493)
        Me.btnExport.Margin = New System.Windows.Forms.Padding(4)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnExport.Size = New System.Drawing.Size(38, 29)
        Me.btnExport.TabIndex = 213
        Me.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnExport, "Shortcut : Alt + F12")
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnAddList
        '
        Me.btnAddList.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAddList.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_FIND
        Me.btnAddList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnAddList.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAddList.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAddList.Location = New System.Drawing.Point(60, 493)
        Me.btnAddList.Name = "btnAddList"
        Me.btnAddList.Size = New System.Drawing.Size(36, 29)
        Me.btnAddList.TabIndex = 216
        Me.btnAddList.UseVisualStyleBackColor = True
        Me.btnAddList.Visible = False
        '
        'frmUtlFindFlds
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(893, 552)
        Me.Controls.Add(Me.fraEdit)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.btnEdit)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.btnAddList)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MinimumSize = New System.Drawing.Size(540, 426)
        Me.Name = "frmUtlFindFlds"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "View Search Display Fields"
        Me.fraEdit.ResumeLayout(False)
        Me.fraEdit.PerformLayout()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents fraEdit As System.Windows.Forms.Panel
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtDispFld As System.Windows.Forms.TextBox
    Friend WithEvents lblTblnam As System.Windows.Forms.Label
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents btnDelete As C1.Win.C1Input.C1Button
    Friend WithEvents btnAdd As C1.Win.C1Input.C1Button
    Friend WithEvents btnEdit As C1.Win.C1Input.C1Button
    Friend WithEvents btnSave As C1.Win.C1Input.C1Button
    Friend WithEvents btnCancel As C1.Win.C1Input.C1Button
    Friend WithEvents lblDispFld As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents btnExport As C1.Win.C1Input.C1Button
    Friend WithEvents cboTblNam As ComboBox
    Friend WithEvents cboStrFld As ComboBox
    Friend WithEvents btnAddList As Button
    Friend WithEvents lblAlias5 As Label
    Friend WithEvents lblAlias2 As Label
    Friend WithEvents lblAlias3 As Label
    Friend WithEvents lblAlias4 As Label
    Friend WithEvents lblAlias1 As Label
    Friend WithEvents txtAlias5 As TextBox
    Friend WithEvents txtAlias4 As TextBox
    Friend WithEvents txtAlias3 As TextBox
    Friend WithEvents txtAlias2 As TextBox
    Friend WithEvents txtAlias1 As TextBox
End Class
