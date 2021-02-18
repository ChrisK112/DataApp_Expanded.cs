using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using word = Microsoft.Office.Interop.Word;
using System.Data;

namespace DataApp
{
    class WordHandler
    {        
        public static void wordtest(DataTable dt)
        {
            word.Application wordapp = new word.Application();
            word.Document wordfile = new word.Document();

            wordfile = wordapp.Documents.Add(Template: @"C:\Users\Gregory\Downloads\test.docx");
            word.Selection selection = wordapp.Selection;
            
            for(int n = 1; n <= dt.Rows.Count; n++)
            {
                object breaktype = word.WdBreakType.wdPageBreak;
                selection.InsertBreak(ref breaktype);
                wordapp.Visible = true;
            }
        }
    }
}
