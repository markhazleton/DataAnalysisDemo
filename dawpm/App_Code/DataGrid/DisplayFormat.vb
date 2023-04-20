Imports Microsoft.VisualBasic
Imports System.Data

Public Enum DisplayFormat
    Number
    Currency
    Text
    Fraction
    Thumbnail
End Enum

Public Module GenericParserDataGrid
    Public Function LoadFromCsvFile(ByVal CsvFilePath As String) As DataGrid
        Dim myDataGrid As New DataGrid
        Using parser = New GenericParsing.GenericParserAdapter()
            parser.SetDataSource(CsvFilePath)
            parser.ColumnDelimiter = ","
            parser.FirstRowHasHeader = True
            parser.SkipStartingDataRows = 10
            parser.MaxBufferSize = 4096
            parser.MaxRows = 50000
            parser.TextQualifier = """"
            Dim myDT = parser.GetDataTable()
            myDataGrid.GridColumns.Clear()
            myDataGrid.GridRows.Clear()
            For ColIndex = 0 To myDT.Columns.Count() - 1
                myDataGrid.GridColumns.Add(New DataGrid.GridColumn With {.DisplayName = myDT.Columns(ColIndex).ColumnName,
                                                              .SourceName = myDT.Columns(ColIndex).ColumnName,
                                                              .Index = ColIndex,
                                                              .MinValue = "ZZZ",
                                                              .MaxValue = "000",
                                                              .UniqueValues = 0,
                                                              .MostCommon = String.Empty,
                                                              .LeastCommon = String.Empty})
            Next

            For Each myRow As DataRow In myDT.Rows
                Dim myDataRow As New DataGrid.GridRow With {.name = String.Empty}
                For Each myCol As DataGrid.GridColumn In myDataGrid.GridColumns
                    If String.IsNullOrEmpty(myCol.MaxValue) Then
                        myCol.MaxValue = myRow.Item(myCol.SourceName)
                        myCol.MinValue = myRow.Item(myCol.SourceName)
                    End If
                    myCol.UpdateDictionary(myRow.Item(myCol.SourceName))
                    If myCol.MinValue > myRow.Item(myCol.SourceName) Then
                        myCol.MinValue = myRow.Item(myCol.SourceName)
                    End If
                    If myCol.MaxValue < myRow.Item(myCol.SourceName) Then
                        myCol.MaxValue = myRow.Item(myCol.SourceName)
                    End If
                    If String.IsNullOrEmpty(myRow.Item(myCol.SourceName)) Or IsNumeric(myRow.Item(myCol.SourceName)) Then
                        myCol.ColumnDisplayFormat = DisplayFormat.Number
                    Else
                        myCol.ColumnDisplayFormat = DisplayFormat.Text
                    End If
                    myDataRow.Value.Add(myRow.Item(myCol.SourceName))
                Next
                myDataGrid.GridRows.Add(myDataRow)
            Next
        End Using
        For Each myCol In myDataGrid.GridColumns
            myCol.SetCommonValues()
        Next
        Return myDataGrid
    End Function

    Public Function GetCSV(ByRef myDataGrid As DataGrid) As String
        Dim csv As New StringBuilder
        For Each column In myDataGrid.GridColumns
            'Add the Header row for CSV file.
            csv.Append(column.DisplayName + ","c)
        Next
        'Add new line.
        csv.Append(vbCr & vbLf)
        For Each row In myDataGrid.GridRows
            For Each myValue In row.Value
                'Add the Data rows.
                csv.Append((myValue).Replace(",", ";") + ","c)
            Next
            'Add new line.
            csv.Append(vbCr & vbLf)
        Next
        Return csv.ToString()
    End Function
End Module
