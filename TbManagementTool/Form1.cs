using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace TbManagementTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //Global Variables
        DataSet ds_import = new DataSet();
        DataSet ds_export = new DataSet();
        OpenFileDialog fileSearch = new OpenFileDialog();
        int filesSelected = 0;
        int lstItemsCheckedCount = 0;

        private void button_DataMapper_FileSearch_Click(object sender, EventArgs e)
        {
            //Get file
            fileSearch.Multiselect = true;
            if (fileSearch.ShowDialog() == DialogResult.OK)
            {
                textBox1_DataMapper_FileName.Text = fileSearch.FileName;
            }   
        }

        private void button_DataMapper_Import_Click(object sender, EventArgs e)
        {
            try
            {                           

                foreach (string file in fileSearch.FileNames)
                {
                    if (File.Exists(file))
                    {
                        string fileExtention = Path.GetExtension(file);

                        if (fileExtention == ".csv" || fileExtention == ".txt")
                        {
                            string fileName = Path.GetFileName(file);
                            DataTable dt = DataHandler.fileToDt(file);

                            //Renames each datatable to be added to Dataset
                            if (ds_import.Tables.Contains(fileName))
                            {
                                fileName += " - Copy";

                                if (ds_import.Tables.Contains(fileName))
                                {
                                    fileName += " (2)";

                                    if (ds_import.Tables.Contains(fileName))
                                    {
                                        foreach (DataTable dt_temp in ds_import.Tables)
                                        {
                                            if (dt_temp.TableName == fileName)
                                            {
                                                Regex regex = new Regex(@"\d+");
                                                Match match = regex.Match(fileName);
                                                if (match.Success)
                                                {
                                                    fileName = $"{Path.GetFileName(file)} - Copy ({Int16.Parse(match.Value) + 1})";

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            //Creates datatables for each file                            
                            dt.TableName = fileName;
                            ds_import.Tables.Add(dt);

                            //Assigns how many files have been imported to the ListView
                            ListViewItem item = new ListViewItem(fileName);
                            item.SubItems.Add($"{dt.Rows.Count:N0}");
                            listView_DataMapper.Items.Add(item);
                            listView_DataMapper.CheckBoxes = true;

                        }
                        else
                        {
                            MessageBox.Show("One or more files are not supported");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please select a file");
                    }

                }                
            }
            catch (System.InvalidOperationException)
            {
                MessageBox.Show("The file you try to import does not contain any rows");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"There is an issue with your file - {ex}");
            }
            
        }

        private void button_DataMapper_FileLoad_Click(object sender, EventArgs e)
        {
            //Check how many files are checked
            lstItemsCheckedCount = listView_DataMapper.Items.OfType<ListViewItem>().Where(x => x.Checked).Count();

            //Works only if one file is checked
            if (lstItemsCheckedCount == 1)
            {
                foreach (ListViewItem lstItem in listView_DataMapper.Items)
                {
                    if (lstItem.Checked)
                    {
                        string[] colNames_import = DataHandler.colNamesArray(ds_import.Tables[lstItem.Text], true);
                        IOrderedEnumerable<KeyValuePair<string, string>> charityNames = DataHandler.CharityNamesPairs();

                        //Main TabControl
                        foreach (Control tab in tabControl_Main.TabPages)
                        {
                            TabPage tabPage = (TabPage)tab;

                            //Data Mapper
                            if (tabPage.Name == "tabPage_DataMapper")
                            {
                                foreach (Control group in tabPage.Controls)
                                {
                                    foreach (Control item in group.Controls)
                                    {
                                        if (item.GetType().Name == "ComboBox")
                                        {
                                            ComboBox comboBox = (ComboBox)item;
                                            Graphics comboBoxGraphic = comboBox.CreateGraphics();
                                            float lSize = 0;

                                            //Assigns specific items to a specific comboBox
                                            if (item.Name == "comboBox_DataMapper_ClientName")
                                            {
                                                comboBox.BindingContext = new BindingContext();
                                                comboBox.DataSource = new BindingSource(charityNames, null);
                                                comboBox.DisplayMember = "Value";
                                                comboBox.ValueMember = "Key";

                                                //Resize the DropDown based on the longest item's name
                                                foreach (var key in charityNames)
                                                {
                                                    SizeF textSize = comboBoxGraphic.MeasureString(key.Value.ToString(), comboBox.Font);
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
                                            //Assigns items to the rest of comboBoxes
                                            else
                                            {
                                                comboBox.BindingContext = new BindingContext();
                                                comboBox.DataSource = new BindingSource(colNames_import, null);

                                                //Resize the DropDown based on the longest item's name
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

                                        }
                                    }
                                }
                            }
                        }
                    }

                }

                
            }
            else
            {
                MessageBox.Show("You have to select 1 datafile");
            }

            //Default settings
            DataHandler.unCheckListView(ref listView_DataMapper);
        }

        private void button_DataMapper_FileDelete_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lstItem in listView_DataMapper.Items)
            {
                //Delete selected datatables and listviewitems
                if (lstItem.Checked)
                {
                    listView_DataMapper.Items.Remove(lstItem);
                    ds_import.Tables.Remove(lstItem.Text);
                }
            }
        }

        private void button_DataMapper_FileMerge_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string dtName = "";

            foreach (ListViewItem lstItem in listView_DataMapper.Items)
            {
                //Merges all selected datatables into one new one
                if (lstItem.Checked)
                {
                    dt.Merge(ds_import.Tables[lstItem.Text]);
                    dtName = "Merged_Files";
                }                
            }

            //Renames each datatable to be added to Dataset
            if (ds_import.Tables.Contains(dtName))
            {
                dtName = "Merged_Files - Copy";

                if (ds_import.Tables.Contains(dtName))
                {
                    dtName = "Merged_Files - Copy (2)";

                    if (ds_import.Tables.Contains(dtName))
                    {
                        foreach(DataTable dt_temp in ds_import.Tables)
                        {
                            if(dt_temp.TableName == dtName)
                            {
                                Regex regex = new Regex(@"\d+");
                                Match match = regex.Match(dtName);
                                if (match.Success)
                                {
                                    dtName = $"Merged_Files - Copy ({Int16.Parse(match.Value) + 1})";
                                }
                            }
                        }

                    }
                }
            }

            //Adds new datatable
            dt.TableName = dtName;
            ds_import.Tables.Add(dt);

            //Add new item to the listview
            ListViewItem item = new ListViewItem(dtName);
            item.SubItems.Add($"{dt.Rows.Count:N0}");
            listView_DataMapper.Items.Add(item);
            listView_DataMapper.CheckBoxes = true;

            //Default settings
            DataHandler.unCheckListView(ref listView_DataMapper);
        }

        private void button_DataMapper_FileSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDestination = new SaveFileDialog();
            fileDestination.Filter = "Text(Tab delimited) (*.txt) |*.txt| CSV (Comma delimited) (*.csv) |*.csv";
            fileDestination.InitialDirectory = Path.GetDirectoryName(fileSearch.FileName);
            if (fileDestination.ShowDialog() == DialogResult.OK)
            {
                foreach (ListViewItem lstItem in listView_DataMapper.Items)
                {
                    //Saves selected
                    if (lstItem.Checked)
                    {
                        DataHandler.DtToFlat(ds_import.Tables[lstItem.Text], fileDestination.FileName);
                    }
                }
            }

            //Default settings
            DataHandler.unCheckListView(ref listView_DataMapper);

        }
    }
}
