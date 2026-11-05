<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlEnv
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlEnv))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CboTemplate = New System.Windows.Forms.ComboBox()
        Me.btnDesign = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtEnvID = New System.Windows.Forms.TextBox()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.pnlGrid = New System.Windows.Forms.Panel()
        Me.ucBtnCntrl = New vLQMS.ucBtn()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.pnlGrid.SuspendLayout()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.Label1.ForeColor = System.Drawing.Color.Red
        Me.Label1.Location = New System.Drawing.Point(13, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(96, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Select Template"
        '
        'CboTemplate
        '
        Me.CboTemplate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboTemplate.FormattingEnabled = True
        Me.CboTemplate.Location = New System.Drawing.Point(127, 11)
        Me.CboTemplate.Name = "CboTemplate"
        Me.CboTemplate.Size = New System.Drawing.Size(182, 23)
        Me.CboTemplate.TabIndex = 1
        '
        'btnDesign
        '
        Me.btnDesign.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnDesign.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDesign.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.btnDesign.Location = New System.Drawing.Point(94, 3)
        Me.btnDesign.Name = "btnDesign"
        Me.btnDesign.Size = New System.Drawing.Size(80, 31)
        Me.btnDesign.TabIndex = 2
        Me.btnDesign.Text = "&Design"
        Me.ToolTip1.SetToolTip(Me.btnDesign, "Shortcut : Alt + D")
        Me.btnDesign.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPrint.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.btnPrint.Image = Global.vLQMS.My.Resources.Resources.T3_PRINT
        Me.btnPrint.Location = New System.Drawing.Point(191, 3)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(90, 31)
        Me.btnPrint.TabIndex = 3
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnPrint, "Shortcut : Alt + P")
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.btnClose.Image = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.Location = New System.Drawing.Point(294, 3)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(91, 31)
        Me.btnClose.TabIndex = 4
        Me.btnClose.Text = "&Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Shortcut : Alt + C")
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'txtEnvID
        '
        Me.txtEnvID.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.txtEnvID.ForeColor = System.Drawing.SystemColors.Control
        Me.txtEnvID.Location = New System.Drawing.Point(444, 8)
        Me.txtEnvID.Name = "txtEnvID"
        Me.txtEnvID.Size = New System.Drawing.Size(56, 21)
        Me.txtEnvID.TabIndex = 12
        Me.txtEnvID.Visible = False
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Controls.Add(Me.Panel1)
        Me.FlowLayoutPanel1.Controls.Add(Me.pnlGrid)
        Me.FlowLayoutPanel1.Controls.Add(Me.Panel3)
        Me.FlowLayoutPanel1.Controls.Add(Me.Label2)
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(2, 6)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(540, 423)
        Me.FlowLayoutPanel1.TabIndex = 13
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnLoad)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.txtEnvID)
        Me.Panel1.Controls.Add(Me.CboTemplate)
        Me.Panel1.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Panel1.Location = New System.Drawing.Point(3, 0)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(533, 41)
        Me.Panel1.TabIndex = 0
        '
        'btnLoad
        '
        Me.btnLoad.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoad.Location = New System.Drawing.Point(315, 10)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(123, 25)
        Me.btnLoad.TabIndex = 13
        Me.btnLoad.Text = "Load"
        Me.btnLoad.UseVisualStyleBackColor = True
        '
        'pnlGrid
        '
        Me.pnlGrid.Controls.Add(Me.ucBtnCntrl)
        Me.pnlGrid.Controls.Add(Me.vsfDet)
        Me.pnlGrid.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.pnlGrid.Location = New System.Drawing.Point(3, 41)
        Me.pnlGrid.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.pnlGrid.Name = "pnlGrid"
        Me.pnlGrid.Size = New System.Drawing.Size(533, 303)
        Me.pnlGrid.TabIndex = 1
        Me.pnlGrid.Visible = False
        '
        'ucBtnCntrl
        '
        Me.ucBtnCntrl.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ucBtnCntrl.Location = New System.Drawing.Point(3, 272)
        Me.ucBtnCntrl.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.ucBtnCntrl.Name = "ucBtnCntrl"
        Me.ucBtnCntrl.Size = New System.Drawing.Size(467, 22)
        Me.ucBtnCntrl.TabIndex = 5
        '
        'vsfDet
        '
        Me.vsfDet.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "2,0,0,0,0,110,Columns:"
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.vsfDet.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(3, 4)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 22
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(526, 260)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 4
        Me.vsfDet.Tag = ""
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.btnDesign)
        Me.Panel3.Controls.Add(Me.btnClose)
        Me.Panel3.Controls.Add(Me.btnPrint)
        Me.Panel3.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Panel3.Location = New System.Drawing.Point(3, 344)
        Me.Panel3.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(533, 39)
        Me.Panel3.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.Label2.ForeColor = System.Drawing.Color.Blue
        Me.Label2.Location = New System.Drawing.Point(3, 383)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(295, 15)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Click and drag values from grid to move up and down"
        Me.Label2.Visible = False
        '
        'frmUtlEnv
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(546, 429)
        Me.Controls.Add(Me.FlowLayoutPanel1)
        Me.KeyPreview = True
        Me.Name = "frmUtlEnv"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Envelope Printing"
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.pnlGrid.ResumeLayout(False)
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents CboTemplate As ComboBox
    Friend WithEvents btnDesign As Button
    Friend WithEvents btnPrint As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents txtEnvID As TextBox
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents btnLoad As Button
    Friend WithEvents pnlGrid As Panel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents Label2 As Label
    Public WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents ucBtnCntrl As ucBtn
End Class
