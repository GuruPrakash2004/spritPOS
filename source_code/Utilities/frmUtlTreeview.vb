Imports System.IO
Imports System.Data.SqlClient
Imports System.Data.OleDb
Public Class frmUtlTreeview
    Dim strsql As String
    Dim clsSVT As New svtLibrary
    Dim intBOMId As Integer
    Dim intCtr As Integer

    Public Sub ShowTreeView(argBOMID As Long)
        tvBOM.Nodes.Clear()
        'Cursor.Current = Cursors.WaitCursor

        strsql = "select ItemCode,ItemName, h.*"
        strsql &= " from MstHdrBOM h, MstItem i"
        strsql &= " where h.BOMItemId = i.ItemID"
        strsql &= " and h.BOMID = " & argBOMID

        Dim groupDS As DataSet = addDS(strsql)

        For Each dr As DataRow In groupDS.Tables(0).Rows
            Dim grpNode As New TreeNode
            ' grpNode.Text = dr("ItemCode").ToString()
            grpNode.Text = dr("ItemCode").ToString() & "--{1}" & Space(3) & "[" & dr("ItemName").ToString & "]"
            grpNode.Tag = dr("BOMID").ToString()
            Dim value As String = dr("BOMID").ToString()
            tvBOM.Nodes.Add(grpNode)

            addNode(grpNode, value)

            tvBOM.ExpandAll()
        Next
    End Sub
    Private Function addDS(ByVal strQry As String) As DataSet
        Dim strConn As String
        strConn = "Persist Security Info=False"
        strConn = strConn & ";User ID=" & gStrDBLoginName
        strConn = strConn & ";Pwd=" & gStrDBLoginPwd
        strConn = strConn & ";Initial Catalog=" & gStrDBName
        strConn = strConn & ";Data Source=" & gStrServerName
        Dim SqlConn As New SqlConnection(strConn)
        Dim da As New SqlDataAdapter(strQry, SqlConn)
        Dim ds As New DataSet()
        da.Fill(ds)
        Return ds
    End Function
    Private Function addNode(node As TreeNode, argBOMID As Long) As Integer
        Dim intItemBOMId As Integer
        strsql = ""

        strsql = "select * from MstDtlBOM d, MstItem i"
        strsql &= " where d.ItemID = i.ItemID"
        strsql &= " and d.BOMID = " & argBOMID
        strsql &= " order by d.SlNo"

        Dim ledgerDS As DataSet = addDS(strsql)
        If ledgerDS.Tables(0).Rows.Count > 0 Then
            For Each dr As DataRow In ledgerDS.Tables(0).Rows
                Dim ledNode As New TreeNode()
                'ledNode.Text = dr("ItemCode").ToString() & Space(3) & " Qty - " & dr("Qty").ToString() 
                ledNode.Text = dr("ItemCode").ToString() & "--{" & dr("Qty").ToString() & "}" & Space(3) & "[" & dr("ItemName").ToString & "]"
                ledNode.Tag = dr("ItemID").ToString().Trim()
                Dim temp As String = dr("BOMID").ToString()
                node.Nodes.Add(ledNode)

                strsql = "select BOMID from MstHdrBOM where BOMItemId = " & dr("ItemID")
                intItemBOMId = clsSVT.getDataLongValue(strsql, 0, False)

                addNode(ledNode, intItemBOMId)
            Next
            Return 0
        Else
            Return 0
        End If
    End Function

    Private Sub UtlTreeview_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Me.KeyPreview = True
        intBOMId = txtBOMID.Text
    End Sub

    Private Sub btnExpand_Click(sender As Object, e As System.EventArgs) Handles btnExpand.Click
        tvBOM.ExpandAll()

    End Sub

    Private Sub btnCollapse_Click(sender As System.Object, e As System.EventArgs) Handles btnCollapse.Click
        tvBOM.CollapseAll()
    End Sub

    Private Sub txtFind_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txtFind.KeyUp
        Dim intNodeNo As Integer

        If e.KeyCode = Keys.Enter Then
            If Trim(txtFind.Text) = "" Then Exit Sub
            If InStr(Trim(txtFind.Text), " Found") > 0 Then Exit Sub
            Dim i As Integer

            With Me.tvBOM
                intCtr = 0
                strsql = "select Count(ItemCode) as no from MstDtlBOM d, MstItem i where d.ItemID = i.ItemID and d.BOMID = " & intBOMId
                intNodeNo = clsSVT.getDataLongValue(strsql, 0, False)
                If InStr(UCase(Trim(.Nodes(0).Text)), UCase(Trim(txtFind.Text))) > 0 Then
                    .Nodes(0).ForeColor = Color.Red
                    .Nodes(0).NodeFont = New Font(tvBOM.Font, FontStyle.Bold)
                    .Nodes(0).EnsureVisible()
                    intCtr = intCtr + 1

                Else
                    .Nodes(0).ForeColor = Color.Black
                End If
                For i = 0 To intNodeNo - 1

                    If InStr(UCase(Trim(.Nodes(0).Nodes(i).Text)), UCase(Trim(txtFind.Text))) > 0 Then
                        Call ColourSubnodes(i, Trim(.Nodes(0).Nodes(i).Text))
                        .Nodes(0).Nodes(i).ForeColor = Color.Red
                        .Nodes(0).Nodes(i).NodeFont = New Font(tvBOM.Font, FontStyle.Bold)
                        .Nodes(0).Nodes(i).EnsureVisible()
                        intCtr = intCtr + 1

                    Else
                        Call ColourSubnodes(i, Trim(.Nodes(0).Nodes(i).Text))
                        .Nodes(0).Nodes(i).ForeColor = Color.Black
                    End If

                Next i

            End With

            If intCtr > 0 Then
                txtFind.Text = intCtr & " Found"
            Else
                txtFind.Text = "NOT FOUND " & txtFind.Text
            End If
        End If
        With txtFind
            '.selstart = 0
            '.SelLength = Len(txtFind.Text)
        End With
    End Sub
    Private Sub ColourSubnodes(i As Integer, argitemcode As String)
        Dim intItemId As Integer, intItemBOMId As Integer, intsubNode As Integer
        strsql = "Select ItemId from MstItem where ItemCode='" & argitemcode & "'"
        intItemId = clsSVT.getDataLongValue(strsql, 0, False)
        strsql = "select BOMID from MstHdrBOM where BOMItemId = " & intItemId
        intItemBOMId = clsSVT.getDataLongValue(strsql, 0, False)
        If intItemBOMId > 0 Then
            strsql = "select Count(ItemCode) as no from MstDtlBOM d, MstItem i where d.ItemID = i.ItemID and d.BOMID = " & intItemBOMId
            intsubNode = clsSVT.getDataLongValue(strsql, 0, False)
            With Me.tvBOM
                If intsubNode > 0 Then
                    For x = 0 To intsubNode - 1
                        If InStr(UCase(Trim(.Nodes(0).Nodes(i).Nodes(x).Text)), UCase(Trim(txtFind.Text))) Then
                            .Nodes(0).Nodes(i).ForeColor = Color.Red
                            .Nodes(0).Nodes(i).NodeFont = New Font(tvBOM.Font, FontStyle.Bold)
                            .Nodes(0).Nodes(i).EnsureVisible()
                            intCtr = intCtr + 1
                        Else
                            .Nodes(0).Nodes(i).ForeColor = Color.Black
                        End If
                    Next x
                End If
            End With
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnClearSerach_Click(sender As Object, e As System.EventArgs) Handles btnClearSerach.Click
        Dim i As Integer
        txtFind.Text = ""
        Dim intNodeNo As Integer
        With Me.tvBOM
            .Nodes(0).ForeColor = Color.Black
            intNodeNo = .Nodes(0).GetNodeCount(True)
            For i = 0 To intNodeNo - 1
                .Nodes(0).Nodes(i).ForeColor = Color.Black
                .Nodes(0).Nodes(i).NodeFont = New Font(tvBOM.Font, FontStyle.Regular)
            Next i
        End With
    End Sub
End Class