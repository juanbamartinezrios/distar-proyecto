using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distar_LogicaNegocio
{
    public class Impresion
    {
        public void registrarImpresion(Distar_EntidadesNegocio.Impresion nueva_impresion)
        {
            Distar_AccesoDatos.Impresion impresionDAL = new Distar_AccesoDatos.Impresion();
            impresionDAL.registrarImpresion(nueva_impresion);
        }
    }
}
