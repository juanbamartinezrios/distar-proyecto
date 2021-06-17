namespace Distar.ADMIN
{
    partial class FrmPatentes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPatentes));
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.HelpProviderHG = new System.Windows.Forms.HelpProvider();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.HelpProviderHG.SetHelpKeyword(this.listBox1, "FrmPatentes_1.htm#listBox1");
            this.HelpProviderHG.SetHelpNavigator(this.listBox1, System.Windows.Forms.HelpNavigator.Topic);
            this.listBox1.ItemHeight = 16;
            this.listBox1.Items.AddRange(new object[] {
            "Patente 1",
            "Patente 2",
            "Patente 3",
            "Patente 4",
            "Patente 5"});
            this.listBox1.Location = new System.Drawing.Point(19, 15);
            this.listBox1.Margin = new System.Windows.Forms.Padding(4);
            this.listBox1.Name = "listBox1";
            this.HelpProviderHG.SetShowHelp(this.listBox1, true);
            this.listBox1.Size = new System.Drawing.Size(479, 228);
            this.listBox1.TabIndex = 10;
            // 
            // HelpProviderHG
            // 
            this.HelpProviderHG.HelpNamespace = "Distar.chm";
            // 
            // FrmPatentes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(519, 260);
            this.Controls.Add(this.listBox1);
            this.HelpProviderHG.SetHelpKeyword(this, "FrmPatentes_1.htm");
            this.HelpProviderHG.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmPatentes";
            this.HelpProviderHG.SetShowHelp(this, true);
            this.Text = "Distar - Patentes";
            this.Load += new System.EventHandler(this.FrmPatentes_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.HelpProvider HelpProviderHG;
    }
}