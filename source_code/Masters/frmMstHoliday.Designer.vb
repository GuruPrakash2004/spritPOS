<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMstHoliday
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMstHoliday))
        Me.vsfCalender = New C1.Win.C1Schedule.C1Calendar()
        Me.lblfrom = New System.Windows.Forms.Label()
        Me.dtpFromdate = New System.Windows.Forms.DateTimePicker()
        Me.lblTo = New System.Windows.Forms.Label()
        Me.dtpTodate = New System.Windows.Forms.DateTimePicker()
        Me.lblMonthsel = New System.Windows.Forms.Label()
        Me.DateName = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnDel = New System.Windows.Forms.Button()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.btnFrmClose = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.cboMonth = New System.Windows.Forms.ComboBox()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.grpDaterange = New System.Windows.Forms.GroupBox()
        Me.pnlSave = New System.Windows.Forms.Panel()
        Me.txtHolidayName = New System.Windows.Forms.TextBox()
        Me.dtpDateSel = New System.Windows.Forms.DateTimePicker()
        Me.lblHoliday = New System.Windows.Forms.Label()
        Me.lblDateSel = New System.Windows.Forms.Label()
        Me.pnlCalender = New System.Windows.Forms.Panel()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.vsfSearch = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.lblSearch = New System.Windows.Forms.Label()
        CType(Me.vsfCalender, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpDaterange.SuspendLayout()
        Me.pnlSave.SuspendLayout()
        Me.pnlCalender.SuspendLayout()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'vsfCalender
        '
        Me.vsfCalender.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfCalender.BoldedDates = New Date(-1) {}
        '
        '
        '
        Me.vsfCalender.CalendarInfo.CultureInfo = New System.Globalization.CultureInfo("en-IN")
        Me.vsfCalender.CalendarInfo.EndDayTime = System.TimeSpan.Parse("19:00:00")
        Me.vsfCalender.CalendarInfo.FirstDate = New Date(2000, 1, 1, 0, 0, 0, 0)
        Me.vsfCalender.CalendarInfo.LastDate = New Date(2220, 12, 31, 0, 0, 0, 0)
        Me.vsfCalender.CalendarInfo.StartDayTime = System.TimeSpan.Parse("07:00:00")
        Me.vsfCalender.CalendarInfo.TimeScale = System.TimeSpan.Parse("00:30:00")
        Me.vsfCalender.CalendarInfo.WeekStart = System.DayOfWeek.Monday
        Me.vsfCalender.CalendarInfo.WorkDays.AddRange(New System.DayOfWeek() {System.DayOfWeek.Monday, System.DayOfWeek.Tuesday, System.DayOfWeek.Wednesday, System.DayOfWeek.Thursday, System.DayOfWeek.Friday})
        Me.vsfCalender.DropDownMonthCount = 1
        Me.vsfCalender.Location = New System.Drawing.Point(0, 0)
        Me.vsfCalender.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.vsfCalender.Name = "vsfCalender"
        Me.vsfCalender.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.vsfCalender.ShowNavigationButtons = False
        Me.vsfCalender.ShowWeekNumbers = False
        Me.vsfCalender.Size = New System.Drawing.Size(151, 143)
        Me.vsfCalender.TabIndex = 0
        '
        '
        '
        Me.vsfCalender.Theme.XmlDefinition = resources.GetString("resource.XmlDefinition")
        Me.vsfCalender.VisualStyle = C1.Win.C1Schedule.UI.VisualStyle.Custom
        '
        'lblfrom
        '
        Me.lblfrom.AutoSize = True
        Me.lblfrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblfrom.Location = New System.Drawing.Point(3, 30)
        Me.lblfrom.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblfrom.Name = "lblfrom"
        Me.lblfrom.Size = New System.Drawing.Size(46, 20)
        Me.lblfrom.TabIndex = 2
        Me.lblfrom.Text = "From"
        '
        'dtpFromdate
        '
        Me.dtpFromdate.CalendarFont = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpFromdate.CustomFormat = "dd/MM/yyyy"
        Me.dtpFromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpFromdate.Location = New System.Drawing.Point(57, 27)
        Me.dtpFromdate.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dtpFromdate.MinDate = New Date(2018, 4, 1, 0, 0, 0, 0)
        Me.dtpFromdate.Name = "dtpFromdate"
        Me.dtpFromdate.Size = New System.Drawing.Size(113, 26)
        Me.dtpFromdate.TabIndex = 0
        Me.dtpFromdate.Value = New Date(2018, 4, 1, 0, 0, 0, 0)
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTo.Location = New System.Drawing.Point(189, 30)
        Me.lblTo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(27, 20)
        Me.lblTo.TabIndex = 4
        Me.lblTo.Text = "To"
        '
        'dtpTodate
        '
        Me.dtpTodate.CalendarFont = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpTodate.CustomFormat = "dd/MM/yyyy"
        Me.dtpTodate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpTodate.Location = New System.Drawing.Point(232, 27)
        Me.dtpTodate.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dtpTodate.MinDate = New Date(2018, 4, 1, 0, 0, 0, 0)
        Me.dtpTodate.Name = "dtpTodate"
        Me.dtpTodate.Size = New System.Drawing.Size(114, 26)
        Me.dtpTodate.TabIndex = 1
        Me.dtpTodate.Value = New Date(2019, 3, 31, 0, 0, 0, 0)
        '
        'lblMonthsel
        '
        Me.lblMonthsel.AutoSize = True
        Me.lblMonthsel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMonthsel.Location = New System.Drawing.Point(10, 8)
        Me.lblMonthsel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblMonthsel.Name = "lblMonthsel"
        Me.lblMonthsel.Size = New System.Drawing.Size(140, 20)
        Me.lblMonthsel.TabIndex = 6
        Me.lblMonthsel.Text = "Number of Months"
        '
        'btnSave
        '
        Me.btnSave.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSave.FlatAppearance.BorderSize = 0
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ForeColor = System.Drawing.Color.Black
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(117, 115)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(96, 34)
        Me.btnSave.TabIndex = 3
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.DateName.SetToolTip(Me.btnSave, "Shortcut: Alt + S")
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatAppearance.BorderSize = 0
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ForeColor = System.Drawing.Color.Black
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(220, 115)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(88, 34)
        Me.btnClose.TabIndex = 4
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.DateName.SetToolTip(Me.btnClose, "Close : Alt + O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnDel
        '
        Me.btnDel.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.btnDel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_DELETE
        Me.btnDel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnDel.FlatAppearance.BorderSize = 0
        Me.btnDel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDel.ForeColor = System.Drawing.Color.Black
        Me.btnDel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDel.Location = New System.Drawing.Point(11, 115)
        Me.btnDel.Name = "btnDel"
        Me.btnDel.Size = New System.Drawing.Size(99, 34)
        Me.btnDel.TabIndex = 2
        Me.btnDel.Text = "&Delete"
        Me.btnDel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.DateName.SetToolTip(Me.btnDel, "Delete : Alt + D")
        Me.btnDel.UseVisualStyleBackColor = False
        '
        'btnRefresh
        '
        Me.btnRefresh.BackColor = System.Drawing.Color.Transparent
        Me.btnRefresh.BackgroundImage = Global.vLQMS.My.Resources.Resources.ucRestore1
        Me.btnRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRefresh.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRefresh.Location = New System.Drawing.Point(365, 27)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(30, 23)
        Me.btnRefresh.TabIndex = 2
        Me.btnRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.DateName.SetToolTip(Me.btnRefresh, "Refresh")
        Me.btnRefresh.UseVisualStyleBackColor = False
        '
        'btnFrmClose
        '
        Me.btnFrmClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFrmClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnFrmClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnFrmClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnFrmClose.ForeColor = System.Drawing.Color.Black
        Me.btnFrmClose.Location = New System.Drawing.Point(807, 415)
        Me.btnFrmClose.Name = "btnFrmClose"
        Me.btnFrmClose.Size = New System.Drawing.Size(86, 34)
        Me.btnFrmClose.TabIndex = 8
        Me.btnFrmClose.Text = "C&lose"
        Me.btnFrmClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.DateName.SetToolTip(Me.btnFrmClose, "Close : Alt+L")
        Me.btnFrmClose.UseVisualStyleBackColor = True
        '
        'btnExport
        '
        Me.btnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExport.BackColor = System.Drawing.Color.Transparent
        Me.btnExport.BackgroundImage = Global.vLQMS.My.Resources.Resources.ucexport_to_XL
        Me.btnExport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnExport.Location = New System.Drawing.Point(490, 415)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(39, 34)
        Me.btnExport.TabIndex = 7
        Me.DateName.SetToolTip(Me.btnExport, "Export To Excel : Alt + E")
        Me.btnExport.UseVisualStyleBackColor = False
        '
        'cboMonth
        '
        Me.cboMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboMonth.FormattingEnabled = True
        Me.cboMonth.Items.AddRange(New Object() {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"})
        Me.cboMonth.Location = New System.Drawing.Point(153, 5)
        Me.cboMonth.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cboMonth.Name = "cboMonth"
        Me.cboMonth.Size = New System.Drawing.Size(77, 28)
        Me.cboMonth.TabIndex = 0
        '
        'vsfDet
        '
        Me.vsfDet.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfDet.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "3,0,0,0,0,125,Columns:0{Width:53;}" & Global.Microsoft.VisualBasic.ChrW(9) & "1{Style:""Format:""""d"""";DataType:System.DateTime" &
    ";TextAlign:LeftCenter;"";}" & Global.Microsoft.VisualBasic.ChrW(9) & "2{Style:""TextAlign:GeneralCenter;"";}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.vsfDet.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.vsfDet.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(491, 104)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.Count = 1
        Me.vsfDet.Rows.DefaultSize = 25
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(402, 301)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 5
        '
        'grpDaterange
        '
        Me.grpDaterange.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpDaterange.Controls.Add(Me.btnRefresh)
        Me.grpDaterange.Controls.Add(Me.dtpFromdate)
        Me.grpDaterange.Controls.Add(Me.lblfrom)
        Me.grpDaterange.Controls.Add(Me.lblTo)
        Me.grpDaterange.Controls.Add(Me.dtpTodate)
        Me.grpDaterange.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.grpDaterange.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpDaterange.Location = New System.Drawing.Point(491, 9)
        Me.grpDaterange.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.grpDaterange.Name = "grpDaterange"
        Me.grpDaterange.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.grpDaterange.Size = New System.Drawing.Size(402, 63)
        Me.grpDaterange.TabIndex = 3
        Me.grpDaterange.TabStop = False
        Me.grpDaterange.Text = "Select Date Range Between Financial Year"
        '
        'pnlSave
        '
        Me.pnlSave.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.pnlSave.Controls.Add(Me.btnDel)
        Me.pnlSave.Controls.Add(Me.txtHolidayName)
        Me.pnlSave.Controls.Add(Me.btnClose)
        Me.pnlSave.Controls.Add(Me.btnSave)
        Me.pnlSave.Controls.Add(Me.dtpDateSel)
        Me.pnlSave.Controls.Add(Me.lblHoliday)
        Me.pnlSave.Controls.Add(Me.lblDateSel)
        Me.pnlSave.Location = New System.Drawing.Point(169, 106)
        Me.pnlSave.Name = "pnlSave"
        Me.pnlSave.Size = New System.Drawing.Size(315, 156)
        Me.pnlSave.TabIndex = 2
        Me.pnlSave.Visible = False
        '
        'txtHolidayName
        '
        Me.txtHolidayName.Location = New System.Drawing.Point(117, 40)
        Me.txtHolidayName.MaxLength = 99
        Me.txtHolidayName.Multiline = True
        Me.txtHolidayName.Name = "txtHolidayName"
        Me.txtHolidayName.Size = New System.Drawing.Size(191, 68)
        Me.txtHolidayName.TabIndex = 1
        '
        'dtpDateSel
        '
        Me.dtpDateSel.CustomFormat = "dd/MM/yyyy"
        Me.dtpDateSel.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpDateSel.Location = New System.Drawing.Point(117, 8)
        Me.dtpDateSel.MinDate = New Date(2018, 4, 1, 0, 0, 0, 0)
        Me.dtpDateSel.Name = "dtpDateSel"
        Me.dtpDateSel.Size = New System.Drawing.Size(191, 26)
        Me.dtpDateSel.TabIndex = 0
        '
        'lblHoliday
        '
        Me.lblHoliday.AutoSize = True
        Me.lblHoliday.Location = New System.Drawing.Point(7, 57)
        Me.lblHoliday.Name = "lblHoliday"
        Me.lblHoliday.Size = New System.Drawing.Size(107, 20)
        Me.lblHoliday.TabIndex = 1
        Me.lblHoliday.Text = "Holiday Name"
        '
        'lblDateSel
        '
        Me.lblDateSel.AutoSize = True
        Me.lblDateSel.Location = New System.Drawing.Point(7, 13)
        Me.lblDateSel.Name = "lblDateSel"
        Me.lblDateSel.Size = New System.Drawing.Size(44, 20)
        Me.lblDateSel.TabIndex = 0
        Me.lblDateSel.Text = "Date"
        '
        'pnlCalender
        '
        Me.pnlCalender.Controls.Add(Me.vsfCalender)
        Me.pnlCalender.Location = New System.Drawing.Point(7, 36)
        Me.pnlCalender.Name = "pnlCalender"
        Me.pnlCalender.Size = New System.Drawing.Size(477, 413)
        Me.pnlCalender.TabIndex = 1
        '
        'txtSearch
        '
        Me.txtSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSearch.Location = New System.Drawing.Point(566, 77)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(327, 26)
        Me.txtSearch.TabIndex = 4
        '
        'vsfSearch
        '
        Me.vsfSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfSearch.BackColor = System.Drawing.Color.White
        Me.vsfSearch.ColumnInfo = "1,0,0,0,0,110,Columns:"
        Me.vsfSearch.ExtendLastCol = True
        Me.vsfSearch.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.vsfSearch.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
        Me.vsfSearch.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfSearch.Location = New System.Drawing.Point(490, 103)
        Me.vsfSearch.Name = "vsfSearch"
        Me.vsfSearch.Rows.Count = 1
        Me.vsfSearch.Rows.DefaultSize = 22
        Me.vsfSearch.Rows.Fixed = 0
        Me.vsfSearch.Size = New System.Drawing.Size(404, 302)
        Me.vsfSearch.StyleInfo = resources.GetString("vsfSearch.StyleInfo")
        Me.vsfSearch.TabIndex = 6
        Me.vsfSearch.Visible = False
        '
        'lblSearch
        '
        Me.lblSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSearch.AutoSize = True
        Me.lblSearch.Location = New System.Drawing.Point(494, 80)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(60, 20)
        Me.lblSearch.TabIndex = 239
        Me.lblSearch.Text = "Search"
        '
        'frmMstHoliday
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(902, 456)
        Me.Controls.Add(Me.lblSearch)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.btnFrmClose)
        Me.Controls.Add(Me.vsfSearch)
        Me.Controls.Add(Me.txtSearch)
        Me.Controls.Add(Me.cboMonth)
        Me.Controls.Add(Me.grpDaterange)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.lblMonthsel)
        Me.Controls.Add(Me.pnlCalender)
        Me.Controls.Add(Me.pnlSave)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmMstHoliday"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Holiday Master"
        CType(Me.vsfCalender, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpDaterange.ResumeLayout(False)
        Me.grpDaterange.PerformLayout()
        Me.pnlSave.ResumeLayout(False)
        Me.pnlSave.PerformLayout()
        Me.pnlCalender.ResumeLayout(False)
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents vsfCalender As C1.Win.C1Schedule.C1Calendar
    Friend WithEvents lblfrom As Label
    Friend WithEvents dtpFromdate As DateTimePicker
    Friend WithEvents lblTo As Label
    Friend WithEvents dtpTodate As DateTimePicker
    Friend WithEvents lblMonthsel As Label
    Friend WithEvents DateName As ToolTip
    Friend WithEvents cboMonth As ComboBox
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents grpDaterange As GroupBox
    Friend WithEvents btnRefresh As Button
    Friend WithEvents pnlSave As Panel
    Friend WithEvents dtpDateSel As DateTimePicker
    Friend WithEvents lblHoliday As Label
    Friend WithEvents lblDateSel As Label
    Friend WithEvents btnSave As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents txtHolidayName As TextBox
    Friend WithEvents btnDel As Button
    Friend WithEvents pnlCalender As Panel
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents vsfSearch As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnFrmClose As Button
    Friend WithEvents btnExport As Button
    Friend WithEvents lblSearch As Label
End Class
