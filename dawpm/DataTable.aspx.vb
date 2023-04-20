
Partial Public Class DataTable_Page
    Inherits dawpmPage
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        dtList.BuildDataTable(Server.MapPath(ActiveCsvPath))
        cmd_GetCSV.Text = string.Format("Download {0}",ActiveCSV)
    End Sub

    Protected Sub cmd_GetCSV_Click(sender As Object, e As EventArgs)
        Response.Redirect(ActiveCsvPath)
    End Sub
End Class
