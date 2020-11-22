using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SpScriptGenrator.Models
{
    public class BaseClass
    {
        static string _staticConnectionString;
        bool _isDisposed = false;

        public static SqlConnection StaticSqlConnection
        {
            get
            {

                SqlConnection staticConnection = new SqlConnection();
                staticConnection.ConnectionString = StaticConnectionString;
                return staticConnection;
            }
        }

        public static string StaticConnectionString
        {
            set { _staticConnectionString = value; }
            get
            {
                if (!string.IsNullOrEmpty(_staticConnectionString))
                    return _staticConnectionString;

                //CVSystem.Decrypt objdecrypt = new CVSystem.Decrypt();
                
                //string str = ConfigurationManager.ConnectionStrings["ERP365CloudDB"].ConnectionString; //ConfigWrapper.GetConnectionString("ERP365CloudDB");
                //string con = str.Split(';')[3].Remove(0, 9);
                //string decon = objdecrypt.decryption(con);
                //con = str.Replace(con, decon);

                string con = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString; 

                //for connection getting create ConfigWrapper class
                // return ConfigWrapper.GetConnectionString("ERP365CloudDB");
                return con;
            }


        }
    }
}