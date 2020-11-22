using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace GenerateSPScript.Common
{
    public class AppConstant
    {

        public static void WriteLogFile(string str)
        {

            try
            {

                if (!String.IsNullOrEmpty(str))
                {
                    try
                    {
                        string filename = "Log_" + DateTime.Now.ToString("MMMddyyyy");

                        string todaysfilepath = HttpContext.Current.Server.MapPath(@"\LogFiles\" + filename + ".txt");

                        if (!File.Exists(todaysfilepath))
                        {
                            File.Create(todaysfilepath).Close();
                        }
                        using (StreamWriter sw = File.AppendText(todaysfilepath))
                        {
                            sw.WriteLine("#");
                            sw.WriteLine(DateTime.Now.ToString());
                            sw.WriteLine(str);
                        }
                    }
                    catch (Exception ex)
                    {
                        //    AppConstant.WriteLogFile(Convert.ToString(ex));
                    }
                }
            }
            catch
            {

                throw;
            }

        }
    }
}