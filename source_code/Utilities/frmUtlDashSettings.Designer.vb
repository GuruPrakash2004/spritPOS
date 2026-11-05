<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlDashSettings
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlDashSettings))
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnBkg = New C1.Win.C1Input.C1Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.pnlImage = New System.Windows.Forms.GroupBox()
        Me.btnDefaultColrs = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblHdr = New System.Windows.Forms.Label()
        Me.btnFValue = New System.Windows.Forms.Button()
        Me.btnFHdr = New System.Windows.Forms.Button()
        Me.chkSettingsShow = New System.Windows.Forms.CheckBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.lblStk = New System.Windows.Forms.Label()
        Me.lblMisc = New System.Windows.Forms.Label()
        Me.lblPB = New System.Windows.Forms.Label()
        Me.lblSales = New System.Windows.Forms.Label()
        Me.pnlDet1 = New System.Windows.Forms.Panel()
        Me.lblHdrSize = New System.Windows.Forms.Label()
        Me.lblHdrFont = New System.Windows.Forms.Label()
        Me.lblHdrStyle = New System.Windows.Forms.Label()
        Me.btnClrCancel = New System.Windows.Forms.Button()
        Me.pnlDet2 = New System.Windows.Forms.Panel()
        Me.lblinfoFont = New System.Windows.Forms.Label()
        Me.lblInfoStyle = New System.Windows.Forms.Label()
        Me.lblinfoSize = New System.Windows.Forms.Label()
        Me.btnClrSave = New System.Windows.Forms.Button()
        Me.pnlDet3 = New System.Windows.Forms.Panel()
        Me.pnlDet4 = New System.Windows.Forms.Panel()
        Me.chkShow = New System.Windows.Forms.CheckBox()
        Me.PnlD1 = New System.Windows.Forms.Panel()
        Me.cboInfoType = New System.Windows.Forms.ComboBox()
        Me.cboSecHdr = New System.Windows.Forms.ComboBox()
        Me.txtInfoHdr = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtGrpHdr = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtOrderNo = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblRemarks = New System.Windows.Forms.Label()
        Me.lable = New System.Windows.Forms.Label()
        Me.LblDashQuery = New System.Windows.Forms.Label()
        Me.chkVisible = New System.Windows.Forms.CheckBox()
        Me.TxtdashQuery = New System.Windows.Forms.TextBox()
        Me.TxtDashid = New System.Windows.Forms.TextBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.BtnSave = New System.Windows.Forms.Button()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.CDColor = New System.Windows.Forms.ColorDialog()
        Me.btnMoveUp = New System.Windows.Forms.Button()
        Me.btnMoveDown = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cdlg = New System.Windows.Forms.FontDialog()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlImage.SuspendLayout()
        Me.pnlDet1.SuspendLayout()
        Me.pnlDet2.SuspendLayout()
        Me.PnlD1.SuspendLayout()
        Me.SuspendLayout()
        '
        'vsfDet
        '
        Me.vsfDet.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "2,1,0,0,0,110,Columns:"
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.vsfDet.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(41, 29)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 22
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(621, 387)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 226
        Me.vsfDet.Tag = ""
        '
        'btnBkg
        '
        Me.btnBkg.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBkg.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBkg.Image = Global.vLQMS.My.Resources.Resources.ucCamera
        Me.btnBkg.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnBkg.Location = New System.Drawing.Point(445, 424)
        Me.btnBkg.Margin = New System.Windows.Forms.Padding(4)
        Me.btnBkg.Name = "btnBkg"
        Me.btnBkg.Size = New System.Drawing.Size(124, 31)
        Me.btnBkg.TabIndex = 227
        Me.btnBkg.Text = "Background"
        Me.btnBkg.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnBkg, "Click To Change Colors")
        Me.btnBkg.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(578, 424)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(85, 30)
        Me.btnClose.TabIndex = 223
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close (Alt + O)")
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'pnlImage
        '
        Me.pnlImage.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlImage.Controls.Add(Me.btnDefaultColrs)
        Me.pnlImage.Controls.Add(Me.Label4)
        Me.pnlImage.Controls.Add(Me.lblHdr)
        Me.pnlImage.Controls.Add(Me.btnFValue)
        Me.pnlImage.Controls.Add(Me.btnFHdr)
        Me.pnlImage.Controls.Add(Me.chkSettingsShow)
        Me.pnlImage.Controls.Add(Me.Label8)
        Me.pnlImage.Controls.Add(Me.lblStk)
        Me.pnlImage.Controls.Add(Me.lblMisc)
        Me.pnlImage.Controls.Add(Me.lblPB)
        Me.pnlImage.Controls.Add(Me.lblSales)
        Me.pnlImage.Controls.Add(Me.pnlDet1)
        Me.pnlImage.Controls.Add(Me.btnClrCancel)
        Me.pnlImage.Controls.Add(Me.pnlDet2)
        Me.pnlImage.Controls.Add(Me.btnClrSave)
        Me.pnlImage.Controls.Add(Me.pnlDet3)
        Me.pnlImage.Controls.Add(Me.pnlDet4)
        Me.pnlImage.Location = New System.Drawing.Point(11, 12)
        Me.pnlImage.Name = "pnlImage"
        Me.pnlImage.Size = New System.Drawing.Size(653, 444)
        Me.pnlImage.TabIndex = 229
        Me.pnlImage.TabStop = False
        Me.pnlImage.Text = "Background Color"
        '
        'btnDefaultColrs
        '
        Me.btnDefaultColrs.BackgroundImage = Global.vLQMS.My.Resources.Resources.setting1
        Me.btnDefaultColrs.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnDefaultColrs.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDefaultColrs.Location = New System.Drawing.Point(315, 19)
        Me.btnDefaultColrs.Name = "btnDefaultColrs"
        Me.btnDefaultColrs.Size = New System.Drawing.Size(26, 23)
        Me.btnDefaultColrs.TabIndex = 660
        Me.btnDefaultColrs.Text = "Button1"
        Me.ToolTip1.SetToolTip(Me.btnDefaultColrs, "Default Color Settings")
        Me.btnDefaultColrs.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.Color.Blue
        Me.Label4.Location = New System.Drawing.Point(7, 419)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(314, 13)
        Me.Label4.TabIndex = 659
        Me.Label4.Text = "Click On Sales/Purchase/Stock/Misc To Change the Font Color "
        '
        'lblHdr
        '
        Me.lblHdr.AutoSize = True
        Me.lblHdr.Location = New System.Drawing.Point(360, 19)
        Me.lblHdr.Name = "lblHdr"
        Me.lblHdr.Size = New System.Drawing.Size(42, 13)
        Me.lblHdr.TabIndex = 6
        Me.lblHdr.Text = "Dummy"
        Me.lblHdr.Visible = False
        '
        'btnFValue
        '
        Me.btnFValue.BackColor = System.Drawing.Color.LightGreen
        Me.btnFValue.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnFValue.Location = New System.Drawing.Point(10, 19)
        Me.btnFValue.Name = "btnFValue"
        Me.btnFValue.Size = New System.Drawing.Size(128, 23)
        Me.btnFValue.TabIndex = 658
        Me.btnFValue.Text = "Select Font For Values"
        Me.ToolTip1.SetToolTip(Me.btnFValue, "Set Color to Values")
        Me.btnFValue.UseVisualStyleBackColor = False
        '
        'btnFHdr
        '
        Me.btnFHdr.BackColor = System.Drawing.Color.LightGreen
        Me.btnFHdr.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnFHdr.Location = New System.Drawing.Point(144, 19)
        Me.btnFHdr.Name = "btnFHdr"
        Me.btnFHdr.Size = New System.Drawing.Size(165, 23)
        Me.btnFHdr.TabIndex = 657
        Me.btnFHdr.Text = "Select Font  for Sub Heading"
        Me.ToolTip1.SetToolTip(Me.btnFHdr, "Set color to SubHeadings")
        Me.btnFHdr.UseVisualStyleBackColor = False
        '
        'chkSettingsShow
        '
        Me.chkSettingsShow.AutoSize = True
        Me.chkSettingsShow.Location = New System.Drawing.Point(380, 401)
        Me.chkSettingsShow.Name = "chkSettingsShow"
        Me.chkSettingsShow.Size = New System.Drawing.Size(97, 17)
        Me.chkSettingsShow.TabIndex = 656
        Me.chkSettingsShow.Text = "Apply Changes"
        Me.chkSettingsShow.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.ForeColor = System.Drawing.Color.Blue
        Me.Label8.Location = New System.Drawing.Point(7, 401)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(244, 13)
        Me.Label8.TabIndex = 64
        Me.Label8.Text = "Click On Panel To Change the BackGround Color "
        '
        'lblStk
        '
        Me.lblStk.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblStk.BackColor = System.Drawing.SystemColors.ControlDark
        Me.lblStk.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblStk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStk.Location = New System.Drawing.Point(6, 243)
        Me.lblStk.Name = "lblStk"
        Me.lblStk.Size = New System.Drawing.Size(315, 22)
        Me.lblStk.TabIndex = 63
        Me.lblStk.Text = "Stock"
        Me.lblStk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ToolTip1.SetToolTip(Me.lblStk, "Click here to Change the Font Color")
        '
        'lblMisc
        '
        Me.lblMisc.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMisc.BackColor = System.Drawing.SystemColors.ControlDark
        Me.lblMisc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblMisc.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMisc.Location = New System.Drawing.Point(328, 243)
        Me.lblMisc.Name = "lblMisc"
        Me.lblMisc.Size = New System.Drawing.Size(315, 22)
        Me.lblMisc.TabIndex = 62
        Me.lblMisc.Text = "Misc"
        Me.lblMisc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ToolTip1.SetToolTip(Me.lblMisc, "Click here to Change the Font Color")
        '
        'lblPB
        '
        Me.lblPB.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPB.BackColor = System.Drawing.SystemColors.ControlDark
        Me.lblPB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPB.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPB.Location = New System.Drawing.Point(327, 62)
        Me.lblPB.Name = "lblPB"
        Me.lblPB.Size = New System.Drawing.Size(315, 23)
        Me.lblPB.TabIndex = 61
        Me.lblPB.Text = "Purchase"
        Me.lblPB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ToolTip1.SetToolTip(Me.lblPB, "Click here to Change the Font Color")
        '
        'lblSales
        '
        Me.lblSales.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSales.BackColor = System.Drawing.SystemColors.ControlDark
        Me.lblSales.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblSales.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSales.Location = New System.Drawing.Point(7, 62)
        Me.lblSales.Name = "lblSales"
        Me.lblSales.Size = New System.Drawing.Size(315, 23)
        Me.lblSales.TabIndex = 60
        Me.lblSales.Text = "Sales"
        Me.lblSales.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ToolTip1.SetToolTip(Me.lblSales, "Click here to Change the Font Color")
        '
        'pnlDet1
        '
        Me.pnlDet1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlDet1.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(187, Byte), Integer), CType(CType(41, Byte), Integer))
        Me.pnlDet1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlDet1.Controls.Add(Me.lblHdrSize)
        Me.pnlDet1.Controls.Add(Me.lblHdrFont)
        Me.pnlDet1.Controls.Add(Me.lblHdrStyle)
        Me.pnlDet1.Location = New System.Drawing.Point(7, 91)
        Me.pnlDet1.Name = "pnlDet1"
        Me.pnlDet1.Size = New System.Drawing.Size(315, 138)
        Me.pnlDet1.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.pnlDet1, "Click on panel To Change BackGround Color Of Sales")
        '
        'lblHdrSize
        '
        Me.lblHdrSize.AutoSize = True
        Me.lblHdrSize.Location = New System.Drawing.Point(46, 34)
        Me.lblHdrSize.Name = "lblHdrSize"
        Me.lblHdrSize.Size = New System.Drawing.Size(19, 13)
        Me.lblHdrSize.TabIndex = 6
        Me.lblHdrSize.Text = "14"
        Me.lblHdrSize.Visible = False
        '
        'lblHdrFont
        '
        Me.lblHdrFont.AutoSize = True
        Me.lblHdrFont.Location = New System.Drawing.Point(37, 10)
        Me.lblHdrFont.Name = "lblHdrFont"
        Me.lblHdrFont.Size = New System.Drawing.Size(52, 13)
        Me.lblHdrFont.TabIndex = 0
        Me.lblHdrFont.Text = "Segoe UI"
        Me.lblHdrFont.Visible = False
        '
        'lblHdrStyle
        '
        Me.lblHdrStyle.AutoSize = True
        Me.lblHdrStyle.Location = New System.Drawing.Point(43, 57)
        Me.lblHdrStyle.Name = "lblHdrStyle"
        Me.lblHdrStyle.Size = New System.Drawing.Size(75, 13)
        Me.lblHdrStyle.TabIndex = 3
        Me.lblHdrStyle.Text = "FontStyle.Bold"
        Me.lblHdrStyle.Visible = False
        '
        'btnClrCancel
        '
        Me.btnClrCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClrCancel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnClrCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClrCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClrCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClrCancel.ForeColor = System.Drawing.Color.Black
        Me.btnClrCancel.ImageIndex = 0
        Me.btnClrCancel.Location = New System.Drawing.Point(565, 393)
        Me.btnClrCancel.Name = "btnClrCancel"
        Me.btnClrCancel.Size = New System.Drawing.Size(78, 30)
        Me.btnClrCancel.TabIndex = 59
        Me.btnClrCancel.Text = "&Cancel"
        Me.btnClrCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClrCancel, "Alt + C To Cancel Changes")
        '
        'pnlDet2
        '
        Me.pnlDet2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlDet2.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(83, Byte), Integer), CType(CType(173, Byte), Integer))
        Me.pnlDet2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlDet2.Controls.Add(Me.lblinfoFont)
        Me.pnlDet2.Controls.Add(Me.lblInfoStyle)
        Me.pnlDet2.Controls.Add(Me.lblinfoSize)
        Me.pnlDet2.Location = New System.Drawing.Point(327, 91)
        Me.pnlDet2.Name = "pnlDet2"
        Me.pnlDet2.Size = New System.Drawing.Size(315, 138)
        Me.pnlDet2.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.pnlDet2, "Click on panel To Change BackGround Color Of Purchase")
        '
        'lblinfoFont
        '
        Me.lblinfoFont.AutoSize = True
        Me.lblinfoFont.Location = New System.Drawing.Point(22, 10)
        Me.lblinfoFont.Name = "lblinfoFont"
        Me.lblinfoFont.Size = New System.Drawing.Size(52, 13)
        Me.lblinfoFont.TabIndex = 5
        Me.lblinfoFont.Text = "Segoe UI"
        Me.lblinfoFont.Visible = False
        '
        'lblInfoStyle
        '
        Me.lblInfoStyle.AutoSize = True
        Me.lblInfoStyle.Location = New System.Drawing.Point(22, 57)
        Me.lblInfoStyle.Name = "lblInfoStyle"
        Me.lblInfoStyle.Size = New System.Drawing.Size(91, 13)
        Me.lblInfoStyle.TabIndex = 2
        Me.lblInfoStyle.Text = "FontStyle.Regular"
        Me.lblInfoStyle.Visible = False
        '
        'lblinfoSize
        '
        Me.lblinfoSize.AutoSize = True
        Me.lblinfoSize.Location = New System.Drawing.Point(22, 34)
        Me.lblinfoSize.Name = "lblinfoSize"
        Me.lblinfoSize.Size = New System.Drawing.Size(19, 13)
        Me.lblinfoSize.TabIndex = 1
        Me.lblinfoSize.Text = "12"
        Me.lblinfoSize.Visible = False
        '
        'btnClrSave
        '
        Me.btnClrSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClrSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnClrSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClrSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClrSave.ForeColor = System.Drawing.Color.Black
        Me.btnClrSave.ImageIndex = 0
        Me.btnClrSave.Location = New System.Drawing.Point(483, 393)
        Me.btnClrSave.Name = "btnClrSave"
        Me.btnClrSave.Size = New System.Drawing.Size(75, 30)
        Me.btnClrSave.TabIndex = 58
        Me.btnClrSave.Text = "&Save"
        Me.btnClrSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClrSave, "Alt + S To Save The Changes")
        '
        'pnlDet3
        '
        Me.pnlDet3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlDet3.BackColor = System.Drawing.Color.FromArgb(CType(CType(69, Byte), Integer), CType(CType(137, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.pnlDet3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlDet3.Location = New System.Drawing.Point(6, 268)
        Me.pnlDet3.Name = "pnlDet3"
        Me.pnlDet3.Size = New System.Drawing.Size(315, 121)
        Me.pnlDet3.TabIndex = 2
        Me.ToolTip1.SetToolTip(Me.pnlDet3, "Click on panel To Change BackGround Color Of Stock")
        '
        'pnlDet4
        '
        Me.pnlDet4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlDet4.BackColor = System.Drawing.Color.FromArgb(CType(CType(90, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(163, Byte), Integer))
        Me.pnlDet4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlDet4.Location = New System.Drawing.Point(328, 268)
        Me.pnlDet4.Name = "pnlDet4"
        Me.pnlDet4.Size = New System.Drawing.Size(315, 121)
        Me.pnlDet4.TabIndex = 1
        Me.ToolTip1.SetToolTip(Me.pnlDet4, "Click on panel To Change BackGround Color Of Misc")
        '
        'chkShow
        '
        Me.chkShow.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkShow.AutoSize = True
        Me.chkShow.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkShow.Location = New System.Drawing.Point(303, 426)
        Me.chkShow.Name = "chkShow"
        Me.chkShow.Size = New System.Drawing.Size(135, 22)
        Me.chkShow.TabIndex = 650
        Me.chkShow.Text = "Show on startup"
        Me.chkShow.UseVisualStyleBackColor = True
        '
        'PnlD1
        '
        Me.PnlD1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.PnlD1.BackColor = System.Drawing.Color.LightSkyBlue
        Me.PnlD1.Controls.Add(Me.cboInfoType)
        Me.PnlD1.Controls.Add(Me.cboSecHdr)
        Me.PnlD1.Controls.Add(Me.txtInfoHdr)
        Me.PnlD1.Controls.Add(Me.Label3)
        Me.PnlD1.Controls.Add(Me.txtGrpHdr)
        Me.PnlD1.Controls.Add(Me.Label2)
        Me.PnlD1.Controls.Add(Me.txtOrderNo)
        Me.PnlD1.Controls.Add(Me.Label1)
        Me.PnlD1.Controls.Add(Me.lblRemarks)
        Me.PnlD1.Controls.Add(Me.lable)
        Me.PnlD1.Controls.Add(Me.LblDashQuery)
        Me.PnlD1.Controls.Add(Me.chkVisible)
        Me.PnlD1.Controls.Add(Me.TxtdashQuery)
        Me.PnlD1.Controls.Add(Me.TxtDashid)
        Me.PnlD1.Controls.Add(Me.btnCancel)
        Me.PnlD1.Controls.Add(Me.BtnSave)
        Me.PnlD1.Location = New System.Drawing.Point(81, 58)
        Me.PnlD1.Name = "PnlD1"
        Me.PnlD1.Size = New System.Drawing.Size(572, 324)
        Me.PnlD1.TabIndex = 651
        '
        'cboInfoType
        '
        Me.cboInfoType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboInfoType.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboInfoType.FormattingEnabled = True
        Me.cboInfoType.Items.AddRange(New Object() {"SINGLEVALUE", "GRIDVALUE"})
        Me.cboInfoType.Location = New System.Drawing.Point(92, 206)
        Me.cboInfoType.Name = "cboInfoType"
        Me.cboInfoType.Size = New System.Drawing.Size(276, 24)
        Me.cboInfoType.TabIndex = 74
        '
        'cboSecHdr
        '
        Me.cboSecHdr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSecHdr.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSecHdr.FormattingEnabled = True
        Me.cboSecHdr.Items.AddRange(New Object() {"SALES", "PURCHASE", "STOCK", "MISC"})
        Me.cboSecHdr.Location = New System.Drawing.Point(92, 92)
        Me.cboSecHdr.Name = "cboSecHdr"
        Me.cboSecHdr.Size = New System.Drawing.Size(276, 24)
        Me.cboSecHdr.TabIndex = 73
        '
        'txtInfoHdr
        '
        Me.txtInfoHdr.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInfoHdr.Location = New System.Drawing.Point(92, 166)
        Me.txtInfoHdr.MaxLength = 50
        Me.txtInfoHdr.Name = "txtInfoHdr"
        Me.txtInfoHdr.Size = New System.Drawing.Size(276, 22)
        Me.txtInfoHdr.TabIndex = 72
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Black
        Me.Label3.Location = New System.Drawing.Point(8, 170)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(63, 13)
        Me.Label3.TabIndex = 71
        Me.Label3.Text = "Info Header"
        '
        'txtGrpHdr
        '
        Me.txtGrpHdr.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGrpHdr.Location = New System.Drawing.Point(92, 129)
        Me.txtGrpHdr.MaxLength = 50
        Me.txtGrpHdr.Name = "txtGrpHdr"
        Me.txtGrpHdr.Size = New System.Drawing.Size(276, 22)
        Me.txtGrpHdr.TabIndex = 70
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Red
        Me.Label2.Location = New System.Drawing.Point(8, 135)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(74, 13)
        Me.Label2.TabIndex = 69
        Me.Label2.Text = "Group Header"
        '
        'txtOrderNo
        '
        Me.txtOrderNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOrderNo.Location = New System.Drawing.Point(92, 129)
        Me.txtOrderNo.Name = "txtOrderNo"
        Me.txtOrderNo.Size = New System.Drawing.Size(276, 22)
        Me.txtOrderNo.TabIndex = 68
        Me.txtOrderNo.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Red
        Me.Label1.Location = New System.Drawing.Point(8, 135)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(47, 13)
        Me.Label1.TabIndex = 67
        Me.Label1.Text = "OrderNo"
        Me.Label1.Visible = False
        '
        'lblRemarks
        '
        Me.lblRemarks.AutoSize = True
        Me.lblRemarks.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRemarks.ForeColor = System.Drawing.Color.Red
        Me.lblRemarks.Location = New System.Drawing.Point(8, 210)
        Me.lblRemarks.Name = "lblRemarks"
        Me.lblRemarks.Size = New System.Drawing.Size(52, 13)
        Me.lblRemarks.TabIndex = 66
        Me.lblRemarks.Text = "Info Type"
        '
        'lable
        '
        Me.lable.AutoSize = True
        Me.lable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lable.ForeColor = System.Drawing.Color.Red
        Me.lable.Location = New System.Drawing.Point(8, 97)
        Me.lable.Name = "lable"
        Me.lable.Size = New System.Drawing.Size(81, 13)
        Me.lable.TabIndex = 64
        Me.lable.Text = "Section Header"
        '
        'LblDashQuery
        '
        Me.LblDashQuery.AutoSize = True
        Me.LblDashQuery.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblDashQuery.ForeColor = System.Drawing.Color.Red
        Me.LblDashQuery.Location = New System.Drawing.Point(8, 45)
        Me.LblDashQuery.Name = "LblDashQuery"
        Me.LblDashQuery.Size = New System.Drawing.Size(60, 13)
        Me.LblDashQuery.TabIndex = 63
        Me.LblDashQuery.Text = "DashQuery"
        '
        'chkVisible
        '
        Me.chkVisible.AutoSize = True
        Me.chkVisible.Location = New System.Drawing.Point(92, 270)
        Me.chkVisible.Name = "chkVisible"
        Me.chkVisible.Size = New System.Drawing.Size(56, 17)
        Me.chkVisible.TabIndex = 61
        Me.chkVisible.Text = "Visible"
        Me.chkVisible.UseVisualStyleBackColor = True
        '
        'TxtdashQuery
        '
        Me.TxtdashQuery.Location = New System.Drawing.Point(92, 18)
        Me.TxtdashQuery.MaxLength = 2000
        Me.TxtdashQuery.Multiline = True
        Me.TxtdashQuery.Name = "TxtdashQuery"
        Me.TxtdashQuery.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TxtdashQuery.Size = New System.Drawing.Size(471, 63)
        Me.TxtdashQuery.TabIndex = 59
        '
        'TxtDashid
        '
        Me.TxtDashid.Location = New System.Drawing.Point(533, 87)
        Me.TxtDashid.Name = "TxtDashid"
        Me.TxtDashid.Size = New System.Drawing.Size(30, 20)
        Me.TxtDashid.TabIndex = 2
        Me.TxtDashid.Visible = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.ForeColor = System.Drawing.Color.Black
        Me.btnCancel.ImageIndex = 0
        Me.btnCancel.Location = New System.Drawing.Point(478, 280)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(78, 30)
        Me.btnCancel.TabIndex = 57
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'BtnSave
        '
        Me.BtnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.BtnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSave.ForeColor = System.Drawing.Color.Black
        Me.BtnSave.ImageIndex = 0
        Me.BtnSave.Location = New System.Drawing.Point(399, 280)
        Me.BtnSave.Name = "BtnSave"
        Me.BtnSave.Size = New System.Drawing.Size(75, 30)
        Me.BtnSave.TabIndex = 56
        Me.BtnSave.Text = "&Save"
        Me.BtnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnEdit
        '
        Me.btnEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnEdit.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_EDIT
        Me.btnEdit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEdit.ForeColor = System.Drawing.Color.Black
        Me.btnEdit.ImageIndex = 0
        Me.btnEdit.Location = New System.Drawing.Point(113, 426)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(68, 30)
        Me.btnEdit.TabIndex = 652
        Me.btnEdit.Text = "&Edit"
        Me.btnEdit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnEdit, "Edit Dash Query (Alt + E)")
        '
        'btnAdd
        '
        Me.btnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAdd.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_NEW
        Me.btnAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAdd.ForeColor = System.Drawing.Color.Black
        Me.btnAdd.ImageIndex = 0
        Me.btnAdd.Location = New System.Drawing.Point(41, 426)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(66, 30)
        Me.btnAdd.TabIndex = 653
        Me.btnAdd.Text = "&Add"
        Me.btnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnAdd, "ADD Dash Query(Alt + A)")
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDelete.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_DELETE
        Me.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDelete.ForeColor = System.Drawing.Color.Black
        Me.btnDelete.ImageIndex = 0
        Me.btnDelete.Location = New System.Drawing.Point(187, 426)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(75, 30)
        Me.btnDelete.TabIndex = 654
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnDelete, "Delete Dash Query (Alt + D)")
        '
        'btnMoveUp
        '
        Me.btnMoveUp.Location = New System.Drawing.Point(11, 147)
        Me.btnMoveUp.Name = "btnMoveUp"
        Me.btnMoveUp.Size = New System.Drawing.Size(22, 34)
        Me.btnMoveUp.TabIndex = 60
        Me.btnMoveUp.Text = "▲"
        Me.ToolTip1.SetToolTip(Me.btnMoveUp, "Move Row Up")
        Me.btnMoveUp.UseVisualStyleBackColor = True
        '
        'btnMoveDown
        '
        Me.btnMoveDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnMoveDown.Location = New System.Drawing.Point(11, 187)
        Me.btnMoveDown.Name = "btnMoveDown"
        Me.btnMoveDown.Size = New System.Drawing.Size(22, 34)
        Me.btnMoveDown.TabIndex = 655
        Me.btnMoveDown.Text = "▼"
        Me.ToolTip1.SetToolTip(Me.btnMoveDown, "Move Row Down")
        Me.btnMoveDown.UseVisualStyleBackColor = True
        '
        'frmUtlDashSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(674, 464)
        Me.Controls.Add(Me.pnlImage)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.PnlD1)
        Me.Controls.Add(Me.btnEdit)
        Me.Controls.Add(Me.chkShow)
        Me.Controls.Add(Me.btnBkg)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.btnMoveDown)
        Me.Controls.Add(Me.btnMoveUp)
        Me.MaximumSize = New System.Drawing.Size(690, 503)
        Me.MinimumSize = New System.Drawing.Size(690, 503)
        Me.Name = "frmUtlDashSettings"
        Me.ShowIcon = False
        Me.Tag = "RptRights"
        Me.Text = "DashBoard Settings"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlImage.ResumeLayout(False)
        Me.pnlImage.PerformLayout()
        Me.pnlDet1.ResumeLayout(False)
        Me.pnlDet1.PerformLayout()
        Me.pnlDet2.ResumeLayout(False)
        Me.pnlDet2.PerformLayout()
        Me.PnlD1.ResumeLayout(False)
        Me.PnlD1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnClose As Button
    Public WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnBkg As C1.Win.C1Input.C1Button
    Friend WithEvents pnlImage As GroupBox
    Friend WithEvents chkShow As CheckBox
    Friend WithEvents PnlD1 As Panel
    Friend WithEvents lblRemarks As Label
    Friend WithEvents lable As Label
    Friend WithEvents LblDashQuery As Label
    Friend WithEvents chkVisible As CheckBox
    Friend WithEvents TxtdashQuery As TextBox
    Friend WithEvents TxtDashid As TextBox
    Friend WithEvents btnCancel As Button
    Friend WithEvents BtnSave As Button
    Friend WithEvents btnEdit As Button
    Friend WithEvents btnAdd As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents cboInfoType As ComboBox
    Friend WithEvents cboSecHdr As ComboBox
    Friend WithEvents txtInfoHdr As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtGrpHdr As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtOrderNo As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents pnlDet2 As Panel
    Friend WithEvents pnlDet3 As Panel
    Friend WithEvents pnlDet4 As Panel
    Friend WithEvents CDColor As ColorDialog
    Friend WithEvents btnClrCancel As Button
    Friend WithEvents btnClrSave As Button
    Friend WithEvents pnlDet1 As Panel
    Friend WithEvents btnMoveUp As Button
    Friend WithEvents btnMoveDown As Button
    Friend WithEvents lblStk As Label
    Friend WithEvents lblMisc As Label
    Friend WithEvents lblPB As Label
    Friend WithEvents lblSales As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents chkSettingsShow As CheckBox
    Friend WithEvents cdlg As FontDialog
    Friend WithEvents lblHdrSize As Label
    Friend WithEvents lblHdrFont As Label
    Friend WithEvents lblHdrStyle As Label
    Friend WithEvents lblinfoFont As Label
    Friend WithEvents lblInfoStyle As Label
    Friend WithEvents lblinfoSize As Label
    Friend WithEvents btnFValue As Button
    Friend WithEvents btnFHdr As Button
    Friend WithEvents lblHdr As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents btnDefaultColrs As Button
End Class
