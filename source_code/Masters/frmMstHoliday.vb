Imports System.Data.oledb
Imports C1.Win.C1FlexGrid

Public Class frmMstHoliday

    Dim strSQL As String
    Dim clsSVT As New svtLibrary
    Dim svtDate As New svtLibrary
    Dim mode As String = ""
    Private Property currentHolidayId As Int32

    Const vId = 0
    Const vDate = 1
    Const vHolidayName = 2
    Const vDetTotal = 3
    Dim rowCount = 1

    Dim intVsfHeight As Int32, intVsfWidth As Int32

    Public Sub InitScreen()
        With vsfDet
            .Rows.Fixed = 1
            .Rows.Count = 1
            .Cols.Count = vDetTotal
            .SetData(0, vId, "Id")
            .SetData(0, vDate, "Date")
            .SetData(0, vHolidayName, "Holiday Name")
            SetFlexColor(vsfDet, "", vId, vDate, vHolidayName)
            Call SetHideCol(vsfDet, True, vId)
        End With
    End Sub

    Private Sub frmMstHoliday_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call initFormVsfDet(Me, Me.Text, vsfDet, "DET")
        Call initGridStyles(Me)
        Call fillFormProperties(Me)
        Call ChangeFontSize(Me)
        KeyPreview = True
        Call InitScreen()
        Call loadgrid()
        cboMonth.SelectedIndex = 0
        intVsfHeight = vsfCalender.Size.Height
        intVsfWidth = vsfCalender.Size.Width
    End Sub

    Private Sub vsfCalender_MouseClick(sender As Object, e As MouseEventArgs) Handles vsfCalender.MouseClick
        Dim rs As oledb.oledbDataReader
        Try
            strSQL = "Select * from MstHoliday where hdate  = " & getDateFormatInCond(vsfCalender.SelectedDates.ToArray.GetValue(0)) & ""
            rs = svtDate.getDataReaderFromSQL(strSQL)
            Using rs
                With rs
                    If Not rs.HasRows Then
                        DateName.SetToolTip(vsfCalender, "")
                        Exit Sub
                    End If
                    If .Read() Then
                        DateName.SetToolTip(vsfCalender, GetVal(!HolidayName, ""))
                    End If
                End With
            End Using
        Catch ex As Exception
            ''MsgBox(ex.Message, MessageBoxIcon.Information, "Holiday")
        End Try
    End Sub

    Private Sub vsfHolidayd_BeforeEdit(sender As Object, e As C1.Win.C1FlexGrid.RowColEventArgs) Handles vsfDet.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If Not ValidFields() Then Exit Sub
        Call SaveDoc()
        Call loadgrid()
        pnlSave.Visible = False

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        pnlSave.Visible = False
    End Sub

    Private Sub btnDel_Click(sender As Object, e As EventArgs) Handles btnDel.Click
        If mode <> "M" Then Exit Sub

        If Not validDate(dtpDateSel.Value) Then
            MsgBox("Holiday Date Should be with the financial year", vbExclamation, Me.Text)
            Exit Sub
        End If

        Dim dialog As DialogResult
        dialog = MessageBox.Show("Are you sure you want to delete?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If dialog = DialogResult.No Then Exit Sub

        If Not clsSVT.execSQLStatement("Delete from MstHoliday Where HDate = " & getDateFormatInCond(dtpDateSel.Value) & "") Then Exit Sub
        MsgBox("Deleted succussfully", MsgBoxStyle.Information, Me.Text)
        pnlSave.Visible = False
        Call loadgrid()
        pnlSave.Visible = False
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        If dtpFromdate.Value > dtpTodate.Value Then
            MsgBox("From Date should be less than To Date....", MsgBoxStyle.Exclamation, Me.Text)
            Exit Sub
        End If
        Call loadgrid()
    End Sub

    Private Sub cboMonth_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboMonth.SelectedIndexChanged

        If cboMonth.SelectedIndex = -1 Then Exit Sub
        If cboMonth.SelectedIndex = 0 Then
            vsfCalender.Size = New Point(intVsfWidth, intVsfHeight)
            Exit Sub
        End If

        Select Case cboMonth.SelectedIndex
            Case 1
                vsfCalender.Size = New Point(intVsfWidth * 3, intVsfHeight)
            Case 2
                pnlCalender.Size = New Point(intVsfWidth * 4, intVsfHeight)
                vsfCalender.Size = New Point(intVsfWidth * 4, intVsfHeight)
            Case 3, 4, 5
                pnlCalender.Size = New Point(intVsfWidth * 4, intVsfHeight * 3)
                vsfCalender.Size = New Point(intVsfWidth * 4, intVsfHeight * 3)
            Case 6, 7, 8
                pnlCalender.Size = New Point(intVsfWidth * 4, intVsfHeight * 4)
                vsfCalender.Size = New Point(intVsfWidth * 4, intVsfHeight * 4)
            Case 9, 10, 11
                pnlCalender.Size = New Point(intVsfWidth * 4, intVsfHeight * 5)
                vsfCalender.Size = New Point(intVsfWidth * 4, intVsfHeight * 5)
        End Select
        vsfCalender.CalendarDimensions = cboMonth.SelectedItem
    End Sub

    Private Sub frmMstHoliday_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyCode = Keys.F9 And e.Control Then
            Call initFormVsfDet(Me, Me.Text, vsfDet, "Holiday")
            MsgBox("restored form and grid layout successfully!", vbInformation, Me.Text)
        End If
        If e.KeyCode = Keys.F8 And e.Control Then
            Call initFormVsfDet(Me, Me.Text, vsfDet, "Holiday")
            MsgBox("restored form and grid layout successfully!", vbInformation, Me.Text)
        End If

        If e.Alt = True And e.KeyCode = Keys.E Then
            Call ExportToXL(vsfDet)
        End If
    End Sub

    Private Sub loadgrid()
        vsfCalender.CalendarInfo.Holidays.Clear()
        With vsfCalender
            .CalendarInfo.FirstDate = gSettings.gCompStDt
            .CalendarInfo.LastDate = gSettings.gCompEndDt 'CNUD
        End With

        Dim strSQL As String = ""
        strSQL = " select Id,Hdate,HolidayName  "
        strSQL &= " from MstHoliday "
        strSQL &= " where Hdate Between " & getDateFormatInCond(dtpFromdate.Value)
        strSQL &= " and " & getDateFormatInCond(dtpTodate.Value)

        Dim rs As oledb.oledbDataReader
        Dim thCb As C1.Win.C1Schedule.UI.CalendarTheme = vsfCalender.Theme
        rs = svtDate.getDataReaderFromSQL(strSQL)
        With vsfDet
            .Rows.Count = 1
            If Not rs.HasRows Then
                vsfCalender.CalendarInfo.Holidays.Clear()
                thCb(7).BackColor = Color.Transparent
                Exit Sub
            End If
            Dim thC As C1.Win.C1Schedule.UI.CalendarTheme = vsfCalender.Theme
            Do While rs.Read
                .Rows.Count = .Rows.Count + 1
                .SetData(.Rows.Count - 1, vId, GetVal(rs!Id, ""))
                .SetData(.Rows.Count - 1, vDate, GetVal(rs!Hdate, ""))
                .SetData(.Rows.Count - 1, vHolidayName, GetVal(rs!HolidayName, ""))
                vsfCalender.CalendarInfo.Holidays.Add(GetVal(rs!Hdate, ""))
                vsfCalender.BoldedDates = vsfCalender.CalendarInfo.Holidays.ToArray
                thC(7).BackColor = Color.Aqua
            Loop
            rs.Close()
        End With

    End Sub

    Public Function ValidFields() As Boolean
        ValidFields = False

        If Trim(txtHolidayName.Text) = "" Then
            MsgBox("Holiday Name should not be blank", MsgBoxStyle.Information, Me.Text)
            txtHolidayName.Focus()
            Exit Function
        End If

        If Not validDate(dtpDateSel.Value) Then
            MsgBox("Holiday Date Should be within the financial year", vbExclamation, Me.Text)
            dtpDateSel.Focus()
            Exit Function
        End If
        Return True
    End Function

    Function SaveDoc() As Boolean
        SaveDoc = False
        Call stringReplace(Me)
        Dim blnFound As Boolean
        Try
            If mode = "A" Then
                Call svtDate.getDataStringValue("Select * from MstHoliday where Hdate = " & getDateFormatInCond(dtpDateSel.Value) & "", "", blnFound)
                If Not blnFound Then
                    strSQL = " insert into MstHoliday(Hdate,HolidayName) Values("
                    strSQL &= "'" & Format(dtpDateSel.Value, "yyyy/MM/dd") & "',"
                    strSQL &= "'" & txtHolidayName.Text & "')"
                Else
                    MsgBox("HolidayName Already exists", MsgBoxStyle.Information, Me.Text)
                    Exit Function
                End If
            ElseIf mode = "M" Then
                Call svtDate.getDataStringValue("Select * from MstHoliday where Hdate = " & getDateFormatInCond(dtpDateSel.Value) & "", "", blnFound)
                currentHolidayId = vsfDet.GetData(vsfDet.Row, 0)
                If blnFound Then
                    strSQL = " update MstHoliday set "
                    strSQL &= "HolidayName = '" & txtHolidayName.Text & "'"
                    strSQL &= "where Hdate = " & getDateFormatInCond(dtpDateSel.Value) & ""

                End If
            End If
            If Not clsSVT.execSQLStatement(strSQL) Then
                Exit Function
            End If

            If mode = "A" Then
                MsgBox("Data saved successfully....", MsgBoxStyle.Information, Me.Text)
            Else
                MsgBox("Updated successfully....", MsgBoxStyle.Information, Me.Text)
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Holiday")
        End Try
        Return True
    End Function

    Private Sub vsfCalender_DoubleClick(sender As Object, e As EventArgs) Handles vsfCalender.DoubleClick
        Try
            With vsfCalender
                If vsfCalender.SelectedDates.Count <= 0 Then Exit Sub
                If Not IsDate(.SelectedDates.ToArray.GetValue(0)) Then Exit Sub
                Call ShowPanel(.SelectedDates.ToArray.GetValue(0))
            End With

        Catch ex As Exception
            pnlSave.Visible = False
        End Try

    End Sub

    Private Sub ShowPanel(ByVal argDate As Date)
        Dim blnfound As Boolean
        clsSVT.getDataLongValue("Select ID from MstHoliday Where HDate = " & getDateFormatInCond(argDate), 0, blnfound)
        If blnfound Then
            mode = "M"
            txtHolidayName.Text = clsSVT.getDataStringValue("Select HolidayName from MstHoliday Where HDate = " & getDateFormatInCond(argDate), "", False)
            btnSave.Text = "&Update"
        Else
            mode = "A"
            txtHolidayName.Text = ""
            btnSave.Text = "&Save"
        End If
        If btnSave.Text = "&Update" Then
            DateName.SetToolTip(Me.btnSave, "Update : Alt+U")
        Else
            DateName.SetToolTip(Me.btnSave, "Save : Alt+S")
        End If
        dtpDateSel.Value = argDate
        btnDel.Visible = blnfound
        dtpDateSel.Enabled = Not blnfound
        pnlSave.Visible = True
        pnlSave.BringToFront()
    End Sub

    Private Sub vsfCalender_Leave(sender As Object, e As EventArgs) Handles vsfCalender.Leave
        If pnlSave.Visible = True Then
            dtpDateSel.Focus()
        ElseIf dtpDateSel.Enabled = False Then
            txtHolidayName.Focus()
        ElseIf btnDel.Visible = True Then
            btnDel.Focus()
        Else
            dtpFromdate.Focus()
        End If
    End Sub

    Private Sub btnRefresh_Leave(sender As Object, e As EventArgs) Handles btnRefresh.Leave
        If Not e.Equals(Keys.Tab) Then
            If vsfDet.Row <= 0 Then
                btnExport.Select()
            Else
                txtSearch.Focus()
            End If
        End If
    End Sub

    Private Sub vsfDet_KeyDown(sender As Object, e As KeyEventArgs) Handles vsfDet.KeyDown
        With vsfDet
            If e.KeyCode = Keys.Tab Then
                If .Row = .Rows.Count - 1 AndAlso .Col = .Cols.Count - 1 Then
                    btnExport.Focus()
                End If
            End If
                If e.KeyCode = Keys.Enter AndAlso .GetData(.Row, 0) Then
                Call ShowPanel(.GetData(.Row, 1))
            End If
        End With
    End Sub

    Private Sub txtHolidayName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtHolidayName.KeyPress
        If IsNumeric(txtHolidayName.Text) Then
            MsgBox("Please Enter holiday in words", MessageBoxIcon.Asterisk, "Holiday")
            txtHolidayName.Text = ""
            txtHolidayName.Focus()
        End If
    End Sub

    Private Sub btnClose_KeyUp(sender As Object, e As KeyEventArgs) Handles btnClose.KeyUp
        If e.KeyCode = Keys.Enter Then
            pnlSave.Visible = False
        End If
        If e.KeyCode = Keys.Alt And Keys.O Then
            btnClose.Focus()
            pnlSave.Visible = False
        End If
    End Sub

    Private Sub vsfDet_Leave(sender As Object, e As EventArgs) Handles vsfDet.Leave
        If Not e.Equals(Keys.Enter) Then
            If pnlSave.Visible = True Then
                txtHolidayName.Focus()
            End If
        End If

    End Sub

    Private Sub btnClose_Leave(sender As Object, e As EventArgs) Handles btnClose.Leave
        If vsfDet.Rows.Count <= 1 Then Exit Sub
        If Not e.Equals(Keys.Enter) Or Not e.Equals(Keys.Tab) Then
            With vsfDet
                .Focus()
                .Row = 1
                .Col = 0
            End With
            'vsfDet.Focus()
        End If
    End Sub

    Private Sub txtHolidayName_Leave(sender As Object, e As EventArgs) Handles txtHolidayName.Leave
        If Not e.Equals(Keys.Tab) Then
            If btnDel.Visible = True Then
                btnDel.Focus()
            Else
                btnSave.Focus()
            End If
        End If
    End Sub

    Private Sub vsfCalender_MouseHover(sender As Object, e As EventArgs) Handles vsfCalender.MouseHover
        DateName.SetToolTip(vsfCalender, "")
    End Sub

    Private Sub btnDel_Leave(sender As Object, e As EventArgs) Handles btnDel.Leave
        If Not Equals(Keys.Enter) Then
            Call loadgrid()
        End If
    End Sub

    Private Sub vsfDet_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles vsfDet.MouseDoubleClick
        If KeyPreview = False Then
            Exit Sub
        End If
        Dim ht = vsfDet.HitTest
        If ht.Row <= 0 Then
            Exit Sub
        End If
        If vsfDet.Rows.Count <= 1 Then
            MsgBox("Please add any date from calender", MsgBoxStyle.Information, Me.Text)
            pnlSave.Visible = False
            Exit Sub
        Else
            Call ShowPanel(vsfDet.GetData(vsfDet.Row, vDate))
        End If
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If Trim(txtSearch.Text) = String.Empty Then
            vsfSearch.SendToBack()
            vsfSearch.Visible = False
        Else
            vsfSearch.BringToFront()
            vsfSearch.Visible = True
        End If
        applyFilterFromGrid()
    End Sub

    Private Function applyFilterFromGrid() As Boolean
        Dim StrQry As String
        Dim dvSearch As DataView
        Dim dtSearch As DataTable
        Dim strSQL As String
        'For index As Int16 = 1 To vsfDet.Rows.Count - 1
        strSQL = "Select Id,Hdate as Date,HolidayName as Name from MstHoliday" 'where Id = " & vsfDet.GetData(index, 0) & ""

        dtSearch = clsSVT.getDATATable(strSQL)
            dvSearch = New DataView(dtSearch)
            vsfSearch.DataSource = dvSearch

            Call SetHideCol(vsfSearch, True, 0)
            applyFilterFromGrid = False
            StrQry = ""
            With vsfSearch
                'If txtSearch.Text = "" Then
                '    Exit Function
                'End If
                Try
                If txtSearch.Text = " " Then
                    dvSearch.RowFilter = "1 = 1"
                    Exit Function
                Else
                    StrQry = StrQry & " Convert(Date,'System.String')" & " like '" & txtSearch.Text & "*'"
                    StrQry = StrQry & " Or Name like '%" & txtSearch.Text & "%'"
                End If
                    dvSearch.RowFilter = StrQry
                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try
            End With
        'Next
        applyFilterFromGrid = True
    End Function

    Private Sub vsfSearch_DoubleClick(sender As Object, e As EventArgs) Handles vsfSearch.DoubleClick
        Call GetHolidayData()
    End Sub

    Private Sub vsfSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles vsfSearch.KeyUp
        If e.KeyCode = Keys.Enter Then
            If Trim(txtSearch.Text) = "" Then Exit Sub
            If vsfSearch.Row < 0 Then Exit Sub
            Call ShowPanel(vsfSearch.GetData(vsfSearch.Row, 1))
            'Call GetHolidayData()
            txtSearch.Text = ""
        End If
    End Sub

    Private Sub GetHolidayData()
        Dim intHId As Int32
        If vsfSearch.Rows.Count < 1 Then Exit Sub
        If vsfSearch.Row < 0 Then Exit Sub
        intHId = vsfSearch.GetData(vsfSearch.Row, 0)
        vsfSearch.Visible = False
        Call ShowPanel(vsfSearch.GetData(vsfSearch.Row, 1))
        txtSearch.Text = ""
        With vsfDet
            For i As Int32 = 1 To .Rows.Count - 1
                If .GetData(i, vId) = intHId Then
                    .Row = i
                    .Focus()
                    Exit Sub
                End If
            Next
        End With


        'Call retreiveData()
    End Sub

    'Public Function CalenderDayHL() As Boolean
    '    CalenderDayHL = False
    '    If vsfSearch.GetData(vsfSearch.Row, 0) = 

    '    CalenderDayHL = True
    'End Function

    Private Sub vsfSearch_BeforeEdit(sender As Object, e As RowColEventArgs) Handles vsfSearch.BeforeEdit
        e.Cancel = True
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Call ExportToXL(vsfDet)
    End Sub

    Private Sub btnFrmClose_Click(sender As Object, e As EventArgs) Handles btnFrmClose.Click
        Call frmMain.CloseSelectedTabPage(Me)
    End Sub

    Private Sub txtSearch_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSearch.KeyPress
        If e.KeyChar = "'" Then
            e.Handled = True
        End If
    End Sub

    Private Sub btnFrmClose_KeyUp(sender As Object, e As KeyEventArgs) Handles btnFrmClose.KeyUp
        If e.KeyCode = Keys.Alt And e.KeyCode = Keys.L Then
            If pnlSave.Visible = True Then
                Exit Sub
            Else
                Call frmMain.CloseSelectedTabPage(Me)
            End If
        Else
            Exit Sub
        End If
    End Sub

    Private Sub txtSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyUp
        If e.KeyCode = Keys.Return Then
            Call applyFilterFromGrid()
            Exit Sub
        End If

        If e.KeyCode = Keys.Down Then
            vsfSearch.Focus()
            Exit Sub
        End If

        If e.KeyCode = Keys.Enter Then
            If txtSearch.Text = String.Empty Then Exit Sub
            Call GetHolidayData()
        End If
    End Sub

    Private Sub btnExport_KeyUp(sender As Object, e As KeyEventArgs) Handles btnExport.KeyUp
        If e.KeyCode = Keys.Alt And e.KeyCode = Keys.E Then
            If vsfDet.Rows.Count < 1 Then
                Exit Sub
            Else
                Call ExportToXL(vsfDet)
            End If
        End If
    End Sub

    Private Sub btnFrmClose_KeyDown(sender As Object, e As KeyEventArgs) Handles btnFrmClose.KeyDown
        If e.KeyCode = Keys.Tab Then
            cboMonth.Focus()
        End If
    End Sub

    Private Sub btnExport_KeyDown(sender As Object, e As KeyEventArgs) Handles btnExport.KeyDown
        If e.KeyCode = Keys.Tab Then
            btnFrmClose.Select()
        End If
    End Sub

    Private Sub frmMstHoliday_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        txtSearch.Focus()
        Call showWaitCursor()
    End Sub
End Class
