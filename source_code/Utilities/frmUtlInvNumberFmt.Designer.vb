<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlInvNumberFmt
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlInvNumberFmt))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.optNone = New System.Windows.Forms.RadioButton()
        Me.optMonthWise = New System.Windows.Forms.RadioButton()
        Me.optDaywise = New System.Windows.Forms.RadioButton()
        Me.fraMonth = New System.Windows.Forms.Panel()
        Me.btnMnthDefault = New C1.Win.C1Input.C1SplitButton()
        Me.Def1 = New C1.Win.C1Input.DropDownItem()
        Me.Def2 = New C1.Win.C1Input.DropDownItem()
        Me.Def3 = New C1.Win.C1Input.DropDownItem()
        Me.vsfMnth = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.chkCounterWsRst = New System.Windows.Forms.CheckBox()
        Me.vsfEx = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfDocNoSet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.txtDocNo = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.btnSave = New C1.Win.C1Input.C1Button()
        Me.cboInvAltF8 = New System.Windows.Forms.ComboBox()
        Me.cboInvCtrlF8 = New System.Windows.Forms.ComboBox()
        Me.cboInvF8 = New System.Windows.Forms.ComboBox()
        Me.lblAltF8 = New System.Windows.Forms.Label()
        Me.lblCtrlF8 = New System.Windows.Forms.Label()
        Me.lblF12 = New System.Windows.Forms.Label()
        Me.chkUpdateRate = New System.Windows.Forms.CheckBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.pnlSMS = New System.Windows.Forms.Panel()
        Me.txtDelMsg = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtCancelMsg = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtSalesMsg = New System.Windows.Forms.TextBox()
        Me.chkForDeletion = New System.Windows.Forms.CheckBox()
        Me.lblMessage = New System.Windows.Forms.Label()
        Me.chkForCancellation = New System.Windows.Forms.CheckBox()
        Me.txtValue = New System.Windows.Forms.TextBox()
        Me.lblValue = New System.Windows.Forms.Label()
        Me.chkForSalesValue = New System.Windows.Forms.CheckBox()
        Me.lblSMS = New System.Windows.Forms.Label()
        Me.fraMonth.SuspendLayout()
        CType(Me.vsfMnth, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.vsfEx, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfDocNoSet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.pnlSMS.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 3)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(40, 17)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Prefix"
        '
        'optNone
        '
        Me.optNone.AutoSize = True
        Me.optNone.Checked = True
        Me.optNone.Location = New System.Drawing.Point(57, 2)
        Me.optNone.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.optNone.Name = "optNone"
        Me.optNone.Size = New System.Drawing.Size(58, 21)
        Me.optNone.TabIndex = 2
        Me.optNone.TabStop = True
        Me.optNone.Text = "None"
        Me.optNone.UseVisualStyleBackColor = True
        '
        'optMonthWise
        '
        Me.optMonthWise.AutoSize = True
        Me.optMonthWise.Location = New System.Drawing.Point(57, 26)
        Me.optMonthWise.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.optMonthWise.Name = "optMonthWise"
        Me.optMonthWise.Size = New System.Drawing.Size(93, 21)
        Me.optMonthWise.TabIndex = 3
        Me.optMonthWise.Text = "Month wise"
        Me.optMonthWise.UseVisualStyleBackColor = True
        '
        'optDaywise
        '
        Me.optDaywise.AutoSize = True
        Me.optDaywise.Location = New System.Drawing.Point(57, 51)
        Me.optDaywise.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.optDaywise.Name = "optDaywise"
        Me.optDaywise.Size = New System.Drawing.Size(77, 21)
        Me.optDaywise.TabIndex = 4
        Me.optDaywise.Text = "Day wise"
        Me.optDaywise.UseVisualStyleBackColor = True
        '
        'fraMonth
        '
        Me.fraMonth.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fraMonth.Controls.Add(Me.btnMnthDefault)
        Me.fraMonth.Controls.Add(Me.vsfMnth)
        Me.fraMonth.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.fraMonth.Location = New System.Drawing.Point(9, 122)
        Me.fraMonth.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.fraMonth.Name = "fraMonth"
        Me.fraMonth.Size = New System.Drawing.Size(199, 245)
        Me.fraMonth.TabIndex = 5
        '
        'btnMnthDefault
        '
        Me.btnMnthDefault.Items.Add(Me.Def1)
        Me.btnMnthDefault.Items.Add(Me.Def2)
        Me.btnMnthDefault.Items.Add(Me.Def3)
        Me.btnMnthDefault.Location = New System.Drawing.Point(14, 5)
        Me.btnMnthDefault.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnMnthDefault.Name = "btnMnthDefault"
        Me.btnMnthDefault.Size = New System.Drawing.Size(134, 30)
        Me.btnMnthDefault.TabIndex = 1
        Me.btnMnthDefault.Text = "Fill Defaults"
        Me.btnMnthDefault.UseVisualStyleBackColor = True
        '
        'Def1
        '
        Me.Def1.Text = "Style-1"
        '
        'Def2
        '
        Me.Def2.Text = "Style-2"
        '
        'Def3
        '
        Me.Def3.Text = "Style-3"
        '
        'vsfMnth
        '
        Me.vsfMnth.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.vsfMnth.ColumnInfo = "10,1,0,0,0,120,Columns:"
        Me.vsfMnth.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfMnth.Location = New System.Drawing.Point(13, 43)
        Me.vsfMnth.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfMnth.Name = "vsfMnth"
        Me.vsfMnth.Rows.Count = 13
        Me.vsfMnth.Rows.DefaultSize = 24
        Me.vsfMnth.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.vsfMnth.Size = New System.Drawing.Size(140, 186)
        Me.vsfMnth.StyleInfo = resources.GetString("vsfMnth.StyleInfo")
        Me.vsfMnth.TabIndex = 0
        Me.vsfMnth.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2010Silver
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.chkCounterWsRst)
        Me.Panel1.Controls.Add(Me.optNone)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.optMonthWise)
        Me.Panel1.Controls.Add(Me.optDaywise)
        Me.Panel1.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Panel1.Location = New System.Drawing.Point(10, 12)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(201, 102)
        Me.Panel1.TabIndex = 9
        '
        'chkCounterWsRst
        '
        Me.chkCounterWsRst.AutoSize = True
        Me.chkCounterWsRst.Location = New System.Drawing.Point(57, 74)
        Me.chkCounterWsRst.Name = "chkCounterWsRst"
        Me.chkCounterWsRst.Size = New System.Drawing.Size(141, 21)
        Me.chkCounterWsRst.TabIndex = 80
        Me.chkCounterWsRst.Tag = ""
        Me.chkCounterWsRst.Text = "Counter Wise Reset"
        Me.chkCounterWsRst.UseVisualStyleBackColor = True
        '
        'vsfEx
        '
        Me.vsfEx.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfEx.BackColor = System.Drawing.Color.White
        Me.vsfEx.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfEx.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfEx.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfEx.ForeColor = System.Drawing.Color.Black
        Me.vsfEx.Location = New System.Drawing.Point(217, 13)
        Me.vsfEx.Name = "vsfEx"
        Me.vsfEx.Rows.Count = 3
        Me.vsfEx.Rows.DefaultSize = 24
        Me.vsfEx.Size = New System.Drawing.Size(771, 83)
        Me.vsfEx.StyleInfo = resources.GetString("vsfEx.StyleInfo")
        Me.vsfEx.TabIndex = 10
        '
        'vsfDocNoSet
        '
        Me.vsfDocNoSet.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDocNoSet.BackColor = System.Drawing.Color.White
        Me.vsfDocNoSet.ColumnInfo = "3,0,0,0,0,120,Columns:"
        Me.vsfDocNoSet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDocNoSet.ForeColor = System.Drawing.Color.Black
        Me.vsfDocNoSet.Location = New System.Drawing.Point(216, 192)
        Me.vsfDocNoSet.Name = "vsfDocNoSet"
        Me.vsfDocNoSet.Rows.Count = 1
        Me.vsfDocNoSet.Rows.DefaultSize = 24
        Me.vsfDocNoSet.Rows.Fixed = 0
        Me.vsfDocNoSet.Size = New System.Drawing.Size(771, 30)
        Me.vsfDocNoSet.StyleInfo = resources.GetString("vsfDocNoSet.StyleInfo")
        Me.vsfDocNoSet.TabIndex = 11
        '
        'txtDocNo
        '
        Me.txtDocNo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDocNo.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtDocNo.Enabled = False
        Me.txtDocNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDocNo.Location = New System.Drawing.Point(217, 249)
        Me.txtDocNo.Name = "txtDocNo"
        Me.txtDocNo.Size = New System.Drawing.Size(771, 38)
        Me.txtDocNo.TabIndex = 12
        '
        'Label2
        '
        Me.Label2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(214, 107)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(436, 17)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "Please enter the no  [ 1- 8 ] with separator to form the document number"
        '
        'Label3
        '
        Me.Label3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(216, 228)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(119, 17)
        Me.Label3.TabIndex = 14
        Me.Label3.Text = "Document Number"
        '
        'Label4
        '
        Me.Label4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(214, 132)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(400, 51)
        Me.Label4.TabIndex = 79
        Me.Label4.Text = "If we select from 1 to 8, then above prefix will be replaced " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "and other than tha" &
    "t ,it is same prefix as used in Document Number." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Eg: 1/JAN/0001"
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(916, 496)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(70, 26)
        Me.btnClose.TabIndex = 85
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Shortcut: Alt+O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(837, 496)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(71, 26)
        Me.btnSave.TabIndex = 86
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "ShortCut: Alt+S")
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'cboInvAltF8
        '
        Me.cboInvAltF8.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cboInvAltF8.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboInvAltF8.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboInvAltF8.FormattingEnabled = True
        Me.cboInvAltF8.Items.AddRange(New Object() {"Save", "Save and Print", "Save and Preview"})
        Me.cboInvAltF8.Location = New System.Drawing.Point(75, 75)
        Me.cboInvAltF8.Name = "cboInvAltF8"
        Me.cboInvAltF8.Size = New System.Drawing.Size(118, 25)
        Me.cboInvAltF8.TabIndex = 5
        '
        'cboInvCtrlF8
        '
        Me.cboInvCtrlF8.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cboInvCtrlF8.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboInvCtrlF8.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboInvCtrlF8.FormattingEnabled = True
        Me.cboInvCtrlF8.Items.AddRange(New Object() {"Save", "Save and Print", "Save and Preview"})
        Me.cboInvCtrlF8.Location = New System.Drawing.Point(75, 45)
        Me.cboInvCtrlF8.Name = "cboInvCtrlF8"
        Me.cboInvCtrlF8.Size = New System.Drawing.Size(118, 25)
        Me.cboInvCtrlF8.TabIndex = 4
        '
        'cboInvF8
        '
        Me.cboInvF8.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cboInvF8.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboInvF8.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboInvF8.FormattingEnabled = True
        Me.cboInvF8.Items.AddRange(New Object() {"Save", "Save and Print", "Save and Preview"})
        Me.cboInvF8.Location = New System.Drawing.Point(54, 17)
        Me.cboInvF8.Name = "cboInvF8"
        Me.cboInvF8.Size = New System.Drawing.Size(139, 25)
        Me.cboInvF8.TabIndex = 3
        '
        'lblAltF8
        '
        Me.lblAltF8.AutoSize = True
        Me.lblAltF8.Location = New System.Drawing.Point(4, 78)
        Me.lblAltF8.Name = "lblAltF8"
        Me.lblAltF8.Size = New System.Drawing.Size(52, 17)
        Me.lblAltF8.TabIndex = 2
        Me.lblAltF8.Text = "Alt+F12"
        '
        'lblCtrlF8
        '
        Me.lblCtrlF8.AutoSize = True
        Me.lblCtrlF8.Location = New System.Drawing.Point(4, 48)
        Me.lblCtrlF8.Name = "lblCtrlF8"
        Me.lblCtrlF8.Size = New System.Drawing.Size(57, 17)
        Me.lblCtrlF8.TabIndex = 1
        Me.lblCtrlF8.Text = "Ctrl+F12"
        '
        'lblF12
        '
        Me.lblF12.AutoSize = True
        Me.lblF12.ForeColor = System.Drawing.Color.Red
        Me.lblF12.Location = New System.Drawing.Point(4, 20)
        Me.lblF12.Name = "lblF12"
        Me.lblF12.Size = New System.Drawing.Size(28, 17)
        Me.lblF12.TabIndex = 0
        Me.lblF12.Tag = "M"
        Me.lblF12.Text = "F12"
        '
        'chkUpdateRate
        '
        Me.chkUpdateRate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkUpdateRate.AutoSize = True
        Me.chkUpdateRate.Location = New System.Drawing.Point(9, 495)
        Me.chkUpdateRate.Name = "chkUpdateRate"
        Me.chkUpdateRate.Size = New System.Drawing.Size(242, 21)
        Me.chkUpdateRate.TabIndex = 87
        Me.chkUpdateRate.Text = "Update Rate From PB to Item Master"
        Me.chkUpdateRate.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.cboInvAltF8)
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Controls.Add(Me.cboInvCtrlF8)
        Me.Panel2.Controls.Add(Me.lblF12)
        Me.Panel2.Controls.Add(Me.cboInvF8)
        Me.Panel2.Controls.Add(Me.lblCtrlF8)
        Me.Panel2.Controls.Add(Me.lblAltF8)
        Me.Panel2.Location = New System.Drawing.Point(9, 373)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(202, 112)
        Me.Panel2.TabIndex = 88
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(1, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(128, 17)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Invoice Print Settings"
        '
        'pnlSMS
        '
        Me.pnlSMS.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlSMS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlSMS.Controls.Add(Me.txtDelMsg)
        Me.pnlSMS.Controls.Add(Me.Label7)
        Me.pnlSMS.Controls.Add(Me.txtCancelMsg)
        Me.pnlSMS.Controls.Add(Me.Label6)
        Me.pnlSMS.Controls.Add(Me.txtSalesMsg)
        Me.pnlSMS.Controls.Add(Me.chkForDeletion)
        Me.pnlSMS.Controls.Add(Me.lblMessage)
        Me.pnlSMS.Controls.Add(Me.chkForCancellation)
        Me.pnlSMS.Controls.Add(Me.txtValue)
        Me.pnlSMS.Controls.Add(Me.lblValue)
        Me.pnlSMS.Controls.Add(Me.chkForSalesValue)
        Me.pnlSMS.Controls.Add(Me.lblSMS)
        Me.pnlSMS.Location = New System.Drawing.Point(217, 293)
        Me.pnlSMS.Name = "pnlSMS"
        Me.pnlSMS.Size = New System.Drawing.Size(771, 192)
        Me.pnlSMS.TabIndex = 89
        '
        'txtDelMsg
        '
        Me.txtDelMsg.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDelMsg.Enabled = False
        Me.txtDelMsg.Location = New System.Drawing.Point(537, 73)
        Me.txtDelMsg.MaxLength = 250
        Me.txtDelMsg.Multiline = True
        Me.txtDelMsg.Name = "txtDelMsg"
        Me.txtDelMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDelMsg.Size = New System.Drawing.Size(218, 102)
        Me.txtDelMsg.TabIndex = 7
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(534, 47)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(61, 17)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "Message"
        '
        'txtCancelMsg
        '
        Me.txtCancelMsg.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtCancelMsg.Enabled = False
        Me.txtCancelMsg.Location = New System.Drawing.Point(297, 73)
        Me.txtCancelMsg.MaxLength = 250
        Me.txtCancelMsg.Multiline = True
        Me.txtCancelMsg.Name = "txtCancelMsg"
        Me.txtCancelMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtCancelMsg.Size = New System.Drawing.Size(221, 102)
        Me.txtCancelMsg.TabIndex = 5
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(295, 47)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(61, 17)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "Message"
        '
        'txtSalesMsg
        '
        Me.txtSalesMsg.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtSalesMsg.Enabled = False
        Me.txtSalesMsg.Location = New System.Drawing.Point(62, 73)
        Me.txtSalesMsg.MaxLength = 250
        Me.txtSalesMsg.Multiline = True
        Me.txtSalesMsg.Name = "txtSalesMsg"
        Me.txtSalesMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtSalesMsg.Size = New System.Drawing.Size(218, 102)
        Me.txtSalesMsg.TabIndex = 3
        '
        'chkForDeletion
        '
        Me.chkForDeletion.AutoSize = True
        Me.chkForDeletion.Location = New System.Drawing.Point(537, 23)
        Me.chkForDeletion.Name = "chkForDeletion"
        Me.chkForDeletion.Size = New System.Drawing.Size(98, 21)
        Me.chkForDeletion.TabIndex = 3
        Me.chkForDeletion.Text = "For Deletion"
        Me.chkForDeletion.UseVisualStyleBackColor = True
        '
        'lblMessage
        '
        Me.lblMessage.AutoSize = True
        Me.lblMessage.Location = New System.Drawing.Point(3, 73)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(61, 17)
        Me.lblMessage.TabIndex = 2
        Me.lblMessage.Text = "Message"
        '
        'chkForCancellation
        '
        Me.chkForCancellation.AutoSize = True
        Me.chkForCancellation.Location = New System.Drawing.Point(298, 23)
        Me.chkForCancellation.Name = "chkForCancellation"
        Me.chkForCancellation.Size = New System.Drawing.Size(120, 21)
        Me.chkForCancellation.TabIndex = 2
        Me.chkForCancellation.Text = "For Cancellation"
        Me.chkForCancellation.UseVisualStyleBackColor = True
        '
        'txtValue
        '
        Me.txtValue.Enabled = False
        Me.txtValue.Location = New System.Drawing.Point(62, 44)
        Me.txtValue.MaxLength = 20
        Me.txtValue.Name = "txtValue"
        Me.txtValue.Size = New System.Drawing.Size(218, 25)
        Me.txtValue.TabIndex = 1
        '
        'lblValue
        '
        Me.lblValue.AutoSize = True
        Me.lblValue.Location = New System.Drawing.Point(2, 47)
        Me.lblValue.Name = "lblValue"
        Me.lblValue.Size = New System.Drawing.Size(53, 17)
        Me.lblValue.TabIndex = 0
        Me.lblValue.Text = "Value >"
        '
        'chkForSalesValue
        '
        Me.chkForSalesValue.AutoSize = True
        Me.chkForSalesValue.Location = New System.Drawing.Point(7, 23)
        Me.chkForSalesValue.Name = "chkForSalesValue"
        Me.chkForSalesValue.Size = New System.Drawing.Size(122, 21)
        Me.chkForSalesValue.TabIndex = 1
        Me.chkForSalesValue.Text = "For Sales Values"
        Me.chkForSalesValue.UseVisualStyleBackColor = True
        '
        'lblSMS
        '
        Me.lblSMS.AutoSize = True
        Me.lblSMS.Location = New System.Drawing.Point(4, 3)
        Me.lblSMS.Name = "lblSMS"
        Me.lblSMS.Size = New System.Drawing.Size(34, 17)
        Me.lblSMS.TabIndex = 0
        Me.lblSMS.Text = "SMS"
        '
        'frmUtlInvNumberFmt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(998, 535)
        Me.Controls.Add(Me.pnlSMS)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.chkUpdateRate)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtDocNo)
        Me.Controls.Add(Me.vsfDocNoSet)
        Me.Controls.Add(Me.fraMonth)
        Me.Controls.Add(Me.vsfEx)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnClose)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(1014, 573)
        Me.Name = "frmUtlInvNumberFmt"
        Me.ShowIcon = False
        Me.Text = "Invoice Settings"
        Me.fraMonth.ResumeLayout(False)
        CType(Me.vsfMnth, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.vsfEx, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfDocNoSet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.pnlSMS.ResumeLayout(False)
        Me.pnlSMS.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents optNone As System.Windows.Forms.RadioButton
    Friend WithEvents optMonthWise As System.Windows.Forms.RadioButton
    Friend WithEvents optDaywise As System.Windows.Forms.RadioButton
    Friend WithEvents fraMonth As System.Windows.Forms.Panel
    Friend WithEvents vsfMnth As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnMnthDefault As C1.Win.C1Input.C1SplitButton
    Friend WithEvents Def1 As C1.Win.C1Input.DropDownItem
    Friend WithEvents Def2 As C1.Win.C1Input.DropDownItem
    Friend WithEvents Def3 As C1.Win.C1Input.DropDownItem
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents vsfEx As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents vsfDocNoSet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents txtDocNo As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents chkCounterWsRst As System.Windows.Forms.CheckBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents cboInvAltF8 As ComboBox
    Friend WithEvents cboInvCtrlF8 As ComboBox
    Friend WithEvents cboInvF8 As ComboBox
    Friend WithEvents lblAltF8 As Label
    Friend WithEvents lblCtrlF8 As Label
    Friend WithEvents lblF12 As Label
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents btnSave As C1.Win.C1Input.C1Button
    Friend WithEvents chkUpdateRate As CheckBox
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Label5 As Label
    Friend WithEvents pnlSMS As Panel
    Friend WithEvents chkForDeletion As CheckBox
    Friend WithEvents chkForCancellation As CheckBox
    Friend WithEvents chkForSalesValue As CheckBox
    Friend WithEvents lblSMS As Label
    Friend WithEvents txtSalesMsg As TextBox
    Friend WithEvents lblMessage As Label
    Friend WithEvents txtValue As TextBox
    Friend WithEvents lblValue As Label
    Friend WithEvents txtDelMsg As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents txtCancelMsg As TextBox
    Friend WithEvents Label6 As Label
End Class
