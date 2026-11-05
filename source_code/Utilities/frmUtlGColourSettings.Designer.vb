<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlGColourSettings
    Inherits System.Windows.Forms.Form
    'Inherits MetroFramework.Forms.MetroForm

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlGColourSettings))
        Me.fbd = New System.Windows.Forms.FolderBrowserDialog()
        Me.fsd = New System.Windows.Forms.OpenFileDialog()
        Me.lblfsize = New System.Windows.Forms.Label()
        Me.lblfname = New System.Windows.Forms.Label()
        Me.lblmods = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCloseq = New vLQMS.CustomButton.CustomButton()
        Me.btnGridClrSave = New vLQMS.CustomButton.CustomButton()
        Me.btnDefault = New vLQMS.CustomButton.CustomButton()
        Me.grpAlternate = New System.Windows.Forms.GroupBox()
        Me.lblAlternateBack = New System.Windows.Forms.Label()
        Me.lblAlternateFore = New System.Windows.Forms.Label()
        Me.lblAltBClr = New System.Windows.Forms.Label()
        Me.lblAltFClr = New System.Windows.Forms.Label()
        Me.grpHighlight = New System.Windows.Forms.GroupBox()
        Me.lblHighlightBack = New System.Windows.Forms.Label()
        Me.lblHighlightFore = New System.Windows.Forms.Label()
        Me.lblHltBClr = New System.Windows.Forms.Label()
        Me.lblHltFClr = New System.Windows.Forms.Label()
        Me.grpFixed = New System.Windows.Forms.GroupBox()
        Me.lblFixedBack = New System.Windows.Forms.Label()
        Me.lblFixedFore = New System.Windows.Forms.Label()
        Me.lblFixedBClr = New System.Windows.Forms.Label()
        Me.lblFixedFClr = New System.Windows.Forms.Label()
        Me.grpFocus = New System.Windows.Forms.GroupBox()
        Me.lblFocusBack = New System.Windows.Forms.Label()
        Me.lblFocusFore = New System.Windows.Forms.Label()
        Me.lblFocusBClr = New System.Windows.Forms.Label()
        Me.lblFocusFClr = New System.Windows.Forms.Label()
        Me.grpNormal = New System.Windows.Forms.GroupBox()
        Me.lblNormalBack = New System.Windows.Forms.Label()
        Me.lblNormalFore = New System.Windows.Forms.Label()
        Me.lblNormBClr = New System.Windows.Forms.Label()
        Me.lblNormFClr = New System.Windows.Forms.Label()
        Me.sampleGrid = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblEmptyAreaBack = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.GrpToolTip = New System.Windows.Forms.GroupBox()
        Me.lblTlTipBack = New System.Windows.Forms.Label()
        Me.lblTlTipFore = New System.Windows.Forms.Label()
        Me.lblTltipBclr = New System.Windows.Forms.Label()
        Me.lblTltipFclr = New System.Windows.Forms.Label()
        Me.pnlTlTip = New System.Windows.Forms.Panel()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.lblToolTip = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Panel3.SuspendLayout()
        Me.grpAlternate.SuspendLayout()
        Me.grpHighlight.SuspendLayout()
        Me.grpFixed.SuspendLayout()
        Me.grpFocus.SuspendLayout()
        Me.grpNormal.SuspendLayout()
        CType(Me.sampleGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.GrpToolTip.SuspendLayout()
        Me.pnlTlTip.SuspendLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'fsd
        '
        Me.fsd.FileName = "OpenFileDialog1"
        '
        'lblfsize
        '
        Me.lblfsize.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblfsize.AutoSize = True
        Me.lblfsize.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblfsize.Location = New System.Drawing.Point(12, 602)
        Me.lblfsize.Name = "lblfsize"
        Me.lblfsize.Size = New System.Drawing.Size(0, 16)
        Me.lblfsize.TabIndex = 13
        '
        'lblfname
        '
        Me.lblfname.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblfname.AutoSize = True
        Me.lblfname.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblfname.Location = New System.Drawing.Point(12, 580)
        Me.lblfname.Name = "lblfname"
        Me.lblfname.Size = New System.Drawing.Size(0, 16)
        Me.lblfname.TabIndex = 14
        '
        'lblmods
        '
        Me.lblmods.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblmods.AutoSize = True
        Me.lblmods.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblmods.Location = New System.Drawing.Point(12, 622)
        Me.lblmods.Name = "lblmods"
        Me.lblmods.Size = New System.Drawing.Size(0, 16)
        Me.lblmods.TabIndex = 15
        '
        'Panel3
        '
        Me.Panel3.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel3.BackColor = System.Drawing.Color.FromArgb(CType(CType(246, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(252, Byte), Integer))
        Me.Panel3.Controls.Add(Me.btnClose)
        Me.Panel3.Controls.Add(Me.btnSave)
        Me.Panel3.Controls.Add(Me.btnCloseq)
        Me.Panel3.Controls.Add(Me.btnGridClrSave)
        Me.Panel3.Controls.Add(Me.btnDefault)
        Me.Panel3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel3.Location = New System.Drawing.Point(12, 594)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(804, 41)
        Me.Panel3.TabIndex = 52
        '
        'btnClose
        '
        Me.btnClose.AutoSize = True
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(713, 5)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(88, 32)
        Me.btnClose.TabIndex = 103
        Me.btnClose.Text = "&Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Shortcut : Alt + C")
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Location = New System.Drawing.Point(619, 5)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(88, 30)
        Me.btnSave.TabIndex = 104
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Shortcut : Alt + S")
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnCloseq
        '
        Me.btnCloseq.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCloseq.BackColor = System.Drawing.Color.Transparent
        Me.btnCloseq.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnCloseq.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnCloseq.Cursor = System.Windows.Forms.Cursors.Arrow
        Me.btnCloseq.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCloseq.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.btnCloseq.ForeColor = System.Drawing.Color.White
        Me.btnCloseq.Location = New System.Drawing.Point(195, 3)
        Me.btnCloseq.Margin = New System.Windows.Forms.Padding(4, 1, 4, 1)
        Me.btnCloseq.Name = "btnCloseq"
        Me.btnCloseq.Padding = New System.Windows.Forms.Padding(4)
        Me.btnCloseq.Size = New System.Drawing.Size(88, 35)
        Me.btnCloseq.TabIndex = 5
        Me.btnCloseq.Text = "Cl&ose"
        Me.btnCloseq.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCloseq, "Close : Alt + O")
        Me.btnCloseq.UseVisualStyleBackColor = False
        Me.btnCloseq.Visible = False
        '
        'btnGridClrSave
        '
        Me.btnGridClrSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGridClrSave.BackColor = System.Drawing.Color.Transparent
        Me.btnGridClrSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnGridClrSave.Cursor = System.Windows.Forms.Cursors.Arrow
        Me.btnGridClrSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGridClrSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.btnGridClrSave.ForeColor = System.Drawing.Color.White
        Me.btnGridClrSave.Location = New System.Drawing.Point(99, 2)
        Me.btnGridClrSave.Margin = New System.Windows.Forms.Padding(4, 1, 4, 1)
        Me.btnGridClrSave.Name = "btnGridClrSave"
        Me.btnGridClrSave.Padding = New System.Windows.Forms.Padding(4)
        Me.btnGridClrSave.Size = New System.Drawing.Size(88, 35)
        Me.btnGridClrSave.TabIndex = 4
        Me.btnGridClrSave.Text = "&Save"
        Me.btnGridClrSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnGridClrSave, "Save : Alt + S")
        Me.btnGridClrSave.UseVisualStyleBackColor = True
        Me.btnGridClrSave.Visible = False
        '
        'btnDefault
        '
        Me.btnDefault.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDefault.BackColor = System.Drawing.Color.Gray
        Me.btnDefault.Cursor = System.Windows.Forms.Cursors.Arrow
        Me.btnDefault.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDefault.ForeColor = System.Drawing.Color.White
        Me.btnDefault.Location = New System.Drawing.Point(527, 2)
        Me.btnDefault.Margin = New System.Windows.Forms.Padding(4, 1, 4, 1)
        Me.btnDefault.Name = "btnDefault"
        Me.btnDefault.Padding = New System.Windows.Forms.Padding(4)
        Me.btnDefault.Size = New System.Drawing.Size(88, 35)
        Me.btnDefault.TabIndex = 3
        Me.btnDefault.Text = "&Default"
        Me.btnDefault.UseVisualStyleBackColor = False
        '
        'grpAlternate
        '
        Me.grpAlternate.Controls.Add(Me.lblAlternateBack)
        Me.grpAlternate.Controls.Add(Me.lblAlternateFore)
        Me.grpAlternate.Controls.Add(Me.lblAltBClr)
        Me.grpAlternate.Controls.Add(Me.lblAltFClr)
        Me.grpAlternate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpAlternate.Location = New System.Drawing.Point(12, 106)
        Me.grpAlternate.Name = "grpAlternate"
        Me.grpAlternate.Size = New System.Drawing.Size(169, 75)
        Me.grpAlternate.TabIndex = 55
        Me.grpAlternate.TabStop = False
        Me.grpAlternate.Text = "Alternate"
        '
        'lblAlternateBack
        '
        Me.lblAlternateBack.AutoSize = True
        Me.lblAlternateBack.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblAlternateBack.Location = New System.Drawing.Point(126, 47)
        Me.lblAlternateBack.Name = "lblAlternateBack"
        Me.lblAlternateBack.Size = New System.Drawing.Size(36, 22)
        Me.lblAlternateBack.TabIndex = 47
        Me.lblAlternateBack.Tag = "3"
        Me.lblAlternateBack.Text = "     "
        '
        'lblAlternateFore
        '
        Me.lblAlternateFore.AutoSize = True
        Me.lblAlternateFore.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblAlternateFore.Location = New System.Drawing.Point(126, 15)
        Me.lblAlternateFore.Name = "lblAlternateFore"
        Me.lblAlternateFore.Size = New System.Drawing.Size(36, 22)
        Me.lblAlternateFore.TabIndex = 46
        Me.lblAlternateFore.Tag = "2"
        Me.lblAlternateFore.Text = "     "
        '
        'lblAltBClr
        '
        Me.lblAltBClr.AutoSize = True
        Me.lblAltBClr.Location = New System.Drawing.Point(19, 49)
        Me.lblAltBClr.Name = "lblAltBClr"
        Me.lblAltBClr.Size = New System.Drawing.Size(106, 20)
        Me.lblAltBClr.TabIndex = 1
        Me.lblAltBClr.Text = "Back Colour"
        '
        'lblAltFClr
        '
        Me.lblAltFClr.AutoSize = True
        Me.lblAltFClr.Location = New System.Drawing.Point(19, 16)
        Me.lblAltFClr.Name = "lblAltFClr"
        Me.lblAltFClr.Size = New System.Drawing.Size(103, 20)
        Me.lblAltFClr.TabIndex = 0
        Me.lblAltFClr.Text = "Fore Colour"
        '
        'grpHighlight
        '
        Me.grpHighlight.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.grpHighlight.Controls.Add(Me.lblHighlightBack)
        Me.grpHighlight.Controls.Add(Me.lblHighlightFore)
        Me.grpHighlight.Controls.Add(Me.lblHltBClr)
        Me.grpHighlight.Controls.Add(Me.lblHltFClr)
        Me.grpHighlight.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpHighlight.Location = New System.Drawing.Point(12, 263)
        Me.grpHighlight.Name = "grpHighlight"
        Me.grpHighlight.Size = New System.Drawing.Size(169, 82)
        Me.grpHighlight.TabIndex = 56
        Me.grpHighlight.TabStop = False
        Me.grpHighlight.Text = "Highlight"
        '
        'lblHighlightBack
        '
        Me.lblHighlightBack.AutoSize = True
        Me.lblHighlightBack.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblHighlightBack.Location = New System.Drawing.Point(126, 52)
        Me.lblHighlightBack.Name = "lblHighlightBack"
        Me.lblHighlightBack.Size = New System.Drawing.Size(36, 22)
        Me.lblHighlightBack.TabIndex = 56
        Me.lblHighlightBack.Tag = "7"
        Me.lblHighlightBack.Text = "     "
        '
        'lblHighlightFore
        '
        Me.lblHighlightFore.AutoSize = True
        Me.lblHighlightFore.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblHighlightFore.Location = New System.Drawing.Point(126, 21)
        Me.lblHighlightFore.Name = "lblHighlightFore"
        Me.lblHighlightFore.Size = New System.Drawing.Size(36, 22)
        Me.lblHighlightFore.TabIndex = 55
        Me.lblHighlightFore.Tag = "6"
        Me.lblHighlightFore.Text = "     "
        '
        'lblHltBClr
        '
        Me.lblHltBClr.AutoSize = True
        Me.lblHltBClr.Location = New System.Drawing.Point(19, 52)
        Me.lblHltBClr.Name = "lblHltBClr"
        Me.lblHltBClr.Size = New System.Drawing.Size(106, 20)
        Me.lblHltBClr.TabIndex = 1
        Me.lblHltBClr.Text = "Back Colour"
        '
        'lblHltFClr
        '
        Me.lblHltFClr.AutoSize = True
        Me.lblHltFClr.Location = New System.Drawing.Point(19, 22)
        Me.lblHltFClr.Name = "lblHltFClr"
        Me.lblHltFClr.Size = New System.Drawing.Size(103, 20)
        Me.lblHltFClr.TabIndex = 0
        Me.lblHltFClr.Text = "Fore Colour"
        '
        'grpFixed
        '
        Me.grpFixed.Controls.Add(Me.lblFixedBack)
        Me.grpFixed.Controls.Add(Me.lblFixedFore)
        Me.grpFixed.Controls.Add(Me.lblFixedBClr)
        Me.grpFixed.Controls.Add(Me.lblFixedFClr)
        Me.grpFixed.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpFixed.Location = New System.Drawing.Point(12, 187)
        Me.grpFixed.Name = "grpFixed"
        Me.grpFixed.Size = New System.Drawing.Size(168, 75)
        Me.grpFixed.TabIndex = 57
        Me.grpFixed.TabStop = False
        Me.grpFixed.Text = "Fixed"
        '
        'lblFixedBack
        '
        Me.lblFixedBack.AutoSize = True
        Me.lblFixedBack.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblFixedBack.Location = New System.Drawing.Point(126, 51)
        Me.lblFixedBack.Name = "lblFixedBack"
        Me.lblFixedBack.Size = New System.Drawing.Size(36, 22)
        Me.lblFixedBack.TabIndex = 56
        Me.lblFixedBack.Tag = "5"
        Me.lblFixedBack.Text = "     "
        '
        'lblFixedFore
        '
        Me.lblFixedFore.AutoSize = True
        Me.lblFixedFore.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblFixedFore.Location = New System.Drawing.Point(126, 16)
        Me.lblFixedFore.Name = "lblFixedFore"
        Me.lblFixedFore.Size = New System.Drawing.Size(36, 22)
        Me.lblFixedFore.TabIndex = 55
        Me.lblFixedFore.Tag = "4"
        Me.lblFixedFore.Text = "     "
        '
        'lblFixedBClr
        '
        Me.lblFixedBClr.AutoSize = True
        Me.lblFixedBClr.Location = New System.Drawing.Point(19, 51)
        Me.lblFixedBClr.Name = "lblFixedBClr"
        Me.lblFixedBClr.Size = New System.Drawing.Size(106, 20)
        Me.lblFixedBClr.TabIndex = 1
        Me.lblFixedBClr.Text = "Back Colour"
        '
        'lblFixedFClr
        '
        Me.lblFixedFClr.AutoSize = True
        Me.lblFixedFClr.Location = New System.Drawing.Point(19, 17)
        Me.lblFixedFClr.Name = "lblFixedFClr"
        Me.lblFixedFClr.Size = New System.Drawing.Size(103, 20)
        Me.lblFixedFClr.TabIndex = 0
        Me.lblFixedFClr.Text = "Fore Colour"
        '
        'grpFocus
        '
        Me.grpFocus.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.grpFocus.Controls.Add(Me.lblFocusBack)
        Me.grpFocus.Controls.Add(Me.lblFocusFore)
        Me.grpFocus.Controls.Add(Me.lblFocusBClr)
        Me.grpFocus.Controls.Add(Me.lblFocusFClr)
        Me.grpFocus.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpFocus.Location = New System.Drawing.Point(12, 351)
        Me.grpFocus.Name = "grpFocus"
        Me.grpFocus.Size = New System.Drawing.Size(169, 81)
        Me.grpFocus.TabIndex = 58
        Me.grpFocus.TabStop = False
        Me.grpFocus.Text = "Focus"
        '
        'lblFocusBack
        '
        Me.lblFocusBack.AutoSize = True
        Me.lblFocusBack.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblFocusBack.Location = New System.Drawing.Point(126, 53)
        Me.lblFocusBack.Name = "lblFocusBack"
        Me.lblFocusBack.Size = New System.Drawing.Size(36, 22)
        Me.lblFocusBack.TabIndex = 56
        Me.lblFocusBack.Tag = "9"
        Me.lblFocusBack.Text = "     "
        '
        'lblFocusFore
        '
        Me.lblFocusFore.AutoSize = True
        Me.lblFocusFore.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblFocusFore.Location = New System.Drawing.Point(126, 17)
        Me.lblFocusFore.Name = "lblFocusFore"
        Me.lblFocusFore.Size = New System.Drawing.Size(36, 22)
        Me.lblFocusFore.TabIndex = 55
        Me.lblFocusFore.Tag = "8"
        Me.lblFocusFore.Text = "     "
        '
        'lblFocusBClr
        '
        Me.lblFocusBClr.AutoSize = True
        Me.lblFocusBClr.Location = New System.Drawing.Point(19, 53)
        Me.lblFocusBClr.Name = "lblFocusBClr"
        Me.lblFocusBClr.Size = New System.Drawing.Size(106, 20)
        Me.lblFocusBClr.TabIndex = 1
        Me.lblFocusBClr.Text = "Back Colour"
        '
        'lblFocusFClr
        '
        Me.lblFocusFClr.AutoSize = True
        Me.lblFocusFClr.Location = New System.Drawing.Point(19, 18)
        Me.lblFocusFClr.Name = "lblFocusFClr"
        Me.lblFocusFClr.Size = New System.Drawing.Size(103, 20)
        Me.lblFocusFClr.TabIndex = 0
        Me.lblFocusFClr.Text = "Fore Colour"
        '
        'grpNormal
        '
        Me.grpNormal.Controls.Add(Me.lblNormalBack)
        Me.grpNormal.Controls.Add(Me.lblNormalFore)
        Me.grpNormal.Controls.Add(Me.lblNormBClr)
        Me.grpNormal.Controls.Add(Me.lblNormFClr)
        Me.grpNormal.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpNormal.Location = New System.Drawing.Point(12, 24)
        Me.grpNormal.Name = "grpNormal"
        Me.grpNormal.Size = New System.Drawing.Size(169, 75)
        Me.grpNormal.TabIndex = 54
        Me.grpNormal.TabStop = False
        Me.grpNormal.Text = "Normal"
        '
        'lblNormalBack
        '
        Me.lblNormalBack.AutoSize = True
        Me.lblNormalBack.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblNormalBack.Location = New System.Drawing.Point(126, 50)
        Me.lblNormalBack.Name = "lblNormalBack"
        Me.lblNormalBack.Size = New System.Drawing.Size(36, 22)
        Me.lblNormalBack.TabIndex = 45
        Me.lblNormalBack.Tag = "1"
        Me.lblNormalBack.Text = "     "
        '
        'lblNormalFore
        '
        Me.lblNormalFore.AutoSize = True
        Me.lblNormalFore.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblNormalFore.Location = New System.Drawing.Point(126, 17)
        Me.lblNormalFore.Name = "lblNormalFore"
        Me.lblNormalFore.Size = New System.Drawing.Size(36, 22)
        Me.lblNormalFore.TabIndex = 44
        Me.lblNormalFore.Tag = "0"
        Me.lblNormalFore.Text = "     "
        '
        'lblNormBClr
        '
        Me.lblNormBClr.AutoSize = True
        Me.lblNormBClr.Location = New System.Drawing.Point(19, 50)
        Me.lblNormBClr.Name = "lblNormBClr"
        Me.lblNormBClr.Size = New System.Drawing.Size(106, 20)
        Me.lblNormBClr.TabIndex = 1
        Me.lblNormBClr.Text = "Back Colour"
        '
        'lblNormFClr
        '
        Me.lblNormFClr.AutoSize = True
        Me.lblNormFClr.Location = New System.Drawing.Point(19, 18)
        Me.lblNormFClr.Name = "lblNormFClr"
        Me.lblNormFClr.Size = New System.Drawing.Size(103, 20)
        Me.lblNormFClr.TabIndex = 0
        Me.lblNormFClr.Text = "Fore Colour"
        '
        'sampleGrid
        '
        Me.sampleGrid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.sampleGrid.ColumnInfo = "10,0,0,0,0,125,Columns:"
        Me.sampleGrid.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.sampleGrid.Location = New System.Drawing.Point(186, 28)
        Me.sampleGrid.Name = "sampleGrid"
        Me.sampleGrid.Rows.DefaultSize = 25
        Me.sampleGrid.Size = New System.Drawing.Size(630, 455)
        Me.sampleGrid.StyleInfo = resources.GetString("sampleGrid.StyleInfo")
        Me.sampleGrid.TabIndex = 53
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(387, 5)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(231, 20)
        Me.Label4.TabIndex = 59
        Me.Label4.Text = "Sample Grid Colour Preview"
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.lblEmptyAreaBack)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(12, 438)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(169, 51)
        Me.GroupBox1.TabIndex = 60
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Empty Area"
        '
        'lblEmptyAreaBack
        '
        Me.lblEmptyAreaBack.AutoSize = True
        Me.lblEmptyAreaBack.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblEmptyAreaBack.Location = New System.Drawing.Point(126, 22)
        Me.lblEmptyAreaBack.Name = "lblEmptyAreaBack"
        Me.lblEmptyAreaBack.Size = New System.Drawing.Size(36, 22)
        Me.lblEmptyAreaBack.TabIndex = 56
        Me.lblEmptyAreaBack.Tag = "10"
        Me.lblEmptyAreaBack.Text = "     "
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(19, 23)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(106, 20)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Back Colour"
        '
        'GrpToolTip
        '
        Me.GrpToolTip.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.GrpToolTip.Controls.Add(Me.lblTlTipBack)
        Me.GrpToolTip.Controls.Add(Me.lblTlTipFore)
        Me.GrpToolTip.Controls.Add(Me.lblTltipBclr)
        Me.GrpToolTip.Controls.Add(Me.lblTltipFclr)
        Me.GrpToolTip.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GrpToolTip.Location = New System.Drawing.Point(12, 495)
        Me.GrpToolTip.Name = "GrpToolTip"
        Me.GrpToolTip.Size = New System.Drawing.Size(169, 82)
        Me.GrpToolTip.TabIndex = 61
        Me.GrpToolTip.TabStop = False
        Me.GrpToolTip.Text = "ToolTip"
        '
        'lblTlTipBack
        '
        Me.lblTlTipBack.AutoSize = True
        Me.lblTlTipBack.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTlTipBack.Location = New System.Drawing.Point(126, 51)
        Me.lblTlTipBack.Name = "lblTlTipBack"
        Me.lblTlTipBack.Size = New System.Drawing.Size(36, 22)
        Me.lblTlTipBack.TabIndex = 56
        Me.lblTlTipBack.Tag = "12"
        Me.lblTlTipBack.Text = "     "
        '
        'lblTlTipFore
        '
        Me.lblTlTipFore.AutoSize = True
        Me.lblTlTipFore.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTlTipFore.Location = New System.Drawing.Point(126, 17)
        Me.lblTlTipFore.Name = "lblTlTipFore"
        Me.lblTlTipFore.Size = New System.Drawing.Size(36, 22)
        Me.lblTlTipFore.TabIndex = 55
        Me.lblTlTipFore.Tag = "11"
        Me.lblTlTipFore.Text = "     "
        '
        'lblTltipBclr
        '
        Me.lblTltipBclr.AutoSize = True
        Me.lblTltipBclr.Location = New System.Drawing.Point(16, 53)
        Me.lblTltipBclr.Name = "lblTltipBclr"
        Me.lblTltipBclr.Size = New System.Drawing.Size(106, 20)
        Me.lblTltipBclr.TabIndex = 1
        Me.lblTltipBclr.Text = "Back Colour"
        '
        'lblTltipFclr
        '
        Me.lblTltipFclr.AutoSize = True
        Me.lblTltipFclr.Location = New System.Drawing.Point(19, 18)
        Me.lblTltipFclr.Name = "lblTltipFclr"
        Me.lblTltipFclr.Size = New System.Drawing.Size(103, 20)
        Me.lblTltipFclr.TabIndex = 0
        Me.lblTltipFclr.Text = "Fore Colour"
        '
        'pnlTlTip
        '
        Me.pnlTlTip.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlTlTip.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(68, Byte), Integer), CType(CType(88, Byte), Integer))
        Me.pnlTlTip.Controls.Add(Me.PictureBox3)
        Me.pnlTlTip.Controls.Add(Me.lblToolTip)
        Me.pnlTlTip.Location = New System.Drawing.Point(186, 504)
        Me.pnlTlTip.Name = "pnlTlTip"
        Me.pnlTlTip.Size = New System.Drawing.Size(629, 73)
        Me.pnlTlTip.TabIndex = 102
        '
        'PictureBox3
        '
        Me.PictureBox3.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox3.BackgroundImage = CType(resources.GetObject("PictureBox3.BackgroundImage"), System.Drawing.Image)
        Me.PictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.PictureBox3.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(45, 40)
        Me.PictureBox3.TabIndex = 219
        Me.PictureBox3.TabStop = False
        '
        'lblToolTip
        '
        Me.lblToolTip.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblToolTip.BackColor = System.Drawing.Color.Transparent
        Me.lblToolTip.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblToolTip.ForeColor = System.Drawing.Color.White
        Me.lblToolTip.Location = New System.Drawing.Point(52, 6)
        Me.lblToolTip.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblToolTip.Name = "lblToolTip"
        Me.lblToolTip.Size = New System.Drawing.Size(516, 64)
        Me.lblToolTip.TabIndex = 216
        Me.lblToolTip.Text = "Help Tips:"
        '
        'frmUtlGColourSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(832, 647)
        Me.Controls.Add(Me.pnlTlTip)
        Me.Controls.Add(Me.GrpToolTip)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.grpAlternate)
        Me.Controls.Add(Me.grpHighlight)
        Me.Controls.Add(Me.grpFixed)
        Me.Controls.Add(Me.grpFocus)
        Me.Controls.Add(Me.grpNormal)
        Me.Controls.Add(Me.sampleGrid)
        Me.Controls.Add(Me.lblmods)
        Me.Controls.Add(Me.lblfname)
        Me.Controls.Add(Me.lblfsize)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MinimumSize = New System.Drawing.Size(530, 583)
        Me.Name = "frmUtlGColourSettings"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Grid Colour Settings"
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.grpAlternate.ResumeLayout(False)
        Me.grpAlternate.PerformLayout()
        Me.grpHighlight.ResumeLayout(False)
        Me.grpHighlight.PerformLayout()
        Me.grpFixed.ResumeLayout(False)
        Me.grpFixed.PerformLayout()
        Me.grpFocus.ResumeLayout(False)
        Me.grpFocus.PerformLayout()
        Me.grpNormal.ResumeLayout(False)
        Me.grpNormal.PerformLayout()
        CType(Me.sampleGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GrpToolTip.ResumeLayout(False)
        Me.GrpToolTip.PerformLayout()
        Me.pnlTlTip.ResumeLayout(False)
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents fbd As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents fsd As System.Windows.Forms.OpenFileDialog
    Friend WithEvents lblfsize As System.Windows.Forms.Label
    Friend WithEvents lblfname As System.Windows.Forms.Label
    Friend WithEvents lblmods As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents btnCloseq As vLQMS.CustomButton.CustomButton
    Friend WithEvents btnGridClrSave As vLQMS.CustomButton.CustomButton
    Friend WithEvents btnDefault As vLQMS.CustomButton.CustomButton
    Friend WithEvents grpAlternate As System.Windows.Forms.GroupBox
    Friend WithEvents lblAlternateBack As System.Windows.Forms.Label
    Friend WithEvents lblAlternateFore As System.Windows.Forms.Label
    Friend WithEvents lblAltBClr As System.Windows.Forms.Label
    Friend WithEvents lblAltFClr As System.Windows.Forms.Label
    Friend WithEvents grpHighlight As System.Windows.Forms.GroupBox
    Friend WithEvents lblHighlightBack As System.Windows.Forms.Label
    Friend WithEvents lblHighlightFore As System.Windows.Forms.Label
    Friend WithEvents lblHltBClr As System.Windows.Forms.Label
    Friend WithEvents lblHltFClr As System.Windows.Forms.Label
    Friend WithEvents grpFixed As System.Windows.Forms.GroupBox
    Friend WithEvents lblFixedBack As System.Windows.Forms.Label
    Friend WithEvents lblFixedFore As System.Windows.Forms.Label
    Friend WithEvents lblFixedBClr As System.Windows.Forms.Label
    Friend WithEvents lblFixedFClr As System.Windows.Forms.Label
    Friend WithEvents grpFocus As System.Windows.Forms.GroupBox
    Friend WithEvents lblFocusBack As System.Windows.Forms.Label
    Friend WithEvents lblFocusFore As System.Windows.Forms.Label
    Friend WithEvents lblFocusBClr As System.Windows.Forms.Label
    Friend WithEvents lblFocusFClr As System.Windows.Forms.Label
    Friend WithEvents grpNormal As System.Windows.Forms.GroupBox
    Friend WithEvents lblNormalBack As System.Windows.Forms.Label
    Friend WithEvents lblNormalFore As System.Windows.Forms.Label
    Friend WithEvents lblNormBClr As System.Windows.Forms.Label
    Friend WithEvents lblNormFClr As System.Windows.Forms.Label
    Friend WithEvents sampleGrid As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lblEmptyAreaBack As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents GrpToolTip As System.Windows.Forms.GroupBox
    Friend WithEvents lblTlTipBack As System.Windows.Forms.Label
    Friend WithEvents lblTlTipFore As System.Windows.Forms.Label
    Friend WithEvents lblTltipBclr As System.Windows.Forms.Label
    Friend WithEvents lblTltipFclr As System.Windows.Forms.Label
    Friend WithEvents pnlTlTip As System.Windows.Forms.Panel
    Friend WithEvents lblToolTip As System.Windows.Forms.Label
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents btnClose As Button
    Friend WithEvents btnSave As Button
End Class
