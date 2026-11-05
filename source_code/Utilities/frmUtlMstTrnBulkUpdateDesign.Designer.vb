<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlMstTrnBulkUpdateDesign
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlMstTrnBulkUpdateDesign))
        Me.vsfDisp = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnSelTblName = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cboTable = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cboPKFld = New System.Windows.Forms.ComboBox()
        Me.grpDisplay = New System.Windows.Forms.GroupBox()
        Me.GrpUpdate = New System.Windows.Forms.GroupBox()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.fraCmdScr3 = New System.Windows.Forms.Panel()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnBCList = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.txtDocName = New System.Windows.Forms.TextBox()
        Me.txtDocID = New System.Windows.Forms.TextBox()
        Me.txtDummy = New System.Windows.Forms.TextBox()
        CType(Me.vsfDisp, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpDisplay.SuspendLayout()
        Me.GrpUpdate.SuspendLayout()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraCmdScr3.SuspendLayout()
        Me.SuspendLayout()
        '
        'vsfDisp
        '
        Me.vsfDisp.AllowFiltering = True
        Me.vsfDisp.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfDisp.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfDisp.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfDisp.AutoResize = True
        Me.vsfDisp.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfDisp.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDisp.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfDisp.Dock = System.Windows.Forms.DockStyle.Fill
        Me.vsfDisp.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfDisp.ExtendLastCol = True
        Me.vsfDisp.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfDisp.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDisp.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDisp.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDisp.Location = New System.Drawing.Point(3, 26)
        Me.vsfDisp.Margin = New System.Windows.Forms.Padding(5, 7, 5, 7)
        Me.vsfDisp.Name = "vsfDisp"
        Me.vsfDisp.Rows.Count = 10
        Me.vsfDisp.Rows.DefaultSize = 28
        Me.vsfDisp.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDisp.Size = New System.Drawing.Size(206, 103)
        Me.vsfDisp.StyleInfo = resources.GetString("vsfDisp.StyleInfo")
        Me.vsfDisp.TabIndex = 2
        Me.vsfDisp.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2007Blue
        '
        'btnSelTblName
        '
        Me.btnSelTblName.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSelTblName.Location = New System.Drawing.Point(561, 43)
        Me.btnSelTblName.Margin = New System.Windows.Forms.Padding(5, 8, 5, 8)
        Me.btnSelTblName.Name = "btnSelTblName"
        Me.btnSelTblName.Size = New System.Drawing.Size(37, 33)
        Me.btnSelTblName.TabIndex = 324
        Me.btnSelTblName.Text = "..."
        Me.ToolTip1.SetToolTip(Me.btnSelTblName, "Select Table Name")
        Me.btnSelTblName.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.Red
        Me.Label1.Location = New System.Drawing.Point(12, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(136, 23)
        Me.Label1.TabIndex = 302
        Me.Label1.Text = "Enter Doc Name"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.Color.Red
        Me.Label4.Location = New System.Drawing.Point(12, 47)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(185, 23)
        Me.Label4.TabIndex = 314
        Me.Label4.Text = "Select SQL Table Name"
        '
        'cboTable
        '
        Me.cboTable.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cboTable.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboTable.Enabled = False
        Me.cboTable.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTable.FormattingEnabled = True
        Me.cboTable.Location = New System.Drawing.Point(204, 44)
        Me.cboTable.Margin = New System.Windows.Forms.Padding(5, 7, 5, 7)
        Me.cboTable.Name = "cboTable"
        Me.cboTable.Size = New System.Drawing.Size(347, 31)
        Me.cboTable.TabIndex = 315
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.Color.Red
        Me.Label5.Location = New System.Drawing.Point(12, 80)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(108, 23)
        Me.Label5.TabIndex = 316
        Me.Label5.Text = "Primary Field"
        '
        'cboPKFld
        '
        Me.cboPKFld.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cboPKFld.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboPKFld.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPKFld.Enabled = False
        Me.cboPKFld.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPKFld.FormattingEnabled = True
        Me.cboPKFld.Location = New System.Drawing.Point(204, 77)
        Me.cboPKFld.Margin = New System.Windows.Forms.Padding(5, 7, 5, 7)
        Me.cboPKFld.Name = "cboPKFld"
        Me.cboPKFld.Size = New System.Drawing.Size(347, 31)
        Me.cboPKFld.TabIndex = 317
        '
        'grpDisplay
        '
        Me.grpDisplay.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.grpDisplay.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.grpDisplay.Controls.Add(Me.vsfDisp)
        Me.grpDisplay.ForeColor = System.Drawing.Color.Black
        Me.grpDisplay.Location = New System.Drawing.Point(674, 1)
        Me.grpDisplay.Name = "grpDisplay"
        Me.grpDisplay.Size = New System.Drawing.Size(212, 132)
        Me.grpDisplay.TabIndex = 318
        Me.grpDisplay.TabStop = False
        Me.grpDisplay.Text = "Columns For Display"
        Me.grpDisplay.Visible = False
        '
        'GrpUpdate
        '
        Me.GrpUpdate.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GrpUpdate.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.GrpUpdate.Controls.Add(Me.vsfDet)
        Me.GrpUpdate.ForeColor = System.Drawing.Color.Black
        Me.GrpUpdate.Location = New System.Drawing.Point(12, 113)
        Me.GrpUpdate.Name = "GrpUpdate"
        Me.GrpUpdate.Size = New System.Drawing.Size(874, 291)
        Me.GrpUpdate.TabIndex = 319
        Me.GrpUpdate.TabStop = False
        Me.GrpUpdate.Text = "Allow For Update"
        '
        'vsfDet
        '
        Me.vsfDet.AllowFiltering = True
        Me.vsfDet.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfDet.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfDet.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfDet.AutoResize = True
        Me.vsfDet.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfDet.Dock = System.Windows.Forms.DockStyle.Fill
        Me.vsfDet.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfDet.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(3, 26)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(5, 7, 5, 7)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.Count = 10
        Me.vsfDet.Rows.DefaultSize = 28
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(868, 262)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 2
        Me.vsfDet.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2007Blue
        '
        'fraCmdScr3
        '
        Me.fraCmdScr3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fraCmdScr3.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.fraCmdScr3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.fraCmdScr3.Controls.Add(Me.btnNew)
        Me.fraCmdScr3.Controls.Add(Me.btnClose)
        Me.fraCmdScr3.Controls.Add(Me.btnCancel)
        Me.fraCmdScr3.Controls.Add(Me.btnSave)
        Me.fraCmdScr3.Controls.Add(Me.btnBCList)
        Me.fraCmdScr3.Controls.Add(Me.btnDelete)
        Me.fraCmdScr3.Location = New System.Drawing.Point(12, 411)
        Me.fraCmdScr3.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.fraCmdScr3.Name = "fraCmdScr3"
        Me.fraCmdScr3.Size = New System.Drawing.Size(874, 55)
        Me.fraCmdScr3.TabIndex = 320
        '
        'btnNew
        '
        Me.btnNew.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNew.BackColor = System.Drawing.SystemColors.MenuBar
        Me.btnNew.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_NEW
        Me.btnNew.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNew.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnNew.ForeColor = System.Drawing.Color.Black
        Me.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnNew.Location = New System.Drawing.Point(303, 9)
        Me.btnNew.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(81, 32)
        Me.btnNew.TabIndex = 6
        Me.btnNew.Tag = "New"
        Me.btnNew.Text = "&New"
        Me.btnNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnNew.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(775, 9)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(82, 32)
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
        Me.btnCancel.Location = New System.Drawing.Point(678, 9)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(90, 32)
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
        Me.btnSave.Location = New System.Drawing.Point(592, 9)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(81, 32)
        Me.btnSave.TabIndex = 3
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnBCList
        '
        Me.btnBCList.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBCList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnBCList.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBCList.Image = Global.vLQMS.My.Resources.Resources.D_EDIT
        Me.btnBCList.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnBCList.Location = New System.Drawing.Point(490, 9)
        Me.btnBCList.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnBCList.Name = "btnBCList"
        Me.btnBCList.Size = New System.Drawing.Size(92, 32)
        Me.btnBCList.TabIndex = 1
        Me.btnBCList.Text = "&List All"
        Me.btnBCList.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnBCList.UseVisualStyleBackColor = False
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDelete.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_DELETE
        Me.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDelete.Location = New System.Drawing.Point(389, 9)
        Me.btnDelete.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(95, 32)
        Me.btnDelete.TabIndex = 0
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnDelete.UseVisualStyleBackColor = False
        '
        'txtDocName
        '
        Me.txtDocName.Enabled = False
        Me.txtDocName.Location = New System.Drawing.Point(204, 12)
        Me.txtDocName.Name = "txtDocName"
        Me.txtDocName.Size = New System.Drawing.Size(347, 30)
        Me.txtDocName.TabIndex = 321
        '
        'txtDocID
        '
        Me.txtDocID.Enabled = False
        Me.txtDocID.Location = New System.Drawing.Point(602, 10)
        Me.txtDocID.Name = "txtDocID"
        Me.txtDocID.Size = New System.Drawing.Size(30, 30)
        Me.txtDocID.TabIndex = 322
        Me.txtDocID.Visible = False
        '
        'txtDummy
        '
        Me.txtDummy.Enabled = False
        Me.txtDummy.Location = New System.Drawing.Point(638, 10)
        Me.txtDummy.Name = "txtDummy"
        Me.txtDummy.Size = New System.Drawing.Size(30, 30)
        Me.txtDummy.TabIndex = 323
        Me.txtDummy.Visible = False
        '
        'frmUtlMstTrnBulkUpdateDesign
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 23.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(892, 473)
        Me.Controls.Add(Me.btnSelTblName)
        Me.Controls.Add(Me.txtDummy)
        Me.Controls.Add(Me.txtDocID)
        Me.Controls.Add(Me.txtDocName)
        Me.Controls.Add(Me.fraCmdScr3)
        Me.Controls.Add(Me.GrpUpdate)
        Me.Controls.Add(Me.grpDisplay)
        Me.Controls.Add(Me.cboPKFld)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.cboTable)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(5, 7, 5, 7)
        Me.Name = "frmUtlMstTrnBulkUpdateDesign"
        Me.ShowIcon = False
        Me.Text = "Master Transaction Bulk Update Design"
        CType(Me.vsfDisp, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpDisplay.ResumeLayout(False)
        Me.GrpUpdate.ResumeLayout(False)
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraCmdScr3.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents vsfDisp As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents Label1 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents cboTable As ComboBox
    Friend WithEvents Label5 As Label
    Friend WithEvents cboPKFld As ComboBox
    Friend WithEvents grpDisplay As GroupBox
    Friend WithEvents GrpUpdate As GroupBox
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents fraCmdScr3 As Panel
    Friend WithEvents btnClose As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnSave As Button
    Friend WithEvents btnBCList As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnNew As Button
    Friend WithEvents txtDocName As TextBox
    Friend WithEvents txtDocID As TextBox
    Friend WithEvents txtDummy As TextBox
    Friend WithEvents btnSelTblName As Button
End Class
