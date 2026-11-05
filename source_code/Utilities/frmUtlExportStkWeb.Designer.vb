<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlExportStkWeb
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlExportStkWeb))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.dtTDt = New System.Windows.Forms.DateTimePicker()
        Me.dtFdt = New System.Windows.Forms.DateTimePicker()
        Me.chkStock = New System.Windows.Forms.CheckBox()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnExportToXL = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.vsfINI = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.vsfDSR = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.vsfINI, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfDSR, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.dtTDt)
        Me.Panel1.Controls.Add(Me.dtFdt)
        Me.Panel1.Location = New System.Drawing.Point(12, 15)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(141, 51)
        Me.Panel1.TabIndex = 20
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(268, 20)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(27, 20)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "To"
        Me.Label4.Visible = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(284, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(46, 20)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "From"
        Me.Label3.Visible = False
        '
        'dtTDt
        '
        Me.dtTDt.CustomFormat = "dd/MM/yyyy"
        Me.dtTDt.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtTDt.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtTDt.Location = New System.Drawing.Point(9, 12)
        Me.dtTDt.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtTDt.Name = "dtTDt"
        Me.dtTDt.Size = New System.Drawing.Size(124, 26)
        Me.dtTDt.TabIndex = 3
        '
        'dtFdt
        '
        Me.dtFdt.CustomFormat = "dd/MM/yyyy"
        Me.dtFdt.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtFdt.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtFdt.Location = New System.Drawing.Point(139, 12)
        Me.dtFdt.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtFdt.Name = "dtFdt"
        Me.dtFdt.Size = New System.Drawing.Size(112, 26)
        Me.dtFdt.TabIndex = 1
        Me.dtFdt.Visible = False
        '
        'chkStock
        '
        Me.chkStock.AutoSize = True
        Me.chkStock.Checked = True
        Me.chkStock.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkStock.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkStock.Location = New System.Drawing.Point(12, 83)
        Me.chkStock.Name = "chkStock"
        Me.chkStock.Size = New System.Drawing.Size(182, 20)
        Me.chkStock.TabIndex = 1
        Me.chkStock.Text = "Export Stock Data to WEB"
        Me.chkStock.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Image = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.Location = New System.Drawing.Point(672, 330)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(88, 30)
        Me.btnClose.TabIndex = 3
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnExportToXL
        '
        Me.btnExportToXL.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExportToXL.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExportToXL.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExportToXL.Location = New System.Drawing.Point(545, 330)
        Me.btnExportToXL.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnExportToXL.Name = "btnExportToXL"
        Me.btnExportToXL.Size = New System.Drawing.Size(121, 30)
        Me.btnExportToXL.TabIndex = 2
        Me.btnExportToXL.Text = "Export Data To Web"
        Me.btnExportToXL.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnExportToXL.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Maroon
        Me.Label1.Location = New System.Drawing.Point(6, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(41, 16)
        Me.Label1.TabIndex = 75
        Me.Label1.Text = "Note"
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.Label7)
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 376)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(764, 103)
        Me.Panel2.TabIndex = 4
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.SystemColors.Control
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Maroon
        Me.Label7.Location = New System.Drawing.Point(6, 77)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(241, 16)
        Me.Label7.TabIndex = 78
        Me.Label7.Text = "Exported Data Is  Based on Sales Rate"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.SystemColors.Control
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Maroon
        Me.Label5.Location = New System.Drawing.Point(6, 54)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(736, 16)
        Me.Label5.TabIndex = 77
        Me.Label5.Text = "Export Stock Data to Web : if uncheck then only sales and purchase data will be e" &
    "xported to Web for the above date Range."
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Maroon
        Me.Label2.Location = New System.Drawing.Point(5, 33)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(634, 16)
        Me.Label2.TabIndex = 76
        Me.Label2.Text = "This interface is used to Export Data from Local Database to Web Database for the" &
    " above given period -1."
        '
        'vsfINI
        '
        Me.vsfINI.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfINI.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfINI.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfINI.ExtendLastCol = True
        Me.vsfINI.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfINI.Location = New System.Drawing.Point(364, 15)
        Me.vsfINI.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfINI.Name = "vsfINI"
        Me.vsfINI.Rows.DefaultSize = 24
        Me.vsfINI.Size = New System.Drawing.Size(338, 81)
        Me.vsfINI.StyleInfo = resources.GetString("vsfINI.StyleInfo")
        Me.vsfINI.TabIndex = 241
        Me.vsfINI.Visible = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(16, 3)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(49, 18)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "As On"
        '
        'lblStatus
        '
        Me.lblStatus.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblStatus.BackColor = System.Drawing.SystemColors.Control
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblStatus.Location = New System.Drawing.Point(12, 330)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(527, 27)
        Me.lblStatus.TabIndex = 242
        Me.lblStatus.Text = "t"
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.PictureBox1.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(4, 3)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(519, 324)
        Me.PictureBox1.TabIndex = 243
        Me.PictureBox1.TabStop = False
        '
        'vsfDSR
        '
        Me.vsfDSR.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfDSR.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDSR.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfDSR.ExtendLastCol = True
        Me.vsfDSR.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDSR.Location = New System.Drawing.Point(301, 15)
        Me.vsfDSR.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDSR.Name = "vsfDSR"
        Me.vsfDSR.Rows.DefaultSize = 24
        Me.vsfDSR.Size = New System.Drawing.Size(401, 81)
        Me.vsfDSR.StyleInfo = resources.GetString("vsfDSR.StyleInfo")
        Me.vsfDSR.TabIndex = 247
        Me.vsfDSR.Visible = False
        '
        'Panel3
        '
        Me.Panel3.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.Panel3.BackColor = System.Drawing.Color.White
        Me.Panel3.Controls.Add(Me.PictureBox1)
        Me.Panel3.Location = New System.Drawing.Point(124, 100)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(527, 337)
        Me.Panel3.TabIndex = 248
        '
        'frmUtlExportStkWeb
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(764, 479)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.vsfDSR)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.vsfINI)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnExportToXL)
        Me.Controls.Add(Me.chkStock)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "frmUtlExportStkWeb"
        Me.ShowIcon = False
        Me.Text = "Export Data To Web"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.vsfINI, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfDSR, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents dtTDt As DateTimePicker
    Friend WithEvents dtFdt As DateTimePicker
    Friend WithEvents chkStock As CheckBox
    Friend WithEvents btnClose As Button
    Friend WithEvents btnExportToXL As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Label5 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents vsfINI As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents Label6 As Label
    Friend WithEvents lblStatus As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents vsfDSR As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents Panel3 As Panel
    Friend WithEvents Label7 As Label
End Class
