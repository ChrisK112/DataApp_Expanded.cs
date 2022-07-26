﻿using GenericParsing;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System;

namespace TbManagementTool
{
    class DataHandler
    {
        public static DataTable flatToDt(string fileName)
        {
            //READS FLAT FILES INTO DATATABLE
            string fileExtention = Path.GetExtension(fileName);
            DataTable dt = new DataTable();

            GenericParserAdapter parser = new GenericParserAdapter(fileName);
            parser.SetDataSource(fileName, Encoding.UTF8);

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

        public static void DtToFlat(System.Data.DataTable dt, string dialogfilename)
        {
            //CONVERTS DATATABLE TO FLAT FILE
            string extention = Path.GetExtension(dialogfilename);
            if (extention == ".csv")
            {
                File.WriteAllLines(dialogfilename, dtToListStr(dt, ',', "\""));
            }
            if (extention == ".txt")
            {
                File.WriteAllLines(dialogfilename, dtToListStr(dt, '|'));
            }

        }

        public static List<string> dtToListStr(DataTable dt, char delimiter, string qualifier="")
        {
            //CONVERTS DATATABLE INTO A LIST<STRING>
            List<string> lines = new List<string>();
            string[] arrayColNames = dt.Columns.Cast<DataColumn>().Select(col => col.ColumnName).ToArray();

            string strColNames = string.Join(delimiter.ToString(), arrayColNames.Select(val => $"{qualifier}{val.ToString().Replace(qualifier,"")}{qualifier}"));
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

        public static void clearListViewCheckedBoxes(ref ListView lstView, string dataInUse)
        {
            //uncheck items from ListView
            foreach (ListViewItem item in lstView.Items)
            {
                if (item.Name != dataInUse)
                {
                    item.Checked = false;
                }
            }
        }

        public static string StrRenamingFromDsTableName(System.Data.DataSet ds, string str = "")
        {
            //DYNAMICALLY RENAMES STRING IF ALREADY EXIST
            str = (str == "") ? $"Data_{DateTime.Now.ToString("ddMMyyyy")}" : Path.GetFileNameWithoutExtension(str);

            return intAutoIncrement(ds, str, str, 0);
        }

        public static string intAutoIncrement(System.Data.DataSet ds, string currentStr, string OriginalStr, int count)
        {
            //DYNAMICALLY INCREASE INTEGER COUNT IF ALREADY EXIST
            if (ds.Tables.Contains(currentStr))
            {
                count++;
                currentStr = intAutoIncrement(ds, OriginalStr + $"({count})", OriginalStr, count);

            }
            return currentStr;
        }


        public static void dataSourceBinder(ComboBox comboBox, object dataSource)
        {
            //ASSIGNS DATASOURCE BASED ON THE DATASOURCE TYPE
            if (dataSource.GetType().Name == "String[]")
            {
                strArrayToComboBox(comboBox, dataSource);
                reSizeStrArrayComboBox(comboBox);
            }
            if (dataSource.GetType().Name == "OrderedEnumerable`2")
            {
                iOrderedEnmKeyValueToComboBox(comboBox, dataSource);
                reSizestrArrayComboBox(comboBox);
            }
        }
        private static void strArrayToComboBox(ComboBox comboBox, object dataSource)
        {
            //BINDS DATASOURCE string[]
            comboBox.BindingContext = new BindingContext();
            comboBox.DataSource = new BindingSource(dataSource, null);
        }

        private static void iOrderedEnmKeyValueToComboBox(ComboBox comboBox, object dataSource)
        {
            //BINDS DATASOURCE keyValuePairs{}
            comboBox.BindingContext = new BindingContext();
            comboBox.DataSource = new BindingSource(dataSource, null);
            comboBox.DisplayMember = "Value";
            comboBox.ValueMember = "Key";
        }

        private static void reSizeStrArrayComboBox(ComboBox comboBox)
        {
            //RESIZES COMBOBOX BASED ON THE LONGEST ITEM FOR  string[] DATASOURCE
            float lSize = 0;
            Graphics comboBoxGraphic = comboBox.CreateGraphics();
            for (int n = 0; n < comboBox.Items.Count; n++)
            {
                SizeF textSize = comboBoxGraphic.MeasureString(comboBox.Items[n].ToString(), comboBox.Font);
                if (textSize.Width > lSize)
                {
                    lSize = textSize.Width;
                }
                if (lSize > 0)
                {
                    comboBox.DropDownWidth = (int)lSize + 30;
                }
            }
        }
        private static void reSizestrArrayComboBox(ComboBox comboBox)
        {
            //RESIZES COMBOBOX REGARDLESS OF THE DATASOURCE
            comboBox.DropDownWidth = 300;
        }

        public static void addItemToListView(ListView lstView, string name, DataTable dt)
        {
            //ADDS NEW ITEM TO THE LISTVIEW
            ListViewItem item = new ListViewItem(name);
            item.SubItems.Add($"{dt.Rows.Count:N0}");
            lstView.Items.Add(item);
            lstView.CheckBoxes = true;
        }

        public static IEnumerable<Control> ienumControlList(Control control, System.Type type)
        {
            //ENLIST ALL CONTROL OF TYPE WITHIN CONTROL
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => ienumControlList(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type);
        }

        public static bool allColumnsExist(DataTable dt_import, DataTable dt_export)
        {
            //CHECKS IF ALL COLUMNS FROM ONE DT_IMPORT EXIST IN ANOTHER DT_EXPORT
            bool allExist = true;
            foreach(DataColumn column_import in dt_import.Columns)
            {
                if (!dt_export.Columns.Contains(column_import.ColumnName))
                {
                    allExist = false;
                }
            }
            return allExist;
        }

    }
}
