<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlInvXLPrintQuery
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlInvXLPrintQuery))
        Me.btnRefreshQuery = New System.Windows.Forms.Button()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.txtQry = New System.Windows.Forms.TextBox()
        Me.btnVerifyQuery = New System.Windows.Forms.Button()
        Me.txtExcelFileName = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnRefreshQuery
        '
        Me.btnRefreshQuery.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRefreshQuery.ForeColor = System.Drawing.Color.Red
        Me.btnRefreshQuery.Location = New System.Drawing.Point(26, 327)
        Me.btnRefreshQuery.Name = "btnRefreshQuery"
        Me.btnRefreshQuery.Size = New System.Drawing.Size(134, 26)
        Me.btnRefreshQuery.TabIndex = 0
        Me.btnRefreshQuery.Text = "Refresh Query"
        Me.btnRefreshQuery.UseVisualStyleBackColor = True
        '
        'btnUpdate
        '
        Me.btnUpdate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnUpdate.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnUpdate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnUpdate.Location = New System.Drawing.Point(381, 326)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(90, 26)
        Me.btnUpdate.TabIndex = 1
        Me.btnUpdate.Text = "Save"
        Me.btnUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Location = New System.Drawing.Point(494, 326)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(90, 26)
        Me.btnClose.TabIndex = 2
        Me.btnClose.Text = "Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'txtQry
        '
        Me.txtQry.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtQry.Location = New System.Drawing.Point(24, 36)
        Me.txtQry.Multiline = True
        Me.txtQry.Name = "txtQry"
        Me.txtQry.Size = New System.Drawing.Size(584, 271)
        Me.txtQry.TabIndex = 3
        '
        'btnVerifyQuery
        '
        Me.btnVerifyQuery.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnVerifyQuery.Location = New System.Drawing.Point(198, 326)
        Me.btnVerifyQuery.Name = "btnVerifyQuery"
        Me.btnVerifyQuery.Size = New System.Drawing.Size(134, 26)
        Me.btnVerifyQuery.TabIndex = 4
        Me.btnVerifyQuery.Text = "Verify Query"
        Me.btnVerifyQuery.UseVisualStyleBackColor = True
        '
        'txtExcelFileName
        '
        Me.txtExcelFileName.Location = New System.Drawing.Point(123, 9)
        Me.txtExcelFileName.Name = "txtExcelFileName"
        Me.txtExcelFileName.Size = New System.Drawing.Size(216, 20)
        Me.txtExcelFileName.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(30, 11)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(90, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Invoice Excel File"
        '
        'vsfDet
        '
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.None
        Me.vsfDet.ColumnInfo = "2,0,0,0,0,130,Columns:"
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.vsfDet.ForeColor = System.Drawing.Color.Black
        Me.vsfDet.HighLight = C1.Win.C1FlexGrid.HighLightEnum.WithFocus
        Me.vsfDet.Location = New System.Drawing.Point(361, 8)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.Count = 2
        Me.vsfDet.Rows.DefaultSize = 26
        Me.vsfDet.Size = New System.Drawing.Size(185, 28)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 54
        Me.vsfDet.Tag = ""
        Me.vsfDet.Visible = False
        '
        'frmUtlInvXLPrintQuery
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(618, 365)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtExcelFileName)
        Me.Controls.Add(Me.btnVerifyQuery)
        Me.Controls.Add(Me.txtQry)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.btnRefreshQuery)
        Me.Name = "frmUtlInvXLPrintQuery"
        Me.ShowIcon = False
        Me.Text = "Invoice Excel Printing Query"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnRefreshQuery As System.Windows.Forms.Button
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents txtQry As System.Windows.Forms.TextBox
    Friend WithEvents btnVerifyQuery As System.Windows.Forms.Button
    Friend WithEvents txtExcelFileName As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
End Class
