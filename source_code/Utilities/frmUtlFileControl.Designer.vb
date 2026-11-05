<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlFileControl
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlFileControl))
        Dim CBlendItems1 As vLQMS.cBlendItems = New vLQMS.cBlendItems()
        Dim CBlendItems2 As vLQMS.cBlendItems = New vLQMS.cBlendItems()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.lblPreview = New System.Windows.Forms.Label()
        Me.img = New C1.Win.C1Input.C1PictureBox()
        Me.ofd = New System.Windows.Forms.OpenFileDialog()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnSave = New C1.Win.C1Input.C1Button()
        Me.lblHelp = New System.Windows.Forms.Label()
        Me.tt = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnAddImg = New vLQMS.CButton()
        Me.btnFileAttach = New vLQMS.CButton()
        Me.Panel1.SuspendLayout()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        CType(Me.img, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Panel1.Controls.Add(Me.vsfDet)
        Me.Panel1.Location = New System.Drawing.Point(5, 42)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(500, 227)
        Me.Panel1.TabIndex = 0
        '
        'vsfDet
        '
        Me.vsfDet.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfDet.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfDet.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.AutoResize = True
        Me.vsfDet.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "3,0,0,0,0,110,Columns:"
        Me.vsfDet.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 8.5!)
        Me.vsfDet.ForeColor = System.Drawing.Color.Black
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.Count = 2
        Me.vsfDet.Rows.DefaultSize = 22
        Me.vsfDet.Rows.Fixed = 0
        Me.vsfDet.Rows.GlyphRow = 0
        Me.vsfDet.Rows.MinSize = 30
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.ShowCellLabels = True
        Me.vsfDet.Size = New System.Drawing.Size(491, 220)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 1
        Me.vsfDet.Tag = "View"
        Me.vsfDet.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2007Blue
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.lblPreview)
        Me.Panel2.Controls.Add(Me.img)
        Me.Panel2.Location = New System.Drawing.Point(510, 42)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(180, 227)
        Me.Panel2.TabIndex = 1
        '
        'lblPreview
        '
        Me.lblPreview.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPreview.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.lblPreview.Location = New System.Drawing.Point(104, 141)
        Me.lblPreview.Name = "lblPreview"
        Me.lblPreview.Size = New System.Drawing.Size(0, 0)
        Me.lblPreview.TabIndex = 23
        Me.lblPreview.Text = "Preview Not Available"
        Me.lblPreview.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'img
        '
        Me.img.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.img.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.img.Location = New System.Drawing.Point(3, 4)
        Me.img.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.img.Name = "img"
        Me.img.Size = New System.Drawing.Size(174, 220)
        Me.img.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.img.TabIndex = 24
        Me.img.TabStop = False
        '
        'ofd
        '
        Me.ofd.FileName = "OpenFileDialog1"
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackColor = System.Drawing.Color.Transparent
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnClose.ForeColor = System.Drawing.Color.Black
        Me.btnClose.Location = New System.Drawing.Point(607, 276)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(80, 30)
        Me.btnClose.TabIndex = 8
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tt.SetToolTip(Me.btnClose, "Shortcut : Alt + O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(513, 276)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(80, 30)
        Me.btnSave.TabIndex = 9
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tt.SetToolTip(Me.btnSave, "Shortcut : Alt + S")
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'lblHelp
        '
        Me.lblHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblHelp.AutoSize = True
        Me.lblHelp.ForeColor = System.Drawing.Color.Blue
        Me.lblHelp.Location = New System.Drawing.Point(5, 276)
        Me.lblHelp.Name = "lblHelp"
        Me.lblHelp.Size = New System.Drawing.Size(198, 13)
        Me.lblHelp.TabIndex = 10
        Me.lblHelp.Text = "Press Ctrl + D to delete the file selected.."
        '
        'btnAddImg
        '
        CBlendItems1.iColor = New System.Drawing.Color() {System.Drawing.Color.AliceBlue, System.Drawing.Color.FromArgb(CType(CType(65, Byte), Integer), CType(CType(105, Byte), Integer), CType(CType(225, Byte), Integer)), System.Drawing.Color.FromArgb(CType(CType(65, Byte), Integer), CType(CType(105, Byte), Integer), CType(CType(225, Byte), Integer)), System.Drawing.Color.Navy}
        CBlendItems1.iPoint = New Single() {0!, 0.00308642!, 0.9969136!, 1.0!}
        Me.btnAddImg.ColorFillBlend = CBlendItems1
        Me.btnAddImg.DesignerSelected = False
        Me.btnAddImg.Font = New System.Drawing.Font("Arial", 15.0!, System.Drawing.FontStyle.Bold)
        Me.btnAddImg.ImageIndex = 0
        Me.btnAddImg.Location = New System.Drawing.Point(1, 1)
        Me.btnAddImg.Name = "btnAddImg"
        Me.btnAddImg.Size = New System.Drawing.Size(34, 30)
        Me.btnAddImg.TabIndex = 481
        Me.btnAddImg.Text = "+"
        '
        'btnFileAttach
        '
        Me.btnFileAttach.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        CBlendItems2.iColor = New System.Drawing.Color() {System.Drawing.Color.AliceBlue, System.Drawing.Color.FromArgb(CType(CType(65, Byte), Integer), CType(CType(105, Byte), Integer), CType(CType(225, Byte), Integer)), System.Drawing.Color.FromArgb(CType(CType(65, Byte), Integer), CType(CType(105, Byte), Integer), CType(CType(225, Byte), Integer)), System.Drawing.Color.Navy}
        CBlendItems2.iPoint = New Single() {0!, 0.00308642!, 0.9969136!, 1.0!}
        Me.btnFileAttach.ColorFillBlend = CBlendItems2
        Me.btnFileAttach.DesignerSelected = False
        Me.btnFileAttach.Image = Global.vLQMS.My.Resources.Resources.attach_20
        Me.btnFileAttach.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnFileAttach.ImageIndex = 0
        Me.btnFileAttach.Imagelist = Nothing
        Me.btnFileAttach.Location = New System.Drawing.Point(0, 0)
        Me.btnFileAttach.Name = "btnFileAttach"
        Me.btnFileAttach.ShowFocus = vLQMS.CButton.eFocus.None
        Me.btnFileAttach.Size = New System.Drawing.Size(705, 33)
        Me.btnFileAttach.TabIndex = 480
        Me.btnFileAttach.Text = "File Attachment "
        '
        'frmUtlFileControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(704, 320)
        Me.Controls.Add(Me.btnAddImg)
        Me.Controls.Add(Me.lblHelp)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.btnFileAttach)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmUtlFileControl"
        Me.ShowIcon = False
        Me.Text = "File Attachment"
        Me.Panel1.ResumeLayout(False)
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        CType(Me.img, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents lblPreview As Label
    Friend WithEvents img As C1.Win.C1Input.C1PictureBox
    Public WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents ofd As OpenFileDialog
    Friend WithEvents btnClose As Button
    Friend WithEvents btnSave As C1.Win.C1Input.C1Button
    Friend WithEvents lblHelp As Label
    Friend WithEvents tt As ToolTip
    Friend WithEvents btnFileAttach As CButton
    Friend WithEvents btnAddImg As CButton
End Class
