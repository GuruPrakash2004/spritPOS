<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmUtlPrintOptionname
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmUtlPrintOptionname))
        Me.PnlprntSetting = New System.Windows.Forms.Panel()
        Me.txtdummy1 = New System.Windows.Forms.TextBox()
        Me.txtdummy2 = New System.Windows.Forms.TextBox()
        Me.txtID = New System.Windows.Forms.TextBox()
        Me.btnExistTrans = New System.Windows.Forms.Button()
        Me.btnDiffTrans = New System.Windows.Forms.Button()
        Me.btnTemplate = New System.Windows.Forms.Button()
        Me.btnRemove = New System.Windows.Forms.Button()
        Me.gbAfterSave = New System.Windows.Forms.GroupBox()
        Me.RdoDontPrint = New System.Windows.Forms.RadioButton()
        Me.RdoPrompt = New System.Windows.Forms.RadioButton()
        Me.RdoWithoutPrompt = New System.Windows.Forms.RadioButton()
        Me.txtPrintName = New System.Windows.Forms.TextBox()
        Me.ChkDefault = New System.Windows.Forms.CheckBox()
        Me.lblPrintName = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.cboRpt = New System.Windows.Forms.ComboBox()
        Me.lblRptFile = New System.Windows.Forms.Label()
        Me.lblNoOfCopies = New System.Windows.Forms.Label()
        Me.txtNoOfCopy = New System.Windows.Forms.TextBox()
        Me.txtFormula = New System.Windows.Forms.TextBox()
        Me.LblFormula = New System.Windows.Forms.Label()
        Me.gbType = New System.Windows.Forms.GroupBox()
        Me.btnMultiPrint = New System.Windows.Forms.Button()
        Me.cboPrintCtrlAltClick = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnEmailSettings = New System.Windows.Forms.Button()
        Me.cboPrintCtrlClick = New System.Windows.Forms.ComboBox()
        Me.cboPrintClick = New System.Windows.Forms.ComboBox()
        Me.txtPrinterName = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblPrinterName = New System.Windows.Forms.Label()
        Me.btnSave = New C1.Win.C1Input.C1Button()
        Me.btnCancel = New C1.Win.C1Input.C1Button()
        Me.vsfDtl = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnAddRow = New System.Windows.Forms.Button()
        Me.btnRemoveRow = New System.Windows.Forms.Button()
        Me.gbEmail = New System.Windows.Forms.GroupBox()
        Me.btnSMTP = New System.Windows.Forms.Button()
        Me.btnEmailCancel = New System.Windows.Forms.Button()
        Me.btnEMailOK = New System.Windows.Forms.Button()
        Me.gbeMailFormat = New System.Windows.Forms.GroupBox()
        Me.txtBody = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.txtCond = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txtFooter = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtSubject = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.gbeMailID = New System.Windows.Forms.GroupBox()
        Me.vsfeMail = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.chkAttachCopy = New System.Windows.Forms.CheckBox()
        Me.rdoSMTP = New System.Windows.Forms.RadioButton()
        Me.rdoOutLook = New System.Windows.Forms.RadioButton()
        Me.gbMultiPrint = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtSqlQuery = New System.Windows.Forms.TextBox()
        Me.chkTrackPrinted = New System.Windows.Forms.CheckBox()
        Me.chkDocNoRange = New System.Windows.Forms.CheckBox()
        Me.chkDateFilter = New System.Windows.Forms.CheckBox()
        Me.btnMultiPrintCancel = New System.Windows.Forms.Button()
        Me.btnMultiPrintOK = New System.Windows.Forms.Button()
        Me.PnlprntSetting.SuspendLayout()
        Me.gbAfterSave.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.gbType.SuspendLayout()
        CType(Me.vsfDtl, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbEmail.SuspendLayout()
        Me.gbeMailFormat.SuspendLayout()
        Me.gbeMailID.SuspendLayout()
        CType(Me.vsfeMail, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbMultiPrint.SuspendLayout()
        Me.SuspendLayout()
        '
        'PnlprntSetting
        '
        Me.PnlprntSetting.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PnlprntSetting.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PnlprntSetting.Controls.Add(Me.txtdummy1)
        Me.PnlprntSetting.Controls.Add(Me.txtdummy2)
        Me.PnlprntSetting.Controls.Add(Me.txtID)
        Me.PnlprntSetting.Controls.Add(Me.btnExistTrans)
        Me.PnlprntSetting.Controls.Add(Me.btnDiffTrans)
        Me.PnlprntSetting.Controls.Add(Me.btnTemplate)
        Me.PnlprntSetting.Controls.Add(Me.btnRemove)
        Me.PnlprntSetting.Controls.Add(Me.gbAfterSave)
        Me.PnlprntSetting.Controls.Add(Me.txtPrintName)
        Me.PnlprntSetting.Controls.Add(Me.ChkDefault)
        Me.PnlprntSetting.Controls.Add(Me.lblPrintName)
        Me.PnlprntSetting.Controls.Add(Me.Panel1)
        Me.PnlprntSetting.Controls.Add(Me.gbType)
        Me.PnlprntSetting.Controls.Add(Me.btnSave)
        Me.PnlprntSetting.Controls.Add(Me.btnCancel)
        Me.PnlprntSetting.Controls.Add(Me.vsfDtl)
        Me.PnlprntSetting.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PnlprntSetting.Location = New System.Drawing.Point(12, 9)
        Me.PnlprntSetting.Name = "PnlprntSetting"
        Me.PnlprntSetting.Size = New System.Drawing.Size(732, 577)
        Me.PnlprntSetting.TabIndex = 234
        '
        'txtdummy1
        '
        Me.txtdummy1.Location = New System.Drawing.Point(313, 495)
        Me.txtdummy1.Margin = New System.Windows.Forms.Padding(2)
        Me.txtdummy1.Name = "txtdummy1"
        Me.txtdummy1.Size = New System.Drawing.Size(76, 23)
        Me.txtdummy1.TabIndex = 236
        Me.txtdummy1.Visible = False
        '
        'txtdummy2
        '
        Me.txtdummy2.Location = New System.Drawing.Point(313, 540)
        Me.txtdummy2.Margin = New System.Windows.Forms.Padding(2)
        Me.txtdummy2.Name = "txtdummy2"
        Me.txtdummy2.Size = New System.Drawing.Size(76, 23)
        Me.txtdummy2.TabIndex = 237
        Me.txtdummy2.Visible = False
        '
        'txtID
        '
        Me.txtID.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtID.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.txtID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtID.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtID.Location = New System.Drawing.Point(688, 4)
        Me.txtID.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtID.MaxLength = 15
        Me.txtID.Name = "txtID"
        Me.txtID.Size = New System.Drawing.Size(39, 26)
        Me.txtID.TabIndex = 252
        Me.txtID.Visible = False
        '
        'btnExistTrans
        '
        Me.btnExistTrans.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExistTrans.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExistTrans.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExistTrans.Location = New System.Drawing.Point(15, 495)
        Me.btnExistTrans.Name = "btnExistTrans"
        Me.btnExistTrans.Size = New System.Drawing.Size(256, 31)
        Me.btnExistTrans.TabIndex = 18
        Me.btnExistTrans.Text = "&Copy From Existing Transaction Different Report"
        Me.btnExistTrans.UseVisualStyleBackColor = True
        '
        'btnDiffTrans
        '
        Me.btnDiffTrans.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDiffTrans.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDiffTrans.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDiffTrans.Location = New System.Drawing.Point(15, 531)
        Me.btnDiffTrans.Name = "btnDiffTrans"
        Me.btnDiffTrans.Size = New System.Drawing.Size(256, 31)
        Me.btnDiffTrans.TabIndex = 19
        Me.btnDiffTrans.Text = "Co&py from Different Transaction Report"
        Me.btnDiffTrans.UseVisualStyleBackColor = True
        '
        'btnTemplate
        '
        Me.btnTemplate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnTemplate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTemplate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTemplate.Location = New System.Drawing.Point(614, 457)
        Me.btnTemplate.Name = "btnTemplate"
        Me.btnTemplate.Size = New System.Drawing.Size(98, 31)
        Me.btnTemplate.TabIndex = 17
        Me.btnTemplate.Text = "&Template"
        Me.ToolTip1.SetToolTip(Me.btnTemplate, "Template : Alt + T")
        Me.btnTemplate.UseVisualStyleBackColor = True
        '
        'btnRemove
        '
        Me.btnRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRemove.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRemove.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRemove.Location = New System.Drawing.Point(15, 457)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(26, 28)
        Me.btnRemove.TabIndex = 14
        Me.ToolTip1.SetToolTip(Me.btnRemove, "Remove : Alt + R")
        Me.btnRemove.UseVisualStyleBackColor = True
        '
        'gbAfterSave
        '
        Me.gbAfterSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbAfterSave.Controls.Add(Me.RdoDontPrint)
        Me.gbAfterSave.Controls.Add(Me.RdoPrompt)
        Me.gbAfterSave.Controls.Add(Me.RdoWithoutPrompt)
        Me.gbAfterSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbAfterSave.Location = New System.Drawing.Point(15, 44)
        Me.gbAfterSave.Name = "gbAfterSave"
        Me.gbAfterSave.Size = New System.Drawing.Size(221, 99)
        Me.gbAfterSave.TabIndex = 7
        Me.gbAfterSave.TabStop = False
        Me.gbAfterSave.Text = "After Save"
        '
        'RdoDontPrint
        '
        Me.RdoDontPrint.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RdoDontPrint.AutoSize = True
        Me.RdoDontPrint.Checked = True
        Me.RdoDontPrint.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RdoDontPrint.ForeColor = System.Drawing.Color.Black
        Me.RdoDontPrint.Location = New System.Drawing.Point(18, 68)
        Me.RdoDontPrint.Name = "RdoDontPrint"
        Me.RdoDontPrint.Size = New System.Drawing.Size(113, 24)
        Me.RdoDontPrint.TabIndex = 9
        Me.RdoDontPrint.TabStop = True
        Me.RdoDontPrint.Text = "Do Not Print"
        Me.RdoDontPrint.UseVisualStyleBackColor = True
        '
        'RdoPrompt
        '
        Me.RdoPrompt.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RdoPrompt.AutoSize = True
        Me.RdoPrompt.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RdoPrompt.ForeColor = System.Drawing.Color.Black
        Me.RdoPrompt.Location = New System.Drawing.Point(18, 18)
        Me.RdoPrompt.Name = "RdoPrompt"
        Me.RdoPrompt.Size = New System.Drawing.Size(150, 24)
        Me.RdoPrompt.TabIndex = 7
        Me.RdoPrompt.Text = "Print With Prompt"
        Me.RdoPrompt.UseVisualStyleBackColor = True
        '
        'RdoWithoutPrompt
        '
        Me.RdoWithoutPrompt.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RdoWithoutPrompt.AutoSize = True
        Me.RdoWithoutPrompt.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RdoWithoutPrompt.ForeColor = System.Drawing.Color.Black
        Me.RdoWithoutPrompt.Location = New System.Drawing.Point(18, 43)
        Me.RdoWithoutPrompt.Name = "RdoWithoutPrompt"
        Me.RdoWithoutPrompt.Size = New System.Drawing.Size(173, 24)
        Me.RdoWithoutPrompt.TabIndex = 8
        Me.RdoWithoutPrompt.Text = "Print Without Prompt"
        Me.RdoWithoutPrompt.UseVisualStyleBackColor = True
        '
        'txtPrintName
        '
        Me.txtPrintName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.txtPrintName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPrintName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPrintName.Location = New System.Drawing.Point(164, 10)
        Me.txtPrintName.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtPrintName.MaxLength = 50
        Me.txtPrintName.Name = "txtPrintName"
        Me.txtPrintName.Size = New System.Drawing.Size(262, 26)
        Me.txtPrintName.TabIndex = 0
        '
        'ChkDefault
        '
        Me.ChkDefault.AutoSize = True
        Me.ChkDefault.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkDefault.Location = New System.Drawing.Point(442, 14)
        Me.ChkDefault.Name = "ChkDefault"
        Me.ChkDefault.Size = New System.Drawing.Size(80, 24)
        Me.ChkDefault.TabIndex = 1
        Me.ChkDefault.Text = "Default"
        Me.ChkDefault.UseVisualStyleBackColor = True
        '
        'lblPrintName
        '
        Me.lblPrintName.AutoSize = True
        Me.lblPrintName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPrintName.ForeColor = System.Drawing.Color.Red
        Me.lblPrintName.Location = New System.Drawing.Point(14, 13)
        Me.lblPrintName.Name = "lblPrintName"
        Me.lblPrintName.Size = New System.Drawing.Size(138, 20)
        Me.lblPrintName.TabIndex = 96
        Me.lblPrintName.Tag = "M"
        Me.lblPrintName.Text = "Print Option Name"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.cboRpt)
        Me.Panel1.Controls.Add(Me.lblRptFile)
        Me.Panel1.Controls.Add(Me.lblNoOfCopies)
        Me.Panel1.Controls.Add(Me.txtNoOfCopy)
        Me.Panel1.Controls.Add(Me.txtFormula)
        Me.Panel1.Controls.Add(Me.LblFormula)
        Me.Panel1.Location = New System.Drawing.Point(15, 202)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(697, 104)
        Me.Panel1.TabIndex = 10
        '
        'cboRpt
        '
        Me.cboRpt.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboRpt.CausesValidation = False
        Me.cboRpt.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboRpt.FormattingEnabled = True
        Me.cboRpt.Location = New System.Drawing.Point(157, 6)
        Me.cboRpt.MaxLength = 50
        Me.cboRpt.Name = "cboRpt"
        Me.cboRpt.Size = New System.Drawing.Size(229, 28)
        Me.cboRpt.TabIndex = 10
        '
        'lblRptFile
        '
        Me.lblRptFile.AutoSize = True
        Me.lblRptFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRptFile.ForeColor = System.Drawing.Color.Black
        Me.lblRptFile.Location = New System.Drawing.Point(5, 10)
        Me.lblRptFile.Name = "lblRptFile"
        Me.lblRptFile.Size = New System.Drawing.Size(110, 20)
        Me.lblRptFile.TabIndex = 81
        Me.lblRptFile.Text = "Rpt File Name"
        '
        'lblNoOfCopies
        '
        Me.lblNoOfCopies.AutoSize = True
        Me.lblNoOfCopies.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNoOfCopies.ForeColor = System.Drawing.Color.Black
        Me.lblNoOfCopies.Location = New System.Drawing.Point(5, 41)
        Me.lblNoOfCopies.Name = "lblNoOfCopies"
        Me.lblNoOfCopies.Size = New System.Drawing.Size(136, 20)
        Me.lblNoOfCopies.TabIndex = 21
        Me.lblNoOfCopies.Text = "Number of Copies"
        '
        'txtNoOfCopy
        '
        Me.txtNoOfCopy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtNoOfCopy.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNoOfCopy.Location = New System.Drawing.Point(158, 41)
        Me.txtNoOfCopy.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtNoOfCopy.MaxLength = 4
        Me.txtNoOfCopy.Name = "txtNoOfCopy"
        Me.txtNoOfCopy.Size = New System.Drawing.Size(65, 26)
        Me.txtNoOfCopy.TabIndex = 11
        Me.txtNoOfCopy.Tag = ""
        Me.txtNoOfCopy.Text = "0"
        '
        'txtFormula
        '
        Me.txtFormula.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFormula.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFormula.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFormula.Location = New System.Drawing.Point(158, 73)
        Me.txtFormula.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtFormula.MaxLength = 250
        Me.txtFormula.Name = "txtFormula"
        Me.txtFormula.Size = New System.Drawing.Size(471, 26)
        Me.txtFormula.TabIndex = 12
        '
        'LblFormula
        '
        Me.LblFormula.AutoSize = True
        Me.LblFormula.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblFormula.ForeColor = System.Drawing.Color.Black
        Me.LblFormula.Location = New System.Drawing.Point(5, 75)
        Me.LblFormula.Name = "LblFormula"
        Me.LblFormula.Size = New System.Drawing.Size(137, 20)
        Me.LblFormula.TabIndex = 77
        Me.LblFormula.Text = "Selection Formula"
        '
        'gbType
        '
        Me.gbType.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbType.Controls.Add(Me.btnMultiPrint)
        Me.gbType.Controls.Add(Me.cboPrintCtrlAltClick)
        Me.gbType.Controls.Add(Me.Label3)
        Me.gbType.Controls.Add(Me.btnEmailSettings)
        Me.gbType.Controls.Add(Me.cboPrintCtrlClick)
        Me.gbType.Controls.Add(Me.cboPrintClick)
        Me.gbType.Controls.Add(Me.txtPrinterName)
        Me.gbType.Controls.Add(Me.Label2)
        Me.gbType.Controls.Add(Me.Label1)
        Me.gbType.Controls.Add(Me.lblPrinterName)
        Me.gbType.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbType.Location = New System.Drawing.Point(254, 44)
        Me.gbType.Name = "gbType"
        Me.gbType.Size = New System.Drawing.Size(467, 150)
        Me.gbType.TabIndex = 2
        Me.gbType.TabStop = False
        '
        'btnMultiPrint
        '
        Me.btnMultiPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnMultiPrint.Location = New System.Drawing.Point(399, 67)
        Me.btnMultiPrint.Name = "btnMultiPrint"
        Me.btnMultiPrint.Size = New System.Drawing.Size(58, 45)
        Me.btnMultiPrint.TabIndex = 255
        Me.ToolTip1.SetToolTip(Me.btnMultiPrint, "Email Settings")
        Me.btnMultiPrint.UseVisualStyleBackColor = True
        Me.btnMultiPrint.Visible = False
        '
        'cboPrintCtrlAltClick
        '
        Me.cboPrintCtrlAltClick.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboPrintCtrlAltClick.CausesValidation = False
        Me.cboPrintCtrlAltClick.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPrintCtrlAltClick.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPrintCtrlAltClick.FormattingEnabled = True
        Me.cboPrintCtrlAltClick.Items.AddRange(New Object() {"", "Direct Print", "Email", "Preview", "Multi Print"})
        Me.cboPrintCtrlAltClick.Location = New System.Drawing.Point(139, 84)
        Me.cboPrintCtrlAltClick.MaxLength = 50
        Me.cboPrintCtrlAltClick.Name = "cboPrintCtrlAltClick"
        Me.cboPrintCtrlAltClick.Size = New System.Drawing.Size(251, 28)
        Me.cboPrintCtrlAltClick.TabIndex = 256
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Black
        Me.Label3.Location = New System.Drawing.Point(6, 89)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(151, 20)
        Me.Label3.TabIndex = 255
        Me.Label3.Text = "Print Ctrl+Alt+Click   "
        '
        'btnEmailSettings
        '
        Me.btnEmailSettings.BackgroundImage = Global.vLQMS.My.Resources.Resources.msg
        Me.btnEmailSettings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnEmailSettings.Location = New System.Drawing.Point(399, 13)
        Me.btnEmailSettings.Name = "btnEmailSettings"
        Me.btnEmailSettings.Size = New System.Drawing.Size(58, 45)
        Me.btnEmailSettings.TabIndex = 254
        Me.ToolTip1.SetToolTip(Me.btnEmailSettings, "Email Settings")
        Me.btnEmailSettings.UseVisualStyleBackColor = True
        Me.btnEmailSettings.Visible = False
        '
        'cboPrintCtrlClick
        '
        Me.cboPrintCtrlClick.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboPrintCtrlClick.CausesValidation = False
        Me.cboPrintCtrlClick.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPrintCtrlClick.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPrintCtrlClick.FormattingEnabled = True
        Me.cboPrintCtrlClick.Items.AddRange(New Object() {"", "Direct Print", "Email", "Preview", "Multi Print"})
        Me.cboPrintCtrlClick.Location = New System.Drawing.Point(139, 47)
        Me.cboPrintCtrlClick.MaxLength = 50
        Me.cboPrintCtrlClick.Name = "cboPrintCtrlClick"
        Me.cboPrintCtrlClick.Size = New System.Drawing.Size(251, 28)
        Me.cboPrintCtrlClick.TabIndex = 253
        '
        'cboPrintClick
        '
        Me.cboPrintClick.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboPrintClick.CausesValidation = False
        Me.cboPrintClick.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPrintClick.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPrintClick.FormattingEnabled = True
        Me.cboPrintClick.Items.AddRange(New Object() {"", "Direct Print", "Email", "Preview", "Multi Print"})
        Me.cboPrintClick.Location = New System.Drawing.Point(139, 13)
        Me.cboPrintClick.MaxLength = 50
        Me.cboPrintClick.Name = "cboPrintClick"
        Me.cboPrintClick.Size = New System.Drawing.Size(251, 28)
        Me.cboPrintClick.TabIndex = 252
        '
        'txtPrinterName
        '
        Me.txtPrinterName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPrinterName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.txtPrinterName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPrinterName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPrinterName.Location = New System.Drawing.Point(114, 115)
        Me.txtPrinterName.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtPrinterName.MaxLength = 50
        Me.txtPrinterName.Name = "txtPrinterName"
        Me.txtPrinterName.Size = New System.Drawing.Size(348, 26)
        Me.txtPrinterName.TabIndex = 4
        Me.txtPrinterName.Visible = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(6, 52)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(119, 20)
        Me.Label2.TabIndex = 251
        Me.Label2.Text = "Print Ctrl + Click"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Red
        Me.Label1.Location = New System.Drawing.Point(6, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(78, 20)
        Me.Label1.TabIndex = 251
        Me.Label1.Tag = "M"
        Me.Label1.Text = "Print Click"
        '
        'lblPrinterName
        '
        Me.lblPrinterName.AutoSize = True
        Me.lblPrinterName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPrinterName.ForeColor = System.Drawing.Color.Black
        Me.lblPrinterName.Location = New System.Drawing.Point(7, 119)
        Me.lblPrinterName.Name = "lblPrinterName"
        Me.lblPrinterName.Size = New System.Drawing.Size(101, 20)
        Me.lblPrinterName.TabIndex = 251
        Me.lblPrinterName.Text = "Printer Name"
        Me.lblPrinterName.Visible = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(531, 539)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(83, 31)
        Me.btnSave.TabIndex = 15
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Save : Alt + S")
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(622, 539)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(90, 31)
        Me.btnCancel.TabIndex = 16
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Cancel : Alt + C")
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'vsfDtl
        '
        Me.vsfDtl.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfDtl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDtl.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDtl.ColumnInfo = "10,0,0,0,0,125,Columns:"
        Me.vsfDtl.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.vsfDtl.Location = New System.Drawing.Point(15, 313)
        Me.vsfDtl.Margin = New System.Windows.Forms.Padding(5)
        Me.vsfDtl.Name = "vsfDtl"
        Me.vsfDtl.Rows.DefaultSize = 25
        Me.vsfDtl.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.RowRange
        Me.vsfDtl.Size = New System.Drawing.Size(696, 137)
        Me.vsfDtl.StyleInfo = resources.GetString("vsfDtl.StyleInfo")
        Me.vsfDtl.TabIndex = 13
        '
        'btnAddRow
        '
        Me.btnAddRow.BackgroundImage = Global.vLQMS.My.Resources.Resources.add
        Me.btnAddRow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnAddRow.Location = New System.Drawing.Point(4, 131)
        Me.btnAddRow.Name = "btnAddRow"
        Me.btnAddRow.Size = New System.Drawing.Size(26, 28)
        Me.btnAddRow.TabIndex = 6
        Me.ToolTip1.SetToolTip(Me.btnAddRow, "Add new eMail ID")
        Me.btnAddRow.UseVisualStyleBackColor = True
        '
        'btnRemoveRow
        '
        Me.btnRemoveRow.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRemoveRow.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnRemoveRow.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRemoveRow.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRemoveRow.Location = New System.Drawing.Point(37, 131)
        Me.btnRemoveRow.Name = "btnRemoveRow"
        Me.btnRemoveRow.Size = New System.Drawing.Size(26, 28)
        Me.btnRemoveRow.TabIndex = 253
        Me.ToolTip1.SetToolTip(Me.btnRemoveRow, "Remove : Alt + R")
        Me.btnRemoveRow.UseVisualStyleBackColor = True
        '
        'gbEmail
        '
        Me.gbEmail.Controls.Add(Me.btnSMTP)
        Me.gbEmail.Controls.Add(Me.btnEmailCancel)
        Me.gbEmail.Controls.Add(Me.btnEMailOK)
        Me.gbEmail.Controls.Add(Me.gbeMailFormat)
        Me.gbEmail.Controls.Add(Me.gbeMailID)
        Me.gbEmail.Controls.Add(Me.chkAttachCopy)
        Me.gbEmail.Controls.Add(Me.rdoSMTP)
        Me.gbEmail.Controls.Add(Me.rdoOutLook)
        Me.gbEmail.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbEmail.Location = New System.Drawing.Point(371, 2)
        Me.gbEmail.Name = "gbEmail"
        Me.gbEmail.Size = New System.Drawing.Size(86, 73)
        Me.gbEmail.TabIndex = 235
        Me.gbEmail.TabStop = False
        Me.gbEmail.Text = "Email Settings"
        Me.gbEmail.Visible = False
        '
        'btnSMTP
        '
        Me.btnSMTP.Location = New System.Drawing.Point(175, 60)
        Me.btnSMTP.Name = "btnSMTP"
        Me.btnSMTP.Size = New System.Drawing.Size(197, 32)
        Me.btnSMTP.TabIndex = 3
        Me.btnSMTP.Text = "SMTP Settings"
        Me.btnSMTP.UseVisualStyleBackColor = True
        '
        'btnEmailCancel
        '
        Me.btnEmailCancel.Location = New System.Drawing.Point(675, 537)
        Me.btnEmailCancel.Name = "btnEmailCancel"
        Me.btnEmailCancel.Size = New System.Drawing.Size(75, 28)
        Me.btnEmailCancel.TabIndex = 11
        Me.btnEmailCancel.Text = "Cancel"
        Me.btnEmailCancel.UseVisualStyleBackColor = True
        '
        'btnEMailOK
        '
        Me.btnEMailOK.Location = New System.Drawing.Point(594, 537)
        Me.btnEMailOK.Name = "btnEMailOK"
        Me.btnEMailOK.Size = New System.Drawing.Size(75, 28)
        Me.btnEMailOK.TabIndex = 10
        Me.btnEMailOK.Text = "OK"
        Me.btnEMailOK.UseVisualStyleBackColor = True
        '
        'gbeMailFormat
        '
        Me.gbeMailFormat.Controls.Add(Me.txtBody)
        Me.gbeMailFormat.Controls.Add(Me.Label14)
        Me.gbeMailFormat.Controls.Add(Me.txtCond)
        Me.gbeMailFormat.Controls.Add(Me.Label13)
        Me.gbeMailFormat.Controls.Add(Me.txtFooter)
        Me.gbeMailFormat.Controls.Add(Me.Label12)
        Me.gbeMailFormat.Controls.Add(Me.txtSubject)
        Me.gbeMailFormat.Controls.Add(Me.Label11)
        Me.gbeMailFormat.Location = New System.Drawing.Point(11, 275)
        Me.gbeMailFormat.Name = "gbeMailFormat"
        Me.gbeMailFormat.Size = New System.Drawing.Size(739, 257)
        Me.gbeMailFormat.TabIndex = 5
        Me.gbeMailFormat.TabStop = False
        Me.gbeMailFormat.Text = "eMail Format"
        '
        'txtBody
        '
        Me.txtBody.Location = New System.Drawing.Point(147, 75)
        Me.txtBody.MaxLength = 500
        Me.txtBody.Multiline = True
        Me.txtBody.Name = "txtBody"
        Me.txtBody.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtBody.Size = New System.Drawing.Size(560, 51)
        Me.txtBody.TabIndex = 7
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(14, 90)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(45, 20)
        Me.Label14.TabIndex = 21
        Me.Label14.Text = "Body"
        '
        'txtCond
        '
        Me.txtCond.Location = New System.Drawing.Point(147, 191)
        Me.txtCond.MaxLength = 500
        Me.txtCond.Multiline = True
        Me.txtCond.Name = "txtCond"
        Me.txtCond.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtCond.Size = New System.Drawing.Size(560, 51)
        Me.txtCond.TabIndex = 9
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(14, 206)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(76, 20)
        Me.Label13.TabIndex = 19
        Me.Label13.Text = "Condition"
        '
        'txtFooter
        '
        Me.txtFooter.Location = New System.Drawing.Point(147, 133)
        Me.txtFooter.MaxLength = 500
        Me.txtFooter.Multiline = True
        Me.txtFooter.Name = "txtFooter"
        Me.txtFooter.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtFooter.Size = New System.Drawing.Size(560, 51)
        Me.txtFooter.TabIndex = 8
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(14, 148)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(56, 20)
        Me.Label12.TabIndex = 17
        Me.Label12.Text = "Footer"
        '
        'txtSubject
        '
        Me.txtSubject.Location = New System.Drawing.Point(148, 17)
        Me.txtSubject.MaxLength = 500
        Me.txtSubject.Multiline = True
        Me.txtSubject.Name = "txtSubject"
        Me.txtSubject.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtSubject.Size = New System.Drawing.Size(560, 51)
        Me.txtSubject.TabIndex = 6
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(14, 32)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(63, 20)
        Me.Label11.TabIndex = 15
        Me.Label11.Text = "Subject"
        '
        'gbeMailID
        '
        Me.gbeMailID.Controls.Add(Me.btnRemoveRow)
        Me.gbeMailID.Controls.Add(Me.btnAddRow)
        Me.gbeMailID.Controls.Add(Me.vsfeMail)
        Me.gbeMailID.Location = New System.Drawing.Point(10, 91)
        Me.gbeMailID.Name = "gbeMailID"
        Me.gbeMailID.Size = New System.Drawing.Size(740, 172)
        Me.gbeMailID.TabIndex = 4
        Me.gbeMailID.TabStop = False
        Me.gbeMailID.Text = "eMail ID Settings"
        '
        'vsfeMail
        '
        Me.vsfeMail.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfeMail.AllowFiltering = True
        Me.vsfeMail.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfeMail.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfeMail.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfeMail.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfeMail.AutoResize = True
        Me.vsfeMail.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfeMail.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfeMail.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfeMail.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfeMail.ExtendLastCol = True
        Me.vsfeMail.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfeMail.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfeMail.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfeMail.Location = New System.Drawing.Point(6, 23)
        Me.vsfeMail.Name = "vsfeMail"
        Me.vsfeMail.Rows.DefaultSize = 24
        Me.vsfeMail.Rows.GlyphRow = 0
        Me.vsfeMail.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfeMail.ShowCellLabels = True
        Me.vsfeMail.Size = New System.Drawing.Size(728, 102)
        Me.vsfeMail.StyleInfo = resources.GetString("vsfeMail.StyleInfo")
        Me.vsfeMail.TabIndex = 5
        Me.vsfeMail.Tag = "View"
        Me.vsfeMail.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2007Blue
        '
        'chkAttachCopy
        '
        Me.chkAttachCopy.AutoSize = True
        Me.chkAttachCopy.Location = New System.Drawing.Point(178, 30)
        Me.chkAttachCopy.Name = "chkAttachCopy"
        Me.chkAttachCopy.Size = New System.Drawing.Size(197, 24)
        Me.chkAttachCopy.TabIndex = 2
        Me.chkAttachCopy.Text = "Do not attach print copy"
        Me.chkAttachCopy.UseVisualStyleBackColor = True
        '
        'rdoSMTP
        '
        Me.rdoSMTP.AutoSize = True
        Me.rdoSMTP.Location = New System.Drawing.Point(20, 60)
        Me.rdoSMTP.Name = "rdoSMTP"
        Me.rdoSMTP.Size = New System.Drawing.Size(70, 24)
        Me.rdoSMTP.TabIndex = 1
        Me.rdoSMTP.Text = "SMTP"
        Me.rdoSMTP.UseVisualStyleBackColor = True
        '
        'rdoOutLook
        '
        Me.rdoOutLook.AutoSize = True
        Me.rdoOutLook.Checked = True
        Me.rdoOutLook.Location = New System.Drawing.Point(20, 30)
        Me.rdoOutLook.Name = "rdoOutLook"
        Me.rdoOutLook.Size = New System.Drawing.Size(82, 24)
        Me.rdoOutLook.TabIndex = 0
        Me.rdoOutLook.TabStop = True
        Me.rdoOutLook.Text = "Outlook"
        Me.rdoOutLook.UseVisualStyleBackColor = True
        '
        'gbMultiPrint
        '
        Me.gbMultiPrint.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbMultiPrint.Controls.Add(Me.Label4)
        Me.gbMultiPrint.Controls.Add(Me.txtSqlQuery)
        Me.gbMultiPrint.Controls.Add(Me.chkTrackPrinted)
        Me.gbMultiPrint.Controls.Add(Me.chkDocNoRange)
        Me.gbMultiPrint.Controls.Add(Me.chkDateFilter)
        Me.gbMultiPrint.Controls.Add(Me.btnMultiPrintCancel)
        Me.gbMultiPrint.Controls.Add(Me.btnMultiPrintOK)
        Me.gbMultiPrint.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbMultiPrint.Location = New System.Drawing.Point(540, 2)
        Me.gbMultiPrint.Margin = New System.Windows.Forms.Padding(2)
        Me.gbMultiPrint.Name = "gbMultiPrint"
        Me.gbMultiPrint.Padding = New System.Windows.Forms.Padding(2)
        Me.gbMultiPrint.Size = New System.Drawing.Size(60, 54)
        Me.gbMultiPrint.TabIndex = 12
        Me.gbMultiPrint.TabStop = False
        Me.gbMultiPrint.Text = "Multi Print"
        Me.gbMultiPrint.Visible = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(23, 106)
        Me.Label4.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(87, 20)
        Me.Label4.TabIndex = 18
        Me.Label4.Text = "SQL Query"
        '
        'txtSqlQuery
        '
        Me.txtSqlQuery.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSqlQuery.Location = New System.Drawing.Point(118, 106)
        Me.txtSqlQuery.Margin = New System.Windows.Forms.Padding(2)
        Me.txtSqlQuery.Multiline = True
        Me.txtSqlQuery.Name = "txtSqlQuery"
        Me.txtSqlQuery.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtSqlQuery.Size = New System.Drawing.Size(302, 95)
        Me.txtSqlQuery.TabIndex = 17
        '
        'chkTrackPrinted
        '
        Me.chkTrackPrinted.AutoSize = True
        Me.chkTrackPrinted.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTrackPrinted.Location = New System.Drawing.Point(307, 64)
        Me.chkTrackPrinted.Margin = New System.Windows.Forms.Padding(2)
        Me.chkTrackPrinted.Name = "chkTrackPrinted"
        Me.chkTrackPrinted.Size = New System.Drawing.Size(121, 24)
        Me.chkTrackPrinted.TabIndex = 16
        Me.chkTrackPrinted.Text = "Track Printed"
        Me.chkTrackPrinted.UseVisualStyleBackColor = True
        '
        'chkDocNoRange
        '
        Me.chkDocNoRange.AutoSize = True
        Me.chkDocNoRange.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDocNoRange.Location = New System.Drawing.Point(150, 64)
        Me.chkDocNoRange.Margin = New System.Windows.Forms.Padding(2)
        Me.chkDocNoRange.Name = "chkDocNoRange"
        Me.chkDocNoRange.Size = New System.Drawing.Size(133, 24)
        Me.chkDocNoRange.TabIndex = 15
        Me.chkDocNoRange.Text = "Doc No Range"
        Me.chkDocNoRange.UseVisualStyleBackColor = True
        '
        'chkDateFilter
        '
        Me.chkDateFilter.AutoSize = True
        Me.chkDateFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDateFilter.Location = New System.Drawing.Point(22, 64)
        Me.chkDateFilter.Margin = New System.Windows.Forms.Padding(2)
        Me.chkDateFilter.Name = "chkDateFilter"
        Me.chkDateFilter.Size = New System.Drawing.Size(102, 24)
        Me.chkDateFilter.TabIndex = 14
        Me.chkDateFilter.Text = "Date Filter"
        Me.chkDateFilter.UseVisualStyleBackColor = True
        '
        'btnMultiPrintCancel
        '
        Me.btnMultiPrintCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMultiPrintCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMultiPrintCancel.Location = New System.Drawing.Point(-21, -29)
        Me.btnMultiPrintCancel.Name = "btnMultiPrintCancel"
        Me.btnMultiPrintCancel.Size = New System.Drawing.Size(75, 28)
        Me.btnMultiPrintCancel.TabIndex = 13
        Me.btnMultiPrintCancel.Text = "Cancel"
        Me.btnMultiPrintCancel.UseVisualStyleBackColor = True
        '
        'btnMultiPrintOK
        '
        Me.btnMultiPrintOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMultiPrintOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMultiPrintOK.Location = New System.Drawing.Point(-106, -29)
        Me.btnMultiPrintOK.Name = "btnMultiPrintOK"
        Me.btnMultiPrintOK.Size = New System.Drawing.Size(75, 28)
        Me.btnMultiPrintOK.TabIndex = 12
        Me.btnMultiPrintOK.Text = "OK"
        Me.btnMultiPrintOK.UseVisualStyleBackColor = True
        '
        'FrmUtlPrintOptionname
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(756, 615)
        Me.Controls.Add(Me.gbEmail)
        Me.Controls.Add(Me.gbMultiPrint)
        Me.Controls.Add(Me.PnlprntSetting)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MinimizeBox = False
        Me.Name = "FrmUtlPrintOptionname"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Print Option Name"
        Me.PnlprntSetting.ResumeLayout(False)
        Me.PnlprntSetting.PerformLayout()
        Me.gbAfterSave.ResumeLayout(False)
        Me.gbAfterSave.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.gbType.ResumeLayout(False)
        Me.gbType.PerformLayout()
        CType(Me.vsfDtl, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbEmail.ResumeLayout(False)
        Me.gbEmail.PerformLayout()
        Me.gbeMailFormat.ResumeLayout(False)
        Me.gbeMailFormat.PerformLayout()
        Me.gbeMailID.ResumeLayout(False)
        CType(Me.vsfeMail, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbMultiPrint.ResumeLayout(False)
        Me.gbMultiPrint.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PnlprntSetting As Panel
    Friend WithEvents btnSave As C1.Win.C1Input.C1Button
    Friend WithEvents btnCancel As C1.Win.C1Input.C1Button
    Friend WithEvents gbAfterSave As GroupBox
    Friend WithEvents RdoDontPrint As RadioButton
    Friend WithEvents RdoPrompt As RadioButton
    Friend WithEvents RdoWithoutPrompt As RadioButton
    Friend WithEvents gbType As GroupBox
    Friend WithEvents vsfDtl As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents lblPrintName As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents lblRptFile As Label
    Friend WithEvents lblNoOfCopies As Label
    Friend WithEvents LblFormula As Label
    Friend WithEvents ChkDefault As CheckBox
    Friend WithEvents txtNoOfCopy As TextBox
    Friend WithEvents txtFormula As TextBox
    Friend WithEvents txtPrintName As TextBox
    Friend WithEvents lblPrinterName As Label
    Friend WithEvents txtPrinterName As TextBox
    Friend WithEvents btnExistTrans As Button
    Friend WithEvents btnDiffTrans As Button
    Friend WithEvents btnTemplate As Button
    Friend WithEvents btnRemove As Button
    Friend WithEvents txtID As TextBox
    Friend WithEvents cboRpt As ComboBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents cboPrintCtrlClick As ComboBox
    Friend WithEvents cboPrintClick As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents btnEmailSettings As Button
    Friend WithEvents gbEmail As GroupBox
    Friend WithEvents chkAttachCopy As CheckBox
    Friend WithEvents rdoSMTP As RadioButton
    Friend WithEvents rdoOutLook As RadioButton
    Friend WithEvents gbeMailID As GroupBox
    Friend WithEvents btnAddRow As Button
    Friend WithEvents vsfeMail As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents gbeMailFormat As GroupBox
    Friend WithEvents txtBody As TextBox
    Friend WithEvents Label14 As Label
    Friend WithEvents txtCond As TextBox
    Friend WithEvents Label13 As Label
    Friend WithEvents txtFooter As TextBox
    Friend WithEvents Label12 As Label
    Friend WithEvents txtSubject As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents btnEmailCancel As Button
    Friend WithEvents btnEMailOK As Button
    Friend WithEvents btnSMTP As Button
    Friend WithEvents cboPrintCtrlAltClick As ComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents btnMultiPrint As Button
    Friend WithEvents gbMultiPrint As GroupBox
    Friend WithEvents txtSqlQuery As TextBox
    Friend WithEvents chkTrackPrinted As CheckBox
    Friend WithEvents chkDocNoRange As CheckBox
    Friend WithEvents chkDateFilter As CheckBox
    Friend WithEvents btnMultiPrintCancel As Button
    Friend WithEvents btnMultiPrintOK As Button
    Friend WithEvents Label4 As Label
    Friend WithEvents txtdummy1 As TextBox
    Friend WithEvents txtdummy2 As TextBox
    Friend WithEvents btnRemoveRow As Button
End Class
