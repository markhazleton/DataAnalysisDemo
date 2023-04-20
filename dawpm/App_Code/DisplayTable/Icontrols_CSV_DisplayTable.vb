
Public Interface Icontrols_CSV_DisplayTable
    Sub BuildDataTable(ByVal filePath As String)
End Interface

Public Interface Icontrols_Chart
    Sub BuildChart(ByVal Configuration As DataGridVisualization, ByRef SourceData As DataGrid)
End Interface

Public Interface Icontrols_ChartConfiguration
    Sub PutChartConfiguration(ByVal Configuration As DataGridVisualization)
    Function GetChartConfiguration() As DataGridVisualization
    Sub SetSourceData(SourceData as DataGrid)

End Interface
