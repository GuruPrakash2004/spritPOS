Imports System.Data.oledb
Public Class frmUtlBOMTreeview
    Dim strsql As String
    Dim clsSVT As New svtLibrary
    Dim intBOMId As Integer
    Dim intCtr As Integer
    Dim tvwNodes As New Collection
    Public Sub ShowTreeView(argBOMID As Long)
        tvBOM.Nodes.Clear()
        'Cursor.Current = Cursors.WaitCursor
        txtBOMID.Text = argBOMID
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
            tvwNodes.Add(grpNode)
            addNode(grpNode, value)
            tvBOM.ExpandAll()
        Next

        'tvwNodes.Clear()
        'Call getAllNodes(tvBOM)
    End Sub
    Private Function addDS(ByVal strQry As String) As DataSet
        Dim strConn As String
        strConn = "Persist Security Info=False"
        strConn = strConn & ";User ID=" & gStrDBLoginName
        strConn = strConn & ";Pwd=" & gStrDBLoginPwd
        strConn = strConn & ";Initial Catalog=" & gStrDBName
        strConn = strConn & ";Data Source=" & gStrServerName
        Dim SqlConn As New oledbConnection(strConn)
        Dim da As New oledbDataAdapter(strQry, SqlConn)
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
                tvwNodes.Add(ledNode)

                strsql = "select BOMID from MstHdrBOM where BOMItemId = " & dr("ItemID") & " and LatestBOM='Y'"
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
        intBOMId = Val(txtBOMID.Text)
    End Sub

    Private Sub btnExpand_Click(sender As Object, e As System.EventArgs) Handles btnExpand.Click
        tvBOM.ExpandAll()

    End Sub

    Private Sub btnCollapse_Click(sender As System.Object, e As System.EventArgs) Handles btnCollapse.Click
        tvBOM.CollapseAll()
    End Sub

    Private Sub txtFind_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txtFind.KeyUp
        If e.KeyCode = Keys.Enter Then
            If Trim(txtFind.Text) = "" Then Exit Sub
            If InStr(Trim(txtFind.Text), " Found") > 0 Then Exit Sub

            With Me.tvBOM
                intCtr = 0
                For Each nd In tvwNodes
                    If InStr(UCase(Trim(nd.Text)), UCase(Trim(txtFind.Text))) > 0 Then
                        nd.ForeColor = Color.Red
                        nd.NodeFont = New Font(tvBOM.Font, FontStyle.Bold)
                        nd.text = nd.text
                        nd.EnsureVisible()
                        intCtr = intCtr + 1
                    Else
                        nd.ForeColor = Color.Black
                        nd.NodeFont = New Font(tvBOM.Font, FontStyle.Regular)
                    End If
                Next
            End With

            If intCtr > 0 Then
                txtFind.Text = intCtr & " Found"
            Else
                txtFind.Text = "NOT FOUND " & txtFind.Text
            End If
            With txtFind
                .SelectAll()
            End With
        End If

    End Sub

    Private Sub btnClose_Click(sender As Object, e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnClearSerach_Click(sender As Object, e As System.EventArgs) Handles btnClearSerach.Click
        txtFind.Text = ""
        With Me.tvBOM
            'tvwNodes.Clear()
            'Call getAllNodes(tvBOM)
            For Each nd In tvwNodes
                nd.ForeColor = Color.Black
                nd.NodeFont = New Font(tvBOM.Font, FontStyle.Regular)
            Next
        End With
    End Sub

    Private Sub btnPring_Click(sender As System.Object, e As System.EventArgs) Handles btnPrint.Click
        With frmUtlBOMPrint
            .Text = Replace(Me.Text, "TreeView", "TreeView Print")
            .InitScreen()
            .LoadBOM(intBOMId)
            .Show()
        End With
    End Sub



    'Private Sub getAllNodes(ByVal treeView As TreeView)
    '    Dim nodes As TreeNodeCollection = treeView.Nodes
    '    For Each n As TreeNode In nodes
    '        getNodes(n)
    '    Next
    'End Sub

    'Private Sub getNodes(ByVal treeNode As TreeNode)
    '    tvwNodes.Add(treeNode)
    '    For Each tn As TreeNode In treeNode.Nodes
    '        getNodes(tn)
    '    Next
    'End Sub
End Class