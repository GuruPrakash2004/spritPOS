<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlNotification
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlNotification))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnCancel = New C1.Win.C1Input.C1Button()
        Me.btnSend = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnDelAll = New System.Windows.Forms.Button()
        Me.btnExport = New C1.Win.C1Input.C1Button()
        Me.btnpnlClose = New System.Windows.Forms.Button()
        Me.btnRecvList = New System.Windows.Forms.Button()
        Me.btnSentList = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.pnlNotify = New System.Windows.Forms.Panel()
        Me.pnlContact = New System.Windows.Forms.Panel()
        Me.vsfSearch = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfContact = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.txtUsers = New System.Windows.Forms.TextBox()
        Me.lblUsers = New System.Windows.Forms.Label()
        Me.txtNotes = New System.Windows.Forms.TextBox()
        Me.lblNotes = New System.Windows.Forms.Label()
        Me.txtTitle = New System.Windows.Forms.TextBox()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.pnlList = New System.Windows.Forms.Panel()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfSearchNotif = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.pnlNotify.SuspendLayout()
        Me.pnlContact.SuspendLayout()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfContact, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlList.SuspendLayout()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfSearchNotif, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(709, 388)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(90, 30)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Cancel : Alt + C")
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnSend
        '
        Me.btnSend.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSend.BackColor = System.Drawing.Color.Transparent
        Me.btnSend.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSend.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnSend.ForeColor = System.Drawing.Color.Black
        Me.btnSend.Location = New System.Drawing.Point(580, 388)
        Me.btnSend.Name = "btnSend"
        Me.btnSend.Size = New System.Drawing.Size(122, 30)
        Me.btnSend.TabIndex = 3
        Me.btnSend.Text = "&Send Notification"
        Me.ToolTip1.SetToolTip(Me.btnSend, "Send Notification : Alt + S")
        Me.btnSend.UseVisualStyleBackColor = False
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDelete.BackColor = System.Drawing.Color.Transparent
        Me.btnDelete.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_DELETE
        Me.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDelete.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnDelete.ForeColor = System.Drawing.Color.Black
        Me.btnDelete.Location = New System.Drawing.Point(254, 401)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(67, 28)
        Me.btnDelete.TabIndex = 46
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnDelete, "Delete : Alt + D")
        Me.btnDelete.UseVisualStyleBackColor = False
        '
        'btnDelAll
        '
        Me.btnDelAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDelAll.BackColor = System.Drawing.Color.Transparent
        Me.btnDelAll.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnDelAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDelAll.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnDelAll.ForeColor = System.Drawing.Color.Black
        Me.btnDelAll.Location = New System.Drawing.Point(327, 401)
        Me.btnDelAll.Name = "btnDelAll"
        Me.btnDelAll.Size = New System.Drawing.Size(75, 28)
        Me.btnDelAll.TabIndex = 47
        Me.btnDelAll.Text = "Delete &All"
        Me.ToolTip1.SetToolTip(Me.btnDelAll, "Delete All : Alt + A")
        Me.btnDelAll.UseVisualStyleBackColor = False
        '
        'btnExport
        '
        Me.btnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExport.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnExport.Image = CType(resources.GetObject("btnExport.Image"), System.Drawing.Image)
        Me.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExport.Location = New System.Drawing.Point(408, 401)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(33, 28)
        Me.btnExport.TabIndex = 225
        Me.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnExport, "Export to Excel : Alt + F12")
        Me.btnExport.UseVisualStyleBackColor = False
        '
        'btnpnlClose
        '
        Me.btnpnlClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnpnlClose.BackColor = System.Drawing.Color.Transparent
        Me.btnpnlClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnpnlClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnpnlClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnpnlClose.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnpnlClose.ForeColor = System.Drawing.Color.Black
        Me.btnpnlClose.Location = New System.Drawing.Point(447, 401)
        Me.btnpnlClose.Name = "btnpnlClose"
        Me.btnpnlClose.Size = New System.Drawing.Size(67, 28)
        Me.btnpnlClose.TabIndex = 226
        Me.btnpnlClose.Text = "Cl&ose"
        Me.btnpnlClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnpnlClose, "Close : Alt + O")
        Me.btnpnlClose.UseVisualStyleBackColor = False
        '
        'btnRecvList
        '
        Me.btnRecvList.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRecvList.BackColor = System.Drawing.Color.Transparent
        Me.btnRecvList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnRecvList.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRecvList.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnRecvList.ForeColor = System.Drawing.Color.Black
        Me.btnRecvList.Location = New System.Drawing.Point(154, 401)
        Me.btnRecvList.Name = "btnRecvList"
        Me.btnRecvList.Size = New System.Drawing.Size(94, 28)
        Me.btnRecvList.TabIndex = 228
        Me.btnRecvList.Text = "&Received List"
        Me.ToolTip1.SetToolTip(Me.btnRecvList, "Received List : Alt + R")
        Me.btnRecvList.UseVisualStyleBackColor = False
        '
        'btnSentList
        '
        Me.btnSentList.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSentList.BackColor = System.Drawing.Color.Transparent
        Me.btnSentList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSentList.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSentList.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnSentList.ForeColor = System.Drawing.Color.Black
        Me.btnSentList.Location = New System.Drawing.Point(78, 401)
        Me.btnSentList.Name = "btnSentList"
        Me.btnSentList.Size = New System.Drawing.Size(70, 28)
        Me.btnSentList.TabIndex = 227
        Me.btnSentList.Text = "Sent &List"
        Me.ToolTip1.SetToolTip(Me.btnSentList, "Sent List : Alt + L")
        Me.btnSentList.UseVisualStyleBackColor = False
        '
        'btnNew
        '
        Me.btnNew.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnNew.AutoSize = True
        Me.btnNew.BackColor = System.Drawing.SystemColors.MenuBar
        Me.btnNew.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_NEW
        Me.btnNew.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNew.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnNew.ForeColor = System.Drawing.Color.Black
        Me.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnNew.Location = New System.Drawing.Point(5, 400)
        Me.btnNew.Margin = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(67, 29)
        Me.btnNew.TabIndex = 50
        Me.btnNew.Tag = "New"
        Me.btnNew.Text = "&New"
        Me.btnNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnNew, "New : Alt + N")
        Me.btnNew.UseVisualStyleBackColor = False
        '
        'pnlNotify
        '
        Me.pnlNotify.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlNotify.Controls.Add(Me.btnCancel)
        Me.pnlNotify.Controls.Add(Me.btnSend)
        Me.pnlNotify.Controls.Add(Me.pnlContact)
        Me.pnlNotify.Controls.Add(Me.txtUsers)
        Me.pnlNotify.Controls.Add(Me.lblUsers)
        Me.pnlNotify.Controls.Add(Me.txtNotes)
        Me.pnlNotify.Controls.Add(Me.lblNotes)
        Me.pnlNotify.Controls.Add(Me.txtTitle)
        Me.pnlNotify.Controls.Add(Me.lblTitle)
        Me.pnlNotify.Location = New System.Drawing.Point(12, 12)
        Me.pnlNotify.Name = "pnlNotify"
        Me.pnlNotify.Size = New System.Drawing.Size(816, 424)
        Me.pnlNotify.TabIndex = 44
        '
        'pnlContact
        '
        Me.pnlContact.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlContact.Controls.Add(Me.vsfSearch)
        Me.pnlContact.Controls.Add(Me.vsfContact)
        Me.pnlContact.Location = New System.Drawing.Point(79, 195)
        Me.pnlContact.Name = "pnlContact"
        Me.pnlContact.Size = New System.Drawing.Size(721, 186)
        Me.pnlContact.TabIndex = 44
        '
        'vsfSearch
        '
        Me.vsfSearch.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfSearch.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfSearch.AutoResize = True
        Me.vsfSearch.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfSearch.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfSearch.ExtendLastCol = True
        Me.vsfSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.vsfSearch.Location = New System.Drawing.Point(21, 5)
        Me.vsfSearch.Margin = New System.Windows.Forms.Padding(5)
        Me.vsfSearch.Name = "vsfSearch"
        Me.vsfSearch.Rows.DefaultSize = 24
        Me.vsfSearch.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.vsfSearch.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.RowRange
        Me.vsfSearch.Size = New System.Drawing.Size(700, 176)
        Me.vsfSearch.TabIndex = 43
        Me.vsfSearch.Visible = False
        '
        'vsfContact
        '
        Me.vsfContact.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfContact.AllowEditing = False
        Me.vsfContact.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfContact.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfContact.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfContact.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfContact.AutoResize = True
        Me.vsfContact.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfContact.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfContact.ColumnInfo = "3,0,0,0,0,120,Columns:"
        Me.vsfContact.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfContact.ExtendLastCol = True
        Me.vsfContact.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfContact.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfContact.ForeColor = System.Drawing.Color.Black
        Me.vsfContact.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfContact.Location = New System.Drawing.Point(21, 3)
        Me.vsfContact.Name = "vsfContact"
        Me.vsfContact.Rows.Count = 2
        Me.vsfContact.Rows.DefaultSize = 24
        Me.vsfContact.Rows.Fixed = 0
        Me.vsfContact.Rows.GlyphRow = 0
        Me.vsfContact.Rows.MinSize = 30
        Me.vsfContact.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfContact.ShowCellLabels = True
        Me.vsfContact.Size = New System.Drawing.Size(700, 180)
        Me.vsfContact.StyleInfo = resources.GetString("vsfContact.StyleInfo")
        Me.vsfContact.TabIndex = 0
        Me.vsfContact.Tag = "View"
        Me.vsfContact.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2007Blue
        '
        'txtUsers
        '
        Me.txtUsers.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtUsers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtUsers.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUsers.Location = New System.Drawing.Point(100, 162)
        Me.txtUsers.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtUsers.MaxLength = 50
        Me.txtUsers.Name = "txtUsers"
        Me.txtUsers.Size = New System.Drawing.Size(700, 26)
        Me.txtUsers.TabIndex = 2
        '
        'lblUsers
        '
        Me.lblUsers.AutoSize = True
        Me.lblUsers.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUsers.ForeColor = System.Drawing.Color.Black
        Me.lblUsers.Location = New System.Drawing.Point(17, 167)
        Me.lblUsers.Name = "lblUsers"
        Me.lblUsers.Size = New System.Drawing.Size(51, 20)
        Me.lblUsers.TabIndex = 42
        Me.lblUsers.Tag = ""
        Me.lblUsers.Text = "Users"
        '
        'txtNotes
        '
        Me.txtNotes.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtNotes.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNotes.Location = New System.Drawing.Point(100, 45)
        Me.txtNotes.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtNotes.MaxLength = 250
        Me.txtNotes.Multiline = True
        Me.txtNotes.Name = "txtNotes"
        Me.txtNotes.Size = New System.Drawing.Size(700, 111)
        Me.txtNotes.TabIndex = 1
        '
        'lblNotes
        '
        Me.lblNotes.AutoSize = True
        Me.lblNotes.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNotes.ForeColor = System.Drawing.Color.Black
        Me.lblNotes.Location = New System.Drawing.Point(17, 48)
        Me.lblNotes.Name = "lblNotes"
        Me.lblNotes.Size = New System.Drawing.Size(43, 20)
        Me.lblNotes.TabIndex = 40
        Me.lblNotes.Tag = ""
        Me.lblNotes.Text = "Note"
        '
        'txtTitle
        '
        Me.txtTitle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTitle.Location = New System.Drawing.Point(100, 11)
        Me.txtTitle.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtTitle.MaxLength = 50
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New System.Drawing.Size(700, 26)
        Me.txtTitle.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.ForeColor = System.Drawing.Color.Black
        Me.lblTitle.Location = New System.Drawing.Point(17, 14)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(38, 20)
        Me.lblTitle.TabIndex = 38
        Me.lblTitle.Tag = ""
        Me.lblTitle.Text = "Title"
        '
        'pnlList
        '
        Me.pnlList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlList.Controls.Add(Me.vsfDet)
        Me.pnlList.Controls.Add(Me.vsfSearchNotif)
        Me.pnlList.Controls.Add(Me.btnRecvList)
        Me.pnlList.Controls.Add(Me.btnSentList)
        Me.pnlList.Controls.Add(Me.btnNew)
        Me.pnlList.Controls.Add(Me.btnpnlClose)
        Me.pnlList.Controls.Add(Me.btnExport)
        Me.pnlList.Controls.Add(Me.btnDelAll)
        Me.pnlList.Controls.Add(Me.btnDelete)
        Me.pnlList.Location = New System.Drawing.Point(11, 2)
        Me.pnlList.Name = "pnlList"
        Me.pnlList.Size = New System.Drawing.Size(816, 445)
        Me.pnlList.TabIndex = 44
        '
        'vsfDet
        '
        Me.vsfDet.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfDet.AllowFiltering = True
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.AutoResize = True
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "10,0,0,0,0,105,Columns:"
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.vsfDet.Location = New System.Drawing.Point(0, 21)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(5)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 21
        Me.vsfDet.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.RowRange
        Me.vsfDet.Size = New System.Drawing.Size(816, 370)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 44
        '
        'vsfSearchNotif
        '
        Me.vsfSearchNotif.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfSearchNotif.AllowFiltering = True
        Me.vsfSearchNotif.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfSearchNotif.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfSearchNotif.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfSearchNotif.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfSearchNotif.AutoResize = True
        Me.vsfSearchNotif.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfSearchNotif.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfSearchNotif.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfSearchNotif.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfSearchNotif.ExtendLastCol = True
        Me.vsfSearchNotif.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfSearchNotif.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfSearchNotif.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfSearchNotif.Location = New System.Drawing.Point(0, 21)
        Me.vsfSearchNotif.Name = "vsfSearchNotif"
        Me.vsfSearchNotif.Rows.Count = 1
        Me.vsfSearchNotif.Rows.DefaultSize = 24
        Me.vsfSearchNotif.Rows.Fixed = 0
        Me.vsfSearchNotif.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.vsfSearchNotif.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfSearchNotif.ShowCellLabels = True
        Me.vsfSearchNotif.Size = New System.Drawing.Size(816, 25)
        Me.vsfSearchNotif.StyleInfo = resources.GetString("vsfSearchNotif.StyleInfo")
        Me.vsfSearchNotif.TabIndex = 229
        Me.vsfSearchNotif.Tag = ""
        Me.vsfSearchNotif.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.System
        '
        'frmUtlNotification
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(840, 449)
        Me.Controls.Add(Me.pnlList)
        Me.Controls.Add(Me.pnlNotify)
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(856, 487)
        Me.Name = "frmUtlNotification"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Notifications"
        Me.pnlNotify.ResumeLayout(False)
        Me.pnlNotify.PerformLayout()
        Me.pnlContact.ResumeLayout(False)
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfContact, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlList.ResumeLayout(False)
        Me.pnlList.PerformLayout()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfSearchNotif, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents pnlNotify As Panel
    Friend WithEvents pnlContact As Panel
    Friend WithEvents vsfSearch As C1.Win.C1FlexGrid.C1FlexGrid
    Public WithEvents vsfContact As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents txtUsers As TextBox
    Friend WithEvents lblUsers As Label
    Friend WithEvents txtNotes As TextBox
    Friend WithEvents lblNotes As Label
    Friend WithEvents txtTitle As TextBox
    Friend WithEvents lblTitle As Label
    Friend WithEvents btnCancel As C1.Win.C1Input.C1Button
    Friend WithEvents btnSend As Button
    Friend WithEvents pnlList As Panel
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnDelAll As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnExport As C1.Win.C1Input.C1Button
    Friend WithEvents btnpnlClose As Button
    Friend WithEvents btnRecvList As Button
    Friend WithEvents btnSentList As Button
    Friend WithEvents btnNew As Button
    Friend WithEvents vsfSearchNotif As C1.Win.C1FlexGrid.C1FlexGrid
End Class
