<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmUtlPrintSettings
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmUtlPrintSettings))
        Me.vsfDtl = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.cboTrnName = New System.Windows.Forms.ComboBox()
        Me.PnlPrntSetting = New System.Windows.Forms.Panel()
        Me.lblTrnName = New System.Windows.Forms.Label()
        Me.BtnLoad = New System.Windows.Forms.Button()
        Me.PnlArrow = New System.Windows.Forms.Panel()
        Me.btnMvUP = New System.Windows.Forms.Button()
        Me.btnMvDwn = New System.Windows.Forms.Button()
        Me.pnlEdit = New System.Windows.Forms.Panel()
        Me.btnDelete = New C1.Win.C1Input.C1Button()
        Me.btnEdit = New C1.Win.C1Input.C1Button()
        Me.pnlSav = New System.Windows.Forms.Panel()
        Me.btnCancel = New C1.Win.C1Input.C1Button()
        Me.btnSave = New C1.Win.C1Input.C1Button()
        Me.btnAlter = New System.Windows.Forms.Button()
        Me.btnAdd = New C1.Win.C1Input.C1Button()
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.vsfDtl, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PnlPrntSetting.SuspendLayout()
        Me.PnlArrow.SuspendLayout()
        Me.pnlEdit.SuspendLayout()
        Me.pnlSav.SuspendLayout()
        Me.SuspendLayout()
        '
        'vsfDtl
        '
        Me.vsfDtl.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfDtl.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.vsfDtl.AutoResize = True
        Me.vsfDtl.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDtl.ColumnInfo = "10,0,0,0,0,125,Columns:"
        Me.vsfDtl.ExtendLastCol = True
        Me.vsfDtl.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.vsfDtl.Location = New System.Drawing.Point(45, 52)
        Me.vsfDtl.Margin = New System.Windows.Forms.Padding(8)
        Me.vsfDtl.Name = "vsfDtl"
        Me.vsfDtl.Rows.DefaultSize = 25
        Me.vsfDtl.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.RowRange
        Me.vsfDtl.Size = New System.Drawing.Size(635, 317)
        Me.vsfDtl.StyleInfo = resources.GetString("vsfDtl.StyleInfo")
        Me.vsfDtl.TabIndex = 2
        '
        'cboTrnName
        '
        Me.cboTrnName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboTrnName.CausesValidation = False
        Me.cboTrnName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTrnName.FormattingEnabled = True
        Me.cboTrnName.Location = New System.Drawing.Point(145, 11)
        Me.cboTrnName.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cboTrnName.Name = "cboTrnName"
        Me.cboTrnName.Size = New System.Drawing.Size(432, 28)
        Me.cboTrnName.TabIndex = 0
        '
        'PnlPrntSetting
        '
        Me.PnlPrntSetting.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.PnlPrntSetting.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PnlPrntSetting.Controls.Add(Me.lblTrnName)
        Me.PnlPrntSetting.Controls.Add(Me.BtnLoad)
        Me.PnlPrntSetting.Controls.Add(Me.PnlArrow)
        Me.PnlPrntSetting.Controls.Add(Me.pnlEdit)
        Me.PnlPrntSetting.Controls.Add(Me.pnlSav)
        Me.PnlPrntSetting.Controls.Add(Me.btnAlter)
        Me.PnlPrntSetting.Controls.Add(Me.btnAdd)
        Me.PnlPrntSetting.Controls.Add(Me.cboTrnName)
        Me.PnlPrntSetting.Controls.Add(Me.vsfDtl)
        Me.PnlPrntSetting.Location = New System.Drawing.Point(10, 16)
        Me.PnlPrntSetting.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.PnlPrntSetting.Name = "PnlPrntSetting"
        Me.PnlPrntSetting.Size = New System.Drawing.Size(689, 421)
        Me.PnlPrntSetting.TabIndex = 233
        '
        'lblTrnName
        '
        Me.lblTrnName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTrnName.AutoSize = True
        Me.lblTrnName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTrnName.ForeColor = System.Drawing.Color.Red
        Me.lblTrnName.Location = New System.Drawing.Point(40, 14)
        Me.lblTrnName.Name = "lblTrnName"
        Me.lblTrnName.Size = New System.Drawing.Size(92, 20)
        Me.lblTrnName.TabIndex = 250
        Me.lblTrnName.Tag = "M"
        Me.lblTrnName.Text = "Transaction"
        '
        'BtnLoad
        '
        Me.BtnLoad.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnLoad.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnLoad.Location = New System.Drawing.Point(585, 8)
        Me.BtnLoad.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.BtnLoad.Name = "BtnLoad"
        Me.BtnLoad.Size = New System.Drawing.Size(95, 32)
        Me.BtnLoad.TabIndex = 1
        Me.BtnLoad.Text = "&Load"
        Me.ToolTip1.SetToolTip(Me.BtnLoad, "Load : Alt + L")
        Me.BtnLoad.UseVisualStyleBackColor = True
        '
        'PnlArrow
        '
        Me.PnlArrow.Controls.Add(Me.btnMvUP)
        Me.PnlArrow.Controls.Add(Me.btnMvDwn)
        Me.PnlArrow.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PnlArrow.Location = New System.Drawing.Point(1, 51)
        Me.PnlArrow.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.PnlArrow.Name = "PnlArrow"
        Me.PnlArrow.Size = New System.Drawing.Size(45, 318)
        Me.PnlArrow.TabIndex = 248
        Me.PnlArrow.Visible = False
        '
        'btnMvUP
        '
        Me.btnMvUP.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.btnMvUP.ForeColor = System.Drawing.Color.Black
        Me.btnMvUP.Image = CType(resources.GetObject("btnMvUP.Image"), System.Drawing.Image)
        Me.btnMvUP.Location = New System.Drawing.Point(4, 1)
        Me.btnMvUP.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnMvUP.Name = "btnMvUP"
        Me.btnMvUP.Size = New System.Drawing.Size(39, 159)
        Me.btnMvUP.TabIndex = 9
        Me.btnMvUP.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnMvUP.UseVisualStyleBackColor = False
        '
        'btnMvDwn
        '
        Me.btnMvDwn.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.btnMvDwn.ForeColor = System.Drawing.Color.Black
        Me.btnMvDwn.Image = CType(resources.GetObject("btnMvDwn.Image"), System.Drawing.Image)
        Me.btnMvDwn.Location = New System.Drawing.Point(4, 160)
        Me.btnMvDwn.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnMvDwn.Name = "btnMvDwn"
        Me.btnMvDwn.Size = New System.Drawing.Size(39, 158)
        Me.btnMvDwn.TabIndex = 10
        Me.btnMvDwn.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnMvDwn.UseVisualStyleBackColor = False
        '
        'pnlEdit
        '
        Me.pnlEdit.Controls.Add(Me.btnDelete)
        Me.pnlEdit.Controls.Add(Me.btnEdit)
        Me.pnlEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlEdit.Location = New System.Drawing.Point(255, 376)
        Me.pnlEdit.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.pnlEdit.Name = "pnlEdit"
        Me.pnlEdit.Size = New System.Drawing.Size(210, 43)
        Me.pnlEdit.TabIndex = 248
        Me.pnlEdit.Visible = False
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDelete.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_DELETE
        Me.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDelete.Location = New System.Drawing.Point(106, 5)
        Me.btnDelete.Margin = New System.Windows.Forms.Padding(6)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(95, 32)
        Me.btnDelete.TabIndex = 1
        Me.btnDelete.Text = "&Delete "
        Me.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnDelete, "Delete : Alt + D")
        Me.btnDelete.UseVisualStyleBackColor = False
        '
        'btnEdit
        '
        Me.btnEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEdit.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_EDIT
        Me.btnEdit.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnEdit.Location = New System.Drawing.Point(5, 5)
        Me.btnEdit.Margin = New System.Windows.Forms.Padding(6)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(95, 32)
        Me.btnEdit.TabIndex = 0
        Me.btnEdit.Text = "&Edit"
        Me.btnEdit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnEdit, "Edit : Alt + E")
        Me.btnEdit.UseVisualStyleBackColor = False
        '
        'pnlSav
        '
        Me.pnlSav.Controls.Add(Me.btnCancel)
        Me.pnlSav.Controls.Add(Me.btnSave)
        Me.pnlSav.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlSav.Location = New System.Drawing.Point(481, 376)
        Me.pnlSav.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.pnlSav.Name = "pnlSav"
        Me.pnlSav.Size = New System.Drawing.Size(207, 42)
        Me.pnlSav.TabIndex = 247
        Me.pnlSav.Visible = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(105, 4)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(6)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(95, 32)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Cancel : Alt + C")
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(5, 4)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(6)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(95, 32)
        Me.btnSave.TabIndex = 0
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Save : Alt + S")
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnAlter
        '
        Me.btnAlter.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAlter.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnAlter.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAlter.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAlter.Location = New System.Drawing.Point(45, 382)
        Me.btnAlter.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnAlter.Name = "btnAlter"
        Me.btnAlter.Size = New System.Drawing.Size(95, 31)
        Me.btnAlter.TabIndex = 2
        Me.btnAlter.Text = "Al&ter Order"
        Me.ToolTip1.SetToolTip(Me.btnAlter, "Alter Order : Alt + T")
        Me.btnAlter.UseVisualStyleBackColor = True
        Me.btnAlter.Visible = False
        '
        'btnAdd
        '
        Me.btnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAdd.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_NEW
        Me.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAdd.Location = New System.Drawing.Point(150, 381)
        Me.btnAdd.Margin = New System.Windows.Forms.Padding(6)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnAdd.Size = New System.Drawing.Size(95, 32)
        Me.btnAdd.TabIndex = 3
        Me.btnAdd.Text = "&Add"
        Me.btnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnAdd, "Add : Alt + A")
        Me.btnAdd.UseVisualStyleBackColor = False
        Me.btnAdd.Visible = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.CausesValidation = False
        Me.btnClose.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(604, 441)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(6)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(95, 32)
        Me.btnClose.TabIndex = 0
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close : Alt + O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'FrmUtlPrintSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(708, 475)
        Me.Controls.Add(Me.PnlPrntSetting)
        Me.Controls.Add(Me.btnClose)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmUtlPrintSettings"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Print Settings"
        CType(Me.vsfDtl, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PnlPrntSetting.ResumeLayout(False)
        Me.PnlPrntSetting.PerformLayout()
        Me.PnlArrow.ResumeLayout(False)
        Me.pnlEdit.ResumeLayout(False)
        Me.pnlSav.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents vsfDtl As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents cboTrnName As ComboBox
    Friend WithEvents btnCancel As C1.Win.C1Input.C1Button
    Friend WithEvents btnSave As C1.Win.C1Input.C1Button
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents btnDelete As C1.Win.C1Input.C1Button
    Friend WithEvents btnAdd As C1.Win.C1Input.C1Button
    Friend WithEvents btnEdit As C1.Win.C1Input.C1Button
    Friend WithEvents PnlPrntSetting As Panel
    Friend WithEvents btnMvDwn As Button
    Friend WithEvents btnMvUP As Button
    Friend WithEvents btnAlter As Button
    Friend WithEvents pnlEdit As Panel
    Friend WithEvents pnlSav As Panel
    Friend WithEvents PnlArrow As Panel
    Friend WithEvents BtnLoad As Button
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents lblTrnName As Label
End Class
