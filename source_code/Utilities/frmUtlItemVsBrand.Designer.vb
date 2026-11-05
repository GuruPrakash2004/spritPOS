<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUtlItemVsBrand
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUtlItemVsBrand))
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.fraLink = New System.Windows.Forms.GroupBox()
        Me.optItemLinked = New System.Windows.Forms.RadioButton()
        Me.optItemUnlinked = New System.Windows.Forms.RadioButton()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.lblSer = New System.Windows.Forms.Label()
        Me.txtItemSearch = New System.Windows.Forms.TextBox()
        Me.vsfItemSearch = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnReplace = New System.Windows.Forms.Button()
        Me.lblTotRec = New System.Windows.Forms.Label()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.fraLink.SuspendLayout()
        CType(Me.vsfItemSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'vsfDet
        '
        Me.vsfDet.AllowFiltering = True
        Me.vsfDet.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "2,0,0,0,0,110,Columns:"
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.Location = New System.Drawing.Point(12, 52)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.Count = 4
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(847, 490)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 5
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.Location = New System.Drawing.Point(699, 549)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(79, 30)
        Me.btnSave.TabIndex = 6
        Me.btnSave.Text = "Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Shortcut : Alt + S")
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(784, 549)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 30)
        Me.btnClose.TabIndex = 7
        Me.btnClose.Text = "Close"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Shortcut : Alt + C")
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'fraLink
        '
        Me.fraLink.Controls.Add(Me.optItemLinked)
        Me.fraLink.Controls.Add(Me.optItemUnlinked)
        Me.fraLink.Location = New System.Drawing.Point(12, 2)
        Me.fraLink.Name = "fraLink"
        Me.fraLink.Size = New System.Drawing.Size(262, 43)
        Me.fraLink.TabIndex = 0
        Me.fraLink.TabStop = False
        Me.fraLink.Text = "Brand Link"
        '
        'optItemLinked
        '
        Me.optItemLinked.AutoSize = True
        Me.optItemLinked.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optItemLinked.Location = New System.Drawing.Point(141, 13)
        Me.optItemLinked.Name = "optItemLinked"
        Me.optItemLinked.Size = New System.Drawing.Size(118, 24)
        Me.optItemLinked.TabIndex = 1
        Me.optItemLinked.TabStop = True
        Me.optItemLinked.Text = "Linked Items"
        Me.optItemLinked.UseVisualStyleBackColor = True
        '
        'optItemUnlinked
        '
        Me.optItemUnlinked.AutoSize = True
        Me.optItemUnlinked.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.optItemUnlinked.Location = New System.Drawing.Point(6, 13)
        Me.optItemUnlinked.Name = "optItemUnlinked"
        Me.optItemUnlinked.Size = New System.Drawing.Size(133, 24)
        Me.optItemUnlinked.TabIndex = 0
        Me.optItemUnlinked.TabStop = True
        Me.optItemUnlinked.Text = "Unlinked Items"
        Me.optItemUnlinked.UseVisualStyleBackColor = True
        '
        'btnLoad
        '
        Me.btnLoad.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_load
        Me.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLoad.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoad.Location = New System.Drawing.Point(280, 11)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(75, 30)
        Me.btnLoad.TabIndex = 1
        Me.btnLoad.Text = "Load"
        Me.btnLoad.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnLoad, "Shortcut : Alt + L")
        Me.btnLoad.UseVisualStyleBackColor = True
        '
        'lblSer
        '
        Me.lblSer.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSer.AutoSize = True
        Me.lblSer.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSer.ForeColor = System.Drawing.Color.Black
        Me.lblSer.Location = New System.Drawing.Point(552, 18)
        Me.lblSer.Name = "lblSer"
        Me.lblSer.Size = New System.Drawing.Size(96, 20)
        Me.lblSer.TabIndex = 2
        Me.lblSer.Tag = ""
        Me.lblSer.Text = "Search Item"
        Me.lblSer.Visible = False
        '
        'txtItemSearch
        '
        Me.txtItemSearch.AcceptsReturn = True
        Me.txtItemSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtItemSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtItemSearch.Location = New System.Drawing.Point(654, 15)
        Me.txtItemSearch.Name = "txtItemSearch"
        Me.txtItemSearch.Size = New System.Drawing.Size(205, 26)
        Me.txtItemSearch.TabIndex = 3
        Me.txtItemSearch.Visible = False
        '
        'vsfItemSearch
        '
        Me.vsfItemSearch.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfItemSearch.BackColor = System.Drawing.Color.White
        Me.vsfItemSearch.ColumnInfo = "1,0,0,0,0,110,Columns:"
        Me.vsfItemSearch.ExtendLastCol = True
        Me.vsfItemSearch.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.vsfItemSearch.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
        Me.vsfItemSearch.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfItemSearch.Location = New System.Drawing.Point(504, 47)
        Me.vsfItemSearch.Name = "vsfItemSearch"
        Me.vsfItemSearch.Rows.Count = 1
        Me.vsfItemSearch.Rows.DefaultSize = 22
        Me.vsfItemSearch.Rows.Fixed = 0
        Me.vsfItemSearch.Size = New System.Drawing.Size(355, 272)
        Me.vsfItemSearch.StyleInfo = resources.GetString("vsfItemSearch.StyleInfo")
        Me.vsfItemSearch.TabIndex = 4
        Me.vsfItemSearch.Visible = False
        '
        'btnReplace
        '
        Me.btnReplace.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnReplace.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnReplace.Location = New System.Drawing.Point(576, 549)
        Me.btnReplace.Name = "btnReplace"
        Me.btnReplace.Size = New System.Drawing.Size(115, 31)
        Me.btnReplace.TabIndex = 651
        Me.btnReplace.Text = "Replace Rest"
        Me.ToolTip1.SetToolTip(Me.btnReplace, "Shortcut: Ctrl + R")
        Me.btnReplace.UseVisualStyleBackColor = True
        '
        'lblTotRec
        '
        Me.lblTotRec.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotRec.AutoSize = True
        Me.lblTotRec.Location = New System.Drawing.Point(9, 549)
        Me.lblTotRec.Name = "lblTotRec"
        Me.lblTotRec.Size = New System.Drawing.Size(0, 13)
        Me.lblTotRec.TabIndex = 652
        '
        'frmUtlItemVsBrand
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(871, 584)
        Me.Controls.Add(Me.lblTotRec)
        Me.Controls.Add(Me.btnReplace)
        Me.Controls.Add(Me.vsfItemSearch)
        Me.Controls.Add(Me.lblSer)
        Me.Controls.Add(Me.txtItemSearch)
        Me.Controls.Add(Me.btnLoad)
        Me.Controls.Add(Me.fraLink)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.vsfDet)
        Me.Name = "frmUtlItemVsBrand"
        Me.ShowIcon = False
        Me.Text = "Item Vs Brand"
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.fraLink.ResumeLayout(False)
        Me.fraLink.PerformLayout()
        CType(Me.vsfItemSearch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnSave As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents fraLink As GroupBox
    Friend WithEvents optItemLinked As RadioButton
    Friend WithEvents optItemUnlinked As RadioButton
    Friend WithEvents btnLoad As Button
    Friend WithEvents lblSer As Label
    Friend WithEvents txtItemSearch As TextBox
    Friend WithEvents vsfItemSearch As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents btnReplace As Button
    Friend WithEvents lblTotRec As Label
End Class
