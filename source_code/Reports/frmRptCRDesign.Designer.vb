<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRptCRDesign
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRptCRDesign))
        Me.optMasters = New System.Windows.Forms.RadioButton()
        Me.optTrnHdr = New System.Windows.Forms.RadioButton()
        Me.optExisting = New System.Windows.Forms.RadioButton()
        Me.fraSel = New System.Windows.Forms.Panel()
        Me.vsfExp = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.pnlExp = New System.Windows.Forms.Panel()
        Me.btnExpClose = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.txtFileName = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.txtView = New System.Windows.Forms.TextBox()
        Me.lblCreateView = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cboSel = New System.Windows.Forms.ComboBox()
        Me.lblSel = New System.Windows.Forms.Label()
        Me.optAdhoc = New System.Windows.Forms.RadioButton()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnHide = New System.Windows.Forms.Button()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnNext = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.fraSel.SuspendLayout()
        CType(Me.vsfExp, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlExp.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'optMasters
        '
        Me.optMasters.AutoSize = True
        Me.optMasters.Checked = True
        Me.optMasters.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optMasters.Location = New System.Drawing.Point(13, 12)
        Me.optMasters.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.optMasters.Name = "optMasters"
        Me.optMasters.Size = New System.Drawing.Size(74, 21)
        Me.optMasters.TabIndex = 0
        Me.optMasters.TabStop = True
        Me.optMasters.Text = "Masters"
        Me.optMasters.UseVisualStyleBackColor = True
        '
        'optTrnHdr
        '
        Me.optTrnHdr.AutoSize = True
        Me.optTrnHdr.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optTrnHdr.Location = New System.Drawing.Point(143, 12)
        Me.optTrnHdr.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.optTrnHdr.Name = "optTrnHdr"
        Me.optTrnHdr.Size = New System.Drawing.Size(104, 21)
        Me.optTrnHdr.TabIndex = 1
        Me.optTrnHdr.Text = "Transactions"
        Me.optTrnHdr.UseVisualStyleBackColor = True
        '
        'optExisting
        '
        Me.optExisting.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.optExisting.AutoSize = True
        Me.optExisting.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optExisting.Location = New System.Drawing.Point(465, 12)
        Me.optExisting.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.optExisting.Name = "optExisting"
        Me.optExisting.Size = New System.Drawing.Size(75, 21)
        Me.optExisting.TabIndex = 2
        Me.optExisting.Text = "Existing"
        Me.optExisting.UseVisualStyleBackColor = True
        '
        'fraSel
        '
        Me.fraSel.BackColor = System.Drawing.Color.LightGray
        Me.fraSel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.fraSel.Controls.Add(Me.vsfExp)
        Me.fraSel.Controls.Add(Me.pnlExp)
        Me.fraSel.Controls.Add(Me.btnLoad)
        Me.fraSel.Controls.Add(Me.txtView)
        Me.fraSel.Controls.Add(Me.lblCreateView)
        Me.fraSel.Controls.Add(Me.Label2)
        Me.fraSel.Controls.Add(Me.cboSel)
        Me.fraSel.Controls.Add(Me.lblSel)
        Me.fraSel.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.fraSel.Location = New System.Drawing.Point(11, 103)
        Me.fraSel.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.fraSel.Name = "fraSel"
        Me.fraSel.Size = New System.Drawing.Size(556, 295)
        Me.fraSel.TabIndex = 3
        '
        'vsfExp
        '
        Me.vsfExp.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfExp.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfExp.Location = New System.Drawing.Point(67, 80)
        Me.vsfExp.Name = "vsfExp"
        Me.vsfExp.Rows.DefaultSize = 24
        Me.vsfExp.Rows.Fixed = 0
        Me.vsfExp.Size = New System.Drawing.Size(420, 132)
        Me.vsfExp.StyleInfo = resources.GetString("vsfExp.StyleInfo")
        Me.vsfExp.TabIndex = 15
        Me.vsfExp.Visible = False
        '
        'pnlExp
        '
        Me.pnlExp.Controls.Add(Me.btnExpClose)
        Me.pnlExp.Controls.Add(Me.btnSave)
        Me.pnlExp.Controls.Add(Me.txtFileName)
        Me.pnlExp.Controls.Add(Me.Label3)
        Me.pnlExp.Location = New System.Drawing.Point(77, 130)
        Me.pnlExp.Name = "pnlExp"
        Me.pnlExp.Size = New System.Drawing.Size(154, 145)
        Me.pnlExp.TabIndex = 14
        Me.pnlExp.Visible = False
        '
        'btnExpClose
        '
        Me.btnExpClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExpClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnExpClose.Image = CType(resources.GetObject("btnExpClose.Image"), System.Drawing.Image)
        Me.btnExpClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExpClose.Location = New System.Drawing.Point(65, 111)
        Me.btnExpClose.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnExpClose.Name = "btnExpClose"
        Me.btnExpClose.Size = New System.Drawing.Size(74, 30)
        Me.btnExpClose.TabIndex = 11
        Me.btnExpClose.Text = "&Cancel"
        Me.btnExpClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnExpClose, "Cancel : Alt+C")
        Me.btnExpClose.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(388, 111)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(74, 30)
        Me.btnSave.TabIndex = 10
        Me.btnSave.Text = "&Save"
        Me.ToolTip1.SetToolTip(Me.btnSave, "Save : Alt+S")
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'txtFileName
        '
        Me.txtFileName.Location = New System.Drawing.Point(87, 59)
        Me.txtFileName.MaxLength = 50
        Me.txtFileName.Name = "txtFileName"
        Me.txtFileName.Size = New System.Drawing.Size(452, 25)
        Me.txtFileName.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(15, 62)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(66, 17)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "File Name"
        '
        'btnLoad
        '
        Me.btnLoad.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoad.Location = New System.Drawing.Point(375, 65)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(55, 29)
        Me.btnLoad.TabIndex = 12
        Me.btnLoad.Text = "&Load"
        Me.ToolTip1.SetToolTip(Me.btnLoad, "Load:Alt+L")
        Me.btnLoad.UseVisualStyleBackColor = True
        Me.btnLoad.Visible = False
        '
        'txtView
        '
        Me.txtView.Location = New System.Drawing.Point(6, 117)
        Me.txtView.Multiline = True
        Me.txtView.Name = "txtView"
        Me.txtView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtView.Size = New System.Drawing.Size(534, 173)
        Me.txtView.TabIndex = 11
        Me.txtView.Visible = False
        '
        'lblCreateView
        '
        Me.lblCreateView.AutoSize = True
        Me.lblCreateView.Location = New System.Drawing.Point(3, 97)
        Me.lblCreateView.Name = "lblCreateView"
        Me.lblCreateView.Size = New System.Drawing.Size(77, 17)
        Me.lblCreateView.TabIndex = 10
        Me.lblCreateView.Text = "Create View"
        Me.lblCreateView.Visible = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(3, 15)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(291, 17)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Which document you want to use for reporting ?"
        '
        'cboSel
        '
        Me.cboSel.FormattingEnabled = True
        Me.cboSel.Location = New System.Drawing.Point(6, 68)
        Me.cboSel.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.cboSel.Name = "cboSel"
        Me.cboSel.Size = New System.Drawing.Size(363, 25)
        Me.cboSel.TabIndex = 1
        '
        'lblSel
        '
        Me.lblSel.AutoSize = True
        Me.lblSel.Location = New System.Drawing.Point(3, 47)
        Me.lblSel.Name = "lblSel"
        Me.lblSel.Size = New System.Drawing.Size(87, 17)
        Me.lblSel.TabIndex = 0
        Me.lblSel.Text = "Select Master"
        '
        'optAdhoc
        '
        Me.optAdhoc.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.optAdhoc.AutoSize = True
        Me.optAdhoc.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optAdhoc.Location = New System.Drawing.Point(300, 12)
        Me.optAdhoc.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.optAdhoc.Name = "optAdhoc"
        Me.optAdhoc.Size = New System.Drawing.Size(106, 21)
        Me.optAdhoc.TabIndex = 5
        Me.optAdhoc.Text = "Adhoc Query"
        Me.optAdhoc.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.optMasters)
        Me.Panel1.Controls.Add(Me.optTrnHdr)
        Me.Panel1.Controls.Add(Me.optAdhoc)
        Me.Panel1.Controls.Add(Me.optExisting)
        Me.Panel1.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Panel1.Location = New System.Drawing.Point(11, 34)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(556, 53)
        Me.Panel1.TabIndex = 7
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(11, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(328, 17)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "You can create a report by selecting the below options"
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.btnExport)
        Me.Panel2.Controls.Add(Me.btnHide)
        Me.Panel2.Controls.Add(Me.btnClear)
        Me.Panel2.Controls.Add(Me.btnClose)
        Me.Panel2.Controls.Add(Me.btnNext)
        Me.Panel2.Controls.Add(Me.btnDelete)
        Me.Panel2.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Panel2.Location = New System.Drawing.Point(11, 412)
        Me.Panel2.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(556, 49)
        Me.Panel2.TabIndex = 9
        '
        'btnExport
        '
        Me.btnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExport.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExport.Location = New System.Drawing.Point(93, 11)
        Me.btnExport.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(74, 30)
        Me.btnExport.TabIndex = 9
        Me.btnExport.Text = "&Export"
        Me.ToolTip1.SetToolTip(Me.btnExport, "Delete:Alt+D")
        Me.btnExport.UseVisualStyleBackColor = False
        Me.btnExport.Visible = False
        '
        'btnHide
        '
        Me.btnHide.Location = New System.Drawing.Point(277, 11)
        Me.btnHide.Name = "btnHide"
        Me.btnHide.Size = New System.Drawing.Size(60, 30)
        Me.btnHide.TabIndex = 8
        Me.btnHide.Text = "Hide"
        Me.btnHide.UseVisualStyleBackColor = True
        Me.btnHide.Visible = False
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(343, 11)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(51, 30)
        Me.btnClear.TabIndex = 7
        Me.btnClear.Text = "Clear"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnClose.Image = CType(resources.GetObject("btnClose.Image"), System.Drawing.Image)
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(472, 11)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(68, 30)
        Me.btnClose.TabIndex = 4
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close:Alt+O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnNext
        '
        Me.btnNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNext.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnNext.Image = CType(resources.GetObject("btnNext.Image"), System.Drawing.Image)
        Me.btnNext.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnNext.Location = New System.Drawing.Point(400, 11)
        Me.btnNext.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(66, 30)
        Me.btnNext.TabIndex = 2
        Me.btnNext.Text = "&Next"
        Me.btnNext.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnNext, "Next:Alt+N")
        Me.btnNext.UseVisualStyleBackColor = False
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDelete.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnDelete.Image = CType(resources.GetObject("btnDelete.Image"), System.Drawing.Image)
        Me.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDelete.Location = New System.Drawing.Point(13, 11)
        Me.btnDelete.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(74, 30)
        Me.btnDelete.TabIndex = 6
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnDelete, "Delete:Alt+D")
        Me.btnDelete.UseVisualStyleBackColor = False
        '
        'frmRptCRDesign
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(580, 483)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.fraSel)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "frmRptCRDesign"
        Me.ShowIcon = False
        Me.Text = "Customised Reports..."
        Me.fraSel.ResumeLayout(False)
        Me.fraSel.PerformLayout()
        CType(Me.vsfExp, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlExp.ResumeLayout(False)
        Me.pnlExp.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents optMasters As System.Windows.Forms.RadioButton
    Friend WithEvents optTrnHdr As System.Windows.Forms.RadioButton
    Friend WithEvents optExisting As System.Windows.Forms.RadioButton
    Friend WithEvents fraSel As System.Windows.Forms.Panel
    Friend WithEvents lblSel As System.Windows.Forms.Label
    Friend WithEvents cboSel As System.Windows.Forms.ComboBox
    Friend WithEvents btnNext As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents optAdhoc As System.Windows.Forms.RadioButton
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblCreateView As Label
    Friend WithEvents btnLoad As Button
    Friend WithEvents txtView As TextBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents btnClear As Button
    Friend WithEvents btnHide As Button
    Friend WithEvents btnExport As Button
    Friend WithEvents pnlExp As Panel
    Friend WithEvents btnExpClose As Button
    Friend WithEvents btnSave As Button
    Friend WithEvents txtFileName As TextBox
    Friend WithEvents Label3 As Label
    Public WithEvents vsfExp As C1.Win.C1FlexGrid.C1FlexGrid
End Class
