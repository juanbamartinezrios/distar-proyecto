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
    public partial class FrmAdmUsuario_familias : Form
    {
        Distar_LogicaNegocio.Familia familiaBL;
        Distar_LogicaNegocio.FamiliaUsuario familiaUsuarioBL;
        Distar_EntidadesNegocio.Usuario userLog;
        Distar_EntidadesNegocio.Usuario usuarioSeleccionado;
        List<Distar_EntidadesNegocio.Familia> lista_familias;
        private Distar_LogicaNegocio.Bitacora bitacoraBL = new Distar_LogicaNegocio.Bitacora();
        DialogResult res;
        MessageBoxButtons buttons;
        public event EventHandler<Boolean> FrmAdmUsuario_familias_actualizacion_ok;
        /** TEXTO **/
        string successMsg = "";
        string errMsg = "";

        public FrmAdmUsuario_familias()
        {
            InitializeComponent();
        }

        public FrmAdmUsuario_familias(Distar_EntidadesNegocio.Usuario user, Distar_EntidadesNegocio.Usuario u): this()
        {
            userLog = user;
            usuarioSeleccionado = u;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmAdmUsuario_familias_Load(object sender, EventArgs e)
        {
            setLanguaje();
            label2.Text = usuarioSeleccionado.apellido+", "+usuarioSeleccionado.nombre;
            checkedListBox1.CheckOnClick = true;
            checkedListBox1.SelectionMode = SelectionMode.One;
            checkedListBox1.DisplayMember = "descripcion";
            getListaFamilias();
        }

        private void getListaFamilias()
        {
            checkedListBox1.Items.Clear();
            familiaBL = new Distar_LogicaNegocio.Familia();
            lista_familias = familiaBL.getAllFamilias();
            foreach (Distar_EntidadesNegocio.Familia familia in lista_familias)
            {
                checkedListBox1.Items.Add(familia);
            }
            if (usuarioSeleccionado.familias.Count > 0)
            {
                getFamiliaUsuario();
            }
        }

        private void getFamiliaUsuario()
        {
            Distar_EntidadesNegocio.Familia auxFamilia;
            foreach (Distar_EntidadesNegocio.Familia familia in usuarioSeleccionado.familias)
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    auxFamilia = (Distar_EntidadesNegocio.Familia)checkedListBox1.Items[i];
                    if (auxFamilia.id_familia == familia.id_familia)
                    {
                        checkedListBox1.SetItemChecked(i, true);
                    }
                }
            }
        }

        // Asignar-Quitar familias
        private void button4_Click(object sender, EventArgs e)
        {
            familiaUsuarioBL = new Distar_LogicaNegocio.FamiliaUsuario();
            List<Distar_EntidadesNegocio.FamiliaUsuario> lista_familiaUsuario_add = new List<Distar_EntidadesNegocio.FamiliaUsuario>();
            List<Distar_EntidadesNegocio.FamiliaUsuario> lista_familiaUsuario_delete = new List<Distar_EntidadesNegocio.FamiliaUsuario>();
            List<int> lista_indexFamiliaUsuario_delete_anulado = new List<int>();
            string mensajeUsoPatente = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "The following Families could not be removed since there would be Patents without use: " : "Las siguientes Familias no se pudieron quitar ya que quedarían Patentes sin uso: ";
            List<Distar_EntidadesNegocio.Familia> lista_checkeados = new List<Distar_EntidadesNegocio.Familia>();
            if (checkedListBox1.CheckedItems.Count > 0)
            {
                // Recorro las familias seleccionadas en el checkBoxList para ver cuáles se agregaron
                for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
                {
                    Distar_EntidadesNegocio.Familia aux = (Distar_EntidadesNegocio.Familia)checkedListBox1.CheckedItems[i];
                    lista_checkeados.Add(aux);
                    if (usuarioSeleccionado.familias.Count > 0)
                    {
                        if (!usuarioSeleccionado.familias.Exists(x => x.id_familia == aux.id_familia))
                        {
                            Distar_EntidadesNegocio.FamiliaUsuario familiaUsuario = new Distar_EntidadesNegocio.FamiliaUsuario();
                            familiaUsuario.id_usuario = usuarioSeleccionado.id_usuario;
                            familiaUsuario.id_familia = aux.id_familia;
                            lista_familiaUsuario_add.Add(familiaUsuario);
                        }
                    }
                    else
                    {
                        Distar_EntidadesNegocio.FamiliaUsuario familiaUsuario = new Distar_EntidadesNegocio.FamiliaUsuario();
                        familiaUsuario.id_usuario = usuarioSeleccionado.id_usuario;
                        familiaUsuario.id_familia = aux.id_familia;
                        lista_familiaUsuario_add.Add(familiaUsuario);
                    }
                }
                // Recorro las familias del usuario selecionado para ver cuáles se quitaron
                foreach (Distar_EntidadesNegocio.Familia familia in usuarioSeleccionado.familias)
                {
                    if (!lista_checkeados.Exists(x => x.id_familia == familia.id_familia))
                    {
                        Distar_EntidadesNegocio.FamiliaUsuario familiaUsuario = new Distar_EntidadesNegocio.FamiliaUsuario();
                        familiaUsuario.id_usuario = usuarioSeleccionado.id_usuario;
                        familiaUsuario.id_familia = familia.id_familia;
                        lista_familiaUsuario_delete.Add(familiaUsuario);
                    }
                }
                // Verificar uso de patentes a quitar antes de operar
                foreach (Distar_EntidadesNegocio.FamiliaUsuario familiaUsuario in lista_familiaUsuario_delete)
                {
                    if (!verificarUsoPatente(familiaUsuario.id_familia, usuarioSeleccionado.id_usuario))
                    {
                        mensajeUsoPatente += getNombreFamilia(familiaUsuario.id_familia) + ", ";
                        lista_indexFamiliaUsuario_delete_anulado.Add(lista_familiaUsuario_delete.IndexOf(familiaUsuario));
                    }
                }
                foreach (int index in lista_indexFamiliaUsuario_delete_anulado)
                {
                    lista_familiaUsuario_delete.RemoveAt(index);
                }
                if (familiaUsuarioBL.actualizarFamiliaAUsuario(lista_familiaUsuario_add, lista_familiaUsuario_delete))
                {
                    if (lista_familiaUsuario_add.Count > 0 || lista_familiaUsuario_delete.Count > 0)
                    {
                        bitacoraBL.setWARNING(DateTime.Now, userLog, "Administración", "Familias de Usuario quitadas/asignadas.");
                    }
                    buttons = MessageBoxButtons.OK;
                    res = Distar.GUIServices.giveMeAlertsWithAction(successMsg, "Distar", buttons);
                    if (res == DialogResult.OK)
                    {
                        if (lista_indexFamiliaUsuario_delete_anulado.Count > 0)
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
                foreach (Distar_EntidadesNegocio.Familia familia in usuarioSeleccionado.familias)
                {
                    if (!lista_checkeados.Exists(x => x.id_familia == familia.id_familia))
                    {
                        Distar_EntidadesNegocio.FamiliaUsuario familiaUsuario = new Distar_EntidadesNegocio.FamiliaUsuario();
                        familiaUsuario.id_usuario = usuarioSeleccionado.id_usuario;
                        familiaUsuario.id_familia = familia.id_familia;
                        lista_familiaUsuario_delete.Add(familiaUsuario);
                    }
                }
                // Verificar uso de patentes a quitar antes de operar
                foreach (Distar_EntidadesNegocio.FamiliaUsuario familiaUsuario in lista_familiaUsuario_delete)
                {
                    if (!verificarUsoPatente(familiaUsuario.id_familia, usuarioSeleccionado.id_usuario))
                    {
                        mensajeUsoPatente += getNombreFamilia(familiaUsuario.id_familia) + ", ";
                        lista_indexFamiliaUsuario_delete_anulado.Add(lista_familiaUsuario_delete.IndexOf(familiaUsuario));
                    }
                }
                foreach (int index in lista_indexFamiliaUsuario_delete_anulado)
                {
                    lista_familiaUsuario_delete.RemoveAt(index);
                }
                if (familiaUsuarioBL.actualizarFamiliaAUsuario(lista_familiaUsuario_add, lista_familiaUsuario_delete))
                {
                    if (lista_familiaUsuario_add.Count > 0 || lista_familiaUsuario_delete.Count > 0)
                    {
                        bitacoraBL.setWARNING(DateTime.Now, userLog, "Administración", "Familias de Usuario quitadas/asignadas.");
                    }
                    buttons = MessageBoxButtons.OK;
                    res = Distar.GUIServices.giveMeAlertsWithAction(successMsg, "Distar", buttons);
                    if (res == DialogResult.OK)
                    {
                        if (lista_indexFamiliaUsuario_delete_anulado.Count > 0)
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

        private string getNombreFamilia(int id_familia)
        {
            string nf = "";
            foreach (Distar_EntidadesNegocio.Familia familia in lista_familias)
            {
                if (familia.id_familia == id_familia)
                {
                    nf = familia.descripcion;
                }
            }
            return nf;
        }

        private Boolean verificarUsoPatente(int id_familia, int id_usuario)
        {
            Distar_LogicaNegocio.FamiliaPatente familiaPatenteBL = new Distar_LogicaNegocio.FamiliaPatente();
            Distar_LogicaNegocio.UsuarioPatente usuarioPatenteBL = new Distar_LogicaNegocio.UsuarioPatente();
            Distar_LogicaNegocio.FamiliaUsuario familiaUsuarioBL = new Distar_LogicaNegocio.FamiliaUsuario();
            List<Distar_EntidadesNegocio.Patente> lista_patentes_en_familia = new List<Distar_EntidadesNegocio.Patente>();
            lista_patentes_en_familia = familiaPatenteBL.obtenerPatentesFamilia(id_familia);
            
            // Verifico si la familia la tiene asignada otro usuario activo
            List<Distar_EntidadesNegocio.Usuario> lista_usuarios = new List<Distar_EntidadesNegocio.Usuario>();
            lista_usuarios = familiaUsuarioBL.obtenerUsuariosEnFamilia(id_familia, id_usuario);
            if (lista_usuarios.Count > 0)
            {
                foreach (Distar_EntidadesNegocio.Usuario usuario in lista_usuarios)
                {
                    foreach (Distar_EntidadesNegocio.Patente patente in lista_patentes_en_familia)
                    {
                        if (usuarioPatenteBL.usuarioTienePatenteNegada(patente.id_patente, usuario.id_usuario))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            // Busco patentes asignadas en la familia
            List<Distar_EntidadesNegocio.Patente> lista_patentes_no_quitar_familia = new List<Distar_EntidadesNegocio.Patente>();
            // Verifico si algún otro usuario tiene asignadas esas patentes
            foreach (Distar_EntidadesNegocio.Patente patente in lista_patentes_en_familia)
	        {
                if (!usuarioPatenteBL.verificarUsoPatente(patente.id_patente)){
                    lista_patentes_no_quitar_familia.Add(patente);
                }
	        }
            if (lista_patentes_no_quitar_familia.Count == 0){
                return true;
            }
            // Verifico si otras familias tienen asignadas esas patentes
            List<int> lista_id_patentes = new List<int>();
            List<int> lista_id_patentes_return = new List<int>();
            foreach (Distar_EntidadesNegocio.Patente patente in lista_patentes_no_quitar_familia)
	        {
	            lista_id_patentes.Add(patente.id_patente);
	        }
            // Traigo las patentes que están en otras familias
            lista_id_patentes_return = familiaPatenteBL.obtenerPatentesNoUtilizadasEnOtrasFamilias(id_familia, lista_id_patentes);
            if (lista_id_patentes_return.Count > 0){
                return false;
            }
            return true;
        }

        public void notificarCambios()
        {
            FrmAdmUsuario_familias_actualizacion_ok.Invoke(this, true);
        }

        private void setLanguaje()
        {
            successMsg = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Families have been updated for the selected User." : "Se han actualizado las Familias para el Usuario seleccionado.";
            errMsg = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Remove/assign Families for the selected User." : "Se produjo un error en quitar/asignar las Familias para el Usuario seleccionado.";
            this.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Distar - Administer User families" : "Distar - Administrar familias de Usuario";
            button4.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "ACCEPT" : "ACEPTAR";
            button3.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "CLOSE" : "SALIR";
            label1.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "User: " : "Usuario: ";
            toolTip1.SetToolTip(button4, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Assign/Remove families from user" : "Asignar/Quitar familias de usuario");
        }
    }
}
