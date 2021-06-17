using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Distar.SEGURIDAD
{
    public partial class FrmIntegridad : Form
    {
        private Distar_LogicaNegocio.Services servicesBL = new Distar_LogicaNegocio.Services();
        Distar_EntidadesNegocio.Usuario userLog;
        List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO> lista_logs;

        public FrmIntegridad(Distar_EntidadesNegocio.Usuario user,List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO> list): this()
        {
            userLog = user;
            lista_logs = list;
        }

        public FrmIntegridad()
        {
            InitializeComponent();
        }

        private void FrmIntegridad_Load(object sender, EventArgs e)
        {
            servicesBL.setPatentesUsuarioLog(userLog);
            listBox1.SelectionMode = SelectionMode.MultiExtended;
            setListaIncidentes();
            validarPatente();
        }

        private void validarPatente()
        {
            label2.Visible = servicesBL.validarPatente(26);
            button1.Text = servicesBL.validarPatente(26) ? "CONTINUAR" : "ACEPTAR";
        }

        private void setListaIncidentes()
        {
            listBox1.Items.Clear();
            foreach (var item in lista_logs)
            {
                string initStr = "Inconsistencia de datos detectada ";
                if (item.id_registro > 0)
                {
                    initStr += "en Entidad: " + item.entidad + " - Registro (ID): " + item.id_registro;
                }
                else
                {
                    initStr += "al calcular DVV en Entidad: " + item.entidad;
                }
                listBox1.Items.Add(initStr);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (servicesBL.validarPatente(26))
            {
                this.Close();
                Distar.FrmNavigation _FrmNavigation = new Distar.FrmNavigation(userLog, lista_logs);
                _FrmNavigation.Show();
            }
            else
            {
                Application.Exit();
            }
        }
    }
}
