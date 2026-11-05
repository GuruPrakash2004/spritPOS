Imports System.Data.oledb
Imports System.IO
Imports C1.Win.C1FlexGrid

Public Class frmUtlAuth
    Dim intTotRec As Integer

    Dim clsSVT As New svtLibrary
    Dim strSQL As String
    Dim strDocName As String
    Dim intDocID As Int32
    Dim strNoPend As String
    Dim strCaption As String
    'Decl for Authorisation

    Const vDocID = 0
    Const vDocShForm = 1
    Const vDocTable = 2
    Const vAuthDoc = 3
    Const vAuthNoofPend = 4
    Const vTotCols = 5

    Private Sub frmUtlAuth_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F9 And e.Control = True Then
            Call initFormSize(Me, Me.Text, False, 681, 512, 0, 0)
            MsgBox("Restored Form & Grid Layout Successfully!!", MsgBoxStyle.Information, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control = True Then
            Call SaveActFormSettings(Me, Me.Text)
            Call SaveActFormVsfDet(Me, Me.Text, "Authorise", vsfAuth)
            MsgBox("Saved Form & Grid Layout Successfully!!", MsgBoxStyle.Information, Me.Text)
        End If

    End Sub

    Private Sub frmUtlAuth_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Call initscreen()
        Me.KeyPreview = True
        Call initFormSize(Me, Me.Text, True, 681, 512, 0, 0)
        Call initFormVsfDet(Me, Me.Text, vsfAuth, "Authorise")
    End Sub

    Private Sub initscreen()
        With vsfAuth
            .Rows.Count = 1
            .Cols.Count = vTotCols
            .SetData(0, vAuthDoc, "Document Name")
            .SetData(0, vAuthNoofPend, "No. of documents to authorise")
        End With

        Call SetColWidth(vsfAuth, 150, 150)
        Call SetFlexColor(vsfAuth, "", vAuthDoc, vAuthNoofPend)
        Call SetHideCol(vsfAuth, True, vDocID, vDocShForm, vDocTable)
        Call SetColAlign(vsfAuth, svtAlign.Left, vAuthNoofPend)

        Dim rs As oledb.oledbDataReader
        rs = clsSVT.getDataReaderFromSQL("Select * From UtlTransactions where AuthReq = 'Y' and DocType = 'Transaction' order by DocName")
        With vsfAuth
            If Not rs.HasRows Then
                rs.Close()
                Exit Sub
            End If

            Do While rs.Read()
                .Rows.Count = .Rows.Count + 1
                .SetData(.Rows.Count - 1, vDocID, rs.Item("DocID"))
                .SetData(.Rows.Count - 1, vAuthDoc, rs.Item("DocName"))
                .SetData(.Rows.Count - 1, vDocShForm, rs.Item("DocShForm"))
                .SetData(.Rows.Count - 1, vDocTable, rs.Item("HdrTblName"))
            Loop
            rs.Close()
            .AutoSizeCols()
        End With

        Call fillPendDtls()
        Call SetColAlign(vsfAuth, svtAlign.Center, 1)
        vsfAuth.Styles.Fixed.Font = New Font("Lucida Sans", vsfAuth.Font.Size, FontStyle.Bold)
        vsfAuth.Styles.Fixed.TextAlign = TextAlignEnum.CenterCenter
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        'Me.Close()
        Dim tb As New C1.Win.C1Command.C1DockingTabPage()
        Dim dockTabMain As New C1.Win.C1Command.C1DockingTab()
        Me.Dispose()
        tb.Controls.Clear()
        frmMain.dockTabMain.SelectedTab.Dispose()
    End Sub

    Private Sub fillPendDtls()
        With vsfAuth
            For i As Int32 = 1 To .Rows.Count - 1
                Call fillPendAuth(.GetData(i, vDocShForm) & "ID", .GetData(i, vDocTable), i, Val(.GetData(i, vDocID)))
            Next
        End With
    End Sub

    Private Sub fillPendAuth(ByVal argID As String, ByVal argTblName As String, ByVal intRow As Integer,
                             Optional ByVal argMenuID As Integer = 0, Optional ByVal argPO2Auth As Boolean = False)
        Dim strSQL As String, intAuthRights As Integer
        Dim intPend As Integer

        strSQL = "select "
        If argPO2Auth = False Then
            strSQL &= " substring(UserRights, 6, 1)"
        Else
            strSQL &= " substring(UserRights, 8, 1)"
        End If
        strSQL &= " from MstUserRights d, MstUser h"
        strSQL &= " where h.UserID = d.UserID"
        strSQL &= " and UserCode = '" & gUserCode & "'"
        strSQL &= " and MenuItemID = " & argMenuID

        intAuthRights = clsSVT.getDataLongValue(strSQL, 0, False)

        If gUserCode <> "ADMIN" Then
            If intAuthRights = 0 Then
                vsfAuth.Rows(intRow).Visible = False
            Else
                vsfAuth.Rows(intRow).Visible = True
            End If
        End If

        strSQL = "Select count(" & argID & ") from " & argTblName & " where Authorised = 'N' and Cancelled = 'N'"

        intPend = clsSVT.getDataLongValue(strSQL, 0, False)
        If intPend = 0 Then
            vsfAuth.SetData(intRow, vAuthNoofPend, "No documents pending for authorisation.")
        Else
            vsfAuth.SetData(intRow, vAuthNoofPend, intPend)
        End If

    End Sub

    Private Sub vDet_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfAuth.BeforeEdit
        e.Cancel = True
    End Sub

    'Private Sub vsfAuth_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfAuth.Click
    '    'With vsfAuth
    '    '    strDocName = vsfAuth.GetData(vsfAuth.Row, vAuthDoc)
    '    '    strNoPend = vsfAuth.GetData(vsfAuth.Row, vAuthNoofPend)
    '    '    strCaption = vsfAuth.GetData(vsfAuth.Row, vDocShForm)
    '    '    intDocID = vsfAuth.GetData(vsfAuth.Row, vDocID)
    '    'End With
    'End Sub

    Private Sub vsfAuth_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles vsfAuth.DoubleClick

        strDocName = vsfAuth.GetData(vsfAuth.Row, vAuthDoc)
        strNoPend = vsfAuth.GetData(vsfAuth.Row, vAuthNoofPend)
        strCaption = vsfAuth.GetData(vsfAuth.Row, vDocShForm)
        intDocID = vsfAuth.GetData(vsfAuth.Row, vDocID)
        Call getAuthDoc(strDocName)
        strDocName = ""
        strNoPend = ""
        strCaption = ""
    End Sub

    Private Sub getAuthDoc(ByVal argDocName As String)
        If strNoPend = "No documents pending for authorisation." Then
            MsgBox("No documents pending for authorisation.", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        Dim objFrm As New frmUtlAuthDoc
        With objFrm
            .Text = "Authorise : " & argDocName
            .intDocID = intDocID
            .StartPosition = FormStartPosition.CenterParent
            frmMain.OpenForms(objFrm, "Authorise : " & argDocName, True)
        End With

        Call fillPendDtls()
        strDocName = ""
        strNoPend = ""
    End Sub

    Private Sub btnAuth_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAuth.Click
        If strDocName = "" Then Exit Sub
        Call getAuthDoc(strDocName)
    End Sub

    Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles BtnRefresh.Click
        Call fillPendDtls()
    End Sub

    Private Sub btnClose_Leave(sender As Object, e As EventArgs) Handles btnClose.Leave
        With vsfAuth
            .Row = 1
            .Col = 1
            Call showWaitCursor()
        End With
    End Sub

    Private Sub btnAuth_LostFocus(sender As Object, e As EventArgs) Handles btnAuth.LostFocus
        btnClose.Focus()
    End Sub

    Private Sub vsfAuth_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfAuth.KeyUp
        If e.KeyCode = Keys.Tab Then
            If vsfAuth.Row = vsfAuth.Rows.Count - 1 AndAlso vsfAuth.Col = vsfAuth.Cols.Count - 1 Then
                BtnRefresh.Focus()
            End If

        End If
    End Sub

    Private Sub BtnRefresh_Leave(sender As Object, e As EventArgs) Handles BtnRefresh.Leave
        btnAuth.Focus()
    End Sub
End Class