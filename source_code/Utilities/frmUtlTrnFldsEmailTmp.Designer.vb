<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlTrnFldsEmailTmp
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlTrnFldsEmailTmp))
        Me.lblTemplate = New System.Windows.Forms.Label()
        Me.vsfFlds = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.cboTemplate = New System.Windows.Forms.ComboBox()
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.btnSave = New C1.Win.C1Input.C1Button()
        Me.btnClear = New C1.Win.C1Input.C1Button()
        Me.btnDelTemplate = New System.Windows.Forms.Button()
        Me.tt = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnNewTem = New System.Windows.Forms.Button()
        Me.vsfTbls = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.chkAttachPrintOut = New System.Windows.Forms.CheckBox()
        Me.cboPrint = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.optQry = New System.Windows.Forms.RadioButton()
        Me.optStdTbls = New System.Windows.Forms.RadioButton()
        Me.txtQuery = New System.Windows.Forms.TextBox()
        Me.txtBody = New vLQMS.Watermark()
        Me.txtSub = New vLQMS.Watermark()
        Me.txtCC = New vLQMS.Watermark()
        Me.txtTo = New vLQMS.Watermark()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.optOutlook = New System.Windows.Forms.RadioButton()
        Me.optSMTP = New System.Windows.Forms.RadioButton()
        CType(Me.vsfFlds, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfTbls, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblTemplate
        '
        Me.lblTemplate.AutoSize = True
        Me.lblTemplate.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblTemplate.Location = New System.Drawing.Point(12, 20)
        Me.lblTemplate.Name = "lblTemplate"
        Me.lblTemplate.Size = New System.Drawing.Size(101, 17)
        Me.lblTemplate.TabIndex = 0
        Me.lblTemplate.Text = "Template Name"
        '
        'vsfFlds
        '
        Me.vsfFlds.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
        Me.vsfFlds.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfFlds.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfFlds.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfFlds.ColumnInfo = "3,0,0,0,0,120,Columns:"
        Me.vsfFlds.ExtendLastCol = True
        Me.vsfFlds.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfFlds.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfFlds.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfFlds.Location = New System.Drawing.Point(342, 48)
        Me.vsfFlds.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfFlds.Name = "vsfFlds"
        Me.vsfFlds.Rows.DefaultSize = 24
        Me.vsfFlds.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfFlds.ShowCellLabels = True
        Me.vsfFlds.Size = New System.Drawing.Size(343, 473)
        Me.vsfFlds.StyleInfo = resources.GetString("vsfFlds.StyleInfo")
        Me.vsfFlds.TabIndex = 3
        Me.vsfFlds.Tag = ""
        '
        'lblInfo
        '
        Me.lblInfo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblInfo.AutoSize = True
        Me.lblInfo.ForeColor = System.Drawing.Color.Blue
        Me.lblInfo.Location = New System.Drawing.Point(207, 545)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(203, 13)
        Me.lblInfo.TabIndex = 15
        Me.lblInfo.Text = "Double Click on the Fields, to be selected"
        '
        'cboTemplate
        '
        Me.cboTemplate.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.cboTemplate.FormattingEnabled = True
        Me.cboTemplate.Location = New System.Drawing.Point(121, 16)
        Me.cboTemplate.Name = "cboTemplate"
        Me.cboTemplate.Size = New System.Drawing.Size(128, 25)
        Me.cboTemplate.TabIndex = 0
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(607, 529)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(78, 29)
        Me.btnClose.TabIndex = 12
        Me.btnClose.Text = "C&lose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tt.SetToolTip(Me.btnClose, "Close: Alt+L")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(521, 529)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(78, 29)
        Me.btnSave.TabIndex = 11
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tt.SetToolTip(Me.btnSave, "Save: Alt + S")
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnClear
        '
        Me.btnClear.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnClear.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClear.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClear.Location = New System.Drawing.Point(15, 530)
        Me.btnClear.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(100, 27)
        Me.btnClear.TabIndex = 10
        Me.btnClear.Text = "Clear text"
        Me.btnClear.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClear.UseVisualStyleBackColor = False
        '
        'btnDelTemplate
        '
        Me.btnDelTemplate.BackgroundImage = CType(resources.GetObject("btnDelTemplate.BackgroundImage"), System.Drawing.Image)
        Me.btnDelTemplate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnDelTemplate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDelTemplate.Location = New System.Drawing.Point(297, 16)
        Me.btnDelTemplate.Name = "btnDelTemplate"
        Me.btnDelTemplate.Size = New System.Drawing.Size(36, 24)
        Me.btnDelTemplate.TabIndex = 2
        Me.btnDelTemplate.Tag = ""
        Me.tt.SetToolTip(Me.btnDelTemplate, "To delete the selected Template")
        Me.btnDelTemplate.UseVisualStyleBackColor = True
        '
        'btnNewTem
        '
        Me.btnNewTem.BackgroundImage = Global.vLQMS.My.Resources.Resources.D_NEW
        Me.btnNewTem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnNewTem.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNewTem.Location = New System.Drawing.Point(255, 16)
        Me.btnNewTem.Name = "btnNewTem"
        Me.btnNewTem.Size = New System.Drawing.Size(36, 24)
        Me.btnNewTem.TabIndex = 1
        Me.tt.SetToolTip(Me.btnNewTem, "New Template ")
        Me.btnNewTem.UseVisualStyleBackColor = True
        '
        'vsfTbls
        '
        Me.vsfTbls.AllowEditing = False
        Me.vsfTbls.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
        Me.vsfTbls.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfTbls.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfTbls.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfTbls.ColumnInfo = "1,0,0,0,0,95,Columns:"
        Me.vsfTbls.ExtendLastCol = True
        Me.vsfTbls.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Solid
        Me.vsfTbls.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.vsfTbls.Location = New System.Drawing.Point(436, 159)
        Me.vsfTbls.Name = "vsfTbls"
        Me.vsfTbls.Rows.Count = 4
        Me.vsfTbls.Rows.DefaultSize = 19
        Me.vsfTbls.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfTbls.Size = New System.Drawing.Size(147, 257)
        Me.vsfTbls.StyleInfo = resources.GetString("vsfTbls.StyleInfo")
        Me.vsfTbls.TabIndex = 207
        Me.vsfTbls.Tag = "DSGR"
        Me.vsfTbls.Visible = False
        '
        'chkAttachPrintOut
        '
        Me.chkAttachPrintOut.AutoSize = True
        Me.chkAttachPrintOut.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.chkAttachPrintOut.Location = New System.Drawing.Point(12, 69)
        Me.chkAttachPrintOut.Name = "chkAttachPrintOut"
        Me.chkAttachPrintOut.Size = New System.Drawing.Size(114, 21)
        Me.chkAttachPrintOut.TabIndex = 4
        Me.chkAttachPrintOut.Text = "Attach PrintOut"
        Me.chkAttachPrintOut.UseVisualStyleBackColor = True
        '
        'cboPrint
        '
        Me.cboPrint.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.cboPrint.FormattingEnabled = True
        Me.cboPrint.Location = New System.Drawing.Point(12, 113)
        Me.cboPrint.Name = "cboPrint"
        Me.cboPrint.Size = New System.Drawing.Size(321, 25)
        Me.cboPrint.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label1.Location = New System.Drawing.Point(9, 93)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(73, 17)
        Me.Label1.TabIndex = 216
        Me.Label1.Text = "Print Name"
        '
        'optQry
        '
        Me.optQry.AutoSize = True
        Me.optQry.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.optQry.Location = New System.Drawing.Point(94, 16)
        Me.optQry.Name = "optQry"
        Me.optQry.Size = New System.Drawing.Size(88, 21)
        Me.optQry.TabIndex = 217
        Me.optQry.Text = "SQL Query"
        Me.optQry.UseVisualStyleBackColor = True
        '
        'optStdTbls
        '
        Me.optStdTbls.AutoSize = True
        Me.optStdTbls.Checked = True
        Me.optStdTbls.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.optStdTbls.Location = New System.Drawing.Point(192, 16)
        Me.optStdTbls.Name = "optStdTbls"
        Me.optStdTbls.Size = New System.Drawing.Size(118, 21)
        Me.optStdTbls.TabIndex = 218
        Me.optStdTbls.TabStop = True
        Me.optStdTbls.Text = "Standard tables"
        Me.optStdTbls.UseVisualStyleBackColor = True
        '
        'txtQuery
        '
        Me.txtQuery.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtQuery.Location = New System.Drawing.Point(342, 49)
        Me.txtQuery.Multiline = True
        Me.txtQuery.Name = "txtQuery"
        Me.txtQuery.Size = New System.Drawing.Size(343, 472)
        Me.txtQuery.TabIndex = 219
        Me.txtQuery.Visible = False
        '
        'txtBody
        '
        Me.txtBody.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtBody.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtBody.Location = New System.Drawing.Point(12, 364)
        Me.txtBody.Multiline = True
        Me.txtBody.Name = "txtBody"
        Me.txtBody.Size = New System.Drawing.Size(321, 157)
        Me.txtBody.TabIndex = 9
        Me.txtBody.WatermarkColor = System.Drawing.Color.Gray
        Me.txtBody.WatermarkText = "Body template"
        '
        'txtSub
        '
        Me.txtSub.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtSub.Location = New System.Drawing.Point(12, 261)
        Me.txtSub.Multiline = True
        Me.txtSub.Name = "txtSub"
        Me.txtSub.Size = New System.Drawing.Size(321, 98)
        Me.txtSub.TabIndex = 8
        Me.txtSub.WatermarkColor = System.Drawing.Color.Gray
        Me.txtSub.WatermarkText = "Subject template"
        '
        'txtCC
        '
        Me.txtCC.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtCC.Location = New System.Drawing.Point(12, 203)
        Me.txtCC.Multiline = True
        Me.txtCC.Name = "txtCC"
        Me.txtCC.Size = New System.Drawing.Size(321, 53)
        Me.txtCC.TabIndex = 7
        Me.txtCC.WatermarkColor = System.Drawing.Color.Gray
        Me.txtCC.WatermarkText = "CC template"
        '
        'txtTo
        '
        Me.txtTo.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtTo.Location = New System.Drawing.Point(12, 144)
        Me.txtTo.Multiline = True
        Me.txtTo.Name = "txtTo"
        Me.txtTo.Size = New System.Drawing.Size(321, 53)
        Me.txtTo.TabIndex = 6
        Me.txtTo.WatermarkColor = System.Drawing.Color.Gray
        Me.txtTo.WatermarkText = "To template"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.optStdTbls)
        Me.GroupBox1.Controls.Add(Me.optQry)
        Me.GroupBox1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(342, 1)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(343, 42)
        Me.GroupBox1.TabIndex = 220
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Field Selection"
        '
        'optOutlook
        '
        Me.optOutlook.AutoSize = True
        Me.optOutlook.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optOutlook.Location = New System.Drawing.Point(210, 60)
        Me.optOutlook.Name = "optOutlook"
        Me.optOutlook.Size = New System.Drawing.Size(72, 21)
        Me.optOutlook.TabIndex = 221
        Me.optOutlook.Text = "Outlook"
        Me.optOutlook.UseVisualStyleBackColor = True
        '
        'optSMTP
        '
        Me.optSMTP.AutoSize = True
        Me.optSMTP.Checked = True
        Me.optSMTP.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optSMTP.Location = New System.Drawing.Point(210, 83)
        Me.optSMTP.Name = "optSMTP"
        Me.optSMTP.Size = New System.Drawing.Size(59, 21)
        Me.optSMTP.TabIndex = 222
        Me.optSMTP.TabStop = True
        Me.optSMTP.Text = "SMTP"
        Me.optSMTP.UseVisualStyleBackColor = True
        '
        'frmUtlTrnFldsEmailTmp
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(697, 570)
        Me.Controls.Add(Me.optSMTP)
        Me.Controls.Add(Me.optOutlook)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.cboPrint)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.chkAttachPrintOut)
        Me.Controls.Add(Me.txtBody)
        Me.Controls.Add(Me.txtSub)
        Me.Controls.Add(Me.txtCC)
        Me.Controls.Add(Me.txtTo)
        Me.Controls.Add(Me.vsfTbls)
        Me.Controls.Add(Me.btnNewTem)
        Me.Controls.Add(Me.btnDelTemplate)
        Me.Controls.Add(Me.cboTemplate)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.lblInfo)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.vsfFlds)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.lblTemplate)
        Me.Controls.Add(Me.txtQuery)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "frmUtlTrnFldsEmailTmp"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        CType(Me.vsfFlds, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfTbls, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblTemplate As Label
    Friend WithEvents btnClear As C1.Win.C1Input.C1Button
    Friend WithEvents vsfFlds As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnSave As C1.Win.C1Input.C1Button
    Friend WithEvents lblInfo As Label
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents cboTemplate As ComboBox
    Friend WithEvents btnDelTemplate As Button
    Friend WithEvents tt As ToolTip
    Friend WithEvents btnNewTem As Button
    Public WithEvents vsfTbls As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents txtBody As Watermark
    Friend WithEvents txtSub As Watermark
    Friend WithEvents txtCC As Watermark
    Friend WithEvents txtTo As Watermark
    Friend WithEvents chkAttachPrintOut As CheckBox
    Friend WithEvents cboPrint As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents optQry As RadioButton
    Friend WithEvents optStdTbls As RadioButton
    Friend WithEvents txtQuery As TextBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents optOutlook As RadioButton
    Friend WithEvents optSMTP As RadioButton
End Class
