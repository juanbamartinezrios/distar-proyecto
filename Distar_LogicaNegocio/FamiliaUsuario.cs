using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distar_LogicaNegocio
{
    public class FamiliaUsuario
    {
        public List<Distar_EntidadesNegocio.Familia> cargarFamiliasDeUsuario(int id_usuario)
        {
            Distar_LogicaNegocio.Familia familiaBL = new Distar_LogicaNegocio.Familia();
            Distar_AccesoDatos.Familia familiaDAL = new Distar_AccesoDatos.Familia();
            Distar_AccesoDatos.FamiliaUsuario familiaUsuarioDAL = new Distar_AccesoDatos.FamiliaUsuario();
            List<int> lista_id_familia = new List<int>();
            List<Distar_EntidadesNegocio.Familia> lista_familia = new List<Distar_EntidadesNegocio.Familia>();

            lista_id_familia = familiaUsuarioDAL.obtenerIDsFamiliaUsuario(id_usuario);

            foreach (int id_familia in lista_id_familia)
            {
                Distar_EntidadesNegocio.Familia familia = new Distar_EntidadesNegocio.Familia();
                familia = familiaBL.obtenerFamiliaConPatentes(id_familia);
                lista_familia.Add(familia);
            }
            return lista_familia;
        }

        public Boolean agregarFamiliaUsuario(List<Distar_EntidadesNegocio.FamiliaUsuario> lista_familiaUsuario_add)
        {
            Distar_AccesoDatos.FamiliaUsuario familiaUsuarioDAL = new Distar_AccesoDatos.FamiliaUsuario();
            if (familiaUsuarioDAL.agregarFamiliaUsuario(lista_familiaUsuario_add))
            {
                // Actualizar las familias del usuario
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean actualizarFamiliaAUsuario(List<Distar_EntidadesNegocio.FamiliaUsuario> lista_familiaUsuario_Add, List<Distar_EntidadesNegocio.FamiliaUsuario> lista_familiaUsuario_delete)
        {
            Distar_AccesoDatos.FamiliaUsuario familiaUsuarioDAL = new Distar_AccesoDatos.FamiliaUsuario();
            if (familiaUsuarioDAL.agregarFamiliaUsuario(lista_familiaUsuario_Add) && familiaUsuarioDAL.quitarFamiliaUsuario(lista_familiaUsuario_delete))
            {
                // Actualizar las familias del usuario
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean existeUsuarioEnFamilia(List<Distar_EntidadesNegocio.Familia> lista_familias, int id_patente = 0)
        {
            Distar_AccesoDatos.FamiliaUsuario familiaUsuarioDAL = new Distar_AccesoDatos.FamiliaUsuario();
            string id_familias_concat = "";
            foreach (Distar_EntidadesNegocio.Familia familia in lista_familias)
            {
                id_familias_concat += Convert.ToString(familia.id_familia) + ", ";
            }
            id_familias_concat = id_familias_concat.Substring(0, id_familias_concat.Length - 2);
            if (id_patente == 0)
            {
                return familiaUsuarioDAL.existeUsuarioActivoEnFamilia(id_familias_concat);
            }
            else
            {
                return familiaUsuarioDAL.existeUsuarioActivoEnFamilia(id_familias_concat, id_patente);
            }
        }

        public Boolean existeOtroUsuarioActivoEnFamilia(int id_familia, int id_usuario, int id_patente = 0)
        {
            Distar_AccesoDatos.FamiliaUsuario familiaUsuarioDAL = new Distar_AccesoDatos.FamiliaUsuario();
            if (id_patente == 0){
                return familiaUsuarioDAL.existeOtroUsuarioActivoEnFamilia(id_familia, id_usuario);
            } else {
                return familiaUsuarioDAL.existeOtroUsuarioActivoEnFamilia(id_familia, id_usuario, id_patente);
            }
        }

        public List<Distar_EntidadesNegocio.Usuario> obtenerUsuariosEnFamilia(int id_familia, int id_usuario = 0)
        {
            Distar_AccesoDatos.FamiliaUsuario familiaUsuarioDAL = new Distar_AccesoDatos.FamiliaUsuario();
            if (id_usuario == 0)
            {
                return familiaUsuarioDAL.obtenerUsuariosDeFamilia(id_familia);
            }
            else
            {
                return familiaUsuarioDAL.obtenerUsuariosDeFamilia(id_familia, id_usuario);
            }
        }
    }
}
