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
    public partial class FrmProductos_alta_modificar : Form
    {
        string accion;
        Distar_EntidadesNegocio.Usuario userLog;
        Distar_EntidadesNegocio.Producto productoSeleccionado;
        Distar_LogicaNegocio.Producto productoBL;
        private Distar_LogicaNegocio.Bitacora bitacoraBL = new Distar_LogicaNegocio.Bitacora();
        public event EventHandler<Boolean> FrmProductos_alta_modificacion_ok;
        /** TEXTO **/
        string frmName = "";
        string lbl1Text = "";
        string lbl2Text = "";
        string lbl3Text = "";
        string btn1Text = "";
        string btn2Text = "";
        string newProdMessage = "";
        string updateProdMessage = "";

        public FrmProductos_alta_modificar()
        {
            InitializeComponent();
        }

        public FrmProductos_alta_modificar(Distar_EntidadesNegocio.Usuario user, Distar_EntidadesNegocio.Producto prod, string alta_mod): this()
        {
            userLog = user;
            productoSeleccionado = prod;
            accion = alta_mod;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmProductos_alta_modificar_Load(object sender, EventArgs e)
        {
            foreach (var item in this.Controls)
            {
                if (item.GetType().ToString() == "System.Windows.Forms.TextBox")
                {
                    ((TextBox)item).KeyPress += FrmProductos_alta_modificar_KeyPress;
                }
            }
            setLanguaje();
            this.Text = frmName;
            if (accion == "M")
            {
                textBox1.Text = productoSeleccionado.descripcion;
                textBox2.Text = productoSeleccionado.p_unitario.ToString();
                textBox3.Text = productoSeleccionado.stock.ToString();
            }
            label1.Text = lbl1Text;
            label2.Text = lbl2Text;
            label3.Text = lbl3Text;
            button1.Text = btn1Text;
            button2.Text = btn2Text;
            this.textBox1.Focus();
            this.textBox1.Select();
        }

        void FrmProductos_alta_modificar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                Distar.GUIServices.giveMeAlerts(Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Please complete the form fields." : "Por favor, complete los campos del formulario.", "Distar", buttons);
            }
            else
            {
                int val;
                float valFloat;
                if (int.TryParse(textBox3.Text, out val) && float.TryParse(textBox2.Text, out valFloat))
                {
                    if (accion == "A")
                    {
                        productoSeleccionado = new Distar_EntidadesNegocio.Producto();
                        productoSeleccionado.descripcion = textBox1.Text;
                        productoSeleccionado.p_unitario = Truncate(float.Parse(textBox2.Text));
                        productoSeleccionado.stock = int.Parse(textBox3.Text);
                        productoSeleccionado.estado = "Activo";
                        altaProducto(productoSeleccionado);
                    }
                    else
                    {
                        productoSeleccionado.descripcion = textBox1.Text;
                        productoSeleccionado.p_unitario = Truncate(float.Parse(textBox2.Text));
                        productoSeleccionado.stock = int.Parse(textBox3.Text);
                        modificacionProducto(productoSeleccionado);
                    }
                }
                else
                {
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    Distar.GUIServices.giveMeAlerts(Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Please complete the 'Unit Price' and 'Stock' fields with decimal and integer values ​​respectively." : "Por favor, complete los campos 'Precio Unitario' y 'Stock' con valores decimales y enteros respectivamente.", "Distar", buttons);
                }
            }
        }

        public static float Truncate(float value)
        {
            double mult = Math.Pow(10.0, 2);
            double result = Math.Truncate( mult * value ) / mult;
            return (float) result;
        }

        private void altaProducto(Distar_EntidadesNegocio.Producto prod) {
            productoBL = new Distar_LogicaNegocio.Producto();
            if (productoBL.create(prod))
            {
                bitacoraBL.setINFO(DateTime.Now, userLog, "Producto", "Alta de Producto.");
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                Distar.GUIServices.giveMeAlerts(newProdMessage.Substring(0, newProdMessage.Length-1)+".", "Distar", buttons);
                Close();
                notificarCambios();
            }
            else
            {
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                Distar.GUIServices.giveMeAlerts(newProdMessage+"- ERROR.", "Distar", buttons);
                refreshForm();
            }
        }

        private void modificacionProducto(Distar_EntidadesNegocio.Producto prod) {
            productoBL = new Distar_LogicaNegocio.Producto();
            if (productoBL.update(prod))
            {
                bitacoraBL.setINFO(DateTime.Now, userLog, "Producto", "Modificación de Producto.");
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                Distar.GUIServices.giveMeAlerts(updateProdMessage.Substring(0, updateProdMessage.Length - 1) + ".", "Distar", buttons);
                Close();
                notificarCambios();
            }
            else
            {
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                Distar.GUIServices.giveMeAlerts(updateProdMessage+"- ERROR.", "Distar", buttons);
                refreshForm();
            }
        }

        private void refreshForm()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
        }

        public void notificarCambios()
        {
            FrmProductos_alta_modificacion_ok.Invoke(this, true);
        }

        private void setLanguaje()
        {
            lbl1Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Description:" : "Descripción:";
            lbl2Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Unit Price:" : "Precio Unitario:";
            lbl3Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Stock:" : "Stock:";
            toolTip1.SetToolTip(label1, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Description of the Product" : "Descripción del Producto");
            toolTip1.SetToolTip(label2, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Unit Price of the Product" : "Precio Unitario del Producto");
            toolTip1.SetToolTip(label3, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Stock of the Product" : "Stock del Producto");
            btn1Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "ACCEPT" : "ACEPTAR";
            btn2Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "CLOSE" : "SALIR";
            updateProdMessage = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Modified product " : "Producto modificado";
            newProdMessage = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Created product " : "Producto creado";
            if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
            {
                frmName = (accion == "A") ? "Distar - New product" : "Distar - Update product";
            }
            else
            {
                frmName = (accion == "A") ? "Distar - Nuevo producto" : "Distar - Modificar producto";
            }
        }
    }
}
