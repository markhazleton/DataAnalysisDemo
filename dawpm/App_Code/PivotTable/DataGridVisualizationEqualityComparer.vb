
Public Class DataGridVisualizationEqualityComparer
    Inherits EqualityComparer(Of DataGridVisualization)

    Public Overloads Overrides Function Equals(ByVal x As DataGridVisualization, _
           ByVal y As DataGridVisualization) As Boolean
        If x Is Nothing Or y Is Nothing Then
            Return False
        ElseIf (x.Name = y.Name) And (x.CSVFile = y.CSVFile) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Overloads Overrides Function _
           GetHashCode(ByVal obj As DataGridVisualization) As Integer
        Return obj.ToString.GetHashCode
    End Function

End Class
