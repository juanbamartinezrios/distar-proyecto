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
    public partial class FrmConfirmar_factura : Form
    {
        Distar_EntidadesNegocio.Usuario userLog;
        Distar_EntidadesNegocio.DetallePedido detalleSeleccionado;
        Distar_EntidadesNegocio.Pedido pedidoSeleccionado;
        Distar_LogicaNegocio.Pedido pedidoBL;
        Distar_LogicaNegocio.Factura facturaBL;
        List<Distar_EntidadesNegocio.DetallePedido> lista_detalle = new List<Distar_EntidadesNegocio.DetallePedido>();
        List<Distar_EntidadesNegocio.DetalleFactura> lista_detalle_factura = new List<Distar_EntidadesNegocio.DetalleFactura>();
        private Distar_LogicaNegocio.Bitacora bitacoraBL = new Distar_LogicaNegocio.Bitacora();
        private float totalIva = 0;
        private float totalPedido = 0;
        MessageBoxButtons buttons;
        public event EventHandler<Boolean> FrmConfirmar_factura_ok;
        /** TEXTO **/
        string gb1Text = "";
        string lbl1Text = "";
        string lbl2Text = "";
        string lbl6Text = "";
        string lbl5Text = "";
        string lbl4Text = "";
        string lbl3Text = "";
        string lbl7Text = "";
        string lbl8Text = "";
        string lbl9Text = "";
        string colHead1 = "";
        string colHead2 = "";
        string colHead3 = "";
        string colHead4 = "";
        string btn1Text = "";
        string btn3Text = "";
        string btn4Text = "";
        string facturaSuccessMessage = "";
        string facturaErrMessage = "";

        public FrmConfirmar_factura(Distar_EntidadesNegocio.Usuario user, Distar_EntidadesNegocio.Pedido pedido): this()
        {
            userLog = user;
            pedidoSeleccionado = pedido;
        }

        public FrmConfirmar_factura()
        {
            InitializeComponent();
        }

        private void FrmConfirmar_factura_Load(object sender, EventArgs e)
        {
            setLanguaje();
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
            groupBox1.Text = gb1Text;
            label3.Text = lbl3Text;
            label1.Text = lbl1Text + pedidoSeleccionado.fecha_creacion.ToShortDateString();
            label2.Text = lbl2Text + pedidoSeleccionado.nro_pedido.ToString();
            label4.Text = lbl4Text + pedidoSeleccionado.cliente.domicilio.direccion + " " + pedidoSeleccionado.cliente.domicilio.numero_dom.Trim() + " - CP: " + pedidoSeleccionado.cliente.domicilio.cp;
            label5.Text = lbl5Text + pedidoSeleccionado.cliente.documento;
            label6.Text = lbl6Text + pedidoSeleccionado.cliente.apellido + ", " + pedidoSeleccionado.cliente.nombre;
            label7.Text = lbl7Text;
            label8.Text = lbl8Text;
            label9.Text = lbl9Text;
            button1.Text = btn1Text;
            button3.Text = btn3Text;
            button4.Text = btn4Text;
            lista_detalle = pedidoSeleccionado.detalle_items;
            refreshDataGridView1();
        }

        private void refreshDataGridView1()
        {
            dataGridView1.DataSource = null;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns[0].HeaderText = colHead1;
            dataGridView1.Columns[1].HeaderText = colHead2;
            dataGridView1.Columns[2].HeaderText = colHead3;
            dataGridView1.Columns[3].HeaderText = colHead4;
            dataGridView1.Columns[0].DataPropertyName = "descripcion_prod";
            dataGridView1.Columns[1].DataPropertyName = "p_unitario_prod";
            dataGridView1.Columns[2].DataPropertyName = "cantidad";
            dataGridView1.Columns[3].DataPropertyName = "importe";
            dataGridView1.Columns[1].DefaultCellStyle.Format = "N2";
            dataGridView1.Columns[3].DefaultCellStyle.Format = "N2";
            dataGridView1.DataSource = lista_detalle;
            refreshTotales();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (lista_detalle.Exists(x => x.id_producto == detalleSeleccionado.id_producto))
            {
                pedidoSeleccionado.total -= detalleSeleccionado.importe;
                lista_detalle.Remove(detalleSeleccionado);
                refreshDataGridView1();
            }
            if (lista_detalle.Count == 0)
            {
                button3.Enabled = false;
            }
            refreshTotales();
        }

        private void refreshTotales()
        {
            label7.Text = lbl7Text + String.Format("{0:C}", pedidoSeleccionado.total);
            totalIva = pedidoSeleccionado.total * 21 / 100;
            label8.Text = lbl8Text + String.Format("{0:C}", totalIva);
            totalPedido = pedidoSeleccionado.total + totalIva;
            label9.Text = lbl9Text + String.Format("{0:C}", totalPedido);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Generar Factura-Ticket y eliminar Pedido
        private void button3_Click(object sender, EventArgs e)
        {
            facturaBL = new Distar_LogicaNegocio.Factura();
            foreach (var item in lista_detalle)
            {
                Distar_EntidadesNegocio.DetalleFactura df = new Distar_EntidadesNegocio.DetalleFactura();
                df.cantidad = item.cantidad;
                df.id_producto = item.id_producto;
                df.nro_detalle = item.nro_detalle;
                df.importe = item.importe;
                lista_detalle_factura.Add(df);
            }
            Distar_EntidadesNegocio.Factura factura = new Distar_EntidadesNegocio.Factura();
            factura.id_cliente = pedidoSeleccionado.id_cliente;
            factura.detalle_items = lista_detalle_factura;
            factura.fecha_emision = DateTime.Now;
            factura.iva = 21;
            factura.tipo_factura = "A";
            Distar_EntidadesNegocio.Ticket ticket = new Distar_EntidadesNegocio.Ticket();
            ticket.estado = "Impago";
            ticket.fecha_emision = DateTime.Now;
            ticket.id_cliente = pedidoSeleccionado.id_cliente;
            ticket.total = totalPedido;
            ticket.cliente = pedidoSeleccionado.cliente;
            if (facturaBL.create(factura, ticket))
            {
                bitacoraBL.setINFO(DateTime.Now, userLog, "Facturas", "Factura creada.");
                bitacoraBL.setINFO(DateTime.Now, userLog, "Tickets", "Ticket creado para el Usuario: "+ticket.cliente.apellido+", "+ticket.cliente.nombre+".");
                buttons = MessageBoxButtons.OK;
                Distar.GUIServices.giveMeAlerts(facturaSuccessMessage, "Distar", buttons);
                pedidoBL = new Distar_LogicaNegocio.Pedido();
                pedidoBL.delete(pedidoSeleccionado);
                Close();
                notificarCambios();
            }
            else
            {
                buttons = MessageBoxButtons.OK;
                Distar.GUIServices.giveMeAlerts(facturaErrMessage, "Distar", buttons);
            }
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
            detalleSeleccionado = (Distar_EntidadesNegocio.DetallePedido)selectedRow.DataBoundItem;
        }

        public void notificarCambios()
        {
            FrmConfirmar_factura_ok.Invoke(this, true);
        }

        private void setLanguaje()
        {
            gb1Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "ORDER DETAIL" : "DETALLE DE PEDIDO";
            lbl1Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Issue Date:" : "Fecha de creación: ";
            lbl2Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Order Number: " : "Nro. Pedido: ";
            lbl6Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Client: " : "Cliente: ";
            lbl5Text = "C.U.I.T. / Documento: ";
            lbl4Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Client Address: " : "Dirección de Cliente: ";
            lbl3Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "If you want to remove a product from the order, select it and then press 'Remove'." : "Si desea elimitar un producto del pedido, seleccionelo y luego presione 'Quitar'.";
            lbl7Text = "Sub-Total: ";
            lbl8Text = "I.V.A. (21%): ";
            lbl9Text = "TOTAL: ";
            colHead1 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Product" : "Producto";
            colHead2 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Price x unit" : "Precio x unidad";
            colHead3 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Quantity" : "Cantidad";
            colHead4 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Amount" : "Importe";
            btn1Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "REMOVE" : "QUITAR";
            btn3Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "ACCEPT" : "ACEPTAR";
            btn4Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "CANCEL" : "CANCELAR";
            facturaSuccessMessage = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Invoice created. Ticket issued." : "Factura creada. Ticket emitido.";
            facturaErrMessage = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Invoice created - ERROR." : "Factura creada - ERROR.";
            this.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Distar - Accept order" : "Distar - Aceptar pedido";
            toolTip1.SetToolTip(button3, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Accept order. Make Invoice and Ticket" : "Aceptar pedido. Realizar Factura y Ticket");
            toolTip1.SetToolTip(button1, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Remove order detail line" : "Quitar detalle del pedido");
        }
    }
}
