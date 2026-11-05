
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Globalization
Imports System.Linq
Imports System.Windows.Forms

Public Class ControlMoverOrResizer
    Private Shared _moving As Boolean
    Private Shared _cursorStartPoint As Point
    Private Shared _moveIsInterNal As Boolean
    Private Shared _resizing As Boolean
    Private Shared _currentControlStartSize As Size
    Friend Shared Property MouseIsInLeftEdge() As Boolean
        Get
            Return m_MouseIsInLeftEdge
        End Get
        Set(value As Boolean)
            m_MouseIsInLeftEdge = value
        End Set
    End Property
    Private Shared m_MouseIsInLeftEdge As Boolean
    Friend Shared Property MouseIsInRightEdge() As Boolean
        Get
            Return m_MouseIsInRightEdge
        End Get
        Set(value As Boolean)
            m_MouseIsInRightEdge = value
        End Set
    End Property
    Private Shared m_MouseIsInRightEdge As Boolean
    Friend Shared Property MouseIsInTopEdge() As Boolean
        Get
            Return m_MouseIsInTopEdge
        End Get
        Set(value As Boolean)
            m_MouseIsInTopEdge = value
        End Set
    End Property
    Private Shared m_MouseIsInTopEdge As Boolean
    Friend Shared Property MouseIsInBottomEdge() As Boolean
        Get
            Return m_MouseIsInBottomEdge
        End Get
        Set(value As Boolean)
            m_MouseIsInBottomEdge = value
        End Set
    End Property
    Private Shared m_MouseIsInBottomEdge As Boolean

    Friend Enum MoveOrResize
        Move
        Resize
        MoveAndResize
    End Enum

    Friend Shared Property WorkType() As MoveOrResize
        Get
            Return m_WorkType
        End Get
        Set(value As MoveOrResize)
            m_WorkType = value
        End Set
    End Property
    Private Shared m_WorkType As MoveOrResize

    Friend Shared Sub Init(control As Control)
        Init(control, control)
    End Sub

    Friend Shared Sub Init(control As Control, container As Control)
        _moving = False
        _resizing = False
        _moveIsInterNal = False
        _cursorStartPoint = Point.Empty
        MouseIsInLeftEdge = False
        MouseIsInLeftEdge = False
        MouseIsInRightEdge = False
        MouseIsInTopEdge = False
        MouseIsInBottomEdge = False
        WorkType = MoveOrResize.MoveAndResize
        AddHandler control.MouseDown, Sub(sender, e) StartMovingOrResizing(control, e)
        AddHandler control.MouseUp, Sub(sender, e) StopDragOrResizing(control)
        AddHandler control.MouseMove, Sub(sender, e) MoveControl(container, e)
    End Sub

    Private Shared Sub UpdateMouseEdgeProperties(control As Control, mouseLocationInControl As Point)
        If WorkType = MoveOrResize.Move Then
            Return
        End If
        MouseIsInLeftEdge = Math.Abs(mouseLocationInControl.X) <= 2
        MouseIsInRightEdge = Math.Abs(mouseLocationInControl.X - control.Width) <= 2
        MouseIsInTopEdge = Math.Abs(mouseLocationInControl.Y) <= 2
        MouseIsInBottomEdge = Math.Abs(mouseLocationInControl.Y - control.Height) <= 2
    End Sub

    Private Shared Sub UpdateMouseCursor(control As Control)
        If WorkType = MoveOrResize.Move Then
            Return
        End If
        If MouseIsInLeftEdge Then
            If MouseIsInTopEdge Then
                control.Cursor = Cursors.SizeNWSE
            ElseIf MouseIsInBottomEdge Then
                control.Cursor = Cursors.SizeNESW
            Else
                control.Cursor = Cursors.SizeWE
            End If
        ElseIf MouseIsInRightEdge Then
            If MouseIsInTopEdge Then
                control.Cursor = Cursors.SizeNESW
            ElseIf MouseIsInBottomEdge Then
                control.Cursor = Cursors.SizeNWSE
            Else
                control.Cursor = Cursors.SizeWE
            End If
        ElseIf MouseIsInTopEdge OrElse MouseIsInBottomEdge Then
            control.Cursor = Cursors.SizeNS
        Else
            control.Cursor = Cursors.[Default]
        End If
    End Sub

    Private Shared Sub StartMovingOrResizing(control As Control, e As MouseEventArgs)
        If _moving OrElse _resizing Then
            Return
        End If
        If WorkType <> MoveOrResize.Move AndAlso (MouseIsInRightEdge OrElse MouseIsInLeftEdge OrElse MouseIsInTopEdge OrElse MouseIsInBottomEdge) Then
            _resizing = True
            _currentControlStartSize = control.Size
        ElseIf WorkType <> MoveOrResize.Resize Then
            _moving = True
            control.Cursor = Cursors.Hand
        End If
        _cursorStartPoint = New Point(e.X, e.Y)
        control.Capture = True
    End Sub

    Private Shared Sub MoveControl(control As Control, e As MouseEventArgs)
        If Not _resizing AndAlso Not _moving Then
            UpdateMouseEdgeProperties(control, New Point(e.X, e.Y))
            UpdateMouseCursor(control)
        End If
        If _resizing Then
            If MouseIsInLeftEdge Then
                If MouseIsInTopEdge Then
                    control.Width -= (e.X - _cursorStartPoint.X)
                    control.Left += (e.X - _cursorStartPoint.X)
                    control.Height -= (e.Y - _cursorStartPoint.Y)
                    control.Top += (e.Y - _cursorStartPoint.Y)
                ElseIf MouseIsInBottomEdge Then
                    control.Width -= (e.X - _cursorStartPoint.X)
                    control.Left += (e.X - _cursorStartPoint.X)
                    control.Height = (e.Y - _cursorStartPoint.Y) + _currentControlStartSize.Height
                Else
                    control.Width -= (e.X - _cursorStartPoint.X)
                    control.Left += (e.X - _cursorStartPoint.X)
                End If
            ElseIf MouseIsInRightEdge Then
                If MouseIsInTopEdge Then
                    control.Width = (e.X - _cursorStartPoint.X) + _currentControlStartSize.Width
                    control.Height -= (e.Y - _cursorStartPoint.Y)

                    control.Top += (e.Y - _cursorStartPoint.Y)
                ElseIf MouseIsInBottomEdge Then
                    control.Width = (e.X - _cursorStartPoint.X) + _currentControlStartSize.Width
                    control.Height = (e.Y - _cursorStartPoint.Y) + _currentControlStartSize.Height
                Else
                    control.Width = (e.X - _cursorStartPoint.X) + _currentControlStartSize.Width
                End If
            ElseIf MouseIsInTopEdge Then
                control.Height -= (e.Y - _cursorStartPoint.Y)
                control.Top += (e.Y - _cursorStartPoint.Y)
            ElseIf MouseIsInBottomEdge Then
                control.Height = (e.Y - _cursorStartPoint.Y) + _currentControlStartSize.Height
            Else
                StopDragOrResizing(control)
            End If
        ElseIf _moving Then
            _moveIsInterNal = Not _moveIsInterNal
            If Not _moveIsInterNal Then
                Dim x As Integer = (e.X - _cursorStartPoint.X) + control.Left
                Dim y As Integer = (e.Y - _cursorStartPoint.Y) + control.Top
                control.Location = New Point(x, y)
            End If
        End If
    End Sub

    Private Shared Sub StopDragOrResizing(control As Control)
        _resizing = False
        _moving = False
        control.Capture = False
        UpdateMouseCursor(control)
    End Sub

