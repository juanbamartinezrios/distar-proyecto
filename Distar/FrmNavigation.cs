using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Distar
{
    public partial class FrmNavigation : Form
    {
        public FrmNavigation()
        {
            InitializeComponent();
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Distar.ADMIN.FrmUsuarios.ActiveForm.ShowDialog();
        }
    }
}
