Imports System.IO
Imports System.Xml.Serialization

Public Class DataGridVisualizationList
    Inherits List(Of DataGridVisualization)

    Public ReadOnly Property PivotTableConfigurationFile As String
        Get
            Return HttpContext.Current.Server.MapPath("~/App_Data/PivotParameterList.xml")
        End Get
    End Property


    Public Function GetParameterString(ByVal CSVName As String, ByVal Name As String) As String
        Try
            Dim myTest = (From i In Me Where i.CSVFile = CSVName And i.Name = Name).SingleOrDefault
            If (myTest Is Nothing) Then
                Return ""
            Else
                Return myTest.GetPivotParm
            End If
        Catch ex As Exception
            Return "derivedAttributes: {},"
        End Try
    End Function

    Public Function GetConfigLookup(ByVal CSVName As String) As List(Of String)
        Try
            Return (From i In Me Where i.CSVFile = CSVName Select i.Name).ToList()
        Catch ex As Exception
            Return New List(Of String) From {CSVName}
        End Try
    End Function
    Public Function GetConfigurationList(ByVal CSVName As String) As List(Of DataGridVisualization)
        Try
            Return ((From i In Me Where i.CSVFile = CSVName).ToList)
        Catch ex As Exception
            Return New list(of DataGridVisualization)
        End Try
    End Function

    Public Function GetConfiguration(ByVal CSVName As String, ByVal Name As String) As DataGridVisualization
        Try
            Return ((From i In Me Where i.CSVFile = CSVName And i.Name = Name).SingleOrDefault)
        Catch ex As Exception
            Return New DataGridVisualization
        End Try
    End Function

    Public Function AddToList(ByVal myConfiguration As DataGridVisualization) As Boolean
        Dim myList = (New DataGridVisualizationList).GetXML()
        If String.IsNullOrEmpty(myConfiguration.Name) Or String.IsNullOrEmpty(myConfiguration.CSVFile) Then
            Return False
        Else
            Dim myIndex As Integer = -1
            For i = 0 To myList.Count - 1
                If myList.Item(i).CSVFile = myConfiguration.CSVFile Then
                    If myList.Item(i).Name = myConfiguration.Name Then
                        myIndex = i
                        Exit For
                    End If
                End If
            Next
            If myIndex > -1 Then
                myList.RemoveAt(myIndex)
            End If
            CType(myList, List(Of DataGridVisualization)).Add(myConfiguration)
            myList.SaveXML()
            Return True
        End If
    End Function

    Public Overloads Function IndexOf(ByVal item As DataGridVisualization) As Integer
        Dim myParm = MyBase.Where(Function(c) c.CSVFile = item.CSVFile).FirstOrDefault
        Return MyBase.IndexOf(myParm)
    End Function

    Public Function SaveXML() As Boolean
        Dim bReturn As Boolean = True
        HttpContext.Current.Application.Lock()
        Try
            Using sw As New StreamWriter(PivotTableConfigurationFile, False)
                Try
                    Dim ViewWriter As New XmlSerializer(GetType(DataGridVisualizationList))
                    ViewWriter.Serialize(sw, Me)
                Catch ex As Exception
                    ApplicationLogging.ErrorLog(String.Format("Error Saving File - {0}", ex), "PivotParameterList.SaveXML")
                    bReturn = False
                End Try
            End Using
        Catch ex As Exception
            ApplicationLogging.ErrorLog(String.Format("Error Before Saving File  - {0}", ex), "PivotParameterList.SaveXML")
            bReturn = False
        End Try
        HttpContext.Current.Application.UnLock()
        Return bReturn
    End Function

    Public Function GetXML() As DataGridVisualizationList
        Dim myConfigList As New DataGridVisualizationList
        Dim x As New XmlSerializer(GetType(DataGridVisualizationList))
        Try
            Using objStreamReader As New StreamReader(PivotTableConfigurationFile)
                myConfigList = CType(x.Deserialize(objStreamReader), DataGridVisualizationList)
            End Using
        Catch ex As Exception
            ApplicationLogging.ErrorLog("PivotParameterList.GetXML", ex.ToString)
            myConfigList.SaveXML()
        End Try
        Return myConfigList
    End Function


    Protected Overridable Function IsInList(ByVal newParm As DataGridVisualization) As Boolean
        Return Me.Contains(newParm, New DataGridVisualizationEqualityComparer)
    End Function

End Class
