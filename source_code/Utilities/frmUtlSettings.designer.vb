<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlSettings
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlSettings))
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.cdg = New System.Windows.Forms.ColorDialog()
        Me.tt = New System.Windows.Forms.ToolTip(Me.components)
        Me.flowBtnCtrls = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnOptions = New C1.Win.C1Input.C1SplitButton()
        Me.btnMovePrev = New System.Windows.Forms.Button()
        Me.btnMoveFirst = New System.Windows.Forms.Button()
        Me.btnFind = New System.Windows.Forms.Button()
        Me.btnMoveNext = New System.Windows.Forms.Button()
        Me.btnMoveLast = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.tabSettings = New vLQMS.DotNetBarTabcontrol()
        Me.tbBtnCtrl = New System.Windows.Forms.TabPage()
        Me.pnlBtnCtrlSettings = New System.Windows.Forms.Panel()
        Me.svtBtnCtrl = New vLQMS.btnCtrl()
        Me.pnlLinksThemes = New System.Windows.Forms.Panel()
        Me.rdoTheme5 = New System.Windows.Forms.RadioButton()
        Me.rdoTheme4 = New System.Windows.Forms.RadioButton()
        Me.rdoTheme3 = New System.Windows.Forms.RadioButton()
        Me.rdoTheme2 = New System.Windows.Forms.RadioButton()
        Me.rdoTheme1 = New System.Windows.Forms.RadioButton()
        Me.rdoDefaultTheme = New System.Windows.Forms.RadioButton()
        Me.lblHotKeys2 = New System.Windows.Forms.LinkLabel()
        Me.lblForeColor2 = New System.Windows.Forms.LinkLabel()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.rdoBtnColor = New System.Windows.Forms.RadioButton()
        Me.rdoBtnTheme = New System.Windows.Forms.RadioButton()
        Me.pnlLinks = New System.Windows.Forms.Panel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblChangeHotKeys = New System.Windows.Forms.LinkLabel()
        Me.lblChangeBackClr = New System.Windows.Forms.LinkLabel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblChangeForeClr = New System.Windows.Forms.LinkLabel()
        Me.txtTheme = New System.Windows.Forms.TextBox()
        Me.tbInvoice = New System.Windows.Forms.TabPage()
        Me.tbPurchase = New System.Windows.Forms.TabPage()
        Me.btnSettClose = New C1.Win.C1Input.C1Button()
        Me.btnSettSave = New C1.Win.C1Input.C1Button()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.flowBtnCtrls.SuspendLayout()
        Me.tabSettings.SuspendLayout()
        Me.tbBtnCtrl.SuspendLayout()
        Me.pnlBtnCtrlSettings.SuspendLayout()
        Me.pnlLinksThemes.SuspendLayout()
        Me.pnlLinks.SuspendLayout()
        Me.SuspendLayout()
        '
        'vsfDet
        '
        Me.vsfDet.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "3,0,0,0,0,120,Columns:"
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.Location = New System.Drawing.Point(13, 186)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(80, 22)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 0
        Me.vsfDet.Visible = False
        '
        'flowBtnCtrls
        '
        Me.flowBtnCtrls.Controls.Add(Me.btnEdit)
        Me.flowBtnCtrls.Controls.Add(Me.btnDelete)
        Me.flowBtnCtrls.Controls.Add(Me.btnClose)
        Me.flowBtnCtrls.Controls.Add(Me.btnPrint)
        Me.flowBtnCtrls.Controls.Add(Me.btnOptions)
        Me.flowBtnCtrls.Controls.Add(Me.btnMovePrev)
        Me.flowBtnCtrls.Controls.Add(Me.btnMoveFirst)
        Me.flowBtnCtrls.Controls.Add(Me.btnFind)
        Me.flowBtnCtrls.Controls.Add(Me.btnMoveNext)
        Me.flowBtnCtrls.Controls.Add(Me.btnMoveLast)
        Me.flowBtnCtrls.Controls.Add(Me.btnSave)
        Me.flowBtnCtrls.Controls.Add(Me.btnCancel)
        Me.flowBtnCtrls.Location = New System.Drawing.Point(110, 175)
        Me.flowBtnCtrls.Name = "flowBtnCtrls"
        Me.flowBtnCtrls.Size = New System.Drawing.Size(10, 36)
        Me.flowBtnCtrls.TabIndex = 32
        Me.flowBtnCtrls.Visible = False
        '
        'btnEdit
        '
        Me.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.btnEdit.AutoSize = True
        Me.btnEdit.BackColor = System.Drawing.SystemColors.MenuBar
        Me.btnEdit.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEdit.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnEdit.ForeColor = System.Drawing.Color.Black
        Me.btnEdit.Image = CType(resources.GetObject("btnEdit.Image"), System.Drawing.Image)
        Me.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnEdit.Location = New System.Drawing.Point(0, 0)
        Me.btnEdit.Margin = New System.Windows.Forms.Padding(0)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(57, 35)
        Me.btnEdit.TabIndex = 20
        Me.btnEdit.Tag = "Edit"
        Me.btnEdit.Text = "&Edit"
        Me.btnEdit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnEdit.UseVisualStyleBackColor = False
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.btnDelete.AutoSize = True
        Me.btnDelete.BackColor = System.Drawing.SystemColors.MenuBar
        Me.btnDelete.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDelete.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnDelete.ForeColor = System.Drawing.Color.Black
        Me.btnDelete.Image = CType(resources.GetObject("btnDelete.Image"), System.Drawing.Image)
        Me.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDelete.Location = New System.Drawing.Point(0, 35)
        Me.btnDelete.Margin = New System.Windows.Forms.Padding(0)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(72, 35)
        Me.btnDelete.TabIndex = 21
        Me.btnDelete.Tag = "Delete"
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnDelete.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.btnClose.AutoSize = True
        Me.btnClose.BackColor = System.Drawing.SystemColors.MenuBar
        Me.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnClose.ForeColor = System.Drawing.Color.Black
        Me.btnClose.Image = CType(resources.GetObject("btnClose.Image"), System.Drawing.Image)
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(0, 70)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(0)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(67, 35)
        Me.btnClose.TabIndex = 23
        Me.btnClose.Tag = "Close"
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnPrint
        '
        Me.btnPrint.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.btnPrint.AutoSize = True
        Me.btnPrint.BackColor = System.Drawing.SystemColors.MenuBar
        Me.btnPrint.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPrint.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnPrint.ForeColor = System.Drawing.Color.Black
        Me.btnPrint.Image = CType(resources.GetObject("btnPrint.Image"), System.Drawing.Image)
        Me.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPrint.Location = New System.Drawing.Point(0, 105)
        Me.btnPrint.Margin = New System.Windows.Forms.Padding(0)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(61, 35)
        Me.btnPrint.TabIndex = 22
        Me.btnPrint.Tag = "Print"
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnPrint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnPrint.UseVisualStyleBackColor = False
        '
        'btnOptions
        '
        Me.btnOptions.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.btnOptions.AutoSize = True
        Me.btnOptions.BackColor = System.Drawing.SystemColors.MenuBar
        Me.btnOptions.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnOptions.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOptions.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnOptions.ForeColor = System.Drawing.Color.Black
        Me.btnOptions.Image = CType(resources.GetObject("btnOptions.Image"), System.Drawing.Image)
        Me.btnOptions.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnOptions.Location = New System.Drawing.Point(0, 140)
        Me.btnOptions.Margin = New System.Windows.Forms.Padding(0)
        Me.btnOptions.Name = "btnOptions"
        Me.btnOptions.Size = New System.Drawing.Size(99, 35)
        Me.btnOptions.TabIndex = 28
        Me.btnOptions.Tag = "Options"
        Me.btnOptions.Text = "Options"
        Me.btnOptions.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnOptions.UseVisualStyleBackColor = False
        '
        'btnMovePrev
        '
        Me.btnMovePrev.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.btnMovePrev.AutoSize = True
        Me.btnMovePrev.BackColor = System.Drawing.SystemColors.MenuBar
        Me.btnMovePrev.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnMovePrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMovePrev.ForeColor = System.Drawing.Color.Black
        Me.btnMovePrev.Image = CType(resources.GetObject("btnMovePrev.Image"), System.Drawing.Image)
        Me.btnMovePrev.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnMovePrev.Location = New System.Drawing.Point(0, 175)
        Me.btnMovePrev.Margin = New System.Windows.Forms.Padding(0)
        Me.btnMovePrev.Name = "btnMovePrev"
        Me.btnMovePrev.Size = New System.Drawing.Size(32, 35)
        Me.btnMovePrev.TabIndex = 25
        Me.btnMovePrev.Tag = "MovePrev"
        Me.btnMovePrev.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnMovePrev.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnMovePrev.UseVisualStyleBackColor = False
        '
        'btnMoveFirst
        '
        Me.btnMoveFirst.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.btnMoveFirst.AutoSize = True
        Me.btnMoveFirst.BackColor = System.Drawing.SystemColors.MenuBar
        Me.btnMoveFirst.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnMoveFirst.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMoveFirst.ForeColor = System.Drawing.Color.Black
        Me.btnMoveFirst.Image = CType(resources.GetObject("btnMoveFirst.Image"), System.Drawing.Image)
        Me.btnMoveFirst.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnMoveFirst.Location = New System.Drawing.Point(0, 210)
        Me.btnMoveFirst.Margin = New System.Windows.Forms.Padding(0)
        Me.btnMoveFirst.Name = "btnMoveFirst"
        Me.btnMoveFirst.Size = New System.Drawing.Size(33, 35)
        Me.btnMoveFirst.TabIndex = 24
        Me.btnMoveFirst.Tag = "MoveFirst"
        Me.btnMoveFirst.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnMoveFirst.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnMoveFirst.UseVisualStyleBackColor = False
        '
        'btnFind
        '
        Me.btnFind.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.btnFind.AutoSize = True
        Me.btnFind.BackColor = System.Drawing.SystemColors.MenuBar
        Me.btnFind.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnFind.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnFind.ForeColor = System.Drawing.Color.Black
        Me.btnFind.Image = CType(resources.GetObject("btnFind.Image"), System.Drawing.Image)
        Me.btnFind.Location = New System.Drawing.Point(0, 245)
        Me.btnFind.Margin = New System.Windows.Forms.Padding(0)
        Me.btnFind.Name = "btnFind"
        Me.btnFind.Size = New System.Drawing.Size(31, 35)
        Me.btnFind.TabIndex = 29
        Me.btnFind.Tag = "Find"
        Me.btnFind.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnFind.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnFind.UseVisualStyleBackColor = False
        '
        'btnMoveNext
        '
        Me.btnMoveNext.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.btnMoveNext.AutoSize = True
        Me.btnMoveNext.BackColor = System.Drawing.SystemColors.MenuBar
        Me.btnMoveNext.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnMoveNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMoveNext.ForeColor = System.Drawing.Color.Black
        Me.btnMoveNext.Image = CType(resources.GetObject("btnMoveNext.Image"), System.Drawing.Image)
        Me.btnMoveNext.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnMoveNext.Location = New System.Drawing.Point(0, 280)
        Me.btnMoveNext.Margin = New System.Windows.Forms.Padding(0)
        Me.btnMoveNext.Name = "btnMoveNext"
        Me.btnMoveNext.Size = New System.Drawing.Size(31, 35)
        Me.btnMoveNext.TabIndex = 26
        Me.btnMoveNext.Tag = "MoveNext"
        Me.btnMoveNext.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnMoveNext.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnMoveNext.UseVisualStyleBackColor = False
        '
        'btnMoveLast
        '
        Me.btnMoveLast.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.btnMoveLast.BackColor = System.Drawing.SystemColors.MenuBar
        Me.btnMoveLast.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnMoveLast.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMoveLast.ForeColor = System.Drawing.Color.Black
        Me.btnMoveLast.Image = CType(resources.GetObject("btnMoveLast.Image"), System.Drawing.Image)
        Me.btnMoveLast.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnMoveLast.Location = New System.Drawing.Point(0, 315)
        Me.btnMoveLast.Margin = New System.Windows.Forms.Padding(0)
        Me.btnMoveLast.Name = "btnMoveLast"
        Me.btnMoveLast.Size = New System.Drawing.Size(33, 35)
        Me.btnMoveLast.TabIndex = 27
        Me.btnMoveLast.Tag = "MoveLast"
        Me.btnMoveLast.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnMoveLast.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.btnSave.AutoSize = True
        Me.btnSave.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnSave.ForeColor = System.Drawing.Color.Black
        Me.btnSave.Image = CType(resources.GetObject("btnSave.Image"), System.Drawing.Image)
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(0, 350)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(0)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(64, 35)
        Me.btnSave.TabIndex = 30
        Me.btnSave.Tag = "Save"
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.btnCancel.AutoSize = True
        Me.btnCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(254, Byte), Integer), CType(CType(201, Byte), Integer), CType(CType(206, Byte), Integer))
        Me.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnCancel.ForeColor = System.Drawing.Color.Black
        Me.btnCancel.Image = CType(resources.GetObject("btnCancel.Image"), System.Drawing.Image)
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(0, 385)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(0)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(73, 35)
        Me.btnCancel.TabIndex = 31
        Me.btnCancel.Tag = "Cancel"
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnNew
        '
        Me.btnNew.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.btnNew.AutoSize = True
        Me.btnNew.BackColor = System.Drawing.SystemColors.MenuBar
        Me.btnNew.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnNew.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnNew.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnNew.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White
        Me.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNew.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnNew.ForeColor = System.Drawing.Color.Black
        Me.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnNew.Location = New System.Drawing.Point(110, 176)
        Me.btnNew.Margin = New System.Windows.Forms.Padding(0)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(61, 35)
        Me.btnNew.TabIndex = 19
        Me.btnNew.Tag = "New"
        Me.btnNew.Text = "&New"
        Me.btnNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnNew.UseVisualStyleBackColor = False
        '
        'tabSettings
        '
        Me.tabSettings.Alignment = System.Windows.Forms.TabAlignment.Left
        Me.tabSettings.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabSettings.Controls.Add(Me.tbBtnCtrl)
        Me.tabSettings.Controls.Add(Me.tbInvoice)
        Me.tabSettings.Controls.Add(Me.tbPurchase)
        Me.tabSettings.Font = New System.Drawing.Font("Consolas", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tabSettings.HotTrack = True
        Me.tabSettings.ItemSize = New System.Drawing.Size(50, 150)
        Me.tabSettings.Location = New System.Drawing.Point(13, 12)
        Me.tabSettings.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.tabSettings.Multiline = True
        Me.tabSettings.Name = "tabSettings"
        Me.tabSettings.SelectedIndex = 0
        Me.tabSettings.Size = New System.Drawing.Size(1146, 156)
        Me.tabSettings.SizeMode = System.Windows.Forms.TabSizeMode.Fixed
        Me.tabSettings.TabIndex = 18
        '
        'tbBtnCtrl
        '
        Me.tbBtnCtrl.BackColor = System.Drawing.Color.White
        Me.tbBtnCtrl.Controls.Add(Me.pnlBtnCtrlSettings)
        Me.tbBtnCtrl.Location = New System.Drawing.Point(154, 4)
        Me.tbBtnCtrl.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.tbBtnCtrl.Name = "tbBtnCtrl"
        Me.tbBtnCtrl.Padding = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.tbBtnCtrl.Size = New System.Drawing.Size(988, 148)
        Me.tbBtnCtrl.TabIndex = 0
        Me.tbBtnCtrl.Text = "Button Control"
        '
        'pnlBtnCtrlSettings
        '
        Me.pnlBtnCtrlSettings.BackColor = System.Drawing.Color.WhiteSmoke
        Me.pnlBtnCtrlSettings.Controls.Add(Me.svtBtnCtrl)
        Me.pnlBtnCtrlSettings.Controls.Add(Me.pnlLinksThemes)
        Me.pnlBtnCtrlSettings.Controls.Add(Me.rdoBtnColor)
        Me.pnlBtnCtrlSettings.Controls.Add(Me.rdoBtnTheme)
        Me.pnlBtnCtrlSettings.Controls.Add(Me.pnlLinks)
        Me.pnlBtnCtrlSettings.Controls.Add(Me.txtTheme)
        Me.pnlBtnCtrlSettings.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlBtnCtrlSettings.Location = New System.Drawing.Point(4, 3)
        Me.pnlBtnCtrlSettings.Name = "pnlBtnCtrlSettings"
        Me.pnlBtnCtrlSettings.Size = New System.Drawing.Size(980, 142)
        Me.pnlBtnCtrlSettings.TabIndex = 19
        '
        'svtBtnCtrl
        '
        Me.svtBtnCtrl.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.svtBtnCtrl.BackColor = System.Drawing.Color.WhiteSmoke
        Me.svtBtnCtrl.CausesValidation = False
        Me.svtBtnCtrl.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.svtBtnCtrl.LayoutStyle = vLQMS.svtModule.typeLayoutStyles.Horizontal
        Me.svtBtnCtrl.Location = New System.Drawing.Point(3, 4)
        Me.svtBtnCtrl.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.svtBtnCtrl.Name = "svtBtnCtrl"
        Me.svtBtnCtrl.Size = New System.Drawing.Size(971, 34)
        Me.svtBtnCtrl.TabIndex = 33
        '
        'pnlLinksThemes
        '
        Me.pnlLinksThemes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlLinksThemes.Controls.Add(Me.rdoTheme5)
        Me.pnlLinksThemes.Controls.Add(Me.rdoTheme4)
        Me.pnlLinksThemes.Controls.Add(Me.rdoTheme3)
        Me.pnlLinksThemes.Controls.Add(Me.rdoTheme2)
        Me.pnlLinksThemes.Controls.Add(Me.rdoTheme1)
        Me.pnlLinksThemes.Controls.Add(Me.rdoDefaultTheme)
        Me.pnlLinksThemes.Controls.Add(Me.lblHotKeys2)
        Me.pnlLinksThemes.Controls.Add(Me.lblForeColor2)
        Me.pnlLinksThemes.Controls.Add(Me.Label5)
        Me.pnlLinksThemes.Controls.Add(Me.Label6)
        Me.pnlLinksThemes.Location = New System.Drawing.Point(338, 50)
        Me.pnlLinksThemes.Name = "pnlLinksThemes"
        Me.pnlLinksThemes.Size = New System.Drawing.Size(636, 83)
        Me.pnlLinksThemes.TabIndex = 40
        '
        'rdoTheme5
        '
        Me.rdoTheme5.AutoSize = True
        Me.rdoTheme5.Location = New System.Drawing.Point(489, 10)
        Me.rdoTheme5.Name = "rdoTheme5"
        Me.rdoTheme5.Size = New System.Drawing.Size(90, 23)
        Me.rdoTheme5.TabIndex = 45
        Me.rdoTheme5.TabStop = True
        Me.rdoTheme5.Tag = "T5"
        Me.rdoTheme5.Text = "Theme 5"
        Me.rdoTheme5.UseVisualStyleBackColor = True
        '
        'rdoTheme4
        '
        Me.rdoTheme4.AutoSize = True
        Me.rdoTheme4.Location = New System.Drawing.Point(393, 10)
        Me.rdoTheme4.Name = "rdoTheme4"
        Me.rdoTheme4.Size = New System.Drawing.Size(90, 23)
        Me.rdoTheme4.TabIndex = 44
        Me.rdoTheme4.TabStop = True
        Me.rdoTheme4.Tag = "T4"
        Me.rdoTheme4.Text = "Theme 4"
        Me.rdoTheme4.UseVisualStyleBackColor = True
        '
        'rdoTheme3
        '
        Me.rdoTheme3.AutoSize = True
        Me.rdoTheme3.Location = New System.Drawing.Point(297, 10)
        Me.rdoTheme3.Name = "rdoTheme3"
        Me.rdoTheme3.Size = New System.Drawing.Size(90, 23)
        Me.rdoTheme3.TabIndex = 43
        Me.rdoTheme3.TabStop = True
        Me.rdoTheme3.Tag = "T3"
        Me.rdoTheme3.Text = "Theme 3"
        Me.rdoTheme3.UseVisualStyleBackColor = True
        '
        'rdoTheme2
        '
        Me.rdoTheme2.AutoSize = True
        Me.rdoTheme2.Location = New System.Drawing.Point(201, 10)
        Me.rdoTheme2.Name = "rdoTheme2"
        Me.rdoTheme2.Size = New System.Drawing.Size(90, 23)
        Me.rdoTheme2.TabIndex = 42
        Me.rdoTheme2.TabStop = True
        Me.rdoTheme2.Tag = "T2"
        Me.rdoTheme2.Text = "Theme 2"
        Me.rdoTheme2.UseVisualStyleBackColor = True
        '
        'rdoTheme1
        '
        Me.rdoTheme1.AutoSize = True
        Me.rdoTheme1.Location = New System.Drawing.Point(105, 10)
        Me.rdoTheme1.Name = "rdoTheme1"
        Me.rdoTheme1.Size = New System.Drawing.Size(90, 23)
        Me.rdoTheme1.TabIndex = 41
        Me.rdoTheme1.TabStop = True
        Me.rdoTheme1.Tag = "T1"
        Me.rdoTheme1.Text = "Theme 1"
        Me.rdoTheme1.UseVisualStyleBackColor = True
        '
        'rdoDefaultTheme
        '
        Me.rdoDefaultTheme.AutoSize = True
        Me.rdoDefaultTheme.Location = New System.Drawing.Point(9, 10)
        Me.rdoDefaultTheme.Name = "rdoDefaultTheme"
        Me.rdoDefaultTheme.Size = New System.Drawing.Size(90, 23)
        Me.rdoDefaultTheme.TabIndex = 40
        Me.rdoDefaultTheme.TabStop = True
        Me.rdoDefaultTheme.Tag = "D"
        Me.rdoDefaultTheme.Text = "Default"
        Me.rdoDefaultTheme.UseVisualStyleBackColor = True
        '
        'lblHotKeys2
        '
        Me.lblHotKeys2.AutoSize = True
        Me.lblHotKeys2.Location = New System.Drawing.Point(5, 50)
        Me.lblHotKeys2.Name = "lblHotKeys2"
        Me.lblHotKeys2.Size = New System.Drawing.Size(288, 19)
        Me.lblHotKeys2.TabIndex = 39
        Me.lblHotKeys2.TabStop = True
        Me.lblHotKeys2.Text = "Change button hotkeys / caption"
        '
        'lblForeColor2
        '
        Me.lblForeColor2.AutoSize = True
        Me.lblForeColor2.Location = New System.Drawing.Point(303, 50)
        Me.lblForeColor2.Name = "lblForeColor2"
        Me.lblForeColor2.Size = New System.Drawing.Size(216, 19)
        Me.lblForeColor2.TabIndex = 37
        Me.lblForeColor2.TabStop = True
        Me.lblForeColor2.Text = "Change button forecolor"
        '
        'Label5
        '
        Me.Label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label5.Location = New System.Drawing.Point(295, 43)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(2, 35)
        Me.Label5.TabIndex = 34
        '
        'Label6
        '
        Me.Label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label6.Location = New System.Drawing.Point(3, 39)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(580, 2)
        Me.Label6.TabIndex = 0
        '
        'rdoBtnColor
        '
        Me.rdoBtnColor.AutoSize = True
        Me.rdoBtnColor.Location = New System.Drawing.Point(125, 61)
        Me.rdoBtnColor.Name = "rdoBtnColor"
        Me.rdoBtnColor.Size = New System.Drawing.Size(180, 23)
        Me.rdoBtnColor.TabIndex = 19
        Me.rdoBtnColor.TabStop = True
        Me.rdoBtnColor.Text = "Use Button Colors"
        Me.rdoBtnColor.UseVisualStyleBackColor = True
        '
        'rdoBtnTheme
        '
        Me.rdoBtnTheme.AutoSize = True
        Me.rdoBtnTheme.Location = New System.Drawing.Point(125, 97)
        Me.rdoBtnTheme.Name = "rdoBtnTheme"
        Me.rdoBtnTheme.Size = New System.Drawing.Size(171, 23)
        Me.rdoBtnTheme.TabIndex = 20
        Me.rdoBtnTheme.TabStop = True
        Me.rdoBtnTheme.Text = "Use Button Theme"
        Me.rdoBtnTheme.UseVisualStyleBackColor = True
        '
        'pnlLinks
        '
        Me.pnlLinks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlLinks.Controls.Add(Me.Label4)
        Me.pnlLinks.Controls.Add(Me.lblChangeHotKeys)
        Me.pnlLinks.Controls.Add(Me.lblChangeBackClr)
        Me.pnlLinks.Controls.Add(Me.Label2)
        Me.pnlLinks.Controls.Add(Me.Label1)
        Me.pnlLinks.Controls.Add(Me.lblChangeForeClr)
        Me.pnlLinks.Location = New System.Drawing.Point(338, 50)
        Me.pnlLinks.Name = "pnlLinks"
        Me.pnlLinks.Size = New System.Drawing.Size(479, 83)
        Me.pnlLinks.TabIndex = 19
        '
        'Label4
        '
        Me.Label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label4.Location = New System.Drawing.Point(751, 4)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(2, 35)
        Me.Label4.TabIndex = 38
        '
        'lblChangeHotKeys
        '
        Me.lblChangeHotKeys.AutoSize = True
        Me.lblChangeHotKeys.Location = New System.Drawing.Point(100, 50)
        Me.lblChangeHotKeys.Name = "lblChangeHotKeys"
        Me.lblChangeHotKeys.Size = New System.Drawing.Size(288, 19)
        Me.lblChangeHotKeys.TabIndex = 39
        Me.lblChangeHotKeys.TabStop = True
        Me.lblChangeHotKeys.Text = "Change button hotkeys / caption"
        '
        'lblChangeBackClr
        '
        Me.lblChangeBackClr.AutoSize = True
        Me.lblChangeBackClr.Location = New System.Drawing.Point(252, 12)
        Me.lblChangeBackClr.Name = "lblChangeBackClr"
        Me.lblChangeBackClr.Size = New System.Drawing.Size(216, 19)
        Me.lblChangeBackClr.TabIndex = 37
        Me.lblChangeBackClr.TabStop = True
        Me.lblChangeBackClr.Text = "Change button backcolor"
        '
        'Label2
        '
        Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label2.Location = New System.Drawing.Point(233, 4)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(2, 35)
        Me.Label2.TabIndex = 34
        '
        'Label1
        '
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label1.Location = New System.Drawing.Point(3, 39)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(470, 2)
        Me.Label1.TabIndex = 0
        '
        'lblChangeForeClr
        '
        Me.lblChangeForeClr.AutoSize = True
        Me.lblChangeForeClr.Location = New System.Drawing.Point(3, 12)
        Me.lblChangeForeClr.Name = "lblChangeForeClr"
        Me.lblChangeForeClr.Size = New System.Drawing.Size(216, 19)
        Me.lblChangeForeClr.TabIndex = 33
        Me.lblChangeForeClr.TabStop = True
        Me.lblChangeForeClr.Text = "Change button forecolor"
        '
        'txtTheme
        '
        Me.txtTheme.Location = New System.Drawing.Point(738, 54)
        Me.txtTheme.Name = "txtTheme"
        Me.txtTheme.Size = New System.Drawing.Size(19, 26)
        Me.txtTheme.TabIndex = 19
        Me.txtTheme.Visible = False
        '
        'tbInvoice
        '
        Me.tbInvoice.BackColor = System.Drawing.Color.White
        Me.tbInvoice.Location = New System.Drawing.Point(154, 4)
        Me.tbInvoice.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.tbInvoice.Name = "tbInvoice"
        Me.tbInvoice.Padding = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.tbInvoice.Size = New System.Drawing.Size(988, 148)
        Me.tbInvoice.TabIndex = 1
        Me.tbInvoice.Text = "Invoice"
        '
        'tbPurchase
        '
        Me.tbPurchase.BackColor = System.Drawing.Color.White
        Me.tbPurchase.Location = New System.Drawing.Point(154, 4)
        Me.tbPurchase.Name = "tbPurchase"
        Me.tbPurchase.Size = New System.Drawing.Size(988, 148)
        Me.tbPurchase.TabIndex = 2
        Me.tbPurchase.Text = "Purchase"
        '
        'btnSettClose
        '
        Me.btnSettClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSettClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnSettClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSettClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSettClose.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSettClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSettClose.Location = New System.Drawing.Point(1079, 175)
        Me.btnSettClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSettClose.Name = "btnSettClose"
        Me.btnSettClose.Size = New System.Drawing.Size(80, 33)
        Me.btnSettClose.TabIndex = 16
        Me.btnSettClose.Text = "Cl&ose"
        Me.btnSettClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSettClose.UseVisualStyleBackColor = False
        '
        'btnSettSave
        '
        Me.btnSettSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSettSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSettSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSettSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSettSave.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSettSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSettSave.Location = New System.Drawing.Point(995, 175)
        Me.btnSettSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSettSave.Name = "btnSettSave"
        Me.btnSettSave.Size = New System.Drawing.Size(75, 33)
        Me.btnSettSave.TabIndex = 15
        Me.btnSettSave.Text = "&Save"
        Me.btnSettSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSettSave.UseVisualStyleBackColor = False
        '
        'frmUtlSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 18.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1172, 221)
        Me.Controls.Add(Me.btnNew)
        Me.Controls.Add(Me.tabSettings)
        Me.Controls.Add(Me.btnSettClose)
        Me.Controls.Add(Me.btnSettSave)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.flowBtnCtrls)
        Me.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MinimumSize = New System.Drawing.Size(996, 259)
        Me.Name = "frmUtlSettings"
        Me.ShowIcon = False
        Me.Text = "Settings..."
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.flowBtnCtrls.ResumeLayout(False)
        Me.flowBtnCtrls.PerformLayout()
        Me.tabSettings.ResumeLayout(False)
        Me.tbBtnCtrl.ResumeLayout(False)
        Me.pnlBtnCtrlSettings.ResumeLayout(False)
        Me.pnlBtnCtrlSettings.PerformLayout()
        Me.pnlLinksThemes.ResumeLayout(False)
        Me.pnlLinksThemes.PerformLayout()
        Me.pnlLinks.ResumeLayout(False)
        Me.pnlLinks.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnSettClose As C1.Win.C1Input.C1Button
    Friend WithEvents btnSettSave As C1.Win.C1Input.C1Button
    Friend WithEvents tabSettings As vLQMS.DotNetBarTabcontrol
    Friend WithEvents tbBtnCtrl As System.Windows.Forms.TabPage
    Friend WithEvents tbInvoice As System.Windows.Forms.TabPage
    Friend WithEvents tbPurchase As System.Windows.Forms.TabPage
    Friend WithEvents pnlBtnCtrlSettings As System.Windows.Forms.Panel
    Friend WithEvents flowBtnCtrls As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnEdit As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnOptions As C1.Win.C1Input.C1SplitButton
    Friend WithEvents btnMovePrev As System.Windows.Forms.Button
    Friend WithEvents btnMoveFirst As System.Windows.Forms.Button
    Friend WithEvents btnFind As System.Windows.Forms.Button
    Friend WithEvents btnMoveNext As System.Windows.Forms.Button
    Friend WithEvents btnMoveLast As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lblChangeForeClr As System.Windows.Forms.LinkLabel
    Friend WithEvents pnlLinks As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblChangeHotKeys As System.Windows.Forms.LinkLabel
    Friend WithEvents lblChangeBackClr As System.Windows.Forms.LinkLabel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cdg As System.Windows.Forms.ColorDialog
    Friend WithEvents tt As System.Windows.Forms.ToolTip
    Friend WithEvents rdoBtnColor As System.Windows.Forms.RadioButton
    Friend WithEvents rdoBtnTheme As System.Windows.Forms.RadioButton
    Friend WithEvents pnlLinksThemes As System.Windows.Forms.Panel
    Friend WithEvents lblHotKeys2 As System.Windows.Forms.LinkLabel
    Friend WithEvents lblForeColor2 As System.Windows.Forms.LinkLabel
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents rdoTheme3 As System.Windows.Forms.RadioButton
    Friend WithEvents rdoTheme2 As System.Windows.Forms.RadioButton
    Friend WithEvents rdoTheme1 As System.Windows.Forms.RadioButton
    Friend WithEvents rdoDefaultTheme As System.Windows.Forms.RadioButton
    Friend WithEvents rdoTheme4 As System.Windows.Forms.RadioButton
    Friend WithEvents txtTheme As System.Windows.Forms.TextBox
    Friend WithEvents svtBtnCtrl As vLQMS.btnCtrl
    Friend WithEvents rdoTheme5 As System.Windows.Forms.RadioButton
    Private WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
End Class
