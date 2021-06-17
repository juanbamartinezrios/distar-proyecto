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
    public partial class FrmPedidos_nuevo : Form
    {
        Distar_EntidadesNegocio.Usuario userLog;
        Distar_EntidadesNegocio.Producto productoSeleccionado;
        Distar_EntidadesNegocio.DetallePedido detalleSeleccionado;
        Distar_LogicaNegocio.Pedido pedidoBL;
        Distar_LogicaNegocio.Producto productoBL;
        List<Distar_EntidadesNegocio.DetallePedido> lista_detalle = new List<Distar_EntidadesNegocio.DetallePedido>();
        List<Distar_EntidadesNegocio.Producto> lista_productos = new List<Distar_EntidadesNegocio.Producto>();
        private Distar_LogicaNegocio.Bitacora bitacoraBL = new Distar_LogicaNegocio.Bitacora();
        MessageBoxButtons buttons;
        public event EventHandler<Boolean> FrmPedidos_nuevo_ok;
        /** TEXTO **/
        string gb2Text = "";
        string gb1Text = "";
        string lbl1Text = "";
        string lbl2Text = "";
        string lbl3Text = "";
        string colHead1 = "";
        string colHead2 = "";
        string colHead3 = "";
        string colHeadSub1 = "";
        string colHeadSub2 = "";
        string colHeadSub3 = "";
        string colHeadSub4 = "";
        string btn1Text = "";
        string btn2Text = "";
        string btn3Text = "";
        string btn4Text = "";
        string itemSelectedMessage = "";
        string newPedidoMessage = "";
        string cantidadAlert = "";
        string cantidadAlert2 = "";
        string cantidadErr = "";

        public FrmPedidos_nuevo(Distar_EntidadesNegocio.Usuario user): this()
        {
            userLog = user;
        }

        public FrmPedidos_nuevo()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Nuevo pedido
        private void button3_Click(object sender, EventArgs e)
        {
            if (lista_detalle.Count > 0)
            {
                Distar_EntidadesNegocio.Pedido pedido = new Distar_EntidadesNegocio.Pedido();
                pedidoBL = new Distar_LogicaNegocio.Pedido();
                pedido.descripcion = "Pedido pendiente de Aceptar-Rechazar.";
                pedido.detalle_items = lista_detalle;
                pedido.estado = "Pendiente";
                pedido.fecha_creacion = DateTime.Now;
                pedido.id_cliente = userLog.id_usuario;
                if (pedidoBL.create(pedido))
                {
                    bitacoraBL.setINFO(DateTime.Now, userLog, "Pedidos", "Pedido creado.");
                    buttons = MessageBoxButtons.OK;
                    Distar.GUIServices.giveMeAlerts(newPedidoMessage.Substring(0, newPedidoMessage.Length-1)+".", "Distar", buttons);
                    Close();
                    notificarCambios();
                }
                else
                {
                    buttons = MessageBoxButtons.OK;
                    Distar.GUIServices.giveMeAlerts(newPedidoMessage.Substring(0, newPedidoMessage.Length - 1) + "- ERROR.", "Distar", buttons);
                }
            }
            else
            {
                buttons = MessageBoxButtons.OK;
                Distar.GUIServices.giveMeAlerts(Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "In order to create the new order you must have at least one Product loaded." : "Para poder crear el nuevo pedido debe tener, al menos, un Producto cargado.", "Distar", buttons);
            }
            Close();
        }

        private void FrmPedidos_nuevo_Load(object sender, EventArgs e)
        {
            setLanguaje();
            groupBox1.Text = gb1Text;
            groupBox2.Text = gb2Text;
            label5.Text = userLog.apellido + ", " + userLog.nombre;
            label1.Text = lbl1Text;
            label2.Text = lbl2Text;
            label3.Text = lbl3Text;
            button1.Text = btn1Text;
            button2.Text = btn2Text;
            button3.Text = btn3Text;
            button4.Text = btn4Text;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.MultiSelect = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridView2.AllowUserToResizeColumns = false;
            dataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.AllowUserToResizeRows = false;
            dataGridView2.MultiSelect = false;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView2.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            this.textBox1.Focus();
            this.textBox1.Select();
            getListaProductos();
            refreshDataGridView2();
        }

        private void getListaProductos()
        {
            productoBL = new Distar_LogicaNegocio.Producto();
            lista_productos = productoBL.getAllProductos();
        }

        private void refreshDataGridView2()
        {
            dataGridView1.Columns[0].HeaderText = colHeadSub1;
            dataGridView1.Columns[1].HeaderText = colHeadSub2;
            dataGridView1.Columns[2].HeaderText = colHeadSub3;
            dataGridView1.Columns[3].HeaderText = colHeadSub4;
            dataGridView2.DataSource = null;
            dataGridView2.AutoGenerateColumns = false;
            dataGridView2.Columns[0].HeaderText = colHead1;
            dataGridView2.Columns[1].HeaderText = colHead2;
            dataGridView2.Columns[2].HeaderText = colHead3;
            dataGridView2.Columns[0].DataPropertyName = "descripcion";
            dataGridView2.Columns[1].DataPropertyName = "p_unitario";
            dataGridView2.Columns[2].DataPropertyName = "stock";
            dataGridView2.Columns[1].DefaultCellStyle.Format = "N2";
            dataGridView2.DataSource = lista_productos;
        }

        private void refreshDataGridView1()
        {
            dataGridView1.DataSource = null;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns[0].HeaderText = colHeadSub1;
            dataGridView1.Columns[1].HeaderText = colHeadSub2;
            dataGridView1.Columns[2].HeaderText = colHeadSub3;
            dataGridView1.Columns[3].HeaderText = colHeadSub4;
            dataGridView1.Columns[0].DataPropertyName = "descripcion_prod";
            dataGridView1.Columns[1].DataPropertyName = "p_unitario_prod";
            dataGridView1.Columns[2].DataPropertyName = "cantidad";
            dataGridView1.Columns[3].DataPropertyName = "importe";
            dataGridView1.Columns[1].DefaultCellStyle.Format = "N2";
            dataGridView1.Columns[3].DefaultCellStyle.Format = "N2";
            dataGridView1.DataSource = lista_detalle;
        }

        // Agregar producto
        private void button2_Click(object sender, EventArgs e)
        {
            int val;
            if (productoSeleccionado != null && (textBox1.Text != "" && int.TryParse(textBox1.Text, out val)))
            {
                if (Convert.ToInt32(textBox1.Text) > productoSeleccionado.stock)
                {
                    buttons = MessageBoxButtons.OK;
                    Distar.GUIServices.giveMeAlerts(cantidadAlert, "Distar", buttons);
                }
                else if (Convert.ToInt32(textBox1.Text) == 0)
                {
                    buttons = MessageBoxButtons.OK;
                    Distar.GUIServices.giveMeAlerts(cantidadErr, "Distar", buttons);
                }
                else
                {
                    if (!lista_detalle.Exists(x => x.id_producto == productoSeleccionado.id_producto))
                    {
                        Distar_EntidadesNegocio.DetallePedido item = new Distar_EntidadesNegocio.DetallePedido();
                        item.id_producto = productoSeleccionado.id_producto;
                        item.importe = Convert.ToInt32(textBox1.Text) * productoSeleccionado.p_unitario;
                        item.p_unitario_prod = productoSeleccionado.p_unitario;
                        item.cantidad = Convert.ToInt32(textBox1.Text);
                        item.descripcion_prod = productoSeleccionado.descripcion;
                        item.producto = productoSeleccionado;
                        lista_detalle.Add(item);
                        textBox1.Clear();
                        refreshDataGridView1();
                    }
                    else
                    {
                        buttons = MessageBoxButtons.OK;
                        Distar.GUIServices.giveMeAlerts(cantidadAlert2, "Distar", buttons);
                    }
                }
            }
            else
            {
                buttons = MessageBoxButtons.OK;
                Distar.GUIServices.giveMeAlerts(cantidadErr, "Distar", buttons);
            }
        }

        private void dataGridView2_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow selectedRow = dataGridView2.Rows[e.RowIndex];
            productoSeleccionado = (Distar_EntidadesNegocio.Producto)selectedRow.DataBoundItem;
        }

        // Quitar producto
        private void button1_Click(object sender, EventArgs e)
        {
            if (detalleSeleccionado != null)
            {
                if (lista_detalle.Exists(x => x.id_producto == detalleSeleccionado.id_producto))
                {
                    lista_detalle.Remove(detalleSeleccionado);
                    refreshDataGridView1();
                }
            }
            else
            {
                buttons = MessageBoxButtons.OK;
                Distar.GUIServices.giveMeAlerts(Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "You must select a detail item to perform this action." : "Debe seleccionar un item del detalle para poder realizar esta acción.", "Distar", buttons);
            }
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
            detalleSeleccionado = (Distar_EntidadesNegocio.DetallePedido)selectedRow.DataBoundItem;
        }

        public void notificarCambios()
        {
            FrmPedidos_nuevo_ok.Invoke(this, true);
        }

        private void setLanguaje()
        {
            gb2Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "PRODUCTS" : "PRODUCTOS";
            gb1Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "ORDER DETAIL" : "DETALLE DE PEDIDO";
            lbl1Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Amount:" : "Cantidad:";
            lbl2Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Select a product and enter the desired quantity, then press 'Add'." : "Seleccione un producto y coloque la cantidad deseada. Luego, presione 'Agregar'.";
            lbl3Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "If you want to remove a product from the order, select it and then press 'Remove'." : "Si desea elimitar un producto del pedido, seleccionelo y luego presione 'Quitar'.";
            colHead1 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Product" : "Producto";
            colHead2 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Price x unit" : "Precio x unidad";
            colHead3 = "Stock";
            colHeadSub1 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Product" : "Producto";
            colHeadSub2 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Price x unit" : "Precio x unidad";
            colHeadSub3 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Quantity" : "Cantidad";
            colHeadSub4 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Amount" : "Importe";
            btn1Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "REMOVE" : "QUITAR";
            btn2Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "ADD" : "AGREGAR";
            btn3Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "ACCEPT" : "ACEPTAR";
            btn4Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "CANCEL" : "CANCELAR";
            itemSelectedMessage = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "You must select a Product from the list and enter a quantity of it." : "Debe seleccionar un Producto del listado e ingresar una cantidad del mismo.";
            newPedidoMessage = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Created order " : "Pedido creado ";
            cantidadAlert = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "The indicated quantity of the selected Product exceeds the available stock of the same." : "La cantidad indicada del Producto seleccionado excede el stock disponible del mismo.";
            cantidadAlert2 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "The selected Product is already added to the detail of the Order." : "El Producto seleccionado ya se encuentra agregado al detalle del Pedido.";
            cantidadErr = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "You must select a Product from the list and enter a valid (numerical) amount." : "Debe seleccionar un Producto del listado e ingresar una cantidad válida (numérica).";
            this.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Distar - New order" : "Distar - Nuevo pedido";
            toolTip1.SetToolTip(button2, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Add selected Product to order detail" : "Agregar el Producto seleccionado al detalle del pedido");
            toolTip1.SetToolTip(button1, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Remove selected Product of order detail" : "Quitar el Producto seleccionado del detalle del pedido");
            toolTip1.SetToolTip(button3, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Create new order" : "Crear nuevo pedido");
        }
    }
}
