Imports System.IO
Imports System.Web.UI.DataVisualization.Charting

Public Class DataGridVisualization
    Public Property Name As String
    Public Property CSVFile As String
    Public Property ChartTitles As New List(Of String)
    Public Property AggregatorName As String
    Public Property rendererName As String
    Public Property ChartType As SeriesChartType
    Public Property ChartStyle As String
    Public Property ChartPalette As String
    Public Property ChartWidth As Int16
    Public Property ChartHeight As Int16
    Public Property BackgroundImage As String
    Public Property Cols As New List(Of DataGrid.GridColumn)
    Public Property Rows As New List(Of DataGrid.GridColumn)
    Public Property Vals As New List(Of DataGrid.GridColumn)
    Property FilterColumn As New DataGrid.GridColumn
    Public Property IncludeValues As New List(Of DataGrid.GridColumn)
    Public Property ExcluedValues As New List(Of DataGrid.GridColumn)
    Public Property SeriesDataList As New List(Of SeriesData)
    Private ChartData As New DataGrid
    Public ReadOnly Property ChartDataGrid As DataGrid
        Get
            Return ChartData
        End Get
    End Property

    Public Sub ReturnChartObject(ByRef yourchart As Chart, ByRef SourceData As DataGrid)
        Try
            ChartData.GridColumns.AddRange((SourceData.GridColumns).ToList())
        Catch ex As Exception
            ApplicationLogging.ErrorLog("ChartConfiguration.ReturnChartObject", ex.ToString)
        End Try

        For Each myCol In SourceData.GridColumns
            If FilterColumn.SourceName = myCol.SourceName Then
                FilterColumn = myCol
            End If
            If Cols(0).SourceName = myCol.SourceName Then
                Cols(0) = myCol
            End If
            If Rows(0).SourceName = myCol.SourceName Then
                Rows(0) = myCol
            End If
            If Vals(0).SourceName = myCol.SourceName Then
                Vals(0) = myCol
            End If
        Next

        For Each myRow In SourceData.GridRows
            If FilterColumn.ColumnValues.Count > 0 Then
                If FilterColumn.ColumnValues.Contains(myRow.Value(FilterColumn.Index)) AndAlso
                    Cols(0).ColumnValues.Contains(myRow.Value(Cols(0).Index)) AndAlso
                     Rows(0).ColumnValues.Contains(myRow.Value(Rows(0).Index)) AndAlso
                      wpm_GetDBDouble(myRow.Value(Vals(0).Index)) > 0 Then
                    ChartData.GridRows.Add(myRow)
                End If
            Else
                If Cols(0).ColumnValues.Count > 0 Then
                    If Cols(0).ColumnValues.Contains(myRow.Value(Cols(0).Index)) AndAlso
                         Rows(0).ColumnValues.Contains(myRow.Value(Rows(0).Index)) AndAlso
                          wpm_GetDBDouble(myRow.Value(Vals(0).Index)) > 0 Then
                        ChartData.GridRows.Add(myRow)
                    End If
                Else
                    If Rows(0).ColumnValues.Count > 0 Then
                        If Rows(0).ColumnValues.Contains(myRow.Value(Rows(0).Index)) AndAlso
                              wpm_GetDBDouble(myRow.Value(Vals(0).Index)) > 0 Then
                            ChartData.GridRows.Add(myRow)
                        End If
                    Else
                        If wpm_GetDBDouble(myRow.Value(Vals(0).Index)) > 0 Then
                            ChartData.GridRows.Add(myRow)
                        End If
                    End If
                End If
            End If
        Next

        If ChartData.GridRows.Count > 0 Then
            For Each SelectedYColumn In Rows(0).ColumnValues
                Dim thisSeriesData = (From s In (From i In ChartData.GridRows Where Rows(0).ColumnValues.Contains(i.Value(Rows(0).Index))).ToList() Where s.Value(Rows(0).Index) = SelectedYColumn)
                Dim AxisLabel As String = String.Empty
                Dim newSeriesData As New SeriesData
                Dim iRunningScore As Double
                Dim iCount As Integer
                newSeriesData = New SeriesData
                For Each Axis In Cols(0).ColumnValues
                    If Axis = String.Empty Then
                        AxisLabel = "Not Answered"
                    Else
                        AxisLabel = Axis
                    End If
                    iCount = 0
                    iRunningScore = 0
                    For Each AxisData In (From i In thisSeriesData Where i.Value(Cols(0).Index) = Axis).ToList
                        iCount += 1
                        iRunningScore = iRunningScore + wpm_GetDBDouble(AxisData.Value(Vals(0).Index))
                    Next
                    If iCount > 0 Then
                        Select Case AggregatorName
                            Case "Sum"
                                newSeriesData.Add(New PointData With {.PointLabel = AxisLabel, .PointValue = Math.Round(iRunningScore, 2)})
                            Case "Average"
                                If iCount > 0 Then
                                    newSeriesData.Add(New PointData With {.PointLabel = AxisLabel, .PointValue = Math.Round(iRunningScore / iCount, 2)})
                                Else
                                    newSeriesData.Add(New PointData With {.PointLabel = AxisLabel, .PointValue = 0})
                                End If
                            Case "Count"
                                newSeriesData.Add(New PointData With {.PointLabel = AxisLabel, .PointValue = iCount})
                            Case Else
                                newSeriesData.Add(New PointData With {.PointLabel = AxisLabel, .PointValue = iRunningScore})
                        End Select
                    End If
                Next
                If SelectedYColumn = String.Empty Then
                    newSeriesData.SeriesName = "Unknown"
                Else
                    newSeriesData.SeriesName = SelectedYColumn
                End If
                If newSeriesData.Count > 0 Then
                    SeriesDataList.Add(newSeriesData)
                End If
            Next
            With yourchart
                .Legends.Clear()
                .Series.Clear()
                .ChartAreas.Clear()
                .Titles.Clear()

                If Not String.IsNullOrEmpty(BackgroundImage) Then
                    .BackImage = BackgroundImage
                End If
                If ChartTitles.Count = 0 Then
                    ChartTitles.Add(Name)
                End If
                For Each i In ChartTitles
                    If Not String.IsNullOrEmpty(i) Then
                        Dim myTitle As New Title() With {.Text = i, .Font = New System.Drawing.Font("Verdana", 12, Drawing.FontStyle.Bold)}
                        .Titles.Add(myTitle)
                    End If
                Next
                .ChartAreas.Add(New ChartArea With {.Name = Name, .BackColor = Drawing.Color.White})
                .ChartAreas(0).AxisX.Interval = 1
                For Each s In SeriesDataList
                    Try
                        .Series.Add(s.GetSeriesData)
                    Catch
                        ApplicationLogging.ErrorLog("ChartConfiguration.ReturnChartObject", "Faild to add series data")
                    End Try
                Next
                .Legends.Add(New Legend With {.Name = "Master Legend"})
                If ChartWidth > 0 Then
                    .Width = ChartWidth
                End If
                If ChartHeight > 0 Then
                    .Height = ChartHeight
                End If
                Try
                    If ChartPalette = String.Empty Then
                        ChartPalette = ChartColorPalette.BrightPastel
                    End If
                    .Palette = CType(ChartPalette, ChartColorPalette)
                Catch ex As Exception
                    .Palette = ChartColorPalette.BrightPastel
                End Try

                Try
                    Select Case ChartType
                        Case DataVisualization.Charting.SeriesChartType.StackedBar
                            ChartStyle = "2D"
                        Case Else
                            ' Do Nothing
                    End Select
                    If ChartStyle = "3D" Then
                        .ChartAreas(0).Area3DStyle.Enable3D = True
                    Else
                        .ChartAreas(0).Area3DStyle.Enable3D = False
                    End If

                    For Each s As Series In .Series
                        s.ChartType = ChartType
                    Next

                Catch ex As Exception
                    ApplicationLogging.ErrorLog("ChartConfiguration.ReturnChartObject", ex.ToString)
                End Try




                Dim mytw As System.IO.TextWriter
                mytw = New StreamWriter(HttpContext.Current.Server.MapPath("~/App_Data/log/TestChartConfig.txt"))
                Using myWriter As New HtmlTextWriter(mytw)
                    Try
                        yourchart.Style.Clear()
                        yourchart.RenderControl(myWriter)
                    Catch ex As Exception
                        ApplicationLogging.ErrorLog("ChartConfiguration.ReturnChartObject-Errror on Test Render Chart", ex.ToString)
                        yourchart.Palette = ChartColorPalette.None
                        For Each s As Series In yourchart.Series
                            s.ChartType = SeriesChartType.Column
                        Next
                        For Each area In yourchart.ChartAreas
                            area.Area3DStyle.Enable3D = False
                        Next
                    End Try
                End Using
                mytw.Close()
                mytw.Dispose()
            End With
        End If
    End Sub
    Public Function GetPivotParm() As String
        Return (String.Format("{0} {1} {2} {3} {4} {5} {6} ",
                       GetColumnNameString(Cols, "cols"),
                       GetColumnNameString(Rows, "rows"),
                       GetColumnNameString(Vals, "vals"),
                       GetColumnValueString(ExcluedValues, "exclusions"),
                       GetColumnValueString(IncludeValues, "inclusions"),
                       GetStringItem(AggregatorName, "aggregatorName"),
                       GetStringItem(rendererName, "rendererName")
                       )) & "derivedAttributes: {},"
    End Function


    Private Shared Function GetStringItem(ByVal myItem As String, ByVal myName As String) As String
        Dim mySB As New StringBuilder
        If Not String.IsNullOrEmpty(myItem) Then
            mySB.Append(String.Format("{0}:", myName))
            mySB.Append(String.Format("""{0}""", myItem))
            mySB.AppendLine(",")
        End If
        Return mySB.ToString()

    End Function
    Private Shared Function GetColumnNameString(ByVal myStringList As List(Of DataGrid.GridColumn), ByVal sName As String) As String
        Dim mySB As New StringBuilder
        If myStringList.Count > 0 Then
            mySB.Append(String.Format("{0}:[", sName))
            For Each myVal In myStringList
                If myVal.SourceName = myStringList.Last.SourceName Then
                    mySB.Append(String.Format("""{0}""", myVal.SourceName))
                Else
                    mySB.Append(String.Format("""{0}"",", myVal.SourceName))
                End If
            Next
            mySB.AppendLine("],")
        End If
        Return mySB.ToString()
    End Function
    Private Shared Function GetColumnValueString(ByVal myValueList As List(Of DataGrid.GridColumn), ByVal sName As String) As String
        Dim mySB As New StringBuilder
        If myValueList.Count > 0 Then
            mySB.Append(String.Format("""{0}"":", sName.Replace("""", String.Empty).Trim) & "{")
            For Each myVal In myValueList
                mySB.Append(String.Format("""{0}"":[", myVal.SourceName.Replace("""", String.Empty).Trim))
                For Each myItem In myVal.ColumnValues
                    mySB.Append(String.Format("""{0}"",", myItem.Replace("""", String.Empty).Trim))
                Next
                mySB.AppendLine("],")
            Next
            mySB.AppendLine("},")
        End If
        Return mySB.ToString()
    End Function

End Class
