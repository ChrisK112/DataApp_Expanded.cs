namespace DataApp
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.button1_Form2_OK = new System.Windows.Forms.Button();
            this.radioButton1_Form2_Other = new System.Windows.Forms.RadioButton();
            this.textBox1_Form2_Other = new System.Windows.Forms.TextBox();
            this.radioButton1_form2_Comma = new System.Windows.Forms.RadioButton();
            this.radioButton1_Form2_Space = new System.Windows.Forms.RadioButton();
            this.radioButton1_Form2_SemiColon = new System.Windows.Forms.RadioButton();
            this.dataGridView1_Form2 = new System.Windows.Forms.DataGridView();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1_Form2)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.button1_Form2_OK);
            this.groupBox5.Controls.Add(this.radioButton1_Form2_Other);
            this.groupBox5.Controls.Add(this.textBox1_Form2_Other);
            this.groupBox5.Controls.Add(this.radioButton1_form2_Comma);
            this.groupBox5.Controls.Add(this.radioButton1_Form2_Space);
            this.groupBox5.Controls.Add(this.radioButton1_Form2_SemiColon);
            this.groupBox5.Location = new System.Drawing.Point(12, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(534, 52);
            this.groupBox5.TabIndex = 39;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "groupBox5";
            // 
            // button1_Form2_OK
            // 
            this.button1_Form2_OK.Location = new System.Drawing.Point(449, 19);
            this.button1_Form2_OK.Name = "button1_Form2_OK";
            this.button1_Form2_OK.Size = new System.Drawing.Size(75, 23);
            this.button1_Form2_OK.TabIndex = 49;
            this.button1_Form2_OK.Text = "OK";
            this.button1_Form2_OK.UseVisualStyleBackColor = true;
            this.button1_Form2_OK.Click += new System.EventHandler(this.button1_Click);
            // 
            // radioButton1_Form2_Other
            // 
            this.radioButton1_Form2_Other.AutoSize = true;
            this.radioButton1_Form2_Other.Location = new System.Drawing.Point(307, 22);
            this.radioButton1_Form2_Other.Name = "radioButton1_Form2_Other";
            this.radioButton1_Form2_Other.Size = new System.Drawing.Size(51, 17);
            this.radioButton1_Form2_Other.TabIndex = 48;
            this.radioButton1_Form2_Other.Text = "Other";
            this.radioButton1_Form2_Other.UseVisualStyleBackColor = true;
            this.radioButton1_Form2_Other.CheckedChanged += new System.EventHandler(this.radioButton1_Form2_Other_CheckedChanged);
            // 
            // textBox1_Form2_Other
            // 
            this.textBox1_Form2_Other.Location = new System.Drawing.Point(386, 21);
            this.textBox1_Form2_Other.MaxLength = 1;
            this.textBox1_Form2_Other.Name = "textBox1_Form2_Other";
            this.textBox1_Form2_Other.Size = new System.Drawing.Size(35, 20);
            this.textBox1_Form2_Other.TabIndex = 47;
            this.textBox1_Form2_Other.Click += new System.EventHandler(this.textBox1_Form2_Other_Click);
            this.textBox1_Form2_Other.TextChanged += new System.EventHandler(this.textBox1_Form2_Other_TextChanged);
            // 
            // radioButton1_form2_Comma
            // 
            this.radioButton1_form2_Comma.AutoSize = true;
            this.radioButton1_form2_Comma.Location = new System.Drawing.Point(219, 22);
            this.radioButton1_form2_Comma.Name = "radioButton1_form2_Comma";
            this.radioButton1_form2_Comma.Size = new System.Drawing.Size(60, 17);
            this.radioButton1_form2_Comma.TabIndex = 46;
            this.radioButton1_form2_Comma.Text = "Comma";
            this.radioButton1_form2_Comma.UseVisualStyleBackColor = true;
            this.radioButton1_form2_Comma.CheckedChanged += new System.EventHandler(this.radioButton1_form2_Comma_CheckedChanged);
            // 
            // radioButton1_Form2_Space
            // 
            this.radioButton1_Form2_Space.AutoSize = true;
            this.radioButton1_Form2_Space.Location = new System.Drawing.Point(135, 22);
            this.radioButton1_Form2_Space.Name = "radioButton1_Form2_Space";
            this.radioButton1_Form2_Space.Size = new System.Drawing.Size(56, 17);
            this.radioButton1_Form2_Space.TabIndex = 45;
            this.radioButton1_Form2_Space.TabStop = true;
            this.radioButton1_Form2_Space.Text = "Space";
            this.radioButton1_Form2_Space.UseVisualStyleBackColor = true;
            this.radioButton1_Form2_Space.CheckedChanged += new System.EventHandler(this.radioButton1_Form2_Space_CheckedChanged);
            // 
            // radioButton1_Form2_SemiColon
            // 
            this.radioButton1_Form2_SemiColon.AutoSize = true;
            this.radioButton1_Form2_SemiColon.Location = new System.Drawing.Point(32, 22);
            this.radioButton1_Form2_SemiColon.Name = "radioButton1_Form2_SemiColon";
            this.radioButton1_Form2_SemiColon.Size = new System.Drawing.Size(75, 17);
            this.radioButton1_Form2_SemiColon.TabIndex = 44;
            this.radioButton1_Form2_SemiColon.TabStop = true;
            this.radioButton1_Form2_SemiColon.Text = "SemiColon";
            this.radioButton1_Form2_SemiColon.UseVisualStyleBackColor = true;
            this.radioButton1_Form2_SemiColon.CheckedChanged += new System.EventHandler(this.radioButton1_Form2_SemiColon_CheckedChanged);
            // 
            // dataGridView1_Form2
            // 
            this.dataGridView1_Form2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dataGridView1_Form2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1_Form2.Location = new System.Drawing.Point(12, 70);
            this.dataGridView1_Form2.MaximumSize = new System.Drawing.Size(534, 240);
            this.dataGridView1_Form2.MinimumSize = new System.Drawing.Size(534, 240);
            this.dataGridView1_Form2.Name = "dataGridView1_Form2";
            this.dataGridView1_Form2.Size = new System.Drawing.Size(534, 240);
            this.dataGridView1_Form2.TabIndex = 40;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 322);
            this.Controls.Add(this.dataGridView1_Form2);
            this.Controls.Add(this.groupBox5);
            this.MaximumSize = new System.Drawing.Size(574, 361);
            this.MinimumSize = new System.Drawing.Size(574, 361);
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Form2";
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1_Form2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton radioButton1_Form2_Other;
        private System.Windows.Forms.TextBox textBox1_Form2_Other;
        private System.Windows.Forms.RadioButton radioButton1_form2_Comma;
        private System.Windows.Forms.RadioButton radioButton1_Form2_Space;
        private System.Windows.Forms.RadioButton radioButton1_Form2_SemiColon;
        private System.Windows.Forms.DataGridView dataGridView1_Form2;
        private System.Windows.Forms.Button button1_Form2_OK;
    }
}