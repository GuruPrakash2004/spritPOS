<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMstPartyContactPerson
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMstPartyContactPerson))
        Me.fraEdit = New System.Windows.Forms.Panel()
        Me.btnRmvImg = New C1.Win.C1Input.C1Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtFileName = New System.Windows.Forms.TextBox()
        Me.PicBox = New System.Windows.Forms.PictureBox()
        Me.chkDefault = New System.Windows.Forms.CheckBox()
        Me.txtDept = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtPhone = New System.Windows.Forms.TextBox()
        Me.btnSave = New C1.Win.C1Input.C1Button()
        Me.btnCancel = New C1.Win.C1Input.C1Button()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtEmail = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtRemarks = New System.Windows.Forms.TextBox()
        Me.txtFax = New System.Windows.Forms.TextBox()
        Me.txtMobNo = New System.Windows.Forms.TextBox()
        Me.txtDesig = New System.Windows.Forms.TextBox()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.btnCloseEdit = New C1.Win.C1Input.C1Button()
        Me.txtCID = New System.Windows.Forms.TextBox()
        Me.txtPartyID = New System.Windows.Forms.TextBox()
        Me.txtPartyCode = New System.Windows.Forms.TextBox()
        Me.lblPartyCode = New System.Windows.Forms.Label()
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnClose = New C1.Win.C1Input.C1Button()
        Me.btnDelete = New C1.Win.C1Input.C1Button()
        Me.btnEdit = New C1.Win.C1Input.C1Button()
        Me.btnAdd = New C1.Win.C1Input.C1Button()
        Me.lblPartyName = New System.Windows.Forms.Label()
        Me.lblNoofAddress = New System.Windows.Forms.Label()
        Me.txtDocId = New System.Windows.Forms.TextBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnExport = New C1.Win.C1Input.C1Button()
        Me.btnConList = New System.Windows.Forms.Button()
        Me.fraEdit.SuspendLayout()
        CType(Me.PicBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'fraEdit
        '
        Me.fraEdit.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fraEdit.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.fraEdit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.fraEdit.Controls.Add(Me.btnRmvImg)
        Me.fraEdit.Controls.Add(Me.Label2)
        Me.fraEdit.Controls.Add(Me.txtFileName)
        Me.fraEdit.Controls.Add(Me.PicBox)
        Me.fraEdit.Controls.Add(Me.chkDefault)
        Me.fraEdit.Controls.Add(Me.txtDept)
        Me.fraEdit.Controls.Add(Me.Label1)
        Me.fraEdit.Controls.Add(Me.Label9)
        Me.fraEdit.Controls.Add(Me.txtPhone)
        Me.fraEdit.Controls.Add(Me.btnSave)
        Me.fraEdit.Controls.Add(Me.btnCancel)
        Me.fraEdit.Controls.Add(Me.Label8)
        Me.fraEdit.Controls.Add(Me.txtEmail)
        Me.fraEdit.Controls.Add(Me.Label6)
        Me.fraEdit.Controls.Add(Me.Label5)
        Me.fraEdit.Controls.Add(Me.Label4)
        Me.fraEdit.Controls.Add(Me.Label3)
        Me.fraEdit.Controls.Add(Me.txtRemarks)
        Me.fraEdit.Controls.Add(Me.txtFax)
        Me.fraEdit.Controls.Add(Me.txtMobNo)
        Me.fraEdit.Controls.Add(Me.txtDesig)
        Me.fraEdit.Controls.Add(Me.txtName)
        Me.fraEdit.Controls.Add(Me.Label7)
        Me.fraEdit.Controls.Add(Me.btnCloseEdit)
        Me.fraEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraEdit.Location = New System.Drawing.Point(39, 68)
        Me.fraEdit.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.fraEdit.Name = "fraEdit"
        Me.fraEdit.Size = New System.Drawing.Size(863, 524)
        Me.fraEdit.TabIndex = 0
        '
        'btnRmvImg
        '
        Me.btnRmvImg.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRmvImg.BackgroundImage = Global.vLQMS.My.Resources.Resources.cancel
        Me.btnRmvImg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnRmvImg.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnRmvImg.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRmvImg.Location = New System.Drawing.Point(637, 134)
        Me.btnRmvImg.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRmvImg.Name = "btnRmvImg"
        Me.btnRmvImg.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnRmvImg.Size = New System.Drawing.Size(22, 22)
        Me.btnRmvImg.TabIndex = 268
        Me.btnRmvImg.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(567, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(54, 20)
        Me.Label2.TabIndex = 270
        Me.Label2.Text = "Image"
        '
        'txtFileName
        '
        Me.txtFileName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFileName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFileName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFileName.Location = New System.Drawing.Point(676, 77)
        Me.txtFileName.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtFileName.MaxLength = 150
        Me.txtFileName.Name = "txtFileName"
        Me.txtFileName.Size = New System.Drawing.Size(142, 26)
        Me.txtFileName.TabIndex = 269
        Me.txtFileName.Visible = False
        '
        'PicBox
        '
        Me.PicBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PicBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PicBox.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PicBox.Location = New System.Drawing.Point(533, 27)
        Me.PicBox.Name = "PicBox"
        Me.PicBox.Size = New System.Drawing.Size(127, 130)
        Me.PicBox.TabIndex = 267
        Me.PicBox.TabStop = False
        '
        'chkDefault
        '
        Me.chkDefault.AutoSize = True
        Me.chkDefault.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDefault.Location = New System.Drawing.Point(179, 332)
        Me.chkDefault.Name = "chkDefault"
        Me.chkDefault.Size = New System.Drawing.Size(84, 24)
        Me.chkDefault.TabIndex = 266
        Me.chkDefault.Text = "Default "
        Me.chkDefault.UseVisualStyleBackColor = True
        '
        'txtDept
        '
        Me.txtDept.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDept.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDept.Location = New System.Drawing.Point(179, 42)
        Me.txtDept.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtDept.MaxLength = 50
        Me.txtDept.Name = "txtDept"
        Me.txtDept.Size = New System.Drawing.Size(331, 26)
        Me.txtDept.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 45)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(94, 20)
        Me.Label1.TabIndex = 209
        Me.Label1.Text = "Department"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(8, 148)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(54, 20)
        Me.Label9.TabIndex = 207
        Me.Label9.Text = "Tel No"
        '
        'txtPhone
        '
        Me.txtPhone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPhone.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPhone.Location = New System.Drawing.Point(179, 141)
        Me.txtPhone.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtPhone.MaxLength = 20
        Me.txtPhone.Name = "txtPhone"
        Me.txtPhone.Size = New System.Drawing.Size(331, 26)
        Me.txtPhone.TabIndex = 4
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_SAVE
        Me.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(550, 488)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(90, 30)
        Me.btnSave.TabIndex = 8
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnSave, "Shortcut : Alt + S")
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CANCEL
        Me.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(648, 488)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(90, 30)
        Me.btnCancel.TabIndex = 9
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCancel, "Shortcut : Alt + C")
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(8, 180)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(48, 20)
        Me.Label8.TabIndex = 45
        Me.Label8.Text = "Email"
        '
        'txtEmail
        '
        Me.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtEmail.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEmail.Location = New System.Drawing.Point(179, 174)
        Me.txtEmail.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtEmail.MaxLength = 50
        Me.txtEmail.Name = "txtEmail"
        Me.txtEmail.Size = New System.Drawing.Size(331, 26)
        Me.txtEmail.TabIndex = 5
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(8, 242)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(73, 20)
        Me.Label6.TabIndex = 41
        Me.Label6.Text = "Remarks"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(8, 213)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(35, 20)
        Me.Label5.TabIndex = 40
        Me.Label5.Text = "Fax"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(8, 115)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(79, 20)
        Me.Label4.TabIndex = 39
        Me.Label4.Text = "Mobile No"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(8, 12)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(165, 20)
        Me.Label3.TabIndex = 36
        Me.Label3.Tag = "M"
        Me.Label3.Text = "Contact Person Name"
        '
        'txtRemarks
        '
        Me.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtRemarks.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRemarks.Location = New System.Drawing.Point(179, 240)
        Me.txtRemarks.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtRemarks.MaxLength = 250
        Me.txtRemarks.Multiline = True
        Me.txtRemarks.Name = "txtRemarks"
        Me.txtRemarks.Size = New System.Drawing.Size(331, 90)
        Me.txtRemarks.TabIndex = 7
        '
        'txtFax
        '
        Me.txtFax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFax.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFax.Location = New System.Drawing.Point(179, 207)
        Me.txtFax.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtFax.MaxLength = 50
        Me.txtFax.Name = "txtFax"
        Me.txtFax.Size = New System.Drawing.Size(331, 26)
        Me.txtFax.TabIndex = 6
        '
        'txtMobNo
        '
        Me.txtMobNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMobNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMobNo.Location = New System.Drawing.Point(179, 108)
        Me.txtMobNo.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtMobNo.MaxLength = 20
        Me.txtMobNo.Name = "txtMobNo"
        Me.txtMobNo.Size = New System.Drawing.Size(331, 26)
        Me.txtMobNo.TabIndex = 3
        '
        'txtDesig
        '
        Me.txtDesig.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDesig.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDesig.Location = New System.Drawing.Point(179, 75)
        Me.txtDesig.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtDesig.MaxLength = 50
        Me.txtDesig.Name = "txtDesig"
        Me.txtDesig.Size = New System.Drawing.Size(331, 26)
        Me.txtDesig.TabIndex = 2
        '
        'txtName
        '
        Me.txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtName.Location = New System.Drawing.Point(179, 9)
        Me.txtName.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtName.MaxLength = 150
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(331, 26)
        Me.txtName.TabIndex = 0
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(8, 81)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(94, 20)
        Me.Label7.TabIndex = 29
        Me.Label7.Text = "Designation"
        '
        'btnCloseEdit
        '
        Me.btnCloseEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCloseEdit.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnCloseEdit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnCloseEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCloseEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCloseEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCloseEdit.Location = New System.Drawing.Point(746, 488)
        Me.btnCloseEdit.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCloseEdit.Name = "btnCloseEdit"
        Me.btnCloseEdit.Size = New System.Drawing.Size(90, 30)
        Me.btnCloseEdit.TabIndex = 10
        Me.btnCloseEdit.Text = "C&lose"
        Me.btnCloseEdit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnCloseEdit, "Shortcut : Alt + L ")
        Me.btnCloseEdit.UseVisualStyleBackColor = False
        Me.btnCloseEdit.Visible = False
        '
        'txtCID
        '
        Me.txtCID.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtCID.Location = New System.Drawing.Point(299, 119)
        Me.txtCID.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtCID.Name = "txtCID"
        Me.txtCID.Size = New System.Drawing.Size(25, 25)
        Me.txtCID.TabIndex = 38
        Me.txtCID.Visible = False
        '
        'txtPartyID
        '
        Me.txtPartyID.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtPartyID.Location = New System.Drawing.Point(454, 13)
        Me.txtPartyID.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtPartyID.Name = "txtPartyID"
        Me.txtPartyID.Size = New System.Drawing.Size(25, 25)
        Me.txtPartyID.TabIndex = 35
        Me.txtPartyID.Visible = False
        '
        'txtPartyCode
        '
        Me.txtPartyCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPartyCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPartyCode.Location = New System.Drawing.Point(171, 4)
        Me.txtPartyCode.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtPartyCode.MaxLength = 150
        Me.txtPartyCode.Name = "txtPartyCode"
        Me.txtPartyCode.Size = New System.Drawing.Size(331, 26)
        Me.txtPartyCode.TabIndex = 31
        '
        'lblPartyCode
        '
        Me.lblPartyCode.AutoSize = True
        Me.lblPartyCode.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPartyCode.ForeColor = System.Drawing.Color.Blue
        Me.lblPartyCode.Location = New System.Drawing.Point(24, 8)
        Me.lblPartyCode.Name = "lblPartyCode"
        Me.lblPartyCode.Size = New System.Drawing.Size(120, 20)
        Me.lblPartyCode.TabIndex = 30
        Me.lblPartyCode.Tag = "O"
        Me.lblPartyCode.Text = "Customer Code"
        '
        'vsfDet
        '
        Me.vsfDet.AllowFiltering = True
        Me.vsfDet.AllowFreezing = C1.Win.C1FlexGrid.AllowFreezingEnum.Columns
        Me.vsfDet.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfDet.AllowMergingFixed = C1.Win.C1FlexGrid.AllowMergingEnum.Free
        Me.vsfDet.AllowResizing = C1.Win.C1FlexGrid.AllowResizingEnum.None
        Me.vsfDet.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.vsfDet.AutoResize = True
        Me.vsfDet.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "2,0,0,0,0,120,Columns:"
        Me.vsfDet.EditOptions = C1.Win.C1FlexGrid.EditFlags.AutoSearch
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Heavy
        Me.vsfDet.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.vsfDet.KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
        Me.vsfDet.Location = New System.Drawing.Point(20, 85)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.DefaultSize = 24
        Me.vsfDet.Rows.GlyphRow = 0
        Me.vsfDet.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.vsfDet.Size = New System.Drawing.Size(882, 507)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 36
        Me.vsfDet.Tag = ""
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_CLOSE
        Me.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClose.Location = New System.Drawing.Point(812, 597)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(90, 30)
        Me.btnClose.TabIndex = 42
        Me.btnClose.Text = "Cl&ose"
        Me.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnClose, "Shortcut : Alt + O")
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDelete.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_DELETE
        Me.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDelete.Location = New System.Drawing.Point(716, 597)
        Me.btnDelete.Margin = New System.Windows.Forms.Padding(4)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(90, 30)
        Me.btnDelete.TabIndex = 40
        Me.btnDelete.Text = "&Delete"
        Me.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnDelete, "Shortcut : Alt + D")
        Me.btnDelete.UseVisualStyleBackColor = False
        '
        'btnEdit
        '
        Me.btnEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEdit.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_EDIT
        Me.btnEdit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEdit.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnEdit.Location = New System.Drawing.Point(620, 597)
        Me.btnEdit.Margin = New System.Windows.Forms.Padding(4)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(90, 30)
        Me.btnEdit.TabIndex = 41
        Me.btnEdit.Text = "&Edit"
        Me.btnEdit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnEdit, "Shortcut : Alt + E")
        Me.btnEdit.UseVisualStyleBackColor = False
        '
        'btnAdd
        '
        Me.btnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAdd.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_NEW
        Me.btnAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAdd.Location = New System.Drawing.Point(524, 597)
        Me.btnAdd.Margin = New System.Windows.Forms.Padding(4)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnAdd.Size = New System.Drawing.Size(90, 30)
        Me.btnAdd.TabIndex = 39
        Me.btnAdd.Text = "&Add"
        Me.btnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnAdd, "Shortcut : Alt + A")
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'lblPartyName
        '
        Me.lblPartyName.AutoSize = True
        Me.lblPartyName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPartyName.ForeColor = System.Drawing.Color.Black
        Me.lblPartyName.Location = New System.Drawing.Point(168, 33)
        Me.lblPartyName.Name = "lblPartyName"
        Me.lblPartyName.Size = New System.Drawing.Size(133, 20)
        Me.lblPartyName.TabIndex = 43
        Me.lblPartyName.Text = "Customer Code"
        '
        'lblNoofAddress
        '
        Me.lblNoofAddress.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblNoofAddress.AutoSize = True
        Me.lblNoofAddress.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNoofAddress.ForeColor = System.Drawing.Color.Blue
        Me.lblNoofAddress.Location = New System.Drawing.Point(17, 631)
        Me.lblNoofAddress.Name = "lblNoofAddress"
        Me.lblNoofAddress.Size = New System.Drawing.Size(124, 20)
        Me.lblNoofAddress.TabIndex = 209
        Me.lblNoofAddress.Text = "Customer Name"
        '
        'txtDocId
        '
        Me.txtDocId.Location = New System.Drawing.Point(569, 36)
        Me.txtDocId.Name = "txtDocId"
        Me.txtDocId.Size = New System.Drawing.Size(32, 25)
        Me.txtDocId.TabIndex = 210
        Me.txtDocId.Visible = False
        '
        'btnExport
        '
        Me.btnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExport.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExport.Image = Global.vLQMS.My.Resources.Resources.ucexport_to_XL
        Me.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExport.Location = New System.Drawing.Point(23, 598)
        Me.btnExport.Margin = New System.Windows.Forms.Padding(4)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnExport.Size = New System.Drawing.Size(36, 29)
        Me.btnExport.TabIndex = 214
        Me.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolTip1.SetToolTip(Me.btnExport, "Shortcut : Alt + X")
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnConList
        '
        Me.btnConList.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnConList.BackgroundImage = Global.vLQMS.My.Resources.Resources.T3_FIND
        Me.btnConList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnConList.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnConList.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnConList.Location = New System.Drawing.Point(66, 598)
        Me.btnConList.Name = "btnConList"
        Me.btnConList.Size = New System.Drawing.Size(36, 29)
        Me.btnConList.TabIndex = 215
        Me.ToolTip1.SetToolTip(Me.btnConList, "View All Contact Persons")
        Me.btnConList.UseVisualStyleBackColor = True
        '
        'frmMstPartyContactPerson
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(188, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(227, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(923, 648)
        Me.Controls.Add(Me.fraEdit)
        Me.Controls.Add(Me.txtDocId)
        Me.Controls.Add(Me.lblPartyName)
        Me.Controls.Add(Me.txtPartyCode)
        Me.Controls.Add(Me.txtPartyID)
        Me.Controls.Add(Me.lblPartyCode)
        Me.Controls.Add(Me.vsfDet)
        Me.Controls.Add(Me.txtCID)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnEdit)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.lblNoofAddress)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.btnConList)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MinimumSize = New System.Drawing.Size(939, 486)
        Me.Name = "frmMstPartyContactPerson"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Contact Person(s)"
        Me.fraEdit.ResumeLayout(False)
        Me.fraEdit.PerformLayout()
        CType(Me.PicBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents fraEdit As System.Windows.Forms.Panel
    Friend WithEvents txtCID As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtRemarks As System.Windows.Forms.TextBox
    Friend WithEvents txtFax As System.Windows.Forms.TextBox
    Friend WithEvents txtMobNo As System.Windows.Forms.TextBox
    Friend WithEvents txtDesig As System.Windows.Forms.TextBox
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtPartyID As System.Windows.Forms.TextBox
    Friend WithEvents txtPartyCode As System.Windows.Forms.TextBox
    Friend WithEvents lblPartyCode As System.Windows.Forms.Label
    Friend WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtEmail As System.Windows.Forms.TextBox
    Friend WithEvents btnCloseEdit As C1.Win.C1Input.C1Button
    Friend WithEvents btnSave As C1.Win.C1Input.C1Button
    Friend WithEvents btnCancel As C1.Win.C1Input.C1Button
    Friend WithEvents btnClose As C1.Win.C1Input.C1Button
    Friend WithEvents btnDelete As C1.Win.C1Input.C1Button
    Friend WithEvents btnEdit As C1.Win.C1Input.C1Button
    Friend WithEvents btnAdd As C1.Win.C1Input.C1Button
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtPhone As System.Windows.Forms.TextBox
    Friend WithEvents lblPartyName As System.Windows.Forms.Label
    Friend WithEvents lblNoofAddress As System.Windows.Forms.Label
    Friend WithEvents txtDocId As System.Windows.Forms.TextBox
    Friend WithEvents txtDept As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents btnExport As C1.Win.C1Input.C1Button
    Friend WithEvents chkDefault As CheckBox
    Friend WithEvents btnRmvImg As C1.Win.C1Input.C1Button
    Friend WithEvents PicBox As PictureBox
    Friend WithEvents txtFileName As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents btnConList As Button
End Class
