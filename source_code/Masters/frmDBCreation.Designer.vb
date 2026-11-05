<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDBCreation
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDBCreation))
        Me.pnlFnlYr = New System.Windows.Forms.Panel()
        Me.btnUpdatePath = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtDBPath = New System.Windows.Forms.TextBox()
        Me.lblNetPath = New System.Windows.Forms.Label()
        Me.txtNetPath = New System.Windows.Forms.TextBox()
        Me.lblMsg = New System.Windows.Forms.Label()
        Me.txtDummy2 = New System.Windows.Forms.TextBox()
        Me.txtDummy = New System.Windows.Forms.TextBox()
        Me.btnChangeFinYr = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnCreateDB = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.dtpTyr = New System.Windows.Forms.DateTimePicker()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.pnlFnlYr.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlFnlYr
        '
        Me.pnlFnlYr.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlFnlYr.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.pnlFnlYr.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlFnlYr.Controls.Add(Me.btnUpdatePath)
        Me.pnlFnlYr.Controls.Add(Me.Label2)
        Me.pnlFnlYr.Controls.Add(Me.txtDBPath)
        Me.pnlFnlYr.Controls.Add(Me.lblNetPath)
        Me.pnlFnlYr.Controls.Add(Me.txtNetPath)
        Me.pnlFnlYr.Controls.Add(Me.lblMsg)
        Me.pnlFnlYr.Controls.Add(Me.txtDummy2)
        Me.pnlFnlYr.Controls.Add(Me.txtDummy)
        Me.pnlFnlYr.Controls.Add(Me.btnChangeFinYr)
        Me.pnlFnlYr.Controls.Add(Me.Label1)
        Me.pnlFnlYr.Controls.Add(Me.btnCreateDB)
        Me.pnlFnlYr.Controls.Add(Me.btnClose)
        Me.pnlFnlYr.Controls.Add(Me.dtpTyr)
        Me.pnlFnlYr.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.pnlFnlYr.Location = New System.Drawing.Point(5, 12)
        Me.pnlFnlYr.Name = "pnlFnlYr"
        Me.pnlFnlYr.Size = New System.Drawing.Size(631, 177)
        Me.pnlFnlYr.TabIndex = 3
        '
        'btnUpdatePath
        '
        Me.btnUpdatePath.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.btnUpdatePath.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnUpdatePath.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUpdatePath.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnUpdatePath.Location = New System.Drawing.Point(381, 78)
        Me.btnUpdatePath.Name = "btnUpdatePath"
        Me.btnUpdatePath.Size = New System.Drawing.Size(147, 29)
        Me.btnUpdatePath.TabIndex = 19
        Me.btnUpdatePath.Text = "Update Path"
        Me.btnUpdatePath.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.Label2.Location = New System.Drawing.Point(5, 112)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(69, 20)
        Me.Label2.TabIndex = 18
        Me.Label2.Text = "DB Path"
        '
        'txtDBPath
        '
        Me.txtDBPath.Location = New System.Drawing.Point(115, 111)
        Me.txtDBPath.Name = "txtDBPath"
        Me.txtDBPath.Size = New System.Drawing.Size(255, 25)
        Me.txtDBPath.TabIndex = 17
        '
        'lblNetPath
        '
        Me.lblNetPath.AutoSize = True
        Me.lblNetPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.lblNetPath.Location = New System.Drawing.Point(5, 84)
        Me.lblNetPath.Name = "lblNetPath"
        Me.lblNetPath.Size = New System.Drawing.Size(104, 20)
        Me.lblNetPath.TabIndex = 16
        Me.lblNetPath.Text = "Network Path"
        '
        'txtNetPath
        '
        Me.txtNetPath.Location = New System.Drawing.Point(115, 79)
        Me.txtNetPath.Name = "txtNetPath"
        Me.txtNetPath.Size = New System.Drawing.Size(255, 25)
        Me.txtNetPath.TabIndex = 15
        '
        'lblMsg
        '
        Me.lblMsg.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblMsg.Font = New System.Drawing.Font("Segoe UI", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMsg.Location = New System.Drawing.Point(3, 144)
        Me.lblMsg.Name = "lblMsg"
        Me.lblMsg.Size = New System.Drawing.Size(238, 25)
        Me.lblMsg.TabIndex = 14
        Me.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblMsg.Visible = False
        '
        'txtDummy2
        '
        Me.txtDummy2.Location = New System.Drawing.Point(301, 145)
        Me.txtDummy2.Name = "txtDummy2"
        Me.txtDummy2.Size = New System.Drawing.Size(100, 25)
        Me.txtDummy2.TabIndex = 13
        Me.txtDummy2.Visible = False
        '
        'txtDummy
        '
        Me.txtDummy.Location = New System.Drawing.Point(301, 145)
        Me.txtDummy.Name = "txtDummy"
        Me.txtDummy.Size = New System.Drawing.Size(100, 25)
        Me.txtDummy.TabIndex = 12
        Me.txtDummy.Visible = False
        '
        'btnChangeFinYr
        '
        Me.btnChangeFinYr.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnChangeFinYr.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnChangeFinYr.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnChangeFinYr.Image = CType(resources.GetObject("btnChangeFinYr.Image"), System.Drawing.Image)
        Me.btnChangeFinYr.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnChangeFinYr.Location = New System.Drawing.Point(461, 29)
        Me.btnChangeFinYr.Name = "btnChangeFinYr"
        Me.btnChangeFinYr.Size = New System.Drawing.Size(161, 37)
        Me.btnChangeFinYr.TabIndex = 11
        Me.btnChangeFinYr.Text = "Change Fin Year"
        Me.btnChangeFinYr.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnChangeFinYr.UseVisualStyleBackColor = False
        Me.btnChangeFinYr.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(2, 38)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(152, 20)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Data Archive Before"
        '
        'btnCreateDB
        '
        Me.btnCreateDB.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCreateDB.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.btnCreateDB.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCreateDB.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCreateDB.Image = CType(resources.GetObject("btnCreateDB.Image"), System.Drawing.Image)
        Me.btnCreateDB.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCreateDB.Location = New System.Drawing.Point(394, 110)
        Me.btnCreateDB.Name = "btnCreateDB"
        Me.btnCreateDB.Size = New System.Drawing.Size(118, 29)
        Me.btnCreateDB.TabIndex = 0
        Me.btnCreateDB.Text = "Create DB"
        Me.btnCreateDB.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCreateDB.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(534, 77)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(90, 29)
        Me.btnClose.TabIndex = 1
        Me.btnClose.Text = "Cancel"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'dtpTyr
        '
        Me.dtpTyr.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtpTyr.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpTyr.Location = New System.Drawing.Point(160, 33)
        Me.dtpTyr.MaxDate = New Date(2075, 12, 31, 0, 0, 0, 0)
        Me.dtpTyr.MinDate = New Date(1990, 1, 1, 0, 0, 0, 0)
        Me.dtpTyr.Name = "dtpTyr"
        Me.dtpTyr.Size = New System.Drawing.Size(113, 26)
        Me.dtpTyr.TabIndex = 1
        Me.dtpTyr.Value = New Date(2025, 12, 31, 0, 0, 0, 0)
        '
        'Timer1
        '
        '
        'frmDBCreation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(639, 189)
        Me.Controls.Add(Me.pnlFnlYr)
        Me.MinimumSize = New System.Drawing.Size(491, 176)
        Me.Name = "frmDBCreation"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Data Carry Forward"
        Me.pnlFnlYr.ResumeLayout(False)
        Me.pnlFnlYr.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlFnlYr As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents btnCreateDB As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents dtpTyr As DateTimePicker
    Friend WithEvents btnChangeFinYr As Button
    Friend WithEvents txtDummy As TextBox
    Friend WithEvents txtDummy2 As TextBox
    Friend WithEvents lblMsg As Label
    Friend WithEvents Timer1 As Timer
    Friend WithEvents lblNetPath As Label
    Friend WithEvents txtNetPath As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtDBPath As TextBox
    Friend WithEvents btnUpdatePath As Button
End Class
