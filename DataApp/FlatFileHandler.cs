using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Windows.Forms;
using GenericParsing;

namespace DataApp
{
    class FlatFileHandler
    {
        public static DataTable ToDataTable(string filepath, char delimiter = ',')
        {
            GenericParserAdapter parserAdapter = new GenericParserAdapter();
            parserAdapter.ColumnDelimiter = delimiter;
            parserAdapter.SetDataSource(filepath);
            parserAdapter.FirstRowHasHeader = true;
            DataTable dt = parserAdapter.GetDataTable();
            return dt;

        }

        public static DataTable ToDataTableFirst10(string filepath, char delimiter = ',')
        {
            GenericParserAdapter parserAdapter = new GenericParserAdapter();
            parserAdapter.ColumnDelimiter = delimiter;
            parserAdapter.SetDataSource(filepath);
            parserAdapter.FirstRowHasHeader = true;
            parserAdapter.MaxRows = 10;
            DataTable dt = parserAdapter.GetDataTable();
            return dt;

        }
    }
}
