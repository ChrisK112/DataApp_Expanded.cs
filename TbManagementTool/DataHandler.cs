using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericParsing;
using TbManagementTool;
using System.Configuration;
using System.Windows.Forms;
using System.Data;
using System.IO;

namespace TbManagementTool
{
    class DataHandler
    {
        public static DataTable excelToDt(string fileName)
        {
            DataTable dt = new DataTable();
            return dt;
        }

        public static DataTable flatToDt(string fileName)
        {
            string fileExtention = Path.GetExtension(fileName);
            DataTable dt = new DataTable();

            GenericParserAdapter parser = new GenericParserAdapter();
            parser.SetDataSource(fileName);
            if (Path.GetExtension(fileName) == ".txt")
            {
                parser.ColumnDelimiter = '|';

            }
            if (Path.GetExtension(fileName) == ".csv")
            {
                parser.ColumnDelimiter = ',';

            }
            parser.SkipEmptyRows = true;
            parser.FirstRowHasHeader = true;
            dt = parser.GetDataTable();
            parser.Close();
            return dt;
        }
        public static DataTable fileToDt(string fileName)
        {
            string fileExtention = Path.GetExtension(fileName);
            DataTable dt = new DataTable();

            if(fileExtention == ".csv" || fileExtention == ".txt")
            {
                dt = flatToDt(fileName);
            }

            if (fileExtention == ".xls" || fileExtention == ".xlsx")
            {
                dt = excelToDt(fileName);
            }
            return dt;

        }


        public static System.Linq.IOrderedEnumerable<System.Collections.Generic.KeyValuePair<string, string>> CharityNamesPairs()
        {
            //CREATES KEYVALUE PAIRS FROM APPSETTINGS
            Dictionary<string, string> CharityNamesDic = new Dictionary<string, string>();
            foreach (string key in ConfigurationManager.AppSettings)
            {
                CharityNamesDic.Add(key.ToString(), ConfigurationManager.AppSettings[key].ToString());
            }
            return CharityNamesDic.OrderBy(key => key.Value);
        }

        public static string[] colNamesArray(DataTable dt, bool emptyRow = false)
        {
            //CREATES ARRAY CONTAINING COLUMN NAMES
            string[] colNames = dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();
            return emptyRow ? addEmptyElementToArray(colNames) : colNames;
        }

        private static string[] addEmptyElementToArray(string[] list)
        {
            //ADDS AN EMPTY VALUE TO ARRAY
            string[] colNamesWithBlank = new string[list.Count() + 1];
            colNamesWithBlank[0] = "";
            for (int n = 1; n <= list.Count(); n++)
            {
                colNamesWithBlank[n] = list[n - 1];
            }
            return colNamesWithBlank;
        }

        public static void DtToFlat(System.Data.DataTable dt, string dialogfilename, int extentionindex, string qualifier = "", char delimiter = ',')
        {
            //CONVERTS DATATABLE TO FLAT FILE
            if (extentionindex == 1)
            {
                File.WriteAllLines(dialogfilename, dtToListStr(dt, delimiter, qualifier));
            }
            else if (extentionindex == 2)
            {
                File.WriteAllLines(dialogfilename, dtToListStr(dt, ',', "\""));
            }

        }

        public static List<string> dtToListStr(DataTable dt, char delimiter, string qualifier)
        {
            //CONVERTS DATATABLE INTO A LIST<STRING>
            List<string> lines = new List<string>();
            string[] arrayColNames = dt.Columns.Cast<DataColumn>().Select(col => col.ColumnName).ToArray();

            string strColNames = string.Join(delimiter.ToString(), arrayColNames.Select(val => $"{qualifier}{val.ToString().Replace(qualifier == "" ? "*" : qualifier, "")}{qualifier}"));
            lines.Add(strColNames);

            EnumerableRowCollection<string> strData = dt.AsEnumerable().Select(row => string.Join(delimiter.ToString(), row.ItemArray.Select(val => $"{qualifier}{val.ToString()}{qualifier}")));
            lines.AddRange(strData);

            return lines;
        }

        public static void dtRemoveDuplicateRows(ref DataTable dt, string colName)
        {
            //REMOVE DUPLICATE ROWS BASED ON 1 COLUMN
            dt = dt.AsEnumerable().GroupBy(x => x.Field<string>(colName)).Select(y => y.First()).CopyToDataTable();
        }

        public static List<string> lstStrData(bool dataloaded, DataTable dt1, DataTable dt2, char delimiter, string qualifier)
        {
            //DECIDES WHAT DATATABLE TO USE DEPENDING ON BOOLEAN AND RETURNS IT AS A LIST
            if (dataloaded)
            {
                return DataHandler.dtToListStr(dt1, delimiter, qualifier);
            }
            else
            {
                return DataHandler.dtToListStr(dt2, delimiter, qualifier);
            }
        }

    }
}
