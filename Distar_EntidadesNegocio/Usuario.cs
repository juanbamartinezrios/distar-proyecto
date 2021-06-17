using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distar_EntidadesNegocio
{
    public class Usuario
    {
        public Boolean activo { get; set; }
        public string apellido { get; set; }
        public int cont_ingresos_incorrectos { get; set; }
        public string contraseña { get; set; }
        public string documento { get; set; }
        public int DVH { get; set; }
        public string email { get; set; }
        public int id_usuario { get; set; }
        public int id_idioma_usuario { get; set; }
        public string nombre { get; set; }
        public List<Patente> patentes { get; set; }
        public List<Familia> familias { get; set; }
        public Domicilio domicilio { get; set; }
        public Telefono telefono { get; set; }
        public CuentaCorriente cta_cte { get; set; }
    }
}
