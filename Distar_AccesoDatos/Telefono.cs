using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Distar_AccesoDatos
{
    public class Telefono
    {

        public Boolean agregarTelefonoUsuario(Distar_EntidadesNegocio.Telefono telefono)
        {
            string query = "INSERT INTO Telefono VALUES (" + telefono.id_usuario + ", '" + telefono.telefono + "', '" + telefono.telefono_alt + "')";
            try
            {
                return Distar_BD.ExecuteNonquery(query) > -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL agregarTelefonoUsuario: " + ex.Message);
                return false;
            }
        }

        public Boolean update(Distar_EntidadesNegocio.Telefono telefono)
        {
            string query = "UPDATE Telefono SET ";
            if (!string.IsNullOrEmpty(telefono.telefono))
                query = query + " telefono='" + telefono.telefono + "',";
            if (!string.IsNullOrEmpty(telefono.telefono_alt))
                query = query + " telefono_alt='" + telefono.telefono_alt + "',";
            query = query.Substring(0, query.Length - 1) + " WHERE id_telefono=" + telefono.id_telefono + " AND id_usuario=" + telefono.id_usuario;
            try
            {
                return Distar_BD.ExecuteNonquery(query) > -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL update: " + ex.Message);
                return false;
            }
        }

        public Distar_EntidadesNegocio.Telefono getTelefonoByIdUsuario(int id_usuario)
        {
            string query = "SELECT * FROM Telefono WHERE id_usuario=" + Convert.ToString(id_usuario);
            Distar_EntidadesNegocio.Telefono telefono = new Distar_EntidadesNegocio.Telefono();
            SqlDataReader dataReader;
            dataReader = Distar_BD.ExecuteReader(query);
            if (dataReader.HasRows)
            {
                dataReader.Read();
                telefono.id_telefono = Convert.ToInt32(dataReader["id_telefono"]);
                telefono.telefono = Convert.ToString(dataReader["telefono"]);
                telefono.telefono_alt = Convert.ToString(dataReader["telefono_alt"]);
            }
            dataReader.Close();
            return telefono;
        }

        public Distar_EntidadesNegocio.Telefono obtenerTelefonoPorId(int id_telefono)
        {
            string query = "SELECT * FROM Telefono WHERE id_telefono=" + id_telefono;
            Distar_EntidadesNegocio.Telefono telefono = new Distar_EntidadesNegocio.Telefono();
            SqlDataReader dataReader;
            dataReader = Distar_BD.ExecuteReader(query);
            if (dataReader.HasRows)
            {
                dataReader.Read();
                telefono.id_telefono = Convert.ToInt32(dataReader["id_telefono"]);
                telefono.telefono = Convert.ToString(dataReader["telefono"]);
                telefono.telefono_alt = Convert.ToString(dataReader["telefono_alt"]);
            }
            dataReader.Close();
            return telefono;
        }
    }
}
