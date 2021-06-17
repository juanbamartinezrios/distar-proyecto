using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distar_LogicaNegocio
{
    public class Producto : Distar_EntidadesNegocio.ABMGenerico<Distar_EntidadesNegocio.Producto>
    {
        public int calcularDVHPorRegistro(string id_producto)
        {
            Distar_AccesoDatos.Producto productoDAL = new Distar_AccesoDatos.Producto();
            Distar_LogicaNegocio.DigitoVerificador DVBL = new Distar_LogicaNegocio.DigitoVerificador();
            string str = productoDAL.obtenerStrParaDVHPorProducto(id_producto);
            return DVBL.calcularDVH(str);
        }

        public Boolean create(Distar_EntidadesNegocio.Producto producto)
        {
            Distar_AccesoDatos.Producto productoDAL = new Distar_AccesoDatos.Producto();
            Distar_AccesoDatos.Services services = new Distar_AccesoDatos.Services();
            Distar_LogicaNegocio.DigitoVerificador DVBL = new Distar_LogicaNegocio.DigitoVerificador();
            producto.DVH = DVBL.calcularDVH(producto.descripcion + producto.estado + producto.p_unitario.ToString() + producto.stock.ToString());
            if (productoDAL.create(producto))
            {
                int id_producto = this.getIdProducto(producto.descripcion, producto.DVH, producto.estado);
                DVBL.actualizarDVHRegistros("Producto", id_producto);
                return true;
            }
            else
            {
                return false;
            }
        }

        public int getIdProducto(string descripcion, int DVH, string estado)
        {
            Distar_AccesoDatos.Producto productoDAL = new Distar_AccesoDatos.Producto();
            return productoDAL.getIdProducto(descripcion, DVH, estado);
        }

        public Boolean delete(Distar_EntidadesNegocio.Producto producto)
        {
            return false;
        }

        public Boolean deleteLogico(Distar_EntidadesNegocio.Producto producto)
        {
            Distar_AccesoDatos.Producto productoDAL = new Distar_AccesoDatos.Producto();
            Distar_LogicaNegocio.DigitoVerificador DVBL = new Distar_LogicaNegocio.DigitoVerificador();
            if (productoDAL.deleteLogico(producto))
            {
                DVBL.actualizarDVHRegistros("Producto", producto.id_producto);
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean update(Distar_EntidadesNegocio.Producto producto)
        {
            Distar_AccesoDatos.Producto productoDAL = new Distar_AccesoDatos.Producto();
            Distar_AccesoDatos.Services services = new Distar_AccesoDatos.Services();
            Distar_LogicaNegocio.DigitoVerificador DVBL = new Distar_LogicaNegocio.DigitoVerificador();
            if (productoDAL.update(producto))
            {
                DVBL.actualizarDVHRegistros("Producto", producto.id_producto);
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Distar_EntidadesNegocio.Producto> getAllProductos()
        {
            Distar_AccesoDatos.Producto productoDAL = new Distar_AccesoDatos.Producto();
            Distar_AccesoDatos.Services services = new Distar_AccesoDatos.Services();
            List<Distar_EntidadesNegocio.Producto> lista_productos = new List<Distar_EntidadesNegocio.Producto>();
            foreach (Distar_EntidadesNegocio.Producto producto in productoDAL.getAllProductos())
            {
                if (producto.estado != "Inactivo")
                {
                    lista_productos.Add(producto);
                }
            }
            return lista_productos;
        }

        public Distar_EntidadesNegocio.Producto obtenerProductoPorId(int id_producto)
        {
            Distar_AccesoDatos.Producto productooDAL = new Distar_AccesoDatos.Producto();
            Distar_EntidadesNegocio.Producto producto = new Distar_EntidadesNegocio.Producto();
            producto = productooDAL.obtenerProductoPorId(id_producto);
            return producto;
        }
    }
}
