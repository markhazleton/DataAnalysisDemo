Imports System.Data

Public Class DataGrid
    Property Title As String
    Property GridColumns As New ColumnColl
    Property GridRows As New RowColl
    Public Class GridColumn
        Property ColumnDisplayFormat As DisplayFormat
        Property DisplayName As String
        Property SourceName As String
        Property Index As Integer
        Property MinValue As String
        Property MaxValue As String
        Property UniqueValues As Integer
        Property MostCommon As String
        Property LeastCommon As String
        Property ColumnValues As new List(Of String)

        Private ColDictionary As new Dictionary(Of String, Integer)
        Public Sub UpdateDictionary(ByVal ColValue As String)
            If ColDictionary.ContainsKey(wpm_GetStringValue(ColValue)) Then
                Dim value As Integer
                If (ColDictionary.TryGetValue(wpm_GetStringValue(ColValue), value)) Then
                    ColDictionary(wpm_GetStringValue(ColValue)) = value + 1
                End If
            Else
                ColDictionary.Add(wpm_GetStringValue(ColValue), 1)
                UniqueValues = UniqueValues + 1
            End If
        End Sub
        Public Sub SetCommonValues()
            MostCommon = (From entry In ColDictionary Order By entry.Value Ascending Select String.Format("'{0}' in {1} rows", entry.key, entry.value)).Last()
            LeastCommon = (From entry In ColDictionary Order By entry.Value Ascending Select String.Format("'{0}' in {1} rows", entry.key, entry.value)).First()
            ColumnValues.Clear
            ColumnValues.AddRange((From entry In ColDictionary Order By entry.Key Ascending Select entry.Key).ToArray())
        End Sub
    End Class
    Public Class ColumnColl
        Inherits List(Of GridColumn)
    End Class
    Public Class ColumnValue
        Property Value As String
        Property Count As Integer
    End Class
    Public Class GridRow
        Property name As String
        Property Value As New List(Of String)
    End Class
    Public Class RowColl
        Inherits List(Of GridRow)
    End Class
End Class
