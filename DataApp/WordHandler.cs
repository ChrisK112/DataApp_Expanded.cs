using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using word = Microsoft.Office.Interop.Word;

namespace DataApp
{
    class WordHandler
    {
        public static void wordtest(DataTable dt)
        {
            word.Application wordapp = new word.Application();
            string path = @"C:\Users\Gregory\Desktop\test.docx";
            word.Document worddoc = wordapp.Documents.Add(path);
            wordapp.Visible = true;
            word.Selection selection = wordapp.Selection;

            foreach(word.Field mergefield in worddoc.Fields)
            {
                if(mergefield.Code.Text.Contains("MERGEFIELD ENVELOPESALUTATION"))
                {
                    mergefield.Select();
                    wordapp.Selection.TypeText("izi pizi nub");
                }
            }
            word.Range range = worddoc.Content;
            range.Copy();
            worddoc.Words.Last.InsertBreak(word.WdBreakType.wdPageBreak);
            
        }
    }
}
