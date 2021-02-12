using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Windows.Forms;
using GenericParsing;
using Microsoft.Office.Interop.Excel;

namespace DataApp
{
    class DataHandler
    {
        public static System.Data.DataTable FlatToDataTable(string filepath, char delimiter = ',', int maxrownumber = 0)
        {
            GenericParserAdapter parserAdapter = new GenericParserAdapter();
            parserAdapter.ColumnDelimiter = delimiter;
            parserAdapter.SetDataSource(filepath);
            parserAdapter.FirstRowHasHeader = true;
            if( maxrownumber != 0)
            {
                parserAdapter.MaxRows = maxrownumber;
            }
            System.Data.DataTable dt = parserAdapter.GetDataTable();
            return dt;

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

                    File.WriteAllLines(dialogfilename + ".txt", lines);
                    break;

                //*.csv
                case 2:
                    var hcsv = string.Join(",", colNames.Select(name => $"\"{name}\""));
                    lines.Add(hcsv);

                    var valuelinescsv = dt.AsEnumerable().Select(row => string.Join(",", row.ItemArray.Select(val => $"\"{val.ToString().Replace("\'", "").Replace("|", "").Replace("   ", " ").Replace("  ", " ").Replace("\"", "")}\"")));
                    lines.AddRange(valuelinescsv);

                    File.WriteAllLines(dialogfilename + ".csv", lines);
                    break;
            }      
        }
    }
}
