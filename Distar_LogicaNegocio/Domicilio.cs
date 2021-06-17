using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distar_LogicaNegocio
{
    public class Domicilio
    {
        public Boolean agregarDomicilioUsuario(Distar_EntidadesNegocio.Domicilio domicilio)
        {
            Distar_AccesoDatos.Domicilio domicilioDAL = new Distar_AccesoDatos.Domicilio();
            return domicilioDAL.agregarDomicilioUsuario(domicilio);
        }

        public Boolean update(Distar_EntidadesNegocio.Domicilio domicilio)
        {
            Distar_AccesoDatos.Domicilio domicilioDAL = new Distar_AccesoDatos.Domicilio();
            Distar_AccesoDatos.Services services =  new Distar_AccesoDatos.Services();
            domicilio.cp = services.Encriptar3D(domicilio.cp);
            domicilio.direccion = services.Encriptar3D(domicilio.direccion);
            if (domicilio.id_domicilio != 0)
            {
                return domicilioDAL.update(domicilio);
            }
            else
            {
                return domicilioDAL.agregarDomicilioUsuario(domicilio);
            }
        }

        public Distar_EntidadesNegocio.Domicilio obtenerDomicilioPorId(int id_domicilio)
        {
            Distar_AccesoDatos.Domicilio domicilioDAL = new Distar_AccesoDatos.Domicilio();
            return domicilioDAL.obtenerDomicilioPorId(id_domicilio);
        }
    }
}
