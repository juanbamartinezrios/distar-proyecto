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
    public partial class FrmNegarHabilitar_UsuarioPatente : Form
    {
        Distar_LogicaNegocio.Patente patenteBL;
        Distar_LogicaNegocio.UsuarioPatente usuarioPatenteBL;
        Distar_EntidadesNegocio.Usuario userLog;
        Distar_EntidadesNegocio.Usuario usuarioSeleccionado;
        List<Distar_EntidadesNegocio.Patente> lista_patentes;
        List<Distar_EntidadesNegocio.Patente> lista_patentes_negadas = new List<Distar_EntidadesNegocio.Patente>();
        private Distar_LogicaNegocio.Bitacora bitacoraBL = new Distar_LogicaNegocio.Bitacora();
        DialogResult res;
        MessageBoxButtons buttons;
        public event EventHandler<Boolean> FrmNegarHabilitar_UsuarioPatente_negar_habilitar_ok;

        public FrmNegarHabilitar_UsuarioPatente()
        {
            InitializeComponent();
        }

        public FrmNegarHabilitar_UsuarioPatente(Distar_EntidadesNegocio.Usuario user, Distar_EntidadesNegocio.Usuario u): this()
        {
            userLog = user;
            usuarioSeleccionado = u;
        }

        private void FrmNegarHabilitar_UsuarioPatente_Load(object sender, EventArgs e)
        {
            setLanguaje();
            checkedListBox1.CheckOnClick = true;
            checkedListBox1.SelectionMode = SelectionMode.One;
            checkedListBox1.DisplayMember = "descripcion";
            getListaPatentes();
        }

        private void getListaPatentes()
        {
            patenteBL = new Distar_LogicaNegocio.Patente();
            lista_patentes = patenteBL.getAllPatentes();
            foreach (Distar_EntidadesNegocio.Patente patente in lista_patentes)
            {
                checkedListBox1.Items.Add(patente);
            }
            setPatentesNegadas();
        }

        private void setPatentesNegadas()
        {
            foreach (Distar_EntidadesNegocio.Patente patente in usuarioSeleccionado.patentes)
            {
                if (patente.negado)
                {
                    for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    {
                        Distar_EntidadesNegocio.Patente aux = (Distar_EntidadesNegocio.Patente) checkedListBox1.Items[i];
                        if (aux.id_patente == patente.id_patente)
                        {
                            lista_patentes_negadas.Add(aux);
                            checkedListBox1.SetItemChecked(i, true);
                        }
                    }
                }
            }
        }

        // Negar-Habilitar patentes
        private void button5_Click(object sender, EventArgs e)
        {
            usuarioPatenteBL = new Distar_LogicaNegocio.UsuarioPatente();
            List<Distar_EntidadesNegocio.UsuarioPatente> lista_patente_neg = new List<Distar_EntidadesNegocio.UsuarioPatente>();
            List<Distar_EntidadesNegocio.UsuarioPatente> lista_patente_hab = new List<Distar_EntidadesNegocio.UsuarioPatente>();
            List<Distar_EntidadesNegocio.UsuarioPatente> lista_indexUsuarioPatente_neg_anulado = new List<Distar_EntidadesNegocio.UsuarioPatente>();
            string mensajeUsoPatente = "Las siguientes Patentes no se pudieron negar ya que quedarían sin uso: ";
            List<Distar_EntidadesNegocio.Patente> lista_checkeados = new List<Distar_EntidadesNegocio.Patente>();
            if (checkedListBox1.CheckedItems.Count > 0)
            {
                // Recorro las patentes seleccionadas en el checkBoxList para ver cuáles se agregaron
                for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
                {
                    Distar_EntidadesNegocio.Patente aux = (Distar_EntidadesNegocio.Patente) checkedListBox1.CheckedItems[i];
                    lista_checkeados.Add(aux);
                    if (lista_patentes_negadas != null && lista_patentes_negadas.Count > 0)
                    {
                        if (!lista_patentes_negadas.Exists(x => x.id_patente == aux.id_patente))
                        {
                            Distar_EntidadesNegocio.UsuarioPatente usuarioPatente = new Distar_EntidadesNegocio.UsuarioPatente();
                            usuarioPatente.id_usuario = usuarioSeleccionado.id_usuario;
                            usuarioPatente.id_patente = aux.id_patente;
                            usuarioPatente.negado = 1;
                            lista_patente_neg.Add(usuarioPatente);
                        }
                    }
                    else
                    {
                        Distar_EntidadesNegocio.UsuarioPatente usuarioPatente = new Distar_EntidadesNegocio.UsuarioPatente();
                        usuarioPatente.id_usuario = usuarioSeleccionado.id_usuario;
                        usuarioPatente.id_patente = aux.id_patente;
                        usuarioPatente.negado = 1;
                        lista_patente_neg.Add(usuarioPatente);
                    }
                }
                // Recorro las patentes del usuario selecionado para ver cuáles se habilitaron
                foreach (Distar_EntidadesNegocio.Patente patente in lista_patentes_negadas)
                {
                    if (!lista_checkeados.Exists(x => x.id_patente == patente.id_patente))
                    {
                        Distar_EntidadesNegocio.UsuarioPatente usuarioPatente = new Distar_EntidadesNegocio.UsuarioPatente();
                        usuarioPatente.id_usuario = usuarioSeleccionado.id_usuario;
                        usuarioPatente.id_patente = patente.id_patente;
                        usuarioPatente.negado = 0;
                        lista_patente_hab.Add(usuarioPatente);
                    }
                }
                // Verificar uso de patentes a quitar antes de operar
                foreach (Distar_EntidadesNegocio.UsuarioPatente usuarioPatente in lista_patente_neg)
                {
                    if (!patenteBL.verificarUsoPatente(usuarioPatente.id_patente, true, 0, usuarioSeleccionado.id_usuario))
                    {
                        mensajeUsoPatente += getNombrePatente(usuarioPatente.id_patente) + ", ";
                        lista_indexUsuarioPatente_neg_anulado.Add(usuarioPatente);
                    }
                }
                foreach (Distar_EntidadesNegocio.UsuarioPatente usuarioPatente in lista_indexUsuarioPatente_neg_anulado)
                {
                    lista_patente_neg.Remove(usuarioPatente);
                }
                if (usuarioPatenteBL.negarPatentesAUsuario(lista_patente_neg) && usuarioPatenteBL.habilitarPatentesAUsuario(lista_patente_hab))
                {
                    if (lista_patente_neg.Count > 0 || lista_patente_hab.Count > 0)
                    {
                        bitacoraBL.setWARNING(DateTime.Now, userLog, "Administración", "Patentes de Usuario negadas/habilitadas.");
                    }
                    buttons = MessageBoxButtons.OK;
                    res = Distar.GUIServices.giveMeAlertsWithAction("Se han actualizado las Patentes para el Usuario seleccionado.", "Distar", buttons);
                    if (res == DialogResult.OK)
                    {
                        if (lista_indexUsuarioPatente_neg_anulado.Count > 0)
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
                    Distar.GUIServices.giveMeAlerts("Se produjo un error en negar/habilitar las Patentes para el Usuario seleccionado.", "Distar", buttons);
                }
            }
            else
            {
                foreach (Distar_EntidadesNegocio.Patente patente in lista_patentes_negadas)
                {
                    if (!lista_checkeados.Exists(x => x.id_patente == patente.id_patente))
                    {
                        Distar_EntidadesNegocio.UsuarioPatente usuarioPatente = new Distar_EntidadesNegocio.UsuarioPatente();
                        usuarioPatente.id_usuario = usuarioSeleccionado.id_usuario;
                        usuarioPatente.id_patente = patente.id_patente;
                        usuarioPatente.negado = 0;
                        lista_patente_hab.Add(usuarioPatente);
                    }
                }
                if (usuarioPatenteBL.negarPatentesAUsuario(lista_patente_neg) && usuarioPatenteBL.habilitarPatentesAUsuario(lista_patente_hab))
                {
                    bitacoraBL.setWARNING(DateTime.Now, userLog, "Administración", "Patentes de Usuario negadas/habilitadas.");
                    buttons = MessageBoxButtons.OK;
                    res = Distar.GUIServices.giveMeAlertsWithAction("Se han actualizado las Patentes para el Usuario seleccionado.", "Distar", buttons);
                    if (res == DialogResult.OK)
                    {
                        Close();
                        notificarCambios();
                    }
                }
                else
                {
                    buttons = MessageBoxButtons.OK;
                    Distar.GUIServices.giveMeAlerts("Se produjo un error en negar/habilitar las Patentes para el Usuario seleccionado.", "Distar", buttons);
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

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void notificarCambios()
        {
            FrmNegarHabilitar_UsuarioPatente_negar_habilitar_ok.Invoke(this, true);
        }

        private void setLanguaje() {
            this.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Distar - Disable/Enable user patents" : "Distar - Negar/Habilitar patentes de Usuario";
            button5.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "ACCEPT" : "ACEPTAR";
            button2.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "CLOSE" : "SALIR";
            toolTip1.SetToolTip(button5, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Disable/Enable selected patents for user" : "Negar/Habilitar patentes seleccionadas para el usuario");
        }
    }
}
