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
using excel = Microsoft.Office.Interop.Excel;

namespace TbManagementTool
{
    class DataHandler
    {
        public static DataTable excelToDt(string fileName)
        {
            excel.Application excel;
            excel.Workbook excelworkBook;
            excel.Worksheet excelSheet;
            excel.Range range;
            DataTable dt = new DataTable();

            try
            {
                // Get Application object.
                excel = new excel.Application();
                excel.Visible = false;
                excel.DisplayAlerts = false;
                // Creation a new Workbook
                excelworkBook = excel.Workbooks.Open(fileName);
                // Work sheet
                excelSheet = excelworkBook.Sheets[1];
                range = excelSheet.UsedRange;
                int cl = range.Columns.Count;
                // loop through each row and add values to our sheet
                int rowcount = range.Rows.Count; ;
                //create the header of table
                for (int n = 1; n <= cl; n++)
                {
                    dt.Columns.Add(Convert.ToString(range.Cells[1, n].Value2), typeof(string));
                }
                //filling the table from  excel file                
                for (int i = 1 + 1; i <= rowcount; i++)
                {
                    DataRow dr = dt.NewRow();
                    for (int n = 1; n <= cl; n++)
                    {

                        dr[n - 1] = Convert.ToString(range.Cells[i, n].Value2);
                    }

                    dt.Rows.InsertAt(dr, dt.Rows.Count + 1);
                }

                //now close the workbook and make the function return the data table

                excelworkBook.Close();
                excel.Quit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            finally
            {
                excelSheet = null;
                range = null;
                excelworkBook = null;
            }
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
