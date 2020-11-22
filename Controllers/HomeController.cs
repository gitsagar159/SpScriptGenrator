using GenerateSPScript.Common;
using SpScriptGenrator.Models;
using SpScriptGenrator.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SpScriptGenrator.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllSP()
        {
            List<SPNames> lstAllSP = new List<SPNames>();
            try
            {
                DataTable SPNameData = BLScript.GetAllSPNames();

                if (SPNameData != null && SPNameData.Rows.Count > 0)
                {
                    foreach (DataRow rowItem in SPNameData.Rows)
                    {
                        SPNames objSPName = new SPNames()
                        {
                            SPName = Convert.ToString(rowItem["name"]),
                            Create_Date = Convert.ToString(rowItem["create_date"]),
                            Modify_Date = Convert.ToString(rowItem["modify_date"])

                        };
                        lstAllSP.Add(objSPName);
                    }
                }

            }
            catch (Exception ex)
            {
                AppConstant.WriteLogFile(ex.ToString());
            }

            return View(lstAllSP);
        }

        [HttpGet]
        public ActionResult GetSPBetweenDates()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetSPBetweenDates(FormCollection objCollection)
        {
            
            List<SPNames> lstAllSP = new List<SPNames>();
            try
            {
                string strFromDate = objCollection["fromdate"] != null ? Convert.ToString(objCollection["fromdate"]) : "";
                string strToDate = objCollection["todate"] != null ? Convert.ToString(objCollection["todate"]) : "";

                DateTime? sqlFromDate = !string.IsNullOrEmpty(strFromDate) ? Convert.ToDateTime(strFromDate) : (DateTime?)null;
                DateTime? sqlToDate = !string.IsNullOrEmpty(strToDate) ? Convert.ToDateTime(strToDate) : (DateTime?)null;

                if(sqlToDate.HasValue)
                    sqlToDate = sqlToDate.Value.AddMinutes(1439);

                DataTable SPNameData = BLScript.GetModifyedSPNamesBetweenDates(sqlFromDate.Value, sqlToDate.Value);

                if (SPNameData != null && SPNameData.Rows.Count > 0)
                {
                    foreach (DataRow rowItem in SPNameData.Rows)
                    {
                        SPNames objSPName = new SPNames()
                        {
                            SPName = Convert.ToString(rowItem["name"]),
                            Create_Date = Convert.ToString(rowItem["create_date"]),
                            Modify_Date = Convert.ToString(rowItem["modify_date"])

                        };
                        lstAllSP.Add(objSPName);
                    }
                }

                ViewBag.FromDate = strFromDate;
                ViewBag.ToDate = strToDate;
            }
            catch (Exception ex)
            {
                AppConstant.WriteLogFile(ex.ToString());
            }

            return View(lstAllSP);
        }

        [HttpGet]
        public ActionResult ExportSPBetweenDates(string FromDate, string ToDate)
        {
            List<SPNames> lstAllSP = new List<SPNames>();
            MemoryStream memoryStream = new MemoryStream();
            string strExportFileName = "Script_" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".sql";
            StringBuilder sbSPText = new StringBuilder();

            try
            {
                DateTime? sqlFromDate = !string.IsNullOrEmpty(FromDate) ? Convert.ToDateTime(FromDate) : (DateTime?)null;
                DateTime? sqlToDate = !string.IsNullOrEmpty(ToDate) ? Convert.ToDateTime(ToDate) : (DateTime?)null;

                if (sqlToDate.HasValue)
                    sqlToDate = sqlToDate.Value.AddMinutes(1439);

                DataTable SPNameData = BLScript.GetModifyedSPNamesBetweenDates(sqlFromDate.Value, sqlToDate.Value);

                if (SPNameData != null && SPNameData.Rows.Count > 0)
                {
                    foreach (DataRow rowItem in SPNameData.Rows)
                    {
                        string strSPName = Convert.ToString(rowItem["name"]);

                        string strDopSP = @"IF EXISTS(SELECT 1 FROM sys.procedures WHERE Name = '" + strSPName + "') BEGIN DROP PROCEDURE [dbo].["+ strSPName + "] END";

                        sbSPText.Append(strDopSP);
                        sbSPText.Append("\n");
                        sbSPText.Append("GO");
                        sbSPText.Append("\n");

                        var data = BLScript.SPSchemaReader(strSPName);
                        DataSet ResDataSet = data;

                        if (ResDataSet.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow spSchemaItem in ResDataSet.Tables[0].Rows)
                            {
                                string strCurrentText = !string.IsNullOrEmpty(Convert.ToString(spSchemaItem["Text"])) ? Convert.ToString(spSchemaItem["Text"]) : "";
                                if (!string.IsNullOrEmpty(strCurrentText))
                                {
                                    sbSPText.Append(strCurrentText);
                                }
                            }
                        }

                        sbSPText.Append("\n");
                        sbSPText.Append("GO");
                        sbSPText.Append("\n");
                    }
                }

                
                TextWriter tw = new StreamWriter(memoryStream);

                tw.WriteLine(sbSPText.ToString());
                tw.Flush();
                tw.Close();

                
            }
            catch (Exception ex)
            {
                AppConstant.WriteLogFile(ex.ToString());
            }

            return File(memoryStream.GetBuffer(), "application/sql", strExportFileName);
        }
    }
}