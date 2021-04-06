using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace DataApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //GLOBAL VARIABLES
        OpenFileDialog ofd = new OpenFileDialog();
        public static string fileWithPath;
        public static DataTable dt;
        public static DataTable dtTarget = new DataTable();
        char delimiterF1;
        public static bool dataLoaded, dataExport = false;
        Form2 form2;

        private void button1_Form1_search_Click(object sender, EventArgs e)
        {
            ofd.Filter = "Text(Tab delimited) (*.txt) |*.txt|CSV (Comma delimited) (*.csv) |*.csv";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBox1_Form1_filePath.Text = ofd.FileName;
                fileWithPath = textBox1_Form1_filePath.Text;                
                Form2.fileWithPathF2 = File.Exists(fileWithPath) == true ? fileWithPath : "";
            }
        }

        private void button1_Form1_import_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (File.Exists(fileWithPath))
                {
                    form2 = new Form2();
                    form2.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Please select a file");
                }
                
                if(Form2.dataOk)
                {
                    delimiterF1 = Form2.delimiterF2;
                    dt = DataHandler.FlatToDt(fileWithPath, delimiterF1);
                    DataTable dtFirst50 = dt.AsEnumerable().Take(50).CopyToDataTable();

                    //DATA SOURCES TO GRID AND COMBOBOXES
                    dataGridView1.Columns.Clear();
                    dataGridView1.DataSource = dtFirst50;

                    foreach (Control tab in tabControl1.TabPages)
                    {
                        TabPage tabPage = (TabPage)tab;

                        //COMMITED GIVING
                        if (tabPage.Name == "CG_tabPage1")
                        {
                            foreach (Control group in tabPage.Controls)
                            {
                                foreach (Control item in group.Controls)
                                {
                                    if (item.GetType().Name == "ComboBox")
                                    {
                                        ComboBox comboBox = (ComboBox)item;
                                        if (item.Name == "comboBox1_CG_ClientName")
                                        {
                                            comboBox.BindingContext = new BindingContext();
                                            comboBox.DataSource = new BindingSource(DataHandler.CharityNamesPairs(), null);
                                            comboBox.DisplayMember = "Value";
                                            comboBox.ValueMember = "Key";
                                        }
                                        else
                                        {
                                            comboBox.BindingContext = new BindingContext();
                                            comboBox.DataSource = DataHandler.colNamesArray(dtFirst50, true);
                                        }
                                    }
                                }
                            }
                            comboBox1_CG_duplicates.DataSource = DataHandler.colNamesArray(DataTableFactory.DtScheme(tabPage.Name), true);
                        }

                    }

                    //DEFAULT VALUES
                    textBox3_CG_Primkey.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    textBox1_CG_AddedDateTime.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    textBox1_CG_AddedBy.Text = "Admin";
                    textBox1_CG_Primkey.Text = "";
                    label2_CG_RowsImported.Text = dt.Rows.Count.ToString();
                    radioButton1_CG_RecordTypeWarm.Checked = true;
                    

                    //GLOBAL VALUES
                    fileWithPath = null;
                    textBox1_Form1_filePath.Text = fileWithPath;
                    Form2.fileWithPathF2 = null;
                    dataLoaded = false;
                    Form2.dataOk = false;
                    label2_CG_RowsImported.Text = dt.Rows.Count.ToString();
                    label2_CG_RowsLoaded.Text = "0";
                    label2_CG_RowsDeleted.Text = "0";
                }
                
            }
            catch (System.InvalidOperationException)
            {
                MessageBox.Show("The file you try to import does not contain any rows");
            }
            catch(Exception)
            {
                MessageBox.Show("There is an issue with your file");
            }
            
        }             

        private void button1_Form1_load_Click(object sender, EventArgs e)
        {          
            
            TabPage tabPage = tabControl1.SelectedTab;
            int rowsDeleted = 0;

            //COMMITTED GIVING
            if (tabPage == CG_tabPage1)
            {
                
                try
                {
                    if (dt != null)
                    {
                        //SPECIAL CHARACTERS TO REMOVE
                        string val1, val2, val3, val4, valblank;
                        val1 = checkBox1_CG_lineFeed.Checked ? "\n" : "*";
                        val2 = checkBox1_CG_quotations.Checked ? "\'" : "*";
                        val3 = checkBox1_CG_doubleQuotations.Checked ? "\"" : "*";
                        val4 = checkBox1_CG_bar.Checked ? "|" : "*";
                        valblank = "";

                        dtTarget = DataTableFactory.DtScheme(CG_tabPage1.Name);
                        var query = from row in dt.AsEnumerable()
                                    select new
                                    {
                                        //CLEAN AND MAP DATA
                                        Primkey = (textBox1_CG_Primkey.Text + textBox2_CG_Primkey.Text + (comboBox1_CG_Primkey.Text == "" ? "" : row.Field<string>(comboBox1_CG_Primkey.Text)) + textBox3_CG_Primkey.Text).Replace(val1, valblank).Replace(val2, valblank).Replace(val3, valblank).Replace(val4, valblank),
                                        PersonRef = (comboBox1_CG_PersonRef.Text == "" ? "" : row.Field<string>(comboBox1_CG_PersonRef.Text)).Replace(val1, valblank).Replace(val2, valblank).Replace(val3, valblank).Replace(val4, valblank),
                                        ClientName = comboBox1_CG_ClientName.Text,
                                        AddedBy = textBox1_CG_AddedBy.Text,
                                        AddedDateTime = textBox1_CG_AddedDateTime.Text,
                                        Title = (comboBox1_CG_Title.Text == "" ? "" : row.Field<string>(comboBox1_CG_Title.Text)).Replace(val1, valblank).Replace(val2, valblank).Replace(val3, valblank).Replace(val4, valblank),
                                        FirstName = (comboBox1_CG_FirstName.Text == "" ? "" : row.Field<string>(comboBox1_CG_FirstName.Text)).Replace(val1, valblank).Replace(val2, valblank).Replace(val3, valblank).Replace(val4, valblank),
                                        MiddleName = (comboBox1_CG_MiddleName.Text == "" ? "" : row.Field<string>(comboBox1_CG_MiddleName.Text)).Replace(val1, valblank).Replace(val2, valblank).Replace(val3, valblank).Replace(val4, valblank),
                                        Surname = (comboBox1_CG_Surname.Text == "" ? "" : row.Field<string>(comboBox1_CG_Surname.Text)).Replace(val1, valblank).Replace(val2, valblank).Replace(val3, valblank).Replace(val4, valblank),
                                        Salutation = (comboBox1_CG_Salutation.Text == "" ? "" : row.Field<string>(comboBox1_CG_Salutation.Text)).Replace(val1, valblank).Replace(val2, valblank).Replace(val3, valblank).Replace(val4, valblank),
                                        AddressLine1 = (comboBox1_CG_AddressLine1.Text == "" ? "" : row.Field<string>(comboBox1_CG_AddressLine1.Text)).Replace(val1, valblank).Replace(val2, valblank).Replace(val3, valblank).Replace(val4, valblank),
                                        AddressLine2 = (comboBox1_CG_AddressLine2.Text == "" ? "" : row.Field<string>(comboBox1_CG_AddressLine2.Text)).Replace(val1, valblank).Replace(val2, valblank).Replace(val3, valblank).Replace(val4, valblank),
                                        AddressLine3 = (((comboBox1_CG_AddressLine3.Text == "" ? "" : row.Field<string>(comboBox1_CG_AddressLine3.Text)) + " " + (comboBox1_CG_AddressLine4.Text == "" ? "" : row.Field<string>(comboBox1_CG_AddressLine4.Text)) + " " + (comboBox1_CG_AddressLine5.Text == "" ? "" : row.Field<string>(comboBox1_CG_AddressLine5.Text))).Replace("  ", " ").TrimStart().TrimEnd()).Replace(val1, valblank).Replace(val2, valblank).Replace(val3, valblank).Replace(val4, valblank),
                                        TownCity = (comboBox1_CG_TownCity.Text == "" ? "" : row.Field<string>(comboBox1_CG_TownCity.Text)).Replace(val1, valblank).Replace(val2, valblank).Replace(val3, valblank).Replace(val4, valblank),
                                        County = (comboBox1_CG_County.Text == "" ? "" : row.Field<string>(comboBox1_CG_County.Text)).Replace(val1, valblank).Replace(val2, valblank).Replace(val3, valblank).Replace(val4, valblank),
                                        Postcode = (comboBox1_CG_Postcode.Text == "" ? "" : row.Field<string>(comboBox1_CG_Postcode.Text)).Replace(val1, valblank).Replace(val2, valblank).Replace(val3, valblank).Replace(val4, valblank),
                                        Country = (checkBox1_CG_uniqueCountry.Checked == true ? textBox1_CG_uniqueCountry.Text : (comboBox1_CG_Country.Text == "" ? "" : row.Field<string>(comboBox1_CG_Country.Text)).Replace("UK", "United Kingdom")).Replace(val1, valblank).Replace(val2, valblank).Replace(val3, valblank).Replace(val4, valblank),
                                        OrganisationName = (comboBox1_CG_OrganisationName.Text == "" ? "" : row.Field<string>(comboBox1_CG_OrganisationName.Text)).Replace(val1, valblank).Replace(val2, valblank).Replace(val3, valblank).Replace(val4, valblank),
                                        TelephoneNumber = (comboBox1_CG_TelephoneNumber.Text == "" ? "" : row.Field<string>(comboBox1_CG_TelephoneNumber.Text)).Replace(val1, valblank).Replace(val2, valblank).Replace(val3, valblank).Replace(val4, valblank),
                                        MobileNumber = (comboBox1_CG_MobileNumber.Text == "" ? "" : row.Field<string>(comboBox1_CG_MobileNumber.Text)).Replace(val1, valblank).Replace(val2, valblank).Replace(val3, valblank).Replace(val4, valblank),
                                        EmailAddress = (comboBox1_CG_EmailAddress.Text == "" ? "" : row.Field<string>(comboBox1_CG_EmailAddress.Text)).Replace(val1, valblank).Replace(val2, valblank).Replace(val3, valblank).Replace(val4, valblank),
                                        AppealCode = textBox1_CG_AppealCode.Text,
                                        PackageCode = (checkBox1_CG_uniquePackcode.Checked == true ? textBox1_CG_uniquePackcode.Text : (comboBox1_CG_PackageCode.Text == "" ? "" : row.Field<string>(comboBox1_CG_PackageCode.Text))).Replace(val1, valblank).Replace(val2, valblank).Replace(val3, valblank).Replace(val4, valblank),
                                        Deceased = "0",
                                        Goneaway = "0",
                                        NoFurtherCommunication = "0",
                                        PreloadedCAFNumber = (comboBox1_CG_PreloadedCAFNumber.Text == "" ? "" : row.Field<string>(comboBox1_CG_PreloadedCAFNumber.Text)).Replace(val1, valblank).Replace(val2, valblank).Replace(val3, valblank).Replace(val4, valblank),
                                        ColdURN = (comboBox1_CG_ColdURN.Text == "" ? "" : row.Field<string>(comboBox1_CG_ColdURN.Text)).Replace(val1, valblank).Replace(val2, valblank).Replace(val3, valblank).Replace(val4, valblank),
                                        ImportFile = textBox1_CG_ImportFile.Text,
                                        RaffleStartNumber = (comboBox1_CG_RaffleStartNumber.Text == "" ? "" : row.Field<string>(comboBox1_CG_RaffleStartNumber.Text)).Replace(val1, valblank).Replace(val2, valblank).Replace(val3, valblank).Replace(val4, valblank),
                                        RaffleEndNumber = (comboBox1_CG_RaffleEndNumber.Text == "" ? "" : row.Field<string>(comboBox1_CG_RaffleEndNumber.Text)).Replace(val1, valblank).Replace(val2, valblank).Replace(val3, valblank).Replace(val4, valblank),
                                        RecordType = radioButton1_CG_RecordTypeWarm.Checked ? "Warm" : "Cold",
                                        GiftAid = "Unknown",
                                        Campaign = textBox1_CG_Campaign.Text,
                                        PhonePreference = "Unknown",
                                        MailPreference = "Unknown",
                                        EmailPreference = "Unknown",
                                        SMSPreference = "Unknown",
                                        ThirdPartyPreference = "Unknown",
                                        Barcode = (((checkBox1_CG_includeAppeal.Checked == true ? textBox1_CG_AppealCode.Text : (comboBox1_CG_Barcode.Text == "" ? "" : row.Field<string>(comboBox1_CG_Barcode.Text))) + textBox1_CG_Barcode.Text + (checkBox1_CG_includePackcode.Checked == true ? textBox1_CG_uniquePackcode.Text : (comboBox2_CG_Barcode.Text == "" ? "" : row.Field<string>(comboBox2_CG_Barcode.Text))) + textBox1_CG_Barcode.Text + (comboBox3_CG_Barcode.Text == "" ? "" : row.Field<string>(comboBox3_CG_Barcode.Text))).TrimStart(Convert.ToChar(textBox1_CG_Barcode.Text == "" ? " " : textBox1_CG_Barcode.Text)).TrimEnd(Convert.ToChar(textBox1_CG_Barcode.Text == "" ? " " : textBox1_CG_Barcode.Text))).Replace(val1, valblank).Replace(val2, valblank).Replace(val3, valblank).Replace(val4, valblank),
                                        ClientData1 = (comboBox1_CG_ClientData1.Text == "" ? "" : row.Field<string>(comboBox1_CG_ClientData1.Text)).Replace(val1, valblank).Replace(val2, valblank).Replace(val3, valblank).Replace(val4, valblank),
                                        ClientData2 = (comboBox1_CG_ClientData2.Text == "" ? "" : row.Field<string>(comboBox1_CG_ClientData2.Text)).Replace(val1, valblank).Replace(val2, valblank).Replace(val3, valblank).Replace(val4, valblank),
                                        ClientData3 = (comboBox1_CG_ClientData3.Text == "" ? "" : row.Field<string>(comboBox1_CG_ClientData3.Text)).Replace(val1, valblank).Replace(val2, valblank).Replace(val3, valblank).Replace(val4, valblank),
                                        ClientData4 = (comboBox1_CG_ClientData4.Text == "" ? "" : row.Field<string>(comboBox1_CG_ClientData4.Text)).Replace(val1, valblank).Replace(val2, valblank).Replace(val3, valblank).Replace(val4, valblank),
                                        ClientData5 = (comboBox1_CG_ClientData5.Text == "" ? "" : row.Field<string>(comboBox1_CG_ClientData5.Text)).Replace(val1, valblank).Replace(val2, valblank).Replace(val3, valblank).Replace(val4, valblank),
                                        ClientData6 = (comboBox1_CG_ClientData6.Text == "" ? "" : row.Field<string>(comboBox1_CG_ClientData6.Text)).Replace(val1, valblank).Replace(val2, valblank).Replace(val3, valblank).Replace(val4, valblank),
                                        ClientData7 = (comboBox1_CG_ClientData7.Text == "" ? "" : row.Field<string>(comboBox1_CG_ClientData7.Text)).Replace(val1, valblank).Replace(val2, valblank).Replace(val3, valblank).Replace(val4, valblank),
                                        ClientData8 = (comboBox1_CG_ClientData8.Text == "" ? "" : row.Field<string>(comboBox1_CG_ClientData8.Text)).Replace(val1, valblank).Replace(val2, valblank).Replace(val3, valblank).Replace(val4, valblank),
                                        ClientData9 = (comboBox1_CG_ClientData9.Text == "" ? "" : row.Field<string>(comboBox1_CG_ClientData9.Text)).Replace(val1, valblank).Replace(val2, valblank).Replace(val3, valblank).Replace(val4, valblank),
                                        ClientData10 = (comboBox1_CG_ClientData10.Text == "" ? "" : row.Field<string>(comboBox1_CG_ClientData10.Text)).Replace(val1, valblank).Replace(val2, valblank).Replace(val3, valblank).Replace(val4, valblank),
                                    };

                        foreach (var row in query)
                        {
                            dtTarget.Rows.Add(row.Primkey, row.PersonRef, row.ClientName, row.AddedBy, row.AddedDateTime, row.Title, row.FirstName, row.MiddleName, row.Surname,
                                row.Salutation, row.AddressLine1, row.AddressLine2, row.AddressLine3, row.TownCity, row.County, row.Postcode, row.Country, row.OrganisationName,
                                row.TelephoneNumber, row.MobileNumber, row.EmailAddress, row.AppealCode, row.PackageCode, row.Deceased, row.Goneaway, row.NoFurtherCommunication,
                                row.PreloadedCAFNumber, row.ColdURN, row.ImportFile, row.RaffleStartNumber, row.RaffleEndNumber, row.RecordType, row.GiftAid, row.Campaign,
                                row.PhonePreference, row.MailPreference, row.EmailPreference, row.SMSPreference, row.ThirdPartyPreference, row.Barcode, row.ClientData1, row.ClientData2,
                                row.ClientData3, row.ClientData4, row.ClientData5, row.ClientData6, row.ClientData7, row.ClientData8, row.ClientData9, row.ClientData10);
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
                
                if (checkBox1_CG_duplicates.Checked)
                {
                    string colName = comboBox1_CG_duplicates.Text == "" ? "Primkey" : comboBox1_CG_duplicates.Text;
                    DataHandler.dtRemoveDuplicateRows(ref dtTarget, colName);
                    rowsDeleted = dt.Rows.Count - dtTarget.Rows.Count;
                }

            }

            //GLOBAL VARIABLES
            dataGridView1.DataSource = dtTarget;
            dataLoaded = true;
            label2_CG_RowsLoaded.Text = checkBox1_CG_duplicates.Checked == true ? (rowsDeleted > 0 ? (dt.Rows.Count - rowsDeleted) : dtTarget.Rows.Count).ToString() : dtTarget.Rows.Count.ToString();
            label2_CG_RowsDeleted.Text = rowsDeleted.ToString();

        }

        private void button1_Form1_save_Click(object sender, EventArgs e)
        {            
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Text(Tab delimited) (*.txt) |*.txt| CSV (Comma delimited) (*.csv) |*.csv";
                sfd.InitialDirectory = Path.GetDirectoryName(fileWithPath);
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string saveas = sfd.FileName;
                    int extentionindex = sfd.FilterIndex;
                    dataExport = true;
                    
                    if (extentionindex == 1)
                    {
                        //IF SAVED AS TEXT, SELECT DELIMITER AND QUALIFIER TO USE
                        form2.ShowDialog(); 
                    }

                    if (Form2.dataOk)
                    {
                        if (dataLoaded)
                        {
                            DataHandler.DtToFlat(dtTarget, saveas, extentionindex, Form2.qualifierR2, Convert.ToChar(Form2.delimiterF2));
                        }
                        else
                        {
                            DataHandler.DtToFlat(dt, saveas, extentionindex, Form2.qualifierR2, Convert.ToChar(Form2.delimiterF2));
                        }
                    }

                    dataExport = false;

                }
            }
            
        }
        private void textBox1_CG_AppealCode_TextChanged(object sender, EventArgs e)
        {
            textBox1_CG_ImportFile.Text = textBox1_CG_AppealCode.Text;
            textBox2_CG_Primkey.Text = textBox1_CG_AppealCode.Text;
        }

        private void comboBox1_CG_ClientName_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1_CG_Primkey.Text = comboBox1_CG_ClientName.SelectedValue.ToString();
            textBox1_CG_Primkey.Text = comboBox1_CG_ClientName.SelectedValue.ToString();
        }


        private void checkBox1_CG_duplicates_CheckedChanged(object sender, EventArgs e)
        {
            comboBox1_CG_duplicates.Enabled = checkBox1_CG_duplicates.Checked == true ? true : false;
        }

        private void checkBox1_CG_uniquePackcode_CheckedChanged(object sender, EventArgs e)
        {
            textBox1_CG_uniquePackcode.Enabled = checkBox1_CG_uniquePackcode.Checked == true ? true : false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1_CG_uniqueCountry.Enabled = checkBox1_CG_uniqueCountry.Checked == true ? true : false;
        }
    }
}
