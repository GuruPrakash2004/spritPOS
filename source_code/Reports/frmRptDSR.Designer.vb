<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmRptDailySales
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRptDailySales))
        Me.chkCustName = New System.Windows.Forms.CheckBox()
        Me.chkZone = New System.Windows.Forms.CheckBox()
        Me.chkItemGroup = New System.Windows.Forms.CheckBox()
        Me.lblTripNo = New System.Windows.Forms.Label()
        Me.pnlDate = New System.Windows.Forms.Panel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.dtpToDate = New System.Windows.Forms.DateTimePicker()
        Me.dtpFromDate = New System.Windows.Forms.DateTimePicker()
        Me.chkDate = New System.Windows.Forms.CheckBox()
        Me.chkSupp = New System.Windows.Forms.CheckBox()
        Me.chkMake = New System.Windows.Forms.CheckBox()
        Me.btnExcel = New System.Windows.Forms.Button()
        Me.optPending = New System.Windows.Forms.RadioButton()
        Me.optAll = New System.Windows.Forms.RadioButton()
        Me.chkMerge = New System.Windows.Forms.CheckBox()
        Me.chkUser = New System.Windows.Forms.CheckBox()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.cboTripNo = New System.Windows.Forms.ComboBox()
        Me.txtItemCode = New System.Windows.Forms.TextBox()
        Me.chkPrincipals = New System.Windows.Forms.CheckBox()
        Me.ChkRegion = New System.Windows.Forms.CheckBox()
        Me.pnPending = New System.Windows.Forms.Panel()
        Me.pnStk = New System.Windows.Forms.Panel()
        Me.optAll1 = New System.Windows.Forms.RadioButton()
        Me.optIS = New System.Windows.Forms.RadioButton()
        Me.optIC = New System.Windows.Forms.RadioButton()
        Me.optIZ = New System.Windows.Forms.RadioButton()
        Me.optItem = New System.Windows.Forms.RadioButton()
        Me.optICZ = New System.Windows.Forms.RadioButton()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnView = New System.Windows.Forms.Button()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfView = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.pnCOD = New System.Windows.Forms.Panel()
        Me.optCODCancel = New System.Windows.Forms.RadioButton()
        Me.OptPendCOd = New System.Windows.Forms.RadioButton()
        Me.optCODRcptList = New System.Windows.Forms.RadioButton()
        Me.pnlCopyShortCut = New System.Windows.Forms.Panel()
        Me.btnCopy = New System.Windows.Forms.Button()
        Me.pnlbtnClose = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.RbnCopyGrid = New System.Windows.Forms.RadioButton()
        Me.RbnCopyRow = New System.Windows.Forms.RadioButton()
        Me.RbnCopyColumn = New System.Windows.Forms.RadioButton()
        Me.RbnCopyCell = New System.Windows.Forms.RadioButton()
        Me.pnlDate.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.pnPending.SuspendLayout()
        Me.pnStk.SuspendLayout()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnCOD.SuspendLayout()
        Me.pnlCopyShortCut.SuspendLayout()
        Me.SuspendLayout()
        '
        'chkCustName
        '
        Me.chkCustName.AutoSize = True
        Me.chkCustName.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkCustName.Location = New System.Drawing.Point(4, 125)
        Me.chkCustName.Margin = New System.Windows.Forms.Padding(4)
        Me.chkCustName.Name = "chkCustName"
        Me.chkCustName.Size = New System.Drawing.Size(121, 21)
        Me.chkCustName.TabIndex = 226
        Me.chkCustName.Text = "Select Customer"
        Me.chkCustName.UseVisualStyleBackColor = True
        '
        'chkZone
        '
        Me.chkZone.AutoSize = True
        Me.chkZone.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkZone.Location = New System.Drawing.Point(246, 156)
        Me.chkZone.Margin = New System.Windows.Forms.Padding(4)
        Me.chkZone.Name = "chkZone"
        Me.chkZone.Size = New System.Drawing.Size(94, 21)
        Me.chkZone.TabIndex = 225
        Me.chkZone.Text = "Select Zone"
        Me.chkZone.UseVisualStyleBackColor = True
        '
        'chkItemGroup
        '
        Me.chkItemGroup.AutoSize = True
        Me.chkItemGroup.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkItemGroup.Location = New System.Drawing.Point(4, 4)
        Me.chkItemGroup.Margin = New System.Windows.Forms.Padding(4)
        Me.chkItemGroup.Name = "chkItemGroup"
        Me.chkItemGroup.Size = New System.Drawing.Size(135, 21)
        Me.chkItemGroup.TabIndex = 224
        Me.chkItemGroup.Text = "Select Item Group "
        Me.chkItemGroup.UseVisualStyleBackColor = True
        '
        'lblTripNo
        '
        Me.lblTripNo.AutoSize = True
        Me.lblTripNo.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTripNo.Location = New System.Drawing.Point(3, 92)
        Me.lblTripNo.Name = "lblTripNo"
        Me.lblTripNo.Size = New System.Drawing.Size(52, 17)
        Me.lblTripNo.TabIndex = 221
        Me.lblTripNo.Text = "Trip No"
        '
        'pnlDate
        '
        Me.pnlDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlDate.Controls.Add(Me.Label4)
        Me.pnlDate.Controls.Add(Me.Label3)
        Me.pnlDate.Controls.Add(Me.dtpToDate)
        Me.pnlDate.Controls.Add(Me.dtpFromDate)
        Me.pnlDate.Controls.Add(Me.chkDate)
        Me.pnlDate.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlDate.Location = New System.Drawing.Point(3, 33)
        Me.pnlDate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.pnlDate.Name = "pnlDate"
        Me.pnlDate.Size = New System.Drawing.Size(378, 55)
        Me.pnlDate.TabIndex = 219
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(172, 28)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(22, 17)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "To"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(3, 26)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(38, 17)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "From"
        '
        'dtpToDate
        '
        Me.dtpToDate.CustomFormat = "dd/MM/yyyy"
        Me.dtpToDate.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpToDate.Location = New System.Drawing.Point(199, 22)
        Me.dtpToDate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtpToDate.Name = "dtpToDate"
        Me.dtpToDate.Size = New System.Drawing.Size(112, 25)
        Me.dtpToDate.TabIndex = 4
        '
        'dtpFromDate
        '
        Me.dtpFromDate.CustomFormat = "dd/MM/yyyy"
        Me.dtpFromDate.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpFromDate.Location = New System.Drawing.Point(47, 22)
        Me.dtpFromDate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtpFromDate.Name = "dtpFromDate"
        Me.dtpFromDate.Size = New System.Drawing.Size(119, 25)
        Me.dtpFromDate.TabIndex = 3
        '
        'chkDate
        '
        Me.chkDate.AutoSize = True
        Me.chkDate.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDate.Location = New System.Drawing.Point(5, 2)
        Me.chkDate.Margin = New System.Windows.Forms.Padding(4)
        Me.chkDate.Name = "chkDate"
        Me.chkDate.Size = New System.Drawing.Size(95, 21)
        Me.chkDate.TabIndex = 228
        Me.chkDate.Text = "Date Range"
        Me.chkDate.UseVisualStyleBackColor = True
        '
        'chkSupp
        '
        Me.chkSupp.AutoSize = True
        Me.chkSupp.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSupp.Location = New System.Drawing.Point(133, 125)
        Me.chkSupp.Margin = New System.Windows.Forms.Padding(4)
        Me.chkSupp.Name = "chkSupp"
        Me.chkSupp.Size = New System.Drawing.Size(113, 21)
        Me.chkSupp.TabIndex = 230
        Me.chkSupp.Text = "Select Supplier"
        Me.chkSupp.UseVisualStyleBackColor = True
        '
        'chkMake
        '
        Me.chkMake.AutoSize = True
        Me.chkMake.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkMake.Location = New System.Drawing.Point(254, 125)
        Me.chkMake.Margin = New System.Windows.Forms.Padding(4)
        Me.chkMake.Name = "chkMake"
        Me.chkMake.Size = New System.Drawing.Size(97, 21)
        Me.chkMake.TabIndex = 231
        Me.chkMake.Text = "Select Make"
        Me.chkMake.UseVisualStyleBackColor = True
        '
        'btnExcel
        '
        Me.btnExcel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExcel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExcel.Location = New System.Drawing.Point(12, 579)
        Me.btnExcel.Name = "btnExcel"
        Me.btnExcel.Size = New System.Drawing.Size(113, 23)
        Me.btnExcel.TabIndex = 233
        Me.btnExcel.Text = "Export to Excel"
        Me.btnExcel.UseVisualStyleBackColor = True
        '
        'optPending
        '
        Me.optPending.AutoSize = True
        Me.optPending.Checked = True
        Me.optPending.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optPending.Location = New System.Drawing.Point(3, 3)
        Me.optPending.Name = "optPending"
        Me.optPending.Size = New System.Drawing.Size(73, 21)
        Me.optPending.TabIndex = 234
        Me.optPending.TabStop = True
        Me.optPending.Text = "Pending"
        Me.optPending.UseVisualStyleBackColor = True
        '
        'optAll
        '
        Me.optAll.AutoSize = True
        Me.optAll.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optAll.Location = New System.Drawing.Point(82, 3)
        Me.optAll.Name = "optAll"
        Me.optAll.Size = New System.Drawing.Size(40, 21)
        Me.optAll.TabIndex = 235
        Me.optAll.Text = "All"
        Me.optAll.UseVisualStyleBackColor = True
        '
        'chkMerge
        '
        Me.chkMerge.AutoSize = True
        Me.chkMerge.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkMerge.Location = New System.Drawing.Point(340, 157)
        Me.chkMerge.Margin = New System.Windows.Forms.Padding(4)
        Me.chkMerge.Name = "chkMerge"
        Me.chkMerge.Size = New System.Drawing.Size(120, 21)
        Me.chkMerge.TabIndex = 236
        Me.chkMerge.Text = "Merge Columns"
        Me.chkMerge.UseVisualStyleBackColor = True
        Me.chkMerge.Visible = False
        '
        'chkUser
        '
        Me.chkUser.AutoSize = True
        Me.chkUser.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkUser.Location = New System.Drawing.Point(189, 96)
        Me.chkUser.Margin = New System.Windows.Forms.Padding(4)
        Me.chkUser.Name = "chkUser"
        Me.chkUser.Size = New System.Drawing.Size(139, 21)
        Me.chkUser.TabIndex = 225
        Me.chkUser.Text = "Select Sales Person"
        Me.chkUser.UseVisualStyleBackColor = True
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Controls.Add(Me.chkItemGroup)
        Me.FlowLayoutPanel1.Controls.Add(Me.pnlDate)
        Me.FlowLayoutPanel1.Controls.Add(Me.lblTripNo)
        Me.FlowLayoutPanel1.Controls.Add(Me.cboTripNo)
        Me.FlowLayoutPanel1.Controls.Add(Me.chkUser)
        Me.FlowLayoutPanel1.Controls.Add(Me.chkCustName)
        Me.FlowLayoutPanel1.Controls.Add(Me.chkSupp)
        Me.FlowLayoutPanel1.Controls.Add(Me.chkMake)
        Me.FlowLayoutPanel1.Controls.Add(Me.txtItemCode)
        Me.FlowLayoutPanel1.Controls.Add(Me.chkPrincipals)
        Me.FlowLayoutPanel1.Controls.Add(Me.ChkRegion)
        Me.FlowLayoutPanel1.Controls.Add(Me.chkZone)
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(12, 0)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(420, 178)
        Me.FlowLayoutPanel1.TabIndex = 237
        '
        'cboTripNo
        '
        Me.cboTripNo.FormattingEnabled = True
        Me.cboTripNo.Location = New System.Drawing.Point(61, 95)
        Me.cboTripNo.Name = "cboTripNo"
        Me.cboTripNo.Size = New System.Drawing.Size(121, 21)
        Me.cboTripNo.TabIndex = 243
        '
        'txtItemCode
        '
        Me.txtItemCode.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtItemCode.Location = New System.Drawing.Point(358, 124)
        Me.txtItemCode.Name = "txtItemCode"
        Me.txtItemCode.Size = New System.Drawing.Size(23, 25)
        Me.txtItemCode.TabIndex = 242
        Me.txtItemCode.Visible = False
        '
        'chkPrincipals
        '
        Me.chkPrincipals.AutoSize = True
        Me.chkPrincipals.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPrincipals.Location = New System.Drawing.Point(4, 156)
        Me.chkPrincipals.Margin = New System.Windows.Forms.Padding(4)
        Me.chkPrincipals.Name = "chkPrincipals"
        Me.chkPrincipals.Size = New System.Drawing.Size(120, 21)
        Me.chkPrincipals.TabIndex = 233
        Me.chkPrincipals.Text = "Select Principals"
        Me.chkPrincipals.UseVisualStyleBackColor = True
        '
        'ChkRegion
        '
        Me.ChkRegion.AutoSize = True
        Me.ChkRegion.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkRegion.Location = New System.Drawing.Point(132, 156)
        Me.ChkRegion.Margin = New System.Windows.Forms.Padding(4)
        Me.ChkRegion.Name = "ChkRegion"
        Me.ChkRegion.Size = New System.Drawing.Size(106, 21)
        Me.ChkRegion.TabIndex = 232
        Me.ChkRegion.Text = "Select Region"
        Me.ChkRegion.UseVisualStyleBackColor = True
        '
        'pnPending
        '
        Me.pnPending.Controls.Add(Me.optPending)
        Me.pnPending.Controls.Add(Me.optAll)
        Me.pnPending.Location = New System.Drawing.Point(448, 4)
        Me.pnPending.Name = "pnPending"
        Me.pnPending.Size = New System.Drawing.Size(131, 25)
        Me.pnPending.TabIndex = 238
        '
        'pnStk
        '
        Me.pnStk.Controls.Add(Me.optAll1)
        Me.pnStk.Controls.Add(Me.optIS)
        Me.pnStk.Controls.Add(Me.optIC)
        Me.pnStk.Controls.Add(Me.optIZ)
        Me.pnStk.Controls.Add(Me.optItem)
        Me.pnStk.Controls.Add(Me.optICZ)
        Me.pnStk.Location = New System.Drawing.Point(450, 31)
        Me.pnStk.Name = "pnStk"
        Me.pnStk.Size = New System.Drawing.Size(395, 50)
        Me.pnStk.TabIndex = 238
        '
        'optAll1
        '
        Me.optAll1.AutoSize = True
        Me.optAll1.Checked = True
        Me.optAll1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optAll1.Location = New System.Drawing.Point(6, 3)
        Me.optAll1.Name = "optAll1"
        Me.optAll1.Size = New System.Drawing.Size(40, 21)
        Me.optAll1.TabIndex = 15
        Me.optAll1.TabStop = True
        Me.optAll1.Text = "All"
        Me.optAll1.UseVisualStyleBackColor = True
        '
        'optIS
        '
        Me.optIS.AutoSize = True
        Me.optIS.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optIS.Location = New System.Drawing.Point(241, 3)
        Me.optIS.Name = "optIS"
        Me.optIS.Size = New System.Drawing.Size(143, 21)
        Me.optIS.TabIndex = 19
        Me.optIS.Text = "Item && Sales Person"
        Me.optIS.UseVisualStyleBackColor = True
        '
        'optIC
        '
        Me.optIC.AutoSize = True
        Me.optIC.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optIC.Location = New System.Drawing.Point(110, 3)
        Me.optIC.Name = "optIC"
        Me.optIC.Size = New System.Drawing.Size(125, 21)
        Me.optIC.TabIndex = 18
        Me.optIC.Text = "Item && Customer"
        Me.optIC.UseVisualStyleBackColor = True
        '
        'optIZ
        '
        Me.optIZ.AutoSize = True
        Me.optIZ.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optIZ.Location = New System.Drawing.Point(6, 25)
        Me.optIZ.Name = "optIZ"
        Me.optIZ.Size = New System.Drawing.Size(98, 21)
        Me.optIZ.TabIndex = 17
        Me.optIZ.Text = "Item && Zone"
        Me.optIZ.UseVisualStyleBackColor = True
        '
        'optItem
        '
        Me.optItem.AutoSize = True
        Me.optItem.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optItem.Location = New System.Drawing.Point(53, 3)
        Me.optItem.Name = "optItem"
        Me.optItem.Size = New System.Drawing.Size(51, 21)
        Me.optItem.TabIndex = 16
        Me.optItem.Text = "Item"
        Me.optItem.UseVisualStyleBackColor = True
        '
        'optICZ
        '
        Me.optICZ.AutoSize = True
        Me.optICZ.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optICZ.Location = New System.Drawing.Point(110, 27)
        Me.optICZ.Name = "optICZ"
        Me.optICZ.Size = New System.Drawing.Size(165, 21)
        Me.optICZ.TabIndex = 20
        Me.optICZ.Text = "Item , Customer && Zone"
        Me.optICZ.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Image = CType(resources.GetObject("btnClose.Image"), System.Drawing.Image)
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(774, 152)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(77, 28)
        Me.btnClose.TabIndex = 223
        Me.btnClose.Text = "Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnView
        '
        Me.btnView.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnView.Image = CType(resources.GetObject("btnView.Image"), System.Drawing.Image)
        Me.btnView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnView.Location = New System.Drawing.Point(686, 153)
        Me.btnView.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(78, 27)
        Me.btnView.TabIndex = 222
        Me.btnView.Text = "View"
        Me.btnView.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnView.UseVisualStyleBackColor = False
        '
        'vsfDet
        '
        Me.vsfDet.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfDet.AllowFiltering = True
        Me.vsfDet.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfDet.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfDet.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.AutoResize = True
        Me.vsfDet.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfDet.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(12, 184)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.Rows.Fixed = 3
        Me.vsfDet.Rows.GlyphRow = 2
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.ShowCellLabels = True
        Me.vsfDet.Size = New System.Drawing.Size(839, 389)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 239
        Me.vsfDet.Tag = "View"
        '
        'vsfView
        '
        Me.vsfView.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfView.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfView.ExtendLastCol = True
        Me.vsfView.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfView.Location = New System.Drawing.Point(76, 210)
        Me.vsfView.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfView.Name = "vsfView"
        Me.vsfView.Rows.DefaultSize = 24
        Me.vsfView.Size = New System.Drawing.Size(609, 362)
        Me.vsfView.StyleInfo = resources.GetString("vsfView.StyleInfo")
        Me.vsfView.TabIndex = 240
        '
        'pnCOD
        '
        Me.pnCOD.Controls.Add(Me.optCODCancel)
        Me.pnCOD.Controls.Add(Me.OptPendCOd)
        Me.pnCOD.Controls.Add(Me.optCODRcptList)
        Me.pnCOD.Location = New System.Drawing.Point(575, 3)
        Me.pnCOD.Name = "pnCOD"
        Me.pnCOD.Size = New System.Drawing.Size(265, 25)
        Me.pnCOD.TabIndex = 241
        '
        'optCODCancel
        '
        Me.optCODCancel.AutoSize = True
        Me.optCODCancel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optCODCancel.Location = New System.Drawing.Point(180, 3)
        Me.optCODCancel.Name = "optCODCancel"
        Me.optCODCancel.Size = New System.Drawing.Size(82, 21)
        Me.optCODCancel.TabIndex = 236
        Me.optCODCancel.Text = "Cancelled"
        Me.optCODCancel.UseVisualStyleBackColor = True
        '
        'OptPendCOd
        '
        Me.OptPendCOd.AutoSize = True
        Me.OptPendCOd.Checked = True
        Me.OptPendCOd.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptPendCOd.Location = New System.Drawing.Point(9, 3)
        Me.OptPendCOd.Name = "OptPendCOd"
        Me.OptPendCOd.Size = New System.Drawing.Size(73, 21)
        Me.OptPendCOd.TabIndex = 234
        Me.OptPendCOd.TabStop = True
        Me.OptPendCOd.Text = "Pending"
        Me.OptPendCOd.UseVisualStyleBackColor = True
        '
        'optCODRcptList
        '
        Me.optCODRcptList.AutoSize = True
        Me.optCODRcptList.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optCODRcptList.Location = New System.Drawing.Point(92, 3)
        Me.optCODRcptList.Name = "optCODRcptList"
        Me.optCODRcptList.Size = New System.Drawing.Size(78, 21)
        Me.optCODRcptList.TabIndex = 235
        Me.optCODRcptList.Text = "Received"
        Me.optCODRcptList.UseVisualStyleBackColor = True
        '
        'pnlCopyShortCut
        '
        Me.pnlCopyShortCut.Controls.Add(Me.btnCopy)
        Me.pnlCopyShortCut.Controls.Add(Me.pnlbtnClose)
        Me.pnlCopyShortCut.Controls.Add(Me.Label5)
        Me.pnlCopyShortCut.Controls.Add(Me.Label1)
        Me.pnlCopyShortCut.Controls.Add(Me.Label6)
        Me.pnlCopyShortCut.Controls.Add(Me.Label7)
        Me.pnlCopyShortCut.Controls.Add(Me.RbnCopyGrid)
        Me.pnlCopyShortCut.Controls.Add(Me.RbnCopyRow)
        Me.pnlCopyShortCut.Controls.Add(Me.RbnCopyColumn)
        Me.pnlCopyShortCut.Controls.Add(Me.RbnCopyCell)
        Me.pnlCopyShortCut.Location = New System.Drawing.Point(250, 289)
        Me.pnlCopyShortCut.Name = "pnlCopyShortCut"
        Me.pnlCopyShortCut.Size = New System.Drawing.Size(329, 177)
        Me.pnlCopyShortCut.TabIndex = 300
        Me.pnlCopyShortCut.Visible = False
        '
        'btnCopy
        '
        Me.btnCopy.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCopy.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnCopy.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_ok
        Me.btnCopy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCopy.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnCopy.Location = New System.Drawing.Point(161, 135)
        Me.btnCopy.Name = "btnCopy"
        Me.btnCopy.Size = New System.Drawing.Size(70, 29)
        Me.btnCopy.TabIndex = 23
        Me.btnCopy.Text = "Copy"
        Me.btnCopy.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCopy.UseVisualStyleBackColor = False
        '
        'pnlbtnClose
        '
        Me.pnlbtnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlbtnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.pnlbtnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.pnlbtnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.pnlbtnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.pnlbtnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.pnlbtnClose.Location = New System.Drawing.Point(237, 135)
        Me.pnlbtnClose.Name = "pnlbtnClose"
        Me.pnlbtnClose.Size = New System.Drawing.Size(70, 29)
        Me.pnlbtnClose.TabIndex = 22
        Me.pnlbtnClose.Text = "Cl&ose"
        Me.pnlbtnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.pnlbtnClose.UseVisualStyleBackColor = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(178, 109)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(88, 16)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "Ctrl + Shift + G"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(178, 79)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(77, 16)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Ctrl + Alt + C"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(178, 48)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(87, 16)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "Ctrl + Shift + C"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(178, 19)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(49, 16)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "Ctrl + C"
        '
        'RbnCopyGrid
        '
        Me.RbnCopyGrid.AutoSize = True
        Me.RbnCopyGrid.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RbnCopyGrid.Location = New System.Drawing.Point(16, 109)
        Me.RbnCopyGrid.Name = "RbnCopyGrid"
        Me.RbnCopyGrid.Size = New System.Drawing.Size(129, 20)
        Me.RbnCopyGrid.TabIndex = 3
        Me.RbnCopyGrid.TabStop = True
        Me.RbnCopyGrid.Text = "Copy Entire Grid  "
        Me.RbnCopyGrid.UseVisualStyleBackColor = True
        '
        'RbnCopyRow
        '
        Me.RbnCopyRow.AutoSize = True
        Me.RbnCopyRow.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RbnCopyRow.Location = New System.Drawing.Point(16, 77)
        Me.RbnCopyRow.Name = "RbnCopyRow"
        Me.RbnCopyRow.Size = New System.Drawing.Size(128, 20)
        Me.RbnCopyRow.TabIndex = 2
        Me.RbnCopyRow.TabStop = True
        Me.RbnCopyRow.Text = "Copy Entire  Row"
        Me.RbnCopyRow.UseVisualStyleBackColor = True
        '
        'RbnCopyColumn
        '
        Me.RbnCopyColumn.AutoSize = True
        Me.RbnCopyColumn.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RbnCopyColumn.Location = New System.Drawing.Point(15, 46)
        Me.RbnCopyColumn.Name = "RbnCopyColumn"
        Me.RbnCopyColumn.Size = New System.Drawing.Size(143, 20)
        Me.RbnCopyColumn.TabIndex = 1
        Me.RbnCopyColumn.TabStop = True
        Me.RbnCopyColumn.Text = "Copy Entire Column"
        Me.RbnCopyColumn.UseVisualStyleBackColor = True
        '
        'RbnCopyCell
        '
        Me.RbnCopyCell.AutoSize = True
        Me.RbnCopyCell.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RbnCopyCell.Location = New System.Drawing.Point(15, 19)
        Me.RbnCopyCell.Name = "RbnCopyCell"
        Me.RbnCopyCell.Size = New System.Drawing.Size(129, 20)
        Me.RbnCopyCell.TabIndex = 0
        Me.RbnCopyCell.TabStop = True
        Me.RbnCopyCell.Text = "Copy Data In Cell"
        Me.RbnCopyCell.UseVisualStyleBackColor = True
        '
        'frmRptDailySales
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.ClientSize = New System.Drawing.Size(857, 604)
        Me.Controls.Add(Me.pnlCopyShortCut)
        Me.Controls.Add(Me.pnCOD)
        Me.Controls.Add(Me.vsfView)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.pnStk)
        Me.Controls.Add(Me.pnPending)
        Me.Controls.Add(Me.FlowLayoutPanel1)
        Me.Controls.Add(Me.chkMerge)
        Me.Controls.Add(Me.btnExcel)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnView)
        Me.KeyPreview = True
        Me.Name = "frmRptDailySales"
        Me.ShowIcon = False
        Me.Text = "Customer Order Report"
        Me.pnlDate.ResumeLayout(False)
        Me.pnlDate.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.PerformLayout()
        Me.pnPending.ResumeLayout(False)
        Me.pnPending.PerformLayout()
        Me.pnStk.ResumeLayout(False)
        Me.pnStk.PerformLayout()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnCOD.ResumeLayout(False)
        Me.pnCOD.PerformLayout()
        Me.pnlCopyShortCut.ResumeLayout(False)
        Me.pnlCopyShortCut.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents chkCustName As System.Windows.Forms.CheckBox
    Friend WithEvents chkZone As System.Windows.Forms.CheckBox
    Friend WithEvents chkItemGroup As System.Windows.Forms.CheckBox
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnView As System.Windows.Forms.Button
    Friend WithEvents lblTripNo As System.Windows.Forms.Label
    Friend WithEvents pnlDate As System.Windows.Forms.Panel
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents dtpToDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpFromDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents chkDate As System.Windows.Forms.CheckBox
    Friend WithEvents chkSupp As System.Windows.Forms.CheckBox
    Friend WithEvents chkMake As System.Windows.Forms.CheckBox
    Friend WithEvents btnExcel As System.Windows.Forms.Button
    Friend WithEvents optPending As System.Windows.Forms.RadioButton
    Friend WithEvents optAll As System.Windows.Forms.RadioButton
    Friend WithEvents chkMerge As System.Windows.Forms.CheckBox
    Friend WithEvents chkUser As System.Windows.Forms.CheckBox
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents pnPending As System.Windows.Forms.Panel
    Friend WithEvents pnStk As System.Windows.Forms.Panel
    Friend WithEvents optAll1 As System.Windows.Forms.RadioButton
    Friend WithEvents optIS As System.Windows.Forms.RadioButton
    Friend WithEvents optIC As System.Windows.Forms.RadioButton
    Friend WithEvents optIZ As System.Windows.Forms.RadioButton
    Friend WithEvents optItem As System.Windows.Forms.RadioButton
    Friend WithEvents optICZ As System.Windows.Forms.RadioButton
    Friend WithEvents ChkRegion As System.Windows.Forms.CheckBox
    Friend WithEvents chkPrincipals As System.Windows.Forms.CheckBox
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents vsfView As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents pnCOD As System.Windows.Forms.Panel
    Friend WithEvents optCODCancel As System.Windows.Forms.RadioButton
    Friend WithEvents OptPendCOd As System.Windows.Forms.RadioButton
    Friend WithEvents optCODRcptList As System.Windows.Forms.RadioButton
    Friend WithEvents cboTripNo As ComboBox
    Friend WithEvents txtItemCode As TextBox
    Friend WithEvents pnlCopyShortCut As Panel
    Friend WithEvents btnCopy As Button
    Friend WithEvents pnlbtnClose As Button
    Friend WithEvents Label5 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents RbnCopyGrid As RadioButton
    Friend WithEvents RbnCopyRow As RadioButton
    Friend WithEvents RbnCopyColumn As RadioButton
    Friend WithEvents RbnCopyCell As RadioButton
End Class
