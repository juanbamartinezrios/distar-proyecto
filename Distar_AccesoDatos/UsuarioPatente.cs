using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Distar_AccesoDatos
{
    public class UsuarioPatente
    {
        public List<Distar_EntidadesNegocio.UsuarioPatente> obtenerUsuarioPatentePorUsuario(int id_usuario)
        {
            List<Distar_EntidadesNegocio.UsuarioPatente> lista_usuarioPatente = new List<Distar_EntidadesNegocio.UsuarioPatente>();
            string query = "SELECT id_patente, negado FROM UsuarioPatente WHERE id_usuario=" + id_usuario;
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    Distar_EntidadesNegocio.UsuarioPatente usuarioPatente = new Distar_EntidadesNegocio.UsuarioPatente();
                    usuarioPatente.id_usuario = id_usuario;
                    usuarioPatente.negado = Convert.ToInt32(dataReader["negado"]);
                    usuarioPatente.id_patente = Convert.ToInt32(dataReader["id_patente"]);
                    lista_usuarioPatente.Add(usuarioPatente);
                }
                dataReader.Close();
                return lista_usuarioPatente;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL UsuarioPatente obtenerUsuarioPatentePorUsuario: " + ex.Message);
                return lista_usuarioPatente;
            }
        }

        public Boolean agregarPatenteUsuario(List<Distar_EntidadesNegocio.UsuarioPatente> lista_add)
        {
            Boolean flag = true;
            foreach (Distar_EntidadesNegocio.UsuarioPatente patenteUsuario in lista_add)
            {
                string query = "INSERT INTO UsuarioPatente VALUES (" + patenteUsuario.DVH + ", " + patenteUsuario.id_usuario + ", " + patenteUsuario.negado + ", " + patenteUsuario.id_patente +")";
                if (Distar_BD.ExecuteNonquery(query) == -1)
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }

        public Boolean quitarPatenteUsuario(List<Distar_EntidadesNegocio.UsuarioPatente> lista_delete)
        {
            Boolean flag = true;
            foreach (Distar_EntidadesNegocio.UsuarioPatente patenteUsuario in lista_delete)
            {
                string query = "DELETE FROM UsuarioPatente WHERE id_patente=" + patenteUsuario.id_patente + " AND id_usuario=" + patenteUsuario.id_usuario;
                if (Distar_BD.ExecuteNonquery(query) == -1)
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }

        public Boolean negarPatenteUsuario(List<Distar_EntidadesNegocio.UsuarioPatente> lista_disable)
        {
            Boolean flag = true;
            foreach (Distar_EntidadesNegocio.UsuarioPatente patenteUsuario in lista_disable)
            {
                string query = "UPDATE UsuarioPatente SET negado=1, DVH=" + patenteUsuario.DVH + " WHERE id_patente=" + patenteUsuario.id_patente + " AND id_usuario=" + patenteUsuario.id_usuario;
                if (Distar_BD.ExecuteNonquery(query) == -1)
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }

        public Boolean habilitarPatenteUsuario(List<Distar_EntidadesNegocio.UsuarioPatente> lista_enable)
        {
            Boolean flag = true;
            foreach (Distar_EntidadesNegocio.UsuarioPatente patenteUsuario in lista_enable)
            {
                string query = "UPDATE UsuarioPatente SET negado=0, DVH=" + patenteUsuario.DVH + " WHERE id_patente=" + patenteUsuario.id_patente + " AND id_usuario=" + patenteUsuario.id_usuario;
                if (Distar_BD.ExecuteNonquery(query) == -1)
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }

        public int verificarUsoPatente(int id_patente, int id_usuario=0)
        {
            int count;
            string query = "SELECT COUNT(*) FROM UsuarioPatente WHERE id_patente=" + id_patente + " AND id_usuario NOT IN (SELECT id_usuario FROM Usuario WHERE (activo=0 OR cont_ingresos_incorrectos >= 3)) AND negado=0";
            if (id_usuario != 0)
            {
                query += " AND id_usuario NOT IN (" + id_usuario + ")";
            }
            try
            {
                count = Distar_BD.ExecuteScalar(query);
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL UsuarioPatente verificarUsoPatente: " + ex.Message);
                return 0;
            }
        }

        public int usuarioTienePatenteNegada(int id_patente, int id_usuario)
        {
            int count;
            string query = "SELECT COUNT(*) FROM UsuarioPatente WHERE id_patente=" + id_patente + " AND id_usuario=" + id_usuario+ " AND negado=1";
            try
            {
                count = Distar_BD.ExecuteScalar(query);
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL UsuarioPatente usuarioTienePatenteNegada: " + ex.Message); 
                return 0;
            }
        }

        public Boolean isPatenteAsignada(Distar_EntidadesNegocio.UsuarioPatente usuarioPatente)
        {
            string query = "SELECT * FROM UsuarioPatente WHERE id_patente=" + usuarioPatente.id_patente + " AND id_usuario=" + usuarioPatente.id_usuario;
            SqlDataReader dataReader;
            dataReader = Distar_BD.ExecuteReader(query);
            if (dataReader.Read())
            {
                dataReader.Close();
                return true;
            }
            else
            {
                dataReader.Close();
                return false;
            }
        }

        public Boolean isPatenteNegada(Distar_EntidadesNegocio.UsuarioPatente usuarioPatente)
        {
            string query = "SELECT * FROM UsuarioPatente WHERE id_patente=" + usuarioPatente.id_patente + " AND id_usuario=" + usuarioPatente.id_usuario + " AND negado=1";
            SqlDataReader dataReader;
            dataReader = Distar_BD.ExecuteReader(query);
            if (dataReader.Read())
            {
                dataReader.Close();
                return true;
            }
            else
            {
                dataReader.Close();
                return false;
            }
        }

        public List<int> obtenerIDsPatentesQueUsanUsuarios(string id_patentes_concat)
        {
            string query = "SELECT * FROM UsuarioPatente WHERE id_patente IN (" + id_patentes_concat + ") AND id_usuario NOT IN (SELECT id_usuario FROM Usuario WHERE (activo=0 OR cont_ingresos_incorrectos >= 3))";
            List<int> result = new List<int>();
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    result.Add(Convert.ToInt32(dataReader["id_patente"]));
                }
                dataReader.Close();
            }

            catch (Exception ex)
            {
                Console.WriteLine("DAL UsuarioPatente obtenerIDsPatentesQueUsanUsuarios: " + ex.Message);
            }
            return result;
        }


        public List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO> obtenerDTO_DV()
        {
            string query = "SELECT id_patente, id_usuario, negado, DVH FROM UsuarioPatente";
            List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO> lista_dv_dto = new List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO>();
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO dto = new Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO();
                    dto.entidad = "UsuarioPatente";
                    dto.valor_db = Convert.ToInt32(dataReader["DVH"]);
                    dto.id_registro = Convert.ToInt32(Convert.ToInt32(dataReader["id_usuario"]).ToString() + Convert.ToInt32(dataReader["id_patente"]).ToString());
                    dto.txtstr = Convert.ToString(dataReader["id_usuario"]) + Convert.ToString(dataReader["id_patente"]) + Convert.ToInt32(dataReader["negado"]).ToString();
                    lista_dv_dto.Add(dto);
                }
                dataReader.Close();
                return lista_dv_dto;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL UsuarioPatente obtenerDTO_DV: " + ex.Message);
                return lista_dv_dto;
            }
        }
    }
}
