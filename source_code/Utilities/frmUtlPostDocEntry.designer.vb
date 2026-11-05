<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlPostDocEntry
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlPostDocEntry))
        Me.vsfPostDoc = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnSelect = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.vsfPostDoc, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'vsfPostDoc
        '
        Me.vsfPostDoc.AllowEditing = False
        Me.vsfPostDoc.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfPostDoc.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfPostDoc.ColumnInfo = "5,0,0,0,0,125,Columns:0{Width:125;}" & Global.Microsoft.VisualBasic.ChrW(9) & "1{Width:125;}" & Global.Microsoft.VisualBasic.ChrW(9) & "2{Width:110;}" & Global.Microsoft.VisualBasic.ChrW(9) & "3{Width:120;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfPostDoc.ExtendLastCol = True
        Me.vsfPostDoc.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold)
        Me.vsfPostDoc.Location = New System.Drawing.Point(12, 12)
        Me.vsfPostDoc.Name = "vsfPostDoc"
        Me.vsfPostDoc.Rows.DefaultSize = 25
        Me.vsfPostDoc.Size = New System.Drawing.Size(639, 345)
        Me.vsfPostDoc.StyleInfo = resources.GetString("vsfPostDoc.StyleInfo")
        Me.vsfPostDoc.TabIndex = 1
        '
        'btnSelect
        '
        Me.btnSelect.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSelect.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSelect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSelect.Location = New System.Drawing.Point(12, 363)
        Me.btnSelect.Name = "btnSelect"
        Me.btnSelect.Size = New System.Drawing.Size(93, 31)
        Me.btnSelect.TabIndex = 2
        Me.btnSelect.Text = "Select"
        Me.btnSelect.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(558, 363)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(93, 31)
        Me.btnClose.TabIndex = 0
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close : Alt + O")
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'frmUtlPostDocEntry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(663, 398)
        Me.Controls.Add(Me.btnSelect)
        Me.Controls.Add(Me.vsfPostDoc)
        Me.Controls.Add(Me.btnClose)
        Me.KeyPreview = True
        Me.Name = "frmUtlPostDocEntry"
        Me.ShowIcon = False
        Me.Text = "Post Doc Entry"
        CType(Me.vsfPostDoc, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnClose As Button
    Friend WithEvents vsfPostDoc As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnSelect As Button
    Friend WithEvents ToolTip1 As ToolTip
End Class
