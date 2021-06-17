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
    public partial class FrmProductos : Form
    {
        Distar_LogicaNegocio.Producto productoBL;
        Distar_EntidadesNegocio.Producto productoSeleccionado;
        Distar_EntidadesNegocio.Usuario userLog;
        List<Distar_EntidadesNegocio.Producto> lista_productos;
        private Distar_LogicaNegocio.Bitacora bitacoraBL = new Distar_LogicaNegocio.Bitacora();
        DialogResult res;
        MessageBoxButtons buttons;
        /** TEXTO **/
        string btn1Text = "";
        string btn2Text = "";
        string btn3Text = "";
        string btn4Text = "";
        string btn3ErrMessage = "";
        string btn3SuccessMessage = "";
        string btn3TextMessage = "";
        string itemSelectedMessage = "";

        public FrmProductos()
        {
            InitializeComponent();
        }

        public FrmProductos(Distar_EntidadesNegocio.Usuario user): this()
        {
            userLog = user;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmProductos_Load(object sender, EventArgs e)
        {
            setLanguaje();
            button1.Text = btn1Text;
            button2.Text = btn2Text;
            button3.Text = btn3Text;
            button5.Text = btn4Text;
            getListaProductos();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (productoSeleccionado != null)
            {
                buttons = MessageBoxButtons.YesNo;
                res = Distar.GUIServices.giveMeAlertsWithAction(btn3TextMessage, "Distar", buttons);
                if (res == DialogResult.Yes)
                {
                    productoBL = new Distar_LogicaNegocio.Producto();
                    if (productoBL.deleteLogico(productoSeleccionado))
                    {
                        bitacoraBL.setINFO(DateTime.Now, userLog, "Producto", "Baja de Producto.");
                        buttons = MessageBoxButtons.OK;
                        Distar.GUIServices.giveMeAlerts(btn3SuccessMessage, "Distar", buttons);
                        getListaProductos();
                    }
                    else
                    {
                        buttons = MessageBoxButtons.OK;
                        Distar.GUIServices.giveMeAlerts(btn3ErrMessage, "Distar", buttons);
                    }
                }
            }
            else
            {
                buttons = MessageBoxButtons.OK;
                Distar.GUIServices.giveMeAlerts(itemSelectedMessage, "Distar", buttons);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (productoSeleccionado != null)
            {
                Distar.FrmProductos_alta_modificar _FrmProductos_alta_modificar = new Distar.FrmProductos_alta_modificar(userLog, productoSeleccionado, "M");
                _FrmProductos_alta_modificar.Show();
                _FrmProductos_alta_modificar.FrmProductos_alta_modificacion_ok += this.OnFrmProductos_alta_modificar;
            }
            else
            {
                buttons = MessageBoxButtons.OK;
                Distar.GUIServices.giveMeAlerts(itemSelectedMessage, "Distar", buttons);
            }
        }

        private void getListaProductos()
        {
            listBox1.Items.Clear();
            listBox1.DisplayMember = "descripcion";
            productoBL = new Distar_LogicaNegocio.Producto();
            lista_productos = productoBL.getAllProductos();
            foreach (Distar_EntidadesNegocio.Producto prod in lista_productos)
            {
                if (prod.estado == "Activo") listBox1.Items.Add(prod);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Distar.FrmProductos_alta_modificar _FrmProductos_alta_modificar = new Distar.FrmProductos_alta_modificar(userLog, null, "A");
            _FrmProductos_alta_modificar.Show();
            _FrmProductos_alta_modificar.FrmProductos_alta_modificacion_ok += this.OnFrmProductos_alta_modificar;
        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            productoSeleccionado = (Distar_EntidadesNegocio.Producto)listBox1.SelectedItem;
        }

        private void OnFrmProductos_alta_modificar(object sender, Boolean flag)
        {
            getListaProductos();
        }

        private void setLanguaje()
        {
            btn1Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "CREATE" : "CREAR";
            btn2Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "UPDATE" : "MODIFICAR";
            btn3Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "DELETE" : "ELIMINAR";
            btn4Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "CLOSE" : "SALIR";
            toolTip1.SetToolTip(button1, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Create new Product" : "Crear nuevo Producto");
            toolTip1.SetToolTip(button2, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Update selected Product" : "Modificar Producto seleccionado");
            toolTip1.SetToolTip(button3, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Delete selected Product" : "Eliminar Producto seleccionado");
            btn3ErrMessage = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "The selected Product could not be eliminated." : "El Producto seleccionado no se pudo eliminar.";
            btn3SuccessMessage = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "The selected Product has been removed." : "Se ha eliminado el Producto seleccionado.";
            btn3TextMessage = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Do you want to delete the selected Product?" : "Desea eliminar el Producto seleccionado?";
            itemSelectedMessage = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "You must select a Product from the list." : "Debe seleccionar un Producto del listado.";
            this.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Distar - Products" : "Distar - Productos";
        }
    }
}
