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

namespace Distar.SEGURIDAD
{
    public partial class FrmBitacora : Form
    {
        Distar.GUIServices servicesGUI;
        Distar_EntidadesNegocio.Usuario userLog;
        Distar_LogicaNegocio.Usuario usuarioBL;
        Distar_LogicaNegocio.Bitacora bitacoraBL;
        List<Distar_EntidadesNegocio.Usuario> lista_usuarios;
        List<Distar_EntidadesNegocio.Bitacora> lista_log;
        DateTime fechaDesde;
        DateTime fechaHasta;
        DialogResult res;
        MessageBoxButtons buttons;
        private int acum = 0;
        /** TEXTO **/
        string gb1Text = "";
        string gb2Text = "";
        string lbl1Text = "";
        string lbl2Text = "";
        string btn2Text = "";
        string btn3Text = "";
        string btn1Text = "";
        string colHead1 = "";
        string colHead2 = "";
        string colHead3 = "";
        string colHead4 = "";
        string colHead5 = "";
        string printAlert = "";
        string printAlert2 = "";
        string fechaAlert = "";

        public FrmBitacora()
        {
            InitializeComponent();
        }

        public FrmBitacora(Distar_EntidadesNegocio.Usuario user): this()
        {
            userLog = user;
        }

        private void FrmBitacora_Load(object sender, EventArgs e)
        {
            setLanguaje();
            servicesGUI = new Distar.GUIServices();
            setLogForm();
            getLogList(dateTimePicker1.Value, dateTimePicker2.Value);
            refreshDataGridView();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value > dateTimePicker2.Value)
            {
                buttons = MessageBoxButtons.OK;
                res = Distar.GUIServices.giveMeAlertsWithAction(fechaAlert, "Distar", buttons);
            }
            else
            {
                getLogList(dateTimePicker1.Value, dateTimePicker2.Value);
                refreshDataGridView();
            }
        }

