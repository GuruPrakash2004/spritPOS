<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmUtlTrnSettings
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
        Me.ChkShowGraceQty = New System.Windows.Forms.CheckBox()
        Me.ChkAllowTenderChange = New System.Windows.Forms.CheckBox()
        Me.ChkFreezeDoc = New System.Windows.Forms.CheckBox()
        Me.ChkAllowDuplicateItems = New System.Windows.Forms.CheckBox()
        Me.ChkAuthorizationReq = New System.Windows.Forms.CheckBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.ChkAllowEditDocDate = New System.Windows.Forms.CheckBox()
        Me.ChkAllowImportExport = New System.Windows.Forms.CheckBox()
        Me.chkLoadfrmBOM = New System.Windows.Forms.CheckBox()
        Me.ChkAmmendmentReq = New System.Windows.Forms.CheckBox()
        Me.ChkBlnEnvPrint = New System.Windows.Forms.CheckBox()
        Me.ChkOfflineCalc = New System.Windows.Forms.CheckBox()
        Me.ChkAllowCopyFrm = New System.Windows.Forms.CheckBox()
        Me.ChkFileAttachReq = New System.Windows.Forms.CheckBox()
        Me.ChkMultiDocGen = New System.Windows.Forms.CheckBox()
        Me.CboAddReduce = New System.Windows.Forms.ComboBox()
        Me.chkAffectsStock = New System.Windows.Forms.CheckBox()
        Me.grpLinkedPending = New System.Windows.Forms.GroupBox()
        Me.ChkLinkedPendSingleDoc = New System.Windows.Forms.CheckBox()
        Me.ChkLinkedPendDocNo = New System.Windows.Forms.CheckBox()
        Me.ChkLinkedPendParty = New System.Windows.Forms.CheckBox()
        Me.GrpPendQuryFlds = New System.Windows.Forms.GroupBox()
        Me.txtPickFldsFrmDocQry = New System.Windows.Forms.TextBox()
        Me.LblPickFldsFrmSrcDocQry = New System.Windows.Forms.Label()
        Me.txtPickFldsFrmSrc = New System.Windows.Forms.TextBox()
        Me.LblPicFldsFrmSrc = New System.Windows.Forms.Label()
        Me.txtQuryCdtn = New System.Windows.Forms.TextBox()
        Me.LblQuryTbls = New System.Windows.Forms.Label()
        Me.txtQuryTbls = New System.Windows.Forms.TextBox()
        Me.LblQuryCdtn = New System.Windows.Forms.Label()
        Me.TxtQuryFlds = New System.Windows.Forms.TextBox()
        Me.LblQryFlds = New System.Windows.Forms.Label()
        Me.GrpAuth = New System.Windows.Forms.GroupBox()
        Me.txtNoOfAuthQueryCond = New System.Windows.Forms.TextBox()
        Me.lblNoOfAuth = New System.Windows.Forms.Label()
        Me.txtNoOfAuth = New System.Windows.Forms.TextBox()
        Me.GrpParty = New System.Windows.Forms.GroupBox()
        Me.TxtRatePartyItem = New System.Windows.Forms.TextBox()
        Me.LblRatePartyItem = New System.Windows.Forms.Label()
        Me.ChkEnforcePartyItemLink = New System.Windows.Forms.CheckBox()
        Me.ChkShowShipAdd = New System.Windows.Forms.CheckBox()
        Me.TxtPartyFilterCond = New System.Windows.Forms.TextBox()
        Me.lblPartyFilterCond = New System.Windows.Forms.Label()
        Me.chkSavePartyData = New System.Windows.Forms.CheckBox()
        Me.ChkManualPartySel = New System.Windows.Forms.CheckBox()
        Me.GrpFreezeDoc = New System.Windows.Forms.GroupBox()
        Me.lblAllowEditDlt = New System.Windows.Forms.Label()
        Me.txtAllowEditDel = New System.Windows.Forms.TextBox()
        Me.lblDays = New System.Windows.Forms.Label()
        Me.txtItemFilterCondition = New System.Windows.Forms.TextBox()
        Me.LblItemFilterCond = New System.Windows.Forms.Label()
        Me.GrpDetSec = New System.Windows.Forms.GroupBox()
        Me.TxtRateMstItem = New System.Windows.Forms.TextBox()
        Me.lblRateMstItem = New System.Windows.Forms.Label()
        Me.TCTrnsSettings = New System.Windows.Forms.TabControl()
        Me.TPMain = New System.Windows.Forms.TabPage()
        Me.TPDetail1 = New System.Windows.Forms.TabPage()
        Me.GrpAccountingSettings = New System.Windows.Forms.GroupBox()
        Me.TxtAcVoucherValFld = New System.Windows.Forms.TextBox()
        Me.LblAcVoucherValFld = New System.Windows.Forms.Label()
        Me.TxtAcLedgerFldName = New System.Windows.Forms.TextBox()
        Me.LblAcctLedgerFldName = New System.Windows.Forms.Label()
        Me.CboAcctMstType = New System.Windows.Forms.ComboBox()
        Me.LblType = New System.Windows.Forms.Label()
        Me.ChkAcctTrn = New System.Windows.Forms.CheckBox()
        Me.LblTenderChange = New System.Windows.Forms.Label()
        Me.TxtTenderChangeFld = New System.Windows.Forms.TextBox()
        Me.TPDetail2 = New System.Windows.Forms.TabPage()
        Me.TPDetail3 = New System.Windows.Forms.TabPage()
        Me.TPDetail4 = New System.Windows.Forms.TabPage()
        Me.TPVouchers = New System.Windows.Forms.TabPage()
        Me.CboDefFirstRowType = New System.Windows.Forms.ComboBox()
        Me.LblDefRow = New System.Windows.Forms.Label()
        Me.ChkChqPrinting = New System.Windows.Forms.CheckBox()
        Me.ChkBillWiseFeature = New System.Windows.Forms.CheckBox()
        Me.ChkBalColReq = New System.Windows.Forms.CheckBox()
        Me.GrpMisc = New System.Windows.Forms.GroupBox()
        Me.chkShowMenuIcons = New System.Windows.Forms.CheckBox()
        Me.BtnSaveMisc = New C1.Win.C1Input.C1Button()
        Me.BtnCancelMisc = New System.Windows.Forms.Button()
        Me.txtSearchTopN = New System.Windows.Forms.TextBox()
        Me.txtItemDescCaption = New System.Windows.Forms.TextBox()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.txtItemNameCaption = New System.Windows.Forms.TextBox()
        Me.txtItemCaption = New System.Windows.Forms.TextBox()
        Me.lblFldSearchTopN = New System.Windows.Forms.Label()
        Me.lblItemDescCaption = New System.Windows.Forms.Label()
        Me.lblItemNameCaption = New System.Windows.Forms.Label()
        Me.LblItemCaption = New System.Windows.Forms.Label()
        Me.GrpHelp = New System.Windows.Forms.GroupBox()
        Me.txtTitle = New System.Windows.Forms.TextBox()
        Me.txtContent = New System.Windows.Forms.TextBox()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.LblTrn = New System.Windows.Forms.Label()
        Me.CboTrnDocType = New System.Windows.Forms.ComboBox()
        Me.btnMisc = New System.Windows.Forms.Button()
        Me.btnSave = New C1.Win.C1Input.C1Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.chkPdfPrintP = New System.Windows.Forms.CheckBox()
        Me.Panel1.SuspendLayout()
        Me.grpLinkedPending.SuspendLayout()
        Me.GrpPendQuryFlds.SuspendLayout()
        Me.GrpAuth.SuspendLayout()
        Me.GrpParty.SuspendLayout()
        Me.GrpFreezeDoc.SuspendLayout()
        Me.GrpDetSec.SuspendLayout()
        Me.TCTrnsSettings.SuspendLayout()
        Me.TPMain.SuspendLayout()
        Me.TPDetail1.SuspendLayout()
        Me.GrpAccountingSettings.SuspendLayout()
        Me.TPDetail2.SuspendLayout()
        Me.TPDetail3.SuspendLayout()
        Me.TPDetail4.SuspendLayout()
        Me.TPVouchers.SuspendLayout()
        Me.GrpMisc.SuspendLayout()
        Me.GrpHelp.SuspendLayout()
        Me.SuspendLayout()
        '
        'ChkShowGraceQty
        '
        Me.ChkShowGraceQty.AutoSize = True
        Me.ChkShowGraceQty.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkShowGraceQty.Location = New System.Drawing.Point(6, 87)
        Me.ChkShowGraceQty.Name = "ChkShowGraceQty"
        Me.ChkShowGraceQty.Size = New System.Drawing.Size(179, 24)
        Me.ChkShowGraceQty.TabIndex = 24
        Me.ChkShowGraceQty.Tag = "Show Grace Quantity▄Grace quantity "
        Me.ChkShowGraceQty.Text = "Show Grace Quantity"
        Me.ChkShowGraceQty.UseVisualStyleBackColor = True
        '
        'ChkAllowTenderChange
        '
        Me.ChkAllowTenderChange.AutoSize = True
        Me.ChkAllowTenderChange.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkAllowTenderChange.Location = New System.Drawing.Point(18, 186)
        Me.ChkAllowTenderChange.Name = "ChkAllowTenderChange"
        Me.ChkAllowTenderChange.Size = New System.Drawing.Size(179, 24)
        Me.ChkAllowTenderChange.TabIndex = 21
        Me.ChkAllowTenderChange.Tag = "Allow Tender Change▄This option allows to provide exact change"
        Me.ChkAllowTenderChange.Text = "Allow Tender Change"
        Me.ChkAllowTenderChange.UseVisualStyleBackColor = True
        '
        'ChkFreezeDoc
        '
        Me.ChkFreezeDoc.AutoSize = True
        Me.ChkFreezeDoc.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkFreezeDoc.Location = New System.Drawing.Point(11, 19)
        Me.ChkFreezeDoc.Name = "ChkFreezeDoc"
        Me.ChkFreezeDoc.Size = New System.Drawing.Size(156, 24)
        Me.ChkFreezeDoc.TabIndex = 20
        Me.ChkFreezeDoc.Tag = "Freeze Document▄It is used to freeze some document"
        Me.ChkFreezeDoc.Text = "Freeze Document"
        Me.ChkFreezeDoc.UseVisualStyleBackColor = True
        '
        'ChkAllowDuplicateItems
        '
        Me.ChkAllowDuplicateItems.AutoSize = True
        Me.ChkAllowDuplicateItems.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkAllowDuplicateItems.Location = New System.Drawing.Point(6, 22)
        Me.ChkAllowDuplicateItems.Name = "ChkAllowDuplicateItems"
        Me.ChkAllowDuplicateItems.Size = New System.Drawing.Size(180, 24)
        Me.ChkAllowDuplicateItems.TabIndex = 28
        Me.ChkAllowDuplicateItems.Tag = "Allow Duplicate Items▄It Allows duplicate Items"
        Me.ChkAllowDuplicateItems.Text = "Allow Duplicate Items"
        Me.ChkAllowDuplicateItems.UseVisualStyleBackColor = True
        '
        'ChkAuthorizationReq
        '
        Me.ChkAuthorizationReq.AutoSize = True
        Me.ChkAuthorizationReq.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkAuthorizationReq.Location = New System.Drawing.Point(0, 25)
        Me.ChkAuthorizationReq.Name = "ChkAuthorizationReq"
        Me.ChkAuthorizationReq.Size = New System.Drawing.Size(191, 24)
        Me.ChkAuthorizationReq.TabIndex = 17
        Me.ChkAuthorizationReq.Tag = "Authorization Required ▄Verify the Document is Authorize or Non Authorized"
        Me.ChkAuthorizationReq.Text = "Authorization Required"
        Me.ChkAuthorizationReq.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.chkPdfPrintP)
        Me.Panel1.Controls.Add(Me.ChkAllowEditDocDate)
        Me.Panel1.Controls.Add(Me.ChkAllowImportExport)
        Me.Panel1.Controls.Add(Me.chkLoadfrmBOM)
        Me.Panel1.Controls.Add(Me.ChkAmmendmentReq)
        Me.Panel1.Controls.Add(Me.ChkBlnEnvPrint)
        Me.Panel1.Controls.Add(Me.ChkOfflineCalc)
        Me.Panel1.Controls.Add(Me.ChkAllowCopyFrm)
        Me.Panel1.Controls.Add(Me.ChkFileAttachReq)
        Me.Panel1.Controls.Add(Me.ChkMultiDocGen)
        Me.Panel1.Controls.Add(Me.CboAddReduce)
        Me.Panel1.Controls.Add(Me.chkAffectsStock)
        Me.Panel1.Controls.Add(Me.grpLinkedPending)
        Me.Panel1.Location = New System.Drawing.Point(21, 6)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(902, 296)
        Me.Panel1.TabIndex = 0
        '
        'ChkAllowEditDocDate
        '
        Me.ChkAllowEditDocDate.AutoSize = True
        Me.ChkAllowEditDocDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkAllowEditDocDate.Location = New System.Drawing.Point(265, 15)
        Me.ChkAllowEditDocDate.Name = "ChkAllowEditDocDate"
        Me.ChkAllowEditDocDate.Size = New System.Drawing.Size(169, 24)
        Me.ChkAllowEditDocDate.TabIndex = 228
        Me.ChkAllowEditDocDate.Tag = "Allow Edit Doc Date▄Allow Doument Date Editing"
        Me.ChkAllowEditDocDate.Text = "Allow Edit Doc Date"
        Me.ChkAllowEditDocDate.UseVisualStyleBackColor = True
        '
        'ChkAllowImportExport
        '
        Me.ChkAllowImportExport.AutoSize = True
        Me.ChkAllowImportExport.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkAllowImportExport.Location = New System.Drawing.Point(15, 262)
        Me.ChkAllowImportExport.Name = "ChkAllowImportExport"
        Me.ChkAllowImportExport.Size = New System.Drawing.Size(165, 24)
        Me.ChkAllowImportExport.TabIndex = 8
        Me.ChkAllowImportExport.Tag = "AllowImportExport ▄ This option allows the Import Export of detail grid in transa" &
    "ction"
        Me.ChkAllowImportExport.Text = "Allow Import Export"
        Me.ChkAllowImportExport.UseVisualStyleBackColor = True
        '
        'chkLoadfrmBOM
        '
        Me.chkLoadfrmBOM.AutoSize = True
        Me.chkLoadfrmBOM.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkLoadfrmBOM.Location = New System.Drawing.Point(15, 49)
        Me.chkLoadfrmBOM.Name = "chkLoadfrmBOM"
        Me.chkLoadfrmBOM.Size = New System.Drawing.Size(145, 24)
        Me.chkLoadfrmBOM.TabIndex = 1
        Me.chkLoadfrmBOM.Tag = "Load From BOM ▄User can select the parent item and according to that the related " &
    "child items are displayed"
        Me.chkLoadfrmBOM.Text = "Load From BOM"
        Me.chkLoadfrmBOM.UseVisualStyleBackColor = True
        '
        'ChkAmmendmentReq
        '
        Me.ChkAmmendmentReq.AutoSize = True
        Me.ChkAmmendmentReq.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkAmmendmentReq.Location = New System.Drawing.Point(15, 112)
        Me.ChkAmmendmentReq.Name = "ChkAmmendmentReq"
        Me.ChkAmmendmentReq.Size = New System.Drawing.Size(197, 24)
        Me.ChkAmmendmentReq.TabIndex = 3
        Me.ChkAmmendmentReq.Tag = "Ammendment Required▄It gives permission that a minor change or addition designed " &
    "to improve a text"
        Me.ChkAmmendmentReq.Text = "Ammendment Required"
        Me.ChkAmmendmentReq.UseVisualStyleBackColor = True
        '
        'ChkBlnEnvPrint
        '
        Me.ChkBlnEnvPrint.AutoSize = True
        Me.ChkBlnEnvPrint.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkBlnEnvPrint.Location = New System.Drawing.Point(15, 142)
        Me.ChkBlnEnvPrint.Name = "ChkBlnEnvPrint"
        Me.ChkBlnEnvPrint.Size = New System.Drawing.Size(130, 24)
        Me.ChkBlnEnvPrint.TabIndex = 4
        Me.ChkBlnEnvPrint.Tag = "Envelope Print▄It is a customized crystal report"
        Me.ChkBlnEnvPrint.Text = "Envelope Print"
        Me.ChkBlnEnvPrint.UseVisualStyleBackColor = True
        '
        'ChkOfflineCalc
        '
        Me.ChkOfflineCalc.AutoSize = True
        Me.ChkOfflineCalc.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkOfflineCalc.Location = New System.Drawing.Point(15, 232)
        Me.ChkOfflineCalc.Name = "ChkOfflineCalc"
        Me.ChkOfflineCalc.Size = New System.Drawing.Size(156, 24)
        Me.ChkOfflineCalc.TabIndex = 7
        Me.ChkOfflineCalc.Tag = "offline Calculation ▄ This calculation will occur at the time of saving"
        Me.ChkOfflineCalc.Text = "Offline Calculation"
        Me.ChkOfflineCalc.UseVisualStyleBackColor = True
        '
        'ChkAllowCopyFrm
        '
        Me.ChkAllowCopyFrm.AutoSize = True
        Me.ChkAllowCopyFrm.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkAllowCopyFrm.Location = New System.Drawing.Point(14, 79)
        Me.ChkAllowCopyFrm.Name = "ChkAllowCopyFrm"
        Me.ChkAllowCopyFrm.Size = New System.Drawing.Size(146, 24)
        Me.ChkAllowCopyFrm.TabIndex = 2
        Me.ChkAllowCopyFrm.Tag = "Allow Copy From ▄It will allow to copy the previous saved data of same transactio" &
    "n"
        Me.ChkAllowCopyFrm.Text = "Allow Copy From"
        Me.ChkAllowCopyFrm.UseVisualStyleBackColor = True
        '
        'ChkFileAttachReq
        '
        Me.ChkFileAttachReq.AutoSize = True
        Me.ChkFileAttachReq.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkFileAttachReq.Location = New System.Drawing.Point(14, 202)
        Me.ChkFileAttachReq.Name = "ChkFileAttachReq"
        Me.ChkFileAttachReq.Size = New System.Drawing.Size(209, 24)
        Me.ChkFileAttachReq.TabIndex = 6
        Me.ChkFileAttachReq.Tag = "File Attachment Required▄Using this option we can select n number of files agains" &
    "t a particular transaction"
        Me.ChkFileAttachReq.Text = "File Attachment Required"
        Me.ChkFileAttachReq.UseVisualStyleBackColor = True
        '
        'ChkMultiDocGen
        '
        Me.ChkMultiDocGen.AutoSize = True
        Me.ChkMultiDocGen.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkMultiDocGen.Location = New System.Drawing.Point(15, 172)
        Me.ChkMultiDocGen.Name = "ChkMultiDocGen"
        Me.ChkMultiDocGen.Size = New System.Drawing.Size(178, 24)
        Me.ChkMultiDocGen.TabIndex = 5
        Me.ChkMultiDocGen.Tag = "Multi Doc Generation▄It is used for generating n number of documents"
        Me.ChkMultiDocGen.Text = "Multi Doc Generation"
        Me.ChkMultiDocGen.UseVisualStyleBackColor = True
        '
        'CboAddReduce
        '
        Me.CboAddReduce.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboAddReduce.FormattingEnabled = True
        Me.CboAddReduce.Items.AddRange(New Object() {"Add", "Reduce"})
        Me.CboAddReduce.Location = New System.Drawing.Point(158, 15)
        Me.CboAddReduce.Name = "CboAddReduce"
        Me.CboAddReduce.Size = New System.Drawing.Size(81, 28)
        Me.CboAddReduce.TabIndex = 9
        '
        'chkAffectsStock
        '
        Me.chkAffectsStock.AutoSize = True
        Me.chkAffectsStock.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAffectsStock.Location = New System.Drawing.Point(15, 17)
        Me.chkAffectsStock.Name = "chkAffectsStock"
        Me.chkAffectsStock.Size = New System.Drawing.Size(124, 24)
        Me.chkAffectsStock.TabIndex = 0
        Me.chkAffectsStock.Tag = "Affects Stock▄This option is used for stock activity, we can add or reduce stock " &
    "by this"
        Me.chkAffectsStock.Text = "Affects Stock"
        Me.chkAffectsStock.UseVisualStyleBackColor = True
        '
        'grpLinkedPending
        '
        Me.grpLinkedPending.Controls.Add(Me.ChkLinkedPendSingleDoc)
        Me.grpLinkedPending.Controls.Add(Me.ChkLinkedPendDocNo)
        Me.grpLinkedPending.Controls.Add(Me.ChkLinkedPendParty)
        Me.grpLinkedPending.Controls.Add(Me.ChkShowGraceQty)
        Me.grpLinkedPending.Location = New System.Drawing.Point(265, 72)
        Me.grpLinkedPending.Name = "grpLinkedPending"
        Me.grpLinkedPending.Size = New System.Drawing.Size(399, 124)
        Me.grpLinkedPending.TabIndex = 227
        Me.grpLinkedPending.TabStop = False
        '
        'ChkLinkedPendSingleDoc
        '
        Me.ChkLinkedPendSingleDoc.AutoSize = True
        Me.ChkLinkedPendSingleDoc.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkLinkedPendSingleDoc.Location = New System.Drawing.Point(6, 40)
        Me.ChkLinkedPendSingleDoc.Name = "ChkLinkedPendSingleDoc"
        Me.ChkLinkedPendSingleDoc.Size = New System.Drawing.Size(218, 24)
        Me.ChkLinkedPendSingleDoc.TabIndex = 4
        Me.ChkLinkedPendSingleDoc.Tag = "Linked Pending Single Doc▄If this option is selected only one document is allowed" &
    " to load at a time for particular transaction"
        Me.ChkLinkedPendSingleDoc.Text = "Linked Pending Single Doc"
        Me.ChkLinkedPendSingleDoc.UseVisualStyleBackColor = True
        '
        'ChkLinkedPendDocNo
        '
        Me.ChkLinkedPendDocNo.AutoSize = True
        Me.ChkLinkedPendDocNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkLinkedPendDocNo.Location = New System.Drawing.Point(6, 63)
        Me.ChkLinkedPendDocNo.Name = "ChkLinkedPendDocNo"
        Me.ChkLinkedPendDocNo.Size = New System.Drawing.Size(194, 24)
        Me.ChkLinkedPendDocNo.TabIndex = 3
        Me.ChkLinkedPendDocNo.Tag = "Linked Pending Doc No▄It this option is selected it will show all pending docs fo" &
    "r selected transaction"
        Me.ChkLinkedPendDocNo.Text = "Linked Pending Doc No"
        Me.ChkLinkedPendDocNo.UseVisualStyleBackColor = True
        '
        'ChkLinkedPendParty
        '
        Me.ChkLinkedPendParty.AutoSize = True
        Me.ChkLinkedPendParty.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkLinkedPendParty.Location = New System.Drawing.Point(6, 19)
        Me.ChkLinkedPendParty.Name = "ChkLinkedPendParty"
        Me.ChkLinkedPendParty.Size = New System.Drawing.Size(177, 24)
        Me.ChkLinkedPendParty.TabIndex = 2
        Me.ChkLinkedPendParty.Tag = "LinkedPendingParty▄This option shows the pending document list based on party sel" &
    "ection"
        Me.ChkLinkedPendParty.Text = "Linked Pending Party"
        Me.ChkLinkedPendParty.UseVisualStyleBackColor = True
        '
        'GrpPendQuryFlds
        '
        Me.GrpPendQuryFlds.Controls.Add(Me.txtPickFldsFrmDocQry)
        Me.GrpPendQuryFlds.Controls.Add(Me.LblPickFldsFrmSrcDocQry)
        Me.GrpPendQuryFlds.Controls.Add(Me.txtPickFldsFrmSrc)
        Me.GrpPendQuryFlds.Controls.Add(Me.LblPicFldsFrmSrc)
        Me.GrpPendQuryFlds.Controls.Add(Me.txtQuryCdtn)
        Me.GrpPendQuryFlds.Controls.Add(Me.LblQuryTbls)
        Me.GrpPendQuryFlds.Controls.Add(Me.txtQuryTbls)
        Me.GrpPendQuryFlds.Controls.Add(Me.LblQuryCdtn)
        Me.GrpPendQuryFlds.Controls.Add(Me.TxtQuryFlds)
        Me.GrpPendQuryFlds.Controls.Add(Me.LblQryFlds)
        Me.GrpPendQuryFlds.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GrpPendQuryFlds.Location = New System.Drawing.Point(14, 14)
        Me.GrpPendQuryFlds.Name = "GrpPendQuryFlds"
        Me.GrpPendQuryFlds.Size = New System.Drawing.Size(917, 294)
        Me.GrpPendQuryFlds.TabIndex = 229
        Me.GrpPendQuryFlds.TabStop = False
        Me.GrpPendQuryFlds.Text = "Pending Query"
        '
        'txtPickFldsFrmDocQry
        '
        Me.txtPickFldsFrmDocQry.Location = New System.Drawing.Point(160, 200)
        Me.txtPickFldsFrmDocQry.MaxLength = 500
        Me.txtPickFldsFrmDocQry.Multiline = True
        Me.txtPickFldsFrmDocQry.Name = "txtPickFldsFrmDocQry"
        Me.txtPickFldsFrmDocQry.Size = New System.Drawing.Size(738, 74)
        Me.txtPickFldsFrmDocQry.TabIndex = 9
        Me.txtPickFldsFrmDocQry.Tag = ""
        '
        'LblPickFldsFrmSrcDocQry
        '
        Me.LblPickFldsFrmSrcDocQry.AutoSize = True
        Me.LblPickFldsFrmSrcDocQry.Location = New System.Drawing.Point(8, 203)
        Me.LblPickFldsFrmSrcDocQry.Name = "LblPickFldsFrmSrcDocQry"
        Me.LblPickFldsFrmSrcDocQry.Size = New System.Drawing.Size(104, 40)
        Me.LblPickFldsFrmSrcDocQry.TabIndex = 8
        Me.LblPickFldsFrmSrcDocQry.Tag = "Pick Fields from Doc Query▄It Retrives values from pending list query"
        Me.LblPickFldsFrmSrcDocQry.Text = "Pick Flds Frm" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " Doc Qry"
        '
        'txtPickFldsFrmSrc
        '
        Me.txtPickFldsFrmSrc.Location = New System.Drawing.Point(160, 280)
        Me.txtPickFldsFrmSrc.MaxLength = 250
        Me.txtPickFldsFrmSrc.Multiline = True
        Me.txtPickFldsFrmSrc.Name = "txtPickFldsFrmSrc"
        Me.txtPickFldsFrmSrc.Size = New System.Drawing.Size(738, 10)
        Me.txtPickFldsFrmSrc.TabIndex = 7
        Me.txtPickFldsFrmSrc.Visible = False
        '
        'LblPicFldsFrmSrc
        '
        Me.LblPicFldsFrmSrc.AutoSize = True
        Me.LblPicFldsFrmSrc.Location = New System.Drawing.Point(6, 271)
        Me.LblPicFldsFrmSrc.Name = "LblPicFldsFrmSrc"
        Me.LblPicFldsFrmSrc.Size = New System.Drawing.Size(132, 20)
        Me.LblPicFldsFrmSrc.TabIndex = 6
        Me.LblPicFldsFrmSrc.Tag = "Pick Flds Frm Src▄icks the field from source"
        Me.LblPicFldsFrmSrc.Text = "Pick Flds Frm Src"
        Me.LblPicFldsFrmSrc.Visible = False
        '
        'txtQuryCdtn
        '
        Me.txtQuryCdtn.Location = New System.Drawing.Point(162, 113)
        Me.txtQuryCdtn.MaxLength = 250
        Me.txtQuryCdtn.Multiline = True
        Me.txtQuryCdtn.Name = "txtQuryCdtn"
        Me.txtQuryCdtn.Size = New System.Drawing.Size(738, 81)
        Me.txtQuryCdtn.TabIndex = 5
        Me.txtQuryCdtn.Tag = ""
        '
        'LblQuryTbls
        '
        Me.LblQuryTbls.AutoSize = True
        Me.LblQuryTbls.Location = New System.Drawing.Point(8, 76)
        Me.LblQuryTbls.Name = "LblQuryTbls"
        Me.LblQuryTbls.Size = New System.Drawing.Size(102, 20)
        Me.LblQuryTbls.TabIndex = 4
        Me.LblQuryTbls.Tag = "Pending Query Tables▄T Adds tables to be shown for query fields"
        Me.LblQuryTbls.Text = "Query Tables"
        '
        'txtQuryTbls
        '
        Me.txtQuryTbls.Location = New System.Drawing.Point(162, 69)
        Me.txtQuryTbls.MaxLength = 250
        Me.txtQuryTbls.Multiline = True
        Me.txtQuryTbls.Name = "txtQuryTbls"
        Me.txtQuryTbls.Size = New System.Drawing.Size(738, 35)
        Me.txtQuryTbls.TabIndex = 3
        Me.txtQuryTbls.Tag = ""
        '
        'LblQuryCdtn
        '
        Me.LblQuryCdtn.AutoSize = True
        Me.LblQuryCdtn.Location = New System.Drawing.Point(8, 116)
        Me.LblQuryCdtn.Name = "LblQuryCdtn"
        Me.LblQuryCdtn.Size = New System.Drawing.Size(122, 20)
        Me.LblQuryCdtn.TabIndex = 2
        Me.LblQuryCdtn.Tag = "Pending Query Condition▄Selects the pending query condition from pending query ta" &
    "bles for adding fields"
        Me.LblQuryCdtn.Text = "Query Condition"
        '
        'TxtQuryFlds
        '
        Me.TxtQuryFlds.Location = New System.Drawing.Point(162, 30)
        Me.TxtQuryFlds.MaxLength = 250
        Me.TxtQuryFlds.Multiline = True
        Me.TxtQuryFlds.Name = "TxtQuryFlds"
        Me.TxtQuryFlds.Size = New System.Drawing.Size(738, 35)
        Me.TxtQuryFlds.TabIndex = 1
        Me.TxtQuryFlds.Tag = ""
        '
        'LblQryFlds
        '
        Me.LblQryFlds.AutoSize = True
        Me.LblQryFlds.Location = New System.Drawing.Point(8, 35)
        Me.LblQryFlds.Name = "LblQryFlds"
        Me.LblQryFlds.Size = New System.Drawing.Size(97, 20)
        Me.LblQryFlds.TabIndex = 0
        Me.LblQryFlds.Tag = "Pending Query Fields▄Adds Fields to be shown in query list"
        Me.LblQryFlds.Text = "Query Fields"
        '
        'GrpAuth
        '
        Me.GrpAuth.Controls.Add(Me.txtNoOfAuthQueryCond)
        Me.GrpAuth.Controls.Add(Me.ChkAuthorizationReq)
        Me.GrpAuth.Controls.Add(Me.lblNoOfAuth)
        Me.GrpAuth.Controls.Add(Me.txtNoOfAuth)
        Me.GrpAuth.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GrpAuth.Location = New System.Drawing.Point(18, 11)
        Me.GrpAuth.Name = "GrpAuth"
        Me.GrpAuth.Size = New System.Drawing.Size(426, 141)
        Me.GrpAuth.TabIndex = 229
        Me.GrpAuth.TabStop = False
        Me.GrpAuth.Text = "Authorization"
        '
        'txtNoOfAuthQueryCond
        '
        Me.txtNoOfAuthQueryCond.Location = New System.Drawing.Point(6, 99)
        Me.txtNoOfAuthQueryCond.MaxLength = 500
        Me.txtNoOfAuthQueryCond.Multiline = True
        Me.txtNoOfAuthQueryCond.Name = "txtNoOfAuthQueryCond"
        Me.txtNoOfAuthQueryCond.Size = New System.Drawing.Size(411, 36)
        Me.txtNoOfAuthQueryCond.TabIndex = 20
        Me.txtNoOfAuthQueryCond.Tag = "No Of Auth Query Cond▄Authorization for Query condition"
        '
        'lblNoOfAuth
        '
        Me.lblNoOfAuth.AutoSize = True
        Me.lblNoOfAuth.Location = New System.Drawing.Point(6, 61)
        Me.lblNoOfAuth.Name = "lblNoOfAuth"
        Me.lblNoOfAuth.Size = New System.Drawing.Size(85, 20)
        Me.lblNoOfAuth.TabIndex = 18
        Me.lblNoOfAuth.Tag = "No of Auth▄It shows number of Authorizations"
        Me.lblNoOfAuth.Text = "No of Auth"
        '
        'txtNoOfAuth
        '
        Me.txtNoOfAuth.Location = New System.Drawing.Point(97, 61)
        Me.txtNoOfAuth.Name = "txtNoOfAuth"
        Me.txtNoOfAuth.Size = New System.Drawing.Size(82, 26)
        Me.txtNoOfAuth.TabIndex = 19
        '
        'GrpParty
        '
        Me.GrpParty.Controls.Add(Me.TxtRatePartyItem)
        Me.GrpParty.Controls.Add(Me.LblRatePartyItem)
        Me.GrpParty.Controls.Add(Me.ChkEnforcePartyItemLink)
        Me.GrpParty.Controls.Add(Me.ChkShowShipAdd)
        Me.GrpParty.Controls.Add(Me.TxtPartyFilterCond)
        Me.GrpParty.Controls.Add(Me.lblPartyFilterCond)
        Me.GrpParty.Controls.Add(Me.chkSavePartyData)
        Me.GrpParty.Controls.Add(Me.ChkManualPartySel)
        Me.GrpParty.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GrpParty.Location = New System.Drawing.Point(4, 3)
        Me.GrpParty.Name = "GrpParty"
        Me.GrpParty.Size = New System.Drawing.Size(950, 281)
        Me.GrpParty.TabIndex = 225
        Me.GrpParty.TabStop = False
        Me.GrpParty.Text = "Party"
        '
        'TxtRatePartyItem
        '
        Me.TxtRatePartyItem.Location = New System.Drawing.Point(174, 65)
        Me.TxtRatePartyItem.MaxLength = 50
        Me.TxtRatePartyItem.Name = "TxtRatePartyItem"
        Me.TxtRatePartyItem.Size = New System.Drawing.Size(115, 26)
        Me.TxtRatePartyItem.TabIndex = 8
        Me.TxtRatePartyItem.Tag = ""
        '
        'LblRatePartyItem
        '
        Me.LblRatePartyItem.AutoSize = True
        Me.LblRatePartyItem.Location = New System.Drawing.Point(13, 68)
        Me.LblRatePartyItem.Name = "LblRatePartyItem"
        Me.LblRatePartyItem.Size = New System.Drawing.Size(139, 20)
        Me.LblRatePartyItem.TabIndex = 7
        Me.LblRatePartyItem.Tag = "Party vs Item Rate Field▄This gives the rate according to party vs Item"
        Me.LblRatePartyItem.Text = "Party vs Item Rate"
        '
        'ChkEnforcePartyItemLink
        '
        Me.ChkEnforcePartyItemLink.AutoSize = True
        Me.ChkEnforcePartyItemLink.Location = New System.Drawing.Point(17, 25)
        Me.ChkEnforcePartyItemLink.Name = "ChkEnforcePartyItemLink"
        Me.ChkEnforcePartyItemLink.Size = New System.Drawing.Size(193, 24)
        Me.ChkEnforcePartyItemLink.TabIndex = 6
        Me.ChkEnforcePartyItemLink.Tag = "Enforce Party Item Link▄By selecting this user will get items against particular " &
    "party"
        Me.ChkEnforcePartyItemLink.Text = "Enforce Party Item Link"
        Me.ChkEnforcePartyItemLink.UseVisualStyleBackColor = True
        '
        'ChkShowShipAdd
        '
        Me.ChkShowShipAdd.AutoSize = True
        Me.ChkShowShipAdd.Location = New System.Drawing.Point(689, 219)
        Me.ChkShowShipAdd.Name = "ChkShowShipAdd"
        Me.ChkShowShipAdd.Size = New System.Drawing.Size(197, 24)
        Me.ChkShowShipAdd.TabIndex = 5
        Me.ChkShowShipAdd.Tag = "Show Shipping Address▄This option will show shipping address"
        Me.ChkShowShipAdd.Text = "Show Shipping Address"
        Me.ChkShowShipAdd.UseVisualStyleBackColor = True
        Me.ChkShowShipAdd.Visible = False
        '
        'TxtPartyFilterCond
        '
        Me.TxtPartyFilterCond.Location = New System.Drawing.Point(174, 98)
        Me.TxtPartyFilterCond.MaxLength = 250
        Me.TxtPartyFilterCond.Multiline = True
        Me.TxtPartyFilterCond.Name = "TxtPartyFilterCond"
        Me.TxtPartyFilterCond.Size = New System.Drawing.Size(705, 74)
        Me.TxtPartyFilterCond.TabIndex = 4
        Me.TxtPartyFilterCond.Tag = ""
        '
        'lblPartyFilterCond
        '
        Me.lblPartyFilterCond.AutoSize = True
        Me.lblPartyFilterCond.Location = New System.Drawing.Point(13, 121)
        Me.lblPartyFilterCond.Name = "lblPartyFilterCond"
        Me.lblPartyFilterCond.Size = New System.Drawing.Size(155, 20)
        Me.lblPartyFilterCond.TabIndex = 3
        Me.lblPartyFilterCond.Tag = "Party Filter Condition▄Adds filter condition for party Items"
        Me.lblPartyFilterCond.Text = "Party Filter Condition"
        '
        'chkSavePartyData
        '
        Me.chkSavePartyData.AutoSize = True
        Me.chkSavePartyData.Location = New System.Drawing.Point(689, 249)
        Me.chkSavePartyData.Name = "chkSavePartyData"
        Me.chkSavePartyData.Size = New System.Drawing.Size(143, 24)
        Me.chkSavePartyData.TabIndex = 1
        Me.chkSavePartyData.Tag = "Save Party Data▄It saves the party data"
        Me.chkSavePartyData.Text = "Save Party Data"
        Me.chkSavePartyData.UseVisualStyleBackColor = True
        Me.chkSavePartyData.Visible = False
        '
        'ChkManualPartySel
        '
        Me.ChkManualPartySel.AutoSize = True
        Me.ChkManualPartySel.Location = New System.Drawing.Point(689, 189)
        Me.ChkManualPartySel.Name = "ChkManualPartySel"
        Me.ChkManualPartySel.Size = New System.Drawing.Size(190, 24)
        Me.ChkManualPartySel.TabIndex = 0
        Me.ChkManualPartySel.Tag = "Manual Party Selection▄It allows the party selection manually"
        Me.ChkManualPartySel.Text = "Manual Party Selection"
        Me.ChkManualPartySel.UseVisualStyleBackColor = True
        Me.ChkManualPartySel.Visible = False
        '
        'GrpFreezeDoc
        '
        Me.GrpFreezeDoc.Controls.Add(Me.ChkFreezeDoc)
        Me.GrpFreezeDoc.Controls.Add(Me.lblAllowEditDlt)
        Me.GrpFreezeDoc.Controls.Add(Me.txtAllowEditDel)
        Me.GrpFreezeDoc.Controls.Add(Me.lblDays)
        Me.GrpFreezeDoc.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GrpFreezeDoc.Location = New System.Drawing.Point(19, 172)
        Me.GrpFreezeDoc.Name = "GrpFreezeDoc"
        Me.GrpFreezeDoc.Size = New System.Drawing.Size(910, 123)
        Me.GrpFreezeDoc.TabIndex = 227
        Me.GrpFreezeDoc.TabStop = False
        Me.GrpFreezeDoc.Text = "Freeze Docs"
        '
        'lblAllowEditDlt
        '
        Me.lblAllowEditDlt.AutoSize = True
        Me.lblAllowEditDlt.Location = New System.Drawing.Point(6, 57)
        Me.lblAllowEditDlt.Name = "lblAllowEditDlt"
        Me.lblAllowEditDlt.Size = New System.Drawing.Size(198, 20)
        Me.lblAllowEditDlt.TabIndex = 222
        Me.lblAllowEditDlt.Text = "Allow Editing/Deleting upto"
        '
        'txtAllowEditDel
        '
        Me.txtAllowEditDel.Location = New System.Drawing.Point(210, 57)
        Me.txtAllowEditDel.Name = "txtAllowEditDel"
        Me.txtAllowEditDel.Size = New System.Drawing.Size(78, 26)
        Me.txtAllowEditDel.TabIndex = 223
        Me.txtAllowEditDel.Tag = "Allow Edit Delete Days▄Allows editing deleting for particular days based on parti" &
    "cular date"
        '
        'lblDays
        '
        Me.lblDays.AutoSize = True
        Me.lblDays.Location = New System.Drawing.Point(294, 60)
        Me.lblDays.Name = "lblDays"
        Me.lblDays.Size = New System.Drawing.Size(45, 20)
        Me.lblDays.TabIndex = 224
        Me.lblDays.Text = "Days"
        '
        'txtItemFilterCondition
        '
        Me.txtItemFilterCondition.Location = New System.Drawing.Point(163, 85)
        Me.txtItemFilterCondition.MaxLength = 250
        Me.txtItemFilterCondition.Multiline = True
        Me.txtItemFilterCondition.Name = "txtItemFilterCondition"
        Me.txtItemFilterCondition.Size = New System.Drawing.Size(746, 44)
        Me.txtItemFilterCondition.TabIndex = 225
        Me.txtItemFilterCondition.Tag = "Item filter Condition▄Filter condition for a particular item"
        '
        'LblItemFilterCond
        '
        Me.LblItemFilterCond.AutoSize = True
        Me.LblItemFilterCond.Location = New System.Drawing.Point(6, 94)
        Me.LblItemFilterCond.Name = "LblItemFilterCond"
        Me.LblItemFilterCond.Size = New System.Drawing.Size(151, 20)
        Me.LblItemFilterCond.TabIndex = 226
        Me.LblItemFilterCond.Text = "Item Filter Condition"
        '
        'GrpDetSec
        '
        Me.GrpDetSec.Controls.Add(Me.TxtRateMstItem)
        Me.GrpDetSec.Controls.Add(Me.lblRateMstItem)
        Me.GrpDetSec.Controls.Add(Me.txtItemFilterCondition)
        Me.GrpDetSec.Controls.Add(Me.LblItemFilterCond)
        Me.GrpDetSec.Controls.Add(Me.ChkAllowDuplicateItems)
        Me.GrpDetSec.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GrpDetSec.Location = New System.Drawing.Point(20, 19)
        Me.GrpDetSec.Name = "GrpDetSec"
        Me.GrpDetSec.Size = New System.Drawing.Size(915, 147)
        Me.GrpDetSec.TabIndex = 227
        Me.GrpDetSec.TabStop = False
        Me.GrpDetSec.Text = "Detail"
        '
        'TxtRateMstItem
        '
        Me.TxtRateMstItem.Location = New System.Drawing.Point(163, 52)
        Me.TxtRateMstItem.MaxLength = 50
        Me.TxtRateMstItem.Name = "TxtRateMstItem"
        Me.TxtRateMstItem.Size = New System.Drawing.Size(115, 26)
        Me.TxtRateMstItem.TabIndex = 228
        Me.TxtRateMstItem.Tag = "Item Rate▄Rate field from MstItem"
        '
        'lblRateMstItem
        '
        Me.lblRateMstItem.AutoSize = True
        Me.lblRateMstItem.Location = New System.Drawing.Point(6, 58)
        Me.lblRateMstItem.Name = "lblRateMstItem"
        Me.lblRateMstItem.Size = New System.Drawing.Size(110, 20)
        Me.lblRateMstItem.TabIndex = 227
        Me.lblRateMstItem.Text = "Rate Mst Item"
        '
        'TCTrnsSettings
        '
        Me.TCTrnsSettings.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TCTrnsSettings.Controls.Add(Me.TPMain)
        Me.TCTrnsSettings.Controls.Add(Me.TPDetail1)
        Me.TCTrnsSettings.Controls.Add(Me.TPDetail2)
        Me.TCTrnsSettings.Controls.Add(Me.TPDetail3)
        Me.TCTrnsSettings.Controls.Add(Me.TPDetail4)
        Me.TCTrnsSettings.Controls.Add(Me.TPVouchers)
        Me.TCTrnsSettings.Location = New System.Drawing.Point(5, 46)
        Me.TCTrnsSettings.Name = "TCTrnsSettings"
        Me.TCTrnsSettings.SelectedIndex = 0
        Me.TCTrnsSettings.Size = New System.Drawing.Size(954, 346)
        Me.TCTrnsSettings.TabIndex = 3
        Me.TCTrnsSettings.Visible = False
        '
        'TPMain
        '
        Me.TPMain.Controls.Add(Me.Panel1)
        Me.TPMain.Location = New System.Drawing.Point(4, 22)
        Me.TPMain.Name = "TPMain"
        Me.TPMain.Padding = New System.Windows.Forms.Padding(3)
        Me.TPMain.Size = New System.Drawing.Size(946, 320)
        Me.TPMain.TabIndex = 0
        Me.TPMain.Text = "Main"
        Me.TPMain.UseVisualStyleBackColor = True
        '
        'TPDetail1
        '
        Me.TPDetail1.Controls.Add(Me.GrpAccountingSettings)
        Me.TPDetail1.Controls.Add(Me.LblTenderChange)
        Me.TPDetail1.Controls.Add(Me.TxtTenderChangeFld)
        Me.TPDetail1.Controls.Add(Me.GrpAuth)
        Me.TPDetail1.Controls.Add(Me.ChkAllowTenderChange)
        Me.TPDetail1.Location = New System.Drawing.Point(4, 22)
        Me.TPDetail1.Name = "TPDetail1"
        Me.TPDetail1.Padding = New System.Windows.Forms.Padding(3)
        Me.TPDetail1.Size = New System.Drawing.Size(946, 320)
        Me.TPDetail1.TabIndex = 1
        Me.TPDetail1.Text = "Detail1"
        Me.TPDetail1.UseVisualStyleBackColor = True
        '
        'GrpAccountingSettings
        '
        Me.GrpAccountingSettings.Controls.Add(Me.TxtAcVoucherValFld)
        Me.GrpAccountingSettings.Controls.Add(Me.LblAcVoucherValFld)
        Me.GrpAccountingSettings.Controls.Add(Me.TxtAcLedgerFldName)
        Me.GrpAccountingSettings.Controls.Add(Me.LblAcctLedgerFldName)
        Me.GrpAccountingSettings.Controls.Add(Me.CboAcctMstType)
        Me.GrpAccountingSettings.Controls.Add(Me.LblType)
        Me.GrpAccountingSettings.Controls.Add(Me.ChkAcctTrn)
        Me.GrpAccountingSettings.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GrpAccountingSettings.Location = New System.Drawing.Point(464, 22)
        Me.GrpAccountingSettings.Name = "GrpAccountingSettings"
        Me.GrpAccountingSettings.Size = New System.Drawing.Size(441, 196)
        Me.GrpAccountingSettings.TabIndex = 232
        Me.GrpAccountingSettings.TabStop = False
        Me.GrpAccountingSettings.Text = "Accounting Settings"
        '
        'TxtAcVoucherValFld
        '
        Me.TxtAcVoucherValFld.Location = New System.Drawing.Point(196, 142)
        Me.TxtAcVoucherValFld.MaxLength = 50
        Me.TxtAcVoucherValFld.Name = "TxtAcVoucherValFld"
        Me.TxtAcVoucherValFld.Size = New System.Drawing.Size(187, 26)
        Me.TxtAcVoucherValFld.TabIndex = 238
        '
        'LblAcVoucherValFld
        '
        Me.LblAcVoucherValFld.AutoSize = True
        Me.LblAcVoucherValFld.Location = New System.Drawing.Point(15, 145)
        Me.LblAcVoucherValFld.Name = "LblAcVoucherValFld"
        Me.LblAcVoucherValFld.Size = New System.Drawing.Size(175, 20)
        Me.LblAcVoucherValFld.TabIndex = 237
        Me.LblAcVoucherValFld.Text = "Ac Voucher Value Field"
        '
        'TxtAcLedgerFldName
        '
        Me.TxtAcLedgerFldName.Location = New System.Drawing.Point(196, 101)
        Me.TxtAcLedgerFldName.MaxLength = 50
        Me.TxtAcLedgerFldName.Name = "TxtAcLedgerFldName"
        Me.TxtAcLedgerFldName.Size = New System.Drawing.Size(187, 26)
        Me.TxtAcLedgerFldName.TabIndex = 236
        '
        'LblAcctLedgerFldName
        '
        Me.LblAcctLedgerFldName.AutoSize = True
        Me.LblAcctLedgerFldName.Location = New System.Drawing.Point(15, 104)
        Me.LblAcctLedgerFldName.Name = "LblAcctLedgerFldName"
        Me.LblAcctLedgerFldName.Size = New System.Drawing.Size(158, 20)
        Me.LblAcctLedgerFldName.TabIndex = 235
        Me.LblAcctLedgerFldName.Text = "Ac. Ledger Fld Name"
        '
        'CboAcctMstType
        '
        Me.CboAcctMstType.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboAcctMstType.FormattingEnabled = True
        Me.CboAcctMstType.Items.AddRange(New Object() {"D", "C"})
        Me.CboAcctMstType.Location = New System.Drawing.Point(196, 64)
        Me.CboAcctMstType.Name = "CboAcctMstType"
        Me.CboAcctMstType.Size = New System.Drawing.Size(187, 28)
        Me.CboAcctMstType.TabIndex = 8
        Me.CboAcctMstType.Tag = "AcctMstType▄Shows Type"
        '
        'LblType
        '
        Me.LblType.AutoSize = True
        Me.LblType.Location = New System.Drawing.Point(15, 67)
        Me.LblType.Name = "LblType"
        Me.LblType.Size = New System.Drawing.Size(43, 20)
        Me.LblType.TabIndex = 234
        Me.LblType.Text = "Type"
        '
        'ChkAcctTrn
        '
        Me.ChkAcctTrn.AutoSize = True
        Me.ChkAcctTrn.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkAcctTrn.Location = New System.Drawing.Point(6, 25)
        Me.ChkAcctTrn.Name = "ChkAcctTrn"
        Me.ChkAcctTrn.Size = New System.Drawing.Size(195, 24)
        Me.ChkAcctTrn.TabIndex = 233
        Me.ChkAcctTrn.Tag = "Accounting Transaction▄For Tally Related Fields"
        Me.ChkAcctTrn.Text = "Accounting Transaction"
        Me.ChkAcctTrn.UseVisualStyleBackColor = True
        '
        'LblTenderChange
        '
        Me.LblTenderChange.AutoSize = True
        Me.LblTenderChange.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblTenderChange.Location = New System.Drawing.Point(209, 155)
        Me.LblTenderChange.Name = "LblTenderChange"
        Me.LblTenderChange.Size = New System.Drawing.Size(119, 20)
        Me.LblTenderChange.TabIndex = 231
        Me.LblTenderChange.Text = "Tender Change"
        '
        'TxtTenderChangeFld
        '
        Me.TxtTenderChangeFld.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtTenderChangeFld.Location = New System.Drawing.Point(213, 178)
        Me.TxtTenderChangeFld.MaxLength = 50
        Me.TxtTenderChangeFld.Multiline = True
        Me.TxtTenderChangeFld.Name = "TxtTenderChangeFld"
        Me.TxtTenderChangeFld.Size = New System.Drawing.Size(222, 32)
        Me.TxtTenderChangeFld.TabIndex = 230
        '
        'TPDetail2
        '
        Me.TPDetail2.Controls.Add(Me.GrpParty)
        Me.TPDetail2.Location = New System.Drawing.Point(4, 22)
        Me.TPDetail2.Name = "TPDetail2"
        Me.TPDetail2.Size = New System.Drawing.Size(946, 320)
        Me.TPDetail2.TabIndex = 2
        Me.TPDetail2.Text = "Detail2"
        Me.TPDetail2.UseVisualStyleBackColor = True
        '
        'TPDetail3
        '
        Me.TPDetail3.Controls.Add(Me.GrpPendQuryFlds)
        Me.TPDetail3.Location = New System.Drawing.Point(4, 22)
        Me.TPDetail3.Name = "TPDetail3"
        Me.TPDetail3.Size = New System.Drawing.Size(946, 320)
        Me.TPDetail3.TabIndex = 3
        Me.TPDetail3.Text = "Detail3"
        Me.TPDetail3.UseVisualStyleBackColor = True
        '
        'TPDetail4
        '
        Me.TPDetail4.Controls.Add(Me.GrpDetSec)
        Me.TPDetail4.Controls.Add(Me.GrpFreezeDoc)
        Me.TPDetail4.Location = New System.Drawing.Point(4, 22)
        Me.TPDetail4.Name = "TPDetail4"
        Me.TPDetail4.Size = New System.Drawing.Size(946, 320)
        Me.TPDetail4.TabIndex = 4
        Me.TPDetail4.Text = "Detail4"
        Me.TPDetail4.UseVisualStyleBackColor = True
        '
        'TPVouchers
        '
        Me.TPVouchers.Controls.Add(Me.CboDefFirstRowType)
        Me.TPVouchers.Controls.Add(Me.LblDefRow)
        Me.TPVouchers.Controls.Add(Me.ChkChqPrinting)
        Me.TPVouchers.Controls.Add(Me.ChkBillWiseFeature)
        Me.TPVouchers.Controls.Add(Me.ChkBalColReq)
        Me.TPVouchers.Location = New System.Drawing.Point(4, 22)
        Me.TPVouchers.Name = "TPVouchers"
        Me.TPVouchers.Size = New System.Drawing.Size(946, 320)
        Me.TPVouchers.TabIndex = 5
        Me.TPVouchers.Text = "Vouchers"
        Me.TPVouchers.UseVisualStyleBackColor = True
        '
        'CboDefFirstRowType
        '
        Me.CboDefFirstRowType.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboDefFirstRowType.FormattingEnabled = True
        Me.CboDefFirstRowType.Items.AddRange(New Object() {"Dr", "Cr"})
        Me.CboDefFirstRowType.Location = New System.Drawing.Point(203, 110)
        Me.CboDefFirstRowType.Name = "CboDefFirstRowType"
        Me.CboDefFirstRowType.Size = New System.Drawing.Size(187, 28)
        Me.CboDefFirstRowType.TabIndex = 8
        Me.CboDefFirstRowType.Tag = "Transaction▄Shows the transaction Document names"
        '
        'LblDefRow
        '
        Me.LblDefRow.AutoSize = True
        Me.LblDefRow.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblDefRow.Location = New System.Drawing.Point(16, 113)
        Me.LblDefRow.Name = "LblDefRow"
        Me.LblDefRow.Size = New System.Drawing.Size(170, 20)
        Me.LblDefRow.TabIndex = 32
        Me.LblDefRow.Text = "Default First Row Type"
        '
        'ChkChqPrinting
        '
        Me.ChkChqPrinting.AutoSize = True
        Me.ChkChqPrinting.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkChqPrinting.Location = New System.Drawing.Point(20, 77)
        Me.ChkChqPrinting.Name = "ChkChqPrinting"
        Me.ChkChqPrinting.Size = New System.Drawing.Size(141, 24)
        Me.ChkChqPrinting.TabIndex = 31
        Me.ChkChqPrinting.Tag = "Cheque Printing"
        Me.ChkChqPrinting.Text = "Cheque Printing"
        Me.ChkChqPrinting.UseVisualStyleBackColor = True
        '
        'ChkBillWiseFeature
        '
        Me.ChkBillWiseFeature.AutoSize = True
        Me.ChkBillWiseFeature.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkBillWiseFeature.Location = New System.Drawing.Point(19, 47)
        Me.ChkBillWiseFeature.Name = "ChkBillWiseFeature"
        Me.ChkBillWiseFeature.Size = New System.Drawing.Size(212, 24)
        Me.ChkBillWiseFeature.TabIndex = 30
        Me.ChkBillWiseFeature.Tag = "Bill wise Feature Required"
        Me.ChkBillWiseFeature.Text = "Bill wise Feature Required"
        Me.ChkBillWiseFeature.UseVisualStyleBackColor = True
        '
        'ChkBalColReq
        '
        Me.ChkBalColReq.AutoSize = True
        Me.ChkBalColReq.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkBalColReq.Location = New System.Drawing.Point(19, 17)
        Me.ChkBalColReq.Name = "ChkBalColReq"
        Me.ChkBalColReq.Size = New System.Drawing.Size(213, 24)
        Me.ChkBalColReq.TabIndex = 29
        Me.ChkBalColReq.Tag = "Balance Column Required"
        Me.ChkBalColReq.Text = "Balance Column Required"
        Me.ChkBalColReq.UseVisualStyleBackColor = True
        '
        'GrpMisc
        '
        Me.GrpMisc.Controls.Add(Me.chkShowMenuIcons)
        Me.GrpMisc.Controls.Add(Me.BtnSaveMisc)
        Me.GrpMisc.Controls.Add(Me.BtnCancelMisc)
        Me.GrpMisc.Controls.Add(Me.txtSearchTopN)
        Me.GrpMisc.Controls.Add(Me.txtItemDescCaption)
        Me.GrpMisc.Controls.Add(Me.btnClose)
        Me.GrpMisc.Controls.Add(Me.txtItemNameCaption)
        Me.GrpMisc.Controls.Add(Me.txtItemCaption)
        Me.GrpMisc.Controls.Add(Me.lblFldSearchTopN)
        Me.GrpMisc.Controls.Add(Me.lblItemDescCaption)
        Me.GrpMisc.Controls.Add(Me.lblItemNameCaption)
        Me.GrpMisc.Controls.Add(Me.LblItemCaption)
        Me.GrpMisc.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GrpMisc.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GrpMisc.Location = New System.Drawing.Point(0, 0)
        Me.GrpMisc.Name = "GrpMisc"
        Me.GrpMisc.Size = New System.Drawing.Size(957, 580)
        Me.GrpMisc.TabIndex = 9
        Me.GrpMisc.TabStop = False
        Me.GrpMisc.Text = "Miscellaneous"
        Me.GrpMisc.Visible = False
        '
        'chkShowMenuIcons
        '
        Me.chkShowMenuIcons.AutoSize = True
        Me.chkShowMenuIcons.Location = New System.Drawing.Point(214, 228)
        Me.chkShowMenuIcons.Name = "chkShowMenuIcons"
        Me.chkShowMenuIcons.Size = New System.Drawing.Size(155, 24)
        Me.chkShowMenuIcons.TabIndex = 12
        Me.chkShowMenuIcons.Text = "Show Menu Icons"
        Me.chkShowMenuIcons.UseVisualStyleBackColor = True
        '
        'BtnSaveMisc
        '
        Me.BtnSaveMisc.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnSaveMisc.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.BtnSaveMisc.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnSaveMisc.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnSaveMisc.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.BtnSaveMisc.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnSaveMisc.Location = New System.Drawing.Point(779, 362)
        Me.BtnSaveMisc.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnSaveMisc.Name = "BtnSaveMisc"
        Me.BtnSaveMisc.Size = New System.Drawing.Size(80, 30)
        Me.BtnSaveMisc.TabIndex = 10
        Me.BtnSaveMisc.Text = "&Save"
        Me.BtnSaveMisc.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.BtnSaveMisc, "Save : Alt + S")
        Me.BtnSaveMisc.UseVisualStyleBackColor = False
        '
        'BtnCancelMisc
        '
        Me.BtnCancelMisc.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnCancelMisc.BackColor = System.Drawing.Color.Transparent
        Me.BtnCancelMisc.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.BtnCancelMisc.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.BtnCancelMisc.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnCancelMisc.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.BtnCancelMisc.ForeColor = System.Drawing.Color.Black
        Me.BtnCancelMisc.Location = New System.Drawing.Point(865, 362)
        Me.BtnCancelMisc.Name = "BtnCancelMisc"
        Me.BtnCancelMisc.Size = New System.Drawing.Size(80, 30)
        Me.BtnCancelMisc.TabIndex = 11
        Me.BtnCancelMisc.Text = "&Cancel"
        Me.BtnCancelMisc.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.BtnCancelMisc, "Cancel : Alt + C")
        Me.BtnCancelMisc.UseVisualStyleBackColor = False
        '
        'txtSearchTopN
        '
        Me.txtSearchTopN.Location = New System.Drawing.Point(214, 181)
        Me.txtSearchTopN.Name = "txtSearchTopN"
        Me.txtSearchTopN.Size = New System.Drawing.Size(281, 26)
        Me.txtSearchTopN.TabIndex = 8
        '
        'txtItemDescCaption
        '
        Me.txtItemDescCaption.Location = New System.Drawing.Point(214, 137)
        Me.txtItemDescCaption.MaxLength = 50
        Me.txtItemDescCaption.Name = "txtItemDescCaption"
        Me.txtItemDescCaption.Size = New System.Drawing.Size(281, 26)
        Me.txtItemDescCaption.TabIndex = 7
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackColor = System.Drawing.Color.Transparent
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnClose.ForeColor = System.Drawing.Color.Black
        Me.btnClose.Location = New System.Drawing.Point(871, 542)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(80, 30)
        Me.btnClose.TabIndex = 5
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close : Alt + O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'txtItemNameCaption
        '
        Me.txtItemNameCaption.Location = New System.Drawing.Point(214, 88)
        Me.txtItemNameCaption.MaxLength = 50
        Me.txtItemNameCaption.Name = "txtItemNameCaption"
        Me.txtItemNameCaption.Size = New System.Drawing.Size(281, 26)
        Me.txtItemNameCaption.TabIndex = 6
        '
        'txtItemCaption
        '
        Me.txtItemCaption.Location = New System.Drawing.Point(214, 45)
        Me.txtItemCaption.MaxLength = 50
        Me.txtItemCaption.Name = "txtItemCaption"
        Me.txtItemCaption.Size = New System.Drawing.Size(281, 26)
        Me.txtItemCaption.TabIndex = 5
        '
        'lblFldSearchTopN
        '
        Me.lblFldSearchTopN.AutoSize = True
        Me.lblFldSearchTopN.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFldSearchTopN.Location = New System.Drawing.Point(29, 181)
        Me.lblFldSearchTopN.Name = "lblFldSearchTopN"
        Me.lblFldSearchTopN.Size = New System.Drawing.Size(132, 20)
        Me.lblFldSearchTopN.TabIndex = 3
        Me.lblFldSearchTopN.Text = "Fld Search Top N"
        '
        'lblItemDescCaption
        '
        Me.lblItemDescCaption.AutoSize = True
        Me.lblItemDescCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItemDescCaption.Location = New System.Drawing.Point(29, 137)
        Me.lblItemDescCaption.Name = "lblItemDescCaption"
        Me.lblItemDescCaption.Size = New System.Drawing.Size(141, 20)
        Me.lblItemDescCaption.TabIndex = 2
        Me.lblItemDescCaption.Text = "Item Desc Caption"
        '
        'lblItemNameCaption
        '
        Me.lblItemNameCaption.AutoSize = True
        Me.lblItemNameCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblItemNameCaption.Location = New System.Drawing.Point(29, 91)
        Me.lblItemNameCaption.Name = "lblItemNameCaption"
        Me.lblItemNameCaption.Size = New System.Drawing.Size(146, 20)
        Me.lblItemNameCaption.TabIndex = 1
        Me.lblItemNameCaption.Text = "Item Name Caption"
        '
        'LblItemCaption
        '
        Me.LblItemCaption.AutoSize = True
        Me.LblItemCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblItemCaption.Location = New System.Drawing.Point(29, 48)
        Me.LblItemCaption.Name = "LblItemCaption"
        Me.LblItemCaption.Size = New System.Drawing.Size(100, 20)
        Me.LblItemCaption.TabIndex = 0
        Me.LblItemCaption.Text = "Item Caption"
        '
        'GrpHelp
        '
        Me.GrpHelp.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GrpHelp.Controls.Add(Me.txtTitle)
        Me.GrpHelp.Controls.Add(Me.txtContent)
        Me.GrpHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GrpHelp.Location = New System.Drawing.Point(5, 395)
        Me.GrpHelp.Name = "GrpHelp"
        Me.GrpHelp.Size = New System.Drawing.Size(954, 133)
        Me.GrpHelp.TabIndex = 4
        Me.GrpHelp.TabStop = False
        Me.GrpHelp.Text = "Help"
        '
        'txtTitle
        '
        Me.txtTitle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTitle.Location = New System.Drawing.Point(13, 25)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.ReadOnly = True
        Me.txtTitle.Size = New System.Drawing.Size(935, 26)
        Me.txtTitle.TabIndex = 215
        Me.txtTitle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtContent
        '
        Me.txtContent.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtContent.Location = New System.Drawing.Point(12, 57)
        Me.txtContent.Multiline = True
        Me.txtContent.Name = "txtContent"
        Me.txtContent.ReadOnly = True
        Me.txtContent.Size = New System.Drawing.Size(936, 70)
        Me.txtContent.TabIndex = 216
        '
        'btnLoad
        '
        Me.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLoad.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoad.Location = New System.Drawing.Point(311, 15)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(88, 28)
        Me.btnLoad.TabIndex = 2
        Me.btnLoad.Text = "&Load"
        Me.ToolTip1.SetToolTip(Me.btnLoad, "Load : Alt + L")
        Me.btnLoad.UseVisualStyleBackColor = False
        '
        'LblTrn
        '
        Me.LblTrn.AutoSize = True
        Me.LblTrn.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblTrn.ForeColor = System.Drawing.Color.Red
        Me.LblTrn.Location = New System.Drawing.Point(9, 17)
        Me.LblTrn.Name = "LblTrn"
        Me.LblTrn.Size = New System.Drawing.Size(92, 20)
        Me.LblTrn.TabIndex = 0
        Me.LblTrn.Tag = "M"
        Me.LblTrn.Text = "Transaction"
        '
        'CboTrnDocType
        '
        Me.CboTrnDocType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.CboTrnDocType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.HistoryList
        Me.CboTrnDocType.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboTrnDocType.FormattingEnabled = True
        Me.CboTrnDocType.Location = New System.Drawing.Point(113, 14)
        Me.CboTrnDocType.Name = "CboTrnDocType"
        Me.CboTrnDocType.Size = New System.Drawing.Size(187, 28)
        Me.CboTrnDocType.TabIndex = 1
        Me.CboTrnDocType.Tag = "Transaction▄Shows the transaction Document names"
        '
        'btnMisc
        '
        Me.btnMisc.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnMisc.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMisc.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMisc.Location = New System.Drawing.Point(18, 539)
        Me.btnMisc.Name = "btnMisc"
        Me.btnMisc.Size = New System.Drawing.Size(172, 33)
        Me.btnMisc.TabIndex = 14
        Me.btnMisc.Text = "Misc. Settings"
        Me.btnMisc.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(700, 542)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(80, 30)
        Me.btnSave.TabIndex = 6
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Save : Alt + S")
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackColor = System.Drawing.Color.Transparent
        Me.btnCancel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnCancel.ForeColor = System.Drawing.Color.Black
        Me.btnCancel.Location = New System.Drawing.Point(786, 542)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(80, 30)
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Cancel : Alt + C")
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'chkPdfPrintP
        '
        Me.chkPdfPrintP.AutoSize = True
        Me.chkPdfPrintP.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPdfPrintP.Location = New System.Drawing.Point(440, 15)
        Me.chkPdfPrintP.Name = "chkPdfPrintP"
        Me.chkPdfPrintP.Size = New System.Drawing.Size(154, 24)
        Me.chkPdfPrintP.TabIndex = 229
        Me.chkPdfPrintP.Tag = "Print Preview In PDF▄This Option shows Print In PDF Format"
        Me.chkPdfPrintP.Text = "PDF Print Preview"
        Me.chkPdfPrintP.UseVisualStyleBackColor = True
        '
        'FrmUtlTrnSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(957, 580)
        Me.Controls.Add(Me.btnMisc)
        Me.Controls.Add(Me.btnLoad)
        Me.Controls.Add(Me.GrpHelp)
        Me.Controls.Add(Me.LblTrn)
        Me.Controls.Add(Me.CboTrnDocType)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.TCTrnsSettings)
        Me.Controls.Add(Me.GrpMisc)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(962, 616)
        Me.Name = "FrmUtlTrnSettings"
        Me.ShowIcon = False
        Me.Text = "Transaction/Voucher Settings"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.grpLinkedPending.ResumeLayout(False)
        Me.grpLinkedPending.PerformLayout()
        Me.GrpPendQuryFlds.ResumeLayout(False)
        Me.GrpPendQuryFlds.PerformLayout()
        Me.GrpAuth.ResumeLayout(False)
        Me.GrpAuth.PerformLayout()
        Me.GrpParty.ResumeLayout(False)
        Me.GrpParty.PerformLayout()
        Me.GrpFreezeDoc.ResumeLayout(False)
        Me.GrpFreezeDoc.PerformLayout()
        Me.GrpDetSec.ResumeLayout(False)
        Me.GrpDetSec.PerformLayout()
        Me.TCTrnsSettings.ResumeLayout(False)
        Me.TPMain.ResumeLayout(False)
        Me.TPDetail1.ResumeLayout(False)
        Me.TPDetail1.PerformLayout()
        Me.GrpAccountingSettings.ResumeLayout(False)
        Me.GrpAccountingSettings.PerformLayout()
        Me.TPDetail2.ResumeLayout(False)
        Me.TPDetail3.ResumeLayout(False)
        Me.TPDetail4.ResumeLayout(False)
        Me.TPVouchers.ResumeLayout(False)
        Me.TPVouchers.PerformLayout()
        Me.GrpMisc.ResumeLayout(False)
        Me.GrpMisc.PerformLayout()
        Me.GrpHelp.ResumeLayout(False)
        Me.GrpHelp.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnSave As C1.Win.C1Input.C1Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents ChkShowGraceQty As CheckBox
    Friend WithEvents ChkAllowTenderChange As CheckBox
    Friend WithEvents ChkFreezeDoc As CheckBox
    Friend WithEvents ChkAllowDuplicateItems As CheckBox
    Friend WithEvents ChkAuthorizationReq As CheckBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents lblNoOfAuth As Label
    Friend WithEvents chkAffectsStock As CheckBox
    Friend WithEvents txtNoOfAuthQueryCond As TextBox
    Friend WithEvents txtNoOfAuth As TextBox
    Friend WithEvents CboAddReduce As ComboBox
    Friend WithEvents lblDays As Label
    Friend WithEvents txtAllowEditDel As TextBox
    Friend WithEvents lblAllowEditDlt As Label
    Friend WithEvents GrpParty As GroupBox
    Friend WithEvents TxtPartyFilterCond As TextBox
    Friend WithEvents lblPartyFilterCond As Label
    Friend WithEvents chkSavePartyData As CheckBox
    Friend WithEvents ChkManualPartySel As CheckBox
    Friend WithEvents txtItemFilterCondition As TextBox
    Friend WithEvents grpLinkedPending As GroupBox
    Friend WithEvents ChkLinkedPendDocNo As CheckBox
    Friend WithEvents ChkLinkedPendParty As CheckBox
    Friend WithEvents LblItemFilterCond As Label
    Friend WithEvents ChkLinkedPendSingleDoc As CheckBox
    Friend WithEvents GrpFreezeDoc As GroupBox
    Friend WithEvents GrpDetSec As GroupBox
    Friend WithEvents GrpPendQuryFlds As GroupBox
    Friend WithEvents LblQuryTbls As Label
    Friend WithEvents txtQuryTbls As TextBox
    Friend WithEvents LblQuryCdtn As Label
    Friend WithEvents TxtQuryFlds As TextBox
    Friend WithEvents LblQryFlds As Label
    Friend WithEvents txtQuryCdtn As TextBox
    Friend WithEvents txtPickFldsFrmDocQry As TextBox
    Friend WithEvents LblPickFldsFrmSrcDocQry As Label
    Friend WithEvents ChkShowShipAdd As CheckBox
    Friend WithEvents GrpAuth As GroupBox
    Friend WithEvents TCTrnsSettings As TabControl
    Friend WithEvents TPMain As TabPage
    Friend WithEvents TPDetail1 As TabPage
    Friend WithEvents TPDetail2 As TabPage
    Friend WithEvents TPDetail3 As TabPage
    Friend WithEvents TPDetail4 As TabPage
    Friend WithEvents GrpHelp As GroupBox
    Friend WithEvents txtTitle As TextBox
    Friend WithEvents txtContent As TextBox
    Friend WithEvents chkLoadfrmBOM As CheckBox
    Friend WithEvents ChkAmmendmentReq As CheckBox
    Friend WithEvents ChkBlnEnvPrint As CheckBox
    Friend WithEvents ChkOfflineCalc As CheckBox
    Friend WithEvents ChkAllowCopyFrm As CheckBox
    Friend WithEvents ChkFileAttachReq As CheckBox
    Friend WithEvents ChkMultiDocGen As CheckBox
    Friend WithEvents TPVouchers As TabPage
    Friend WithEvents btnLoad As Button
    Friend WithEvents LblTrn As Label
    Friend WithEvents CboTrnDocType As ComboBox
    Friend WithEvents LblTenderChange As Label
    Friend WithEvents TxtTenderChangeFld As TextBox
    Friend WithEvents ChkAllowImportExport As CheckBox
    Friend WithEvents ChkEnforcePartyItemLink As CheckBox
    Friend WithEvents TxtRatePartyItem As TextBox
    Friend WithEvents LblRatePartyItem As Label
    Friend WithEvents TxtRateMstItem As TextBox
    Friend WithEvents lblRateMstItem As Label
    Friend WithEvents GrpAccountingSettings As GroupBox
    Friend WithEvents CboAcctMstType As ComboBox
    Friend WithEvents LblType As Label
    Friend WithEvents TxtAcLedgerFldName As TextBox
    Friend WithEvents LblAcctLedgerFldName As Label
    Friend WithEvents TxtAcVoucherValFld As TextBox
    Friend WithEvents LblAcVoucherValFld As Label
    Friend WithEvents ChkBillWiseFeature As CheckBox
    Friend WithEvents ChkBalColReq As CheckBox
    Friend WithEvents ChkChqPrinting As CheckBox
    Friend WithEvents CboDefFirstRowType As ComboBox
    Friend WithEvents LblDefRow As Label
    Friend WithEvents ChkAllowEditDocDate As CheckBox
    Friend WithEvents ChkAcctTrn As CheckBox
    Friend WithEvents btnMisc As Button
    Friend WithEvents GrpMisc As GroupBox
    Friend WithEvents BtnSaveMisc As C1.Win.C1Input.C1Button
    Friend WithEvents BtnCancelMisc As Button
    Friend WithEvents txtSearchTopN As TextBox
    Friend WithEvents txtItemDescCaption As TextBox
    Friend WithEvents txtItemNameCaption As TextBox
    Friend WithEvents txtItemCaption As TextBox
    Friend WithEvents lblFldSearchTopN As Label
    Friend WithEvents lblItemDescCaption As Label
    Friend WithEvents lblItemNameCaption As Label
    Friend WithEvents LblItemCaption As Label
    Friend WithEvents chkShowMenuIcons As CheckBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents txtPickFldsFrmSrc As TextBox
    Friend WithEvents LblPicFldsFrmSrc As Label
    Friend WithEvents chkPdfPrintP As CheckBox
End Class
