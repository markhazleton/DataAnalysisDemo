Imports System.Data
Imports System.Web.UI.DataVisualization.Charting

Partial Class Chart
    Inherits dawpmPage
    Public mySourceData As new DataGrid
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ddlConfig.Items.AddRange((From i In PivotParmList.GetConfigLookup(ActiveCSV) Select New ListItem With {.Text = i, .Value = i}).ToArray())
            If ddlConfig.Items.Count < 1 Then
                ddlConfig.Items.Add(New ListItem With {.Text = ActiveCSV, .Value = ActiveCSV})
                tbName.Enabled = False
            Else
                tbName.Enabled = True
            End If

            If ddlConfig.SelectedValue = ActiveCSV Then
                cmd_Delete.Enabled = False
                cmd_Delete.Visible = False
            Else
                cmd_Delete.Enabled = True
                cmd_Delete.Visible = True
            End If
            ddlConfig.SelectedValue = ActiveCSV

            hfSeries.Value = ""
            hfAxis.Value = ""
            hfFilter.Value = ""
            tbTitle.Text = ""

            ddlChartType.DataSource = CType([Enum].GetNames(GetType(SeriesChartType)), String())
            ddlChartType.DataBind()
            ddlChartType.SelectedValue = "StackedColumn"

            ddlChartPalette.DataSource = CType([Enum].GetNames(GetType(ChartColorPalette)), String())
            ddlChartPalette.DataBind()
            ddlChartPalette.SelectedValue = "BrightPastel"

            ddlCalc.Items.Clear()
            ddlCalc.Items.Add(New ListItem With {.Text = "Average", .Value = "Average"})
            ddlCalc.Items.Add(New ListItem With {.Text = "Sum", .Value = "Sum", .Selected = True})
            ddlCalc.Items.Add(New ListItem With {.Text = "Count", .Value = "Count"})
            ddlCalc.SelectedValue = "Count"

            ResetForm(ActiveCSV)

        End If
    End Sub
    Public Function SetData() As DataGridVisualization
        dim myChartConfig = PivotParmList.GetConfiguration(ActiveCSV, ddlConfig.SelectedValue)
        cmd_GetCSV.Text = ActiveCSV

        Try
            MySourceData = GenericParserDataGrid.LoadFromCsvFile(httpcontext.Current.Server.MapPath(ActiveCsvPath))
            If MySourceData.GridRows.Count > 0 Then
                ddlValueColumn.Items.Clear()
                For Each c In (From i In MySourceData.GridColumns Where i.ColumnDisplayFormat <> DisplayFormat.Text Select i.DisplayName Distinct)
                    ddlValueColumn.Items.Add(c)
                Next
                ddlAxis.Items.Clear()
                ddlFilter.Items.Clear()
                ddlSeries.Items.Clear()
                For Each c In (From i In MySourceData.GridColumns Where i.ColumnDisplayFormat <> DisplayFormat.Number Select i.DisplayName)
                    ddlAxis.Items.Add(c)
                    ddlSeries.Items.Add(c)
                    ddlFilter.Items.Add(c)
                Next

                ddlValueColumn.SelectedValue = myChartConfig.Vals(0).SourceName
                ddlSeries.SelectedValue = myChartConfig.Rows(0).SourceName
                ddlAxis.SelectedValue = myChartConfig.Cols(0).SourceName
                ddlFilter.SelectedValue = myChartConfig.FilterColumn.SourceName

                hfSeries.Value = ddlSeries.SelectedValue
                hfAxis.Value = ddlAxis.SelectedValue
                hfFilter.Value = ddlFilter.SelectedValue


                SetupColumnSelection(myChartConfig.Rows, MySourceData, ddlSeries, cbSeries, ddlSeries.SelectedValue)
                SetupColumnSelection(myChartConfig.Cols, MySourceData, ddlAxis, cbAxis, ddlAxis.SelectedValue)

                Dim myFilerList = New List(Of DataGrid.GridColumn)
                myFilerList.Add(myChartConfig.FilterColumn)
                SetupColumnSelection(myFilerList, MySourceData, ddlFilter, cbFilter, ddlFilter.SelectedValue)

            End If
        Catch ex As Exception
            ApplicationLogging.ErrorLog(ex.ToString, "Chart.SetData")
        End Try
        Return myChartConfig
    End Function
    Public Sub DrawChart()

        Dim myChartConfig = PivotParmList.GetConfiguration(ActiveCSV, ddlConfig.SelectedValue)

        MySourceData = GenericParserDataGrid.LoadFromCsvFile(httpcontext.Current.Server.MapPath(ActiveCsvPath))

        For i = 0 To MySourceData.GridColumns.Count - 1
            If MySourceData.GridColumns(i).DisplayName = ddlValueColumn.SelectedValue Then
                If myChartConfig.Vals.Count = 0 Then
                    myChartConfig.Vals.Add(New DataGrid.GridColumn)
                End If
                myChartConfig.Vals(0).Index = i
                myChartConfig.Vals(0).SourceName = ddlValueColumn.SelectedValue
            End If
            If MySourceData.GridColumns(i).DisplayName = ddlAxis.SelectedValue Then
                If myChartConfig.Cols.Count = 0 Then
                    myChartConfig.Cols.Add(New DataGrid.GridColumn)
                End If
                myChartConfig.Cols(0).Index = i
                myChartConfig.Cols(0).SourceName = ddlAxis.SelectedValue
                myChartConfig.Cols(0).ColumnValues.Clear()
                For ic = 0 To cbAxis.Items.Count - 1
                    If cbAxis.Items(ic).Selected = True Then
                        myChartConfig.Cols(0).ColumnValues.Add(cbAxis.Items(ic).Text)
                    End If
                Next
            End If
            If MySourceData.GridColumns(i).DisplayName = ddlSeries.SelectedValue Then
                If myChartConfig.Rows.Count = 0 Then
                    myChartConfig.rows.Add(New DataGrid.GridColumn)
                End If
                myChartConfig.Rows(0).Index = i
                myChartConfig.Rows(0).SourceName = ddlSeries.SelectedValue
                myChartConfig.Rows(0).ColumnValues.Clear()
                For ic = 0 To cbSeries.Items.Count - 1
                    If cbSeries.Items(ic).Selected = True Then
                        myChartConfig.Rows(0).ColumnValues.Add(cbSeries.Items(ic).Text)
                    End If
                Next
            End If
            If MySourceData.GridColumns(i).DisplayName = ddlFilter.SelectedValue Then
                myChartConfig.FilterColumn.Index = i
                myChartConfig.FilterColumn.SourceName = ddlFilter.SelectedValue
                myChartConfig.FilterColumn.ColumnValues.Clear()
                For ic = 0 To cbFilter.Items.Count - 1
                    If cbFilter.Items(ic).Selected = True Then
                        myChartConfig.FilterColumn.ColumnValues.Add(cbFilter.Items(ic).Text)
                    End If
                Next
            End If
        Next

        myChartConfig.ChartTitles.Clear()
        myChartConfig.ChartTitles.Add(tbTitle.Text)
        myChartConfig.ChartTitles.Add(tbSubTitle.Text)
        MySourceData.Title = ActiveCSV
        myChartConfig.CSVFile = ActiveCSV
        myChartConfig.ChartWidth = 800
        myChartConfig.ChartHeight = 400

        If ddlChartStyle.SelectedIndex > 0 Then
            myChartConfig.ChartStyle = ddlChartStyle.SelectedValue.ToUpper()
        Else
            myChartConfig.ChartStyle = "2D"
        End If
        myChartConfig.ChartType = CType(ddlChartType.SelectedIndex, SeriesChartType)
        myChartConfig.ChartPalette = CType(ddlChartPalette.SelectedIndex, ChartColorPalette)
        myChartConfig.AggregatorName = ddlCalc.SelectedValue

        myChartConfig.ReturnChartObject(myChart, mySourceData)

        If myChartConfig.ChartDataGrid.GridRows.Count > 0 Then
            cmd_SetChartConfig.CssClass = "btn btn-primary"
        Else
            cmd_SetChartConfig.CssClass = "btn btn-warning"
        End If
    End Sub
    Protected Sub ddlDataSet_SelectedIndexChanged(sender As Object, e As EventArgs)
        ddlAxis.Items.Clear()
        ddlFilter.Items.Clear()
        ddlSeries.Items.Clear()
        cbAxis.Items.Clear()
        cbFilter.Items.Clear()
        cbSeries.Items.Clear()
        SetData()
    End Sub
    Protected Sub lbFilterData_Click(sender As Object, e As EventArgs)
        If (From i As ListItem In cbFilter.Items Where i.Selected = True).Count = 0 Then
            For i = 0 To cbFilter.Items.Count - 1
                cbFilter.Items(i).Selected = True
            Next
        Else
            For i = 0 To cbFilter.Items.Count - 1
                cbFilter.Items(i).Selected = False
            Next
        End If
    End Sub
    Protected Sub lbAxisData_Click(sender As Object, e As EventArgs)
        If (From i As ListItem In cbAxis.Items Where i.Selected = True).Count = 0 Then
            For i = 0 To cbAxis.Items.Count - 1
                cbAxis.Items(i).Selected = True
            Next
        Else
            For i = 0 To cbAxis.Items.Count - 1
                cbAxis.Items(i).Selected = False
            Next
        End If
    End Sub
    Protected Sub lbSeriesData_Click(sender As Object, e As EventArgs)
        If (From i As ListItem In cbSeries.Items Where i.Selected = True).Count = 0 Then
            For i = 0 To cbSeries.Items.Count - 1
                cbSeries.Items(i).Selected = True
            Next
        Else
            For i = 0 To cbSeries.Items.Count - 1
                cbSeries.Items(i).Selected = False
            Next
        End If
    End Sub
    Protected Sub cmd_SetChartConfig_Click(sender As Object, e As EventArgs)
        SetData()
        DrawChart()
        '        PivotParmList.AddToList(myChartConfig)
        '       PivotParmList.SaveXML(PivotTableConfigurationFile)
    End Sub
    Protected Sub ddlSeries_SelectedIndexChanged(sender As Object, e As EventArgs)
        If hfSeries.Value <> ddlSeries.SelectedValue Then
            dim myChartConfig = PivotParmList.GetConfiguration(ActiveCSV, ddlConfig.SelectedValue)
            hfSeries.Value = ddlSeries.SelectedValue
            SetupColumnSelection(MySourceData.GridColumns, MySourceData, ddlSeries, cbSeries, ddlSeries.SelectedValue)
        End If
    End Sub
    Protected Sub ddlAxis_SelectedIndexChanged(sender As Object, e As EventArgs)
        If hfAxis.Value <> ddlAxis.SelectedValue Then
            dim myChartConfig = PivotParmList.GetConfiguration(ActiveCSV, ddlConfig.SelectedValue)
            hfAxis.Value = ddlAxis.SelectedValue
            SetupColumnSelection(MySourceData.GridColumns, MySourceData, ddlAxis, cbAxis, ddlAxis.SelectedValue)
        End If
    End Sub
    Protected Sub ddlFilter_SelectedIndexChanged(sender As Object, e As EventArgs)
        If hfFilter.Value <> ddlFilter.SelectedValue Then
            dim myChartConfig = PivotParmList.GetConfiguration(ActiveCSV, ddlConfig.SelectedValue)
            hfFilter.Value = ddlFilter.SelectedValue
            SetupColumnSelection(MySourceData.GridColumns, MySourceData, ddlFilter, cbFilter, ddlFilter.SelectedValue)
        End If
    End Sub
    Protected Sub cmd_GetCSV_Click(sender As Object, e As EventArgs)
        Response.Redirect(ActiveCsvPath)
    End Sub

    Protected Sub ddlConfig_SelectedIndexChanged(sender As Object, e As EventArgs)
        ResetForm(ddlConfig.SelectedValue)
    End Sub
    Protected Sub cmd_Delete_Click(sender As Object, e As EventArgs)
        If ddlConfig.SelectedValue <> ActiveCSV Then
            PivotParmList.Remove(PivotParmList.GetConfiguration(ActiveCSV, ddlConfig.SelectedValue))
        End If
        ResetForm(ActiveCSV)
    End Sub


    Public Sub ResetForm(ByVal Name As String)
        tbName.Text = String.Empty
        ddlConfig.Items.Clear()
        ddlConfig.Items.AddRange((From i In PivotParmList.GetConfigLookup(ActiveCSV) Select New ListItem With {.Text = i, .Value = i}).ToArray())
        If ddlConfig.Items.Count < 1 Then
            ddlConfig.Items.Add(New ListItem With {.Text = ActiveCSV, .Value = ActiveCSV})
            tbName.Enabled = False
        Else
            tbName.Enabled = True
        End If
        ddlConfig.SelectedValue = Name
        If Name = ActiveCSV Then
            cmd_Delete.Enabled = False
            cmd_Delete.Visible = False
        Else
            cmd_Delete.Enabled = True
            cmd_Delete.Visible = True
        End If

        SetData()
        DrawChart()
    End Sub


    Public Sub SetupColumnSelection(ByVal ColumnList As List(Of DataGrid.GridColumn), ByRef MyGrid As DataGrid, ByRef ddl As DropDownList, ByRef cbl As CheckBoxList, ByVal ColumnName As String)
        For index = 0 To MyGrid.GridColumns.Count - 1
            If MyGrid.GridColumns(index).SourceName = ColumnName Then
                ColumnList(0).Index = index
                ColumnList(0).SourceName = MyGrid.GridColumns(index).SourceName
                ddl.SelectedValue = ColumnList(0).SourceName
                Exit For
            End If
        Next
        cbl.Items.Clear()
        For Each c In (From i In MyGrid.GridRows Where i.Value(ColumnList(0).Index) <> String.Empty Select i.Value(ColumnList(0).Index) Distinct).Take(30)
            cbl.Items.Add(c)
        Next
        For i = 0 To cbl.Items.Count - 1
            cbl.Items(i).Selected = True
        Next
    End Sub


End Class
