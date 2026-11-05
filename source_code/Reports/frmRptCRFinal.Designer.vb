<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRptCRFinal
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRptCRFinal))
        Me.btnFinish = New System.Windows.Forms.Button()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.fraRptName = New System.Windows.Forms.Panel()
        Me.cboGroup = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtRptName = New System.Windows.Forms.TextBox()
        Me.vsfTbls = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.txtRptID = New System.Windows.Forms.TextBox()
        Me.txtCRDesc = New System.Windows.Forms.TextBox()
        Me.txtHdrTblName = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnMoveUp = New System.Windows.Forms.Button()
        Me.btnMoveDown = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtCondition = New System.Windows.Forms.TextBox()
        Me.txtRptFileName = New System.Windows.Forms.TextBox()
        Me.lblRptFileName = New System.Windows.Forms.Label()
        Me.pnlAddnlCol = New System.Windows.Forms.Panel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.vsfFields = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.txtAdd = New System.Windows.Forms.TextBox()
        Me.lblCaption = New System.Windows.Forms.Label()
        Me.txtCaption = New System.Windows.Forms.TextBox()
        Me.chkSlno = New System.Windows.Forms.CheckBox()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraRptName.SuspendLayout()
        CType(Me.vsfTbls, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlAddnlCol.SuspendLayout()
        CType(Me.vsfFields, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnFinish
        '
        Me.btnFinish.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFinish.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnFinish.Image = CType(resources.GetObject("btnFinish.Image"), System.Drawing.Image)
        Me.btnFinish.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnFinish.Location = New System.Drawing.Point(727, 577)
        Me.btnFinish.Name = "btnFinish"
        Me.btnFinish.Size = New System.Drawing.Size(75, 29)
        Me.btnFinish.TabIndex = 206
        Me.btnFinish.Text = "&Create"
        Me.btnFinish.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnFinish.UseVisualStyleBackColor = False
        '
        'vsfDet
        '
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.Location = New System.Drawing.Point(53, 7)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.Count = 4
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(822, 340)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 204
        Me.vsfDet.Tag = "DSGR"
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnClose.Image = CType(resources.GetObject("btnClose.Image"), System.Drawing.Image)
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(808, 577)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(67, 29)
        Me.btnClose.TabIndex = 208
        Me.btnClose.Text = "&Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(50, 350)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(142, 17)
        Me.Label1.TabIndex = 211
        Me.Label1.Text = "Report Name && Group"
        '
        'fraRptName
        '
        Me.fraRptName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fraRptName.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.fraRptName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.fraRptName.Controls.Add(Me.cboGroup)
        Me.fraRptName.Controls.Add(Me.Label4)
        Me.fraRptName.Controls.Add(Me.Label3)
        Me.fraRptName.Controls.Add(Me.txtRptName)
        Me.fraRptName.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.fraRptName.Location = New System.Drawing.Point(53, 370)
        Me.fraRptName.Name = "fraRptName"
        Me.fraRptName.Size = New System.Drawing.Size(822, 52)
        Me.fraRptName.TabIndex = 212
        '
        'cboGroup
        '
        Me.cboGroup.FormattingEnabled = True
        Me.cboGroup.Location = New System.Drawing.Point(518, 13)
        Me.cboGroup.Name = "cboGroup"
        Me.cboGroup.Size = New System.Drawing.Size(279, 25)
        Me.cboGroup.TabIndex = 215
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.Color.Black
        Me.Label4.Location = New System.Drawing.Point(465, 17)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(45, 17)
        Me.Label4.TabIndex = 214
        Me.Label4.Text = "Group"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.Color.Black
        Me.Label3.Location = New System.Drawing.Point(11, 17)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(43, 17)
        Me.Label3.TabIndex = 213
        Me.Label3.Text = "Name"
        '
        'txtRptName
        '
        Me.txtRptName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtRptName.Location = New System.Drawing.Point(62, 13)
        Me.txtRptName.Name = "txtRptName"
        Me.txtRptName.Size = New System.Drawing.Size(394, 25)
        Me.txtRptName.TabIndex = 212
        '
        'vsfTbls
        '
        Me.vsfTbls.AllowEditing = False
        Me.vsfTbls.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
        Me.vsfTbls.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfTbls.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfTbls.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfTbls.ColumnInfo = "1,0,0,0,0,120,Columns:"
        Me.vsfTbls.ExtendLastCol = True
        Me.vsfTbls.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Solid
        Me.vsfTbls.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfTbls.Location = New System.Drawing.Point(68, 61)
        Me.vsfTbls.Name = "vsfTbls"
        Me.vsfTbls.Rows.Count = 4
        Me.vsfTbls.Rows.DefaultSize = 24
        Me.vsfTbls.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfTbls.Size = New System.Drawing.Size(402, 294)
        Me.vsfTbls.StyleInfo = resources.GetString("vsfTbls.StyleInfo")
        Me.vsfTbls.TabIndex = 213
        Me.vsfTbls.Tag = "DSGR"
        Me.vsfTbls.Visible = False
        '
        'txtRptID
        '
        Me.txtRptID.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtRptID.Location = New System.Drawing.Point(764, 122)
        Me.txtRptID.Name = "txtRptID"
        Me.txtRptID.Size = New System.Drawing.Size(77, 25)
        Me.txtRptID.TabIndex = 214
        Me.txtRptID.Visible = False
        '
        'txtCRDesc
        '
        Me.txtCRDesc.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCRDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCRDesc.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtCRDesc.Location = New System.Drawing.Point(53, 445)
        Me.txtCRDesc.MaxLength = 250
        Me.txtCRDesc.Multiline = True
        Me.txtCRDesc.Name = "txtCRDesc"
        Me.txtCRDesc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtCRDesc.Size = New System.Drawing.Size(822, 32)
        Me.txtCRDesc.TabIndex = 216
        '
        'txtHdrTblName
        '
        Me.txtHdrTblName.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtHdrTblName.Location = New System.Drawing.Point(764, 156)
        Me.txtHdrTblName.Name = "txtHdrTblName"
        Me.txtHdrTblName.Size = New System.Drawing.Size(77, 25)
        Me.txtHdrTblName.TabIndex = 217
        Me.txtHdrTblName.Visible = False
        '
        'Label2
        '
        Me.Label2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(50, 425)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(129, 17)
        Me.Label2.TabIndex = 218
        Me.Label2.Text = "Description / Notes :"
        '
        'btnMoveUp
        '
        Me.btnMoveUp.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnMoveUp.Image = CType(resources.GetObject("btnMoveUp.Image"), System.Drawing.Image)
        Me.btnMoveUp.Location = New System.Drawing.Point(12, 122)
        Me.btnMoveUp.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnMoveUp.Name = "btnMoveUp"
        Me.btnMoveUp.Size = New System.Drawing.Size(32, 29)
        Me.btnMoveUp.TabIndex = 219
        Me.btnMoveUp.UseVisualStyleBackColor = True
        '
        'btnMoveDown
        '
        Me.btnMoveDown.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnMoveDown.Image = CType(resources.GetObject("btnMoveDown.Image"), System.Drawing.Image)
        Me.btnMoveDown.Location = New System.Drawing.Point(12, 159)
        Me.btnMoveDown.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnMoveDown.Name = "btnMoveDown"
        Me.btnMoveDown.Size = New System.Drawing.Size(32, 29)
        Me.btnMoveDown.TabIndex = 220
        Me.btnMoveDown.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(50, 478)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(110, 17)
        Me.Label5.TabIndex = 221
        Me.Label5.Text = "Query Condition :"
        '
        'txtCondition
        '
        Me.txtCondition.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCondition.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCondition.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtCondition.Location = New System.Drawing.Point(53, 498)
        Me.txtCondition.Multiline = True
        Me.txtCondition.Name = "txtCondition"
        Me.txtCondition.Size = New System.Drawing.Size(822, 25)
        Me.txtCondition.TabIndex = 222
        '
        'txtRptFileName
        '
        Me.txtRptFileName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtRptFileName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtRptFileName.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtRptFileName.Location = New System.Drawing.Point(53, 546)
        Me.txtRptFileName.Multiline = True
        Me.txtRptFileName.Name = "txtRptFileName"
        Me.txtRptFileName.Size = New System.Drawing.Size(822, 25)
        Me.txtRptFileName.TabIndex = 224
        '
        'lblRptFileName
        '
        Me.lblRptFileName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblRptFileName.AutoSize = True
        Me.lblRptFileName.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblRptFileName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRptFileName.Location = New System.Drawing.Point(50, 526)
        Me.lblRptFileName.Name = "lblRptFileName"
        Me.lblRptFileName.Size = New System.Drawing.Size(93, 17)
        Me.lblRptFileName.TabIndex = 223
        Me.lblRptFileName.Text = "Rpt FileName :"
        '
        'pnlAddnlCol
        '
        Me.pnlAddnlCol.Controls.Add(Me.btnCancel)
        Me.pnlAddnlCol.Controls.Add(Me.vsfFields)
        Me.pnlAddnlCol.Controls.Add(Me.btnOK)
        Me.pnlAddnlCol.Controls.Add(Me.txtAdd)
        Me.pnlAddnlCol.Controls.Add(Me.lblCaption)
        Me.pnlAddnlCol.Controls.Add(Me.txtCaption)
        Me.pnlAddnlCol.Location = New System.Drawing.Point(190, 7)
        Me.pnlAddnlCol.Name = "pnlAddnlCol"
        Me.pnlAddnlCol.Size = New System.Drawing.Size(593, 462)
        Me.pnlAddnlCol.TabIndex = 225
        Me.pnlAddnlCol.Visible = False
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(467, 410)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(58, 34)
        Me.btnCancel.TabIndex = 215
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'vsfFields
        '
        Me.vsfFields.AllowEditing = False
        Me.vsfFields.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
        Me.vsfFields.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfFields.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfFields.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfFields.ColumnInfo = "1,0,0,0,0,120,Columns:"
        Me.vsfFields.ExtendLastCol = True
        Me.vsfFields.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Solid
        Me.vsfFields.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfFields.Location = New System.Drawing.Point(29, 72)
        Me.vsfFields.Name = "vsfFields"
        Me.vsfFields.Rows.Count = 4
        Me.vsfFields.Rows.DefaultSize = 24
        Me.vsfFields.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfFields.Size = New System.Drawing.Size(534, 194)
        Me.vsfFields.StyleInfo = resources.GetString("vsfFields.StyleInfo")
        Me.vsfFields.TabIndex = 214
        Me.vsfFields.Tag = "DSGR"
        '
        'btnOK
        '
        Me.btnOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOK.Location = New System.Drawing.Point(411, 410)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(50, 34)
        Me.btnOK.TabIndex = 4
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'txtAdd
        '
        Me.txtAdd.Location = New System.Drawing.Point(29, 276)
        Me.txtAdd.Multiline = True
        Me.txtAdd.Name = "txtAdd"
        Me.txtAdd.Size = New System.Drawing.Size(496, 123)
        Me.txtAdd.TabIndex = 3
        '
        'lblCaption
        '
        Me.lblCaption.AutoSize = True
        Me.lblCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCaption.Location = New System.Drawing.Point(25, 34)
        Me.lblCaption.Name = "lblCaption"
        Me.lblCaption.Size = New System.Drawing.Size(64, 20)
        Me.lblCaption.TabIndex = 1
        Me.lblCaption.Text = "Caption"
        '
        'txtCaption
        '
        Me.txtCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCaption.Location = New System.Drawing.Point(113, 31)
        Me.txtCaption.Name = "txtCaption"
        Me.txtCaption.Size = New System.Drawing.Size(412, 26)
        Me.txtCaption.TabIndex = 0
        '
        'chkSlno
        '
        Me.chkSlno.AutoSize = True
        Me.chkSlno.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.chkSlno.Location = New System.Drawing.Point(53, 580)
        Me.chkSlno.Name = "chkSlno"
        Me.chkSlno.Size = New System.Drawing.Size(122, 21)
        Me.chkSlno.TabIndex = 226
        Me.chkSlno.Text = "Is Slno Required"
        Me.chkSlno.UseVisualStyleBackColor = True
        '
        'frmRptCRFinal
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(916, 609)
        Me.Controls.Add(Me.chkSlno)
        Me.Controls.Add(Me.pnlAddnlCol)
        Me.Controls.Add(Me.txtRptFileName)
        Me.Controls.Add(Me.lblRptFileName)
        Me.Controls.Add(Me.txtCondition)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.btnMoveDown)
        Me.Controls.Add(Me.btnMoveUp)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtHdrTblName)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtCRDesc)
        Me.Controls.Add(Me.txtRptID)
        Me.Controls.Add(Me.vsfTbls)
        Me.Controls.Add(Me.fraRptName)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnFinish)
        Me.Controls.Add(Me.vsfDet)
        Me.MinimumSize = New System.Drawing.Size(932, 519)
        Me.Name = "frmRptCRFinal"
        Me.ShowIcon = False
        Me.Text = "Customised Reports..."
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraRptName.ResumeLayout(False)
        Me.fraRptName.PerformLayout()
        CType(Me.vsfTbls, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlAddnlCol.ResumeLayout(False)
        Me.pnlAddnlCol.PerformLayout()
        CType(Me.vsfFields, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnFinish As System.Windows.Forms.Button
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents fraRptName As System.Windows.Forms.Panel
    Friend WithEvents txtRptName As System.Windows.Forms.TextBox
    Friend WithEvents vsfTbls As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents txtRptID As System.Windows.Forms.TextBox
    Friend WithEvents txtCRDesc As System.Windows.Forms.TextBox
    Friend WithEvents txtHdrTblName As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cboGroup As System.Windows.Forms.ComboBox
    Friend WithEvents btnMoveUp As System.Windows.Forms.Button
    Friend WithEvents btnMoveDown As System.Windows.Forms.Button
    Friend WithEvents Label5 As Label
    Friend WithEvents txtCondition As TextBox
    Friend WithEvents txtRptFileName As TextBox
    Friend WithEvents lblRptFileName As Label
    Friend WithEvents pnlAddnlCol As Panel
    Friend WithEvents btnOK As Button
    Friend WithEvents txtAdd As TextBox
    Friend WithEvents lblCaption As Label
    Friend WithEvents txtCaption As TextBox
    Friend WithEvents vsfFields As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnCancel As Button
    Friend WithEvents chkSlno As CheckBox
End Class
