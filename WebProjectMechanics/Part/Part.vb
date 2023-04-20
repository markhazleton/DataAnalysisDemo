Imports System.Data.OleDb

Public Class Part
    Public Property Title As String
    Public Property URL As String
    Public Property Description As String
    Public Property PartCategoryID As String
    Public Property PartTypeCD As String
    Public Property PartCategoryTitle As String
    Public Property PartID As String
    Public Property SiteCategoryTypeID As String
    Public Property SiteCategorytypeNM As String
    Public Property SiteCategoryGroupID As String
    Public Property SiteCategoryGroupNM As String
    Public Property View As Boolean
    Public Property ModifiedDT As Date
    Public Property PartSortOrder As Integer
    Public Property UserID As String
    Public Property AmazonIndex As String
    Public Property PartSource As String
    Public Property CompanyID As String
    Public Property CompanyNM As String
    Public Property LocationNM As String
    Private _LocationID As String
    Public Property LocationID() As String
        Get
            Return _LocationID
        End Get
        Set(ByVal value As String)
            If value = "CAT-" Then
                value = String.Empty
            End If
            _LocationID = value
        End Set
    End Property

    Sub UpdatePart()
        If CInt(PartID) > 0 Then
            UpdatePartDB(New Integer)
        Else
            InsertPartDB(New Integer)
        End If
    End Sub

    Sub DeletePart()
        If PartSource = "Link" Then
            wpm_RunDeleteSQL("DELETE FROM [Link] WHERE [ID] =" & PartID, "Link")
        ElseIf PartSource = "SiteLink" Then
            wpm_RunDeleteSQL("DELETE FROM [SiteLink] WHERE [ID] =" & PartID, "SiteLink")
        Else
            ApplicationLogging.ErrorLog("Unknown Part Source:" & PartSource, "Part.DeletePartDB")
        End If
    End Sub

    Private Sub UpdatePartDB(ByRef iRowsAffected As Integer)
        Dim sSQL As String = String.Empty
        If PartSource = "Link" Then
            sSQL = "UPDATE [Link] SET [CompanyID] =@CompanyID, [LinkTypeCD] =@LinkTypeCD, [CategoryID] =@CategoryID, [PageID] =@PageID, [Title] =@Title, [Description] =@Description, [URL] = @URL, [DateAdd] =@DateAdd, [Ranks] = @Ranks, [Views] = @Views, [UserName] = @UserName, [UserID] = @UserID, [ASIN] = @ASIN, [SiteCategoryGroupID] = @SiteCategoryGroupID WHERE [ID] = @ID"
            Using conn As New OleDbConnection(wpm_SQLDBConnString)
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = sSQL}
                    Try
                        wpm_AddParameterValue("@CompanyID", CompanyID, SqlDbType.Int, cmd)
                        wpm_AddParameterStringValue("@LinkTypeCD", PartTypeCD, cmd)
                        wpm_AddParameterValue("@CategoryID", PartCategoryID, SqlDbType.Int, cmd)
                        wpm_AddParameterValue("@PageID", LocationID, SqlDbType.Int, cmd)
                        wpm_AddParameterStringValue("@Title", Title, cmd)
                        wpm_AddParameterStringValue("@Description", Description, cmd)
                        wpm_AddParameterStringValue("@URL", URL, cmd)
                        wpm_AddParameterValue("@DateAdd", ModifiedDT, SqlDbType.DateTime, cmd)
                        wpm_AddParameterValue("@Ranks", PartSortOrder, SqlDbType.Int, cmd)
                        wpm_AddParameterValue("@Views", View, SqlDbType.Bit, cmd)
                        wpm_AddParameterStringValue("@UserName", String.Empty, cmd)
                        wpm_AddParameterValue("@UserID", UserID, SqlDbType.Int, cmd)
                        wpm_AddParameterStringValue("@ASIN", AmazonIndex, cmd)
                        wpm_AddParameterValue("@SiteCategoryGroupID ", SiteCategoryGroupID, SqlDbType.Int, cmd)
                        wpm_AddParameterValue("@ID", PartID, SqlDbType.Int, cmd)
                        iRowsAffected = cmd.ExecuteNonQuery()
                    Catch ex As Exception
                        ApplicationLogging.SQLUpdateError(cmd.CommandText, "Part.UpdatePartDB")
                    End Try
                End Using
            End Using
        ElseIf PartSource = "SiteLink" Then
            sSQL = "UPDATE SiteLink SET [URL] = @URL, [LinkTypeCD]=@LinkTypeCD, [CategoryID]=@CategoryID, [Title]= @Title, [Description]=@Description, [DateAdd]=now(), [Ranks]= @Ranks, [UserName]= @UserName, [UserID]=@UserID, [ASIN]=@ASIN, [Views]=@Views, [SiteCategoryID]=@SiteCategoryID, [CompanyID]=@CompanyID WHERE [ID]=@PartID"
            Using conn As New OleDbConnection(wpm_SQLDBConnString)
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = sSQL}
                    Try
                        wpm_AddParameterStringValue("@URL", URL, cmd)
                        wpm_AddParameterStringValue("@LinkTypeCD", PartTypeCD, cmd)
                        wpm_AddParameterStringValue("@CategoryID", PartCategoryID, cmd)
                        wpm_AddParameterStringValue("@Title", Title, cmd)
                        wpm_AddParameterStringValue("@Description", Description, cmd)
                        wpm_AddParameterValue("@Ranks", PartSortOrder, SqlDbType.Int, cmd)
                        wpm_AddParameterStringValue("@UserName", UserID, cmd)
                        wpm_AddParameterStringValue("@UserID", UserID, cmd)
                        wpm_AddParameterStringValue("@ASIN", AmazonIndex, cmd)
                        wpm_AddParameterValue("@Views", True, SqlDbType.Binary, cmd)
                        wpm_AddParameterStringValue("@SiteCategoryID", LocationID.Replace("CAT-", String.Empty), cmd)
                        wpm_AddParameterStringValue("@CompanyID", CompanyID, cmd)
                        wpm_AddParameterStringValue("@LinkID", PartID, cmd)
                        iRowsAffected = cmd.ExecuteNonQuery()
                    Catch ex As Exception
                        ApplicationLogging.SQLSelectError(cmd.CommandText, String.Format("Error on UtilityDB.RunUpdateSQL - {0} ({1})", "PartBuisnessLogic.UpdatePart", ex.Message))
                    End Try
                End Using
            End Using
        Else
            ApplicationLogging.ErrorLog("Unknown Part Source:" & PartSource, "Part.UpdatePartDB")
        End If


    End Sub

    Private Sub InsertPartDB(ByRef iRowsAffected As Integer)

        Dim sSQL As String = String.Empty
        If PartSource = "Link" Then
            sSQL = "INSERT INTO [Link] ( [CompanyID], [LinkTypeCD], [CategoryID], [PageID], [Title], [Description], [URL], [DateAdd], [Ranks], [Views], [UserName], [UserID], [ASIN], [SiteCategoryGroupID]) VALUES (@CompanyID,@LinkTypeCD,@CategoryID, @PageID, @Title, @Description, @URL, @DateAdd, @Ranks, @Views, @UserName, @UserID,  @ASIN, @SiteCategoryGroupID )"
            Using conn As New OleDbConnection(wpm_SQLDBConnString)
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = sSQL}
                    Try
                        wpm_AddParameterValue("@CompanyID", CompanyID, SqlDbType.Int, cmd)
                        wpm_AddParameterStringValue("@LinkTypeCD", PartTypeCD, cmd)
                        wpm_AddParameterValue("@CategoryID", PartCategoryID, SqlDbType.Int, cmd)
                        wpm_AddParameterValue("@PageID", LocationID, SqlDbType.Int, cmd)
                        wpm_AddParameterStringValue("@Title", Title, cmd)
                        wpm_AddParameterStringValue("@Description", Description, cmd)
                        wpm_AddParameterStringValue("@URL", URL, cmd)
                        wpm_AddParameterValue("@DateAdd", Now(), SqlDbType.DateTime, cmd)
                        wpm_AddParameterValue("@Ranks", PartSortOrder, SqlDbType.Int, cmd)
                        wpm_AddParameterValue("@Views", View, SqlDbType.Bit, cmd)
                        wpm_AddParameterStringValue("@UserName", String.Empty, cmd)
                        wpm_AddParameterValue("@UserID", UserID, SqlDbType.Int, cmd)
                        wpm_AddParameterStringValue("@ASIN", AmazonIndex, cmd)
                        wpm_AddParameterValue("@SiteCategoryGroupID ", SiteCategoryGroupID, SqlDbType.Int, cmd)
                        iRowsAffected = cmd.ExecuteNonQuery()
                    Catch ex As Exception
                        ApplicationLogging.SQLInsertError(ex.Message, "Part.InsertPartDB-{" & cmd.CommandText & "}")
                    End Try
                End Using
            End Using
        ElseIf PartSource = "SiteLink" Then
            sSQL = "INSERT INTO SiteLink ([URL],[LinkTypeCD],[CategoryID],[SiteCategoryTypeID],[Title],[Description],[DateAdd],[Ranks],[UserName],[UserID],[ASIN],[Views],[SiteCategoryID],[CompanyID]  ) VALUES (@URL,@LinkTypeCD,@CategoryID,@SiteCategoryTypeID, @Title,@Description,now(),@Ranks,@UserName,@UserID,@ASIN,@Views,@SiteCategoryID,@CompanyID )"
            Using conn As New OleDbConnection(wpm_SQLDBConnString)
                conn.Open()
                Using cmd As New OleDbCommand() With {.Connection = conn, .CommandType = CommandType.Text, .CommandText = sSQL}
                    Try
                        wpm_AddParameterStringValue("@URL", URL, cmd)
                        wpm_AddParameterStringValue("@LinkTypeCD", PartTypeCD, cmd)
                        wpm_AddParameterStringValue("@CategoryID", PartCategoryID, cmd)
                        wpm_AddParameterStringValue("@SiteCategoryTypeID", SiteCategoryTypeID, cmd)
                        wpm_AddParameterStringValue("@Title", Title, cmd)
                        wpm_AddParameterStringValue("@Description", Description, cmd)
                        wpm_AddParameterValue("@Ranks", PartSortOrder, SqlDbType.Int, cmd)
                        wpm_AddParameterStringValue("@UserName", UserID, cmd)
                        wpm_AddParameterStringValue("@UserID", UserID, cmd)
                        wpm_AddParameterStringValue("@ASIN", AmazonIndex, cmd)
                        wpm_AddParameterValue("@Views", True, SqlDbType.Binary, cmd)
                        wpm_AddParameterValue("@SiteCategoryID", LocationID.Replace("CAT-", String.Empty), SqlDbType.Int, cmd)
                        wpm_AddParameterValue("@CompanyID", CompanyID, SqlDbType.Int, cmd)

                        iRowsAffected = cmd.ExecuteNonQuery()
                    Catch ex As Exception
                        ApplicationLogging.SQLSelectError(cmd.CommandText, String.Format("Error on UtilityDB.RunUpdateSQL - {0} ({1})", "PartBuisnessLogic.UpdatePart", ex.Message))
                    End Try
                End Using
            End Using
        Else
            ApplicationLogging.ErrorLog("Unknown Part Source:" & PartSource, "Part.InsertPartDB")

        End If


    End Sub




End Class