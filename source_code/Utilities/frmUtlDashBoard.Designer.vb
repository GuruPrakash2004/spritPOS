<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlDashboard
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlDashboard))
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Title1 As System.Windows.Forms.DataVisualization.Charting.Title = New System.Windows.Forms.DataVisualization.Charting.Title()
        Dim ChartArea2 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend2 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series2 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series3 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Title2 As System.Windows.Forms.DataVisualization.Charting.Title = New System.Windows.Forms.DataVisualization.Charting.Title()
        Me.Tbllout = New System.Windows.Forms.TableLayoutPanel()
        Me.pnl4 = New System.Windows.Forms.Panel()
        Me.vsfDet4 = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnXLMisc = New System.Windows.Forms.Button()
        Me.lbl4 = New System.Windows.Forms.Label()
        Me.pnl3 = New System.Windows.Forms.Panel()
        Me.VsfDet3 = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnXLStock = New System.Windows.Forms.Button()
        Me.lbl3 = New System.Windows.Forms.Label()
        Me.pnl2 = New System.Windows.Forms.Panel()
        Me.vsfDet2 = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnXLPB = New System.Windows.Forms.Button()
        Me.lbl2 = New System.Windows.Forms.Label()
        Me.pnl1 = New System.Windows.Forms.Panel()
        Me.vsfDet1 = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnXLSales = New System.Windows.Forms.Button()
        Me.lbl1 = New System.Windows.Forms.Label()
        Me.ChartExpense = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.ChartSaleVsPB = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.lblSettings = New System.Windows.Forms.Label()
        Me.btnSettings = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.lblRefresh = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Tbllout.SuspendLayout()
        Me.pnl4.SuspendLayout()
        CType(Me.vsfDet4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnl3.SuspendLayout()
        CType(Me.VsfDet3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnl2.SuspendLayout()
        CType(Me.vsfDet2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnl1.SuspendLayout()
        CType(Me.vsfDet1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ChartExpense, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ChartSaleVsPB, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Tbllout
        '
        Me.Tbllout.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Tbllout.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Tbllout.ColumnCount = 2
        Me.Tbllout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.6358!))
        Me.Tbllout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.3642!))
        Me.Tbllout.Controls.Add(Me.pnl4, 1, 1)
        Me.Tbllout.Controls.Add(Me.pnl3, 0, 1)
        Me.Tbllout.Controls.Add(Me.pnl2, 1, 0)
        Me.Tbllout.Controls.Add(Me.pnl1, 0, 0)
        Me.Tbllout.Controls.Add(Me.ChartExpense, 0, 2)
        Me.Tbllout.Controls.Add(Me.ChartSaleVsPB, 1, 2)
        Me.Tbllout.Location = New System.Drawing.Point(10, 11)
        Me.Tbllout.Name = "Tbllout"
        Me.Tbllout.RowCount = 3
        Me.Tbllout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.85066!))
        Me.Tbllout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 49.14934!))
        Me.Tbllout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 158.0!))
        Me.Tbllout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21.0!))
        Me.Tbllout.Size = New System.Drawing.Size(963, 600)
        Me.Tbllout.TabIndex = 19
        '
        'pnl4
        '
        Me.pnl4.BackColor = System.Drawing.Color.FromArgb(CType(CType(90, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(163, Byte), Integer))
        Me.pnl4.Controls.Add(Me.vsfDet4)
        Me.pnl4.Controls.Add(Me.btnXLMisc)
        Me.pnl4.Controls.Add(Me.lbl4)
        Me.pnl4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnl4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.pnl4.Location = New System.Drawing.Point(480, 227)
        Me.pnl4.Name = "pnl4"
        Me.pnl4.Size = New System.Drawing.Size(480, 211)
        Me.pnl4.TabIndex = 216
        '
        'vsfDet4
        '
        Me.vsfDet4.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet4.BackColor = System.Drawing.Color.FromArgb(CType(CType(90, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(163, Byte), Integer))
        Me.vsfDet4.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None
        Me.vsfDet4.ColumnInfo = resources.GetString("vsfDet4.ColumnInfo")
        Me.vsfDet4.ExtendLastCol = True
        Me.vsfDet4.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet4.HighLight = C1.Win.C1FlexGrid.HighLightEnum.Never
        Me.vsfDet4.Location = New System.Drawing.Point(6, 34)
        Me.vsfDet4.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet4.Name = "vsfDet4"
        Me.vsfDet4.Rows.DefaultSize = 24
        Me.vsfDet4.Size = New System.Drawing.Size(471, 138)
        Me.vsfDet4.StyleInfo = resources.GetString("vsfDet4.StyleInfo")
        Me.vsfDet4.TabIndex = 257
        '
        'btnXLMisc
        '
        Me.btnXLMisc.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnXLMisc.BackColor = System.Drawing.Color.Transparent
        Me.btnXLMisc.FlatAppearance.BorderSize = 0
        Me.btnXLMisc.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.btnXLMisc.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.btnXLMisc.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnXLMisc.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnXLMisc.Image = CType(resources.GetObject("btnXLMisc.Image"), System.Drawing.Image)
        Me.btnXLMisc.Location = New System.Drawing.Point(445, 179)
        Me.btnXLMisc.Name = "btnXLMisc"
        Me.btnXLMisc.Size = New System.Drawing.Size(32, 26)
        Me.btnXLMisc.TabIndex = 255
        Me.btnXLMisc.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnXLMisc, "Export Misc Info To Excel ")
        Me.btnXLMisc.UseVisualStyleBackColor = False
        '
        'lbl4
        '
        Me.lbl4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbl4.Font = New System.Drawing.Font("Berlin Sans FB", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lbl4.Location = New System.Drawing.Point(6, 2)
        Me.lbl4.Name = "lbl4"
        Me.lbl4.Size = New System.Drawing.Size(471, 23)
        Me.lbl4.TabIndex = 74
        Me.lbl4.Text = " Other/Misc Info"
        Me.lbl4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'pnl3
        '
        Me.pnl3.BackColor = System.Drawing.Color.FromArgb(CType(CType(69, Byte), Integer), CType(CType(137, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.pnl3.Controls.Add(Me.VsfDet3)
        Me.pnl3.Controls.Add(Me.btnXLStock)
        Me.pnl3.Controls.Add(Me.lbl3)
        Me.pnl3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnl3.Location = New System.Drawing.Point(3, 227)
        Me.pnl3.Name = "pnl3"
        Me.pnl3.Size = New System.Drawing.Size(471, 211)
        Me.pnl3.TabIndex = 215
        '
        'VsfDet3
        '
        Me.VsfDet3.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.VsfDet3.BackColor = System.Drawing.Color.FromArgb(CType(CType(69, Byte), Integer), CType(CType(137, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.VsfDet3.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None
        Me.VsfDet3.ColumnInfo = resources.GetString("VsfDet3.ColumnInfo")
        Me.VsfDet3.ExtendLastCol = True
        Me.VsfDet3.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.VsfDet3.HighLight = C1.Win.C1FlexGrid.HighLightEnum.Never
        Me.VsfDet3.Location = New System.Drawing.Point(5, 34)
        Me.VsfDet3.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.VsfDet3.Name = "VsfDet3"
        Me.VsfDet3.Rows.DefaultSize = 24
        Me.VsfDet3.Size = New System.Drawing.Size(461, 138)
        Me.VsfDet3.StyleInfo = resources.GetString("VsfDet3.StyleInfo")
        Me.VsfDet3.TabIndex = 258
        '
        'btnXLStock
        '
        Me.btnXLStock.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnXLStock.BackColor = System.Drawing.Color.Transparent
        Me.btnXLStock.FlatAppearance.BorderSize = 0
        Me.btnXLStock.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.btnXLStock.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.btnXLStock.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnXLStock.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnXLStock.Image = CType(resources.GetObject("btnXLStock.Image"), System.Drawing.Image)
        Me.btnXLStock.Location = New System.Drawing.Point(434, 179)
        Me.btnXLStock.Name = "btnXLStock"
        Me.btnXLStock.Size = New System.Drawing.Size(32, 26)
        Me.btnXLStock.TabIndex = 256
        Me.btnXLStock.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnXLStock, "Export Stock Info To Excel ")
        Me.btnXLStock.UseVisualStyleBackColor = False
        '
        'lbl3
        '
        Me.lbl3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbl3.Font = New System.Drawing.Font("Berlin Sans FB", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lbl3.Location = New System.Drawing.Point(5, 2)
        Me.lbl3.Name = "lbl3"
        Me.lbl3.Size = New System.Drawing.Size(461, 23)
        Me.lbl3.TabIndex = 74
        Me.lbl3.Text = "Stock Summary Info"
        Me.lbl3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnl2
        '
        Me.pnl2.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(83, Byte), Integer), CType(CType(173, Byte), Integer))
        Me.pnl2.Controls.Add(Me.vsfDet2)
        Me.pnl2.Controls.Add(Me.btnXLPB)
        Me.pnl2.Controls.Add(Me.lbl2)
        Me.pnl2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnl2.Location = New System.Drawing.Point(480, 3)
        Me.pnl2.Name = "pnl2"
        Me.pnl2.Size = New System.Drawing.Size(480, 218)
        Me.pnl2.TabIndex = 213
        '
        'vsfDet2
        '
        Me.vsfDet2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet2.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(83, Byte), Integer), CType(CType(173, Byte), Integer))
        Me.vsfDet2.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None
        Me.vsfDet2.ColumnInfo = resources.GetString("vsfDet2.ColumnInfo")
        Me.vsfDet2.ExtendLastCol = True
        Me.vsfDet2.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet2.HighLight = C1.Win.C1FlexGrid.HighLightEnum.Never
        Me.vsfDet2.Location = New System.Drawing.Point(6, 29)
        Me.vsfDet2.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet2.Name = "vsfDet2"
        Me.vsfDet2.Rows.DefaultSize = 24
        Me.vsfDet2.Size = New System.Drawing.Size(471, 153)
        Me.vsfDet2.StyleInfo = resources.GetString("vsfDet2.StyleInfo")
        Me.vsfDet2.TabIndex = 257
        '
        'btnXLPB
        '
        Me.btnXLPB.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnXLPB.BackColor = System.Drawing.Color.Transparent
        Me.btnXLPB.FlatAppearance.BorderSize = 0
        Me.btnXLPB.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.btnXLPB.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.btnXLPB.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnXLPB.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnXLPB.Image = CType(resources.GetObject("btnXLPB.Image"), System.Drawing.Image)
        Me.btnXLPB.Location = New System.Drawing.Point(445, 189)
        Me.btnXLPB.Name = "btnXLPB"
        Me.btnXLPB.Size = New System.Drawing.Size(32, 26)
        Me.btnXLPB.TabIndex = 255
        Me.btnXLPB.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnXLPB, "Export Purchase Info To Excel ")
        Me.btnXLPB.UseVisualStyleBackColor = False
        '
        'lbl2
        '
        Me.lbl2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbl2.Font = New System.Drawing.Font("Berlin Sans FB", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lbl2.Location = New System.Drawing.Point(6, 3)
        Me.lbl2.Name = "lbl2"
        Me.lbl2.Size = New System.Drawing.Size(471, 23)
        Me.lbl2.TabIndex = 73
        Me.lbl2.Text = "Purchase Summary Info"
        Me.lbl2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnl1
        '
        Me.pnl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnl1.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(187, Byte), Integer), CType(CType(41, Byte), Integer))
        Me.pnl1.Controls.Add(Me.vsfDet1)
        Me.pnl1.Controls.Add(Me.btnXLSales)
        Me.pnl1.Controls.Add(Me.lbl1)
        Me.pnl1.Location = New System.Drawing.Point(3, 3)
        Me.pnl1.Name = "pnl1"
        Me.pnl1.Size = New System.Drawing.Size(471, 218)
        Me.pnl1.TabIndex = 214
        '
        'vsfDet1
        '
        Me.vsfDet1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet1.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(187, Byte), Integer), CType(CType(41, Byte), Integer))
        Me.vsfDet1.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None
        Me.vsfDet1.ColumnInfo = resources.GetString("vsfDet1.ColumnInfo")
        Me.vsfDet1.ExtendLastCol = True
        Me.vsfDet1.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet1.HighLight = C1.Win.C1FlexGrid.HighLightEnum.Never
        Me.vsfDet1.Location = New System.Drawing.Point(5, 29)
        Me.vsfDet1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet1.Name = "vsfDet1"
        Me.vsfDet1.Rows.DefaultSize = 24
        Me.vsfDet1.Size = New System.Drawing.Size(461, 153)
        Me.vsfDet1.StyleInfo = resources.GetString("vsfDet1.StyleInfo")
        Me.vsfDet1.TabIndex = 256
        '
        'btnXLSales
        '
        Me.btnXLSales.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnXLSales.BackColor = System.Drawing.Color.Transparent
        Me.btnXLSales.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnXLSales.FlatAppearance.BorderSize = 0
        Me.btnXLSales.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.btnXLSales.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.btnXLSales.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnXLSales.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnXLSales.Image = CType(resources.GetObject("btnXLSales.Image"), System.Drawing.Image)
        Me.btnXLSales.Location = New System.Drawing.Point(434, 189)
        Me.btnXLSales.Name = "btnXLSales"
        Me.btnXLSales.Size = New System.Drawing.Size(32, 26)
        Me.btnXLSales.TabIndex = 254
        Me.btnXLSales.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnXLSales, "Export Sales info  To Excel ")
        Me.btnXLSales.UseVisualStyleBackColor = False
        '
        'lbl1
        '
        Me.lbl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbl1.Font = New System.Drawing.Font("Berlin Sans FB", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lbl1.Location = New System.Drawing.Point(5, 3)
        Me.lbl1.Name = "lbl1"
        Me.lbl1.Size = New System.Drawing.Size(461, 23)
        Me.lbl1.TabIndex = 72
        Me.lbl1.Text = "Sales Summary Info"
        Me.lbl1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ChartExpense
        '
        Me.ChartExpense.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        ChartArea1.AxisX.IsLabelAutoFit = False
        ChartArea1.Name = "ChartArea1"
        Me.ChartExpense.ChartAreas.Add(ChartArea1)
        Legend1.Name = "Legend1"
        Me.ChartExpense.Legends.Add(Legend1)
        Me.ChartExpense.Location = New System.Drawing.Point(3, 444)
        Me.ChartExpense.Name = "ChartExpense"
        Series1.ChartArea = "ChartArea1"
        Series1.Legend = "Legend1"
        Series1.Name = "Expenses"
        Me.ChartExpense.Series.Add(Series1)
        Me.ChartExpense.Size = New System.Drawing.Size(471, 153)
        Me.ChartExpense.TabIndex = 217
        Me.ChartExpense.Text = "Expense Info"
        Title1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Title1.ForeColor = System.Drawing.Color.DarkRed
        Title1.Name = "Title1"
        Title1.Text = "Expense Info"
        Me.ChartExpense.Titles.Add(Title1)
        '
        'ChartSaleVsPB
        '
        Me.ChartSaleVsPB.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        ChartArea2.AxisX.Interval = 1.0R
        ChartArea2.Name = "ChartArea1"
        Me.ChartSaleVsPB.ChartAreas.Add(ChartArea2)
        Legend2.Name = "Legend1"
        Me.ChartSaleVsPB.Legends.Add(Legend2)
        Me.ChartSaleVsPB.Location = New System.Drawing.Point(480, 444)
        Me.ChartSaleVsPB.Name = "ChartSaleVsPB"
        Series2.ChartArea = "ChartArea1"
        Series2.Legend = "Legend1"
        Series2.Name = "Sales"
        Series3.ChartArea = "ChartArea1"
        Series3.Legend = "Legend1"
        Series3.Name = "Purchase"
        Me.ChartSaleVsPB.Series.Add(Series2)
        Me.ChartSaleVsPB.Series.Add(Series3)
        Me.ChartSaleVsPB.Size = New System.Drawing.Size(480, 153)
        Me.ChartSaleVsPB.TabIndex = 218
        Me.ChartSaleVsPB.Text = "SalevsPB"
        Title2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Title2.ForeColor = System.Drawing.Color.DarkRed
        Title2.Name = "Title1"
        Title2.Text = "Sales vs Purchase"
        Me.ChartSaleVsPB.Titles.Add(Title2)
        '
        'lblSettings
        '
        Me.lblSettings.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblSettings.AutoSize = True
        Me.lblSettings.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lblSettings.ForeColor = System.Drawing.Color.Red
        Me.lblSettings.Location = New System.Drawing.Point(39, 621)
        Me.lblSettings.Name = "lblSettings"
        Me.lblSettings.Size = New System.Drawing.Size(200, 13)
        Me.lblSettings.TabIndex = 58
        Me.lblSettings.Text = "F4 - To Change The DashBoard Settings"
        '
        'btnSettings
        '
        Me.btnSettings.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSettings.BackgroundImage = Global.vLQMS.My.Resources.Resources.setting1
        Me.btnSettings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSettings.Location = New System.Drawing.Point(12, 616)
        Me.btnSettings.Name = "btnSettings"
        Me.btnSettings.Size = New System.Drawing.Size(23, 23)
        Me.btnSettings.TabIndex = 60
        Me.ToolTip1.SetToolTip(Me.btnSettings, "F4 - To DashBoard Setting")
        Me.btnSettings.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(898, 613)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 29)
        Me.btnClose.TabIndex = 224
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close Alt + O")
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'lblRefresh
        '
        Me.lblRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblRefresh.AutoSize = True
        Me.lblRefresh.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lblRefresh.ForeColor = System.Drawing.Color.Red
        Me.lblRefresh.Location = New System.Drawing.Point(380, 621)
        Me.lblRefresh.Name = "lblRefresh"
        Me.lblRefresh.Size = New System.Drawing.Size(236, 13)
        Me.lblRefresh.TabIndex = 225
        Me.lblRefresh.Text = "F6 - To Refresh Data (Auto set for 10 mins once)"
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.BackgroundImage = Global.vLQMS.My.Resources.Resources.refresh
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBox1.Location = New System.Drawing.Point(357, 616)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(22, 23)
        Me.PictureBox1.TabIndex = 226
        Me.PictureBox1.TabStop = False
        Me.ToolTip1.SetToolTip(Me.PictureBox1, "F6 - Refresh Data")
        '
        'frmUtlDashboard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(985, 645)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.lblRefresh)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnSettings)
        Me.Controls.Add(Me.lblSettings)
        Me.Controls.Add(Me.Tbllout)
        Me.Name = "frmUtlDashboard"
        Me.ShowIcon = False
        Me.Tbllout.ResumeLayout(False)
        Me.pnl4.ResumeLayout(False)
        CType(Me.vsfDet4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnl3.ResumeLayout(False)
        CType(Me.VsfDet3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnl2.ResumeLayout(False)
        CType(Me.vsfDet2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnl1.ResumeLayout(False)
        CType(Me.vsfDet1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ChartExpense, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ChartSaleVsPB, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pnl3 As Panel
    Friend WithEvents pnl1 As Panel
    Friend WithEvents pnl2 As Panel
    Friend WithEvents pnl4 As Panel
    Friend WithEvents lbl1 As Label
    Friend WithEvents lbl2 As Label
    Friend WithEvents lbl4 As Label
    Friend WithEvents lbl3 As Label
    Friend WithEvents lblSettings As Label
    Friend WithEvents btnSettings As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents lblRefresh As Label
    Public WithEvents Tbllout As TableLayoutPanel
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents btnXLMisc As Button
    Friend WithEvents btnXLStock As Button
    Friend WithEvents btnXLPB As Button
    Friend WithEvents btnXLSales As Button
    Friend WithEvents vsfDet1 As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents vsfDet4 As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents VsfDet3 As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents vsfDet2 As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents ChartExpense As DataVisualization.Charting.Chart
    Friend WithEvents ChartSaleVsPB As DataVisualization.Charting.Chart
End Class
