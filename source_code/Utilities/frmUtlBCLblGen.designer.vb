<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlBCLblGen
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlBCLblGen))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.btnDesign = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.GBView = New System.Windows.Forms.GroupBox()
        Me.vsfSearch = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfView = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.txtPrinter = New System.Windows.Forms.TextBox()
        Me.PBRpts = New System.Windows.Forms.PictureBox()
        Me.txtEditor = New System.Windows.Forms.TextBox()
        Me.cboBCLbl = New System.Windows.Forms.ComboBox()
        Me.lblBC = New System.Windows.Forms.Label()
        Me.lblPrinter = New System.Windows.Forms.Label()
        Me.fraCmdScr2 = New System.Windows.Forms.Panel()
        Me.chkEditor = New System.Windows.Forms.CheckBox()
        Me.lblNoOfCopies = New System.Windows.Forms.Label()
        Me.txtNoOfCopies = New System.Windows.Forms.TextBox()
        Me.txtLblID = New System.Windows.Forms.TextBox()
        Me.lblRmv = New System.Windows.Forms.Label()
        Me.btnSelAll = New System.Windows.Forms.Button()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GBView.SuspendLayout()
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.PBRpts, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraCmdScr2.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnLoad
        '
        Me.btnLoad.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLoad.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.btnLoad.Location = New System.Drawing.Point(467, 5)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(75, 29)
        Me.btnLoad.TabIndex = 13
        Me.btnLoad.Text = "&Load"
        Me.ToolTip1.SetToolTip(Me.btnLoad, "Load : Alt + L")
        Me.btnLoad.UseVisualStyleBackColor = False
        '
        'btnDesign
        '
        Me.btnDesign.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDesign.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnDesign.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDesign.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.btnDesign.Image = CType(resources.GetObject("btnDesign.Image"), System.Drawing.Image)
        Me.btnDesign.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDesign.Location = New System.Drawing.Point(852, 3)
        Me.btnDesign.Name = "btnDesign"
        Me.btnDesign.Size = New System.Drawing.Size(91, 34)
        Me.btnDesign.TabIndex = 11
        Me.btnDesign.Text = " &Design"
        Me.btnDesign.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnDesign, "Design : Alt + D")
        Me.btnDesign.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(104, 5)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(88, 29)
        Me.btnClose.TabIndex = 1
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close : Alt + O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnPrint
        '
        Me.btnPrint.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPrint.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_PRINT
        Me.btnPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPrint.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPrint.Location = New System.Drawing.Point(10, 5)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(88, 29)
        Me.btnPrint.TabIndex = 2
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnPrint, "Print : Alt + P")
        Me.btnPrint.UseVisualStyleBackColor = False
        '
        'vsfDet
        '
        Me.vsfDet.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfDet.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(12, 398)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(940, 194)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 38
        Me.vsfDet.Tag = ""
        '
        'GBView
        '
        Me.GBView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GBView.Controls.Add(Me.vsfSearch)
        Me.GBView.Controls.Add(Me.vsfView)
        Me.GBView.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.GBView.Location = New System.Drawing.Point(3, 164)
        Me.GBView.Name = "GBView"
        Me.GBView.Size = New System.Drawing.Size(957, 193)
        Me.GBView.TabIndex = 37
        Me.GBView.TabStop = False
        Me.GBView.Text = "Select Data"
        '
        'vsfSearch
        '
        Me.vsfSearch.AllowFiltering = True
        Me.vsfSearch.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfSearch.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfSearch.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfSearch.AutoResize = True
        Me.vsfSearch.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfSearch.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfSearch.ColumnInfo = "2,0,0,0,0,95,Columns:"
        Me.vsfSearch.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfSearch.ExtendLastCol = True
        Me.vsfSearch.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.vsfSearch.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfSearch.Location = New System.Drawing.Point(9, 16)
        Me.vsfSearch.Name = "vsfSearch"
        Me.vsfSearch.Rows.Count = 1
        Me.vsfSearch.Rows.DefaultSize = 19
        Me.vsfSearch.Rows.Fixed = 0
        Me.vsfSearch.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.vsfSearch.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfSearch.ShowCellLabels = True
        Me.vsfSearch.Size = New System.Drawing.Size(940, 20)
        Me.vsfSearch.StyleInfo = resources.GetString("vsfSearch.StyleInfo")
        Me.vsfSearch.TabIndex = 31
        Me.vsfSearch.Tag = ""
        Me.vsfSearch.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.System
        '
        'vsfView
        '
        Me.vsfView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfView.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfView.ColumnInfo = "10,0,0,0,0,95,Columns:"
        Me.vsfView.ExtendLastCol = True
        Me.vsfView.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.vsfView.Location = New System.Drawing.Point(9, 37)
        Me.vsfView.Name = "vsfView"
        Me.vsfView.Rows.DefaultSize = 19
        Me.vsfView.Size = New System.Drawing.Size(940, 149)
        Me.vsfView.StyleInfo = resources.GetString("vsfView.StyleInfo")
        Me.vsfView.TabIndex = 30
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.txtPrinter)
        Me.Panel1.Controls.Add(Me.PBRpts)
        Me.Panel1.Controls.Add(Me.txtEditor)
        Me.Panel1.Controls.Add(Me.btnLoad)
        Me.Panel1.Controls.Add(Me.cboBCLbl)
        Me.Panel1.Controls.Add(Me.lblBC)
        Me.Panel1.Controls.Add(Me.lblPrinter)
        Me.Panel1.Controls.Add(Me.btnDesign)
        Me.Panel1.Controls.Add(Me.fraCmdScr2)
        Me.Panel1.Controls.Add(Me.chkEditor)
        Me.Panel1.Controls.Add(Me.lblNoOfCopies)
        Me.Panel1.Controls.Add(Me.txtNoOfCopies)
        Me.Panel1.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Panel1.Location = New System.Drawing.Point(3, 12)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(957, 146)
        Me.Panel1.TabIndex = 36
        '
        'txtPrinter
        '
        Me.txtPrinter.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPrinter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPrinter.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.txtPrinter.Location = New System.Drawing.Point(455, 52)
        Me.txtPrinter.MaxLength = 3
        Me.txtPrinter.Name = "txtPrinter"
        Me.txtPrinter.Size = New System.Drawing.Size(290, 26)
        Me.txtPrinter.TabIndex = 34
        '
        'PBRpts
        '
        Me.PBRpts.Location = New System.Drawing.Point(95, 42)
        Me.PBRpts.Name = "PBRpts"
        Me.PBRpts.Size = New System.Drawing.Size(123, 99)
        Me.PBRpts.TabIndex = 33
        Me.PBRpts.TabStop = False
        '
        'txtEditor
        '
        Me.txtEditor.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtEditor.Location = New System.Drawing.Point(761, 87)
        Me.txtEditor.Multiline = True
        Me.txtEditor.Name = "txtEditor"
        Me.txtEditor.Size = New System.Drawing.Size(88, 33)
        Me.txtEditor.TabIndex = 32
        Me.txtEditor.Visible = False
        '
        'cboBCLbl
        '
        Me.cboBCLbl.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboBCLbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBCLbl.FormattingEnabled = True
        Me.cboBCLbl.Location = New System.Drawing.Point(175, 6)
        Me.cboBCLbl.MaxLength = 150
        Me.cboBCLbl.Name = "cboBCLbl"
        Me.cboBCLbl.Size = New System.Drawing.Size(286, 24)
        Me.cboBCLbl.TabIndex = 1
        '
        'lblBC
        '
        Me.lblBC.AutoSize = True
        Me.lblBC.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.lblBC.Location = New System.Drawing.Point(5, 9)
        Me.lblBC.Name = "lblBC"
        Me.lblBC.Size = New System.Drawing.Size(164, 20)
        Me.lblBC.TabIndex = 0
        Me.lblBC.Text = "Select BarCode Label"
        '
        'lblPrinter
        '
        Me.lblPrinter.AutoSize = True
        Me.lblPrinter.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.lblPrinter.Location = New System.Drawing.Point(336, 57)
        Me.lblPrinter.Name = "lblPrinter"
        Me.lblPrinter.Size = New System.Drawing.Size(55, 20)
        Me.lblPrinter.TabIndex = 0
        Me.lblPrinter.Text = "Printer"
        '
        'fraCmdScr2
        '
        Me.fraCmdScr2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fraCmdScr2.Controls.Add(Me.btnPrint)
        Me.fraCmdScr2.Controls.Add(Me.btnClose)
        Me.fraCmdScr2.Location = New System.Drawing.Point(751, 45)
        Me.fraCmdScr2.Name = "fraCmdScr2"
        Me.fraCmdScr2.Size = New System.Drawing.Size(197, 38)
        Me.fraCmdScr2.TabIndex = 10
        '
        'chkEditor
        '
        Me.chkEditor.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkEditor.AutoSize = True
        Me.chkEditor.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.chkEditor.Location = New System.Drawing.Point(529, 85)
        Me.chkEditor.Name = "chkEditor"
        Me.chkEditor.Size = New System.Drawing.Size(131, 24)
        Me.chkEditor.TabIndex = 9
        Me.chkEditor.Text = "Open In Editor"
        Me.chkEditor.UseVisualStyleBackColor = True
        '
        'lblNoOfCopies
        '
        Me.lblNoOfCopies.AutoSize = True
        Me.lblNoOfCopies.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.lblNoOfCopies.ForeColor = System.Drawing.Color.Black
        Me.lblNoOfCopies.Location = New System.Drawing.Point(336, 87)
        Me.lblNoOfCopies.Name = "lblNoOfCopies"
        Me.lblNoOfCopies.Size = New System.Drawing.Size(100, 20)
        Me.lblNoOfCopies.TabIndex = 7
        Me.lblNoOfCopies.Tag = ""
        Me.lblNoOfCopies.Text = "No of Copies"
        '
        'txtNoOfCopies
        '
        Me.txtNoOfCopies.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNoOfCopies.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtNoOfCopies.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.txtNoOfCopies.Location = New System.Drawing.Point(455, 84)
        Me.txtNoOfCopies.MaxLength = 3
        Me.txtNoOfCopies.Name = "txtNoOfCopies"
        Me.txtNoOfCopies.Size = New System.Drawing.Size(69, 26)
        Me.txtNoOfCopies.TabIndex = 8
        Me.txtNoOfCopies.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtLblID
        '
        Me.txtLblID.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtLblID.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.txtLblID.ForeColor = System.Drawing.SystemColors.Control
        Me.txtLblID.Location = New System.Drawing.Point(801, 582)
        Me.txtLblID.Name = "txtLblID"
        Me.txtLblID.Size = New System.Drawing.Size(56, 21)
        Me.txtLblID.TabIndex = 36
        Me.txtLblID.Visible = False
        '
        'lblRmv
        '
        Me.lblRmv.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblRmv.AutoSize = True
        Me.lblRmv.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.lblRmv.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblRmv.Location = New System.Drawing.Point(774, 374)
        Me.lblRmv.Name = "lblRmv"
        Me.lblRmv.Size = New System.Drawing.Size(178, 20)
        Me.lblRmv.TabIndex = 40
        Me.lblRmv.Tag = "R"
        Me.lblRmv.Text = "Double Click to Remove"
        '
        'btnSelAll
        '
        Me.btnSelAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSelAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSelAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.btnSelAll.Image = CType(resources.GetObject("btnSelAll.Image"), System.Drawing.Image)
        Me.btnSelAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSelAll.Location = New System.Drawing.Point(13, 363)
        Me.btnSelAll.Name = "btnSelAll"
        Me.btnSelAll.Size = New System.Drawing.Size(117, 29)
        Me.btnSelAll.TabIndex = 39
        Me.btnSelAll.Text = "&Select All"
        Me.btnSelAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSelAll.UseVisualStyleBackColor = False
        '
        'frmUtlBCLblGen
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(962, 604)
        Me.Controls.Add(Me.txtLblID)
        Me.Controls.Add(Me.lblRmv)
        Me.Controls.Add(Me.btnSelAll)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.GBView)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "frmUtlBCLblGen"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "BarCode Label Generation"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GBView.ResumeLayout(False)
        CType(Me.vsfSearch, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PBRpts, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraCmdScr2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents GBView As GroupBox
    Friend WithEvents vsfSearch As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents vsfView As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents Panel1 As Panel
    Friend WithEvents txtLblID As TextBox
    Friend WithEvents txtEditor As TextBox
    Friend WithEvents btnLoad As Button
    Friend WithEvents cboBCLbl As ComboBox
    Friend WithEvents lblBC As Label
    Friend WithEvents btnDesign As Button
    Friend WithEvents fraCmdScr2 As Panel
    Friend WithEvents btnClose As Button
    Friend WithEvents chkEditor As CheckBox
    Friend WithEvents lblNoOfCopies As Label
    Friend WithEvents txtNoOfCopies As TextBox
    Friend WithEvents lblRmv As Label
    Friend WithEvents btnSelAll As Button
    Friend WithEvents lblPrinter As Label
    Friend WithEvents btnPrint As Button
    Friend WithEvents PBRpts As PictureBox
    Friend WithEvents txtPrinter As TextBox
End Class
