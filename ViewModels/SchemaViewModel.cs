using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SpScriptGenrator.ViewModels
{
    public class SchemaViewModel
    {
    }
    public class clsSPSchemaViewModel
    {
        public DataTable tblData { get; set; }
        public DataSet dsData { get; set; }

        public string strSPText { get; set; }
    }

    public class SPNames
    {
        public string SPName { get; set; }
        public string Create_Date { get; set; }
        public string Modify_Date { get; set; }
    }
}