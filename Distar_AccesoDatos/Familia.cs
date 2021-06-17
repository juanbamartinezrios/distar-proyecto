using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Distar_AccesoDatos
{
    public class Familia : Distar_EntidadesNegocio.ABMGenerico<Distar_EntidadesNegocio.Familia>
    {
        public List<Distar_EntidadesNegocio.Familia> getFamiliasByIdUsuario(int id_usuario){
            List<Distar_EntidadesNegocio.Familia> familiasUsuario = new List<Distar_EntidadesNegocio.Familia>();
            string query = "SELECT Familia.id_familia, Familia.descripcion FROM Familia INNER JOIN FamiliaUsuario ON Familia.id_familia=FamiliaUsuario.id_familia WHERE FamiliaUsuario.id_usuario="+id_usuario;
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    Distar_EntidadesNegocio.Familia familia = new Distar_EntidadesNegocio.Familia();
                    familia.id_familia = Convert.ToInt32(dataReader["id_familia"]);
                    familia.descripcion = Convert.ToString(dataReader["descripcion"]);
                    familiasUsuario.Add(familia);
                }
                dataReader.Close();
                return familiasUsuario;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Familia getFamiliasByIdUsuario: " + ex.Message);
                return familiasUsuario;
            }
        }

        public List<Distar_EntidadesNegocio.Familia> getAllFamilias()
        {
            List<Distar_EntidadesNegocio.Familia> listaFamilias = new List<Distar_EntidadesNegocio.Familia>();
            string query = "SELECT id_familia, descripcion FROM Familia";
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    Distar_EntidadesNegocio.Familia familia = new Distar_EntidadesNegocio.Familia();
                    familia.id_familia = Convert.ToInt32(dataReader["id_familia"]);
                    familia.descripcion = Convert.ToString(dataReader["descripcion"]);
                    listaFamilias.Add(familia);
                }
                dataReader.Close();
                return listaFamilias;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Familia getAllFamilias: " + ex.Message);
                return listaFamilias;
            }
        }

        public List<Distar_EntidadesNegocio.Familia> obtenerFamiliasSinUsuariosActivos()
        {
            List<Distar_EntidadesNegocio.Familia> listaFamilias = new List<Distar_EntidadesNegocio.Familia>();
            string query = "SELECT id_familia FROM Familia WHERE id_familia NOT IN (SELECT id_familia FROM FamiliaUsuario WHERE id_usuario IN (SELECT id_usuario FROM Usuario WHERE (activo=1 AND cont_ingresos_incorrectos < 3)))";
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    Distar_EntidadesNegocio.Familia familia = new Distar_EntidadesNegocio.Familia();
                    familia.id_familia = Convert.ToInt32(dataReader["id_familia"]);
                    listaFamilias.Add(familia);
                }
                dataReader.Close();
                return listaFamilias;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Familia obtenerFamiliasSinUsuariosActivos: " + ex.Message);
                return listaFamilias;
            }
        }

        public Boolean create(Distar_EntidadesNegocio.Familia familia)
        {
            string query = "INSERT INTO Familia VALUES ('" + familia.descripcion + "')";
            try
            {
                return Distar_BD.ExecuteNonquery(query) > -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Familia create: " + ex.Message);
                return false;
            }
        }

        public Boolean delete(Distar_EntidadesNegocio.Familia familia)
        {
            string query = "DELETE FROM Familia WHERE id_familia="+familia.id_familia;
            try
            {
                return Distar_BD.ExecuteNonquery(query) > -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Familia delete: " + ex.Message);
                return false;
            }
        }

        public Boolean deleteLogico(Distar_EntidadesNegocio.Familia familia)
        {
            return false;
        }

        public Boolean update(Distar_EntidadesNegocio.Familia familia)
        {
            string query = "UPDATE Familia SET descripcion ='" + familia.descripcion + "' WHERE id_familia=" + familia.id_familia;
            try
            {
                return Distar_BD.ExecuteNonquery(query) > -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Familia update: " + ex.Message);
                return false;
            }
        }

        public Distar_EntidadesNegocio.Familia getFamiliaById(int id_familia)
        {
            Distar_EntidadesNegocio.Familia familia = new Distar_EntidadesNegocio.Familia();
            string query = "SELECT * FROM Familia WHERE id_familia=" + id_familia;
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                if (dataReader != null && dataReader.HasRows)
                {
                    dataReader.Read();
                    familia.id_familia = Convert.ToInt32(dataReader["id_familia"]);
                    familia.descripcion = Convert.ToString(dataReader["descripcion"]);
                }
                dataReader.Close();
                return familia;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Familia getFamiliaById: " + ex.Message);
                return familia;
            }
        }
    }
}
