using System;
using System.Collections.Generic;
using System.Collections;
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
    public partial class FrmPedidos : Form
    {
        Distar.GUIServices servicesGUI;
        Distar_EntidadesNegocio.Usuario userLog;
        Distar_LogicaNegocio.Pedido pedidoBL;
        Distar_EntidadesNegocio.Pedido pedidoSeleccionado;
        List<Distar_EntidadesNegocio.Pedido> lista_pedidos = new List<Distar_EntidadesNegocio.Pedido>();
        private Distar_LogicaNegocio.Bitacora bitacoraBL = new Distar_LogicaNegocio.Bitacora();
        private Distar_LogicaNegocio.Services servicesBL = new Distar_LogicaNegocio.Services();
        DialogResult res;
        MessageBoxButtons buttons;
        private int acum = 0;
        /** TEXTO **/
        string gb2Text = "";
        string lbl1Text = "";
        string lbl2Text = "";
        string lbl4Text = "";
        string lbl5Text = "";
        string lbl6Text = "";
        string lbl10Text = "";
        string colHead1 = "";
        string colHead2 = "";
        string colHead3 = "";
        string colHead4 = "";
        string colHeadSub1 = "";
        string colHeadSub2 = "";
        string colHeadSub3 = "";
        string colHeadSub4 = "";
        string btn1Text = "";
        string btn2Text = "";
        string btn3Text = "";
        string btn4Text = "";
        string btn5Text = "";
        string itemSelectedMessage = "";
        string accepPedidoMessage = "";
        string refusePedidoMessage = "";
        string inputText = "";
        string inputTextTitle = "";
        string pedidoSuccessMessage = "";
        string pedidoErrMessage = "";
        string printAlert = "";
        string printAlert2 = "";

        public FrmPedidos()
        {
            InitializeComponent();
        }

        public FrmPedidos(Distar_EntidadesNegocio.Usuario user): this()
        {
            userLog = user;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (pedidoSeleccionado != null)
            {
                buttons = MessageBoxButtons.YesNo;
                res = Distar.GUIServices.giveMeAlertsWithAction(accepPedidoMessage, "Distar", buttons);
                if (res == DialogResult.Yes)
                {
                    Distar.FrmConfirmar_factura _FrmConfirmar_factura = new FrmConfirmar_factura(userLog, pedidoSeleccionado);
                    _FrmConfirmar_factura.Show();
                    _FrmConfirmar_factura.FrmConfirmar_factura_ok += this.OnFrmConfirmar_factura;
                }
            }
            else
            {
                buttons = MessageBoxButtons.OK;
                Distar.GUIServices.giveMeAlerts(itemSelectedMessage, "Distar", buttons);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            buttons = MessageBoxButtons.YesNo;
            res = Distar.GUIServices.giveMeAlertsWithAction(refusePedidoMessage, "Distar", buttons);
            if (res == DialogResult.Yes)
            {
                Object myval = new Object();
                myval = Microsoft.VisualBasic.Interaction.InputBox(inputText, inputTextTitle, null);
                if (myval != null && myval != "")
                {
                    pedidoBL = new Distar_LogicaNegocio.Pedido();
                    if (pedidoBL.rechazarPedido(myval.ToString(), pedidoSeleccionado.id_pedido))
                    {
                        bitacoraBL.setINFO(DateTime.Now, userLog, "Pedidos", "Pedido rechazado.");
                        buttons = MessageBoxButtons.OK;
                        Distar.GUIServices.giveMeAlerts(pedidoSuccessMessage, "Distar", buttons);
                        getListaPedidos();
                        refreshDataGridView();
                    }
                    else
                    {
                        buttons = MessageBoxButtons.OK;
                        Distar.GUIServices.giveMeAlerts(pedidoErrMessage, "Distar", buttons);
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Distar.FrmPedidos_nuevo _FrmPedidos_nuevo = new Distar.FrmPedidos_nuevo(userLog);
            _FrmPedidos_nuevo.Show();
            _FrmPedidos_nuevo.FrmPedidos_nuevo_ok += this.OnFrmPedidos_nuevo;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmPedidos_Load(object sender, EventArgs e)
        {
            setLanguaje();
            button1.Text = btn1Text;
            button2.Text = btn2Text;
            button3.Text = btn3Text;
            button4.Text = btn4Text;
            button5.Text = btn5Text;
            groupBox2.Text = gb2Text;
            label1.Text = lbl1Text;
            label2.Text = lbl2Text;
            label4.Text = lbl4Text;
            label5.Text = lbl5Text;
            label6.Text = lbl6Text;
            label10.Text = lbl10Text;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.MultiSelect = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; 
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridView2.AllowUserToResizeColumns = false;
            dataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView2.AllowUserToResizeRows = false;
            dataGridView2.MultiSelect = false;
            dataGridView2.ReadOnly = true;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.DefaultCellStyle.SelectionBackColor = dataGridView2.DefaultCellStyle.BackColor;
            dataGridView2.DefaultCellStyle.SelectionForeColor = dataGridView2.DefaultCellStyle.ForeColor;
            dataGridView2.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView2.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; 
            servicesGUI = new Distar.GUIServices();
            servicesBL.setPatentesUsuarioLog(userLog);
            setPermisosNavegavilidad();
            getListaPedidos();
            refreshDataGridView();
        }

        private void getListaPedidos()
        {
            pedidoSeleccionado = null;
            pedidoBL = new Distar_LogicaNegocio.Pedido();
            lista_pedidos = pedidoBL.getAllPedidos();
        }

        private void refreshDataGridView()
        {
            dataGridView1.DataSource = null;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns[0].HeaderText = colHead1;
            dataGridView1.Columns[1].HeaderText = colHead2;
            dataGridView1.Columns[2].HeaderText = colHead3;
            dataGridView1.Columns[3].HeaderText = colHead4;
            dataGridView1.Columns[0].DataPropertyName = "fecha_creacion";
            dataGridView1.Columns[1].DataPropertyName = "descripcion";
            dataGridView1.Columns[2].DataPropertyName = "estado";
            dataGridView1.Columns[3].DataPropertyName = "total";
            dataGridView1.Columns[3].DefaultCellStyle.Format = "N2";
            dataGridView1.DataSource = lista_pedidos;
        }

        private void refreshDataGridViewListaDetalle()
        {
            dataGridView2.DataSource = null;
            dataGridView2.AutoGenerateColumns = false;
            dataGridView2.Columns[0].HeaderText = colHeadSub1;
            dataGridView2.Columns[1].HeaderText = colHeadSub2;
            dataGridView2.Columns[2].HeaderText = colHeadSub3;
            dataGridView2.Columns[3].HeaderText = colHeadSub4;
            dataGridView2.Columns[0].DataPropertyName = "descripcion_prod";
            dataGridView2.Columns[1].DataPropertyName = "p_unitario_prod";
            dataGridView2.Columns[2].DataPropertyName = "cantidad";
            dataGridView2.Columns[3].DataPropertyName = "importe";
            dataGridView2.Columns[1].DefaultCellStyle.Format = "N2";
            dataGridView2.Columns[3].DefaultCellStyle.Format = "N2";
            dataGridView2.DataSource = pedidoSeleccionado != null ? pedidoSeleccionado.detalle_items : null;
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
            pedidoSeleccionado = (Distar_EntidadesNegocio.Pedido)selectedRow.DataBoundItem;
            button1.Enabled = ((pedidoSeleccionado.estado != "Rechazado") && servicesBL.validarPatente(19) && pedidoSeleccionado != null) ? true : false;
            button3.Enabled = ((pedidoSeleccionado.estado != "Rechazado") && servicesBL.validarPatente(19) && pedidoSeleccionado != null) ? true : false;
            refreshDetallePedido(pedidoSeleccionado);
        }

        private void refreshDetallePedido(Distar_EntidadesNegocio.Pedido pedido)
        {
            label1.Text = lbl1Text + pedido.fecha_creacion.ToShortDateString();
            label2.Text = lbl2Text + pedido.nro_pedido.ToString();
            label4.Text = lbl4Text + pedido.cliente.domicilio.direccion + " " + pedido.cliente.domicilio.numero_dom.Trim() + " - CP: " + pedido.cliente.domicilio.cp;
            label5.Text = lbl5Text + pedido.cliente.documento;
            label6.Text = lbl6Text + pedido.cliente.apellido + ", " + pedido.cliente.nombre;
            label10.Text = lbl10Text + String.Format("{0:C}", pedido.total);
            refreshDataGridViewListaDetalle();
        }

        private void OnFrmPedidos_nuevo(object sender, Boolean flag)
        {
            getListaPedidos();
            refreshDataGridView();
            refreshDataGridViewListaDetalle();
        }

        private void OnFrmConfirmar_factura(object sender, Boolean flag)
        {
            getListaPedidos();
            refreshDataGridView();
            refreshDataGridViewListaDetalle();
        }

        private void setPermisosNavegavilidad()
        {
            // AdministrarPedido y GenerarPedido
            button1.Enabled = servicesBL.validarPatente(19);
            button3.Enabled = servicesBL.validarPatente(19);
            button4.Enabled = servicesBL.validarPatente(20);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (lista_pedidos.Count > 0)
            {
                servicesGUI.setNuevaImpresion();
                if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
                {
                    printDocument1.Print();
                }
            }
            else
            {
                buttons = MessageBoxButtons.OK;
                res = Distar.GUIServices.giveMeAlertsWithAction(printAlert, "Distar", buttons);
            }
        }

        private void printDocument1_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            servicesGUI.iniciarImpresion(dataGridView1, sender, e);
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            {
                servicesGUI.imprimirReporte(dataGridView1, "Pedidos", sender, e, userLog);
            }
            catch (Exception ex)
            {
                buttons = MessageBoxButtons.OK;
                res = Distar.GUIServices.giveMeAlertsWithAction(printAlert2, "Distar", buttons);
            }
        }

        private void printDocument1_EndPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (acum == 0)
            {
                bitacoraBL.setINFO(DateTime.Now, userLog, "Pedidos", "Impresión de reporte.");
                Distar_LogicaNegocio.Impresion impresionBL = new Distar_LogicaNegocio.Impresion();
                Distar_EntidadesNegocio.Impresion impresion = new Distar_EntidadesNegocio.Impresion();
                impresion.fecha = DateTime.Now;
                impresion.id_usuario = userLog.id_usuario;
                impresion.reporte = "Pedidos";
                impresionBL.registrarImpresion(impresion);
                acum += 1;
            }
        }

        private void setLanguaje()
        {
            gb2Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "ORDER DETAIL" : "DETALLE DE PEDIDO";
            lbl1Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Issue Date:" : "Fecha de creación: ";
            lbl2Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Order Number: " : "Nro. Pedido: ";
            lbl4Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Client Address: " : "Dirección del Cliente: ";
            lbl5Text = "C.U.I.T. / Documento: ";
            lbl6Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Client: " : "Cliente: ";
            lbl10Text = "Total: ";
            colHead1 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Issue Date" : "Fecha de creación";
            colHead2 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Description" : "Descripción";
            colHead3 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Status" : "Estado";
            colHead4 = "Total";
            colHeadSub1 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Product" : "Producto";
            colHeadSub2 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Price x unit" : "Precio x unidad";
            colHeadSub3 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Quantity" : "Cantidad";
            colHeadSub4 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Amount" : "Importe";
            btn1Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "REFUSE" : "RECHAZAR";
            btn2Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "CLOSE" : "SALIR";
            btn3Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "ACCEPT" : "ACEPTAR";
            btn4Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "NEW ORDER" : "NUEVO PEDIDO";
            btn5Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "PRINT" : "IMPRIMIR";
            itemSelectedMessage = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "You must select an Order from the list." : "Debe seleccionar un Pedido del listado.";
            accepPedidoMessage = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Do you want to accept the selected Order?" : "Desea aceptar el Pedido seleccionado?";
            refusePedidoMessage = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Do you want to reject the selected Order?" : "Desea rechazar el Pedido seleccionado?";
            inputText = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Reason of rejection:" : "Motivo del rechazo:";
            inputTextTitle = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Distar - Order refuse" : "Distar - Rechazo de pedido";
            pedidoSuccessMessage = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Order refused." : "Pedido rechazado.";
            pedidoErrMessage = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "The selected Order could not be rejected." : "El Pedido seleccionado no se pudo rechazar.";
            printAlert = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "You can not print a Report without records." : "No se puede realizar la impresión de un Reporte sin registros.";
            printAlert2 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "The requested Report could not be printed because an error occurred." : "No se pudo realizar la impresión del Reporte solicitado ya que ocurrió un error.";
            this.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Distar - Orders" : "Distar - Pedidos";
            toolTip1.SetToolTip(button1, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Refuse selected order" : "Rechazar pedido seleccionado");
            toolTip1.SetToolTip(button3, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Accept selected order" : "Aceptar pedido seleccionado");
            toolTip1.SetToolTip(button4, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Create new order" : "Crear nuevo pedido");
            toolTip1.SetToolTip(button5, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Print the order lists" : "Imprimir la lista de pedidos");
        }
    }
}
