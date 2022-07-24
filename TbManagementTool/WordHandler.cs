using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using word = Microsoft.Office.Interop.Word;
using System.Data;

namespace TbManagementTool
{
    class WordHandler
    {
        public static void dtToWord(System.Data.DataTable dt)
        {
            //Object oMissing = System.Reflection.Missing.Value;

            //Object oTemplatePath = "C:\\Users\\Gregory\\Desktop\\WORD_TEST.docx";
            //string datasource = "C:\\Users\\Gregory\\Desktop\\SMALL DATA3.csv";
            //List<word.MailMergeDataField> columns = new List<word.MailMergeDataField>();

            //DataTable table = new DataTable("Test");
            //table.Columns.Add("CustomerName");
            //table.Columns.Add("Address");
            //table.Rows.Add(new object[] { "Thomas Hardy", "120 Hanover Sq., London" });
            //table.Rows.Add(new object[] { "Paolo Accorti", "Via Monte Bianco 34, Torino" });

            //word.Application wordApp = new word.Application();
            //word.Document wordDoc = wordApp.Documents.Add(ref oTemplatePath, ref oMissing, ref oMissing, ref oMissing);
            //word.MailMerge merge = wordApp.m;

            

            

            //wordDoc.MailMerge.Execute(table);
            //////foreach(System.Data.DataRow row in dt.Rows)
            //////{
            //////    object startPoint = 0;
            //////    object missing = System.Type.Missing;
            //////    word.Range range = wordDoc.Range(ref startPoint, ref missing);
            //////    range.Collapse(word.WdCollapseDir‌​ection.wdCollapseEnd‌​);
            //////    range.InsertBreak(word.WdBreakTyp‌​e.wdSectionBreakNext‌​Page);
            //////    range.InsertFile("C:\\Users\\Gregory\\Desktop\\WORD_TEST.docx");
            //////}
            //wordDoc.SaveAs("C:\\Users\\Gregory\\Desktop\\WORD_TEST_3.docx");
            //////wordApp.Documents.Open("myFile.doc");
            //wordApp.Application.Quit();


        }

    }

}
