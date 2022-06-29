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
        DataSet dataset = new DataSet();
        OpenFileDialog fileSearch = new OpenFileDialog();
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
                            string dtName = DataHandler.StrRenamingFromDsTableName(dataset, file); 
                            DataTable dt = DataHandler.fileToDt(file);
                           
                            dt.TableName = dtName;
                            dataset.Tables.Add(dt);

                            //Add new item to the listview
                            DataHandler.addItemToListView(listView_DataMapper, dtName, dt);

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
                        string[] colNames_import = DataHandler.colNamesArray(dataset.Tables[lstItem.Text], true);
                        IOrderedEnumerable<KeyValuePair<string, string>> charityNames = DataHandler.CharityNamesPairs();
                        DataHandler.comboBoxProcess(tabControl_Main, colNames_import);
                        DataHandler.comboBoxProcess(tabControl_Main, charityNames, comboBox_DataMapper_ClientName.Name);

                    }

                }

                
            }
            else
            {
                MessageBox.Show("You have to select 1 datafile");
            }

            //Default settings
            DataHandler.clearListViewCheckedBoxes(ref listView_DataMapper);
        }

        private void button_DataMapper_FileDelete_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lstItem in listView_DataMapper.Items)
            {
                //Delete selected datatables and listviewitems
                if (lstItem.Checked)
                {
                    listView_DataMapper.Items.Remove(lstItem);
                    dataset.Tables.Remove(lstItem.Text);
                }
            }
        }

        private void button_DataMapper_FileMerge_Click(object sender, EventArgs e)
        {
            //Check how many files are checked
            lstItemsCheckedCount = listView_DataMapper.Items.OfType<ListViewItem>().Where(x => x.Checked).Count();

            //Works only if 2 or more files
            if (lstItemsCheckedCount > 1)
            {
                DataTable dt = new DataTable();
                string dtName = DataHandler.StrRenamingFromDsTableName(dataset);

                foreach (ListViewItem lstItem in listView_DataMapper.Items)
                {
                    //Merges all selected datatables into one new one
                    if (lstItem.Checked)
                    {
                        dt.Merge(dataset.Tables[lstItem.Text]);
                    }
                }

                //Adds new datatable
                dt.TableName = dtName;
                dataset.Tables.Add(dt);

                //Add new item to the listview
                DataHandler.addItemToListView(listView_DataMapper, dtName, dt);
            }
            else
            {
                MessageBox.Show("You need to select 2 or more datafiles");
            }
            

            //Default settings
            DataHandler.clearListViewCheckedBoxes(ref listView_DataMapper);
        }        

        private void button_DataMapper_FileCreate_Click(object sender, EventArgs e)
        {
            DataTable dt = DataTableFactory.DtCgSpec();


        }        

        private void button_DataMapper_FileClear_Click(object sender, EventArgs e)
        {
            
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
                        DataHandler.DtToFlat(dataset.Tables[lstItem.Text], fileDestination.FileName);
                    }
                }
            }

            //Default settings
            DataHandler.clearListViewCheckedBoxes(ref listView_DataMapper);

        }
    }
}
