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
    public partial class FrmAdmUsuario_patentes : Form
    {
        Distar_LogicaNegocio.Patente patenteBL;
        Distar_LogicaNegocio.UsuarioPatente usuarioPatenteBL;
        Distar_LogicaNegocio.Usuario usuarioBL;
        Distar_EntidadesNegocio.Usuario userLog;
        Distar_EntidadesNegocio.Usuario usuarioSeleccionado;
        List<Distar_EntidadesNegocio.Patente> lista_patentes;
        private Distar_LogicaNegocio.Bitacora bitacoraBL = new Distar_LogicaNegocio.Bitacora();
        DialogResult res;
        MessageBoxButtons buttons;
        public event EventHandler<Boolean> FrmAdmUsuario_patentes_asignar_quitar_ok;
        /** TEXTO **/
        string successMsg = "";
        string errMsg = "";

        public FrmAdmUsuario_patentes()
        {
            InitializeComponent();
        }

        public FrmAdmUsuario_patentes(Distar_EntidadesNegocio.Usuario user, Distar_EntidadesNegocio.Usuario u): this()
        {
            userLog = user;
            usuarioSeleccionado = u;
        }

        private void FrmAdmUsuario_patentes_Load(object sender, EventArgs e)
        {
            setLanguaje();
            label2.Text = usuarioSeleccionado.apellido + ", " + usuarioSeleccionado.nombre;
            checkedListBox1.CheckOnClick = true;
            checkedListBox1.SelectionMode = SelectionMode.One;
            checkedListBox1.DisplayMember = "descripcion";
            getListaPatentes();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Asignar-Quitar patentes
        private void button5_Click(object sender, EventArgs e)
        {
            usuarioPatenteBL = new Distar_LogicaNegocio.UsuarioPatente();
            List<Distar_EntidadesNegocio.UsuarioPatente> lista_usuarioPatente_add = new List<Distar_EntidadesNegocio.UsuarioPatente>();
            List<Distar_EntidadesNegocio.UsuarioPatente> lista_usuarioPatente_delete = new List<Distar_EntidadesNegocio.UsuarioPatente>();
            List <Distar_EntidadesNegocio.UsuarioPatente> lista_indexUsuarioPatente_delete_anulado = new List<Distar_EntidadesNegocio.UsuarioPatente>();
            string mensajeUsoPatente = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "The following Patents could not be removed since they would be left unused: " : "Las siguientes Patentes no se pudieron quitar ya que quedarían sin uso: ";
            List<Distar_EntidadesNegocio.Patente> lista_checkeados = new List<Distar_EntidadesNegocio.Patente>();
            if (checkedListBox1.CheckedItems.Count > 0)
            {
                // Recorro las patentes seleccionadas en el checkBoxList para ver cuáles se agregaron
                for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
                {
                    Distar_EntidadesNegocio.Patente aux = (Distar_EntidadesNegocio.Patente)checkedListBox1.CheckedItems[i];
                    lista_checkeados.Add(aux);
                    if (usuarioSeleccionado.patentes != null && usuarioSeleccionado.patentes.Count > 0)
                    {
                        if (!usuarioSeleccionado.patentes.Exists(x => x.id_patente == aux.id_patente))
                        {
                            Distar_EntidadesNegocio.UsuarioPatente usuarioPatente = new Distar_EntidadesNegocio.UsuarioPatente();
                            usuarioPatente.id_usuario = usuarioSeleccionado.id_usuario;
                            usuarioPatente.id_patente = aux.id_patente;
                            usuarioPatente.negado = 0;
                            lista_usuarioPatente_add.Add(usuarioPatente);
                        }
                    }
                    else
                    {
                        Distar_EntidadesNegocio.UsuarioPatente usuarioPatente = new Distar_EntidadesNegocio.UsuarioPatente();
                        usuarioPatente.id_usuario = usuarioSeleccionado.id_usuario;
                        usuarioPatente.id_patente = aux.id_patente;
                        usuarioPatente.negado = 0;
                        lista_usuarioPatente_add.Add(usuarioPatente);
                    }
                }
                // Recorro las patentes de la familia selecionada para ver cuáles se quitaron
                foreach (Distar_EntidadesNegocio.Patente patente in usuarioSeleccionado.patentes)
                {
                    if (!lista_checkeados.Exists(x => x.id_patente == patente.id_patente))
                    {
                        Distar_EntidadesNegocio.UsuarioPatente usuarioPatente = new Distar_EntidadesNegocio.UsuarioPatente();
                        usuarioPatente.id_usuario = usuarioSeleccionado.id_usuario;
                        usuarioPatente.id_patente = patente.id_patente;
                        usuarioPatente.negado = 0;
                        lista_usuarioPatente_delete.Add(usuarioPatente);
                    }
                }
                // Verificar uso de patentes a quitar antes de operar
                foreach (Distar_EntidadesNegocio.UsuarioPatente usuarioPatente in lista_usuarioPatente_delete)
                {
                    if (!patenteBL.verificarUsoPatente(usuarioPatente.id_patente, false, 0, usuarioSeleccionado.id_usuario))
                    {
                        mensajeUsoPatente += getNombrePatente(usuarioPatente.id_patente) + ", ";
                        lista_indexUsuarioPatente_delete_anulado.Add(usuarioPatente);
                    }
                }
                foreach (Distar_EntidadesNegocio.UsuarioPatente item in lista_indexUsuarioPatente_delete_anulado)
                {
                    lista_usuarioPatente_delete.Remove(item);
                }
                if (usuarioPatenteBL.actualizarPatenteAUsuario(lista_usuarioPatente_add, lista_usuarioPatente_delete))
                {
                    if (lista_usuarioPatente_add.Count > 0 || lista_usuarioPatente_delete.Count > 0)
                    {
                        bitacoraBL.setWARNING(DateTime.Now, userLog, "Administración", "Patentes de Usuario quitadas/asignadas.");
                    }
                    buttons = MessageBoxButtons.OK;
                    res = Distar.GUIServices.giveMeAlertsWithAction(successMsg, "Distar", buttons);
                    if (res == DialogResult.OK)
                    {
                        if (lista_indexUsuarioPatente_delete_anulado.Count > 0)
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
                foreach (Distar_EntidadesNegocio.Patente patente in usuarioSeleccionado.patentes)
                {
                    if (!lista_checkeados.Exists(x => x.id_patente == patente.id_patente))
                    {
                        Distar_EntidadesNegocio.UsuarioPatente usuarioPatente = new Distar_EntidadesNegocio.UsuarioPatente();
                        usuarioPatente.id_usuario = usuarioSeleccionado.id_usuario;
                        usuarioPatente.id_patente = patente.id_patente;
                        usuarioPatente.negado = 0;
                        lista_usuarioPatente_delete.Add(usuarioPatente);
                    }
                }
                // Verificar uso de patentes a quitar antes de operar
                foreach (Distar_EntidadesNegocio.UsuarioPatente usuarioPatente in lista_usuarioPatente_delete)
                {
                    if (!patenteBL.verificarUsoPatente(usuarioPatente.id_patente, true, 0, usuarioSeleccionado.id_usuario))
                    {
                        mensajeUsoPatente += getNombrePatente(usuarioPatente.id_patente) + ", ";
                        lista_indexUsuarioPatente_delete_anulado.Add(usuarioPatente);
                    }
                }
                foreach (Distar_EntidadesNegocio.UsuarioPatente item in lista_indexUsuarioPatente_delete_anulado)
                {
                    lista_usuarioPatente_delete.Remove(item);
                }
                if (usuarioPatenteBL.actualizarPatenteAUsuario(lista_usuarioPatente_add, lista_usuarioPatente_delete))
                {

                    if (lista_usuarioPatente_add.Count > 0 || lista_usuarioPatente_delete.Count > 0)
                    {
                        bitacoraBL.setWARNING(DateTime.Now, userLog, "Administración", "Patentes de Usuario quitadas/asignadas.");
                    }
                    buttons = MessageBoxButtons.OK;
                    res = Distar.GUIServices.giveMeAlertsWithAction(successMsg, "Distar", buttons);
                    if (res == DialogResult.OK)
                    {
                        if (lista_indexUsuarioPatente_delete_anulado.Count > 0)
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

        // Negar-Habilitar patentes
        private void button1_Click(object sender, EventArgs e)
        {
            Distar.ADMIN.FrmNegarHabilitar_UsuarioPatente _FrmNegarHabilitar_UsuarioPatente = new Distar.ADMIN.FrmNegarHabilitar_UsuarioPatente(userLog, usuarioSeleccionado);
            _FrmNegarHabilitar_UsuarioPatente.Show();
            _FrmNegarHabilitar_UsuarioPatente.FrmNegarHabilitar_UsuarioPatente_negar_habilitar_ok += this.OnFrmNegarHabilitar_UsuarioPatente;
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
            if (usuarioSeleccionado.patentes.Count > 0)
            {
                getPatenteUsuario();
            }
        }

        private void getPatenteUsuario()
        {
            Distar_EntidadesNegocio.Patente auxPatente;
            foreach (Distar_EntidadesNegocio.Patente patente in usuarioSeleccionado.patentes)
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    auxPatente = (Distar_EntidadesNegocio.Patente)checkedListBox1.Items[i];
                    if (auxPatente.id_patente == patente.id_patente)
                    {
                        if (!patente.negado)
                        {
                            checkedListBox1.SetItemChecked(i, true);
                        }
                        else
                        {
                            checkedListBox1.SetItemCheckState(i, CheckState.Indeterminate);
                        }
                    }
                }
            }
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

        private void refreshUsuarioSeleccionado()
        {
            usuarioBL = new Distar_LogicaNegocio.Usuario();
            Distar_EntidadesNegocio.Usuario usuarioAux = usuarioBL.obtenerUsuarioPorId(usuarioSeleccionado.id_usuario);
            if (usuarioAux != null)
            {
                usuarioSeleccionado = usuarioAux;
            }
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.CurrentValue == CheckState.Indeterminate)
            {
                e.NewValue = CheckState.Indeterminate;
            }
        }

        public void notificarCambios()
        {
            FrmAdmUsuario_patentes_asignar_quitar_ok.Invoke(this, true);
        }

        // Negar/Habilitar patente a Usuario
        private void OnFrmNegarHabilitar_UsuarioPatente(object sender, Boolean flag)
        {
            refreshUsuarioSeleccionado();
            getListaPatentes();
            notificarCambios();
        }

        private void setLanguaje()
        {
            successMsg = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "The Patents for the selected User have been updated." : "Se han actualizado las Patentes para el Usuario seleccionado.";
            errMsg = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "There was an error in removing/assigning Patents for the selected User." : "Se produjo un error en quitar/asignar las Patentes para el Usuario seleccionado.";
            this.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Distar - Administer User patents" : "Distar - Administrar patentes de Usuario";
            button1.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "DISABLE/ENABLE" : "NEGAR/HABILITAR";
            button3.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "CLOSE" : "SALIR";
            button5.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "ASSIGN/REMOVE" : "ASIGNAR/QUITAR";
            label1.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "User: " : "Usuario: ";
            toolTip1.SetToolTip(button1, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Disable/Enable patentes for user" : "Negar/Habilitar patentes para usuario");
            toolTip1.SetToolTip(button5, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Assign/Remove selected patents for user" : "Asignar/Quitar patentes seleccionadas para usuario");
        }
    }
}
