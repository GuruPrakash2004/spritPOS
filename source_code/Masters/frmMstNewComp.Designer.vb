<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMstNewComp
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMstNewComp))
        Me.gbCompInfo = New System.Windows.Forms.GroupBox()
        Me.btnCancel = New C1.Win.C1Input.C1Button()
        Me.btnNextMain = New C1.Win.C1Input.C1Button()
        Me.chkLoginReq = New System.Windows.Forms.CheckBox()
        Me.gbFinYear = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dtpTyr = New System.Windows.Forms.DateTimePicker()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dtpFyr = New System.Windows.Forms.DateTimePicker()
        Me.vsfCDET = New C1.Win.C1FlexGrid.C1FlexGrid()
        'Me.ShapeContainer1 = New Microsoft.VisualBasic.PowerPacks.ShapeContainer()
        'Me.RectangleShape1 = New Microsoft.VisualBasic.PowerPacks.RectangleShape()
        Me.gbExistingDB = New System.Windows.Forms.GroupBox()
        Me.cboDBTemplate = New System.Windows.Forms.ComboBox()
        Me.btnBackExisting = New C1.Win.C1Input.C1Button()
        Me.btnNextExisting = New C1.Win.C1Input.C1Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.rdBtnOnlyMasters = New System.Windows.Forms.RadioButton()
        Me.rdBtnTrnandMst = New System.Windows.Forms.RadioButton()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.gbSel = New System.Windows.Forms.GroupBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.rdbtnExistingDB = New System.Windows.Forms.RadioButton()
        Me.btnBackSel = New C1.Win.C1Input.C1Button()
        Me.btnNextSel = New C1.Win.C1Input.C1Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.rdbtnDemoComp = New System.Windows.Forms.RadioButton()
        Me.rdbtnFromTemplate = New System.Windows.Forms.RadioButton()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.gbFinish = New System.Windows.Forms.GroupBox()
        Me.txtFinishDet = New System.Windows.Forms.TextBox()
        Me.btnFinishBack = New C1.Win.C1Input.C1Button()
        Me.btnFinish = New C1.Win.C1Input.C1Button()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.gbCompInfo.SuspendLayout()
        Me.gbFinYear.SuspendLayout()
        CType(Me.vsfCDET, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbExistingDB.SuspendLayout()
        Me.gbSel.SuspendLayout()
        Me.gbFinish.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbCompInfo
        '
        Me.gbCompInfo.Controls.Add(Me.btnCancel)
        Me.gbCompInfo.Controls.Add(Me.btnNextMain)
        Me.gbCompInfo.Controls.Add(Me.chkLoginReq)
        Me.gbCompInfo.Controls.Add(Me.gbFinYear)
        Me.gbCompInfo.Controls.Add(Me.vsfCDET)
        'Me.gbCompInfo.Controls.Add(Me.ShapeContainer1)
        Me.gbCompInfo.Location = New System.Drawing.Point(12, 12)
        Me.gbCompInfo.Name = "gbCompInfo"
        Me.gbCompInfo.Size = New System.Drawing.Size(483, 289)
        Me.gbCompInfo.TabIndex = 10
        Me.gbCompInfo.TabStop = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Image = CType(resources.GetObject("btnCancel.Image"), System.Drawing.Image)
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(404, 251)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(70, 31)
        Me.btnCancel.TabIndex = 13
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnNextMain
        '
        Me.btnNextMain.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNextMain.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNextMain.Image = Global.vICAxon.My.Resources.Resources.D_NEXT
        Me.btnNextMain.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnNextMain.Location = New System.Drawing.Point(331, 251)
        Me.btnNextMain.Margin = New System.Windows.Forms.Padding(4)
        Me.btnNextMain.Name = "btnNextMain"
        Me.btnNextMain.Size = New System.Drawing.Size(65, 31)
        Me.btnNextMain.TabIndex = 12
        Me.btnNextMain.Text = "&Next"
        Me.btnNextMain.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnNextMain.UseVisualStyleBackColor = False
        '
        'chkLoginReq
        '
        Me.chkLoginReq.AutoSize = True
        Me.chkLoginReq.Location = New System.Drawing.Point(339, 208)
        Me.chkLoginReq.Name = "chkLoginReq"
        Me.chkLoginReq.Size = New System.Drawing.Size(98, 17)
        Me.chkLoginReq.TabIndex = 11
        Me.chkLoginReq.Text = "Login Required"
        Me.chkLoginReq.UseVisualStyleBackColor = True
        '
        'gbFinYear
        '
        Me.gbFinYear.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.gbFinYear.Controls.Add(Me.Label1)
        Me.gbFinYear.Controls.Add(Me.dtpTyr)
        Me.gbFinYear.Controls.Add(Me.Label2)
        Me.gbFinYear.Controls.Add(Me.dtpFyr)
        Me.gbFinYear.Location = New System.Drawing.Point(9, 183)
        Me.gbFinYear.Name = "gbFinYear"
        Me.gbFinYear.Size = New System.Drawing.Size(319, 51)
        Me.gbFinYear.TabIndex = 10
        Me.gbFinYear.TabStop = False
        Me.gbFinYear.Text = "Financial Year"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label1.Location = New System.Drawing.Point(18, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(38, 17)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "From"
        '
        'dtpTyr
        '
        Me.dtpTyr.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.dtpTyr.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpTyr.Location = New System.Drawing.Point(200, 17)
        Me.dtpTyr.MaxDate = New Date(2025, 12, 31, 0, 0, 0, 0)
        Me.dtpTyr.MinDate = New Date(1990, 1, 1, 0, 0, 0, 0)
        Me.dtpTyr.Name = "dtpTyr"
        Me.dtpTyr.Size = New System.Drawing.Size(94, 25)
        Me.dtpTyr.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label2.Location = New System.Drawing.Point(173, 21)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(23, 17)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "To"
        '
        'dtpFyr
        '
        Me.dtpFyr.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.dtpFyr.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFyr.Location = New System.Drawing.Point(62, 17)
        Me.dtpFyr.MaxDate = New Date(2025, 12, 31, 0, 0, 0, 0)
        Me.dtpFyr.MinDate = New Date(1990, 1, 1, 0, 0, 0, 0)
        Me.dtpFyr.Name = "dtpFyr"
        Me.dtpFyr.Size = New System.Drawing.Size(94, 25)
        Me.dtpFyr.TabIndex = 0
        '
        'vsfCDET
        '
        Me.vsfCDET.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfCDET.ColumnInfo = "2,1,0,0,0,120,Columns:1{Style:""TextAlign:LeftCenter;"";StyleFixed:""TextAlign:LeftC" & _
    "enter;"";}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfCDET.ExtendLastCol = True
        Me.vsfCDET.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfCDET.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfCDET.Location = New System.Drawing.Point(6, 17)
        Me.vsfCDET.Name = "vsfCDET"
        Me.vsfCDET.Rows.Count = 4
        Me.vsfCDET.Rows.DefaultSize = 24
        Me.vsfCDET.Rows.Fixed = 0
        Me.vsfCDET.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfCDET.Size = New System.Drawing.Size(471, 159)
        Me.vsfCDET.StyleInfo = resources.GetString("vsfCDET.StyleInfo")
        Me.vsfCDET.TabIndex = 9
        '
        'ShapeContainer1
        '
        'Me.ShapeContainer1.Location = New System.Drawing.Point(3, 16)
        'Me.ShapeContainer1.Margin = New System.Windows.Forms.Padding(0)
        'Me.ShapeContainer1.Name = "ShapeContainer1"
        'Me.ShapeContainer1.Shapes.AddRange(New Microsoft.VisualBasic.PowerPacks.Shape() {Me.RectangleShape1})
        'Me.ShapeContainer1.Size = New System.Drawing.Size(477, 270)
        'Me.ShapeContainer1.TabIndex = 14
        'Me.ShapeContainer1.TabStop = False
        '
        'RectangleShape1
        '
        'Me.RectangleShape1.Location = New System.Drawing.Point(4, 225)
        'Me.RectangleShape1.Name = "RectangleShape1"
        'Me.RectangleShape1.Size = New System.Drawing.Size(470, 2)
        '
        'gbExistingDB
        '
        Me.gbExistingDB.Controls.Add(Me.cboDBTemplate)
        Me.gbExistingDB.Controls.Add(Me.btnBackExisting)
        Me.gbExistingDB.Controls.Add(Me.btnNextExisting)
        Me.gbExistingDB.Controls.Add(Me.Label5)
        Me.gbExistingDB.Controls.Add(Me.Label4)
        Me.gbExistingDB.Controls.Add(Me.rdBtnOnlyMasters)
        Me.gbExistingDB.Controls.Add(Me.rdBtnTrnandMst)
        Me.gbExistingDB.Controls.Add(Me.Label3)
        Me.gbExistingDB.Location = New System.Drawing.Point(12, 12)
        Me.gbExistingDB.Name = "gbExistingDB"
        Me.gbExistingDB.Size = New System.Drawing.Size(483, 289)
        Me.gbExistingDB.TabIndex = 11
        Me.gbExistingDB.TabStop = False
        '
        'cboDBTemplate
        '
        Me.cboDBTemplate.FormattingEnabled = True
        Me.cboDBTemplate.Location = New System.Drawing.Point(146, 24)
        Me.cboDBTemplate.Name = "cboDBTemplate"
        Me.cboDBTemplate.Size = New System.Drawing.Size(296, 21)
        Me.cboDBTemplate.TabIndex = 15
        '
        'btnBackExisting
        '
        Me.btnBackExisting.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBackExisting.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBackExisting.Image = CType(resources.GetObject("btnBackExisting.Image"), System.Drawing.Image)
        Me.btnBackExisting.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnBackExisting.Location = New System.Drawing.Point(364, 251)
        Me.btnBackExisting.Margin = New System.Windows.Forms.Padding(4)
        Me.btnBackExisting.Name = "btnBackExisting"
        Me.btnBackExisting.Size = New System.Drawing.Size(70, 31)
        Me.btnBackExisting.TabIndex = 14
        Me.btnBackExisting.Text = "Back"
        Me.btnBackExisting.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnBackExisting.UseVisualStyleBackColor = False
        '
        'btnNextExisting
        '
        Me.btnNextExisting.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNextExisting.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNextExisting.Image = Global.vICAxon.My.Resources.Resources.D_NEXT
        Me.btnNextExisting.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnNextExisting.Location = New System.Drawing.Point(291, 251)
        Me.btnNextExisting.Margin = New System.Windows.Forms.Padding(4)
        Me.btnNextExisting.Name = "btnNextExisting"
        Me.btnNextExisting.Size = New System.Drawing.Size(65, 31)
        Me.btnNextExisting.TabIndex = 13
        Me.btnNextExisting.Text = "&Next"
        Me.btnNextExisting.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnNextExisting.UseVisualStyleBackColor = False
        '
        'Label5
        '
        Me.Label5.ForeColor = System.Drawing.Color.IndianRed
        Me.Label5.Location = New System.Drawing.Point(117, 183)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(325, 44)
        Me.Label5.TabIndex = 5
        Me.Label5.Text = "Imports the Structure of Masters Only"
        '
        'Label4
        '
        Me.Label4.ForeColor = System.Drawing.Color.IndianRed
        Me.Label4.Location = New System.Drawing.Point(117, 112)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(325, 44)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Imports the Structure of Transactions and Masters from the Existing Database"
        '
        'rdBtnOnlyMasters
        '
        Me.rdBtnOnlyMasters.AutoSize = True
        Me.rdBtnOnlyMasters.Location = New System.Drawing.Point(97, 159)
        Me.rdBtnOnlyMasters.Name = "rdBtnOnlyMasters"
        Me.rdBtnOnlyMasters.Size = New System.Drawing.Size(126, 17)
        Me.rdBtnOnlyMasters.TabIndex = 3
        Me.rdBtnOnlyMasters.TabStop = True
        Me.rdBtnOnlyMasters.Text = "Import Masters ONLY"
        Me.rdBtnOnlyMasters.UseVisualStyleBackColor = True
        '
        'rdBtnTrnandMst
        '
        Me.rdBtnTrnandMst.AutoSize = True
        Me.rdBtnTrnandMst.Location = New System.Drawing.Point(97, 92)
        Me.rdBtnTrnandMst.Name = "rdBtnTrnandMst"
        Me.rdBtnTrnandMst.Size = New System.Drawing.Size(169, 17)
        Me.rdBtnTrnandMst.TabIndex = 2
        Me.rdBtnTrnandMst.TabStop = True
        Me.rdBtnTrnandMst.Text = "Import Transaction and Master"
        Me.rdBtnTrnandMst.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(43, 24)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(84, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Select Company"
        '
        'gbSel
        '
        Me.gbSel.Controls.Add(Me.Label9)
        Me.gbSel.Controls.Add(Me.rdbtnExistingDB)
        Me.gbSel.Controls.Add(Me.btnBackSel)
        Me.gbSel.Controls.Add(Me.btnNextSel)
        Me.gbSel.Controls.Add(Me.Label6)
        Me.gbSel.Controls.Add(Me.Label7)
        Me.gbSel.Controls.Add(Me.rdbtnDemoComp)
        Me.gbSel.Controls.Add(Me.rdbtnFromTemplate)
        Me.gbSel.Controls.Add(Me.Label8)
        Me.gbSel.Location = New System.Drawing.Point(12, 12)
        Me.gbSel.Name = "gbSel"
        Me.gbSel.Size = New System.Drawing.Size(483, 289)
        Me.gbSel.TabIndex = 12
        Me.gbSel.TabStop = False
        '
        'Label9
        '
        Me.Label9.ForeColor = System.Drawing.Color.IndianRed
        Me.Label9.Location = New System.Drawing.Point(117, 185)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(325, 44)
        Me.Label9.TabIndex = 16
        Me.Label9.Text = "Creates a Company from Existing Company already present"
        '
        'rdbtnExistingDB
        '
        Me.rdbtnExistingDB.AutoSize = True
        Me.rdbtnExistingDB.Location = New System.Drawing.Point(97, 161)
        Me.rdbtnExistingDB.Name = "rdbtnExistingDB"
        Me.rdbtnExistingDB.Size = New System.Drawing.Size(108, 17)
        Me.rdbtnExistingDB.TabIndex = 15
        Me.rdbtnExistingDB.TabStop = True
        Me.rdbtnExistingDB.Text = "Existing Company"
        Me.rdbtnExistingDB.UseVisualStyleBackColor = True
        '
        'btnBackSel
        '
        Me.btnBackSel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBackSel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBackSel.Image = CType(resources.GetObject("btnBackSel.Image"), System.Drawing.Image)
        Me.btnBackSel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnBackSel.Location = New System.Drawing.Point(364, 251)
        Me.btnBackSel.Margin = New System.Windows.Forms.Padding(4)
        Me.btnBackSel.Name = "btnBackSel"
        Me.btnBackSel.Size = New System.Drawing.Size(70, 31)
        Me.btnBackSel.TabIndex = 14
        Me.btnBackSel.Text = "Back"
        Me.btnBackSel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnBackSel.UseVisualStyleBackColor = False
        '
        'btnNextSel
        '
        Me.btnNextSel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNextSel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNextSel.Image = Global.vICAxon.My.Resources.Resources.D_NEXT
        Me.btnNextSel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnNextSel.Location = New System.Drawing.Point(291, 251)
        Me.btnNextSel.Margin = New System.Windows.Forms.Padding(4)
        Me.btnNextSel.Name = "btnNextSel"
        Me.btnNextSel.Size = New System.Drawing.Size(65, 31)
        Me.btnNextSel.TabIndex = 13
        Me.btnNextSel.Text = "&Next"
        Me.btnNextSel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnNextSel.UseVisualStyleBackColor = False
        '
        'Label6
        '
        Me.Label6.ForeColor = System.Drawing.Color.IndianRed
        Me.Label6.Location = New System.Drawing.Point(117, 131)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(325, 23)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "Creates a Company with DEMO DATA provided by Softvent. "
        '
        'Label7
        '
        Me.Label7.ForeColor = System.Drawing.Color.IndianRed
        Me.Label7.Location = New System.Drawing.Point(117, 74)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(325, 26)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "Creates a Company from the Template provided by Softvent. "
        '
        'rdbtnDemoComp
        '
        Me.rdbtnDemoComp.AutoSize = True
        Me.rdbtnDemoComp.Location = New System.Drawing.Point(97, 107)
        Me.rdbtnDemoComp.Name = "rdbtnDemoComp"
        Me.rdbtnDemoComp.Size = New System.Drawing.Size(100, 17)
        Me.rdbtnDemoComp.TabIndex = 3
        Me.rdbtnDemoComp.TabStop = True
        Me.rdbtnDemoComp.Text = "Demo Company"
        Me.rdbtnDemoComp.UseVisualStyleBackColor = True
        '
        'rdbtnFromTemplate
        '
        Me.rdbtnFromTemplate.AutoSize = True
        Me.rdbtnFromTemplate.Location = New System.Drawing.Point(97, 50)
        Me.rdbtnFromTemplate.Name = "rdbtnFromTemplate"
        Me.rdbtnFromTemplate.Size = New System.Drawing.Size(113, 17)
        Me.rdbtnFromTemplate.TabIndex = 2
        Me.rdbtnFromTemplate.TabStop = True
        Me.rdbtnFromTemplate.Text = "SoftVent Template"
        Me.rdbtnFromTemplate.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(43, 24)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(145, 13)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Create New Company from ..."
        '
        'gbFinish
        '
        Me.gbFinish.Controls.Add(Me.txtFinishDet)
        Me.gbFinish.Controls.Add(Me.btnFinishBack)
        Me.gbFinish.Controls.Add(Me.btnFinish)
        Me.gbFinish.Controls.Add(Me.Label12)
        Me.gbFinish.Location = New System.Drawing.Point(12, 12)
        Me.gbFinish.Name = "gbFinish"
        Me.gbFinish.Size = New System.Drawing.Size(483, 289)
        Me.gbFinish.TabIndex = 13
        Me.gbFinish.TabStop = False
        '
        'txtFinishDet
        '
        Me.txtFinishDet.BackColor = System.Drawing.SystemColors.Info
        Me.txtFinishDet.Location = New System.Drawing.Point(9, 15)
        Me.txtFinishDet.Multiline = True
        Me.txtFinishDet.Name = "txtFinishDet"
        Me.txtFinishDet.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtFinishDet.Size = New System.Drawing.Size(461, 229)
        Me.txtFinishDet.TabIndex = 16
        '
        'btnFinishBack
        '
        Me.btnFinishBack.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFinishBack.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFinishBack.Image = CType(resources.GetObject("btnFinishBack.Image"), System.Drawing.Image)
        Me.btnFinishBack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnFinishBack.Location = New System.Drawing.Point(405, 251)
        Me.btnFinishBack.Margin = New System.Windows.Forms.Padding(4)
        Me.btnFinishBack.Name = "btnFinishBack"
        Me.btnFinishBack.Size = New System.Drawing.Size(70, 31)
        Me.btnFinishBack.TabIndex = 14
        Me.btnFinishBack.Text = "Back"
        Me.btnFinishBack.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnFinishBack.UseVisualStyleBackColor = False
        '
        'btnFinish
        '
        Me.btnFinish.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFinish.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFinish.Image = Global.vICAxon.My.Resources.Resources.D_NEXT
        Me.btnFinish.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnFinish.Location = New System.Drawing.Point(332, 251)
        Me.btnFinish.Margin = New System.Windows.Forms.Padding(4)
        Me.btnFinish.Name = "btnFinish"
        Me.btnFinish.Size = New System.Drawing.Size(65, 31)
        Me.btnFinish.TabIndex = 13
        Me.btnFinish.Text = "Finish"
        Me.btnFinish.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnFinish.UseVisualStyleBackColor = False
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.ForeColor = System.Drawing.Color.Red
        Me.Label12.Location = New System.Drawing.Point(6, 261)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(285, 13)
        Me.Label12.TabIndex = 0
        Me.Label12.Text = "To Create a Company with the Above Info , Click Finish --->"
        '
        'frmMstNewComp
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(502, 306)
        Me.Controls.Add(Me.gbCompInfo)
        Me.Controls.Add(Me.gbSel)
        Me.Controls.Add(Me.gbExistingDB)
        Me.Controls.Add(Me.gbFinish)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(518, 344)
        Me.MinimumSize = New System.Drawing.Size(518, 344)
        Me.Name = "frmMstNewComp"
        Me.Text = "Create New Company"
        Me.gbCompInfo.ResumeLayout(False)
        Me.gbCompInfo.PerformLayout()
        Me.gbFinYear.ResumeLayout(False)
        Me.gbFinYear.PerformLayout()
        CType(Me.vsfCDET, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbExistingDB.ResumeLayout(False)
        Me.gbExistingDB.PerformLayout()
        Me.gbSel.ResumeLayout(False)
        Me.gbSel.PerformLayout()
        Me.gbFinish.ResumeLayout(False)
        Me.gbFinish.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbCompInfo As System.Windows.Forms.GroupBox
    Friend WithEvents btnCancel As C1.Win.C1Input.C1Button
    Friend WithEvents btnNextMain As C1.Win.C1Input.C1Button
    Friend WithEvents chkLoginReq As System.Windows.Forms.CheckBox
    Friend WithEvents gbFinYear As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dtpTyr As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dtpFyr As System.Windows.Forms.DateTimePicker
    Friend WithEvents vsfCDET As C1.Win.C1FlexGrid.C1FlexGrid
    'Friend WithEvents ShapeContainer1 As Microsoft.VisualBasic.PowerPacks.ShapeContainer
    'Friend WithEvents RectangleShape1 As Microsoft.VisualBasic.PowerPacks.RectangleShape
    Friend WithEvents gbExistingDB As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents rdBtnOnlyMasters As System.Windows.Forms.RadioButton
    Friend WithEvents rdBtnTrnandMst As System.Windows.Forms.RadioButton
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents btnBackExisting As C1.Win.C1Input.C1Button
    Friend WithEvents btnNextExisting As C1.Win.C1Input.C1Button
    Friend WithEvents gbSel As System.Windows.Forms.GroupBox
    Friend WithEvents btnBackSel As C1.Win.C1Input.C1Button
    Friend WithEvents btnNextSel As C1.Win.C1Input.C1Button
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents rdbtnDemoComp As System.Windows.Forms.RadioButton
    Friend WithEvents rdbtnFromTemplate As System.Windows.Forms.RadioButton
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents rdbtnExistingDB As System.Windows.Forms.RadioButton
    Friend WithEvents gbFinish As System.Windows.Forms.GroupBox
    Friend WithEvents btnFinishBack As C1.Win.C1Input.C1Button
    Friend WithEvents btnFinish As C1.Win.C1Input.C1Button
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtFinishDet As System.Windows.Forms.TextBox
    Friend WithEvents cboDBTemplate As System.Windows.Forms.ComboBox
End Class
