using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distar_LogicaNegocio
{
    public class CuentaCorriente
    {
        public int calcularDVHPorRegistro(int id_cta_cte)
        {
            Distar_AccesoDatos.CuentaCorriente cuentaCorrienteDAL = new Distar_AccesoDatos.CuentaCorriente();
            Distar_LogicaNegocio.DigitoVerificador DVBL = new Distar_LogicaNegocio.DigitoVerificador();
            string str = cuentaCorrienteDAL.calcularDVHPorRegistro(id_cta_cte);
            return DVBL.calcularDVH(str);
        }

        public string getRandomCtaNum()
        {
            string randomStr;
            string substr1;
            string substr2;
            string s = "0123456789";
            Random rd = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 10; i++)
            {
                int idx = rd.Next(0, 5);
                sb.Append(s.Substring(idx, 1));
            }
            randomStr = sb.ToString();
            substr1 = randomStr.Substring(0, 6);
            substr2 = randomStr.Substring(7, 3);
            randomStr = substr1 + "-" + substr1.Substring(3, 1) + " " + substr2 + "-" + substr2.Substring(1,1);
            return randomStr;
        }

        public Boolean agregarCtaCteUsuario(Distar_EntidadesNegocio.CuentaCorriente ctaCte)
        {
            Distar_AccesoDatos.CuentaCorriente cuentaCorrienteDAL = new Distar_AccesoDatos.CuentaCorriente();
            Distar_LogicaNegocio.DigitoVerificador DVBL = new Distar_LogicaNegocio.DigitoVerificador();
            if (cuentaCorrienteDAL.agregarCtaCteUsuario(ctaCte))
            {
                int id_cta_cte = this.getIdCtaCte(ctaCte.nro_cta);
                DVBL.actualizarDVHRegistros("CuentaCorriente", id_cta_cte);
                return true;
            }
            else
            {
                return false;
            }
        }

        public int getIdCtaCte(string nro_cta)
        {
            Distar_AccesoDatos.CuentaCorriente cuentaCorrienteDAL = new Distar_AccesoDatos.CuentaCorriente();
            return cuentaCorrienteDAL.getIdCtaCte(nro_cta);
        }

        public Distar_EntidadesNegocio.CuentaCorriente obtenerCtaCtePorId(int id_cta_cte)
        {
            Distar_AccesoDatos.CuentaCorriente cuentaCorrienteDAL = new Distar_AccesoDatos.CuentaCorriente();
            return cuentaCorrienteDAL.obtenerCtaCtePorId(id_cta_cte);
        }

        public Distar_EntidadesNegocio.CuentaCorriente cargarCuentaCorrienteDeusuario(int id_usuario)
        {
            Distar_AccesoDatos.CuentaCorriente cuentaCorrienteDAL = new Distar_AccesoDatos.CuentaCorriente();
            return cuentaCorrienteDAL.cargarCuentaCorrienteDeusuario(id_usuario);
        }

        public float getSaldoDeudor(int id_cliente)
        {
            float saldo = 0;
            Distar_AccesoDatos.Ticket ticketDAL = new Distar_AccesoDatos.Ticket();
            List<Distar_EntidadesNegocio.DetalleFactura> lista_detalle = new List<Distar_EntidadesNegocio.DetalleFactura>();
            lista_detalle = ticketDAL.getImportes(id_cliente);
            foreach (Distar_EntidadesNegocio.DetalleFactura detalle in lista_detalle)
            {
                saldo += detalle.importe;
            }
            return (float)(saldo * 1.21);
        }
    }
}
