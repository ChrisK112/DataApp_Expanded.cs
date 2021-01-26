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
        public static string sourceFile;
        public static string fileName;
        public DataTable dt;
        public DataTable dt_target = new DataTable();
        char delimiter_f1;
        

        //Browse File
        private void button1_Form1_search_Click(object sender, EventArgs e)
        {
            textBox1_Form1_filePath.Clear();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                sourceFile = ofd.FileName;
                textBox1_Form1_filePath.Text = sourceFile;
                Form2 form2 = new Form2();
                form2.ShowDialog();
                fileName = ofd.SafeFileName;
            }
            //Select delimiter
            
        }

        private void button1_Form1_import_Click(object sender, EventArgs e)
        {
            try
            {
                delimiter_f1 = Form2.delimiter_f2;
                dt = DataHandler.FlatToDataTable(sourceFile, delimiter_f1);
                dt.Columns.Add("                -", typeof(string));
                DataTable temp_dt = dt.AsEnumerable().Take(20).CopyToDataTable();

                //Comboboxes
                string[] colNames = temp_dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();
                string[] colNamesWithBlank = new string[temp_dt.Columns.Count + 1];
                colNamesWithBlank[0] = "                -";/*Add an extra empty option to the list*/
                for(int n = 1; n < temp_dt.Columns.Count; n++)
                {
                    colNamesWithBlank[n] = colNames[n - 1];
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

                    //Committed Giving
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
                                        comboBox.DataSource = new BindingSource(CharityNamesPairs, null);
                                        comboBox.DisplayMember = "Value";
                                        comboBox.ValueMember = "Key";
                                    }
                                    else
                                    {
                                        comboBox.BindingContext = new BindingContext();
                                        comboBox.DataSource = colNamesWithBlank;
                                        comboBox.SelectedIndex = 0;
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

        private void button1_Form1_load_Click(object sender, EventArgs e)
        {
            TabPage tabPage = tabControl1.SelectedTab;

            //Committed Giving
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
                        //Deceased = row.Field<string>(comboBox1_CG_Deceased.Text),
                        //Goneaway = row.Field<string>(comboBox1_CG_Goneaway.Text),
                        //NoFurtherCommunication = row.Field<string>(comboBox1_CG_NoFurtherCommunication.Text),
                        PreloadedCAFNumber = row.Field<string>(comboBox1_CG_PreloadedCAFNumber.Text),
                        ColdURN = row.Field<string>(comboBox1_CG_ColdURN.Text),
                        ImportFile = textBox1_CG_ImportFile.Text,
                        RaffleStartNumber = row.Field<string>(comboBox1_CG_RaffleStartNumber.Text),
                        RaffleEndNumber = row.Field<string>(comboBox1_CG_RaffleEndNumber.Text),
                        RecordType = row.Field<string>(comboBox1_CG_RecordType.Text),
                        GiftAid = row.Field<string>(comboBox1_CG_GiftAid.Text),
                        Campaign = textBox1_CG_Campaign.Text,
                        //PhonePreference = row.Field<string>(comboBox1_CG_PhonePreference.Text),
                        //MailPreference = row.Field<string>(comboBox1_CG_MailPreference.Text),
                        //EmailPreference = row.Field<string>(comboBox1_CG_EmailPreference.Text),
                        //SMSPreference = row.Field<string>(comboBox1_CG_SMSPreference.Text),
                        //ThirdPartyPreference = row.Field<string>(comboBox1_CG_ThirdPartyPreference.Text),
                        Barcode = row.Field<string>(comboBox1_CG_Barcode.Text) + textBox1_CG_Barcode.Text + row.Field<string>(comboBox2_CG_Barcode.Text) + textBox1_CG_Barcode.Text + row.Field<string>(comboBox3_CG_Barcode.Text),
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
                        /*row.Deceased*/ "0",
                        /*row.Goneaway*/ "0",
                        /*row.NoFurtherCommunication*/ "0",
                        row.PreloadedCAFNumber,
                        row.ColdURN,
                        row.ImportFile,
                        row.RaffleStartNumber,
                        row.RaffleEndNumber,
                        row.RecordType,
                        row.GiftAid,
                        row.Campaign,
                        /*row.PhonePreference*/ "Unknown",
                        /*row.MailPreference*/ "Unknown",
                        /*row.EmailPreference*/ "Unknown",
                        /*row.SMSPreference*/ "Unknown",
                        /*row.ThirdPartyPreference*/ "Unknown",
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
            }
                dataGridView1.DataSource = dt_target;            
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            textBox1_Form1_filePath.Enabled = true;
        }

        private void button1_Form1_save_Click(object sender, EventArgs e)
        {            
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Text(Tab delimited) (*.txt) | *.txt | CSV (Comma delimited) (*.csv) | *.csv ";
                sfd.InitialDirectory = Path.GetDirectoryName(sourceFile);
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string saveas = sfd.FileName;
                    int extentionindex = sfd.FilterIndex;

                    DataHandler.DataTableToFlatFile(dt_target, saveas, extentionindex);
                }
            }
            
        }

        
    }
}
