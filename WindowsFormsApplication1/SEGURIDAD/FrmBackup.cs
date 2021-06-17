using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;

namespace Distar.SEGURIDAD
{
    public partial class FrmBackup : Form
    {
        Distar_EntidadesNegocio.Usuario userLog;
        private Distar_LogicaNegocio.Bitacora bitacoraBL = new Distar_LogicaNegocio.Bitacora();
        MessageBoxButtons buttons;
        /** TEXTO **/
        string lbl1Text = "";
        string lbl3Text = "";
        string lbl2Text = "";
        string btn2Text = "";
        string btn1Text = "";
        string btn3Text = "";
        string backupSuccessMessage = "";
        string backupErrMessage = "";
        string backupAlertMessage = "";

        public FrmBackup(Distar_EntidadesNegocio.Usuario user): this()
        {
            userLog = user;
        }

        public FrmBackup()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog explorerDialog = new System.Windows.Forms.FolderBrowserDialog();
            if (explorerDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = explorerDialog.SelectedPath;
                Environment.SpecialFolder root = explorerDialog.RootFolder;
            }
        }

        private void FrmBackup_Load(object sender, EventArgs e)
        {
            setLanguaje();
            comboBox1.Items.Add("1");
            comboBox1.Items.Add("2");
            comboBox1.Items.Add("3");
            comboBox1.Items.Add("4");
            comboBox1.Items.Add("5");
            label1.Text = lbl1Text;
            label2.Text = lbl2Text;
            label3.Text = lbl3Text;
            button1.Text = btn1Text;
            button2.Text = btn2Text;
            button3.Text = btn3Text;
            button1.Enabled = ((textBox1.Text != null) && (textBox2.Text != null) && (comboBox1.SelectedItem != null)) ? true : false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((textBox1.Text != "") && (textBox2.Text != "") && (comboBox1.SelectedItem != null))
            {
                Distar_LogicaNegocio.Services servicesBL = new Distar_LogicaNegocio.Services();
                if (servicesBL.CrearCopiaDeSeguridad(textBox1.Text, textBox2.Text, comboBox1.SelectedItem.ToString()))
                {
                    bitacoraBL.setINFO(DateTime.Now, userLog, "Seguridad", "Back-up realizado.");
                    buttons = MessageBoxButtons.OK;
                    Distar.GUIServices.giveMeAlerts(backupSuccessMessage, "Distar", buttons);
                    Close();
                }
                else
                {
                    bitacoraBL.setWARNING(DateTime.Now, userLog, "Seguridad", "Back-up fallido.");
                    buttons = MessageBoxButtons.OK;
                    Distar.GUIServices.giveMeAlerts(backupErrMessage, "Distar", buttons);
                }
            }
            else
            {
                buttons = MessageBoxButtons.OK;
                Distar.GUIServices.giveMeAlerts(backupAlertMessage, "Distar", buttons);
            }
        }

        private void setLanguaje()
        {
            lbl1Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Location of copies: " : "Ubicación de las copias: ";
            lbl3Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Name of the copy: " : "Nombre de la copia: ";
            lbl2Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Volumes: " : "Volúmenes: ";
            btn1Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "ACCEPT" : "ACEPTAR";
            btn2Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "EXAMINE" : "EXAMINAR";
            btn3Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "CANCEL" : "CANCELAR";
            backupSuccessMessage = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Generation of successful backups." : "Generación de copias de seguridad exitosa.";
            backupErrMessage = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Generation of backups - Error: You don't have permissions of the S.O for generate Back-up" : "Generación de copias de seguridad - Error: No tiene permisos del S.O para generar la copia de seguridad.";
            backupAlertMessage = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "You must complete all the fields of the form to perform the Back-up." : "Debe completar todos los campos del formulario para realizar el Back-up.";
            this.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Distar - Make security copies" : "Distar - Realizar copias de seguridad";
            toolTip1.SetToolTip(button1, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Generate backup" : "Generar copias de seguridad");
            toolTip1.SetToolTip(button2, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Examine path of the backup files" : "Examinar la ruta de los archivos de copia de seguridad");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = ((textBox1.Text != null) && (textBox2.Text != null) && (comboBox1.SelectedItem != null)) ? true : false;
        }
    }
}
