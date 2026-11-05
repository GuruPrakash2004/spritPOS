<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMstOPBalEntry
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMstOPBalEntry))
        Me.fraMain = New System.Windows.Forms.Panel()
        Me.fraSearch = New System.Windows.Forms.Panel()
        Me.vsfView = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfSearch = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnCloseSearch = New System.Windows.Forms.Button()
        Me.txtLocID = New System.Windows.Forms.TextBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnSave = New C1.Win.C1Input.C1Button()
        Me.vsfEdit = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.pnlCtrls = New System.Windows.Forms.Panel()
        Me.btnAdd = New C1.Win.C1Input.C1Button()
        Me.btnExportToXL = New C1.Win.C1Input.C1Button()
        Me.btnRemoveRow = New C1.Win.C1Input.C1Button()
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtRowCount = New System.Windows.Forms.TextBox()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfOptions = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfSearchDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblFrm = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.fraMain.SuspendLayout()
        Me.fraSearch.SuspendLayout()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlCtrls.SuspendLayout()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfOptions, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfSearchDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'fraMain
        '
        Me.fraMain.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fraMain.Controls.Add(Me.fraSearch)
        Me.fraMain.Controls.Add(Me.txtLocID)
        Me.fraMain.Controls.Add(Me.btnCancel)
        Me.fraMain.Controls.Add(Me.btnSave)
        Me.fraMain.Controls.Add(Me.vsfEdit)
        Me.fraMain.Location = New System.Drawing.Point(0, 34)
        Me.fraMain.Name = "fraMain"
        Me.fraMain.Size = New System.Drawing.Size(897, 246)
        Me.fraMain.TabIndex = 1
        '
        'fraSearch
        '
        Me.fraSearch.Controls.Add(Me.vsfView)
        Me.fraSearch.Controls.Add(Me.vsfSearch)
        Me.fraSearch.Controls.Add(Me.btnCloseSearch)
        Me.fraSearch.Location = New System.Drawing.Point(199, 62)
        Me.fraSearch.Name = "fraSearch"
        Me.fraSearch.Size = New System.Drawing.Size(465, 196)
        Me.fraSearch.TabIndex = 217
        Me.fraSearch.Visible = False
        '
        'vsfView
        '
        Me.vsfView.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfView.AutoResize = True
        Me.vsfView.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfView.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfView.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfView.ExtendLastCol = True
        Me.vsfView.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfView.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfView.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
        Me.vsfView.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfView.Location = New System.Drawing.Point(2, 24)
        Me.vsfView.Name = "vsfView"
        Me.vsfView.Rows.DefaultSize = 24
        Me.vsfView.Rows.GlyphRow = 0
        Me.vsfView.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfView.ShowCellLabels = True
        Me.vsfView.Size = New System.Drawing.Size(463, 169)
        Me.vsfView.StyleInfo = resources.GetString("vsfView.StyleInfo")
        Me.vsfView.TabIndex = 1
        Me.vsfView.Tag = ""
        Me.vsfView.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2007Blue
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
        Me.vsfSearch.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfSearch.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfSearch.Location = New System.Drawing.Point(2, 1)
        Me.vsfSearch.Name = "vsfSearch"
        Me.vsfSearch.Rows.Count = 1
        Me.vsfSearch.Rows.DefaultSize = 24
        Me.vsfSearch.Rows.Fixed = 0
        Me.vsfSearch.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.vsfSearch.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfSearch.ShowCellLabels = True
        Me.vsfSearch.Size = New System.Drawing.Size(463, 25)
        Me.vsfSearch.StyleInfo = resources.GetString("vsfSearch.StyleInfo")
        Me.vsfSearch.TabIndex = 22
        Me.vsfSearch.Tag = ""
        Me.vsfSearch.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.System
        '
        'btnCloseSearch
        '
        Me.btnCloseSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCloseSearch.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnCloseSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnCloseSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCloseSearch.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnCloseSearch.Image = CType(resources.GetObject("btnCloseSearch.Image"), System.Drawing.Image)
        Me.btnCloseSearch.Location = New System.Drawing.Point(440, 4)
        Me.btnCloseSearch.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnCloseSearch.Name = "btnCloseSearch"
        Me.btnCloseSearch.Size = New System.Drawing.Size(27, 22)
        Me.btnCloseSearch.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.btnCloseSearch, "Click to close search window")
        Me.btnCloseSearch.UseVisualStyleBackColor = False
        '
        'txtLocID
        '
        Me.txtLocID.Location = New System.Drawing.Point(764, 46)
        Me.txtLocID.Name = "txtLocID"
        Me.txtLocID.Size = New System.Drawing.Size(100, 20)
        Me.txtLocID.TabIndex = 1
        Me.txtLocID.Visible = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.btnCancel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(811, 198)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(83, 29)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Shortcut: Alt + C")
        Me.btnCancel.UseVisualStyleBackColor = False
        Me.btnCancel.Visible = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(723, 198)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(83, 29)
        Me.btnSave.TabIndex = 2
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Shortcut: Alt + S")
        Me.btnSave.UseVisualStyleBackColor = False
        Me.btnSave.Visible = False
        '
        'vsfEdit
        '
        Me.vsfEdit.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfEdit.AllowFiltering = True
        Me.vsfEdit.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfEdit.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfEdit.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfEdit.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfEdit.AutoResize = True
        Me.vsfEdit.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfEdit.ColumnInfo = "2,1,0,0,0,120,Columns:"
        Me.vsfEdit.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfEdit.ExtendLastCol = True
        Me.vsfEdit.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfEdit.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfEdit.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
        Me.vsfEdit.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcrossOut
        Me.vsfEdit.Location = New System.Drawing.Point(2, 7)
        Me.vsfEdit.Name = "vsfEdit"
        Me.vsfEdit.Rows.DefaultSize = 24
        Me.vsfEdit.Rows.Fixed = 0
        Me.vsfEdit.Rows.GlyphRow = 0
        Me.vsfEdit.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfEdit.ShowCellLabels = True
        Me.vsfEdit.Size = New System.Drawing.Size(714, 239)
        Me.vsfEdit.StyleInfo = resources.GetString("vsfEdit.StyleInfo")
        Me.vsfEdit.TabIndex = 0
        Me.vsfEdit.Tag = ""
        Me.vsfEdit.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2007Blue
        '
        'pnlCtrls
        '
        Me.pnlCtrls.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlCtrls.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlCtrls.Controls.Add(Me.btnAdd)
        Me.pnlCtrls.Controls.Add(Me.btnExportToXL)
        Me.pnlCtrls.Controls.Add(Me.btnRemoveRow)
        Me.pnlCtrls.Controls.Add(Me.btnClose)
        Me.pnlCtrls.Controls.Add(Me.Label1)
        Me.pnlCtrls.Controls.Add(Me.txtRowCount)
        Me.pnlCtrls.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlCtrls.Location = New System.Drawing.Point(3, 380)
        Me.pnlCtrls.Name = "pnlCtrls"
        Me.pnlCtrls.Size = New System.Drawing.Size(893, 59)
        Me.pnlCtrls.TabIndex = 212
        '
        'btnAdd
        '
        Me.btnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAdd.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_NEW
        Me.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAdd.Location = New System.Drawing.Point(526, 12)
        Me.btnAdd.Margin = New System.Windows.Forms.Padding(4)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnAdd.Size = New System.Drawing.Size(81, 29)
        Me.btnAdd.TabIndex = 1
        Me.btnAdd.Text = "&Add"
        Me.btnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnExportToXL
        '
        Me.btnExportToXL.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExportToXL.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExportToXL.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExportToXL.Image = CType(resources.GetObject("btnExportToXL.Image"), System.Drawing.Image)
        Me.btnExportToXL.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExportToXL.Location = New System.Drawing.Point(757, 12)
        Me.btnExportToXL.Name = "btnExportToXL"
        Me.btnExportToXL.Size = New System.Drawing.Size(34, 29)
        Me.btnExportToXL.TabIndex = 3
        Me.btnExportToXL.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnExportToXL.UseVisualStyleBackColor = False
        '
        'btnRemoveRow
        '
        Me.btnRemoveRow.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRemoveRow.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRemoveRow.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRemoveRow.Image = CType(resources.GetObject("btnRemoveRow.Image"), System.Drawing.Image)
        Me.btnRemoveRow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRemoveRow.Location = New System.Drawing.Point(615, 12)
        Me.btnRemoveRow.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRemoveRow.Name = "btnRemoveRow"
        Me.btnRemoveRow.Size = New System.Drawing.Size(134, 29)
        Me.btnRemoveRow.TabIndex = 2
        Me.btnRemoveRow.Text = "&Remove Row"
        Me.btnRemoveRow.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRemoveRow.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(799, 12)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(81, 29)
        Me.btnClose.TabIndex = 4
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(13, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(106, 20)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "No. of Rows"
        '
        'txtRowCount
        '
        Me.txtRowCount.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtRowCount.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRowCount.Location = New System.Drawing.Point(125, 19)
        Me.txtRowCount.Name = "txtRowCount"
        Me.txtRowCount.ReadOnly = True
        Me.txtRowCount.Size = New System.Drawing.Size(50, 26)
        Me.txtRowCount.TabIndex = 0
        Me.txtRowCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'vsfDet
        '
        Me.vsfDet.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcrossOut
        Me.vsfDet.Location = New System.Drawing.Point(1, 306)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.Count = 6
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.Size = New System.Drawing.Size(889, 69)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 1
        Me.vsfDet.Tag = "DSGR"
        '
        'vsfOptions
        '
        Me.vsfOptions.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfOptions.ColumnInfo = "2,1,0,0,0,120,Columns:"
        Me.vsfOptions.ExtendLastCol = True
        Me.vsfOptions.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfOptions.Location = New System.Drawing.Point(499, 298)
        Me.vsfOptions.Name = "vsfOptions"
        Me.vsfOptions.Rows.Count = 6
        Me.vsfOptions.Rows.DefaultSize = 24
        Me.vsfOptions.Rows.Fixed = 0
        Me.vsfOptions.Size = New System.Drawing.Size(242, 46)
        Me.vsfOptions.StyleInfo = resources.GetString("vsfOptions.StyleInfo")
        Me.vsfOptions.TabIndex = 218
        Me.vsfOptions.Tag = "DSGR"
        Me.vsfOptions.Visible = False
        '
        'vsfSearchDet
        '
        Me.vsfSearchDet.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfSearchDet.AllowFiltering = True
        Me.vsfSearchDet.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfSearchDet.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfSearchDet.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfSearchDet.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfSearchDet.AutoResize = True
        Me.vsfSearchDet.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfSearchDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfSearchDet.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfSearchDet.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfSearchDet.ExtendLastCol = True
        Me.vsfSearchDet.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfSearchDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfSearchDet.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
        Me.vsfSearchDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfSearchDet.Location = New System.Drawing.Point(2, 281)
        Me.vsfSearchDet.Name = "vsfSearchDet"
        Me.vsfSearchDet.Rows.Count = 1
        Me.vsfSearchDet.Rows.DefaultSize = 24
        Me.vsfSearchDet.Rows.Fixed = 0
        Me.vsfSearchDet.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.vsfSearchDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfSearchDet.ShowCellLabels = True
        Me.vsfSearchDet.Size = New System.Drawing.Size(888, 25)
        Me.vsfSearchDet.StyleInfo = resources.GetString("vsfSearchDet.StyleInfo")
        Me.vsfSearchDet.TabIndex = 0
        Me.vsfSearchDet.Tag = ""
        Me.vsfSearchDet.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.System
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(53, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(106, Byte), Integer))
        Me.Panel1.Controls.Add(Me.lblFrm)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel1.ForeColor = System.Drawing.Color.WhiteSmoke
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(897, 35)
        Me.Panel1.TabIndex = 219
        '
        'lblFrm
        '
        Me.lblFrm.AutoSize = True
        Me.lblFrm.BackColor = System.Drawing.Color.Transparent
        Me.lblFrm.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFrm.ForeColor = System.Drawing.Color.White
        Me.lblFrm.Location = New System.Drawing.Point(3, 6)
        Me.lblFrm.Name = "lblFrm"
        Me.lblFrm.Size = New System.Drawing.Size(343, 25)
        Me.lblFrm.TabIndex = 0
        Me.lblFrm.Text = "Item Wise Opening Balance Updation"
        '
        'frmMstOPBalEntry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(897, 443)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.vsfSearchDet)
        Me.Controls.Add(Me.fraMain)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.pnlCtrls)
        Me.Controls.Add(Me.vsfOptions)
        Me.MinimumSize = New System.Drawing.Size(879, 479)
        Me.Name = "frmMstOPBalEntry"
        Me.ShowIcon = False
        Me.Text = "Item Opening Balance"
        Me.fraMain.ResumeLayout(False)
        Me.fraMain.PerformLayout()
        Me.fraSearch.ResumeLayout(False)
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfEdit, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlCtrls.ResumeLayout(False)
        Me.pnlCtrls.PerformLayout()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfOptions, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfSearchDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents fraMain As System.Windows.Forms.Panel
    Friend WithEvents pnlCtrls As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtRowCount As System.Windows.Forms.TextBox
    Friend WithEvents fraSearch As System.Windows.Forms.Panel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lblFrm As System.Windows.Forms.Label
    Friend WithEvents btnSave As C1.Win.C1Input.C1Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents btnExportToXL As C1.Win.C1Input.C1Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents btnRemoveRow As C1.Win.C1Input.C1Button
    Friend WithEvents btnCloseSearch As System.Windows.Forms.Button
    Private WithEvents vsfEdit As C1.Win.C1FlexGrid.C1FlexGrid
    Private WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Private WithEvents vsfView As C1.Win.C1FlexGrid.C1FlexGrid
    Private WithEvents vsfSearch As C1.Win.C1FlexGrid.C1FlexGrid
    Private WithEvents vsfOptions As C1.Win.C1FlexGrid.C1FlexGrid
    Private WithEvents vsfSearchDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnAdd As C1.Win.C1Input.C1Button
    Friend WithEvents txtLocID As TextBox
End Class
