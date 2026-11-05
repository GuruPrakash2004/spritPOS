<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlDeleteNonLiqItems
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlDeleteNonLiqItems))
        Me.txtItemGroup = New System.Windows.Forms.TextBox()
        Me.btnSelItemGroup = New System.Windows.Forms.Button()
        Me.lblPartyCode = New System.Windows.Forms.Label()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.chkItemGroup = New System.Windows.Forms.CheckBox()
        Me.btnCarryOver = New System.Windows.Forms.Button()
        Me.vsfGroup = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.lblCnt = New System.Windows.Forms.Label()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        CType(Me.vsfGroup, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtItemGroup
        '
        Me.txtItemGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtItemGroup.Location = New System.Drawing.Point(217, 28)
        Me.txtItemGroup.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtItemGroup.Name = "txtItemGroup"
        Me.txtItemGroup.Size = New System.Drawing.Size(355, 30)
        Me.txtItemGroup.TabIndex = 7
        '
        'btnSelItemGroup
        '
        Me.btnSelItemGroup.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSelItemGroup.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSelItemGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSelItemGroup.Location = New System.Drawing.Point(581, 28)
        Me.btnSelItemGroup.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnSelItemGroup.Name = "btnSelItemGroup"
        Me.btnSelItemGroup.Size = New System.Drawing.Size(39, 32)
        Me.btnSelItemGroup.TabIndex = 8
        Me.btnSelItemGroup.Text = "..."
        Me.btnSelItemGroup.UseVisualStyleBackColor = True
        '
        'lblPartyCode
        '
        Me.lblPartyCode.AutoSize = True
        Me.lblPartyCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPartyCode.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblPartyCode.Location = New System.Drawing.Point(29, 28)
        Me.lblPartyCode.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblPartyCode.Name = "lblPartyCode"
        Me.lblPartyCode.Size = New System.Drawing.Size(163, 25)
        Me.lblPartyCode.TabIndex = 10
        Me.lblPartyCode.Tag = "M"
        Me.lblPartyCode.Text = "Select ItemGroup"
        '
        'vsfDet
        '
        Me.vsfDet.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "2,1,0,0,0,120,Columns:"
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(20, 69)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 28
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(1103, 448)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 11
        Me.vsfDet.Tag = ""
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(8, 635)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(117, 37)
        Me.btnClose.TabIndex = 235
        Me.btnClose.Text = "&Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDelete.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.btnDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDelete.Location = New System.Drawing.Point(516, 544)
        Me.btnDelete.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(596, 39)
        Me.btnDelete.TabIndex = 522
        Me.btnDelete.Text = "Delete the above Listed items along with Stock and Transactions"
        Me.btnDelete.UseVisualStyleBackColor = False
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Location = New System.Drawing.Point(3, 4)
        Me.TabControl1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.TabControl1.Multiline = True
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1148, 623)
        Me.TabControl1.TabIndex = 523
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.lblCnt)
        Me.TabPage1.Controls.Add(Me.btnSelItemGroup)
        Me.TabPage1.Controls.Add(Me.btnDelete)
        Me.TabPage1.Controls.Add(Me.lblPartyCode)
        Me.TabPage1.Controls.Add(Me.txtItemGroup)
        Me.TabPage1.Controls.Add(Me.vsfDet)
        Me.TabPage1.Location = New System.Drawing.Point(4, 25)
        Me.TabPage1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.TabPage1.Size = New System.Drawing.Size(1140, 594)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Single Group - Delete All - Items/Trn/Stock"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.chkItemGroup)
        Me.TabPage2.Controls.Add(Me.btnCarryOver)
        Me.TabPage2.Controls.Add(Me.vsfGroup)
        Me.TabPage2.Location = New System.Drawing.Point(4, 25)
        Me.TabPage2.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.TabPage2.Size = New System.Drawing.Size(1140, 594)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Delete only Trns - Carry Op Bal"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'chkItemGroup
        '
        Me.chkItemGroup.AutoSize = True
        Me.chkItemGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkItemGroup.Location = New System.Drawing.Point(8, 23)
        Me.chkItemGroup.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.chkItemGroup.Name = "chkItemGroup"
        Me.chkItemGroup.Size = New System.Drawing.Size(147, 24)
        Me.chkItemGroup.TabIndex = 525
        Me.chkItemGroup.Text = "Selected Group"
        Me.chkItemGroup.UseVisualStyleBackColor = True
        '
        'btnCarryOver
        '
        Me.btnCarryOver.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCarryOver.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.btnCarryOver.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCarryOver.Location = New System.Drawing.Point(656, 526)
        Me.btnCarryOver.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnCarryOver.Name = "btnCarryOver"
        Me.btnCarryOver.Size = New System.Drawing.Size(456, 39)
        Me.btnCarryOver.TabIndex = 524
        Me.btnCarryOver.Text = "Delete the Transactions and Carry over Stock "
        Me.btnCarryOver.UseVisualStyleBackColor = False
        '
        'vsfGroup
        '
        Me.vsfGroup.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfGroup.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfGroup.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfGroup.ColumnInfo = "2,1,0,0,0,120,Columns:"
        Me.vsfGroup.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfGroup.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
        Me.vsfGroup.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfGroup.Location = New System.Drawing.Point(8, 57)
        Me.vsfGroup.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.vsfGroup.Name = "vsfGroup"
        Me.vsfGroup.Rows.DefaultSize = 28
        Me.vsfGroup.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfGroup.Size = New System.Drawing.Size(1103, 448)
        Me.vsfGroup.StyleInfo = resources.GetString("vsfGroup.StyleInfo")
        Me.vsfGroup.TabIndex = 523
        Me.vsfGroup.Tag = ""
        '
        'lblCnt
        '
        Me.lblCnt.AutoSize = True
        Me.lblCnt.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCnt.ForeColor = System.Drawing.Color.Black
        Me.lblCnt.Location = New System.Drawing.Point(16, 544)
        Me.lblCnt.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCnt.Name = "lblCnt"
        Me.lblCnt.Size = New System.Drawing.Size(19, 25)
        Me.lblCnt.TabIndex = 523
        Me.lblCnt.Tag = ""
        Me.lblCnt.Text = "-"
        '
        'frmUtlDeleteNonLiqItems
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1167, 688)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.btnClose)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "frmUtlDeleteNonLiqItems"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Delete Non-Liqour Items"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        CType(Me.vsfGroup, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents txtItemGroup As TextBox
    Friend WithEvents btnSelItemGroup As Button
    Friend WithEvents lblPartyCode As Label
    Public WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnClose As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents btnCarryOver As Button
    Public WithEvents vsfGroup As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents chkItemGroup As CheckBox
    Friend WithEvents lblCnt As Label
End Class
