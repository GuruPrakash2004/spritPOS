<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRptBCPrint
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRptBCPrint))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtPrinter = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtNoOfCopies = New System.Windows.Forms.TextBox()
        Me.chkEditor = New System.Windows.Forms.CheckBox()
        Me.fraCmdScr2 = New System.Windows.Forms.Panel()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.cboTemplate = New System.Windows.Forms.ComboBox()
        Me.btnDesign = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblImage = New System.Windows.Forms.Label()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.imgBC = New System.Windows.Forms.PictureBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.vsfSearch = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfView = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnSelAll = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.pnlCopyShortCut = New System.Windows.Forms.Panel()
        Me.btnCopy = New System.Windows.Forms.Button()
        Me.pnlbtnClose = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.RbnCopyGrid = New System.Windows.Forms.RadioButton()
        Me.RbnCopyRow = New System.Windows.Forms.RadioButton()
        Me.RbnCopyColumn = New System.Windows.Forms.RadioButton()
        Me.RbnCopyCell = New System.Windows.Forms.RadioButton()
        Me.fraCmdScr2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.imgBC, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlCopyShortCut.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(129, 17)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Select Barcode Label"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 40)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(46, 17)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Printer"
        '
        'txtPrinter
        '
        Me.txtPrinter.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPrinter.Location = New System.Drawing.Point(138, 36)
        Me.txtPrinter.Name = "txtPrinter"
        Me.txtPrinter.Size = New System.Drawing.Size(279, 25)
        Me.txtPrinter.TabIndex = 6
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(3, 71)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(86, 17)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "No of Copies"
        '
        'txtNoOfCopies
        '
        Me.txtNoOfCopies.Location = New System.Drawing.Point(138, 67)
        Me.txtNoOfCopies.Name = "txtNoOfCopies"
        Me.txtNoOfCopies.Size = New System.Drawing.Size(69, 25)
        Me.txtNoOfCopies.TabIndex = 8
        '
        'chkEditor
        '
        Me.chkEditor.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkEditor.AutoSize = True
        Me.chkEditor.Location = New System.Drawing.Point(305, 69)
        Me.chkEditor.Name = "chkEditor"
        Me.chkEditor.Size = New System.Drawing.Size(112, 21)
        Me.chkEditor.TabIndex = 9
        Me.chkEditor.Text = "Open In Editor"
        Me.chkEditor.UseVisualStyleBackColor = True
        '
        'fraCmdScr2
        '
        Me.fraCmdScr2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fraCmdScr2.Controls.Add(Me.btnClose)
        Me.fraCmdScr2.Controls.Add(Me.btnPrint)
        Me.fraCmdScr2.Location = New System.Drawing.Point(435, 138)
        Me.fraCmdScr2.Name = "fraCmdScr2"
        Me.fraCmdScr2.Size = New System.Drawing.Size(152, 38)
        Me.fraCmdScr2.TabIndex = 10
        '
        'btnClose
        '
        Me.btnClose.Image = CType(resources.GetObject("btnClose.Image"), System.Drawing.Image)
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(76, 4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(71, 29)
        Me.btnClose.TabIndex = 1
        Me.btnClose.Text = "&Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnPrint
        '
        Me.btnPrint.Image = CType(resources.GetObject("btnPrint.Image"), System.Drawing.Image)
        Me.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPrint.Location = New System.Drawing.Point(4, 4)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(64, 29)
        Me.btnPrint.TabIndex = 0
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnPrint.UseVisualStyleBackColor = False
        '
        'cboTemplate
        '
        Me.cboTemplate.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboTemplate.FormattingEnabled = True
        Me.cboTemplate.Location = New System.Drawing.Point(138, 5)
        Me.cboTemplate.Name = "cboTemplate"
        Me.cboTemplate.Size = New System.Drawing.Size(279, 25)
        Me.cboTemplate.TabIndex = 1
        '
        'btnDesign
        '
        Me.btnDesign.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDesign.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnDesign.Image = CType(resources.GetObject("btnDesign.Image"), System.Drawing.Image)
        Me.btnDesign.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDesign.Location = New System.Drawing.Point(504, 3)
        Me.btnDesign.Name = "btnDesign"
        Me.btnDesign.Size = New System.Drawing.Size(78, 29)
        Me.btnDesign.TabIndex = 11
        Me.btnDesign.Text = " &Design"
        Me.btnDesign.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnDesign.UseVisualStyleBackColor = False
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.lblImage)
        Me.Panel1.Controls.Add(Me.btnLoad)
        Me.Panel1.Controls.Add(Me.cboTemplate)
        Me.Panel1.Controls.Add(Me.imgBC)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.btnDesign)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.fraCmdScr2)
        Me.Panel1.Controls.Add(Me.txtPrinter)
        Me.Panel1.Controls.Add(Me.chkEditor)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.txtNoOfCopies)
        Me.Panel1.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Panel1.Location = New System.Drawing.Point(12, 12)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(596, 183)
        Me.Panel1.TabIndex = 13
        '
        'lblImage
        '
        Me.lblImage.Location = New System.Drawing.Point(16, 111)
        Me.lblImage.Name = "lblImage"
        Me.lblImage.Size = New System.Drawing.Size(80, 52)
        Me.lblImage.TabIndex = 14
        Me.lblImage.Text = "No Image. Please copy the image File in Rpts Folder."
        Me.lblImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnLoad
        '
        Me.btnLoad.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLoad.Location = New System.Drawing.Point(423, 3)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(75, 29)
        Me.btnLoad.TabIndex = 13
        Me.btnLoad.Text = "Load"
        Me.btnLoad.UseVisualStyleBackColor = False
        '
        'imgBC
        '
        Me.imgBC.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.imgBC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.imgBC.Location = New System.Drawing.Point(6, 98)
        Me.imgBC.Name = "imgBC"
        Me.imgBC.Size = New System.Drawing.Size(102, 78)
        Me.imgBC.TabIndex = 12
        Me.imgBC.TabStop = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.pnlCopyShortCut)
        Me.GroupBox1.Controls.Add(Me.vsfSearch)
        Me.GroupBox1.Controls.Add(Me.vsfView)
        Me.GroupBox1.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 201)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(596, 190)
        Me.GroupBox1.TabIndex = 14
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Select Data"
        '
        'vsfSearch
        '
        Me.vsfSearch.AllowFiltering = True
        Me.vsfSearch.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfSearch.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfSearch.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfSearch.AutoResize = True
        Me.vsfSearch.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfSearch.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfSearch.ColumnInfo = "2,0,0,0,0,95,Columns:"
        Me.vsfSearch.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfSearch.ExtendLastCol = True
        Me.vsfSearch.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.vsfSearch.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfSearch.Location = New System.Drawing.Point(8, 16)
        Me.vsfSearch.Name = "vsfSearch"
        Me.vsfSearch.Rows.Count = 1
        Me.vsfSearch.Rows.DefaultSize = 19
        Me.vsfSearch.Rows.Fixed = 0
        Me.vsfSearch.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.vsfSearch.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfSearch.ShowCellLabels = True
        Me.vsfSearch.Size = New System.Drawing.Size(580, 17)
        Me.vsfSearch.StyleInfo = resources.GetString("vsfSearch.StyleInfo")
        Me.vsfSearch.TabIndex = 31
        Me.vsfSearch.Tag = ""
        Me.vsfSearch.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.System
        '
        'vsfView
        '
        Me.vsfView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfView.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfView.ColumnInfo = "10,0,0,0,0,95,Columns:"
        Me.vsfView.ExtendLastCol = True
        Me.vsfView.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.vsfView.Location = New System.Drawing.Point(8, 32)
        Me.vsfView.Name = "vsfView"
        Me.vsfView.Rows.DefaultSize = 19
        Me.vsfView.Size = New System.Drawing.Size(580, 152)
        Me.vsfView.StyleInfo = resources.GetString("vsfView.StyleInfo")
        Me.vsfView.TabIndex = 30
        '
        'btnSelAll
        '
        Me.btnSelAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSelAll.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnSelAll.Image = CType(resources.GetObject("btnSelAll.Image"), System.Drawing.Image)
        Me.btnSelAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSelAll.Location = New System.Drawing.Point(12, 397)
        Me.btnSelAll.Name = "btnSelAll"
        Me.btnSelAll.Size = New System.Drawing.Size(88, 29)
        Me.btnSelAll.TabIndex = 15
        Me.btnSelAll.Text = "&Select All"
        Me.btnSelAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSelAll.UseVisualStyleBackColor = False
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(461, 403)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(147, 17)
        Me.Label4.TabIndex = 16
        Me.Label4.Text = "Double Click to Remove"
        '
        'vsfDet
        '
        Me.vsfDet.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfDet.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(12, 432)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(596, 119)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 35
        Me.vsfDet.Tag = ""
        '
        'pnlCopyShortCut
        '
        Me.pnlCopyShortCut.Controls.Add(Me.btnCopy)
        Me.pnlCopyShortCut.Controls.Add(Me.pnlbtnClose)
        Me.pnlCopyShortCut.Controls.Add(Me.Label5)
        Me.pnlCopyShortCut.Controls.Add(Me.Label6)
        Me.pnlCopyShortCut.Controls.Add(Me.Label7)
        Me.pnlCopyShortCut.Controls.Add(Me.Label8)
        Me.pnlCopyShortCut.Controls.Add(Me.RbnCopyGrid)
        Me.pnlCopyShortCut.Controls.Add(Me.RbnCopyRow)
        Me.pnlCopyShortCut.Controls.Add(Me.RbnCopyColumn)
        Me.pnlCopyShortCut.Controls.Add(Me.RbnCopyCell)
        Me.pnlCopyShortCut.Location = New System.Drawing.Point(114, 24)
        Me.pnlCopyShortCut.Name = "pnlCopyShortCut"
        Me.pnlCopyShortCut.Size = New System.Drawing.Size(329, 177)
        Me.pnlCopyShortCut.TabIndex = 298
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
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(178, 79)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(77, 16)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "Ctrl + Alt + C"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(178, 48)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(87, 16)
        Me.Label7.TabIndex = 5
        Me.Label7.Text = "Ctrl + Shift + C"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(178, 19)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(49, 16)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "Ctrl + C"
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
        'frmRptBCPrint
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(620, 563)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.btnSelAll)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Panel1)
        Me.MinimumSize = New System.Drawing.Size(636, 601)
        Me.Name = "frmRptBCPrint"
        Me.ShowIcon = False
        Me.Text = "Barcode Printing"
        Me.fraCmdScr2.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.imgBC, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlCopyShortCut.ResumeLayout(False)
        Me.pnlCopyShortCut.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtPrinter As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtNoOfCopies As System.Windows.Forms.TextBox
    Friend WithEvents chkEditor As System.Windows.Forms.CheckBox
    Friend WithEvents fraCmdScr2 As System.Windows.Forms.Panel
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents cboTemplate As System.Windows.Forms.ComboBox
    Friend WithEvents btnDesign As System.Windows.Forms.Button
    Friend WithEvents imgBC As System.Windows.Forms.PictureBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents vsfSearch As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents vsfView As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnSelAll As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnLoad As System.Windows.Forms.Button
    Friend WithEvents lblImage As System.Windows.Forms.Label
    Friend WithEvents pnlCopyShortCut As Panel
    Friend WithEvents btnCopy As Button
    Friend WithEvents pnlbtnClose As Button
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents RbnCopyGrid As RadioButton
    Friend WithEvents RbnCopyRow As RadioButton
    Friend WithEvents RbnCopyColumn As RadioButton
    Friend WithEvents RbnCopyCell As RadioButton
End Class
