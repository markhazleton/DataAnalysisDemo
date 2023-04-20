
Public Module Utility
    Public Function wpm_ApplyHTMLFormatting(ByVal strInput As String) As String
        strInput = "~" & strInput
        strInput = Replace(strInput, ",", "-")
        strInput = Replace(strInput, "'", "&quot;")
        strInput = Replace(strInput, """", "&quot;")
        strInput = Replace(strInput, "~", String.Empty)
        '  strInput = Replace(strInput, " " , "_")
        Return strInput
    End Function
    '    '********************************************************************************
    Public Function wpm_GetDBString(ByVal dbObject As Object) As String
        Dim strEntry As String = String.Empty
        If Not (IsDBNull(dbObject) Or dbObject Is Nothing) Then
            strEntry = CStr(dbObject)
            If strEntry = " " Then strEntry = String.Empty
        End If
        Return strEntry.Trim
    End Function
    Public Function wpm_GetStringValue(ByVal myString As String) As String
        If (IsDBNull(myString) Or myString Is Nothing) Then
            myString = String.Empty
        End If
        Return myString
    End Function
    Public Function wpm_GetDBString(ByVal dbObject As Object, Byval DefaultValue As String) As String
        Dim strEntry As String = String.Empty
        If Not (IsDBNull(dbObject) Or dbObject Is Nothing) Then
            strEntry = CStr(dbObject)
            If strEntry = " " Then strEntry = DefaultValue
        End If
        Return strEntry.Trim
    End Function
    '********************************************************************************
    Public Function wpm_GetDBDate(ByVal dbObject As Object) As DateTime
        If IsDBNull(dbObject) Then
            Return New DateTime
        ElseIf dbObject Is Nothing Then
            Return New DateTime
        ElseIf String.IsNullOrWhiteSpace(dbObject.ToString) Then
            Return New DateTime
        ElseIf wpm_IsDate(dbObject.ToString) Then
            Return CDate(dbObject)
        Else
            Return New DateTime
        End If

    End Function
    Public Function wpm_GetDBDouble(ByVal dbObject As Object) As Double
        If IsDBNull(dbObject) Then
            Return Nothing
        ElseIf dbObject Is Nothing Then
            Return Nothing
        Else
            If IsNumeric(dbObject) Then
                Return CDbl(dbObject)
            Else
                Return Nothing
            End If
        End If
    End Function
    Public Function wpm_GetDBInteger(ByVal dbObject As Object) As Integer
        If IsDBNull(dbObject) Then
            Return Nothing
        ElseIf dbObject Is Nothing Then
            Return Nothing
        Else
            If IsNumeric(dbObject) Then
                Return CInt(dbObject)
            Else
                Return Nothing
            End If
        End If
    End Function
    Public Function wpm_GetDBInteger(ByVal dbObject As Object, ByVal DefaultValue As Integer) As Integer
        If IsDBNull(dbObject) Then
            Return DefaultValue
        ElseIf dbObject Is Nothing Then
            Return DefaultValue
        Else
            If IsNumeric(dbObject) Then
                Return CInt(dbObject)
            Else
                Return DefaultValue
            End If
        End If
    End Function
    Public Function wpm_GetDBBoolean(ByVal dbObject As Object) As Boolean
        If IsDBNull(dbObject) Then
            Return False
        Else
            Return CBool(dbObject)
        End If
    End Function
    Public Function wpm_IsDate(ByVal strDate As String) As Boolean
        Dim dtDate As DateTime
        Dim bValid As Boolean = True
        Try
            dtDate = DateTime.Parse(strDate)
        Catch eFormatException As FormatException
            ' the Parse method failed => the string strDate cannot be converted to a date.
            bValid = False
        End Try
        Return bValid
    End Function
End Module

