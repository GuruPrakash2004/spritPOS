<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlQuickRef
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlQuickRef))
        Me.txtItem = New System.Windows.Forms.TextBox()
        Me.txtItemDet = New System.Windows.Forms.TextBox()
        Me.txtMRP = New System.Windows.Forms.TextBox()
        Me.txtSP = New System.Windows.Forms.TextBox()
        Me.txtQOH = New System.Windows.Forms.TextBox()
        Me.txtDummy = New System.Windows.Forms.TextBox()
        Me.lblSP = New System.Windows.Forms.Label()
        Me.lblQoh = New System.Windows.Forms.Label()
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblMRP = New System.Windows.Forms.Label()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfSearch = New C1.Win.C1FlexGrid.C1FlexGrid()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtItem
        '
        Me.txtItem.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtItem.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtItem.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtItem.Location = New System.Drawing.Point(149, 12)
        Me.txtItem.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtItem.MaxLength = 500
        Me.txtItem.Name = "txtItem"
        Me.txtItem.Size = New System.Drawing.Size(363, 14)
        Me.txtItem.TabIndex = 0
        '
        'txtItemDet
        '
        Me.txtItemDet.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtItemDet.BackColor = System.Drawing.Color.White
        Me.txtItemDet.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtItemDet.Cursor = System.Windows.Forms.Cursors.Default
        Me.txtItemDet.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtItemDet.Location = New System.Drawing.Point(149, 32)
        Me.txtItemDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtItemDet.MaxLength = 500
        Me.txtItemDet.Multiline = True
        Me.txtItemDet.Name = "txtItemDet"
        Me.txtItemDet.ReadOnly = True
        Me.txtItemDet.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtItemDet.Size = New System.Drawing.Size(363, 78)
        Me.txtItemDet.TabIndex = 32
        '
        'txtMRP
        '
        Me.txtMRP.BackColor = System.Drawing.Color.White
        Me.txtMRP.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtMRP.Cursor = System.Windows.Forms.Cursors.Default
        Me.txtMRP.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMRP.Location = New System.Drawing.Point(149, 117)
        Me.txtMRP.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtMRP.MaxLength = 50
        Me.txtMRP.Name = "txtMRP"
        Me.txtMRP.ReadOnly = True
        Me.txtMRP.Size = New System.Drawing.Size(148, 14)
        Me.txtMRP.TabIndex = 33
        '
        'txtSP
        '
        Me.txtSP.BackColor = System.Drawing.Color.White
        Me.txtSP.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtSP.Cursor = System.Windows.Forms.Cursors.Default
        Me.txtSP.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSP.Location = New System.Drawing.Point(149, 139)
        Me.txtSP.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtSP.MaxLength = 50
        Me.txtSP.Name = "txtSP"
        Me.txtSP.ReadOnly = True
        Me.txtSP.Size = New System.Drawing.Size(148, 14)
        Me.txtSP.TabIndex = 35
        '
        'txtQOH
        '
        Me.txtQOH.BackColor = System.Drawing.Color.White
        Me.txtQOH.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtQOH.Cursor = System.Windows.Forms.Cursors.Default
        Me.txtQOH.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtQOH.Location = New System.Drawing.Point(149, 162)
        Me.txtQOH.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtQOH.MaxLength = 50
        Me.txtQOH.Name = "txtQOH"
        Me.txtQOH.ReadOnly = True
        Me.txtQOH.Size = New System.Drawing.Size(148, 14)
        Me.txtQOH.TabIndex = 36
        '
        'txtDummy
        '
        Me.txtDummy.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDummy.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtDummy.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDummy.Location = New System.Drawing.Point(9, 78)
        Me.txtDummy.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtDummy.MaxLength = 500
        Me.txtDummy.Name = "txtDummy"
        Me.txtDummy.Size = New System.Drawing.Size(107, 28)
        Me.txtDummy.TabIndex = 27
        Me.txtDummy.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtDummy.Visible = False
        '
        'lblSP
        '
        Me.lblSP.BackColor = System.Drawing.Color.Transparent
        Me.lblSP.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.lblSP.ForeColor = System.Drawing.Color.Blue
        Me.lblSP.Location = New System.Drawing.Point(10, 112)
        Me.lblSP.Name = "lblSP"
        Me.lblSP.Size = New System.Drawing.Size(111, 22)
        Me.lblSP.TabIndex = 24
        Me.lblSP.Tag = "R"
        Me.lblSP.Text = "Selling Price"
        Me.lblSP.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblQoh
        '
        Me.lblQoh.BackColor = System.Drawing.Color.Transparent
        Me.lblQoh.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.lblQoh.ForeColor = System.Drawing.Color.Blue
        Me.lblQoh.Location = New System.Drawing.Point(11, 160)
        Me.lblQoh.Name = "lblQoh"
        Me.lblQoh.Size = New System.Drawing.Size(57, 16)
        Me.lblQoh.TabIndex = 25
        Me.lblQoh.Tag = "R"
        Me.lblQoh.Text = "QOH"
        Me.lblQoh.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblSearch
        '
        Me.lblSearch.BackColor = System.Drawing.Color.Transparent
        Me.lblSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.lblSearch.ForeColor = System.Drawing.Color.Black
        Me.lblSearch.Location = New System.Drawing.Point(11, 9)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(67, 18)
        Me.lblSearch.TabIndex = 15
        Me.lblSearch.Tag = ""
        Me.lblSearch.Text = "Search"
        Me.lblSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(11, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(109, 21)
        Me.Label1.TabIndex = 19
        Me.Label1.Tag = "R"
        Me.Label1.Text = "Item Details"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblMRP
        '
        Me.lblMRP.BackColor = System.Drawing.Color.Transparent
        Me.lblMRP.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.lblMRP.ForeColor = System.Drawing.Color.Blue
        Me.lblMRP.Location = New System.Drawing.Point(10, 137)
        Me.lblMRP.Name = "lblMRP"
        Me.lblMRP.Size = New System.Drawing.Size(57, 16)
        Me.lblMRP.TabIndex = 23
        Me.lblMRP.Tag = "R"
        Me.lblMRP.Text = "MRP"
        Me.lblMRP.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'vsfDet
        '
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.Location = New System.Drawing.Point(8, 201)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.Size = New System.Drawing.Size(506, 268)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 2
        '
        'vsfSearch
        '
        Me.vsfSearch.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfSearch.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfSearch.ExtendLastCol = True
        Me.vsfSearch.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfSearch.Location = New System.Drawing.Point(8, 29)
        Me.vsfSearch.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfSearch.Name = "vsfSearch"
        Me.vsfSearch.Rows.DefaultSize = 24
        Me.vsfSearch.Size = New System.Drawing.Size(506, 440)
        Me.vsfSearch.StyleInfo = resources.GetString("vsfSearch.StyleInfo")
        Me.vsfSearch.TabIndex = 1
        '
        'frmUtlQuickRef
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(522, 471)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.txtDummy)
        Me.Controls.Add(Me.lblQoh)
        Me.Controls.Add(Me.lblSP)
        Me.Controls.Add(Me.lblMRP)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtItem)
        Me.Controls.Add(Me.lblSearch)
        Me.Controls.Add(Me.txtQOH)
        Me.Controls.Add(Me.txtSP)
        Me.Controls.Add(Me.txtMRP)
        Me.Controls.Add(Me.txtItemDet)
        Me.Controls.Add(Me.vsfSearch)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmUtlQuickRef"
        Me.ShowIcon = False
        Me.Text = "Quick Reference"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents txtItem As TextBox
    Public WithEvents txtItemDet As TextBox
    Public WithEvents txtMRP As TextBox
    Public WithEvents txtSP As TextBox
    Public WithEvents txtQOH As TextBox
    Public WithEvents txtDummy As TextBox
    Friend WithEvents lblSP As Label
    Friend WithEvents lblQoh As Label
    Friend WithEvents lblSearch As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents lblMRP As Label
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents vsfSearch As C1.Win.C1FlexGrid.C1FlexGrid
End Class
