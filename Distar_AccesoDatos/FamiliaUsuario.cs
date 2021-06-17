using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Distar_AccesoDatos
{
    public class FamiliaUsuario
    {
        public List<int> obtenerIDsFamiliaUsuario(int id_usuario)
        {
            List<int> lista_id_familia = new List<int>();
            string query = "SELECT id_familia FROM FamiliaUsuario WHERE id_usuario=" + id_usuario;
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    lista_id_familia.Add(Convert.ToInt32(dataReader["id_familia"]));
                }
                dataReader.Close();
                return lista_id_familia;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL FamiliaUsuario obtenerIDsFamiliaUsuario: " + ex.Message);
                return lista_id_familia;
            }
        }

        public Boolean agregarFamiliaUsuario(List<Distar_EntidadesNegocio.FamiliaUsuario> lista_add)
        {
            Boolean flag = true;
            foreach (Distar_EntidadesNegocio.FamiliaUsuario familiaUsuario in lista_add)
            {
                string query = "INSERT INTO FamiliaUsuario VALUES (" + familiaUsuario.id_familia + ", " + familiaUsuario.id_usuario + ")";
                if (Distar_BD.ExecuteNonquery(query) == -1)
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }

        public Boolean quitarFamiliaUsuario(List<Distar_EntidadesNegocio.FamiliaUsuario> lista_delete)
        {
            Boolean flag = true;
            foreach (Distar_EntidadesNegocio.FamiliaUsuario familiaUsuario in lista_delete)
            {
                string query = "DELETE FROM FamiliaUsuario WHERE id_familia=" + familiaUsuario.id_familia + " AND id_usuario=" + familiaUsuario.id_usuario;
                if (Distar_BD.ExecuteNonquery(query) == -1)
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }

        public Boolean existeUsuarioActivoEnFamilia(string id_familias_concat, int id_patente = 0)
        {
            string query = "";
            if (id_patente == 0)
            {
                query = "SELECT * FROM FamiliaUsuario WHERE id_familia IN (" + id_familias_concat + ") AND id_usuario IN (SELECT id_usuario FROM Usuario WHERE (activo=1 AND cont_ingresos_incorrectos < 3))";
            }
            else
            {
                query = "SELECT COUNT (*) FROM FamiliaUsuario FU LEFT JOIN UsuarioPatente UP ON UP.id_patente=" + id_patente + " AND negado=0 AND FU.id_usuario=UP.id_usuario WHERE FU.id_familia=(" + id_familias_concat + ") AND FU.id_usuario IN (SELECT id_usuario FROM Usuario WHERE (activo=1 AND cont_ingresos_incorrectos < 3))";
            }
            try
            {
                return Distar_BD.ExecuteScalar(query) > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL FamiliaUsuario existeUsuarioActivoEnFamilia: " + ex.Message);
                return false;
            }
        }

        public Boolean existeOtroUsuarioActivoEnFamilia(int id_familia, int id_usuario, int id_patente = 0)
        {
            string query = "";
            if (id_patente == 0)
            {
                query = "SELECT COUNT(*) FROM FamiliaUsuario WHERE id_familia IN (" + id_familia + ") AND id_usuario NOT IN (" + id_usuario + ") AND id_usuario IN (SELECT id_usuario FROM Usuario WHERE (activo=1 AND cont_ingresos_incorrectos < 3))";
            }
            else
            {
                query = "SELECT COUNT(*) FROM UsuarioPatente UP WHERE UP.id_usuario IN (SELECT UP.id_usuario FROM FamiliaUsuario FU WHERE id_familia IN ("+id_familia+") AND id_usuario NOT IN ("+id_usuario+") AND id_usuario IN (SELECT id_usuario FROM Usuario WHERE (activo=1 AND cont_ingresos_incorrectos < 3))) AND UP.id_patente="+id_patente+" AND UP.negado=0";
            }
            try
            {
                return Distar_BD.ExecuteScalar(query) > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL FamiliaUsuario existeOtroUsuarioActivoEnFamilia: " + ex.Message);
                return false;
            }
        }

        public List<Distar_EntidadesNegocio.Usuario> obtenerUsuariosDeFamilia(int id_familia, int id_usuario = 0)
        {
            List<Distar_EntidadesNegocio.Usuario> lista_usuarios = new List<Distar_EntidadesNegocio.Usuario>();
            string query = "SELECT id_usuario FROM FamiliaUsuario FU WHERE FU.id_familia=" + id_familia;
            if (id_familia != 0)
            {
                query += " AND id_usuario NOT IN (" + id_usuario + ")";
            }
            query += " AND id_usuario IN (SELECT id_usuario FROM Usuario WHERE activo=1)";
            Distar_AccesoDatos.Usuario usuarioDAL = new Distar_AccesoDatos.Usuario();
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    Distar_EntidadesNegocio.Usuario user;
                    user = usuarioDAL.obtenerUsuarioPorId(Convert.ToInt32(dataReader["id_usuario"]));
                    if (user.id_usuario != 0)
                    {
                        lista_usuarios.Add(user);
                    }
                }
                dataReader.Close();
                return lista_usuarios;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL FamiliaUsuario obtenerUsuariosDeFamilia: " + ex.Message);
                return lista_usuarios;
            }
        }
    }
}
