<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class svtParty
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(svtParty))
        Me.lblParty = New System.Windows.Forms.Label()
        Me.lblAddr = New System.Windows.Forms.Label()
        Me.lblCity = New System.Windows.Forms.Label()
        Me.lblPincode = New System.Windows.Forms.Label()
        Me.lblState = New System.Windows.Forms.Label()
        Me.txtID = New System.Windows.Forms.TextBox()
        Me.btnAddr = New System.Windows.Forms.Button()
        Me.cboState = New System.Windows.Forms.ComboBox()
        Me.txtCode = New System.Windows.Forms.TextBox()
        Me.txtPincode = New System.Windows.Forms.TextBox()
        Me.txtAdd1 = New System.Windows.Forms.TextBox()
        Me.cboCity = New System.Windows.Forms.ComboBox()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.txtAdd3 = New System.Windows.Forms.TextBox()
        Me.txtAdd2 = New System.Windows.Forms.TextBox()
        Me.cboCountry = New System.Windows.Forms.ComboBox()
        Me.lblCountry = New System.Windows.Forms.Label()
        Me.vsfView = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.txtShipAddID = New System.Windows.Forms.TextBox()
        Me.txtAdd4 = New System.Windows.Forms.TextBox()
        Me.txtAddr = New System.Windows.Forms.TextBox()
        Me.cmsParty = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.tsbCopy = New System.Windows.Forms.ToolStripMenuItem()
        Me.lblShipAddress = New System.Windows.Forms.Label()
        Me.cboShipAddr = New System.Windows.Forms.ComboBox()
        Me.cboContact = New System.Windows.Forms.ComboBox()
        Me.lblContacts = New System.Windows.Forms.Label()
        Me.btnShip = New System.Windows.Forms.Button()
        Me.txtContID = New System.Windows.Forms.TextBox()
        Me.txtShipAddress = New System.Windows.Forms.TextBox()
        Me.flowLayOut = New System.Windows.Forms.FlowLayoutPanel()
        Me.pnlParty = New System.Windows.Forms.Panel()
        Me.PnlAddrComb = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.pnlAddr = New System.Windows.Forms.Panel()
        Me.lblPartyName = New System.Windows.Forms.Label()
        Me.pnlCity = New System.Windows.Forms.Panel()
        Me.PnlState = New System.Windows.Forms.Panel()
        Me.pnlShip = New System.Windows.Forms.Panel()
        Me.pnlShipAddr = New System.Windows.Forms.Panel()
        Me.pnlContact = New System.Windows.Forms.Panel()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmsParty.SuspendLayout()
        Me.flowLayOut.SuspendLayout()
        Me.pnlParty.SuspendLayout()
        Me.PnlAddrComb.SuspendLayout()
        Me.pnlAddr.SuspendLayout()
        Me.pnlCity.SuspendLayout()
        Me.PnlState.SuspendLayout()
        Me.pnlShip.SuspendLayout()
        Me.pnlShipAddr.SuspendLayout()
        Me.pnlContact.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblParty
        '
        Me.lblParty.AutoSize = True
        Me.lblParty.Font = New System.Drawing.Font("Segoe UI", 8.0!)
        Me.lblParty.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblParty.Location = New System.Drawing.Point(0, 5)
        Me.lblParty.Name = "lblParty"
        Me.lblParty.Size = New System.Drawing.Size(32, 13)
        Me.lblParty.TabIndex = 0
        Me.lblParty.Tag = "M"
        Me.lblParty.Text = "Party"
        Me.lblParty.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblAddr
        '
        Me.lblAddr.AutoSize = True
        Me.lblAddr.Font = New System.Drawing.Font("Segoe UI", 8.0!)
        Me.lblAddr.Location = New System.Drawing.Point(6, 7)
        Me.lblAddr.Name = "lblAddr"
        Me.lblAddr.Size = New System.Drawing.Size(48, 13)
        Me.lblAddr.TabIndex = 0
        Me.lblAddr.Text = "Address"
        Me.lblAddr.Visible = False
        '
        'lblCity
        '
        Me.lblCity.AutoSize = True
        Me.lblCity.Font = New System.Drawing.Font("Segoe UI", 8.0!)
        Me.lblCity.Location = New System.Drawing.Point(6, 4)
        Me.lblCity.Name = "lblCity"
        Me.lblCity.Size = New System.Drawing.Size(26, 13)
        Me.lblCity.TabIndex = 0
        Me.lblCity.Text = "City"
        '
        'lblPincode
        '
        Me.lblPincode.AutoSize = True
        Me.lblPincode.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPincode.Location = New System.Drawing.Point(182, 4)
        Me.lblPincode.Name = "lblPincode"
        Me.lblPincode.Size = New System.Drawing.Size(48, 13)
        Me.lblPincode.TabIndex = 0
        Me.lblPincode.Text = "Pincode"
        '
        'lblState
        '
        Me.lblState.AutoSize = True
        Me.lblState.Font = New System.Drawing.Font("Segoe UI", 8.0!)
        Me.lblState.Location = New System.Drawing.Point(6, 6)
        Me.lblState.Name = "lblState"
        Me.lblState.Size = New System.Drawing.Size(33, 13)
        Me.lblState.TabIndex = 0
        Me.lblState.Text = "State"
        '
        'txtID
        '
        Me.txtID.Location = New System.Drawing.Point(9, 58)
        Me.txtID.Margin = New System.Windows.Forms.Padding(1, 1, 0, 0)
        Me.txtID.Name = "txtID"
        Me.txtID.Size = New System.Drawing.Size(33, 22)
        Me.txtID.TabIndex = 3
        Me.txtID.Visible = False
        '
        'btnAddr
        '
        Me.btnAddr.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAddr.Image = CType(resources.GetObject("btnAddr.Image"), System.Drawing.Image)
        Me.btnAddr.Location = New System.Drawing.Point(20, 19)
        Me.btnAddr.Name = "btnAddr"
        Me.btnAddr.Size = New System.Drawing.Size(34, 26)
        Me.btnAddr.TabIndex = 251
        Me.btnAddr.UseVisualStyleBackColor = True
        Me.btnAddr.Visible = False
        '
        'cboState
        '
        Me.cboState.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cboState.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboState.FormattingEnabled = True
        Me.cboState.Location = New System.Drawing.Point(63, 2)
        Me.cboState.Margin = New System.Windows.Forms.Padding(1, 2, 0, 0)
        Me.cboState.MaxLength = 50
        Me.cboState.Name = "cboState"
        Me.cboState.Size = New System.Drawing.Size(116, 21)
        Me.cboState.TabIndex = 6
        '
        'txtCode
        '
        Me.txtCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCode.Font = New System.Drawing.Font("Segoe UI", 8.0!)
        Me.txtCode.Location = New System.Drawing.Point(60, 1)
        Me.txtCode.Margin = New System.Windows.Forms.Padding(1, 1, 0, 0)
        Me.txtCode.MaxLength = 50
        Me.txtCode.Name = "txtCode"
        Me.txtCode.Size = New System.Drawing.Size(240, 22)
        Me.txtCode.TabIndex = 8
        '
        'txtPincode
        '
        Me.txtPincode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPincode.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPincode.Location = New System.Drawing.Point(229, 1)
        Me.txtPincode.Margin = New System.Windows.Forms.Padding(1, 6, 0, 0)
        Me.txtPincode.MaxLength = 50
        Me.txtPincode.Name = "txtPincode"
        Me.txtPincode.Size = New System.Drawing.Size(71, 22)
        Me.txtPincode.TabIndex = 5
        '
        'txtAdd1
        '
        Me.txtAdd1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAdd1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdd1.Location = New System.Drawing.Point(61, 22)
        Me.txtAdd1.Margin = New System.Windows.Forms.Padding(1, 1, 0, 0)
        Me.txtAdd1.MaxLength = 50
        Me.txtAdd1.Name = "txtAdd1"
        Me.txtAdd1.Size = New System.Drawing.Size(239, 22)
        Me.txtAdd1.TabIndex = 1
        '
        'cboCity
        '
        Me.cboCity.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cboCity.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCity.FormattingEnabled = True
        Me.cboCity.Location = New System.Drawing.Point(62, 1)
        Me.cboCity.Margin = New System.Windows.Forms.Padding(1, 1, 0, 0)
        Me.cboCity.MaxLength = 50
        Me.cboCity.Name = "cboCity"
        Me.cboCity.Size = New System.Drawing.Size(117, 21)
        Me.cboCity.TabIndex = 4
        '
        'txtName
        '
        Me.txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtName.Font = New System.Drawing.Font("Segoe UI", 8.0!)
        Me.txtName.Location = New System.Drawing.Point(61, 1)
        Me.txtName.Margin = New System.Windows.Forms.Padding(1, 1, 0, 0)
        Me.txtName.MaxLength = 100
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(239, 22)
        Me.txtName.TabIndex = 0
        '
        'txtAdd3
        '
        Me.txtAdd3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAdd3.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdd3.Location = New System.Drawing.Point(61, 64)
        Me.txtAdd3.Margin = New System.Windows.Forms.Padding(1, 1, 0, 0)
        Me.txtAdd3.MaxLength = 50
        Me.txtAdd3.Name = "txtAdd3"
        Me.txtAdd3.Size = New System.Drawing.Size(239, 22)
        Me.txtAdd3.TabIndex = 3
        '
        'txtAdd2
        '
        Me.txtAdd2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAdd2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdd2.Location = New System.Drawing.Point(61, 43)
        Me.txtAdd2.Margin = New System.Windows.Forms.Padding(1, 1, 0, 0)
        Me.txtAdd2.MaxLength = 50
        Me.txtAdd2.Name = "txtAdd2"
        Me.txtAdd2.Size = New System.Drawing.Size(239, 22)
        Me.txtAdd2.TabIndex = 2
        '
        'cboCountry
        '
        Me.cboCountry.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cboCountry.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCountry.FormattingEnabled = True
        Me.cboCountry.Location = New System.Drawing.Point(230, 2)
        Me.cboCountry.Margin = New System.Windows.Forms.Padding(1, 1, 0, 0)
        Me.cboCountry.MaxLength = 50
        Me.cboCountry.Name = "cboCountry"
        Me.cboCountry.Size = New System.Drawing.Size(70, 21)
        Me.cboCountry.TabIndex = 7
        '
        'lblCountry
        '
        Me.lblCountry.AutoSize = True
        Me.lblCountry.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCountry.Location = New System.Drawing.Point(181, 5)
        Me.lblCountry.Name = "lblCountry"
        Me.lblCountry.Size = New System.Drawing.Size(48, 13)
        Me.lblCountry.TabIndex = 0
        Me.lblCountry.Text = "Country"
        '
        'vsfView
        '
        Me.vsfView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfView.BackColor = System.Drawing.Color.WhiteSmoke
        Me.vsfView.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfView.ColumnInfo = "10,0,0,0,0,105,Columns:"
        Me.vsfView.ExtendLastCol = True
        Me.vsfView.Font = New System.Drawing.Font("Segoe UI", 8.0!)
        Me.vsfView.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
        Me.vsfView.Location = New System.Drawing.Point(32, 24)
        Me.vsfView.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfView.Name = "vsfView"
        Me.vsfView.Rows.DefaultSize = 21
        Me.vsfView.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfView.Size = New System.Drawing.Size(289, 51)
        Me.vsfView.StyleInfo = resources.GetString("vsfView.StyleInfo")
        Me.vsfView.TabIndex = 256
        Me.vsfView.Visible = False
        '
        'txtShipAddID
        '
        Me.txtShipAddID.Location = New System.Drawing.Point(9, 35)
        Me.txtShipAddID.Margin = New System.Windows.Forms.Padding(1, 1, 0, 0)
        Me.txtShipAddID.Name = "txtShipAddID"
        Me.txtShipAddID.Size = New System.Drawing.Size(33, 22)
        Me.txtShipAddID.TabIndex = 3
        Me.txtShipAddID.Visible = False
        '
        'txtAdd4
        '
        Me.txtAdd4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAdd4.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdd4.Location = New System.Drawing.Point(61, 85)
        Me.txtAdd4.Margin = New System.Windows.Forms.Padding(1, 1, 0, 0)
        Me.txtAdd4.MaxLength = 50
        Me.txtAdd4.Name = "txtAdd4"
        Me.txtAdd4.Size = New System.Drawing.Size(239, 22)
        Me.txtAdd4.TabIndex = 4
        '
        'txtAddr
        '
        Me.txtAddr.ContextMenuStrip = Me.cmsParty
        Me.txtAddr.Font = New System.Drawing.Font("Segoe UI", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddr.Location = New System.Drawing.Point(60, 1)
        Me.txtAddr.MaxLength = 200
        Me.txtAddr.Multiline = True
        Me.txtAddr.Name = "txtAddr"
        Me.txtAddr.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtAddr.Size = New System.Drawing.Size(240, 50)
        Me.txtAddr.TabIndex = 1
        '
        'cmsParty
        '
        Me.cmsParty.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbCopy})
        Me.cmsParty.Name = "cmsParty"
        Me.cmsParty.Size = New System.Drawing.Size(103, 26)
        '
        'tsbCopy
        '
        Me.tsbCopy.Name = "tsbCopy"
        Me.tsbCopy.Size = New System.Drawing.Size(102, 22)
        Me.tsbCopy.Text = "Copy"
        '
        'lblShipAddress
        '
        Me.lblShipAddress.AutoSize = True
        Me.lblShipAddress.Font = New System.Drawing.Font("Segoe UI", 8.0!)
        Me.lblShipAddress.Location = New System.Drawing.Point(6, 6)
        Me.lblShipAddress.Name = "lblShipAddress"
        Me.lblShipAddress.Size = New System.Drawing.Size(98, 13)
        Me.lblShipAddress.TabIndex = 0
        Me.lblShipAddress.Text = "Shipping Address"
        '
        'cboShipAddr
        '
        Me.cboShipAddr.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cboShipAddr.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboShipAddr.FormattingEnabled = True
        Me.cboShipAddr.Location = New System.Drawing.Point(108, 2)
        Me.cboShipAddr.Margin = New System.Windows.Forms.Padding(1, 2, 0, 0)
        Me.cboShipAddr.MaxLength = 100
        Me.cboShipAddr.Name = "cboShipAddr"
        Me.cboShipAddr.Size = New System.Drawing.Size(164, 21)
        Me.cboShipAddr.TabIndex = 6
        '
        'cboContact
        '
        Me.cboContact.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cboContact.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboContact.FormattingEnabled = True
        Me.cboContact.Location = New System.Drawing.Point(91, 2)
        Me.cboContact.Margin = New System.Windows.Forms.Padding(1, 2, 0, 0)
        Me.cboContact.MaxLength = 150
        Me.cboContact.Name = "cboContact"
        Me.cboContact.Size = New System.Drawing.Size(209, 21)
        Me.cboContact.TabIndex = 6
        '
        'lblContacts
        '
        Me.lblContacts.AutoSize = True
        Me.lblContacts.Font = New System.Drawing.Font("Segoe UI", 8.0!)
        Me.lblContacts.Location = New System.Drawing.Point(6, 6)
        Me.lblContacts.Name = "lblContacts"
        Me.lblContacts.Size = New System.Drawing.Size(85, 13)
        Me.lblContacts.TabIndex = 0
        Me.lblContacts.Text = "Contact Person"
        '
        'btnShip
        '
        Me.btnShip.Location = New System.Drawing.Point(274, 1)
        Me.btnShip.Name = "btnShip"
        Me.btnShip.Size = New System.Drawing.Size(26, 23)
        Me.btnShip.TabIndex = 257
        Me.btnShip.Text = "..."
        Me.btnShip.UseVisualStyleBackColor = True
        '
        'txtContID
        '
        Me.txtContID.Location = New System.Drawing.Point(9, 81)
        Me.txtContID.Margin = New System.Windows.Forms.Padding(1, 1, 0, 0)
        Me.txtContID.Name = "txtContID"
        Me.txtContID.Size = New System.Drawing.Size(33, 22)
        Me.txtContID.TabIndex = 3
        Me.txtContID.Visible = False
        '
        'txtShipAddress
        '
        Me.txtShipAddress.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtShipAddress.Location = New System.Drawing.Point(62, 1)
        Me.txtShipAddress.MaxLength = 200
        Me.txtShipAddress.Multiline = True
        Me.txtShipAddress.Name = "txtShipAddress"
        Me.txtShipAddress.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtShipAddress.Size = New System.Drawing.Size(238, 24)
        Me.txtShipAddress.TabIndex = 258
        '
        'flowLayOut
        '
        Me.flowLayOut.AutoScroll = True
        Me.flowLayOut.Controls.Add(Me.pnlParty)
        Me.flowLayOut.Controls.Add(Me.PnlAddrComb)
        Me.flowLayOut.Controls.Add(Me.pnlAddr)
        Me.flowLayOut.Controls.Add(Me.pnlCity)
        Me.flowLayOut.Controls.Add(Me.PnlState)
        Me.flowLayOut.Controls.Add(Me.pnlShip)
        Me.flowLayOut.Controls.Add(Me.pnlShipAddr)
        Me.flowLayOut.Controls.Add(Me.pnlContact)
        Me.flowLayOut.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.flowLayOut.Location = New System.Drawing.Point(0, 0)
        Me.flowLayOut.Name = "flowLayOut"
        Me.flowLayOut.Size = New System.Drawing.Size(323, 75)
        Me.flowLayOut.TabIndex = 260
        '
        'pnlParty
        '
        Me.pnlParty.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlParty.Controls.Add(Me.txtCode)
        Me.pnlParty.Controls.Add(Me.lblParty)
        Me.pnlParty.Location = New System.Drawing.Point(3, 0)
        Me.pnlParty.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.pnlParty.Name = "pnlParty"
        Me.pnlParty.Size = New System.Drawing.Size(304, 24)
        Me.pnlParty.TabIndex = 0
        '
        'PnlAddrComb
        '
        Me.PnlAddrComb.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PnlAddrComb.Controls.Add(Me.Label1)
        Me.PnlAddrComb.Controls.Add(Me.txtAddr)
        Me.PnlAddrComb.Controls.Add(Me.btnAddr)
        Me.PnlAddrComb.Location = New System.Drawing.Point(3, 24)
        Me.PnlAddrComb.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.PnlAddrComb.Name = "PnlAddrComb"
        Me.PnlAddrComb.Size = New System.Drawing.Size(304, 53)
        Me.PnlAddrComb.TabIndex = 2
        Me.PnlAddrComb.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 8.0!)
        Me.Label1.Location = New System.Drawing.Point(6, 3)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(48, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Address"
        Me.Label1.Visible = False
        '
        'pnlAddr
        '
        Me.pnlAddr.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlAddr.Controls.Add(Me.txtAdd1)
        Me.pnlAddr.Controls.Add(Me.txtName)
        Me.pnlAddr.Controls.Add(Me.lblPartyName)
        Me.pnlAddr.Controls.Add(Me.txtAdd2)
        Me.pnlAddr.Controls.Add(Me.txtAdd3)
        Me.pnlAddr.Controls.Add(Me.txtContID)
        Me.pnlAddr.Controls.Add(Me.txtShipAddID)
        Me.pnlAddr.Controls.Add(Me.txtID)
        Me.pnlAddr.Controls.Add(Me.txtAdd4)
        Me.pnlAddr.Controls.Add(Me.lblAddr)
        Me.pnlAddr.Location = New System.Drawing.Point(3, 77)
        Me.pnlAddr.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.pnlAddr.Name = "pnlAddr"
        Me.pnlAddr.Size = New System.Drawing.Size(304, 108)
        Me.pnlAddr.TabIndex = 1
        Me.pnlAddr.Visible = False
        '
        'lblPartyName
        '
        Me.lblPartyName.AutoSize = True
        Me.lblPartyName.Font = New System.Drawing.Font("Segoe UI", 8.0!)
        Me.lblPartyName.Location = New System.Drawing.Point(22, 87)
        Me.lblPartyName.Name = "lblPartyName"
        Me.lblPartyName.Size = New System.Drawing.Size(32, 13)
        Me.lblPartyName.TabIndex = 0
        Me.lblPartyName.Text = "Party"
        Me.lblPartyName.Visible = False
        '
        'pnlCity
        '
        Me.pnlCity.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlCity.Controls.Add(Me.txtPincode)
        Me.pnlCity.Controls.Add(Me.lblPincode)
        Me.pnlCity.Controls.Add(Me.cboCity)
        Me.pnlCity.Controls.Add(Me.lblCity)
        Me.pnlCity.Location = New System.Drawing.Point(3, 185)
        Me.pnlCity.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.pnlCity.Name = "pnlCity"
        Me.pnlCity.Size = New System.Drawing.Size(304, 24)
        Me.pnlCity.TabIndex = 261
        Me.pnlCity.Visible = False
        '
        'PnlState
        '
        Me.PnlState.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PnlState.Controls.Add(Me.cboCountry)
        Me.PnlState.Controls.Add(Me.lblState)
        Me.PnlState.Controls.Add(Me.cboState)
        Me.PnlState.Controls.Add(Me.lblCountry)
        Me.PnlState.Location = New System.Drawing.Point(3, 209)
        Me.PnlState.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.PnlState.Name = "PnlState"
        Me.PnlState.Size = New System.Drawing.Size(304, 25)
        Me.PnlState.TabIndex = 3
        Me.PnlState.Visible = False
        '
        'pnlShip
        '
        Me.pnlShip.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlShip.Controls.Add(Me.cboShipAddr)
        Me.pnlShip.Controls.Add(Me.btnShip)
        Me.pnlShip.Controls.Add(Me.lblShipAddress)
        Me.pnlShip.Location = New System.Drawing.Point(3, 234)
        Me.pnlShip.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.pnlShip.Name = "pnlShip"
        Me.pnlShip.Size = New System.Drawing.Size(304, 24)
        Me.pnlShip.TabIndex = 4
        Me.pnlShip.Visible = False
        '
        'pnlShipAddr
        '
        Me.pnlShipAddr.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlShipAddr.Controls.Add(Me.txtShipAddress)
        Me.pnlShipAddr.Location = New System.Drawing.Point(3, 258)
        Me.pnlShipAddr.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.pnlShipAddr.Name = "pnlShipAddr"
        Me.pnlShipAddr.Size = New System.Drawing.Size(304, 26)
        Me.pnlShipAddr.TabIndex = 259
        Me.pnlShipAddr.Visible = False
        '
        'pnlContact
        '
        Me.pnlContact.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlContact.Controls.Add(Me.cboContact)
        Me.pnlContact.Controls.Add(Me.lblContacts)
        Me.pnlContact.Location = New System.Drawing.Point(3, 284)
        Me.pnlContact.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.pnlContact.Name = "pnlContact"
        Me.pnlContact.Size = New System.Drawing.Size(304, 26)
        Me.pnlContact.TabIndex = 260
        Me.pnlContact.Visible = False
        '
        'svtParty
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me.vsfView)
        Me.Controls.Add(Me.flowLayOut)
        Me.Name = "svtParty"
        Me.Size = New System.Drawing.Size(326, 79)
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmsParty.ResumeLayout(False)
        Me.flowLayOut.ResumeLayout(False)
        Me.pnlParty.ResumeLayout(False)
        Me.pnlParty.PerformLayout()
        Me.PnlAddrComb.ResumeLayout(False)
        Me.PnlAddrComb.PerformLayout()
        Me.pnlAddr.ResumeLayout(False)
        Me.pnlAddr.PerformLayout()
        Me.pnlCity.ResumeLayout(False)
        Me.pnlCity.PerformLayout()
        Me.PnlState.ResumeLayout(False)
        Me.PnlState.PerformLayout()
        Me.pnlShip.ResumeLayout(False)
        Me.pnlShip.PerformLayout()
        Me.pnlShipAddr.ResumeLayout(False)
        Me.pnlShipAddr.PerformLayout()
        Me.pnlContact.ResumeLayout(False)
        Me.pnlContact.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblParty As System.Windows.Forms.Label
    Friend WithEvents lblAddr As System.Windows.Forms.Label
    Friend WithEvents lblCity As System.Windows.Forms.Label
    Friend WithEvents lblPincode As System.Windows.Forms.Label
    Friend WithEvents lblState As System.Windows.Forms.Label
    Friend WithEvents btnAddr As System.Windows.Forms.Button
    Friend WithEvents cboState As System.Windows.Forms.ComboBox
    Friend WithEvents txtCode As System.Windows.Forms.TextBox
    Friend WithEvents txtPincode As System.Windows.Forms.TextBox
    Friend WithEvents txtAdd1 As System.Windows.Forms.TextBox
    Friend WithEvents cboCity As System.Windows.Forms.ComboBox
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents txtAdd3 As System.Windows.Forms.TextBox
    Friend WithEvents txtAdd2 As System.Windows.Forms.TextBox
    Friend WithEvents lblCountry As System.Windows.Forms.Label
    Friend WithEvents cboCountry As System.Windows.Forms.ComboBox
    Friend WithEvents vsfView As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents txtShipAddID As System.Windows.Forms.TextBox
    Public WithEvents txtID As System.Windows.Forms.TextBox
    Friend WithEvents txtAdd4 As TextBox
    Friend WithEvents txtAddr As TextBox
    Friend WithEvents lblShipAddress As Label
    Friend WithEvents cboShipAddr As ComboBox
    Friend WithEvents cboContact As ComboBox
    Friend WithEvents lblContacts As Label
    Friend WithEvents btnShip As Button
    Public WithEvents txtContID As TextBox
    Friend WithEvents txtShipAddress As TextBox
    Friend WithEvents flowLayOut As FlowLayoutPanel
    Friend WithEvents pnlParty As Panel
    Friend WithEvents pnlAddr As Panel
    Friend WithEvents PnlAddrComb As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents PnlState As Panel
    Friend WithEvents pnlShip As Panel
    Friend WithEvents pnlShipAddr As Panel
    Friend WithEvents pnlContact As Panel
    Friend WithEvents pnlCity As Panel
    Friend WithEvents lblPartyName As Label
    Friend WithEvents cmsParty As ContextMenuStrip
    Friend WithEvents tsbCopy As ToolStripMenuItem
End Class
