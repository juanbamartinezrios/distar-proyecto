using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distar_LogicaNegocio
{
    public class FamiliaPatente
    {
        public Boolean actualizarPatenteAFamilia(List<Distar_EntidadesNegocio.FamiliaPatente> lista_familiaPatente_add, List<Distar_EntidadesNegocio.FamiliaPatente> lista_familiaPatente_delete)
        {
            Distar_AccesoDatos.FamiliaPatente familiaPatenteDAL = new Distar_AccesoDatos.FamiliaPatente();
            Distar_LogicaNegocio.DigitoVerificador DVBL = new Distar_LogicaNegocio.DigitoVerificador();
            foreach (Distar_EntidadesNegocio.FamiliaPatente familiaPatente in lista_familiaPatente_add)
            {
                familiaPatente.DVH = DVBL.calcularDVH(familiaPatente.id_familia.ToString() + familiaPatente.id_patente.ToString());
            }

            if (familiaPatenteDAL.agregarPatenteAFamilia(lista_familiaPatente_add) && familiaPatenteDAL.quitarPatenteAFamilia(lista_familiaPatente_delete))
            {
                DVBL.actualizarDVV("FamiliaPatente");
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Distar_EntidadesNegocio.Patente> obtenerPatentesFamilia(int id_familia)
        {
            Distar_AccesoDatos.FamiliaPatente familiaPatenteDAL = new Distar_AccesoDatos.FamiliaPatente();
            Distar_AccesoDatos.Patente patenteDAL = new Distar_AccesoDatos.Patente();
            List<Distar_EntidadesNegocio.Patente> listaPatentes = new List<Distar_EntidadesNegocio.Patente>();
            List<int> listaIsPatente = new List<int>();
            listaIsPatente = familiaPatenteDAL.obtenerIdPatentePorFamilia(id_familia);
            foreach (int id in listaIsPatente)
	        {
		        Distar_EntidadesNegocio.Patente patente = new Distar_EntidadesNegocio.Patente();
                patente = patenteDAL.obtenerPatentePorId(Convert.ToString(id));
                listaPatentes.Add(patente);
	        }
            return listaPatentes;
        }

        public Boolean verificarUsoPatente(int id_patente, int id_familia = 0)
        {
            Distar_AccesoDatos.FamiliaPatente familiaPatenteDAL = new Distar_AccesoDatos.FamiliaPatente();
            return familiaPatenteDAL.obtenerCountPatenteEnOtrasFamilias(id_patente, id_familia) > 0;
        }

        public List<Distar_EntidadesNegocio.Familia> verificarFamiliaUsaPatente(int id_patente, int id_familia = 0)
        {
            Distar_AccesoDatos.FamiliaPatente familiaPatenteDAL = new Distar_AccesoDatos.FamiliaPatente();
            List<Distar_EntidadesNegocio.Familia> lista_familias = new List<Distar_EntidadesNegocio.Familia>();
            lista_familias = familiaPatenteDAL.obtenerFamiliasConPatente(id_patente, id_familia);
            return lista_familias;
        }

        public List<int> obtenerIDsPatentesEnFamilia(int id_familia)
        {
            Distar_AccesoDatos.FamiliaPatente familiaPatenteDAL = new Distar_AccesoDatos.FamiliaPatente();
            List<int> lista = new List<int>();
            lista = familiaPatenteDAL.obtenerIdPatentePorFamilia(id_familia);
            return lista;
        }

        public Boolean eliminarReferenciaFP(int id_familia)
        {
            Distar_AccesoDatos.FamiliaPatente familiaPatenteDAL = new Distar_AccesoDatos.FamiliaPatente();
            return familiaPatenteDAL.eliminarReferenciaFP(id_familia);
        }

        public List<int> obtenerPatentesNoUtilizadasEnOtrasFamilias(int id_familia, List<int> lista_id_patentes, int id_usuario = 0)
        {
            Distar_LogicaNegocio.Familia familiaBL = new Distar_LogicaNegocio.Familia();
            Distar_AccesoDatos.FamiliaPatente familiaPatenteDAL = new Distar_AccesoDatos.FamiliaPatente();
            List<int> lista_id_patentes_utilizadas_en_otras_familias = new List<int>();
            List<Distar_EntidadesNegocio.Familia> lista_familia_sin_usuarios = new List<Distar_EntidadesNegocio.Familia>();
            string id_patentes_concat = "0, ";
            foreach (int id in lista_id_patentes)
            {
                id_patentes_concat += Convert.ToString(id) + ", ";
            }
            id_patentes_concat = id_patentes_concat.Substring(0, id_patentes_concat.Length - 2);

            lista_familia_sin_usuarios = familiaBL.obtenerFamiliasSinUsuariosActivos();
            string id_familias_concat = Convert.ToString(id_familia) + ", ";
            foreach (Distar_EntidadesNegocio.Familia familia in lista_familia_sin_usuarios)
            {
                id_familias_concat += Convert.ToString(familia.id_familia) + ", ";
            }

            if (id_usuario != 0)
            {
                Distar_LogicaNegocio.FamiliaUsuario familiaUsuarioBL = new Distar_LogicaNegocio.FamiliaUsuario();
                foreach (Distar_EntidadesNegocio.Familia familia in familiaUsuarioBL.cargarFamiliasDeUsuario(id_usuario))
                {
                    if (!familiaUsuarioBL.existeOtroUsuarioActivoEnFamilia(familia.id_familia, id_usuario))
                    {
                        id_familias_concat += Convert.ToString(familia.id_familia) + ", ";
                    }
                }
            }

            id_familias_concat = id_familias_concat.Substring(0, id_familias_concat.Length - 2);
            lista_id_patentes_utilizadas_en_otras_familias = familiaPatenteDAL.obtenerIdPatenteQueNoSeUtilizanEnOtrasFamilias(id_familias_concat, id_patentes_concat);
            foreach (int item in lista_id_patentes_utilizadas_en_otras_familias)
            {
                lista_id_patentes.Remove(item);
            }
            return lista_id_patentes;
        }
    }
}
