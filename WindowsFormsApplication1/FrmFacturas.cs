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
    public partial class FrmFacturas : Form
    {
        Distar_EntidadesNegocio.Usuario userLog;
        Distar_LogicaNegocio.Factura facturaBL;
        Distar_EntidadesNegocio.Factura facturaSeleccionada;
        List<Distar_EntidadesNegocio.Factura> lista_facturas = new List<Distar_EntidadesNegocio.Factura>();
        private float totalIva = 0;
        private float totalPedido = 0;
        /** TEXTO **/
        string gb2Text = "";
        string lbl11Text = "";
        string lbl9Text = "";
        string lbl4Text = "";
        string lbl12Text = "";
        string lbl7Text = "";
        string lbl6Text = "";
        string lbl5Text = "";
        string lbl3Text = "";
        string lbl2Text = "";
        string lbl1Text = "";
        string colHead1 = "";
        string colHead2 = "";
        string colHead3 = "";
        string colHeadSub1 = "";
        string colHeadSub2 = "";
        string colHeadSub3 = "";
        string colHeadSub4 = "";
        string btn2Text = "";

        public FrmFacturas(Distar_EntidadesNegocio.Usuario user): this()
        {
            userLog = user;
        }

        public FrmFacturas()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmFacturas_Load(object sender, EventArgs e)
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
            dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; 
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
            label3.Text = lbl3Text;
            label2.Text = lbl2Text;
            label1.Text = lbl1Text;
            label4.Text = lbl4Text;
            label5.Text = lbl5Text;
            label6.Text = lbl6Text;
            label7.Text = lbl7Text;
            label9.Text = lbl9Text;
            label11.Text = lbl11Text;
            label12.Text = lbl12Text;
            button2.Text = btn2Text;
            groupBox2.Text = gb2Text;
            getListaFacturas();
            refreshDataGridView();
        }

        private void getListaFacturas()
        {
            facturaBL = new Distar_LogicaNegocio.Factura();
            lista_facturas = facturaBL.getAllFacturas();
        }

        private void refreshDataGridView()
        {
            dataGridView1.DataSource = null;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns[0].HeaderText = colHead1;
            dataGridView1.Columns[1].HeaderText = colHead2;
            dataGridView1.Columns[2].HeaderText = colHead3;
            dataGridView1.Columns[0].DataPropertyName = "fecha_emision";
            dataGridView1.Columns[1].DataPropertyName = "nro_factura";
            dataGridView1.Columns[2].DataPropertyName = "total";
            dataGridView1.Columns[2].DefaultCellStyle.Format = "N2";
            dataGridView1.DataSource = lista_facturas;
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
            dataGridView2.DataSource = facturaSeleccionada.detalle_items;
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
            facturaSeleccionada = (Distar_EntidadesNegocio.Factura)selectedRow.DataBoundItem;
            refreshDetalleFactura(facturaSeleccionada);
        }

        private void refreshDetalleFactura(Distar_EntidadesNegocio.Factura factura)
        {
            label4.Text = lbl4Text + factura.tipo_factura;
            label5.Text = lbl5Text + factura.cliente.domicilio.direccion + " " + factura.cliente.domicilio.numero_dom.Trim() + " - CP: " + factura.cliente.domicilio.cp;
            label6.Text = lbl6Text + factura.cliente.documento;
            label7.Text = lbl7Text + factura.cliente.apellido + ", " + factura.cliente.nombre;
            label9.Text = lbl9Text + factura.nro_factura.ToString();
            label11.Text = lbl11Text + factura.fecha_emision.ToShortDateString();
            label12.Text = lbl12Text + factura.iva.ToString();
            refreshTotales();
            refreshDataGridViewListaDetalle();
        }

        private void refreshTotales()
        {
            label3.Text = lbl3Text + String.Format("{0:C}", facturaSeleccionada.total);
            totalIva = facturaSeleccionada.total * 21 / 100;
            label2.Text = lbl2Text + String.Format("{0:C}", totalIva);
            totalPedido = facturaSeleccionada.total + totalIva;
            label1.Text = lbl1Text + String.Format("{0:C}", totalPedido);
        }

        private void setLanguaje()
        {   
            gb2Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "INVOICE" : "FACTURA";
            lbl11Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Issue Date:" : "Fecha de emisión: ";
            lbl9Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Invoice Number: " : "Nro. Factura: ";
            lbl4Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Invoice Type: " : "Tipo de Factura: ";
            lbl12Text = "I.V.A.: ";
            lbl7Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Client: " : "Cliente: ";
            lbl6Text = "C.U.I.T. / Documento: ";
            lbl5Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Client Address: " : "Dirección de Cliente: ";
            lbl3Text = "Sub-Total: ";
            lbl2Text = "I.V.A. (21%): ";
            lbl1Text = "TOTAL: ";
            colHead1 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Issue Date" : "Fecha de emisión";
            colHead2 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Invoice Number" : "Nro. Factura";
            colHead3 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Sub-Total" : "Sub-Total";
            colHeadSub1 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Product" : "Producto";
            colHeadSub2 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Price x unit" : "Precio x unidad";
            colHeadSub3 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Quantity" : "Cantidad";
            colHeadSub4 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Amount" : "Importe";
            btn2Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "CLOSE" : "SALIR";
            this.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Distar - Invoices" : "Distar - Facturas";
        }
    }
}
