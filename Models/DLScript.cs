using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SpScriptGenrator.Models
{
    public class DLScript : BaseClass
    {

        public static DataSet SPSchemaReader(string strSPName)
        {
            var staticConnection = StaticSqlConnection;

            var command = new SqlCommand
            {
                CommandText = "sys.sp_helptext",
                CommandType = CommandType.StoredProcedure,
                Connection = staticConnection
            };
            command.Parameters.AddWithValue("@objname", strSPName);

            var objDataAdapter = new SqlDataAdapter();
            var ds = new DataSet();
            try
            {
                staticConnection.Open();
                objDataAdapter.SelectCommand = command;
                objDataAdapter.Fill(ds);
                //SqlDataReader reader = command.ExecuteReader();
                //dt = reader.GetSchemaTable();
                //reader.Close();
                staticConnection.Close();
                return ds;
            }
            catch
            {
                throw;
            }
            finally
            {
                ds.Dispose();
                //objDataAdapter.Dispose();
                staticConnection.Close();
                command.Dispose();
            }
        }



        public static DataTable GetResultDataTable(string strQuery, CommandType SqlcommandType, List<SqlParameter> lstParams)
        {
            var staticConnection = StaticSqlConnection;

            var command = new SqlCommand
            {
                CommandText = strQuery,
                CommandType = SqlcommandType,
                Connection = staticConnection,
            };

            if (lstParams != null)
                command.Parameters.AddRange(lstParams.ToArray());


            var objDataAdapter = new SqlDataAdapter();
            var dt = new DataTable();
            try
            {
                staticConnection.Open();
                objDataAdapter.SelectCommand = command;
                objDataAdapter.Fill(dt);
                staticConnection.Close();
                return dt;
            }
            catch
            {
                throw;
            }
            finally
            {
                dt.Dispose();
                objDataAdapter.Dispose();
                staticConnection.Close();
                command.Dispose();
            }
        }


        public static DataTable GetModifyedSPNames(string strSPName)
        {
            var staticConnection = StaticSqlConnection;

            var command = new SqlCommand
            {
                CommandText = @"select name,
                                      create_date,
                                      modify_date
                                from sys.procedures
                                where
	                                modify_date between '2020-11-08' AND GETDATE()
                                order by modify_date desc",

                CommandType = CommandType.Text,
                Connection = staticConnection

            };

            command.Parameters.AddWithValue("@test", 1);

            var objDataAdapter = new SqlDataAdapter();
            var dt = new DataTable();
            try
            {
                staticConnection.Open();
                objDataAdapter.SelectCommand = command;
                objDataAdapter.Fill(dt);
                staticConnection.Close();
                return dt;
            }
            catch
            {
                throw;
            }
            finally
            {
                dt.Dispose();
                objDataAdapter.Dispose();
                staticConnection.Close();
                command.Dispose();
            }
        }

    }
}