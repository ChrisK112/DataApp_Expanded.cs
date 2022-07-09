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
            //Iterate through the pages
            foreach(TabPage tabpage in tabControl_Main.TabPages)
            {
                //Check the current TabPage
                if(tabpage.Name == tabControl_Main.SelectedTab.Name)
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

                                //Get datasources
                                string[] colNames_import = DataHandler.colNamesArray(dataset.Tables[lstItem.Text], true);
                                string[] colNames_Cg = DataHandler.colNamesArray(DataTableFactory.DtCgSpec(), true);
                                IOrderedEnumerable<KeyValuePair<string, string>> charityNames = DataHandler.CharityNamesPairs();

                                //GET COMBOBOXES
                                IEnumerable<Control> comboBoxLst = DataHandler.ienumControlList(tabpage, typeof(ComboBox));

                                foreach (ComboBox box in comboBoxLst)
                                {
                                    //BIND DATA SOURCES
                                    if (box.Name == "comboBox_DataMapper_ClientName")
                                    {
                                        DataHandler.dataSourceBinder(box, charityNames);
                                    }
                                    else if (box.Name == "comboBox_DataMapper_RemoveDuplicate" || box.Name == "comboBox_DataMapper_Replace")
                                    {
                                        DataHandler.dataSourceBinder(box, colNames_Cg);
                                    }
                                    else
                                    {
                                        DataHandler.dataSourceBinder(box, colNames_import);
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
                    DataHandler.clearListViewCheckedBoxes(ref listView_DataMapper, dataInUse);
                    textBox3_DataMapper_Primkey.Text = DateTime.Now.ToString("ddMMyyyy");
                    textBox_DataMapper_AddedDateTime.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    textBox_DataMapper_AddedBy.Text = "Admin";
                }
            }                        
        }

        private void button_DataMapper_FileDelete_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lstItem in listView_DataMapper.Items)
            {
                //Delete selected datatables and listviewitems
                if (lstItem.Checked)
                {
                    //Check if it has not been loaded
                    if(lstItem.Text != dataInUse)
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

            //Works only if 2 or more files are checked
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
                DataTable dt = DataTableFactory.DtCgSpec();
                string joinedStr = ""; /*This is use below to join addressLines and Barcode comboboxes*/
                foreach(DataRow row_import in dataset.Tables[dataInUse].Rows)
                {
                    DataRow row_export = dt.NewRow();

                    //Primkey
                    if(comboBox_DataMapper_Primkey.Text != "")
                    {
                        row_export["Primkey"] = /**/textBox1_DataMapper_Primkey.Text + /**/textBox2_DataMapper_Primkey.Text + /**/row_import.Field<string>(comboBox_DataMapper_Primkey.Text) + /**/textBox3_DataMapper_Primkey.Text;
                    }

                    //PersonRef
                    if(comboBox_DataMapper_PersonRef.Text != "")
                    {
                        row_export["PersonRef"] = row_import.Field<string>(comboBox_DataMapper_PersonRef.Text);
                    }

                    //ClientName
                    if (comboBox_DataMapper_ClientName.Text != "")
                    {
                        row_export["ClientName"] = row_import.Field<string>(comboBox_DataMapper_ClientName.Text);
                    }

                    //AddedBy
                    if (textBox_DataMapper_AddedBy.Text != "")
                    {
                        row_export["AddedBy"] = textBox_DataMapper_AddedBy.Text;
                    }

                    //AddedDateTime
                    if (textBox_DataMapper_AddedDateTime.Text != "")
                    {
                        row_export["AddedDateTime"] = textBox_DataMapper_AddedDateTime.Text;
                    }

                    //Title
                    if (comboBox_DataMapper_Title.Text != "")
                    {
                        row_export["Title"] = row_import.Field<string>(comboBox_DataMapper_Title.Text);
                    }

                    //FirstName
                    if (comboBox_DataMapper_FirstName.Text != "")
                    {
                        row_export["FirstName"] = row_import.Field<string>(comboBox_DataMapper_FirstName.Text);
                    }

                    //MiddleName
                    if (comboBox_DataMapper_MiddleName.Text != "")
                    {
                        row_export["MiddleName"] = row_import.Field<string>(comboBox_DataMapper_MiddleName.Text);
                    }

                    //Surname
                    if (comboBox_DataMapper_Surname.Text != "")
                    {
                        row_export["Surname"] = row_import.Field<string>(comboBox_DataMapper_Surname.Text);
                    }

                    //Salutation
                    if (comboBox_DataMapper_Salutation.Text != "")
                    {
                        row_export["Salutation"] = row_import.Field<string>(comboBox_DataMapper_Salutation.Text);
                    }

                    //AddressLine1
                    if (comboBox_DataMapper_AddressLine1.Text != "")
                    {
                        row_export["AddressLine1"] = row_import.Field<string>(comboBox_DataMapper_AddressLine1.Text);
                    }

                    //AddressLine2
                    if (comboBox_DataMapper_AddressLine2.Text != "")
                    {
                        row_export["AddressLine2"] = row_import.Field<string>(comboBox_DataMapper_AddressLine2.Text);
                    }

                    //AddressLine3
                    if(comboBox_DataMapper_AddressLine3.Text != "")
                    {
                        joinedStr = row_import.Field<string>(comboBox_DataMapper_AddressLine3.Text);
                    }
                    if (comboBox_DataMapper_AddressLine4.Text != "")
                    {
                        joinedStr += " " + row_import.Field<string>(comboBox_DataMapper_AddressLine4.Text);
                    }
                    if (comboBox_DataMapper_AddressLine5.Text != "")
                    {
                        joinedStr += " " + row_import.Field<string>(comboBox_DataMapper_AddressLine5.Text);
                    }
                    
                    row_export["AddressLine3"] = joinedStr.Replace("  ", " ").TrimStart().TrimEnd();

                    //TownCity
                    if (comboBox_DataMapper_TownCity.Text != "")
                    {
                        row_export["TownCity"] = row_import.Field<string>(comboBox_DataMapper_TownCity.Text);
                    }

                    //County
                    if (comboBox_DataMapper_County.Text != "")
                    {
                        row_export["County"] = row_import.Field<string>(comboBox_DataMapper_County.Text);
                    }

                    //Postcode
                    if (comboBox_DataMapper_Postcode.Text != "")
                    {
                        row_export["Postcode"] = row_import.Field<string>(comboBox_DataMapper_Postcode.Text);
                    }

                    //Country
                    if (comboBox_DataMapper_Country.Text != "")
                    {
                        row_export["Country"] = row_import.Field<string>(comboBox_DataMapper_Country.Text);
                    }

                    //OrganisationName
                    if (comboBox_DataMapper_OrganisationName.Text != "")
                    {
                        row_export["OrganisationName"] = row_import.Field<string>(comboBox_DataMapper_OrganisationName.Text);
                    }

                    //TelephoneNumber
                    if (comboBox_DataMapper_TelephoneNumber.Text != "")
                    {
                        row_export["TelephoneNumber"] = row_import.Field<string>(comboBox_DataMapper_TelephoneNumber.Text);
                    }

                    //MobileNumber
                    if (comboBox_DataMapper_MobileNumber.Text != "")
                    {
                        row_export["MobileNumber"] = row_import.Field<string>(comboBox_DataMapper_MobileNumber.Text);
                    }

                    //EmailAddress
                    if (comboBox_DataMapper_EmailAddress.Text != "")
                    {
                        row_export["EmailAddress"] = row_import.Field<string>(comboBox_DataMapper_EmailAddress.Text);
                    }

                    //AppealCode
                    if (textBox_DataMapper_AppealCode.Text != "")
                    {
                        row_export["AppealCode"] = textBox_DataMapper_AppealCode.Text;
                    }

                    //PackageCode
                    if (comboBox_DataMapper_PackageCode.Text != "")
                    {
                        row_export["PackageCode"] = row_import.Field<string>(comboBox_DataMapper_PackageCode.Text);
                    }

                    //Deceased
                    if (comboBox_DataMapper_Deceased.Text != "")
                    {
                        row_export["Deceased"] = row_import.Field<string>(comboBox_DataMapper_Deceased.Text);
                    }
                    else
                    {
                        row_export["Deceased"] = "0";
                    }

                    //Goneaway
                    if (comboBox_DataMapper_Goneaway.Text != "")
                    {
                        row_export["Goneaway"] = row_import.Field<string>(comboBox_DataMapper_Goneaway.Text);
                    }
                    else
                    {
                        row_export["Goneaway"] = "0";
                    }

                    //NoFurtherCommunication
                    if (comboBox_DataMapper_NoFurtherCommunication.Text != "")
                    {
                        row_export["NoFurtherCommunication"] = row_import.Field<string>(comboBox_DataMapper_NoFurtherCommunication.Text);
                    }
                    else
                    {
                        row_export["NoFurtherCommunication"] = "0";
                    }

                    //PreloadedCAFNumber
                    if (comboBox_DataMapper_PreloadedCAFNumber.Text != "")
                    {
                        row_export["PreloadedCAFNumber"] = row_import.Field<string>(comboBox_DataMapper_PreloadedCAFNumber.Text);
                    }

                    //ColdURN
                    if (comboBox_DataMapper_ColdURN.Text != "")
                    {
                        row_export["ColdURN"] = row_import.Field<string>(comboBox_DataMapper_ColdURN.Text);
                    }

                    //ImportFile
                    if (textBox_DataMapper_ImportFile.Text != "")
                    {
                        row_export["ImportFile"] = textBox_DataMapper_ImportFile.Text;
                    }

                    //RaffleStartNumber
                    if (comboBox_DataMapper_RaffleStartNumber.Text != "")
                    {
                        row_export["RaffleStartNumber"] = row_import.Field<string>(comboBox_DataMapper_RaffleStartNumber.Text);
                    }

                    //RaffleStartNumber
                    if (comboBox_DataMapper_RaffleEndNumber.Text != "")
                    {
                        row_export["RaffleEndNumber"] = row_import.Field<string>(comboBox_DataMapper_RaffleEndNumber.Text);
                    }

                    //RecordType
                    row_export["RecordType"] = (radioButton1_DataMapper_RecordType.Checked ? "Warm" : "Cold");

                    //GiftAid
                    if (comboBox_DataMapper_GiftAid.Text != "")
                    {
                        row_export["GiftAid"] = row_import.Field<string>(comboBox_DataMapper_GiftAid.Text);
                    }
                    else
                    {
                        row_export["GiftAid"] = "Unknown";
                    }

                    //Campaign
                    if (textBox_DataMapper_ImportFile.Text != "")
                    {
                        row_export["Campaign"] = textBox_DataMapper_Campaign.Text;
                    }

                    //PhonePreference
                    if (comboBox_DataMapper_PhonePreference.Text != "")
                    {
                        row_export["PhonePreference"] = row_import.Field<string>(comboBox_DataMapper_PhonePreference.Text);
                    }
                    else
                    {
                        row_export["PhonePreference"] = "Unknown";
                    }

                    //MailPreference
                    if (comboBox_DataMapper_MailPreference.Text != "")
                    {
                        row_export["MailPreference"] = row_import.Field<string>(comboBox_DataMapper_MailPreference.Text);
                    }
                    else
                    {
                        row_export["MailPreference"] = "Unknown";
                    }

                    //EmailPreference
                    if (comboBox_DataMapper_EmailPreference.Text != "")
                    {
                        row_export["EmailPreference"] = row_import.Field<string>(comboBox_DataMapper_EmailPreference.Text);
                    }
                    else
                    {
                        row_export["EmailPreference"] = "Unknown";
                    }

                    //SMSPreference
                    if (comboBox_DataMapper_SMSPreference.Text != "")
                    {
                        row_export["SMSPreference"] = row_import.Field<string>(comboBox_DataMapper_SMSPreference.Text);
                    }
                    else
                    {
                        row_export["SMSPreference"] = "Unknown";
                    }

                    //ThirdPartyPreference
                    if (comboBox_DataMapper_ThirdPartyPreference.Text != "")
                    {
                        row_export["ThirdPartyPreference"] = row_import.Field<string>(comboBox_DataMapper_ThirdPartyPreference.Text);
                    }
                    else
                    {
                        row_export["ThirdPartyPreference"] = "Unknown";
                    }

                    //Barcode
                    string delimiter = (textBox1_DataMapper_Barcode.Text == "" ? " " : textBox1_DataMapper_Barcode.Text);
                    if (comboBox_DataMapper_Barcode1.Text != "")
                    {
                        joinedStr = row_import.Field<string>(comboBox_DataMapper_Barcode1.Text);
                    }
                    if (comboBox_DataMapper_Barcode2.Text != "")
                    {
                        joinedStr += textBox1_DataMapper_Barcode.Text + row_import.Field<string>(comboBox_DataMapper_Barcode2.Text);
                    }
                    if (comboBox_DataMapper_Barcode3.Text != "")
                    {
                        joinedStr += textBox1_DataMapper_Barcode.Text + row_import.Field<string>(comboBox_DataMapper_Barcode3.Text);
                    }

                    row_export["Barcode"] = joinedStr.ToString().Replace(delimiter + delimiter, delimiter).TrimStart(Convert.ToChar(delimiter)).TrimEnd(Convert.ToChar(delimiter));

                    //ClientData1
                    if (comboBox_DataMapper_ClientData1.Text != "")
                    {
                        row_export["ClientData1"] = row_import.Field<string>(comboBox_DataMapper_ClientData1.Text);
                    }

                    //ClientData2
                    if (comboBox_DataMapper_ClientData2.Text != "")
                    {
                        row_export["ClientData2"] = row_import.Field<string>(comboBox_DataMapper_ClientData2.Text);
                    }

                    //ClientData3
                    if (comboBox_DataMapper_ClientData3.Text != "")
                    {
                        row_export["ClientData3"] = row_import.Field<string>(comboBox_DataMapper_ClientData3.Text);
                    }

                    //ClientData4
                    if (comboBox_DataMapper_ClientData4.Text != "")
                    {
                        row_export["ClientData4"] = row_import.Field<string>(comboBox_DataMapper_ClientData4.Text);
                    }

                    //ClientData5
                    if (comboBox_DataMapper_ClientData5.Text != "")
                    {
                        row_export["ClientData5"] = row_import.Field<string>(comboBox_DataMapper_ClientData5.Text);
                    }

                    //ClientData6
                    if (comboBox_DataMapper_ClientData6.Text != "")
                    {
                        row_export["ClientData6"] = row_import.Field<string>(comboBox_DataMapper_ClientData6.Text);
                    }

                    //ClientData7
                    if (comboBox_DataMapper_ClientData7.Text != "")
                    {
                        row_export["ClientData7"] = row_import.Field<string>(comboBox_DataMapper_ClientData7.Text);
                    }

                    //ClientData8
                    if (comboBox_DataMapper_ClientData8.Text != "")
                    {
                        row_export["ClientData8"] = row_import.Field<string>(comboBox_DataMapper_ClientData8.Text);
                    }

                    //ClientData9
                    if (comboBox_DataMapper_ClientData9.Text != "")
                    {
                        row_export["ClientData9"] = row_import.Field<string>(comboBox_DataMapper_ClientData9.Text);
                    }

                    //ClientData10
                    if (comboBox_DataMapper_ClientData10.Text != "")
                    {
                        row_export["ClientData10"] = row_import.Field<string>(comboBox_DataMapper_ClientData10.Text);
                    }

                    dt.Rows.Add(row_export);
                }

                dt.TableName = DataHandler.StrRenamingFromDsTableName(dataset, $"{textBox_DataMapper_AppealCode.Text}_{textBox3_DataMapper_Primkey.Text}");
                if (checkBox_DataMapper_RemoveDuplicate.Checked)
                {
                    string colDuplicateSelected = comboBox_DataMapper_RemoveDuplicate.Text == "" ? "Primkey" : comboBox_DataMapper_RemoveDuplicate.Text;
                    DataHandler.dtRemoveDuplicateRows(ref dt, colDuplicateSelected);
                }

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

            //Iterate through the pages
            foreach (TabPage tabpage in tabControl_Main.TabPages)
            {
                //Check the current TabPage
                if (tabpage.Name == tabControl_Main.SelectedTab.Name)
                {
                    //GET COMBOBOXES
                    IEnumerable<Control> comboBoxLst = DataHandler.ienumControlList(tabpage, typeof(ComboBox));

                    foreach (ComboBox box in comboBoxLst)
                    {
                        box.DataSource = null;
                    }

                    //GET TEXTBOXES
                    IEnumerable<Control> textBoxLst = DataHandler.ienumControlList(tabpage, typeof(TextBox));

                    foreach (TextBox box in textBoxLst)
                    {
                        box.Text = "";
                    }
                }
            }

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

        private void checkBox_DataMapper_RemoveDuplicate_CheckedChanged(object sender, EventArgs e)
        {
            comboBox_DataMapper_RemoveDuplicate.Enabled = (checkBox_DataMapper_RemoveDuplicate.Checked == true ? true : false);
        }

        private void checkBox_DataMapper_Replace_CheckedChanged(object sender, EventArgs e)
        {
            comboBox_DataMapper_Replace.Enabled = (checkBox_DataMapper_Replace.Checked == true ? true : false);
        }

        private void button_DataMapper_ToMiScan_Click(object sender, EventArgs e)
        {
            //Check how many files are checked
            lstItemsCheckedCount = listView_DataMapper.Items.OfType<ListViewItem>().Where(x => x.Checked).Count();

            //Works only if 1 file is checked
            if (lstItemsCheckedCount == 1)
            {
                
                System.Data.DataTable dt_export = DataTableFactory.DtMiScanSpec();

                foreach (ListViewItem lstItem in listView_DataMapper.Items)
                {
                    if (lstItem.Checked)
                    {
                        System.Data.DataTable dt_import = dataset.Tables[lstItem.Text];

                        //CHECK ALL COLUMNS EXIST
                        if(DataHandler.allColumnsExist(dt_import, DataTableFactory.DtCgSpec()))
                        {
                            foreach (DataRow row_import in dt_import.Rows)
                            {
                                DataRow row_export = dt_export.NewRow();

                                row_export["URN"] = row_import.Field<string>("PersonRef");
                                row_export["Cold URN"] = row_import.Field<string>("ColdURN");
                                row_export["ClientName"] = row_import.Field<string>("ClientName");
                                row_export["Title"] = row_import.Field<string>("Title");
                                row_export["FirstName/Initial"] = row_import.Field<string>("FirstName");
                                row_export["MiddleName"] = row_import.Field<string>("MiddleName");
                                row_export["Surname"] = row_import.Field<string>("Surname");
                                row_export["Salutation"] = row_import.Field<string>("Salutation");
                                row_export["OrganisationName"] = row_import.Field<string>("OrganisationName");
                                row_export["Address1"] = row_import.Field<string>("AddressLine1");
                                row_export["Address2"] = row_import.Field<string>("AddressLine2");
                                row_export["Address3"] = row_import.Field<string>("AddressLine3");
                                row_export["TownCity"] = row_import.Field<string>("TownCity");
                                row_export["County"] = row_import.Field<string>("County");
                                row_export["Postcode"] = row_import.Field<string>("Postcode");
                                row_export["Country"] = row_import.Field<string>("Country");
                                row_export["TelephoneNumber"] = row_import.Field<string>("TelephoneNumber");
                                row_export["MobileNumber"] = row_import.Field<string>("MobileNumber");
                                row_export["EmailAddress"] = row_import.Field<string>("EmailAddress");
                                //row_export["WorkEmailAddress"] = row_import.Field<string>();
                                row_export["AppealCode"] = row_import.Field<string>("AppealCode");
                                row_export["PackageCode"] = row_import.Field<string>("PackageCode");
                                row_export["Campaign"] = row_import.Field<string>("Campaign");
                                row_export["GiftAid"] = row_import.Field<string>("GiftAid");
                                row_export["NoFurtherCommunication"] = row_import.Field<string>("NoFurtherCommunication");
                                row_export["PhonePreference"] = row_import.Field<string>("PhonePreference");
                                row_export["MailPreference"] = row_import.Field<string>("MailPreference");
                                row_export["EmailPreference"] = row_import.Field<string>("EmailPreference");
                                row_export["SMSPreference"] = row_import.Field<string>("SMSPreference");
                                row_export["ThirdPartyPreference"] = row_import.Field<string>("ThirdPartyPreference");
                                
                                if (checkBox_DataMapper_ClientFieldsIncluded.Checked)
                                {
                                    row_export["Spare1"] = row_import.Field<string>("ClientData1");
                                    row_export["Spare2"] = row_import.Field<string>("ClientData2");
                                    row_export["Spare3"] = row_import.Field<string>("ClientData3");
                                    row_export["Spare4"] = row_import.Field<string>("ClientData4");
                                    row_export["Spare5"] = row_import.Field<string>("ClientData5");
                                    row_export["Spare6"] = row_import.Field<string>("ClientData6");
                                    row_export["Spare7"] = row_import.Field<string>("ClientData7");
                                    row_export["Spare8"] = row_import.Field<string>("ClientData8");
                                    row_export["Spare9"] = row_import.Field<string>("ClientData9");
                                    row_export["Spare10"] = row_import.Field<string>("ClientData10");
                                    //row_export["Spare11"] = row_import.Field<string>();
                                    //row_export["Spare12"] = row_import.Field<string>();
                                    //row_export["Spare13"] = row_import.Field<string>();
                                    //row_export["Spare14"] = row_import.Field<string>();
                                    //row_export["Spare15"] = row_import.Field<string>();
                                    //row_export["Spare16"] = row_import.Field<string>();
                                    //row_export["Spare17"] = row_import.Field<string>();
                                    //row_export["Spare18"] = row_import.Field<string>();
                                    //row_export["Spare19"] = row_import.Field<string>();
                                    //row_export["Spare20"] = row_import.Field<string>();
                                    //row_export["Spare21"] = row_import.Field<string>();
                                    //row_export["Spare22"] = row_import.Field<string>();
                                    //row_export["Spare23"] = row_import.Field<string>();
                                    //row_export["Spare24"] = row_import.Field<string>();
                                    //row_export["Spare25"] = row_import.Field<string>();
                                }
                                dt_export.Rows.Add(row_export);
                            }

                            //Adds new datatable
                            string dtName = DataHandler.StrRenamingFromDsTableName(dataset);
                            dt_export.TableName = dtName;
                            dataset.Tables.Add(dt_export);

                            //Add new item to the listview
                            DataHandler.addItemToListView(listView_DataMapper, dtName, dt_export);
                        }
                        else
                        {
                            MessageBox.Show("This function only accept Committed Giving Data Spec\nto convert to MiScan Data spec");
                        }
                        
                    }
                }
            }
            else
            {
                MessageBox.Show("You need to select 1 datafile");
            }
            
            //Default settings
            DataHandler.clearListViewCheckedBoxes(ref listView_DataMapper, dataInUse);
        }
    }
}
