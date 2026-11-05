<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlMultiSelection
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlMultiSelection))
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfSearch = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.chkAll = New System.Windows.Forms.CheckBox()
        Me.btnPaste = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.pnlPaste = New System.Windows.Forms.GroupBox()
        Me.btnPasteCncl = New System.Windows.Forms.Button()
        Me.btnPasteOK = New System.Windows.Forms.Button()
        Me.txtPasteData = New System.Windows.Forms.TextBox()
        Me.cboColName = New System.Windows.Forms.ComboBox()
        Me.lblLocation = New System.Windows.Forms.Label()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlPaste.SuspendLayout()
        Me.SuspendLayout()
        '
        'vsfDet
        '
        Me.vsfDet.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.AutoResize = True
        Me.vsfDet.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfDet.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.Location = New System.Drawing.Point(0, 24)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.ScrollOptions = C1.Win.C1FlexGrid.ScrollFlags.ShowScrollTips
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(565, 415)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 1
        Me.vsfDet.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2010Silver
        '
        'vsfSearch
        '
        Me.vsfSearch.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfSearch.AllowFiltering = True
        Me.vsfSearch.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfSearch.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfSearch.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfSearch.AutoResize = True
        Me.vsfSearch.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfSearch.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfSearch.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfSearch.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfSearch.ExtendLastCol = True
        Me.vsfSearch.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfSearch.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfSearch.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfSearch.Location = New System.Drawing.Point(1, 1)
        Me.vsfSearch.Name = "vsfSearch"
        Me.vsfSearch.Rows.Count = 1
        Me.vsfSearch.Rows.DefaultSize = 24
        Me.vsfSearch.Rows.Fixed = 0
        Me.vsfSearch.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.vsfSearch.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfSearch.ShowCellLabels = True
        Me.vsfSearch.Size = New System.Drawing.Size(565, 24)
        Me.vsfSearch.StyleInfo = resources.GetString("vsfSearch.StyleInfo")
        Me.vsfSearch.TabIndex = 0
        Me.vsfSearch.Tag = ""
        Me.vsfSearch.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.System
        '
        'chkAll
        '
        Me.chkAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkAll.AutoSize = True
        Me.chkAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAll.Location = New System.Drawing.Point(12, 448)
        Me.chkAll.Name = "chkAll"
        Me.chkAll.Size = New System.Drawing.Size(94, 24)
        Me.chkAll.TabIndex = 2
        Me.chkAll.Text = "Select &All"
        Me.chkAll.UseVisualStyleBackColor = True
        '
        'btnPaste
        '
        Me.btnPaste.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnPaste.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPaste.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPaste.Location = New System.Drawing.Point(126, 446)
        Me.btnPaste.Name = "btnPaste"
        Me.btnPaste.Size = New System.Drawing.Size(79, 29)
        Me.btnPaste.TabIndex = 5
        Me.btnPaste.Text = "&Paste"
        Me.btnPaste.UseVisualStyleBackColor = False
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOK.Image = CType(resources.GetObject("btnOK.Image"), System.Drawing.Image)
        Me.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnOK.Location = New System.Drawing.Point(391, 448)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(84, 29)
        Me.btnOK.TabIndex = 3
        Me.btnOK.Text = "&Ok"
        Me.btnOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnOK.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(481, 448)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(84, 29)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'pnlPaste
        '
        Me.pnlPaste.Controls.Add(Me.btnPasteCncl)
        Me.pnlPaste.Controls.Add(Me.btnPasteOK)
        Me.pnlPaste.Controls.Add(Me.txtPasteData)
        Me.pnlPaste.Controls.Add(Me.cboColName)
        Me.pnlPaste.Controls.Add(Me.lblLocation)
        Me.pnlPaste.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.pnlPaste.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlPaste.Location = New System.Drawing.Point(62, 24)
        Me.pnlPaste.Name = "pnlPaste"
        Me.pnlPaste.Size = New System.Drawing.Size(309, 415)
        Me.pnlPaste.TabIndex = 6
        Me.pnlPaste.TabStop = False
        Me.pnlPaste.Text = "Paste Data you want to Select"
        Me.pnlPaste.Visible = False
        '
        'btnPasteCncl
        '
        Me.btnPasteCncl.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnPasteCncl.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnPasteCncl.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPasteCncl.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPasteCncl.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPasteCncl.Location = New System.Drawing.Point(215, 378)
        Me.btnPasteCncl.Name = "btnPasteCncl"
        Me.btnPasteCncl.Size = New System.Drawing.Size(88, 31)
        Me.btnPasteCncl.TabIndex = 264
        Me.btnPasteCncl.Text = "&Cancel"
        Me.btnPasteCncl.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnPasteCncl.UseVisualStyleBackColor = False
        '
        'btnPasteOK
        '
        Me.btnPasteOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnPasteOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPasteOK.Image = CType(resources.GetObject("btnPasteOK.Image"), System.Drawing.Image)
        Me.btnPasteOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPasteOK.Location = New System.Drawing.Point(133, 377)
        Me.btnPasteOK.Name = "btnPasteOK"
        Me.btnPasteOK.Size = New System.Drawing.Size(75, 33)
        Me.btnPasteOK.TabIndex = 263
        Me.btnPasteOK.Text = "&Ok"
        Me.btnPasteOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnPasteOK.UseVisualStyleBackColor = False
        '
        'txtPasteData
        '
        Me.txtPasteData.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPasteData.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPasteData.Location = New System.Drawing.Point(9, 86)
        Me.txtPasteData.Multiline = True
        Me.txtPasteData.Name = "txtPasteData"
        Me.txtPasteData.Size = New System.Drawing.Size(294, 285)
        Me.txtPasteData.TabIndex = 262
        '
        'cboColName
        '
        Me.cboColName.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboColName.FormattingEnabled = True
        Me.cboColName.Location = New System.Drawing.Point(9, 55)
        Me.cboColName.Name = "cboColName"
        Me.cboColName.Size = New System.Drawing.Size(294, 25)
        Me.cboColName.TabIndex = 254
        '
        'lblLocation
        '
        Me.lblLocation.AutoSize = True
        Me.lblLocation.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.lblLocation.Location = New System.Drawing.Point(6, 36)
        Me.lblLocation.Name = "lblLocation"
        Me.lblLocation.Size = New System.Drawing.Size(139, 16)
        Me.lblLocation.TabIndex = 255
        Me.lblLocation.Text = "Select Colum to Paste"
        '
        'frmUtlMultiSelection
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(568, 480)
        Me.Controls.Add(Me.pnlPaste)
        Me.Controls.Add(Me.btnPaste)
        Me.Controls.Add(Me.chkAll)
        Me.Controls.Add(Me.vsfSearch)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnCancel)
        Me.Name = "frmUtlMultiSelection"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frmUtlSelection"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlPaste.ResumeLayout(False)
        Me.pnlPaste.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents vsfSearch As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents chkAll As System.Windows.Forms.CheckBox
    Public WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnPaste As Button
    Friend WithEvents pnlPaste As GroupBox
    Friend WithEvents cboColName As ComboBox
    Friend WithEvents lblLocation As Label
    Friend WithEvents btnPasteOK As Button
    Friend WithEvents txtPasteData As TextBox
    Friend WithEvents btnPasteCncl As Button
End Class
