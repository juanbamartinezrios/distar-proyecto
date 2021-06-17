using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Globalization;

namespace Distar.SEGURIDAD
{
    public partial class FrmRestore : Form
    {
        Distar_EntidadesNegocio.Usuario userLog;
        private Distar_LogicaNegocio.Bitacora bitacoraBL = new Distar_LogicaNegocio.Bitacora();
        private string nombreVol;
        private int contVol = 0;
        MessageBoxButtons buttons;
        /** TEXTO **/
        string lbl1Text = "";
        string lbl2Text = "";
        string btn1Text = "";
        string btn2Text = "";
        string btn3Text = "";
        string restoreSuccessMessage = "";
        string restoreErrMessage = "";

        public FrmRestore(Distar_EntidadesNegocio.Usuario user): this()
        {
            userLog = user;
        }

        public FrmRestore()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog explorerDialog = new System.Windows.Forms.FolderBrowserDialog();
            explorerDialog.ShowNewFolderButton = false;
            if (explorerDialog.ShowDialog() == DialogResult.OK)
            {
                comboBox1.Items.Clear();
                contVol = 0;
                textBox1.Text = explorerDialog.SelectedPath;
                foreach (String str in Directory.GetFiles(explorerDialog.SelectedPath))
                {
                    if (str.ToLower().EndsWith(".bak"))
                    {
                        nombreVol = Path.GetFileNameWithoutExtension(str);
                        nombreVol = nombreVol.Substring(0, nombreVol.Length - 1);
                        contVol += 1;
                    }
                }
                comboBox1.Items.Add(contVol.ToString());
                comboBox1.SelectedIndex = 0;
                button1.Enabled = contVol == 0 ? false : true;
                Environment.SpecialFolder root = explorerDialog.RootFolder;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmRestore_Load(object sender, EventArgs e)
        {
            setLanguaje();
            comboBox1.Enabled = false;
            button1.Enabled = contVol == 0 ? false : true;
            label1.Text = lbl1Text;
            label2.Text = lbl2Text;
            button1.Text = btn1Text;
            button2.Text = btn2Text;
            button3.Text = btn3Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Distar_LogicaNegocio.Services servicesBL = new Distar_LogicaNegocio.Services();
            if (servicesBL.RestaurarCopiaDeSeguridad(textBox1.Text, nombreVol, comboBox1.SelectedItem.ToString()))
            {
                bitacoraBL.setINFO(DateTime.Now, userLog, "Seguridad", "Restauración de copias de seguridad realizado.");
                buttons = MessageBoxButtons.OK;
                Distar.GUIServices.giveMeAlerts(restoreSuccessMessage, "Distar", buttons);
                Close();
            }
            else
            {
                bitacoraBL.setWARNING(DateTime.Now, userLog, "Seguridad", "Restauración de copias de seguridad fallido.");
                buttons = MessageBoxButtons.OK;
                Distar.GUIServices.giveMeAlerts(restoreErrMessage, "Distar", buttons);
            }
        }

        private void setLanguaje()
        {
            lbl1Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Location of copies: " : "Ubicación de las copias: ";
            lbl2Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Volumes: " : "Volúmenes: ";
            btn1Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "ACCEPT" : "ACEPTAR";
            btn2Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "EXAMINE" : "EXAMINAR";
            btn3Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "CANCEL" : "CANCELAR";
            restoreSuccessMessage = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Restoring successful backups." : "Restauración de copias de seguridad exitoso.";
            restoreErrMessage = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Restoring backup copies - ERROR." : "Restauración de copias de seguridad - ERROR.";
            this.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Distar - Restore backup copies" : "Distar - Restaurar copias de seguridad";
            toolTip1.SetToolTip(button1, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Make the restore of backup copies" : "Realizar la restauración de copias de seguridad");
            toolTip1.SetToolTip(button2, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Examine path of the backup files" : "Examinar la ruta de los archivos de copia de seguridad");
        }

        private void toolTip2_Popup(object sender, PopupEventArgs e)
        {

        }
    }
}
