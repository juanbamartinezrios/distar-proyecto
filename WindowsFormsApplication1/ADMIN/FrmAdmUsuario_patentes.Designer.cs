namespace Distar.ADMIN
{
    partial class FrmAdmUsuario_patentes
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAdmUsuario_patentes));
            this.button3 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.HelpProviderHG = new System.Windows.Forms.HelpProvider();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button3
            // 
            this.HelpProviderHG.SetHelpKeyword(this.button3, "FrmAdmUsuario_patentes_1.htm#button3");
            this.HelpProviderHG.SetHelpNavigator(this.button3, System.Windows.Forms.HelpNavigator.Topic);
            this.button3.Location = new System.Drawing.Point(333, 276);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.HelpProviderHG.SetShowHelp(this.button3, true);
            this.button3.Size = new System.Drawing.Size(153, 28);
            this.button3.TabIndex = 61;
            this.button3.Text = "SALIR";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(75, 17);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 17);
            this.label2.TabIndex = 58;
            this.label2.Text = "xxxx";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 17);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 17);
            this.label1.TabIndex = 57;
            this.label1.Text = "Usuario:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkedListBox1);
            this.groupBox1.Location = new System.Drawing.Point(12, 54);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(475, 209);
            this.groupBox1.TabIndex = 56;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "PATENTES";
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.HelpProviderHG.SetHelpKeyword(this.checkedListBox1, "FrmAdmUsuario_patentes_1.htm#checkedListBox1");
            this.HelpProviderHG.SetHelpNavigator(this.checkedListBox1, System.Windows.Forms.HelpNavigator.Topic);
            this.checkedListBox1.Items.AddRange(new object[] {
            "Patente 1",
            "Patente 2",
            "Patente 3",
            "Patente 4",
            "Patente 5",
            "Patente 6",
            "Patente 7",
            "Patente 8",
            "Patente 9"});
            this.checkedListBox1.Location = new System.Drawing.Point(8, 23);
            this.checkedListBox1.Margin = new System.Windows.Forms.Padding(4);
            this.checkedListBox1.Name = "checkedListBox1";
            this.HelpProviderHG.SetShowHelp(this.checkedListBox1, true);
            this.checkedListBox1.Size = new System.Drawing.Size(455, 157);
            this.checkedListBox1.TabIndex = 56;
            this.checkedListBox1.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBox1_ItemCheck);
            // 
            // button1
            // 
            this.HelpProviderHG.SetHelpKeyword(this.button1, "FrmAdmUsuario_patentes_1.htm#button1");
            this.HelpProviderHG.SetHelpNavigator(this.button1, System.Windows.Forms.HelpNavigator.Topic);
            this.button1.Location = new System.Drawing.Point(173, 276);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.HelpProviderHG.SetShowHelp(this.button1, true);
            this.button1.Size = new System.Drawing.Size(153, 28);
            this.button1.TabIndex = 65;
            this.button1.Text = "NEGAR/HABILITAR";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button5
            // 
            this.HelpProviderHG.SetHelpKeyword(this.button5, "FrmAdmUsuario_patentes_1.htm#button5");
            this.HelpProviderHG.SetHelpNavigator(this.button5, System.Windows.Forms.HelpNavigator.Topic);
            this.button5.Location = new System.Drawing.Point(12, 276);
            this.button5.Margin = new System.Windows.Forms.Padding(4);
            this.button5.Name = "button5";
            this.HelpProviderHG.SetShowHelp(this.button5, true);
            this.button5.Size = new System.Drawing.Size(153, 28);
            this.button5.TabIndex = 63;
            this.button5.Text = "ASIGNAR/QUITAR";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // HelpProviderHG
            // 
            this.HelpProviderHG.HelpNamespace = "Distar.chm";
            // 
            // FrmAdmUsuario_patentes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(500, 321);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.HelpProviderHG.SetHelpKeyword(this, "FrmAdmUsuario_patentes_1.htm");
            this.HelpProviderHG.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAdmUsuario_patentes";
            this.HelpProviderHG.SetShowHelp(this, true);
            this.Text = "Distar - Administrar patentes de Usuario";
            this.Load += new System.EventHandler(this.FrmAdmUsuario_patentes_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.HelpProvider HelpProviderHG;
    }
}