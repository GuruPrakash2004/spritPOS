<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class svtFileCtrl
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(svtFileCtrl))
        Me.vsfDet = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnAddImg = New C1.Win.C1Input.C1SplitButton()
        Me.srcWebCam = New C1.Win.C1Input.DropDownItem()
        Me.img = New C1.Win.C1Input.C1PictureBox()
        Me.btnRemoveImg = New System.Windows.Forms.Button()
        Me.lblPreview = New System.Windows.Forms.Label()
        Me.ofd = New System.Windows.Forms.OpenFileDialog()
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.img, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'vsfDet
        '
        Me.vsfDet.AllowResizing = C1.Win.C1FlexGrid.AllowResizingEnum.None
        Me.vsfDet.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.vsfDet.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
        Me.vsfDet.ColumnInfo = "1,0,0,0,0,95,Columns:"
        Me.vsfDet.ExtendLastCol = True
        Me.vsfDet.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.vsfDet.Location = New System.Drawing.Point(88, 0)
        Me.vsfDet.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.vsfDet.Name = "vsfDet"
        Me.vsfDet.Rows.Count = 4
        Me.vsfDet.Rows.DefaultSize = 19
        Me.vsfDet.Size = New System.Drawing.Size(203, 74)
        Me.vsfDet.StyleInfo = resources.GetString("vsfDet.StyleInfo")
        Me.vsfDet.TabIndex = 1
        '
        'btnAddImg
        '
        Me.btnAddImg.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnAddImg.ForeColor = System.Drawing.Color.Black
        Me.btnAddImg.Image = CType(resources.GetObject("btnAddImg.Image"), System.Drawing.Image)
        Me.btnAddImg.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAddImg.Items.Add(Me.srcWebCam)
        Me.btnAddImg.Location = New System.Drawing.Point(135, 76)
        Me.btnAddImg.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnAddImg.Name = "btnAddImg"
        Me.btnAddImg.Size = New System.Drawing.Size(136, 31)
        Me.btnAddImg.TabIndex = 21
        Me.btnAddImg.Text = "From File..."
        Me.btnAddImg.UseVisualStyleBackColor = False
        '
        'srcWebCam
        '
        Me.srcWebCam.Text = "From WebCam"
        '
        'img
        '
        Me.img.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.img.Location = New System.Drawing.Point(-1, 0)
        Me.img.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.img.Name = "img"
        Me.img.Size = New System.Drawing.Size(90, 74)
        Me.img.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.img.TabIndex = 20
        Me.img.TabStop = False
        '
        'btnRemoveImg
        '
        Me.btnRemoveImg.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.btnRemoveImg.BackgroundImage = CType(resources.GetObject("btnRemoveImg.BackgroundImage"), System.Drawing.Image)
        Me.btnRemoveImg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnRemoveImg.Location = New System.Drawing.Point(5, 77)
        Me.btnRemoveImg.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnRemoveImg.Name = "btnRemoveImg"
        Me.btnRemoveImg.Size = New System.Drawing.Size(40, 27)
        Me.btnRemoveImg.TabIndex = 19
        Me.btnRemoveImg.UseVisualStyleBackColor = False
        '
        'lblPreview
        '
        Me.lblPreview.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.lblPreview.Location = New System.Drawing.Point(0, 8)
        Me.lblPreview.Name = "lblPreview"
        Me.lblPreview.Size = New System.Drawing.Size(87, 55)
        Me.lblPreview.TabIndex = 22
        Me.lblPreview.Text = "Preview Not Available"
        Me.lblPreview.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ofd
        '
        Me.ofd.FileName = "OpenFileDialog1"
        '
        'svtFileCtrl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(233, Byte), Integer), CType(CType(237, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.lblPreview)
        Me.Controls.Add(Me.btnAddImg)
        Me.Controls.Add(Me.img)
        Me.Controls.Add(Me.btnRemoveImg)
        Me.Controls.Add(Me.vsfDet)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximumSize = New System.Drawing.Size(290, 108)
        Me.MinimumSize = New System.Drawing.Size(290, 108)
        Me.Name = "svtFileCtrl"
        Me.Size = New System.Drawing.Size(288, 106)
        CType(Me.vsfDet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.img, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnAddImg As C1.Win.C1Input.C1SplitButton
    Friend WithEvents srcWebCam As C1.Win.C1Input.DropDownItem
    Friend WithEvents img As C1.Win.C1Input.C1PictureBox
    Friend WithEvents btnRemoveImg As System.Windows.Forms.Button
    Friend WithEvents lblPreview As System.Windows.Forms.Label
    Friend WithEvents ofd As System.Windows.Forms.OpenFileDialog
    Public WithEvents vsfDet As C1.Win.C1FlexGrid.C1FlexGrid

End Class