#Region "Save And Load"

    Private Shared Function GetAllChildControls(control As Control, list As List(Of Control)) As List(Of Control)
        Dim controls As List(Of Control) = control.Controls.Cast(Of Control)().ToList()
        list.AddRange(controls)
        Return controls.SelectMany(Function(ctrl) GetAllChildControls(ctrl, list)).ToList()
    End Function

    Friend Shared Function GetSizeAndPositionOfControlsToString(container As Control) As String
        Dim controls As New List(Of Control)()
        GetAllChildControls(container, controls)
        Dim cultureInfo As New CultureInfo("en")
        Dim info As String = String.Empty
        For Each control As Control In controls
            info += control.Name + ":" + control.Left.ToString(cultureInfo) + "," + control.Top.ToString(cultureInfo) + "," + control.Width.ToString(cultureInfo) + "," + control.Height.ToString(cultureInfo) + "*"
        Next
        Return info
    End Function
    Friend Shared Sub SetSizeAndPositionOfControlsFromString(container As Control, controlsInfoStr As String)
        Dim controls As New List(Of Control)()
        GetAllChildControls(container, controls)
        Dim cr() As Char = "*"
        Dim sr() As Char = ":"
        Dim pr() As Char = ","
        Dim controlsInfo As String() = controlsInfoStr.Split(cr, StringSplitOptions.RemoveEmptyEntries)
        Dim controlsInfoDictionary As New Dictionary(Of String, String)()
        For Each controlInfo As String In controlsInfo
            Dim info As String() = controlInfo.Split(sr, StringSplitOptions.RemoveEmptyEntries)
            controlsInfoDictionary.Add(info(0), info(1))
        Next
        For Each control As Control In controls
            Dim propertiesStr As String = ""
            controlsInfoDictionary.TryGetValue(control.Name, propertiesStr)
            Dim properties As String() = propertiesStr.Split(pr, StringSplitOptions.RemoveEmptyEntries)
            If properties.Length = 4 Then
                control.Left = Integer.Parse(properties(0))
                control.Top = Integer.Parse(properties(1))
                control.Width = Integer.Parse(properties(2))
                control.Height = Integer.Parse(properties(3))
            End If
        Next
    End Sub

#End Region
End Class

