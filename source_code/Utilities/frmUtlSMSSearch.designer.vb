<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlSMSSearch
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlSMSSearch))
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnCancel = New C1.Win.C1Input.C1Button()
        Me.btnLoad = New C1.Win.C1Input.C1Button()
        Me.chkSelClr = New System.Windows.Forms.CheckBox()
        Me.btnMoveUP = New System.Windows.Forms.Button()
        Me.btnMoveDown = New System.Windows.Forms.Button()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'vsfDet
        '
        Me.vsfDet.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "3,0,0,0,0,120,Columns:"
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.Location = New System.Drawing.Point(43, 30)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(430, 407)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Label1.Location = New System.Drawing.Point(40, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(363, 17)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Please check the fields on which the search should take place"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(404, 440)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(70, 31)
        Me.btnCancel.TabIndex = 16
        Me.btnCancel.Text = "Cl&ose"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnLoad
        '
        Me.btnLoad.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLoad.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoad.Image = CType(resources.GetObject("btnLoad.Image"), System.Drawing.Image)
        Me.btnLoad.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnLoad.Location = New System.Drawing.Point(331, 440)
        Me.btnLoad.Margin = New System.Windows.Forms.Padding(4)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(65, 31)
        Me.btnLoad.TabIndex = 15
        Me.btnLoad.Text = "&Load"
        Me.btnLoad.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnLoad.UseVisualStyleBackColor = False
        '
        'chkSelClr
        '
        Me.chkSelClr.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkSelClr.AutoSize = True
        Me.chkSelClr.Location = New System.Drawing.Point(43, 450)
        Me.chkSelClr.Name = "chkSelClr"
        Me.chkSelClr.Size = New System.Drawing.Size(114, 21)
        Me.chkSelClr.TabIndex = 17
        Me.chkSelClr.Text = "Select/Clear All"
        Me.chkSelClr.UseVisualStyleBackColor = True
        '
        'btnMoveUP
        '
        Me.btnMoveUP.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.btnMoveUP.ForeColor = System.Drawing.Color.Black
        Me.btnMoveUP.Image = CType(resources.GetObject("btnMoveUP.Image"), System.Drawing.Image)
        Me.btnMoveUP.Location = New System.Drawing.Point(12, 30)
        Me.btnMoveUP.Name = "btnMoveUP"
        Me.btnMoveUP.Size = New System.Drawing.Size(28, 188)
        Me.btnMoveUP.TabIndex = 201
        Me.btnMoveUP.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnMoveUP.UseVisualStyleBackColor = False
        '
        'btnMoveDown
        '
        Me.btnMoveDown.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.btnMoveDown.ForeColor = System.Drawing.Color.Black
        Me.btnMoveDown.Image = CType(resources.GetObject("btnMoveDown.Image"), System.Drawing.Image)
        Me.btnMoveDown.Location = New System.Drawing.Point(12, 217)
        Me.btnMoveDown.Name = "btnMoveDown"
        Me.btnMoveDown.Size = New System.Drawing.Size(28, 220)
        Me.btnMoveDown.TabIndex = 202
        Me.btnMoveDown.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnMoveDown.UseVisualStyleBackColor = False
        '
        'frmUtlSMSSearch
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(485, 486)
        Me.Controls.Add(Me.btnMoveDown)
        Me.Controls.Add(Me.btnMoveUP)
        Me.Controls.Add(Me.chkSelClr)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnLoad)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.vsfDet)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MinimumSize = New System.Drawing.Size(470, 524)
        Me.Name = "frmUtlSMSSearch"
        Me.ShowIcon = False
        Me.Text = "Search Settings..."
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnCancel As C1.Win.C1Input.C1Button
    Friend WithEvents btnLoad As C1.Win.C1Input.C1Button
    Friend WithEvents chkSelClr As System.Windows.Forms.CheckBox
    Friend WithEvents btnMoveUP As System.Windows.Forms.Button
    Friend WithEvents btnMoveDown As System.Windows.Forms.Button
    Public WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
End Class
