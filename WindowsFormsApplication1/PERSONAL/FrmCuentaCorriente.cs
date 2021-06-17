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

namespace Distar.PERSONAL
{
    public partial class FrmCuentaCorriente : Form
    {
        Distar.GUIServices servicesGUI;
        Distar_EntidadesNegocio.Usuario userLog;
        Distar_LogicaNegocio.Movimiento movimientoBL;
        Distar_LogicaNegocio.CuentaCorriente cuentaCorrienteBL = new Distar_LogicaNegocio.CuentaCorriente();
        List<Distar_EntidadesNegocio.Movimiento> lista_movimientos = new List<Distar_EntidadesNegocio.Movimiento>();
        private Distar_LogicaNegocio.Bitacora bitacoraBL = new Distar_LogicaNegocio.Bitacora();
        DialogResult res;
        MessageBoxButtons buttons;
        private int acum = 0;
        /** TEXTO **/
        string gb1Text = "";
        string gb2Text = "";
        string lbl1Text = "";
        string lbl2Text = "";
        string lbl6Text = "";
        string lbl5Text = "";
        string colHead1 = "";
        string colHead2 = "";
        string colHead3 = "";
        string btn1Text = "";
        string printAlert = "";
        string printAlert2 = "";

        public FrmCuentaCorriente(Distar_EntidadesNegocio.Usuario user): this()
        {
            userLog = user;
        }

        public FrmCuentaCorriente()
        {
            InitializeComponent();
        }

        private void FrmCuentaCorriente_Load(object sender, EventArgs e)
        {
            setLanguaje();
            groupBox1.Text = gb1Text;
            groupBox2.Text = gb2Text;
            button1.Text = btn1Text;
            userLog.cta_cte.saldo_deudor = cuentaCorrienteBL.getSaldoDeudor(userLog.id_usuario);
            label1.Text = lbl1Text + userLog.cta_cte.nro_cta;
            label2.Text = lbl2Text + String.Format("{0:C}", userLog.cta_cte.saldo_deudor);
            label5.Text = lbl5Text + userLog.documento;
            label6.Text = lbl6Text + userLog.apellido + ", " + userLog.nombre;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridView2.AllowUserToResizeColumns = false;
            dataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView2.AllowUserToResizeRows = false;
            dataGridView2.MultiSelect = false;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            getListaMovimientos();
            refreshDataGridView();
            servicesGUI = new Distar.GUIServices();
            servicesGUI.setNuevaImpresion();
        }

        private void getListaMovimientos()
        {
            movimientoBL = new Distar_LogicaNegocio.Movimiento();
            lista_movimientos = movimientoBL.obtenerMovimientosCta(userLog.cta_cte.nro_cta);
        }

        private void refreshDataGridView()
        {
            dataGridView2.DataSource = null;
            dataGridView2.AutoGenerateColumns = false;
            dataGridView2.Columns[0].HeaderText = colHead1;
            dataGridView2.Columns[1].HeaderText = colHead2;
            dataGridView2.Columns[2].HeaderText = colHead3;
            dataGridView2.Columns[0].DataPropertyName = "fecha_mov";
            dataGridView2.Columns[1].DataPropertyName = "tipo_mov";
            dataGridView2.Columns[2].DataPropertyName = "monto";
            dataGridView2.Columns[2].DefaultCellStyle.Format = "N2";
            dataGridView2.DataSource = lista_movimientos;
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            {
                servicesGUI.imprimirReporte(dataGridView2, "Cuenta Corriente - Movimientos", sender, e, userLog);
            }
            catch (Exception ex)
            {
                buttons = MessageBoxButtons.OK;
                res = Distar.GUIServices.giveMeAlertsWithAction(printAlert2, "Distar", buttons);
            }
        }

        private void printDocument1_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            servicesGUI.iniciarImpresion(dataGridView2, sender, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (lista_movimientos.Count > 0)
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

        private void printDocument1_EndPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (acum == 0)
            {
                bitacoraBL.setINFO(DateTime.Now, userLog, "Cuenta Corriente - Movimientos", "Impresión de reporte.");
                Distar_LogicaNegocio.Impresion impresionBL = new Distar_LogicaNegocio.Impresion();
                Distar_EntidadesNegocio.Impresion impresion = new Distar_EntidadesNegocio.Impresion();
                impresion.fecha = DateTime.Now;
                impresion.id_usuario = userLog.id_usuario;
                impresion.reporte = "Cta. Cte. - Movimientos";
                impresionBL.registrarImpresion(impresion);
                acum += 1;
            }
        }

        private void usuarioPoseeCta()
        {
            if (userLog.cta_cte.nro_cta == "")
            {
                buttons = MessageBoxButtons.OK;
                res = Distar.GUIServices.giveMeAlertsWithAction(Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "The User does not have a Account." : "El Usuario no posee Cuenta Corriente.", "Distar", buttons);
                if (res == DialogResult.OK)
                {
                    Close();
                }
            }
        }

        private void setLanguaje()
        {
            gb1Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "ACCOUNT DETAIL" : "DETALLE DE CUENTA";
            gb2Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "LAST MOVEMENTS" : "ÚLT. MOVIMIENTOS";
            lbl1Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Account Number: " : "Nro. de Cuenta: ";
            lbl2Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Debit balance: " : "Saldo deudor: ";
            lbl6Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Client: " : "Cliente: ";
            lbl5Text = "C.U.I.T. / Documento: ";
            colHead1 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Issue Date" : "Fecha";
            colHead2 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Movement detail" : "Detalle de movimiento";
            colHead3 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Amount" : "Importe";
            btn1Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "PRINT" : "IMPRIMIR";
            printAlert = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "You can not print a Report without records." : "No se puede realizar la impresión de un Reporte sin registros.";
            printAlert2 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "The requested Report could not be printed because an error occurred." : "No se pudo realizar la impresión del Reporte solicitado ya que ocurrió un error.";
            this.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Distar - Accept order" : "Distar - Aceptar pedido";
            toolTip1.SetToolTip(button1, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Print the account movement lists" : "Imprimir la lista de movimientos de la Cuenta Corriente");
        }
    }
}
