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
    public partial class FrmDatosPersonales : Form
    {

        Distar_EntidadesNegocio.Usuario userLog;
        Distar_EntidadesNegocio.Usuario usuarioSeleccionado;
        Distar_EntidadesNegocio.Usuario usuarioModificado;
        Distar_LogicaNegocio.Usuario usuarioBL;
        private Distar_LogicaNegocio.Bitacora bitacoraBL = new Distar_LogicaNegocio.Bitacora();
        Boolean editEmail = false;
        public event EventHandler<Boolean> FrmDatosPersonales_actualizacion_ok;
        /** TEXTO **/
        string gb1Text = "";
        string gb2Text = "";
        string gb3Text = "";
        string lbl1Text = "";
        string lbl11Text = "";
        string lbl12Text = "";
        string lbl13Text = "";
        string lbl2Text = "";
        string lbl3Text = "";
        string lbl4Text = "";
        string lbl5Text = "";
        string lbl6Text = "";
        string btn1Text = "";
        string btn2Text = "";
        string usuarioSuccessMessage = "";
        string usuarioErrMessage = "";

        public FrmDatosPersonales(Distar_EntidadesNegocio.Usuario user=null, Distar_EntidadesNegocio.Usuario u=null): this()
        {
            userLog = user;
            usuarioSeleccionado = u;
        }

        public FrmDatosPersonales()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmDatosPersonales_Load(object sender, EventArgs e)
        {
            foreach (var item in this.Controls)
            {
                if (item.GetType().ToString() == "System.Windows.Forms.GroupBox")
                {
                    ((GroupBox)item).KeyPress += FrmDatosPersonales_KeyPress;
                    foreach (var gb in ((GroupBox)item).Controls)
                    {
                        if (gb.GetType().ToString() == "System.Windows.Forms.TextBox")
                        {
                            ((TextBox)gb).KeyPress += FrmDatosPersonales_KeyPress;
                        }
                    }
                }
            }

            setLanguaje();
            if (usuarioSeleccionado == null)
            {
                this.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Distar - Modify personal data" : "Distar - Modificar datos personales";
                usuarioBL = new Distar_LogicaNegocio.Usuario();
                setFormText(usuarioBL.obtenerUsuarioPorId(userLog.id_usuario), false);
            }
            else
            {
                this.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Distar - Modify user" : "Distar - Modificar usuario";
                setFormText(usuarioSeleccionado, true);
            }
            groupBox1.Text = gb1Text;
            groupBox2.Text = gb2Text;
            groupBox3.Text = gb3Text;
            label1.Text = lbl1Text;
            label2.Text = lbl2Text;
            label3.Text = lbl3Text;
            label4.Text = lbl4Text;
            label5.Text = lbl5Text;
            label6.Text = lbl6Text;
            label11.Text = lbl11Text;
            label12.Text = lbl12Text;
            label13.Text = lbl13Text;
            button1.Text = btn1Text;
            button2.Text = btn2Text;
            this.textBox1.Focus();
            this.textBox1.Select();
        }

        void FrmDatosPersonales_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void setFormText(Distar_EntidadesNegocio.Usuario user, Boolean flag)
        {
            editEmail = flag;
            usuarioModificado = user;
            textBox1.Text = user.nombre;
            textBox2.Text = user.apellido;
            textBox3.Text = user.email;
            textBox4.Text = user.documento;
            textBox8.Text = user.domicilio.direccion;
            textBox9.Text = user.domicilio.numero_dom;
            textBox10.Text = user.domicilio.cp;
            textBox7.Text = user.telefono.telefono;
            textBox11.Text = user.telefono.telefono_alt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            usuarioBL = new Distar_LogicaNegocio.Usuario();
            usuarioModificado.nombre = textBox1.Text;
            usuarioModificado.apellido = textBox2.Text;
            usuarioModificado.documento = textBox4.Text;
            usuarioModificado.email = textBox3.Text;
            usuarioModificado.domicilio.id_usuario = usuarioModificado.id_usuario;
            usuarioModificado.domicilio.direccion = textBox8.Text;
            usuarioModificado.domicilio.numero_dom = textBox9.Text;
            usuarioModificado.domicilio.cp = textBox10.Text;
            usuarioModificado.telefono.id_usuario = usuarioModificado.id_usuario;
            usuarioModificado.telefono.telefono = textBox7.Text;
            usuarioModificado.telefono.telefono_alt = textBox11.Text;
            if (usuarioBL.update(usuarioModificado))
            {
                string func = usuarioSeleccionado == null ? "Personal" : "Usuarios";
                bitacoraBL.setWARNING(DateTime.Now, userLog, func, "Modificación de Usuario.");
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                Distar.GUIServices.giveMeAlerts(usuarioSuccessMessage, "Distar", buttons);
                Close();
                if (usuarioSeleccionado != null)
                {
                    notificarCambios();
                }
            }
            else
            {
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                Distar.GUIServices.giveMeAlerts(usuarioErrMessage, "Distar", buttons);
            }
        }

        public void notificarCambios()
        {
            FrmDatosPersonales_actualizacion_ok.Invoke(this, true);
        }

        private void setLanguaje()
        {
            gb1Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "USER" : "USUARIO";
            gb2Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "ADDRESS" : "DOMICILIO";
            gb3Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "CONTACT" : "CONTACTO";
            lbl1Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Name:" : "Nombre: ";
            lbl11Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Last Name: " : "Apellido: ";
            lbl12Text = "E-mail: ";
            lbl13Text = "C.U.I.T. / Documento: ";
            lbl2Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Address: " : "Dirección: ";
            lbl3Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Address Number: " : "Número: ";
            lbl4Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Postal Code: " : "Código Postal (CP): ";
            lbl5Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Telephone: " : "Teléfono: ";
            lbl6Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Alternative telephone: " : "Teléfono alternativo: ";
            btn1Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "ACCEPT" : "ACEPTAR";
            btn2Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "CLOSE" : "SALIR";
            usuarioSuccessMessage = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Modified user." : "Usuario modificado.";
            usuarioErrMessage = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Modified user - ERROR." : "Usuario modificado - ERROR.";
            toolTip1.SetToolTip(button1, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Accept user changes" : "Aceptar los cambios de usuario");
            toolTip1.SetToolTip(label2, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Street name - Location" : "Nombre calle - Localidad");
            toolTip1.SetToolTip(label2, "C.U.I.T. / Documento");
        }
    }
}
