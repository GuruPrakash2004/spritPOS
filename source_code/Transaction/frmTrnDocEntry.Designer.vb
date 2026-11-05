<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTrnDocEntry
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTrnDocEntry))
        Me.btnSave = New C1.Win.C1Input.C1Button()
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.pnlBtns = New System.Windows.Forms.Panel()
        Me.btnExport = New C1.Win.C1Input.C1Button()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.lblSupp = New System.Windows.Forms.Label()
        Me.cboSupp = New System.Windows.Forms.ComboBox()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.pnlPending = New System.Windows.Forms.Panel()
        Me.optAll = New System.Windows.Forms.RadioButton()
        Me.optPending = New System.Windows.Forms.RadioButton()
        Me.fraDate = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.dtpToDate = New System.Windows.Forms.DateTimePicker()
        Me.dtpFromDate = New System.Windows.Forms.DateTimePicker()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.vsfOptions = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.pnlBtns.SuspendLayout()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlPending.SuspendLayout()
        Me.fraDate.SuspendLayout()
        CType(Me.vsfOptions, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnSave.Image = Global.vFactIC.My.Resources.Resources.T3_SAVE
        Me.btnSave.Location = New System.Drawing.Point(2, 3)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(90, 30)
        Me.btnSave.TabIndex = 8
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Save : Alt + S")
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnClose.Image = Global.vFactIC.My.Resources.Resources.T3_CLOSE
        Me.btnClose.Location = New System.Drawing.Point(99, 3)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(90, 30)
        Me.btnClose.TabIndex = 42
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close : Alt + O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'pnlBtns
        '
        Me.pnlBtns.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlBtns.Controls.Add(Me.btnSave)
        Me.pnlBtns.Controls.Add(Me.btnClose)
        Me.pnlBtns.Location = New System.Drawing.Point(599, 407)
        Me.pnlBtns.Name = "pnlBtns"
        Me.pnlBtns.Size = New System.Drawing.Size(192, 36)
        Me.pnlBtns.TabIndex = 217
        '
        'btnExport
        '
        Me.btnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExport.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnExport.Image = CType(resources.GetObject("btnExport.Image"), System.Drawing.Image)
        Me.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExport.Location = New System.Drawing.Point(12, 407)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(33, 31)
        Me.btnExport.TabIndex = 225
        Me.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnExport, "Export to Excel : Alt + E")
        Me.btnExport.UseVisualStyleBackColor = False
        '
        'vsfDet
        '
        Me.vsfDet.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfDet.AllowFiltering = True
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.AutoClipboard = True
        Me.vsfDet.AutoResize = True
        Me.vsfDet.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfDet.ClipboardCopyMode = C1.Win.C1FlexGrid.ClipboardCopyModeEnum.DataAndColumnHeaders
        Me.vsfDet.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.Location = New System.Drawing.Point(12, 101)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.ScrollOptions = C1.Win.C1FlexGrid.ScrollFlags.ShowScrollTips
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.CellRange
        Me.vsfDet.Size = New System.Drawing.Size(776, 300)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 226
        Me.vsfDet.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2010Silver
        '
        'lblSupp
        '
        Me.lblSupp.AutoSize = True
        Me.lblSupp.Location = New System.Drawing.Point(171, 17)
        Me.lblSupp.Name = "lblSupp"
        Me.lblSupp.Size = New System.Drawing.Size(45, 13)
        Me.lblSupp.TabIndex = 227
        Me.lblSupp.Text = "Supplier"
        '
        'cboSupp
        '
        Me.cboSupp.FormattingEnabled = True
        Me.cboSupp.Location = New System.Drawing.Point(222, 13)
        Me.cboSupp.Name = "cboSupp"
        Me.cboSupp.Size = New System.Drawing.Size(201, 21)
        Me.cboSupp.TabIndex = 228
        '
        'btnLoad
        '
        Me.btnLoad.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnLoad.ForeColor = System.Drawing.Color.Black
        Me.btnLoad.Location = New System.Drawing.Point(429, 12)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(96, 24)
        Me.btnLoad.TabIndex = 229
        Me.btnLoad.Text = "&Load"
        Me.ToolTip1.SetToolTip(Me.btnLoad, "Load : Alt + L")
        Me.btnLoad.UseVisualStyleBackColor = True
        '
        'pnlPending
        '
        Me.pnlPending.Controls.Add(Me.optAll)
        Me.pnlPending.Location = New System.Drawing.Point(12, 12)
        Me.pnlPending.Name = "pnlPending"
        Me.pnlPending.Size = New System.Drawing.Size(143, 30)
        Me.pnlPending.TabIndex = 239
        '
        'optAll
        '
        Me.optAll.AutoSize = True
        Me.optAll.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optAll.Location = New System.Drawing.Point(4, 3)
        Me.optAll.Name = "optAll"
        Me.optAll.Size = New System.Drawing.Size(40, 21)
        Me.optAll.TabIndex = 235
        Me.optAll.Text = "All"
        Me.optAll.UseVisualStyleBackColor = True
        '
        'optPending
        '
        Me.optPending.AutoSize = True
        Me.optPending.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optPending.Location = New System.Drawing.Point(79, 15)
        Me.optPending.Name = "optPending"
        Me.optPending.Size = New System.Drawing.Size(73, 21)
        Me.optPending.TabIndex = 234
        Me.optPending.Text = "Pending"
        Me.optPending.UseVisualStyleBackColor = True
        '
        'fraDate
        '
        Me.fraDate.Controls.Add(Me.Label4)
        Me.fraDate.Controls.Add(Me.Label3)
        Me.fraDate.Controls.Add(Me.dtpToDate)
        Me.fraDate.Controls.Add(Me.dtpFromDate)
        Me.fraDate.Location = New System.Drawing.Point(174, 40)
        Me.fraDate.Name = "fraDate"
        Me.fraDate.Size = New System.Drawing.Size(351, 55)
        Me.fraDate.TabIndex = 241
        Me.fraDate.TabStop = False
        Me.fraDate.Text = "Receipt Date Range"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(189, 24)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(22, 17)
        Me.Label4.TabIndex = 15
        Me.Label4.Text = "To"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(23, 22)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(38, 17)
        Me.Label3.TabIndex = 14
        Me.Label3.Text = "From"
        '
        'dtpToDate
        '
        Me.dtpToDate.CustomFormat = "dd/MM/yyyy"
        Me.dtpToDate.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpToDate.Location = New System.Drawing.Point(216, 18)
        Me.dtpToDate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtpToDate.Name = "dtpToDate"
        Me.dtpToDate.Size = New System.Drawing.Size(112, 25)
        Me.dtpToDate.TabIndex = 13
        '
        'dtpFromDate
        '
        Me.dtpFromDate.CustomFormat = "dd/MM/yyyy"
        Me.dtpFromDate.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpFromDate.Location = New System.Drawing.Point(67, 18)
        Me.dtpFromDate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtpFromDate.Name = "dtpFromDate"
        Me.dtpFromDate.Size = New System.Drawing.Size(119, 25)
        Me.dtpFromDate.TabIndex = 12
        '
        'vsfOptions
        '
        Me.vsfOptions.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfOptions.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfOptions.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfOptions.ColumnInfo = "2,1,0,0,0,120,Columns:"
        Me.vsfOptions.ExtendLastCol = True
        Me.vsfOptions.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfOptions.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfOptions.Location = New System.Drawing.Point(241, 338)
        Me.vsfOptions.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfOptions.Name = "vsfOptions"
        Me.vsfOptions.Rows.DefaultSize = 24
        Me.vsfOptions.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfOptions.Size = New System.Drawing.Size(162, 41)
        Me.vsfOptions.StyleInfo = resources.GetString("vsfOptions.StyleInfo")
        Me.vsfOptions.TabIndex = 242
        Me.vsfOptions.Tag = ""
        Me.vsfOptions.Visible = False
        '
        'frmTrnDocEntry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.optPending)
        Me.Controls.Add(Me.fraDate)
        Me.Controls.Add(Me.pnlPending)
        Me.Controls.Add(Me.btnLoad)
        Me.Controls.Add(Me.cboSupp)
        Me.Controls.Add(Me.lblSupp)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.pnlBtns)
        Me.Controls.Add(Me.vsfOptions)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmTrnDocEntry"
        Me.Text = "Post Document Entry"
        Me.pnlBtns.ResumeLayout(False)
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlPending.ResumeLayout(False)
        Me.pnlPending.PerformLayout()
        Me.fraDate.ResumeLayout(False)
        Me.fraDate.PerformLayout()
        CType(Me.vsfOptions, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnSave As C1.Win.C1Input.C1Button
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents pnlBtns As Panel
    Friend WithEvents btnExport As C1.Win.C1Input.C1Button
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents lblSupp As Label
    Friend WithEvents cboSupp As ComboBox
    Friend WithEvents btnLoad As Button
    Friend WithEvents pnlPending As Panel
    Friend WithEvents optAll As RadioButton
    Friend WithEvents optPending As RadioButton
    Friend WithEvents fraDate As GroupBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents dtpToDate As DateTimePicker
    Friend WithEvents dtpFromDate As DateTimePicker
    Friend WithEvents ToolTip1 As ToolTip
    Public WithEvents vsfOptions As C1.Win.C1FlexGrid.C1FlexGrid
End Class
