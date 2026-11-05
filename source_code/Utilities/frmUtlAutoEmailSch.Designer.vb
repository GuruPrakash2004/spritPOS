<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlAutoEmailSch
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlAutoEmailSch))
        Me.vsfSch = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnExport = New C1.Win.C1Input.C1Button()
        Me.btnRemSch = New C1.Win.C1Input.C1Button()
        Me.btnVewRpt = New C1.Win.C1Input.C1Button()
        Me.PnlRpt = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnRptSave = New System.Windows.Forms.Button()
        Me.btnRptCls = New System.Windows.Forms.Button()
        Me.txtRmk = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ChkInAct = New System.Windows.Forms.CheckBox()
        Me.txtAddEmail = New System.Windows.Forms.TextBox()
        Me.txtRptName = New System.Windows.Forms.TextBox()
        Me.lblAddEmail = New System.Windows.Forms.Label()
        Me.lblRptName = New System.Windows.Forms.Label()
        Me.btnAddRpt = New C1.Win.C1Input.C1Button()
        Me.chkSelRpt = New System.Windows.Forms.CheckBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnSchSave = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.pnlDayWise = New System.Windows.Forms.Panel()
        Me.optMonthWise = New System.Windows.Forms.RadioButton()
        Me.optDayWise = New System.Windows.Forms.RadioButton()
        Me.optSelDays = New System.Windows.Forms.RadioButton()
        Me.pnlShowDays = New System.Windows.Forms.Panel()
        Me.chkWednesday = New System.Windows.Forms.CheckBox()
        Me.chkFriday = New System.Windows.Forms.CheckBox()
        Me.chkMonday = New System.Windows.Forms.CheckBox()
        Me.chkThursday = New System.Windows.Forms.CheckBox()
        Me.chkSaturday = New System.Windows.Forms.CheckBox()
        Me.chkSunday = New System.Windows.Forms.CheckBox()
        Me.chkTuesday = New System.Windows.Forms.CheckBox()
        Me.txtDocNo = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.pnlSchSave = New System.Windows.Forms.Panel()
        Me.lblUserId = New System.Windows.Forms.Label()
        Me.lblRptID = New System.Windows.Forms.Label()
        Me.pnlMonthWise = New System.Windows.Forms.Panel()
        Me.optMonthEndDt = New System.Windows.Forms.RadioButton()
        Me.optMonthStartDt = New System.Windows.Forms.RadioButton()
        Me.lblrptNameSch = New System.Windows.Forms.Label()
        Me.optOutLook = New System.Windows.Forms.RadioButton()
        Me.optSmtp = New System.Windows.Forms.RadioButton()
        CType(Me.vsfSch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PnlRpt.SuspendLayout()
        Me.pnlDayWise.SuspendLayout()
        Me.pnlShowDays.SuspendLayout()
        Me.pnlSchSave.SuspendLayout()
        Me.pnlMonthWise.SuspendLayout()
        Me.SuspendLayout()
        '
        'vsfSch
        '
        Me.vsfSch.AllowFiltering = True
        Me.vsfSch.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfSch.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfSch.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfSch.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfSch.ColumnInfo = "5,4,0,0,0,165,Columns:0{Width:1;}" & Global.Microsoft.VisualBasic.ChrW(9) & "1{Width:269;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfSch.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.vsfSch.Location = New System.Drawing.Point(13, 59)
        Me.vsfSch.Margin = New System.Windows.Forms.Padding(4)
        Me.vsfSch.Name = "vsfSch"
        Me.vsfSch.Rows.Count = 2
        Me.vsfSch.Rows.DefaultSize = 33
        Me.vsfSch.Rows.Fixed = 2
        Me.vsfSch.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfSch.Size = New System.Drawing.Size(1149, 611)
        Me.vsfSch.StyleInfo = resources.GetString("vsfSch.StyleInfo")
        Me.vsfSch.TabIndex = 121
        Me.vsfSch.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2007Black
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.btnClose.Location = New System.Drawing.Point(1061, 678)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(100, 36)
        Me.btnClose.TabIndex = 264
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(955, 679)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(100, 36)
        Me.btnSave.TabIndex = 4
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = False
        Me.btnSave.Visible = False
        '
        'btnExport
        '
        Me.btnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExport.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnExport.Image = CType(resources.GetObject("btnExport.Image"), System.Drawing.Image)
        Me.btnExport.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnExport.Location = New System.Drawing.Point(13, 679)
        Me.btnExport.Margin = New System.Windows.Forms.Padding(4)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(41, 38)
        Me.btnExport.TabIndex = 265
        Me.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnExport.UseVisualStyleBackColor = False
        '
        'btnRemSch
        '
        Me.btnRemSch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRemSch.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.btnRemSch.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnRemSch.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnRemSch.Location = New System.Drawing.Point(491, 678)
        Me.btnRemSch.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRemSch.Name = "btnRemSch"
        Me.btnRemSch.Size = New System.Drawing.Size(456, 39)
        Me.btnRemSch.TabIndex = 266
        Me.btnRemSch.Text = "Remove All Schedules"
        Me.btnRemSch.UseVisualStyleBackColor = False
        '
        'btnVewRpt
        '
        Me.btnVewRpt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnVewRpt.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnVewRpt.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnVewRpt.Location = New System.Drawing.Point(71, 679)
        Me.btnVewRpt.Margin = New System.Windows.Forms.Padding(4)
        Me.btnVewRpt.Name = "btnVewRpt"
        Me.btnVewRpt.Size = New System.Drawing.Size(125, 38)
        Me.btnVewRpt.TabIndex = 647
        Me.btnVewRpt.Text = "View Report"
        Me.btnVewRpt.UseVisualStyleBackColor = False
        Me.btnVewRpt.Visible = False
        '
        'PnlRpt
        '
        Me.PnlRpt.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.PnlRpt.Controls.Add(Me.Label1)
        Me.PnlRpt.Controls.Add(Me.btnRptSave)
        Me.PnlRpt.Controls.Add(Me.btnRptCls)
        Me.PnlRpt.Controls.Add(Me.txtRmk)
        Me.PnlRpt.Controls.Add(Me.Label4)
        Me.PnlRpt.Controls.Add(Me.ChkInAct)
        Me.PnlRpt.Controls.Add(Me.txtAddEmail)
        Me.PnlRpt.Controls.Add(Me.txtRptName)
        Me.PnlRpt.Controls.Add(Me.lblAddEmail)
        Me.PnlRpt.Location = New System.Drawing.Point(224, 165)
        Me.PnlRpt.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.PnlRpt.Name = "PnlRpt"
        Me.PnlRpt.Size = New System.Drawing.Size(615, 350)
        Me.PnlRpt.TabIndex = 281
        Me.PnlRpt.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(40, 19)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(108, 20)
        Me.Label1.TabIndex = 292
        Me.Label1.Text = "Report Name"
        '
        'btnRptSave
        '
        Me.btnRptSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRptSave.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnRptSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnRptSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnRptSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRptSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.btnRptSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRptSave.Location = New System.Drawing.Point(317, 304)
        Me.btnRptSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRptSave.Name = "btnRptSave"
        Me.btnRptSave.Size = New System.Drawing.Size(100, 36)
        Me.btnRptSave.TabIndex = 295
        Me.btnRptSave.Text = "&Save"
        Me.btnRptSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRptSave.UseVisualStyleBackColor = False
        '
        'btnRptCls
        '
        Me.btnRptCls.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRptCls.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnRptCls.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnRptCls.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnRptCls.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRptCls.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.btnRptCls.Location = New System.Drawing.Point(425, 304)
        Me.btnRptCls.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRptCls.Name = "btnRptCls"
        Me.btnRptCls.Size = New System.Drawing.Size(113, 36)
        Me.btnRptCls.TabIndex = 296
        Me.btnRptCls.Text = "Close"
        Me.btnRptCls.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRptCls.UseVisualStyleBackColor = False
        '
        'txtRmk
        '
        Me.txtRmk.Location = New System.Drawing.Point(199, 214)
        Me.txtRmk.Margin = New System.Windows.Forms.Padding(4)
        Me.txtRmk.Multiline = True
        Me.txtRmk.Name = "txtRmk"
        Me.txtRmk.Size = New System.Drawing.Size(339, 63)
        Me.txtRmk.TabIndex = 294
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(40, 212)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(76, 20)
        Me.Label4.TabIndex = 293
        Me.Label4.Text = "Remarks"
        '
        'ChkInAct
        '
        Me.ChkInAct.AutoSize = True
        Me.ChkInAct.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!)
        Me.ChkInAct.Location = New System.Drawing.Point(199, 172)
        Me.ChkInAct.Margin = New System.Windows.Forms.Padding(5)
        Me.ChkInAct.Name = "ChkInAct"
        Me.ChkInAct.Size = New System.Drawing.Size(98, 28)
        Me.ChkInAct.TabIndex = 292
        Me.ChkInAct.Text = "InActive"
        Me.ChkInAct.UseVisualStyleBackColor = True
        '
        'txtAddEmail
        '
        Me.txtAddEmail.Location = New System.Drawing.Point(199, 69)
        Me.txtAddEmail.Margin = New System.Windows.Forms.Padding(4)
        Me.txtAddEmail.Multiline = True
        Me.txtAddEmail.Name = "txtAddEmail"
        Me.txtAddEmail.Size = New System.Drawing.Size(339, 83)
        Me.txtAddEmail.TabIndex = 291
        '
        'txtRptName
        '
        Me.txtRptName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRptName.Location = New System.Drawing.Point(199, 17)
        Me.txtRptName.Margin = New System.Windows.Forms.Padding(4)
        Me.txtRptName.Name = "txtRptName"
        Me.txtRptName.Size = New System.Drawing.Size(339, 30)
        Me.txtRptName.TabIndex = 290
        '
        'lblAddEmail
        '
        Me.lblAddEmail.AutoSize = True
        Me.lblAddEmail.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddEmail.Location = New System.Drawing.Point(40, 71)
        Me.lblAddEmail.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblAddEmail.Name = "lblAddEmail"
        Me.lblAddEmail.Size = New System.Drawing.Size(138, 20)
        Me.lblAddEmail.TabIndex = 287
        Me.lblAddEmail.Text = "Additional Emails"
        '
        'lblRptName
        '
        Me.lblRptName.AutoSize = True
        Me.lblRptName.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblRptName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRptName.Location = New System.Drawing.Point(159, 18)
        Me.lblRptName.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblRptName.Name = "lblRptName"
        Me.lblRptName.Size = New System.Drawing.Size(19, 25)
        Me.lblRptName.TabIndex = 289
        Me.lblRptName.Text = "-"
        '
        'btnAddRpt
        '
        Me.btnAddRpt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAddRpt.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnAddRpt.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnAddRpt.Location = New System.Drawing.Point(204, 679)
        Me.btnAddRpt.Margin = New System.Windows.Forms.Padding(4)
        Me.btnAddRpt.Name = "btnAddRpt"
        Me.btnAddRpt.Size = New System.Drawing.Size(189, 38)
        Me.btnAddRpt.TabIndex = 649
        Me.btnAddRpt.Text = "Add New Report"
        Me.btnAddRpt.UseVisualStyleBackColor = False
        '
        'chkSelRpt
        '
        Me.chkSelRpt.AutoSize = True
        Me.chkSelRpt.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!)
        Me.chkSelRpt.Location = New System.Drawing.Point(13, 16)
        Me.chkSelRpt.Margin = New System.Windows.Forms.Padding(5)
        Me.chkSelRpt.Name = "chkSelRpt"
        Me.chkSelRpt.Size = New System.Drawing.Size(223, 28)
        Me.chkSelRpt.TabIndex = 650
        Me.chkSelRpt.Text = "View Selected Reports"
        Me.chkSelRpt.UseVisualStyleBackColor = True
        Me.chkSelRpt.Visible = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnCancel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.btnCancel.Location = New System.Drawing.Point(896, 265)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(113, 36)
        Me.btnCancel.TabIndex = 5
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnSchSave
        '
        Me.btnSchSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSchSave.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnSchSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSchSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSchSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSchSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.btnSchSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSchSave.Location = New System.Drawing.Point(788, 265)
        Me.btnSchSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSchSave.Name = "btnSchSave"
        Me.btnSchSave.Size = New System.Drawing.Size(100, 36)
        Me.btnSchSave.TabIndex = 4
        Me.btnSchSave.Text = "&Save"
        Me.btnSchSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSchSave.UseVisualStyleBackColor = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(20, 79)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(123, 20)
        Me.Label6.TabIndex = 269
        Me.Label6.Text = "Schedule Info"
        '
        'pnlDayWise
        '
        Me.pnlDayWise.Controls.Add(Me.optMonthWise)
        Me.pnlDayWise.Controls.Add(Me.optDayWise)
        Me.pnlDayWise.Controls.Add(Me.optSelDays)
        Me.pnlDayWise.Location = New System.Drawing.Point(17, 101)
        Me.pnlDayWise.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.pnlDayWise.Name = "pnlDayWise"
        Me.pnlDayWise.Size = New System.Drawing.Size(974, 41)
        Me.pnlDayWise.TabIndex = 272
        '
        'optMonthWise
        '
        Me.optMonthWise.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.optMonthWise.AutoSize = True
        Me.optMonthWise.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold)
        Me.optMonthWise.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.optMonthWise.Location = New System.Drawing.Point(695, 7)
        Me.optMonthWise.Margin = New System.Windows.Forms.Padding(4)
        Me.optMonthWise.Name = "optMonthWise"
        Me.optMonthWise.Size = New System.Drawing.Size(148, 29)
        Me.optMonthWise.TabIndex = 271
        Me.optMonthWise.Text = "Month Wise"
        Me.optMonthWise.UseVisualStyleBackColor = True
        '
        'optDayWise
        '
        Me.optDayWise.AutoSize = True
        Me.optDayWise.Checked = True
        Me.optDayWise.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold)
        Me.optDayWise.Location = New System.Drawing.Point(9, 7)
        Me.optDayWise.Margin = New System.Windows.Forms.Padding(4)
        Me.optDayWise.Name = "optDayWise"
        Me.optDayWise.Size = New System.Drawing.Size(81, 29)
        Me.optDayWise.TabIndex = 270
        Me.optDayWise.TabStop = True
        Me.optDayWise.Text = "Daily"
        Me.optDayWise.UseVisualStyleBackColor = True
        '
        'optSelDays
        '
        Me.optSelDays.AutoSize = True
        Me.optSelDays.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold)
        Me.optSelDays.Location = New System.Drawing.Point(170, 6)
        Me.optSelDays.Margin = New System.Windows.Forms.Padding(4)
        Me.optSelDays.Name = "optSelDays"
        Me.optSelDays.Size = New System.Drawing.Size(173, 29)
        Me.optSelDays.TabIndex = 272
        Me.optSelDays.Text = "Selected Days"
        Me.optSelDays.UseVisualStyleBackColor = True
        '
        'pnlShowDays
        '
        Me.pnlShowDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlShowDays.Controls.Add(Me.chkWednesday)
        Me.pnlShowDays.Controls.Add(Me.chkFriday)
        Me.pnlShowDays.Controls.Add(Me.chkMonday)
        Me.pnlShowDays.Controls.Add(Me.chkThursday)
        Me.pnlShowDays.Controls.Add(Me.chkSaturday)
        Me.pnlShowDays.Controls.Add(Me.chkSunday)
        Me.pnlShowDays.Controls.Add(Me.chkTuesday)
        Me.pnlShowDays.Location = New System.Drawing.Point(180, 147)
        Me.pnlShowDays.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.pnlShowDays.Name = "pnlShowDays"
        Me.pnlShowDays.Size = New System.Drawing.Size(515, 91)
        Me.pnlShowDays.TabIndex = 281
        Me.pnlShowDays.Visible = False
        '
        'chkWednesday
        '
        Me.chkWednesday.AutoSize = True
        Me.chkWednesday.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!)
        Me.chkWednesday.Location = New System.Drawing.Point(248, 15)
        Me.chkWednesday.Margin = New System.Windows.Forms.Padding(5)
        Me.chkWednesday.Name = "chkWednesday"
        Me.chkWednesday.Size = New System.Drawing.Size(133, 28)
        Me.chkWednesday.TabIndex = 275
        Me.chkWednesday.Text = "Wednesday"
        Me.chkWednesday.UseVisualStyleBackColor = True
        '
        'chkFriday
        '
        Me.chkFriday.AutoSize = True
        Me.chkFriday.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!)
        Me.chkFriday.Location = New System.Drawing.Point(24, 52)
        Me.chkFriday.Margin = New System.Windows.Forms.Padding(5)
        Me.chkFriday.Name = "chkFriday"
        Me.chkFriday.Size = New System.Drawing.Size(84, 28)
        Me.chkFriday.TabIndex = 277
        Me.chkFriday.Text = "Friday"
        Me.chkFriday.UseVisualStyleBackColor = True
        '
        'chkMonday
        '
        Me.chkMonday.AutoSize = True
        Me.chkMonday.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!)
        Me.chkMonday.Location = New System.Drawing.Point(24, 15)
        Me.chkMonday.Margin = New System.Windows.Forms.Padding(5)
        Me.chkMonday.Name = "chkMonday"
        Me.chkMonday.Size = New System.Drawing.Size(100, 28)
        Me.chkMonday.TabIndex = 273
        Me.chkMonday.Text = "Monday"
        Me.chkMonday.UseVisualStyleBackColor = True
        '
        'chkThursday
        '
        Me.chkThursday.AutoSize = True
        Me.chkThursday.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!)
        Me.chkThursday.Location = New System.Drawing.Point(392, 15)
        Me.chkThursday.Margin = New System.Windows.Forms.Padding(5)
        Me.chkThursday.Name = "chkThursday"
        Me.chkThursday.Size = New System.Drawing.Size(111, 28)
        Me.chkThursday.TabIndex = 276
        Me.chkThursday.Text = "Thursday"
        Me.chkThursday.UseVisualStyleBackColor = True
        '
        'chkSaturday
        '
        Me.chkSaturday.AutoSize = True
        Me.chkSaturday.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!)
        Me.chkSaturday.Location = New System.Drawing.Point(133, 52)
        Me.chkSaturday.Margin = New System.Windows.Forms.Padding(5)
        Me.chkSaturday.Name = "chkSaturday"
        Me.chkSaturday.Size = New System.Drawing.Size(105, 28)
        Me.chkSaturday.TabIndex = 278
        Me.chkSaturday.Text = "Saturday"
        Me.chkSaturday.UseVisualStyleBackColor = True
        '
        'chkSunday
        '
        Me.chkSunday.AutoSize = True
        Me.chkSunday.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!)
        Me.chkSunday.Location = New System.Drawing.Point(248, 52)
        Me.chkSunday.Margin = New System.Windows.Forms.Padding(5)
        Me.chkSunday.Name = "chkSunday"
        Me.chkSunday.Size = New System.Drawing.Size(96, 28)
        Me.chkSunday.TabIndex = 279
        Me.chkSunday.Text = "Sunday"
        Me.chkSunday.UseVisualStyleBackColor = True
        '
        'chkTuesday
        '
        Me.chkTuesday.AutoSize = True
        Me.chkTuesday.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!)
        Me.chkTuesday.Location = New System.Drawing.Point(133, 15)
        Me.chkTuesday.Margin = New System.Windows.Forms.Padding(5)
        Me.chkTuesday.Name = "chkTuesday"
        Me.chkTuesday.Size = New System.Drawing.Size(105, 28)
        Me.chkTuesday.TabIndex = 274
        Me.chkTuesday.Text = "Tuesday"
        Me.chkTuesday.UseVisualStyleBackColor = True
        '
        'txtDocNo
        '
        Me.txtDocNo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDocNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDocNo.Location = New System.Drawing.Point(612, 9)
        Me.txtDocNo.Margin = New System.Windows.Forms.Padding(4)
        Me.txtDocNo.Name = "txtDocNo"
        Me.txtDocNo.Size = New System.Drawing.Size(52, 30)
        Me.txtDocNo.TabIndex = 284
        Me.txtDocNo.Visible = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(21, 18)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(126, 25)
        Me.Label3.TabIndex = 286
        Me.Label3.Text = "Report Name"
        '
        'pnlSchSave
        '
        Me.pnlSchSave.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.pnlSchSave.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.pnlSchSave.Controls.Add(Me.lblUserId)
        Me.pnlSchSave.Controls.Add(Me.lblRptID)
        Me.pnlSchSave.Controls.Add(Me.pnlMonthWise)
        Me.pnlSchSave.Controls.Add(Me.lblrptNameSch)
        Me.pnlSchSave.Controls.Add(Me.pnlShowDays)
        Me.pnlSchSave.Controls.Add(Me.Label3)
        Me.pnlSchSave.Controls.Add(Me.txtDocNo)
        Me.pnlSchSave.Controls.Add(Me.pnlDayWise)
        Me.pnlSchSave.Controls.Add(Me.lblRptName)
        Me.pnlSchSave.Controls.Add(Me.Label6)
        Me.pnlSchSave.Controls.Add(Me.btnSchSave)
        Me.pnlSchSave.Controls.Add(Me.btnCancel)
        Me.pnlSchSave.Location = New System.Drawing.Point(33, 217)
        Me.pnlSchSave.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.pnlSchSave.Name = "pnlSchSave"
        Me.pnlSchSave.Size = New System.Drawing.Size(1022, 310)
        Me.pnlSchSave.TabIndex = 648
        Me.pnlSchSave.Visible = False
        '
        'lblUserId
        '
        Me.lblUserId.AutoSize = True
        Me.lblUserId.Location = New System.Drawing.Point(755, 52)
        Me.lblUserId.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblUserId.Name = "lblUserId"
        Me.lblUserId.Size = New System.Drawing.Size(51, 17)
        Me.lblUserId.TabIndex = 291
        Me.lblUserId.Text = "Label2"
        Me.lblUserId.Visible = False
        '
        'lblRptID
        '
        Me.lblRptID.AutoSize = True
        Me.lblRptID.Location = New System.Drawing.Point(703, 49)
        Me.lblRptID.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblRptID.Name = "lblRptID"
        Me.lblRptID.Size = New System.Drawing.Size(51, 17)
        Me.lblRptID.TabIndex = 290
        Me.lblRptID.Text = "Label1"
        Me.lblRptID.Visible = False
        '
        'pnlMonthWise
        '
        Me.pnlMonthWise.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlMonthWise.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlMonthWise.Controls.Add(Me.optMonthEndDt)
        Me.pnlMonthWise.Controls.Add(Me.optMonthStartDt)
        Me.pnlMonthWise.Location = New System.Drawing.Point(712, 145)
        Me.pnlMonthWise.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.pnlMonthWise.Name = "pnlMonthWise"
        Me.pnlMonthWise.Size = New System.Drawing.Size(279, 92)
        Me.pnlMonthWise.TabIndex = 280
        Me.pnlMonthWise.Visible = False
        '
        'optMonthEndDt
        '
        Me.optMonthEndDt.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.optMonthEndDt.AutoSize = True
        Me.optMonthEndDt.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!)
        Me.optMonthEndDt.Location = New System.Drawing.Point(19, 52)
        Me.optMonthEndDt.Margin = New System.Windows.Forms.Padding(4)
        Me.optMonthEndDt.Name = "optMonthEndDt"
        Me.optMonthEndDt.Size = New System.Drawing.Size(167, 28)
        Me.optMonthEndDt.TabIndex = 272
        Me.optMonthEndDt.Text = "Month End Date"
        Me.optMonthEndDt.UseVisualStyleBackColor = True
        '
        'optMonthStartDt
        '
        Me.optMonthStartDt.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.optMonthStartDt.AutoSize = True
        Me.optMonthStartDt.Checked = True
        Me.optMonthStartDt.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!)
        Me.optMonthStartDt.Location = New System.Drawing.Point(19, 16)
        Me.optMonthStartDt.Margin = New System.Windows.Forms.Padding(4)
        Me.optMonthStartDt.Name = "optMonthStartDt"
        Me.optMonthStartDt.Size = New System.Drawing.Size(168, 28)
        Me.optMonthStartDt.TabIndex = 271
        Me.optMonthStartDt.TabStop = True
        Me.optMonthStartDt.Text = "Month Start Date"
        Me.optMonthStartDt.UseVisualStyleBackColor = True
        '
        'lblrptNameSch
        '
        Me.lblrptNameSch.AutoSize = True
        Me.lblrptNameSch.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblrptNameSch.Location = New System.Drawing.Point(176, 18)
        Me.lblrptNameSch.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblrptNameSch.Name = "lblrptNameSch"
        Me.lblrptNameSch.Size = New System.Drawing.Size(0, 20)
        Me.lblrptNameSch.TabIndex = 289
        '
        'optOutLook
        '
        Me.optOutLook.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.optOutLook.AutoSize = True
        Me.optOutLook.Checked = True
        Me.optOutLook.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Bold)
        Me.optOutLook.Location = New System.Drawing.Point(797, 13)
        Me.optOutLook.Margin = New System.Windows.Forms.Padding(4)
        Me.optOutLook.Name = "optOutLook"
        Me.optOutLook.Size = New System.Drawing.Size(170, 28)
        Me.optOutLook.TabIndex = 651
        Me.optOutLook.TabStop = True
        Me.optOutLook.Text = "Outlook Server"
        Me.optOutLook.UseVisualStyleBackColor = True
        '
        'optSmtp
        '
        Me.optSmtp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.optSmtp.AutoSize = True
        Me.optSmtp.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Bold)
        Me.optSmtp.Location = New System.Drawing.Point(1007, 13)
        Me.optSmtp.Margin = New System.Windows.Forms.Padding(4)
        Me.optSmtp.Name = "optSmtp"
        Me.optSmtp.Size = New System.Drawing.Size(154, 28)
        Me.optSmtp.TabIndex = 652
        Me.optSmtp.Text = "SMTP Server"
        Me.optSmtp.UseVisualStyleBackColor = True
        '
        'frmUtlAutoEmailSch
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1175, 727)
        Me.Controls.Add(Me.optSmtp)
        Me.Controls.Add(Me.optOutLook)
        Me.Controls.Add(Me.chkSelRpt)
        Me.Controls.Add(Me.PnlRpt)
        Me.Controls.Add(Me.btnAddRpt)
        Me.Controls.Add(Me.pnlSchSave)
        Me.Controls.Add(Me.btnVewRpt)
        Me.Controls.Add(Me.btnRemSch)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.vsfSch)
        Me.Controls.Add(Me.btnSave)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "frmUtlAutoEmailSch"
        Me.Text = "Auto Email Scheduler"
        CType(Me.vsfSch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PnlRpt.ResumeLayout(False)
        Me.PnlRpt.PerformLayout()
        Me.pnlDayWise.ResumeLayout(False)
        Me.pnlDayWise.PerformLayout()
        Me.pnlShowDays.ResumeLayout(False)
        Me.pnlShowDays.PerformLayout()
        Me.pnlSchSave.ResumeLayout(False)
        Me.pnlSchSave.PerformLayout()
        Me.pnlMonthWise.ResumeLayout(False)
        Me.pnlMonthWise.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents vsfSch As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnClose As Button
    Friend WithEvents btnSave As Button
    Friend WithEvents btnExport As C1.Win.C1Input.C1Button
    Friend WithEvents btnRemSch As C1.Win.C1Input.C1Button
    Friend WithEvents btnVewRpt As C1.Win.C1Input.C1Button
    Friend WithEvents PnlRpt As Panel
    Friend WithEvents btnRptSave As Button
    Friend WithEvents btnRptCls As Button
    Friend WithEvents txtRmk As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents ChkInAct As CheckBox
    Friend WithEvents txtAddEmail As TextBox
    Friend WithEvents txtRptName As TextBox
    Friend WithEvents lblRptName As Label
    Friend WithEvents lblAddEmail As Label
    Friend WithEvents btnAddRpt As C1.Win.C1Input.C1Button
    Friend WithEvents chkSelRpt As CheckBox
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnSchSave As Button
    Friend WithEvents Label6 As Label
    Friend WithEvents pnlDayWise As Panel
    Friend WithEvents optMonthWise As RadioButton
    Friend WithEvents optDayWise As RadioButton
    Friend WithEvents optSelDays As RadioButton
    Friend WithEvents pnlShowDays As Panel
    Friend WithEvents chkWednesday As CheckBox
    Friend WithEvents chkMonday As CheckBox
    Friend WithEvents chkSunday As CheckBox
    Friend WithEvents chkTuesday As CheckBox
    Friend WithEvents chkSaturday As CheckBox
    Friend WithEvents chkThursday As CheckBox
    Friend WithEvents chkFriday As CheckBox
    Friend WithEvents txtDocNo As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents pnlSchSave As Panel
    Friend WithEvents lblrptNameSch As Label
    Friend WithEvents pnlMonthWise As Panel
    Friend WithEvents optMonthEndDt As RadioButton
    Friend WithEvents optMonthStartDt As RadioButton
    Friend WithEvents lblUserId As Label
    Friend WithEvents lblRptID As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents optOutLook As RadioButton
    Friend WithEvents optSmtp As RadioButton
End Class
