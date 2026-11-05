Imports System.IO
Imports System.Collections.ObjectModel
Namespace My

    ' The following events are available for MyApplication:
    ' 
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.

    Partial Friend Class MyApplication

        Private Sub MyApplication_UnhandledException1(sender As Object, e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException

            MessageBox.Show(e.Exception.Message & vbCrLf & "Continue?", "Continue?", MessageBoxButtons.OK, MessageBoxIcon.Error)
            RecordLog(e.Exception)

        End Sub

        Protected Overrides Function OnInitialize(commandLineArgs As ReadOnlyCollection(Of String)) As Boolean
            Me.MinimumSplashScreenDisplayTime = 1000
            Return MyBase.OnInitialize(commandLineArgs)
        End Function

    End Class


End Namespace
