Imports System.IO

Partial Class _Default
    Inherits dawpmPage

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            file_DatabindListbox(Server.MapPath("~/CSV/"), ".csv")
        End If
        myFileListBox.Visible = False
        For Each myRow As GridViewRow In gvFiles.Rows
            If myRow.Cells(1).Text = ActiveCSV Then
                gvFiles.SelectedIndex = myRow.RowIndex
                Exit For
            End If
        Next
    End Sub
    Private Sub file_DatabindListbox(ByVal sPath As String, ByVal sExt As String)
        For Each baseFile As String In My.Computer.FileSystem.GetFiles(sPath)
            If (Right(baseFile, 4).ToLower = sExt.ToLower) Then
                Dim li As New ListItem() With {.Text = IO.Path.GetFileName(baseFile), .Value = IO.Path.GetFileName(baseFile)}
                myFileListBox.Items.Add(li)
            End If
        Next
        Try
            myFileListBox.SelectedValue = ActiveCSV
            CsvDataGrid = GenericParserDataGrid.LoadFromCsvFile(HttpContext.Current.Server.MapPath(ActiveCsvPath))
            gvCsvColumns.DataSource = CsvDataGrid.GridColumns
            gvCsvColumns.DataBind()
            tbCsvFileName.Text = String.Format("The {0} data set has {1} columns and {2} rows", ActiveCSV, CsvDataGrid.GridColumns.Count, CsvDataGrid.GridRows.Count)
            lblChartHeading.Text = ActiveCSV
            rptCharts.Controls.Clear()
            For Each myConfig In PivotParmList.GetConfigurationList(ActiveCSV)
                Dim myControl = DirectCast(Page.LoadControl("~/controls/chart.ascx"), Icontrols_Chart)
                myControl.BuildChart(myConfig, CsvDataGrid)
                rptCharts.Controls.Add(myControl)
            Next
        Catch ex As Exception
            myFileListBox.SelectedIndex = -1
        End Try
    End Sub

    Protected Sub btwDownload_Click(sender As Object, e As EventArgs)
        Response.Redirect(String.Format("{0}", String.Format("/CSV/{0}", myFileListBox.SelectedItem.Text)))
    End Sub

    Protected Sub myFileListBox_SelectedIndexChanged(sender As Object, e As EventArgs)
        ActiveCSV = myFileListBox.SelectedItem.Text
        CsvDataGrid = GenericParserDataGrid.LoadFromCsvFile(HttpContext.Current.Server.MapPath(ActiveCsvPath))
        gvCsvColumns.DataSource = CsvDataGrid.GridColumns
        gvCsvColumns.DataBind()
        tbCsvFileName.Text = String.Format("The file {0} has {1} columns and {2} rows", ActiveCSV, CsvDataGrid.GridColumns.Count, CsvDataGrid.GridRows.Count)
        lblChartHeading.Text = ActiveCSV

    End Sub
    Protected Sub gvFiles_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvFiles.SelectedIndexChanged
        ActiveCSV = gvFiles.SelectedRow.Cells(1).Text
        myFileListBox.SelectedValue = ActiveCSV

        CsvDataGrid = GenericParserDataGrid.LoadFromCsvFile(HttpContext.Current.Server.MapPath(ActiveCsvPath))
        gvCsvColumns.DataSource = CsvDataGrid.GridColumns
        gvCsvColumns.DataBind()
        tbCsvFileName.Text = String.Format("The file {0} has {1} columns and {2} rows", ActiveCSV, CsvDataGrid.GridColumns.Count, CsvDataGrid.GridRows.Count)
        lblChartHeading.Text = ActiveCSV

        rptCharts.Controls.Clear()
        For Each myConfig In PivotParmList.GetConfigurationList(ActiveCSV)
            Dim myControl = DirectCast(Page.LoadControl("~/controls/chart.ascx"), Icontrols_Chart)
            myControl.BuildChart(myConfig, CsvDataGrid)
            rptCharts.Controls.Add(myControl)
        Next
    End Sub
    Protected Sub cmd_PivotTable_Click(sender As Object, e As EventArgs)
        Response.Redirect("~/PivotTable.aspx")
    End Sub
End Class
