<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlInsertfromDoc
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlInsertfromDoc))
        Me.cboInsertFromDocName = New System.Windows.Forms.ComboBox()
        Me.lblDocNo = New System.Windows.Forms.Label()
        Me.cboInsertFromDocNo = New System.Windows.Forms.ComboBox()
        Me.btnInsert = New System.Windows.Forms.Button()
        Me.vsfOptions = New C1.Win.C1FlexGrid.C1FlexGrid()
        CType(Me.vsfOptions, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cboInsertFromDocName
        '
        Me.cboInsertFromDocName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cboInsertFromDocName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboInsertFromDocName.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cboInsertFromDocName.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboInsertFromDocName.FormattingEnabled = True
        Me.cboInsertFromDocName.ItemHeight = 17
        Me.cboInsertFromDocName.Location = New System.Drawing.Point(12, 12)
        Me.cboInsertFromDocName.MaxDropDownItems = 5
        Me.cboInsertFromDocName.MaxLength = 10
        Me.cboInsertFromDocName.Name = "cboInsertFromDocName"
        Me.cboInsertFromDocName.Size = New System.Drawing.Size(233, 25)
        Me.cboInsertFromDocName.TabIndex = 2
        '
        'lblDocNo
        '
        Me.lblDocNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDocNo.Location = New System.Drawing.Point(12, 55)
        Me.lblDocNo.Name = "lblDocNo"
        Me.lblDocNo.Size = New System.Drawing.Size(148, 16)
        Me.lblDocNo.TabIndex = 3
        Me.lblDocNo.Text = "Doc No"
        '
        'cboInsertFromDocNo
        '
        Me.cboInsertFromDocNo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cboInsertFromDocNo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboInsertFromDocNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cboInsertFromDocNo.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboInsertFromDocNo.FormattingEnabled = True
        Me.cboInsertFromDocNo.ItemHeight = 17
        Me.cboInsertFromDocNo.Location = New System.Drawing.Point(12, 73)
        Me.cboInsertFromDocNo.MaxDropDownItems = 50
        Me.cboInsertFromDocNo.MaxLength = 10
        Me.cboInsertFromDocNo.Name = "cboInsertFromDocNo"
        Me.cboInsertFromDocNo.Size = New System.Drawing.Size(148, 25)
        Me.cboInsertFromDocNo.TabIndex = 4
        '
        'btnInsert
        '
        Me.btnInsert.BackColor = System.Drawing.Color.Transparent
        Me.btnInsert.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnInsert.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnInsert.Image = CType(resources.GetObject("btnInsert.Image"), System.Drawing.Image)
        Me.btnInsert.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnInsert.Location = New System.Drawing.Point(172, 71)
        Me.btnInsert.Name = "btnInsert"
        Me.btnInsert.Size = New System.Drawing.Size(73, 29)
        Me.btnInsert.TabIndex = 5
        Me.btnInsert.Text = "&Load"
        Me.btnInsert.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnInsert.UseVisualStyleBackColor = False
        '
        'vsfOptions
        '
        Me.vsfOptions.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfOptions.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfOptions.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfOptions.ColumnInfo = "2,1,0,0,0,120,Columns:"
        Me.vsfOptions.ExtendLastCol = True
        Me.vsfOptions.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfOptions.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfOptions.Location = New System.Drawing.Point(215, 43)
        Me.vsfOptions.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfOptions.Name = "vsfOptions"
        Me.vsfOptions.Rows.DefaultSize = 24
        Me.vsfOptions.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfOptions.Size = New System.Drawing.Size(30, 28)
        Me.vsfOptions.StyleInfo = resources.GetString("vsfOptions.StyleInfo")
        Me.vsfOptions.TabIndex = 39
        Me.vsfOptions.Tag = ""
        Me.vsfOptions.Visible = False
        '
        'frmUtlInsertfromDoc
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(253, 104)
        Me.Controls.Add(Me.vsfOptions)
        Me.Controls.Add(Me.btnInsert)
        Me.Controls.Add(Me.cboInsertFromDocNo)
        Me.Controls.Add(Me.lblDocNo)
        Me.Controls.Add(Me.cboInsertFromDocName)
        Me.ForeColor = System.Drawing.SystemColors.ControlText
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(269, 142)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(269, 142)
        Me.Name = "frmUtlInsertfromDoc"
        Me.ShowIcon = False
        Me.Text = "Insert from Document"
        CType(Me.vsfOptions, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cboInsertFromDocName As System.Windows.Forms.ComboBox
    Friend WithEvents lblDocNo As System.Windows.Forms.Label
    Friend WithEvents cboInsertFromDocNo As System.Windows.Forms.ComboBox
    Friend WithEvents btnInsert As System.Windows.Forms.Button
    Public WithEvents vsfOptions As C1.Win.C1FlexGrid.C1FlexGrid
End Class
