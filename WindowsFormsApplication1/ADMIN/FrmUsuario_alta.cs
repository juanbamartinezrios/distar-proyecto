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
    public partial class FrmUsuario_alta : Form
    {
        Distar_EntidadesNegocio.Usuario userLog;
        Distar_LogicaNegocio.Usuario usuarioBL;
        Distar_LogicaNegocio.Familia familiaBL;
        List<Distar_EntidadesNegocio.Familia> lista_familias;
        private Distar_LogicaNegocio.Bitacora bitacoraBL = new Distar_LogicaNegocio.Bitacora();
        public event EventHandler<Boolean> FrmUsuario_alta_ok;

        public FrmUsuario_alta()
        {
            InitializeComponent();
        }

        public FrmUsuario_alta(Distar_EntidadesNegocio.Usuario user): this()
        {
            userLog = user;
        }

        private void FrmUsuario_alta_Load(object sender, EventArgs e)
        {
            foreach (var item in this.Controls)
            {
                if (item.GetType().ToString() == "System.Windows.Forms.GroupBox")
                {
                    ((GroupBox)item).KeyPress += FrmUsuario_alta_KeyPress;
                    foreach (var gb in ((GroupBox)item).Controls)
                    {
                        if (gb.GetType().ToString() == "System.Windows.Forms.TextBox")
                        {
                            ((TextBox)gb).KeyPress += FrmUsuario_alta_KeyPress;
                        }
                    }
                }
            }
            setLanguaje();
            getListaFamilias();
            this.textBox1.Focus();
            this.textBox1.Select();
        }

        void FrmUsuario_alta_KeyPress(object sender, KeyPressEventArgs e)
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
            Distar_EntidadesNegocio.Usuario user = new Distar_EntidadesNegocio.Usuario();
            Distar_EntidadesNegocio.Telefono telefono = new Distar_EntidadesNegocio.Telefono();
            Distar_EntidadesNegocio.Domicilio domicilio = new Distar_EntidadesNegocio.Domicilio();
            user.nombre = textBox1.Text;
            user.apellido = textBox2.Text;
            user.email = textBox3.Text;
            user.documento = textBox4.Text;
            domicilio.direccion = textBox8.Text;
            domicilio.numero_dom = textBox9.Text;
            domicilio.cp = textBox10.Text;
            telefono.telefono = textBox7.Text;
            telefono.telefono_alt = textBox11.Text;
            user.domicilio = domicilio;
            user.telefono = telefono;
            if (checkedListBox1.CheckedItems.Count > 0)
            {
                List<Distar_EntidadesNegocio.Familia> lista_familias = new List<Distar_EntidadesNegocio.Familia>();
                for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
                {
                    Distar_EntidadesNegocio.Familia aux = (Distar_EntidadesNegocio.Familia)checkedListBox1.CheckedItems[i];
                    lista_familias.Add(aux);
                }
                user.familias = lista_familias;
            }
            usuarioBL = new Distar_LogicaNegocio.Usuario();
            if (!usuarioBL.validarUsuarioNuevo(user.email))
            {
                if (usuarioBL.create(user))
                {
                    bitacoraBL.setINFO(DateTime.Now, userLog, "Usuarios", "Usuario creado.");
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    Distar.GUIServices.giveMeAlerts(Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "User created." : "Usuario creado.", "Distar", buttons);
                    Close();
                    notificarCambios();
                }
                else
                {
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    Distar.GUIServices.giveMeAlerts(Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "User created - Error. The user could not be created." : "Usuario creado - Error. No se pudo crear el usuario.", "Distar", buttons);
                    refreshForm();
                }
            }
            else
            {
                bitacoraBL.setWARNING(DateTime.Now, userLog, "Usuarios", "Error en alta de Usuario por e-mail ya existente.");
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                Distar.GUIServices.giveMeAlerts(Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "User created - Error. The user could not be created since the entered e-mail is in use." : "Usuario creado - Error. No se pudo crear el usuario ya que el e-mail ingresado está en uso.", "Distar", buttons);
                refreshForm();
            }
        }

        private void refreshForm()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }

        private void getListaFamilias()
        {
            checkedListBox1.CheckOnClick = true;
            checkedListBox1.SelectionMode = SelectionMode.One;
            checkedListBox1.DisplayMember = "descripcion";
            checkedListBox1.Items.Clear();
            familiaBL = new Distar_LogicaNegocio.Familia();
            lista_familias = familiaBL.getAllFamilias();
            foreach (Distar_EntidadesNegocio.Familia familia in lista_familias)
            {
                checkedListBox1.Items.Add(familia);
            }
        }

        public void notificarCambios()
        {
            FrmUsuario_alta_ok.Invoke(this, true);
        }

        private void setLanguaje()
        {
            this.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Distar - New user" : "Distar - Nuevo usuario";
            toolTip1.SetToolTip(button1, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Create user" : "Crear usuario");
            toolTip1.SetToolTip(label2, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Street name - Location" : "Nombre calle - Localidad");
            toolTip1.SetToolTip(label13, "C.U.I.T. / Documento");
        }
    }
}
