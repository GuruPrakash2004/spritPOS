<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlBulkItemUpdate
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlBulkItemUpdate))
        Me.chkSelIG = New System.Windows.Forms.CheckBox()
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.lblCount = New System.Windows.Forms.Label()
        Me.btnClrGridFilters = New System.Windows.Forms.Button()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.chkSelItem = New System.Windows.Forms.CheckBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.txtCondition = New System.Windows.Forms.TextBox()
        Me.chkCondition = New System.Windows.Forms.CheckBox()
        Me.txtReplTo = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtReplFrom = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblSelected = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfSearch = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.tt = New System.Windows.Forms.ToolTip(Me.components)
        Me.pnlHeader.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'chkSelIG
        '
        Me.chkSelIG.AutoSize = True
        Me.chkSelIG.Font = New System.Drawing.Font("Consolas", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSelIG.Location = New System.Drawing.Point(20, 44)
        Me.chkSelIG.Margin = New System.Windows.Forms.Padding(4)
        Me.chkSelIG.Name = "chkSelIG"
        Me.chkSelIG.Size = New System.Drawing.Size(226, 23)
        Me.chkSelIG.TabIndex = 1
        Me.chkSelIG.Text = "Selected Item Group(s)"
        Me.chkSelIG.UseVisualStyleBackColor = True
        '
        'pnlHeader
        '
        Me.pnlHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlHeader.Controls.Add(Me.lblCount)
        Me.pnlHeader.Controls.Add(Me.btnClrGridFilters)
        Me.pnlHeader.Controls.Add(Me.btnLoad)
        Me.pnlHeader.Controls.Add(Me.chkSelItem)
        Me.pnlHeader.Controls.Add(Me.chkSelIG)
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Location = New System.Drawing.Point(10, 10)
        Me.pnlHeader.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(1078, 78)
        Me.pnlHeader.TabIndex = 2
        '
        'lblCount
        '
        Me.lblCount.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCount.AutoSize = True
        Me.lblCount.Font = New System.Drawing.Font("Consolas", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCount.Location = New System.Drawing.Point(637, 48)
        Me.lblCount.Name = "lblCount"
        Me.lblCount.Size = New System.Drawing.Size(162, 19)
        Me.lblCount.TabIndex = 7
        Me.lblCount.Text = "No. Of Items: 0/0"
        '
        'btnClrGridFilters
        '
        Me.btnClrGridFilters.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClrGridFilters.Font = New System.Drawing.Font("Consolas", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClrGridFilters.Location = New System.Drawing.Point(870, 28)
        Me.btnClrGridFilters.Name = "btnClrGridFilters"
        Me.btnClrGridFilters.Size = New System.Drawing.Size(193, 39)
        Me.btnClrGridFilters.TabIndex = 6
        Me.btnClrGridFilters.Text = "Clear Grid Filters"
        Me.btnClrGridFilters.UseVisualStyleBackColor = True
        '
        'btnLoad
        '
        Me.btnLoad.Font = New System.Drawing.Font("Consolas", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoad.Location = New System.Drawing.Point(278, 10)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(237, 57)
        Me.btnLoad.TabIndex = 5
        Me.btnLoad.Text = "Load Item Master Data"
        Me.btnLoad.UseVisualStyleBackColor = True
        '
        'chkSelItem
        '
        Me.chkSelItem.AutoSize = True
        Me.chkSelItem.Font = New System.Drawing.Font("Consolas", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSelItem.Location = New System.Drawing.Point(20, 13)
        Me.chkSelItem.Margin = New System.Windows.Forms.Padding(4)
        Me.chkSelItem.Name = "chkSelItem"
        Me.chkSelItem.Size = New System.Drawing.Size(172, 23)
        Me.chkSelItem.TabIndex = 1
        Me.chkSelItem.Text = "Selected Item(s)"
        Me.chkSelItem.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(10, 88)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1078, 10)
        Me.Panel1.TabIndex = 8
        '
        'Panel3
        '
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel3.Location = New System.Drawing.Point(10, 579)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1078, 10)
        Me.Panel3.TabIndex = 0
        '
        'Panel4
        '
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Controls.Add(Me.btnCancel)
        Me.Panel4.Controls.Add(Me.btnUpdate)
        Me.Panel4.Controls.Add(Me.btnClear)
        Me.Panel4.Controls.Add(Me.txtCondition)
        Me.Panel4.Controls.Add(Me.chkCondition)
        Me.Panel4.Controls.Add(Me.txtReplTo)
        Me.Panel4.Controls.Add(Me.Label2)
        Me.Panel4.Controls.Add(Me.txtReplFrom)
        Me.Panel4.Controls.Add(Me.Label1)
        Me.Panel4.Controls.Add(Me.lblSelected)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel4.Location = New System.Drawing.Point(10, 589)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(1078, 141)
        Me.Panel4.TabIndex = 0
        '
        'btnCancel
        '
        Me.btnCancel.BackgroundImage = Global.vICAxon.My.Resources.Resources.T3_CANCEL
        Me.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Location = New System.Drawing.Point(718, 104)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(108, 31)
        Me.btnCancel.TabIndex = 17
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnUpdate
        '
        Me.btnUpdate.Enabled = False
        Me.btnUpdate.Location = New System.Drawing.Point(637, 104)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(75, 31)
        Me.btnUpdate.TabIndex = 16
        Me.btnUpdate.Text = "&Update"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(224, 104)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(75, 31)
        Me.btnClear.TabIndex = 15
        Me.btnClear.Text = "Clea&r"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'txtCondition
        '
        Me.txtCondition.Enabled = False
        Me.txtCondition.Location = New System.Drawing.Point(224, 72)
        Me.txtCondition.Name = "txtCondition"
        Me.txtCondition.Size = New System.Drawing.Size(602, 26)
        Me.txtCondition.TabIndex = 14
        '
        'chkCondition
        '
        Me.chkCondition.AutoSize = True
        Me.chkCondition.Font = New System.Drawing.Font("Consolas", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCondition.Location = New System.Drawing.Point(108, 73)
        Me.chkCondition.Margin = New System.Windows.Forms.Padding(4)
        Me.chkCondition.Name = "chkCondition"
        Me.chkCondition.Size = New System.Drawing.Size(109, 23)
        Me.chkCondition.TabIndex = 13
        Me.chkCondition.Text = "Condition"
        Me.chkCondition.UseVisualStyleBackColor = True
        '
        'txtReplTo
        '
        Me.txtReplTo.Location = New System.Drawing.Point(506, 40)
        Me.txtReplTo.Name = "txtReplTo"
        Me.txtReplTo.Size = New System.Drawing.Size(222, 26)
        Me.txtReplTo.TabIndex = 12
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Consolas", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(455, 43)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(45, 19)
        Me.Label2.TabIndex = 11
        Me.Label2.Text = "with"
        '
        'txtReplFrom
        '
        Me.txtReplFrom.Location = New System.Drawing.Point(224, 40)
        Me.txtReplFrom.Name = "txtReplFrom"
        Me.txtReplFrom.ReadOnly = True
        Me.txtReplFrom.Size = New System.Drawing.Size(225, 26)
        Me.txtReplFrom.TabIndex = 10
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Consolas", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(104, 43)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 19)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Replace"
        '
        'lblSelected
        '
        Me.lblSelected.AutoSize = True
        Me.lblSelected.Font = New System.Drawing.Font("Consolas", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSelected.Location = New System.Drawing.Point(16, 11)
        Me.lblSelected.Name = "lblSelected"
        Me.lblSelected.Size = New System.Drawing.Size(252, 19)
        Me.lblSelected.TabIndex = 8
        Me.lblSelected.Text = "For the 0 selected items..."
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.vsfDet)
        Me.Panel2.Controls.Add(Me.vsfSearch)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(10, 98)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1078, 481)
        Me.Panel2.TabIndex = 9
        '
        'vsfDet
        '
        Me.vsfDet.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfDet.Dock = System.Windows.Forms.DockStyle.Fill
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcrossOut
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcrossOut
        Me.vsfDet.Location = New System.Drawing.Point(0, 29)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.Count = 1
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.RowRange
        Me.vsfDet.Size = New System.Drawing.Size(1076, 450)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 14
        '
        'vsfSearch
        '
        Me.vsfSearch.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.vsfSearch.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfSearch.Dock = System.Windows.Forms.DockStyle.Top
        Me.vsfSearch.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfSearch.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcrossOut
        Me.vsfSearch.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcrossOut
        Me.vsfSearch.Location = New System.Drawing.Point(0, 0)
        Me.vsfSearch.Name = "vsfSearch"
        Me.vsfSearch.Rows.Count = 1
        Me.vsfSearch.Rows.DefaultSize = 24
        Me.vsfSearch.Rows.Fixed = 0
        Me.vsfSearch.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.vsfSearch.Size = New System.Drawing.Size(1076, 29)
        Me.vsfSearch.StyleInfo = resources.GetString("vsfSearch.StyleInfo")
        Me.vsfSearch.TabIndex = 13
        '
        'frmUtlBulkItemUpdate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1098, 740)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.pnlHeader)
        Me.Font = New System.Drawing.Font("Consolas", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmUtlBulkItemUpdate"
        Me.Padding = New System.Windows.Forms.Padding(10)
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Bulk Updation of Item Master..."
        Me.pnlHeader.ResumeLayout(False)
        Me.pnlHeader.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents chkSelIG As System.Windows.Forms.CheckBox
    Friend WithEvents pnlHeader As System.Windows.Forms.Panel
    Friend WithEvents chkSelItem As System.Windows.Forms.CheckBox
    Friend WithEvents btnLoad As System.Windows.Forms.Button
    Friend WithEvents btnClrGridFilters As System.Windows.Forms.Button
    Friend WithEvents lblCount As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Public WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Public WithEvents vsfSearch As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents lblSelected As System.Windows.Forms.Label
    Friend WithEvents txtReplTo As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtReplFrom As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkCondition As System.Windows.Forms.CheckBox
    Friend WithEvents txtCondition As System.Windows.Forms.TextBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents tt As System.Windows.Forms.ToolTip
End Class
