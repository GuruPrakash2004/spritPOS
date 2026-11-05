<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmTrnAddMultiItem
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTrnAddMultiItem))
        Me.lblItemGroup = New System.Windows.Forms.Label()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.txtID = New System.Windows.Forms.TextBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.btnView = New C1.Win.C1Input.C1Button()
        Me.btnReplace = New C1.Win.C1Input.C1Button()
        Me.btnApply = New C1.Win.C1Input.C1Button()
        Me.btnCancel = New C1.Win.C1Input.C1Button()
        Me.cboItemGroup = New System.Windows.Forms.ComboBox()
        Me.txtBCScan = New System.Windows.Forms.TextBox()
        Me.txtPAckingItem = New System.Windows.Forms.TextBox()
        Me.txtShortName = New System.Windows.Forms.TextBox()
        Me.lblBCScan = New System.Windows.Forms.Label()
        Me.lblPackItem = New System.Windows.Forms.Label()
        Me.lblShortName = New System.Windows.Forms.Label()
        Me.btnShrtName = New System.Windows.Forms.Button()
        Me.btnItemGroup = New System.Windows.Forms.Button()
        Me.btnPackingItem = New System.Windows.Forms.Button()
        Me.lblType = New System.Windows.Forms.Label()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblItemGroup
        '
        Me.lblItemGroup.AutoSize = True
        Me.lblItemGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItemGroup.ForeColor = System.Drawing.Color.Black
        Me.lblItemGroup.Location = New System.Drawing.Point(7, 17)
        Me.lblItemGroup.Name = "lblItemGroup"
        Me.lblItemGroup.Size = New System.Drawing.Size(90, 20)
        Me.lblItemGroup.TabIndex = 30
        Me.lblItemGroup.Tag = ""
        Me.lblItemGroup.Text = "Item Group"
        '
        'vsfDet
        '
        Me.vsfDet.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfDet.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfDet.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfDet.AllowResizing = C1.Win.C1FlexGrid.AllowResizingEnum.None
        Me.vsfDet.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.AutoResize = True
        Me.vsfDet.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfDet.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfDet.Location = New System.Drawing.Point(11, 156)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(786, 225)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 36
        Me.vsfDet.Tag = ""
        '
        'txtID
        '
        Me.txtID.Location = New System.Drawing.Point(154, 248)
        Me.txtID.Name = "txtID"
        Me.txtID.Size = New System.Drawing.Size(32, 25)
        Me.txtID.TabIndex = 210
        Me.txtID.Visible = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = CType(resources.GetObject("btnClose.BackgroundImage"), System.Drawing.Image)
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(504, 389)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(196, 30)
        Me.btnClose.TabIndex = 40
        Me.btnClose.Text = "F11 - Apply and Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close : Alt + O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnView
        '
        Me.btnView.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnView.BackgroundImage = CType(resources.GetObject("btnView.BackgroundImage"), System.Drawing.Image)
        Me.btnView.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnView.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnView.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnView.Location = New System.Drawing.Point(691, 122)
        Me.btnView.Margin = New System.Windows.Forms.Padding(4)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(105, 29)
        Me.btnView.TabIndex = 218
        Me.btnView.Text = "F4 - &View"
        Me.btnView.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnView, "Shortcut : Alt + V")
        Me.btnView.UseVisualStyleBackColor = False
        '
        'btnReplace
        '
        Me.btnReplace.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnReplace.BackgroundImage = Global.vLQMS.My.Resources.Resources.ucRestore1
        Me.btnReplace.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnReplace.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReplace.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnReplace.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnReplace.Location = New System.Drawing.Point(12, 389)
        Me.btnReplace.Margin = New System.Windows.Forms.Padding(4)
        Me.btnReplace.Name = "btnReplace"
        Me.btnReplace.Size = New System.Drawing.Size(30, 30)
        Me.btnReplace.TabIndex = 584
        Me.btnReplace.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnReplace, "Remove : Alt + R")
        Me.btnReplace.UseVisualStyleBackColor = False
        '
        'btnApply
        '
        Me.btnApply.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnApply.BackColor = System.Drawing.Color.Transparent
        Me.btnApply.BackgroundImage = CType(resources.GetObject("btnApply.BackgroundImage"), System.Drawing.Image)
        Me.btnApply.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnApply.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnApply.Location = New System.Drawing.Point(379, 389)
        Me.btnApply.Margin = New System.Windows.Forms.Padding(4)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(117, 30)
        Me.btnApply.TabIndex = 216
        Me.btnApply.Text = "F10 - &Apply"
        Me.btnApply.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnApply, "Apply : Alt + A")
        Me.btnApply.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(708, 389)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(90, 30)
        Me.btnCancel.TabIndex = 42
        Me.btnCancel.Text = "&Clear"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Cancel : Alt + C")
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'cboItemGroup
        '
        Me.cboItemGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboItemGroup.FormattingEnabled = True
        Me.cboItemGroup.Location = New System.Drawing.Point(143, 13)
        Me.cboItemGroup.Name = "cboItemGroup"
        Me.cboItemGroup.Size = New System.Drawing.Size(473, 28)
        Me.cboItemGroup.TabIndex = 217
        Me.cboItemGroup.Tag = "ItemType"
        '
        'txtBCScan
        '
        Me.txtBCScan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtBCScan.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBCScan.Location = New System.Drawing.Point(143, 123)
        Me.txtBCScan.Name = "txtBCScan"
        Me.txtBCScan.Size = New System.Drawing.Size(473, 26)
        Me.txtBCScan.TabIndex = 224
        '
        'txtPAckingItem
        '
        Me.txtPAckingItem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPAckingItem.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPAckingItem.Location = New System.Drawing.Point(143, 87)
        Me.txtPAckingItem.Name = "txtPAckingItem"
        Me.txtPAckingItem.Size = New System.Drawing.Size(473, 26)
        Me.txtPAckingItem.TabIndex = 223
        '
        'txtShortName
        '
        Me.txtShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtShortName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtShortName.Location = New System.Drawing.Point(143, 51)
        Me.txtShortName.Name = "txtShortName"
        Me.txtShortName.Size = New System.Drawing.Size(473, 26)
        Me.txtShortName.TabIndex = 222
        '
        'lblBCScan
        '
        Me.lblBCScan.AutoSize = True
        Me.lblBCScan.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBCScan.Location = New System.Drawing.Point(7, 127)
        Me.lblBCScan.Name = "lblBCScan"
        Me.lblBCScan.Size = New System.Drawing.Size(113, 20)
        Me.lblBCScan.TabIndex = 221
        Me.lblBCScan.Text = "BarCode Scan"
        '
        'lblPackItem
        '
        Me.lblPackItem.AutoSize = True
        Me.lblPackItem.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPackItem.Location = New System.Drawing.Point(7, 91)
        Me.lblPackItem.Name = "lblPackItem"
        Me.lblPackItem.Size = New System.Drawing.Size(101, 20)
        Me.lblPackItem.TabIndex = 220
        Me.lblPackItem.Text = "Packing Item"
        '
        'lblShortName
        '
        Me.lblShortName.AutoSize = True
        Me.lblShortName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShortName.Location = New System.Drawing.Point(7, 55)
        Me.lblShortName.Name = "lblShortName"
        Me.lblShortName.Size = New System.Drawing.Size(94, 20)
        Me.lblShortName.TabIndex = 219
        Me.lblShortName.Text = "Short Name"
        '
        'btnShrtName
        '
        Me.btnShrtName.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnShrtName.Location = New System.Drawing.Point(622, 51)
        Me.btnShrtName.Name = "btnShrtName"
        Me.btnShrtName.Size = New System.Drawing.Size(33, 26)
        Me.btnShrtName.TabIndex = 225
        Me.btnShrtName.Text = "..."
        Me.btnShrtName.UseVisualStyleBackColor = True
        '
        'btnItemGroup
        '
        Me.btnItemGroup.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnItemGroup.Location = New System.Drawing.Point(622, 15)
        Me.btnItemGroup.Name = "btnItemGroup"
        Me.btnItemGroup.Size = New System.Drawing.Size(33, 26)
        Me.btnItemGroup.TabIndex = 226
        Me.btnItemGroup.Text = "..."
        Me.btnItemGroup.UseVisualStyleBackColor = True
        '
        'btnPackingItem
        '
        Me.btnPackingItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPackingItem.Location = New System.Drawing.Point(622, 86)
        Me.btnPackingItem.Name = "btnPackingItem"
        Me.btnPackingItem.Size = New System.Drawing.Size(33, 26)
        Me.btnPackingItem.TabIndex = 227
        Me.btnPackingItem.Text = "..."
        Me.btnPackingItem.UseVisualStyleBackColor = True
        '
        'lblType
        '
        Me.lblType.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblType.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.lblType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblType.ForeColor = System.Drawing.Color.Red
        Me.lblType.Location = New System.Drawing.Point(49, 389)
        Me.lblType.Name = "lblType"
        Me.lblType.Size = New System.Drawing.Size(311, 30)
        Me.lblType.TabIndex = 583
        Me.lblType.Text = "Press Escape to Close Form." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Press Delete to Remove a Row"
        '
        'frmTrnAddMultiItem
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ClientSize = New System.Drawing.Size(809, 424)
        Me.Controls.Add(Me.btnReplace)
        Me.Controls.Add(Me.lblType)
        Me.Controls.Add(Me.btnPackingItem)
        Me.Controls.Add(Me.btnItemGroup)
        Me.Controls.Add(Me.btnShrtName)
        Me.Controls.Add(Me.txtBCScan)
        Me.Controls.Add(Me.txtPAckingItem)
        Me.Controls.Add(Me.txtShortName)
        Me.Controls.Add(Me.lblBCScan)
        Me.Controls.Add(Me.lblPackItem)
        Me.Controls.Add(Me.lblShortName)
        Me.Controls.Add(Me.btnView)
        Me.Controls.Add(Me.cboItemGroup)
        Me.Controls.Add(Me.btnApply)
        Me.Controls.Add(Me.lblItemGroup)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.txtID)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "frmTrnAddMultiItem"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Add Multiple Items "
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblItemGroup As System.Windows.Forms.Label
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnCancel As C1.Win.C1Input.C1Button
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents txtID As System.Windows.Forms.TextBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents btnApply As C1.Win.C1Input.C1Button
    Friend WithEvents cboItemGroup As ComboBox
    Friend WithEvents btnView As C1.Win.C1Input.C1Button
    Friend WithEvents txtBCScan As TextBox
    Friend WithEvents txtPAckingItem As TextBox
    Friend WithEvents txtShortName As TextBox
    Friend WithEvents lblBCScan As Label
    Friend WithEvents lblPackItem As Label
    Friend WithEvents lblShortName As Label
    Friend WithEvents btnShrtName As Button
    Friend WithEvents btnItemGroup As Button
    Friend WithEvents btnPackingItem As Button
    Friend WithEvents lblType As Label
    Friend WithEvents btnReplace As C1.Win.C1Input.C1Button
End Class
