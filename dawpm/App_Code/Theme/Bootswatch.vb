Public Class Theme
    Public Property name() As String
    Public Property description() As String
    Public Property thumbnail() As String
    Public Property preview() As String
    Public Property css() As String
    Public Property cssMin() As String
    Public Property cssCdn() As String
    Public Property less() As String
    Public Property lessVariables() As String
    Public Property scss() As String
    Public Property scssVariables() As String
End Class

Public Class Bootswatch
    Public Property version() As String
    Public Property themes() As List(Of Theme)
End Class

