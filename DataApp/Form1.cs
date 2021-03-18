using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
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

        //GLOBAL VARIABLES
        OpenFileDialog ofd = new OpenFileDialog();
        public static string sourceFile;
        public static string fileName;
        public DataTable dt;
        DataTable dtTarget = new DataTable();
        DataTable dtUniqueValues;
        char delimiterF1;
        bool dataLoaded, dataUnique = false;

        


        private void button1_Form1_search_Click(object sender, EventArgs e)
        {
            ofd.Filter = "Text(Tab delimited) (*.txt) |*.txt|CSV (Comma delimited) (*.csv) |*.csv";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                sourceFile = ofd.FileName;
                textBox1_Form1_filePath.Text = sourceFile;
                fileName = ofd.SafeFileName;
                Form2.filepath = sourceFile;
            }
        }

        private void button1_Form1_import_Click(object sender, EventArgs e)
        {
            try
            {
                Form2 form2 = new Form2();
                form2.ShowDialog();
                delimiterF1 = Form2.delimiter_f2;
                dt = DataHandler.FlatToDataTable(sourceFile, delimiterF1);
                DataHandler.addOrDeleteEmptyColumn(ref dt);
                DataTable dtFirst20 = dt.AsEnumerable().Take(20).CopyToDataTable();
                
                //DATA SOURCES TO GRID AND COMBOBOXES
                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = dtFirst20;
                if (dataGridView1.Columns.Count < 13)
                {
                    dataGridView1.AutoSize = true;
                }

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
                                        comboBox.DataSource = DataHandler.columnNamesWithExtraEmptyRow(dtFirst20);
                                        comboBox.SelectedIndex = 0;
                                    }
                                }
                            }
                        }
                    }

                }

                //DEFAULT VALUES
                textBox3_CG_Primkey.Text = DateTime.Now.ToString("dd/MM/yyyy");
                textBox1_CG_AddedDateTime.Text = DateTime.Now.ToString("dd/MM/yyyy");
                textBox1_CG_AddedBy.Text = "Admin";
                textBox1_CG_Primkey.Text = "";
                label2_CG_RowsImported.Text = dt.Rows.Count.ToString();
                radioButton1_CG_RecordTypeWarm.Checked = true;

                //CLEAN GLOBAL VALUES
                sourceFile = null;
                fileName = null;
                textBox1_Form1_filePath.Text = sourceFile;
                Form2.filepath = null;                
            }

            catch (System.ArgumentNullException)
            {
                MessageBox.Show("Please select a file");
            }
            catch (System.InvalidOperationException)
            {
                MessageBox.Show("The file you try to import does not contain any row");
            }
            catch(Exception)
            {
                MessageBox.Show("There is an issue with your file");
            }
            
        }

        private void textBox1_CG_AppealCode_TextChanged(object sender, EventArgs e)
        {
            textBox1_CG_ImportFile.Text = textBox1_CG_AppealCode.Text;
            textBox2_CG_Primkey.Text = textBox1_CG_AppealCode.Text;
            textBox1_CG_Barcode.Text = textBox1_CG_AppealCode.Text;
        }

        private void comboBox1_CG_ClientName_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1_CG_Primkey.Text = comboBox1_CG_ClientName.SelectedValue.ToString();
        }        

        private void button1_Form1_load_Click(object sender, EventArgs e)
        {
            dataLoaded = true;
            TabPage tabPage = tabControl1.SelectedTab;

            //COMMITTED GIVING
            if (tabControl1.SelectedTab == CG_tabPage1)
            {
                string recordtype;
                if (radioButton1_CG_RecordTypeWarm.Checked)
                {
                    recordtype = radioButton1_CG_RecordTypeWarm.Text;
                }
                else
                {
                    recordtype = radioButton2_RecordTypeCold.Text;
                }


                dtTarget = DataTableFactory.DtScheme(CG_tabPage1.Name);
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
                        AddressLine3 = row.Field<string>(comboBox1_CG_AddressLine3.Text) + " " + row.Field<string>(comboBox1_CG_AddressLine4.Text) + " " + row.Field<string>(comboBox1_CG_AddressLine5.Text),
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
                        Deceased = 0,
                        Goneaway = 0,
                        NoFurtherCommunication = 0,
                        PreloadedCAFNumber = row.Field<string>(comboBox1_CG_PreloadedCAFNumber.Text),
                        ColdURN = row.Field<string>(comboBox1_CG_ColdURN.Text),
                        ImportFile = textBox1_CG_ImportFile.Text,
                        RaffleStartNumber = row.Field<string>(comboBox1_CG_RaffleStartNumber.Text),
                        RaffleEndNumber = row.Field<string>(comboBox1_CG_RaffleEndNumber.Text),
                        RecordType = recordtype,
                        GiftAid = "Unknown",
                        Campaign = textBox1_CG_Campaign.Text,
                        PhonePreference = "Unknown",
                        MailPreference = "Unknown",
                        EmailPreference = "Unknown",
                        SMSPreference = "Unknown",
                        ThirdPartyPreference = "Unknown",
                        Barcode = textBox1_CG_Barcode.Text + textBox2_CG_Barcode.Text + row.Field<string>(comboBox2_CG_Barcode.Text) + textBox2_CG_Barcode.Text + row.Field<string>(comboBox3_CG_Barcode.Text),
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
                    dtTarget.Rows.Add(
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
            }
            
            dtUniqueValues = new DataTable();

            if (checkBox1_CG_duplicates.Checked)
            {
                dtUniqueValues = dtTarget.AsEnumerable().GroupBy(x => x.Field<string>("Primkey")).Select(y => y.First()).CopyToDataTable();
                dataGridView1.DataSource = dtUniqueValues;
            }           
            
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            textBox1_Form1_filePath.Enabled = true;
        }

        private void button1_Form1_save_Click(object sender, EventArgs e)
        {            
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Text(Tab delimited) (*.txt) |*.txt| CSV (Comma delimited) (*.csv) |*.csv";
                sfd.InitialDirectory = Path.GetDirectoryName(sourceFile);
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string saveas = sfd.FileName;
                    int extentionindex = sfd.FilterIndex;

                    if (dataLoaded && dataUnique)
                    {
                        DataHandler.DataTableToFlatFile(dtUniqueValues, saveas, extentionindex);
                    }
                    if (dataLoaded)
                    {
                        DataHandler.DataTableToFlatFile(dtTarget, saveas, extentionindex);
                    }
                    else
                    {
                        DataHandler.addOrDeleteEmptyColumn(ref dt);
                        DataHandler.DataTableToFlatFile(dt, saveas, extentionindex);
                    }
                    
                }
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WordHandler.wordtest(dt);
        }

    }
}
