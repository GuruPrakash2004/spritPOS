<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMstSCC
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMstSCC))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblFrm = New System.Windows.Forms.Label()
        Me.pnlSearch = New System.Windows.Forms.Panel()
        Me.lblSearchImage = New System.Windows.Forms.Label()
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.txtID = New System.Windows.Forms.TextBox()
        Me.vsfHdr = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.txtCode = New System.Windows.Forms.TextBox()
        Me.lblCode = New System.Windows.Forms.Label()
        Me.chkInactive = New System.Windows.Forms.CheckBox()
        Me.vsfSearch = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.tt = New System.Windows.Forms.ToolTip(Me.components)
        Me.ColorDialog = New System.Windows.Forms.ColorDialog()
        Me.cboCountry = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cboState = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtInActiveRem = New System.Windows.Forms.TextBox()
        Me.lblApproved = New System.Windows.Forms.Label()
        Me.btnMstState = New System.Windows.Forms.Button()
        Me.svtBtnCtrl = New vLQMS.btnCtrl()
        Me.svtAttach = New vLQMS.svtFileCtrl()
        Me.Panel1.SuspendLayout()
        Me.pnlSearch.SuspendLayout()
        CType(Me.vsfHdr, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(53, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(106, Byte), Integer))
        Me.Panel1.Controls.Add(Me.lblFrm)
        Me.Panel1.Controls.Add(Me.pnlSearch)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(835, 35)
        Me.Panel1.TabIndex = 0
        '
        'lblFrm
        '
        Me.lblFrm.AutoSize = True
        Me.lblFrm.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lblFrm.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFrm.ForeColor = System.Drawing.Color.White
        Me.lblFrm.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblFrm.Location = New System.Drawing.Point(21, 6)
        Me.lblFrm.Name = "lblFrm"
        Me.lblFrm.Size = New System.Drawing.Size(0, 25)
        Me.lblFrm.TabIndex = 2
        '
        'pnlSearch
        '
        Me.pnlSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.pnlSearch.Controls.Add(Me.lblSearchImage)
        Me.pnlSearch.Controls.Add(Me.lblSearch)
        Me.pnlSearch.Controls.Add(Me.txtSearch)
        Me.pnlSearch.Location = New System.Drawing.Point(527, 3)
        Me.pnlSearch.Name = "pnlSearch"
        Me.pnlSearch.Size = New System.Drawing.Size(304, 30)
        Me.pnlSearch.TabIndex = 0
        '
        'lblSearchImage
        '
        Me.lblSearchImage.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSearchImage.BackColor = System.Drawing.Color.White
        Me.lblSearchImage.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSearchImage.ForeColor = System.Drawing.Color.Transparent
        Me.lblSearchImage.Image = Global.vLQMS.My.Resources.Resources.D_FIND
        Me.lblSearchImage.Location = New System.Drawing.Point(228, 5)
        Me.lblSearchImage.Name = "lblSearchImage"
        Me.lblSearchImage.Size = New System.Drawing.Size(34, 18)
        Me.lblSearchImage.TabIndex = 15
        '
        'lblSearch
        '
        Me.lblSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSearch.AutoSize = True
        Me.lblSearch.BackColor = System.Drawing.Color.Transparent
        Me.lblSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSearch.ForeColor = System.Drawing.Color.White
        Me.lblSearch.Location = New System.Drawing.Point(19, 3)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(66, 20)
        Me.lblSearch.TabIndex = 0
        Me.lblSearch.Text = "Search"
        '
        'txtSearch
        '
        Me.txtSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSearch.BackColor = System.Drawing.Color.White
        Me.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSearch.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSearch.Location = New System.Drawing.Point(92, 3)
        Me.txtSearch.Margin = New System.Windows.Forms.Padding(4)
        Me.txtSearch.MaxLength = 50
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(170, 26)
        Me.txtSearch.TabIndex = 0
        '
        'txtID
        '
        Me.txtID.BackColor = System.Drawing.Color.White
        Me.txtID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtID.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtID.ForeColor = System.Drawing.Color.Black
        Me.txtID.Location = New System.Drawing.Point(566, 125)
        Me.txtID.Margin = New System.Windows.Forms.Padding(4)
        Me.txtID.MaxLength = 250
        Me.txtID.Multiline = True
        Me.txtID.Name = "txtID"
        Me.txtID.Size = New System.Drawing.Size(22, 23)
        Me.txtID.TabIndex = 82
        Me.txtID.Visible = False
        '
        'vsfHdr
        '
        Me.vsfHdr.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfHdr.BackColor = System.Drawing.Color.White
        Me.vsfHdr.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.vsfHdr.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None
        Me.vsfHdr.ColumnInfo = "2,1,0,0,0,100,Columns:"
        Me.vsfHdr.ExtendLastCol = True
        Me.vsfHdr.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.vsfHdr.ForeColor = System.Drawing.Color.Black
        Me.vsfHdr.Location = New System.Drawing.Point(565, 54)
        Me.vsfHdr.Name = "vsfHdr"
        Me.vsfHdr.Rows.Count = 3
        Me.vsfHdr.Rows.DefaultSize = 20
        Me.vsfHdr.Rows.Fixed = 0
        Me.vsfHdr.Size = New System.Drawing.Size(258, 367)
        Me.vsfHdr.StyleInfo = resources.GetString("vsfHdr.StyleInfo")
        Me.vsfHdr.TabIndex = 56
        Me.vsfHdr.Tag = "DSGR"
        '
        'txtCode
        '
        Me.txtCode.BackColor = System.Drawing.Color.White
        Me.txtCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCode.Enabled = False
        Me.txtCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCode.ForeColor = System.Drawing.Color.Black
        Me.txtCode.Location = New System.Drawing.Point(134, 53)
        Me.txtCode.Margin = New System.Windows.Forms.Padding(4)
        Me.txtCode.MaxLength = 20
        Me.txtCode.Multiline = True
        Me.txtCode.Name = "txtCode"
        Me.txtCode.Size = New System.Drawing.Size(295, 28)
        Me.txtCode.TabIndex = 1
        '
        'lblCode
        '
        Me.lblCode.AutoSize = True
        Me.lblCode.BackColor = System.Drawing.Color.Transparent
        Me.lblCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCode.ForeColor = System.Drawing.Color.Red
        Me.lblCode.Location = New System.Drawing.Point(8, 56)
        Me.lblCode.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCode.Name = "lblCode"
        Me.lblCode.Size = New System.Drawing.Size(118, 20)
        Me.lblCode.TabIndex = 85
        Me.lblCode.Tag = "M"
        Me.lblCode.Text = "Billing Category"
        '
        'chkInactive
        '
        Me.chkInactive.AutoSize = True
        Me.chkInactive.BackColor = System.Drawing.Color.Transparent
        Me.chkInactive.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkInactive.ForeColor = System.Drawing.Color.Black
        Me.chkInactive.Location = New System.Drawing.Point(134, 155)
        Me.chkInactive.Margin = New System.Windows.Forms.Padding(4)
        Me.chkInactive.Name = "chkInactive"
        Me.chkInactive.Size = New System.Drawing.Size(83, 24)
        Me.chkInactive.TabIndex = 5
        Me.chkInactive.Text = "Inactive"
        Me.chkInactive.UseVisualStyleBackColor = False
        '
        'vsfSearch
        '
        Me.vsfSearch.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfSearch.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfSearch.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfSearch.ExtendLastCol = True
        Me.vsfSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.vsfSearch.Location = New System.Drawing.Point(566, 52)
        Me.vsfSearch.Margin = New System.Windows.Forms.Padding(4)
        Me.vsfSearch.Name = "vsfSearch"
        Me.vsfSearch.Rows.DefaultSize = 24
        Me.vsfSearch.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.RowRange
        Me.vsfSearch.Size = New System.Drawing.Size(256, 369)
        Me.vsfSearch.TabIndex = 50
        Me.vsfSearch.Visible = False
        '
        'Panel3
        '
        Me.Panel3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.ForeColor = System.Drawing.Color.Black
        Me.Panel3.Location = New System.Drawing.Point(0, 427)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(835, 4)
        Me.Panel3.TabIndex = 60
        '
        'Panel4
        '
        Me.Panel4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Location = New System.Drawing.Point(542, 6)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(2, 420)
        Me.Panel4.TabIndex = 55
        '
        'cboCountry
        '
        Me.cboCountry.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cboCountry.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCountry.FormattingEnabled = True
        Me.cboCountry.Items.AddRange(New Object() {"INDIA"})
        Me.cboCountry.Location = New System.Drawing.Point(134, 121)
        Me.cboCountry.Margin = New System.Windows.Forms.Padding(4)
        Me.cboCountry.MaxLength = 100
        Me.cboCountry.Name = "cboCountry"
        Me.cboCountry.Size = New System.Drawing.Size(295, 28)
        Me.cboCountry.TabIndex = 3
        Me.cboCountry.Tag = "Country"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Black
        Me.Label4.Location = New System.Drawing.Point(8, 125)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(64, 20)
        Me.Label4.TabIndex = 284
        Me.Label4.Text = "Country"
        '
        'cboState
        '
        Me.cboState.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cboState.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboState.FormattingEnabled = True
        Me.cboState.Location = New System.Drawing.Point(134, 87)
        Me.cboState.Margin = New System.Windows.Forms.Padding(4)
        Me.cboState.MaxLength = 100
        Me.cboState.Name = "cboState"
        Me.cboState.Size = New System.Drawing.Size(295, 28)
        Me.cboState.TabIndex = 2
        Me.cboState.Tag = "State"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Red
        Me.Label5.Location = New System.Drawing.Point(8, 93)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(48, 20)
        Me.Label5.TabIndex = 283
        Me.Label5.Tag = "M"
        Me.Label5.Text = "State"
        '
        'txtInActiveRem
        '
        Me.txtInActiveRem.BackColor = System.Drawing.Color.White
        Me.txtInActiveRem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtInActiveRem.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInActiveRem.ForeColor = System.Drawing.Color.Black
        Me.txtInActiveRem.Location = New System.Drawing.Point(134, 185)
        Me.txtInActiveRem.Margin = New System.Windows.Forms.Padding(4)
        Me.txtInActiveRem.MaxLength = 250
        Me.txtInActiveRem.Multiline = True
        Me.txtInActiveRem.Name = "txtInActiveRem"
        Me.txtInActiveRem.Size = New System.Drawing.Size(301, 64)
        Me.txtInActiveRem.TabIndex = 6
        Me.txtInActiveRem.Tag = "InActiveRem"
        '
        'lblApproved
        '
        Me.lblApproved.BackColor = System.Drawing.Color.Transparent
        Me.lblApproved.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblApproved.ForeColor = System.Drawing.Color.Green
        Me.lblApproved.Location = New System.Drawing.Point(433, 52)
        Me.lblApproved.Name = "lblApproved"
        Me.lblApproved.Size = New System.Drawing.Size(107, 29)
        Me.lblApproved.TabIndex = 463
        Me.lblApproved.Text = "APPROVED"
        Me.lblApproved.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnMstState
        '
        Me.btnMstState.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMstState.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnMstState.ForeColor = System.Drawing.Color.Black
        Me.btnMstState.Location = New System.Drawing.Point(101, 93)
        Me.btnMstState.Name = "btnMstState"
        Me.btnMstState.Size = New System.Drawing.Size(26, 23)
        Me.btnMstState.TabIndex = 466
        Me.btnMstState.Text = "..."
        Me.btnMstState.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnMstState.UseVisualStyleBackColor = True
        '
        'svtBtnCtrl
        '
        Me.svtBtnCtrl.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.svtBtnCtrl.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.svtBtnCtrl.CausesValidation = False
        Me.svtBtnCtrl.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.svtBtnCtrl.LayoutStyle = vLQMS.svtModule.typeLayoutStyles.Horizontal
        Me.svtBtnCtrl.Location = New System.Drawing.Point(-5, 438)
        Me.svtBtnCtrl.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.svtBtnCtrl.Name = "svtBtnCtrl"
        Me.svtBtnCtrl.Size = New System.Drawing.Size(948, 42)
        Me.svtBtnCtrl.TabIndex = 8
        '
        'svtAttach
        '
        Me.svtAttach.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.svtAttach.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.svtAttach.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.svtAttach.Location = New System.Drawing.Point(25, 293)
        Me.svtAttach.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.svtAttach.MaximumSize = New System.Drawing.Size(254, 110)
        Me.svtAttach.MinimumSize = New System.Drawing.Size(254, 110)
        Me.svtAttach.Name = "svtAttach"
        Me.svtAttach.ShowPrintSelection = False
        Me.svtAttach.Size = New System.Drawing.Size(254, 110)
        Me.svtAttach.TabIndex = 7
        Me.svtAttach.ViewMode = False
        '
        'frmMstSCC
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(835, 515)
        Me.Controls.Add(Me.btnMstState)
        Me.Controls.Add(Me.lblApproved)
        Me.Controls.Add(Me.txtInActiveRem)
        Me.Controls.Add(Me.cboCountry)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.cboState)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.svtBtnCtrl)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.svtAttach)
        Me.Controls.Add(Me.chkInactive)
        Me.Controls.Add(Me.txtCode)
        Me.Controls.Add(Me.lblCode)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.txtID)
        Me.Controls.Add(Me.vsfSearch)
        Me.Controls.Add(Me.vsfHdr)
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(832, 392)
        Me.Name = "frmMstSCC"
        Me.ShowIcon = False
        Me.Text = "Billing Category Master"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.pnlSearch.ResumeLayout(False)
        Me.pnlSearch.PerformLayout()
        CType(Me.vsfHdr, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents lblSearch As System.Windows.Forms.Label
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents pnlSearch As System.Windows.Forms.Panel
    Friend WithEvents txtID As System.Windows.Forms.TextBox
    Friend WithEvents vsfHdr As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents txtCode As System.Windows.Forms.TextBox
    Friend WithEvents lblCode As System.Windows.Forms.Label
    Friend WithEvents chkInactive As System.Windows.Forms.CheckBox
    Friend WithEvents svtAttach As vLQMS.svtFileCtrl
    Friend WithEvents vsfSearch As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents tt As System.Windows.Forms.ToolTip
    Friend WithEvents ColorDialog As System.Windows.Forms.ColorDialog
    Friend WithEvents svtBtnCtrl As vLQMS.btnCtrl
    Friend WithEvents cboCountry As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cboState As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtInActiveRem As TextBox
    Public WithEvents lblFrm As Label
    Friend WithEvents lblSearchImage As Label
    Friend WithEvents lblApproved As Label
    Friend WithEvents btnMstState As Button
End Class
