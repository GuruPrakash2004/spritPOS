Imports System.Data.OleDb
Imports C1.Win.C1FlexGrid

Public Class frmRptProfitLoss
    Public strList As String
    Dim strItem As String
    Dim strAgentList As String
    Dim clsSVT As New svtLibrary

    Dim dt As DataTable
    Dim dv As DataView

    Private Sub btnView_Click(sender As Object, e As EventArgs) Handles btnView.Click
        Dim strSQL As String = ""

        strSQL = " Select VhclNo VehicleNo, LDGDate Date,"
        strSQL &= " stuff((select distinct ', ' +  CustomerCode  from  MstCustomer c , TrnDtlLDG d  "
        strSQL &= " Where d.LDGID=h.ldgid and c.CustomerID = d.CustomerID FOR XML PATH('')),1,2,'') as Loader, "
        strSQL &= " GrandTot Amount, RetAmt RetAmount, DiffAmt DiffAmount"
        strSQL &= " From TrnHdrLDG h "
        strSQL &= " Where 1=1  "
        strSQL &= " and h.Cancelled = 'N' "

        If strAgentList <> "" Then
            strSQL &= " and h.AGENTID in (" & strAgentList & ")"
        End If

        If strItem <> "" Then
            strSQL &= " and d.vItemID in (" & strItem & ")"
        End If

        If chkDate.Checked Then
            strSQL &= " and h.LDGDate between " & getDateFormatInCond(dtpFromDate.Value) & " and " & getDateFormatInCond(dtpToDate.Value)
        End If

        strSQL &= " Order by LDGDate"

        vsfDet.Rows.Fixed = 4
        If Not (vsfDet.Rows.Count > 0) Then
            vsfDet.Rows.Count = 4
        End If

        vsfDet.Rows.GlyphRow = 3
        vsfDet.BeginUpdate()
        dt = clsSVT.getDATATable(strSQL)
        dv = New DataView(dt)
        vsfDet.DataSource = dv

        vsfDet.Refresh()
        If chkDate.Checked Then
            Call initVsfForHdrs(Me.Text, "Date Range From " & dtpFromDate.Value & " To " & dtpToDate.Value)
        Else
            Call initVsfForHdrs(Me.Text, "")
        End If
        vsfDet.EndUpdate()

        vsfDet.Rows(0).TextAlignFixed = TextAlignEnum.LeftCenter
        vsfDet.Rows(1).TextAlignFixed = TextAlignEnum.LeftCenter

        If chkMerge.Checked Then
            vsfDet.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictAll
            For i = 0 To 10
                vsfDet.Cols(i).AllowMerging = True
            Next i
        Else
            vsfDet.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.None
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub chkItem_CheckedChanged(sender As Object, e As EventArgs) Handles chkItem.CheckedChanged
        Dim strSQL As String

        If chkItem.Checked = False Then
            strList = ""
            strItem = ""
            Exit Sub
        End If

        Dim frmSel As New frmUtlMultiSelRpt

        strSQL = "Select distinct 0 as sel, VITEMID, VITEMCODE from MstVItem where VITEMID > 0 and "
        strSQL = strSQL & " VITEMCODE Not in ('-' , '') and InActive = 'N' order by VITEMCODE"
        Call frmSel.InitVsfSel(strSQL, 1, 1, True, Me, False)
        frmSel.Text = "Select [Multiple] Items"
        frmSel.vsfDet.SetData(0, 0, "Sel")
        frmSel.ShowDialog()
        strItem = strList
        If strItem = "" Then chkItem.Checked = False
    End Sub

    Private Sub chkAgent_CheckedChanged(sender As Object, e As EventArgs) Handles chkAgent.CheckedChanged
        Dim strSQL As String

        If chkAgent.Checked = False Then
            strList = ""
            strAgentList = ""
            Exit Sub
        End If

        Dim frmSel As New frmUtlMultiSelRpt

        strSQL = "Select distinct 0 as sel, AgentID, AgentCode from MstAgent where AgentID > 0 and "
        strSQL = strSQL & " AgentCode Not in ('-' , '') and InActive = 'N' order by AgentCode"
        Call frmSel.InitVsfSel(strSQL, 1, 1, True, Me, False)
        frmSel.Text = "Select [Multiple] AgentCode"
        frmSel.vsfDet.SetData(0, 0, "Sel")
        frmSel.ShowDialog()
        strAgentList = strList
        If strAgentList = "" Then chkAgent.Checked = False
    End Sub

    Private Sub btnExcel_Click(sender As System.Object, e As System.EventArgs) Handles btnExcel.Click
        Call ExportToXL(vsfDet, argFileName:=Me.Text & " ")
    End Sub

    Private Sub frmRptAgtBal_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control Then
            Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
            MsgBox("Restored Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormVsfDet(Me, Me.Text, "DET", vsfDet)
            MsgBox("Saved Form & Grid Layout Successfully!!", vbInformation, Me.Text)
        End If
    End Sub

    Private Sub frmRptAgtBal_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Me.Text = "Profit Loss Report"
        vsfView.Visible = False
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        Call fillFormProperties(Me)
        Call initGridStyles(Me)
    End Sub

    Private Sub chkMerge_CheckedChanged(sender As Object, e As EventArgs) Handles chkMerge.CheckedChanged

    End Sub

    Private Sub initVsfForHdrs(ByVal argHdrLine1 As String, ByVal argHdrLine2 As String)
        Dim i As Integer

        With vsfDet
            For i = 0 To .Cols.Count - 1
                .SetData(3, i, .GetData(0, i))

                .SetData(0, i, gSettings.gCompName & ",  " & gSettings.gCompAdd1 & gSettings.gCompAdd2 & gSettings.gCompAdd3 &
                          gSettings.gCompAdd4 & gSettings.gCompAdd5)
                .SetData(1, i, argHdrLine1)
                .SetData(2, i, argHdrLine2)
            Next
            Dim csf1 As C1.Win.C1FlexGrid.CellStyle = vsfDet.Styles.Add("F1")
            csf1.BackColor = Color.Beige

            .Rows(0).Style = csf1
            .Rows(1).Style = csf1
            .Rows(2).Style = csf1

            If argHdrLine1 = "" Then
                .Rows(1).Visible = False
            Else
                .Rows(1).Visible = True
            End If

            If argHdrLine2 = "" Then
                .Rows(2).Visible = False
            Else
                .Rows(2).Visible = True
            End If

            .Rows(0).AllowMerging = True
            .Rows(1).AllowMerging = True
            .Rows(2).AllowMerging = True
            .Rows(3).AllowMerging = True

        End With
    End Sub
End Class