using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distar_LogicaNegocio
{
    public class Idioma
    {
        public List<Distar_EntidadesNegocio.Idioma> getAllIdiomas()
        {
            Distar_AccesoDatos.Idioma idiomaDAL = new Distar_AccesoDatos.Idioma();
            List<Distar_EntidadesNegocio.Idioma> lista_idiomas = new List<Distar_EntidadesNegocio.Idioma>();
            foreach (Distar_EntidadesNegocio.Idioma idioma in idiomaDAL.getAllIdiomas())
            {
                lista_idiomas.Add(idioma);
            }
            return lista_idiomas;
        }
    }
}
