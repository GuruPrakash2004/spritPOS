<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlShowHelpFiles
    Inherits System.Windows.Forms.Form
    'Inherits MetroFramework.Forms.MetroForm

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlShowHelpFiles))
        Me.fbd = New System.Windows.Forms.FolderBrowserDialog()
        Me.fsd = New System.Windows.Forms.OpenFileDialog()
        Me.lblfsize = New System.Windows.Forms.Label()
        Me.lblfname = New System.Windows.Forms.Label()
        Me.lblmod = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'fsd
        '
        Me.fsd.FileName = "OpenFileDialog1"
        '
        'lblfsize
        '
        Me.lblfsize.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblfsize.AutoSize = True
        Me.lblfsize.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblfsize.Location = New System.Drawing.Point(12, 253)
        Me.lblfsize.Name = "lblfsize"
        Me.lblfsize.Size = New System.Drawing.Size(0, 16)
        Me.lblfsize.TabIndex = 13
        '
        'lblfname
        '
        Me.lblfname.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblfname.AutoSize = True
        Me.lblfname.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblfname.Location = New System.Drawing.Point(12, 231)
        Me.lblfname.Name = "lblfname"
        Me.lblfname.Size = New System.Drawing.Size(0, 16)
        Me.lblfname.TabIndex = 14
        '
        'lblmod
        '
        Me.lblmod.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblmod.AutoSize = True
        Me.lblmod.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblmod.Location = New System.Drawing.Point(12, 273)
        Me.lblmod.Name = "lblmod"
        Me.lblmod.Size = New System.Drawing.Size(0, 16)
        Me.lblmod.TabIndex = 15
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.TextBox1.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(22, 188)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(452, 53)
        Me.TextBox1.TabIndex = 19
        Me.TextBox1.Text = "Note : Double Click the File Type in the Grid to Open The Required File"
        '
        'btnClose
        '
        Me.btnClose.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Location = New System.Drawing.Point(394, 250)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(65, 31)
        Me.btnClose.TabIndex = 18
        Me.btnClose.Text = "&Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnAdd
        '
        Me.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnAdd.Location = New System.Drawing.Point(296, 250)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(77, 31)
        Me.btnAdd.TabIndex = 17
        Me.btnAdd.Text = "Open"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'vsfDet
        '
        Me.vsfDet.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "10,0,0,0,0,95,Columns:0{AllowMerging:True;Style:""TextAlign:LeftCenter;"";StyleFixe" &
    "d:""TextAlign:LeftCenter;"";}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfDet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.vsfDet.Location = New System.Drawing.Point(22, 17)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 19
        Me.vsfDet.Rows.Fixed = 0
        Me.vsfDet.Rows.GlyphRow = 0
        Me.vsfDet.Size = New System.Drawing.Size(454, 169)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 16
        '
        'frmUtlShowHelpFiles
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(498, 298)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.lblmod)
        Me.Controls.Add(Me.lblfname)
        Me.Controls.Add(Me.lblfsize)
        Me.Name = "frmUtlShowHelpFiles"
        Me.ShowIcon = False
        Me.Text = "Help Files..."
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents fbd As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents fsd As System.Windows.Forms.OpenFileDialog
    Friend WithEvents lblfsize As System.Windows.Forms.Label
    Friend WithEvents lblfname As System.Windows.Forms.Label
    Friend WithEvents lblmod As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
End Class
