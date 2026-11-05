<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRptQuery
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRptQuery))
        Me.vsfQuery = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.txtQuery = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.btnRemoveQuery = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.txtTableName = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cboGrp = New System.Windows.Forms.ComboBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnResult = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.Cancel = New System.Windows.Forms.Button()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.btnLoad = New System.Windows.Forms.Button()
        CType(Me.vsfQuery, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'vsfQuery
        '
        Me.vsfQuery.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfQuery.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfQuery.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfQuery.ColumnInfo = "2,0,0,0,0,110,Columns:"
        Me.vsfQuery.ExtendLastCol = True
        Me.vsfQuery.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.vsfQuery.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfQuery.Location = New System.Drawing.Point(4, 19)
        Me.vsfQuery.Name = "vsfQuery"
        Me.vsfQuery.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.vsfQuery.Rows.Count = 4
        Me.vsfQuery.Rows.DefaultSize = 22
        Me.vsfQuery.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfQuery.Size = New System.Drawing.Size(215, 462)
        Me.vsfQuery.StyleInfo = resources.GetString("vsfQuery.StyleInfo")
        Me.vsfQuery.TabIndex = 230
        Me.vsfQuery.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2010Silver
        '
        'txtQuery
        '
        Me.txtQuery.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtQuery.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtQuery.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtQuery.Location = New System.Drawing.Point(232, 80)
        Me.txtQuery.Multiline = True
        Me.txtQuery.Name = "txtQuery"
        Me.txtQuery.Size = New System.Drawing.Size(392, 399)
        Me.txtQuery.TabIndex = 2
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.btnRemoveQuery)
        Me.GroupBox1.Controls.Add(Me.vsfQuery)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(2, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(226, 510)
        Me.GroupBox1.TabIndex = 237
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Query List"
        '
        'btnRemoveQuery
        '
        Me.btnRemoveQuery.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRemoveQuery.ForeColor = System.Drawing.Color.Black
        Me.btnRemoveQuery.Image = CType(resources.GetObject("btnRemoveQuery.Image"), System.Drawing.Image)
        Me.btnRemoveQuery.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRemoveQuery.Location = New System.Drawing.Point(185, 484)
        Me.btnRemoveQuery.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnRemoveQuery.Name = "btnRemoveQuery"
        Me.btnRemoveQuery.Size = New System.Drawing.Size(34, 23)
        Me.btnRemoveQuery.TabIndex = 288
        Me.btnRemoveQuery.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnRemoveQuery, "Remove Row")
        Me.btnRemoveQuery.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.vsfDet)
        Me.GroupBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(629, 5)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(235, 505)
        Me.GroupBox2.TabIndex = 238
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Table List"
        '
        'vsfDet
        '
        Me.vsfDet.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "2,0,0,0,0,110,Columns:"
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfDet.Location = New System.Drawing.Point(6, 20)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.vsfDet.Rows.Count = 4
        Me.vsfDet.Rows.DefaultSize = 22
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(224, 479)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 230
        Me.vsfDet.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2010Silver
        '
        'txtTableName
        '
        Me.txtTableName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTableName.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtTableName.Location = New System.Drawing.Point(312, 49)
        Me.txtTableName.MaxLength = 50
        Me.txtTableName.Name = "txtTableName"
        Me.txtTableName.Size = New System.Drawing.Size(230, 25)
        Me.txtTableName.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Maroon
        Me.Label1.Location = New System.Drawing.Point(230, 55)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(66, 13)
        Me.Label1.TabIndex = 241
        Me.Label1.Tag = "M"
        Me.Label1.Text = "Query Name"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 6)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(399, 13)
        Me.Label2.TabIndex = 242
        Me.Label2.Text = "For Strings use single quotation marks. Eg:  Select * from MstItem where status =" &
    " 'Y'"
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Location = New System.Drawing.Point(6, 522)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(403, 68)
        Me.Panel1.TabIndex = 243
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(3, 42)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(317, 13)
        Me.Label4.TabIndex = 244
        Me.Label4.Text = "For Numbers Eg:  Select partNo from MstItem where minStock = 0"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(3, 24)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(328, 13)
        Me.Label3.TabIndex = 243
        Me.Label3.Text = "For Dates Eg:  Select * from MstItem where DocDate = '01 jan 2001'"
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.Color.Blue
        Me.Label5.Location = New System.Drawing.Point(629, 522)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(219, 13)
        Me.Label5.TabIndex = 244
        Me.Label5.Tag = "R"
        Me.Label5.Text = "Select && Right Click To Get The Table Name"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Black
        Me.Label6.Location = New System.Drawing.Point(230, 26)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(67, 13)
        Me.Label6.TabIndex = 246
        Me.Label6.Tag = "M"
        Me.Label6.Text = "Group Name"
        '
        'cboGrp
        '
        Me.cboGrp.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.cboGrp.FormattingEnabled = True
        Me.cboGrp.Location = New System.Drawing.Point(312, 20)
        Me.cboGrp.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.cboGrp.MaxLength = 50
        Me.cboGrp.Name = "cboGrp"
        Me.cboGrp.Size = New System.Drawing.Size(231, 25)
        Me.cboGrp.TabIndex = 0
        '
        'btnNew
        '
        Me.btnNew.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnNew.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_NEW
        Me.btnNew.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNew.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnNew.Location = New System.Drawing.Point(272, 484)
        Me.btnNew.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(72, 29)
        Me.btnNew.TabIndex = 247
        Me.btnNew.Text = "&New"
        Me.btnNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnNew, "Shortcut: Alt+N")
        Me.btnNew.UseVisualStyleBackColor = False
        '
        'btnResult
        '
        Me.btnResult.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnResult.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnResult.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnResult.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnResult.Location = New System.Drawing.Point(426, 484)
        Me.btnResult.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnResult.Name = "btnResult"
        Me.btnResult.Size = New System.Drawing.Size(81, 29)
        Me.btnResult.TabIndex = 248
        Me.btnResult.Text = "&View Result"
        Me.ToolTip1.SetToolTip(Me.btnResult, "Shortcut: Alt+V")
        Me.btnResult.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(704, 562)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(72, 29)
        Me.btnSave.TabIndex = 249
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Shortcut: Alt+S")
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(782, 561)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(72, 29)
        Me.btnClose.TabIndex = 250
        Me.btnClose.Text = "C&lose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Shortcut: Alt+L")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'Cancel
        '
        Me.Cancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Cancel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.Cancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Cancel.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Cancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Cancel.Location = New System.Drawing.Point(512, 484)
        Me.Cancel.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Cancel.Name = "Cancel"
        Me.Cancel.Size = New System.Drawing.Size(72, 29)
        Me.Cancel.TabIndex = 252
        Me.Cancel.Text = "&Cancel"
        Me.Cancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.Cancel, "Shortcut: Alt+C")
        Me.Cancel.UseVisualStyleBackColor = False
        '
        'btnEdit
        '
        Me.btnEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnEdit.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_EDIT
        Me.btnEdit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEdit.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnEdit.Location = New System.Drawing.Point(349, 484)
        Me.btnEdit.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(72, 29)
        Me.btnEdit.TabIndex = 253
        Me.btnEdit.Text = "Edit"
        Me.btnEdit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnEdit, "Shortcut: Alt+N")
        Me.btnEdit.UseVisualStyleBackColor = False
        '
        'btnLoad
        '
        Me.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLoad.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoad.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnLoad.Location = New System.Drawing.Point(552, 21)
        Me.btnLoad.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(70, 25)
        Me.btnLoad.TabIndex = 251
        Me.btnLoad.Text = "&Load"
        Me.btnLoad.UseVisualStyleBackColor = False
        '
        'frmRptQuery
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(866, 594)
        Me.Controls.Add(Me.btnEdit)
        Me.Controls.Add(Me.Cancel)
        Me.Controls.Add(Me.btnLoad)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnResult)
        Me.Controls.Add(Me.btnNew)
        Me.Controls.Add(Me.cboGrp)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtTableName)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.txtQuery)
        Me.KeyPreview = True
        Me.Name = "frmRptQuery"
        Me.ShowIcon = False
        Me.Text = "Query Report"
        CType(Me.vsfQuery, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents vsfQuery As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents txtQuery As TextBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents txtTableName As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents btnRemoveQuery As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents cboGrp As ComboBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents btnNew As Button
    Friend WithEvents btnResult As Button
    Friend WithEvents btnSave As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents btnLoad As Button
    Friend WithEvents Cancel As Button
    Friend WithEvents btnEdit As Button
End Class
