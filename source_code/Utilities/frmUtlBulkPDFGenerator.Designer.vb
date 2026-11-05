<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlBulkPDFGenerator
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlBulkPDFGenerator))
        Me.lblTrn = New System.Windows.Forms.Label()
        Me.cboTrn = New System.Windows.Forms.ComboBox()
        Me.cboPrintType = New System.Windows.Forms.ComboBox()
        Me.lblPrintType = New System.Windows.Forms.Label()
        Me.lblTo = New System.Windows.Forms.Label()
        Me.lblFrom = New System.Windows.Forms.Label()
        Me.dtpToDate = New System.Windows.Forms.DateTimePicker()
        Me.dtpFromDate = New System.Windows.Forms.DateTimePicker()
        Me.lblPath = New System.Windows.Forms.Label()
        Me.txtPath = New System.Windows.Forms.TextBox()
        Me.btnLoadPath = New System.Windows.Forms.Button()
        Me.chkParty = New System.Windows.Forms.CheckBox()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.btnGENPDF = New C1.Win.C1Input.C1Button()
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.lblTolRec = New System.Windows.Forms.Label()
        Me.lblExpVsRec = New System.Windows.Forms.Label()
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
        Me.Panel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.pnlCopyShortCut.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblTrn
        '
        Me.lblTrn.AutoSize = True
        Me.lblTrn.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTrn.Location = New System.Drawing.Point(8, 9)
        Me.lblTrn.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblTrn.Name = "lblTrn"
        Me.lblTrn.Size = New System.Drawing.Size(74, 17)
        Me.lblTrn.TabIndex = 0
        Me.lblTrn.Tag = "M"
        Me.lblTrn.Text = "Transaction"
        '
        'cboTrn
        '
        Me.cboTrn.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboTrn.FormattingEnabled = True
        Me.cboTrn.Location = New System.Drawing.Point(141, 5)
        Me.cboTrn.Name = "cboTrn"
        Me.cboTrn.Size = New System.Drawing.Size(360, 25)
        Me.cboTrn.TabIndex = 0
        '
        'cboPrintType
        '
        Me.cboPrintType.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPrintType.FormattingEnabled = True
        Me.cboPrintType.Location = New System.Drawing.Point(141, 39)
        Me.cboPrintType.Name = "cboPrintType"
        Me.cboPrintType.Size = New System.Drawing.Size(360, 25)
        Me.cboPrintType.TabIndex = 1
        '
        'lblPrintType
        '
        Me.lblPrintType.AutoSize = True
        Me.lblPrintType.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPrintType.Location = New System.Drawing.Point(8, 43)
        Me.lblPrintType.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblPrintType.Name = "lblPrintType"
        Me.lblPrintType.Size = New System.Drawing.Size(103, 17)
        Me.lblPrintType.TabIndex = 2
        Me.lblPrintType.Tag = "M"
        Me.lblPrintType.Text = "Select Print Type"
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTo.Location = New System.Drawing.Point(278, 29)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(22, 17)
        Me.lblTo.TabIndex = 2
        Me.lblTo.Text = "To"
        '
        'lblFrom
        '
        Me.lblFrom.AutoSize = True
        Me.lblFrom.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFrom.Location = New System.Drawing.Point(110, 29)
        Me.lblFrom.Name = "lblFrom"
        Me.lblFrom.Size = New System.Drawing.Size(38, 17)
        Me.lblFrom.TabIndex = 0
        Me.lblFrom.Text = "From"
        '
        'dtpToDate
        '
        Me.dtpToDate.CustomFormat = "dd/MM/yyyy"
        Me.dtpToDate.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpToDate.Location = New System.Drawing.Point(307, 25)
        Me.dtpToDate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtpToDate.Name = "dtpToDate"
        Me.dtpToDate.Size = New System.Drawing.Size(114, 25)
        Me.dtpToDate.TabIndex = 1
        '
        'dtpFromDate
        '
        Me.dtpFromDate.CustomFormat = "dd/MM/yyyy"
        Me.dtpFromDate.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpFromDate.Location = New System.Drawing.Point(154, 25)
        Me.dtpFromDate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtpFromDate.Name = "dtpFromDate"
        Me.dtpFromDate.Size = New System.Drawing.Size(114, 25)
        Me.dtpFromDate.TabIndex = 0
        '
        'lblPath
        '
        Me.lblPath.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblPath.AutoSize = True
        Me.lblPath.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPath.Location = New System.Drawing.Point(2, 496)
        Me.lblPath.Name = "lblPath"
        Me.lblPath.Size = New System.Drawing.Size(100, 17)
        Me.lblPath.TabIndex = 4
        Me.lblPath.Tag = "M"
        Me.lblPath.Text = "PDF Folder Path"
        Me.ToolTip1.SetToolTip(Me.lblPath, "Click here to open selected folder")
        '
        'txtPath
        '
        Me.txtPath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPath.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPath.Location = New System.Drawing.Point(134, 493)
        Me.txtPath.MaxLength = 100
        Me.txtPath.Name = "txtPath"
        Me.txtPath.Size = New System.Drawing.Size(686, 25)
        Me.txtPath.TabIndex = 2
        '
        'btnLoadPath
        '
        Me.btnLoadPath.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLoadPath.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoadPath.Location = New System.Drawing.Point(826, 493)
        Me.btnLoadPath.Name = "btnLoadPath"
        Me.btnLoadPath.Size = New System.Drawing.Size(35, 26)
        Me.btnLoadPath.TabIndex = 3
        Me.btnLoadPath.Text = "..."
        Me.btnLoadPath.UseVisualStyleBackColor = True
        '
        'chkParty
        '
        Me.chkParty.AutoSize = True
        Me.chkParty.Enabled = False
        Me.chkParty.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkParty.Location = New System.Drawing.Point(15, 122)
        Me.chkParty.Margin = New System.Windows.Forms.Padding(4)
        Me.chkParty.Name = "chkParty"
        Me.chkParty.Size = New System.Drawing.Size(94, 21)
        Me.chkParty.TabIndex = 0
        Me.chkParty.Text = "Select Party"
        Me.chkParty.UseVisualStyleBackColor = True
        '
        'vsfDet
        '
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.ColumnInfo = "5,0,0,0,0,125,Columns:"
        Me.vsfDet.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.vsfDet.Location = New System.Drawing.Point(5, 231)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(2)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.Count = 1
        Me.vsfDet.Rows.DefaultSize = 25
        Me.vsfDet.Size = New System.Drawing.Size(856, 223)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 1
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.lblTrn)
        Me.Panel1.Controls.Add(Me.cboTrn)
        Me.Panel1.Controls.Add(Me.lblPrintType)
        Me.Panel1.Controls.Add(Me.cboPrintType)
        Me.Panel1.Location = New System.Drawing.Point(4, 33)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(857, 77)
        Me.Panel1.TabIndex = 1
        '
        'lblTitle
        '
        Me.lblTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(53, Byte), Integer), CType(CType(73, Byte), Integer), CType(CType(106, Byte), Integer))
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.ForeColor = System.Drawing.Color.White
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(865, 31)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Bulk PDF Generator"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.btnLoad)
        Me.GroupBox1.Controls.Add(Me.lblFrom)
        Me.GroupBox1.Controls.Add(Me.dtpFromDate)
        Me.GroupBox1.Controls.Add(Me.dtpToDate)
        Me.GroupBox1.Controls.Add(Me.lblTo)
        Me.GroupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.GroupBox1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.ForeColor = System.Drawing.Color.Black
        Me.GroupBox1.Location = New System.Drawing.Point(4, 156)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(857, 70)
        Me.GroupBox1.TabIndex = 3
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Date Range"
        '
        'btnLoad
        '
        Me.btnLoad.Location = New System.Drawing.Point(427, 25)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(75, 25)
        Me.btnLoad.TabIndex = 2
        Me.btnLoad.Text = "&Load"
        Me.ToolTip1.SetToolTip(Me.btnLoad, "Load : Atl + L")
        Me.btnLoad.UseVisualStyleBackColor = True
        '
        'btnGENPDF
        '
        Me.btnGENPDF.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGENPDF.BackColor = System.Drawing.Color.LimeGreen
        Me.btnGENPDF.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnGENPDF.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGENPDF.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGENPDF.Image = Global.vLQMS.My.Resources.Resources.DownloadPDF
        Me.btnGENPDF.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGENPDF.Location = New System.Drawing.Point(621, 536)
        Me.btnGENPDF.Margin = New System.Windows.Forms.Padding(4)
        Me.btnGENPDF.Name = "btnGENPDF"
        Me.btnGENPDF.Size = New System.Drawing.Size(147, 40)
        Me.btnGENPDF.TabIndex = 4
        Me.btnGENPDF.Text = "&Download"
        Me.btnGENPDF.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnGENPDF, "Download : Alt + D")
        Me.btnGENPDF.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(776, 536)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(85, 40)
        Me.btnClose.TabIndex = 5
        Me.btnClose.Text = "&Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'lblTolRec
        '
        Me.lblTolRec.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTolRec.AutoSize = True
        Me.lblTolRec.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTolRec.Location = New System.Drawing.Point(2, 456)
        Me.lblTolRec.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblTolRec.Name = "lblTolRec"
        Me.lblTolRec.Size = New System.Drawing.Size(103, 17)
        Me.lblTolRec.TabIndex = 6
        Me.lblTolRec.Tag = "R"
        Me.lblTolRec.Text = "Total records : 0"
        '
        'lblExpVsRec
        '
        Me.lblExpVsRec.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblExpVsRec.AutoSize = True
        Me.lblExpVsRec.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExpVsRec.Location = New System.Drawing.Point(670, 456)
        Me.lblExpVsRec.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblExpVsRec.Name = "lblExpVsRec"
        Me.lblExpVsRec.Size = New System.Drawing.Size(191, 17)
        Me.lblExpVsRec.TabIndex = 7
        Me.lblExpVsRec.Tag = "R"
        Me.lblExpVsRec.Text = "Exported/Record Selected : 0/0"
        Me.lblExpVsRec.TextAlign = System.Drawing.ContentAlignment.TopRight
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
        Me.pnlCopyShortCut.Location = New System.Drawing.Point(239, 262)
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
        'frmUtlBulkPDFGenerator
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(865, 589)
        Me.Controls.Add(Me.pnlCopyShortCut)
        Me.Controls.Add(Me.lblExpVsRec)
        Me.Controls.Add(Me.lblTolRec)
        Me.Controls.Add(Me.btnGENPDF)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.chkParty)
        Me.Controls.Add(Me.btnLoadPath)
        Me.Controls.Add(Me.txtPath)
        Me.Controls.Add(Me.lblPath)
        Me.KeyPreview = True
        Me.Name = "frmUtlBulkPDFGenerator"
        Me.ShowIcon = False
        Me.Text = "PDF Generator - Bulk"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.pnlCopyShortCut.ResumeLayout(False)
        Me.pnlCopyShortCut.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblTrn As Label
    Friend WithEvents cboTrn As ComboBox
    Friend WithEvents cboPrintType As ComboBox
    Friend WithEvents lblPrintType As Label
    Friend WithEvents lblTo As Label
    Friend WithEvents lblFrom As Label
    Friend WithEvents dtpToDate As DateTimePicker
    Friend WithEvents dtpFromDate As DateTimePicker
    Friend WithEvents lblPath As Label
    Friend WithEvents txtPath As TextBox
    Friend WithEvents btnLoadPath As Button
    Friend WithEvents chkParty As CheckBox
    Private WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents Panel1 As Panel
    Friend WithEvents lblTitle As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents btnLoad As Button
    Friend WithEvents btnGENPDF As C1.Win.C1Input.C1Button
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents lblTolRec As Label
    Friend WithEvents lblExpVsRec As Label
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
