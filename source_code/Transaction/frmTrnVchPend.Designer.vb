<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTrnVchPend
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTrnVchPend))
        Me.txtRemAmt = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnClearAdj = New System.Windows.Forms.Button()
        Me.btnAutoAdj = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.lblParty = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtTOAdjust = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.optOnAccount = New System.Windows.Forms.RadioButton()
        Me.optNewRef = New System.Windows.Forms.RadioButton()
        Me.txtNewRef = New System.Windows.Forms.TextBox()
        Me.lblBalance = New System.Windows.Forms.Label()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtRemAmt
        '
        Me.txtRemAmt.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtRemAmt.BackColor = System.Drawing.Color.White
        Me.txtRemAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtRemAmt.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRemAmt.Location = New System.Drawing.Point(543, 22)
        Me.txtRemAmt.Name = "txtRemAmt"
        Me.txtRemAmt.ReadOnly = True
        Me.txtRemAmt.Size = New System.Drawing.Size(226, 29)
        Me.txtRemAmt.TabIndex = 0
        Me.txtRemAmt.TabStop = False
        Me.txtRemAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(608, 5)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(96, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Remaining Amount"
        '
        'btnClearAdj
        '
        Me.btnClearAdj.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClearAdj.BackColor = System.Drawing.Color.WhiteSmoke
        Me.btnClearAdj.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClearAdj.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClearAdj.Location = New System.Drawing.Point(452, 411)
        Me.btnClearAdj.Name = "btnClearAdj"
        Me.btnClearAdj.Size = New System.Drawing.Size(122, 28)
        Me.btnClearAdj.TabIndex = 2
        Me.btnClearAdj.Text = "Clea&r Adjustment"
        Me.ToolTip1.SetToolTip(Me.btnClearAdj, "Clear Adjustment:Alt:R")
        Me.btnClearAdj.UseVisualStyleBackColor = False
        '
        'btnAutoAdj
        '
        Me.btnAutoAdj.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAutoAdj.BackColor = System.Drawing.Color.WhiteSmoke
        Me.btnAutoAdj.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAutoAdj.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAutoAdj.Location = New System.Drawing.Point(183, 24)
        Me.btnAutoAdj.Name = "btnAutoAdj"
        Me.btnAutoAdj.Size = New System.Drawing.Size(122, 27)
        Me.btnAutoAdj.TabIndex = 3
        Me.btnAutoAdj.Text = "&Auto Adjustment"
        Me.ToolTip1.SetToolTip(Me.btnAutoAdj, "Auto Adjustment:Alt+A")
        Me.btnAutoAdj.UseVisualStyleBackColor = False
        Me.btnAutoAdj.Visible = False
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.BackColor = System.Drawing.Color.WhiteSmoke
        Me.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOK.Location = New System.Drawing.Point(678, 409)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(90, 29)
        Me.btnOK.TabIndex = 4
        Me.btnOK.Text = "O&K"
        Me.ToolTip1.SetToolTip(Me.btnOK, "OK:Alt+K")
        Me.btnOK.UseVisualStyleBackColor = False
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
        Me.vsfDet.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus
        Me.vsfDet.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(12, 60)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(757, 331)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 5
        Me.vsfDet.Tag = ""
        '
        'lblParty
        '
        Me.lblParty.AutoSize = True
        Me.lblParty.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblParty.ForeColor = System.Drawing.Color.Blue
        Me.lblParty.Location = New System.Drawing.Point(11, 13)
        Me.lblParty.Name = "lblParty"
        Me.lblParty.Size = New System.Drawing.Size(47, 18)
        Me.lblParty.TabIndex = 7
        Me.lblParty.Text = "Party"
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(389, 4)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(70, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Party Amount"
        '
        'txtTOAdjust
        '
        Me.txtTOAdjust.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTOAdjust.BackColor = System.Drawing.Color.White
        Me.txtTOAdjust.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTOAdjust.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTOAdjust.Location = New System.Drawing.Point(311, 22)
        Me.txtTOAdjust.Name = "txtTOAdjust"
        Me.txtTOAdjust.ReadOnly = True
        Me.txtTOAdjust.Size = New System.Drawing.Size(226, 29)
        Me.txtTOAdjust.TabIndex = 8
        Me.txtTOAdjust.TabStop = False
        Me.txtTOAdjust.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(22, 397)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(184, 13)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "Un-Adjusted Amount to be treated as "
        '
        'optOnAccount
        '
        Me.optOnAccount.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.optOnAccount.AutoSize = True
        Me.optOnAccount.Location = New System.Drawing.Point(22, 421)
        Me.optOnAccount.Name = "optOnAccount"
        Me.optOnAccount.Size = New System.Drawing.Size(82, 17)
        Me.optOnAccount.TabIndex = 11
        Me.optOnAccount.TabStop = True
        Me.optOnAccount.Text = "On-Account"
        Me.optOnAccount.UseVisualStyleBackColor = True
        '
        'optNewRef
        '
        Me.optNewRef.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.optNewRef.AutoSize = True
        Me.optNewRef.Location = New System.Drawing.Point(112, 420)
        Me.optNewRef.Name = "optNewRef"
        Me.optNewRef.Size = New System.Drawing.Size(100, 17)
        Me.optNewRef.TabIndex = 12
        Me.optNewRef.TabStop = True
        Me.optNewRef.Text = "New-Reference"
        Me.optNewRef.UseVisualStyleBackColor = True
        '
        'txtNewRef
        '
        Me.txtNewRef.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtNewRef.BackColor = System.Drawing.Color.White
        Me.txtNewRef.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtNewRef.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNewRef.Location = New System.Drawing.Point(212, 413)
        Me.txtNewRef.MaxLength = 250
        Me.txtNewRef.Name = "txtNewRef"
        Me.txtNewRef.Size = New System.Drawing.Size(144, 29)
        Me.txtNewRef.TabIndex = 13
        Me.txtNewRef.TabStop = False
        Me.txtNewRef.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblBalance
        '
        Me.lblBalance.AutoSize = True
        Me.lblBalance.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBalance.ForeColor = System.Drawing.Color.Blue
        Me.lblBalance.Location = New System.Drawing.Point(12, 35)
        Me.lblBalance.Name = "lblBalance"
        Me.lblBalance.Size = New System.Drawing.Size(102, 18)
        Me.lblBalance.TabIndex = 14
        Me.lblBalance.Text = "Balance Amt"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Image = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnCancel.Location = New System.Drawing.Point(581, 410)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(90, 29)
        Me.btnCancel.TabIndex = 15
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Cancel:Alt+C")
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'frmTrnVchPend
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(777, 448)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.lblBalance)
        Me.Controls.Add(Me.txtNewRef)
        Me.Controls.Add(Me.optNewRef)
        Me.Controls.Add(Me.optOnAccount)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtTOAdjust)
        Me.Controls.Add(Me.lblParty)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnAutoAdj)
        Me.Controls.Add(Me.btnClearAdj)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtRemAmt)
        Me.Name = "frmTrnVchPend"
        Me.ShowIcon = False
        Me.Text = "Bill wise Adjustment"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtRemAmt As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnClearAdj As System.Windows.Forms.Button
    Friend WithEvents btnAutoAdj As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents lblParty As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtTOAdjust As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents optOnAccount As System.Windows.Forms.RadioButton
    Friend WithEvents optNewRef As System.Windows.Forms.RadioButton
    Friend WithEvents txtNewRef As System.Windows.Forms.TextBox
    Friend WithEvents lblBalance As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As ToolTip
End Class
