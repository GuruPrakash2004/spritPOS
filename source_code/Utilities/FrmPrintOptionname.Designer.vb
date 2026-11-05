<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmPrintOptionname
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmPrintOptionname))
        Me.PnlprntSetting = New System.Windows.Forms.Panel()
        Me.txtordrno = New System.Windows.Forms.TextBox()
        Me.txtid = New System.Windows.Forms.TextBox()
        Me.btnexist = New System.Windows.Forms.Button()
        Me.btndiff = New System.Windows.Forms.Button()
        Me.btntmpl = New System.Windows.Forms.Button()
        Me.btnremv = New System.Windows.Forms.Button()
        Me.txtprtname = New System.Windows.Forms.TextBox()
        Me.Chkdefault = New System.Windows.Forms.CheckBox()
        Me.lblprntname = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.cborpt = New System.Windows.Forms.ComboBox()
        Me.lblrptfile = New System.Windows.Forms.Label()
        Me.lblcopies = New System.Windows.Forms.Label()
        Me.txtcopies = New System.Windows.Forms.TextBox()
        Me.txtfrmla = New System.Windows.Forms.TextBox()
        Me.Lblfrmla = New System.Windows.Forms.Label()
        Me.Grpaftrsav = New System.Windows.Forms.GroupBox()
        Me.Rdodntprnt = New System.Windows.Forms.RadioButton()
        Me.Rdoprompt = New System.Windows.Forms.RadioButton()
        Me.Rdowithoutprmpt = New System.Windows.Forms.RadioButton()
        Me.GrpType = New System.Windows.Forms.GroupBox()
        Me.txtprntname = New System.Windows.Forms.TextBox()
        Me.chkoutlk = New System.Windows.Forms.CheckBox()
        Me.lblprtname = New System.Windows.Forms.Label()
        Me.rdoPrvw = New System.Windows.Forms.RadioButton()
        Me.Rdopdf = New System.Windows.Forms.RadioButton()
        Me.RdoDrctPrnt = New System.Windows.Forms.RadioButton()
        Me.btnSave = New C1.Win.C1Input.C1Button()
        Me.btnCancel = New C1.Win.C1Input.C1Button()
        Me.vsfdtl = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.PnlprntSetting.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Grpaftrsav.SuspendLayout()
        Me.GrpType.SuspendLayout()
        CType(Me.vsfdtl, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PnlprntSetting
        '
        Me.PnlprntSetting.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.PnlprntSetting.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PnlprntSetting.Controls.Add(Me.txtordrno)
        Me.PnlprntSetting.Controls.Add(Me.txtid)
        Me.PnlprntSetting.Controls.Add(Me.btnexist)
        Me.PnlprntSetting.Controls.Add(Me.btndiff)
        Me.PnlprntSetting.Controls.Add(Me.btntmpl)
        Me.PnlprntSetting.Controls.Add(Me.btnremv)
        Me.PnlprntSetting.Controls.Add(Me.txtprtname)
        Me.PnlprntSetting.Controls.Add(Me.Chkdefault)
        Me.PnlprntSetting.Controls.Add(Me.lblprntname)
        Me.PnlprntSetting.Controls.Add(Me.Panel1)
        Me.PnlprntSetting.Controls.Add(Me.Grpaftrsav)
        Me.PnlprntSetting.Controls.Add(Me.GrpType)
        Me.PnlprntSetting.Controls.Add(Me.btnSave)
        Me.PnlprntSetting.Controls.Add(Me.btnCancel)
        Me.PnlprntSetting.Controls.Add(Me.vsfdtl)
        Me.PnlprntSetting.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PnlprntSetting.Location = New System.Drawing.Point(8, 10)
        Me.PnlprntSetting.Name = "PnlprntSetting"
        Me.PnlprntSetting.Size = New System.Drawing.Size(680, 567)
        Me.PnlprntSetting.TabIndex = 234
        '
        'txtordrno
        '
        Me.txtordrno.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.txtordrno.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.txtordrno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtordrno.Font = New System.Drawing.Font("Calibri", 9.75!)
        Me.txtordrno.Location = New System.Drawing.Point(416, 35)
        Me.txtordrno.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtordrno.MaxLength = 15
        Me.txtordrno.Name = "txtordrno"
        Me.txtordrno.Size = New System.Drawing.Size(61, 23)
        Me.txtordrno.TabIndex = 254
        '
        'txtid
        '
        Me.txtid.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.txtid.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.txtid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtid.Font = New System.Drawing.Font("Calibri", 9.75!)
        Me.txtid.Location = New System.Drawing.Point(416, 9)
        Me.txtid.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtid.MaxLength = 15
        Me.txtid.Name = "txtid"
        Me.txtid.Size = New System.Drawing.Size(144, 23)
        Me.txtid.TabIndex = 252
        '
        'btnexist
        '
        Me.btnexist.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnexist.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnexist.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnexist.Location = New System.Drawing.Point(13, 488)
        Me.btnexist.Name = "btnexist"
        Me.btnexist.Size = New System.Drawing.Size(256, 31)
        Me.btnexist.TabIndex = 253
        Me.btnexist.Text = "&Copy From Existing Transaction Different Report"
        Me.btnexist.UseVisualStyleBackColor = True
        '
        'btndiff
        '
        Me.btndiff.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btndiff.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btndiff.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btndiff.Location = New System.Drawing.Point(13, 525)
        Me.btndiff.Name = "btndiff"
        Me.btndiff.Size = New System.Drawing.Size(256, 31)
        Me.btndiff.TabIndex = 252
        Me.btndiff.Text = "&Copy from Different Transaction Report"
        Me.btndiff.UseVisualStyleBackColor = True
        '
        'btntmpl
        '
        Me.btntmpl.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btntmpl.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btntmpl.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btntmpl.Location = New System.Drawing.Point(580, 451)
        Me.btntmpl.Name = "btntmpl"
        Me.btntmpl.Size = New System.Drawing.Size(69, 31)
        Me.btntmpl.TabIndex = 251
        Me.btntmpl.Text = "&Template"
        Me.btntmpl.UseVisualStyleBackColor = True
        '
        'btnremv
        '
        Me.btnremv.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnremv.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnremv.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnremv.Location = New System.Drawing.Point(13, 451)
        Me.btnremv.Name = "btnremv"
        Me.btnremv.Size = New System.Drawing.Size(69, 31)
        Me.btnremv.TabIndex = 250
        Me.btnremv.Text = "&Remove"
        Me.btnremv.UseVisualStyleBackColor = True
        '
        'txtprtname
        '
        Me.txtprtname.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.txtprtname.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.txtprtname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtprtname.Font = New System.Drawing.Font("Calibri", 9.75!)
        Me.txtprtname.Location = New System.Drawing.Point(139, 9)
        Me.txtprtname.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtprtname.MaxLength = 15
        Me.txtprtname.Name = "txtprtname"
        Me.txtprtname.Size = New System.Drawing.Size(255, 23)
        Me.txtprtname.TabIndex = 82
        '
        'Chkdefault
        '
        Me.Chkdefault.AutoSize = True
        Me.Chkdefault.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Chkdefault.Location = New System.Drawing.Point(28, 36)
        Me.Chkdefault.Name = "Chkdefault"
        Me.Chkdefault.Size = New System.Drawing.Size(66, 19)
        Me.Chkdefault.TabIndex = 249
        Me.Chkdefault.Text = "Default"
        Me.Chkdefault.UseVisualStyleBackColor = True
        '
        'lblprntname
        '
        Me.lblprntname.AutoSize = True
        Me.lblprntname.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblprntname.ForeColor = System.Drawing.Color.Black
        Me.lblprntname.Location = New System.Drawing.Point(25, 13)
        Me.lblprntname.Name = "lblprntname"
        Me.lblprntname.Size = New System.Drawing.Size(108, 15)
        Me.lblprntname.TabIndex = 96
        Me.lblprntname.Text = "Print Option Name"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.cborpt)
        Me.Panel1.Controls.Add(Me.lblrptfile)
        Me.Panel1.Controls.Add(Me.lblcopies)
        Me.Panel1.Controls.Add(Me.txtcopies)
        Me.Panel1.Controls.Add(Me.txtfrmla)
        Me.Panel1.Controls.Add(Me.Lblfrmla)
        Me.Panel1.Location = New System.Drawing.Point(15, 169)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(605, 99)
        Me.Panel1.TabIndex = 213
        '
        'cborpt
        '
        Me.cborpt.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cborpt.CausesValidation = False
        Me.cborpt.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cborpt.FormattingEnabled = True
        Me.cborpt.Location = New System.Drawing.Point(125, 6)
        Me.cborpt.Name = "cborpt"
        Me.cborpt.Size = New System.Drawing.Size(154, 23)
        Me.cborpt.TabIndex = 234
        '
        'lblrptfile
        '
        Me.lblrptfile.AutoSize = True
        Me.lblrptfile.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblrptfile.ForeColor = System.Drawing.Color.Black
        Me.lblrptfile.Location = New System.Drawing.Point(5, 10)
        Me.lblrptfile.Name = "lblrptfile"
        Me.lblrptfile.Size = New System.Drawing.Size(82, 15)
        Me.lblrptfile.TabIndex = 81
        Me.lblrptfile.Text = "Rpt File Name"
        '
        'lblcopies
        '
        Me.lblcopies.AutoSize = True
        Me.lblcopies.Font = New System.Drawing.Font("Calibri", 9.75!)
        Me.lblcopies.ForeColor = System.Drawing.Color.Black
        Me.lblcopies.Location = New System.Drawing.Point(5, 37)
        Me.lblcopies.Name = "lblcopies"
        Me.lblcopies.Size = New System.Drawing.Size(104, 15)
        Me.lblcopies.TabIndex = 21
        Me.lblcopies.Text = "Number of Copies"
        '
        'txtcopies
        '
        Me.txtcopies.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.txtcopies.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtcopies.Font = New System.Drawing.Font("Calibri", 9.75!)
        Me.txtcopies.Location = New System.Drawing.Point(125, 34)
        Me.txtcopies.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtcopies.MaxLength = 250
        Me.txtcopies.Name = "txtcopies"
        Me.txtcopies.Size = New System.Drawing.Size(50, 23)
        Me.txtcopies.TabIndex = 1
        Me.txtcopies.Tag = ""
        Me.txtcopies.Text = " "
        '
        'txtfrmla
        '
        Me.txtfrmla.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.txtfrmla.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtfrmla.Font = New System.Drawing.Font("Calibri", 9.75!)
        Me.txtfrmla.Location = New System.Drawing.Point(125, 67)
        Me.txtfrmla.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtfrmla.MaxLength = 250
        Me.txtfrmla.Name = "txtfrmla"
        Me.txtfrmla.Size = New System.Drawing.Size(449, 23)
        Me.txtfrmla.TabIndex = 2
        '
        'Lblfrmla
        '
        Me.Lblfrmla.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Lblfrmla.AutoSize = True
        Me.Lblfrmla.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lblfrmla.ForeColor = System.Drawing.Color.Black
        Me.Lblfrmla.Location = New System.Drawing.Point(5, 73)
        Me.Lblfrmla.Name = "Lblfrmla"
        Me.Lblfrmla.Size = New System.Drawing.Size(105, 15)
        Me.Lblfrmla.TabIndex = 77
        Me.Lblfrmla.Text = "Selection Formula"
        '
        'Grpaftrsav
        '
        Me.Grpaftrsav.Controls.Add(Me.Rdodntprnt)
        Me.Grpaftrsav.Controls.Add(Me.Rdoprompt)
        Me.Grpaftrsav.Controls.Add(Me.Rdowithoutprmpt)
        Me.Grpaftrsav.Location = New System.Drawing.Point(416, 61)
        Me.Grpaftrsav.Name = "Grpaftrsav"
        Me.Grpaftrsav.Size = New System.Drawing.Size(181, 99)
        Me.Grpaftrsav.TabIndex = 248
        Me.Grpaftrsav.TabStop = False
        Me.Grpaftrsav.Text = "After Save"
        '
        'Rdodntprnt
        '
        Me.Rdodntprnt.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Rdodntprnt.AutoSize = True
        Me.Rdodntprnt.Checked = True
        Me.Rdodntprnt.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Rdodntprnt.ForeColor = System.Drawing.Color.Black
        Me.Rdodntprnt.Location = New System.Drawing.Point(16, 66)
        Me.Rdodntprnt.Name = "Rdodntprnt"
        Me.Rdodntprnt.Size = New System.Drawing.Size(94, 19)
        Me.Rdodntprnt.TabIndex = 212
        Me.Rdodntprnt.TabStop = True
        Me.Rdodntprnt.Text = "Do Not Print"
        Me.Rdodntprnt.UseVisualStyleBackColor = True
        '
        'Rdoprompt
        '
        Me.Rdoprompt.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Rdoprompt.AutoSize = True
        Me.Rdoprompt.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Rdoprompt.ForeColor = System.Drawing.Color.Black
        Me.Rdoprompt.Location = New System.Drawing.Point(17, 20)
        Me.Rdoprompt.Name = "Rdoprompt"
        Me.Rdoprompt.Size = New System.Drawing.Size(127, 19)
        Me.Rdoprompt.TabIndex = 210
        Me.Rdoprompt.Text = "Print With Prompt"
        Me.Rdoprompt.UseVisualStyleBackColor = True
        '
        'Rdowithoutprmpt
        '
        Me.Rdowithoutprmpt.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Rdowithoutprmpt.AutoSize = True
        Me.Rdowithoutprmpt.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Rdowithoutprmpt.ForeColor = System.Drawing.Color.Black
        Me.Rdowithoutprmpt.Location = New System.Drawing.Point(16, 43)
        Me.Rdowithoutprmpt.Name = "Rdowithoutprmpt"
        Me.Rdowithoutprmpt.Size = New System.Drawing.Size(146, 19)
        Me.Rdowithoutprmpt.TabIndex = 211
        Me.Rdowithoutprmpt.Text = "Print Without Prompt"
        Me.Rdowithoutprmpt.UseVisualStyleBackColor = True
        '
        'GrpType
        '
        Me.GrpType.Controls.Add(Me.txtprntname)
        Me.GrpType.Controls.Add(Me.chkoutlk)
        Me.GrpType.Controls.Add(Me.lblprtname)
        Me.GrpType.Controls.Add(Me.rdoPrvw)
        Me.GrpType.Controls.Add(Me.Rdopdf)
        Me.GrpType.Controls.Add(Me.RdoDrctPrnt)
        Me.GrpType.Location = New System.Drawing.Point(21, 61)
        Me.GrpType.Name = "GrpType"
        Me.GrpType.Size = New System.Drawing.Size(389, 99)
        Me.GrpType.TabIndex = 247
        Me.GrpType.TabStop = False
        Me.GrpType.Text = "Print Type"
        '
        'txtprntname
        '
        Me.txtprntname.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.txtprntname.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.txtprntname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtprntname.Font = New System.Drawing.Font("Calibri", 9.75!)
        Me.txtprntname.Location = New System.Drawing.Point(239, 39)
        Me.txtprntname.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtprntname.MaxLength = 15
        Me.txtprntname.Name = "txtprntname"
        Me.txtprntname.Size = New System.Drawing.Size(144, 23)
        Me.txtprntname.TabIndex = 82
        Me.txtprntname.Visible = False
        '
        'chkoutlk
        '
        Me.chkoutlk.AutoSize = True
        Me.chkoutlk.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkoutlk.Location = New System.Drawing.Point(157, 69)
        Me.chkoutlk.Name = "chkoutlk"
        Me.chkoutlk.Size = New System.Drawing.Size(116, 19)
        Me.chkoutlk.TabIndex = 250
        Me.chkoutlk.Text = "Open in Outlook"
        Me.chkoutlk.UseVisualStyleBackColor = True
        Me.chkoutlk.Visible = False
        '
        'lblprtname
        '
        Me.lblprtname.AutoSize = True
        Me.lblprtname.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblprtname.ForeColor = System.Drawing.Color.Black
        Me.lblprtname.Location = New System.Drawing.Point(154, 45)
        Me.lblprtname.Name = "lblprtname"
        Me.lblprtname.Size = New System.Drawing.Size(79, 15)
        Me.lblprtname.TabIndex = 251
        Me.lblprtname.Text = "Printer Name"
        Me.lblprtname.Visible = False
        '
        'rdoPrvw
        '
        Me.rdoPrvw.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rdoPrvw.AutoSize = True
        Me.rdoPrvw.Checked = True
        Me.rdoPrvw.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rdoPrvw.ForeColor = System.Drawing.Color.Black
        Me.rdoPrvw.Location = New System.Drawing.Point(7, 20)
        Me.rdoPrvw.Name = "rdoPrvw"
        Me.rdoPrvw.Size = New System.Drawing.Size(130, 19)
        Me.rdoPrvw.TabIndex = 210
        Me.rdoPrvw.TabStop = True
        Me.rdoPrvw.Text = "Print With preview"
        Me.rdoPrvw.UseVisualStyleBackColor = True
        '
        'Rdopdf
        '
        Me.Rdopdf.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Rdopdf.AutoSize = True
        Me.Rdopdf.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Rdopdf.ForeColor = System.Drawing.Color.Black
        Me.Rdopdf.Location = New System.Drawing.Point(7, 68)
        Me.Rdopdf.Name = "Rdopdf"
        Me.Rdopdf.Size = New System.Drawing.Size(90, 19)
        Me.Rdopdf.TabIndex = 211
        Me.Rdopdf.Text = "Print as PDF"
        Me.Rdopdf.UseVisualStyleBackColor = True
        '
        'RdoDrctPrnt
        '
        Me.RdoDrctPrnt.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RdoDrctPrnt.AutoSize = True
        Me.RdoDrctPrnt.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RdoDrctPrnt.ForeColor = System.Drawing.Color.Black
        Me.RdoDrctPrnt.Location = New System.Drawing.Point(7, 43)
        Me.RdoDrctPrnt.Name = "RdoDrctPrnt"
        Me.RdoDrctPrnt.Size = New System.Drawing.Size(145, 19)
        Me.RdoDrctPrnt.TabIndex = 209
        Me.RdoDrctPrnt.Text = "Print Direct to Printer"
        Me.RdoDrctPrnt.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImage = Global.vFactIC.My.Resources.Resources.T3_SAVE
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(485, 515)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(82, 31)
        Me.btnSave.TabIndex = 235
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackgroundImage = Global.vFactIC.My.Resources.Resources.T3_CANCEL
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(575, 515)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(82, 31)
        Me.btnCancel.TabIndex = 236
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'vsfdtl
        '
        Me.vsfdtl.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfdtl.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.vsfdtl.AutoResize = True
        Me.vsfdtl.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfdtl.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfdtl.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.vsfdtl.Location = New System.Drawing.Point(13, 274)
        Me.vsfdtl.Margin = New System.Windows.Forms.Padding(5)
        Me.vsfdtl.Name = "vsfdtl"
        Me.vsfdtl.Rows.DefaultSize = 24
        Me.vsfdtl.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.RowRange
        Me.vsfdtl.Size = New System.Drawing.Size(636, 174)
        Me.vsfdtl.TabIndex = 229
        '
        'FrmPrintOptionname
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(700, 589)
        Me.Controls.Add(Me.PnlprntSetting)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmPrintOptionname"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Print Option Name"
        Me.PnlprntSetting.ResumeLayout(False)
        Me.PnlprntSetting.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Grpaftrsav.ResumeLayout(False)
        Me.Grpaftrsav.PerformLayout()
        Me.GrpType.ResumeLayout(False)
        Me.GrpType.PerformLayout()
        CType(Me.vsfdtl, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PnlprntSetting As Panel
    Friend WithEvents btnSave As C1.Win.C1Input.C1Button
    Friend WithEvents btnCancel As C1.Win.C1Input.C1Button
    Friend WithEvents Grpaftrsav As GroupBox
    Friend WithEvents Rdodntprnt As RadioButton
    Friend WithEvents Rdoprompt As RadioButton
    Friend WithEvents Rdowithoutprmpt As RadioButton
    Friend WithEvents GrpType As GroupBox
    Friend WithEvents rdoPrvw As RadioButton
    Friend WithEvents Rdopdf As RadioButton
    Friend WithEvents RdoDrctPrnt As RadioButton
    Friend WithEvents vsfdtl As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents lblprntname As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents lblrptfile As Label
    Friend WithEvents lblcopies As Label
    Friend WithEvents Lblfrmla As Label
    Friend WithEvents Chkdefault As CheckBox
    Friend WithEvents chkoutlk As CheckBox
    Friend WithEvents txtcopies As TextBox
    Friend WithEvents txtfrmla As TextBox
    Friend WithEvents txtprtname As TextBox
    Friend WithEvents lblprtname As Label
    Friend WithEvents txtprntname As TextBox
    Friend WithEvents btnexist As Button
    Friend WithEvents btndiff As Button
    Friend WithEvents btntmpl As Button
    Friend WithEvents btnremv As Button
    Friend WithEvents txtid As TextBox
    Friend WithEvents cborpt As ComboBox
    Friend WithEvents txtordrno As TextBox
End Class
