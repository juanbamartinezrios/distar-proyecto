namespace Distar.ADMIN
{
    partial class FrmNegarHabilitar_UsuarioPatente
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmNegarHabilitar_UsuarioPatente));
            this.button5 = new System.Windows.Forms.Button();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.HelpProviderHG = new System.Windows.Forms.HelpProvider();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button5
            // 
            this.HelpProviderHG.SetHelpKeyword(this.button5, "FrmNegarHabilitar_UsuarioPatente_1.htm#button5");
            this.HelpProviderHG.SetHelpNavigator(this.button5, System.Windows.Forms.HelpNavigator.Topic);
            this.button5.Location = new System.Drawing.Point(32, 278);
            this.button5.Margin = new System.Windows.Forms.Padding(4);
            this.button5.Name = "button5";
            this.HelpProviderHG.SetShowHelp(this.button5, true);
            this.button5.Size = new System.Drawing.Size(147, 28);
            this.button5.TabIndex = 66;
            this.button5.Text = "ACEPTAR";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.HelpProviderHG.SetHelpKeyword(this.checkedListBox1, "FrmNegarHabilitar_UsuarioPatente_1.htm#checkedListBox1");
            this.HelpProviderHG.SetHelpNavigator(this.checkedListBox1, System.Windows.Forms.HelpNavigator.Topic);
            this.checkedListBox1.Location = new System.Drawing.Point(13, 18);
            this.checkedListBox1.Margin = new System.Windows.Forms.Padding(4);
            this.checkedListBox1.Name = "checkedListBox1";
            this.HelpProviderHG.SetShowHelp(this.checkedListBox1, true);
            this.checkedListBox1.Size = new System.Drawing.Size(312, 225);
            this.checkedListBox1.TabIndex = 68;
            // 
            // button2
            // 
            this.HelpProviderHG.SetHelpKeyword(this.button2, "FrmNegarHabilitar_UsuarioPatente_1.htm#button2");
            this.HelpProviderHG.SetHelpNavigator(this.button2, System.Windows.Forms.HelpNavigator.Topic);
            this.button2.Location = new System.Drawing.Point(199, 278);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.HelpProviderHG.SetShowHelp(this.button2, true);
            this.button2.Size = new System.Drawing.Size(147, 28);
            this.button2.TabIndex = 69;
            this.button2.Text = "SALIR";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkedListBox1);
            this.groupBox1.Location = new System.Drawing.Point(19, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(333, 256);
            this.groupBox1.TabIndex = 70;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "PATENTES";
            // 
            // HelpProviderHG
            // 
            this.HelpProviderHG.HelpNamespace = "Distar.chm";
            // 
            // FrmNegarHabilitar_UsuarioPatente
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 321);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button5);
            this.HelpProviderHG.SetHelpKeyword(this, "FrmNegarHabilitar_UsuarioPatente_1.htm");
            this.HelpProviderHG.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmNegarHabilitar_UsuarioPatente";
            this.HelpProviderHG.SetShowHelp(this, true);
            this.Text = "Distar - Negar/Habilitar patentes de Usuario";
            this.Load += new System.EventHandler(this.FrmNegarHabilitar_UsuarioPatente_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.HelpProvider HelpProviderHG;
    }
}