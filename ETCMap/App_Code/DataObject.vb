Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

''' <summary>
''' 資料庫存取模組
''' </summary>
''' <remarks></remarks>
Public Module ClassDB

    ''' <summary>
    ''' 資料庫連線字串
    ''' </summary>
    ''' <remarks></remarks>
    Private CnnString As String = System.Configuration.ConfigurationManager.ConnectionStrings("AdminModuleConnectionString").ToString

    ''' <summary>
    ''' 取得並打開資料庫連結物件
    ''' </summary>
    ''' <returns>資料庫連結物件</returns>
    ''' <remarks></remarks>
    Private Function GetConnection() As SqlConnection
        Dim ret_conn As SqlConnection

        ret_conn = New SqlConnection(CnnString)
        ret_conn.Open()
        GetConnection = ret_conn
    End Function

    ''' <summary>
    ''' 關閉資料庫連結物件
    ''' </summary>
    ''' <param name="conn">資料庫連結物件</param>
    ''' <remarks></remarks>
    Private Sub CloseConnection(ByVal conn As SqlConnection)
        conn.Close()
        conn = Nothing
    End Sub


#Region " getReturnValue(RV_Para-String, RVAL_Para-ArrayList, DS_Para-DataSet, DR_Para-SqlDataReader) "


    ''' <summary>
    ''' 傳回SqlDataReader(使用預存程序)
    ''' </summary>
    ''' <param name="strSP">SQL預存程序名稱</param>
    ''' <param name="commandParameters">SQL預存程序參數</param>
    ''' <returns>SqlDataReader物件</returns>
    ''' <remarks></remarks>
    Public Function GetDataReaderSP(ByVal strSP As String, ByVal ParamArray commandParameters() As SqlParameter) As SqlDataReader

        Dim rdr As SqlDataReader

        Dim cn As SqlConnection = GetConnection()
        Using cmd As New SqlCommand(strSP, cn)
                'cmd.CommandTimeout = 9999
                cmd.CommandType = CommandType.StoredProcedure
                Try

                    For i As Integer = 0 To commandParameters.Length - 1
                        Dim p As SqlParameter = commandParameters(i)
                        p = cmd.Parameters.Add(p)
                        p.Direction = ParameterDirection.Input
                    Next
                    rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                Catch ex As Exception
                    rdr = Nothing
                Finally
                End Try
            End Using

        Return rdr

    End Function


    ''' <summary>
    ''' 傳回SqlDataReader
    ''' </summary>
    ''' <param name="strSQL">SQL查詢字串</param>
    ''' <returns>SqlDataReader物件</returns>
    ''' <remarks></remarks>
    Public Function GetDataReader(ByVal strSQL As String) As SqlDataReader

        Dim rdr As SqlDataReader
        Dim cn As SqlConnection = GetConnection()
        Using cmd As New SqlCommand(strSQL, cn)
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
        End Using
        Return rdr
    End Function

    ''' <summary>
    ''' 傳回SqlDataReader
    ''' </summary>
    ''' <param name="strSQL">SQL查詢字串</param>
    ''' <returns>SqlDataReader物件</returns>
    ''' <remarks></remarks>
    Public Function GetDataReaderParam(ByVal strSQL As String, ByVal ParamArray commandParameters() As SqlParameter) As SqlDataReader
        Dim rdr As SqlDataReader
        Dim cn As SqlConnection = GetConnection()
        Using cmd As New SqlCommand(strSQL, cn)
            cmd.CommandType = CommandType.Text
            Try
                For i As Integer = 0 To commandParameters.Length - 1
                    Dim p As SqlParameter = commandParameters(i)
                    p = cmd.Parameters.Add(p)
                    p.Direction = ParameterDirection.Input
                Next
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            Catch ex As Exception
            Finally
            End Try
        End Using

        Return rdr
    End Function


    ''' <summary>
    ''' 執行SQL(預存程序)查詢並回傳參數
    ''' </summary>
    ''' <param name="strSP">SQL預存程序名稱</param>
    ''' <param name="commandParameters">預存程序參數</param>
    ''' <returns>數值</returns>
    ''' <remarks></remarks>
    Public Function RunSPReturnInteger(ByVal strSP As String, ByVal ParamArray commandParameters() As SqlParameter) As Integer
        Dim retVal As Integer

        Using cn As SqlConnection = GetConnection()
            Using cmd As New SqlCommand(strSP, cn)
                Try

                    cmd.CommandType = CommandType.StoredProcedure
                    For i As Integer = 0 To commandParameters.Count - 1
                        Dim p As SqlParameter = commandParameters(i)
                        p = commandParameters(i)
                        p = cmd.Parameters.Add(p)
                        p.Direction = ParameterDirection.Input
                    Next
                    Dim pr As SqlParameter = cmd.Parameters.Add(New SqlParameter("@RetVal", SqlDbType.Int))
                    pr.Direction = ParameterDirection.Output
                    cmd.ExecuteNonQuery()
                    retVal = cmd.Parameters("@RetVal").Value()
                Catch ex As Exception
                    'Call ModuleWriteLog.writeLog("ClassDB.RunSPReturnInteger", "預存失敗--Err.GetException-->" & Err.GetException.ToString & "@retVal-->" & retVal)
                    ModuleWriteLog.WriteLog("ClassDB.RunSPReturnInteger預存失敗->" & strSP, ex.Message & ex.Source & ex.StackTrace & "@retVal-->" & retVal)
                Finally
                End Try
            End Using
        End Using
        Return retVal
    End Function



    ''' <summary>
    ''' 執行SQL(預存程序)查詢並回傳DataTable物件
    ''' </summary>
    ''' <param name="strSP">SQL預存程序名稱</param>
    ''' <param name="commandParameters">預存程序參數</param>
    ''' <returns>DataTable物件</returns>
    ''' <remarks></remarks>
    Public Function RunSPReturnDataTable(ByVal strSP As String, ByVal ParamArray commandParameters() As SqlParameter) As DataTable
        Dim dt As New DataTable
        Using cn As SqlConnection = GetConnection()
            Using da As New SqlDataAdapter(strSP, cn)
                da.SelectCommand.CommandType() = CommandType.StoredProcedure
                For i As Integer = 0 To commandParameters.Length - 1
                    Dim p As SqlParameter = commandParameters(i)
                    da.SelectCommand.Parameters.Add(p)
                    p.Direction = ParameterDirection.Input
                Next
                da.Fill(dt)
            End Using
        End Using
        Return dt
    End Function

    ''' <summary>
    ''' 執行SQL查詢並回傳DataTable物件
    ''' </summary>
    ''' <param name="strSP">查詢字串</param>
    ''' <returns>DataTable物件</returns>
    ''' <remarks></remarks>
    Public Function RunReturnDataTable(ByVal strSP As String, ByVal ParamArray commandParameters() As SqlParameter) As DataTable
        Dim dt As New DataTable
        Using cn As SqlConnection = GetConnection()
            Using da As New SqlDataAdapter(strSP, cn)
                da.SelectCommand.CommandType() = CommandType.Text
                For i As Integer = 0 To commandParameters.Length - 1
                    Dim p As SqlParameter = commandParameters(i)
                    da.SelectCommand.Parameters.Add(p)
                    p.Direction = ParameterDirection.Input
                Next
                da.Fill(dt)
            End Using
        End Using
        Return dt
    End Function



    ''' <summary>
    ''' 執行SQL(預存程序)查詢並回傳DataSet物件
    ''' </summary>
    ''' <param name="strSP">SQL預存程序名稱</param>
    ''' <param name="DataTableName">DataTable名稱</param>
    ''' <param name="commandParameters">預存程序參數</param>
    ''' <returns>DataSet物件</returns>
    ''' <remarks></remarks>
    Public Function RunSPReturnDataSet(ByVal strSP As String, ByVal DataTableName As String, ByVal ParamArray commandParameters() As SqlParameter) As DataSet
        Dim TableName As String = RemoveSqlInjection(DataTableName)
        Dim ds As New DataSet
        Using cn As SqlConnection = GetConnection()
            Using da As New SqlDataAdapter(strSP, cn)
                da.SelectCommand.CommandType() = CommandType.StoredProcedure
                For i As Integer = 0 To commandParameters.Length - 1
                    Dim p As SqlParameter = commandParameters(i)

                    da.SelectCommand.Parameters.Add(p)
                    p.Direction = ParameterDirection.Input
                Next
                da.Fill(ds, TableName)
            End Using
        End Using
        Return ds
    End Function

    ''' <summary>
    ''' 執行SQL查詢並回傳DataAdapter物件
    ''' </summary>
    ''' <param name="strSP">查詢字串</param>
    ''' <param name="DataTableName">DataTable名稱</param>
    ''' <returns>DataAdapter物件</returns>
    ''' <remarks></remarks>
    Public Function RunReturnDataAdapter(ByVal strSP As String, ByVal DataTableName As String) As SqlDataAdapter
        Dim da As New SqlDataAdapter
        Using cn As SqlConnection = GetConnection()
            da = New SqlDataAdapter(strSP, cn)
        End Using
        Return da
    End Function
    ''' <summary>
    ''' 執行SQL查詢並回傳DataSet物件
    ''' </summary>
    ''' <param name="strSP">查詢字串</param>
    ''' <param name="DataTableName">DataTable名稱</param>
    ''' <returns>DataSet物件</returns>
    ''' <remarks></remarks>
    Public Function RunReturnDataSet(ByVal strSP As String, ByVal DataTableName As String, ByVal ParamArray commandParameters() As SqlParameter) As DataSet
        Dim TableName As String = RemoveSqlInjection(DataTableName)
        Dim ds As New DataSet
        Using cn As SqlConnection = GetConnection()
            Using da As New SqlDataAdapter(strSP, cn)
                da.SelectCommand.CommandType() = CommandType.Text
                For i As Integer = 0 To commandParameters.Length - 1
                    Dim p As SqlParameter = commandParameters(i)
                    da.SelectCommand.Parameters.Add(p)
                    p.Direction = ParameterDirection.Input
                Next
                da.Fill(ds, TableName)
            End Using
        End Using
        Return ds
    End Function

#End Region

#Region " SQLDataTransaction(Insert、Update、Delete) "

    ''' <summary>
    ''' 資料庫更新查詢(預存程序)
    ''' </summary>
    ''' <param name="strSP">預存程序名稱</param>
    ''' <param name="commandParameters">預存程序參數</param>
    ''' <remarks></remarks>
    Public Sub UpdateDB(ByVal strSP As String, ByVal ParamArray commandParameters() As SqlParameter)
        Using cn As SqlConnection = GetConnection()

            '資料交易開始
            'Dim myTrans As SqlTransaction = cn.BeginTransaction(IsolationLevel.ReadCommitted, "DataTransaction")
            Using cmd As New SqlCommand(strSP, cn)
                Try
                    cmd.CommandType = CommandType.StoredProcedure
                    For i As Integer = 0 To commandParameters.Length - 1
                        Dim p As SqlParameter = commandParameters(i)
                        p = cmd.Parameters.Add(p)
                        p.Direction = ParameterDirection.Input
                    Next
                    cmd.ExecuteNonQuery()
                    '交易完成
                    ' myTrans.Commit()
                Catch ex As Exception
                    ModuleWriteLog.WriteLog("ClassDB.UpdateDB->" & strSP, ex.Message & ex.Source & ex.StackTrace, , False)
                    'myTrans.Rollback("DataTransaction") '交易失敗回復
                Finally
                End Try
            End Using
        End Using
    End Sub
    ''' <summary>
    ''' 資料庫更新查詢(預存程序)
    ''' </summary>
    ''' <param name="strSP">預存程序名稱</param>
    ''' <param name="commandParameters">預存程序參數</param>
    ''' <remarks></remarks>
    Public Sub UpdateDBText(ByVal strSP As String, ByVal ParamArray commandParameters() As SqlParameter)
        Using cn As SqlConnection = GetConnection()

            '資料交易開始
            'Dim myTrans As SqlTransaction = cn.BeginTransaction(IsolationLevel.ReadCommitted, "DataTransaction")
            Using cmd As New SqlCommand(strSP, cn)
                Try
                    cmd.CommandType = CommandType.Text
                    For i As Integer = 0 To commandParameters.Length - 1
                        Dim p As SqlParameter = commandParameters(i)
                        p = cmd.Parameters.Add(p)
                        p.Direction = ParameterDirection.Input
                    Next
                    cmd.ExecuteNonQuery()
                    '交易完成
                    ' myTrans.Commit()
                Catch ex As Exception
                    ModuleWriteLog.WriteLog("ClassDB.UpdateDBText->" & strSP, ex.Message & ex.Source & ex.StackTrace, , False)
                    'myTrans.Rollback("DataTransaction") '交易失敗回復
                Finally
                End Try
            End Using
        End Using
    End Sub

    ''' <summary>
    ''' 資料庫更新查詢
    ''' </summary>
    ''' <param name="strSP">查詢字串</param>
    ''' <remarks></remarks>
    Public Sub UpdateDB(ByVal strSP As String)
        Using cn As SqlConnection = GetConnection()
            '資料交易開始
            'Dim myTrans As SqlTransaction = cn.BeginTransaction(IsolationLevel.ReadCommitted, "DataTransaction")
            Using cmd As New SqlCommand(strSP, cn)
                Try
                    cmd.CommandType = CommandType.Text
                    cmd.ExecuteNonQuery()
                    '交易完成
                    ' myTrans.Commit()
                Catch ex As Exception
                    ModuleWriteLog.WriteLog("ClassDB.UpdateDB->" & strSP, ex.Message & ex.Source & ex.StackTrace, , False)
                    'myTrans.Rollback("DataTransaction") '交易失敗回復
                Finally
                End Try
            End Using
        End Using
    End Sub
#End Region
End Module
