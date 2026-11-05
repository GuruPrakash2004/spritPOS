<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlFldCaptions
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlFldCaptions))
        Me.vsfSearch = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.btnPnlClose = New System.Windows.Forms.Button()
        Me.btnPnlSave = New System.Windows.Forms.Button()
        Me.btnRemove = New C1.Win.C1Input.C1Button()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.pnlAdd = New System.Windows.Forms.Panel()
        Me.txtFldCode = New System.Windows.Forms.TextBox()
        Me.lblFldCode = New System.Windows.Forms.Label()
        Me.lblFldCaption = New System.Windows.Forms.Label()
        Me.txtFldCaption = New System.Windows.Forms.TextBox()
        Me.pnlbtns = New System.Windows.Forms.Panel()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlAdd.SuspendLayout()
        Me.pnlbtns.SuspendLayout()
        Me.SuspendLayout()
        '
        'vsfSearch
        '
        Me.vsfSearch.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfSearch.AllowFiltering = True
        Me.vsfSearch.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfSearch.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfSearch.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfSearch.AutoResize = True
        Me.vsfSearch.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfSearch.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfSearch.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfSearch.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfSearch.ExtendLastCol = True
        Me.vsfSearch.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfSearch.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfSearch.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfSearch.Location = New System.Drawing.Point(11, 16)
        Me.vsfSearch.Name = "vsfSearch"
        Me.vsfSearch.Rows.Count = 1
        Me.vsfSearch.Rows.DefaultSize = 24
        Me.vsfSearch.Rows.Fixed = 0
        Me.vsfSearch.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.vsfSearch.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfSearch.ShowCellLabels = True
        Me.vsfSearch.Size = New System.Drawing.Size(453, 25)
        Me.vsfSearch.StyleInfo = resources.GetString("vsfSearch.StyleInfo")
        Me.vsfSearch.TabIndex = 22
        Me.vsfSearch.Tag = ""
        Me.vsfSearch.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.System
        '
        'btnAdd
        '
        Me.btnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAdd.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_NEW
        Me.btnAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAdd.Location = New System.Drawing.Point(11, 393)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(27, 31)
        Me.btnAdd.TabIndex = 80
        Me.btnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnAdd, "Add : Alt + A")
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Location = New System.Drawing.Point(9, 3)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(82, 30)
        Me.btnSave.TabIndex = 81
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Shortcut : Alt + S")
        Me.btnSave.UseVisualStyleBackColor = True
        Me.btnSave.Visible = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnCancel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(97, 2)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(88, 30)
        Me.btnCancel.TabIndex = 82
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Shortcut : Alt + C")
        Me.btnCancel.UseVisualStyleBackColor = True
        Me.btnCancel.Visible = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(192, 2)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(86, 30)
        Me.btnClose.TabIndex = 220
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close : Alt + O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnPnlClose
        '
        Me.btnPnlClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPnlClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnPnlClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnPnlClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPnlClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPnlClose.Location = New System.Drawing.Point(357, 142)
        Me.btnPnlClose.Name = "btnPnlClose"
        Me.btnPnlClose.Size = New System.Drawing.Size(88, 31)
        Me.btnPnlClose.TabIndex = 84
        Me.btnPnlClose.Text = "Cl&ose"
        Me.btnPnlClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnPnlClose, "Shortcut : Alt + O")
        Me.btnPnlClose.UseVisualStyleBackColor = True
        '
        'btnPnlSave
        '
        Me.btnPnlSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPnlSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnPnlSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnPnlSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPnlSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPnlSave.Location = New System.Drawing.Point(269, 143)
        Me.btnPnlSave.Name = "btnPnlSave"
        Me.btnPnlSave.Size = New System.Drawing.Size(82, 31)
        Me.btnPnlSave.TabIndex = 83
        Me.btnPnlSave.Text = "&Save"
        Me.btnPnlSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnPnlSave, "Shortcut : Alt + S")
        Me.btnPnlSave.UseVisualStyleBackColor = True
        '
        'btnRemove
        '
        Me.btnRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRemove.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnRemove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRemove.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRemove.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRemove.Location = New System.Drawing.Point(45, 393)
        Me.btnRemove.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(28, 31)
        Me.btnRemove.TabIndex = 219
        Me.btnRemove.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnRemove, "Remove : Alt + R")
        Me.btnRemove.UseVisualStyleBackColor = False
        '
        'vsfDet
        '
        Me.vsfDet.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfDet.Location = New System.Drawing.Point(11, 36)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.Count = 1
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(453, 352)
        Me.vsfDet.TabIndex = 83
        Me.vsfDet.Tag = ""
        '
        'pnlAdd
        '
        Me.pnlAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlAdd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlAdd.Controls.Add(Me.txtFldCode)
        Me.pnlAdd.Controls.Add(Me.lblFldCode)
        Me.pnlAdd.Controls.Add(Me.lblFldCaption)
        Me.pnlAdd.Controls.Add(Me.txtFldCaption)
        Me.pnlAdd.Controls.Add(Me.btnPnlClose)
        Me.pnlAdd.Controls.Add(Me.btnPnlSave)
        Me.pnlAdd.Location = New System.Drawing.Point(11, 97)
        Me.pnlAdd.Name = "pnlAdd"
        Me.pnlAdd.Size = New System.Drawing.Size(453, 180)
        Me.pnlAdd.TabIndex = 221
        '
        'txtFldCode
        '
        Me.txtFldCode.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFldCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFldCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFldCode.Location = New System.Drawing.Point(121, 32)
        Me.txtFldCode.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtFldCode.MaxLength = 100
        Me.txtFldCode.Name = "txtFldCode"
        Me.txtFldCode.Size = New System.Drawing.Size(324, 26)
        Me.txtFldCode.TabIndex = 270
        '
        'lblFldCode
        '
        Me.lblFldCode.AutoSize = True
        Me.lblFldCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFldCode.ForeColor = System.Drawing.Color.Red
        Me.lblFldCode.Location = New System.Drawing.Point(13, 35)
        Me.lblFldCode.Name = "lblFldCode"
        Me.lblFldCode.Size = New System.Drawing.Size(85, 20)
        Me.lblFldCode.TabIndex = 269
        Me.lblFldCode.Tag = "M"
        Me.lblFldCode.Text = "Field Code"
        '
        'lblFldCaption
        '
        Me.lblFldCaption.AutoSize = True
        Me.lblFldCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFldCaption.ForeColor = System.Drawing.Color.Red
        Me.lblFldCaption.Location = New System.Drawing.Point(13, 83)
        Me.lblFldCaption.Name = "lblFldCaption"
        Me.lblFldCaption.Size = New System.Drawing.Size(102, 20)
        Me.lblFldCaption.TabIndex = 268
        Me.lblFldCaption.Tag = "M"
        Me.lblFldCaption.Text = "Field Caption"
        '
        'txtFldCaption
        '
        Me.txtFldCaption.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFldCaption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFldCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFldCaption.Location = New System.Drawing.Point(121, 80)
        Me.txtFldCaption.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtFldCaption.MaxLength = 100
        Me.txtFldCaption.Name = "txtFldCaption"
        Me.txtFldCaption.Size = New System.Drawing.Size(324, 26)
        Me.txtFldCaption.TabIndex = 271
        '
        'pnlbtns
        '
        Me.pnlbtns.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlbtns.Controls.Add(Me.btnClose)
        Me.pnlbtns.Controls.Add(Me.btnSave)
        Me.pnlbtns.Controls.Add(Me.btnCancel)
        Me.pnlbtns.Location = New System.Drawing.Point(185, 393)
        Me.pnlbtns.Name = "pnlbtns"
        Me.pnlbtns.Size = New System.Drawing.Size(284, 34)
        Me.pnlbtns.TabIndex = 271
        '
        'frmUtlFldCaptions
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(476, 431)
        Me.Controls.Add(Me.pnlbtns)
        Me.Controls.Add(Me.pnlAdd)
        Me.Controls.Add(Me.btnRemove)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.vsfSearch)
        Me.KeyPreview = True
        Me.Name = "frmUtlFldCaptions"
        Me.ShowIcon = False
        Me.Text = "Field Captions"
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlAdd.ResumeLayout(False)
        Me.pnlAdd.PerformLayout()
        Me.pnlbtns.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents vsfSearch As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents btnAdd As Button
    Friend WithEvents btnSave As Button
    Friend WithEvents btnCancel As Button
    Private WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnRemove As C1.Win.C1Input.C1Button
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents pnlAdd As Panel
    Friend WithEvents btnPnlClose As Button
    Friend WithEvents btnPnlSave As Button
    Friend WithEvents txtFldCode As TextBox
    Friend WithEvents lblFldCode As Label
    Friend WithEvents lblFldCaption As Label
    Friend WithEvents txtFldCaption As TextBox
    Friend WithEvents pnlbtns As Panel
End Class
