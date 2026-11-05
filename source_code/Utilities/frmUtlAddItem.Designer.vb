<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlAddItem
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlAddItem))
        Me.btnLoadItemsToCPO = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.vsfItems = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnLoadItems = New System.Windows.Forms.Button()
        Me.txtData = New System.Windows.Forms.TextBox()
        CType(Me.vsfItems, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnLoadItemsToCPO
        '
        Me.btnLoadItemsToCPO.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoadItemsToCPO.Location = New System.Drawing.Point(2, 250)
        Me.btnLoadItemsToCPO.Name = "btnLoadItemsToCPO"
        Me.btnLoadItemsToCPO.Size = New System.Drawing.Size(152, 26)
        Me.btnLoadItemsToCPO.TabIndex = 230
        Me.btnLoadItemsToCPO.Text = "Load Items To Order"
        Me.btnLoadItemsToCPO.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Blue
        Me.Label5.Location = New System.Drawing.Point(338, 250)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(194, 17)
        Me.Label5.TabIndex = 228
        Me.Label5.Text = "After pasting click on << button"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Blue
        Me.Label6.Location = New System.Drawing.Point(333, 2)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(236, 17)
        Me.Label6.TabIndex = 227
        Me.Label6.Text = "Paste the List of Items in the box below"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'vsfItems
        '
        Me.vsfItems.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfItems.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfItems.ColumnInfo = "2,1,0,0,0,120,Columns:"
        Me.vsfItems.ExtendLastCol = True
        Me.vsfItems.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfItems.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcrossOut
        Me.vsfItems.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcrossOut
        Me.vsfItems.Location = New System.Drawing.Point(3, 2)
        Me.vsfItems.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfItems.Name = "vsfItems"
        Me.vsfItems.Rows.DefaultSize = 24
        Me.vsfItems.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfItems.Size = New System.Drawing.Size(268, 241)
        Me.vsfItems.StyleInfo = resources.GetString("vsfItems.StyleInfo")
        Me.vsfItems.TabIndex = 226
        Me.vsfItems.Tag = ""
        '
        'btnLoadItems
        '
        Me.btnLoadItems.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.btnLoadItems.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoadItems.Location = New System.Drawing.Point(276, 104)
        Me.btnLoadItems.Name = "btnLoadItems"
        Me.btnLoadItems.Size = New System.Drawing.Size(54, 26)
        Me.btnLoadItems.TabIndex = 1
        Me.btnLoadItems.Text = "<<<<"
        Me.btnLoadItems.UseVisualStyleBackColor = False
        '
        'txtData
        '
        Me.txtData.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtData.Location = New System.Drawing.Point(336, 22)
        Me.txtData.Multiline = True
        Me.txtData.Name = "txtData"
        Me.txtData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtData.Size = New System.Drawing.Size(233, 218)
        Me.txtData.TabIndex = 0
        '
        'frmUtlAddItem
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(582, 294)
        Me.Controls.Add(Me.btnLoadItemsToCPO)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.vsfItems)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtData)
        Me.Controls.Add(Me.btnLoadItems)
        Me.Name = "frmUtlAddItem"
        Me.ShowIcon = False
        Me.Text = "Add Items into Transaction"
        CType(Me.vsfItems, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnLoadItemsToCPO As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents vsfItems As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnLoadItems As System.Windows.Forms.Button
    Friend WithEvents txtData As System.Windows.Forms.TextBox
End Class
