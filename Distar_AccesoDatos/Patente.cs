using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Distar_AccesoDatos
{
    public class Patente
    {
        public List<Distar_EntidadesNegocio.Patente> getAllPatentes()
        {
            List<Distar_EntidadesNegocio.Patente> listaPatentes = new List<Distar_EntidadesNegocio.Patente>();
            string query = "SELECT id_patente, descripcion FROM Patente";
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    Distar_EntidadesNegocio.Patente patente = new Distar_EntidadesNegocio.Patente();
                    patente.id_patente = Convert.ToInt32(dataReader["id_patente"]);
                    patente.descripcion = Convert.ToString(dataReader["descripcion"]);
                    listaPatentes.Add(patente);
                }
                dataReader.Close();
                return listaPatentes;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Patente getAllPatentes: " + ex.Message);
                return listaPatentes;
            }
        }

        public Distar_EntidadesNegocio.Patente obtenerPatentePorId(string id_patente){
            Distar_EntidadesNegocio.Patente patente = new Distar_EntidadesNegocio.Patente();
            string query = "SELECT * FROM Patente WHERE id_patente="+id_patente;
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    patente.id_patente = Convert.ToInt32(dataReader["id_patente"]);
                    patente.descripcion = Convert.ToString(dataReader["descripcion"]);
                }
                dataReader.Close();
                return patente;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Patente obtenerPatentePorId: " + ex.Message);
                return patente;
            }
        }
    }
}
