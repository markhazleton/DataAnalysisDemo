Public Class PivotTable_Page
    Inherits dawpmPage
    Public Property PivotParms As String = "derivedAttributes: {},"

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            PivotParms = PivotParmList.GetParameterString(ActiveCSV, ActiveCSV)
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
        End If
        cmd_GetCSV.Text = ActiveCSV
    End Sub

    Protected Sub cmd_ReadJSON_Click(sender As Object, e As EventArgs)
        Dim myConfigurationList As New DataGridVisualizationList
        Dim myJSON As New JSONObject(tbJSON.Text)

        myConfigurationList.AddRange(PivotParmList.ToArray)

        Dim myConfiguration = myConfigurationList.GetConfiguration(ActiveCSV, ddlConfig.SelectedValue)
        If myConfiguration Is Nothing Then
            myConfiguration = New DataGridVisualization
            myConfiguration.Name = ddlConfig.SelectedValue
            myConfiguration.CSVFile = ActiveCSV
        End If

        If Not String.IsNullOrEmpty(tbName.Text) Then
            myConfiguration.Name = tbName.Text.Trim()
            myConfiguration.CSVFile = ActiveCSV
        End If

        myConfiguration.Cols.Clear()
        For Each mycol As JSONValue In myJSON.GetProperty("cols").Value
            myConfiguration.Cols.Add(New DataGrid.GridColumn With {.SourceName = mycol.Value(), .DisplayName = mycol.Value()})
        Next

        myConfiguration.Rows.Clear()
        For Each mycol As JSONValue In myJSON.GetProperty("rows").Value
            myConfiguration.Rows.Add(New DataGrid.GridColumn With {.SourceName = mycol.Value(), .DisplayName = mycol.Value()})
        Next

        myConfiguration.Vals.Clear()
        For Each mycol As JSONValue In myJSON.GetProperty("vals").Value
            myConfiguration.Vals.Add(New DataGrid.GridColumn With {.SourceName = mycol.Value(), .DisplayName = mycol.Value()})
        Next

        Dim myEx = TryCast(myJSON.GetProperty("exclusions").Value, JSONObject)
        For Each myExclusion In myEx.GetProperties().ToList
            Dim myValSet As New DataGrid.GridColumn With {.SourceName = myExclusion.Key, .DisplayName = myExclusion.key}
            For Each myVal In myExclusion.Value.Replace("[", String.Empty).Replace("]", String.Empty).Split(",")
                myValSet.ColumnValues.Add(myVal)
            Next
            myConfiguration.ExcluedValues.Add(myValSet)
        Next

        Dim myIn = TryCast(myJSON.GetProperty("inclusions").Value, JSONObject)
        For Each myExclusion In myIn.GetProperties().ToList
            Dim myValSet As New DataGrid.GridColumn With {.SourceName = myExclusion.Key, .DisplayName = myExclusion.key}
            For Each myVal In myExclusion.Value.Replace("[", String.Empty).Replace("]", String.Empty).Split(",")
                myValSet.ColumnValues.Add(myVal)
            Next
            myConfiguration.IncludeValues.Add(myValSet)
        Next

        myConfiguration.AggregatorName = myJSON.GetProperty("aggregatorName").Value
        myConfiguration.rendererName = myJSON.GetProperty("rendererName").Value

        myConfigurationList.AddToList(myConfiguration)
        PivotParms = myConfiguration.GetPivotParm
        ResetForm(myConfiguration.Name)
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
        PivotParms = PivotParmList.GetParameterString(ActiveCSV, Name)
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
    End Sub
End Class
