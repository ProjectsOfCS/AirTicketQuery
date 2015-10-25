namespace AirTicketQuery.Modules.Code
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// SQL VerifyType
    /// </summary>
    public enum VerifyTypeEnum
    {
        /// <summary>
        /// windows
        /// </summary>
        Windows = 0,

        /// <summary>
        /// Database
        /// </summary>
        Database = 1
    }

    /// <summary>
    /// The Database Class
    /// </summary>
    public class DB : IDisposable
    {
        #region Field

        /// <summary>
        /// Transaction
        /// </summary>
        private SqlTransaction sqltrans;

        /// <summary>
        /// Transaction IsolationLevel
        /// </summary>
        private IsolationLevel isolationLevel;

        /// <summary>
        /// DBConnection
        /// </summary>
        private SqlConnection sqlConn;

        /// <summary>
        /// database connect string
        /// </summary>
        private string dbConnStr = string.Empty;

        /// <summary>
        /// keepConnectionOpen
        /// </summary>
        private bool keepConnectionOpen = false;

        private bool needLog = true;

        #endregion

        #region Construct

        /// <summary>
        /// New
        /// </summary>
        public DB()
        {
            this.isolationLevel = IsolationLevel.ReadCommitted;
        }

        /// <summary>
        /// New
        /// </summary>
        /// <param name="dbConnParam">Database connent Class</param>
        public DB(DBConnectParams dbConnParam)
        {
            this.dbConnStr = dbConnParam.ToString();
            this.isolationLevel = IsolationLevel.ReadCommitted;
        }

        /// <summary>
        /// New
        /// </summary>
        /// <param name="dbConnstring">Database connent string</param>
        public DB(string dbConnstring)
        {
            this.dbConnStr = dbConnstring;
            this.isolationLevel = IsolationLevel.ReadCommitted;
        }

        #endregion

        #region Property

        /// <summary>
        /// IsInTransaction
        /// </summary>
        public bool IsInTransaction
        {
            get
            {
                return this.sqltrans == null ? false : true;
            }
        }

        /// <summary>
        /// Transaction IsolationLevel
        /// </summary>
        public IsolationLevel TransIsolationLevel
        {
            get
            {
                return this.isolationLevel;
            }

            set
            {
                this.isolationLevel = value;
            }
        }

        /// <summary>
        /// keepConnectionOpen
        /// </summary>
        public bool KeepConnectionOpen
        {
            get
            {
                return this.keepConnectionOpen;
            }
            set
            {
                this.keepConnectionOpen = value;
                if (this.keepConnectionOpen)
                {
                    this.sqlConn = new SqlConnection(this.DBConnString);
                }
            }
        }

        public bool NeedLog
        {
            get
            {
                return this.needLog;
            }
            set
            {
                this.needLog = value;
            }
        }

        /// <summary>
        ///     Property ConnectionString is used to get tmis's database connection string.
        ///     <remarks>The database connection string.</remarks>
        /// </summary>
        private string DBConnString
        {
            get
            {
                if (string.IsNullOrEmpty(this.dbConnStr))
                {
                    this.dbConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                }

                return this.dbConnStr;
            }
        }
        #endregion

        #region Construct & Init
        /// <summary>
        /// Init the Command and Connection
        /// </summary>
        /// <param name="cmd">SqlCommand</param>
        /// <param name="conn">SqlConnection</param>
        /// <param name="strSQL">SqlText</param>
        /// <param name="parameterArray">parameterArray</param>
        private void Init(ref SqlCommand cmd, ref SqlConnection conn, string strSQL, SqlParameter[] parameterArray)
        {
            if (string.IsNullOrEmpty(strSQL) || strSQL.Length < 4)
            {
                throw new Exception("The SQL is Error!");
            }

            if (!this.IsInTransaction && !this.keepConnectionOpen)
            {
                conn = new SqlConnection(this.DBConnString);
                cmd = new SqlCommand(strSQL, conn);
                conn.Open();
            }
            else
            {
                if (this.sqlConn == null)
                    throw new Exception("The SqlConnection is not init!");
                cmd = new SqlCommand(strSQL, this.sqlConn);
                if (this.IsInTransaction)
                    cmd.Transaction = this.sqltrans;
                if (this.sqlConn.State != ConnectionState.Open)
                    this.sqlConn.Open();
            }

            if (parameterArray != null)
            {
                SqlParameter[] clonedParameters = new SqlParameter[parameterArray.Length];
                for (int i = 0; i < parameterArray.Length; i++)
                {
                    clonedParameters[i] = (SqlParameter)((ICloneable)parameterArray[i]).Clone();
                }

                cmd.Parameters.AddRange(clonedParameters);
            }

            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 6000;
        }

        /// <summary>
        /// Init the Command and Connection Use Store Procedure
        /// </summary>
        /// <param name="cmd">SqlCommand</param>
        /// <param name="conn">SqlConnection</param>
        /// <param name="strSPName">strSPName</param>
        /// <param name="parameterArray">parameterArray</param>
        private void InitSP(ref SqlCommand cmd, ref SqlConnection conn, string strSPName, SqlParameter[] parameterArray)
        {
            if (strSPName.Length == 0)
            {
                throw new Exception("The Store Procedure Name is not assign!");
            }

            if (!this.IsInTransaction && !this.keepConnectionOpen)
            {
                conn = new SqlConnection(this.DBConnString);
                cmd = new SqlCommand(strSPName, conn);
                conn.Open();
            }
            else
            {
                if (this.sqlConn == null)
                    throw new Exception("The SqlConnection is not init!");
                cmd = new SqlCommand(strSPName, this.sqlConn);
                if (this.IsInTransaction)
                    cmd.Transaction = this.sqltrans;
                if (this.sqlConn.State != ConnectionState.Open)
                    this.sqlConn.Open();
            }

            cmd.CommandType = CommandType.StoredProcedure;
            if (parameterArray != null)
            {
                SqlParameter[] clonedParameters = new SqlParameter[parameterArray.Length];
                for (int i = 0; i < parameterArray.Length; i++)
                {
                    clonedParameters[i] = (SqlParameter)((ICloneable)parameterArray[i]).Clone();
                }

                cmd.Parameters.AddRange(clonedParameters);
            }

            cmd.CommandTimeout = 6000;
        }

        public bool TestDBConnect(out string strErrMsg)
        {
            bool result = false;
            strErrMsg = string.Empty;
            try
            {
                SqlConnection conn = new SqlConnection(this.DBConnString);
                try
                {
                    conn.Open();
                    result = true;
                }
                catch (System.ArgumentException ex)
                {
                    Log.LogErr(ex);
                    strErrMsg = "Login failed for user.\r\n" + ex.Message;
                    return false;
                }
                catch (System.Data.SqlClient.SqlException sqlException)
                {
                    Log.LogErr(sqlException);
                    strErrMsg = "The server was not found or was not accessible. Verify that the instance name is correct and that SQL Server is configured to allow remote connections.\r\n\r\n" + sqlException.Message;
                    return false;
                }
                catch (Exception ex)
                {
                    strErrMsg = ex.Message;
                    result = false;
                }
                finally
                {
                    if (conn != null)
                    {
                        if (conn.State == ConnectionState.Open)
                            conn.Close();
                        conn = null;
                    }
                }
            }
            catch
            {
                result = false;
            }

            return result;
        }
        #endregion

        #region Transaction

        /// <summary>
        /// BeginTransaction
        /// </summary>
        public void BeginTransaction()
        {
            if (this.sqlConn == null)
                this.sqlConn = new SqlConnection(this.DBConnString);
            if (this.sqlConn.State != ConnectionState.Open)
                this.sqlConn.Open();
            this.sqltrans = this.sqlConn.BeginTransaction(this.TransIsolationLevel);
        }

        /// <summary>
        /// BeginTransaction
        /// </summary>
        /// <param name="transisolationLevel">IsolationLevel</param>
        public void BeginTransaction(IsolationLevel transisolationLevel)
        {
            if (this.sqlConn == null)
                this.sqlConn = new SqlConnection(this.DBConnString);
            if (this.sqlConn.State != ConnectionState.Open)
                this.sqlConn.Open();
            this.sqltrans = this.sqlConn.BeginTransaction(transisolationLevel);
        }

        /// <summary>
        /// CommitTransaction
        /// </summary>
        public void CommitTransaction()
        {
            this.CommitTransaction(true);
        }

        /// <summary>
        /// CommitTransaction
        /// </summary>
        /// <param name="closeConnection">closeConnection</param>
        public void CommitTransaction(bool closeConnection)
        {
            this.sqltrans.Commit();
            this.DisposeTransaction(closeConnection);
        }

        /// <summary>
        /// AbortTransaction
        /// </summary>
        public void AbortTransaction()
        {
            if (this.IsInTransaction)
            {
                this.sqltrans.Rollback();
                this.DisposeTransaction(true);
            }
        }

        /// <summary>
        /// AbortTransaction
        /// </summary>
        /// <param name="closeConnection">closeConnection</param>
        public void AbortTransaction(bool closeConnection)
        {
            if (this.IsInTransaction)
            {
                this.sqltrans.Rollback();
                this.DisposeTransaction(closeConnection);
            }
        }

        /// <summary>
        /// DisposeTransaction
        /// </summary>
        /// <param name="closeConnection">CloseConnection</param>
        private void DisposeTransaction(bool closeConnection)
        {
            if (closeConnection && this.sqlConn != null)
            {
                this.sqlConn.Close();
                this.sqlConn.Dispose();
                this.sqlConn = null;
            }

            this.sqltrans.Dispose();
            this.sqltrans = null;
        }

        #endregion

        #region GetDataTable

        /// <summary>
        /// GetDataTable
        /// </summary>
        /// <param name="strSQL">SqlText</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string strSQL)
        {
            return this.GetDataTable(strSQL, null);
        }

        /// <summary>
        /// GetDataTable
        /// </summary>
        /// <param name="strSQL">SqlText</param>
        /// <param name="parameterArray">parameterArray</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string strSQL, SqlParameter[] parameterArray)
        {
            return this.GetDataSet(strSQL, parameterArray).Tables[0];
        }

        #endregion

        #region GetDataView

        /// <summary>
        /// GetDataView
        /// </summary>
        /// <param name="strSQL">SqlText</param>
        /// <returns>DataView</returns>
        public DataView GetDataView(string strSQL)
        {
            return this.GetDataView(strSQL, null);
        }

        /// <summary>
        /// GetDataView
        /// </summary>
        /// <param name="strSQL">SqlText</param>
        /// <param name="parameterArray">parameterArray</param>
        /// <returns>DataView</returns>
        public DataView GetDataView(string strSQL, SqlParameter[] parameterArray)
        {
            return this.GetDataTable(strSQL, parameterArray).DefaultView;
        }

        #endregion

        #region GetPageData

        /// <summary>
        /// GetPageData
        /// </summary>
        /// <param name="strSQL">SqlText</param>
        /// <param name="pageIndex">pageIndex</param>
        /// <param name="pageSize">pageSize</param>
        /// <returns>DataTable</returns>
        public DataTable GetPageData(string strSQL, int pageIndex, int pageSize)
        {
            return this.GetPageData(strSQL, pageIndex, pageSize, null);
        }

        /// <summary>
        /// GetPageData
        /// </summary>
        /// <param name="strSQL">SqlText</param>
        /// <param name="pageIndex">pageIndex</param>
        /// <param name="pageSize">pageSize</param>
        /// <param name="parameterArray">parameterArray</param>
        /// <returns>DataTable</returns>
        public DataTable GetPageData(string strSQL, int pageIndex, int pageSize, SqlParameter[] parameterArray)
        {
            int startNum = (pageIndex - 1) * pageSize;
            DataSet ds = new DataSet();

            SqlDataAdapter da = null;
            SqlCommand cmd = null;
            SqlConnection conn = null;

            try
            {
                da = new SqlDataAdapter();
                this.Init(ref cmd, ref conn, strSQL, parameterArray);
                da.SelectCommand = cmd;
                da.Fill(ds, startNum, pageSize, "tableTmp");
            }
            catch (Exception e)
            {
                if (this.needLog)
                    Log.LogErr(strSQL, e);
                throw;
            }
            finally
            {
                if (!this.IsInTransaction && !this.KeepConnectionOpen)
                {
                    conn.Close();
                    conn.Dispose();
                }

                cmd.Parameters.Clear();
                cmd.Dispose();
                da.Dispose();
            }

            return ds.Tables["tableTmp"];
        }

        #endregion

        #region GetDataSet

        /// <summary>
        /// GetDataSet
        /// </summary>
        /// <param name="strSQL">SqlText</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(string strSQL)
        {
            return this.GetDataSet(strSQL, null);
        }

        /// <summary>
        /// GetDataSet
        /// </summary>
        /// <param name="strSQL">SqlText</param>
        /// <param name="parameterArray">parameterArray</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(string strSQL, SqlParameter[] parameterArray)
        {
            DataSet ds = new DataSet();

            SqlDataAdapter da = null;
            SqlCommand cmd = null;
            SqlConnection conn = null;

            try
            {
                da = new SqlDataAdapter();
                this.Init(ref cmd, ref conn, strSQL, parameterArray);
                da.SelectCommand = cmd;
                ((SqlDataAdapter)da).Fill(ds);
            }
            catch (Exception e)
            {
                if (this.needLog)
                {
                    StringBuilder sbparam = new StringBuilder();
                    sbparam.Append(strSQL + ";\n");
                    if (parameterArray != null)
                    {
                        for (int j = 0; j < parameterArray.Length; j++)
                        {
                            if (parameterArray[j].Value != null)
                            {
                                sbparam.Append(
                                     parameterArray[j].ParameterName + "=" + parameterArray[j].Value.ToString() + ";");
                            }
                            else
                            {
                                sbparam.Append(parameterArray[j].ParameterName + "=null;");
                            }
                        }
                    }

                    Log.LogErr(sbparam.ToString(), e);
                }

                throw;
            }
            finally
            {
                if (!this.IsInTransaction && !this.KeepConnectionOpen)
                {
                    conn.Close();
                    conn.Dispose();
                }

                cmd.Parameters.Clear();
                cmd.Dispose();
                ((IDisposable)da).Dispose();
            }

            return ds;
        }

        #endregion

        #region GetDataReader

        /// <summary>
        /// GetDataReader
        /// </summary>
        /// <param name="strSQL">SqlText</param>
        /// <returns>IDataReader</returns>
        public SqlDataReader GetDataReader(string strSQL)
        {
            return this.GetDataReader(strSQL, null);
        }

        /// <summary>
        /// GetDataReader
        /// </summary>
        /// <param name="strSQL">SqlText</param>
        /// <param name="parameterArray">parameterArray</param>
        /// <returns>IDataReader</returns>
        public SqlDataReader GetDataReader(string strSQL, SqlParameter[] parameterArray)
        {
            SqlCommand cmd = null;
            SqlConnection conn = null;
            SqlDataReader reader;

            try
            {
                this.Init(ref cmd, ref conn, strSQL, parameterArray);
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception e)
            {
                if (!this.IsInTransaction)
                {
                    conn.Close();
                    conn.Dispose();
                }

                if (this.needLog)
                {
                    StringBuilder sbparam = new StringBuilder();
                    sbparam.Append(strSQL + ";\n");
                    if (parameterArray != null)
                    {
                        for (int j = 0; j < parameterArray.Length; j++)
                        {
                            if (parameterArray[j].Value != null)
                            {
                                sbparam.Append(
                                     parameterArray[j].ParameterName + "=" + parameterArray[j].Value.ToString() + ";");
                            }
                            else
                            {
                                sbparam.Append(parameterArray[j].ParameterName + "=null;");
                            }
                        }
                    }

                    Log.LogErr(sbparam.ToString(), e);
                }
                throw;
            }
            finally
            {
                cmd.Parameters.Clear();
                cmd.Dispose();
            }

            return reader;
        }

        #endregion

        #region ExecNonQuery

        /// <summary>
        /// ExecNonQuery
        /// </summary>
        /// <param name="strSQL">SqlText</param>
        /// <returns></returns>
        public int ExecNonQuery(string strSQL)
        {
            return this.ExecNonQuery(strSQL, null);
        }

        /// <summary>
        /// ExecNonQuery
        /// </summary>
        /// <param name="strSQL">SqlText</param>
        /// <param name="parameterArray">parameterArray</param>
        /// <returns></returns>
        public int ExecNonQuery(string strSQL, SqlParameter[] parameterArray)
        {
            int i = 0;
            SqlCommand cmd = null;
            SqlConnection conn = null;
            if (strSQL == string.Empty)
            {
                return i;
            }
            try
            {
                this.Init(ref cmd, ref conn, strSQL, parameterArray);
                i = cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                if (this.needLog)
                {
                    StringBuilder sbparam = new StringBuilder();
                    sbparam.Append(strSQL + ";\n");
                    if (parameterArray != null)
                    {
                        for (int j = 0; j < parameterArray.Length; j++)
                        {
                            if (parameterArray[j].Value != null)
                            {
                                sbparam.Append(
                                     parameterArray[j].ParameterName + "=" + parameterArray[j].Value.ToString() + ";");
                            }
                            else
                            {
                                sbparam.Append(parameterArray[j].ParameterName + "=null;");
                            }
                        }
                    }

                    Log.LogErr(sbparam.ToString(), e);
                }

                throw;
            }
            finally
            {
                if (!this.IsInTransaction && !this.KeepConnectionOpen)
                {
                    conn.Close();
                    conn.Dispose();
                }

                cmd.Parameters.Clear();
                cmd.Dispose();
            }

            return i;
        }
        #endregion

        #region ExecScalar

        /// <summary>
        /// ExecScalar
        /// </summary>
        /// <param name="strSQL">SqlText</param>
        /// <param name="nullValue">when result is null,return nullValue</param>
        /// <returns></returns>
        public object ExecScalar(string strSQL, string nullValue)
        {
            return this.ExecScalar(strSQL, null, nullValue);
        }

        /// <summary>
        /// ExecScalar
        /// </summary>
        /// <param name="strSQL">SqlText</param>
        /// <returns></returns>
        public object ExecScalar(string strSQL)
        {
            return this.ExecScalar(strSQL, null, null);
        }

        /// <summary>
        /// ExecScalar
        /// </summary>
        /// <param name="strSQL">SqlText</param>
        /// <param name="parameterArray">parameterArray</param>
        /// <returns></returns>
        public object ExecScalar(string strSQL, SqlParameter[] parameterArray)
        {
            return this.ExecScalar(strSQL, parameterArray, null);
        }

        /// <summary>
        /// ExecScalar
        /// </summary>
        /// <param name="strSQL">SqlText</param>
        /// <param name="parameterArray">parameterArray</param>
        /// <param name="nullValue">when result is null,return nullValue</param>
        /// <returns></returns>
        public object ExecScalar(string strSQL, SqlParameter[] parameterArray, object nullValue)
        {
            object obj;
            SqlCommand cmd = null;
            SqlConnection conn = null;
            try
            {
                this.Init(ref cmd, ref conn, strSQL, parameterArray);
                obj = cmd.ExecuteScalar();
                if (obj == null || obj == DBNull.Value)
                    obj = nullValue;
            }
            catch (Exception e)
            {
                if (this.needLog)
                {
                    StringBuilder sbparam = new StringBuilder();
                    sbparam.Append(strSQL + ";\n");
                    if (parameterArray != null)
                    {
                        for (int j = 0; j < parameterArray.Length; j++)
                        {
                            if (parameterArray[j].Value != null)
                            {
                                sbparam.Append(
                                     parameterArray[j].ParameterName + "=" + parameterArray[j].Value.ToString() + ";");
                            }
                            else
                            {
                                sbparam.Append(parameterArray[j].ParameterName + "=null;");
                            }
                        }
                    }

                    Log.LogErr(sbparam.ToString(), e);
                }

                throw;
            }
            finally
            {
                if (!this.IsInTransaction && !this.KeepConnectionOpen)
                {
                    conn.Close();
                    conn.Dispose();
                }

                cmd.Parameters.Clear();
                cmd.Dispose();
            }

            return obj;
        }

        #endregion

        #region Store Procedure

        /// <summary>
        /// ExecNonQuery with SP
        /// </summary>
        /// <param name="strSPName">SP Name</param>
        /// <returns></returns>
        public int ExecNonQuerySP(string strSPName)
        {
            int retvalue;
            return this.ExecNonQuerySP(strSPName, out retvalue, null);
        }

        /// <summary>
        /// ExecNonQuery with SP
        /// </summary>
        /// <param name="strSPName">SP Name</param>
        /// <param name="retvalue">SP Return Value</param>
        /// <returns></returns>
        public int ExecNonQuerySP(string strSPName, out int retvalue)
        {
            return this.ExecNonQuerySP(strSPName, out retvalue, null);
        }

        /// <summary>
        /// ExecNonQuery  with SP
        /// </summary>
        /// <param name="strSPName">SP Name</param>
        /// <param name="parameterArray">parameterArray</param>
        /// <returns></returns>
        public int ExecNonQuerySP(string strSPName, SqlParameter[] parameterArray)
        {
            int retvalue;
            return this.ExecNonQuerySP(strSPName, out retvalue, parameterArray);
        }

        /// <summary>
        /// ExecNonQuery  with SP
        /// </summary>
        /// <param name="strSPName">SP Name</param>
        /// <param name="retvalue">SP Return Value</param>
        /// <param name="parameterArray">parameterArray</param>
        /// <returns></returns>
        public int ExecNonQuerySP(string strSPName, out int retvalue, SqlParameter[] parameterArray)
        {
            int i = 0;
            retvalue = 0;
            SqlCommand cmd = null;
            SqlConnection conn = null;
            if (string.IsNullOrEmpty(strSPName))
            {
                throw new Exception("The Store Procedure Name is not assign!");
            }
            try
            {
                this.InitSP(ref cmd, ref conn, strSPName, parameterArray);

                cmd.Parameters.Add("@Ret", SqlDbType.Int);
                cmd.Parameters["@Ret"].Direction = ParameterDirection.ReturnValue;

                i = cmd.ExecuteNonQuery();

                if (parameterArray != null)
                {
                    for (int j = 0; j < parameterArray.Length; j++)
                    {
                        if (parameterArray[j].Direction == ParameterDirection.Output || parameterArray[j].Direction == ParameterDirection.InputOutput)
                        {
                            parameterArray[j].Value = cmd.Parameters[parameterArray[j].ParameterName].Value;
                        }
                    }
                }

                retvalue = (int)cmd.Parameters["@Ret"].Value;
            }
            catch (Exception e)
            {
                if (this.needLog)
                {
                    StringBuilder sbparam = new StringBuilder();
                    sbparam.Append(strSPName + ";\n");
                    if (parameterArray != null)
                    {
                        for (int j = 0; j < parameterArray.Length; j++)
                        {
                            if (parameterArray[j].Value != null)
                            {
                                sbparam.Append(
                                     parameterArray[j].ParameterName + "=" + parameterArray[j].Value.ToString() + ";");
                            }
                            else
                            {
                                sbparam.Append(parameterArray[j].ParameterName + "=null;");
                            }
                        }
                    }

                    Log.LogErr(sbparam.ToString(), e);
                }

                throw;
            }
            finally
            {
                if (!this.IsInTransaction && !this.KeepConnectionOpen)
                {
                    conn.Close();
                    conn.Dispose();
                }

                cmd.Parameters.Clear();
                cmd.Dispose();
            }

            return i;
        }

        /// <summary>
        /// GetDataTable with SP
        /// </summary>
        /// <param name="strSPName">SP Name</param>
        /// <returns></returns>
        public DataTable GetDataTableSP(string strSPName)
        {
            return this.GetDataTableSP(strSPName, null);
        }

        /// <summary>
        /// GetDataTable with SP
        /// </summary>
        /// <param name="strSPName">SP Name</param>
        /// <param name="parameterArray">parameterArray</param>
        /// <returns></returns>
        public DataTable GetDataTableSP(string strSPName, SqlParameter[] parameterArray)
        {
            DataTable dtResult = new DataTable();
            SqlCommand cmd = null;
            SqlConnection conn = null;
            if (string.IsNullOrEmpty(strSPName))
            {
                throw new Exception("The Store Procedure Name is not assign!");
            }
            try
            {
                this.InitSP(ref cmd, ref conn, strSPName, parameterArray);

                // Define the data adapter and fill the dataset 
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dtResult);
                }
            }
            catch (Exception e)
            {
                if (this.needLog)
                {
                    StringBuilder sbparam = new StringBuilder();
                    sbparam.Append(strSPName + ";\n");
                    if (parameterArray != null)
                    {
                        for (int j = 0; j < parameterArray.Length; j++)
                        {
                            if (parameterArray[j].Value != null)
                            {
                                sbparam.Append(
                                     parameterArray[j].ParameterName + "=" + parameterArray[j].Value.ToString() + ";");
                            }
                            else
                            {
                                sbparam.Append(parameterArray[j].ParameterName + "=null;");
                            }
                        }
                    }

                    Log.LogErr(sbparam.ToString(), e);
                }

                throw;
            }
            finally
            {
                if (!this.IsInTransaction && !this.KeepConnectionOpen)
                {
                    conn.Close();
                    conn.Dispose();
                }

                cmd.Parameters.Clear();
                cmd.Dispose();
            }

            return dtResult;
        }
        #endregion

        #region WriteDataToTempTable
        private void CreateTmpTable(DataTable dt)
        {
            string s = string.Empty;
            if (string.IsNullOrEmpty(dt.TableName))
                dt.TableName = "#tempTable";

            string coll = this.ExecScalar("SELECT SERVERPROPERTY('collation')").ToString();
            foreach (DataColumn dc in dt.Columns)
            {
                s += ",[" + dc.ColumnName + "]";
                string t = dc.DataType.BaseType.Name.ToLower();
                if (t.IndexOf("int") > -1)
                {
                    s += " int null";
                }
                else
                    if (t.IndexOf("double") > -1)
                    {
                        s += " double null";
                    }
                    else
                        if (t.IndexOf("float") > -1)
                        {
                            s += " float null";
                        }
                        else
                            if (t.IndexOf("date") > -1)
                            {
                                s += " datetime null";
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(coll))
                                {
                                    s += " varchar(50) null ";
                                }
                                else
                                {
                                    s += " varchar(50) COLLATE " + coll + " null";
                                }
                            }
            }

            string sql = string.Concat("create table #", dt.TableName.Replace("#", string.Empty), " (", s.Substring(1), ")");
            this.ExecNonQuery(sql);
        }

        public void WriteData(DataTable dt, string tableName)
        {
            SqlConnection conn = null;
            try
            {
                try
                {
                    if (!this.IsInTransaction && !this.keepConnectionOpen)
                    {
                        conn = new SqlConnection(this.DBConnString);
                        conn.Open();
                    }
                    else
                    {
                        conn = this.sqlConn;
                        if (conn.State != ConnectionState.Open)
                            conn.Open();
                    }

                    if (string.IsNullOrEmpty(tableName))
                    {
                        if (string.IsNullOrEmpty(dt.TableName))
                            dt.TableName = "#tempTable";
                        this.CreateTmpTable(dt);
                        tableName = dt.TableName;
                    }

                    SqlBulkCopy sbc;
                    if (!this.IsInTransaction)
                    {
                        sbc = new SqlBulkCopy(conn);
                    }
                    else
                    {
                        sbc = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, this.sqltrans);
                    }

                    sbc.DestinationTableName = tableName;

                    // Set up the column mappings by name.
                    foreach (DataColumn dcItem in dt.Columns)
                    {
                        SqlBulkCopyColumnMapping mapID = new SqlBulkCopyColumnMapping(dcItem.ColumnName, dcItem.ColumnName);
                        sbc.ColumnMappings.Add(mapID);
                    }

                    sbc.WriteToServer(dt);
                    sbc.Close();
                }
                catch (Exception e)
                {
                    if (this.needLog)
                        Log.LogErr(e);
                    throw;
                }
            }
            finally
            {
                if (!this.IsInTransaction && !this.KeepConnectionOpen)
                {
                    conn.Close();
                }
            }
        }
        #endregion

        #region UpdateDataset

        /// <summary>
        /// update table by DataSet
        /// </summary>
        /// <param name="dataSet">DataSet</param>
        /// <param name="tableName">tableName</param>
        public int UpdateDataset(DataSet dataSet, string tableName)
        {
            return this.UpdateDataset(dataSet, tableName, string.Empty, string.Empty, null, null);
        }

        /// <summary>
        /// update table by DataSet
        /// </summary>   
        /// <param name="dataSet">DataSet</param>
        /// <param name="tableName">tableName</param>
        /// <param name="deleteCommandText">deleteCommandText</param>
        /// <param name="updateCommandText">updateCommandText</param>
        /// <param name="needUpdateColName">needUpdateColName,use for updateCommand and deleteCommandText</param>
        /// <param name="prikeyColName">prikeyColName,use for updateCommand and deleteCommandText</param>
        public int UpdateDataset(DataSet dataSet, string tableName, string deleteCommandText, string updateCommandText,
            List<string> needUpdateColName, List<string> prikeyColName)
        {
            int result = 0;
            string sSql = "Select * from " + tableName;

            if (!this.IsInTransaction && !this.keepConnectionOpen && this.sqlConn == null)
            {
                this.sqlConn = new SqlConnection(this.DBConnString);
            }

            if (this.sqlConn.State == ConnectionState.Closed)
                this.sqlConn.Open();

            try
            {
                // Create a SqlDataAdapter, and dispose of it after we are done	
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(sSql, this.sqlConn))
                {
                    dataAdapter.UpdateBatchSize = 10;
                    if (this.IsInTransaction)
                    {
                        dataAdapter.SelectCommand.Transaction = this.sqltrans;
                    }

                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                    commandBuilder.QuotePrefix = "[";
                    commandBuilder.QuoteSuffix = "]";

                    dataAdapter.InsertCommand = commandBuilder.GetInsertCommand();

                    if (string.IsNullOrEmpty(deleteCommandText))
                    {
                        dataAdapter.DeleteCommand = commandBuilder.GetDeleteCommand();
                    }
                    else
                    {
                        dataAdapter.DeleteCommand = new SqlCommand(deleteCommandText, this.sqlConn);

                        int priKeyCount = 0;
                        foreach (SqlParameter mParam in commandBuilder.GetDeleteCommand().Parameters)
                        {
                            if (priKeyCount < prikeyColName.Count && prikeyColName.Contains(mParam.SourceColumn))
                            {
                                dataAdapter.DeleteCommand.Parameters.Add(string.Format("@{0}", mParam.SourceColumn),
                                    mParam.SqlDbType, mParam.Size, mParam.SourceColumn);
                                priKeyCount++;
                            }

                            if (priKeyCount == prikeyColName.Count)
                            {
                                break;
                            }
                        }

                        dataAdapter.DeleteCommand.UpdatedRowSource = UpdateRowSource.None;
                    }

                    if (string.IsNullOrEmpty(updateCommandText))
                    {
                        dataAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
                    }
                    else
                    {
                        dataAdapter.UpdateCommand = new SqlCommand(updateCommandText, this.sqlConn);

                        int priKeyCount = 0;
                        int updateColCount = 0;

                        foreach (SqlParameter mParam in commandBuilder.GetUpdateCommand().Parameters)
                        {
                            if (priKeyCount < prikeyColName.Count && prikeyColName.Contains(mParam.SourceColumn.ToUpper()))
                            {
                                dataAdapter.UpdateCommand.Parameters.Add(string.Format("@{0}", mParam.SourceColumn),
                                    mParam.SqlDbType, mParam.Size, mParam.SourceColumn);
                                priKeyCount++;
                            }

                            if (updateColCount < needUpdateColName.Count && needUpdateColName.Contains(mParam.SourceColumn.ToUpper()))
                            {
                                dataAdapter.UpdateCommand.Parameters.Add(string.Format("@{0}", mParam.SourceColumn),
                                    mParam.SqlDbType, mParam.Size, mParam.SourceColumn);
                                updateColCount++;
                            }

                            if (priKeyCount == prikeyColName.Count && updateColCount == needUpdateColName.Count)
                            {
                                break;
                            }
                        }

                        dataAdapter.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;
                    }

                    if (this.IsInTransaction)
                    {
                        dataAdapter.InsertCommand.Transaction = this.sqltrans;
                        dataAdapter.UpdateCommand.Transaction = this.sqltrans;
                        dataAdapter.DeleteCommand.Transaction = this.sqltrans;
                    }

                    result = dataAdapter.Update(dataSet, tableName);
                    dataSet.AcceptChanges();
                    return result;
                }
            }
            catch (Exception e)
            {
                if (this.needLog)
                {
                    StringBuilder sbparam = new StringBuilder();
                    sbparam.Append(tableName + ";\n");
                    Log.LogErr(sbparam.ToString(), e);
                }

                throw;
            }
            finally
            {
                if (!this.IsInTransaction && !this.KeepConnectionOpen)
                {
                    this.sqlConn.Close();
                }
            }
        }

        #endregion

        #region Dispose / Destructor
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose of remaining objects.
                if (this.sqlConn != null)
                {
                    this.sqlConn.Close();
                    this.sqlConn.Dispose();
                    this.sqlConn = null;
                }

                if (this.sqltrans != null)
                {
                    this.sqltrans.Dispose();
                    this.sqltrans = null;
                }
            }
        }

        #endregion
    }

    public class DBConnectParams
    {
        private string _server = string.Empty, _database = string.Empty, _userId = string.Empty, _password = string.Empty;
        private VerifyTypeEnum _verifyType = VerifyTypeEnum.Database;
        private int _maxPool = 100, _connectTimeout = 30000;

        /// <summary>
        /// New with Empty
        /// </summary>
        public DBConnectParams()
        {
            this._verifyType = VerifyTypeEnum.Windows;
            this._server = string.Empty;
        }

        /// <summary>
        /// New with VerifyTypeEnum.Windows
        /// </summary>
        /// <param name="strServer">serverName</param>
        /// <param name="strDB">database</param>
        public DBConnectParams(string strServer, string strDB)
        {
            this._verifyType = VerifyTypeEnum.Windows;
            this._server = strServer;
            this._database = strDB;
        }

        /// <summary>
        /// New with VerifyTypeEnum.Database
        /// </summary>
        /// <param name="strServer">servername</param>
        /// <param name="strDB">DataBase</param>
        /// <param name="strUserID">UserID</param>
        /// <param name="strPwd">UserPassword</param>
        public DBConnectParams(string strServer, string strDB, string strUserID, string strPwd)
        {
            this._verifyType = VerifyTypeEnum.Database;
            this._server = strServer;
            this._database = strDB;
            this._userId = strUserID;
            this._password = strPwd;
        }

        /// <summary>
        /// Data Source=*;Initial Catalog=*;Connect Timeout=0;Persist Security Info=True;User ID=*;Password=*      
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        public DBConnectParams(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new Exception("Fail to get ConnectionString,Please check config file! ");
            else
            {
                MatchCollection matches = Regex.Matches(connectionString, @"(([^;=]+)=([^;=#]*))", RegexOptions.Compiled);
                if (!connectionString.ToLower().Contains("integrated security"))
                {
                    foreach (Match m in matches)
                    {
                        switch (m.Groups[2].Value.ToLower().Trim())
                        {
                            case "data source":
                            case "server":
                                this._server = m.Groups[3].Value;
                                break;
                            case "initial catalog":
                            case "database":
                                this._database = m.Groups[3].Value;
                                break;
                            case "user id":
                            case "uid":
                                this._userId = m.Groups[3].Value;
                                break;
                            case "password":
                                this._password = m.Groups[3].Value;
                                break;
                            case "pwd":
                                this._password = m.Groups[3].Value;
                                break;
                        }
                    }

                    if (string.IsNullOrEmpty(this._server) || string.IsNullOrEmpty(this._database)
                        || string.IsNullOrEmpty(this._userId))
                        throw new Exception("Fail to get ConnectionString,Please check! ");
                    this._verifyType = VerifyTypeEnum.Database;
                }
                else
                {
                    foreach (Match m in matches)
                    {
                        switch (m.Groups[2].Value.ToLower().Trim())
                        {
                            case "data source":
                            case "server":
                                this._server = m.Groups[3].Value;
                                break;
                            case "initial catalog":
                            case "database":
                                this._database = m.Groups[3].Value;
                                break;
                        }
                    }

                    if (string.IsNullOrEmpty(this._server) || string.IsNullOrEmpty(this._database))
                        throw new Exception("Fail to get ConnectionString,Please check! ");
                    this._verifyType = VerifyTypeEnum.Windows;
                }
            }
        }

        public bool IsEmpty
        {
            get
            {
                return this._server.Equals(string.Empty);
            }
        }

        public VerifyTypeEnum VerifyType
        {
            get
            {
                return this._verifyType;
            }
            set
            {
                this._verifyType = value;
            }
        }

        public string Server
        {
            get
            {
                return this._server;
            }
            set
            {
                this._server = value;
            }
        }

        public string Database
        {
            get
            {
                return this._database;
            }
            set
            {
                this._database = value;
            }
        }

        public string UserID
        {
            get
            {
                return this._userId;
            }
            set
            {
                this._userId = value;
            }
        }

        public string Password
        {
            get
            {
                return this._password;
            }
            set
            {
                this._password = value;
            }
        }

        public int MaxPoolSize
        {
            get
            {
                return this._maxPool;
            }
            set
            {
                this._maxPool = value;
            }
        }

        public int ConnectTimeout
        {
            get
            {
                return this._connectTimeout;
            }
            set
            {
                this._connectTimeout = value;
            }
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(this._database) || string.IsNullOrEmpty(this._server))
            {
                return string.Empty;
            }

            if (this._verifyType == VerifyTypeEnum.Windows)
            {
                return string.Format("Integrated Security=SSPI;Persist Security Info=False;"
                    + "Data Source={0};Initial Catalog={1};Connect Timeout={2};Max Pool Size={3}", this._server, this._database, this._connectTimeout, this._maxPool);
            }
            else
            {
                return string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3};Connect Timeout={4};Max Pool Size={5} ",
                    this._server, this._database, this._userId, this._password, this._connectTimeout, this._maxPool);
            }
        }
    }
}