<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTrnBill
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTrnBill))
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.btnGeneric = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.ttBtnShrcts = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnPrintPre = New System.Windows.Forms.Button()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.lblPrintFormat = New System.Windows.Forms.Label()
        Me.cboPrintFormat = New System.Windows.Forms.ComboBox()
        Me.btnEdit = New System.Windows.Forms.Button()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtSearch
        '
        Me.txtSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSearch.Location = New System.Drawing.Point(78, 16)
        Me.txtSearch.MaxLength = 500
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(272, 26)
        Me.txtSearch.TabIndex = 0
        '
        'lblSearch
        '
        Me.lblSearch.AutoSize = True
        Me.lblSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSearch.Location = New System.Drawing.Point(12, 22)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(60, 20)
        Me.lblSearch.TabIndex = 1
        Me.lblSearch.Text = "Search"
        '
        'btnGeneric
        '
        Me.btnGeneric.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGeneric.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGeneric.Location = New System.Drawing.Point(716, 440)
        Me.btnGeneric.Name = "btnGeneric"
        Me.btnGeneric.Size = New System.Drawing.Size(131, 37)
        Me.btnGeneric.TabIndex = 2
        Me.btnGeneric.Text = "Generic"
        Me.btnGeneric.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(848, 440)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(93, 37)
        Me.btnClose.TabIndex = 3
        Me.btnClose.Text = "&Close"
        Me.ttBtnShrcts.SetToolTip(Me.btnClose, "Cancel: Alt+C")
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnPrintPre
        '
        Me.btnPrintPre.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPrintPre.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrintPre.Location = New System.Drawing.Point(579, 440)
        Me.btnPrintPre.Name = "btnPrintPre"
        Me.btnPrintPre.Size = New System.Drawing.Size(131, 37)
        Me.btnPrintPre.TabIndex = 23
        Me.btnPrintPre.Text = "&Print Preview"
        Me.ttBtnShrcts.SetToolTip(Me.btnPrintPre, "Print Preview : Alt + P")
        Me.btnPrintPre.UseVisualStyleBackColor = True
        Me.btnPrintPre.Visible = False
        '
        'vsfDet
        '
        Me.vsfDet.AllowFiltering = True
        Me.vsfDet.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfDet.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfDet.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.AutoResize = True
        Me.vsfDet.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "10,0,0,0,0,140,Columns:4{Width:197;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfDet.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.vsfDet.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(12, 48)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 28
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(929, 386)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 22
        Me.vsfDet.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2007Blue
        '
        'lblPrintFormat
        '
        Me.lblPrintFormat.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblPrintFormat.AutoSize = True
        Me.lblPrintFormat.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPrintFormat.Location = New System.Drawing.Point(12, 451)
        Me.lblPrintFormat.Name = "lblPrintFormat"
        Me.lblPrintFormat.Size = New System.Drawing.Size(96, 20)
        Me.lblPrintFormat.TabIndex = 24
        Me.lblPrintFormat.Text = "Print Format"
        Me.lblPrintFormat.Visible = False
        '
        'cboPrintFormat
        '
        Me.cboPrintFormat.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cboPrintFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPrintFormat.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboPrintFormat.FormattingEnabled = True
        Me.cboPrintFormat.Location = New System.Drawing.Point(113, 446)
        Me.cboPrintFormat.Name = "cboPrintFormat"
        Me.cboPrintFormat.Size = New System.Drawing.Size(170, 28)
        Me.cboPrintFormat.TabIndex = 25
        Me.cboPrintFormat.Visible = False
        '
        'btnEdit
        '
        Me.btnEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEdit.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.btnEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEdit.Location = New System.Drawing.Point(423, 440)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(131, 37)
        Me.btnEdit.TabIndex = 26
        Me.btnEdit.Text = "Edit Bill"
        Me.btnEdit.UseVisualStyleBackColor = False
        '
        'frmTrnBill
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(952, 489)
        Me.Controls.Add(Me.btnEdit)
        Me.Controls.Add(Me.cboPrintFormat)
        Me.Controls.Add(Me.lblPrintFormat)
        Me.Controls.Add(Me.btnPrintPre)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnGeneric)
        Me.Controls.Add(Me.lblSearch)
        Me.Controls.Add(Me.txtSearch)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmTrnBill"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmTrnBill"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtSearch As TextBox
    Friend WithEvents lblSearch As Label
    Friend WithEvents btnGeneric As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents ttBtnShrcts As ToolTip
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnPrintPre As Button
    Friend WithEvents lblPrintFormat As Label
    Friend WithEvents cboPrintFormat As ComboBox
    Friend WithEvents btnEdit As Button
End Class
