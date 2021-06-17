using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distar_LogicaNegocio
{
    public class Telefono
    {
        public Boolean agregarTelefonoUsuario(Distar_EntidadesNegocio.Telefono telefono)
        {
            Distar_AccesoDatos.Telefono telefonoDAL = new Distar_AccesoDatos.Telefono();
            return telefonoDAL.agregarTelefonoUsuario(telefono);
        }

        public Boolean update(Distar_EntidadesNegocio.Telefono telefono)
        {
            Distar_AccesoDatos.Telefono telefonoDAL = new Distar_AccesoDatos.Telefono();
            if (telefono.id_telefono != 0)
            {
                return telefonoDAL.update(telefono);
            }
            else
            {
                return agregarTelefonoUsuario(telefono);
            }
        }

        public Distar_EntidadesNegocio.Telefono obtenerTelefonoPorId(int id_telefono)
        {
            Distar_AccesoDatos.Telefono telefonoDAL = new Distar_AccesoDatos.Telefono();
            return telefonoDAL.obtenerTelefonoPorId(id_telefono);
        }
    }
}
