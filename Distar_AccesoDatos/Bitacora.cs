using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;

namespace Distar_AccesoDatos
{
    public class Bitacora
    {
        public static Bitacora instancia;
        public static Bitacora GetInstance()
        {
            if (instancia == null)
            {
                return new Bitacora();
            }
            return instancia;
        }

        public void guardarLog(Distar_EntidadesNegocio.Bitacora log)
        {
            string query = "INSERT INTO Bitacora VALUES('" + log.criticidad + "', '" + log.descripcion + "', " + log.DVH + ", CONVERT(datetime, '" + log.fecha.ToString("yyyy-MM-dd HH:mm") + "', 101), '" + log.funcionalidad + "', " + log.id_usuario + ")";
            try
            {
                Distar_BD.ExecuteNonquery(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Bitacora guardarLog: " + ex.Message);
            }
        }

        public List<Distar_EntidadesNegocio.Bitacora> obtenerResultados(Distar_EntidadesNegocio.DTO.BitacoraDTO filtros_log)
        {
            Distar_AccesoDatos.Services services = new Distar_AccesoDatos.Services();
            List<Distar_EntidadesNegocio.Bitacora> lista_result = new List<Distar_EntidadesNegocio.Bitacora>();
            string query = "SELECT id_log, fecha, Bitacora.id_usuario, funcionalidad, descripcion, criticidad, Bitacora.DVH, email FROM Bitacora INNER JOIN Usuario ON Bitacora.id_usuario = Usuario.id_usuario WHERE fecha >= CONVERT(datetime, '" + filtros_log.fechaDesde + "', 101)" + " AND fecha <= CONVERT(datetime, '" + filtros_log.fechaHasta + "', 101)";
            if (filtros_log.lista_usuarios != null)
                query += " AND Bitacora.id_usuario IN (" + filtros_log.lista_usuarios + ")";
            if (filtros_log.lista_criticidad != null)
                query += " AND criticidad IN (" + filtros_log.lista_criticidad + ")";
            query += " AND Bitacora.id_usuario != 0 ORDER BY fecha DESC";
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    Distar_EntidadesNegocio.Bitacora bitacora = new Distar_EntidadesNegocio.Bitacora();
                    bitacora.id_usuario = Convert.ToInt32(dataReader["id_usuario"]);
                    bitacora.criticidad = Convert.ToString(dataReader["criticidad"]);
                    bitacora.descripcion = Convert.ToString(dataReader["descripcion"]);
                    bitacora.DVH = Convert.ToInt32(dataReader["DVH"]);
                    bitacora.fecha = Convert.ToDateTime(dataReader["fecha"]);
                    bitacora.funcionalidad = services.Desencriptar3D(Convert.ToString(dataReader["funcionalidad"]));
                    bitacora.id_log = Convert.ToInt32(dataReader["id_log"]);
                    bitacora.usuario_email = services.Desencriptar3D(Convert.ToString(dataReader["email"]));
                    lista_result.Add(bitacora);
                }
                dataReader.Close();
                return lista_result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Bitacora obtenerResultados: " + ex.Message); 
                return lista_result;
            }
        }

        public Boolean actualizarDVH(string valorCalculado, int id_registro)
        {
            string query = "UPDATE Bitacora SET DVH=" + valorCalculado + " WHERE id_log=" + id_registro;
            try
            {
                return Distar_BD.ExecuteNonquery(query) > -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Bitacora actualizarDVH: " + ex.Message); 
                return false;
            }
        }

        public List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO> obtenerDTO_DV()
        {
            string query = "SELECT criticidad, descripcion, fecha, funcionalidad, id_usuario, id_log, DVH FROM Bitacora";
            List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO> lista_dv_dto = new List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO>();
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO dto = new Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO();
                    dto.entidad = "Bitacora";
                    dto.valor_db = Convert.ToInt32(dataReader["DVH"]);
                    dto.id_registro = Convert.ToInt32(dataReader["id_log"]);
                    dto.txtstr = Convert.ToString(dataReader["criticidad"]) + Convert.ToString(dataReader["descripcion"]) + Convert.ToDateTime(dataReader["fecha"]).ToString("yyyy-MM-dd HH:mm") + Convert.ToString(dataReader["funcionalidad"]) + Convert.ToString(dataReader["id_usuario"]);
                    lista_dv_dto.Add(dto);
                }
                dataReader.Close();
                return lista_dv_dto;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Bitacora obtenerDTO_DV: " + ex.Message); 
                return lista_dv_dto;
            }
        }
    }
}
