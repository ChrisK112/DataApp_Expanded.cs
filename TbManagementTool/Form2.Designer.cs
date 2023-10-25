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
            this.radio_OverwriteCurrentProfile = new System.Windows.Forms.RadioButton();
            this.radio_SaveAsNewProfile = new System.Windows.Forms.RadioButton();
            this.textBox_SaveAsNewProfile = new System.Windows.Forms.TextBox();
            this.button_SaveProfile = new System.Windows.Forms.Button();
            this.button_CancelProfile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // radio_OverwriteCurrentProfile
            // 
            this.radio_OverwriteCurrentProfile.AutoSize = true;
            this.radio_OverwriteCurrentProfile.Location = new System.Drawing.Point(26, 27);
            this.radio_OverwriteCurrentProfile.Name = "radio_OverwriteCurrentProfile";
            this.radio_OverwriteCurrentProfile.Size = new System.Drawing.Size(156, 19);
            this.radio_OverwriteCurrentProfile.TabIndex = 0;
            this.radio_OverwriteCurrentProfile.TabStop = true;
            this.radio_OverwriteCurrentProfile.Text = "Overwrite Current Profile";
            this.radio_OverwriteCurrentProfile.UseVisualStyleBackColor = true;
            this.radio_OverwriteCurrentProfile.CheckedChanged += new System.EventHandler(this.radio_OverwriteExisting_CheckedChanged);
            // 
            // radio_SaveAsNewProfile
            // 
            this.radio_SaveAsNewProfile.AutoSize = true;
            this.radio_SaveAsNewProfile.Location = new System.Drawing.Point(26, 52);
            this.radio_SaveAsNewProfile.Name = "radio_SaveAsNewProfile";
            this.radio_SaveAsNewProfile.Size = new System.Drawing.Size(129, 19);
            this.radio_SaveAsNewProfile.TabIndex = 1;
            this.radio_SaveAsNewProfile.TabStop = true;
            this.radio_SaveAsNewProfile.Text = "Save As New Profile";
            this.radio_SaveAsNewProfile.UseVisualStyleBackColor = true;
            this.radio_SaveAsNewProfile.CheckedChanged += new System.EventHandler(this.radio_SaveAsNewProfile_CheckedChanged);
            // 
            // textBox_SaveAsNewProfile
            // 
            this.textBox_SaveAsNewProfile.Enabled = false;
            this.textBox_SaveAsNewProfile.Location = new System.Drawing.Point(26, 77);
            this.textBox_SaveAsNewProfile.Name = "textBox_SaveAsNewProfile";
            this.textBox_SaveAsNewProfile.Size = new System.Drawing.Size(138, 23);
            this.textBox_SaveAsNewProfile.TabIndex = 2;
            // 
            // button_SaveProfile
            // 
            this.button_SaveProfile.Enabled = false;
            this.button_SaveProfile.Location = new System.Drawing.Point(195, 104);
            this.button_SaveProfile.Name = "button_SaveProfile";
            this.button_SaveProfile.Size = new System.Drawing.Size(75, 23);
            this.button_SaveProfile.TabIndex = 3;
            this.button_SaveProfile.Text = "Save";
            this.button_SaveProfile.UseVisualStyleBackColor = true;
            this.button_SaveProfile.Click += new System.EventHandler(this.button_SaveProfile_Click);
            // 
            // button_CancelProfile
            // 
            this.button_CancelProfile.Location = new System.Drawing.Point(287, 104);
            this.button_CancelProfile.Name = "button_CancelProfile";
            this.button_CancelProfile.Size = new System.Drawing.Size(75, 23);
            this.button_CancelProfile.TabIndex = 4;
            this.button_CancelProfile.Text = "Cancel";
            this.button_CancelProfile.UseVisualStyleBackColor = true;
            this.button_CancelProfile.Click += new System.EventHandler(this.button_CancelProfile_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 139);
            this.Controls.Add(this.button_CancelProfile);
            this.Controls.Add(this.button_SaveProfile);
            this.Controls.Add(this.textBox_SaveAsNewProfile);
            this.Controls.Add(this.radio_SaveAsNewProfile);
            this.Controls.Add(this.radio_OverwriteCurrentProfile);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.RadioButton radio_OverwriteCurrentProfile;
    private System.Windows.Forms.RadioButton radio_SaveAsNewProfile;
    private System.Windows.Forms.TextBox textBox_SaveAsNewProfile;
    private System.Windows.Forms.Button button_SaveProfile;
    private System.Windows.Forms.Button button_CancelProfile;
}
}