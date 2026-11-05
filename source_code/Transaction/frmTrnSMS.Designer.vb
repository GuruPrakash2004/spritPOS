<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmTrnSMS
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTrnSMS))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblCount = New System.Windows.Forms.Label()
        Me.txtTo = New System.Windows.Forms.TextBox()
        Me.txtMsg = New System.Windows.Forms.TextBox()
        Me.btnSend = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnClr = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnMultiSel = New System.Windows.Forms.Button()
        Me.btnContact = New System.Windows.Forms.Button()
        Me.vsfSearch = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnTemplate = New System.Windows.Forms.Button()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.pnlContact = New System.Windows.Forms.Panel()
        Me.vsfContact = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cboTemplate = New System.Windows.Forms.ComboBox()
        Me.btnTemplateSettings = New System.Windows.Forms.Button()
        Me.BottomToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.TopToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.RightToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.LeftToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.ContentPanel = New System.Windows.Forms.ToolStripContentPanel()
        Me.Panel10 = New System.Windows.Forms.Panel()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.pnlTotMsgsLeft = New System.Windows.Forms.Panel()
        Me.btnLastMsg = New System.Windows.Forms.Button()
        Me.lblMsgDtls = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cboProvider = New System.Windows.Forms.ComboBox()
        Me.txtTranID = New System.Windows.Forms.TextBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.pnlContact.SuspendLayout()
        CType(Me.vsfContact, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        Me.Panel10.SuspendLayout()
        Me.pnlTotMsgsLeft.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(8, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(27, 23)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "To"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.Color.Black
        Me.Label3.Location = New System.Drawing.Point(8, 3)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(76, 23)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Message"
        '
        'lblCount
        '
        Me.lblCount.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblCount.AutoSize = True
        Me.lblCount.ForeColor = System.Drawing.Color.Black
        Me.lblCount.Location = New System.Drawing.Point(260, 509)
        Me.lblCount.Name = "lblCount"
        Me.lblCount.Size = New System.Drawing.Size(40, 23)
        Me.lblCount.TabIndex = 1
        Me.lblCount.Text = "Text"
        '
        'txtTo
        '
        Me.txtTo.Location = New System.Drawing.Point(88, 11)
        Me.txtTo.MaxLength = 10
        Me.txtTo.Name = "txtTo"
        Me.txtTo.Size = New System.Drawing.Size(357, 29)
        Me.txtTo.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.txtTo, "Receiver's Mobile No")
        '
        'txtMsg
        '
        Me.txtMsg.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMsg.Location = New System.Drawing.Point(80, 3)
        Me.txtMsg.MaxLength = 150
        Me.txtMsg.Multiline = True
        Me.txtMsg.Name = "txtMsg"
        Me.txtMsg.Size = New System.Drawing.Size(357, 254)
        Me.txtMsg.TabIndex = 0
        Me.ToolTip1.SetToolTip(Me.txtMsg, "Type Message")
        '
        'btnSend
        '
        Me.btnSend.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSend.BackColor = System.Drawing.Color.Transparent
        Me.btnSend.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSend.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnSend.ForeColor = System.Drawing.Color.Black
        Me.btnSend.Location = New System.Drawing.Point(255, 523)
        Me.btnSend.Name = "btnSend"
        Me.btnSend.Size = New System.Drawing.Size(95, 30)
        Me.btnSend.TabIndex = 3
        Me.btnSend.Text = "Se&nd (F6)"
        Me.ToolTip1.SetToolTip(Me.btnSend, "Shortcut Alt+N")
        Me.btnSend.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackColor = System.Drawing.Color.Transparent
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnClose.ForeColor = System.Drawing.Color.Black
        Me.btnClose.Location = New System.Drawing.Point(356, 523)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(88, 30)
        Me.btnClose.TabIndex = 4
        Me.btnClose.Text = "&Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Shortcut Alt+C")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnClr
        '
        Me.btnClr.BackColor = System.Drawing.Color.Transparent
        Me.btnClr.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClr.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClr.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnClr.ForeColor = System.Drawing.Color.Black
        Me.btnClr.Location = New System.Drawing.Point(687, 9)
        Me.btnClr.Name = "btnClr"
        Me.btnClr.Size = New System.Drawing.Size(97, 30)
        Me.btnClr.TabIndex = 6
        Me.btnClr.Text = "Clea&r"
        Me.ToolTip1.SetToolTip(Me.btnClr, "Shortcut Alt+R")
        Me.btnClr.UseVisualStyleBackColor = False
        Me.btnClr.Visible = False
        '
        'btnMultiSel
        '
        Me.btnMultiSel.BackColor = System.Drawing.Color.Transparent
        Me.btnMultiSel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnMultiSel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMultiSel.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnMultiSel.ForeColor = System.Drawing.Color.Black
        Me.btnMultiSel.Location = New System.Drawing.Point(496, 9)
        Me.btnMultiSel.Name = "btnMultiSel"
        Me.btnMultiSel.Size = New System.Drawing.Size(130, 30)
        Me.btnMultiSel.TabIndex = 15
        Me.btnMultiSel.Text = "&Multi Select"
        Me.ToolTip1.SetToolTip(Me.btnMultiSel, "Shortcut Alt+M")
        Me.btnMultiSel.UseVisualStyleBackColor = False
        Me.btnMultiSel.Visible = False
        '
        'btnContact
        '
        Me.btnContact.BackColor = System.Drawing.Color.Transparent
        Me.btnContact.BackgroundImage = CType(resources.GetObject("btnContact.BackgroundImage"), System.Drawing.Image)
        Me.btnContact.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnContact.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnContact.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnContact.ForeColor = System.Drawing.Color.Black
        Me.btnContact.Location = New System.Drawing.Point(1041, 14)
        Me.btnContact.Name = "btnContact"
        Me.btnContact.Size = New System.Drawing.Size(29, 26)
        Me.btnContact.TabIndex = 1
        Me.btnContact.Text = "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.ToolTip1.SetToolTip(Me.btnContact, "New Contact")
        Me.btnContact.UseVisualStyleBackColor = False
        Me.btnContact.Visible = False
        '
        'vsfSearch
        '
        Me.vsfSearch.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfSearch.AllowFiltering = True
        Me.vsfSearch.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfSearch.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfSearch.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.vsfSearch.AutoResize = True
        Me.vsfSearch.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfSearch.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfSearch.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfSearch.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfSearch.ExtendLastCol = True
        Me.vsfSearch.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfSearch.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfSearch.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfSearch.Location = New System.Drawing.Point(496, 57)
        Me.vsfSearch.Name = "vsfSearch"
        Me.vsfSearch.Rows.DefaultSize = 28
        Me.vsfSearch.Rows.GlyphRow = 0
        Me.vsfSearch.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfSearch.ShowCellLabels = True
        Me.vsfSearch.Size = New System.Drawing.Size(306, 323)
        Me.vsfSearch.StyleInfo = resources.GetString("vsfSearch.StyleInfo")
        Me.vsfSearch.TabIndex = 14
        Me.vsfSearch.Tag = "View"
        Me.vsfSearch.Visible = False
        Me.vsfSearch.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2007Blue
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.txtMsg)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.btnTemplate)
        Me.Panel1.Location = New System.Drawing.Point(3, 171)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(476, 263)
        Me.Panel1.TabIndex = 2
        '
        'btnTemplate
        '
        Me.btnTemplate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnTemplate.BackColor = System.Drawing.Color.Transparent
        Me.btnTemplate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnTemplate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTemplate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTemplate.ForeColor = System.Drawing.Color.Black
        Me.btnTemplate.Image = CType(resources.GetObject("btnTemplate.Image"), System.Drawing.Image)
        Me.btnTemplate.Location = New System.Drawing.Point(440, 3)
        Me.btnTemplate.Name = "btnTemplate"
        Me.btnTemplate.Size = New System.Drawing.Size(33, 25)
        Me.btnTemplate.TabIndex = 1
        Me.btnTemplate.Text = "..."
        Me.btnTemplate.UseVisualStyleBackColor = False
        Me.btnTemplate.Visible = False
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.AllowDrop = True
        Me.FlowLayoutPanel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.FlowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel1.Controls.Add(Me.pnlContact)
        Me.FlowLayoutPanel1.Controls.Add(Me.Panel3)
        Me.FlowLayoutPanel1.Controls.Add(Me.Panel1)
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(4, 79)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(480, 438)
        Me.FlowLayoutPanel1.TabIndex = 2
        '
        'pnlContact
        '
        Me.pnlContact.Controls.Add(Me.vsfContact)
        Me.pnlContact.Location = New System.Drawing.Point(3, 3)
        Me.pnlContact.Name = "pnlContact"
        Me.pnlContact.Size = New System.Drawing.Size(470, 124)
        Me.pnlContact.TabIndex = 0
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
        Me.vsfContact.Location = New System.Drawing.Point(80, 3)
        Me.vsfContact.Name = "vsfContact"
        Me.vsfContact.Rows.Count = 2
        Me.vsfContact.Rows.DefaultSize = 28
        Me.vsfContact.Rows.Fixed = 0
        Me.vsfContact.Rows.GlyphRow = 0
        Me.vsfContact.Rows.MinSize = 30
        Me.vsfContact.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfContact.ShowCellLabels = True
        Me.vsfContact.Size = New System.Drawing.Size(357, 118)
        Me.vsfContact.StyleInfo = resources.GetString("vsfContact.StyleInfo")
        Me.vsfContact.TabIndex = 0
        Me.vsfContact.Tag = "View"
        Me.vsfContact.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2007Blue
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.Label2)
        Me.Panel3.Controls.Add(Me.cboTemplate)
        Me.Panel3.Controls.Add(Me.btnTemplateSettings)
        Me.Panel3.Location = New System.Drawing.Point(3, 133)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(470, 32)
        Me.Panel3.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(8, 8)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(78, 20)
        Me.Label2.TabIndex = 35
        Me.Label2.Text = "Template"
        '
        'cboTemplate
        '
        Me.cboTemplate.FormattingEnabled = True
        Me.cboTemplate.Location = New System.Drawing.Point(80, 4)
        Me.cboTemplate.Name = "cboTemplate"
        Me.cboTemplate.Size = New System.Drawing.Size(357, 29)
        Me.cboTemplate.TabIndex = 0
        '
        'btnTemplateSettings
        '
        Me.btnTemplateSettings.Location = New System.Drawing.Point(439, 4)
        Me.btnTemplateSettings.Name = "btnTemplateSettings"
        Me.btnTemplateSettings.Size = New System.Drawing.Size(27, 25)
        Me.btnTemplateSettings.TabIndex = 1
        Me.btnTemplateSettings.Tag = "SMS Template Settings"
        Me.btnTemplateSettings.Text = "..."
        Me.btnTemplateSettings.UseVisualStyleBackColor = True
        '
        'BottomToolStripPanel
        '
        Me.BottomToolStripPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.BottomToolStripPanel.Location = New System.Drawing.Point(0, 175)
        Me.BottomToolStripPanel.Name = "BottomToolStripPanel"
        Me.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.BottomToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.BottomToolStripPanel.Size = New System.Drawing.Size(150, 0)
        '
        'TopToolStripPanel
        '
        Me.TopToolStripPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me.TopToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.TopToolStripPanel.Name = "TopToolStripPanel"
        Me.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.TopToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.TopToolStripPanel.Size = New System.Drawing.Size(150, 25)
        '
        'RightToolStripPanel
        '
        Me.RightToolStripPanel.Dock = System.Windows.Forms.DockStyle.Right
        Me.RightToolStripPanel.Location = New System.Drawing.Point(150, 25)
        Me.RightToolStripPanel.Name = "RightToolStripPanel"
        Me.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Vertical
        Me.RightToolStripPanel.RowMargin = New System.Windows.Forms.Padding(0, 3, 0, 0)
        Me.RightToolStripPanel.Size = New System.Drawing.Size(0, 150)
        '
        'LeftToolStripPanel
        '
        Me.LeftToolStripPanel.Dock = System.Windows.Forms.DockStyle.Left
        Me.LeftToolStripPanel.Location = New System.Drawing.Point(0, 25)
        Me.LeftToolStripPanel.Name = "LeftToolStripPanel"
        Me.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Vertical
        Me.LeftToolStripPanel.RowMargin = New System.Windows.Forms.Padding(0, 3, 0, 0)
        Me.LeftToolStripPanel.Size = New System.Drawing.Size(0, 150)
        '
        'ContentPanel
        '
        Me.ContentPanel.Size = New System.Drawing.Size(150, 150)
        '
        'Panel10
        '
        Me.Panel10.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel10.BackColor = System.Drawing.Color.FromArgb(CType(CType(185, Byte), Integer), CType(CType(66, Byte), Integer), CType(CType(71, Byte), Integer))
        Me.Panel10.Controls.Add(Me.Label5)
        Me.Panel10.Location = New System.Drawing.Point(452, 386)
        Me.Panel10.Name = "Panel10"
        Me.Panel10.Size = New System.Drawing.Size(170, 26)
        Me.Panel10.TabIndex = 20
        Me.Panel10.Visible = False
        '
        'Label5
        '
        Me.Label5.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(1, 40)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(166, 23)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "My Last Message Details"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.Label5.Visible = False
        '
        'pnlTotMsgsLeft
        '
        Me.pnlTotMsgsLeft.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlTotMsgsLeft.BackColor = System.Drawing.Color.Transparent
        Me.pnlTotMsgsLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlTotMsgsLeft.Controls.Add(Me.btnLastMsg)
        Me.pnlTotMsgsLeft.Controls.Add(Me.lblMsgDtls)
        Me.pnlTotMsgsLeft.Location = New System.Drawing.Point(452, 411)
        Me.pnlTotMsgsLeft.Name = "pnlTotMsgsLeft"
        Me.pnlTotMsgsLeft.Size = New System.Drawing.Size(170, 166)
        Me.pnlTotMsgsLeft.TabIndex = 19
        Me.pnlTotMsgsLeft.Visible = False
        '
        'btnLastMsg
        '
        Me.btnLastMsg.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.btnLastMsg.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLastMsg.Location = New System.Drawing.Point(4, 130)
        Me.btnLastMsg.Name = "btnLastMsg"
        Me.btnLastMsg.Size = New System.Drawing.Size(161, 28)
        Me.btnLastMsg.TabIndex = 2
        Me.btnLastMsg.Text = "&Last Messages"
        Me.btnLastMsg.UseVisualStyleBackColor = False
        Me.btnLastMsg.Visible = False
        '
        'lblMsgDtls
        '
        Me.lblMsgDtls.BackColor = System.Drawing.Color.Transparent
        Me.lblMsgDtls.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMsgDtls.ForeColor = System.Drawing.Color.White
        Me.lblMsgDtls.Location = New System.Drawing.Point(1, 3)
        Me.lblMsgDtls.Name = "lblMsgDtls"
        Me.lblMsgDtls.Size = New System.Drawing.Size(166, 161)
        Me.lblMsgDtls.TabIndex = 1
        Me.lblMsgDtls.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblMsgDtls.Visible = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Black
        Me.Label4.Location = New System.Drawing.Point(8, 52)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(71, 20)
        Me.Label4.TabIndex = 34
        Me.Label4.Text = "Provider"
        '
        'cboProvider
        '
        Me.cboProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboProvider.FormattingEnabled = True
        Me.cboProvider.Location = New System.Drawing.Point(88, 48)
        Me.cboProvider.Name = "cboProvider"
        Me.cboProvider.Size = New System.Drawing.Size(357, 29)
        Me.cboProvider.TabIndex = 1
        '
        'txtTranID
        '
        Me.txtTranID.Location = New System.Drawing.Point(527, 382)
        Me.txtTranID.Name = "txtTranID"
        Me.txtTranID.Size = New System.Drawing.Size(25, 29)
        Me.txtTranID.TabIndex = 35
        Me.txtTranID.Visible = False
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.cboProvider)
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Controls.Add(Me.txtTo)
        Me.Panel2.Controls.Add(Me.Label4)
        Me.Panel2.Controls.Add(Me.FlowLayoutPanel1)
        Me.Panel2.Controls.Add(Me.btnSend)
        Me.Panel2.Controls.Add(Me.btnClose)
        Me.Panel2.Location = New System.Drawing.Point(5, 16)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(623, 561)
        Me.Panel2.TabIndex = 0
        '
        'frmTrnSMS
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(632, 573)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.txtTranID)
        Me.Controls.Add(Me.Panel10)
        Me.Controls.Add(Me.pnlTotMsgsLeft)
        Me.Controls.Add(Me.vsfSearch)
        Me.Controls.Add(Me.btnMultiSel)
        Me.Controls.Add(Me.btnClr)
        Me.Controls.Add(Me.btnContact)
        Me.Controls.Add(Me.lblCount)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(650, 620)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(650, 620)
        Me.Name = "frmTrnSMS"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Send  Party SMS"
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.pnlContact.ResumeLayout(False)
        CType(Me.vsfContact, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel10.ResumeLayout(False)
        Me.pnlTotMsgsLeft.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents lblCount As Label
    Friend WithEvents txtTo As TextBox
    Friend WithEvents txtMsg As TextBox
    Friend WithEvents btnSend As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents btnContact As Button
    Friend WithEvents btnTemplate As Button
    Friend WithEvents btnClr As Button
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents vsfSearch As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnMultiSel As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents BottomToolStripPanel As ToolStripPanel
    Friend WithEvents TopToolStripPanel As ToolStripPanel
    Friend WithEvents RightToolStripPanel As ToolStripPanel
    Friend WithEvents LeftToolStripPanel As ToolStripPanel
    Friend WithEvents ContentPanel As ToolStripContentPanel
    Public WithEvents vsfContact As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents pnlContact As Panel
    Friend WithEvents Panel10 As Panel
    Friend WithEvents Label5 As Label
    Friend WithEvents pnlTotMsgsLeft As Panel
    Friend WithEvents lblMsgDtls As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents cboProvider As ComboBox
    Friend WithEvents btnLastMsg As Button
    Friend WithEvents txtTranID As TextBox
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents btnTemplateSettings As Button
    Friend WithEvents cboTemplate As ComboBox
    Friend WithEvents Label2 As Label
End Class
