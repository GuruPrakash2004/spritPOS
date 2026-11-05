<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPartyItem
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPartyItem))
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.txtPartyID = New System.Windows.Forms.TextBox()
        Me.txtPartyCode = New System.Windows.Forms.TextBox()
        Me.btSearch = New System.Windows.Forms.Button()
        Me.btnTransactions = New System.Windows.Forms.Button()
        Me.lblPartyName2 = New System.Windows.Forms.Label()
        Me.lblPartyName = New System.Windows.Forms.Label()
        Me.lblPartyCode = New System.Windows.Forms.Label()
        Me.btLoad = New System.Windows.Forms.Button()
        Me.btnCancel = New C1.Win.C1Input.C1Button()
        Me.btnImpXL = New System.Windows.Forms.Button()
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.btnSave = New C1.Win.C1Input.C1Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnExport = New C1.Win.C1Input.C1Button()
        Me.btnRemoveRow = New C1.Win.C1Input.C1Button()
        Me.vsfView = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.vsfSearch = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfOptions = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.gbMemorandum = New System.Windows.Forms.GroupBox()
        Me.vsfMem = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.txtDummy = New System.Windows.Forms.TextBox()
        Me.Panel2.SuspendLayout()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfOptions, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbMemorandum.SuspendLayout()
        CType(Me.vsfMem, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel2.BackColor = System.Drawing.Color.Transparent
        Me.Panel2.Controls.Add(Me.txtPartyID)
        Me.Panel2.Controls.Add(Me.txtPartyCode)
        Me.Panel2.Controls.Add(Me.btSearch)
        Me.Panel2.Controls.Add(Me.btnTransactions)
        Me.Panel2.Controls.Add(Me.lblPartyName2)
        Me.Panel2.Controls.Add(Me.lblPartyName)
        Me.Panel2.Controls.Add(Me.lblPartyCode)
        Me.Panel2.Controls.Add(Me.btLoad)
        Me.Panel2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel2.Location = New System.Drawing.Point(0, 35)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(956, 75)
        Me.Panel2.TabIndex = 0
        '
        'txtPartyID
        '
        Me.txtPartyID.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPartyID.Location = New System.Drawing.Point(519, 38)
        Me.txtPartyID.Name = "txtPartyID"
        Me.txtPartyID.Size = New System.Drawing.Size(19, 26)
        Me.txtPartyID.TabIndex = 5
        Me.txtPartyID.Visible = False
        '
        'txtPartyCode
        '
        Me.txtPartyCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPartyCode.Location = New System.Drawing.Point(116, 11)
        Me.txtPartyCode.Name = "txtPartyCode"
        Me.txtPartyCode.Size = New System.Drawing.Size(267, 26)
        Me.txtPartyCode.TabIndex = 0
        '
        'btSearch
        '
        Me.btSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btSearch.Location = New System.Drawing.Point(384, 11)
        Me.btSearch.Name = "btSearch"
        Me.btSearch.Size = New System.Drawing.Size(29, 26)
        Me.btSearch.TabIndex = 1
        Me.btSearch.Text = "..."
        Me.ToolTip1.SetToolTip(Me.btSearch, "Will Return the List of Supplier")
        Me.btSearch.UseVisualStyleBackColor = True
        '
        'btnTransactions
        '
        Me.btnTransactions.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTransactions.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTransactions.ForeColor = System.Drawing.Color.Black
        Me.btnTransactions.Location = New System.Drawing.Point(519, 9)
        Me.btnTransactions.Name = "btnTransactions"
        Me.btnTransactions.Size = New System.Drawing.Size(176, 30)
        Me.btnTransactions.TabIndex = 3
        Me.btnTransactions.Text = "Open Transaction"
        Me.btnTransactions.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ToolTip1.SetToolTip(Me.btnTransactions, "Shortcut: Alt + T")
        Me.btnTransactions.UseVisualStyleBackColor = True
        '
        'lblPartyName2
        '
        Me.lblPartyName2.BackColor = System.Drawing.Color.White
        Me.lblPartyName2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPartyName2.ForeColor = System.Drawing.Color.Black
        Me.lblPartyName2.Location = New System.Drawing.Point(117, 40)
        Me.lblPartyName2.Name = "lblPartyName2"
        Me.lblPartyName2.Size = New System.Drawing.Size(393, 23)
        Me.lblPartyName2.TabIndex = 4
        Me.lblPartyName2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblPartyName
        '
        Me.lblPartyName.AutoSize = True
        Me.lblPartyName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPartyName.ForeColor = System.Drawing.Color.Blue
        Me.lblPartyName.Location = New System.Drawing.Point(3, 39)
        Me.lblPartyName.Name = "lblPartyName"
        Me.lblPartyName.Size = New System.Drawing.Size(113, 20)
        Me.lblPartyName.TabIndex = 6
        Me.lblPartyName.Tag = "R"
        Me.lblPartyName.Text = "Supplier Name"
        '
        'lblPartyCode
        '
        Me.lblPartyCode.AutoSize = True
        Me.lblPartyCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPartyCode.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblPartyCode.Location = New System.Drawing.Point(3, 12)
        Me.lblPartyCode.Name = "lblPartyCode"
        Me.lblPartyCode.Size = New System.Drawing.Size(116, 20)
        Me.lblPartyCode.TabIndex = 6
        Me.lblPartyCode.Tag = "M"
        Me.lblPartyCode.Text = "Select Supplier"
        '
        'btLoad
        '
        Me.btLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btLoad.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btLoad.ForeColor = System.Drawing.Color.Black
        Me.btLoad.Location = New System.Drawing.Point(415, 11)
        Me.btLoad.Name = "btLoad"
        Me.btLoad.Size = New System.Drawing.Size(96, 26)
        Me.btLoad.TabIndex = 2
        Me.btLoad.Text = "&Load"
        Me.btLoad.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ToolTip1.SetToolTip(Me.btLoad, "Shortcut: Alt + L")
        Me.btLoad.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Image = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnCancel.Location = New System.Drawing.Point(764, 513)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(92, 30)
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Shortcut: Alt + O")
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnImpXL
        '
        Me.btnImpXL.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnImpXL.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnImpXL.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnImpXL.ForeColor = System.Drawing.Color.Black
        Me.btnImpXL.Location = New System.Drawing.Point(0, 514)
        Me.btnImpXL.Name = "btnImpXL"
        Me.btnImpXL.Size = New System.Drawing.Size(140, 29)
        Me.btnImpXL.TabIndex = 5
        Me.btnImpXL.Text = "Impor&t from XL"
        Me.ToolTip1.SetToolTip(Me.btnImpXL, "Shortcut: Alt + T")
        Me.btnImpXL.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Image = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.Location = New System.Drawing.Point(864, 513)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(92, 30)
        Me.btnClose.TabIndex = 8
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Shortcut: Alt + O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Image = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.Location = New System.Drawing.Point(664, 514)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(92, 30)
        Me.btnSave.TabIndex = 6
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Shortcut: Alt + S")
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnExport
        '
        Me.btnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExport.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExport.Image = CType(resources.GetObject("btnExport.Image"), System.Drawing.Image)
        Me.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExport.Location = New System.Drawing.Point(156, 512)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(33, 31)
        Me.btnExport.TabIndex = 4
        Me.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnExport, "Export to Excel : Alt + E")
        Me.btnExport.UseVisualStyleBackColor = False
        '
        'btnRemoveRow
        '
        Me.btnRemoveRow.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRemoveRow.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRemoveRow.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRemoveRow.Image = CType(resources.GetObject("btnRemoveRow.Image"), System.Drawing.Image)
        Me.btnRemoveRow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRemoveRow.Location = New System.Drawing.Point(208, 512)
        Me.btnRemoveRow.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRemoveRow.Name = "btnRemoveRow"
        Me.btnRemoveRow.Size = New System.Drawing.Size(30, 30)
        Me.btnRemoveRow.TabIndex = 3
        Me.btnRemoveRow.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnRemoveRow, "Remove Row : Alt + R")
        Me.btnRemoveRow.UseVisualStyleBackColor = False
        '
        'vsfView
        '
        Me.vsfView.AllowEditing = False
        Me.vsfView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfView.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfView.ExtendLastCol = True
        Me.vsfView.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfView.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
        Me.vsfView.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfView.Location = New System.Drawing.Point(47, 141)
        Me.vsfView.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfView.Name = "vsfView"
        Me.vsfView.Rows.DefaultSize = 24
        Me.vsfView.Size = New System.Drawing.Size(705, 199)
        Me.vsfView.StyleInfo = resources.GetString("vsfView.StyleInfo")
        Me.vsfView.TabIndex = 1
        Me.vsfView.Visible = False
        '
        'vsfDet
        '
        Me.vsfDet.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "2,1,0,0,0,120,Columns:"
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(0, 117)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(956, 287)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 2
        Me.vsfDet.Tag = ""
        '
        'lblTitle
        '
        Me.lblTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(53, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(106, Byte), Integer))
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.ForeColor = System.Drawing.Color.White
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(956, 30)
        Me.lblTitle.TabIndex = 12
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'vsfSearch
        '
        Me.vsfSearch.AllowEditing = False
        Me.vsfSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.vsfSearch.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfSearch.ExtendLastCol = True
        Me.vsfSearch.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfSearch.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
        Me.vsfSearch.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfSearch.Location = New System.Drawing.Point(116, 106)
        Me.vsfSearch.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfSearch.Name = "vsfSearch"
        Me.vsfSearch.Rows.DefaultSize = 24
        Me.vsfSearch.Size = New System.Drawing.Size(596, 267)
        Me.vsfSearch.StyleInfo = resources.GetString("vsfSearch.StyleInfo")
        Me.vsfSearch.TabIndex = 0
        Me.vsfSearch.Visible = False
        '
        'vsfOptions
        '
        Me.vsfOptions.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfOptions.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfOptions.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfOptions.ColumnInfo = "2,1,0,0,0,120,Columns:"
        Me.vsfOptions.ExtendLastCol = True
        Me.vsfOptions.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfOptions.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfOptions.Location = New System.Drawing.Point(288, 264)
        Me.vsfOptions.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfOptions.Name = "vsfOptions"
        Me.vsfOptions.Rows.DefaultSize = 24
        Me.vsfOptions.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfOptions.Size = New System.Drawing.Size(114, 76)
        Me.vsfOptions.StyleInfo = resources.GetString("vsfOptions.StyleInfo")
        Me.vsfOptions.TabIndex = 9
        Me.vsfOptions.Tag = ""
        Me.vsfOptions.Visible = False
        '
        'gbMemorandum
        '
        Me.gbMemorandum.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.gbMemorandum.Controls.Add(Me.vsfMem)
        Me.gbMemorandum.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbMemorandum.Location = New System.Drawing.Point(368, 411)
        Me.gbMemorandum.Name = "gbMemorandum"
        Me.gbMemorandum.Size = New System.Drawing.Size(222, 136)
        Me.gbMemorandum.TabIndex = 3
        Me.gbMemorandum.TabStop = False
        Me.gbMemorandum.Visible = False
        '
        'vsfMem
        '
        Me.vsfMem.AllowEditing = False
        Me.vsfMem.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.vsfMem.ColumnInfo = "10,1,0,0,0,120,Columns:"
        Me.vsfMem.ExtendLastCol = True
        Me.vsfMem.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfMem.Location = New System.Drawing.Point(331, 13)
        Me.vsfMem.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfMem.Name = "vsfMem"
        Me.vsfMem.Rows.DefaultSize = 24
        Me.vsfMem.Size = New System.Drawing.Size(159, 119)
        Me.vsfMem.StyleInfo = resources.GetString("vsfMem.StyleInfo")
        Me.vsfMem.TabIndex = 0
        '
        'txtDummy
        '
        Me.txtDummy.Location = New System.Drawing.Point(730, 194)
        Me.txtDummy.Name = "txtDummy"
        Me.txtDummy.Size = New System.Drawing.Size(100, 20)
        Me.txtDummy.TabIndex = 9
        Me.txtDummy.Visible = False
        '
        'frmPartyItem
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(956, 547)
        Me.Controls.Add(Me.btnRemoveRow)
        Me.Controls.Add(Me.gbMemorandum)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.vsfSearch)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.btnImpXL)
        Me.Controls.Add(Me.vsfView)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.vsfOptions)
        Me.Controls.Add(Me.txtDummy)
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(819, 447)
        Me.Name = "frmPartyItem"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Supplier Vs Item "
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfOptions, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbMemorandum.ResumeLayout(False)
        CType(Me.vsfMem, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents btLoad As System.Windows.Forms.Button
    Friend WithEvents lblPartyCode As System.Windows.Forms.Label
    Friend WithEvents btnTransactions As System.Windows.Forms.Button
    Friend WithEvents lblPartyName2 As System.Windows.Forms.Label
    Friend WithEvents btSearch As System.Windows.Forms.Button
    Friend WithEvents lblPartyName As System.Windows.Forms.Label
    Friend WithEvents btnSave As C1.Win.C1Input.C1Button
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents btnImpXL As System.Windows.Forms.Button
    Friend WithEvents vsfView As C1.Win.C1FlexGrid.C1FlexGrid
    Public WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Public WithEvents lblTitle As Label
    Friend WithEvents txtPartyCode As TextBox
    Friend WithEvents txtPartyID As TextBox
    Friend WithEvents vsfSearch As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnCancel As C1.Win.C1Input.C1Button
    Public WithEvents vsfOptions As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents gbMemorandum As GroupBox
    Friend WithEvents vsfMem As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents txtDummy As TextBox
    Friend WithEvents btnExport As C1.Win.C1Input.C1Button
    Friend WithEvents btnRemoveRow As C1.Win.C1Input.C1Button
End Class
