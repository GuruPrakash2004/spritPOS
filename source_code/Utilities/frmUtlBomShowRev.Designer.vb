<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlBomShowRev
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlBomShowRev))
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblBOM = New System.Windows.Forms.Label()
        Me.btnExport = New C1.Win.C1Input.C1Button()
        Me.gbEff = New System.Windows.Forms.GroupBox()
        Me.vsfEff = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.dtEffTo = New System.Windows.Forms.DateTimePicker()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.dtEffFrom = New System.Windows.Forms.DateTimePicker()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblItemName = New System.Windows.Forms.Label()
        Me.lblItemCode = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbEff.SuspendLayout()
        CType(Me.vsfEff, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'vsfDet
        '
        Me.vsfDet.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "2,1,0,0,0,120,Columns:"
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.Location = New System.Drawing.Point(16, 149)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.Count = 6
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.Size = New System.Drawing.Size(918, 323)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 13
        Me.vsfDet.Tag = ""
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(859, 479)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 29)
        Me.btnClose.TabIndex = 124
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close:Alt+O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(13, 126)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(317, 17)
        Me.Label2.TabIndex = 125
        Me.Label2.Text = "[Red--Indicates Change,Green--Indicates No Change]"
        '
        'lblBOM
        '
        Me.lblBOM.AutoSize = True
        Me.lblBOM.BackColor = System.Drawing.SystemColors.Control
        Me.lblBOM.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblBOM.ForeColor = System.Drawing.Color.Red
        Me.lblBOM.Location = New System.Drawing.Point(13, 100)
        Me.lblBOM.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblBOM.Name = "lblBOM"
        Me.lblBOM.Size = New System.Drawing.Size(131, 17)
        Me.lblBOM.TabIndex = 126
        Me.lblBOM.Text = "BOM's For Child Item"
        '
        'btnExport
        '
        Me.btnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExport.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnExport.Image = CType(resources.GetObject("btnExport.Image"), System.Drawing.Image)
        Me.btnExport.Location = New System.Drawing.Point(819, 477)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(33, 31)
        Me.btnExport.TabIndex = 207
        Me.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnExport, "Export to Excel:Alt+E")
        Me.btnExport.UseVisualStyleBackColor = False
        '
        'gbEff
        '
        Me.gbEff.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbEff.Controls.Add(Me.vsfEff)
        Me.gbEff.Location = New System.Drawing.Point(575, 3)
        Me.gbEff.Name = "gbEff"
        Me.gbEff.Size = New System.Drawing.Size(359, 140)
        Me.gbEff.TabIndex = 586
        Me.gbEff.TabStop = False
        '
        'vsfEff
        '
        Me.vsfEff.AllowResizing = C1.Win.C1FlexGrid.AllowResizingEnum.None
        Me.vsfEff.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfEff.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfEff.ColumnInfo = "2,1,0,0,0,120,Columns:"
        Me.vsfEff.ExtendLastCol = True
        Me.vsfEff.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfEff.Location = New System.Drawing.Point(6, 11)
        Me.vsfEff.Name = "vsfEff"
        Me.vsfEff.Rows.Count = 5
        Me.vsfEff.Rows.DefaultSize = 24
        Me.vsfEff.Size = New System.Drawing.Size(347, 123)
        Me.vsfEff.StyleInfo = resources.GetString("vsfEff.StyleInfo")
        Me.vsfEff.TabIndex = 13
        Me.vsfEff.Tag = ""
        '
        'dtEffTo
        '
        Me.dtEffTo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dtEffTo.CustomFormat = "dd/MM/yyyy"
        Me.dtEffTo.Enabled = False
        Me.dtEffTo.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.dtEffTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtEffTo.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.dtEffTo.Location = New System.Drawing.Point(457, 112)
        Me.dtEffTo.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtEffTo.MinDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtEffTo.Name = "dtEffTo"
        Me.dtEffTo.Size = New System.Drawing.Size(112, 25)
        Me.dtEffTo.TabIndex = 114
        Me.dtEffTo.Value = New Date(9998, 12, 31, 0, 0, 0, 0)
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Label4.ForeColor = System.Drawing.Color.Blue
        Me.Label4.Location = New System.Drawing.Point(362, 116)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(79, 16)
        Me.Label4.TabIndex = 115
        Me.Label4.Text = "Effective To"
        '
        'dtEffFrom
        '
        Me.dtEffFrom.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dtEffFrom.CustomFormat = "dd/MM/yyyy"
        Me.dtEffFrom.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.dtEffFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtEffFrom.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.dtEffFrom.Location = New System.Drawing.Point(457, 84)
        Me.dtEffFrom.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dtEffFrom.Name = "dtEffFrom"
        Me.dtEffFrom.Size = New System.Drawing.Size(112, 25)
        Me.dtEffFrom.TabIndex = 111
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Label3.ForeColor = System.Drawing.Color.Red
        Me.Label3.Location = New System.Drawing.Point(362, 88)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(93, 16)
        Me.Label3.TabIndex = 113
        Me.Label3.Text = "Effective From"
        '
        'lblItemName
        '
        Me.lblItemName.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!)
        Me.lblItemName.ForeColor = System.Drawing.Color.Blue
        Me.lblItemName.Location = New System.Drawing.Point(13, 47)
        Me.lblItemName.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblItemName.Name = "lblItemName"
        Me.lblItemName.Size = New System.Drawing.Size(317, 28)
        Me.lblItemName.TabIndex = 587
        Me.lblItemName.Text = "Item Name"
        Me.lblItemName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblItemCode
        '
        Me.lblItemCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!)
        Me.lblItemCode.ForeColor = System.Drawing.Color.Blue
        Me.lblItemCode.Location = New System.Drawing.Point(13, 9)
        Me.lblItemCode.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblItemCode.Name = "lblItemCode"
        Me.lblItemCode.Size = New System.Drawing.Size(317, 30)
        Me.lblItemCode.TabIndex = 588
        Me.lblItemCode.Text = "Item Code"
        Me.lblItemCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'frmUtlBomShowRev
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(946, 519)
        Me.Controls.Add(Me.lblItemCode)
        Me.Controls.Add(Me.lblItemName)
        Me.Controls.Add(Me.dtEffTo)
        Me.Controls.Add(Me.gbEff)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.lblBOM)
        Me.Controls.Add(Me.dtEffFrom)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.vsfDet)
        Me.KeyPreview = True
        Me.Name = "frmUtlBomShowRev"
        Me.ShowIcon = False
        Me.Text = "Revisions of BOM"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbEff.ResumeLayout(False)
        CType(Me.vsfEff, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblBOM As System.Windows.Forms.Label
    Friend WithEvents btnExport As C1.Win.C1Input.C1Button
    Friend WithEvents gbEff As GroupBox
    Public WithEvents dtEffTo As DateTimePicker
    Friend WithEvents Label4 As Label
    Friend WithEvents vsfEff As C1.Win.C1FlexGrid.C1FlexGrid
    Public WithEvents dtEffFrom As DateTimePicker
    Friend WithEvents Label3 As Label
    Friend WithEvents lblItemName As Label
    Friend WithEvents lblItemCode As Label
    Friend WithEvents ToolTip1 As ToolTip
End Class
