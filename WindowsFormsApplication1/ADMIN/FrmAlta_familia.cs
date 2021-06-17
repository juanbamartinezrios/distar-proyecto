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

namespace Distar.ADMIN
{
    public partial class FrmAlta_familia : Form
    {
        Distar_EntidadesNegocio.Usuario userLog;
        Distar_LogicaNegocio.Familia familiaBL;
        private Distar_LogicaNegocio.Bitacora bitacoraBL = new Distar_LogicaNegocio.Bitacora();
        public event EventHandler<Boolean> FrmAlta_familia_alta_ok;
        string familiaTxt = "";
        string successMsg = "";
        string errMsg = "";

        public FrmAlta_familia()
        {
            InitializeComponent();
        }

        public FrmAlta_familia(Distar_EntidadesNegocio.Usuario user): this()
        {
            userLog = user;
        }

        private void FrmAlta_familia_Load(object sender, EventArgs e)
        {
            setLanguaje();
            label2.Text = familiaTxt;
            this.textBox1.Focus();
            this.textBox1.Select();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                Distar_EntidadesNegocio.Familia familia = new Distar_EntidadesNegocio.Familia();
                familia.descripcion = textBox1.Text;
                familiaBL = new Distar_LogicaNegocio.Familia();
                if (familiaBL.create(familia))
                {
                    bitacoraBL.setINFO(DateTime.Now, userLog, "Administración", "Familia creada.");
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    Distar.GUIServices.giveMeAlerts(successMsg, "Distar", buttons);
                    Close();
                    notificarCambios();
                }
                else
                {
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    Distar.GUIServices.giveMeAlerts(errMsg, "Distar", buttons);
                    refreshForm();
                }
            }
            else
            {
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                Distar.GUIServices.giveMeAlerts(Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Please complete the form fields." : "Por favor, complete los campos del formulario.", "Distar", buttons);
            }
        }

        private void refreshForm()
        {
            textBox1.Clear();
        }

        public void notificarCambios()
        {
            FrmAlta_familia_alta_ok.Invoke(this, true);
        }

        private void setLanguaje()
        {
            familiaTxt = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Family name: " : "Nombre de Familia: ";
            successMsg = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Family created." : "Familia creada.";
            errMsg = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Family created - Error" : "Familia creada - Error";
            this.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Distar - New Family" : "Distar - Nueva Familia";
            button1.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "ACCEPT" : "ACEPTAR";
            button3.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "CANCEL" : "CANCELAR";
            toolTip1.SetToolTip(button1, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Create new family" : "Crear nueva familia");
        }
    }
}
