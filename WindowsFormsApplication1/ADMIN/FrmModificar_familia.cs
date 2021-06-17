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
    public partial class FrmModificar_familia : Form
    {
        Distar_EntidadesNegocio.Usuario userLog;
        Distar_EntidadesNegocio.Familia familiaSeleccionada;
        Distar_LogicaNegocio.Familia familiaBL;
        private Distar_LogicaNegocio.Bitacora bitacoraBL = new Distar_LogicaNegocio.Bitacora();
        public event EventHandler<Boolean> FrmModificar_familia_actualizacion_ok;
        string familiaTxt = "";
        string successMsg = "";
        string errMsg = "";

        public FrmModificar_familia()
        {
            InitializeComponent();
        }

        public FrmModificar_familia(Distar_EntidadesNegocio.Usuario user, Distar_EntidadesNegocio.Familia fam): this()
        {
            userLog = user;
            familiaSeleccionada = fam;
        }

        private void FrmModificar_familia_Load(object sender, EventArgs e)
        {
            setLanguaje();
            label2.Text = familiaTxt + familiaSeleccionada.descripcion;
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
                familiaSeleccionada.descripcion = textBox1.Text;
                familiaBL = new Distar_LogicaNegocio.Familia();
                if (familiaBL.update(familiaSeleccionada))
                {
                    bitacoraBL.setINFO(DateTime.Now, userLog, "Administración", "Modificación de Familia.");
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
            FrmModificar_familia_actualizacion_ok.Invoke(this, true);
        }

        private void setLanguaje()
        {
            familiaTxt = Thread.CurrentThread.CurrentCulture.Name == "en-US" ?  "Family to modify: " : "Familia a modificar: ";
            successMsg = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Updated family." : "Familia modificada.";
            errMsg = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Updated family - ERROR" : "Familia modificada - ERROR";
            this.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Distar - Modify Family" : "Distar - Modificar Familia";
            button1.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "ACCEPT" : "ACEPTAR";
            button3.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "CANCEL" : "CANCELAR";
            toolTip1.SetToolTip(button1, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Accept family changes" : "Aceptar cambios de familia");
        }
    }
}
