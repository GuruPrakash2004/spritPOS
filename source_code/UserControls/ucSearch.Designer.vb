<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ucSearch
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucSearch))
        Me.lblParty = New System.Windows.Forms.Label()
        Me.txtPartyID = New System.Windows.Forms.TextBox()
        Me.vsfView = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.vsfAddress = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnLoad = New C1.Win.C1Input.C1Button()
        Me.lblSearchImage = New System.Windows.Forms.Label()
        Me.txtParty = New vLQMS.Watermark()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfAddress, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblParty
        '
        Me.lblParty.AutoSize = True
        Me.lblParty.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblParty.ForeColor = System.Drawing.Color.White
        Me.lblParty.Location = New System.Drawing.Point(6, 9)
        Me.lblParty.Name = "lblParty"
        Me.lblParty.Size = New System.Drawing.Size(44, 17)
        Me.lblParty.TabIndex = 0
        Me.lblParty.Text = "Party "
        '
        'txtPartyID
        '
        Me.txtPartyID.Location = New System.Drawing.Point(95, 3)
        Me.txtPartyID.Name = "txtPartyID"
        Me.txtPartyID.Size = New System.Drawing.Size(100, 25)
        Me.txtPartyID.TabIndex = 2
        Me.txtPartyID.Visible = False
        '
        'vsfView
        '
        Me.vsfView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfView.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfView.ExtendLastCol = True
        Me.vsfView.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfView.Location = New System.Drawing.Point(3, 31)
        Me.vsfView.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfView.Name = "vsfView"
        Me.vsfView.Rows.DefaultSize = 24
        Me.vsfView.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfView.Size = New System.Drawing.Size(443, 117)
        Me.vsfView.StyleInfo = resources.GetString("vsfView.StyleInfo")
        Me.vsfView.TabIndex = 3
        '
        'vsfAddress
        '
        Me.vsfAddress.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfAddress.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfAddress.ColumnInfo = "1,0,0,0,0,120,Columns:"
        Me.vsfAddress.ExtendLastCol = True
        Me.vsfAddress.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfAddress.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfAddress.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfAddress.Location = New System.Drawing.Point(95, 31)
        Me.vsfAddress.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfAddress.Name = "vsfAddress"
        Me.vsfAddress.Rows.Count = 5
        Me.vsfAddress.Rows.DefaultSize = 24
        Me.vsfAddress.Rows.Fixed = 0
        Me.vsfAddress.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfAddress.Size = New System.Drawing.Size(351, 99)
        Me.vsfAddress.StyleInfo = resources.GetString("vsfAddress.StyleInfo")
        Me.vsfAddress.TabIndex = 11
        Me.vsfAddress.TabStop = False
        Me.vsfAddress.Tag = ""
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(6, 59)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 17)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Address"
        '
        'btnLoad
        '
        Me.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLoad.Location = New System.Drawing.Point(380, 2)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(35, 27)
        Me.btnLoad.TabIndex = 12
        Me.btnLoad.Text = "..."
        Me.btnLoad.UseVisualStyleBackColor = True
        Me.btnLoad.Visible = False
        '
        'lblSearchImage
        '
        Me.lblSearchImage.BackColor = System.Drawing.Color.White
        Me.lblSearchImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.lblSearchImage.Image = CType(resources.GetObject("lblSearchImage.Image"), System.Drawing.Image)
        Me.lblSearchImage.Location = New System.Drawing.Point(384, 6)
        Me.lblSearchImage.Name = "lblSearchImage"
        Me.lblSearchImage.Size = New System.Drawing.Size(23, 21)
        Me.lblSearchImage.TabIndex = 16
        '
        'txtParty
        '
        Me.txtParty.Location = New System.Drawing.Point(56, 4)
        Me.txtParty.Name = "txtParty"
        Me.txtParty.Size = New System.Drawing.Size(359, 25)
        Me.txtParty.TabIndex = 13
        Me.txtParty.WatermarkColor = System.Drawing.Color.Gray
        Me.txtParty.WatermarkText = "Search..."
        '
        'ucSearch
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.Controls.Add(Me.lblSearchImage)
        Me.Controls.Add(Me.txtParty)
        Me.Controls.Add(Me.vsfView)
        Me.Controls.Add(Me.btnLoad)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblParty)
        Me.Controls.Add(Me.txtPartyID)
        Me.Controls.Add(Me.vsfAddress)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "ucSearch"
        Me.Size = New System.Drawing.Size(452, 152)
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfAddress, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtPartyID As System.Windows.Forms.TextBox
    Friend WithEvents vsfView As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents vsfAddress As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnLoad As C1.Win.C1Input.C1Button
    Public WithEvents lblParty As Label
    Friend WithEvents txtParty As Watermark
    Friend WithEvents lblSearchImage As Label
End Class
