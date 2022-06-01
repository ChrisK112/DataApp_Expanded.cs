﻿using System;
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
                                            if (item.Name == "comboBox_DataMapper_ClientName")
                                            {
                                                comboBox.BindingContext = new BindingContext();
                                                comboBox.DataSource = new BindingSource(DataHandler.CharityNamesPairs(), null);
                                                comboBox.DisplayMember = "Value";
                                                comboBox.ValueMember = "Key";
                                            }
                                            else
                                            {
                                                comboBox.BindingContext = new BindingContext();
                                                comboBox.DataSource = new BindingSource(DataHandler.colNamesArray(dt_import, true), null);
                                            }
                                        }
                                    }
                                }
                                //comboBox1_CG_duplicates.DataSource = DataHandler.colNamesArray(DataTableFactory.DtScheme(tabPage.Name), true);
                            }
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