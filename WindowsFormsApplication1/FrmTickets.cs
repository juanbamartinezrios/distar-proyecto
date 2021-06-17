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
    public partial class FrmTickets : Form
    {
        Distar_EntidadesNegocio.Usuario userLog;
        Distar_LogicaNegocio.Ticket ticketBL;
        Distar_EntidadesNegocio.Ticket ticketSeleccionado;
        List<Distar_EntidadesNegocio.Ticket> lista_tickets = new List<Distar_EntidadesNegocio.Ticket>();
        private Distar_LogicaNegocio.Bitacora bitacoraBL = new Distar_LogicaNegocio.Bitacora();
        DialogResult res;
        MessageBoxButtons buttons;
        private float totalIva = 0;
        private float totalPedido = 0;
        /** TEXTO **/
        string btn3Message = "";
        string pagoSuccessMessage = "";
        string pagoErrMessage = "";
        string itemSelectedMessage = "";
        string btn1Text = "";
        string btn3Text = "";
        string btn3TTText = "";
        string lbl3Text = "";
        string lbl10Text = "";
        string colHead1 = "";
        string colHead2 = "";
        string colHead3 = "";
        string colHeadSub1 = "";
        string colHeadSub2 = "";
        string colHeadSub3 = "";
        string colHeadSub4 = "";
        string direccion = "";
        string cliente = "";
        string nro_ticket = "";
        string deuda = "";
        string fecha = "";

        public FrmTickets(Distar_EntidadesNegocio.Usuario user): this()
        {
            userLog = user;
        }

        public FrmTickets()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (ticketSeleccionado != null)
            {
                buttons = MessageBoxButtons.YesNo;
                res = Distar.GUIServices.giveMeAlertsWithAction(btn3Message, "Distar", buttons);
                if (res == DialogResult.Yes)
                {
                    ticketBL = new Distar_LogicaNegocio.Ticket();
                    Distar_EntidadesNegocio.Movimiento movimiento = new Distar_EntidadesNegocio.Movimiento();
                    movimiento.fecha_mov = DateTime.Now;
                    movimiento.monto = (float) (ticketSeleccionado.total*1.21);
                    movimiento.nro_cta = userLog.cta_cte.nro_cta;
                    movimiento.tipo_mov = "Pago";
                    if (ticketBL.pagarDeuda(ticketSeleccionado.id_ticket, movimiento))
                    {
                        bitacoraBL.setINFO(DateTime.Now, userLog, "Tickets", "Ticket pagado. Deuda saldada.");
                        buttons = MessageBoxButtons.OK;
                        Distar.GUIServices.giveMeAlerts(pagoSuccessMessage, "Distar", buttons);
                        getListaTickets();
                        refreshDataGridView();
                    }
                    else
                    {
                        buttons = MessageBoxButtons.OK;
                        Distar.GUIServices.giveMeAlerts(pagoErrMessage, "Distar", buttons);
                    }
                }
            }
            else
            {
                buttons = MessageBoxButtons.OK;
                Distar.GUIServices.giveMeAlerts(itemSelectedMessage, "Distar", buttons);
            }
        }

        private void FrmTickets_Load(object sender, EventArgs e)
        {
            setLanguaje();
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
            dataGridView2.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView2.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            button1.Text = btn1Text;
            button3.Text = btn3Text;
            label11.Text = fecha;
            label9.Text = nro_ticket;
            label10.Text = lbl10Text;
            label7.Text = cliente;
            label6.Text = "C.U.I.T. / Documento: ";
            label5.Text = direccion;
            getListaTickets();
            refreshDataGridView();
        }

        private void getListaTickets()
        {
            ticketBL = new Distar_LogicaNegocio.Ticket();
            lista_tickets = ticketBL.getAllTickets(userLog.id_usuario);
        }

        private void refreshDataGridView()
        {
            dataGridView1.DataSource = null;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns[0].HeaderText = colHead1;
            dataGridView1.Columns[1].HeaderText = colHead2;
            dataGridView1.Columns[2].HeaderText = colHead3;
            dataGridView1.Columns[0].DataPropertyName = "fecha_emision";
            dataGridView1.Columns[1].DataPropertyName = "nro_ticket";
            dataGridView1.Columns[2].DataPropertyName = "estado";
            dataGridView1.Columns[2].DefaultCellStyle.Format = "N2";
            dataGridView1.DataSource = lista_tickets;
            dataGridView2.Columns[1].HeaderText = colHeadSub1;
            dataGridView2.Columns[2].HeaderText = colHeadSub2;
            dataGridView2.Columns[3].HeaderText = colHeadSub3;
            dataGridView2.Columns[4].HeaderText = colHeadSub4;
        }

        private void refreshDataGridViewListaDetalle()
        {
            dataGridView2.DataSource = null;
            dataGridView2.AutoGenerateColumns = false;
            dataGridView2.Columns[0].HeaderText = "#";
            dataGridView2.Columns[1].HeaderText = colHeadSub1;
            dataGridView2.Columns[2].HeaderText = colHeadSub2;
            dataGridView2.Columns[3].HeaderText = colHeadSub3;
            dataGridView2.Columns[4].HeaderText = colHeadSub4;
            dataGridView2.Columns[0].DataPropertyName = "nro_detalle";
            dataGridView2.Columns[1].DataPropertyName = "descripcion_prod";
            dataGridView2.Columns[2].DataPropertyName = "p_unitario_prod";
            dataGridView2.Columns[3].DataPropertyName = "cantidad";
            dataGridView2.Columns[4].DataPropertyName = "importe";
            dataGridView2.Columns[2].DefaultCellStyle.Format = "N2";
            dataGridView2.Columns[4].DefaultCellStyle.Format = "N2";
            dataGridView2.DataSource = ticketSeleccionado.detalle_items;
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
            ticketSeleccionado = (Distar_EntidadesNegocio.Ticket)selectedRow.DataBoundItem;
            button3.Enabled = ((ticketSeleccionado.estado != "Pago") && ticketSeleccionado != null) ? true : false;
            refreshDetalleTicket(ticketSeleccionado);
        }

        private void refreshDetalleTicket(Distar_EntidadesNegocio.Ticket ticket)
        {
            label5.Text = direccion + ticket.cliente.domicilio.direccion + " " + ticket.cliente.domicilio.numero_dom.Trim() + " - CP: " + ticket.cliente.domicilio.cp;
            label6.Text = "C.U.I.T. / Documento: " + ticket.cliente.documento;
            label7.Text = cliente + ticket.cliente.apellido + ", " + ticket.cliente.nombre;
            label9.Text = nro_ticket + ticket.nro_ticket.ToString();
            label10.Text = deuda + String.Format("{0:C}", ticket.total * 1.21);
            label11.Text = fecha + ticket.fecha_emision.ToShortDateString();
            refreshDataGridViewListaDetalle();
        }

        private void setLanguaje()
        {
            if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
            {
                btn3Message = "Do you want to pay the debt of the selected Ticket?";
                pagoSuccessMessage = "Debt paid, check your movements in the section 'Current Account'.";
                pagoErrMessage = "Could not pay what was owed.";
                itemSelectedMessage = "You must select a Ticket from the list.";
                btn3Text = "DO PAYMENT";
                btn1Text = "CLOSE";
                toolTip1.SetToolTip(button3, "Make the payment of the debt corresponding to the selected Ticket.");
                lbl3Text = "Debt (w / IVA): -";
                lbl10Text = "Debt:";
                colHead1 = "Issue Date";
                colHead2 = "Ticket Number";
                colHead3 = "State";
                colHeadSub1 = "Product";
                colHeadSub2 = "Price x unit";
                colHeadSub3 = "Quantity";
                colHeadSub4 = "Amount";
            }
            else
            {
                btn3Message = "Desea pagar la deuda del Ticket seleccionado?";
                pagoSuccessMessage = "Deuda pagada. Consulte sus movimientos en el apartado 'Cuenta Corriente'.";
                pagoErrMessage = "No se pudo pagar lo adeudado.";
                itemSelectedMessage = "Debe seleccionar un Ticket del listado.";
                btn3Text = "PAGAR";
                btn1Text = "SALIR";
                lbl3Text = "Deuda (c / IVA): -";
                toolTip1.SetToolTip(button3, "Realizar el pago de la deuda correspondiente al Ticket seleccionado.");
                lbl10Text = "Deuda";
                colHead1 = "Fecha Emisión";
                colHead2 = "Nro. Ticket";
                colHead3 = "Estado";
                colHeadSub1 = "Producto";
                colHeadSub2 = "Precio x unidad";
                colHeadSub3 = "Cantidad";
                colHeadSub4 = "Importe";
            }
            direccion = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Client Address: " : "Dirección de Cliente";
            cliente = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Client: " : "Cliente";
            nro_ticket = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Ticket Number: " : "Nro. Ticket";
            deuda = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Debt (w / IVA): " : "Deuda (c / IVA): ";
            fecha = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Issue Date: " : "Fecha de emisión: ";
        }
    }
}
