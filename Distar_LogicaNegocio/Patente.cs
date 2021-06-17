using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distar_LogicaNegocio
{
    public class Patente
    {
        public List<Distar_EntidadesNegocio.Patente> getAllPatentes()
        {
            Distar_AccesoDatos.Patente patenteDAL = new Distar_AccesoDatos.Patente();
            List<Distar_EntidadesNegocio.Patente> lista_patentes = new List<Distar_EntidadesNegocio.Patente>();
            foreach (Distar_EntidadesNegocio.Patente patente in patenteDAL.getAllPatentes())
            {
                lista_patentes.Add(patente);
            }
            return lista_patentes;
        }

        public Boolean verificarUsoPatente(int id_patente, Boolean verificar_usuario, int id_familia = 0, int id_usuario = 0)
        {
            Distar_LogicaNegocio.UsuarioPatente usuarioPatenteBL = new Distar_LogicaNegocio.UsuarioPatente();
            Distar_LogicaNegocio.FamiliaPatente familiaPatenteBL = new Distar_LogicaNegocio.FamiliaPatente();
            Distar_LogicaNegocio.FamiliaUsuario familiaUsuarioBL = new Distar_LogicaNegocio.FamiliaUsuario();
            List<int> list_id_patentes_return = new List<int>();
            List<int> list_id_patentes_return_final = new List<int>();

            // Busco: de las patentes que se quieren quitar, si están siendo usadas por otra familia
            if (id_familia != 0)
            {
                if (!familiaPatenteBL.verificarUsoPatente(id_patente, id_familia))
                {
                    list_id_patentes_return.Add(id_patente);
                }
                else
                {
                    List<Distar_EntidadesNegocio.Familia> lista_familias = new List<Distar_EntidadesNegocio.Familia>();
                    lista_familias = familiaPatenteBL.verificarFamiliaUsaPatente(id_patente, id_familia);
                    if (!familiaUsuarioBL.existeUsuarioEnFamilia(lista_familias))
                    {
                        list_id_patentes_return.Add(id_patente);
                    }
                }
            }
            if (list_id_patentes_return.Count > 0)
            {
                foreach (int id_pat in list_id_patentes_return)
                {
                    if (!usuarioPatenteBL.verificarUsoPatente(id_pat))
                    {
                        return false;
                    }
                }
            }
            // Busco: de las patentes que se quieren quitar, si están siendo usadas por otro usuario
            if (id_usuario != 0)
            {
                // Si en UsuarioPatente algún otro usuario usa la patente, sigo de largo.
                // Si en UsuarioPatente ningún usuario usa la patente, cargo la lista para verificar el uso en familia
                if (!usuarioPatenteBL.verificarUsoPatente(id_patente, id_usuario))
                {
                    list_id_patentes_return.Add(id_patente);
                }
                if (list_id_patentes_return.Count > 0)
                {
                    foreach (int id_pat in list_id_patentes_return)
                    {
                        List<Distar_EntidadesNegocio.Familia> lista_familias = new List<Distar_EntidadesNegocio.Familia>();
                        lista_familias = familiaPatenteBL.verificarFamiliaUsaPatente(id_pat);
                        // Se tienen las familias que usan esas patentes; se verifica si algún usuario tiene esa familias asignadas (que no sea el mismo a quien se le está quitando la patente)
                        if (lista_familias.Count > 0)
                        {
                            foreach (Distar_EntidadesNegocio.Familia familia in lista_familias)
                            {
                                // Obtengo los usuarios de dichas familias. Verifico si alguno de estos tienen la patente asignada y negada
                                // Si existe al menos uno que no la tenga negada, ya se puede quitar
                                List<Distar_EntidadesNegocio.Usuario> lista_usuarios_enfamilia = new List<Distar_EntidadesNegocio.Usuario>();
                                if (verificar_usuario)
                                {
                                    lista_usuarios_enfamilia = familiaUsuarioBL.obtenerUsuariosEnFamilia(familia.id_familia, id_usuario);
                                }
                                else
                                {
                                    lista_usuarios_enfamilia = familiaUsuarioBL.obtenerUsuariosEnFamilia(familia.id_familia);
                                }

                                if (lista_usuarios_enfamilia.Count > 0)
                                {
                                    foreach (Distar_EntidadesNegocio.Usuario usuario in lista_usuarios_enfamilia)
                                    {
                                        if (!usuarioPatenteBL.usuarioTienePatenteNegada(id_patente, usuario.id_usuario) && usuario.activo)
                                        {
                                            return true;
                                        }
                                    }
                                    return false;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return true;
                }
            }
            return true;
        }
    }
}
