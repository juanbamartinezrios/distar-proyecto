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
    public partial class FrmPatentes : Form
    {
        Distar_LogicaNegocio.Patente patenteBL;
        Distar_EntidadesNegocio.Usuario userLog;
        List<Distar_EntidadesNegocio.Patente> lista_patentes;

        public FrmPatentes()
        {
            InitializeComponent();
        }

        public FrmPatentes(Distar_EntidadesNegocio.Usuario user): this()
        {
            userLog = user;
        }

        private void FrmPatentes_Load(object sender, EventArgs e)
        {
            this.Text = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? "Distar - Patents" : "Distar - Patentes";
            listBox1.SelectionMode = SelectionMode.MultiExtended;
            getListaPatentes();
        }

        private void getListaPatentes()
        {
            listBox1.Items.Clear();
            listBox1.DisplayMember = "descripcion";
            patenteBL = new Distar_LogicaNegocio.Patente();
            lista_patentes = patenteBL.getAllPatentes();
            foreach (Distar_EntidadesNegocio.Patente patente in lista_patentes)
            {
                listBox1.Items.Add(patente);
            }
        }
    }
}
