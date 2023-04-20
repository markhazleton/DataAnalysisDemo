
Imports System.Drawing
Imports System.IO
Imports System.Web.UI.DataVisualization.Charting

Partial Class controls_Chart
    Inherits System.Web.UI.UserControl
    Implements Icontrols_Chart
    Public Sub BuildChart(myChartConfig As DataGridVisualization, ByRef SourceData As DataGrid) Implements Icontrols_Chart.BuildChart
        Try
            Dim myChart As New Chart()

            With myChart
                .AlternateText = myChartConfig.Name
                .Titles.Add(myChartConfig.Name)
                .ClientIDMode = ClientIDMode.Predictable
                .EnableTheming = "false"
                .BackSecondaryColor = Color.White
                .Width = 1920
                .Height = 1080
                .BorderlineDashStyle = ChartDashStyle.Solid
                .BorderlineWidth = "5"
                .BorderlineColor = Color.Transparent
                .ImageStorageMode = ImageStorageMode.UseHttpHandler
                .ViewStateContent = SerializationContents.Appearance
                .ValidateRequestMode = ValidateRequestMode.Disabled
                .ViewStateMode = ViewStateMode.Disabled
                .BackImageWrapMode = ChartImageWrapMode.Scaled
                .BackImageTransparentColor = Color.White
                .CssClass = "img-responsive"
                .ImageType = ChartImageType.Png
            End With

            myChartConfig.ReturnChartObject(myChart, SourceData)

            Dim sb As New StringBuilder()
            Dim tw As New StringWriter(sb)
            Dim hw As New HtmlTextWriter(tw)
            myChart.RenderControl(hw)

            sb.Replace("style", "ignore")
            Dim html = sb.ToString()



            litChart.Text = html
            litTitle.Text = myChartConfig.Name

        Catch ex As Exception
            ApplicationLogging.ErrorLog("Chart.BuildChart", ex.ToString())
        End Try
    End Sub
End Class
