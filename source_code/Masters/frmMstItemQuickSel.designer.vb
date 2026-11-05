<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMstItemQuickSel
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMstItemQuickSel))
        Me.lblSelName = New System.Windows.Forms.Label()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.txtID = New System.Windows.Forms.TextBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnSave = New C1.Win.C1Input.C1Button()
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.btnDelete = New C1.Win.C1Input.C1Button()
        Me.btnLoad = New C1.Win.C1Input.C1Button()
        Me.btnRemove = New C1.Win.C1Input.C1Button()
        Me.btnAddItem = New System.Windows.Forms.Button()
        Me.cboSel = New System.Windows.Forms.ComboBox()
        Me.txtDummy2 = New System.Windows.Forms.TextBox()
        Me.txtDummy = New System.Windows.Forms.TextBox()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblSelName
        '
        Me.lblSelName.AutoSize = True
        Me.lblSelName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSelName.ForeColor = System.Drawing.Color.Black
        Me.lblSelName.Location = New System.Drawing.Point(12, 20)
        Me.lblSelName.Name = "lblSelName"
        Me.lblSelName.Size = New System.Drawing.Size(155, 20)
        Me.lblSelName.TabIndex = 30
        Me.lblSelName.Tag = ""
        Me.lblSelName.Text = "Item Quick Selection"
        '
        'vsfDet
        '
        Me.vsfDet.AllowFiltering = True
        Me.vsfDet.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfDet.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfDet.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
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
        Me.vsfDet.Location = New System.Drawing.Point(11, 50)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.Rows.GlyphRow = 0
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(560, 452)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 36
        Me.vsfDet.Tag = ""
        '
        'txtID
        '
        Me.txtID.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtID.Location = New System.Drawing.Point(194, 514)
        Me.txtID.Name = "txtID"
        Me.txtID.Size = New System.Drawing.Size(32, 25)
        Me.txtID.TabIndex = 210
        Me.txtID.Visible = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(285, 510)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(90, 30)
        Me.btnSave.TabIndex = 216
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Save : Alt + S")
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(481, 510)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(90, 30)
        Me.btnClose.TabIndex = 42
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close : Alt + O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDelete.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_DELETE
        Me.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDelete.Location = New System.Drawing.Point(383, 510)
        Me.btnDelete.Margin = New System.Windows.Forms.Padding(4)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(90, 30)
        Me.btnDelete.TabIndex = 40
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnDelete, "Delete : Alt + D")
        Me.btnDelete.UseVisualStyleBackColor = False
        '
        'btnLoad
        '
        Me.btnLoad.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLoad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLoad.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoad.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnLoad.Location = New System.Drawing.Point(481, 17)
        Me.btnLoad.Margin = New System.Windows.Forms.Padding(4)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(90, 29)
        Me.btnLoad.TabIndex = 218
        Me.btnLoad.Text = "&Load"
        Me.ToolTip1.SetToolTip(Me.btnLoad, "Shortcut : Alt + L")
        Me.btnLoad.UseVisualStyleBackColor = False
        '
        'btnRemove
        '
        Me.btnRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRemove.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRemove.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRemove.Location = New System.Drawing.Point(11, 509)
        Me.btnRemove.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(30, 30)
        Me.btnRemove.TabIndex = 219
        Me.btnRemove.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnRemove, "Remove : Alt + R")
        Me.btnRemove.UseVisualStyleBackColor = False
        '
        'btnAddItem
        '
        Me.btnAddItem.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAddItem.BackgroundImage = Global.vLQMS.My.Resources.Resources.add
        Me.btnAddItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnAddItem.Location = New System.Drawing.Point(48, 508)
        Me.btnAddItem.Name = "btnAddItem"
        Me.btnAddItem.Size = New System.Drawing.Size(38, 32)
        Me.btnAddItem.TabIndex = 222
        Me.ToolTip1.SetToolTip(Me.btnAddItem, "Add Item : F4")
        Me.btnAddItem.UseVisualStyleBackColor = True
        '
        'cboSel
        '
        Me.cboSel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSel.FormattingEnabled = True
        Me.cboSel.Location = New System.Drawing.Point(173, 17)
        Me.cboSel.Name = "cboSel"
        Me.cboSel.Size = New System.Drawing.Size(287, 28)
        Me.cboSel.TabIndex = 217
        Me.cboSel.Tag = "ItemType"
        '
        'txtDummy2
        '
        Me.txtDummy2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtDummy2.Location = New System.Drawing.Point(258, 514)
        Me.txtDummy2.Name = "txtDummy2"
        Me.txtDummy2.Size = New System.Drawing.Size(20, 25)
        Me.txtDummy2.TabIndex = 220
        Me.txtDummy2.Visible = False
        '
        'txtDummy
        '
        Me.txtDummy.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtDummy.Location = New System.Drawing.Point(232, 514)
        Me.txtDummy.Name = "txtDummy"
        Me.txtDummy.Size = New System.Drawing.Size(21, 25)
        Me.txtDummy.TabIndex = 221
        Me.txtDummy.Visible = False
        '
        'frmMstItemQuickSel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(584, 553)
        Me.Controls.Add(Me.btnAddItem)
        Me.Controls.Add(Me.txtDummy)
        Me.Controls.Add(Me.txtDummy2)
        Me.Controls.Add(Me.btnRemove)
        Me.Controls.Add(Me.btnLoad)
        Me.Controls.Add(Me.cboSel)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.txtID)
        Me.Controls.Add(Me.lblSelName)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnDelete)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximumSize = New System.Drawing.Size(600, 591)
        Me.MinimumSize = New System.Drawing.Size(600, 591)
        Me.Name = "frmMstItemQuickSel"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Item Quick Selection"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents btnDelete As C1.Win.C1Input.C1Button
    Friend WithEvents txtID As System.Windows.Forms.TextBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents btnSave As C1.Win.C1Input.C1Button
    Friend WithEvents cboSel As ComboBox
    Friend WithEvents btnLoad As C1.Win.C1Input.C1Button
    Friend WithEvents btnRemove As C1.Win.C1Input.C1Button
    Friend WithEvents txtDummy2 As TextBox
    Friend WithEvents txtDummy As TextBox
    Friend WithEvents btnAddItem As Button
    Public WithEvents lblSelName As Label
End Class
