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

namespace Distar
{
    public partial class FrmNavigation : Form
    {
        DialogResult res;
        MessageBoxButtons buttons;
        private Distar_EntidadesNegocio.Usuario userLog;
        private Distar_LogicaNegocio.Usuario usuarioBL;
        private Distar_LogicaNegocio.Bitacora bitacoraBL = new Distar_LogicaNegocio.Bitacora();
        private Distar_LogicaNegocio.Services servicesBL = new Distar_LogicaNegocio.Services();
        private List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO> lista_integridad = new List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO>();
        private string logoutMessage = "";

        public FrmNavigation()
        {
            InitializeComponent();
        }

        public FrmNavigation(Distar_EntidadesNegocio.Usuario user, List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO> lista): this()
        {
            userLog = user;
            lista_integridad = lista;
            loadLanguaje(userLog.id_idioma_usuario);
        }

        private void loadLanguaje(int id_idioma){
            CultureInfo culture;
            if (id_idioma == 1)
            {
                culture = CultureInfo.CreateSpecificCulture("en-US");
            }
            else 
            {
                culture = CultureInfo.CreateSpecificCulture("es-AR");
            }
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Distar.ADMIN.FrmUsuarios _FrmUsuarios = new Distar.ADMIN.FrmUsuarios(userLog);
            _FrmUsuarios.MdiParent = this;
            _FrmUsuarios.StartPosition = FormStartPosition.CenterParent;
            _FrmUsuarios.Show();
            _FrmUsuarios.FrmUsuario_actualizacion_ok += this.OnFrmUsuario;
        }

        private void familiasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Distar.ADMIN.FrmFamilias _FrmFamilias = new Distar.ADMIN.FrmFamilias(userLog);
            _FrmFamilias.MdiParent = this;
            _FrmFamilias.StartPosition = FormStartPosition.CenterParent;
            _FrmFamilias.Show();
            _FrmFamilias.FrmAdmFamilia_actualizacion_ok += this.OnFrmAdmFamilia;
        }

