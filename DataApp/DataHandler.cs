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
        public static System.Data.DataTable FlatToDataTable(string filepath, char delimiter = ',')
        {
            GenericParserAdapter parserAdapter = new GenericParserAdapter();
            parserAdapter.ColumnDelimiter = delimiter;
            parserAdapter.SetDataSource(filepath);
            parserAdapter.FirstRowHasHeader = true;
            System.Data.DataTable dt = parserAdapter.GetDataTable();
            return dt;

        }

        public static System.Data.DataTable FlatToDataTableFirst10(string filepath, char delimiter = ',')
        {
            GenericParserAdapter parserAdapter = new GenericParserAdapter();
            parserAdapter.ColumnDelimiter = delimiter;
            parserAdapter.SetDataSource(filepath);
            parserAdapter.FirstRowHasHeader = true;
            parserAdapter.MaxRows = 10;
            System.Data.DataTable dt = parserAdapter.GetDataTable();
            return dt;

        }

        public static void DataTableToFlatFile(System.Data.DataTable dt, string dialogfilename, string extention, string delimiter, string qualifier)
        {
            MessageBox.Show(extention);
            var lines = new List<string>();
            string[] colNames = dt.Columns.Cast<DataColumn>().Select(col => col.ColumnName).ToArray();
            

            if (extention == ".csv")
            {
                var header = string.Join(",", colNames.Select(name => $"\"{name}\""));
                lines.Add(header);

                var valuelines = dt.AsEnumerable().Select(row => string.Join(",", row.ItemArray.Select(val => $"\"{val}\"")));
                lines.AddRange(valuelines);

                File.WriteAllLines(dialogfilename, lines);
                MessageBox.Show(Path.GetExtension(dialogfilename));
            }
            else if(extention == ".txt")
            {
                var header = string.Join(delimiter, colNames.Select(name => name));
                lines.Add(header);

                var valuelines = dt.AsEnumerable().Select(row => string.Join(delimiter, row.ItemArray.Select(val => val.ToString())));
                lines.AddRange(valuelines);

                File.WriteAllLines(dialogfilename, lines);
            }
            else
            {
                MessageBox.Show($"No extention? {extention}");
            }
            
            
        }
    }
}
