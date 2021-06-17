using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distar_LogicaNegocio
{
    public class UsuarioPatente
    {
        public List<Distar_EntidadesNegocio.Patente> cargarPatentesDeUsuario(int id_usuario)
        {
            Distar_AccesoDatos.UsuarioPatente usuarioPatenteDAL = new Distar_AccesoDatos.UsuarioPatente();
            Distar_AccesoDatos.Patente patenteDAL = new Distar_AccesoDatos.Patente();
            List<Distar_EntidadesNegocio.UsuarioPatente> lista_patenteUSuario = new List<Distar_EntidadesNegocio.UsuarioPatente>();
            List<Distar_EntidadesNegocio.Patente> lista_patentes = new List<Distar_EntidadesNegocio.Patente>();

            lista_patenteUSuario = usuarioPatenteDAL.obtenerUsuarioPatentePorUsuario(id_usuario);

            foreach (Distar_EntidadesNegocio.UsuarioPatente usuarioPatente in lista_patenteUSuario)
            {
                Distar_EntidadesNegocio.Patente patente = new Distar_EntidadesNegocio.Patente();
                patente = patenteDAL.obtenerPatentePorId(Convert.ToString(usuarioPatente.id_patente));
                patente.negado = usuarioPatente.negado == 0 ? false : true;
                lista_patentes.Add(patente);
            }
            return lista_patentes;
        }

        public Boolean actualizarPatenteAUsuario(List<Distar_EntidadesNegocio.UsuarioPatente> lista_patenteUsuario_Add, List<Distar_EntidadesNegocio.UsuarioPatente> lista_patenteUsuario_delete)
        {
            Distar_AccesoDatos.UsuarioPatente usuarioPatenteDAL = new Distar_AccesoDatos.UsuarioPatente();
            Distar_LogicaNegocio.DigitoVerificador DVBL = new Distar_LogicaNegocio.DigitoVerificador();
            foreach (Distar_EntidadesNegocio.UsuarioPatente usuarioPatente in lista_patenteUsuario_Add)
            {
                usuarioPatente.DVH = DVBL.calcularDVH(usuarioPatente.id_usuario.ToString() + usuarioPatente.id_patente.ToString() + usuarioPatente.negado.ToString());
            }
            if (usuarioPatenteDAL.agregarPatenteUsuario(lista_patenteUsuario_Add) && usuarioPatenteDAL.quitarPatenteUsuario(lista_patenteUsuario_delete))
            {
                DVBL.actualizarDVV("UsuarioPatente");
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean agregarPatenteUsuario(List<Distar_EntidadesNegocio.UsuarioPatente> lista_patenteUsuario_add)
        {
            Distar_AccesoDatos.UsuarioPatente usuarioPatenteDAL = new Distar_AccesoDatos.UsuarioPatente();
            Distar_LogicaNegocio.DigitoVerificador DVBL = new Distar_LogicaNegocio.DigitoVerificador();
            if (usuarioPatenteDAL.agregarPatenteUsuario(lista_patenteUsuario_add))
            {
                DVBL.actualizarDVV("UsuarioPatente");
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean verificarUsoPatente(int id_patente, int id_usuario = 0)
        {
            Distar_AccesoDatos.UsuarioPatente usuarioPatenteDAL = new Distar_AccesoDatos.UsuarioPatente();
            if (usuarioPatenteDAL.verificarUsoPatente(id_patente, id_usuario) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean negarPatentesAUsuario(List<Distar_EntidadesNegocio.UsuarioPatente> lista_patenteUsuario_disable)
        {
            Distar_AccesoDatos.UsuarioPatente usuarioPatenteDAL = new Distar_AccesoDatos.UsuarioPatente();
            Distar_LogicaNegocio.DigitoVerificador DVBL = new Distar_LogicaNegocio.DigitoVerificador();
            List<Distar_EntidadesNegocio.UsuarioPatente> lista_noAgregadas_paraNegar = new List<Distar_EntidadesNegocio.UsuarioPatente>();
            foreach (Distar_EntidadesNegocio.UsuarioPatente usuarioPatente in lista_patenteUsuario_disable)
            {
                usuarioPatente.DVH = DVBL.calcularDVH(usuarioPatente.id_usuario.ToString() + usuarioPatente.id_patente.ToString() + usuarioPatente.negado.ToString());
                if (!isPatenteAsignada(usuarioPatente))
                {
                    lista_noAgregadas_paraNegar.Add(usuarioPatente);
                }
            }
            if (lista_noAgregadas_paraNegar.Count > 0)
            {
                agregarPatenteUsuario(lista_noAgregadas_paraNegar);
            }
            if (usuarioPatenteDAL.negarPatenteUsuario(lista_patenteUsuario_disable))
            {
                DVBL.actualizarDVV("UsuarioPatente");
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean habilitarPatentesAUsuario(List<Distar_EntidadesNegocio.UsuarioPatente> lista_patenteUsuario_enable)
        {
            Distar_AccesoDatos.UsuarioPatente usuarioPatenteDAL = new Distar_AccesoDatos.UsuarioPatente();
            Distar_LogicaNegocio.DigitoVerificador DVBL = new Distar_LogicaNegocio.DigitoVerificador();
            foreach (Distar_EntidadesNegocio.UsuarioPatente usuarioPatente in lista_patenteUsuario_enable)
            {
                usuarioPatente.DVH = DVBL.calcularDVH(usuarioPatente.id_usuario.ToString() + usuarioPatente.id_patente.ToString() + usuarioPatente.negado.ToString());
            }
            if (usuarioPatenteDAL.habilitarPatenteUsuario(lista_patenteUsuario_enable))
            {
                DVBL.actualizarDVV("UsuarioPatente");
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean isPatenteAsignada(Distar_EntidadesNegocio.UsuarioPatente usuarioPatente)
        {
            Distar_AccesoDatos.UsuarioPatente usuarioPatenteDAL = new Distar_AccesoDatos.UsuarioPatente();
            return usuarioPatenteDAL.isPatenteAsignada(usuarioPatente);
        }

        public Boolean isPatenteNegada(Distar_EntidadesNegocio.UsuarioPatente usuarioPatente)
        {
            Distar_AccesoDatos.UsuarioPatente usuarioPatenteDAL = new Distar_AccesoDatos.UsuarioPatente();
            return usuarioPatenteDAL.isPatenteNegada(usuarioPatente);
        }

        public Boolean usuarioTienePatenteNegada(int id_patente, int id_usuario)
        {
            Distar_AccesoDatos.UsuarioPatente usuarioPatenteDAL = new Distar_AccesoDatos.UsuarioPatente();
            if (usuarioPatenteDAL.usuarioTienePatenteNegada(id_patente, id_usuario) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<int> obtenerPatentesNoUtilizadasEnOtrosUsuarios(List<int> lista_id_patentes, int id_usuario = 0)
        {
            Distar_AccesoDatos.UsuarioPatente usuarioPatenteDAL = new Distar_AccesoDatos.UsuarioPatente(); 
            List<int> lista_id_patentes_en_otras_flias = new List<int>();
            string id_patentes_concat = null;

            foreach (int id in lista_id_patentes)
            {
                id_patentes_concat += Convert.ToString(id) + ", ";
            }
            id_patentes_concat = id_patentes_concat.Substring(0, id_patentes_concat.Length - 2);
            lista_id_patentes_en_otras_flias = usuarioPatenteDAL.obtenerIDsPatentesQueUsanUsuarios(id_patentes_concat);
            foreach (var item in lista_id_patentes_en_otras_flias)
            {
                lista_id_patentes.Remove(item);
            }
            return lista_id_patentes;
        }

    }
}
