<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmUtlDeleteLinkageUpdation
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmUtlDeleteLinkageUpdation))
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.vsfFlds = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfDtl = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfMain = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.cboDelTblName = New System.Windows.Forms.ComboBox()
        Me.fraUtlDeletion = New System.Windows.Forms.Panel()
        Me.txtDocName = New System.Windows.Forms.TextBox()
        Me.lblDocName = New System.Windows.Forms.Label()
        Me.LblChkDeletion = New System.Windows.Forms.Label()
        Me.lblTblnam = New System.Windows.Forms.Label()
        Me.btnSearch = New C1.Win.C1Input.C1Button()
        Me.btnRemove = New C1.Win.C1Input.C1Button()
        Me.btnCancel = New C1.Win.C1Input.C1Button()
        Me.btnSave = New C1.Win.C1Input.C1Button()
        Me.btnAdd = New C1.Win.C1Input.C1Button()
        Me.btnEdit = New C1.Win.C1Input.C1Button()
        Me.btnDel = New C1.Win.C1Input.C1Button()
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.ucBtnTools = New vLQMS.ucBtn()
        CType(Me.vsfFlds, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfDtl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraUtlDeletion.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'vsfFlds
        '
        Me.vsfFlds.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfFlds.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfFlds.AutoResize = True
        Me.vsfFlds.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfFlds.ColumnInfo = "10,0,0,0,0,125,Columns:"
        Me.vsfFlds.ExtendLastCol = True
        Me.vsfFlds.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.vsfFlds.Location = New System.Drawing.Point(7, 273)
        Me.vsfFlds.Margin = New System.Windows.Forms.Padding(7, 6, 7, 6)
        Me.vsfFlds.Name = "vsfFlds"
        Me.vsfFlds.Rows.DefaultSize = 29
        Me.vsfFlds.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.RowRange
        Me.vsfFlds.Size = New System.Drawing.Size(707, 333)
        Me.vsfFlds.StyleInfo = resources.GetString("vsfFlds.StyleInfo")
        Me.vsfFlds.TabIndex = 229
        '
        'vsfDtl
        '
        Me.vsfDtl.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfDtl.AutoResize = True
        Me.vsfDtl.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDtl.ColumnInfo = "10,0,0,0,0,125,Columns:"
        Me.vsfDtl.ExtendLastCol = True
        Me.vsfDtl.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.vsfDtl.Location = New System.Drawing.Point(63, 140)
        Me.vsfDtl.Margin = New System.Windows.Forms.Padding(7, 6, 7, 6)
        Me.vsfDtl.Name = "vsfDtl"
        Me.vsfDtl.Rows.DefaultSize = 29
        Me.vsfDtl.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.RowRange
        Me.vsfDtl.Size = New System.Drawing.Size(263, 120)
        Me.vsfDtl.StyleInfo = resources.GetString("vsfDtl.StyleInfo")
        Me.vsfDtl.TabIndex = 230
        '
        'vsfMain
        '
        Me.vsfMain.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfMain.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfMain.AutoResize = True
        Me.vsfMain.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfMain.ColumnInfo = "10,0,0,0,0,125,Columns:"
        Me.vsfMain.ExtendLastCol = True
        Me.vsfMain.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.vsfMain.Location = New System.Drawing.Point(728, 12)
        Me.vsfMain.Margin = New System.Windows.Forms.Padding(7, 6, 7, 6)
        Me.vsfMain.Name = "vsfMain"
        Me.vsfMain.Rows.DefaultSize = 29
        Me.vsfMain.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.RowRange
        Me.vsfMain.Size = New System.Drawing.Size(607, 643)
        Me.vsfMain.StyleInfo = resources.GetString("vsfMain.StyleInfo")
        Me.vsfMain.TabIndex = 231
        '
        'cboDelTblName
        '
        Me.cboDelTblName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cboDelTblName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboDelTblName.CausesValidation = False
        Me.cboDelTblName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDelTblName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboDelTblName.FormattingEnabled = True
        Me.cboDelTblName.Location = New System.Drawing.Point(241, 12)
        Me.cboDelTblName.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cboDelTblName.Name = "cboDelTblName"
        Me.cboDelTblName.Size = New System.Drawing.Size(327, 33)
        Me.cboDelTblName.TabIndex = 233
        '
        'fraUtlDeletion
        '
        Me.fraUtlDeletion.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fraUtlDeletion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.fraUtlDeletion.Controls.Add(Me.ucBtnTools)
        Me.fraUtlDeletion.Controls.Add(Me.txtDocName)
        Me.fraUtlDeletion.Controls.Add(Me.lblDocName)
        Me.fraUtlDeletion.Controls.Add(Me.LblChkDeletion)
        Me.fraUtlDeletion.Controls.Add(Me.lblTblnam)
        Me.fraUtlDeletion.Controls.Add(Me.cboDelTblName)
        Me.fraUtlDeletion.Controls.Add(Me.btnSearch)
        Me.fraUtlDeletion.Controls.Add(Me.vsfMain)
        Me.fraUtlDeletion.Controls.Add(Me.vsfDtl)
        Me.fraUtlDeletion.Controls.Add(Me.vsfFlds)
        Me.fraUtlDeletion.Controls.Add(Me.btnRemove)
        Me.fraUtlDeletion.Location = New System.Drawing.Point(8, 10)
        Me.fraUtlDeletion.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.fraUtlDeletion.Name = "fraUtlDeletion"
        Me.fraUtlDeletion.Size = New System.Drawing.Size(1345, 665)
        Me.fraUtlDeletion.TabIndex = 232
        '
        'txtDocName
        '
        Me.txtDocName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDocName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDocName.Location = New System.Drawing.Point(241, 65)
        Me.txtDocName.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtDocName.MaxLength = 100
        Me.txtDocName.Name = "txtDocName"
        Me.txtDocName.Size = New System.Drawing.Size(327, 30)
        Me.txtDocName.TabIndex = 253
        Me.txtDocName.Tag = ""
        '
        'lblDocName
        '
        Me.lblDocName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDocName.AutoSize = True
        Me.lblDocName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDocName.ForeColor = System.Drawing.Color.Red
        Me.lblDocName.Location = New System.Drawing.Point(61, 69)
        Me.lblDocName.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblDocName.Name = "lblDocName"
        Me.lblDocName.Size = New System.Drawing.Size(158, 25)
        Me.lblDocName.TabIndex = 252
        Me.lblDocName.Tag = "M"
        Me.lblDocName.Text = "Document Name"
        '
        'LblChkDeletion
        '
        Me.LblChkDeletion.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LblChkDeletion.AutoSize = True
        Me.LblChkDeletion.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblChkDeletion.ForeColor = System.Drawing.Color.Red
        Me.LblChkDeletion.Location = New System.Drawing.Point(61, 107)
        Me.LblChkDeletion.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblChkDeletion.Name = "LblChkDeletion"
        Me.LblChkDeletion.Size = New System.Drawing.Size(297, 25)
        Me.LblChkDeletion.TabIndex = 251
        Me.LblChkDeletion.Tag = "M"
        Me.LblChkDeletion.Text = "Field To be Checked for Deletion"
        '
        'lblTblnam
        '
        Me.lblTblnam.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTblnam.AutoSize = True
        Me.lblTblnam.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTblnam.ForeColor = System.Drawing.Color.Red
        Me.lblTblnam.Location = New System.Drawing.Point(61, 17)
        Me.lblTblnam.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTblnam.Name = "lblTblnam"
        Me.lblTblnam.Size = New System.Drawing.Size(119, 25)
        Me.lblTblnam.TabIndex = 244
        Me.lblTblnam.Tag = "M"
        Me.lblTblnam.Text = "Table Name"
        '
        'btnSearch
        '
        Me.btnSearch.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSearch.BackgroundImage = Global.vLQMS.My.Resources.Resources.D_FIND
        Me.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.btnSearch.CausesValidation = False
        Me.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSearch.Location = New System.Drawing.Point(388, 177)
        Me.btnSearch.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(80, 43)
        Me.btnSearch.TabIndex = 235
        Me.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSearch, "Search")
        Me.btnSearch.UseVisualStyleBackColor = False
        '
        'btnRemove
        '
        Me.btnRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRemove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRemove.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRemove.Image = Global.vLQMS.My.Resources.Resources.ucRemove_Row
        Me.btnRemove.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRemove.Location = New System.Drawing.Point(5, 614)
        Me.btnRemove.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(135, 38)
        Me.btnRemove.TabIndex = 244
        Me.btnRemove.Text = "&Remove"
        Me.btnRemove.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnRemove, "Remove : Alt + R")
        Me.btnRemove.UseVisualStyleBackColor = False
        Me.btnRemove.Visible = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(352, 8)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(113, 38)
        Me.btnCancel.TabIndex = 241
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Cancel : Alt + C")
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(475, 8)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(109, 38)
        Me.btnSave.TabIndex = 240
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Save : Alt + S")
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnAdd
        '
        Me.btnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAdd.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_NEW
        Me.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAdd.Location = New System.Drawing.Point(5, 5)
        Me.btnAdd.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnAdd.Size = New System.Drawing.Size(103, 41)
        Me.btnAdd.TabIndex = 242
        Me.btnAdd.Text = "&Add"
        Me.btnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnAdd, "Add : Alt + A")
        Me.btnAdd.UseVisualStyleBackColor = False
        '
        'btnEdit
        '
        Me.btnEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnEdit.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_EDIT
        Me.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnEdit.Location = New System.Drawing.Point(118, 8)
        Me.btnEdit.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(105, 38)
        Me.btnEdit.TabIndex = 246
        Me.btnEdit.Text = "&Edit"
        Me.btnEdit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnEdit, "Edit : Alt + E")
        Me.btnEdit.UseVisualStyleBackColor = False
        '
        'btnDel
        '
        Me.btnDel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_DELETE
        Me.btnDel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDel.Location = New System.Drawing.Point(233, 8)
        Me.btnDel.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.btnDel.Name = "btnDel"
        Me.btnDel.Size = New System.Drawing.Size(109, 38)
        Me.btnDel.TabIndex = 245
        Me.btnDel.Text = "&Delete "
        Me.btnDel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnDel, "Delete : Alt + D")
        Me.btnDel.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.CausesValidation = False
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(1244, 704)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(109, 38)
        Me.btnClose.TabIndex = 243
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close : Alt + O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Controls.Add(Me.btnAdd)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnEdit)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnDel)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnCancel)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnSave)
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(16, 687)
        Me.FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(708, 50)
        Me.FlowLayoutPanel1.TabIndex = 244
        '
        'ucBtnTools
        '
        Me.ucBtnTools.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ucBtnTools.BackColor = System.Drawing.Color.Transparent
        Me.ucBtnTools.Location = New System.Drawing.Point(7, 619)
        Me.ucBtnTools.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.ucBtnTools.Name = "ucBtnTools"
        Me.ucBtnTools.Size = New System.Drawing.Size(708, 36)
        Me.ucBtnTools.TabIndex = 254
        '
        'FrmUtlDeleteLinkageUpdation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1360, 746)
        Me.Controls.Add(Me.FlowLayoutPanel1)
        Me.Controls.Add(Me.fraUtlDeletion)
        Me.Controls.Add(Me.btnClose)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(1142, 659)
        Me.Name = "FrmUtlDeleteLinkageUpdation"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Delete Linkage Updation"
        CType(Me.vsfFlds, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfDtl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraUtlDeletion.ResumeLayout(False)
        Me.fraUtlDeletion.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents vsfFlds As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents vsfDtl As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents vsfMain As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnSearch As C1.Win.C1Input.C1Button
    Friend WithEvents cboDelTblName As ComboBox
    Friend WithEvents fraUtlDeletion As Panel
    Friend WithEvents btnDel As C1.Win.C1Input.C1Button
    Friend WithEvents btnSave As C1.Win.C1Input.C1Button
    Friend WithEvents btnCancel As C1.Win.C1Input.C1Button
    Friend WithEvents btnEdit As C1.Win.C1Input.C1Button
    Friend WithEvents btnRemove As C1.Win.C1Input.C1Button
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents btnAdd As C1.Win.C1Input.C1Button
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents lblTblnam As Label
    Friend WithEvents LblChkDeletion As Label
    Friend WithEvents lblDocName As Label
    Friend WithEvents txtDocName As TextBox
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents ucBtnTools As ucBtn
End Class
