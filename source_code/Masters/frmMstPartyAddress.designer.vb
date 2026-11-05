<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMstPartyAddress
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMstPartyAddress))
        Me.lblPartyCode = New System.Windows.Forms.Label()
        Me.txtPartyCode = New System.Windows.Forms.TextBox()
        Me.txtPartyName = New System.Windows.Forms.TextBox()
        Me.fraEdit = New System.Windows.Forms.Panel()
        Me.ChkDefShipAd = New System.Windows.Forms.CheckBox()
        Me.cboCountry = New System.Windows.Forms.ComboBox()
        Me.cboState = New System.Windows.Forms.ComboBox()
        Me.cboCity = New System.Windows.Forms.ComboBox()
        Me.txtAdd4 = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtPincode = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnSave = New C1.Win.C1Input.C1Button()
        Me.btnCancel = New C1.Win.C1Input.C1Button()
        Me.txtAddID = New System.Windows.Forms.TextBox()
        Me.txtAddCat = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtAdd3 = New System.Windows.Forms.TextBox()
        Me.txtAdd2 = New System.Windows.Forms.TextBox()
        Me.txtAdd1 = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtPartyID = New System.Windows.Forms.TextBox()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.lblPartyName = New System.Windows.Forms.Label()
        Me.lblNoofAddress = New System.Windows.Forms.Label()
        Me.txtDocId = New System.Windows.Forms.TextBox()
        Me.txtCID = New System.Windows.Forms.TextBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.btnDelete = New C1.Win.C1Input.C1Button()
        Me.btnAdd = New C1.Win.C1Input.C1Button()
        Me.btnEdit = New C1.Win.C1Input.C1Button()
        Me.btnExport = New C1.Win.C1Input.C1Button()
        Me.btnAddList = New System.Windows.Forms.Button()
        Me.fraEdit.SuspendLayout()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblPartyCode
        '
        Me.lblPartyCode.AutoSize = True
        Me.lblPartyCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPartyCode.ForeColor = System.Drawing.Color.Blue
        Me.lblPartyCode.Location = New System.Drawing.Point(24, 15)
        Me.lblPartyCode.Name = "lblPartyCode"
        Me.lblPartyCode.Size = New System.Drawing.Size(120, 20)
        Me.lblPartyCode.TabIndex = 19
        Me.lblPartyCode.Tag = "O"
        Me.lblPartyCode.Text = "Customer Code"
        '
        'txtPartyCode
        '
        Me.txtPartyCode.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPartyCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPartyCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPartyCode.Location = New System.Drawing.Point(143, 12)
        Me.txtPartyCode.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtPartyCode.MaxLength = 150
        Me.txtPartyCode.Name = "txtPartyCode"
        Me.txtPartyCode.Size = New System.Drawing.Size(700, 26)
        Me.txtPartyCode.TabIndex = 0
        '
        'txtPartyName
        '
        Me.txtPartyName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPartyName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPartyName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPartyName.Location = New System.Drawing.Point(767, 41)
        Me.txtPartyName.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtPartyName.MaxLength = 150
        Me.txtPartyName.Name = "txtPartyName"
        Me.txtPartyName.Size = New System.Drawing.Size(102, 26)
        Me.txtPartyName.TabIndex = 1
        Me.txtPartyName.Visible = False
        '
        'fraEdit
        '
        Me.fraEdit.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fraEdit.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.fraEdit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.fraEdit.Controls.Add(Me.ChkDefShipAd)
        Me.fraEdit.Controls.Add(Me.cboCountry)
        Me.fraEdit.Controls.Add(Me.cboState)
        Me.fraEdit.Controls.Add(Me.cboCity)
        Me.fraEdit.Controls.Add(Me.txtAdd4)
        Me.fraEdit.Controls.Add(Me.Label5)
        Me.fraEdit.Controls.Add(Me.Label4)
        Me.fraEdit.Controls.Add(Me.txtPincode)
        Me.fraEdit.Controls.Add(Me.Label2)
        Me.fraEdit.Controls.Add(Me.Label1)
        Me.fraEdit.Controls.Add(Me.btnSave)
        Me.fraEdit.Controls.Add(Me.btnCancel)
        Me.fraEdit.Controls.Add(Me.txtAddID)
        Me.fraEdit.Controls.Add(Me.txtAddCat)
        Me.fraEdit.Controls.Add(Me.Label3)
        Me.fraEdit.Controls.Add(Me.txtAdd3)
        Me.fraEdit.Controls.Add(Me.txtAdd2)
        Me.fraEdit.Controls.Add(Me.txtAdd1)
        Me.fraEdit.Controls.Add(Me.Label7)
        Me.fraEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraEdit.Location = New System.Drawing.Point(12, 75)
        Me.fraEdit.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.fraEdit.Name = "fraEdit"
        Me.fraEdit.Size = New System.Drawing.Size(849, 478)
        Me.fraEdit.TabIndex = 4
        '
        'ChkDefShipAd
        '
        Me.ChkDefShipAd.AutoSize = True
        Me.ChkDefShipAd.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkDefShipAd.Location = New System.Drawing.Point(110, 316)
        Me.ChkDefShipAd.Name = "ChkDefShipAd"
        Me.ChkDefShipAd.Size = New System.Drawing.Size(179, 24)
        Me.ChkDefShipAd.TabIndex = 215
        Me.ChkDefShipAd.Text = "Default Ship Address"
        Me.ChkDefShipAd.UseVisualStyleBackColor = True
        '
        'cboCountry
        '
        Me.cboCountry.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cboCountry.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cboCountry.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCountry.FormattingEnabled = True
        Me.cboCountry.IntegralHeight = False
        Me.cboCountry.Items.AddRange(New Object() {""})
        Me.cboCountry.Location = New System.Drawing.Point(127, 262)
        Me.cboCountry.Margin = New System.Windows.Forms.Padding(4)
        Me.cboCountry.Name = "cboCountry"
        Me.cboCountry.Size = New System.Drawing.Size(145, 28)
        Me.cboCountry.TabIndex = 8
        Me.cboCountry.Tag = "M"
        '
        'cboState
        '
        Me.cboState.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cboState.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cboState.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboState.FormattingEnabled = True
        Me.cboState.IntegralHeight = False
        Me.cboState.Location = New System.Drawing.Point(127, 197)
        Me.cboState.Margin = New System.Windows.Forms.Padding(4)
        Me.cboState.Name = "cboState"
        Me.cboState.Size = New System.Drawing.Size(145, 28)
        Me.cboState.TabIndex = 6
        Me.cboState.Tag = "M"
        Me.cboState.Text = "KARNATAKA"
        '
        'cboCity
        '
        Me.cboCity.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.cboCity.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cboCity.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCity.FormattingEnabled = True
        Me.cboCity.IntegralHeight = False
        Me.cboCity.Location = New System.Drawing.Point(127, 163)
        Me.cboCity.Margin = New System.Windows.Forms.Padding(4)
        Me.cboCity.Name = "cboCity"
        Me.cboCity.Size = New System.Drawing.Size(145, 28)
        Me.cboCity.TabIndex = 5
        Me.cboCity.Tag = "M"
        '
        'txtAdd4
        '
        Me.txtAdd4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAdd4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAdd4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdd4.Location = New System.Drawing.Point(127, 132)
        Me.txtAdd4.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtAdd4.MaxLength = 50
        Me.txtAdd4.Name = "txtAdd4"
        Me.txtAdd4.Size = New System.Drawing.Size(703, 26)
        Me.txtAdd4.TabIndex = 4
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Red
        Me.Label5.Location = New System.Drawing.Point(8, 265)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(64, 20)
        Me.Label5.TabIndex = 214
        Me.Label5.Text = "Country"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Black
        Me.Label4.Location = New System.Drawing.Point(8, 232)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(69, 20)
        Me.Label4.TabIndex = 212
        Me.Label4.Text = "PinCode"
        '
        'txtPincode
        '
        Me.txtPincode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPincode.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPincode.Location = New System.Drawing.Point(127, 229)
        Me.txtPincode.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtPincode.MaxLength = 6
        Me.txtPincode.Name = "txtPincode"
        Me.txtPincode.Size = New System.Drawing.Size(155, 26)
        Me.txtPincode.TabIndex = 7
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Red
        Me.Label2.Location = New System.Drawing.Point(8, 201)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(48, 20)
        Me.Label2.TabIndex = 210
        Me.Label2.Text = "State"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Red
        Me.Label1.Location = New System.Drawing.Point(8, 167)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(35, 20)
        Me.Label1.TabIndex = 209
        Me.Label1.Text = "City"
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(658, 437)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(90, 30)
        Me.btnSave.TabIndex = 10
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Shortcut : Alt + S")
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(754, 437)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(90, 30)
        Me.btnCancel.TabIndex = 11
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Shortcut : Alt + C")
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'txtAddID
        '
        Me.txtAddID.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtAddID.Location = New System.Drawing.Point(464, 9)
        Me.txtAddID.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtAddID.Name = "txtAddID"
        Me.txtAddID.Size = New System.Drawing.Size(10, 25)
        Me.txtAddID.TabIndex = 38
        Me.txtAddID.Visible = False
        '
        'txtAddCat
        '
        Me.txtAddCat.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAddCat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAddCat.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddCat.Location = New System.Drawing.Point(127, 8)
        Me.txtAddCat.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtAddCat.MaxLength = 50
        Me.txtAddCat.Name = "txtAddCat"
        Me.txtAddCat.Size = New System.Drawing.Size(703, 26)
        Me.txtAddCat.TabIndex = 0
        '
        'Label3
        '
        Me.Label3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Red
        Me.Label3.Location = New System.Drawing.Point(8, 11)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(73, 20)
        Me.Label3.TabIndex = 36
        Me.Label3.Tag = "M"
        Me.Label3.Text = "Category"
        '
        'txtAdd3
        '
        Me.txtAdd3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAdd3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAdd3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdd3.Location = New System.Drawing.Point(127, 101)
        Me.txtAdd3.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtAdd3.MaxLength = 50
        Me.txtAdd3.Name = "txtAdd3"
        Me.txtAdd3.Size = New System.Drawing.Size(703, 26)
        Me.txtAdd3.TabIndex = 3
        '
        'txtAdd2
        '
        Me.txtAdd2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAdd2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAdd2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdd2.Location = New System.Drawing.Point(127, 70)
        Me.txtAdd2.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtAdd2.MaxLength = 50
        Me.txtAdd2.Name = "txtAdd2"
        Me.txtAdd2.Size = New System.Drawing.Size(703, 26)
        Me.txtAdd2.TabIndex = 2
        '
        'txtAdd1
        '
        Me.txtAdd1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAdd1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAdd1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAdd1.Location = New System.Drawing.Point(127, 39)
        Me.txtAdd1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtAdd1.MaxLength = 50
        Me.txtAdd1.Name = "txtAdd1"
        Me.txtAdd1.Size = New System.Drawing.Size(703, 26)
        Me.txtAdd1.TabIndex = 1
        '
        'Label7
        '
        Me.Label7.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Red
        Me.Label7.Location = New System.Drawing.Point(8, 42)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(68, 20)
        Me.Label7.TabIndex = 29
        Me.Label7.Tag = "M"
        Me.Label7.Text = "Address"
        '
        'txtPartyID
        '
        Me.txtPartyID.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtPartyID.Location = New System.Drawing.Point(811, 93)
        Me.txtPartyID.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtPartyID.Name = "txtPartyID"
        Me.txtPartyID.Size = New System.Drawing.Size(25, 25)
        Me.txtPartyID.TabIndex = 24
        Me.txtPartyID.Visible = False
        '
        'vsfDet
        '
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "2,0,0,0,0,120,Columns:0{Visible:False;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.vsfDet.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfDet.Location = New System.Drawing.Point(15, 75)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.Rows.GlyphRow = 0
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(842, 419)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 2
        Me.vsfDet.Tag = ""
        '
        'lblPartyName
        '
        Me.lblPartyName.AutoSize = True
        Me.lblPartyName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPartyName.ForeColor = System.Drawing.Color.Black
        Me.lblPartyName.Location = New System.Drawing.Point(142, 52)
        Me.lblPartyName.Name = "lblPartyName"
        Me.lblPartyName.Size = New System.Drawing.Size(137, 20)
        Me.lblPartyName.TabIndex = 207
        Me.lblPartyName.Text = "Customer Name"
        '
        'lblNoofAddress
        '
        Me.lblNoofAddress.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblNoofAddress.AutoSize = True
        Me.lblNoofAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNoofAddress.ForeColor = System.Drawing.Color.Blue
        Me.lblNoofAddress.Location = New System.Drawing.Point(12, 533)
        Me.lblNoofAddress.Name = "lblNoofAddress"
        Me.lblNoofAddress.Size = New System.Drawing.Size(124, 20)
        Me.lblNoofAddress.TabIndex = 208
        Me.lblNoofAddress.Text = "Customer Name"
        '
        'txtDocId
        '
        Me.txtDocId.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDocId.Location = New System.Drawing.Point(829, 94)
        Me.txtDocId.Name = "txtDocId"
        Me.txtDocId.Size = New System.Drawing.Size(32, 26)
        Me.txtDocId.TabIndex = 211
        Me.txtDocId.Visible = False
        '
        'txtCID
        '
        Me.txtCID.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCID.Location = New System.Drawing.Point(829, 62)
        Me.txtCID.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtCID.Name = "txtCID"
        Me.txtCID.Size = New System.Drawing.Size(25, 26)
        Me.txtCID.TabIndex = 212
        Me.txtCID.Visible = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(769, 499)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(90, 30)
        Me.btnClose.TabIndex = 206
        Me.btnClose.Text = "C&lose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Shortcut : Alt + L")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDelete.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_DELETE
        Me.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDelete.Location = New System.Drawing.Point(673, 499)
        Me.btnDelete.Margin = New System.Windows.Forms.Padding(4)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(90, 30)
        Me.btnDelete.TabIndex = 204
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnDelete, "Shortcut : Alt + D")
        Me.btnDelete.UseVisualStyleBackColor = False
        '
        'btnAdd
        '
        Me.btnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAdd.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_NEW
        Me.btnAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAdd.Location = New System.Drawing.Point(481, 499)
        Me.btnAdd.Margin = New System.Windows.Forms.Padding(4)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnAdd.Size = New System.Drawing.Size(90, 30)
        Me.btnAdd.TabIndex = 203
        Me.btnAdd.Text = "&Add"
        Me.btnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnAdd, "Shortcut : Alt + A")
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnEdit
        '
        Me.btnEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEdit.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_EDIT
        Me.btnEdit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnEdit.Location = New System.Drawing.Point(577, 499)
        Me.btnEdit.Margin = New System.Windows.Forms.Padding(4)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(90, 30)
        Me.btnEdit.TabIndex = 205
        Me.btnEdit.Text = "&Edit"
        Me.btnEdit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnEdit, "Shortcut : Alt + E")
        Me.btnEdit.UseVisualStyleBackColor = False
        '
        'btnExport
        '
        Me.btnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExport.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExport.Image = Global.vLQMS.My.Resources.Resources.ucexport_to_XL
        Me.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExport.Location = New System.Drawing.Point(15, 499)
        Me.btnExport.Margin = New System.Windows.Forms.Padding(4)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnExport.Size = New System.Drawing.Size(38, 29)
        Me.btnExport.TabIndex = 213
        Me.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnExport, "Shortcut : Alt + X")
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnAddList
        '
        Me.btnAddList.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAddList.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_FIND
        Me.btnAddList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnAddList.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAddList.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAddList.Location = New System.Drawing.Point(60, 499)
        Me.btnAddList.Name = "btnAddList"
        Me.btnAddList.Size = New System.Drawing.Size(36, 29)
        Me.btnAddList.TabIndex = 216
        Me.ToolTip1.SetToolTip(Me.btnAddList, "View All Addresses")
        Me.btnAddList.UseVisualStyleBackColor = True
        '
        'frmMstPartyAddress
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(862, 558)
        Me.Controls.Add(Me.txtCID)
        Me.Controls.Add(Me.txtDocId)
        Me.Controls.Add(Me.lblPartyName)
        Me.Controls.Add(Me.txtPartyID)
        Me.Controls.Add(Me.txtPartyName)
        Me.Controls.Add(Me.txtPartyCode)
        Me.Controls.Add(Me.lblPartyCode)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.btnEdit)
        Me.Controls.Add(Me.lblNoofAddress)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.btnAddList)
        Me.Controls.Add(Me.fraEdit)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MinimumSize = New System.Drawing.Size(540, 426)
        Me.Name = "frmMstPartyAddress"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Party Address"
        Me.fraEdit.ResumeLayout(False)
        Me.fraEdit.PerformLayout()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblPartyCode As System.Windows.Forms.Label
    Friend WithEvents txtPartyCode As System.Windows.Forms.TextBox
    Friend WithEvents txtPartyName As System.Windows.Forms.TextBox
    Friend WithEvents fraEdit As System.Windows.Forms.Panel
    Friend WithEvents txtAdd3 As System.Windows.Forms.TextBox
    Friend WithEvents txtAdd2 As System.Windows.Forms.TextBox
    Friend WithEvents txtAdd1 As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtAddCat As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtPartyID As System.Windows.Forms.TextBox
    Friend WithEvents txtAddID As System.Windows.Forms.TextBox
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents btnDelete As C1.Win.C1Input.C1Button
    Friend WithEvents btnAdd As C1.Win.C1Input.C1Button
    Friend WithEvents btnEdit As C1.Win.C1Input.C1Button
    Friend WithEvents btnSave As C1.Win.C1Input.C1Button
    Friend WithEvents btnCancel As C1.Win.C1Input.C1Button
    Friend WithEvents lblPartyName As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblNoofAddress As System.Windows.Forms.Label
    Friend WithEvents txtDocId As System.Windows.Forms.TextBox
    Friend WithEvents txtCID As System.Windows.Forms.TextBox
    Friend WithEvents txtAdd4 As TextBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents btnExport As C1.Win.C1Input.C1Button
    Friend WithEvents txtPincode As TextBox
    Friend WithEvents cboCity As ComboBox
    Friend WithEvents cboState As ComboBox
    Friend WithEvents cboCountry As ComboBox
    Friend WithEvents btnAddList As Button
    Friend WithEvents ChkDefShipAd As CheckBox
End Class
