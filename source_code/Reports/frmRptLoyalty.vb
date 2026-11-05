Imports C1.Win.C1FlexGrid

Public Class frmRptLoyalty
    Dim clsSvt As New svtLibrary
    Dim strSQL As String
    Public strList As String
    Public strCustList As String
    Public blnInvoiceVw As Boolean = False

    Const vSmryCustName = 0
    Const vSmryMobNo = 1
    Const vSmryCardNo = 2
    Const vSmryTotPoints = 3
    Const vSmryRPoints = 4
    Const vSmryBalPoints = 5
    Const vSmryAmt = 6
    Const VSmryTotCols = 7

    Const vDetCustName = 0
    Const vDetMobNo = 1
    Const vDetCardNo = 2
    Const vDetDocDate = 3
    Const vDetDocType = 4
    Const vDetDocRef = 5
    Const vDetPointsAdded = 6
    Const vDetRPoints = 7
    Const vDetTotCols = 8

    Private Sub initscreen()
        With vsfDet
            .Rows.Count = 1
            .Cols.Count = VSmryTotCols
            .SetData(0, vSmryCustName, "Customer")
            .SetData(0, vSmryMobNo, "Phone No")
            .SetData(0, vSmryCardNo, "Card No")
            .SetData(0, vSmryTotPoints, "Total Points")
            .SetData(0, vSmryRPoints, "Redeemed")
            .SetData(0, vSmryBalPoints, "Balance Points")
            .SetData(0, vSmryAmt, "Amount")
            SetFlexColorFullGrid(vsfDet, "")
        End With
    End Sub

    Private Sub frmRptPendingPO_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        fillFormProperties(Me)
        initGridStyles(Me)
        initscreen()
        initFormVsfDet(Me, Me.Text, vsfDet, "SMRY")
    End Sub

    Private Sub chkCust_CheckedChanged(sender As Object, e As EventArgs) Handles chkCust.CheckedChanged
        Dim strSQL As String
        If chkCust.Checked = False Then
            strCustList = ""
            Exit Sub
        End If
        Dim frmSel As New frmUtlMultiSelection

        strSQL = "Select 0 as sel, CustID,CustName from MstCust Where CustID > 0 Order by 1"

        Call frmSel.InitVsfSel(strSQL, 1, 1, True, Me, False)
        frmSel.Text = "Select [Multiple] Customer"
        frmSel.vsfDet.SetData(0, 0, "Sel")
        frmSel.StartPosition = FormStartPosition.CenterScreen
        frmSel.ShowDialog()
        strCustList = strList
        If strCustList = "" Then chkCust.Checked = False
    End Sub

    Public Sub btnView_Click(sender As Object, e As EventArgs) Handles btnView.Click
        If optSummary.Checked Then
            PrintRptSmry()
        Else
            PrintRptDet()
        End If
    End Sub

    Private Sub btnExportToXL_Click(sender As Object, e As EventArgs) Handles btnExportToXL.Click
        ExportToXL(vsfDet,,,, True, Me.Text, "")
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        frmMain.CloseSelectedTabPage(Me)
    End Sub

    Public Sub PrintRptSmry()
        With vsfDet
            .Rows.Count = 1
            .Cols.Count = VSmryTotCols
            .SetData(0, vSmryCustName, "Customer")
            .SetData(0, vSmryMobNo, "Phone No")
            .SetData(0, vSmryCardNo, "Card No")
            .SetData(0, vSmryTotPoints, "Total Points")
            .SetData(0, vSmryRPoints, "Redeemed")
            .SetData(0, vSmryBalPoints, "Balance Points")
            .SetData(0, vSmryAmt, "Amount")
            SetFlexColorFullGrid(vsfDet, "")
        End With

        Dim rs As oledb.oledbDataReader
        Dim strSQL As String
        Dim i As Int32

        strSQL = "Select Custcode, phno, cardno, sum(invpoints) as iPoint, "
        strSQL &= " sum(Rpoints) as RPoint, sum(InvPoints - Rpoints) as BPoint"
        strSQL &= " From vwRedeem"
        If (chkCust.Checked AndAlso strCustList <> "") Or blnInvoiceVw Then
            strSQL &= " where CustID IN (" & strCustList & ")"
        End If
        strSQL &= " group by CUSTCODE, PHNO, CARDNO"
        strSQL &= " having sum(invpoints-Rpoints) > 0"

        rs = clsSvt.getDataReaderFromSQL(strSQL)

        With vsfDet
            .Rows.Count = 1
            If rs.HasRows Then
                Do While rs.Read
                    .Rows.Count = .Rows.Count + 1
                    i = .Rows.Count - 1
                    .SetData(i, vSmryCustName, GetVal(rs("CustCode"), ""))
                    .SetData(i, vSmryMobNo, GetVal(rs("PhNo"), ""))
                    .SetData(i, vSmryCardNo, GetVal(rs("CardNo"), ""))
                    .SetData(i, vSmryTotPoints, GetVal(rs("iPoint"), ""))
                    .SetData(i, vSmryRPoints, GetVal(rs("RPoint"), ""))
                    .SetData(i, vSmryBalPoints, GetVal(rs("BPoint"), 0))
                Loop
                rs.Close()
            Else
                MsgBox("No Records Found!", MsgBoxStyle.Information, Me.Text)
                .Rows.Count = 1
            End If
            initFormVsfDet(Me, Me.Text, vsfDet, "SMRY")
        End With
    End Sub

    Private Sub frmRptPendingPO_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            If optSummary.Checked Then
                Call SaveActFormVsfDet(Me, Me.Text, "SMRY", vsfDet)
            Else
                Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            End If
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
            End If
    End Sub

    Private Sub vsfDet_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub PrintRptDet()
        With vsfDet
            .Rows.Count = 1
            .Cols.Count = vDetTotCols
            .SetData(0, vDetCustName, "Customer")
            .SetData(0, vDetMobNo, "Phone No")
            .SetData(0, vDetCardNo, "Card No")
            .SetData(0, vDetDocDate, "Doc Date")
            .SetData(0, vDetDocRef, "Doc Ref")
            .SetData(0, vDetDocType, "Doc Type")
            .SetData(0, vDetPointsAdded, "Points Added")
            .SetData(0, vDetRPoints, "Redeemed")

            SetFlexColorFullGrid(vsfDet, "")
        End With

        Dim rs As oledb.oledbDataReader
        Dim strSQL As String
        Dim i As Int32

        strSQL = "Select Custcode, phno, cardno, docType, DocDate, DocRef, InvPoints, RPoints"
        strSQL &= " From vwRedeem"
        If chkCust.Checked AndAlso strCustList <> "" Then
            strSQL &= " where CustID IN (" & strCustList & ")"
        End If

        rs = clsSvt.getDataReaderFromSQL(strSQL)

        With vsfDet
            .Rows.Count = 1
            If rs.HasRows Then
                Do While rs.Read
                    .Rows.Count = .Rows.Count + 1
                    i = .Rows.Count - 1
                    .SetData(i, vDetCustName, GetVal(rs("CustCode"), ""))
                    .SetData(i, vDetMobNo, GetVal(rs("PhNo"), ""))
                    .SetData(i, vDetCardNo, GetVal(rs("CardNo"), ""))
                    .SetData(i, vDetDocType, GetVal(rs("Doctype"), ""))
                    .SetData(i, vDetDocDate, GetVal(rs("docdate"), ""))
                    .SetData(i, vDetDocRef, GetVal(rs("docref"), ""))
                    .SetData(i, vDetPointsAdded, GetVal(rs("invPoints"), ""))
                    .SetData(i, vDetRPoints, GetVal(rs("RPoints"), ""))
                Loop
                rs.Close()
            Else
                MsgBox("No Records Found!", MsgBoxStyle.Information, Me.Text)
                .Rows.Count = 1
            End If
            initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        End With
    End Sub

    Private Sub pnlbtnClose_Click(sender As Object, e As EventArgs) Handles pnlbtnClose.Click
        pnlCopyShortCut.Visible = False
    End Sub

    Private Sub btnCopy_Click(sender As Object, e As EventArgs) Handles btnCopy.Click

        If RbnCopyCell.Checked = True Then
            CopyGrid(vsfDet, "Cell")
        ElseIf RbnCopyColumn.Checked = True Then

            CopyGrid(vsfDet, "Column")

        ElseIf RbnCopyRow.Checked = True Then

            CopyGrid(vsfDet, "Row")
        ElseIf RbnCopyGrid.Checked = True Then

            CopyGrid(vsfDet, "Full Grid")

        End If

        pnlCopyShortCut.Visible = False
    End Sub

    Private Sub vsfDet_Click(sender As Object, e As EventArgs) Handles vsfDet.Click

        If (Control.ModifierKeys And Keys.Control) = Keys.Control AndAlso (Control.ModifierKeys And Keys.Alt) = Keys.Alt Then
            pnlCopyShortCut.Show()
        End If
    End Sub

End Class