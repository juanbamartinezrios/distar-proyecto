using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Distar_AccesoDatos
{
    public class DigitoVerificador
    {
        private const string NOMBRE_ENTIDAD_USUARIO = "Usuario";
        private const string NOMBRE_ENTIDAD_BITACORA = "Bitacora";
        private const string NOMBRE_ENTIDAD_TICKET = "Ticket";
        private const string NOMBRE_ENTIDAD_PRODUCTO = "Producto";
        private const string NOMBRE_ENTIDAD_USUARIOPATENTE = "UsuarioPatente";
        private const string NOMBRE_ENTIDAD_FAMILIAPATENTE = "FamiliaPatente";

        public int obtenerDVV(string entidad) {
            string query = "SELECT valor_dv FROM DVV WHERE entidad='"+entidad+"'";
            SqlDataReader dataReader;
            int valor_dv = 0;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    if (dataReader[0] == null)
                    {
                        valor_dv = 0;
                    }
                    else
                    {
                        valor_dv = Convert.ToInt32(dataReader[0]);
                    }
                }
                dataReader.Close();
                return valor_dv;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL DigitoVerificador obtenerDVV: " + ex.Message);
                return 0;
            }
        }

        public Boolean actualizarDVV(string entidad, int valor) {
            string query = "UPDATE DVV SET valor_dv="+valor+" WHERE entidad='"+ entidad + "'";
            try
            {
                return Distar_BD.ExecuteNonquery(query) != -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL DigitoVerificador actualizarDVV: " + ex.Message);
                return false;
            }
        }

        public Boolean actualizarDVVList(List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO> lista)
        {
            string query = "";
            foreach (var item in lista)
            {
                if (query.Length > 0)
                {
                    query+=";";
                }
                query += "UPDATE DVV SET valor_dv=" + item.valor_calc + " WHERE entidad='" + item.entidad + "'";
            }
            try
            {
                return Distar_BD.ExecuteNonquery(query) != -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL DigitoVerificador actualizarDVVList: " + ex.Message);
                return false;
            }
        }

        public int obtenerSumaDVH(string entidad)
        {
            string query = "SELECT SUM(DVH) FROM "+entidad;
            SqlDataReader dataReader;
            int returnVal;
            dataReader = Distar_BD.ExecuteReader(query);
            try {
                if (dataReader.HasRows) {
                    dataReader.Read();
                    returnVal = dataReader[0] is System.DBNull ? 0 : Convert.ToInt32(dataReader[0]);
                    return returnVal;
                } else {
                    return -1;
                }
            } catch(Exception ex) {
                Console.WriteLine("DAL DigitoVerificador obtenerSumaDVH: " + ex.Message);
                return -1;
            } finally {
                dataReader.Close();
            }
        }
    }
}
