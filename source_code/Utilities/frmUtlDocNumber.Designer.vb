<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlDocNumber
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlDocNumber))
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.btnSave = New C1.Win.C1Input.C1Button()
        Me.fraMain = New System.Windows.Forms.Panel()
        Me.fraPrefix = New System.Windows.Forms.Panel()
        Me.vsfPfx = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnPRemove = New C1.Win.C1Input.C1Button()
        Me.btnPClose = New C1.Win.C1Input.C1Button()
        Me.btnPSave = New C1.Win.C1Input.C1Button()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfEx = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnPrefixSettings = New C1.Win.C1Input.C1Button()
        Me.btnResetNoFmt = New C1.Win.C1Input.C1Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.lblInformation = New System.Windows.Forms.Label()
        Me.fraMain.SuspendLayout()
        Me.fraPrefix.SuspendLayout()
        CType(Me.vsfPfx, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfEx, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(1084, 608)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(76, 29)
        Me.btnClose.TabIndex = 10
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close : Alt + O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(1000, 608)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(76, 29)
        Me.btnSave.TabIndex = 9
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Save : Alt + S")
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'fraMain
        '
        Me.fraMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fraMain.Controls.Add(Me.fraPrefix)
        Me.fraMain.Controls.Add(Me.vsfDet)
        Me.fraMain.Controls.Add(Me.vsfEx)
        Me.fraMain.Location = New System.Drawing.Point(12, 12)
        Me.fraMain.Name = "fraMain"
        Me.fraMain.Size = New System.Drawing.Size(1148, 589)
        Me.fraMain.TabIndex = 11
        '
        'fraPrefix
        '
        Me.fraPrefix.Controls.Add(Me.vsfPfx)
        Me.fraPrefix.Controls.Add(Me.btnPRemove)
        Me.fraPrefix.Controls.Add(Me.btnPClose)
        Me.fraPrefix.Controls.Add(Me.btnPSave)
        Me.fraPrefix.Location = New System.Drawing.Point(219, 169)
        Me.fraPrefix.Name = "fraPrefix"
        Me.fraPrefix.Size = New System.Drawing.Size(496, 323)
        Me.fraPrefix.TabIndex = 12
        Me.fraPrefix.Visible = False
        '
        'vsfPfx
        '
        Me.vsfPfx.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfPfx.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfPfx.BackColor = System.Drawing.Color.White
        Me.vsfPfx.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfPfx.ExtendLastCol = True
        Me.vsfPfx.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfPfx.ForeColor = System.Drawing.Color.Black
        Me.vsfPfx.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfPfx.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfPfx.Location = New System.Drawing.Point(17, 15)
        Me.vsfPfx.Name = "vsfPfx"
        Me.vsfPfx.Rows.Count = 4
        Me.vsfPfx.Rows.DefaultSize = 24
        Me.vsfPfx.Size = New System.Drawing.Size(463, 259)
        Me.vsfPfx.StyleInfo = resources.GetString("vsfPfx.StyleInfo")
        Me.vsfPfx.TabIndex = 13
        '
        'btnPRemove
        '
        Me.btnPRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnPRemove.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPRemove.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPRemove.Location = New System.Drawing.Point(17, 281)
        Me.btnPRemove.Margin = New System.Windows.Forms.Padding(4)
        Me.btnPRemove.Name = "btnPRemove"
        Me.btnPRemove.Size = New System.Drawing.Size(79, 29)
        Me.btnPRemove.TabIndex = 13
        Me.btnPRemove.Text = "&Remove"
        Me.btnPRemove.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnPRemove, "Remove : Alt + R")
        Me.btnPRemove.UseVisualStyleBackColor = False
        '
        'btnPClose
        '
        Me.btnPClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnPClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPClose.Location = New System.Drawing.Point(404, 281)
        Me.btnPClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnPClose.Name = "btnPClose"
        Me.btnPClose.Size = New System.Drawing.Size(76, 29)
        Me.btnPClose.TabIndex = 12
        Me.btnPClose.Text = "C&lose"
        Me.btnPClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnPClose, "Close : Alt + L")
        Me.btnPClose.UseVisualStyleBackColor = False
        '
        'btnPSave
        '
        Me.btnPSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnPSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPSave.Location = New System.Drawing.Point(320, 281)
        Me.btnPSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnPSave.Name = "btnPSave"
        Me.btnPSave.Size = New System.Drawing.Size(76, 29)
        Me.btnPSave.TabIndex = 11
        Me.btnPSave.Text = "&Save"
        Me.btnPSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnPSave, "Save : Alt  + S")
        Me.btnPSave.UseVisualStyleBackColor = False
        '
        'vsfDet
        '
        Me.vsfDet.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.BackColor = System.Drawing.Color.White
        Me.vsfDet.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.ForeColor = System.Drawing.Color.Black
        Me.vsfDet.Location = New System.Drawing.Point(3, 119)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.Count = 4
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.Size = New System.Drawing.Size(1142, 467)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 2
        '
        'vsfEx
        '
        Me.vsfEx.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfEx.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfEx.BackColor = System.Drawing.Color.White
        Me.vsfEx.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfEx.ExtendLastCol = True
        Me.vsfEx.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfEx.ForeColor = System.Drawing.Color.Black
        Me.vsfEx.Location = New System.Drawing.Point(3, 5)
        Me.vsfEx.Name = "vsfEx"
        Me.vsfEx.Rows.Count = 4
        Me.vsfEx.Rows.DefaultSize = 24
        Me.vsfEx.Size = New System.Drawing.Size(1142, 138)
        Me.vsfEx.StyleInfo = resources.GetString("vsfEx.StyleInfo")
        Me.vsfEx.TabIndex = 1
        '
        'btnPrefixSettings
        '
        Me.btnPrefixSettings.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPrefixSettings.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPrefixSettings.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPrefixSettings.Location = New System.Drawing.Point(859, 608)
        Me.btnPrefixSettings.Margin = New System.Windows.Forms.Padding(4)
        Me.btnPrefixSettings.Name = "btnPrefixSettings"
        Me.btnPrefixSettings.Size = New System.Drawing.Size(133, 29)
        Me.btnPrefixSettings.TabIndex = 12
        Me.btnPrefixSettings.Text = "&Prefix Settings"
        Me.btnPrefixSettings.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnPrefixSettings, "Prefix Settings : Alt + P")
        Me.btnPrefixSettings.UseVisualStyleBackColor = False
        '
        'btnResetNoFmt
        '
        Me.btnResetNoFmt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnResetNoFmt.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnResetNoFmt.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnResetNoFmt.Location = New System.Drawing.Point(12, 608)
        Me.btnResetNoFmt.Margin = New System.Windows.Forms.Padding(4)
        Me.btnResetNoFmt.Name = "btnResetNoFmt"
        Me.btnResetNoFmt.Size = New System.Drawing.Size(299, 29)
        Me.btnResetNoFmt.TabIndex = 13
        Me.btnResetNoFmt.Text = "&Reset Numbers[Format] of the selected document"
        Me.btnResetNoFmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnResetNoFmt, "Reset : Alt + R")
        Me.btnResetNoFmt.UseVisualStyleBackColor = False
        '
        'lblInformation
        '
        Me.lblInformation.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblInformation.AutoSize = True
        Me.lblInformation.Location = New System.Drawing.Point(318, 604)
        Me.lblInformation.Name = "lblInformation"
        Me.lblInformation.Size = New System.Drawing.Size(537, 39)
        Me.lblInformation.TabIndex = 14
        Me.lblInformation.Text = resources.GetString("lblInformation.Text")
        '
        'frmUtlDocNumber
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1172, 650)
        Me.Controls.Add(Me.lblInformation)
        Me.Controls.Add(Me.btnResetNoFmt)
        Me.Controls.Add(Me.btnPrefixSettings)
        Me.Controls.Add(Me.fraMain)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnSave)
        Me.Name = "frmUtlDocNumber"
        Me.ShowIcon = False
        Me.Text = "Document Number"
        Me.fraMain.ResumeLayout(False)
        Me.fraPrefix.ResumeLayout(False)
        CType(Me.vsfPfx, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfEx, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents btnSave As C1.Win.C1Input.C1Button
    Friend WithEvents fraMain As System.Windows.Forms.Panel
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents vsfEx As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents fraPrefix As System.Windows.Forms.Panel
    Friend WithEvents vsfPfx As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnPRemove As C1.Win.C1Input.C1Button
    Friend WithEvents btnPClose As C1.Win.C1Input.C1Button
    Friend WithEvents btnPSave As C1.Win.C1Input.C1Button
    Friend WithEvents btnPrefixSettings As C1.Win.C1Input.C1Button
    Friend WithEvents btnResetNoFmt As C1.Win.C1Input.C1Button
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents lblInformation As Label
End Class
