using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Distar_AccesoDatos
{
    public class Usuario : Distar_EntidadesNegocio.ABMGenerico<Distar_EntidadesNegocio.Usuario>
    {
        public string obtenerStrParaDVHPorUsuario(string id_usuario){
            string query = "SELECT email, activo, documento, nombre, apellido, id_idioma_usuario FROM Usuario WHERE id_usuario="+id_usuario;
            string str = null;
            SqlDataReader dataReader;
            dataReader = Distar_BD.ExecuteReader(query);
            if (dataReader != null && dataReader.HasRows)
            {
                dataReader.Read();
                str = dataReader["nombre"].ToString() + dataReader["apellido"].ToString() + dataReader["email"].ToString() + dataReader["documento"].ToString();
                dataReader.Close();
            }
            return str;
        }

        public Boolean actualizarDVH(string valorActualizado, int id_registro){
            string query = "UPDATE Usuario SET DVH="+valorActualizado+ " WHERE id_usuario="+id_registro.ToString();
            try
            {
                return Distar_BD.ExecuteNonquery(query) > -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Usuario actualizarDVH: " + ex.Message);
                return false;
            }
        }

        public int getIdUsuario(string email)
        {
            string query = "SELECT id_usuario FROM Usuario WHERE email='" + email + "'";
            Distar_EntidadesNegocio.Usuario user = new Distar_EntidadesNegocio.Usuario();
            SqlDataReader dataReader;
            dataReader = Distar_BD.ExecuteReader(query);
            while (dataReader.Read())
            {
                user.id_usuario = Convert.ToInt32(dataReader["id_usuario"]);
            }
            dataReader.Close();
            return user.id_usuario;
        }

        public Boolean existeEmailUsuario(string txtLog)
        {
            string query = "SELECT * FROM Usuario WHERE email='" + txtLog + "'";
            SqlDataReader dataReader;
            dataReader = Distar_BD.ExecuteReader(query);
            try
            {
                if (dataReader != null && dataReader.HasRows)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                dataReader.Close();
            }
        }

        public void aumentarContIngresosIncorrectos(string email){
            string query = "UPDATE Usuario SET cont_ingresos_incorrectos=cont_ingresos_incorrectos+1 WHERE email='" + email + "'";
            try
            {
                Distar_BD.ExecuteNonquery(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Usuario actualizarDVH: " + ex.Message);
            }
        }

        public int obtenerContIngresosIncorrectos(string email)
        {
            int cont_ingresos_incorrectos = 0;
            string query = "SELECT cont_ingresos_incorrectos FROM Usuario WHERE email='" + email + "'";
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                if (dataReader != null && dataReader.HasRows)
                {
                    dataReader.Read();
                    cont_ingresos_incorrectos = Convert.ToInt32(dataReader["cont_ingresos_incorrectos"]);
                    dataReader.Close();
                }
                return cont_ingresos_incorrectos;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Usuario autenticarUsuario: " + ex.Message);
                return -1;
            }
        }

        public Distar_EntidadesNegocio.Usuario autenticarUsuario(string txtEmail, string txtPw){
            string query = "SELECT * FROM Usuario WHERE email='"+txtEmail+"' AND contraseña='"+txtPw+"'";
            Distar_EntidadesNegocio.Usuario user = new Distar_EntidadesNegocio.Usuario();
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                if (dataReader != null && dataReader.HasRows)
                {
                    dataReader.Read();
                    user.id_usuario = Convert.ToInt32(dataReader["id_usuario"]);
                    user.nombre = Convert.ToString(dataReader["nombre"]);
                    user.apellido = Convert.ToString(dataReader["apellido"]);
                    user.documento = Convert.ToString(dataReader["documento"]);
                    user.activo = Convert.ToBoolean(dataReader["activo"].ToString());
                    user.cont_ingresos_incorrectos = Convert.ToInt32(dataReader["cont_ingresos_incorrectos"]);
                    user.id_idioma_usuario = Convert.ToInt32(dataReader["id_idioma_usuario"]);
                    user.email = Convert.ToString(dataReader["email"]);
                    user.familias = getFamiliasUsuario(user.id_usuario);
                    user.domicilio = getDomicilioUsuario(user.id_usuario);
                    user.telefono = getTelefonoUsuario(user.id_usuario);
                    dataReader.Close();
                }
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Usuario autenticarUsuario: " + ex.Message);
                return user;
            }
        }

        public Boolean create(Distar_EntidadesNegocio.Usuario user)
        {
            string query = "INSERT INTO Usuario VALUES (" +
                           "'1'," +
                           "'" + user.apellido + "'," +
                           "'0'," +
                           "'" + user.contraseña + "'," +
                           "'" + user.documento + "'," +
                           "'" + user.DVH + "'," +
                           "'" + user.email + "'," +
                           "'1'," +
                           "'" + user.nombre + "')";
            try
            {
                return Distar_BD.ExecuteNonquery(query) > -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Usuario create: " + ex.Message);
                return false;
            }
        }

        public Boolean update(Distar_EntidadesNegocio.Usuario user)
        {
            string query = "UPDATE Usuario SET ";
            if (!string.IsNullOrEmpty(user.apellido))
                query = query + " apellido='" + user.apellido + "',";
            if (!string.IsNullOrEmpty(user.nombre))
                query = query + " nombre='" + user.nombre + "',";
            query = query + " email='" + user.email + "',";
            if (!string.IsNullOrEmpty(user.documento))
                query = query + " documento='" + user.documento + "',";
            query = query.Substring(0, query.Length - 1) + " WHERE id_usuario=" + user.id_usuario;
            try
            {
                return Distar_BD.ExecuteNonquery(query) > -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Usuario update: " + ex.Message);
                return false;
            }
        }

        public Boolean delete(Distar_EntidadesNegocio.Usuario user)
        {
            return false;
        }

        public Boolean deleteLogico(Distar_EntidadesNegocio.Usuario user)
        {
            string query = "UPDATE Usuario SET activo=0 WHERE id_usuario=" + user.id_usuario;
            try
            {
                return Distar_BD.ExecuteNonquery(query) > -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Usuario deleteLogico: " + ex.Message);
                return false;
            }
        }

        public Boolean blanquearUsuario(Distar_EntidadesNegocio.Usuario user)
        {
            string query = "UPDATE Usuario SET contraseña='"+user.contraseña+"', DVH="+user.DVH+" WHERE id_usuario=" + user.id_usuario;
            try
            {
                return Distar_BD.ExecuteNonquery(query) > -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Usuario blanquearUsuario: " + ex.Message);
                return false;
            }
        }

        public List<Distar_EntidadesNegocio.Usuario> getAllUsers()
        {
            List<Distar_EntidadesNegocio.Usuario> listaUsuarios = new List<Distar_EntidadesNegocio.Usuario>();
            string query = "SELECT nombre, apellido, documento, id_usuario, email, activo, cont_ingresos_incorrectos, id_idioma_usuario FROM Usuario ORDER BY apellido ASC";
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    Distar_EntidadesNegocio.Usuario user = new Distar_EntidadesNegocio.Usuario();
                    user.id_usuario = Convert.ToInt32(dataReader["id_usuario"]);
                    user.nombre = Convert.ToString(dataReader["nombre"]);
                    user.apellido = Convert.ToString(dataReader["apellido"]);
                    user.documento = Convert.ToString(dataReader["documento"]);
                    user.activo = Convert.ToBoolean(dataReader["activo"].ToString());
                    user.cont_ingresos_incorrectos = Convert.ToInt32(dataReader["cont_ingresos_incorrectos"]);
                    user.email = Convert.ToString(dataReader["email"]);
                    user.id_idioma_usuario = Convert.ToInt32(dataReader["id_idioma_usuario"]);
                    user.familias = getFamiliasUsuario(user.id_usuario);
                    user.domicilio = getDomicilioUsuario(user.id_usuario);
                    user.telefono = getTelefonoUsuario(user.id_usuario);
                    listaUsuarios.Add(user);
                }
                dataReader.Close();
                return listaUsuarios;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Usuario getAllUsers: " + ex.Message);
                return listaUsuarios;
            }
        }

        public List<Distar_EntidadesNegocio.Familia> getFamiliasUsuario(int id_usuario)
        {
            Distar_AccesoDatos.Familia familiaDAL = new Distar_AccesoDatos.Familia();
            return familiaDAL.getFamiliasByIdUsuario(id_usuario);
        }

        public Distar_EntidadesNegocio.Domicilio getDomicilioUsuario(int id_usuario)
        {
            Distar_AccesoDatos.Domicilio domicilioDAL = new Distar_AccesoDatos.Domicilio();
            return domicilioDAL.getDomicilioByIdUsuario(id_usuario);
        }

        public Distar_EntidadesNegocio.Telefono getTelefonoUsuario(int id_usuario)
        {
            Distar_AccesoDatos.Telefono telefonoDAL = new Distar_AccesoDatos.Telefono();
            return telefonoDAL.getTelefonoByIdUsuario(id_usuario);
        }

        public Distar_EntidadesNegocio.Idioma getIdiomaUsuario(int id_idioma)
        {
            Distar_AccesoDatos.Idioma idiomaDAL = new Distar_AccesoDatos.Idioma();
            return idiomaDAL.getIdiomaById(id_idioma);
        }

        public Boolean cambiarIdiomaUsuario(int id_usuario, int id_idioma)
        {
            string query = "UPDATE Usuario SET id_idioma_usuario="+id_idioma+" WHERE id_usuario=" + id_usuario;
            try
            {
                return Distar_BD.ExecuteNonquery(query) > -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Usuario cambiarIdiomaUsuario: " + ex.Message);
                return false;
            }
        }

        public Boolean bloquearUsuario(int id_usuario)
        {
            string query = "UPDATE Usuario SET cont_ingresos_incorrectos=3 WHERE id_usuario="+id_usuario.ToString();
            try
            {
                return Distar_BD.ExecuteNonquery(query) > -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Usuario bloquearUsuario: " + ex.Message);
                return false;
            }
        }

        public Boolean desbloquearUsuario(int id_usuario)
        {
            string query = "UPDATE Usuario SET cont_ingresos_incorrectos=0 WHERE id_usuario="+id_usuario.ToString();
            try
            {
                return Distar_BD.ExecuteNonquery(query) > -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Usuario desbloquearUsuario: " + ex.Message);
                return false;
            }
        }

        public Boolean cambiarContraseña(string encriptedPw, int id_usuario, string encriptedPwOld)
        {
            string query = "UPDATE Usuario SET contraseña='"+encriptedPw+"' WHERE id_usuario="+id_usuario;
            string queryValidate = "SELECT * FROM Usuario WHERE contraseña='"+encriptedPwOld+"'";
            SqlDataReader dataReader;
            dataReader = Distar_BD.ExecuteReader(queryValidate);
            if (dataReader != null && dataReader.HasRows)
            {
                if (Distar_BD.ExecuteNonquery(query) > -1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public Distar_EntidadesNegocio.Usuario obtenerUsuarioPorEmail(string email)
        {
            Distar_EntidadesNegocio.Usuario usuario = new Distar_EntidadesNegocio.Usuario();
            string query = "SELECT * FROM Usuario WHERE email='" + email + "'";
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    usuario.id_usuario = Convert.ToInt32(dataReader["id_usuario"]);
                    usuario.nombre = Convert.ToString(dataReader["nombre"]);
                    usuario.apellido = Convert.ToString(dataReader["apellido"]);
                    usuario.documento = Convert.ToString(dataReader["documento"]);
                    usuario.activo = Convert.ToBoolean(dataReader["activo"].ToString());
                    usuario.cont_ingresos_incorrectos = Convert.ToInt32(dataReader["cont_ingresos_incorrectos"]);
                    usuario.email = Convert.ToString(dataReader["email"]);
                    usuario.id_idioma_usuario = Convert.ToInt32(dataReader["id_idioma_usuario"]);
                    usuario.familias = getFamiliasUsuario(usuario.id_usuario);
                    usuario.domicilio = getDomicilioUsuario(usuario.id_usuario);
                    usuario.telefono = getTelefonoUsuario(usuario.id_usuario);
                }
                dataReader.Close();
                return usuario;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Usuario obtenerUsuarioPorEmail: " + ex.Message);
                return usuario;
            }
        }

        public Distar_EntidadesNegocio.Usuario obtenerUsuarioPorId(int id_usuario)
        {
            Distar_EntidadesNegocio.Usuario usuario = new Distar_EntidadesNegocio.Usuario();
            string query = "SELECT * FROM Usuario WHERE id_usuario="+id_usuario;
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    usuario.id_usuario = Convert.ToInt32(dataReader["id_usuario"]);
                    usuario.nombre = Convert.ToString(dataReader["nombre"]);
                    usuario.apellido = Convert.ToString(dataReader["apellido"]);
                    usuario.documento = Convert.ToString(dataReader["documento"]);
                    usuario.activo = Convert.ToBoolean(dataReader["activo"].ToString());
                    usuario.cont_ingresos_incorrectos = Convert.ToInt32(dataReader["cont_ingresos_incorrectos"]);
                    usuario.email = Convert.ToString(dataReader["email"]);
                    usuario.id_idioma_usuario = Convert.ToInt32(dataReader["id_idioma_usuario"]);
                    usuario.familias = getFamiliasUsuario(usuario.id_usuario);
                    usuario.domicilio = getDomicilioUsuario(usuario.id_usuario);
                    usuario.telefono = getTelefonoUsuario(usuario.id_usuario);
                }
                dataReader.Close();
                return usuario;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Usuario obtenerUsuarioPorId: " + ex.Message);
                return usuario;
            }
        }

        public List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO> obtenerDTO_DV()
        {
            string query = "SELECT id_usuario, nombre, apellido, documento, email, DVH FROM Usuario";
            List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO> lista_dv_dto = new List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO>();
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO dto = new Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO();
                    dto.entidad = "Usuario";
                    dto.valor_db = Convert.ToInt32(dataReader["DVH"]);
                    dto.id_registro = Convert.ToInt32(dataReader["id_usuario"]);
                    dto.txtstr = Convert.ToString(dataReader["nombre"]) + Convert.ToString(dataReader["apellido"]) + Convert.ToString(dataReader["email"]) + Convert.ToString(dataReader["documento"]);
                    lista_dv_dto.Add(dto);
                }
                dataReader.Close();
                return lista_dv_dto;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Usuario obtenerDTO_DV: " + ex.Message);
                return lista_dv_dto;
            }
        }
    }
}