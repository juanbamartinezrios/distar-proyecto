using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Distar_AccesoDatos
{
    public class Idioma
    {
        public Distar_EntidadesNegocio.Idioma getIdiomaById(int id_idioma_usuario){
            Distar_EntidadesNegocio.Idioma idioma = new Distar_EntidadesNegocio.Idioma();
            string query = "SELECT * FROM Idioma WHERE id_idioma="+id_idioma_usuario;
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    idioma.id_idioma = Convert.ToInt32(dataReader["id_idioma"]);
                    idioma.descripcion = Convert.ToString(dataReader["descripcion"]);
                }
                dataReader.Close();
                return idioma;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Idiomal getIdiomaById: " + ex.Message);
                return idioma;
            }
        }

        public List<Distar_EntidadesNegocio.Idioma> getAllIdiomas()
        {
            List<Distar_EntidadesNegocio.Idioma> listaIdiomas = new List<Distar_EntidadesNegocio.Idioma>();
            string query = "SELECT id_idioma, descripcion FROM Idioma";
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    Distar_EntidadesNegocio.Idioma idioma = new Distar_EntidadesNegocio.Idioma();
                    idioma.id_idioma = Convert.ToInt32(dataReader["id_idioma"]);
                    idioma.descripcion = Convert.ToString(dataReader["descripcion"]);
                    listaIdiomas.Add(idioma);
                }
                dataReader.Close();
                return listaIdiomas;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Idiomal getAllIdiomas: " + ex.Message);
                return listaIdiomas;
            }
        }
    }
}
