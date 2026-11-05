<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlBarCodePrint
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlBarCodePrint))
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfItems = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnLoadItems = New System.Windows.Forms.Button()
        Me.vsfTemp = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnSelItem = New System.Windows.Forms.Button()
        Me.btnSavefldMatrix = New System.Windows.Forms.Button()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfItems, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfTemp, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'vsfDet
        '
        Me.vsfDet.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
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
        Me.vsfDet.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfDet.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfDet.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(16, 58)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 28
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(1034, 435)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 251
        Me.vsfDet.Tag = ""
        '
        'vsfItems
        '
        Me.vsfItems.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfItems.AllowFiltering = True
        Me.vsfItems.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfItems.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfItems.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfItems.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfItems.AutoResize = True
        Me.vsfItems.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfItems.AutoSearchDelay = 2.0R
        Me.vsfItems.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfItems.ColumnInfo = "2,1,0,0,0,120,Columns:0{Width:150;}" & Global.Microsoft.VisualBasic.ChrW(9) & "1{Width:197;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfItems.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfItems.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfItems.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfItems.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfItems.Location = New System.Drawing.Point(805, 38)
        Me.vsfItems.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.vsfItems.Name = "vsfItems"
        Me.vsfItems.Rows.DefaultSize = 28
        Me.vsfItems.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfItems.Size = New System.Drawing.Size(245, 444)
        Me.vsfItems.StyleInfo = resources.GetString("vsfItems.StyleInfo")
        Me.vsfItems.TabIndex = 253
        Me.vsfItems.Tag = ""
        Me.vsfItems.Visible = False
        '
        'btnPrint
        '
        Me.btnPrint.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnPrint.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPrint.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPrint.Location = New System.Drawing.Point(16, 502)
        Me.btnPrint.Margin = New System.Windows.Forms.Padding(4)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(216, 36)
        Me.btnPrint.TabIndex = 281
        Me.btnPrint.Text = "Print"
        Me.btnPrint.UseVisualStyleBackColor = False
        '
        'btnLoadItems
        '
        Me.btnLoadItems.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLoadItems.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnLoadItems.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLoadItems.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnLoadItems.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnLoadItems.Location = New System.Drawing.Point(602, 502)
        Me.btnLoadItems.Margin = New System.Windows.Forms.Padding(4)
        Me.btnLoadItems.Name = "btnLoadItems"
        Me.btnLoadItems.Size = New System.Drawing.Size(161, 36)
        Me.btnLoadItems.TabIndex = 282
        Me.btnLoadItems.Text = "Load for Printing"
        Me.btnLoadItems.UseVisualStyleBackColor = False
        Me.btnLoadItems.Visible = False
        '
        'vsfTemp
        '
        Me.vsfTemp.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfTemp.AllowFiltering = True
        Me.vsfTemp.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfTemp.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfTemp.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfTemp.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfTemp.AutoResize = True
        Me.vsfTemp.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfTemp.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfTemp.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfTemp.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfTemp.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfTemp.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfTemp.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfTemp.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfTemp.Location = New System.Drawing.Point(16, 113)
        Me.vsfTemp.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.vsfTemp.Name = "vsfTemp"
        Me.vsfTemp.Rows.DefaultSize = 28
        Me.vsfTemp.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfTemp.Size = New System.Drawing.Size(178, 158)
        Me.vsfTemp.StyleInfo = resources.GetString("vsfTemp.StyleInfo")
        Me.vsfTemp.TabIndex = 283
        Me.vsfTemp.Tag = ""
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnClose.Location = New System.Drawing.Point(949, 503)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(101, 36)
        Me.btnClose.TabIndex = 252
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnSelItem
        '
        Me.btnSelItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnSelItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSelItem.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnSelItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSelItem.Location = New System.Drawing.Point(16, 13)
        Me.btnSelItem.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSelItem.Name = "btnSelItem"
        Me.btnSelItem.Size = New System.Drawing.Size(186, 36)
        Me.btnSelItem.TabIndex = 284
        Me.btnSelItem.Text = "Select Items"
        Me.btnSelItem.UseVisualStyleBackColor = False
        '
        'btnSavefldMatrix
        '
        Me.btnSavefldMatrix.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSavefldMatrix.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnSavefldMatrix.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSavefldMatrix.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnSavefldMatrix.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSavefldMatrix.Location = New System.Drawing.Point(780, 503)
        Me.btnSavefldMatrix.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSavefldMatrix.Name = "btnSavefldMatrix"
        Me.btnSavefldMatrix.Size = New System.Drawing.Size(161, 36)
        Me.btnSavefldMatrix.TabIndex = 285
        Me.btnSavefldMatrix.Text = "Save Field Matrix"
        Me.btnSavefldMatrix.UseVisualStyleBackColor = False
        Me.btnSavefldMatrix.Visible = False
        '
        'frmUtlBarCodePrint
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1067, 554)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.btnSavefldMatrix)
        Me.Controls.Add(Me.btnSelItem)
        Me.Controls.Add(Me.btnLoadItems)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.vsfItems)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.vsfTemp)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmUtlBarCodePrint"
        Me.ShowIcon = False
        Me.Text = "Barcode Printing"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfItems, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfTemp, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Public WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnClose As Button
    Public WithEvents vsfItems As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnPrint As Button
    Friend WithEvents btnLoadItems As Button
    Public WithEvents vsfTemp As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnSelItem As Button
    Friend WithEvents btnSavefldMatrix As Button
End Class
