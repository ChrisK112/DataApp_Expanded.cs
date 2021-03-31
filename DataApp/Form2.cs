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
        public static char delimiter_f2;
        public static string qualifier_f2;
        public static string filepath = Form1.sourceFile;
        public static DataTable temp_dt;
        int numberOfRows = 50;
        public static bool dataOk = false;
        public Form2()
        {
            InitializeComponent();
            temp_dt = DataHandler.FlatToDt(filepath, maxrownumber: numberOfRows);
            dataGridView1_Form2.DataSource = temp_dt;
        }

        private void button1_Form2_OK_Click(object sender, EventArgs e)
        {
            qualifier_f2 = textBox2_Form2_qualifier.Text;

            if (radioButton1_Form2_SemiColon.Checked)
            {
                delimiter_f2 = ';';
                this.Close();
            }
            else if (radioButton1_Form2_Space.Checked)
            {
                delimiter_f2 = ' ';
                this.Close();
            }
            else if (radioButton1_form2_Comma.Checked)
            {
                delimiter_f2 = ',';
                this.Close();
            }
            else if (radioButton1_Form2_Other.Checked)
            {
                if(textBox1_Form2_Other.Text.Length <1)
                {
                    MessageBox.Show("Please introduce a delimiter or select one");
                }
                else
                {
                    delimiter_f2 = Convert.ToChar(textBox1_Form2_Other.Text);
                    this.Close();
                }               
            }
            dataOk = true;
        }

        private void radioButton1_Form2_SemiColon_CheckedChanged(object sender, EventArgs e)
        {
            delimiter_f2 = ';';
            temp_dt = DataHandler.FlatToDt(filepath, delimiter_f2, numberOfRows);
            dataGridView1_Form2.DataSource = temp_dt;
        }

        private void radioButton1_Form2_Space_CheckedChanged(object sender, EventArgs e)
        {
            delimiter_f2 = ' ';
            temp_dt = DataHandler.FlatToDt(filepath, delimiter_f2, numberOfRows);
            dataGridView1_Form2.DataSource = temp_dt;
        }

        private void radioButton1_form2_Comma_CheckedChanged(object sender, EventArgs e)
        {
            delimiter_f2 = ',';
            temp_dt = DataHandler.FlatToDt(filepath, delimiter_f2, numberOfRows);
            dataGridView1_Form2.DataSource = temp_dt;
            dataGridView1_Form2.AutoSize = true;
        }

        private void radioButton1_Form2_Other_CheckedChanged(object sender, EventArgs e)
        {
            textBox1_Form2_Other.Select();
            if (textBox1_Form2_Other.Text.Length > 0)
            {
                delimiter_f2 = Convert.ToChar(textBox1_Form2_Other.Text);
                temp_dt = DataHandler.FlatToDt(Form1.sourceFile, delimiter_f2, numberOfRows);
                dataGridView1_Form2.DataSource = temp_dt;
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
                delimiter_f2 = Convert.ToChar(textBox1_Form2_Other.Text);
                temp_dt = DataHandler.FlatToDt(Form1.sourceFile, delimiter_f2, numberOfRows);
                dataGridView1_Form2.DataSource = temp_dt;
            }
        }

        private void checkBox1_Form2_qualifier_CheckedChanged(object sender, EventArgs e)
        {
            textBox2_Form2_qualifier.Enabled = checkBox1_Form2_qualifier.Checked == true ? true : false;
        }
    }
}
