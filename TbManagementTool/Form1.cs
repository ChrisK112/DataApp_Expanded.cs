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

namespace TbManagementTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //Global Variables
        string fileName;
        string fileExtention;
        DataSet ds_import = new DataSet();
        DataSet ds_export = new DataSet();
        DataTable dt_import = new DataTable();
        DataTable dt_export = new DataTable();
        OpenFileDialog fileSearch = new OpenFileDialog();

        private void button_DataMapper_FileSearch_Click(object sender, EventArgs e)
        {
            //Get file
            fileSearch.Multiselect = true;
            if (fileSearch.ShowDialog() == DialogResult.OK)
            {
                fileName = fileSearch.FileName;
                fileExtention = Path.GetExtension(fileName);
                textBox1_DataMapper_FileName.Text = fileName;
            }   
        }

        private void button_DataMapper_Import_Click(object sender, EventArgs e)
        {
            try
            {                
                if (File.Exists(fileName))
                {
                    if (fileExtention == ".xls" || fileExtention == ".xlsx" || fileExtention == ".csv" || fileExtention == ".txt")
                    {
                        dt_import = DataHandler.fileToDt(fileName);
                        string[] colNames_import = DataHandler.colNamesArray(dt_import, true);
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
                                //comboBox1_CG_duplicates.DataSource = DataHandler.colNamesArray(DataTableFactory.DtScheme(tabPage.Name), true);
                            }
                        }

                        //Assigns how many files have been imported to the ListView
                        foreach (string fileName in fileSearch.FileNames)
                        {                            
                            ListViewItem item = new ListViewItem(Path.GetFileName(fileName));
                            item.SubItems.Add(dt_import.Rows.Count.ToString());
                            listView_DataMapper.Items.Add(item);
                        }
                    }
                    else
                    {
                        MessageBox.Show("this file is not supported");
                    }
                    
                }
                else
                {
                    MessageBox.Show("Please select a file");
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
            try
            {
                dt_export = DataTableFactory.DtCgSpec();

                if (dt_export != null)
                {
                    //Loop through each column
                    for(int col= 0; col < dt_import.Columns.Count; col++)
                    {
                        //loop through each row
                        for (int row = 0; row < dt_import.Rows.Count; row++)
                        {
                            


                        }
                    }
                }
                else
                {
                    MessageBox.Show("There is no data to load");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
            }
        }
    }
}