        private void setLogForm()
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.MultiSelect = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            checkedListBox1.CheckOnClick = true;
            checkedListBox1.SelectionMode = SelectionMode.One;
            checkedListBox1.DisplayMember = "nombre";
            checkedListBox2.CheckOnClick = true;
            checkedListBox2.SelectionMode = SelectionMode.One;
            checkedListBox1.Items.Clear();
            checkedListBox2.Items.Clear();
            dateTimePicker1.Value = DateTime.Now.AddDays(-7);
            dateTimePicker2.Value = DateTime.Now;
            usuarioBL = new Distar_LogicaNegocio.Usuario();
            lista_usuarios = usuarioBL.getAllUsers();
            foreach (Distar_EntidadesNegocio.Usuario user in lista_usuarios)
            {
                checkedListBox1.Items.Add(user);
            }
            bitacoraBL = new Distar_LogicaNegocio.Bitacora();
            foreach (string criticidad in bitacoraBL.getCriticidad())
            {
                checkedListBox2.Items.Add(criticidad);
            }
            groupBox1.Text = gb1Text;
            groupBox2.Text = gb2Text;
            label1.Text = lbl1Text;
            label2.Text = lbl2Text;
            button1.Text = btn1Text;
            button2.Text = btn2Text;
            button3.Text = btn3Text;
        }

        private void getLogList(DateTime fd, DateTime fh){
            bitacoraBL = new Distar_LogicaNegocio.Bitacora();
            Distar_EntidadesNegocio.DTO.BitacoraDTO DTO = new Distar_EntidadesNegocio.DTO.BitacoraDTO();
            usuarioBL = new Distar_LogicaNegocio.Usuario();
            lista_log = new List<Distar_EntidadesNegocio.Bitacora>();

            string lista_id_usuarios = "";
            string lista_criticidad = "";
            fechaDesde = fd;
            fechaHasta = fh;

            DTO.fechaDesde = fechaDesde.ToString("yyyy-MM-dd") + " 00:00";
            DTO.fechaHasta = fechaHasta.ToString("yyyy-MM-dd") + " 23:59";
            if (checkedListBox1.CheckedItems.Count > 0)
            {
                foreach (Distar_EntidadesNegocio.Usuario user in checkedListBox1.CheckedItems)
                {
                    lista_id_usuarios += "'" + user.id_usuario + "',";
                }
                DTO.lista_usuarios = lista_id_usuarios.Substring(0, lista_id_usuarios.Length - 1);
            }

            if (checkedListBox2.CheckedItems.Count > 0)
            {
                foreach (string item in checkedListBox2.CheckedItems)
                {
                    lista_criticidad += "'" + item + "',";
                }
                DTO.lista_criticidad = lista_criticidad.Substring(0, lista_criticidad.Length - 1);
            }

            lista_log = bitacoraBL.filtrarBitacora(DTO);
        }

        private void refreshDataGridView()
        {
            dataGridView1.DataSource = null;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns[0].HeaderText = colHead1;
            dataGridView1.Columns[1].HeaderText = colHead2;
            dataGridView1.Columns[2].HeaderText = colHead3;
            dataGridView1.Columns[4].HeaderText = colHead4;
            dataGridView1.Columns[3].HeaderText = colHead5;
            dataGridView1.Columns[0].DataPropertyName = "fecha";
            dataGridView1.Columns[1].DataPropertyName = "usuario_email";
            dataGridView1.Columns[2].DataPropertyName = "funcionalidad";
            dataGridView1.Columns[4].DataPropertyName = "descripcion";
            dataGridView1.Columns[3].DataPropertyName = "criticidad";
            dataGridView1.DataSource = lista_log;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.CheckedItems.Count > 0)
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i, false);
                }
            }

            if (checkedListBox2.CheckedItems.Count > 0)
            {
                for (int i = 0; i < checkedListBox2.Items.Count; i++)
                {
                    checkedListBox2.SetItemChecked(i, false);
                }
            }
            setLogForm();
            getLogList(dateTimePicker1.Value, dateTimePicker2.Value);
            refreshDataGridView();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (lista_log.Count > 0)
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

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            {
                servicesGUI.imprimirReporte(dataGridView1, "Bitácora", sender, e, userLog);
            }
            catch (Exception ex)
            {
                buttons = MessageBoxButtons.OK;
                res = Distar.GUIServices.giveMeAlertsWithAction(printAlert2, "Distar", buttons);
            }
        }

        private void printDocument1_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            servicesGUI.iniciarImpresion(dataGridView1, sender, e);
        }

        private void printDocument1_EndPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (acum == 0)
            {
                bitacoraBL.setINFO(DateTime.Now, userLog, "Bitácora", "Impresión de reporte.");
                Distar_LogicaNegocio.Impresion impresionBL = new Distar_LogicaNegocio.Impresion();
                Distar_EntidadesNegocio.Impresion impresion = new Distar_EntidadesNegocio.Impresion();
                impresion.fecha = DateTime.Now;
                impresion.id_usuario = userLog.id_usuario;
                impresion.reporte = "Bitácora";
                impresionBL.registrarImpresion(impresion);
                acum += 1;
            }
        }

        private void setLanguaje()
        {
            gb1Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "USERS" : "USUARIOS";
            gb2Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "CRITICALITY" : "CRITICIDAD";
            lbl1Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "FROM:" : "DESDE: ";
            lbl2Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "TO: " : "HASTA: ";
            btn2Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "FILTER" : "FILTRAR";
            btn3Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "CLEAN" : "LIMPIAR";
            btn1Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "PRINT" : "IMPRIMIR";
            colHead1 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Issue Date" : "Fecha";
            colHead2 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "User" : "Usuario";
            colHead3 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Functionality" : "Funcionalidad";
            colHead4 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Description" : "Descripción";
            colHead5 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Criticality" : "Criticidad";
            fechaAlert = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Date 'FROM' can not be greater than Date 'TO' Please re-enter the filters by date." : "Fecha 'DESDE' no puede ser mayor a Fecha 'HASTA'. Por favor, reingrese los filtros por fecha.";
            printAlert = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "You can not print a Report without records." : "No se puede realizar la impresión de un Reporte sin registros.";
            printAlert2 = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "The requested Report could not be printed because an error occurred." : "No se pudo realizar la impresión del Reporte solicitado ya que ocurrió un error.";
            toolTip1.SetToolTip(label1, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Set 'from' date" : "Coloque la fecha 'desde'");
            toolTip1.SetToolTip(label2, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Set 'to' date" : "Coloque la fecha 'hasta'");
            toolTip1.SetToolTip(button1, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Print the log lists" : "Imprimir la lista de registros de bitácora");
            toolTip1.SetToolTip(button3, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Clean filters" : "Limpiar filtros");
            toolTip1.SetToolTip(button2, Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Filter log list by Date-User-Criticality" : "Filtrar la bitácora por Fecha-Usuario-Criticidad");
        }
    }
}
