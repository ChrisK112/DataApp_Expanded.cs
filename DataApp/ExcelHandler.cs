using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using _Excel = Microsoft.Office.Interop.Excel;
using System.Data;

namespace DataApp
{
    class ExcelHandler
    {
        public Application ExcelApp;
        public Workbook ExcelWorkbook;
        public Worksheet ExcelWorkSheet;
        public Range ExcelRange;
        public int colcount;
        public int rowcount;

        public void ExcelOpen(string path, int sheet)
        {
            ExcelApp = new Application();
            ExcelWorkbook = ExcelApp.Workbooks.Open(path);
            ExcelWorkSheet = ExcelWorkbook.Worksheets[sheet];
            ExcelRange = ExcelWorkSheet.UsedRange;
            colcount = ExcelRange.Columns.Count;
            rowcount = ExcelRange.Rows.Count;
        }

        public void ExcelClose()
        {
            ExcelWorkbook.Close();
            ExcelApp.Quit();
        }

        public string[] RowToArray(int rownumber)
        {
            string[] RowArray = new string[colcount+1];
            int col;

            for(col = 0; col <= colcount; col++)
            {
                if(ExcelWorkSheet.Cells[rownumber, col + 1].value == null)
                {
                    RowArray[col] ="";
                }
                else
                {
                    RowArray[col] = ExcelWorkSheet.Cells[rownumber, col + 1].value.ToString();
                }            
            }
            return RowArray;
        }
    }
}
