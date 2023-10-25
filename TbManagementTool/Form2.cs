using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DataApp
{
    public partial class Form2 : Form
    {
        string currentProfile;
        IEnumerable<Control> comboBoxList;
        string currentBarcodeDelimiter;
        string clientName;
        public string newProfile;

        public Form2(string currentProfile, IEnumerable<Control> comboBoxList, string currentBarcodeDelimiter, string clientName)
        {
            InitializeComponent();

            this.currentProfile= currentProfile;
            this.comboBoxList= comboBoxList;
            this.currentBarcodeDelimiter= currentBarcodeDelimiter;
            this.clientName = clientName;
            this.newProfile = "";

        }

        private void button_SaveProfile_Click(object sender, EventArgs e)
        {
            if (this.radio_OverwriteCurrentProfile.Checked) overwriteCurrentProfile();
            if (this.radio_SaveAsNewProfile.Checked) saveNewProfile();
        }
        private void radio_OverwriteExisting_CheckedChanged(object sender, EventArgs e)
        {
            this.textBox_SaveAsNewProfile.Enabled = false;
            this.button_SaveProfile.Enabled = true;

        }
        private void radio_SaveAsNewProfile_CheckedChanged(object sender, EventArgs e)
        {
            this.textBox_SaveAsNewProfile.Enabled = true;
            this.button_SaveProfile.Enabled = true;

        }
        private void button_CancelProfile_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void overwriteCurrentProfile()
        {
            //overwrite currently active profile
            if (currentProfile.Equals("") || currentProfile == null)
            {
                MessageBox.Show("New profile name empty!");
                return;
            }
            //delete current file
            File.Delete("Profiles\\" + this.clientName + "\\" + this.currentProfile + ".xml");

            //save new
            if (buildXML(this.currentProfile))
            {
                closeForm(currentProfile);
                
            }
        }

        private void saveNewProfile()
        {
            //get name of new profile
            string newProfileName = textBox_SaveAsNewProfile.Text;

            //check if its empty
            if(newProfileName.Equals(""))
            {
                MessageBox.Show("New profile name empty!");
                return;
            }
            //check if it already exists
            if (checkIfProfileExists(newProfileName))
            {
                MessageBox.Show("Profile name already exists for client!");
                return;
            }

            /*
            foreach (System.Windows.Forms.ComboBox comboBox in comboBoxLst)
            {
                comboBoxList.Add(new KeyValuePair<string, System.Windows.Forms.ComboBox>(comboBox.Name, comboBox));
            }              
            */

            //build xml
            if (buildXML(newProfileName))
            {
                closeForm(newProfileName);
            }

        }

        private void closeForm(string sendBack) 
        {
            this.newProfile = sendBack;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private bool checkIfProfileExists(string toCheck)
        {

            DirectoryInfo di = new DirectoryInfo("Profiles\\" + this.clientName + "\\");
            FileInfo[] files = di.GetFiles("*.xml");

            foreach (FileInfo file in files)
            {
                string filename = Path.GetFileNameWithoutExtension(file.Name);
                if (filename.Equals(toCheck)) return true;
            }

            return false;


        }
        
        //TOTO
        private bool buildXML(string newProfileName)
        {
            List<XElement> columns = new List<XElement>();


            XElement xprofile = new XElement("profile");

            foreach (System.Windows.Forms.ComboBox comboBox in comboBoxList)
            {
                //skip unwanted fields
                string comboBoxName = comboBox.Name.ToString();
                if (comboBoxName.Equals("profileBox")) continue;
                if (comboBoxName.Equals("comboBox_DataMapper_RemoveDuplicate")) continue;
                if (comboBoxName.Equals("comboBox_DataMapper_ClientName")) continue;
                if (comboBox.Text.Equals("")) continue;
                xprofile.Add(new XElement("column", new XAttribute("name", comboBox.Name.Substring("comboBox_DataMapper_".Length)), new XAttribute("value", comboBox.Text.ToString())));
            }

            //barcode
            xprofile.Add(new XElement("column", new XAttribute("name", "BarcodeDelimiter"), new XAttribute("value", this.currentBarcodeDelimiter)));

            XDocument xmlDoc = new XDocument(new XDeclaration("1.0", "utf-8", null));
            xmlDoc.Add(xprofile);
            xmlDoc.Save("Profiles\\" + this.clientName + "\\" + newProfileName + ".xml");

            //reset clientname to fire event
            /*
            foreach (System.Windows.Forms.ComboBox comboBox in comboBoxList)
            {
                string comboBoxName = comboBox.Name.ToString();
                if (!comboBoxName.Equals("comboBox_DataMapper_ClientName")) continue;
                comboBox.Text = this.clientName;
                
            }
            */
            return true;
 
        }

    }
}
