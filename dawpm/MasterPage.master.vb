Imports System.Net
Imports System.Web.Script.Serialization

Public Class DefaultMasterPage
    Inherits System.Web.UI.MasterPage

    Public Property Themes As Dictionary(Of String, String)
        Get
            If Application("siteThemes") Is Nothing Then
                Application("siteThemes") = GetSiteThemes()
            End If
            Return TryCast(Application("siteThemes"), Dictionary(Of String, String))
        End Get
        Set(value As Dictionary(Of String, String))
            Application("siteThemes") = value
        End Set
    End Property

    Public ReadOnly Property UserName As String
        Get
            Return HttpContext.Current.User.Identity.Name
        End Get
    End Property

    Public ReadOnly Property ThemeCSS As String
        Get
            Return String.Format("<link rel='stylesheet' href='{0}' >", Themes(ThemeName))
        End Get
    End Property
    Public ReadOnly Property RootURL As String
        Get
            Dim request = HttpContext.Current.Request
            Dim appUrl = HttpRuntime.AppDomainAppVirtualPath
            If appUrl <> "/" Then
                appUrl = appUrl & "/"
            End If
            Dim baseUrl = String.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl)
            Return baseUrl
            '            If HttpRuntime.AppDomainAppVirtualPath <> "/" Then
            '                Return String.Format("{0}://{1}{2}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority, HttpRuntime.AppDomainAppVirtualPath)
            '            Else
            '                Return String.Format("{0}://{1}{2}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority, HttpRuntime.AppDomainAppVirtualPath & "/")
            '            End If
        End Get
    End Property
    Public Property ThemeName As String
        Get
            Return Application("ThemeName")
        End Get
        Set(value As String)
            Application("ThemeName") = value
        End Set
    End Property

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If String.IsNullOrEmpty(ThemeName) Then
            ThemeName = "Yeti"
        End If
    End Sub

    Private Function GetSiteThemes() As Dictionary(Of String, String)
        Dim SiteThemes As New Dictionary(Of String, String)
        Try

            Dim myClient As New WebClient()
            Dim myJSONstring = myClient.DownloadString("http://bootswatch.com/api/3.json")

            Dim jss As New JavaScriptSerializer()
            For Each myTheme As Theme In jss.Deserialize(Of Bootswatch)(myJSONstring).themes
                SiteThemes.Add(myTheme.name, myTheme.cssCdn)
            Next
        Catch ex As Exception

        End Try
        Return SiteThemes
    End Function


End Class

