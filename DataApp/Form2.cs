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

namespace DataApp
{
    public partial class Form2 : Form
    {
        public static char delimiterF2;
        public static string qualifierR2;
        public static string fileWithPathF2 = Form1.fileWithPath;
        public static DataTable temp_dt;
        int numberOfRows = 50;
        public static bool dataOk = false;
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Form2_OK_Click(object sender, EventArgs e)
        {

            qualifierR2 = textBox2_Form2_qualifier.Text;

            if (radioButton1_Form2_SemiColon.Checked)
            {
                delimiterF2 = ';';
            }
            else if (radioButton1_Form2_Space.Checked)
            {
                delimiterF2 = ' ';
            }
            else if (radioButton1_form2_Comma.Checked)
            {
                delimiterF2 = ',';
            }
            else if (radioButton1_Form2_Other.Checked)
            {
                if(textBox1_Form2_Other.Text.Length >0)
                {                    
                    delimiterF2 = Convert.ToChar(textBox1_Form2_Other.Text);
                }
                else
                {
                    MessageBox.Show("Please introduce a delimiter or select one");
                }               
            }

            this.Close();
            dataOk = true;
        }

        private void radioButton1_Form2_SemiColon_CheckedChanged(object sender, EventArgs e)
        {
            delimiterF2 = ';';
            if (Form1.dataExport)
            {                
                dataGridView1_Form2.DataSource = DataHandler.lstStrData(Form1.dataLoaded, Form1.dtTarget, Form1.dt, delimiterF2, qualifierR2).ConvertAll(x => new { Value = x });
            }
            else
            {
                temp_dt = DataHandler.FlatToDt(fileWithPathF2, delimiterF2, numberOfRows);
                dataGridView1_Form2.DataSource = temp_dt;
            }
        }

        private void radioButton1_Form2_Space_CheckedChanged(object sender, EventArgs e)
        {
            delimiterF2 = ' ';
            if (Form1.dataExport)
            {
                dataGridView1_Form2.DataSource = DataHandler.lstStrData(Form1.dataLoaded, Form1.dtTarget, Form1.dt, delimiterF2, qualifierR2).ConvertAll(x => new { Value = x });
            }
            else
            {
                temp_dt = DataHandler.FlatToDt(fileWithPathF2, delimiterF2, numberOfRows);
                dataGridView1_Form2.DataSource = temp_dt;
            }
        }

        private void radioButton1_form2_Comma_CheckedChanged(object sender, EventArgs e)
        {
            delimiterF2 = ',';
            if (Form1.dataExport)
            {
                dataGridView1_Form2.DataSource = DataHandler.lstStrData(Form1.dataLoaded, Form1.dtTarget, Form1.dt, delimiterF2, qualifierR2).ConvertAll(x => new { Value = x });
            }
            else
            {
                temp_dt = DataHandler.FlatToDt(fileWithPathF2, delimiterF2, numberOfRows);
                dataGridView1_Form2.DataSource = temp_dt;
            }
        }

        private void radioButton1_Form2_Other_CheckedChanged(object sender, EventArgs e)
        {
            textBox1_Form2_Other.Select();
            if (textBox1_Form2_Other.Text.Length > 0)
            {
                delimiterF2 = Convert.ToChar(textBox1_Form2_Other.Text);
                if (Form1.dataExport)
                {
                    dataGridView1_Form2.DataSource = DataHandler.lstStrData(Form1.dataLoaded, Form1.dtTarget, Form1.dt, delimiterF2, qualifierR2).ConvertAll(x => new { Value = x });
                }
                else
                {
                    temp_dt = DataHandler.FlatToDt(fileWithPathF2, delimiterF2, numberOfRows);
                    dataGridView1_Form2.DataSource = temp_dt;
                }
            }
        }

        private void textBox1_Form2_Other_Click(object sender, EventArgs e)
        {
            radioButton1_Form2_Other.Select();
            textBox1_Form2_Other.Select();
        }

        private void textBox1_Form2_Other_TextChanged(object sender, EventArgs e)
        {
            if (textBox1_Form2_Other.Text.Length > 0)
            {
                radioButton1_Form2_Other.Checked = true;
                delimiterF2 = Convert.ToChar(textBox1_Form2_Other.Text);
                if (Form1.dataExport)
                {
                    dataGridView1_Form2.DataSource = DataHandler.lstStrData(Form1.dataLoaded, Form1.dtTarget, Form1.dt, delimiterF2, qualifierR2).ConvertAll(x => new { Value = x });
                }
                else
                {
                    temp_dt = DataHandler.FlatToDt(fileWithPathF2, delimiterF2, numberOfRows);
                    dataGridView1_Form2.DataSource = temp_dt;
                }
            }
        }

        private void checkBox1_Form2_qualifier_CheckedChanged(object sender, EventArgs e)
        {
            textBox2_Form2_qualifier.Enabled = checkBox1_Form2_qualifier.Checked == true ? true : false;
        }

        private void textBox2_Form2_qualifier_TextChanged(object sender, EventArgs e)
        {
            qualifierR2 = checkBox1_Form2_qualifier.Checked == true ? textBox2_Form2_qualifier.Text : "";
            if (Form1.dataExport)
            {
                dataGridView1_Form2.DataSource = DataHandler.lstStrData(Form1.dataLoaded, Form1.dtTarget, Form1.dt, delimiterF2, qualifierR2).ConvertAll(x => new { Value = x });
            }
        }
    }
}
