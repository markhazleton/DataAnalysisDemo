Imports System.IO

Public Class dawpmPage
    Inherits System.Web.UI.Page

    Public Property CsvDataGrid As New DataGrid
    Public ReadOnly Property CsvFileList As List(Of CsvFile)
        Get
            Dim mylist As New List(Of CsvFile)

            Dim filePaths() As String = Directory.GetFiles(Server.MapPath("~/CSV/"))
            For Each filePath As String In filePaths
                mylist.Add(New CsvFile With {.FileName = IO.Path.GetFileName(filePath), .FileDate = File.GetLastWriteTime(filePath), .FileSize = New FileInfo(filePath).Length})
            Next
            Return mylist
        End Get
    End Property

    Public Class CsvFile
        Property FileName As String
        Property FileDate As Date
        Property FileSize As String
    End Class

    Public Function GetCsvFileList() As List(Of CsvFile)
        Return CsvFileList
    End Function

    Public Property PivotParmList As DataGridVisualizationList
        Get
            Dim myParmList As New DataGridVisualizationList()
            Return myParmList.GetXML()
        End Get
        Set(value As DataGridVisualizationList)
            value.SaveXML()
        End Set
    End Property

    Public Property ActiveCSV As String
        Get
            Return wpm_GetDBString(Session("ActiveCSV"))
        End Get
        Set(value As String)
            Session("ActiveCSV") = value
        End Set
    End Property

    Public ReadOnly Property ActiveCsvPath As String
        Get
            Dim appUrl = HttpRuntime.AppDomainAppVirtualPath
            If appUrl <> "/" Then
                appUrl = appUrl & "/"
            End If
            Return String.Format("{0}CSV/{1}", appUrl, ActiveCSV)
        End Get
    End Property

    Public ReadOnly Property RootPath As String
        Get
            Dim appUrl = HttpRuntime.AppDomainAppVirtualPath
            If HttpRuntime.AppDomainAppVirtualPath <> "/" Then
                appUrl = appUrl & "/"
            End If
            Dim baseUrl = String.Format("{0}://{1}{2}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority, appUrl)
            Return baseUrl
        End Get
    End Property

    Public ReadOnly Property ApplicationUrl As String
        Get
            If "/" = HttpRuntime.AppDomainAppVirtualPath Then
                Return "/"
            Else
                Return HttpRuntime.AppDomainAppVirtualPath & "/"
            End If
        End Get
    End Property


    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        If GetProperty("csv", String.Empty) = String.Empty Then
            If String.IsNullOrEmpty(ActiveCSV) Then
                ActiveCSV = GetProperty("csv", "mps.csv")
            End If
        Else
            ActiveCSV = GetProperty("csv", "mps.csv")
        End If
    End Sub
    Public Function GetProperty(ByVal myProperty As String, ByVal curValue As String) As String
        Dim myValue As String = String.Empty
        If Len(HttpContext.Current.Request.QueryString(myProperty)) > 0 Then
            myValue = HttpContext.Current.Request.QueryString(myProperty).ToString
        ElseIf Len(HttpContext.Current.Request.Form.Item(myProperty)) > 0 Then
            myValue = HttpContext.Current.Request.Form.Item(myProperty).ToString
        Else
            If curValue Is Nothing Then
                myValue = String.Empty
            Else
                myValue = curValue
            End If
        End If
        Return myValue
    End Function

End Class
