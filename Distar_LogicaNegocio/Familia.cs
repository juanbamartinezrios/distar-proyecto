using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distar_LogicaNegocio
{
    public class Familia : Distar_EntidadesNegocio.ABMGenerico<Distar_EntidadesNegocio.Familia>
    {
        public Boolean create(Distar_EntidadesNegocio.Familia familia) 
        {
            Distar_AccesoDatos.Familia familiaDAL = new Distar_AccesoDatos.Familia();
            Distar_AccesoDatos.Services services = new Distar_AccesoDatos.Services();
            familia.descripcion = services.Encriptar3D(familia.descripcion);
            if (familiaDAL.create(familia))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Boolean eliminarFamilia(Distar_EntidadesNegocio.Familia familia)
        {
            Distar_AccesoDatos.Familia familiaDAL = new Distar_AccesoDatos.Familia();
            Distar_LogicaNegocio.FamiliaPatente familiaPatenteBL = new Distar_LogicaNegocio.FamiliaPatente();
            Distar_LogicaNegocio.UsuarioPatente usuarioPatenteBL = new Distar_LogicaNegocio.UsuarioPatente();
            Distar_LogicaNegocio.DigitoVerificador DVBL = new Distar_LogicaNegocio.DigitoVerificador();
            List<int> lista_id_patentes = familiaPatenteBL.obtenerIDsPatentesEnFamilia(familia.id_familia);
            List<int> lista = familiaPatenteBL.obtenerPatentesNoUtilizadasEnOtrasFamilias(familia.id_familia, lista_id_patentes);
            if (lista.Count == 0)
            {
                if (familiaDAL.delete(familia))
                {
                    if (familiaPatenteBL.eliminarReferenciaFP(familia.id_familia))
                    {
                        DVBL.actualizarDVV("FamiliaPatente");
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                List<int> lista_u = usuarioPatenteBL.obtenerPatentesNoUtilizadasEnOtrosUsuarios(lista_id_patentes);
                if (!(lista_u.Count > 0))
                {
                    if (familiaDAL.delete(familia))
                    {
                        if (familiaPatenteBL.eliminarReferenciaFP(familia.id_familia))
                        {
                            DVBL.actualizarDVV("FamiliaPatente");
                        }
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
        }

        public Boolean delete(Distar_EntidadesNegocio.Familia familia)
        {
            return eliminarFamilia(familia);
        }

        public Boolean deleteLogico(Distar_EntidadesNegocio.Familia familia)
        {
            return false;
        }

        public Boolean update(Distar_EntidadesNegocio.Familia familia)
        {
            Distar_AccesoDatos.Familia familiaDAL = new Distar_AccesoDatos.Familia();
            Distar_AccesoDatos.Services services = new Distar_AccesoDatos.Services();
            familia.descripcion = services.Encriptar3D(familia.descripcion);
            if (familiaDAL.update(familia))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Distar_EntidadesNegocio.Familia> getAllFamilias()
        {
            Distar_AccesoDatos.Familia familiaDAL = new Distar_AccesoDatos.Familia();
            Distar_AccesoDatos.Services services = new Distar_AccesoDatos.Services();
            List<Distar_EntidadesNegocio.Familia> lista_familias = new List<Distar_EntidadesNegocio.Familia>();
            foreach (Distar_EntidadesNegocio.Familia familia in familiaDAL.getAllFamilias())
            {
                familia.descripcion = services.Desencriptar3D(familia.descripcion);
                familia.patentes = obtenerPatentesFamilia(familia.id_familia);
                lista_familias.Add(familia);
            }
            return lista_familias;
        }

        private List<Distar_EntidadesNegocio.Patente> obtenerPatentesFamilia(int id_familia){
            List<Distar_EntidadesNegocio.Patente> listaPatentes = new List<Distar_EntidadesNegocio.Patente>();
            Distar_LogicaNegocio.FamiliaPatente familiaPatenteBL = new Distar_LogicaNegocio.FamiliaPatente();
            listaPatentes = familiaPatenteBL.obtenerPatentesFamilia(id_familia);
            return listaPatentes;
        }

        public Distar_EntidadesNegocio.Familia obtenerFamiliaConPatentes(int id_familia)
        {
            Distar_LogicaNegocio.Familia familiaBL = new Distar_LogicaNegocio.Familia();
            Distar_AccesoDatos.Familia familiaDAL = new Distar_AccesoDatos.Familia();
            Distar_EntidadesNegocio.Familia familia = new Distar_EntidadesNegocio.Familia();
            List<Distar_EntidadesNegocio.Patente> lista_patentes = new List<Distar_EntidadesNegocio.Patente>();
            Distar_AccesoDatos.Services services = new Distar_AccesoDatos.Services();

            familia = familiaDAL.getFamiliaById(id_familia);
            familia.descripcion = services.Desencriptar3D(familia.descripcion);
            lista_patentes = completarPatentesDeFamilia(familia.id_familia);
            familia.patentes = lista_patentes;
            return familia;
        }

        public List<Distar_EntidadesNegocio.Familia> obtenerFamiliasSinUsuariosActivos()
        {
            Distar_AccesoDatos.Familia familiaDAL = new Distar_AccesoDatos.Familia();
            return familiaDAL.obtenerFamiliasSinUsuariosActivos();
        }

        private List<Distar_EntidadesNegocio.Patente> completarPatentesDeFamilia(int id_familia)
        {
            List<Distar_EntidadesNegocio.Patente> lista_patentes = new List<Distar_EntidadesNegocio.Patente>();
            Distar_LogicaNegocio.FamiliaPatente familiaPatenteBL = new Distar_LogicaNegocio.FamiliaPatente();
            lista_patentes = familiaPatenteBL.obtenerPatentesFamilia(id_familia);
            return lista_patentes;
        }
    }
}
