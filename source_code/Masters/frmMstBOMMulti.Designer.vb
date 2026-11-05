<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMstBOMMulti
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMstBOMMulti))
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.cboItemGrp = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.lblFrm = New System.Windows.Forms.Label()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cboItem = New System.Windows.Forms.ComboBox()
        Me.grpChangeItem = New System.Windows.Forms.GroupBox()
        Me.cboTo = New System.Windows.Forms.ComboBox()
        Me.chkChangeItem = New System.Windows.Forms.CheckBox()
        Me.LBLUom = New System.Windows.Forms.Label()
        Me.ucBtnTools = New vLQMS.ucBtn()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.grpChangeItem.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Image = Global.vLQMS.My.Resources.Resources.D_CANCEL
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(629, 526)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(89, 28)
        Me.btnCancel.TabIndex = 9
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Image = Global.vLQMS.My.Resources.Resources.D_SAVE
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(539, 526)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(85, 28)
        Me.btnSave.TabIndex = 10
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'cboItemGrp
        '
        Me.cboItemGrp.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboItemGrp.FormattingEnabled = True
        Me.cboItemGrp.Location = New System.Drawing.Point(378, 59)
        Me.cboItemGrp.Name = "cboItemGrp"
        Me.cboItemGrp.Size = New System.Drawing.Size(253, 28)
        Me.cboItemGrp.TabIndex = 13
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Red
        Me.Label1.Location = New System.Drawing.Point(407, 36)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(181, 20)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "BOM belonging to group"
        '
        'vsfDet
        '
        Me.vsfDet.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfDet.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Custom
        Me.vsfDet.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(6, 198)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(812, 321)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 11
        Me.vsfDet.Tag = ""
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(53, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(106, Byte), Integer))
        Me.Panel2.Controls.Add(Me.lblFrm)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(818, 35)
        Me.Panel2.TabIndex = 110
        '
        'lblFrm
        '
        Me.lblFrm.AutoSize = True
        Me.lblFrm.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lblFrm.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFrm.ForeColor = System.Drawing.Color.White
        Me.lblFrm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblFrm.Location = New System.Drawing.Point(13, 10)
        Me.lblFrm.Name = "lblFrm"
        Me.lblFrm.Size = New System.Drawing.Size(0, 25)
        Me.lblFrm.TabIndex = 1
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Image = Global.vLQMS.My.Resources.Resources.D_CLOSE
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(724, 526)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(82, 28)
        Me.btnClose.TabIndex = 111
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnLoad
        '
        Me.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLoad.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoad.ForeColor = System.Drawing.Color.Black
        Me.btnLoad.Image = CType(resources.GetObject("btnLoad.Image"), System.Drawing.Image)
        Me.btnLoad.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnLoad.Location = New System.Drawing.Point(656, 61)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(79, 26)
        Me.btnLoad.TabIndex = 112
        Me.btnLoad.Text = "&Load"
        Me.btnLoad.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnLoad.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Red
        Me.Label2.Location = New System.Drawing.Point(66, 36)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(143, 20)
        Me.Label2.TabIndex = 118
        Me.Label2.Text = "BOM for Child Item"
        '
        'cboItem
        '
        Me.cboItem.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboItem.FormattingEnabled = True
        Me.cboItem.Location = New System.Drawing.Point(6, 59)
        Me.cboItem.Name = "cboItem"
        Me.cboItem.Size = New System.Drawing.Size(253, 28)
        Me.cboItem.TabIndex = 119
        '
        'grpChangeItem
        '
        Me.grpChangeItem.Controls.Add(Me.cboTo)
        Me.grpChangeItem.Location = New System.Drawing.Point(6, 111)
        Me.grpChangeItem.Name = "grpChangeItem"
        Me.grpChangeItem.Size = New System.Drawing.Size(278, 55)
        Me.grpChangeItem.TabIndex = 120
        Me.grpChangeItem.TabStop = False
        '
        'cboTo
        '
        Me.cboTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTo.FormattingEnabled = True
        Me.cboTo.Location = New System.Drawing.Point(11, 16)
        Me.cboTo.Name = "cboTo"
        Me.cboTo.Size = New System.Drawing.Size(253, 28)
        Me.cboTo.TabIndex = 120
        '
        'chkChangeItem
        '
        Me.chkChangeItem.AutoSize = True
        Me.chkChangeItem.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkChangeItem.Location = New System.Drawing.Point(7, 91)
        Me.chkChangeItem.Name = "chkChangeItem"
        Me.chkChangeItem.Size = New System.Drawing.Size(142, 24)
        Me.chkChangeItem.TabIndex = 121
        Me.chkChangeItem.Text = "Change Item To"
        Me.chkChangeItem.UseVisualStyleBackColor = True
        '
        'LBLUom
        '
        Me.LBLUom.AutoSize = True
        Me.LBLUom.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LBLUom.ForeColor = System.Drawing.Color.Black
        Me.LBLUom.Location = New System.Drawing.Point(9, 173)
        Me.LBLUom.Name = "LBLUom"
        Me.LBLUom.Size = New System.Drawing.Size(44, 20)
        Me.LBLUom.TabIndex = 121
        Me.LBLUom.Text = "No's"
        '
        'ucBtnTools
        '
        Me.ucBtnTools.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ucBtnTools.Location = New System.Drawing.Point(3, 526)
        Me.ucBtnTools.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.ucBtnTools.Name = "ucBtnTools"
        Me.ucBtnTools.Size = New System.Drawing.Size(373, 35)
        Me.ucBtnTools.TabIndex = 117
        '
        'frmMstBOMMulti
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(818, 559)
        Me.Controls.Add(Me.chkChangeItem)
        Me.Controls.Add(Me.LBLUom)
        Me.Controls.Add(Me.grpChangeItem)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cboItem)
        Me.Controls.Add(Me.ucBtnTools)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnLoad)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.cboItemGrp)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnCancel)
        Me.Name = "frmMstBOMMulti"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Multiple BOM Update"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.grpChangeItem.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents cboItemGrp As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents lblFrm As System.Windows.Forms.Label
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnLoad As Button
    Friend WithEvents ucBtnTools As ucBtn
    Friend WithEvents Label2 As Label
    Friend WithEvents cboItem As ComboBox
    Friend WithEvents grpChangeItem As GroupBox
    Friend WithEvents chkChangeItem As CheckBox
    Friend WithEvents cboTo As ComboBox
    Friend WithEvents LBLUom As Label
End Class
