using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Distar_AccesoDatos
{
    public class FamiliaPatente
    {
        public Boolean agregarPatenteAFamilia(List<Distar_EntidadesNegocio.FamiliaPatente> lista_add){
            Boolean flag = true;
            foreach (Distar_EntidadesNegocio.FamiliaPatente familiaPatente in lista_add)
	        {
                string query = "INSERT INTO FamiliaPatente VALUES (" + familiaPatente.DVH + ", " + familiaPatente.id_familia + ", " + familiaPatente.id_patente + ")";
                if (Distar_BD.ExecuteNonquery(query) == -1)
                {
                    flag = false;
                    break;
                }
	        }
            return flag;
        }

        public Boolean eliminarReferenciaFP(int id_familia)
        {
            string query = "DELETE FROM FamiliaPatente WHERE id_familia=" + id_familia;
            try
            {
                return Distar_BD.ExecuteNonquery(query) > -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL FamiliaPatente eliminarReferenciaFP: " + ex.Message);
                return false;
            }
        }

        public Boolean quitarPatenteAFamilia(List<Distar_EntidadesNegocio.FamiliaPatente> lista_delete)
        {
            Boolean flag = true;
            foreach (Distar_EntidadesNegocio.FamiliaPatente familiaPatente in lista_delete)
            {
                string query = "DELETE FROM FamiliaPatente WHERE id_familia=" + familiaPatente.id_familia + " AND id_patente=" + familiaPatente.id_patente;
                if (Distar_BD.ExecuteNonquery(query) == -1)
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }

        public List<int> obtenerIdPatentePorFamilia(int id_familia){
            string query = "SELECT id_patente FROM FamiliaPatente WHERE id_familia=" + id_familia;
            List<int> listaId = new List<int>();
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                if (dataReader != null && dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        listaId.Add(Convert.ToInt32(dataReader["id_patente"]));
                    }
                }
                dataReader.Close();
                return listaId;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL FamiliaPatente obtenerIdPatentePorFamilia: " + ex.Message);
                return listaId;
            }
        }

        public List<int> obtenerIdPatenteQueNoSeUtilizanEnOtrasFamilias(string list_id_familias, string list_id_patentes)
        {
            string query = "SELECT * FROM FamiliaPatente WHERE id_patente IN (" + list_id_patentes + ") AND id_familia NOT IN (" + list_id_familias + ")";
            List<int> lista_id_return = new List<int>();
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    lista_id_return.Add(Convert.ToInt32(dataReader["id_patente"]));
                }
                dataReader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL FamiliaPatente obtenerIdPatenteQueNoSeUtilizanEnOtrasFamilias: " + ex.Message);
            }
            return lista_id_return;
        }

        public int obtenerCountPatenteEnOtrasFamilias(int id_patente, int id_familia)
        {
            string query = "SELECT COUNT(*) FROM FamiliaPatente WHERE id_patente=" + id_patente;
            if (id_familia != 0)
            {
                query += " AND id_familia NOT IN (" + id_familia + ")";
            }
            try
            {
                return Distar_BD.ExecuteScalar(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL FamiliaPatente obtenerIdPatenteQueNoSeUtilizanEnOtrasFamilias: " + ex.Message);
                return 0;
            }
        }

        public List<Distar_EntidadesNegocio.Familia> obtenerFamiliasConPatente(int id_patente, int id_familia)
        {
            Distar_AccesoDatos.Familia familiaDAL = new Distar_AccesoDatos.Familia();
            List<Distar_EntidadesNegocio.Familia> lista_familias = new List<Distar_EntidadesNegocio.Familia>();
            string query = "SELECT id_familia FROM FamiliaPatente WHERE id_patente=" + id_patente;
            if (id_familia != 0)
            {
                query += " AND id_familia NOT IN (" + id_familia + ")";
            }
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    lista_familias.Add(familiaDAL.getFamiliaById(Convert.ToInt32(dataReader["id_familia"])));
                }
                dataReader.Close();
                return lista_familias;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL FamiliaPatente obtenerFamiliasConPatente: " + ex.Message);
                return lista_familias;
            }
        }

        public List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO> obtenerDTO_DV()
        {
            string query = "SELECT id_familia, id_patente, DVH FROM FamiliaPatente";
            List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO> lista_dv_dto = new List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO>();
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO dto = new Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO();
                    dto.entidad = "FamiliaPatente";
                    dto.valor_db = Convert.ToInt32(dataReader["DVH"]);
                    dto.id_registro = Convert.ToInt32(Convert.ToInt32(dataReader["id_familia"]).ToString() + Convert.ToInt32(dataReader["id_patente"]).ToString());
                    dto.txtstr = Convert.ToString(dataReader["id_familia"]) + Convert.ToString(dataReader["id_patente"]);
                    lista_dv_dto.Add(dto);
                }
                dataReader.Close();
                return lista_dv_dto;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL FamiliaPatente obtenerDTO_DV: " + ex.Message);
                return lista_dv_dto;
            }
        }
    }
}
