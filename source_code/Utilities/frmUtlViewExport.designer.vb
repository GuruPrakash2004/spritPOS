<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlViewExport
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlViewExport))
        Me.fraView = New System.Windows.Forms.Panel()
        Me.vsfSearch = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfView = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.fraControls = New System.Windows.Forms.Panel()
        Me.btnExport = New C1.Win.C1Input.C1Button()
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.fraView.SuspendLayout()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraControls.SuspendLayout()
        Me.SuspendLayout()
        '
        'fraView
        '
        Me.fraView.Controls.Add(Me.vsfSearch)
        Me.fraView.Controls.Add(Me.vsfView)
        Me.fraView.Controls.Add(Me.fraControls)
        Me.fraView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.fraView.Location = New System.Drawing.Point(0, 0)
        Me.fraView.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.fraView.Name = "fraView"
        Me.fraView.Size = New System.Drawing.Size(1280, 722)
        Me.fraView.TabIndex = 1
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
        Me.vsfSearch.Location = New System.Drawing.Point(4, 2)
        Me.vsfSearch.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.vsfSearch.Name = "vsfSearch"
        Me.vsfSearch.Rows.Count = 1
        Me.vsfSearch.Rows.DefaultSize = 28
        Me.vsfSearch.Rows.Fixed = 0
        Me.vsfSearch.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.vsfSearch.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfSearch.ShowCellLabels = True
        Me.vsfSearch.Size = New System.Drawing.Size(1271, 32)
        Me.vsfSearch.StyleInfo = resources.GetString("vsfSearch.StyleInfo")
        Me.vsfSearch.TabIndex = 6
        Me.vsfSearch.Tag = ""
        Me.vsfSearch.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.System
        '
        'vsfView
        '
        Me.vsfView.AllowEditing = False
        Me.vsfView.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfView.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfView.ColumnInfo = "4,1,0,0,0,120,Columns:0{Width:50;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfView.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfView.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfView.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfView.Location = New System.Drawing.Point(4, 32)
        Me.vsfView.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.vsfView.Name = "vsfView"
        Me.vsfView.Rows.DefaultSize = 28
        Me.vsfView.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfView.Size = New System.Drawing.Size(1271, 606)
        Me.vsfView.StyleInfo = resources.GetString("vsfView.StyleInfo")
        Me.vsfView.TabIndex = 3
        Me.vsfView.Tag = ""
        '
        'fraControls
        '
        Me.fraControls.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fraControls.Controls.Add(Me.btnExport)
        Me.fraControls.Controls.Add(Me.btnClose)
        Me.fraControls.Location = New System.Drawing.Point(4, 644)
        Me.fraControls.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.fraControls.Name = "fraControls"
        Me.fraControls.Size = New System.Drawing.Size(1272, 71)
        Me.fraControls.TabIndex = 4
        '
        'btnExport
        '
        Me.btnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExport.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnExport.Image = CType(resources.GetObject("btnExport.Image"), System.Drawing.Image)
        Me.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExport.Location = New System.Drawing.Point(1108, 18)
        Me.btnExport.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(44, 38)
        Me.btnExport.TabIndex = 205
        Me.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnExport.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(1160, 18)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(99, 38)
        Me.btnClose.TabIndex = 201
        Me.btnClose.Text = "&Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'frmUtlViewExport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1280, 722)
        Me.Controls.Add(Me.fraView)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "frmUtlViewExport"
        Me.ShowIcon = False
        Me.Text = "View / Export"
        Me.fraView.ResumeLayout(False)
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraControls.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents fraView As System.Windows.Forms.Panel
    Friend WithEvents vsfView As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents fraControls As System.Windows.Forms.Panel
    Friend WithEvents btnExport As C1.Win.C1Input.C1Button
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents vsfSearch As C1.Win.C1FlexGrid.C1FlexGrid
End Class
