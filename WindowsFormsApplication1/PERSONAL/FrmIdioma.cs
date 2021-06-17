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

namespace Distar.PERSONAL
{
    public partial class FrmIdioma : Form
    {
        Distar_EntidadesNegocio.Usuario userLog;
        Distar_EntidadesNegocio.Idioma idiomaSeleccionado;
        Distar_LogicaNegocio.Idioma idiomaBL;
        Distar_LogicaNegocio.Usuario usuarioBL;
        List<Distar_EntidadesNegocio.Idioma> lista_idiomas = new List<Distar_EntidadesNegocio.Idioma>();
        private Distar_LogicaNegocio.Bitacora bitacoraBL = new Distar_LogicaNegocio.Bitacora();
        MessageBoxButtons buttons;
        public event EventHandler<Boolean> FrmIdioma_actualizacion_ok;
        /** TEXTO **/
        string lbl2Text = "";
        string btn1Text = "";
        string btn3Text = "";
        string idiomaSuccessMessage = "";
        string idiomaErrMessage = "";

        public FrmIdioma(Distar_EntidadesNegocio.Usuario user): this()
        {
            userLog = user;
        }

        public FrmIdioma()
        {
            InitializeComponent();
        }

        private void FrmIdioma_Load(object sender, EventArgs e)
        {
            setLanguaje();
            label2.Text = lbl2Text;
            button1.Text = btn1Text;
            button3.Text = btn3Text;
            this.comboBox1.Focus();
            this.comboBox1.Select();
            getListaIdiomas();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void getListaIdiomas()
        {
            comboBox1.Items.Clear();
            comboBox1.DisplayMember = "descripcion";
            idiomaSeleccionado = null;
            idiomaBL = new Distar_LogicaNegocio.Idioma();
            lista_idiomas = idiomaBL.getAllIdiomas();
            foreach (Distar_EntidadesNegocio.Idioma idioma in lista_idiomas)
            {
                comboBox1.Items.Add(idioma);
            }
            foreach (Distar_EntidadesNegocio.Idioma idioma in lista_idiomas)
            {
                if (idioma.id_idioma == userLog.id_idioma_usuario)
                {
                    idiomaSeleccionado = idioma;
                }
            }
            comboBox1.SelectedItem = idiomaSeleccionado;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            usuarioBL = new Distar_LogicaNegocio.Usuario();
            if (usuarioBL.cambiarIdiomaUsuario(userLog.id_usuario, idiomaSeleccionado.id_idioma))
            {
                bitacoraBL.setINFO(DateTime.Now, userLog, "Personal", "Cambio de idioma.");
                buttons = MessageBoxButtons.OK;
                Distar.GUIServices.giveMeAlerts(idiomaSuccessMessage, "Distar", buttons);
                Close();
                notificarCambios();
            }
            else
            {
                buttons = MessageBoxButtons.OK;
                Distar.GUIServices.giveMeAlerts(idiomaErrMessage, "Distar", buttons);
            }
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            idiomaSeleccionado = (Distar_EntidadesNegocio.Idioma) comboBox1.SelectedItem;
        }

        public void notificarCambios()
        {
            FrmIdioma_actualizacion_ok.Invoke(this, true);
        }

        private void setLanguaje()
        {
            lbl2Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Select a language from the list:" : "Seleccione un idioma de la lista:";
            btn1Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "ACCEPT" : "ACEPTAR";
            btn3Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "CANCEL" : "CANCELAR";
            idiomaSuccessMessage = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "System Language has been changed. The interface will be updated in the next login." : "Se ha cambiado el Idioma del Sistema. Se actualizará la interfaz en el siguiente login.";
            idiomaErrMessage = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "An error occurred while changing the System Language." : "Se ha producido un error al cambiar el Idioma del Sistema.";
            this.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Distar - Language" : "Distar - Idioma";
            toolTip1.SetToolTip(button1, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Change the language of the System" : "Cambiar el lenguaje del Sistema");
        }
    }
}
