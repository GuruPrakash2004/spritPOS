<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlLoadFromDoc
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
        Me.lblSelect = New System.Windows.Forms.Label()
        Me.txtDocNo = New System.Windows.Forms.TextBox()
        Me.btnSelect = New System.Windows.Forms.Button()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.txtRet = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'lblSelect
        '
        Me.lblSelect.AutoSize = True
        Me.lblSelect.Location = New System.Drawing.Point(19, 17)
        Me.lblSelect.Name = "lblSelect"
        Me.lblSelect.Size = New System.Drawing.Size(89, 13)
        Me.lblSelect.TabIndex = 0
        Me.lblSelect.Text = "Select Document"
        '
        'txtDocNo
        '
        Me.txtDocNo.Location = New System.Drawing.Point(115, 12)
        Me.txtDocNo.Name = "txtDocNo"
        Me.txtDocNo.Size = New System.Drawing.Size(143, 20)
        Me.txtDocNo.TabIndex = 1
        '
        'btnSelect
        '
        Me.btnSelect.Location = New System.Drawing.Point(264, 13)
        Me.btnSelect.Name = "btnSelect"
        Me.btnSelect.Size = New System.Drawing.Size(26, 21)
        Me.btnSelect.TabIndex = 2
        Me.btnSelect.Text = "..."
        Me.btnSelect.UseVisualStyleBackColor = True
        '
        'btnLoad
        '
        Me.btnLoad.Location = New System.Drawing.Point(183, 73)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(75, 23)
        Me.btnLoad.TabIndex = 3
        Me.btnLoad.Text = "Load"
        Me.btnLoad.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(264, 73)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'txtRet
        '
        Me.txtRet.Location = New System.Drawing.Point(108, 48)
        Me.txtRet.Name = "txtRet"
        Me.txtRet.Size = New System.Drawing.Size(59, 20)
        Me.txtRet.TabIndex = 5
        Me.txtRet.Visible = False
        '
        'frmUtlLoadFromDoc
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(358, 117)
        Me.Controls.Add(Me.txtRet)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnLoad)
        Me.Controls.Add(Me.btnSelect)
        Me.Controls.Add(Me.txtDocNo)
        Me.Controls.Add(Me.lblSelect)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmUtlLoadFromDoc"
        Me.ShowIcon = False
        Me.Text = "Load from "
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblSelect As Label
    Friend WithEvents txtDocNo As TextBox
    Friend WithEvents btnSelect As Button
    Friend WithEvents btnLoad As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents txtRet As TextBox
End Class
