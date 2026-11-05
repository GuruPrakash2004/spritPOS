<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRptCRFldSel
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRptCRFldSel))
        Me.vsfFrom = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfTo = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnRemove = New System.Windows.Forms.Button()
        Me.btnNext = New System.Windows.Forms.Button()
        Me.txtRptID = New System.Windows.Forms.TextBox()
        Me.vsfTbls = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.txtHdrTblName = New System.Windows.Forms.TextBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnAddAll = New System.Windows.Forms.Button()
        Me.btnRemAll = New System.Windows.Forms.Button()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.vsfFrom, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfTo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfTbls, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'vsfFrom
        '
        Me.vsfFrom.AllowEditing = False
        Me.vsfFrom.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
        Me.vsfFrom.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfFrom.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfFrom.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfFrom.ColumnInfo = "2,0,0,0,0,95,Columns:"
        Me.vsfFrom.ExtendLastCol = True
        Me.vsfFrom.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Solid
        Me.vsfFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.vsfFrom.Location = New System.Drawing.Point(37, 112)
        Me.vsfFrom.Name = "vsfFrom"
        Me.vsfFrom.Rows.Count = 4
        Me.vsfFrom.Rows.DefaultSize = 19
        Me.vsfFrom.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfFrom.Size = New System.Drawing.Size(334, 364)
        Me.vsfFrom.StyleInfo = resources.GetString("vsfFrom.StyleInfo")
        Me.vsfFrom.TabIndex = 197
        Me.vsfFrom.Tag = "DSGR"
        '
        'vsfTo
        '
        Me.vsfTo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfTo.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfTo.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfTo.ColumnInfo = "2,0,0,0,0,95,Columns:"
        Me.vsfTo.ExtendLastCol = True
        Me.vsfTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.vsfTo.Location = New System.Drawing.Point(545, 112)
        Me.vsfTo.Name = "vsfTo"
        Me.vsfTo.Rows.Count = 4
        Me.vsfTo.Rows.DefaultSize = 19
        Me.vsfTo.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfTo.Size = New System.Drawing.Size(283, 364)
        Me.vsfTo.StyleInfo = resources.GetString("vsfTo.StyleInfo")
        Me.vsfTo.TabIndex = 198
        Me.vsfTo.Tag = "DSGR"
        '
        'btnAdd
        '
        Me.btnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAdd.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnAdd.Location = New System.Drawing.Point(393, 120)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(132, 29)
        Me.btnAdd.TabIndex = 199
        Me.btnAdd.Text = "Add-->"
        Me.btnAdd.UseVisualStyleBackColor = False
        '
        'btnRemove
        '
        Me.btnRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRemove.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnRemove.Location = New System.Drawing.Point(393, 176)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(132, 29)
        Me.btnRemove.TabIndex = 200
        Me.btnRemove.Text = "<--Remove"
        Me.btnRemove.UseVisualStyleBackColor = False
        '
        'btnNext
        '
        Me.btnNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNext.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnNext.Image = CType(resources.GetObject("btnNext.Image"), System.Drawing.Image)
        Me.btnNext.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnNext.Location = New System.Drawing.Point(651, 6)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(64, 29)
        Me.btnNext.TabIndex = 203
        Me.btnNext.Text = "&Next"
        Me.btnNext.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnNext, "Next:Alt+N")
        Me.btnNext.UseVisualStyleBackColor = False
        '
        'txtRptID
        '
        Me.txtRptID.Location = New System.Drawing.Point(270, 256)
        Me.txtRptID.Name = "txtRptID"
        Me.txtRptID.Size = New System.Drawing.Size(59, 20)
        Me.txtRptID.TabIndex = 205
        Me.txtRptID.Visible = False
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
        Me.vsfTbls.ColumnInfo = "1,0,0,0,0,95,Columns:"
        Me.vsfTbls.ExtendLastCol = True
        Me.vsfTbls.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Solid
        Me.vsfTbls.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.vsfTbls.Location = New System.Drawing.Point(411, 236)
        Me.vsfTbls.Name = "vsfTbls"
        Me.vsfTbls.Rows.Count = 4
        Me.vsfTbls.Rows.DefaultSize = 19
        Me.vsfTbls.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfTbls.Size = New System.Drawing.Size(107, 101)
        Me.vsfTbls.StyleInfo = resources.GetString("vsfTbls.StyleInfo")
        Me.vsfTbls.TabIndex = 206
        Me.vsfTbls.Tag = "DSGR"
        Me.vsfTbls.Visible = False
        '
        'txtHdrTblName
        '
        Me.txtHdrTblName.Location = New System.Drawing.Point(261, 230)
        Me.txtHdrTblName.Name = "txtHdrTblName"
        Me.txtHdrTblName.Size = New System.Drawing.Size(77, 20)
        Me.txtHdrTblName.TabIndex = 218
        Me.txtHdrTblName.Visible = False
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.Silver
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PictureBox1.Location = New System.Drawing.Point(37, 3)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(793, 51)
        Me.PictureBox1.TabIndex = 219
        Me.PictureBox1.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.White
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(247, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(372, 25)
        Me.Label1.TabIndex = 220
        Me.Label1.Text = "Specify the data that you want to collect."
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label2.ForeColor = System.Drawing.Color.Blue
        Me.Label2.Location = New System.Drawing.Point(84, 62)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(269, 25)
        Me.Label2.TabIndex = 221
        Me.Label2.Text = "Choose the fields that you want to include in your report"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(34, 87)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(96, 17)
        Me.Label3.TabIndex = 222
        Me.Label3.Text = "Fields available"
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(542, 87)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(157, 17)
        Me.Label4.TabIndex = 223
        Me.Label4.Text = "Fields to Include in report"
        '
        'btnAddAll
        '
        Me.btnAddAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAddAll.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnAddAll.Location = New System.Drawing.Point(393, 376)
        Me.btnAddAll.Name = "btnAddAll"
        Me.btnAddAll.Size = New System.Drawing.Size(132, 29)
        Me.btnAddAll.TabIndex = 224
        Me.btnAddAll.Text = "Add All -->>"
        Me.btnAddAll.UseVisualStyleBackColor = False
        '
        'btnRemAll
        '
        Me.btnRemAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRemAll.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnRemAll.Location = New System.Drawing.Point(393, 427)
        Me.btnRemAll.Name = "btnRemAll"
        Me.btnRemAll.Size = New System.Drawing.Size(132, 29)
        Me.btnRemAll.TabIndex = 225
        Me.btnRemAll.Text = "<<--Remove All"
        Me.btnRemAll.UseVisualStyleBackColor = False
        '
        'btnBack
        '
        Me.btnBack.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBack.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnBack.Image = CType(resources.GetObject("btnBack.Image"), System.Drawing.Image)
        Me.btnBack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnBack.Location = New System.Drawing.Point(579, 6)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(67, 29)
        Me.btnBack.TabIndex = 226
        Me.btnBack.Text = "&Back"
        Me.btnBack.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnBack, "Back:Alt+B")
        Me.btnBack.UseVisualStyleBackColor = False
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.btnClose)
        Me.Panel1.Controls.Add(Me.btnCancel)
        Me.Panel1.Controls.Add(Me.btnNext)
        Me.Panel1.Controls.Add(Me.btnBack)
        Me.Panel1.Location = New System.Drawing.Point(37, 488)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(791, 42)
        Me.Panel1.TabIndex = 227
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnClose.Image = CType(resources.GetObject("btnClose.Image"), System.Drawing.Image)
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(719, 6)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(67, 29)
        Me.btnClose.TabIndex = 228
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close:Alt+O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnCancel.Image = CType(resources.GetObject("btnCancel.Image"), System.Drawing.Image)
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(1, 6)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 29)
        Me.btnCancel.TabIndex = 227
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Cancel:Alt+C")
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'frmRptCRFldSel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(861, 536)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.btnRemAll)
        Me.Controls.Add(Me.btnAddAll)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.txtHdrTblName)
        Me.Controls.Add(Me.vsfTbls)
        Me.Controls.Add(Me.txtRptID)
        Me.Controls.Add(Me.btnRemove)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.vsfTo)
        Me.Controls.Add(Me.vsfFrom)
        Me.MinimumSize = New System.Drawing.Size(877, 574)
        Me.Name = "frmRptCRFldSel"
        Me.ShowIcon = False
        Me.Text = "Field Selection"
        CType(Me.vsfFrom, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfTo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfTbls, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents vsfFrom As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents vsfTo As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnRemove As System.Windows.Forms.Button
    Friend WithEvents btnNext As System.Windows.Forms.Button
    Friend WithEvents txtRptID As System.Windows.Forms.TextBox
    Friend WithEvents vsfTbls As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents txtHdrTblName As System.Windows.Forms.TextBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnAddAll As System.Windows.Forms.Button
    Friend WithEvents btnRemAll As System.Windows.Forms.Button
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnClose As Button
    Friend WithEvents ToolTip1 As ToolTip
End Class
