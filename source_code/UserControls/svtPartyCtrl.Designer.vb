<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class svtPartyCtrl
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(svtPartyCtrl))
        Me.lblParty = New System.Windows.Forms.Label()
        Me.txtPartyCode = New System.Windows.Forms.TextBox()
        Me.vsfView = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.cmsParty = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.tsbCopy = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlShipping = New System.Windows.Forms.Panel()
        Me.lblShp = New System.Windows.Forms.LinkLabel()
        Me.lblShipAdd2 = New System.Windows.Forms.Label()
        Me.lblShipMob = New System.Windows.Forms.Label()
        Me.lblShipPSCC = New System.Windows.Forms.Label()
        Me.lblShipAdd3 = New System.Windows.Forms.Label()
        Me.lblShipAdd1 = New System.Windows.Forms.Label()
        Me.lblShipCName = New System.Windows.Forms.Label()
        Me.lblShipAdd = New System.Windows.Forms.Label()
        Me.lblBillCName = New System.Windows.Forms.Label()
        Me.lblBillAdd1 = New System.Windows.Forms.Label()
        Me.lblBillAdd3 = New System.Windows.Forms.Label()
        Me.lblBillPSCC = New System.Windows.Forms.Label()
        Me.lblBillMob = New System.Windows.Forms.Label()
        Me.txtBillID = New System.Windows.Forms.TextBox()
        Me.lblBillAdd2 = New System.Windows.Forms.Label()
        Me.lblBillAdd = New System.Windows.Forms.Label()
        Me.pnlBilling = New System.Windows.Forms.Panel()
        Me.lblBill = New System.Windows.Forms.LinkLabel()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnMaster = New System.Windows.Forms.Button()
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmsParty.SuspendLayout()
        Me.pnlShipping.SuspendLayout()
        Me.pnlBilling.SuspendLayout()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblParty
        '
        Me.lblParty.AutoSize = True
        Me.lblParty.Font = New System.Drawing.Font("Segoe UI", 8.0!)
        Me.lblParty.ForeColor = System.Drawing.Color.Brown
        Me.lblParty.Location = New System.Drawing.Point(1, 8)
        Me.lblParty.Name = "lblParty"
        Me.lblParty.Size = New System.Drawing.Size(32, 13)
        Me.lblParty.TabIndex = 0
        Me.lblParty.Tag = "M"
        Me.lblParty.Text = "Party"
        Me.lblParty.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtPartyCode
        '
        Me.txtPartyCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPartyCode.Font = New System.Drawing.Font("Segoe UI", 8.0!)
        Me.txtPartyCode.Location = New System.Drawing.Point(104, 3)
        Me.txtPartyCode.Margin = New System.Windows.Forms.Padding(1, 1, 0, 0)
        Me.txtPartyCode.MaxLength = 150
        Me.txtPartyCode.Name = "txtPartyCode"
        Me.txtPartyCode.Size = New System.Drawing.Size(315, 22)
        Me.txtPartyCode.TabIndex = 0
        '
        'vsfView
        '
        Me.vsfView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfView.BackColor = System.Drawing.Color.WhiteSmoke
        Me.vsfView.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfView.ColumnInfo = "10,0,0,0,0,105,Columns:"
        Me.vsfView.ExtendLastCol = True
        Me.vsfView.Font = New System.Drawing.Font("Segoe UI", 8.0!)
        Me.vsfView.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
        Me.vsfView.Location = New System.Drawing.Point(0, 25)
        Me.vsfView.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfView.Name = "vsfView"
        Me.vsfView.Rows.DefaultSize = 21
        Me.vsfView.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfView.Size = New System.Drawing.Size(574, 110)
        Me.vsfView.StyleInfo = resources.GetString("vsfView.StyleInfo")
        Me.vsfView.TabIndex = 3
        Me.vsfView.Visible = False
        '
        'cmsParty
        '
        Me.cmsParty.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbCopy})
        Me.cmsParty.Name = "cmsParty"
        Me.cmsParty.Size = New System.Drawing.Size(103, 26)
        '
        'tsbCopy
        '
        Me.tsbCopy.Name = "tsbCopy"
        Me.tsbCopy.Size = New System.Drawing.Size(102, 22)
        Me.tsbCopy.Text = "Copy"
        '
        'pnlShipping
        '
        Me.pnlShipping.Controls.Add(Me.lblShp)
        Me.pnlShipping.Controls.Add(Me.lblShipAdd2)
        Me.pnlShipping.Controls.Add(Me.lblShipMob)
        Me.pnlShipping.Controls.Add(Me.lblShipPSCC)
        Me.pnlShipping.Controls.Add(Me.lblShipAdd3)
        Me.pnlShipping.Controls.Add(Me.lblShipAdd1)
        Me.pnlShipping.Controls.Add(Me.lblShipCName)
        Me.pnlShipping.Controls.Add(Me.lblShipAdd)
        Me.pnlShipping.Location = New System.Drawing.Point(291, 25)
        Me.pnlShipping.Margin = New System.Windows.Forms.Padding(0)
        Me.pnlShipping.Name = "pnlShipping"
        Me.pnlShipping.Size = New System.Drawing.Size(286, 114)
        Me.pnlShipping.TabIndex = 2
        '
        'lblShp
        '
        Me.lblShp.AutoSize = True
        Me.lblShp.DisabledLinkColor = System.Drawing.Color.Transparent
        Me.lblShp.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Italic)
        Me.lblShp.LinkColor = System.Drawing.Color.DimGray
        Me.lblShp.Location = New System.Drawing.Point(97, 3)
        Me.lblShp.Name = "lblShp"
        Me.lblShp.Size = New System.Drawing.Size(43, 13)
        Me.lblShp.TabIndex = 0
        Me.lblShp.TabStop = True
        Me.lblShp.Text = "Change"
        '
        'lblShipAdd2
        '
        Me.lblShipAdd2.AutoSize = True
        Me.lblShipAdd2.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShipAdd2.Location = New System.Drawing.Point(4, 42)
        Me.lblShipAdd2.Name = "lblShipAdd2"
        Me.lblShipAdd2.Size = New System.Drawing.Size(32, 13)
        Me.lblShipAdd2.TabIndex = 14
        Me.lblShipAdd2.Text = "Add2"
        '
        'lblShipMob
        '
        Me.lblShipMob.AutoSize = True
        Me.lblShipMob.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShipMob.Location = New System.Drawing.Point(4, 84)
        Me.lblShipMob.Name = "lblShipMob"
        Me.lblShipMob.Size = New System.Drawing.Size(138, 13)
        Me.lblShipMob.TabIndex = 11
        Me.lblShipMob.Text = "Contact Person - Mobile No"
        '
        'lblShipPSCC
        '
        Me.lblShipPSCC.AutoSize = True
        Me.lblShipPSCC.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShipPSCC.Location = New System.Drawing.Point(4, 70)
        Me.lblShipPSCC.Name = "lblShipPSCC"
        Me.lblShipPSCC.Size = New System.Drawing.Size(150, 13)
        Me.lblShipPSCC.TabIndex = 10
        Me.lblShipPSCC.Text = "City - Pincode; State - Country"
        '
        'lblShipAdd3
        '
        Me.lblShipAdd3.AutoSize = True
        Me.lblShipAdd3.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShipAdd3.Location = New System.Drawing.Point(4, 56)
        Me.lblShipAdd3.Name = "lblShipAdd3"
        Me.lblShipAdd3.Size = New System.Drawing.Size(71, 13)
        Me.lblShipAdd3.TabIndex = 9
        Me.lblShipAdd3.Text = "Add3 && Add4"
        '
        'lblShipAdd1
        '
        Me.lblShipAdd1.AutoSize = True
        Me.lblShipAdd1.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShipAdd1.Location = New System.Drawing.Point(4, 28)
        Me.lblShipAdd1.Name = "lblShipAdd1"
        Me.lblShipAdd1.Size = New System.Drawing.Size(32, 13)
        Me.lblShipAdd1.TabIndex = 8
        Me.lblShipAdd1.Text = "Add1"
        '
        'lblShipCName
        '
        Me.lblShipCName.AutoSize = True
        Me.lblShipCName.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShipCName.Location = New System.Drawing.Point(2, 15)
        Me.lblShipCName.Name = "lblShipCName"
        Me.lblShipCName.Size = New System.Drawing.Size(83, 13)
        Me.lblShipCName.TabIndex = 7
        Me.lblShipCName.Text = "Customer Name"
        '
        'lblShipAdd
        '
        Me.lblShipAdd.AutoSize = True
        Me.lblShipAdd.Font = New System.Drawing.Font("Segoe UI", 8.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShipAdd.Location = New System.Drawing.Point(3, 2)
        Me.lblShipAdd.Name = "lblShipAdd"
        Me.lblShipAdd.Size = New System.Drawing.Size(99, 13)
        Me.lblShipAdd.TabIndex = 6
        Me.lblShipAdd.Text = "Shipping Address"
        '
        'lblBillCName
        '
        Me.lblBillCName.AutoSize = True
        Me.lblBillCName.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBillCName.Location = New System.Drawing.Point(3, 15)
        Me.lblBillCName.Name = "lblBillCName"
        Me.lblBillCName.Size = New System.Drawing.Size(83, 13)
        Me.lblBillCName.TabIndex = 1
        Me.lblBillCName.Text = "Customer Name"
        '
        'lblBillAdd1
        '
        Me.lblBillAdd1.AutoSize = True
        Me.lblBillAdd1.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBillAdd1.Location = New System.Drawing.Point(3, 29)
        Me.lblBillAdd1.Name = "lblBillAdd1"
        Me.lblBillAdd1.Size = New System.Drawing.Size(32, 13)
        Me.lblBillAdd1.TabIndex = 2
        Me.lblBillAdd1.Text = "Add1"
        '
        'lblBillAdd3
        '
        Me.lblBillAdd3.AutoSize = True
        Me.lblBillAdd3.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBillAdd3.Location = New System.Drawing.Point(3, 57)
        Me.lblBillAdd3.Name = "lblBillAdd3"
        Me.lblBillAdd3.Size = New System.Drawing.Size(71, 13)
        Me.lblBillAdd3.TabIndex = 3
        Me.lblBillAdd3.Text = "Add3 && Add4"
        '
        'lblBillPSCC
        '
        Me.lblBillPSCC.AutoSize = True
        Me.lblBillPSCC.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBillPSCC.Location = New System.Drawing.Point(3, 72)
        Me.lblBillPSCC.Name = "lblBillPSCC"
        Me.lblBillPSCC.Size = New System.Drawing.Size(150, 13)
        Me.lblBillPSCC.TabIndex = 4
        Me.lblBillPSCC.Text = "City - Pincode; State - Country"
        '
        'lblBillMob
        '
        Me.lblBillMob.AutoSize = True
        Me.lblBillMob.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBillMob.Location = New System.Drawing.Point(3, 85)
        Me.lblBillMob.Name = "lblBillMob"
        Me.lblBillMob.Size = New System.Drawing.Size(138, 13)
        Me.lblBillMob.TabIndex = 5
        Me.lblBillMob.Text = "Contact Person - Mobile No"
        '
        'txtBillID
        '
        Me.txtBillID.Location = New System.Drawing.Point(206, 41)
        Me.txtBillID.Name = "txtBillID"
        Me.txtBillID.Size = New System.Drawing.Size(24, 20)
        Me.txtBillID.TabIndex = 6
        Me.txtBillID.Visible = False
        '
        'lblBillAdd2
        '
        Me.lblBillAdd2.AutoSize = True
        Me.lblBillAdd2.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBillAdd2.Location = New System.Drawing.Point(3, 43)
        Me.lblBillAdd2.Name = "lblBillAdd2"
        Me.lblBillAdd2.Size = New System.Drawing.Size(32, 13)
        Me.lblBillAdd2.TabIndex = 8
        Me.lblBillAdd2.Text = "Add2"
        '
        'lblBillAdd
        '
        Me.lblBillAdd.AutoSize = True
        Me.lblBillAdd.Font = New System.Drawing.Font("Segoe UI", 8.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBillAdd.Location = New System.Drawing.Point(3, 2)
        Me.lblBillAdd.Name = "lblBillAdd"
        Me.lblBillAdd.Size = New System.Drawing.Size(85, 13)
        Me.lblBillAdd.TabIndex = 0
        Me.lblBillAdd.Text = "Billing Address"
        '
        'pnlBilling
        '
        Me.pnlBilling.Controls.Add(Me.lblBill)
        Me.pnlBilling.Controls.Add(Me.vsfDet)
        Me.pnlBilling.Controls.Add(Me.lblBillAdd)
        Me.pnlBilling.Controls.Add(Me.lblBillAdd2)
        Me.pnlBilling.Controls.Add(Me.txtBillID)
        Me.pnlBilling.Controls.Add(Me.lblBillMob)
        Me.pnlBilling.Controls.Add(Me.lblBillPSCC)
        Me.pnlBilling.Controls.Add(Me.lblBillAdd3)
        Me.pnlBilling.Controls.Add(Me.lblBillAdd1)
        Me.pnlBilling.Controls.Add(Me.lblBillCName)
        Me.pnlBilling.Location = New System.Drawing.Point(0, 25)
        Me.pnlBilling.Margin = New System.Windows.Forms.Padding(0)
        Me.pnlBilling.Name = "pnlBilling"
        Me.pnlBilling.Size = New System.Drawing.Size(290, 114)
        Me.pnlBilling.TabIndex = 1
        '
        'lblBill
        '
        Me.lblBill.AutoSize = True
        Me.lblBill.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Italic)
        Me.lblBill.LinkColor = System.Drawing.Color.DimGray
        Me.lblBill.Location = New System.Drawing.Point(81, 3)
        Me.lblBill.Name = "lblBill"
        Me.lblBill.Size = New System.Drawing.Size(43, 13)
        Me.lblBill.TabIndex = 0
        Me.lblBill.TabStop = True
        Me.lblBill.Text = "Change"
        '
        'vsfDet
        '
        Me.vsfDet.BackColor = System.Drawing.Color.WhiteSmoke
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "16,0,0,0,0,105,Columns:"
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 8.0!)
        Me.vsfDet.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
        Me.vsfDet.Location = New System.Drawing.Point(238, 15)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.Count = 2
        Me.vsfDet.Rows.DefaultSize = 21
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(48, 21)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 257
        Me.vsfDet.Visible = False
        '
        'btnMaster
        '
        Me.btnMaster.BackColor = System.Drawing.Color.White
        Me.btnMaster.BackgroundImage = CType(resources.GetObject("btnMaster.BackgroundImage"), System.Drawing.Image)
        Me.btnMaster.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.btnMaster.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btnMaster.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMaster.Font = New System.Drawing.Font("Segoe UI", 6.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle))
        Me.btnMaster.ForeColor = System.Drawing.Color.Transparent
        Me.btnMaster.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnMaster.Location = New System.Drawing.Point(82, 3)
        Me.btnMaster.Name = "btnMaster"
        Me.btnMaster.Size = New System.Drawing.Size(23, 18)
        Me.btnMaster.TabIndex = 259
        Me.btnMaster.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.btnMaster.UseVisualStyleBackColor = False
        '
        'svtPartyCtrl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me.btnMaster)
        Me.Controls.Add(Me.vsfView)
        Me.Controls.Add(Me.txtPartyCode)
        Me.Controls.Add(Me.pnlShipping)
        Me.Controls.Add(Me.lblParty)
        Me.Controls.Add(Me.pnlBilling)
        Me.Name = "svtPartyCtrl"
        Me.Size = New System.Drawing.Size(577, 143)
        CType(Me.vsfView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmsParty.ResumeLayout(False)
        Me.pnlShipping.ResumeLayout(False)
        Me.pnlShipping.PerformLayout()
        Me.pnlBilling.ResumeLayout(False)
        Me.pnlBilling.PerformLayout()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblParty As System.Windows.Forms.Label
    Friend WithEvents txtPartyCode As System.Windows.Forms.TextBox
    Friend WithEvents vsfView As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents cmsParty As ContextMenuStrip
    Friend WithEvents tsbCopy As ToolStripMenuItem
    Friend WithEvents pnlShipping As Panel
    Friend WithEvents lblShipMob As Label
    Friend WithEvents lblShipPSCC As Label
    Friend WithEvents lblShipAdd3 As Label
    Friend WithEvents lblShipAdd1 As Label
    Friend WithEvents lblShipCName As Label
    Friend WithEvents lblShipAdd As Label
    Friend WithEvents lblShipAdd2 As Label
    Friend WithEvents lblBillCName As Label
    Friend WithEvents lblBillAdd1 As Label
    Friend WithEvents lblBillAdd3 As Label
    Friend WithEvents lblBillPSCC As Label
    Friend WithEvents lblBillMob As Label
    Friend WithEvents txtBillID As TextBox
    Friend WithEvents lblBillAdd2 As Label
    Friend WithEvents lblBillAdd As Label
    Friend WithEvents pnlBilling As Panel
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents btnMaster As Button
    Friend WithEvents lblShp As LinkLabel
    Friend WithEvents lblBill As LinkLabel
End Class
