<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUtlCopyFrom
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cboDocName = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtDocNo = New System.Windows.Forms.TextBox()
        Me.btnSelDoc = New System.Windows.Forms.Button()
        Me.chkAppendData = New System.Windows.Forms.CheckBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.txtDocID = New System.Windows.Forms.TextBox()
        Me.lblPartyFilter = New System.Windows.Forms.Label()
        Me.chkShowAllParty = New System.Windows.Forms.CheckBox()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.txtID = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.chkCloseWindow = New System.Windows.Forms.CheckBox()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.chkImpAppend = New System.Windows.Forms.CheckBox()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnTemplate = New System.Windows.Forms.Button()
        Me.txtFilePath = New System.Windows.Forms.TextBox()
        Me.btnImport = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(10, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Document"
        '
        'cboDocName
        '
        Me.cboDocName.FormattingEnabled = True
        Me.cboDocName.Location = New System.Drawing.Point(76, 10)
        Me.cboDocName.Name = "cboDocName"
        Me.cboDocName.Size = New System.Drawing.Size(256, 21)
        Me.cboDocName.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(23, 42)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(44, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Doc No"
        '
        'txtDocNo
        '
        Me.txtDocNo.Location = New System.Drawing.Point(76, 39)
        Me.txtDocNo.Name = "txtDocNo"
        Me.txtDocNo.ReadOnly = True
        Me.txtDocNo.Size = New System.Drawing.Size(140, 20)
        Me.txtDocNo.TabIndex = 3
        '
        'btnSelDoc
        '
        Me.btnSelDoc.BackColor = System.Drawing.Color.Transparent
        Me.btnSelDoc.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSelDoc.Location = New System.Drawing.Point(222, 39)
        Me.btnSelDoc.Name = "btnSelDoc"
        Me.btnSelDoc.Size = New System.Drawing.Size(24, 23)
        Me.btnSelDoc.TabIndex = 4
        Me.btnSelDoc.Text = "..."
        Me.btnSelDoc.UseVisualStyleBackColor = False
        '
        'chkAppendData
        '
        Me.chkAppendData.AutoSize = True
        Me.chkAppendData.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.chkAppendData.Location = New System.Drawing.Point(128, 114)
        Me.chkAppendData.Name = "chkAppendData"
        Me.chkAppendData.Size = New System.Drawing.Size(137, 17)
        Me.chkAppendData.TabIndex = 5
        Me.chkAppendData.Text = "Append to Existing Data"
        Me.chkAppendData.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.txtDocID)
        Me.Panel1.Controls.Add(Me.lblPartyFilter)
        Me.Panel1.Controls.Add(Me.chkShowAllParty)
        Me.Panel1.Controls.Add(Me.btnLoad)
        Me.Panel1.Controls.Add(Me.cboDocName)
        Me.Panel1.Controls.Add(Me.chkAppendData)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.btnSelDoc)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.txtDocNo)
        Me.Panel1.Controls.Add(Me.txtID)
        Me.Panel1.Location = New System.Drawing.Point(10, 6)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(384, 142)
        Me.Panel1.TabIndex = 6
        '
        'txtDocID
        '
        Me.txtDocID.Location = New System.Drawing.Point(266, 41)
        Me.txtDocID.Name = "txtDocID"
        Me.txtDocID.Size = New System.Drawing.Size(41, 20)
        Me.txtDocID.TabIndex = 9
        Me.txtDocID.Visible = False
        '
        'lblPartyFilter
        '
        Me.lblPartyFilter.AutoSize = True
        Me.lblPartyFilter.Location = New System.Drawing.Point(21, 65)
        Me.lblPartyFilter.Name = "lblPartyFilter"
        Me.lblPartyFilter.Size = New System.Drawing.Size(207, 13)
        Me.lblPartyFilter.TabIndex = 8
        Me.lblPartyFilter.Text = "(Currently Data is being filtered for ..XXX...)"
        '
        'chkShowAllParty
        '
        Me.chkShowAllParty.AutoSize = True
        Me.chkShowAllParty.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.chkShowAllParty.Location = New System.Drawing.Point(29, 86)
        Me.chkShowAllParty.Name = "chkShowAllParty"
        Me.chkShowAllParty.Size = New System.Drawing.Size(124, 17)
        Me.chkShowAllParty.TabIndex = 7
        Me.chkShowAllParty.Text = "Show all Parties Data"
        Me.chkShowAllParty.UseVisualStyleBackColor = True
        '
        'btnLoad
        '
        Me.btnLoad.BackColor = System.Drawing.Color.Transparent
        Me.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLoad.Location = New System.Drawing.Point(269, 110)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(74, 23)
        Me.btnLoad.TabIndex = 6
        Me.btnLoad.Text = "Load"
        Me.btnLoad.UseVisualStyleBackColor = False
        '
        'txtID
        '
        Me.txtID.Location = New System.Drawing.Point(266, 42)
        Me.txtID.Name = "txtID"
        Me.txtID.Size = New System.Drawing.Size(38, 20)
        Me.txtID.TabIndex = 10
        Me.txtID.Visible = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(9, 166)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(381, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "This will insert the Details of the selected document to current active document." &
    ""
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Location = New System.Drawing.Point(6, 9)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(428, 245)
        Me.TabControl1.TabIndex = 9
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabPage1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TabPage1.Controls.Add(Me.chkCloseWindow)
        Me.TabPage1.Controls.Add(Me.Panel1)
        Me.TabPage1.Controls.Add(Me.Label3)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(420, 219)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Load from an Existing Document"
        '
        'chkCloseWindow
        '
        Me.chkCloseWindow.AutoSize = True
        Me.chkCloseWindow.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.chkCloseWindow.Location = New System.Drawing.Point(10, 194)
        Me.chkCloseWindow.Name = "chkCloseWindow"
        Me.chkCloseWindow.Size = New System.Drawing.Size(247, 17)
        Me.chkCloseWindow.TabIndex = 11
        Me.chkCloseWindow.Text = "Keep this window open after loading document."
        Me.chkCloseWindow.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.TabPage2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TabPage2.Controls.Add(Me.Label5)
        Me.TabPage2.Controls.Add(Me.Panel2)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(420, 219)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Import from XL"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(71, 191)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(287, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Imports details from excel sheet to current active document."
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.chkImpAppend)
        Me.Panel2.Controls.Add(Me.btnBrowse)
        Me.Panel2.Controls.Add(Me.Label4)
        Me.Panel2.Controls.Add(Me.btnTemplate)
        Me.Panel2.Controls.Add(Me.txtFilePath)
        Me.Panel2.Controls.Add(Me.btnImport)
        Me.Panel2.Location = New System.Drawing.Point(6, 6)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(408, 172)
        Me.Panel2.TabIndex = 0
        '
        'chkImpAppend
        '
        Me.chkImpAppend.AutoSize = True
        Me.chkImpAppend.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.chkImpAppend.Location = New System.Drawing.Point(6, 144)
        Me.chkImpAppend.Name = "chkImpAppend"
        Me.chkImpAppend.Size = New System.Drawing.Size(137, 17)
        Me.chkImpAppend.TabIndex = 6
        Me.chkImpAppend.Text = "Append to Existing Data"
        Me.chkImpAppend.UseVisualStyleBackColor = True
        '
        'btnBrowse
        '
        Me.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBrowse.Location = New System.Drawing.Point(328, 64)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowse.TabIndex = 5
        Me.btnBrowse.Text = "Browse"
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(3, 65)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(69, 18)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "File Path:"
        '
        'btnTemplate
        '
        Me.btnTemplate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTemplate.Location = New System.Drawing.Point(247, 144)
        Me.btnTemplate.Name = "btnTemplate"
        Me.btnTemplate.Size = New System.Drawing.Size(75, 23)
        Me.btnTemplate.TabIndex = 3
        Me.btnTemplate.Text = "Template"
        Me.btnTemplate.UseVisualStyleBackColor = True
        '
        'txtFilePath
        '
        Me.txtFilePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFilePath.Location = New System.Drawing.Point(78, 65)
        Me.txtFilePath.Name = "txtFilePath"
        Me.txtFilePath.ReadOnly = True
        Me.txtFilePath.Size = New System.Drawing.Size(244, 20)
        Me.txtFilePath.TabIndex = 1
        '
        'btnImport
        '
        Me.btnImport.Enabled = False
        Me.btnImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnImport.Location = New System.Drawing.Point(328, 144)
        Me.btnImport.Name = "btnImport"
        Me.btnImport.Size = New System.Drawing.Size(75, 23)
        Me.btnImport.TabIndex = 4
        Me.btnImport.Text = "Import"
        Me.btnImport.UseVisualStyleBackColor = True
        '
        'frmUtlCopyFrom
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(434, 254)
        Me.Controls.Add(Me.TabControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmUtlCopyFrom"
        Me.ShowIcon = False
        Me.Text = "Load/Copy information from Another Document..."
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cboDocName As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtDocNo As System.Windows.Forms.TextBox
    Friend WithEvents btnSelDoc As System.Windows.Forms.Button
    Friend WithEvents chkAppendData As System.Windows.Forms.CheckBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnLoad As System.Windows.Forms.Button
    Friend WithEvents chkShowAllParty As System.Windows.Forms.CheckBox
    Friend WithEvents lblPartyFilter As System.Windows.Forms.Label
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents txtDocID As System.Windows.Forms.TextBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnTemplate As System.Windows.Forms.Button
    Friend WithEvents txtFilePath As System.Windows.Forms.TextBox
    Friend WithEvents btnImport As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtID As System.Windows.Forms.TextBox
    Friend WithEvents chkCloseWindow As System.Windows.Forms.CheckBox
    Friend WithEvents chkImpAppend As System.Windows.Forms.CheckBox
End Class
