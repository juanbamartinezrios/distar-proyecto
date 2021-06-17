using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Distar_LogicaNegocio
{
    public class Usuario : Distar_EntidadesNegocio.ABMGenerico<Distar_EntidadesNegocio.Usuario>
    {
        public int calcularDVHPorRegistro(string id_usuario)
        {
            Distar_AccesoDatos.Usuario usuarioDAL = new Distar_AccesoDatos.Usuario();
            Distar_LogicaNegocio.DigitoVerificador DVBL = new Distar_LogicaNegocio.DigitoVerificador();
            string str = usuarioDAL.obtenerStrParaDVHPorUsuario(id_usuario);
            return DVBL.calcularDVH(str);
        }

        public Distar_EntidadesNegocio.Usuario autenticarUsuario(string txtEmail, string txtPw)
        {
            Distar_LogicaNegocio.FamiliaUsuario familiaUsuarioBL = new Distar_LogicaNegocio.FamiliaUsuario();
            Distar_LogicaNegocio.UsuarioPatente usuarioPatenteBL = new Distar_LogicaNegocio.UsuarioPatente();
            Distar_LogicaNegocio.CuentaCorriente cuentaCorrienteBL = new Distar_LogicaNegocio.CuentaCorriente();
            Distar_AccesoDatos.Usuario usuarioDAL = new Distar_AccesoDatos.Usuario();
            Distar_AccesoDatos.Services services = new Distar_AccesoDatos.Services();
            Distar_EntidadesNegocio.Usuario user = new Distar_EntidadesNegocio.Usuario();
            user = usuarioDAL.autenticarUsuario(services.Encriptar3D(txtEmail), services.MD5EncriptPW(txtPw));
            if (user.id_usuario > 0)
            {
                user.email = services.Desencriptar3D(user.email);
                user.domicilio.cp = services.Desencriptar3D(user.domicilio.cp);
                user.domicilio.direccion = services.Desencriptar3D(user.domicilio.direccion);
                user.patentes = usuarioPatenteBL.cargarPatentesDeUsuario(user.id_usuario);
                user.familias = familiaUsuarioBL.cargarFamiliasDeUsuario(user.id_usuario);
                user.cta_cte = cuentaCorrienteBL.cargarCuentaCorrienteDeusuario(user.id_usuario);
                if (user.cta_cte != null){
                    user.cta_cte.nro_cta = services.Desencriptar3D(user.cta_cte.nro_cta);
                    user.cta_cte.saldo_deudor = cuentaCorrienteBL.getSaldoDeudor(user.id_usuario);
                }
            }
            return user;
        }

        public Boolean create(Distar_EntidadesNegocio.Usuario user)
        {
            Distar_LogicaNegocio.FamiliaUsuario familiaUsuarioBL = new Distar_LogicaNegocio.FamiliaUsuario();
            Distar_LogicaNegocio.Telefono telefonoBL = new Distar_LogicaNegocio.Telefono();
            Distar_LogicaNegocio.Domicilio domicilioBL = new Distar_LogicaNegocio.Domicilio();
            Distar_LogicaNegocio.CuentaCorriente cuentaCorrienteBL = new Distar_LogicaNegocio.CuentaCorriente();
            Distar_LogicaNegocio.Usuario usuarioBL = new Distar_LogicaNegocio.Usuario();
            Distar_AccesoDatos.Usuario usuarioDAL = new Distar_AccesoDatos.Usuario();
            Distar_AccesoDatos.Services services = new Distar_AccesoDatos.Services();
            Distar_LogicaNegocio.DigitoVerificador DVBL = new Distar_LogicaNegocio.DigitoVerificador();
            string emailNoEncriptado = user.email;
            string random_pw = getRandomPw();
            user.contraseña = services.MD5EncriptPW(random_pw);
            user.email = services.Encriptar3D(user.email);
            user.DVH = DVBL.calcularDVH(user.nombre + user.email + user.apellido + user.documento);
            if (usuarioDAL.create(user))
            {
                fileNuevoUsuario(emailNoEncriptado, random_pw, user.nombre, user.apellido);
                List<Distar_EntidadesNegocio.FamiliaUsuario> lista_familiaUsuario = new List<Distar_EntidadesNegocio.FamiliaUsuario>();
                int id_usuario = usuarioBL.getIdUsuario(user.email);
                if (user.familias != null)
                {
                    foreach (Distar_EntidadesNegocio.Familia familia in user.familias)
                    {
                        Distar_EntidadesNegocio.FamiliaUsuario aux = new Distar_EntidadesNegocio.FamiliaUsuario();
                        aux.id_familia = familia.id_familia;
                        aux.id_usuario = id_usuario;
                        lista_familiaUsuario.Add(aux);
                    }
                }
                // Crear CTA.CTE a TODOS
                Distar_EntidadesNegocio.CuentaCorriente cta = new Distar_EntidadesNegocio.CuentaCorriente();
                cta.id_cliente = id_usuario;
                cta.saldo_deudor = 0;
                cta.nro_cta = services.Encriptar3D(cuentaCorrienteBL.getRandomCtaNum());
                cta.DVH = DVBL.calcularDVH(cta.id_cliente.ToString() + cta.saldo_deudor.ToString() + cta.nro_cta);
                cuentaCorrienteBL.agregarCtaCteUsuario(cta);
                if (lista_familiaUsuario.Count > 0)
                {
                    familiaUsuarioBL.agregarFamiliaUsuario(lista_familiaUsuario);
                }
                user.telefono.id_usuario = id_usuario;
                string cp = services.Encriptar3D(user.domicilio.cp);
                user.domicilio.cp = cp;
                string direccion = services.Encriptar3D(user.domicilio.direccion);
                user.domicilio.direccion = direccion;
                user.domicilio.id_usuario = id_usuario;
                telefonoBL.agregarTelefonoUsuario(user.telefono);
                domicilioBL.agregarDomicilioUsuario(user.domicilio);
                DVBL.actualizarDVHRegistros("Usuario", id_usuario);
                return true;
            }
            else
            {
                return false;
            }
        }

        private string getRandomPw()
        {
            string passwordAleatorio;
            string s = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random rd = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 8; i++)
			{
			    int idx = rd.Next(0,35);
                sb.Append(s.Substring(idx,1));
			}
            passwordAleatorio = sb.ToString();
            return passwordAleatorio;
        }

        public Boolean update(Distar_EntidadesNegocio.Usuario user)
        {
            Distar_AccesoDatos.Services services = new Distar_AccesoDatos.Services();
            Distar_AccesoDatos.Usuario usuarioDAL = new Distar_AccesoDatos.Usuario();
            Distar_LogicaNegocio.Domicilio domicilioBL = new Distar_LogicaNegocio.Domicilio();
            Distar_LogicaNegocio.Telefono telefonoBL = new Distar_LogicaNegocio.Telefono();
            Distar_LogicaNegocio.DigitoVerificador DVBL = new Distar_LogicaNegocio.DigitoVerificador();
            user.email = services.Encriptar3D(user.email);
            if (usuarioDAL.update(user))
            {
                domicilioBL.update(user.domicilio);
                telefonoBL.update(user.telefono);
                DVBL.actualizarDVHRegistros("Usuario", user.id_usuario);
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean delete(Distar_EntidadesNegocio.Usuario user)
        {
            return false;
        }

        public Boolean deleteLogico(Distar_EntidadesNegocio.Usuario user)
        {
            Distar_AccesoDatos.Usuario usuarioDAL = new Distar_AccesoDatos.Usuario();
            return usuarioDAL.deleteLogico(user);
        }

        public List<Distar_EntidadesNegocio.Usuario> getAllUsers()
        {
            Distar_LogicaNegocio.FamiliaUsuario familiaUsuarioBL = new Distar_LogicaNegocio.FamiliaUsuario();
            Distar_LogicaNegocio.UsuarioPatente usuarioPatenteBL = new Distar_LogicaNegocio.UsuarioPatente();
            Distar_AccesoDatos.Usuario usuarioDAL = new Distar_AccesoDatos.Usuario();
            Distar_AccesoDatos.Services services = new Distar_AccesoDatos.Services();
            List<Distar_EntidadesNegocio.Usuario> lista_usuarios = new List<Distar_EntidadesNegocio.Usuario>();
            foreach (Distar_EntidadesNegocio.Usuario user in usuarioDAL.getAllUsers())
            {
                user.email = services.Desencriptar3D(user.email);
                user.domicilio.cp = services.Desencriptar3D(user.domicilio.cp);
                user.domicilio.direccion = services.Desencriptar3D(user.domicilio.direccion);
                user.patentes = usuarioPatenteBL.cargarPatentesDeUsuario(user.id_usuario);
                user.familias = familiaUsuarioBL.cargarFamiliasDeUsuario(user.id_usuario);
                lista_usuarios.Add(user);
            }
            return lista_usuarios;
        }

        public Boolean cambiarIdiomaUsuario(int id_usuario, int id_idioma)
        {
            Distar_AccesoDatos.Usuario usuarioDAL = new Distar_AccesoDatos.Usuario();
            return usuarioDAL.cambiarIdiomaUsuario(id_usuario, id_idioma);
        }

        public Boolean bloquearUsuario(int id_usuario)
        {
            Distar_AccesoDatos.Usuario usuarioDAL = new Distar_AccesoDatos.Usuario();
            return usuarioDAL.bloquearUsuario(id_usuario);
        }

        public Boolean desbloquearUsuario(int id_usuario)
        {
            Distar_AccesoDatos.Usuario usuarioDAL = new Distar_AccesoDatos.Usuario();
            return usuarioDAL.desbloquearUsuario(id_usuario);
        }

        public Boolean blanquearUsuario(Distar_EntidadesNegocio.Usuario user)
        {
            Distar_AccesoDatos.Usuario usuarioDAL = new Distar_AccesoDatos.Usuario();
            Distar_AccesoDatos.Services services = new Distar_AccesoDatos.Services();
            Distar_LogicaNegocio.DigitoVerificador DVBL = new Distar_LogicaNegocio.DigitoVerificador();
            string randomPw = getRandomPw();
            string emailNoEncriptado = user.email;
            user.contraseña = services.MD5EncriptPW(randomPw);
            user.email = services.Encriptar3D(user.email);
            if (usuarioDAL.blanquearUsuario(user))
            {
                DVBL.actualizarDVHRegistros("Usuario", user.id_usuario);
                fileNuevoUsuario(emailNoEncriptado, randomPw, user.nombre, user.apellido);
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean cambiarContraseña(Distar_EntidadesNegocio.Usuario user, string viejaPw, string nuevaPw){
            Distar_AccesoDatos.Usuario usuarioDAL = new Distar_AccesoDatos.Usuario();
            Distar_AccesoDatos.Services services = new Distar_AccesoDatos.Services();
            return usuarioDAL.cambiarContraseña(services.MD5EncriptPW(nuevaPw), user.id_usuario, services.MD5EncriptPW(viejaPw));
        }

        public int obtenerContIngresosIncorrectos(string txtLog)
        {
            Distar_AccesoDatos.Usuario usuarioDAL = new Distar_AccesoDatos.Usuario();
            Distar_AccesoDatos.Services services = new Distar_AccesoDatos.Services();
            string auxStr = services.Encriptar3D(txtLog);
            return usuarioDAL.obtenerContIngresosIncorrectos(auxStr);
        }

        public void aumentarContIngresosIncorrectos(string txtLog)
        {
            Distar_AccesoDatos.Usuario usuarioDAL = new Distar_AccesoDatos.Usuario();
            Distar_AccesoDatos.Services services = new Distar_AccesoDatos.Services();
            string auxStr = services.Encriptar3D(txtLog);
            if (usuarioDAL.existeEmailUsuario(auxStr))
            {
                usuarioDAL.aumentarContIngresosIncorrectos(auxStr);
            }
        }

        public Boolean validarUsuarioNuevo(string usuarioEmail)
        {
            Distar_AccesoDatos.Usuario usuarioDAL = new Distar_AccesoDatos.Usuario();
            Distar_AccesoDatos.Services services = new Distar_AccesoDatos.Services();
            string auxStr = services.Encriptar3D(usuarioEmail);
            return usuarioDAL.existeEmailUsuario(auxStr);
        }

        public int getIdUsuario(string email)
        {
            Distar_AccesoDatos.Usuario usuarioDAL = new Distar_AccesoDatos.Usuario();
            return usuarioDAL.getIdUsuario(email);
        }

        public Distar_EntidadesNegocio.Usuario obtenerUsuarioPorId(int id_usuario)
        {
            Distar_AccesoDatos.Usuario usuarioDAL = new Distar_AccesoDatos.Usuario();
            Distar_EntidadesNegocio.Usuario usuario = new Distar_EntidadesNegocio.Usuario();
            Distar_AccesoDatos.Services services = new Distar_AccesoDatos.Services();
            Distar_LogicaNegocio.CuentaCorriente cuentaCorrienteBL = new Distar_LogicaNegocio.CuentaCorriente();
            Distar_LogicaNegocio.FamiliaUsuario familiaUsuarioBL = new Distar_LogicaNegocio.FamiliaUsuario();
            Distar_LogicaNegocio.UsuarioPatente usuarioPatenteBL = new Distar_LogicaNegocio.UsuarioPatente();
            Distar_LogicaNegocio.Domicilio domicilioBL = new Distar_LogicaNegocio.Domicilio();
            Distar_LogicaNegocio.Telefono telefonoBL = new Distar_LogicaNegocio.Telefono();
            usuario = usuarioDAL.obtenerUsuarioPorId(id_usuario);
            if (usuario.id_usuario != 0)
            {
                usuario.email = services.Desencriptar3D(usuario.email);
                usuario.domicilio = domicilioBL.obtenerDomicilioPorId(usuario.domicilio.id_domicilio);
                if (usuario.domicilio != null)
                {
                    usuario.domicilio.cp = services.Desencriptar3D(usuario.domicilio.cp);
                    usuario.domicilio.direccion = services.Desencriptar3D(usuario.domicilio.direccion);
                }
                usuario.telefono = telefonoBL.obtenerTelefonoPorId(usuario.telefono.id_telefono);
                usuario.patentes = usuarioPatenteBL.cargarPatentesDeUsuario(usuario.id_usuario);
                usuario.familias = familiaUsuarioBL.cargarFamiliasDeUsuario(usuario.id_usuario);
                usuario.cta_cte = cuentaCorrienteBL.cargarCuentaCorrienteDeusuario(usuario.id_usuario);
            }
            return usuario;
        }

        public Distar_EntidadesNegocio.Usuario obtenerUsuarioPorEmail(string email)
        {
            Distar_AccesoDatos.Usuario usuarioDAL = new Distar_AccesoDatos.Usuario();
            Distar_EntidadesNegocio.Usuario usuario = new Distar_EntidadesNegocio.Usuario();
            Distar_AccesoDatos.Services services = new Distar_AccesoDatos.Services();
            Distar_LogicaNegocio.CuentaCorriente cuentaCorrienteBL = new Distar_LogicaNegocio.CuentaCorriente();
            Distar_LogicaNegocio.FamiliaUsuario familiaUsuarioBL = new Distar_LogicaNegocio.FamiliaUsuario();
            Distar_LogicaNegocio.UsuarioPatente usuarioPatenteBL = new Distar_LogicaNegocio.UsuarioPatente();
            Distar_LogicaNegocio.Domicilio domicilioBL = new Distar_LogicaNegocio.Domicilio();
            Distar_LogicaNegocio.Telefono telefonoBL = new Distar_LogicaNegocio.Telefono();
            if (usuarioDAL.existeEmailUsuario(services.Encriptar3D(email)))
            {
                usuario = usuarioDAL.obtenerUsuarioPorEmail(services.Encriptar3D(email));
                if (usuario.id_usuario != 0)
                {
                    usuario.email = services.Desencriptar3D(usuario.email);
                    usuario.domicilio = domicilioBL.obtenerDomicilioPorId(usuario.domicilio.id_domicilio);
                    if (usuario.domicilio != null)
                    {
                        usuario.domicilio.cp = services.Desencriptar3D(usuario.domicilio.cp);
                        usuario.domicilio.direccion = services.Desencriptar3D(usuario.domicilio.direccion);
                    }
                    usuario.telefono = telefonoBL.obtenerTelefonoPorId(usuario.telefono.id_telefono);
                    usuario.patentes = usuarioPatenteBL.cargarPatentesDeUsuario(usuario.id_usuario);
                    usuario.familias = familiaUsuarioBL.cargarFamiliasDeUsuario(usuario.id_usuario);
                    usuario.cta_cte = cuentaCorrienteBL.cargarCuentaCorrienteDeusuario(usuario.id_usuario);
                }
            }
            return usuario;
        }

        public string obtenerEmailUsuarioPorId(int id_usuario)
        {
            Distar_AccesoDatos.Usuario usuarioDAL = new Distar_AccesoDatos.Usuario();
            Distar_EntidadesNegocio.Usuario usuario = new Distar_EntidadesNegocio.Usuario();
            Distar_AccesoDatos.Services services = new Distar_AccesoDatos.Services();
            usuario = usuarioDAL.obtenerUsuarioPorId(id_usuario);
            if (usuario.id_usuario != 0)
            {
                usuario.email = services.Desencriptar3D(usuario.email);
           
            }
            return usuario.email;
        }

        public void fileNuevoUsuario(string email, string pw, string nombre, string apellido)
        {
            try
            {
                string filePath = String.Format("C:\\Distar\\datos - {0} {1}.txt", nombre, apellido);
                Boolean filePathExists = File.Exists(filePath);
                StreamWriter sw = new StreamWriter(File.Open(filePath, FileMode.OpenOrCreate));
                sw.WriteLine("Usuario: {0} - Contraseña: {1}", email, pw);
                sw.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("UsuarioBL fileNuevoUsuario ERROR. - "+ex.Message.ToString());
            }
        }

    }
}
