using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distar_LogicaNegocio
{
    public class DigitoVerificador
    {
        private const string NOMBRE_ENTIDAD_USUARIO = "Usuario";
        private const string NOMBRE_ENTIDAD_BITACORA = "Bitacora";
        private const string NOMBRE_ENTIDAD_TICKET = "Ticket";
        private const string NOMBRE_ENTIDAD_PRODUCTO = "Producto";
        private const string NOMBRE_ENTIDAD_USUARIOPATENTE = "UsuarioPatente";
        private const string NOMBRE_ENTIDAD_FAMILIAPATENTE = "FamiliaPatente";
        private const string NOMBRE_ENTIDAD_CUENTACORRIENTE = "CuentaCorriente";
        private static Distar_EntidadesNegocio.Usuario userLog;
        public List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO> verificarIntegridad(Distar_EntidadesNegocio.Usuario user)
        {
            userLog = user;
            Distar_LogicaNegocio.Bitacora bitacoraBL = new Distar_LogicaNegocio.Bitacora();
            List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO> retVerificacion = new List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO>();
            retVerificacion = verificarDVHEntidad(NOMBRE_ENTIDAD_USUARIO);
            retVerificacion.AddRange(verificarDVHEntidad(NOMBRE_ENTIDAD_BITACORA));
            retVerificacion.AddRange(verificarDVHEntidad(NOMBRE_ENTIDAD_TICKET));
            retVerificacion.AddRange(verificarDVHEntidad(NOMBRE_ENTIDAD_PRODUCTO));
            retVerificacion.AddRange(verificarDVHEntidad(NOMBRE_ENTIDAD_USUARIOPATENTE));
            retVerificacion.AddRange(verificarDVHEntidad(NOMBRE_ENTIDAD_FAMILIAPATENTE));
            retVerificacion.AddRange(verificarDVHEntidad(NOMBRE_ENTIDAD_CUENTACORRIENTE));
            if (retVerificacion.Count == 0)
            {
                bitacoraBL.setINFO(DateTime.Now, userLog, "Integridad de datos", "No se encontraron inconsistencias de datos en almacenamiento.");
            }
            return retVerificacion;
        }

        public List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO> verificarDVHEntidad(string ENTIDAD)
        {
            int valor_dv = 0;
            int valor_sum_vertical = 0;
            List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO> lista_RV = new List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO>();
            List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO> lista_RVD = new List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO>();
            Distar_LogicaNegocio.Bitacora bitacoraBL = new Distar_LogicaNegocio.Bitacora();
            Distar_AccesoDatos.DigitoVerificador digitoVerificadorDAL = new Distar_AccesoDatos.DigitoVerificador();
            switch (ENTIDAD)
            {
                case NOMBRE_ENTIDAD_USUARIO:
                    Distar_AccesoDatos.Usuario usuarioDAL = new Distar_AccesoDatos.Usuario();
                    lista_RV = usuarioDAL.obtenerDTO_DV();
                    valor_dv = 0;
                    valor_dv = digitoVerificadorDAL.obtenerDVV(NOMBRE_ENTIDAD_USUARIO);
                    break;
                case NOMBRE_ENTIDAD_BITACORA:
                    lista_RV = Distar_AccesoDatos.Bitacora.GetInstance().obtenerDTO_DV();
                    valor_dv = 0;
                    valor_dv = digitoVerificadorDAL.obtenerDVV(NOMBRE_ENTIDAD_BITACORA);
                    break;
                case NOMBRE_ENTIDAD_USUARIOPATENTE:
                    Distar_AccesoDatos.UsuarioPatente usuarioPatenteDAL = new Distar_AccesoDatos.UsuarioPatente();
                    lista_RV = usuarioPatenteDAL.obtenerDTO_DV();
                    valor_dv = 0;
                    valor_dv = digitoVerificadorDAL.obtenerDVV(NOMBRE_ENTIDAD_USUARIOPATENTE);
                    break;
                case NOMBRE_ENTIDAD_FAMILIAPATENTE:
                    Distar_AccesoDatos.FamiliaPatente familiaPatenteDAL = new Distar_AccesoDatos.FamiliaPatente();
                    lista_RV = familiaPatenteDAL.obtenerDTO_DV();
                    valor_dv = 0;
                    valor_dv = digitoVerificadorDAL.obtenerDVV(NOMBRE_ENTIDAD_FAMILIAPATENTE);
                    break;
                case NOMBRE_ENTIDAD_PRODUCTO:
                    Distar_AccesoDatos.Producto productoDAL = new Distar_AccesoDatos.Producto();
                    lista_RV = productoDAL.obtenerDTO_DV();
                    valor_dv = 0;
                    valor_dv = digitoVerificadorDAL.obtenerDVV(NOMBRE_ENTIDAD_PRODUCTO);
                    break;
                case NOMBRE_ENTIDAD_CUENTACORRIENTE:
                    Distar_AccesoDatos.CuentaCorriente cuentaCorrienteDAL = new Distar_AccesoDatos.CuentaCorriente();
                    lista_RV = cuentaCorrienteDAL.obtenerDTO_DV();
                    valor_dv = 0;
                    valor_dv = digitoVerificadorDAL.obtenerDVV(NOMBRE_ENTIDAD_CUENTACORRIENTE);
                    break;
                case NOMBRE_ENTIDAD_TICKET:
                    Distar_AccesoDatos.Ticket ticketDAL = new Distar_AccesoDatos.Ticket();
                    lista_RV = ticketDAL.obtenerDTO_DV();
                    valor_dv = 0;
                    valor_dv = digitoVerificadorDAL.obtenerDVV(NOMBRE_ENTIDAD_TICKET);
                    break;
                default:
                    break;
            }
            foreach (Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO registro in lista_RV)
            {
                Distar_EntidadesNegocio.Usuario sistema = new Distar_EntidadesNegocio.Usuario();
                Distar_AccesoDatos.Services services = new Distar_AccesoDatos.Services();
                sistema.email = services.Encriptar3D("Sistema");
                registro.valor_calc = calcularDVH(registro.txtstr);
                if (!(registro.valor_calc == registro.valor_db))
                {
                    bitacoraBL.setERROR(DateTime.Now, userLog, "Integridad de datos", "Inconsistencia de datos detectada en entidad: " + registro.entidad + " - registro: " + registro.id_registro);
                    Console.WriteLine("Inconsistencia de datos detectada en Entidad: " + registro.entidad + " - Registro (ID): " + registro.id_registro);
                    lista_RVD.Add(registro);
                }
                valor_sum_vertical += registro.valor_db;
            }
            if (!(valor_dv == valor_sum_vertical))
            {
                Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO DVV = new Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO();
                DVV.entidad = ENTIDAD;
                DVV.valor_db = valor_dv;
                DVV.valor_calc = valor_sum_vertical;
                bitacoraBL.setERROR(DateTime.Now, userLog, "Integridad de datos", "Inconsistencia de datos detectada al calcular DVV en entidad: " + DVV.entidad);
                Console.WriteLine("Inconsistencia de datos detectada al calcular DVV en Entidad: " + DVV.entidad);
                lista_RVD.Add(DVV);
            }
            return lista_RVD;
        }

        public int calcularDVH(string registroStr)
        {
            int valorFinal = 0;
            int valorAscPosicion;
            char[] valorCharArray = registroStr.ToCharArray();
            for (int i = 0; i < valorCharArray.Length; i++)
            {
                valorAscPosicion = (valorCharArray[i] * (i + 1));
                valorFinal = valorFinal + valorAscPosicion;
            }
            return valorFinal;
        }

        public Boolean recalcularDVPorEntidadRegistro(string entidad, int id_registro, string valorCalculado=null){
            switch (entidad)
            {
                case NOMBRE_ENTIDAD_USUARIO:
                    Distar_AccesoDatos.Usuario usuarioDAL = new Distar_AccesoDatos.Usuario();
                    if (valorCalculado == null){
                        Distar_LogicaNegocio.Usuario usuarioBL = new Distar_LogicaNegocio.Usuario();
                        valorCalculado = usuarioBL.calcularDVHPorRegistro(id_registro.ToString()).ToString();
                    }
                    usuarioDAL.actualizarDVH(valorCalculado, id_registro);
                    break;
                case NOMBRE_ENTIDAD_BITACORA:
                    Distar_AccesoDatos.Bitacora.GetInstance().actualizarDVH(valorCalculado, id_registro);
                    break;
                case NOMBRE_ENTIDAD_TICKET:
                    Distar_AccesoDatos.Ticket ticketDAL = new Distar_AccesoDatos.Ticket();
                    if (valorCalculado == null){
                        Distar_LogicaNegocio.Ticket ticketBL = new Distar_LogicaNegocio.Ticket();
                        valorCalculado = ticketBL.calcularDVHPorRegistro(id_registro.ToString()).ToString();
                    }
                    ticketDAL.actualizarDVH(valorCalculado, id_registro);
                    break;
                case NOMBRE_ENTIDAD_PRODUCTO:
                    Distar_AccesoDatos.Producto productoDAL = new Distar_AccesoDatos.Producto();
                    if (valorCalculado == null){
                        Distar_LogicaNegocio.Producto productoBL = new Distar_LogicaNegocio.Producto();
                        valorCalculado = productoBL.calcularDVHPorRegistro(id_registro.ToString()).ToString();
                    }
                    productoDAL.actualizarDVH(valorCalculado, id_registro);
                    break;
                case NOMBRE_ENTIDAD_CUENTACORRIENTE:
                    Distar_AccesoDatos.CuentaCorriente cuentaCorrienteDAL = new Distar_AccesoDatos.CuentaCorriente();
                    if (valorCalculado == null)
                    {
                        Distar_LogicaNegocio.CuentaCorriente cuentaCorrienteBL = new Distar_LogicaNegocio.CuentaCorriente();
                        valorCalculado = cuentaCorrienteBL.calcularDVHPorRegistro(id_registro).ToString();
                    }
                    cuentaCorrienteDAL.actualizarDVH(valorCalculado, id_registro);
                    break;
                // FamiliaUsuario y FamiliaPatente se actualizan "solos" porque siempre se realiza alta-baja, no modificación
            }
            return true;
        }

        public Boolean recalcularDVV(string entidad, int valor)
        {
            Distar_AccesoDatos.DigitoVerificador digitoVerificadorDAL = new Distar_AccesoDatos.DigitoVerificador();
            if (digitoVerificadorDAL.actualizarDVV(entidad, valor))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean recalcularDVV_lista(List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO> lista)
        {
            Distar_AccesoDatos.DigitoVerificador digitoVerificadorDAL = new Distar_AccesoDatos.DigitoVerificador();
            return digitoVerificadorDAL.actualizarDVVList(lista);
        }

        public Boolean actualizarDVV(string entidad)
        {
            int sumaDVH;
            Distar_AccesoDatos.DigitoVerificador digitoVerificadorDAL = new Distar_AccesoDatos.DigitoVerificador();
            sumaDVH = digitoVerificadorDAL.obtenerSumaDVH(entidad);
            if (sumaDVH > -1)
            {
                if (digitoVerificadorDAL.actualizarDVV(entidad, sumaDVH))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void actualizarDVHRegistros(string entidad, int id_registro)
        {
            recalcularDVPorEntidadRegistro(entidad, id_registro);
            actualizarDVV(entidad);
        }
    }
}
