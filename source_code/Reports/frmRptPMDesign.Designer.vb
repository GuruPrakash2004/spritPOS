Imports C1.Win.C1FlexGrid

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmRptPMDesign
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRptPMDesign))
        Me.txtDocName = New System.Windows.Forms.TextBox()
        Me.lblName = New System.Windows.Forms.Label()
        Me.lblTblName = New System.Windows.Forms.Label()
        Me.cboDocType = New System.Windows.Forms.ComboBox()
        Me.lblMainFld = New System.Windows.Forms.Label()
        Me.cboRateType = New System.Windows.Forms.ComboBox()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.txtCondText = New System.Windows.Forms.TextBox()
        Me.lblCondText = New System.Windows.Forms.Label()
        Me.btnSave = New C1.Win.C1Input.C1Button()
        Me.btnCancel = New C1.Win.C1Input.C1Button()
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btDocClose = New C1.Win.C1Input.C1Button()
        Me.btnDocSave = New C1.Win.C1Input.C1Button()
        Me.lblSearchFld = New System.Windows.Forms.Label()
        Me.vsfDoc = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.gbDocuments = New System.Windows.Forms.GroupBox()
        Me.txtDocCond = New System.Windows.Forms.TextBox()
        Me.txtDateFld = New System.Windows.Forms.TextBox()
        Me.ucBtnTools = New vLQMS.ucBtn()
        Me.pnlCopyShortCut = New System.Windows.Forms.Panel()
        Me.btnCopy = New System.Windows.Forms.Button()
        Me.pnlbtnClose = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.RbnCopyGrid = New System.Windows.Forms.RadioButton()
        Me.RbnCopyRow = New System.Windows.Forms.RadioButton()
        Me.RbnCopyColumn = New System.Windows.Forms.RadioButton()
        Me.RbnCopyCell = New System.Windows.Forms.RadioButton()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfDoc, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbDocuments.SuspendLayout()
        Me.pnlCopyShortCut.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtDocName
        '
        Me.txtDocName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDocName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.txtDocName.Location = New System.Drawing.Point(137, 5)
        Me.txtDocName.MaxLength = 50
        Me.txtDocName.Name = "txtDocName"
        Me.txtDocName.Size = New System.Drawing.Size(309, 26)
        Me.txtDocName.TabIndex = 0
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.lblName.ForeColor = System.Drawing.Color.Red
        Me.lblName.Location = New System.Drawing.Point(12, 9)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(84, 20)
        Me.lblName.TabIndex = 3
        Me.lblName.Tag = "M"
        Me.lblName.Text = "Doc Name"
        '
        'lblTblName
        '
        Me.lblTblName.AutoSize = True
        Me.lblTblName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTblName.ForeColor = System.Drawing.Color.Red
        Me.lblTblName.Location = New System.Drawing.Point(8, 32)
        Me.lblTblName.Name = "lblTblName"
        Me.lblTblName.Size = New System.Drawing.Size(76, 20)
        Me.lblTblName.TabIndex = 253
        Me.lblTblName.Tag = "M"
        Me.lblTblName.Text = "Doc Type"
        '
        'cboDocType
        '
        Me.cboDocType.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboDocType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDocType.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.cboDocType.FormattingEnabled = True
        Me.cboDocType.Items.AddRange(New Object() {"", "Sales", "Purchase"})
        Me.cboDocType.Location = New System.Drawing.Point(113, 29)
        Me.cboDocType.MaxLength = 50
        Me.cboDocType.Name = "cboDocType"
        Me.cboDocType.Size = New System.Drawing.Size(90, 28)
        Me.cboDocType.TabIndex = 1
        '
        'lblMainFld
        '
        Me.lblMainFld.AutoSize = True
        Me.lblMainFld.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMainFld.ForeColor = System.Drawing.Color.Red
        Me.lblMainFld.Location = New System.Drawing.Point(12, 42)
        Me.lblMainFld.Name = "lblMainFld"
        Me.lblMainFld.Size = New System.Drawing.Size(82, 20)
        Me.lblMainFld.TabIndex = 255
        Me.lblMainFld.Tag = "M"
        Me.lblMainFld.Text = "Rate Type"
        '
        'cboRateType
        '
        Me.cboRateType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboRateType.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.cboRateType.FormattingEnabled = True
        Me.cboRateType.Items.AddRange(New Object() {"", "Standard Rate", "Avg Rate", "Last Purchase Rate"})
        Me.cboRateType.Location = New System.Drawing.Point(137, 37)
        Me.cboRateType.MaxLength = 50
        Me.cboRateType.Name = "cboRateType"
        Me.cboRateType.Size = New System.Drawing.Size(309, 28)
        Me.cboRateType.TabIndex = 2
        '
        'vsfDet
        '
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.vsfDet.Location = New System.Drawing.Point(12, 308)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.RowRange
        Me.vsfDet.Size = New System.Drawing.Size(643, 229)
        Me.vsfDet.TabIndex = 5
        '
        'txtCondText
        '
        Me.txtCondText.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCondText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCondText.Location = New System.Drawing.Point(12, 131)
        Me.txtCondText.MaxLength = 500
        Me.txtCondText.Multiline = True
        Me.txtCondText.Name = "txtCondText"
        Me.txtCondText.Size = New System.Drawing.Size(644, 132)
        Me.txtCondText.TabIndex = 6
        '
        'lblCondText
        '
        Me.lblCondText.AutoSize = True
        Me.lblCondText.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCondText.ForeColor = System.Drawing.Color.Black
        Me.lblCondText.Location = New System.Drawing.Point(12, 108)
        Me.lblCondText.Name = "lblCondText"
        Me.lblCondText.Size = New System.Drawing.Size(110, 20)
        Me.lblCondText.TabIndex = 265
        Me.lblCondText.Tag = ""
        Me.lblCondText.Text = "Condition Text"
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Image = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.Location = New System.Drawing.Point(473, 270)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(90, 30)
        Me.btnSave.TabIndex = 7
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Save : Alt + S")
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Image = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnCancel.Location = New System.Drawing.Point(566, 308)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(90, 30)
        Me.btnCancel.TabIndex = 8
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Cancel : Alt + C")
        Me.btnCancel.UseVisualStyleBackColor = False
        Me.btnCancel.Visible = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Image = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.Location = New System.Drawing.Point(566, 270)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(90, 30)
        Me.btnClose.TabIndex = 9
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close : Alt + O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btDocClose
        '
        Me.btDocClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btDocClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btDocClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btDocClose.Image = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btDocClose.Location = New System.Drawing.Point(110, 174)
        Me.btDocClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btDocClose.Name = "btDocClose"
        Me.btDocClose.Size = New System.Drawing.Size(90, 30)
        Me.btDocClose.TabIndex = 3
        Me.btDocClose.Text = "Cl&ose"
        Me.btDocClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btDocClose, "Close : Alt + O")
        Me.btDocClose.UseVisualStyleBackColor = False
        '
        'btnDocSave
        '
        Me.btnDocSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDocSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDocSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDocSave.Image = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnDocSave.Location = New System.Drawing.Point(12, 174)
        Me.btnDocSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnDocSave.Name = "btnDocSave"
        Me.btnDocSave.Size = New System.Drawing.Size(90, 30)
        Me.btnDocSave.TabIndex = 579
        Me.btnDocSave.Text = "&Save"
        Me.btnDocSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnDocSave, "Save : Alt + S")
        Me.btnDocSave.UseVisualStyleBackColor = False
        '
        'lblSearchFld
        '
        Me.lblSearchFld.AutoSize = True
        Me.lblSearchFld.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.lblSearchFld.ForeColor = System.Drawing.Color.Red
        Me.lblSearchFld.Location = New System.Drawing.Point(12, 75)
        Me.lblSearchFld.Name = "lblSearchFld"
        Me.lblSearchFld.Size = New System.Drawing.Size(82, 20)
        Me.lblSearchFld.TabIndex = 270
        Me.lblSearchFld.Tag = "M"
        Me.lblSearchFld.Text = "Date Field"
        '
        'vsfDoc
        '
        Me.vsfDoc.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfDoc.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfDoc.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDoc.ColumnInfo = "10,0,0,0,0,120,Columns:3{Width:122;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfDoc.ExtendLastCol = True
        Me.vsfDoc.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDoc.Location = New System.Drawing.Point(12, 65)
        Me.vsfDoc.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDoc.Name = "vsfDoc"
        Me.vsfDoc.Rows.DefaultSize = 24
        Me.vsfDoc.Size = New System.Drawing.Size(188, 25)
        Me.vsfDoc.StyleInfo = resources.GetString("vsfDoc.StyleInfo")
        Me.vsfDoc.TabIndex = 578
        '
        'gbDocuments
        '
        Me.gbDocuments.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbDocuments.Controls.Add(Me.txtDocCond)
        Me.gbDocuments.Controls.Add(Me.btnDocSave)
        Me.gbDocuments.Controls.Add(Me.btDocClose)
        Me.gbDocuments.Controls.Add(Me.vsfDoc)
        Me.gbDocuments.Controls.Add(Me.lblTblName)
        Me.gbDocuments.Controls.Add(Me.cboDocType)
        Me.gbDocuments.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbDocuments.Location = New System.Drawing.Point(452, 5)
        Me.gbDocuments.Name = "gbDocuments"
        Me.gbDocuments.Size = New System.Drawing.Size(207, 211)
        Me.gbDocuments.TabIndex = 10
        Me.gbDocuments.TabStop = False
        Me.gbDocuments.Text = "Documents"
        Me.gbDocuments.Visible = False
        '
        'txtDocCond
        '
        Me.txtDocCond.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDocCond.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDocCond.Location = New System.Drawing.Point(12, 99)
        Me.txtDocCond.MaxLength = 500
        Me.txtDocCond.Multiline = True
        Me.txtDocCond.Name = "txtDocCond"
        Me.txtDocCond.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDocCond.Size = New System.Drawing.Size(188, 68)
        Me.txtDocCond.TabIndex = 580
        '
        'txtDateFld
        '
        Me.txtDateFld.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDateFld.Location = New System.Drawing.Point(137, 71)
        Me.txtDateFld.Name = "txtDateFld"
        Me.txtDateFld.Size = New System.Drawing.Size(309, 26)
        Me.txtDateFld.TabIndex = 271
        '
        'ucBtnTools
        '
        Me.ucBtnTools.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ucBtnTools.BackColor = System.Drawing.Color.Transparent
        Me.ucBtnTools.Location = New System.Drawing.Point(12, 545)
        Me.ucBtnTools.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.ucBtnTools.Name = "ucBtnTools"
        Me.ucBtnTools.Size = New System.Drawing.Size(637, 29)
        Me.ucBtnTools.TabIndex = 269
        '
        'pnlCopyShortCut
        '
        Me.pnlCopyShortCut.Controls.Add(Me.btnCopy)
        Me.pnlCopyShortCut.Controls.Add(Me.pnlbtnClose)
        Me.pnlCopyShortCut.Controls.Add(Me.Label5)
        Me.pnlCopyShortCut.Controls.Add(Me.Label4)
        Me.pnlCopyShortCut.Controls.Add(Me.Label6)
        Me.pnlCopyShortCut.Controls.Add(Me.Label7)
        Me.pnlCopyShortCut.Controls.Add(Me.RbnCopyGrid)
        Me.pnlCopyShortCut.Controls.Add(Me.RbnCopyRow)
        Me.pnlCopyShortCut.Controls.Add(Me.RbnCopyColumn)
        Me.pnlCopyShortCut.Controls.Add(Me.RbnCopyCell)
        Me.pnlCopyShortCut.Location = New System.Drawing.Point(137, 337)
        Me.pnlCopyShortCut.Name = "pnlCopyShortCut"
        Me.pnlCopyShortCut.Size = New System.Drawing.Size(329, 177)
        Me.pnlCopyShortCut.TabIndex = 309
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
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(178, 79)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(77, 16)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Ctrl + Alt + C"
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
        'frmRptPMDesign
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(661, 587)
        Me.Controls.Add(Me.pnlCopyShortCut)
        Me.Controls.Add(Me.gbDocuments)
        Me.Controls.Add(Me.txtDateFld)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.lblSearchFld)
        Me.Controls.Add(Me.ucBtnTools)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.lblCondText)
        Me.Controls.Add(Me.txtCondText)
        Me.Controls.Add(Me.lblMainFld)
        Me.Controls.Add(Me.cboRateType)
        Me.Controls.Add(Me.txtDocName)
        Me.Controls.Add(Me.lblName)
        Me.Name = "frmRptPMDesign"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "PM Designer"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfDoc, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbDocuments.ResumeLayout(False)
        Me.gbDocuments.PerformLayout()
        Me.pnlCopyShortCut.ResumeLayout(False)
        Me.pnlCopyShortCut.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub



    Friend WithEvents txtDocName As TextBox
    Friend WithEvents lblName As Label
    Friend WithEvents lblTblName As Label
    Friend WithEvents cboDocType As ComboBox
    Friend WithEvents lblMainFld As Label
    Friend WithEvents cboRateType As ComboBox
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents txtCondText As TextBox
    Friend WithEvents lblCondText As Label
    Friend WithEvents btnSave As C1.Win.C1Input.C1Button
    Friend WithEvents btnCancel As C1.Win.C1Input.C1Button
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents ucBtnTools As ucBtn
    Friend WithEvents lblSearchFld As Label
    Friend WithEvents vsfDoc As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents gbDocuments As GroupBox
    Friend WithEvents btDocClose As C1.Win.C1Input.C1Button
    Friend WithEvents txtDateFld As TextBox
    Friend WithEvents btnDocSave As C1.Win.C1Input.C1Button
    Friend WithEvents txtDocCond As TextBox
    Friend WithEvents pnlCopyShortCut As Panel
    Friend WithEvents btnCopy As Button
    Friend WithEvents pnlbtnClose As Button
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents RbnCopyGrid As RadioButton
    Friend WithEvents RbnCopyRow As RadioButton
    Friend WithEvents RbnCopyColumn As RadioButton
    Friend WithEvents RbnCopyCell As RadioButton
End Class
