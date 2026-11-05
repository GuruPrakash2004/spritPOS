Imports C1.Win.C1FlexGrid
Public Class frmUtlBomShowRev
    Dim clsSVT As New svtLibrary
    Public intBOMItemID As Integer
    Const vDetSlno As Byte = 0
    Const vDetItemName As Byte = 1

    Const vEffID = 0
    Const vEffFrom = 1
    Const vEffTo = 2

    Dim strRights As String
    Public argFrm As Object
    Private Sub btnClose_Click(sender As Object, e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
    Private Sub markChange()
        Dim i As Integer, j As Integer
        Dim sngQty As Double, blnChanged As Boolean
        With vsfDet
            For i = 1 To .Rows.Count - 1
                sngQty = .GetData(i, 2)
                blnChanged = False
                For j = 3 To .Cols.Count - 1
                    If sngQty <> .GetData(i, j) Then
                        SetFlexBackColor(vsfDet, "M", i)
                        '.Cell(flexcpFloodColor, i, vDetSlno) = vbRed
                        '.Cell(flexcpFloodPercent, i, vDetSlno) = 30
                        blnChanged = True
                        Exit For
                    End If
                Next j
                If Not blnChanged Then
                    SetFlexBackColor(vsfDet, "", i)
                    '.Cell(flexcpFloodColor, i, vDetSlno) = vbGreen
                    '.Cell(flexcpFloodPercent, i, vDetSlno) = 30
                End If
            Next i
        End With
    End Sub
    
 
    Public Sub InitScreen()
        With vsfDet
            .Rows.Count = 2
            .Cols.Count = 5
            .SetData(0, vDetItemName, "Item Description")
            .SetData(0, vDetSlno, "SlNo")
            SetFlexColor(vsfDet, "", vDetSlno, vDetItemName)
            .AutoSizeCols()
        End With
        With vsfEff
            .Cols.Count = 3
            .Rows.Count = 1
            .Cols(vEffID).Visible = False
            .SetData(0, vEffFrom, "Eff.From")
            .SetData(0, vEffTo, "Eff.To")
            SetColWidth(vsfEff, -1, 161, 45)
        End With

    End Sub

    Private Sub fillEffDtls(ByVal argItemID As Integer)
        Dim rs As oledb.oledbDataReader
        Dim strSQL As String

        strSQL = "Select * from MsthdrBOM where BomItemID = " & argItemID & " order by EffTo desc"
        rs = clsSVT.getDataReaderFromSQL(strSQL)

        With rs
            vsfEff.Rows.Count = 1
            If Not .HasRows Then
            End If
            Do While .Read
                vsfEff.Rows.Count = vsfEff.Rows.Count + 1
                vsfEff.SetData(vsfEff.Rows.Count - 1, vEffID, !BomID)
                vsfEff.SetData(vsfEff.Rows.Count - 1, vEffFrom, !EffFrom)
                vsfEff.SetData(vsfEff.Rows.Count - 1, vEffTo, !EffTo)
            Loop
            .Close()
        End With

    End Sub

    Public Sub ShowRev(argItem As String)

        Dim rsTemp As oledb.oledbDataReader
      
        Dim blnFound As Boolean
        Dim Strsql As String

        Dim rsBom As oledb.oledbDataReader

        Dim i As Integer, intRow As Integer
        Dim lngItemId As Long

        lblItemCode.Text = argItem
        lblItemName.Text = clsSVT.getDataStringValue("Select ItemName from MstItem where ItemCode = '" & argItem & "'", "", False)

        lngItemId = clsSVT.getDataLongValue("Select ItemID from MstItem Where ItemCode = '" & argItem & "'", 0, False)
        Cursor = Cursors.WaitCursor
        Strsql = "Select * from MstHdrBOM where BomItemID = " & lngItemId & " order by EffTo"
        rsTemp = clsSVT.getDataReaderFromSQL(strSQL)

        With rsTemp
            vsfDet.Cols.Count = 2
            vsfDet.Rows.Count = 1
            Do While .Read
                vsfDet.Cols.Count = vsfDet.Cols.Count + 1
                If Year(!EffTo) = 9998 Then
                    vsfDet.SetData(0, vsfDet.Cols.Count - 1, Format(!EffFrom, "dd/MM/yyyy") & " - " & Format(!EffTo, "dd/MM/yyyy") & " (Latest)")
                Else
                    vsfDet.SetData(0, vsfDet.Cols.Count - 1, Format(!EffFrom, "dd/MM/yyyy") & " - " & Format(!EffTo, "dd/MM/yyyy"))
                End If
                'vsfDet.Cols(vsfDet.Cols.Count - 1).Format = "dd/MM/yyyy"
                vsfDet.Cols(vsfDet.Cols.Count - 1).Width = 200
                '.Cols(vDetDocDate).Format = "dd/MM/yyyy"
                Strsql = " Select ItemName, Qty" & _
                        " from MstItem i, MstDtlBOM b " & _
                        " Where i.ItemID = b.ItemId " & _
                        " and b.BOMId = " & rsTemp!BomID
                rsBom = clsSVT.getDataReaderFromSQL(Strsql)

                With rsBom
                    Do While .Read
                        blnFound = False
                        For i = 1 To vsfDet.Rows.Count - 1
                            If vsfDet.GetData(i, vDetItemName) = rsBom("itemName") Then
                                blnFound = True
                                intRow = i
                                Exit For
                            End If
                        Next i
                        If Not blnFound Then
                            vsfDet.Rows.Count = vsfDet.Rows.Count + 1
                            intRow = vsfDet.Rows.Count - 1
                            vsfDet.SetData(intRow, vDetSlno, intRow)
                            vsfDet.SetData(intRow, vDetItemName, rsBom("itemName"))
                        End If
                        vsfDet.SetData(intRow, vsfDet.Cols.Count - 1, rsBom("Qty"))

                    Loop
                    rsBom.Close()
                End With
            Loop
            rsTemp.Close()
        End With
        ' vsfDet.Cell(flexcpFontBold, 0, vsfDet.Cols - 1, vsfDet.Rows - 1, vsfDet.Cols - 1) = True
        ' CellStyle(cs = vsfDet.Cols.Count - 1.StyleNew)
        Dim cs As C1.Win.C1FlexGrid.CellStyle = vsfDet.Styles.Add("FontBold")
        cs.Font = New Font(vsfDet.Font.Name, vsfDet.Font.Size, FontStyle.Bold)
        vsfDet.Cols(vsfDet.Cols.Count - 1).Style = cs
        Call markChange()
        vsfDet.AutoSizeCols()
        Call fillEffDtls(lngItemId)
EndProc:
        Cursor = Cursors.Default
        rsTemp = Nothing
    End Sub

    Private Sub vsfDet_BeforeEdit1(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub frmUtlBomShowRev_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control Then
            Call initFormSize(Me, Me.Text, True, 824, 557, 0, 0)
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.Alt = True AndAlso e.KeyCode = Keys.E Then
            Call ExportToXL(vsfDet)
        End If
    End Sub

    Private Sub btnExptoXL_Click(sender As System.Object, e As System.EventArgs)
        Call ExportToXL(vsfDet)
    End Sub

    Private Sub frmUtlBomShowRev_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Call initFormSize(Me, Me.Text, False, 824, 557, 0, 0)
        Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
        dtEffFrom.Enabled = False
    End Sub

    Private Sub SetFlexBackColor(ByVal argVsfGrid As C1.Win.C1FlexGrid.C1FlexGrid, ByVal argType As String, ByVal argIntRow As Integer)
        Dim csRed As CellStyle = argVsfGrid.Styles.Add("RED")
        csRed.ForeColor = Color.Red

        Dim csGreen As CellStyle = argVsfGrid.Styles.Add("GREEN")
        csGreen.ForeColor = Color.Green

        With argVsfGrid
            If argType = "M" Then
                Dim rgRed As CellRange = .GetCellRange(argIntRow, vDetItemName)
                rgRed.Style = argVsfGrid.Styles("RED")
            Else
                Dim rgGreen As CellRange = .GetCellRange(argIntRow, vDetItemName)
                rgGreen.Style = argVsfGrid.Styles("GREEN")
            End If
        End With
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Call ExportToXL(vsfDet)
    End Sub

    Private Sub vsfEff_Click(sender As Object, e As EventArgs) Handles vsfEff.Click

    End Sub

    Private Sub vsfEff_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfEff.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub vsfEff_DoubleClick(sender As Object, e As EventArgs) Handles vsfEff.DoubleClick
        With vsfEff
            If .Row <= 0 Then Exit Sub
            Call argFrm.FillDocDetails(.GetData(.Row, vEffID))
            Me.Close()
        End With
    End Sub
End Class