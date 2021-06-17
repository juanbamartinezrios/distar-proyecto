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
    public partial class FrmFamilias : Form
    {
        Distar_LogicaNegocio.Familia familiaBL;
        Distar_EntidadesNegocio.Familia familiaSeleccionada;
        Distar_EntidadesNegocio.Usuario userLog;
        List<Distar_EntidadesNegocio.Familia> lista_familias;
        private Distar_LogicaNegocio.Bitacora bitacoraBL = new Distar_LogicaNegocio.Bitacora();
        public event EventHandler<Boolean> FrmAdmFamilia_actualizacion_ok;
        DialogResult res;
        MessageBoxButtons buttons;
        /** TEXTO **/
        string btn2Text = "";
        string btn1Text = "";
        string btn3Text = "";
        string btn4Text = "";
        string btn5Text = "";
        string SuccessMessage = "";
        string ErrMessage = "";
        string AlertMessage = "";
        string questMessage = "";

        public FrmFamilias()
        {
            InitializeComponent();
        }

        public FrmFamilias(Distar_EntidadesNegocio.Usuario user): this()
        {
            userLog = user;
        }

        private void FrmFamilias_Load(object sender, EventArgs e)
        {
            setLanguaje();
            button1.Text = btn1Text;
            button2.Text = btn2Text;
            button3.Text = btn3Text;
            button4.Text = btn4Text;
            button5.Text = btn5Text;
            getListaFamilias();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (familiaSeleccionada != null)
            {
                buttons = MessageBoxButtons.YesNo;
                res = Distar.GUIServices.giveMeAlertsWithAction(questMessage, "Distar", buttons);
                if (res == DialogResult.Yes)
                {
                    familiaBL = new Distar_LogicaNegocio.Familia();
                    if (familiaBL.delete(familiaSeleccionada))
                    {
                        bitacoraBL.setWARNING(DateTime.Now, userLog, "Administración", "Baja de Familia.");
                        buttons = MessageBoxButtons.OK;
                        Distar.GUIServices.giveMeAlerts(SuccessMessage, "Distar", buttons);
                        getListaFamilias();
                    }
                    else
                    {
                        buttons = MessageBoxButtons.OK;
                        Distar.GUIServices.giveMeAlerts(ErrMessage, "Distar", buttons);
                    }
                }
            }
            else
            {
                buttons = MessageBoxButtons.OK;
                Distar.GUIServices.giveMeAlerts(AlertMessage, "Distar", buttons);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (familiaSeleccionada != null)
            {
                Distar.ADMIN.FrmModificar_familia _FrmModificar_familia = new Distar.ADMIN.FrmModificar_familia(userLog, familiaSeleccionada);
                _FrmModificar_familia.Show();
                _FrmModificar_familia.FrmModificar_familia_actualizacion_ok += this.OnFrmModificar_familia;
            }
            else
            {
                buttons = MessageBoxButtons.OK;
                Distar.GUIServices.giveMeAlerts(AlertMessage, "Distar", buttons);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Distar.ADMIN.FrmAlta_familia _FrmAlta_familia = new Distar.ADMIN.FrmAlta_familia(userLog);
            _FrmAlta_familia.Show();
            _FrmAlta_familia.FrmAlta_familia_alta_ok += this.OnFrmAlta_familia;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (familiaSeleccionada != null)
            {
                Distar.ADMIN.FrmAdmFamilia _FrmAdmFamilia = new Distar.ADMIN.FrmAdmFamilia(userLog, familiaSeleccionada);
                _FrmAdmFamilia.Show();
                _FrmAdmFamilia.FrmAdmFamilia_patentes_asignar_quitar_ok += this.OnFrmAdmFamilia;
            }
            else
            {
                buttons = MessageBoxButtons.OK;
                Distar.GUIServices.giveMeAlerts(AlertMessage, "Distar", buttons);
            }
        }

        private void getListaFamilias()
        {
            listBox1.Items.Clear();
            listBox1.DisplayMember = "descripcion";
            familiaSeleccionada = null;
            familiaBL = new Distar_LogicaNegocio.Familia();
            lista_familias = familiaBL.getAllFamilias();
            foreach (Distar_EntidadesNegocio.Familia familia in lista_familias)
            {
                listBox1.Items.Add(familia);
            }
        }
        public void notificarCambios()
        {
            FrmAdmFamilia_actualizacion_ok.Invoke(this, true);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            familiaSeleccionada = (Distar_EntidadesNegocio.Familia) listBox1.SelectedItem;
        }

        private void OnFrmAlta_familia(object sender, Boolean flag)
        {
            getListaFamilias();
        }

        private void OnFrmModificar_familia(object sender, Boolean flag)
        {
            getListaFamilias();
        }

        private void OnFrmAdmFamilia(object sender, Boolean flag)
        {
            notificarCambios();
            getListaFamilias();
        }

        private void setLanguaje()
        {
            btn1Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "CREATE" : "CREAR";
            btn2Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "MODIFY" : "MODIFICAR";
            btn3Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "DELETE" : "ELIMINAR";
            btn4Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "CLOSE" : "SALIR";
            btn5Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "ADM. PATENTS" : "ADM. PATENTES";
            SuccessMessage = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Family deleted." : "Familia dada de baja.";
            questMessage = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Do you want to unsubscribe the selected family?" : "Desea dar de baja la familia seleccionada?";
            ErrMessage = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Family deleted - Error" : "Familia dada de baja - Error";
            AlertMessage = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "You must select a Family from the list." : "Debe seleccionar una Familia del listado.";
            this.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Distar - Families" : "Distar - Familias";
            toolTip1.SetToolTip(button1, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Create new family" : "Crear nueva familia");
            toolTip1.SetToolTip(button2, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Modify selected family" : "Modificar familia seleccionada");
            toolTip1.SetToolTip(button3, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Delete selected family" : "Eliminarfamilia seleccionada");
            toolTip1.SetToolTip(button5, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Administer patents of selected family" : "Administrar patentes de familia seleccionada");
        }
    }
}
