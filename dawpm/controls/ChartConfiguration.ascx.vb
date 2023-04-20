Imports System.Web.UI.DataVisualization.Charting

Partial Class controls_ChartConfiguration
    Inherits System.Web.UI.UserControl
    Implements Icontrols_ChartConfiguration


    Private myConfiguration As New DataGridVisualization
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        If Not IsPostBack Then
            ddlChartType.DataSource = CType([Enum].GetNames(GetType(SeriesChartType)), String())
            ddlChartType.DataBind()
            ddlChartType.SelectedValue = "StackedColumn"

            ddlChartPalette.DataSource = CType([Enum].GetNames(GetType(ChartColorPalette)), String())
            ddlChartPalette.DataBind()
            ddlChartPalette.SelectedValue = "BrightPastel"

        End If
    End Sub
    Public Function GetChartConfiguration() As DataGridVisualization Implements Icontrols_ChartConfiguration.GetChartConfiguration
        myConfiguration.ChartTitles.Clear()
        myConfiguration.ChartTitles.Add(tbTitle.Text)
        myConfiguration.ChartTitles.Add(tbSubTitle.Text)
        myConfiguration.ChartPalette = ddlChartPalette.SelectedIndex
        myConfiguration.ChartType = CType(ddlChartType.SelectedIndex, SeriesChartType)
        myConfiguration.ChartStyle = ddlChartStyle.SelectedValue
        myConfiguration.BackgroundImage = ddlBackgroundImage.SelectedValue

        Return myConfiguration
    End Function

    Public Sub PutChartConfiguration(Configuration As DataGridVisualization) Implements Icontrols_ChartConfiguration.PutChartConfiguration
        myConfiguration = Configuration
        If myConfiguration.ChartTitles.Count = 1 Then
            tbTitle.Text = myConfiguration.ChartTitles(0)
        ElseIf myConfiguration.ChartTitles.Count = 2 Then
            tbTitle.Text = myConfiguration.ChartTitles(0)
            tbSubTitle.Text = myConfiguration.ChartTitles(1)
        Else
            tbTitle.Text = String.Format("{0} ({1})", myConfiguration.Name, myConfiguration.CSVFile)
            tbSubTitle.Text = String.Empty
        End If
        ddlBackgroundImage.SelectedValue = myConfiguration.BackgroundImage
        ddlChartPalette.SelectedIndex = myConfiguration.ChartPalette
        ddlChartType.SelectedIndex = myConfiguration.ChartType
        ddlChartStyle.SelectedValue = myConfiguration.ChartStyle
    End Sub

    Public Sub SetSourceData(SourceData As DataGrid) Implements Icontrols_ChartConfiguration.SetSourceData
      '  myConfiguration.SourceData = SourceData
    End Sub

End Class
