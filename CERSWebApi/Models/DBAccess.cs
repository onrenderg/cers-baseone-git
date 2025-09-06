using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CERSWebApi.Models
{
    public class DBAccess
    {
        private SqlCommand cmd = new SqlCommand();
        private SqlDataAdapter da = new SqlDataAdapter();
        public DataTable getDBData(SqlCommand cmdparameters, string spname, string ConnName = "DBConn")
        {
            var response = new Generic_Responce();
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnName].ToString());
            cmd = cmdparameters;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = spname;
            
            cmd.Connection = sqlConnection;
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
           
            try
            {
                da.Fill(dt);
                dt.TableName = "OK";
                response.status_code = 200;
                response.developer_message = "DB OK";
                return dt;
            }
            catch (Exception e)
            {
                //'Cannot insert duplicate key row in object 'sec.FLCDetails' with unique index 'IX_FLCDetails'. The duplicate key value is (<NULL>).
                dt.TableName = e.Message;
                response.status_code = 500;
                response.developer_message = e.Message;
                return dt;
            }
            finally
            {
                // sqlConnection.Close()
                sqlConnection.Dispose();
            }
        }

        public DataSet getDBDataSet(SqlCommand cmdparameters, string spname, string ConnName = "DBConn")
        {
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnName].ToString());
            cmd = cmdparameters;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = spname;

            cmd.Connection = sqlConnection;
            //cmd.CommandTimeout = 240;
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            try
            {

                da.Fill(ds);
                return ds;
            }
            catch (Exception e)
            {
                //da.TableName = e.Message;
                return ds;
            }
            finally
            {
                // sqlConnection.Close()
                sqlConnection.Dispose();
            }
        }

    }
    public class DBAccess_DataSet
    {
        private SqlCommand cmd = new SqlCommand();
        private SqlDataAdapter da = new SqlDataAdapter();
        public List<DataTable> getDBData(SqlCommand cmdparameters, string spname)
        {
            var response = new Generic_Responce();
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConn"].ToString());
            cmd = cmdparameters;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = spname;

            cmd.Connection = sqlConnection;
            da.SelectCommand = cmd;
            DataSet ds = new DataSet();
            List<DataTable> CompeteData = new List<DataTable>();
            try
            {
                da.Fill(ds);
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    CompeteData.Add(ds.Tables[i]);
                }
                response.status_code = 200;
                response.developer_message = "DB OK";
                return CompeteData;
            }
            catch (Exception e)
            {
                response.status_code = 500;
                response.developer_message = e.Message;
                return CompeteData;
            }
            finally
            {
                // sqlConnection.Close()
                sqlConnection.Dispose();
            }
        }
    }

   
}