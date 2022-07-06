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
        string dataInUse = "";

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
                DataHandler.replaceSpecialChar("hola");
                foreach (string file in fileSearch.FileNames)
                {
                    if (File.Exists(file))
                    {
                        string fileExtention = Path.GetExtension(file);

                        if (fileExtention == ".csv" || fileExtention == ".txt")
                        {
                            string dtName = DataHandler.StrRenamingFromDsTableName(dataset, file); 
                            DataTable dt = DataHandler.fileToDt(file);
                            MessageBox.Show(dtName);
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
                        //Update what data is being loaded
                        dataInUse = lstItem.Text;
                        MessageBox.Show(dataInUse);
                        
                        //Get datasources
                        string[] colNames_import = DataHandler.colNamesArray(dataset.Tables[lstItem.Text], true);
                        IOrderedEnumerable<KeyValuePair<string, string>> charityNames = DataHandler.CharityNamesPairs();

                        //Bind data sources to ComboBox
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
            DataHandler.clearListViewCheckedBoxes(ref listView_DataMapper, dataInUse);
            textBox3_DataMapper_Primkey.Text = DateTime.Now.ToString("ddMMyyyy");
            textBox_DataMapper_AddedDateTime.Text = DateTime.Now.ToString("dd/MM/yyyy");
            textBox_DataMapper_AddedBy.Text = "Admin";
        }

        private void button_DataMapper_FileDelete_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lstItem in listView_DataMapper.Items)
            {
                //Delete selected datatables and listviewitems
                if (lstItem.Checked)
                {
                    //Check if it has not been loaded
                    if(lstItem.Name != dataInUse)
                    {
                        listView_DataMapper.Items.Remove(lstItem);
                        dataset.Tables.Remove(lstItem.Text);
                    }
                    else
                    {
                        MessageBox.Show("The data is currently being used\nPlease clear the data loaded first");
                    }
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
            DataHandler.clearListViewCheckedBoxes(ref listView_DataMapper, dataInUse);
        }        

        private void button_DataMapper_FileCreate_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show(dataInUse);
                DataTable dt = DataTableFactory.DtCgSpec();
                foreach(DataRow row_import in dataset.Tables[dataInUse].Rows)
                {
                    DataRow row_export = dt.NewRow();

                    //Primkey
                    row_export["Primkey"] = /**/textBox1_DataMapper_Primkey.Text + /**/textBox2_DataMapper_Primkey.Text + /**/row_import.Field<string>(comboBox_DataMapper_Primkey.Text) + /**/textBox3_DataMapper_Primkey.Text;

                    //row_export["PersonRef"] =
                    //row_export["ClientName"] =
                    //row_export["AddedBy"] =
                    //row_export["AddedDateTime"] =
                    //row_export["Title"] =
                    //row_export["FirstName"] =
                    //row_export["MiddleName"] =
                    //row_export["Surname"] =
                    //row_export["Salutation"] =
                    //row_export["AddressLine1"] =
                    //row_export["AddressLine2"] =
                    //row_export["AddressLine3"] =
                    //row_export["TownCity"] =
                    //row_export["County"] =
                    //row_export["Postcode"] =
                    //row_export["Country"] =
                    //row_export["OrganisationName"] =
                    //row_export["TelephoneNumber"] =
                    //row_export["MobileNumber"] =
                    //row_export["EmailAddress"] =
                    //row_export["AppealCode"] =
                    //row_export["PackageCode"] =
                    //row_export["Deceased"] =
                    //row_export["Goneaway"] =
                    //row_export["NoFurtherCommunication"] =
                    //row_export["PreloadedCAFNumber"] =
                    //row_export["ColdURN"] =
                    //row_export["ImportFile"] =
                    //row_export["RaffleStartNumber"] =
                    //row_export["RaffleEndNumber"] =
                    //row_export["RecordType"] =
                    //row_export["GiftAid"] =
                    //row_export["Campaign"] =
                    //row_export["PhonePreference"] =
                    //row_export["MailPreference"] =
                    //row_export["EmailPreference"] =
                    //row_export["SMSPreference"] =
                    //row_export["ThirdPartyPreference"] =
                    //row_export["Barcode"] =
                    //row_export["ClientData1"] =
                    //row_export["ClientData2"] =
                    //row_export["ClientData3"] =
                    //row_export["ClientData4"] =
                    //row_export["ClientData5"] =
                    //row_export["ClientData6"] =
                    //row_export["ClientData7"] =
                    //row_export["ClientData8"] =
                    //row_export["ClientData9"] =
                    //row_export["ClientData10"] =

                    dt.Rows.Add(row_export);
                }
                dt.TableName = DataHandler.StrRenamingFromDsTableName(dataset, $"{textBox_DataMapper_AppealCode.Text}_{textBox3_DataMapper_Primkey.Text}");
                dataset.Tables.Add(dt);

                //Add new item to the listview
                DataHandler.addItemToListView(listView_DataMapper, dt.TableName, dt);
            }
            catch 
            {

            }


        }        

        private void button_DataMapper_FileClear_Click(object sender, EventArgs e)
        {
            //Clear dataloaded
            dataInUse = "";
            DataHandler.clearListViewCheckedBoxes(ref listView_DataMapper, dataInUse);
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
            DataHandler.clearListViewCheckedBoxes(ref listView_DataMapper, dataInUse);

        }

        private void comboBox_DataMapper_ClientName_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1_DataMapper_Primkey.Text = ((KeyValuePair<string,string>) comboBox_DataMapper_ClientName.SelectedItem).Key;
        }

        private void textBox_DataMapper_AppealCode_TextChanged(object sender, EventArgs e)
        {
            textBox2_DataMapper_Primkey.Text = textBox_DataMapper_AppealCode.Text;
            textBox_DataMapper_ImportFile.Text = textBox_DataMapper_AppealCode.Text;
        }
    }
}
