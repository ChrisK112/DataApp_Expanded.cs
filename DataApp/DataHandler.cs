using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Data;
using GenericParsing;
using System.Configuration;
using excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;

namespace DataApp
{
    class DataHandler
    {
        public static System.Data.DataTable FlatToDataTable(string filepath, char delimiter = ',', int maxrownumber = 0)
        {
            GenericParserAdapter parserAdapter = new GenericParserAdapter();
            parserAdapter.ColumnDelimiter = delimiter;
            parserAdapter.SkipEmptyRows = true;
            parserAdapter.SetDataSource(filepath);
            parserAdapter.FirstRowHasHeader = true;
            if (maxrownumber != 0)
            {
                parserAdapter.MaxRows = maxrownumber;
            }
            return parserAdapter.GetDataTable();

        }

        public static System.Linq.IOrderedEnumerable<System.Collections.Generic.KeyValuePair<string, string>> CharityNamesPairs()
        {
            Dictionary<string, string> CharityNamesDic = new Dictionary<string, string>();
            foreach (string key in ConfigurationManager.AppSettings)
            {
                CharityNamesDic.Add(key.ToString(), ConfigurationManager.AppSettings[key].ToString());
            }
            return CharityNamesDic.OrderBy(key => key.Value);
        }

        public static string[] columnNamesWithExtraEmptyRow(DataTable dt)
        {
            string[] colNames = dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();            
            return addEmptyElementToArray(colNames);
        }

        private static string[] addEmptyElementToArray(string[] list)
        {
            string[] colNamesWithBlank = new string[list.Count() + 1];
            colNamesWithBlank[0] = "                -";
            for (int n = 1; n < list.Count(); n++)
            {
                colNamesWithBlank[n] = list[n - 1];
            }
            return colNamesWithBlank;
        }

        public static void addOrDeleteEmptyColumn(ref DataTable dt)
        {
            bool exist = false;
            foreach(DataColumn col in dt.Columns)
            {
                if(col.ColumnName == "                -")
                {
                    exist = true;
                }
            }
            if (exist)
            {
                dt.Columns.Remove("                -");
            }
            else
            {
                dt.Columns.Add("                -");
            }
        }


        public static void DataTableToFlatFile(System.Data.DataTable dt, string dialogfilename, int extentionindex)
        {
            var lines = new List<string>();
            string[] colNames = dt.Columns.Cast<DataColumn>().Select(col => col.ColumnName).ToArray();

            switch(extentionindex)
            {
                //*.txt
                case 1:
                    var htxt = string.Join("|", colNames.Select(name => name));
                    lines.Add(htxt);

                    var valuelinestxt = dt.AsEnumerable().Select(row => string.Join("|", row.ItemArray.Select(val => val.ToString().Replace("\'","").Replace("|","").Replace("   "," ").Replace("  "," ").Replace("\"",""))));
                    lines.AddRange(valuelinestxt);

                    File.WriteAllLines(dialogfilename, lines);
                    break;

                //*.csv
                case 2:
                    var hcsv = string.Join(",", colNames.Select(name => $"\"{name}\""));
                    lines.Add(hcsv);

                    var valuelinescsv = dt.AsEnumerable().Select(row => string.Join(",", row.ItemArray.Select(val => $"\"{val.ToString().Replace("\'", "").Replace("|", "").Replace("   ", " ").Replace("  ", " ").Replace("\"", "")}\"")));
                    lines.AddRange(valuelinescsv);

                    File.WriteAllLines(dialogfilename, lines);
                    break;
            }      
        }
    }
}
