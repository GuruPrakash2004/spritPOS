<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlImportXL
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlImportXL))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtXLFilePath = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.vsfTmp = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.lblError = New System.Windows.Forms.Label()
        Me.Cdlg = New System.Windows.Forms.OpenFileDialog()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.optAppnd = New System.Windows.Forms.RadioButton()
        Me.optDel = New System.Windows.Forms.RadioButton()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.vsfXL = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.vsfExpTemp = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnImpClose = New System.Windows.Forms.Button()
        Me.btnExportToXL = New System.Windows.Forms.Button()
        Me.btnRemErr = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnGenTemplate = New System.Windows.Forms.Button()
        Me.btnImportFromXL = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.vsf = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnLoadTemplate = New System.Windows.Forms.Button()
        Me.chkClientXL = New System.Windows.Forms.CheckBox()
        Me.pnlImportClientXL = New System.Windows.Forms.Panel()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnRefersh = New System.Windows.Forms.Button()
        Me.txtRowStart = New System.Windows.Forms.TextBox()
        Me.txtColHdr = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.lblRecNo = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cboActionType = New vLQMS.ComboEz()
        Me.cboDoc = New vLQMS.ComboEz()
        CType(Me.vsfTmp, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfXL, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfExpTemp, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsf, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlImportClientXL.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(20, 94)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(125, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Select Excel File"
        '
        'txtXLFilePath
        '
        Me.txtXLFilePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtXLFilePath.Enabled = False
        Me.txtXLFilePath.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtXLFilePath.Location = New System.Drawing.Point(145, 92)
        Me.txtXLFilePath.Name = "txtXLFilePath"
        Me.txtXLFilePath.Size = New System.Drawing.Size(380, 26)
        Me.txtXLFilePath.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(21, 151)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(252, 20)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Total Number of Records Loaded :"
        '
        'vsfTmp
        '
        Me.vsfTmp.ColumnInfo = "10,1,0,0,0,95,Columns:"
        Me.vsfTmp.Location = New System.Drawing.Point(924, 12)
        Me.vsfTmp.Name = "vsfTmp"
        Me.vsfTmp.Rows.DefaultSize = 19
        Me.vsfTmp.Size = New System.Drawing.Size(58, 36)
        Me.vsfTmp.TabIndex = 9
        Me.vsfTmp.Visible = False
        '
        'lblError
        '
        Me.lblError.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblError.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblError.ForeColor = System.Drawing.Color.Red
        Me.lblError.Location = New System.Drawing.Point(31, 606)
        Me.lblError.Name = "lblError"
        Me.lblError.Size = New System.Drawing.Size(10, 13)
        Me.lblError.TabIndex = 10
        '
        'Cdlg
        '
        Me.Cdlg.FileName = "Cdlg"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Blue
        Me.Label4.Location = New System.Drawing.Point(142, 125)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(396, 20)
        Me.Label4.TabIndex = 16
        Me.Label4.Text = "Please select a excel file containing Item Details"
        '
        'optAppnd
        '
        Me.optAppnd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.optAppnd.AutoSize = True
        Me.optAppnd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optAppnd.ForeColor = System.Drawing.Color.Red
        Me.optAppnd.Location = New System.Drawing.Point(988, 12)
        Me.optAppnd.Name = "optAppnd"
        Me.optAppnd.Size = New System.Drawing.Size(152, 17)
        Me.optAppnd.TabIndex = 20
        Me.optAppnd.TabStop = True
        Me.optAppnd.Text = "Append Previous Data"
        Me.optAppnd.UseVisualStyleBackColor = True
        Me.optAppnd.Visible = False
        '
        'optDel
        '
        Me.optDel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.optDel.AutoSize = True
        Me.optDel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optDel.ForeColor = System.Drawing.Color.Red
        Me.optDel.Location = New System.Drawing.Point(988, 40)
        Me.optDel.Name = "optDel"
        Me.optDel.Size = New System.Drawing.Size(146, 17)
        Me.optDel.TabIndex = 21
        Me.optDel.TabStop = True
        Me.optDel.Text = "Delete Previous Data"
        Me.optDel.UseVisualStyleBackColor = True
        Me.optDel.Visible = False
        '
        'btnBrowse
        '
        Me.btnBrowse.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnBrowse.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBrowse.Location = New System.Drawing.Point(531, 88)
        Me.btnBrowse.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(40, 29)
        Me.btnBrowse.TabIndex = 3
        Me.btnBrowse.Text = "..."
        Me.btnBrowse.UseVisualStyleBackColor = False
        '
        'vsfXL
        '
        Me.vsfXL.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfXL.ColumnInfo = resources.GetString("vsfXL.ColumnInfo")
        Me.vsfXL.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfXL.Location = New System.Drawing.Point(24, 177)
        Me.vsfXL.Name = "vsfXL"
        Me.vsfXL.Rows.DefaultSize = 24
        Me.vsfXL.Rows.Fixed = 6
        Me.vsfXL.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfXL.Size = New System.Drawing.Size(1098, 377)
        Me.vsfXL.StyleInfo = resources.GetString("vsfXL.StyleInfo")
        Me.vsfXL.TabIndex = 7
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(21, 606)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(416, 20)
        Me.Label5.TabIndex = 33
        Me.Label5.Text = "Importing of data is not allowed until all the data is correct."
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(21, 569)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(415, 20)
        Me.Label3.TabIndex = 32
        Me.Label3.Text = "Records displayed with           coloured font are erroneous."
        Me.Label3.Visible = False
        '
        'Label6
        '
        Me.Label6.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Red
        Me.Label6.Location = New System.Drawing.Point(192, 569)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(35, 20)
        Me.Label6.TabIndex = 34
        Me.Label6.Text = "red"
        Me.Label6.Visible = False
        '
        'vsfExpTemp
        '
        Me.vsfExpTemp.AllowEditing = False
        Me.vsfExpTemp.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
        Me.vsfExpTemp.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfExpTemp.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfExpTemp.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfExpTemp.ColumnInfo = "3,0,0,0,0,95,Columns:0{Caption:"" "";}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfExpTemp.ExtendLastCol = True
        Me.vsfExpTemp.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Solid
        Me.vsfExpTemp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.vsfExpTemp.Location = New System.Drawing.Point(617, 315)
        Me.vsfExpTemp.Name = "vsfExpTemp"
        Me.vsfExpTemp.Rows.Count = 4
        Me.vsfExpTemp.Rows.DefaultSize = 19
        Me.vsfExpTemp.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfExpTemp.Size = New System.Drawing.Size(488, 185)
        Me.vsfExpTemp.StyleInfo = resources.GetString("vsfExpTemp.StyleInfo")
        Me.vsfExpTemp.TabIndex = 207
        Me.vsfExpTemp.Tag = "DSGR"
        Me.vsfExpTemp.Visible = False
        '
        'btnImpClose
        '
        Me.btnImpClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnImpClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnImpClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnImpClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnImpClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnImpClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnImpClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnImpClose.Location = New System.Drawing.Point(1045, 598)
        Me.btnImpClose.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnImpClose.Name = "btnImpClose"
        Me.btnImpClose.Size = New System.Drawing.Size(77, 29)
        Me.btnImpClose.TabIndex = 11
        Me.btnImpClose.Text = "&Close"
        Me.btnImpClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnImpClose, "Close:Alt+C")
        Me.btnImpClose.UseVisualStyleBackColor = False
        '
        'btnExportToXL
        '
        Me.btnExportToXL.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExportToXL.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnExportToXL.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExportToXL.Image = CType(resources.GetObject("btnExportToXL.Image"), System.Drawing.Image)
        Me.btnExportToXL.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExportToXL.Location = New System.Drawing.Point(898, 598)
        Me.btnExportToXL.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnExportToXL.Name = "btnExportToXL"
        Me.btnExportToXL.Size = New System.Drawing.Size(141, 29)
        Me.btnExportToXL.TabIndex = 10
        Me.btnExportToXL.Text = "&Export to excel"
        Me.btnExportToXL.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnExportToXL, "Export to Excel:Alt+E")
        Me.btnExportToXL.UseVisualStyleBackColor = False
        '
        'btnRemErr
        '
        Me.btnRemErr.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRemErr.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnRemErr.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRemErr.Image = CType(resources.GetObject("btnRemErr.Image"), System.Drawing.Image)
        Me.btnRemErr.ImageAlign = System.Drawing.ContentAlignment.BottomLeft
        Me.btnRemErr.Location = New System.Drawing.Point(700, 598)
        Me.btnRemErr.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnRemErr.Name = "btnRemErr"
        Me.btnRemErr.Size = New System.Drawing.Size(192, 29)
        Me.btnRemErr.TabIndex = 9
        Me.btnRemErr.Text = "&Remove erroneous records"
        Me.btnRemErr.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRemErr.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Image = CType(resources.GetObject("btnSave.Image"), System.Drawing.Image)
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(898, 563)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(224, 29)
        Me.btnSave.TabIndex = 8
        Me.btnSave.Text = "Import data to database"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnGenTemplate
        '
        Me.btnGenTemplate.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnGenTemplate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGenTemplate.Image = CType(resources.GetObject("btnGenTemplate.Image"), System.Drawing.Image)
        Me.btnGenTemplate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGenTemplate.Location = New System.Drawing.Point(603, 54)
        Me.btnGenTemplate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnGenTemplate.Name = "btnGenTemplate"
        Me.btnGenTemplate.Size = New System.Drawing.Size(173, 29)
        Me.btnGenTemplate.TabIndex = 4
        Me.btnGenTemplate.Text = "Generate &Template"
        Me.btnGenTemplate.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGenTemplate.UseVisualStyleBackColor = False
        '
        'btnImportFromXL
        '
        Me.btnImportFromXL.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnImportFromXL.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnImportFromXL.Image = CType(resources.GetObject("btnImportFromXL.Image"), System.Drawing.Image)
        Me.btnImportFromXL.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnImportFromXL.Location = New System.Drawing.Point(603, 88)
        Me.btnImportFromXL.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnImportFromXL.Name = "btnImportFromXL"
        Me.btnImportFromXL.Size = New System.Drawing.Size(173, 29)
        Me.btnImportFromXL.TabIndex = 5
        Me.btnImportFromXL.Text = "&Load from Excel"
        Me.btnImportFromXL.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnImportFromXL.UseVisualStyleBackColor = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Black
        Me.Label7.Location = New System.Drawing.Point(21, 32)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(83, 20)
        Me.Label7.TabIndex = 209
        Me.Label7.Text = "Document"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.Black
        Me.Label8.Location = New System.Drawing.Point(21, 62)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(92, 20)
        Me.Label8.TabIndex = 211
        Me.Label8.Text = "Action Type"
        '
        'vsf
        '
        Me.vsf.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
        Me.vsf.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.MultiColumn
        Me.vsf.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsf.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsf.ColumnInfo = "3,0,0,0,0,95,Columns:0{Caption:"" "";}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsf.ExtendLastCol = True
        Me.vsf.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Solid
        Me.vsf.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.vsf.Location = New System.Drawing.Point(9, 84)
        Me.vsf.Name = "vsf"
        Me.vsf.Rows.Count = 1
        Me.vsf.Rows.DefaultSize = 19
        Me.vsf.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsf.Size = New System.Drawing.Size(455, 370)
        Me.vsf.StyleInfo = resources.GetString("vsf.StyleInfo")
        Me.vsf.TabIndex = 212
        Me.vsf.Tag = ""
        '
        'btnLoadTemplate
        '
        Me.btnLoadTemplate.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnLoadTemplate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoadTemplate.Location = New System.Drawing.Point(531, 56)
        Me.btnLoadTemplate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnLoadTemplate.Name = "btnLoadTemplate"
        Me.btnLoadTemplate.Size = New System.Drawing.Size(40, 29)
        Me.btnLoadTemplate.TabIndex = 2
        Me.btnLoadTemplate.Text = "..."
        Me.btnLoadTemplate.UseVisualStyleBackColor = False
        '
        'chkClientXL
        '
        Me.chkClientXL.AutoSize = True
        Me.chkClientXL.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkClientXL.Location = New System.Drawing.Point(784, 96)
        Me.chkClientXL.Name = "chkClientXL"
        Me.chkClientXL.Size = New System.Drawing.Size(210, 24)
        Me.chkClientXL.TabIndex = 6
        Me.chkClientXL.Text = "Not according to template"
        Me.chkClientXL.UseVisualStyleBackColor = True
        '
        'pnlImportClientXL
        '
        Me.pnlImportClientXL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlImportClientXL.Controls.Add(Me.btnCancel)
        Me.pnlImportClientXL.Controls.Add(Me.btnOK)
        Me.pnlImportClientXL.Controls.Add(Me.btnRefersh)
        Me.pnlImportClientXL.Controls.Add(Me.txtRowStart)
        Me.pnlImportClientXL.Controls.Add(Me.txtColHdr)
        Me.pnlImportClientXL.Controls.Add(Me.vsf)
        Me.pnlImportClientXL.Controls.Add(Me.Label12)
        Me.pnlImportClientXL.Controls.Add(Me.Label11)
        Me.pnlImportClientXL.Controls.Add(Me.Label10)
        Me.pnlImportClientXL.Controls.Add(Me.Label9)
        Me.pnlImportClientXL.Controls.Add(Me.lblStatus)
        Me.pnlImportClientXL.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlImportClientXL.Location = New System.Drawing.Point(386, 96)
        Me.pnlImportClientXL.Name = "pnlImportClientXL"
        Me.pnlImportClientXL.Size = New System.Drawing.Size(471, 499)
        Me.pnlImportClientXL.TabIndex = 5
        Me.pnlImportClientXL.Visible = False
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(386, 469)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(80, 25)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOK.Location = New System.Drawing.Point(300, 469)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(80, 25)
        Me.btnOK.TabIndex = 0
        Me.btnOK.Text = "Ok"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnRefersh
        '
        Me.btnRefersh.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRefersh.Location = New System.Drawing.Point(284, 28)
        Me.btnRefersh.Name = "btnRefersh"
        Me.btnRefersh.Size = New System.Drawing.Size(24, 23)
        Me.btnRefersh.TabIndex = 212
        Me.btnRefersh.Text = "..."
        Me.btnRefersh.UseVisualStyleBackColor = True
        '
        'txtRowStart
        '
        Me.txtRowStart.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRowStart.Location = New System.Drawing.Point(200, 56)
        Me.txtRowStart.Name = "txtRowStart"
        Me.txtRowStart.Size = New System.Drawing.Size(76, 26)
        Me.txtRowStart.TabIndex = 211
        Me.txtRowStart.Text = "2"
        Me.txtRowStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtColHdr
        '
        Me.txtColHdr.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtColHdr.Location = New System.Drawing.Point(200, 25)
        Me.txtColHdr.Name = "txtColHdr"
        Me.txtColHdr.Size = New System.Drawing.Size(76, 26)
        Me.txtColHdr.TabIndex = 211
        Me.txtColHdr.Text = "1"
        Me.txtColHdr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.Color.Black
        Me.Label12.Location = New System.Drawing.Point(179, 4)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(113, 20)
        Me.Label12.TabIndex = 210
        Me.Label12.Text = "0 if no heading"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.Black
        Me.Label11.Location = New System.Drawing.Point(7, 59)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(192, 20)
        Me.Label11.TabIndex = 210
        Me.Label11.Text = "Data Starts From Row No"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.Black
        Me.Label10.Location = New System.Drawing.Point(7, 29)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(187, 20)
        Me.Label10.TabIndex = 210
        Me.Label10.Text = "Column Heading Row No"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.Black
        Me.Label9.Location = New System.Drawing.Point(42, 16)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(0, 16)
        Me.Label9.TabIndex = 19
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.Color.Red
        Me.lblStatus.Location = New System.Drawing.Point(155, 16)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(0, 13)
        Me.lblStatus.TabIndex = 18
        '
        'lblRecNo
        '
        Me.lblRecNo.AutoSize = True
        Me.lblRecNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRecNo.ForeColor = System.Drawing.Color.Maroon
        Me.lblRecNo.Location = New System.Drawing.Point(270, 154)
        Me.lblRecNo.Name = "lblRecNo"
        Me.lblRecNo.Size = New System.Drawing.Size(18, 20)
        Me.lblRecNo.TabIndex = 216
        Me.lblRecNo.Text = "0"
        '
        'cboActionType
        '
        Me.cboActionType.CueBannerAlways = False
        Me.cboActionType.CueBannerText = ""
        Me.cboActionType.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboActionType.FormattingEnabled = True
        Me.cboActionType.Items.AddRange(New Object() {"Import from XL", "Update from XL", "Update from Database"})
        Me.cboActionType.Location = New System.Drawing.Point(145, 59)
        Me.cboActionType.Name = "cboActionType"
        Me.cboActionType.Size = New System.Drawing.Size(380, 28)
        Me.cboActionType.TabIndex = 1
        '
        'cboDoc
        '
        Me.cboDoc.CueBannerAlways = False
        Me.cboDoc.CueBannerText = ""
        Me.cboDoc.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboDoc.FormattingEnabled = True
        Me.cboDoc.Location = New System.Drawing.Point(145, 29)
        Me.cboDoc.Name = "cboDoc"
        Me.cboDoc.Size = New System.Drawing.Size(380, 28)
        Me.cboDoc.TabIndex = 0
        '
        'frmUtlImportXL
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1134, 637)
        Me.Controls.Add(Me.pnlImportClientXL)
        Me.Controls.Add(Me.lblRecNo)
        Me.Controls.Add(Me.chkClientXL)
        Me.Controls.Add(Me.btnLoadTemplate)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.cboActionType)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.cboDoc)
        Me.Controls.Add(Me.vsfExpTemp)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.btnImpClose)
        Me.Controls.Add(Me.btnExportToXL)
        Me.Controls.Add(Me.btnRemErr)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.vsfXL)
        Me.Controls.Add(Me.btnGenTemplate)
        Me.Controls.Add(Me.btnImportFromXL)
        Me.Controls.Add(Me.btnBrowse)
        Me.Controls.Add(Me.optDel)
        Me.Controls.Add(Me.optAppnd)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.lblError)
        Me.Controls.Add(Me.vsfTmp)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtXLFilePath)
        Me.Controls.Add(Me.Label1)
        Me.MinimumSize = New System.Drawing.Size(930, 643)
        Me.Name = "frmUtlImportXL"
        Me.ShowIcon = False
        Me.Text = "Import Data from Excel..."
        CType(Me.vsfTmp, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfXL, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfExpTemp, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsf, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlImportClientXL.ResumeLayout(False)
        Me.pnlImportClientXL.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtXLFilePath As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents vsfTmp As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents lblError As System.Windows.Forms.Label
    Friend WithEvents Cdlg As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents optAppnd As System.Windows.Forms.RadioButton
    Friend WithEvents optDel As System.Windows.Forms.RadioButton
    Friend WithEvents btnGenTemplate As System.Windows.Forms.Button
    Friend WithEvents btnImportFromXL As System.Windows.Forms.Button
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents vsfXL As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnExportToXL As System.Windows.Forms.Button
    Friend WithEvents btnRemErr As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnImpClose As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents vsfExpTemp As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents cboDoc As vLQMS.ComboEz
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cboActionType As vLQMS.ComboEz
    Friend WithEvents vsf As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnLoadTemplate As System.Windows.Forms.Button
    Friend WithEvents chkClientXL As CheckBox
    Friend WithEvents pnlImportClientXL As Panel
    Friend WithEvents Label9 As Label
    Friend WithEvents lblStatus As Label
    Friend WithEvents txtRowStart As TextBox
    Friend WithEvents txtColHdr As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents btnRefersh As Button
    Friend WithEvents Label12 As Label
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnOK As Button
    Friend WithEvents lblRecNo As Label
    Friend WithEvents ToolTip1 As ToolTip
End Class
