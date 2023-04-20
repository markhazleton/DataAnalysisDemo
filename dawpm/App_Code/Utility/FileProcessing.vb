Imports System.IO

Public Module FileProcessing
    Public Function VerifyFolderExists(ByVal sPath As String) As Boolean
        Return Directory.Exists(sPath)
    End Function
    Public Function CreateFolder(ByVal sPath As String) As Boolean
        Dim myDirInfo As DirectoryInfo = Directory.CreateDirectory(sPath)
        Return myDirInfo.Exists
    End Function
    Public Function GetFolders(ByVal sPath As String) As String()
        Return Directory.GetDirectories(HttpContext.Current.Server.MapPath(sPath))
    End Function
    Public Function RemoveFolder(ByVal sPath As String) As Boolean
        Dim bReturn As Boolean = True
        Try
            Directory.Delete(sPath, True)
        Catch ex As Exception
            bReturn = False
        End Try
        Return bReturn
    End Function
    Public Function IsValidFolder(ByVal sFolderPath As String) As Boolean
        Return My.Computer.FileSystem.DirectoryExists(sFolderPath)
    End Function
    Public Function FileExists(ByVal sPath As String) As Boolean
        Return My.Computer.FileSystem.FileExists(sPath)
    End Function
    Public Function IsValidPath(ByVal sPath As String) As Boolean
        Return My.Computer.FileSystem.FileExists(sPath)
    End Function
End Module
