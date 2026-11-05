
Imports System.Drawing
Imports System.Windows.Forms

Namespace CustomButton
    Partial Public Class CustomButton
        Inherits Button
        Public Sub New()
            'InitializeComponent()
            'Set default property values for the button during start up
            SetNormalValues()
        End Sub
        ''' <summary>
        ''' To Set button properties when not active.i.e when button not in focus.
        ''' </summary>
        Private Sub SetNormalValues()
            Me.Font = New Font("Verdana", 8.0F, FontStyle.Bold)
            Me.BackColor = Color.Gray
            Me.ForeColor = Color.White
            Me.Margin = New Padding(4, 1, 4, 1)
            Me.Padding = New Padding(4)
            'Me.MinimumSize = New Size(150, 35)
            Me.Cursor = Cursors.Arrow
        End Sub

        ''' <summary>
        ''' Set attributes to highlight button when it is under focus/active.
        ''' Change the cursor also as Hand type
        ''' </summary>
        Private Sub SetValuesOnFocus()
            'Increase the font size and colors on focus
            Me.Font = New Font("Verdana", 9.0F, FontStyle.Bold)
            Me.BackColor = Color.FromArgb(113, 188, 120)
            Me.ForeColor = Color.White
            'Set the cursor to Hand type
            Me.Cursor = Cursors.Hand
        End Sub


        ''' <summary>
        ''' Default handler.Nothing to do here since we don't need to repaint the button.
        ''' </summary>
        ''' <param name="pe"></param>
        Protected Overrides Sub OnPaint(ByVal pe As PaintEventArgs)
            MyBase.OnPaint(pe)
        End Sub

        ''' <summary>
        ''' Event handler which call SetValuesOnFocus() method to give apecial
        ''' effect to button while active
        ''' </summary>
        ''' <param name="e"></param>
        Protected Overrides Sub OnMouseHover(ByVal e As EventArgs)
            MyBase.OnMouseHover(e)
            SetValuesOnFocus()
        End Sub

        ''' <summary>
        ''' Event handler which call SetNormalValues() method to set back the button
        ''' to normal state
        ''' </summary>
        ''' <param name="e"></param>
        Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
            MyBase.OnMouseLeave(e)
            If Me.Focused = True Then
                SetValuesOnFocus()
            Else
                SetNormalValues()
            End If
        End Sub

        Protected Overrides Sub OnEnter(ByVal e As EventArgs)
            MyBase.OnEnter(e)
            SetValuesOnFocus()
        End Sub

        Protected Overrides Sub OnLeave(ByVal e As EventArgs)
            MyBase.OnLeave(e)
            SetNormalValues()
        End Sub

        Protected Overrides Sub OnGotFocus(ByVal e As System.EventArgs)
            MyBase.OnGotFocus(e)
            SetValuesOnFocus()
        End Sub

        Protected Overrides Sub OnLostFocus(ByVal e As System.EventArgs)
            MyBase.OnLostFocus(e)
            SetNormalValues()
        End Sub

    End Class

End Namespace