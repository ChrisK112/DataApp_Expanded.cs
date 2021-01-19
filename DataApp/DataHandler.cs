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

        public static System.Data.DataTable ExcelToDataTable(string worksheetName, string saveAsLocation, string reportType, int headerLine, int columnStart)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            Microsoft.Office.Interop.Excel.Application excel;
            Microsoft.Office.Interop.Excel.Workbook excelworkBook;
            Microsoft.Office.Interop.Excel.Worksheet excelSheet;
            Microsoft.Office.Interop.Excel.Range range;

            try
            {
                // Get Application object.
                excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Visible = false;
                excel.DisplayAlerts = false;
                // Creation a new Workbook
                excelworkBook = excel.Workbooks.Open(saveAsLocation);
                // Workk sheet
                excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)
                excelworkBook.Worksheets.Item[worksheetName];
                range = excelSheet.UsedRange;
                int cl = range.Columns.Count;
                // loop through each row and add values to our sheet
                int rowcount = range.Rows.Count; ;
                //create the header of table
                for (int j = columnStart; j <= cl; j++)
                {
                    dt.Columns.Add(Convert.ToString
                                         (range.Cells[headerLine, j].Value2), typeof(string));
                }
                //filling the table from  excel file                
                for (int i = headerLine + 1; i <= rowcount; i++)
                {
                    DataRow dr = dt.NewRow();
                    for (int j = columnStart; j <= cl; j++)
                    {

                        dr[j - columnStart] = Convert.ToString(range.Cells[i, j].Value2);
                    }

                    dt.Rows.InsertAt(dr, dt.Rows.Count + 1);
                }

                //now close the workbook and make the function return the data table

                excelworkBook.Close();
                excel.Quit();
                return dt;
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
        }

        public static void DataTableToExcel(System.Data.DataTable dt, string worksheetName, string saveAsLocation)
        {
            Microsoft.Office.Interop.Excel.Application excel;
            Microsoft.Office.Interop.Excel.Workbook excelworkBook;
            Microsoft.Office.Interop.Excel.Worksheet excelSheet;
            Microsoft.Office.Interop.Excel.Range excelCellrange;

            try
            {
                //  get Application object.
                excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Visible = false;
                excel.DisplayAlerts = false;

                // Creation a new Workbook
                excelworkBook = excel.Workbooks.Add(Type.Missing);

                // Workk sheet
                excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelworkBook.ActiveSheet;
                excelSheet.Name = worksheetName;

                // loop through each row and add values to our sheet
                int rowcount = 1;

                foreach (DataRow datarow in dt.Rows)
                {
                    rowcount += 1;
                    for (int i = 1; i <= dt.Columns.Count; i++)
                    {
                        // on the first iteration we add the column headers
                        if (rowcount == 3)
                        {
                            excelSheet.Cells[2, i] = dt.Columns[i - 1].ColumnName;
                        }
                        // Filling the excel file 
                        excelSheet.Cells[rowcount, i] = datarow[i - 1].ToString();
                    }
                }

                //now save the workbook and exit Excel
                excelworkBook.SaveAs(saveAsLocation); ;
                excelworkBook.Close();
                excel.Quit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                excelSheet = null;
                excelCellrange = null;
                excelworkBook = null;
            }
        }
    }
}
