<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMstCreateFinYear
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMstCreateFinYear))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.dtCurDate = New System.Windows.Forms.DateTimePicker()
        Me.dtLFYStDate = New System.Windows.Forms.DateTimePicker()
        Me.dtLFYEndDate = New System.Windows.Forms.DateTimePicker()
        Me.dtCFYStDate = New System.Windows.Forms.DateTimePicker()
        Me.dtCFYEndDate = New System.Windows.Forms.DateTimePicker()
        Me.btnCreate = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 12.0!, CType(((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic) _
                Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(449, 28)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Create New Fin-Year & Carry Forward Stocks"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 40)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(102, 21)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Current Date:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 122)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(100, 21)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Last Fin Year:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 168)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(104, 21)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "New Fin Year:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Red
        Me.Label5.Location = New System.Drawing.Point(12, 77)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(395, 21)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Note: Current Date not in Financial Year Date Range"
        '
        'dtCurDate
        '
        Me.dtCurDate.Enabled = False
        Me.dtCurDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtCurDate.Location = New System.Drawing.Point(162, 37)
        Me.dtCurDate.Name = "dtCurDate"
        Me.dtCurDate.Size = New System.Drawing.Size(131, 29)
        Me.dtCurDate.TabIndex = 5
        '
        'dtLFYStDate
        '
        Me.dtLFYStDate.Enabled = False
        Me.dtLFYStDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtLFYStDate.Location = New System.Drawing.Point(162, 116)
        Me.dtLFYStDate.Name = "dtLFYStDate"
        Me.dtLFYStDate.Size = New System.Drawing.Size(131, 29)
        Me.dtLFYStDate.TabIndex = 6
        '
        'dtLFYEndDate
        '
        Me.dtLFYEndDate.Enabled = False
        Me.dtLFYEndDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtLFYEndDate.Location = New System.Drawing.Point(299, 116)
        Me.dtLFYEndDate.Name = "dtLFYEndDate"
        Me.dtLFYEndDate.Size = New System.Drawing.Size(131, 29)
        Me.dtLFYEndDate.TabIndex = 7
        '
        'dtCFYStDate
        '
        Me.dtCFYStDate.Enabled = False
        Me.dtCFYStDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtCFYStDate.Location = New System.Drawing.Point(162, 162)
        Me.dtCFYStDate.Name = "dtCFYStDate"
        Me.dtCFYStDate.Size = New System.Drawing.Size(131, 29)
        Me.dtCFYStDate.TabIndex = 8
        '
        'dtCFYEndDate
        '
        Me.dtCFYEndDate.Enabled = False
        Me.dtCFYEndDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtCFYEndDate.Location = New System.Drawing.Point(299, 162)
        Me.dtCFYEndDate.Name = "dtCFYEndDate"
        Me.dtCFYEndDate.Size = New System.Drawing.Size(131, 29)
        Me.dtCFYEndDate.TabIndex = 9
        '
        'btnCreate
        '
        Me.btnCreate.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCreate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCreate.Image = CType(resources.GetObject("btnCreate.Image"), System.Drawing.Image)
        Me.btnCreate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCreate.Location = New System.Drawing.Point(244, 213)
        Me.btnCreate.Name = "btnCreate"
        Me.btnCreate.Size = New System.Drawing.Size(90, 29)
        Me.btnCreate.TabIndex = 10
        Me.btnCreate.Text = "Create"
        Me.btnCreate.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCreate.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(340, 213)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(90, 29)
        Me.btnClose.TabIndex = 11
        Me.btnClose.Text = "Cancel"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'vsfDet
        '
        Me.vsfDet.AllowEditing = False
        Me.vsfDet.AllowFiltering = True
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.Location = New System.Drawing.Point(361, 37)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.Count = 1
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.Rows.GlyphRow = 0
        Me.vsfDet.Size = New System.Drawing.Size(76, 60)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 13
        Me.vsfDet.Visible = False
        '
        'frmMstCreateFinYear
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(449, 254)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.btnCreate)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.dtCFYEndDate)
        Me.Controls.Add(Me.dtCFYStDate)
        Me.Controls.Add(Me.dtLFYEndDate)
        Me.Controls.Add(Me.dtLFYStDate)
        Me.Controls.Add(Me.dtCurDate)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(465, 293)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(465, 293)
        Me.Name = "frmMstCreateFinYear"
        Me.ShowIcon = False
        Me.Text = "Create New FinYear"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents dtCurDate As DateTimePicker
    Friend WithEvents dtLFYStDate As DateTimePicker
    Friend WithEvents dtLFYEndDate As DateTimePicker
    Friend WithEvents dtCFYStDate As DateTimePicker
    Friend WithEvents dtCFYEndDate As DateTimePicker
    Friend WithEvents btnCreate As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
End Class
