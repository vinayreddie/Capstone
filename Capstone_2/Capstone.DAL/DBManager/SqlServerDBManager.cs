using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Framework;

namespace  Capstone.DAL
{
    public class SqlServerDBManager
    {
        #region Global
        SqlCommand command;        
        string ConStr;
        #endregion

        #region contructor
        public SqlServerDBManager()
        {
           ConStr =ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ConStr"].ToUpper()].ConnectionString;
        }

        #endregion

        #region Readers

        public DataTable ExecuteQuery(string Query)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConStr))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(Query, connection))
                    {
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            dt.Load(dr);
                        }
                    }
                    return dt;
                }
                //catch (DBConnectionException dbEx)
                //{
                //    throw new DBConnectionException(dbEx.Message);
                //}
                catch (SqlException Ex)
                {
                    //JScript.ShowDBConAlert();
                    //Logger.LogError("SQLException", Ex.Message);
                    return null;
                }
                catch (Exception Ex)
                {
                    throw new Exception(Ex.Message);
                }
            }
        }

        public DataTable ExecuteQuery(string Query, params object[] ParamsList)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConStr))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(Query, connection))
                    {
                        PrepareParameters(cmd, ParamsList);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            dt.Load(dr);
                        }
                    }
                    return dt;
                }
                //catch (DBConnectionException dbEx)
                //{
                //    throw new DBConnectionException(dbEx.Message);
                //}
                catch (Exception Ex)
                {
                    throw new Exception(Ex.Message);
                }
            }
        }

        public DataTable ExecuteQueryWithDA(string Query)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConStr))
            {
                try
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(Query, connection))
                    {
                        da.SelectCommand.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeout"]);
                        da.Fill(dt);
                    }
                    return dt;
                }
                //catch (DBConnectionException dbEx)
                //{
                //    throw new DBConnectionException(dbEx.Message);
                //}
                catch (Exception Ex)
                {
                    //JScript.ShowDBConAlert();
                    //Logger.LogError("SQLException", Ex.Message);
                    return null;
                }
            }
        }

        public DataTable ExecuteQueryWithDA(string Query, List<SqlParameter> ParamsList)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConStr))
            {
                try
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(Query, connection))
                    {
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeout"]);

                        foreach (var param in ParamsList)
                        {
                            da.SelectCommand.Parameters.Add(param.ParameterName, param.SqlDbType).Value = param.Value;
                        }

                        da.Fill(dt);
                    }
                    return dt;
                }
                //catch (DBConnectionException dbEx)
                //{
                //    throw new DBConnectionException(dbEx.Message);
                //}
                catch (Exception Ex)
                {
                    //JScript.ShowDBConAlert();
                    //Logger.LogError("SQLException", Ex.Message);
                    return null;
                }
            }
        }

        public DataSet ExecuteQueryWithDADS(string Query)
        {
            using (SqlConnection connection = new SqlConnection(ConStr))
            {
                try
                {
                    DataSet ds = new DataSet();
                    using (SqlDataAdapter da = new SqlDataAdapter(Query, connection))
                    {
                        da.SelectCommand.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeout"]);
                        da.Fill(ds);
                    }
                    return ds;
                }
                //catch (DBConnectionException dbEx)
                //{
                //    throw new DBConnectionException(dbEx.Message);
                //}
                catch (Exception Ex)
                {
                    //JScript.ShowDBConAlert();
                    //Logger.LogError("SQLException", Ex.Message);
                    return null;
                }
            }
        }

        public int ExecuteQuery(string Query, int a)
        {
            int maxid = 0;
            using (SqlConnection connection = new SqlConnection(ConStr))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(Query, connection))
                    {
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                string id = dr[0].ToString();
                                if (id == "")
                                {
                                    maxid = 0;
                                }
                                else
                                {
                                    maxid = Convert.ToInt32(dr[0]);
                                }
                            }
                        }
                    }
                    return maxid;
                }
                //catch (DBConnectionException dbEx)
                //{
                //    throw new DBConnectionException(dbEx.Message);
                //}
                catch (Exception Ex)
                {
                    throw new Exception(Ex.Message);
                }
            }
        }

        public DataTable ExecuteStoredProc(string Query)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConStr))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(Query, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            dt.Load(dr);
                        }
                    }
                    return dt;
                }
                //catch (DBConnectionException dbEx)
                //{
                //    throw new DBConnectionException(dbEx.Message);
                //}
                catch (Exception Ex)
                {
                    throw new Exception(Ex.Message);
                }
            }
        }

        public DataTable ExecuteStoredProc(string Query, List<SqlParameter> ParamsList)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConStr))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(Query, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        foreach (SqlParameter Param in ParamsList)
                        {
                            cmd.Parameters.Add(Param);
                        }
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            dt.Load(dr);
                        }
                    }
                    return dt;
                }
                //catch (DBConnectionException dbEx)
                //{
                //    throw new DBConnectionException(dbEx.Message);
                //}
                catch (Exception Ex)
                {
                    throw new Exception(Ex.Message);
                }
            }
        }


        public object ExecuteScalar(string Query)
        {
            object obj;
            using (SqlConnection connection = new SqlConnection(ConStr))
            {
                try
                {
                    connection.Open();
                    DataTable dt = new DataTable();
                    using (SqlCommand cmd = new SqlCommand(Query, connection))
                    {
                        obj = cmd.ExecuteScalar();
                    }
                    return obj;
                }
                //catch (DBConnectionException dbEx)
                //{
                //    throw new DBConnectionException(dbEx.Message);
                //}
                catch (Exception Ex)
                {
                    throw new Exception(Ex.Message);
                }
            }
        }

        public object ExecuteScalar(string Query, params object[] ParamsList)
        {
            object obj;
            using (SqlConnection connection = new SqlConnection(ConStr))
            {
                try
                {
                    connection.Open();
                    DataTable dt = new DataTable();
                    using (SqlCommand cmd = new SqlCommand(Query, connection))
                    {
                        foreach (SqlParameter Param in ParamsList)
                        {
                            cmd.Parameters.Add(Param);
                        }
                        obj = cmd.ExecuteScalar(); 
                    }
                    return obj;
                }
                //catch (DBConnectionException dbEx)
                //{
                //    throw new DBConnectionException(dbEx.Message);
                //}
                catch (Exception Ex)
                {
                    throw new Exception(Ex.Message);
                }
            }
        }

        public DataSet ExecuteSPMultipleResultSet(string Query, List<SqlParameter> ParamsList)
        {
            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(ConStr))
            {
                try
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(Query, connection))
                    {
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        foreach (SqlParameter Param in ParamsList)
                        {
                            da.SelectCommand.Parameters.Add(Param);
                        }
                        da.Fill(ds);
                        return ds;
                    }
                }

                //catch (DBConnectionException Ex)
                //{
                //    Logger.LogError("SQLException", Ex.Message);
                //    return null;
                //}
                catch (Exception Ex)
                {
                    //Logger.LogError("SQLException", Ex.Message);
                    return null;
                }
            }
        }

        public DataSet ExecuteSPMultipleResultSet(string Query)
        {
            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(ConStr))
            {
                try
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(Query, connection))
                    {
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.Fill(ds);
                        return ds;
                    }
                }

                //catch (DBConnectionException Ex)
                //{
                //    Logger.LogError("SQLException", Ex.Message);
                //    return null;
                //}
                catch (Exception Ex)
                {
                    //Logger.LogError("SQLException", Ex.Message);
                    return null;
                }
            }
        }

        #endregion

        #region Writers

        public int ExecuteNonQuery(string Query)
        {
            int ival;
            using (SqlConnection connection = new SqlConnection(ConStr))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(Query, connection))
                    {
                        ival = cmd.ExecuteNonQuery();
                    }
                    return ival;
                }
                //catch (DBConnectionException Ex)
                //{
                //    Logger.LogError("SQLException", Ex.Message);
                //    throw new DBConnectionException(Ex.Message);
                //}
                catch (Exception Ex)
                {
                    // Logger.LogError("SQLException", Ex.Message);
                    throw new Exception(Ex.Message);
                }
            }
        }

        public int ExecuteNonQuery(string Query, params object[] ParamsList)
        {
            int ival;
            using (SqlConnection connection = new SqlConnection(ConStr))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(Query, connection))
                    {
                        PrepareParameters(cmd, ParamsList);
                        ival = cmd.ExecuteNonQuery();
                    }
                    return ival;
                }
                //catch (DBConnectionException Ex)
                //{
                //    Logger.LogError("SQLException", Ex.Message);
                //    throw new DBConnectionException(Ex.Message);
                //}
                catch (Exception Ex)
                {
                    //Logger.LogError("SQLException", Ex.Message);
                    throw new Exception(Ex.Message);
                }
            }
        }

        public SqlCommand ExecuteNonQuery(string Query, List<SqlParameter> ParamList)
        {
            using (SqlConnection connection = new SqlConnection(ConStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(Query, connection);
                   // connection.Open();
                    foreach (SqlParameter Param in ParamList)
                    {
                        cmd.Parameters.Add(Param);
                    }
                    cmd.ExecuteNonQuery();
                    return cmd;
                }
                //catch (DBConnectionException Ex)
                //{
                //    Logger.LogError("SQLException", Ex.Message);
                //    throw new DBConnectionException(Ex.Message);
                //}
                catch (Exception Ex)
                {
                    //Logger.LogError("SQLException", Ex.Message);
                    throw new Exception(Ex.Message);
                }
            }
        }

        public SqlCommand ExecuteProcedure(string Query, List<SqlParameter> ParamList)
        {
            using (SqlConnection connection = new SqlConnection(ConStr))
            {
                try
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(Query, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeout"]);

                    foreach (SqlParameter Param in ParamList)
                    {
                        cmd.Parameters.Add(Param);
                    }
                    // Logger.SetStartTime();
                    cmd.ExecuteNonQuery();
                    return cmd;
                }
                catch (Exception Ex)
                {
                    throw new Exception(Ex.Message);
                }
            }
        }

        public SqlCommand ExecuteProcedure(string Query, List<SqlParameter> ParamList, SqlConnection connection, ref SqlTransaction Trans)
        {

            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                SqlCommand cmd = new SqlCommand(Query, connection, Trans);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeout"]);

                foreach (SqlParameter Param in ParamList)
                {
                    cmd.Parameters.Add(Param);
                }
                // Logger.SetStartTime();
                cmd.ExecuteNonQuery();
                return cmd;
            }
            //catch (DBConnectionException Ex)
            //{
            //    Logger.LogError("SQLException", Ex.Message);
            //    throw new DBConnectionException(Ex.Message);
            //}
            catch (Exception Ex)
            {
                //JScript.ShowDBConAlert();
                //Logger.SetEndTime();
                //Logger.LogError("Time Taken :", Logger.GetTimeTaken());
                //Logger.LogError("SQLException", Ex.Message);                
                return null;
            }

        }

        public SqlCommand ExecuteProcedure(string storedProcedure, List<SqlParameter> paramList, ref SqlTransaction transaction)
        {
            try
            {
                command = new SqlCommand(storedProcedure, transaction.Connection, transaction);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeout"]);

                foreach (SqlParameter Param in paramList)
                    command.Parameters.Add(Param);

                command.ExecuteNonQuery();
                return command;
            }
            catch (Exception ex)
            {
                ExceptionModel exception = ExceptionHandling.GetExceptionDetails(ex);
                Logger.LogError(exception);
                return null;
            }
            finally
            {
                command.Dispose();
            }

        }

        #endregion

        #region Preparations
        private void PrepareParameters(SqlCommand cmd, object[] ParamsList)
        {
            for (int i = 0; i < (ParamsList.Length / 2); i++)
            {
                cmd.Parameters.AddWithValue(ParamsList[i * 2].ToString(), ParamsList[(i * 2) + 1]);
            }
        }
        #endregion
    }
}
