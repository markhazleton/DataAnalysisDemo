
Partial Class ViewChart
    Inherits dawpmPage
    Public Property mySourceData As New DataGrid
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ddlConfig.Items.AddRange((From i In PivotParmList.GetConfigLookup(ActiveCSV) Select New ListItem With {.Text = i, .Value = i}).ToArray())
            If ddlConfig.Items.Count < 1 Then
                Response.Redirect("~/PivotTable.aspx")
                ddlConfig.Items.Add(New ListItem With {.Text = ActiveCSV, .Value = ActiveCSV})
            End If
            ddlConfig.SelectedValue = ActiveCSV

            ChartConfiguration1.PutChartConfiguration(PivotParmList.GetConfiguration(ActiveCSV, ddlConfig.SelectedValue))
            mySourceData = GenericParserDataGrid.LoadFromCsvFile(HttpContext.Current.Server.MapPath(ActiveCsvPath))
            ChartConfiguration1.SetSourceData(mySourceData)
        End If
    End Sub

    Protected Sub ddlConfig_SelectedIndexChanged(sender As Object, e As EventArgs)
        ChartConfiguration1.PutChartConfiguration(PivotParmList.GetConfiguration(ActiveCSV, ddlConfig.SelectedValue))
        mySourceData = GenericParserDataGrid.LoadFromCsvFile(HttpContext.Current.Server.MapPath(ActiveCsvPath))
        ChartConfiguration1.SetSourceData(mySourceData)
    End Sub

    Protected Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Try
            mySourceData = GenericParserDataGrid.LoadFromCsvFile(HttpContext.Current.Server.MapPath(ActiveCsvPath))
            Chart1.BuildChart(PivotParmList.GetConfiguration(ActiveCSV, ddlConfig.SelectedValue), mySourceData)
        Catch ex As Exception
            ApplicationLogging.ErrorLog("ViewChart.Page_PreRender", ex.ToString())
        End Try
    End Sub

    Protected Sub cmd_refresh_Click(sender As Object, e As EventArgs)

        Dim myConfiguration As DataGridVisualization = PivotParmList.GetConfiguration(ActiveCSV, ddlConfig.SelectedValue)
        With ChartConfiguration1.GetChartConfiguration()
            myConfiguration.BackgroundImage = .BackgroundImage
            myConfiguration.ChartTitles.Clear()
            myConfiguration.ChartTitles.AddRange(.ChartTitles.ToArray)
            myConfiguration.ChartPalette = .ChartPalette
            myConfiguration.ChartType = .ChartType

            Select Case myConfiguration.ChartType
                Case DataVisualization.Charting.SeriesChartType.StackedBar
                    myConfiguration.ChartStyle = "2D"
                Case Else
                    myConfiguration.ChartStyle = .ChartStyle
            End Select

        End With
        PivotParmList.AddToList(myConfiguration)


    End Sub
End Class
