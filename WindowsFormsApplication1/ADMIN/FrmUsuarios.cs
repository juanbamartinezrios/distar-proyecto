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
    public partial class FrmUsuarios : Form
    {
        Distar_LogicaNegocio.Usuario usuarioBL;
        Distar_EntidadesNegocio.Usuario usuarioSeleccionado;
        Distar_EntidadesNegocio.Usuario userLog;
        List<Distar_EntidadesNegocio.Usuario> lista_usuarios;
        private Distar_LogicaNegocio.Bitacora bitacoraBL = new Distar_LogicaNegocio.Bitacora();
        private Distar_LogicaNegocio.Services servicesBL = new Distar_LogicaNegocio.Services();
        public event EventHandler<Boolean> FrmUsuario_actualizacion_ok;
        DialogResult res;
        MessageBoxButtons buttons;

        public FrmUsuarios()
        {
            InitializeComponent();
        }

        public FrmUsuarios(Distar_EntidadesNegocio.Usuario user): this()
        {
            userLog = user;
        }

        private void FrmUsuarios_Load(object sender, EventArgs e)
        {
            setLanguaje();
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.MultiSelect = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            servicesBL.setPatentesUsuarioLog(userLog);
            setPermisosNavegavilidad();
            getListaUsuario();
            refreshDataGridView();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Boolean permisoEliminar = true;
            usuarioBL = new Distar_LogicaNegocio.Usuario();
            Distar_LogicaNegocio.DigitoVerificador DVBL = new Distar_LogicaNegocio.DigitoVerificador();
            Distar_LogicaNegocio.UsuarioPatente usuarioPatenteBL = new Distar_LogicaNegocio.UsuarioPatente();
            Distar_LogicaNegocio.FamiliaPatente familiaPatenteBL = new Distar_LogicaNegocio.FamiliaPatente();
            Distar_LogicaNegocio.FamiliaUsuario familiaUsuarioBL = new Distar_LogicaNegocio.FamiliaUsuario();
            List<Distar_EntidadesNegocio.Patente> lista_patentes_usuario = new List<Distar_EntidadesNegocio.Patente>();
            lista_patentes_usuario = usuarioPatenteBL.cargarPatentesDeUsuario(usuarioSeleccionado.id_usuario);
            foreach (Distar_EntidadesNegocio.Familia familia in usuarioSeleccionado.familias)
            {
                if (lista_patentes_usuario.Count == 0)
                {
                    lista_patentes_usuario.AddRange(familiaPatenteBL.obtenerPatentesFamilia(familia.id_familia));
                }
                else
                {
                    lista_patentes_usuario.Union(familiaPatenteBL.obtenerPatentesFamilia(familia.id_familia));
                }
            }
            List<int> lista_patentes_no_asignadas_usuario = new List<int>();
            foreach (Distar_EntidadesNegocio.Patente patente in lista_patentes_usuario)
            {
                if (!(usuarioPatenteBL.verificarUsoPatente(patente.id_patente, usuarioSeleccionado.id_usuario)))
                {
                    lista_patentes_no_asignadas_usuario.Add(patente.id_patente);
                }
            }
            if (familiaPatenteBL.obtenerPatentesNoUtilizadasEnOtrasFamilias(0, lista_patentes_no_asignadas_usuario, usuarioSeleccionado.id_usuario).Count > 0)
            {
                permisoEliminar = false;
            }
            if (permisoEliminar)
            {
                buttons = MessageBoxButtons.YesNo;
                res = Distar.GUIServices.giveMeAlertsWithAction(Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Do you want to unsubscribe the selected User?" : "Desea dar de baja al Usuario seleccionado?", "Distar", buttons);
                if (res == DialogResult.Yes)
                {
                    usuarioBL = new Distar_LogicaNegocio.Usuario();
                    if (usuarioBL.deleteLogico(usuarioSeleccionado))
                    {
                        bitacoraBL.setWARNING(DateTime.Now, userLog, "Usuarios", "Baja de Usuario.");
                        buttons = MessageBoxButtons.OK;
                        Distar.GUIServices.giveMeAlerts(Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Unsubscribed user." : "Usuario dado de baja.", "Distar", buttons);
                        getListaUsuario();
                        refreshDataGridView();
                    }
                    else
                    {
                        buttons = MessageBoxButtons.OK;
                        Distar.GUIServices.giveMeAlerts(Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "The selected User could not unsubscribe." : "El Usuario seleccionado no se pudo dar de baja.", "Distar", buttons);
                    }
                }
            }
            else
            {
                buttons = MessageBoxButtons.OK;
                Distar.GUIServices.giveMeAlerts(Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "The selected User could not be deregistered due to internal control rules." : "El Usuario seleccionado no se pudo dar de baja por normas de control interno.", "Distar", buttons);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            buttons = MessageBoxButtons.YesNo;
            res = Distar.GUIServices.giveMeAlertsWithAction(Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Do you want to unblock the selected User?" : "Desea desbloquear el Usuario seleccionado?", "Distar", buttons);
            if (res == DialogResult.Yes)
            {
                usuarioBL = new Distar_LogicaNegocio.Usuario();
                if (usuarioBL.desbloquearUsuario(usuarioSeleccionado.id_usuario))
                {
                    bitacoraBL.setINFO(DateTime.Now, userLog, "Usuarios", "Desbloqueo de Usuario.");
                    buttons = MessageBoxButtons.OK;
                    Distar.GUIServices.giveMeAlerts(Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "User unlocked." : "Usuario desbloqueado.", "Distar", buttons);
                    getListaUsuario();
                    refreshDataGridView();
                }
                else
                {
                    buttons = MessageBoxButtons.OK;
                    Distar.GUIServices.giveMeAlerts(Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "The selected User could not be unlocked." : "El Usuario seleccionado no se pudo desbloquear.", "Distar", buttons);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            buttons = MessageBoxButtons.YesNo;
            res = Distar.GUIServices.giveMeAlertsWithAction(Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Do you want to block the selected User?" : "Desea bloquear el Usuario seleccionado?", "Distar", buttons);
            if (res == DialogResult.Yes)
            {
                usuarioBL = new Distar_LogicaNegocio.Usuario();
                if (usuarioBL.bloquearUsuario(usuarioSeleccionado.id_usuario))
                {
                    bitacoraBL.setINFO(DateTime.Now, userLog, "Usuarios", "Bloqueo de Usuario.");
                    buttons = MessageBoxButtons.OK;
                    Distar.GUIServices.giveMeAlerts(Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "User blocked." : "Usuario bloqueado.", "Distar", buttons);
                    getListaUsuario();
                    refreshDataGridView();
                }
                else
                {
                    buttons = MessageBoxButtons.OK;
                    Distar.GUIServices.giveMeAlerts(Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "The selected User could not be blocked." : "El Usuario seleccionado no se pudo bloquear.", "Distar", buttons);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Distar.ADMIN.FrmUsuario_alta _FrmUsuario_alta = new Distar.ADMIN.FrmUsuario_alta(userLog);
            _FrmUsuario_alta.Show();
            _FrmUsuario_alta.FrmUsuario_alta_ok += this.OnFrmUsuario_alta;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (usuarioSeleccionado != null)
            {
                Distar.ADMIN.FrmAdmUsuario_patentes _FrmAdmUsuario_patentes = new Distar.ADMIN.FrmAdmUsuario_patentes(userLog, usuarioSeleccionado);
                _FrmAdmUsuario_patentes.Show();
                _FrmAdmUsuario_patentes.FrmAdmUsuario_patentes_asignar_quitar_ok += this.OnFrmAdmUsuario_patentes;
            }
            else
            {
                buttons = MessageBoxButtons.OK;
                Distar.GUIServices.giveMeAlerts(Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "You must select a User from the list." : "Debe seleccionar un Usuario del listado.", "Distar", buttons);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (usuarioSeleccionado != null)
            {
                Distar.ADMIN.FrmAdmUsuario_familias _FrmAdmUsuario_familias = new Distar.ADMIN.FrmAdmUsuario_familias(userLog, usuarioSeleccionado);
                _FrmAdmUsuario_familias.Show();
                _FrmAdmUsuario_familias.FrmAdmUsuario_familias_actualizacion_ok += this.OnFrmAdmUsuario_familias;
            }
            else
            {
                buttons = MessageBoxButtons.OK;
                Distar.GUIServices.giveMeAlerts(Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "You must select a User from the list." : "Debe seleccionar un Usuario del listado.", "Distar", buttons);
            }
        }

        private void getListaUsuario()
        {
            usuarioBL = new Distar_LogicaNegocio.Usuario();
            lista_usuarios = usuarioBL.getAllUsers();
        }

        private void refreshDataGridView()
        {
            dataGridView1.DataSource = null;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns[0].Name = "Nombre";
            dataGridView1.Columns[1].Name = "Apellido";
            dataGridView1.Columns[0].DataPropertyName = "nombre";
            dataGridView1.Columns[1].DataPropertyName = "apellido";
            dataGridView1.DataSource = lista_usuarios;
        }

        private void refreshDetalleUsuario(Distar_EntidadesNegocio.Usuario user)
        {
            label11.Text = user.nombre;
            label12.Text = user.apellido;
            label13.Text = user.documento;
            label14.Text = user.email;
            label15.Text = (user.activo) ? "Activo" : "Baja";
            label16.Text = user.domicilio.direccion;
            label17.Text = user.domicilio.numero_dom;
            label18.Text = user.domicilio.cp;
            label19.Text = user.telefono.telefono;
            label20.Text = user.telefono.telefono_alt;
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
            usuarioSeleccionado = (Distar_EntidadesNegocio.Usuario)selectedRow.DataBoundItem;
            button1.Enabled = (usuarioSeleccionado.activo && servicesBL.validarPatente(1)) ? true : false;
            button4.Enabled = ((usuarioSeleccionado.cont_ingresos_incorrectos < 3) && servicesBL.validarPatente(10)) ? true : false;
            button5.Enabled = ((usuarioSeleccionado.cont_ingresos_incorrectos >= 3) && servicesBL.validarPatente(11)) ? true : false;
            refreshDetalleUsuario(usuarioSeleccionado);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (usuarioSeleccionado != null)
            {
                Distar.PERSONAL.FrmDatosPersonales _FrmDatosPersonales = new Distar.PERSONAL.FrmDatosPersonales(userLog, usuarioSeleccionado);
                _FrmDatosPersonales.Show();
                _FrmDatosPersonales.FrmDatosPersonales_actualizacion_ok += this.OnFrmDatosPersonales;
            }
            else
            {
                buttons = MessageBoxButtons.OK;
                Distar.GUIServices.giveMeAlerts(Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "You must select a User from the list." : "Debe seleccionar un Usuario del listado.", "Distar", buttons);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            buttons = MessageBoxButtons.YesNo;
            res = Distar.GUIServices.giveMeAlertsWithAction(Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Do you want to whiten the password of the selected User?" : "Desea blanquear la contraseña del Usuario seleccionado?", "Distar", buttons);
            if (res == DialogResult.Yes)
            {
                usuarioBL = new Distar_LogicaNegocio.Usuario();
                if (usuarioBL.blanquearUsuario(usuarioSeleccionado))
                {
                    bitacoraBL.setINFO(DateTime.Now, userLog, "Usuarios", "Blanqueo de contraseña a Usuario.");
                    buttons = MessageBoxButtons.OK;
                    Distar.GUIServices.giveMeAlerts(Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "The password of the selected User has been bleached." : "La contraseña del Usuario seleccionado ha sido blanqueada.", "Distar", buttons);
                    usuarioBL.desbloquearUsuario(usuarioSeleccionado.id_usuario);
                    getListaUsuario();
                    refreshDataGridView();
                }
                else
                {
                    buttons = MessageBoxButtons.OK;
                    Distar.GUIServices.giveMeAlerts(Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Failed to perform password blanking for selected User." : "No se pudo realizar el blanqueo de contraseña de Usuario seleccionado.", "Distar", buttons);
                }
            }
        }

        // Alta Usuario
        private void OnFrmUsuario_alta(object sender, Boolean flag)
        {
            getListaUsuario();
            refreshDataGridView();
        }

        // Modificación Usuario
        private void OnFrmDatosPersonales(object sender, Boolean flag)
        {
            getListaUsuario();
            refreshDataGridView();
        }

        // Asignar/Quitar - Negar/Habilitar patente a Usuario
        private void OnFrmAdmUsuario_patentes(object sender, Boolean flag)
        {
            notificarCambios();
            getListaUsuario();
            refreshDataGridView();
        }

        // Asignar/Quitar familia a Usuario
        private void OnFrmAdmUsuario_familias(object sender, Boolean flag)
        {
            notificarCambios();
            getListaUsuario();
            refreshDataGridView();
        }

        public void notificarCambios()
        {
            FrmUsuario_actualizacion_ok.Invoke(this, true);
        }

        private void setPermisosNavegavilidad()
        {
            // AdministrarUsuarios, BloquearUsuario, DesbloquearUsuario, AsignarPatenteUsuario, QuitarPatenteUsuario y NegarPatenteUsuario
            button1.Enabled = servicesBL.validarPatente(1);
            button2.Enabled = servicesBL.validarPatente(1);
            button3.Enabled = servicesBL.validarPatente(1);
            button4.Enabled = servicesBL.validarPatente(10);
            button5.Enabled = servicesBL.validarPatente(11);
            button6.Enabled = servicesBL.validarPatente(12) && servicesBL.validarPatente(13) && servicesBL.validarPatente(14);
            button7.Enabled = servicesBL.validarPatente(1);
            button9.Enabled = servicesBL.validarPatente(1);
        }

        private void setLanguaje()
        {
            this.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Users" : "Usuarios";
            button1.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "DELETE" : "BAJA";
            button2.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "MODIFY" : "MODIFICAR";
            button6.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "ADM. PATENTS" : "ADM. PATENTES";
            button3.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "ADM. FAMILIES" : "ADM. FAMILIAS";
            button4.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "BLOCK" : "BLOQUEAR";
            button5.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "UNBLOCK" : "DESBLOQUEAR";
            button7.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "NEW USER" : "ALTA";
            button9.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "WHITEN USER" : "BLANQUEAR";
            button8.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "CLOSE" : "SALIR";
            toolTip1.SetToolTip(button1, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Delete selected user" : "Eliminar usuario seleccionado");
            toolTip1.SetToolTip(button2, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Modify selected user" : "Modificar usuario seleccionado");
            toolTip1.SetToolTip(button6, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Administer patents of selected user" : "Administrar patentes del usuario seleccionado");
            toolTip1.SetToolTip(button3, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Administer families of selected user" : "Administrar las familias del usuario seleccionado");
            toolTip1.SetToolTip(button4, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Block selected user" : "Bloquear usuario seleccionado");
            toolTip1.SetToolTip(button5, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Unblock selected user" : "Desbloquear usuario seleccionado");
            toolTip1.SetToolTip(button7, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Create new user" : "Crear nuevo usuario");
            toolTip1.SetToolTip(button9, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Whiten selected user" : "Blanquear usuario seleccionado");
        }
    }
}
