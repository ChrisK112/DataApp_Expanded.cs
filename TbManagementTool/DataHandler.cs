using GenericParsing;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions;

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
        public static DataTable fileToDt(string fileName)
        {
            string fileExtention = Path.GetExtension(fileName);
            DataTable dt = new DataTable();

            if (fileExtention == ".csv" || fileExtention == ".txt")
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

        public static List<string> dtToListStr(DataTable dt, char delimiter, string qualifier = "")
        {
            //CONVERTS DATATABLE INTO A LIST<STRING>
            List<string> lines = new List<string>();
            string[] arrayColNames = dt.Columns.Cast<DataColumn>().Select(col => col.ColumnName).ToArray();

            string strColNames = string.Join(delimiter.ToString(), arrayColNames.Select(val => $"{qualifier}{val.ToString().Replace(qualifier == "" ? "*" : qualifier, "")}{qualifier}"));
            lines.Add(strColNames);

            EnumerableRowCollection<string> strData = dt.AsEnumerable().Select(row => string.Join(delimiter.ToString(), row.ItemArray.Select(val => $"{qualifier}{val.ToString()}{qualifier}")));
            lines.AddRange(strData);

            for(int n = 0; n < lines.Count - 1; n++)
            {
                lines[n] = replaceSpecialChar(lines[n]);
            }
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
            //Dynamically renames strings if already exist
            str = (str == "") ? "Data" : Path.GetFileNameWithoutExtension(str);

            return intAutoIncrement(ds, str, str, 0);
        }

        public static string intAutoIncrement(System.Data.DataSet ds, string currentStr, string OriginalStr, int count)
        {
            //Dynamically increase integer count if already exist
            if (ds.Tables.Contains(currentStr))
            {
                count++;
                currentStr = intAutoIncrement(ds, OriginalStr + $"({count})", OriginalStr, count);

            }
            return currentStr;
        }

        public static void comboBoxProcess(TabControl tabControl_Main, object dataSource, string Exception = "")
        {
            //Iterates through tabpages from main tabcontrol
            foreach (Control tab in tabControl_Main.TabPages)
            {
                TabPage tabPage = (TabPage)tab;
                //Selected TabPage
                if (tabControl_Main.SelectedTab.Name == tabPage.Name)
                {
                    //Groupboxes
                    foreach (Control group in tabPage.Controls)
                    {
                        //Group Items
                        foreach (Control item in group.Controls)
                        {
                            //Select comboBoxes only
                            if (item.GetType().Name == "ComboBox")
                            {
                                ComboBox comboBox = (ComboBox)item;

                                //If one comboBox has different datasource
                                if (Exception != "")
                                {
                                    if (item.Name == Exception)
                                    {
                                        dataSourceBinder(ref comboBox, dataSource);
                                    }
                                }
                                else
                                {
                                    dataSourceBinder(ref comboBox, dataSource);
                                }


                            }

                        }

                    }

                }
            }


        }
        private static void dataSourceBinder(ref ComboBox comboBox, object dataSource)
        {
            if (dataSource.GetType().Name == "String[]")
            {
                strArrayToComboBox(ref comboBox, dataSource);
                reSizeStrArrayComboBox(comboBox);
            }
            if (dataSource.GetType().Name == "OrderedEnumerable`2")
            {
                iOrderedEnmKeyValueToComboBox(ref comboBox, dataSource);
                reSizestrArrayComboBox(comboBox);
            }
        }
        private static void strArrayToComboBox(ref ComboBox comboBox, object dataSource)
        {
            //Binds datasource string[]
            comboBox.BindingContext = new BindingContext();
            comboBox.DataSource = new BindingSource(dataSource, null);
        }

        private static void iOrderedEnmKeyValueToComboBox(ref ComboBox comboBox, object dataSource)
        {
            //Binds datasource keyValuePairs
            comboBox.BindingContext = new BindingContext();
            comboBox.DataSource = new BindingSource(dataSource, null);
            comboBox.DisplayMember = "Value";
            comboBox.ValueMember = "Key";
        }

        private static void reSizeStrArrayComboBox(ComboBox comboBox)
        {
            //Resizes Combobox based on the longest item name to string[] datasource
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
            //Resizes Combobox regardless of the datasource
            comboBox.DropDownWidth = 300;
        }

        public static void addItemToListView(ListView lstView, string name, DataTable dt)
        {
            //Add new item to the listview
            ListViewItem item = new ListViewItem(name);
            item.SubItems.Add($"{dt.Rows.Count:N0}");
            lstView.Items.Add(item);
            lstView.CheckBoxes = true;
        }

        //private static Dictionary<string, string> specialCharLst()
        //{
        //    Dictionary<string, string> strDic = new Dictionary<string, string>()
        //    {
        //        //{"â‚¬","€" },
        //        //{"â€š","‚" },
        //        //{"Æ’","ƒ" },
        //        //{"â€ž","„" },
        //        //{"â€¦","…" },
        //        //{"â€","†" },
        //        //{"â€¡","‡" },
        //        //{"Ë†","ˆ" },
        //        //{"â€°","‰" },
        //        //{"Å","Š" },
        //        //{"â€¹","‹" },
        //        //{"Å’","Œ" },
        //        //{"Å½","Ž" },
        //        //{"â€˜","‘" },
        //        //{"â€™","’" },
        //        //{"â€œ","“" },
        //        //{"â€","”" },
        //        //{"â€¢","•" },
        //        //{"â€“","–" },
        //        //{"â€”","—" },
        //        //{"Ëœ","˜" },
        //        //{"â„¢","™" },
        //        //{"Å¡","š" },
        //        //{"â€º","›" },
        //        //{"Å“","œ" },
        //        {"izi","pizi" },
        //    };                

        //    return strDic;
        //}
        private static string[,] specialCharLst()
        {
            string[,]array2d = new string[,]
            {
                { "â‚¬","€" },
                { "â€š","‚" },
                { "Æ’","ƒ" },
                { "â€ž","„" },
                { "â€¦","…" },
                { "â€","†" },
                { "â€¡","‡" },
                { "Ë†","ˆ" },
                { "â€°","‰" },
                { "Å","Š" },
                { "â€¹","‹" },
                { "Å’","Œ" },
                { "Å½","Ž" },
                { "â€˜","‘" },
                { "â€™","’" },
                { "â€œ","“" },
                { "â€","”" },
                { "â€¢","•" },
                { "â€“","–" },
                { "â€”","—" },
                { "Ëœ","˜" },
                { "â„¢","™" },
                { "Å¡","š" },
                { "â€º","›" },
                { "Å“","œ" },
            };

            return array2d;
        }
        public static string replaceSpecialChar(string str)
        {
            string[,] array = specialCharLst();

            for (int n = 0; n< array.Length; n++)
            {
                if (str.Contains(array[n, 0]))
                {
                    str.Replace(array[n, 0], array[n, 1]);
                }
            }
            return str;
        }
        //public static string replaceSpecialChar(string str)
        //{
        //    string str_temp = "";
        //    foreach(var item in specialCharLst())
        //    {
        //        //if (str.Contains(item.Key))
        //        //{
        //        //    str.Replace(item.Key, item.Value);
        //        //}
        //        str_temp += item.Key;
        //    }
        //    MessageBox.Show(str_temp);
        //    return str;
        //}

    }
}
