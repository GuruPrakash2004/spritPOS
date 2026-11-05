<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlRecAuditLog
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlRecAuditLog))
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.cboMaster = New System.Windows.Forms.ComboBox()
        Me.lblCaption = New System.Windows.Forms.Label()
        Me.btnView = New System.Windows.Forms.Button()
        Me.pnlDate = New System.Windows.Forms.Panel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.dtpToDate = New System.Windows.Forms.DateTimePicker()
        Me.dtpFromDate = New System.Windows.Forms.DateTimePicker()
        Me.chkDate = New System.Windows.Forms.CheckBox()
        Me.vsfRec = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnExcel = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtNoOfInv = New System.Windows.Forms.TextBox()
        Me.btnFreeze = New System.Windows.Forms.Button()
        Me.pnlDet = New System.Windows.Forms.Panel()
        Me.vsfChild2 = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnCloseAddItems = New System.Windows.Forms.Button()
        Me.vsfChild = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.chkTrans = New System.Windows.Forms.CheckBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnCancel = New System.Windows.Forms.Button()
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
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlDate.SuspendLayout()
        CType(Me.vsfRec, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlDet.SuspendLayout()
        CType(Me.vsfChild2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfChild, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlCopyShortCut.SuspendLayout()
        Me.SuspendLayout()
        '
        'vsfDet
        '
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
        Me.vsfDet.ColumnInfo = "10,0,0,0,0,120,Columns:4{Width:197;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfDet.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(8, 187)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(827, 251)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 4
        Me.vsfDet.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2007Blue
        '
        'cboMaster
        '
        Me.cboMaster.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cboMaster.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboMaster.FormattingEnabled = True
        Me.cboMaster.Location = New System.Drawing.Point(142, 40)
        Me.cboMaster.Margin = New System.Windows.Forms.Padding(5)
        Me.cboMaster.MaxLength = 100
        Me.cboMaster.Name = "cboMaster"
        Me.cboMaster.Size = New System.Drawing.Size(279, 28)
        Me.cboMaster.TabIndex = 1
        '
        'lblCaption
        '
        Me.lblCaption.AutoSize = True
        Me.lblCaption.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCaption.ForeColor = System.Drawing.Color.Red
        Me.lblCaption.Location = New System.Drawing.Point(12, 43)
        Me.lblCaption.Name = "lblCaption"
        Me.lblCaption.Size = New System.Drawing.Size(66, 20)
        Me.lblCaption.TabIndex = 285
        Me.lblCaption.Tag = "M"
        Me.lblCaption.Text = "Masters"
        '
        'btnView
        '
        Me.btnView.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnView.Location = New System.Drawing.Point(361, 94)
        Me.btnView.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(92, 38)
        Me.btnView.TabIndex = 3
        Me.btnView.Text = "&Load"
        Me.btnView.UseVisualStyleBackColor = False
        '
        'pnlDate
        '
        Me.pnlDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlDate.Controls.Add(Me.Label4)
        Me.pnlDate.Controls.Add(Me.Label3)
        Me.pnlDate.Controls.Add(Me.dtpToDate)
        Me.pnlDate.Controls.Add(Me.dtpFromDate)
        Me.pnlDate.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlDate.Location = New System.Drawing.Point(8, 94)
        Me.pnlDate.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.pnlDate.Name = "pnlDate"
        Me.pnlDate.Size = New System.Drawing.Size(347, 56)
        Me.pnlDate.TabIndex = 288
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(182, 21)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(27, 20)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "To"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(3, 20)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(46, 20)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "From"
        '
        'dtpToDate
        '
        Me.dtpToDate.CustomFormat = "dd/MM/yyyy"
        Me.dtpToDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpToDate.Location = New System.Drawing.Point(216, 16)
        Me.dtpToDate.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.dtpToDate.Name = "dtpToDate"
        Me.dtpToDate.Size = New System.Drawing.Size(121, 26)
        Me.dtpToDate.TabIndex = 1
        '
        'dtpFromDate
        '
        Me.dtpFromDate.CustomFormat = "dd/MM/yyyy"
        Me.dtpFromDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpFromDate.Location = New System.Drawing.Point(55, 16)
        Me.dtpFromDate.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.dtpFromDate.Name = "dtpFromDate"
        Me.dtpFromDate.Size = New System.Drawing.Size(119, 26)
        Me.dtpFromDate.TabIndex = 0
        '
        'chkDate
        '
        Me.chkDate.AutoSize = True
        Me.chkDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDate.Location = New System.Drawing.Point(14, 68)
        Me.chkDate.Margin = New System.Windows.Forms.Padding(5)
        Me.chkDate.Name = "chkDate"
        Me.chkDate.Size = New System.Drawing.Size(115, 24)
        Me.chkDate.TabIndex = 2
        Me.chkDate.Text = "Date Range"
        Me.chkDate.UseVisualStyleBackColor = True
        '
        'vsfRec
        '
        Me.vsfRec.AllowFiltering = True
        Me.vsfRec.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfRec.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfRec.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfRec.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfRec.AutoResize = True
        Me.vsfRec.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfRec.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfRec.ColumnInfo = "10,0,0,0,0,120,Columns:4{Width:197;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfRec.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfRec.ExtendLastCol = True
        Me.vsfRec.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfRec.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfRec.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfRec.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfRec.Location = New System.Drawing.Point(491, 3)
        Me.vsfRec.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfRec.Name = "vsfRec"
        Me.vsfRec.Rows.DefaultSize = 24
        Me.vsfRec.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfRec.Size = New System.Drawing.Size(344, 129)
        Me.vsfRec.StyleInfo = resources.GetString("vsfRec.StyleInfo")
        Me.vsfRec.TabIndex = 4
        Me.vsfRec.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2007Blue
        '
        'btnExcel
        '
        Me.btnExcel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExcel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExcel.Location = New System.Drawing.Point(8, 444)
        Me.btnExcel.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnExcel.Name = "btnExcel"
        Me.btnExcel.Size = New System.Drawing.Size(138, 30)
        Me.btnExcel.TabIndex = 5
        Me.btnExcel.Text = "&Export to Excel"
        Me.ToolTip1.SetToolTip(Me.btnExcel, "Export to Excel:Alt+E")
        Me.btnExcel.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(5, 155)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(252, 20)
        Me.Label2.TabIndex = 291
        Me.Label2.Text = "Total Number of Records Loaded :"
        '
        'txtNoOfInv
        '
        Me.txtNoOfInv.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtNoOfInv.Enabled = False
        Me.txtNoOfInv.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNoOfInv.Location = New System.Drawing.Point(263, 153)
        Me.txtNoOfInv.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtNoOfInv.Name = "txtNoOfInv"
        Me.txtNoOfInv.Size = New System.Drawing.Size(68, 26)
        Me.txtNoOfInv.TabIndex = 290
        '
        'btnFreeze
        '
        Me.btnFreeze.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnFreeze.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFreeze.Location = New System.Drawing.Point(152, 444)
        Me.btnFreeze.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnFreeze.Name = "btnFreeze"
        Me.btnFreeze.Size = New System.Drawing.Size(132, 30)
        Me.btnFreeze.TabIndex = 6
        Me.btnFreeze.Text = "&Freeze"
        Me.ToolTip1.SetToolTip(Me.btnFreeze, "Freeze:Alt+F")
        Me.btnFreeze.UseVisualStyleBackColor = True
        '
        'pnlDet
        '
        Me.pnlDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlDet.Controls.Add(Me.vsfChild2)
        Me.pnlDet.Controls.Add(Me.btnCloseAddItems)
        Me.pnlDet.Controls.Add(Me.vsfChild)
        Me.pnlDet.Location = New System.Drawing.Point(34, 73)
        Me.pnlDet.Name = "pnlDet"
        Me.pnlDet.Size = New System.Drawing.Size(775, 368)
        Me.pnlDet.TabIndex = 2
        Me.pnlDet.Visible = False
        '
        'vsfChild2
        '
        Me.vsfChild2.AllowFiltering = True
        Me.vsfChild2.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfChild2.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfChild2.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfChild2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfChild2.AutoResize = True
        Me.vsfChild2.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfChild2.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfChild2.ColumnInfo = "10,0,0,0,0,120,Columns:4{Width:197;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfChild2.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfChild2.ExtendLastCol = True
        Me.vsfChild2.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfChild2.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfChild2.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfChild2.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfChild2.Location = New System.Drawing.Point(4, 167)
        Me.vsfChild2.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfChild2.Name = "vsfChild2"
        Me.vsfChild2.Rows.DefaultSize = 24
        Me.vsfChild2.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfChild2.Size = New System.Drawing.Size(768, 195)
        Me.vsfChild2.StyleInfo = resources.GetString("vsfChild2.StyleInfo")
        Me.vsfChild2.TabIndex = 0
        Me.vsfChild2.Visible = False
        Me.vsfChild2.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2007Blue
        '
        'btnCloseAddItems
        '
        Me.btnCloseAddItems.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCloseAddItems.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnCloseAddItems.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnCloseAddItems.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCloseAddItems.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnCloseAddItems.Image = CType(resources.GetObject("btnCloseAddItems.Image"), System.Drawing.Image)
        Me.btnCloseAddItems.Location = New System.Drawing.Point(746, 4)
        Me.btnCloseAddItems.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnCloseAddItems.Name = "btnCloseAddItems"
        Me.btnCloseAddItems.Size = New System.Drawing.Size(26, 26)
        Me.btnCloseAddItems.TabIndex = 0
        Me.btnCloseAddItems.UseVisualStyleBackColor = False
        '
        'vsfChild
        '
        Me.vsfChild.AllowFiltering = True
        Me.vsfChild.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfChild.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfChild.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfChild.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfChild.AutoResize = True
        Me.vsfChild.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfChild.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfChild.ColumnInfo = "10,0,0,0,0,120,Columns:4{Width:197;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfChild.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfChild.ExtendLastCol = True
        Me.vsfChild.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfChild.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfChild.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfChild.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfChild.Location = New System.Drawing.Point(0, 38)
        Me.vsfChild.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfChild.Name = "vsfChild"
        Me.vsfChild.Rows.DefaultSize = 24
        Me.vsfChild.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfChild.Size = New System.Drawing.Size(768, 124)
        Me.vsfChild.StyleInfo = resources.GetString("vsfChild.StyleInfo")
        Me.vsfChild.TabIndex = 231
        Me.vsfChild.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2007Blue
        '
        'chkTrans
        '
        Me.chkTrans.AutoSize = True
        Me.chkTrans.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTrans.Location = New System.Drawing.Point(263, 12)
        Me.chkTrans.Name = "chkTrans"
        Me.chkTrans.Size = New System.Drawing.Size(111, 24)
        Me.chkTrans.TabIndex = 0
        Me.chkTrans.Text = "Transaction"
        Me.chkTrans.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(750, 444)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(87, 30)
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Text = " &Close"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Close:Alt+C")
        Me.btnCancel.UseVisualStyleBackColor = True
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
        Me.pnlCopyShortCut.Location = New System.Drawing.Point(473, 24)
        Me.pnlCopyShortCut.Name = "pnlCopyShortCut"
        Me.pnlCopyShortCut.Size = New System.Drawing.Size(329, 177)
        Me.pnlCopyShortCut.TabIndex = 299
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
        'frmUtlRecAuditLog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(839, 477)
        Me.Controls.Add(Me.pnlCopyShortCut)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.chkTrans)
        Me.Controls.Add(Me.btnFreeze)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtNoOfInv)
        Me.Controls.Add(Me.btnExcel)
        Me.Controls.Add(Me.vsfRec)
        Me.Controls.Add(Me.chkDate)
        Me.Controls.Add(Me.pnlDate)
        Me.Controls.Add(Me.btnView)
        Me.Controls.Add(Me.cboMaster)
        Me.Controls.Add(Me.lblCaption)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.pnlDet)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "frmUtlRecAuditLog"
        Me.ShowIcon = False
        Me.Text = "1"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlDate.ResumeLayout(False)
        Me.pnlDate.PerformLayout()
        CType(Me.vsfRec, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlDet.ResumeLayout(False)
        CType(Me.vsfChild2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfChild, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlCopyShortCut.ResumeLayout(False)
        Me.pnlCopyShortCut.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents cboMaster As ComboBox
    Friend WithEvents lblCaption As Label
    Friend WithEvents btnView As Button
    Friend WithEvents pnlDate As Panel
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents dtpToDate As DateTimePicker
    Friend WithEvents dtpFromDate As DateTimePicker
    Friend WithEvents chkDate As CheckBox
    Friend WithEvents vsfRec As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnExcel As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents txtNoOfInv As TextBox
    Friend WithEvents btnFreeze As Button
    Friend WithEvents pnlDet As Panel
    Friend WithEvents btnCloseAddItems As Button
    Friend WithEvents vsfChild2 As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents vsfChild As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents chkTrans As CheckBox
    Friend WithEvents ToolTip1 As ToolTip
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
