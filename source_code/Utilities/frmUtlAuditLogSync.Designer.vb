<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlAuditLogSync
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlAuditLogSync))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtLogDB = New System.Windows.Forms.TextBox()
        Me.btnSync = New System.Windows.Forms.Button()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnClose = New C1.Win.C1Input.C1Button()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label1.Location = New System.Drawing.Point(12, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(93, 17)
        Me.Label1.TabIndex = 0
        Me.Label1.Tag = "R"
        Me.Label1.Text = "Log Database "
        '
        'txtLogDB
        '
        Me.txtLogDB.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtLogDB.Location = New System.Drawing.Point(111, 21)
        Me.txtLogDB.Name = "txtLogDB"
        Me.txtLogDB.Size = New System.Drawing.Size(261, 25)
        Me.txtLogDB.TabIndex = 1
        '
        'btnSync
        '
        Me.btnSync.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnSync.Location = New System.Drawing.Point(378, 20)
        Me.btnSync.Name = "btnSync"
        Me.btnSync.Size = New System.Drawing.Size(154, 27)
        Me.btnSync.TabIndex = 23
        Me.btnSync.Text = "Sync Structure"
        Me.btnSync.UseVisualStyleBackColor = True
        '
        'vsfDet
        '
        Me.vsfDet.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.ColumnInfo = "5,0,0,0,0,120,Columns:"
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.HighLight = C1.Win.C1FlexGrid.HighLightEnum.Never
        Me.vsfDet.Location = New System.Drawing.Point(11, 68)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(2)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.Count = 1
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.Size = New System.Drawing.Size(651, 380)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 24
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(577, 454)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(85, 37)
        Me.btnClose.TabIndex = 25
        Me.btnClose.Text = "&Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'frmUtlAuditLogSync
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(673, 499)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.btnSync)
        Me.Controls.Add(Me.txtLogDB)
        Me.Controls.Add(Me.Label1)
        Me.KeyPreview = True
        Me.Name = "frmUtlAuditLogSync"
        Me.ShowIcon = False
        Me.Text = "AuditLog Sync"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents txtLogDB As TextBox
    Friend WithEvents btnSync As Button
    Private WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
End Class
