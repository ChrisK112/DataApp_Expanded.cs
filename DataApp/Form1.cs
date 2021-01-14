using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataApp;
using System.Collections;
using System.Data.OleDb;
using System.IO;
using System.Configuration;

namespace DataApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Global Variables
        OpenFileDialog ofd = new OpenFileDialog();
        public static string sourcePath;
        DataTable dt;
        char delimiter_f1;
        

        //Browse File
        private void button1_Form1_search_Click(object sender, EventArgs e)
        {
            textBox1_Form1_filePath.Clear();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                sourcePath = ofd.FileName;
                textBox1_Form1_filePath.Text = sourcePath;
                Form2 form2 = new Form2();
                form2.ShowDialog();
            }
            //Select delimiter
            
        }

        private void button1_Form1_import_Click(object sender, EventArgs e)
        {
            try
            {
                delimiter_f1 = Form2.delimiter_f2;
                dt = FlatFileHandler.ToDataTable(sourcePath, delimiter_f1);
                DataTable temp_dt = dt.AsEnumerable().Take(20).CopyToDataTable();

                //Comboboxes
                string[] colNamesArray = temp_dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();
                string[] colNamesArrayWithBlank = new string[temp_dt.Columns.Count + 1];
                colNamesArrayWithBlank[0] = "";/*Add an extra empty option to the list*/
                for(int n = 1; n < temp_dt.Columns.Count; n++)
                {
                    colNamesArrayWithBlank[n] = colNamesArray[n - 1];
                }

                Dictionary<string, string> CharityNamesPairs = new Dictionary<string, string>();
                foreach(string key in ConfigurationManager.AppSettings)
                {
                    CharityNamesPairs.Add(key.ToString(), ConfigurationManager.AppSettings[key].ToString());
                }

                //Add Data to Grid and ComboBoxes
                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = temp_dt;
                if (dataGridView1.Columns.Count < 13)
                {
                    dataGridView1.AutoSize = true;
                }

                foreach (Control tab in tabControl1.TabPages)
                {
                    TabPage tabPage = (TabPage)tab;

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
                                        comboBox.BindingContext = new BindingContext(); /*This prevents those from changing together*/
                                        comboBox.DropDownStyle = ComboBoxStyle.DropDownList; /*No manual imput allowed*/
                                        comboBox.DataSource = new BindingSource(CharityNamesPairs, null);
                                        comboBox.DisplayMember = "Value";
                                        comboBox.ValueMember = "Key";
                                    }
                                    else
                                    {
                                        comboBox.BindingContext = new BindingContext();
                                        comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                                        comboBox.DataSource = colNamesArrayWithBlank;
                                    }
                                }
                            }
                        }
                    }
                    
                }
                //Default TextBox values
                textBox3_CG_Primkey.Text = DateTime.Now.ToString("dd/MM/yyyy");
                textBox1_CG_AddedDateTime.Text = DateTime.Now.ToString("dd/MM/yyyy");
                textBox1_CG_AddedBy.Text = "Admin";
                textBox1_CG_Primkey.Text = "";
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
        }

        private void button1_Form1_save_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if(sfd.ShowDialog() == DialogResult.OK)
            {
                string targetPath = sfd.FileName;
            }

        }

        private void button1_Form1_load_Click(object sender, EventArgs e)
        {
            TabPage tabPage = tabControl1.SelectedTab;
            DataTable dt_target = new DataTable();

            if (tabControl1.SelectedTab == CG_tabPage1)
            {
                dt_target = DataTableFactory.DtScheme(CG_tabPage1.Name);
                var query =
                    from row in dt.AsEnumerable()
                    select new
                    {
                        Primkey = textBox1_CG_Primkey.Text + textBox2_CG_Primkey.Text + row.Field<string>(comboBox1_CG_Primkey.Text) + textBox3_CG_Primkey.Text,
                        PersonRef = row.Field<string>(comboBox1_CG_PersonRef.Text),
                        ClientName = comboBox1_CG_ClientName.Text,
                        AddedBy = textBox1_CG_AddedBy.Text,
                        AddedDateTime = textBox1_CG_AddedDateTime.Text,
                        Title = row.Field<string>(comboBox1_CG_Title.Text),
                        FirstName = row.Field<string>(comboBox1_CG_FirstName.Text),
                        MiddleName = row.Field<string>(comboBox1_CG_MiddleName.Text),
                        Surname = row.Field<string>(comboBox1_CG_Surname.Text),
                        Salutation = row.Field<string>(comboBox1_CG_Salutation.Text),
                        AddressLine1 = row.Field<string>(comboBox1_CG_AddressLine1.Text),
                        AddressLine2 = row.Field<string>(comboBox1_CG_AddressLine2.Text),
                        AddressLine3 = row.Field<string>(comboBox1_CG_AddressLine3.Text),
                        TownCity = row.Field<string>(comboBox1_CG_TownCity.Text),
                        County = row.Field<string>(comboBox1_CG_County.Text),
                        Postcode = row.Field<string>(comboBox1_CG_Postcode.Text),
                        Country = row.Field<string>(comboBox1_CG_Country.Text),
                        OrganisationName = row.Field<string>(comboBox1_CG_OrganisationName.Text),
                        TelephoneNumber = row.Field<string>(comboBox1_CG_TelephoneNumber.Text),
                        MobileNumber = row.Field<string>(comboBox1_CG_MobileNumber.Text),
                        EmailAddress = row.Field<string>(comboBox1_CG_EmailAddress.Text),
                        AppealCode = textBox1_CG_AppealCode.Text,
                        PackageCode = row.Field<string>(comboBox1_CG_PackageCode.Text),
                        Deceased = row.Field<string>(comboBox1_CG_Deceased.Text),
                        Goneaway = row.Field<string>(comboBox1_CG_Goneaway.Text),
                        NoFurtherCommunication = row.Field<string>(comboBox1_CG_NoFurtherCommunication.Text),
                        PreloadedCAFNumber = row.Field<string>(comboBox1_CG_PreloadedCAFNumber.Text),
                        ColdURN = row.Field<string>(comboBox1_CG_ColdURN.Text),
                        ImportFile = textBox1_CG_ImportFile.Text,
                        RaffleStartNumber = row.Field<string>(comboBox1_CG_RaffleStartNumber.Text),
                        RaffleEndNumber = row.Field<string>(comboBox1_CG_RaffleEndNumber.Text),
                        RecordType = row.Field<string>(comboBox1_CG_RecordType.Text),
                        GiftAid = row.Field<string>(comboBox1_CG_GiftAid.Text),
                        Campaign = textBox1_CG_Campaign.Text,
                        PhonePreference = row.Field<string>(comboBox1_CG_PhonePreference.Text),
                        MailPreference = row.Field<string>(comboBox1_CG_MailPreference.Text),
                        EmailPreference = row.Field<string>(comboBox1_CG_EmailPreference.Text),
                        SMSPreference = row.Field<string>(comboBox1_CG_SMSPreference.Text),
                        ThirdPartyPreference = row.Field<string>(comboBox1_CG_ThirdPartyPreference.Text),
                        Barcode = row.Field<string>(comboBox1_CG_Barcode.Text) + row.Field<string>(comboBox2_CG_Barcode.Text) + row.Field<string>(comboBox3_CG_Barcode.Text),
                        ClientData1 = row.Field<string>(comboBox1_CG_ClientData1.Text),
                        ClientData2 = row.Field<string>(comboBox1_CG_ClientData2.Text),
                        ClientData3 = row.Field<string>(comboBox1_CG_ClientData3.Text),
                        ClientData4 = row.Field<string>(comboBox1_CG_ClientData4.Text),
                        ClientData5 = row.Field<string>(comboBox1_CG_ClientData5.Text),
                        ClientData6 = row.Field<string>(comboBox1_CG_ClientData6.Text),
                        ClientData7 = row.Field<string>(comboBox1_CG_ClientData7.Text),
                        ClientData8 = row.Field<string>(comboBox1_CG_ClientData8.Text),
                        ClientData9 = row.Field<string>(comboBox1_CG_ClientData9.Text),
                        ClientData10 = row.Field<string>(comboBox1_CG_ClientData10.Text),
                    };
                foreach (var row in query)
                {
                    dt_target.Rows.Add(
                        row.Primkey,
                        row.PersonRef,
                        row.ClientName,
                        row.AddedBy,
                        row.AddedDateTime,
                        row.Title,
                        row.FirstName,
                        row.MiddleName,
                        row.Surname,
                        row.Salutation,
                        row.AddressLine1,
                        row.AddressLine2,
                        row.AddressLine3,
                        row.TownCity,
                        row.County,
                        row.Postcode,
                        row.Country,
                        row.OrganisationName,
                        row.TelephoneNumber,
                        row.MobileNumber,
                        row.EmailAddress,
                        row.AppealCode,
                        row.PackageCode,
                        row.Deceased,
                        row.Goneaway,
                        row.NoFurtherCommunication,
                        row.PreloadedCAFNumber,
                        row.ColdURN,
                        row.ImportFile,
                        row.RaffleStartNumber,
                        row.RaffleEndNumber,
                        row.RecordType,
                        row.GiftAid,
                        row.Campaign,
                        row.PhonePreference,
                        row.MailPreference,
                        row.EmailPreference,
                        row.SMSPreference,
                        row.ThirdPartyPreference,
                        row.Barcode,
                        row.ClientData1,
                        row.ClientData2,
                        row.ClientData3,
                        row.ClientData4,
                        row.ClientData5,
                        row.ClientData6,
                        row.ClientData7,
                        row.ClientData8,
                        row.ClientData9,
                        row.ClientData10
                        );
                }

                //foreach (DataRow row in dt.Rows)
                //{
                //    DataRow addrow = dt_target.NewRow();
                //    addrow[label1_CG_Primkey.Text] = textBox1_CG_Primkey.Text.ToString() + textBox2_CG_Primkey.Text.ToString() + row.Field<string>(comboBox1_CG_Primkey.Text) + textBox3_CG_Primkey.Text.ToString();
                //    addrow[label1_CG_PersonRef.Text] = row.Field<string>(comboBox1_CG_PersonRef.Text);
                //    addrow[label1_CG_ClientName.Text] = comboBox1_CG_ClientName.Text;
                //    addrow[label1_CG_AddedBy.Text] = textBox1_CG_AddedBy.Text;
                //    addrow[label1_CG_AddedDateTime.Text] = textBox1_CG_AddedDateTime.Text;
                //    addrow[label1_CG_Title.Text] = row.Field<string>(comboBox1_CG_Title.Text);
                //    addrow[label1_CG_FirstName.Text] = row.Field<string>(comboBox1_CG_FirstName.Text);
                //    addrow[label1_CG_MiddleName.Text] = row.Field<string>(comboBox1_CG_MiddleName.Text);
                //    addrow[label1_CG_Surname.Text] = row.Field<string>(comboBox1_CG_Surname.Text);
                //    addrow[label1_CG_Salutation.Text] = row.Field<string>(comboBox1_CG_Salutation.Text);
                //    addrow[label1_CG_AddressLine1.Text] = row.Field<string>(comboBox1_CG_AddressLine1.Text);
                //    addrow[label1_CG_AddressLine2.Text] = row.Field<string>(comboBox1_CG_AddressLine2.Text);
                //    addrow[label1_CG_AddressLine3.Text] = row.Field<string>(comboBox1_CG_AddressLine3.Text);
                //    addrow[label1_CG_TownCity.Text] = row.Field<string>(comboBox1_CG_TownCity.Text);
                //    addrow[label1_CG_County.Text] = row.Field<string>(comboBox1_CG_County.Text);
                //    addrow[label1_CG_Postcode.Text] = row.Field<string>(comboBox1_CG_Postcode.Text);
                //    addrow[label1_CG_Country.Text] = row.Field<string>(comboBox1_CG_Country.Text);
                //    addrow[label1_CG_OrganisationName.Text] = row.Field<string>(comboBox1_CG_OrganisationName.Text);
                //    addrow[label1_CG_TelephoneNumber.Text] = row.Field<string>(comboBox1_CG_TelephoneNumber.Text);
                //    addrow[label1_CG_MobileNumber.Text] = row.Field<string>(comboBox1_CG_MobileNumber.Text);
                //    addrow[label1_CG_EmailAddress.Text] = row.Field<string>(comboBox1_CG_EmailAddress.Text);
                //    addrow[label1_CG_AppealCode.Text] = textBox1_CG_AppealCode.Text;
                //    addrow[label1_CG_PackageCode.Text] = row.Field<string>(comboBox1_CG_PackageCode.Text);
                //    addrow[label1_CG_Deceased.Text] = row.Field<string>(comboBox1_CG_Deceased.Text);
                //    addrow[label1_CG_Goneaway.Text] = row.Field<string>(comboBox1_CG_Goneaway.Text);
                //    addrow[label1_CG_NoFurtherCommunication.Text] = row.Field<string>(comboBox1_CG_NoFurtherCommunication.Text);
                //    addrow[label1_CG_PreloadedCAFNumber.Text] = row.Field<string>(comboBox1_CG_PreloadedCAFNumber.Text);
                //    addrow[label1_CG_ColdURN.Text] = row.Field<string>(comboBox1_CG_ColdURN.Text);
                //    addrow[label1_CG_ImportFile.Text] = textBox1_CG_ImportFile.Text;
                //    addrow[label1_CG_RaffleStartNumber.Text] = row.Field<string>(comboBox1_CG_RaffleStartNumber.Text);
                //    addrow[label1_CG_RaffleEndNumber.Text] = row.Field<string>(comboBox1_CG_RaffleEndNumber.Text);
                //    addrow[label1_CG_RecordType.Text] = row.Field<string>(comboBox1_CG_RecordType.Text);
                //    addrow[label1_CG_GiftAid.Text] = row.Field<string>(comboBox1_CG_GiftAid.Text);
                //    addrow[label1_CG_Campaign.Text] = textBox1_CG_Campaign.Text;
                //    addrow[label1_CG_PhonePreference.Text] = row.Field<string>(comboBox1_CG_PhonePreference.Text);
                //    addrow[label1_CG_MailPreference.Text] = row.Field<string>(comboBox1_CG_MailPreference.Text);
                //    addrow[label1_CG_EmailPreference.Text] = row.Field<string>(comboBox1_CG_EmailPreference.Text);
                //    addrow[label1_CG_SMSPreference.Text] = row.Field<string>(comboBox1_CG_SMSPreference.Text);
                //    addrow[label1_CG_ThirdPartyPreference.Text] = row.Field<string>(comboBox1_CG_ThirdPartyPreference.Text);
                //    addrow[label1_CG_Barcode.Text] = row.Field<string>(comboBox1_CG_Barcode.Text) + row.Field<string>(comboBox2_CG_Barcode.Text) + row.Field<string>(comboBox3_CG_Barcode.Text);
                //    addrow[label1_CG_ClientData1.Text] = row.Field<string>(comboBox1_CG_ClientData1.Text);
                //    addrow[label1_CG_ClientData2.Text] = row.Field<string>(comboBox1_CG_ClientData2.Text);
                //    addrow[label1_CG_ClientData3.Text] = row.Field<string>(comboBox1_CG_ClientData3.Text);
                //    addrow[label1_CG_ClientData4.Text] = row.Field<string>(comboBox1_CG_ClientData4.Text);
                //    addrow[label1_CG_ClientData5.Text] = row.Field<string>(comboBox1_CG_ClientData5.Text);
                //    addrow[label1_CG_ClientData6.Text] = row.Field<string>(comboBox1_CG_ClientData6.Text);
                //    addrow[label1_CG_ClientData7.Text] = row.Field<string>(comboBox1_CG_ClientData7.Text);
                //    addrow[label1_CG_ClientData8.Text] = row.Field<string>(comboBox1_CG_ClientData8.Text);
                //    addrow[label1_CG_ClientData9.Text] = row.Field<string>(comboBox1_CG_ClientData9.Text);
                //    addrow[label1_CG_ClientData10.Text] = row.Field<string>(comboBox1_CG_ClientData10.Text);

                //    dt_target.Rows.Add(addrow);
            }
                dataGridView1.DataSource = dt_target;
            
        }
    }
}
