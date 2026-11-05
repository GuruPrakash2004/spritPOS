<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmTrnSelfInv
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmTrnSelfInv))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblCounter = New System.Windows.Forms.Label()
        Me.lblSalPer = New System.Windows.Forms.Label()
        Me.lblInvDtTime = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.Panel6 = New System.Windows.Forms.Panel()
        Me.btn5 = New System.Windows.Forms.Button()
        Me.btn6 = New System.Windows.Forms.Button()
        Me.Panel7 = New System.Windows.Forms.Panel()
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Panel8 = New System.Windows.Forms.Panel()
        Me.lblTotBasic = New System.Windows.Forms.Label()
        Me.lblTotCGST = New System.Windows.Forms.Label()
        Me.lblTotSGST = New System.Windows.Forms.Label()
        Me.lblPayAmt = New System.Windows.Forms.Label()
        Me.txtAmt = New System.Windows.Forms.TextBox()
        Me.Watermark1 = New vLQMS.Watermark()
        Me.btnPrintCpy = New System.Windows.Forms.Button()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.img = New System.Windows.Forms.PictureBox()
        Me.imgRightArr = New System.Windows.Forms.PictureBox()
        Me.btn10 = New System.Windows.Forms.Button()
        Me.btn9 = New System.Windows.Forms.Button()
        Me.btn8 = New System.Windows.Forms.Button()
        Me.btn7 = New System.Windows.Forms.Button()
        Me.btn1 = New System.Windows.Forms.Button()
        Me.btn4 = New System.Windows.Forms.Button()
        Me.btn3 = New System.Windows.Forms.Button()
        Me.btn2 = New System.Windows.Forms.Button()
        Me.imgleft = New System.Windows.Forms.PictureBox()
        Me.btnTopTen = New System.Windows.Forms.Button()
        Me.imgSalPer = New System.Windows.Forms.PictureBox()
        Me.imgCounName = New System.Windows.Forms.PictureBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.imgSoft = New System.Windows.Forms.PictureBox()
        Me.imgSoftvent = New System.Windows.Forms.PictureBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Panel1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.Panel6.SuspendLayout()
        Me.Panel7.SuspendLayout()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel8.SuspendLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.img, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.imgRightArr, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.imgleft, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.imgSalPer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.imgCounName, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.imgSoft, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.imgSoftvent, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.MidnightBlue
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Controls.Add(Me.Panel4)
        Me.Panel1.Controls.Add(Me.Panel3)
        Me.Panel1.Controls.Add(Me.imgSoft)
        Me.Panel1.Controls.Add(Me.imgSoftvent)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Location = New System.Drawing.Point(2, 2)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1251, 59)
        Me.Panel1.TabIndex = 25
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(1082, 6)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(165, 50)
        Me.Label4.TabIndex = 598
        Me.Label4.Tag = "R"
        Me.Label4.Text = "9880606365  80-26794645 mail@softvent.com www.softvent.com" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblCounter
        '
        Me.lblCounter.AutoSize = True
        Me.lblCounter.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCounter.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.lblCounter.Location = New System.Drawing.Point(31, 11)
        Me.lblCounter.Name = "lblCounter"
        Me.lblCounter.Size = New System.Drawing.Size(58, 16)
        Me.lblCounter.TabIndex = 599
        Me.lblCounter.Text = "Counter-"
        '
        'lblSalPer
        '
        Me.lblSalPer.AutoSize = True
        Me.lblSalPer.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSalPer.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.lblSalPer.Location = New System.Drawing.Point(31, 11)
        Me.lblSalPer.Name = "lblSalPer"
        Me.lblSalPer.Size = New System.Drawing.Size(93, 16)
        Me.lblSalPer.TabIndex = 600
        Me.lblSalPer.Text = "Sales Person-"
        '
        'lblInvDtTime
        '
        Me.lblInvDtTime.AutoSize = True
        Me.lblInvDtTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInvDtTime.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.lblInvDtTime.Location = New System.Drawing.Point(31, 10)
        Me.lblInvDtTime.Name = "lblInvDtTime"
        Me.lblInvDtTime.Size = New System.Drawing.Size(145, 16)
        Me.lblInvDtTime.TabIndex = 601
        Me.lblInvDtTime.Text = "Invoice No-Date & Time "
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label5.Location = New System.Drawing.Point(108, 9)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(0, 16)
        Me.Label5.TabIndex = 602
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.PictureBox2)
        Me.Panel3.Controls.Add(Me.lblInvDtTime)
        Me.Panel3.Location = New System.Drawing.Point(141, 14)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(179, 36)
        Me.Panel3.TabIndex = 26
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label3.Location = New System.Drawing.Point(31, 8)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(169, 20)
        Me.Label3.TabIndex = 601
        Me.Label3.Text = "Invoice No-Date & Time "
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(206, 12)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(0, 13)
        Me.Label6.TabIndex = 601
        Me.Label6.Text = "Label6"
        '
        'Panel4
        '
        Me.Panel4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel4.Controls.Add(Me.Label5)
        Me.Panel4.Controls.Add(Me.Label7)
        Me.Panel4.Controls.Add(Me.lblCounter)
        Me.Panel4.Controls.Add(Me.imgCounName)
        Me.Panel4.Location = New System.Drawing.Point(643, 12)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(92, 36)
        Me.Panel4.TabIndex = 602
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(206, 12)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(0, 13)
        Me.Label7.TabIndex = 601
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Label9)
        Me.Panel2.Controls.Add(Me.imgSalPer)
        Me.Panel2.Controls.Add(Me.lblSalPer)
        Me.Panel2.Location = New System.Drawing.Point(418, 14)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(125, 36)
        Me.Panel2.TabIndex = 602
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(206, 12)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(0, 13)
        Me.Label9.TabIndex = 601
        '
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.imgRightArr)
        Me.Panel5.Controls.Add(Me.Panel6)
        Me.Panel5.Controls.Add(Me.imgleft)
        Me.Panel5.Controls.Add(Me.btnTopTen)
        Me.Panel5.Location = New System.Drawing.Point(2, 66)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(1249, 82)
        Me.Panel5.TabIndex = 26
        '
        'Panel6
        '
        Me.Panel6.Controls.Add(Me.btn10)
        Me.Panel6.Controls.Add(Me.btn9)
        Me.Panel6.Controls.Add(Me.btn8)
        Me.Panel6.Controls.Add(Me.btn7)
        Me.Panel6.Controls.Add(Me.btn1)
        Me.Panel6.Controls.Add(Me.btn6)
        Me.Panel6.Controls.Add(Me.btn5)
        Me.Panel6.Controls.Add(Me.btn4)
        Me.Panel6.Controls.Add(Me.btn3)
        Me.Panel6.Controls.Add(Me.btn2)
        Me.Panel6.Location = New System.Drawing.Point(331, 7)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(839, 72)
        Me.Panel6.TabIndex = 27
        '
        'btn5
        '
        Me.btn5.BackColor = System.Drawing.Color.Turquoise
        Me.btn5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn5.Location = New System.Drawing.Point(191, 3)
        Me.btn5.Name = "btn5"
        Me.btn5.Size = New System.Drawing.Size(70, 58)
        Me.btn5.TabIndex = 29
        Me.btn5.Text = "8 PM"
        Me.btn5.UseVisualStyleBackColor = False
        '
        'btn6
        '
        Me.btn6.BackColor = System.Drawing.Color.Orange
        Me.btn6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn6.Location = New System.Drawing.Point(422, 5)
        Me.btn6.Name = "btn6"
        Me.btn6.Size = New System.Drawing.Size(70, 58)
        Me.btn6.TabIndex = 29
        Me.btn6.Text = "RUM"
        Me.btn6.UseVisualStyleBackColor = False
        '
        'Panel7
        '
        Me.Panel7.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Panel7.Controls.Add(Me.Watermark1)
        Me.Panel7.Controls.Add(Me.PictureBox3)
        Me.Panel7.Controls.Add(Me.img)
        Me.Panel7.Location = New System.Drawing.Point(2, 157)
        Me.Panel7.Name = "Panel7"
        Me.Panel7.Size = New System.Drawing.Size(1249, 59)
        Me.Panel7.TabIndex = 27
        '
        'vsfDet
        '
        Me.vsfDet.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.Location = New System.Drawing.Point(12, 217)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.Size = New System.Drawing.Size(1237, 174)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 602
        Me.vsfDet.Visible = False
        '
        'Panel8
        '
        Me.Panel8.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Panel8.Controls.Add(Me.lblTotSGST)
        Me.Panel8.Controls.Add(Me.lblTotCGST)
        Me.Panel8.Controls.Add(Me.lblTotBasic)
        Me.Panel8.Location = New System.Drawing.Point(16, 405)
        Me.Panel8.Name = "Panel8"
        Me.Panel8.Size = New System.Drawing.Size(596, 34)
        Me.Panel8.TabIndex = 603
        '
        'lblTotBasic
        '
        Me.lblTotBasic.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotBasic.AutoSize = True
        Me.lblTotBasic.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotBasic.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.lblTotBasic.Location = New System.Drawing.Point(3, 9)
        Me.lblTotBasic.Name = "lblTotBasic"
        Me.lblTotBasic.Size = New System.Drawing.Size(82, 16)
        Me.lblTotBasic.TabIndex = 0
        Me.lblTotBasic.Text = "Total Basic :"
        '
        'lblTotCGST
        '
        Me.lblTotCGST.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotCGST.AutoSize = True
        Me.lblTotCGST.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotCGST.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.lblTotCGST.Location = New System.Drawing.Point(156, 9)
        Me.lblTotCGST.Name = "lblTotCGST"
        Me.lblTotCGST.Size = New System.Drawing.Size(82, 16)
        Me.lblTotCGST.TabIndex = 1
        Me.lblTotCGST.Text = "Total CGST:"
        '
        'lblTotSGST
        '
        Me.lblTotSGST.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotSGST.AutoSize = True
        Me.lblTotSGST.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotSGST.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.lblTotSGST.Location = New System.Drawing.Point(372, 9)
        Me.lblTotSGST.Name = "lblTotSGST"
        Me.lblTotSGST.Size = New System.Drawing.Size(85, 16)
        Me.lblTotSGST.TabIndex = 2
        Me.lblTotSGST.Text = "Total SGST :"
        '
        'lblPayAmt
        '
        Me.lblPayAmt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblPayAmt.AutoSize = True
        Me.lblPayAmt.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPayAmt.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.lblPayAmt.Location = New System.Drawing.Point(629, 409)
        Me.lblPayAmt.Name = "lblPayAmt"
        Me.lblPayAmt.Size = New System.Drawing.Size(128, 25)
        Me.lblPayAmt.TabIndex = 604
        Me.lblPayAmt.Text = "Pay Amount"
        '
        'txtAmt
        '
        Me.txtAmt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAmt.BackColor = System.Drawing.Color.MidnightBlue
        Me.txtAmt.Location = New System.Drawing.Point(763, 408)
        Me.txtAmt.Multiline = True
        Me.txtAmt.Name = "txtAmt"
        Me.txtAmt.Size = New System.Drawing.Size(126, 30)
        Me.txtAmt.TabIndex = 605
        '
        'Watermark1
        '
        Me.Watermark1.Font = New System.Drawing.Font("Verdana", 26.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Watermark1.Location = New System.Drawing.Point(258, 3)
        Me.Watermark1.Name = "Watermark1"
        Me.Watermark1.Size = New System.Drawing.Size(734, 50)
        Me.Watermark1.TabIndex = 606
        Me.Watermark1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.Watermark1.WatermarkColor = System.Drawing.Color.Gray
        Me.Watermark1.WatermarkText = "Scan Bar Code"
        '
        'btnPrintCpy
        '
        Me.btnPrintCpy.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPrintCpy.BackColor = System.Drawing.Color.Red
        Me.btnPrintCpy.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrintCpy.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnPrintCpy.Image = CType(resources.GetObject("btnPrintCpy.Image"), System.Drawing.Image)
        Me.btnPrintCpy.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.btnPrintCpy.Location = New System.Drawing.Point(972, 400)
        Me.btnPrintCpy.Name = "btnPrintCpy"
        Me.btnPrintCpy.Size = New System.Drawing.Size(281, 66)
        Me.btnPrintCpy.TabIndex = 31
        Me.btnPrintCpy.Text = "Print Bill Copy"
        Me.btnPrintCpy.UseVisualStyleBackColor = False
        '
        'PictureBox3
        '
        Me.PictureBox3.BackgroundImage = CType(resources.GetObject("PictureBox3.BackgroundImage"), System.Drawing.Image)
        Me.PictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PictureBox3.Location = New System.Drawing.Point(998, 3)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(245, 50)
        Me.PictureBox3.TabIndex = 29
        Me.PictureBox3.TabStop = False
        '
        'img
        '
        Me.img.BackgroundImage = CType(resources.GetObject("img.BackgroundImage"), System.Drawing.Image)
        Me.img.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.img.Location = New System.Drawing.Point(7, 3)
        Me.img.Name = "img"
        Me.img.Size = New System.Drawing.Size(245, 50)
        Me.img.TabIndex = 28
        Me.img.TabStop = False
        '
        'imgRightArr
        '
        Me.imgRightArr.BackgroundImage = CType(resources.GetObject("imgRightArr.BackgroundImage"), System.Drawing.Image)
        Me.imgRightArr.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.imgRightArr.Location = New System.Drawing.Point(1176, 12)
        Me.imgRightArr.Name = "imgRightArr"
        Me.imgRightArr.Size = New System.Drawing.Size(67, 50)
        Me.imgRightArr.TabIndex = 30
        Me.imgRightArr.TabStop = False
        '
        'btn10
        '
        Me.btn10.BackColor = System.Drawing.Color.Fuchsia
        Me.btn10.BackgroundImage = CType(resources.GetObject("btn10.BackgroundImage"), System.Drawing.Image)
        Me.btn10.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btn10.Location = New System.Drawing.Point(754, 5)
        Me.btn10.Name = "btn10"
        Me.btn10.Size = New System.Drawing.Size(70, 58)
        Me.btn10.TabIndex = 32
        Me.btn10.UseVisualStyleBackColor = False
        '
        'btn9
        '
        Me.btn9.BackColor = System.Drawing.Color.Fuchsia
        Me.btn9.BackgroundImage = CType(resources.GetObject("btn9.BackgroundImage"), System.Drawing.Image)
        Me.btn9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btn9.Location = New System.Drawing.Point(678, 5)
        Me.btn9.Name = "btn9"
        Me.btn9.Size = New System.Drawing.Size(70, 58)
        Me.btn9.TabIndex = 31
        Me.btn9.UseVisualStyleBackColor = False
        '
        'btn8
        '
        Me.btn8.BackColor = System.Drawing.Color.Yellow
        Me.btn8.BackgroundImage = CType(resources.GetObject("btn8.BackgroundImage"), System.Drawing.Image)
        Me.btn8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btn8.Location = New System.Drawing.Point(574, 5)
        Me.btn8.Name = "btn8"
        Me.btn8.Size = New System.Drawing.Size(70, 58)
        Me.btn8.TabIndex = 29
        Me.btn8.UseVisualStyleBackColor = False
        '
        'btn7
        '
        Me.btn7.BackColor = System.Drawing.Color.Yellow
        Me.btn7.BackgroundImage = CType(resources.GetObject("btn7.BackgroundImage"), System.Drawing.Image)
        Me.btn7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btn7.Location = New System.Drawing.Point(498, 5)
        Me.btn7.Name = "btn7"
        Me.btn7.Size = New System.Drawing.Size(70, 58)
        Me.btn7.TabIndex = 29
        Me.btn7.UseVisualStyleBackColor = False
        '
        'btn1
        '
        Me.btn1.BackColor = System.Drawing.Color.SpringGreen
        Me.btn1.BackgroundImage = CType(resources.GetObject("btn1.BackgroundImage"), System.Drawing.Image)
        Me.btn1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btn1.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btn1.FlatAppearance.BorderSize = 3
        Me.btn1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn1.Location = New System.Drawing.Point(16, 4)
        Me.btn1.Name = "btn1"
        Me.btn1.Size = New System.Drawing.Size(70, 58)
        Me.btn1.TabIndex = 27
        Me.btn1.Text = "KF 650 ml"
        Me.btn1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btn1.UseVisualStyleBackColor = False
        '
        'btn4
        '
        Me.btn4.BackColor = System.Drawing.Color.Orange
        Me.btn4.BackgroundImage = CType(resources.GetObject("btn4.BackgroundImage"), System.Drawing.Image)
        Me.btn4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btn4.Location = New System.Drawing.Point(346, 5)
        Me.btn4.Name = "btn4"
        Me.btn4.Size = New System.Drawing.Size(70, 58)
        Me.btn4.TabIndex = 30
        Me.btn4.UseVisualStyleBackColor = False
        '
        'btn3
        '
        Me.btn3.BackColor = System.Drawing.Color.Turquoise
        Me.btn3.BackgroundImage = CType(resources.GetObject("btn3.BackgroundImage"), System.Drawing.Image)
        Me.btn3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.btn3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn3.Location = New System.Drawing.Point(267, 5)
        Me.btn3.Name = "btn3"
        Me.btn3.Size = New System.Drawing.Size(70, 58)
        Me.btn3.TabIndex = 29
        Me.btn3.Text = "Snacks"
        Me.btn3.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btn3.UseVisualStyleBackColor = False
        '
        'btn2
        '
        Me.btn2.BackColor = System.Drawing.Color.SpringGreen
        Me.btn2.BackgroundImage = CType(resources.GetObject("btn2.BackgroundImage"), System.Drawing.Image)
        Me.btn2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.btn2.Location = New System.Drawing.Point(104, 5)
        Me.btn2.Name = "btn2"
        Me.btn2.Size = New System.Drawing.Size(70, 58)
        Me.btn2.TabIndex = 28
        Me.btn2.Text = "Cigerette"
        Me.btn2.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btn2.UseVisualStyleBackColor = False
        '
        'imgleft
        '
        Me.imgleft.BackgroundImage = CType(resources.GetObject("imgleft.BackgroundImage"), System.Drawing.Image)
        Me.imgleft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.imgleft.Location = New System.Drawing.Point(274, 12)
        Me.imgleft.Name = "imgleft"
        Me.imgleft.Size = New System.Drawing.Size(67, 50)
        Me.imgleft.TabIndex = 29
        Me.imgleft.TabStop = False
        '
        'btnTopTen
        '
        Me.btnTopTen.BackColor = System.Drawing.Color.RoyalBlue
        Me.btnTopTen.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTopTen.Image = CType(resources.GetObject("btnTopTen.Image"), System.Drawing.Image)
        Me.btnTopTen.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnTopTen.Location = New System.Drawing.Point(7, 6)
        Me.btnTopTen.Name = "btnTopTen"
        Me.btnTopTen.Size = New System.Drawing.Size(245, 56)
        Me.btnTopTen.TabIndex = 27
        Me.btnTopTen.Text = "Top 10 Items"
        Me.btnTopTen.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnTopTen.UseVisualStyleBackColor = False
        '
        'imgSalPer
        '
        Me.imgSalPer.BackgroundImage = CType(resources.GetObject("imgSalPer.BackgroundImage"), System.Drawing.Image)
        Me.imgSalPer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.imgSalPer.Location = New System.Drawing.Point(3, 3)
        Me.imgSalPer.Name = "imgSalPer"
        Me.imgSalPer.Size = New System.Drawing.Size(22, 31)
        Me.imgSalPer.TabIndex = 26
        Me.imgSalPer.TabStop = False
        '
        'imgCounName
        '
        Me.imgCounName.BackgroundImage = CType(resources.GetObject("imgCounName.BackgroundImage"), System.Drawing.Image)
        Me.imgCounName.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.imgCounName.Location = New System.Drawing.Point(3, 3)
        Me.imgCounName.Name = "imgCounName"
        Me.imgCounName.Size = New System.Drawing.Size(22, 31)
        Me.imgCounName.TabIndex = 26
        Me.imgCounName.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.BackgroundImage = CType(resources.GetObject("PictureBox2.BackgroundImage"), System.Drawing.Image)
        Me.PictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PictureBox2.Location = New System.Drawing.Point(3, 3)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(22, 31)
        Me.PictureBox2.TabIndex = 26
        Me.PictureBox2.TabStop = False
        '
        'imgSoft
        '
        Me.imgSoft.BackgroundImage = CType(resources.GetObject("imgSoft.BackgroundImage"), System.Drawing.Image)
        Me.imgSoft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.imgSoft.ErrorImage = Nothing
        Me.imgSoft.Location = New System.Drawing.Point(9, 14)
        Me.imgSoft.Name = "imgSoft"
        Me.imgSoft.Size = New System.Drawing.Size(126, 34)
        Me.imgSoft.TabIndex = 26
        Me.imgSoft.TabStop = False
        '
        'imgSoftvent
        '
        Me.imgSoftvent.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.imgSoftvent.BackColor = System.Drawing.Color.Transparent
        Me.imgSoftvent.BackgroundImage = CType(resources.GetObject("imgSoftvent.BackgroundImage"), System.Drawing.Image)
        Me.imgSoftvent.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.imgSoftvent.Location = New System.Drawing.Point(873, 10)
        Me.imgSoftvent.Margin = New System.Windows.Forms.Padding(4)
        Me.imgSoftvent.Name = "imgSoftvent"
        Me.imgSoftvent.Size = New System.Drawing.Size(208, 54)
        Me.imgSoftvent.TabIndex = 597
        Me.imgSoftvent.TabStop = False
        '
        'PictureBox1
        '
        Me.PictureBox1.BackgroundImage = CType(resources.GetObject("PictureBox1.BackgroundImage"), System.Drawing.Image)
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PictureBox1.Location = New System.Drawing.Point(3, 3)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(22, 31)
        Me.PictureBox1.TabIndex = 26
        Me.PictureBox1.TabStop = False
        '
        'FrmTrnSelfInv
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.MidnightBlue
        Me.ClientSize = New System.Drawing.Size(1259, 478)
        Me.Controls.Add(Me.btnPrintCpy)
        Me.Controls.Add(Me.txtAmt)
        Me.Controls.Add(Me.lblPayAmt)
        Me.Controls.Add(Me.Panel8)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.Panel7)
        Me.Controls.Add(Me.Panel5)
        Me.Controls.Add(Me.Panel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FrmTrnSelfInv"
        Me.Panel1.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel5.ResumeLayout(False)
        Me.Panel6.ResumeLayout(False)
        Me.Panel7.ResumeLayout(False)
        Me.Panel7.PerformLayout()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel8.ResumeLayout(False)
        Me.Panel8.PerformLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.img, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.imgRightArr, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.imgleft, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.imgSalPer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.imgCounName, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.imgSoft, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.imgSoftvent, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label4 As Label
    Friend WithEvents imgSoftvent As PictureBox
    Friend WithEvents Label5 As Label
    Friend WithEvents lblCounter As Label
    Friend WithEvents lblInvDtTime As Label
    Friend WithEvents lblSalPer As Label
    Friend WithEvents Panel3 As Panel
    Friend WithEvents PictureBox2 As PictureBox
    Friend WithEvents imgSoft As PictureBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Panel4 As Panel
    Friend WithEvents Label7 As Label
    Friend WithEvents imgCounName As PictureBox
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Label9 As Label
    Friend WithEvents imgSalPer As PictureBox
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Panel5 As Panel
    Friend WithEvents btnTopTen As Button
    Friend WithEvents imgRightArr As PictureBox
    Friend WithEvents Panel6 As Panel
    Friend WithEvents btn10 As Button
    Friend WithEvents btn9 As Button
    Friend WithEvents btn8 As Button
    Friend WithEvents btn7 As Button
    Friend WithEvents btn1 As Button
    Friend WithEvents btn6 As Button
    Friend WithEvents btn5 As Button
    Friend WithEvents btn4 As Button
    Friend WithEvents btn3 As Button
    Friend WithEvents btn2 As Button
    Friend WithEvents Panel7 As Panel
    Friend WithEvents img As PictureBox
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents Panel8 As Panel
    Friend WithEvents lblTotSGST As Label
    Friend WithEvents lblTotCGST As Label
    Friend WithEvents lblTotBasic As Label
    Friend WithEvents lblPayAmt As Label
    Friend WithEvents txtAmt As TextBox
    Friend WithEvents btnPrintCpy As Button
    Friend WithEvents PictureBox3 As PictureBox
    Friend WithEvents imgleft As PictureBox
    Friend WithEvents Watermark1 As Watermark
End Class
