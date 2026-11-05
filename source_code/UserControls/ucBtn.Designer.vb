<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucBtn
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucBtn))
        Me.tsBtn = New System.Windows.Forms.ToolStrip()
        Me.tsbInsert = New System.Windows.Forms.ToolStripButton()
        Me.GSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbRemove = New System.Windows.Forms.ToolStripButton()
        Me.GSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbCopy = New System.Windows.Forms.ToolStripButton()
        Me.tsbPaste = New System.Windows.Forms.ToolStripButton()
        Me.tsbReplace = New System.Windows.Forms.ToolStripButton()
        Me.GSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbMoveUp = New System.Windows.Forms.ToolStripButton()
        Me.tsbMoveDown = New System.Windows.Forms.ToolStripButton()
        Me.GSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbExport = New System.Windows.Forms.ToolStripButton()
        Me.tsbAttachment = New System.Windows.Forms.ToolStripButton()
        Me.GSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbRestore = New System.Windows.Forms.ToolStripButton()
        Me.tsbFreeze = New System.Windows.Forms.ToolStripButton()
        Me.GSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbAddItem = New System.Windows.Forms.ToolStripButton()
        Me.tsbDocExport = New System.Windows.Forms.ToolStripButton()
        Me.GSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbImport = New System.Windows.Forms.ToolStripButton()
        Me.tsbTemplate = New System.Windows.Forms.ToolStripButton()
        Me.tsbCopyFrom = New System.Windows.Forms.ToolStripButton()
        Me.tsbAmmend = New System.Windows.Forms.ToolStripButton()
        Me.tsbTender = New System.Windows.Forms.ToolStripButton()
        Me.tsbInserDoc = New System.Windows.Forms.ToolStripButton()
        Me.tsbMultiDoc = New System.Windows.Forms.ToolStripButton()
        Me.tsbSMS = New System.Windows.Forms.ToolStripButton()
        Me.tsbBOM = New System.Windows.Forms.ToolStripButton()
        Me.tsbCalTotal = New System.Windows.Forms.ToolStripButton()
        Me.tsbPending = New System.Windows.Forms.ToolStripButton()
        Me.tsbResize = New System.Windows.Forms.ToolStripButton()
        Me.tsbAddnl = New System.Windows.Forms.ToolStripSplitButton()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Cdlg = New System.Windows.Forms.OpenFileDialog()
        Me.tsBtn.SuspendLayout()
        Me.SuspendLayout()
        '
        'tsBtn
        '
        Me.tsBtn.AutoSize = False
        Me.tsBtn.BackColor = System.Drawing.Color.Transparent
        Me.tsBtn.GripMargin = New System.Windows.Forms.Padding(0)
        Me.tsBtn.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.tsBtn.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbInsert, Me.GSeparator1, Me.tsbRemove, Me.GSeparator2, Me.tsbCopy, Me.tsbPaste, Me.tsbReplace, Me.GSeparator3, Me.tsbMoveUp, Me.tsbMoveDown, Me.GSeparator4, Me.tsbExport, Me.tsbAttachment, Me.GSeparator5, Me.tsbRestore, Me.tsbFreeze, Me.GSeparator6, Me.tsbAddItem, Me.tsbDocExport, Me.GSeparator7, Me.tsbImport, Me.tsbTemplate, Me.tsbCopyFrom, Me.tsbAmmend, Me.tsbTender, Me.tsbInserDoc, Me.tsbMultiDoc, Me.tsbSMS, Me.tsbBOM, Me.tsbCalTotal, Me.tsbPending, Me.tsbResize, Me.tsbAddnl})
        Me.tsBtn.Location = New System.Drawing.Point(0, 0)
        Me.tsBtn.Name = "tsBtn"
        Me.tsBtn.Size = New System.Drawing.Size(712, 21)
        Me.tsBtn.TabIndex = 0
        '
        'tsbInsert
        '
        Me.tsbInsert.AutoSize = False
        Me.tsbInsert.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbInsert.Image = Global.vLQMS.My.Resources.Resources.ucInsert
        Me.tsbInsert.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbInsert.Name = "tsbInsert"
        Me.tsbInsert.Size = New System.Drawing.Size(20, 20)
        Me.tsbInsert.Text = "Insert"
        Me.tsbInsert.ToolTipText = "Insert : Ctrl+Shift+I"
        '
        'GSeparator1
        '
        Me.GSeparator1.BackColor = System.Drawing.Color.White
        Me.GSeparator1.ForeColor = System.Drawing.Color.Black
        Me.GSeparator1.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.GSeparator1.Name = "GSeparator1"
        Me.GSeparator1.Size = New System.Drawing.Size(6, 21)
        '
        'tsbRemove
        '
        Me.tsbRemove.AutoSize = False
        Me.tsbRemove.BackColor = System.Drawing.Color.Orange
        Me.tsbRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbRemove.Image = Global.vLQMS.My.Resources.Resources.ucRemove_Row
        Me.tsbRemove.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbRemove.Name = "tsbRemove"
        Me.tsbRemove.Size = New System.Drawing.Size(20, 20)
        Me.tsbRemove.Text = "Remove : Ctrl+Shift+R"
        Me.tsbRemove.ToolTipText = "Remove : Ctrl+Shift+ R"
        '
        'GSeparator2
        '
        Me.GSeparator2.BackColor = System.Drawing.Color.Gray
        Me.GSeparator2.ForeColor = System.Drawing.Color.Black
        Me.GSeparator2.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.GSeparator2.Name = "GSeparator2"
        Me.GSeparator2.Size = New System.Drawing.Size(6, 21)
        '
        'tsbCopy
        '
        Me.tsbCopy.AutoSize = False
        Me.tsbCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbCopy.Image = Global.vLQMS.My.Resources.Resources.ucCopy
        Me.tsbCopy.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbCopy.Name = "tsbCopy"
        Me.tsbCopy.Size = New System.Drawing.Size(20, 20)
        Me.tsbCopy.Text = "Copy : Ctrl+Shift+C"
        '
        'tsbPaste
        '
        Me.tsbPaste.AutoSize = False
        Me.tsbPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbPaste.Image = Global.vLQMS.My.Resources.Resources.ucPaste
        Me.tsbPaste.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbPaste.Name = "tsbPaste"
        Me.tsbPaste.Size = New System.Drawing.Size(20, 20)
        Me.tsbPaste.Text = "Paste : Ctrl+Alt+P"
        '
        'tsbReplace
        '
        Me.tsbReplace.AutoSize = False
        Me.tsbReplace.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbReplace.Image = CType(resources.GetObject("tsbReplace.Image"), System.Drawing.Image)
        Me.tsbReplace.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbReplace.Name = "tsbReplace"
        Me.tsbReplace.Size = New System.Drawing.Size(20, 20)
        Me.tsbReplace.Text = "Replace Rest : Ctrl+Alt+N"
        '
        'GSeparator3
        '
        Me.GSeparator3.BackColor = System.Drawing.Color.Gray
        Me.GSeparator3.ForeColor = System.Drawing.Color.Black
        Me.GSeparator3.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.GSeparator3.Name = "GSeparator3"
        Me.GSeparator3.Size = New System.Drawing.Size(6, 21)
        '
        'tsbMoveUp
        '
        Me.tsbMoveUp.AutoSize = False
        Me.tsbMoveUp.BackColor = System.Drawing.Color.Transparent
        Me.tsbMoveUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.tsbMoveUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbMoveUp.Image = Global.vLQMS.My.Resources.Resources.ucMove_Up
        Me.tsbMoveUp.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbMoveUp.Name = "tsbMoveUp"
        Me.tsbMoveUp.Size = New System.Drawing.Size(20, 20)
        Me.tsbMoveUp.Text = "Move Up  : Ctrl+Shift+U"
        '
        'tsbMoveDown
        '
        Me.tsbMoveDown.AutoSize = False
        Me.tsbMoveDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.tsbMoveDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbMoveDown.Image = Global.vLQMS.My.Resources.Resources.ucMove_Down
        Me.tsbMoveDown.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbMoveDown.Name = "tsbMoveDown"
        Me.tsbMoveDown.Size = New System.Drawing.Size(20, 20)
        Me.tsbMoveDown.Text = "Move Down : Ctrl+Shift+V"
        '
        'GSeparator4
        '
        Me.GSeparator4.BackColor = System.Drawing.Color.Gray
        Me.GSeparator4.ForeColor = System.Drawing.Color.Black
        Me.GSeparator4.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.GSeparator4.Name = "GSeparator4"
        Me.GSeparator4.Size = New System.Drawing.Size(6, 21)
        '
        'tsbExport
        '
        Me.tsbExport.AutoSize = False
        Me.tsbExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbExport.Image = Global.vLQMS.My.Resources.Resources.ucExport
        Me.tsbExport.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbExport.Name = "tsbExport"
        Me.tsbExport.Size = New System.Drawing.Size(20, 20)
        Me.tsbExport.Text = "Dtl Section XL Export : Ctrl+Shift+E"
        '
        'tsbAttachment
        '
        Me.tsbAttachment.AutoSize = False
        Me.tsbAttachment.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbAttachment.Image = Global.vLQMS.My.Resources.Resources.ucAttach
        Me.tsbAttachment.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbAttachment.Name = "tsbAttachment"
        Me.tsbAttachment.Size = New System.Drawing.Size(20, 20)
        Me.tsbAttachment.Text = "Attachment : Ctrl+Shift+A"
        '
        'GSeparator5
        '
        Me.GSeparator5.BackColor = System.Drawing.Color.Gray
        Me.GSeparator5.ForeColor = System.Drawing.Color.Black
        Me.GSeparator5.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.GSeparator5.Name = "GSeparator5"
        Me.GSeparator5.Size = New System.Drawing.Size(6, 21)
        '
        'tsbRestore
        '
        Me.tsbRestore.AutoSize = False
        Me.tsbRestore.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbRestore.Image = Global.vLQMS.My.Resources.Resources.ucRestore1
        Me.tsbRestore.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbRestore.Name = "tsbRestore"
        Me.tsbRestore.Size = New System.Drawing.Size(20, 20)
        Me.tsbRestore.Text = "Restore : Ctrl+Shift+O"
        '
        'tsbFreeze
        '
        Me.tsbFreeze.AutoSize = False
        Me.tsbFreeze.BackColor = System.Drawing.Color.Transparent
        Me.tsbFreeze.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.tsbFreeze.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbFreeze.Image = Global.vLQMS.My.Resources.Resources.freeze
        Me.tsbFreeze.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbFreeze.Name = "tsbFreeze"
        Me.tsbFreeze.Size = New System.Drawing.Size(20, 20)
        Me.tsbFreeze.Text = "Freeze : Ctrl+Shift+Z"
        '
        'GSeparator6
        '
        Me.GSeparator6.BackColor = System.Drawing.Color.Gray
        Me.GSeparator6.ForeColor = System.Drawing.Color.Black
        Me.GSeparator6.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.GSeparator6.Name = "GSeparator6"
        Me.GSeparator6.Size = New System.Drawing.Size(6, 21)
        '
        'tsbAddItem
        '
        Me.tsbAddItem.AutoSize = False
        Me.tsbAddItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbAddItem.Image = Global.vLQMS.My.Resources.Resources.Add_Items
        Me.tsbAddItem.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbAddItem.Name = "tsbAddItem"
        Me.tsbAddItem.Size = New System.Drawing.Size(20, 20)
        Me.tsbAddItem.Text = "Add List of Items : Ctrl+Shift+Q"
        '
        'tsbDocExport
        '
        Me.tsbDocExport.AutoSize = False
        Me.tsbDocExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbDocExport.Image = Global.vLQMS.My.Resources.Resources.docexport_xl
        Me.tsbDocExport.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbDocExport.Margin = New System.Windows.Forms.Padding(0)
        Me.tsbDocExport.Name = "tsbDocExport"
        Me.tsbDocExport.Size = New System.Drawing.Size(20, 20)
        Me.tsbDocExport.Text = "Doc XL Export"
        Me.tsbDocExport.ToolTipText = "Doc XL Export : Ctrl + Shift + X"
        '
        'GSeparator7
        '
        Me.GSeparator7.BackColor = System.Drawing.Color.Gray
        Me.GSeparator7.ForeColor = System.Drawing.Color.Black
        Me.GSeparator7.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.GSeparator7.Name = "GSeparator7"
        Me.GSeparator7.Size = New System.Drawing.Size(6, 21)
        '
        'tsbImport
        '
        Me.tsbImport.AutoSize = False
        Me.tsbImport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbImport.Image = Global.vLQMS.My.Resources.Resources.ucImport
        Me.tsbImport.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbImport.Name = "tsbImport"
        Me.tsbImport.Size = New System.Drawing.Size(20, 20)
        Me.tsbImport.Text = "XL Import : Ctrl+Shift+H"
        '
        'tsbTemplate
        '
        Me.tsbTemplate.AutoSize = False
        Me.tsbTemplate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbTemplate.Image = Global.vLQMS.My.Resources.Resources.ucTemplate
        Me.tsbTemplate.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbTemplate.Name = "tsbTemplate"
        Me.tsbTemplate.Size = New System.Drawing.Size(20, 20)
        Me.tsbTemplate.Text = "XL Template : Ctrl+Shift+T"
        '
        'tsbCopyFrom
        '
        Me.tsbCopyFrom.AutoSize = False
        Me.tsbCopyFrom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbCopyFrom.Image = Global.vLQMS.My.Resources.Resources.copyFrom
        Me.tsbCopyFrom.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbCopyFrom.Name = "tsbCopyFrom"
        Me.tsbCopyFrom.Size = New System.Drawing.Size(20, 20)
        Me.tsbCopyFrom.Text = "Copy From : Ctrl+Alt+F"
        '
        'tsbAmmend
        '
        Me.tsbAmmend.AutoSize = False
        Me.tsbAmmend.BackColor = System.Drawing.Color.Transparent
        Me.tsbAmmend.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.tsbAmmend.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbAmmend.Image = Global.vLQMS.My.Resources.Resources.ammendment
        Me.tsbAmmend.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbAmmend.Name = "tsbAmmend"
        Me.tsbAmmend.Size = New System.Drawing.Size(20, 20)
        Me.tsbAmmend.Text = "Ammend: Ctrl + Shift + J"
        '
        'tsbTender
        '
        Me.tsbTender.AutoSize = False
        Me.tsbTender.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbTender.Image = Global.vLQMS.My.Resources.Resources.tenderChange
        Me.tsbTender.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbTender.Name = "tsbTender"
        Me.tsbTender.Size = New System.Drawing.Size(20, 20)
        Me.tsbTender.Text = "Tender Change : Ctrl+Shift+Y"
        '
        'tsbInserDoc
        '
        Me.tsbInserDoc.AutoSize = False
        Me.tsbInserDoc.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbInserDoc.Image = Global.vLQMS.My.Resources.Resources.insertdoc
        Me.tsbInserDoc.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbInserDoc.Name = "tsbInserDoc"
        Me.tsbInserDoc.Size = New System.Drawing.Size(20, 20)
        Me.tsbInserDoc.Text = "Insert Doc : Ctrl+Shift+B"
        '
        'tsbMultiDoc
        '
        Me.tsbMultiDoc.AutoSize = False
        Me.tsbMultiDoc.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbMultiDoc.Image = Global.vLQMS.My.Resources.Resources.multidoc
        Me.tsbMultiDoc.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbMultiDoc.Name = "tsbMultiDoc"
        Me.tsbMultiDoc.Size = New System.Drawing.Size(20, 20)
        Me.tsbMultiDoc.Text = "Multi Doc Generation : Ctrl+Shift+G"
        '
        'tsbSMS
        '
        Me.tsbSMS.AutoSize = False
        Me.tsbSMS.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbSMS.Image = CType(resources.GetObject("tsbSMS.Image"), System.Drawing.Image)
        Me.tsbSMS.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSMS.Name = "tsbSMS"
        Me.tsbSMS.Size = New System.Drawing.Size(20, 20)
        Me.tsbSMS.Text = "SMS"
        '
        'tsbBOM
        '
        Me.tsbBOM.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbBOM.Image = Global.vLQMS.My.Resources.Resources.ucBOM
        Me.tsbBOM.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbBOM.Name = "tsbBOM"
        Me.tsbBOM.Size = New System.Drawing.Size(23, 18)
        Me.tsbBOM.Text = "BOM Details"
        '
        'tsbCalTotal
        '
        Me.tsbCalTotal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbCalTotal.Image = CType(resources.GetObject("tsbCalTotal.Image"), System.Drawing.Image)
        Me.tsbCalTotal.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbCalTotal.Name = "tsbCalTotal"
        Me.tsbCalTotal.Size = New System.Drawing.Size(23, 18)
        Me.tsbCalTotal.Text = "Cal Total"
        '
        'tsbPending
        '
        Me.tsbPending.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbPending.Image = CType(resources.GetObject("tsbPending.Image"), System.Drawing.Image)
        Me.tsbPending.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbPending.Name = "tsbPending"
        Me.tsbPending.Size = New System.Drawing.Size(23, 18)
        Me.tsbPending.Text = "Pending"
        Me.tsbPending.ToolTipText = "Pending List"
        Me.tsbPending.Visible = False
        '
        'tsbResize
        '
        Me.tsbResize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbResize.Image = CType(resources.GetObject("tsbResize.Image"), System.Drawing.Image)
        Me.tsbResize.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbResize.Name = "tsbResize"
        Me.tsbResize.Size = New System.Drawing.Size(23, 18)
        Me.tsbResize.Text = "Resize"
        Me.tsbResize.ToolTipText = "Resize Grid"
        Me.tsbResize.Visible = False
        '
        'tsbAddnl
        '
        Me.tsbAddnl.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbAddnl.Image = CType(resources.GetObject("tsbAddnl.Image"), System.Drawing.Image)
        Me.tsbAddnl.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbAddnl.Name = "tsbAddnl"
        Me.tsbAddnl.Size = New System.Drawing.Size(32, 18)
        Me.tsbAddnl.Text = "Additional"
        Me.tsbAddnl.ToolTipText = "Additional Actions"
        '
        'Cdlg
        '
        Me.Cdlg.FileName = "Cdlg"
        '
        'ucBtn
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.tsBtn)
        Me.Name = "ucBtn"
        Me.Size = New System.Drawing.Size(712, 22)
        Me.tsBtn.ResumeLayout(False)
        Me.tsBtn.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tsBtn As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents tsbInsert As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbRemove As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbCopy As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbPaste As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbMoveUp As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbMoveDown As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbReplace As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbExport As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbImport As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbTemplate As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbAttachment As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbRestore As System.Windows.Forms.ToolStripButton
    Friend WithEvents Cdlg As System.Windows.Forms.OpenFileDialog
    Friend WithEvents tsbCopyFrom As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbAmmend As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbFreeze As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbAddItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbTender As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbInserDoc As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbMultiDoc As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbDocExport As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbBOM As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbCalTotal As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbPending As ToolStripButton
    Friend WithEvents tsbResize As ToolStripButton
    Friend WithEvents GSeparator1 As ToolStripSeparator
    Friend WithEvents GSeparator2 As ToolStripSeparator
    Friend WithEvents GSeparator5 As ToolStripSeparator
    Friend WithEvents GSeparator4 As ToolStripSeparator
    Friend WithEvents GSeparator3 As ToolStripSeparator
    Friend WithEvents GSeparator6 As ToolStripSeparator
    Friend WithEvents GSeparator7 As ToolStripSeparator
    Friend WithEvents tsbAddnl As ToolStripSplitButton
    Friend WithEvents tsbSMS As ToolStripButton
End Class
