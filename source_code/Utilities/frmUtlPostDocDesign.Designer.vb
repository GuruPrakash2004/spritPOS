<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlPostDocDesign
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
        Me.components = New System.ComponentModel.Container()
        Me.lblName = New System.Windows.Forms.Label()
        Me.lblRemaks = New System.Windows.Forms.Label()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.txtRmks = New System.Windows.Forms.TextBox()
        Me.lblCondition = New System.Windows.Forms.Label()
        Me.txtCond = New System.Windows.Forms.TextBox()
        Me.txtPendCond = New System.Windows.Forms.TextBox()
        Me.lblPendingCondition = New System.Windows.Forms.Label()
        Me.lblPrimaryCol = New System.Windows.Forms.Label()
        Me.vsfDetail = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfPK = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnSave = New C1.Win.C1Input.C1Button()
        Me.btnCancel = New C1.Win.C1Input.C1Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.lblRptColumn = New System.Windows.Forms.Label()
        Me.UcBtnPk = New vLQMS.ucBtn()
        Me.UcBtnDet = New vLQMS.ucBtn()
        CType(Me.vsfDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfPK, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblName.ForeColor = System.Drawing.Color.Red
        Me.lblName.Location = New System.Drawing.Point(6, 27)
        Me.lblName.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(51, 20)
        Me.lblName.TabIndex = 0
        Me.lblName.Tag = "M"
        Me.lblName.Text = "Name"
        '
        'lblRemaks
        '
        Me.lblRemaks.AutoSize = True
        Me.lblRemaks.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRemaks.Location = New System.Drawing.Point(6, 64)
        Me.lblRemaks.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblRemaks.Name = "lblRemaks"
        Me.lblRemaks.Size = New System.Drawing.Size(73, 20)
        Me.lblRemaks.TabIndex = 1
        Me.lblRemaks.Text = "Remarks"
        '
        'txtName
        '
        Me.txtName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtName.Location = New System.Drawing.Point(96, 23)
        Me.txtName.Margin = New System.Windows.Forms.Padding(2)
        Me.txtName.MaxLength = 50
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(346, 26)
        Me.txtName.TabIndex = 2
        '
        'txtRmks
        '
        Me.txtRmks.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRmks.Location = New System.Drawing.Point(96, 57)
        Me.txtRmks.Margin = New System.Windows.Forms.Padding(2)
        Me.txtRmks.MaxLength = 500
        Me.txtRmks.Multiline = True
        Me.txtRmks.Name = "txtRmks"
        Me.txtRmks.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtRmks.Size = New System.Drawing.Size(347, 49)
        Me.txtRmks.TabIndex = 3
        '
        'lblCondition
        '
        Me.lblCondition.AutoSize = True
        Me.lblCondition.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCondition.ForeColor = System.Drawing.Color.Red
        Me.lblCondition.Location = New System.Drawing.Point(5, 115)
        Me.lblCondition.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblCondition.Name = "lblCondition"
        Me.lblCondition.Size = New System.Drawing.Size(76, 20)
        Me.lblCondition.TabIndex = 5
        Me.lblCondition.Tag = "M"
        Me.lblCondition.Text = "Condition"
        '
        'txtCond
        '
        Me.txtCond.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCond.Location = New System.Drawing.Point(8, 138)
        Me.txtCond.Margin = New System.Windows.Forms.Padding(2)
        Me.txtCond.MaxLength = 500
        Me.txtCond.Multiline = True
        Me.txtCond.Name = "txtCond"
        Me.txtCond.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtCond.Size = New System.Drawing.Size(435, 60)
        Me.txtCond.TabIndex = 6
        '
        'txtPendCond
        '
        Me.txtPendCond.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPendCond.Location = New System.Drawing.Point(8, 226)
        Me.txtPendCond.Margin = New System.Windows.Forms.Padding(2)
        Me.txtPendCond.MaxLength = 500
        Me.txtPendCond.Multiline = True
        Me.txtPendCond.Name = "txtPendCond"
        Me.txtPendCond.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtPendCond.Size = New System.Drawing.Size(434, 60)
        Me.txtPendCond.TabIndex = 7
        '
        'lblPendingCondition
        '
        Me.lblPendingCondition.AutoSize = True
        Me.lblPendingCondition.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPendingCondition.ForeColor = System.Drawing.Color.Red
        Me.lblPendingCondition.Location = New System.Drawing.Point(6, 204)
        Me.lblPendingCondition.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblPendingCondition.Name = "lblPendingCondition"
        Me.lblPendingCondition.Size = New System.Drawing.Size(138, 20)
        Me.lblPendingCondition.TabIndex = 8
        Me.lblPendingCondition.Tag = "M"
        Me.lblPendingCondition.Text = "Pending Condition"
        '
        'lblPrimaryCol
        '
        Me.lblPrimaryCol.AutoSize = True
        Me.lblPrimaryCol.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPrimaryCol.Location = New System.Drawing.Point(466, 7)
        Me.lblPrimaryCol.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblPrimaryCol.Name = "lblPrimaryCol"
        Me.lblPrimaryCol.Size = New System.Drawing.Size(127, 20)
        Me.lblPrimaryCol.TabIndex = 10
        Me.lblPrimaryCol.Text = "Primary Columns"
        '
        'vsfDetail
        '
        Me.vsfDetail.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDetail.ColumnInfo = "10,1,0,0,0,105,Columns:"
        Me.vsfDetail.ExtendLastCol = True
        Me.vsfDetail.Location = New System.Drawing.Point(8, 291)
        Me.vsfDetail.Margin = New System.Windows.Forms.Padding(2)
        Me.vsfDetail.Name = "vsfDetail"
        Me.vsfDetail.Rows.DefaultSize = 21
        Me.vsfDetail.Size = New System.Drawing.Size(770, 163)
        Me.vsfDetail.TabIndex = 11
        '
        'vsfPK
        '
        Me.vsfPK.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfPK.ColumnInfo = "10,1,0,0,0,105,Columns:"
        Me.vsfPK.ExtendLastCol = True
        Me.vsfPK.Location = New System.Drawing.Point(470, 30)
        Me.vsfPK.Margin = New System.Windows.Forms.Padding(2)
        Me.vsfPK.Name = "vsfPK"
        Me.vsfPK.Rows.DefaultSize = 21
        Me.vsfPK.Size = New System.Drawing.Size(308, 229)
        Me.vsfPK.TabIndex = 12
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(595, 459)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(83, 31)
        Me.btnSave.TabIndex = 17
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Save : Alt+ S")
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
        Me.btnCancel.Location = New System.Drawing.Point(686, 459)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(90, 31)
        Me.btnCancel.TabIndex = 18
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Cancel : Alt + C")
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'lblRptColumn
        '
        Me.lblRptColumn.AutoSize = True
        Me.lblRptColumn.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRptColumn.Location = New System.Drawing.Point(6, 308)
        Me.lblRptColumn.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblRptColumn.Name = "lblRptColumn"
        Me.lblRptColumn.Size = New System.Drawing.Size(124, 20)
        Me.lblRptColumn.TabIndex = 32
        Me.lblRptColumn.Text = "Report Columns"
        Me.lblRptColumn.Visible = False
        '
        'UcBtnPk
        '
        Me.UcBtnPk.Location = New System.Drawing.Point(470, 264)
        Me.UcBtnPk.Name = "UcBtnPk"
        Me.UcBtnPk.Size = New System.Drawing.Size(308, 22)
        Me.UcBtnPk.TabIndex = 31
        '
        'UcBtnDet
        '
        Me.UcBtnDet.Location = New System.Drawing.Point(8, 459)
        Me.UcBtnDet.Name = "UcBtnDet"
        Me.UcBtnDet.Size = New System.Drawing.Size(442, 22)
        Me.UcBtnDet.TabIndex = 30
        '
        'frmUtlPostDocDesign
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(786, 496)
        Me.Controls.Add(Me.UcBtnPk)
        Me.Controls.Add(Me.UcBtnDet)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.vsfPK)
        Me.Controls.Add(Me.vsfDetail)
        Me.Controls.Add(Me.lblPrimaryCol)
        Me.Controls.Add(Me.lblPendingCondition)
        Me.Controls.Add(Me.txtPendCond)
        Me.Controls.Add(Me.txtCond)
        Me.Controls.Add(Me.lblCondition)
        Me.Controls.Add(Me.txtRmks)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.lblRemaks)
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.lblRptColumn)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(802, 534)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(802, 534)
        Me.Name = "frmUtlPostDocDesign"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Post Document Designer"
        CType(Me.vsfDetail, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfPK, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblName As Label
    Friend WithEvents lblRemaks As Label
    Friend WithEvents txtName As TextBox
    Friend WithEvents txtRmks As TextBox
    Friend WithEvents lblCondition As Label
    Friend WithEvents txtCond As TextBox
    Friend WithEvents txtPendCond As TextBox
    Friend WithEvents lblPendingCondition As Label
    Friend WithEvents lblPrimaryCol As Label
    Friend WithEvents btnSave As C1.Win.C1Input.C1Button
    Friend WithEvents btnCancel As C1.Win.C1Input.C1Button
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents UcBtnDet As ucBtn
    Friend WithEvents UcBtnPk As ucBtn
    Private WithEvents vsfDetail As C1.Win.C1FlexGrid.C1FlexGrid
    Private WithEvents vsfPK As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents lblRptColumn As Label
End Class
