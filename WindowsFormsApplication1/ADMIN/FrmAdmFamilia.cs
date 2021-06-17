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
    public partial class FrmAdmFamilia : Form
    {
        Distar_LogicaNegocio.Patente patenteBL;
        Distar_EntidadesNegocio.Familia familiaSeleccionada;
        Distar_LogicaNegocio.FamiliaPatente familiaPatenteBL;
        Distar_EntidadesNegocio.Usuario userLog;
        List<Distar_EntidadesNegocio.Patente> lista_patentes;
        private Distar_LogicaNegocio.Bitacora bitacoraBL = new Distar_LogicaNegocio.Bitacora();
        DialogResult res;
        MessageBoxButtons buttons;
        public event EventHandler<Boolean> FrmAdmFamilia_patentes_asignar_quitar_ok;
        /** TEXTO **/
        string successMsg = "";
        string errMsg = "";

        public FrmAdmFamilia(Distar_EntidadesNegocio.Usuario user, Distar_EntidadesNegocio.Familia fam): this()
        {
            userLog = user;
            familiaSeleccionada = fam;
        }

        public FrmAdmFamilia()
        {
            InitializeComponent();
        }

        private void FrmAdmFamilia_Load(object sender, EventArgs e)
        {
            setLanguaje();
            label2.Text = familiaSeleccionada.descripcion;
            checkedListBox1.CheckOnClick = true;
            checkedListBox1.SelectionMode = SelectionMode.One;
            checkedListBox1.DisplayMember = "descripcion";
            getListaPatentes();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void getListaPatentes()
        {
            checkedListBox1.Items.Clear();
            patenteBL = new Distar_LogicaNegocio.Patente();
            lista_patentes = patenteBL.getAllPatentes();
            foreach (Distar_EntidadesNegocio.Patente patente in lista_patentes)
            {
                checkedListBox1.Items.Add(patente);
            }
            checkPatentesFamilia();
        }

        private void checkPatentesFamilia()
        {
            Distar_EntidadesNegocio.Patente auxPatente;
            foreach (Distar_EntidadesNegocio.Patente patente in familiaSeleccionada.patentes)
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    auxPatente = (Distar_EntidadesNegocio.Patente) checkedListBox1.Items[i];
                    if (auxPatente.id_patente == patente.id_patente)
                    {
                        checkedListBox1.SetItemChecked(i, true);
                    }
                }
            }
        }

        // Asignar-Quitar patentes
        private void button4_Click(object sender, EventArgs e)
        {
            familiaPatenteBL = new Distar_LogicaNegocio.FamiliaPatente();
            List<Distar_EntidadesNegocio.FamiliaPatente> lista_familiaPatente_add = new List<Distar_EntidadesNegocio.FamiliaPatente>();
            List<Distar_EntidadesNegocio.FamiliaPatente> lista_familiaPatente_delete = new List<Distar_EntidadesNegocio.FamiliaPatente>();
            List<int> lista_indexFamiliaPatente_delete_anulado = new List<int>();
            string mensajeUsoPatente = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "The following Patents could not be removed since they would be left unused: " : "Las siguientes Patentes no se pudieron quitar ya que quedarían sin uso: ";
            List<Distar_EntidadesNegocio.Patente> lista_checkeados = new List<Distar_EntidadesNegocio.Patente>();
            if (checkedListBox1.CheckedItems.Count > 0)
            {
                // Recorro las patentes seleccionadas en el checkBoxList para ver cuáles se agregaron
                for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
                {
                    Distar_EntidadesNegocio.Patente aux = (Distar_EntidadesNegocio.Patente)checkedListBox1.CheckedItems[i];
                    lista_checkeados.Add(aux);
                    if (familiaSeleccionada.patentes.Count > 0)
                    {
                        if (!familiaSeleccionada.patentes.Exists(x => x.id_patente == aux.id_patente))
                        {
                            Distar_EntidadesNegocio.FamiliaPatente familiaPatente = new Distar_EntidadesNegocio.FamiliaPatente();
                            familiaPatente.id_familia = familiaSeleccionada.id_familia;
                            familiaPatente.id_patente = aux.id_patente;
                            lista_familiaPatente_add.Add(familiaPatente);
                        }
                    }
                    else
                    {
                        Distar_EntidadesNegocio.FamiliaPatente familiaPatente = new Distar_EntidadesNegocio.FamiliaPatente();
                        familiaPatente.id_familia = familiaSeleccionada.id_familia;
                        familiaPatente.id_patente = aux.id_patente;
                        lista_familiaPatente_add.Add(familiaPatente);
                    }
                }
                // Recorro las patentes de la familia selecionada para ver cuáles se quitaron
                foreach (Distar_EntidadesNegocio.Patente patente in familiaSeleccionada.patentes)
                {
                    if (!lista_checkeados.Exists(x => x.id_patente == patente.id_patente))
                    {
                        Distar_EntidadesNegocio.FamiliaPatente familiaPatente = new Distar_EntidadesNegocio.FamiliaPatente();
                        familiaPatente.id_familia = familiaSeleccionada.id_familia;
                        familiaPatente.id_patente = patente.id_patente;
                        lista_familiaPatente_delete.Add(familiaPatente);
                    }
                }
                // Verificar uso de patentes a quitar antes de operar
                foreach (Distar_EntidadesNegocio.FamiliaPatente familiaPatente in lista_familiaPatente_delete)
                {
                    if (!verificarUsuarioEnFamilia(familiaSeleccionada))
                    {
                        if (!patenteBL.verificarUsoPatente(familiaPatente.id_patente, false, familiaSeleccionada.id_familia))
                        {
                            mensajeUsoPatente += getNombrePatente(familiaPatente.id_patente) + ", ";
                            lista_indexFamiliaPatente_delete_anulado.Add(lista_familiaPatente_delete.IndexOf(familiaPatente));
                        }
                        else
                        {
                            if (!patenteBL.verificarUsoPatente(familiaPatente.id_patente, true, familiaSeleccionada.id_familia))
                            {
                                mensajeUsoPatente += getNombrePatente(familiaPatente.id_patente) + ", ";
                                lista_indexFamiliaPatente_delete_anulado.Add(lista_familiaPatente_delete.IndexOf(familiaPatente));
                            }
                        }
                    }
                }
                foreach (int index in lista_indexFamiliaPatente_delete_anulado)
                {
                    try
                    {
                        lista_familiaPatente_delete.RemoveAt(index);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                if (familiaPatenteBL.actualizarPatenteAFamilia(lista_familiaPatente_add, lista_familiaPatente_delete))
                {
                    if (lista_familiaPatente_add.Count > 0 || lista_familiaPatente_delete.Count > 0)
                    {
                        bitacoraBL.setWARNING(DateTime.Now, userLog, "Administración", "Patentes de Familia quitadas/asignadas.");
                    }
                    buttons = MessageBoxButtons.OK;
                    res = Distar.GUIServices.giveMeAlertsWithAction(successMsg, "Distar", buttons);
                    if (res == DialogResult.OK)
                    {
                        if (lista_indexFamiliaPatente_delete_anulado.Count > 0)
                        {
                            Distar.GUIServices.giveMeAlerts(mensajeUsoPatente.Substring(0, mensajeUsoPatente.Length - 2), "Distar", MessageBoxButtons.OK);
                            Close();
                        }
                        else
                        {
                            Close();
                        }
                        notificarCambios();
                    }
                }
                else
                {
                    buttons = MessageBoxButtons.OK;
                    Distar.GUIServices.giveMeAlerts(errMsg, "Distar", buttons);
                }
            }
            else
            {
                foreach (Distar_EntidadesNegocio.Patente patente in familiaSeleccionada.patentes)
                {
                    if (!lista_checkeados.Exists(x => x.id_patente == patente.id_patente))
                    {
                        Distar_EntidadesNegocio.FamiliaPatente familiaPatente = new Distar_EntidadesNegocio.FamiliaPatente();
                        familiaPatente.id_familia = familiaSeleccionada.id_familia;
                        familiaPatente.id_patente = patente.id_patente;
                        lista_familiaPatente_delete.Add(familiaPatente);
                    }
                }
                // Verificar uso de patentes a quitar antes de operar
                foreach (Distar_EntidadesNegocio.FamiliaPatente familiaPatente in lista_familiaPatente_delete)
                {
                    if (!verificarUsuarioEnFamilia(familiaSeleccionada))
                    {
                        if (!patenteBL.verificarUsoPatente(familiaPatente.id_patente, false, familiaSeleccionada.id_familia))
                        {
                            mensajeUsoPatente += getNombrePatente(familiaPatente.id_patente) + ", ";
                            lista_indexFamiliaPatente_delete_anulado.Add(lista_familiaPatente_delete.IndexOf(familiaPatente));
                        }
                    }
                    else
                    {
                        if (!patenteBL.verificarUsoPatente(familiaPatente.id_patente, true, familiaSeleccionada.id_familia))
                        {
                            mensajeUsoPatente += getNombrePatente(familiaPatente.id_patente) + ", ";
                            lista_indexFamiliaPatente_delete_anulado.Add(lista_familiaPatente_delete.IndexOf(familiaPatente));
                        }
                    }
                }
                foreach (int index in lista_indexFamiliaPatente_delete_anulado)
                {
                    lista_familiaPatente_delete.RemoveAt(index);
                }
                if (familiaPatenteBL.actualizarPatenteAFamilia(lista_familiaPatente_add, lista_familiaPatente_delete))
                {
                    if (lista_familiaPatente_add.Count > 0 || lista_familiaPatente_delete.Count > 0)
                    {
                        bitacoraBL.setWARNING(DateTime.Now, userLog, "Administración", "Patentes de Familia quitadas/asignadas.");
                    }
                    buttons = MessageBoxButtons.OK;
                    res = Distar.GUIServices.giveMeAlertsWithAction(successMsg, "Distar", buttons);
                    if (res == DialogResult.OK)
                    {
                        if (lista_indexFamiliaPatente_delete_anulado.Count > 0)
                        {
                            Distar.GUIServices.giveMeAlerts(mensajeUsoPatente.Substring(0, mensajeUsoPatente.Length - 2), "Distar", MessageBoxButtons.OK);
                            Close();
                        }
                        else
                        {
                            Close();
                        }
                        notificarCambios();
                    }
                }
                else
                {
                    buttons = MessageBoxButtons.OK;
                    Distar.GUIServices.giveMeAlerts(errMsg, "Distar", buttons);
                }
            }
        }

        private Boolean verificarUsuarioEnFamilia(Distar_EntidadesNegocio.Familia familia)
        {
            Distar_LogicaNegocio.FamiliaUsuario familiaUsuarioBL = new Distar_LogicaNegocio.FamiliaUsuario();
            List<Distar_EntidadesNegocio.Familia> lista_familia = new List<Distar_EntidadesNegocio.Familia>();
            lista_familia.Add(familia);
            Boolean existe_usuario = true;
            if (!familiaUsuarioBL.existeUsuarioEnFamilia(lista_familia)){
                existe_usuario = false;
            }
            return existe_usuario;
        }

        private string getNombrePatente(int id_patente)
        {
            string np = "";
            foreach (Distar_EntidadesNegocio.Patente patente in lista_patentes)
            {
                if (patente.id_patente == id_patente)
                {
                    np = patente.descripcion;
                }
            }
            return np;
        }

        public void notificarCambios()
        {
            FrmAdmFamilia_patentes_asignar_quitar_ok.Invoke(this, true);
        }

        private void setLanguaje()
        {
            successMsg = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Patents for the selected Family have been updated." : "Se han actualizado las Patentes para la Familia seleccionada.";
            errMsg = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "There was an error in removing / assigning Patents for the selected Family." : "Se produjo un error en quitar/asignar las Patentes para la Familia seleccionada.";
            this.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Distar - Administer Familiy patents" : "Distar - Administrar patentes de Familia";
            button4.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "ACCEPT" : "ACEPTAR";
            button3.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "CLOSE" : "SALIR";
            label1.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Family: " : "Familia: ";
            toolTip1.SetToolTip(button4, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Assign/Remove patents from family" : "Asignar/Quitar patentes de familia");
        }
    }
}
