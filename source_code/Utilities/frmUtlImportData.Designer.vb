<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlImportData
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlImportData))
        Me.btnLoadItem = New System.Windows.Forms.Button()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.chkAll = New System.Windows.Forms.CheckBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.pnlMaster = New System.Windows.Forms.Panel()
        Me.optMasters = New System.Windows.Forms.RadioButton()
        Me.optTransaction = New System.Windows.Forms.RadioButton()
        Me.pnlTrn = New System.Windows.Forms.Panel()
        Me.vsfTrn = New C1.Win.C1FlexGrid.C1FlexGrid()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlMaster.SuspendLayout()
        Me.pnlTrn.SuspendLayout()
        CType(Me.vsfTrn, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnLoadItem
        '
        Me.btnLoadItem.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLoadItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnLoadItem.Location = New System.Drawing.Point(647, 567)
        Me.btnLoadItem.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnLoadItem.Name = "btnLoadItem"
        Me.btnLoadItem.Size = New System.Drawing.Size(75, 29)
        Me.btnLoadItem.TabIndex = 2
        Me.btnLoadItem.Text = "Import"
        Me.btnLoadItem.UseVisualStyleBackColor = False
        '
        'vsfDet
        '
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.ColumnInfo = resources.GetString("vsfDet.ColumnInfo")
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfDet.Location = New System.Drawing.Point(9, 13)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(768, 479)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 28
        Me.vsfDet.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2010Silver
        '
        'chkAll
        '
        Me.chkAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkAll.AutoSize = True
        Me.chkAll.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAll.Location = New System.Drawing.Point(12, 578)
        Me.chkAll.Name = "chkAll"
        Me.chkAll.Size = New System.Drawing.Size(79, 21)
        Me.chkAll.TabIndex = 29
        Me.chkAll.Text = "Select &All"
        Me.chkAll.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackgroundImage = Global.vICAxon.My.Resources.Resources.T3_CLOSE
        Me.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(728, 568)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 29)
        Me.btnCancel.TabIndex = 30
        Me.btnCancel.Text = "Close"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'pnlMaster
        '
        Me.pnlMaster.Controls.Add(Me.vsfDet)
        Me.pnlMaster.Location = New System.Drawing.Point(23, 57)
        Me.pnlMaster.Name = "pnlMaster"
        Me.pnlMaster.Size = New System.Drawing.Size(780, 495)
        Me.pnlMaster.TabIndex = 31
        '
        'optMasters
        '
        Me.optMasters.AutoSize = True
        Me.optMasters.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optMasters.Location = New System.Drawing.Point(32, 12)
        Me.optMasters.Name = "optMasters"
        Me.optMasters.Size = New System.Drawing.Size(81, 20)
        Me.optMasters.TabIndex = 32
        Me.optMasters.TabStop = True
        Me.optMasters.Text = "Masters"
        Me.optMasters.UseVisualStyleBackColor = True
        '
        'optTransaction
        '
        Me.optTransaction.AutoSize = True
        Me.optTransaction.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optTransaction.Location = New System.Drawing.Point(161, 12)
        Me.optTransaction.Name = "optTransaction"
        Me.optTransaction.Size = New System.Drawing.Size(116, 20)
        Me.optTransaction.TabIndex = 33
        Me.optTransaction.TabStop = True
        Me.optTransaction.Text = "Transactions"
        Me.optTransaction.UseVisualStyleBackColor = True
        '
        'pnlTrn
        '
        Me.pnlTrn.Controls.Add(Me.vsfTrn)
        Me.pnlTrn.Location = New System.Drawing.Point(21, 54)
        Me.pnlTrn.Name = "pnlTrn"
        Me.pnlTrn.Size = New System.Drawing.Size(780, 495)
        Me.pnlTrn.TabIndex = 34
        '
        'vsfTrn
        '
        Me.vsfTrn.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfTrn.ColumnInfo = resources.GetString("vsfTrn.ColumnInfo")
        Me.vsfTrn.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfTrn.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
        Me.vsfTrn.Location = New System.Drawing.Point(9, 13)
        Me.vsfTrn.Name = "vsfTrn"
        Me.vsfTrn.Rows.DefaultSize = 24
        Me.vsfTrn.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfTrn.Size = New System.Drawing.Size(768, 479)
        Me.vsfTrn.StyleInfo = resources.GetString("vsfTrn.StyleInfo")
        Me.vsfTrn.TabIndex = 28
        Me.vsfTrn.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2010Silver
        '
        'frmUtlImportData
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(815, 612)
        Me.Controls.Add(Me.pnlTrn)
        Me.Controls.Add(Me.optTransaction)
        Me.Controls.Add(Me.optMasters)
        Me.Controls.Add(Me.pnlMaster)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.chkAll)
        Me.Controls.Add(Me.btnLoadItem)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "frmUtlImportData"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Import Data"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlMaster.ResumeLayout(False)
        Me.pnlTrn.ResumeLayout(False)
        CType(Me.vsfTrn, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnLoadItem As System.Windows.Forms.Button
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents chkAll As System.Windows.Forms.CheckBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents pnlMaster As System.Windows.Forms.Panel
    Friend WithEvents optMasters As System.Windows.Forms.RadioButton
    Friend WithEvents optTransaction As System.Windows.Forms.RadioButton
    Friend WithEvents pnlTrn As System.Windows.Forms.Panel
    Friend WithEvents vsfTrn As C1.Win.C1FlexGrid.C1FlexGrid
End Class