        private void patentesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Distar.ADMIN.FrmPatentes _FrmPatentes = new Distar.ADMIN.FrmPatentes(userLog);
            _FrmPatentes.MdiParent = this;
            _FrmPatentes.StartPosition = FormStartPosition.CenterParent;
            _FrmPatentes.Show();
        }

        private void facturasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Distar.GUIServices.giveMeForms("Distar.FrmFacturas").Show();
        }

        private void pedidosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Distar.FrmPedidos _FrmPedidos = new Distar.FrmPedidos(userLog);
            _FrmPedidos.MdiParent = this;
            _FrmPedidos.StartPosition = FormStartPosition.CenterParent;
            _FrmPedidos.Show();
        }

        private void productosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Distar.FrmProductos _FrmProductos = new Distar.FrmProductos(userLog);
            _FrmProductos.MdiParent = this;
            _FrmProductos.StartPosition = FormStartPosition.CenterParent;
            _FrmProductos.Show();
        }

        private void ticketsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Distar.FrmTickets _FrmTickets = new Distar.FrmTickets(userLog);
            _FrmTickets.MdiParent = this;
            _FrmTickets.StartPosition = FormStartPosition.CenterParent;
            _FrmTickets.Show();
        }

        private void cuentaCorrienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Distar.PERSONAL.FrmCuentaCorriente _FrmCuentaCorriente = new PERSONAL.FrmCuentaCorriente(userLog);
            _FrmCuentaCorriente.MdiParent = this;
            _FrmCuentaCorriente.StartPosition = FormStartPosition.CenterParent;
            _FrmCuentaCorriente.Show();
        }

        private void idiomaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Distar.PERSONAL.FrmIdioma _FrmIdioma = new PERSONAL.FrmIdioma(userLog);
            _FrmIdioma.MdiParent = this;
            _FrmIdioma.StartPosition = FormStartPosition.CenterParent;
            _FrmIdioma.Show();
            _FrmIdioma.FrmIdioma_actualizacion_ok += this.OnFrmIdioma_actualizacion;
        }

        private void datosPersonalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Distar.PERSONAL.FrmDatosPersonales _FrmDatosPersonales = new PERSONAL.FrmDatosPersonales(userLog);
            _FrmDatosPersonales.MdiParent = this;
            _FrmDatosPersonales.StartPosition = FormStartPosition.CenterParent;
            _FrmDatosPersonales.Show();
            _FrmDatosPersonales.FrmDatosPersonales_actualizacion_ok += this.OnFrmDatosPersonales;
        }

        private void cerrarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttons = MessageBoxButtons.YesNo;
            res = Distar.GUIServices.giveMeAlertsWithAction(logoutMessage, "Distar", buttons);
            if (res == DialogResult.Yes)
            {
                bitacoraBL.setINFO(DateTime.Now, userLog, "Logout", "Se realizó un logout.");
                userLog = null;
                Close();
                Distar.GUIServices.giveMeForms("Distar.FrmLogin").Show();
            }
        }

        private void bitácoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Distar.SEGURIDAD.FrmBitacora _FrmBitacora = new SEGURIDAD.FrmBitacora(userLog);
            _FrmBitacora.MdiParent = this;
            _FrmBitacora.StartPosition = FormStartPosition.CenterParent;
            _FrmBitacora.Show();
        }

        private void realizarBackUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Distar.SEGURIDAD.FrmBackup _FrmBackup = new SEGURIDAD.FrmBackup(userLog);
            _FrmBackup.MdiParent = this;
            _FrmBackup.StartPosition = FormStartPosition.CenterParent;
            _FrmBackup.Show();
        }

        private void restaurarBackUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Distar.SEGURIDAD.FrmRestore _FrmRestore = new SEGURIDAD.FrmRestore(userLog);
            _FrmRestore.MdiParent = this;
            _FrmRestore.StartPosition = FormStartPosition.CenterParent;
            _FrmRestore.Show();
        }

        private void cambioDeContraseñaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Distar.SEGURIDAD.FrmCambioContraseña _FrmCambioContraseña = new SEGURIDAD.FrmCambioContraseña(userLog);
            _FrmCambioContraseña.MdiParent = this;
            _FrmCambioContraseña.StartPosition = FormStartPosition.CenterParent;
            _FrmCambioContraseña.Show();
        }

        private void recalcularDigitosVerificadoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lista_integridad.Count > 0)
            {
                buttons = MessageBoxButtons.YesNo;
                res = Distar.GUIServices.giveMeAlertsWithAction(Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "There are integrity inconsistencies in the Database. Do you want to recalculate the Verifier Digits?" : "Hay inconsistencias de integridad en la Base da Datos. Desea recalcular los Dígitos Verificadores?", "Distar", buttons);
                if (res == DialogResult.Yes)
                {
                    recalculateDV();
                }
            }
            else
            {
                buttons = MessageBoxButtons.OK;
                Distar.GUIServices.giveMeAlertsWithAction(Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "There are no integrity inconsistencies in the Database." : "No hay inconsistencias de integridad en la Base da Datos.", "Distar", buttons);
            }
        }

        private void FrmNavigation_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            servicesBL.setPatentesUsuarioLog(userLog);
            setPermisosNavegavilidad();
            setLanguaje();
        }

        private const int CP_DISABLE_CLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle = cp.ClassStyle | CP_DISABLE_CLOSE_BUTTON;
                return cp;
            }
        }

        private void OnFrmIdioma_actualizacion(object sender, Boolean flag)
        {
            usuarioBL = new Distar_LogicaNegocio.Usuario();
            userLog = usuarioBL.obtenerUsuarioPorId(userLog.id_usuario);
        }

        private void OnFrmDatosPersonales(object sender, Boolean flag)
        {
            usuarioBL = new Distar_LogicaNegocio.Usuario();
            userLog = usuarioBL.obtenerUsuarioPorId(userLog.id_usuario);
        }

        private void OnFrmAdmFamilia(object sender, Boolean flag)
        {
            usuarioBL = new Distar_LogicaNegocio.Usuario();
            userLog = usuarioBL.obtenerUsuarioPorId(userLog.id_usuario);
            servicesBL.setPatentesUsuarioLog(userLog);
            setPermisosNavegavilidad();
        }

        private void OnFrmUsuario(object sender, Boolean flag)
        {
            usuarioBL = new Distar_LogicaNegocio.Usuario();
            userLog = usuarioBL.obtenerUsuarioPorId(userLog.id_usuario);
            servicesBL.setPatentesUsuarioLog(userLog);
            setPermisosNavegavilidad();
        }

        private void setLanguaje()
        {
            if (Thread.CurrentThread.CurrentCulture.Name == "en-US"){
                menuStrip1.Items[0].Text = "Administration";
                usuariosToolStripMenuItem.Text = "Users";
                familiasToolStripMenuItem.Text = "Families";
                patentesToolStripMenuItem.Text = "Patents";
                menuStrip1.Items[1].Text = "Invoices";
                menuStrip1.Items[2].Text = "Orders";
                menuStrip1.Items[3].Text = "Products";
                menuStrip1.Items[4].Text = "Tickets";
                menuStrip1.Items[5].Text = "Personal";
                cuentaCorrienteToolStripMenuItem.Text = "Account";
                idiomaToolStripMenuItem.Text = "Language";
                datosPersonalesToolStripMenuItem.Text = "Personal Info.";
                menuStrip1.Items[6].Text = "Security";
                bitácoraToolStripMenuItem.Text = "Logs";
                realizarBackUpToolStripMenuItem.Text = "Back-up";
                restaurarBackUpToolStripMenuItem.Text = "Restore";
                cambioDeContraseñaToolStripMenuItem.Text = "Password change";
                recalcularDigitosVerificadoresToolStripMenuItem.Text = "Recalculate Check Digits";
                cerrarSesiónToolStripMenuItem.Text = "Log-out";
                logoutMessage = "Are you sure you want to log out?";
            } else {
                menuStrip1.Items[0].Text = "Administración";
                usuariosToolStripMenuItem.Text = "Usuarios";
                familiasToolStripMenuItem.Text = "Familias";
                patentesToolStripMenuItem.Text = "Patentes";
                menuStrip1.Items[1].Text = "Facturas";
                menuStrip1.Items[2].Text = "Pedidos";
                menuStrip1.Items[3].Text = "Productos";
                menuStrip1.Items[4].Text = "Tickets";
                menuStrip1.Items[5].Text = "Personal";
                cuentaCorrienteToolStripMenuItem.Text = "Cuenta Corriente";
                idiomaToolStripMenuItem.Text = "Idioma";
                datosPersonalesToolStripMenuItem.Text = "Datos Personales";
                menuStrip1.Items[6].Text = "Seguridad";
                bitácoraToolStripMenuItem.Text = "Bitácora";
                realizarBackUpToolStripMenuItem.Text = "Realizar Back-up";
                restaurarBackUpToolStripMenuItem.Text = "Restaurar Back-up";
                cambioDeContraseñaToolStripMenuItem.Text = "Cambio de contraseña";
                recalcularDigitosVerificadoresToolStripMenuItem.Text = "Recalcular Dígitos Verificadores";
                cerrarSesiónToolStripMenuItem.Text = "Cerrar Sesión";
                logoutMessage = "Seguro que desea cerrar sesión?";
            }
        }

        private void setPermisosNavegavilidad()
        {
            // Administracion
            menuStrip1.Items[0].Enabled = (servicesBL.validarPatente(1) && servicesBL.validarPatente(2) && servicesBL.validarPatente(3)) || servicesBL.validarPatente(5) || servicesBL.validarPatente(6);
            usuariosToolStripMenuItem.Enabled = servicesBL.validarPatente(1) || (servicesBL.validarPatente(2) && servicesBL.validarPatente(3));
            familiasToolStripMenuItem.Enabled = servicesBL.validarPatente(5);
            patentesToolStripMenuItem.Enabled = servicesBL.validarPatente(6);
            // Facturas
            menuStrip1.Items[1].Enabled = servicesBL.validarPatente(15) && servicesBL.validarPatente(16);
            // Pedidos
            menuStrip1.Items[2].Enabled = servicesBL.validarPatente(17) && servicesBL.validarPatente(18);
            // Productos
            menuStrip1.Items[3].Enabled = servicesBL.validarPatente(24);
            // Tickets
            menuStrip1.Items[4].Enabled = servicesBL.validarPatente(21) && servicesBL.validarPatente(22);
            // Personal
            menuStrip1.Items[5].Enabled = servicesBL.validarPatente(23) || servicesBL.validarPatente(25);
            idiomaToolStripMenuItem.Enabled = servicesBL.validarPatente(23);
            datosPersonalesToolStripMenuItem.Enabled = servicesBL.validarPatente(25);
            // Seguridad
            menuStrip1.Items[6].Enabled = servicesBL.validarPatente(4) || servicesBL.validarPatente(7) || servicesBL.validarPatente(8) || servicesBL.validarPatente(9);
            bitácoraToolStripMenuItem.Enabled = servicesBL.validarPatente(9);
            realizarBackUpToolStripMenuItem.Enabled = servicesBL.validarPatente(7);
            restaurarBackUpToolStripMenuItem.Enabled = servicesBL.validarPatente(8);
            cambioDeContraseñaToolStripMenuItem.Enabled = servicesBL.validarPatente(23);
            recalcularDigitosVerificadoresToolStripMenuItem.Enabled = servicesBL.validarPatente(26);
        }

        private void shutDown()
        {
            Application.Exit();
        }

        private void recalculateDV()
        {
            List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO> lista_RDVV = new List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO>();
            List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO> lista_RDVH = new List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO>();
            Distar_LogicaNegocio.DigitoVerificador DVBL = new Distar_LogicaNegocio.DigitoVerificador();
            foreach (Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO item in lista_integridad)
            {   
                if (item.txtstr != null)
                {
                    lista_RDVH.Add(item);
                }
                else
                {
                    lista_RDVV.Add(item);
                }
            }
            if (lista_RDVV.Count > 0) {
                if (DVBL.recalcularDVV_lista(lista_RDVV))
                {
                    foreach (var item in lista_RDVV)
                    {
                        bitacoraBL.setWARNING(DateTime.Now, userLog, "Integridad de datos", "Actualización de DV - Entidad: " + item.entidad);
                    }
                    buttons = MessageBoxButtons.OK;
                    Distar.GUIServices.giveMeAlertsWithAction(Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "The inconsistencies of data in the DVV entity were corrected." : "Se corrigieron las inconsistencias de datos en la entidad DVV.", "Distar", buttons);
                }
                else
                {
                    bitacoraBL.setERROR(DateTime.Now, userLog, "Integridad de datos", "Fallo en actualización de DV");
                    buttons = MessageBoxButtons.OK;
                    Distar.GUIServices.giveMeAlertsWithAction(Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "An error occurred while trying to correct data inconsistencies in the DVV entity." : "Ocurrió un error al intentar corregir las inconsistencias de datos en la entidad DVV.", "Distar", buttons);
                }
            }
            if (lista_RDVH.Count > 0)
            {
                int acum_ok = 0;
                foreach (var item in lista_RDVH)
                {
                    if (DVBL.recalcularDVPorEntidadRegistro(item.entidad, item.id_registro, item.valor_calc.ToString()))
                    {
                        bitacoraBL.setWARNING(DateTime.Now, userLog, "Integridad de datos", "Actualización de DVH - Entidad: " + item.entidad + " Registro (ID): " + item.id_registro);
                        acum_ok++;
                    }
                    else
                    {
                        bitacoraBL.setWARNING(DateTime.Now, userLog, "Integridad de datos", "Fallo en actualización de DVH - Entidad: " + item.entidad + " Registro (ID): " + item.id_registro);
                        acum_ok--;
                    }
                }
                if (acum_ok == lista_RDVH.Count)
                {
                    buttons = MessageBoxButtons.OK;
                    Distar.GUIServices.giveMeAlertsWithAction(Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "The inconsistencies of data in the affected records were corrected." : "Se corrigieron las inconsistencias de datos en los registros afectados.", "Distar", buttons);
                }
                else
                {
                    buttons = MessageBoxButtons.OK;
                    Distar.GUIServices.giveMeAlertsWithAction(Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "An error occurred when trying to correct data inconsistencies in affected records." : "Ocurrió un error al intentar corregir las inconsistencias de datos en los registros afectados.", "Distar", buttons);
                }
            }
        }

        private void FrmNavigation_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
