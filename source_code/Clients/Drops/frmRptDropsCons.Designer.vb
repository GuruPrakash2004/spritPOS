<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRptDropsCons
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRptDropsCons))
        Me.btnExcel = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.dtpFromDate = New System.Windows.Forms.DateTimePicker()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnView = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dtpToDate = New System.Windows.Forms.DateTimePicker()
        Me.chkUpdateFIFOrate = New System.Windows.Forms.CheckBox()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnExcel
        '
        Me.btnExcel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExcel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExcel.Location = New System.Drawing.Point(10, 465)
        Me.btnExcel.Name = "btnExcel"
        Me.btnExcel.Size = New System.Drawing.Size(113, 26)
        Me.btnExcel.TabIndex = 251
        Me.btnExcel.Text = "Export to Excel"
        Me.btnExcel.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(28, 21)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(38, 17)
        Me.Label3.TabIndex = 250
        Me.Label3.Text = "From"
        '
        'vsfDet
        '
        Me.vsfDet.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfDet.AllowEditing = False
        Me.vsfDet.AllowFiltering = True
        Me.vsfDet.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfDet.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfDet.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfDet.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.AutoResize = True
        Me.vsfDet.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfDet.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(10, 53)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.Rows.Fixed = 3
        Me.vsfDet.Rows.GlyphRow = 2
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.ShowCellLabels = True
        Me.vsfDet.Size = New System.Drawing.Size(815, 403)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 248
        Me.vsfDet.Tag = "View"
        '
        'dtpFromDate
        '
        Me.dtpFromDate.CustomFormat = "dd/MM/yyyy"
        Me.dtpFromDate.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpFromDate.Location = New System.Drawing.Point(72, 18)
        Me.dtpFromDate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtpFromDate.Name = "dtpFromDate"
        Me.dtpFromDate.Size = New System.Drawing.Size(119, 25)
        Me.dtpFromDate.TabIndex = 0
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Image = CType(resources.GetObject("btnClose.Image"), System.Drawing.Image)
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(746, 463)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(78, 28)
        Me.btnClose.TabIndex = 249
        Me.btnClose.Text = "Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnView
        '
        Me.btnView.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnView.Image = CType(resources.GetObject("btnView.Image"), System.Drawing.Image)
        Me.btnView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnView.Location = New System.Drawing.Point(401, 13)
        Me.btnView.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnView.Name = "btnView"
        Me.btnView.Size = New System.Drawing.Size(79, 30)
        Me.btnView.TabIndex = 2
        Me.btnView.Text = "View"
        Me.btnView.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnView.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(198, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(22, 17)
        Me.Label1.TabIndex = 253
        Me.Label1.Text = "To"
        '
        'dtpToDate
        '
        Me.dtpToDate.CustomFormat = "dd/MM/yyyy"
        Me.dtpToDate.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpToDate.Location = New System.Drawing.Point(242, 18)
        Me.dtpToDate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtpToDate.Name = "dtpToDate"
        Me.dtpToDate.Size = New System.Drawing.Size(119, 25)
        Me.dtpToDate.TabIndex = 1
        '
        'chkUpdateFIFOrate
        '
        Me.chkUpdateFIFOrate.AutoSize = True
        Me.chkUpdateFIFOrate.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkUpdateFIFOrate.Location = New System.Drawing.Point(505, 18)
        Me.chkUpdateFIFOrate.Name = "chkUpdateFIFOrate"
        Me.chkUpdateFIFOrate.Size = New System.Drawing.Size(275, 21)
        Me.chkUpdateFIFOrate.TabIndex = 254
        Me.chkUpdateFIFOrate.Text = "Update Rate Before Generating Report"
        Me.chkUpdateFIFOrate.UseVisualStyleBackColor = True
        '
        'frmRptDropsCons
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(842, 502)
        Me.Controls.Add(Me.chkUpdateFIFOrate)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.dtpToDate)
        Me.Controls.Add(Me.btnExcel)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.btnView)
        Me.Controls.Add(Me.dtpFromDate)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "frmRptDropsCons"
        Me.Text = "Consolidated Report"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnExcel As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents btnClose As Button
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnView As Button
    Friend WithEvents dtpFromDate As DateTimePicker
    Friend WithEvents Label1 As Label
    Friend WithEvents dtpToDate As DateTimePicker
    Friend WithEvents chkUpdateFIFOrate As CheckBox
End Class
