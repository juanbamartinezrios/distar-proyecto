using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Distar_LogicaNegocio
{
    public class Bitacora
    {
        private const string CRITICIDAD_ALTA = "ALTA";
        private const string CRITICIDAD_MEDIA = "MEDIA";
        private const string CRITICIDAD_BAJA = "BAJA";

        public void setERROR(DateTime fecha, Distar_EntidadesNegocio.Usuario user, string funcionalidad, string descripcion)
        {
            Distar_EntidadesNegocio.Bitacora log = new Distar_EntidadesNegocio.Bitacora();
            Distar_LogicaNegocio.DigitoVerificador DVBL = new Distar_LogicaNegocio.DigitoVerificador();
            Distar_AccesoDatos.Services services = new Distar_AccesoDatos.Services();

            log.criticidad = CRITICIDAD_ALTA;
            log.descripcion = descripcion;
            log.fecha = fecha;
            log.funcionalidad = services.Encriptar3D(funcionalidad);
            log.id_usuario = user.id_usuario;
            log.DVH = DVBL.calcularDVH(log.criticidad + log.descripcion + log.fecha.ToString("yyyy-MM-dd HH:mm") + log.funcionalidad + log.id_usuario.ToString());
            Distar_AccesoDatos.Bitacora.GetInstance().guardarLog(log);
            DVBL.actualizarDVV("Bitacora");
        }

        public void setWARNING(DateTime fecha, Distar_EntidadesNegocio.Usuario user, string funcionalidad, string descripcion)
        {
            Distar_EntidadesNegocio.Bitacora log = new Distar_EntidadesNegocio.Bitacora();
            Distar_LogicaNegocio.DigitoVerificador DVBL = new Distar_LogicaNegocio.DigitoVerificador();
            Distar_AccesoDatos.Services services = new Distar_AccesoDatos.Services();

            log.criticidad = CRITICIDAD_MEDIA;
            log.descripcion = descripcion;
            log.fecha = fecha;
            log.funcionalidad = services.Encriptar3D(funcionalidad);
            log.id_usuario = user.id_usuario;
            log.DVH = DVBL.calcularDVH(log.criticidad + log.descripcion + log.fecha.ToString("yyyy-MM-dd HH:mm") + log.funcionalidad + log.id_usuario.ToString());
            Distar_AccesoDatos.Bitacora.GetInstance().guardarLog(log);
            DVBL.actualizarDVV("Bitacora");
        }

        public void setINFO(DateTime fecha, Distar_EntidadesNegocio.Usuario user, string funcionalidad, string descripcion)
        {
            Distar_EntidadesNegocio.Bitacora log = new Distar_EntidadesNegocio.Bitacora();
            Distar_LogicaNegocio.DigitoVerificador DVBL = new Distar_LogicaNegocio.DigitoVerificador();
            Distar_AccesoDatos.Services services = new Distar_AccesoDatos.Services();
            Distar_AccesoDatos.Bitacora bitacoraDAL = new Distar_AccesoDatos.Bitacora();

            log.criticidad = CRITICIDAD_BAJA;
            log.descripcion = descripcion;
            log.fecha = fecha;
            log.funcionalidad = services.Encriptar3D(funcionalidad);
            log.id_usuario = user.id_usuario;
            log.DVH = DVBL.calcularDVH(log.criticidad + log.descripcion + log.fecha.ToString("yyyy-MM-dd HH:mm") + log.funcionalidad + log.id_usuario.ToString());
            Distar_AccesoDatos.Bitacora.GetInstance().guardarLog(log);
            DVBL.actualizarDVV("Bitacora");
        }

        public List<string> getCriticidad()
        {
            List<string> listaCriticidad = new List<string>();
            listaCriticidad.Add(CRITICIDAD_ALTA);
            listaCriticidad.Add(CRITICIDAD_MEDIA);
            listaCriticidad.Add(CRITICIDAD_BAJA);
            return listaCriticidad;
        }

        public List<Distar_EntidadesNegocio.Bitacora> filtrarBitacora(Distar_EntidadesNegocio.DTO.BitacoraDTO filtros_log)
        {
            List<Distar_EntidadesNegocio.Bitacora> lista_log = new List<Distar_EntidadesNegocio.Bitacora>();
            List<Distar_EntidadesNegocio.Bitacora> lista_log_filtrada = new List<Distar_EntidadesNegocio.Bitacora>();
            Distar_AccesoDatos.Services services = new Distar_AccesoDatos.Services();
            Distar_LogicaNegocio.Usuario usuarioBL = new Distar_LogicaNegocio.Usuario();
            lista_log = Distar_AccesoDatos.Bitacora.GetInstance().obtenerResultados(filtros_log);
            return lista_log;
        }
    }
}
