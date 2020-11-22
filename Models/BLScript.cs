using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SpScriptGenrator.Models
{
    public class BLScript
    {
        static List<SqlParameter> lstParams;
        public static DataTable GetAllSPNames()
        {
            try
            {
                lstParams = new List<SqlParameter>();

                string strQuery = @"SELECT 
	                                    name,
                                        create_date,
                                        modify_date
                                    FROM 
	                                    sys.procedures
                                    ORDER BY 
	                                    modify_date DESC";

                return DLScript.GetResultDataTable(strQuery, CommandType.Text, lstParams);
            }
            catch
            {
                throw;
            }
        }
        public static DataSet SPSchemaReader(string strSPName)
        {
            try
            {
                return DLScript.SPSchemaReader(strSPName);
            }
            catch
            {
                throw;
            }
        }

        public static DataTable GetModifyedSPNamesBetweenDates(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                lstParams = new List<SqlParameter>();

                string strQuery = @"SELECT 
	                                    name,
                                        create_date,
                                        modify_date
                                    FROM 
	                                    sys.procedures
                                    WHERE
	                                    modify_date BETWEEN @FromDate AND @ToDate
                                    ORDER BY 
	                                    modify_date DESC";

                lstParams.Add(new SqlParameter() { ParameterName = "@FromDate", SqlDbType = SqlDbType.DateTime, Value = FromDate });
                lstParams.Add(new SqlParameter() { ParameterName = "@ToDate", SqlDbType = SqlDbType.DateTime, Value = ToDate });

                return DLScript.GetResultDataTable(strQuery, CommandType.Text, lstParams);
            }
            catch
            {
                throw;
            }
        }
    }
}