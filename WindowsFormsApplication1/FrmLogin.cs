using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Distar
{
    public partial class FrmLogin : Form
    {
        private string email = "";
        private string pw = "";
        Distar_EntidadesNegocio.Usuario userLog;
        Distar_EntidadesNegocio.Usuario userAux;
        Distar_LogicaNegocio.DigitoVerificador digitoVerificadorBL;
        Distar_LogicaNegocio.Bitacora bitacoraBL = new Distar_LogicaNegocio.Bitacora();
        Distar_LogicaNegocio.Services servicesBL = new Distar_LogicaNegocio.Services();

        public FrmLogin()
        {
            this.FormClosing += appExit;
            InitializeComponent();
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            foreach (var item in this.Controls)
            {
                if (item.GetType().ToString() == "System.Windows.Forms.TextBox")
                {
                    ((TextBox)item).KeyPress += FrmLogin_KeyPress;
                }
            }
            this.textBox1.Focus();
            this.textBox1.Select();
            textBox2.UseSystemPasswordChar = true;
            toolTip1.SetToolTip(label1, "E-mail con el cual se dió de alta en el Sistema");
            toolTip1.SetToolTip(label2, "Contraseña para el ingreso al Sistema");
            toolTip1.SetToolTip(button1, "Realizar login e ingresar al Sistema");
            toolTip1.SetToolTip(button2, "Realizar la conexión a la Base de Datos");
            toolTip1.SetToolTip(label3, "Servidor en el cual está alojada la Base de Datos");
            toolTip1.SetToolTip(label4, "Nombre de la Base de Datos");
            toolTip1.SetToolTip(label5, "Contraseña de usuario de la Base de Datos. Dato opcional");
            toolTip1.SetToolTip(label6, "Usuario (Administrador) de la Base de Datos. Dato opcional");
            Distar_LogicaNegocio.Services servicesBL = new Distar_LogicaNegocio.Services();
            if (servicesBL.existeConnectionstring())
            {
                string connectionString;
                connectionString = servicesBL.getConnectionString();
                try
                {
                    connectionString = servicesBL.DesencriptarASCII(connectionString);
                }
                catch (Exception ex)
                {
                    connectionString = "";
                }
                if (!servicesBL.probarConexion(connectionString))
                {
                    button1.Visible = false;
                    label1.Visible = false;
                    label2.Visible = false;
                    textBox1.Visible = false;
                    textBox2.Visible = false;
                    button2.Visible = true;
                    label3.Visible = true;
                    label4.Visible = true;
                    label6.Visible = true;
                    label5.Visible = true;
                    textBox3.Visible = true;
                    textBox4.Visible = true;
                    textBox6.Visible = true;
                    textBox5.Visible = true;
                }
                else
                {
                    button1.Visible = true;
                    button1.Visible = true;
                    label1.Visible = true;
                    label2.Visible = true;
                    textBox1.Visible = true;
                    textBox2.Visible = true;
                    button2.Visible = false;
                    label3.Visible = false;
                    label4.Visible = false;
                    label6.Visible = false;
                    label5.Visible = false;
                    textBox3.Visible = false;
                    textBox4.Visible = false;
                    textBox6.Visible = false;
                    textBox5.Visible = false;
                }
            }
        }

        void FrmLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            email = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            pw = textBox2.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Distar_LogicaNegocio.Usuario usuarioBL = new Distar_LogicaNegocio.Usuario();
            if (email != "" && pw != "")
            {
                if (loginUsuario(email, pw))
                {
                    if (userLog.cont_ingresos_incorrectos >= 3)
                    {
                        validarUsuario();
                    }
                    else if (!userLog.activo)
                    {
                        bitacoraBL.setINFO(DateTime.Now, userLog, "Login", "Usuario dado de baja. No se puede realizar login.");
                        MessageBoxButtons buttons = MessageBoxButtons.OK;
                        GUIServices.giveMeAlerts("Usuario dado de baja. Por favor, póngase en contacto con el Administrador.", "Distar", buttons);
                    }
                    else
                    {
                        digitoVerificadorBL = new Distar_LogicaNegocio.DigitoVerificador();
                        List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO> lista = new List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO>();
                        lista = digitoVerificadorBL.verificarIntegridad(userLog);
                        if (lista.Count > 0)
                        {
                            this.Hide();
                            Distar.SEGURIDAD.FrmIntegridad _FrmIntegridad = new SEGURIDAD.FrmIntegridad(userLog, lista);
                            _FrmIntegridad.Show();
                        }
                        else
                        {
                            this.Hide();
                            Distar.FrmNavigation _FrmNavigation = new Distar.FrmNavigation(userLog, lista);
                            _FrmNavigation.Show();
                        }
                    }
                }
                else
                {
                    if (usuarioBL.obtenerContIngresosIncorrectos(email) >= 3)
                    {
                        validarUsuario();
                    }
                    else
                    {
                        bitacoraBL.setINFO(DateTime.Now, userLog, "Login", "Login incorrecto.");
                        MessageBoxButtons buttons = MessageBoxButtons.OK;
                        GUIServices.giveMeAlerts("Login incorrecto.", "Distar", buttons);
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox1.Focus();
                    }
                }
            }
            else
            {
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                GUIServices.giveMeAlerts("Por favor, complete los campos de E-mail y Contraseña.", "Distar", buttons);
                textBox1.Clear();
                textBox2.Clear();
                textBox1.Focus();
            }
        }

        private void validarUsuario()
        {
            bitacoraBL.setINFO(DateTime.Now, userLog, "Login", "Usuario bloqueado. No se puede realizar login.");
            if (servicesBL.validarPatente(11))
            {
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult res = GUIServices.giveMeAlertsWithAction("Usuario bloqueado. Al seleccionar el botón 'Aceptar' se le generará una nueva contraseña para ingresar al Sistema.", "Distar", buttons);
                if (res == DialogResult.OK)
                {
                    desbloquearConNuevaPw(userAux);
                }
            }
            else
            {
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                GUIServices.giveMeAlerts("Usuario bloqueado. Por favor, póngase en contacto con el Administrador para blanquear su contraseña.", "Distar", buttons);
                textBox1.Clear();
                textBox2.Clear();
                textBox1.Focus();
            }
        }

        private void desbloquearConNuevaPw(Distar_EntidadesNegocio.Usuario user)
        {
            Distar_LogicaNegocio.Usuario usuarioBL = new Distar_LogicaNegocio.Usuario();
            if (usuarioBL.desbloquearUsuario(user.id_usuario))
            {
                bitacoraBL.setINFO(DateTime.Now, userLog, "Usuarios", "Desbloqueo de Usuario.");
                if (usuarioBL.blanquearUsuario(user))
                {
                    bitacoraBL.setINFO(DateTime.Now, userLog, "Usuarios", "Blanqueo de contraseña a Usuario.");
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    Distar.GUIServices.giveMeAlerts("Se desbloqueó el usuario y se generó una nueva contraseña. Por favor, intente nuevamente el log-in.", "Distar", buttons);
                    usuarioBL.desbloquearUsuario(user.id_usuario);
                }
                else
                {
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    Distar.GUIServices.giveMeAlerts("No se pudo generar una nueva contraseña para el usuario.", "Distar", buttons);
                }
            }
            else
            {
                bitacoraBL.setINFO(DateTime.Now, userLog, "Usuarios", "Error en desbloqueo de Usuario.");
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                Distar.GUIServices.giveMeAlerts("No se pudo desbloquear el usuario.", "Distar", buttons);
            }
            textBox1.Clear();
            textBox2.Clear();
            textBox1.Focus();
        }

        private Boolean loginUsuario(string email, string pw)
        {
            Distar_LogicaNegocio.Usuario usuarioBL = new Distar_LogicaNegocio.Usuario();
            userLog = new Distar_EntidadesNegocio.Usuario();
            userLog = usuarioBL.autenticarUsuario(email, pw);
            if (userLog.id_usuario != 0)
            {
                servicesBL.setPatentesUsuarioLog(userLog);
                return true;
            }
            else
            {
                usuarioBL.aumentarContIngresosIncorrectos(email);
                userAux = usuarioBL.obtenerUsuarioPorEmail(email);
                if (userAux != null)
                {
                    servicesBL.setPatentesUsuarioLog(userAux);
                }
                return false;
            }
        }

        private void appExit(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Distar_LogicaNegocio.Services servicesBL = new Distar_LogicaNegocio.Services();
            if (servicesBL.probarConexion(textBox6.Text, textBox5.Text, textBox4.Text, textBox3.Text, true))
            {
                servicesBL.actualizarConexion(textBox6.Text, textBox5.Text, textBox4.Text, textBox3.Text);
                servicesBL.setConnextionString(textBox6.Text, textBox5.Text, textBox4.Text, textBox3.Text, true);
                button1.Visible = true;
                button1.Visible = true;
                label1.Visible = true;
                label2.Visible = true;
                textBox1.Visible = true;
                textBox2.Visible = true;
                button2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label6.Visible = false;
                label5.Visible = false;
                textBox3.Visible = false;
                textBox4.Visible = false;
                textBox6.Visible = false;
                textBox5.Visible = false;
            }
            else
            {
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                GUIServices.giveMeAlerts("No se pudo realizar una conexión satisfactoria a la Base de Datos indicada. Por favor, verifique los datos ingresados e intente nuevamente.", "Distar", buttons);
            }
        }
    }
}
