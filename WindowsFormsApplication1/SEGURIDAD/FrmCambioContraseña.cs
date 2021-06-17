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

namespace Distar.SEGURIDAD
{
    public partial class FrmCambioContraseña : Form
    {
        DialogResult res;
        MessageBoxButtons buttons;
        private Distar_EntidadesNegocio.Usuario userLog;
        private Distar_LogicaNegocio.Usuario usuarioBL;
        private Distar_LogicaNegocio.Bitacora bitacoraBL = new Distar_LogicaNegocio.Bitacora();
        /** TEXTO **/
        string lbl1Text = "";
        string lbl3Text = "";
        string lbl2Text = "";
        string btn1Text = "";
        string btn2Text = "";
        string cambiopwSuccessMessage = "";
        string cambiopwErrMessage = "";

        public FrmCambioContraseña()
        {
            InitializeComponent();
        }

        public FrmCambioContraseña(Distar_EntidadesNegocio.Usuario user): this()
        {
            userLog = user;
        }

        private void FrmCambioContraseña_Load(object sender, EventArgs e)
        {
            foreach (var item in this.Controls)
            {
                if (item.GetType().ToString() == "System.Windows.Forms.TextBox")
                {
                    ((TextBox)item).KeyPress += FrmCambioContraseña_KeyPress;
                }
            }
            setLanguaje();
            label1.Text = lbl1Text;
            label2.Text = lbl2Text;
            label3.Text = lbl3Text;
            button1.Text = btn1Text;
            button2.Text = btn2Text;
            textBox1.UseSystemPasswordChar = true;
            textBox2.UseSystemPasswordChar = true;
            textBox3.UseSystemPasswordChar = true;
            this.textBox1.Focus();
            this.textBox1.Select();
        }

        void FrmCambioContraseña_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            buttons = MessageBoxButtons.OK;
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "")
            {
                if (textBox2.Text != textBox3.Text)
                {
                    Distar.GUIServices.giveMeAlerts(Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "The new password does not match the confirmation." : "La contreña nueva no coincide con la confirmación.", "Distar", buttons);
                }
                else
                {
                    usuarioBL = new Distar_LogicaNegocio.Usuario();
                    if (usuarioBL.cambiarContraseña(userLog, textBox1.Text, textBox3.Text))
                    {
                        bitacoraBL.setINFO(DateTime.Now, userLog, "Seguridad", "Cambio de contraseña.");
                        res = Distar.GUIServices.giveMeAlertsWithAction(cambiopwSuccessMessage, "Distar", buttons);
                        if (res == DialogResult.OK)
                        {
                            Close();
                        }
                    }
                    else
                    {
                        Distar.GUIServices.giveMeAlerts(cambiopwErrMessage, "Distar", buttons);
                    }
                }
            }
            else
            {
                Distar.GUIServices.giveMeAlerts(Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Please complete the form fields." : "Por favor, complete los campos del formulario.", "Distar", buttons);
            }
        }

        private void setLanguaje()
        {
            lbl1Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Current password: " : "Contraseña actual: ";
            lbl3Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "New password: " : "Nueva contraseña: ";
            lbl2Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Confirm password: " : "Confirmar contraseña: ";
            btn1Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "CONFIRM" : "CONFIRMAR";
            btn2Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "CANCEL" : "CANCELAR";
            cambiopwSuccessMessage = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Password change made." : "Cambio de contraseña realizado.";
            cambiopwErrMessage = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Password change - Error" : "Cambio de contraseña - Error";
            this.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Distar - Password change" : "Distar - Cambio de contraseña";
            toolTip1.SetToolTip(label1, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Enter the current password" : "Ingrese su contraseña actual");
            toolTip1.SetToolTip(label2, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Enter the new password" : "Ingrese la nueva contraseña");
            toolTip1.SetToolTip(label3, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Confirm the new password" : "Confirme la nueva contraseña");
        }
    }
}
