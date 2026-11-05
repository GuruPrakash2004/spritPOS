<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlSQLQueryExec
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlSQLQueryExec))
        Me.txtQuery = New System.Windows.Forms.TextBox()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.GBErr = New System.Windows.Forms.GroupBox()
        Me.txtErrMsg = New System.Windows.Forms.TextBox()
        Me.vDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.cs = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.tsmEdit = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmSelect = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmScript = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmCreate = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmDrop = New System.Windows.Forms.ToolStripMenuItem()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.dv = New System.Windows.Forms.DataGridView()
        Me.btnExecute = New System.Windows.Forms.Button()
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.lblSearchImage = New System.Windows.Forms.Label()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblDBName = New System.Windows.Forms.Label()
        Me.lblServer = New System.Windows.Forms.Label()
        Me.ButtonPinHide1 = New System.Windows.Forms.Button()
        Me.ButtonPin = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnExport = New C1.Win.C1Input.C1Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.BindingSource1 = New System.Windows.Forms.BindingSource(Me.components)
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GBErr.SuspendLayout()
        CType(Me.vDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cs.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.dv, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.BindingSource1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtQuery
        '
        Me.txtQuery.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtQuery.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtQuery.Location = New System.Drawing.Point(3, 38)
        Me.txtQuery.Multiline = True
        Me.txtQuery.Name = "txtQuery"
        Me.txtQuery.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtQuery.Size = New System.Drawing.Size(555, 143)
        Me.txtQuery.TabIndex = 0
        '
        'vsfDet
        '
        Me.vsfDet.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.vsfDet.AllowFiltering = True
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.AutoClipboard = True
        Me.vsfDet.AutoResize = True
        Me.vsfDet.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfDet.ClipboardCopyMode = C1.Win.C1FlexGrid.ClipboardCopyModeEnum.DataAndColumnHeaders
        Me.vsfDet.ColumnInfo = "10,0,0,0,0,120,Columns:"
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.Location = New System.Drawing.Point(3, 3)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.ScrollOptions = C1.Win.C1FlexGrid.ScrollFlags.ShowScrollTips
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.CellRange
        Me.vsfDet.Size = New System.Drawing.Size(555, 255)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 9
        Me.vsfDet.VisualStyle = C1.Win.C1FlexGrid.VisualStyle.Office2010Silver
        '
        'GBErr
        '
        Me.GBErr.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GBErr.Controls.Add(Me.txtErrMsg)
        Me.GBErr.Location = New System.Drawing.Point(4, 114)
        Me.GBErr.Name = "GBErr"
        Me.GBErr.Size = New System.Drawing.Size(551, 63)
        Me.GBErr.TabIndex = 228
        Me.GBErr.TabStop = False
        Me.GBErr.Text = "Message"
        '
        'txtErrMsg
        '
        Me.txtErrMsg.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtErrMsg.BackColor = System.Drawing.Color.White
        Me.txtErrMsg.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtErrMsg.Location = New System.Drawing.Point(3, 17)
        Me.txtErrMsg.Multiline = True
        Me.txtErrMsg.Name = "txtErrMsg"
        Me.txtErrMsg.ReadOnly = True
        Me.txtErrMsg.Size = New System.Drawing.Size(542, 40)
        Me.txtErrMsg.TabIndex = 224
        Me.txtErrMsg.Visible = False
        '
        'vDet
        '
        Me.vDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vDet.BackColor = System.Drawing.Color.White
        Me.vDet.ColumnInfo = "1,0,0,0,0,110,Columns:"
        Me.vDet.ContextMenuStrip = Me.cs
        Me.vDet.ExtendLastCol = True
        Me.vDet.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.vDet.Location = New System.Drawing.Point(3, 101)
        Me.vDet.Name = "vDet"
        Me.vDet.Rows.Count = 1
        Me.vDet.Rows.DefaultSize = 22
        Me.vDet.Rows.Fixed = 0
        Me.vDet.Size = New System.Drawing.Size(240, 393)
        Me.vDet.StyleInfo = resources.GetString("vDet.StyleInfo")
        Me.vDet.TabIndex = 227
        '
        'cs
        '
        Me.cs.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmEdit, Me.tsmSelect, Me.tsmScript})
        Me.cs.Name = "ContextMenuStrip1"
        Me.cs.Size = New System.Drawing.Size(182, 70)
        Me.cs.Text = "a"
        '
        'tsmEdit
        '
        Me.tsmEdit.Name = "tsmEdit"
        Me.tsmEdit.Size = New System.Drawing.Size(181, 22)
        Me.tsmEdit.Text = "Edit Top 200 Rows"
        '
        'tsmSelect
        '
        Me.tsmSelect.Name = "tsmSelect"
        Me.tsmSelect.Size = New System.Drawing.Size(181, 22)
        Me.tsmSelect.Text = "Select Top 100 Rows"
        '
        'tsmScript
        '
        Me.tsmScript.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmCreate, Me.tsmDrop})
        Me.tsmScript.Name = "tsmScript"
        Me.tsmScript.Size = New System.Drawing.Size(181, 22)
        Me.tsmScript.Text = "Script Table As"
        '
        'tsmCreate
        '
        Me.tsmCreate.Name = "tsmCreate"
        Me.tsmCreate.Size = New System.Drawing.Size(108, 22)
        Me.tsmCreate.Text = "Create"
        '
        'tsmDrop
        '
        Me.tsmDrop.Name = "tsmDrop"
        Me.tsmDrop.Size = New System.Drawing.Size(108, 22)
        Me.tsmDrop.Text = "Drop"
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SplitContainer1.Location = New System.Drawing.Point(3, 3)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.dv)
        Me.SplitContainer1.Panel1.Controls.Add(Me.btnExecute)
        Me.SplitContainer1.Panel1.Controls.Add(Me.btnClose)
        Me.SplitContainer1.Panel1.Controls.Add(Me.GBErr)
        Me.SplitContainer1.Panel1.Controls.Add(Me.txtQuery)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.vsfDet)
        Me.SplitContainer1.Size = New System.Drawing.Size(563, 452)
        Me.SplitContainer1.SplitterDistance = 186
        Me.SplitContainer1.TabIndex = 229
        '
        'dv
        '
        Me.dv.AllowUserToOrderColumns = True
        Me.dv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dv.Location = New System.Drawing.Point(156, 11)
        Me.dv.Name = "dv"
        Me.dv.Size = New System.Drawing.Size(326, 86)
        Me.dv.TabIndex = 229
        '
        'btnExecute
        '
        Me.btnExecute.BackgroundImage = CType(resources.GetObject("btnExecute.BackgroundImage"), System.Drawing.Image)
        Me.btnExecute.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnExecute.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExecute.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExecute.Location = New System.Drawing.Point(3, 4)
        Me.btnExecute.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.btnExecute.Name = "btnExecute"
        Me.btnExecute.Size = New System.Drawing.Size(116, 30)
        Me.btnExecute.TabIndex = 223
        Me.btnExecute.Text = "Execute Query"
        Me.btnExecute.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnExecute, "Execute : F5")
        Me.btnExecute.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnClose.Image = CType(resources.GetObject("btnClose.Image"), System.Drawing.Image)
        Me.btnClose.Location = New System.Drawing.Point(526, 4)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(33, 30)
        Me.btnClose.TabIndex = 24
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Close : Alt + C")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SplitContainer2.Location = New System.Drawing.Point(1, 2)
        Me.SplitContainer2.Name = "SplitContainer2"
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.SplitContainer2.Panel1.Controls.Add(Me.lblSearch)
        Me.SplitContainer2.Panel1.Controls.Add(Me.lblSearchImage)
        Me.SplitContainer2.Panel1.Controls.Add(Me.txtSearch)
        Me.SplitContainer2.Panel1.Controls.Add(Me.Panel1)
        Me.SplitContainer2.Panel1.Controls.Add(Me.ButtonPinHide1)
        Me.SplitContainer2.Panel1.Controls.Add(Me.ButtonPin)
        Me.SplitContainer2.Panel1.Controls.Add(Me.vDet)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.Label4)
        Me.SplitContainer2.Panel2.Controls.Add(Me.Label3)
        Me.SplitContainer2.Panel2.Controls.Add(Me.btnExport)
        Me.SplitContainer2.Panel2.Controls.Add(Me.SplitContainer1)
        Me.SplitContainer2.Size = New System.Drawing.Size(825, 500)
        Me.SplitContainer2.SplitterDistance = 250
        Me.SplitContainer2.TabIndex = 230
        '
        'lblSearch
        '
        Me.lblSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSearch.AutoSize = True
        Me.lblSearch.BackColor = System.Drawing.Color.Transparent
        Me.lblSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSearch.ForeColor = System.Drawing.Color.Black
        Me.lblSearch.Location = New System.Drawing.Point(-1, 75)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(66, 20)
        Me.lblSearch.TabIndex = 232
        Me.lblSearch.Text = "Search"
        '
        'lblSearchImage
        '
        Me.lblSearchImage.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSearchImage.BackColor = System.Drawing.Color.Transparent
        Me.lblSearchImage.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSearchImage.Image = CType(resources.GetObject("lblSearchImage.Image"), System.Drawing.Image)
        Me.lblSearchImage.Location = New System.Drawing.Point(212, 75)
        Me.lblSearchImage.Name = "lblSearchImage"
        Me.lblSearchImage.Size = New System.Drawing.Size(31, 23)
        Me.lblSearchImage.TabIndex = 231
        Me.lblSearchImage.Visible = False
        '
        'txtSearch
        '
        Me.txtSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSearch.BackColor = System.Drawing.Color.White
        Me.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSearch.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSearch.Location = New System.Drawing.Point(72, 72)
        Me.txtSearch.Margin = New System.Windows.Forms.Padding(4)
        Me.txtSearch.MaxLength = 25
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(129, 26)
        Me.txtSearch.TabIndex = 5
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.lblDBName)
        Me.Panel1.Controls.Add(Me.lblServer)
        Me.Panel1.Location = New System.Drawing.Point(4, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(242, 64)
        Me.Panel1.TabIndex = 230
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Blue
        Me.Label2.Location = New System.Drawing.Point(5, 41)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(73, 16)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "DB Name :"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(5, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(94, 16)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Server Name :"
        '
        'lblDBName
        '
        Me.lblDBName.AutoSize = True
        Me.lblDBName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.lblDBName.Location = New System.Drawing.Point(77, 40)
        Me.lblDBName.Name = "lblDBName"
        Me.lblDBName.Size = New System.Drawing.Size(0, 16)
        Me.lblDBName.TabIndex = 2
        '
        'lblServer
        '
        Me.lblServer.AutoSize = True
        Me.lblServer.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.lblServer.Location = New System.Drawing.Point(98, 10)
        Me.lblServer.Name = "lblServer"
        Me.lblServer.Size = New System.Drawing.Size(0, 16)
        Me.lblServer.TabIndex = 0
        '
        'ButtonPinHide1
        '
        Me.ButtonPinHide1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonPinHide1.BackColor = System.Drawing.Color.Transparent
        Me.ButtonPinHide1.ForeColor = System.Drawing.Color.Transparent
        Me.ButtonPinHide1.Image = CType(resources.GetObject("ButtonPinHide1.Image"), System.Drawing.Image)
        Me.ButtonPinHide1.Location = New System.Drawing.Point(208, 9)
        Me.ButtonPinHide1.Name = "ButtonPinHide1"
        Me.ButtonPinHide1.Size = New System.Drawing.Size(25, 23)
        Me.ButtonPinHide1.TabIndex = 229
        Me.ButtonPinHide1.UseVisualStyleBackColor = False
        Me.ButtonPinHide1.Visible = False
        '
        'ButtonPin
        '
        Me.ButtonPin.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonPin.BackColor = System.Drawing.Color.Transparent
        Me.ButtonPin.ForeColor = System.Drawing.Color.Transparent
        Me.ButtonPin.Image = CType(resources.GetObject("ButtonPin.Image"), System.Drawing.Image)
        Me.ButtonPin.Location = New System.Drawing.Point(207, 8)
        Me.ButtonPin.Name = "ButtonPin"
        Me.ButtonPin.Size = New System.Drawing.Size(26, 23)
        Me.ButtonPin.TabIndex = 228
        Me.ButtonPin.UseVisualStyleBackColor = False
        Me.ButtonPin.Visible = False
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Label4.ForeColor = System.Drawing.Color.Blue
        Me.Label4.Location = New System.Drawing.Point(116, 471)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(0, 16)
        Me.Label4.TabIndex = 231
        Me.Label4.Visible = False
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Label3.ForeColor = System.Drawing.Color.Blue
        Me.Label3.Location = New System.Drawing.Point(5, 471)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(111, 16)
        Me.Label3.TabIndex = 230
        Me.Label3.Text = "Total no of rows  :"
        Me.Label3.Visible = False
        '
        'btnExport
        '
        Me.btnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExport.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.btnExport.Image = CType(resources.GetObject("btnExport.Image"), System.Drawing.Image)
        Me.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExport.Location = New System.Drawing.Point(533, 463)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(33, 31)
        Me.btnExport.TabIndex = 224
        Me.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnExport, "Export to Excel : Alt + E")
        Me.btnExport.UseVisualStyleBackColor = False
        '
        'frmUtlSQLQueryExec
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(827, 503)
        Me.Controls.Add(Me.SplitContainer2)
        Me.KeyPreview = True
        Me.Name = "frmUtlSQLQueryExec"
        Me.ShowIcon = False
        Me.Text = "SQL Query Execution"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GBErr.ResumeLayout(False)
        Me.GBErr.PerformLayout()
        CType(Me.vDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cs.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.dv, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.PerformLayout()
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        Me.SplitContainer2.Panel2.PerformLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.BindingSource1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents txtQuery As TextBox
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents btnExecute As Button
    Friend WithEvents btnExport As C1.Win.C1Input.C1Button
    Friend WithEvents GBErr As GroupBox
    Friend WithEvents txtErrMsg As TextBox
    Friend WithEvents vDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents SplitContainer2 As SplitContainer
    Friend WithEvents ButtonPin As Button
    Friend WithEvents ButtonPinHide1 As Button
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents Panel1 As Panel
    Friend WithEvents lblDBName As Label
    Friend WithEvents lblServer As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents dv As DataGridView
    Friend WithEvents BindingSource1 As BindingSource
    Friend WithEvents cs As ContextMenuStrip
    Friend WithEvents tsmEdit As ToolStripMenuItem
    Friend WithEvents tsmSelect As ToolStripMenuItem
    Friend WithEvents tsmScript As ToolStripMenuItem
    Friend WithEvents tsmCreate As ToolStripMenuItem
    Friend WithEvents tsmDrop As ToolStripMenuItem
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents lblSearchImage As Label
    Friend WithEvents lblSearch As Label
End Class
