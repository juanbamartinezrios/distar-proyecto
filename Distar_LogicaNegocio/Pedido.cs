using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distar_LogicaNegocio
{
    public class Pedido
    {
        public List<Distar_EntidadesNegocio.Pedido> getAllPedidos()
        {
            Distar_LogicaNegocio.DetallePedido detallePedidoBL = new Distar_LogicaNegocio.DetallePedido();
            Distar_LogicaNegocio.Producto productoBL = new Distar_LogicaNegocio.Producto();
            Distar_LogicaNegocio.Usuario usuarioBL = new Distar_LogicaNegocio.Usuario();
            Distar_AccesoDatos.Pedido pedidoDAL = new Distar_AccesoDatos.Pedido();
            Distar_AccesoDatos.Services services = new Distar_AccesoDatos.Services();
            List<Distar_EntidadesNegocio.Pedido> lista_pedidos = new List<Distar_EntidadesNegocio.Pedido>();
            foreach (Distar_EntidadesNegocio.Pedido pedido in pedidoDAL.getAllPedidos())
            {
                pedido.cliente = usuarioBL.obtenerUsuarioPorId(pedido.id_cliente);
                //if (pedido.cliente != null)
                //{
                //    pedido.cliente.domicilio.cp = services.Desencriptar3D(pedido.cliente.domicilio.cp);
                //    pedido.cliente.domicilio.direccion = services.Desencriptar3D(pedido.cliente.domicilio.direccion);
                //}
                pedido.detalle_items = detallePedidoBL.cargarDetalleDePedido(pedido.id_pedido);
                foreach (Distar_EntidadesNegocio.DetallePedido item in pedido.detalle_items)
                {
                    pedido.total += item.importe;
                }
                lista_pedidos.Add(pedido);
            }
            return lista_pedidos;
        }

        public Boolean create(Distar_EntidadesNegocio.Pedido pedido)
        {
            Distar_LogicaNegocio.DetallePedido detallePedidoBL = new Distar_LogicaNegocio.DetallePedido();
            Distar_LogicaNegocio.Producto productoBL = new Distar_LogicaNegocio.Producto();
            Distar_LogicaNegocio.Pedido pedidoBL = new Distar_LogicaNegocio.Pedido();
            Distar_AccesoDatos.Pedido pedidoDAL = new Distar_AccesoDatos.Pedido();
            pedido.nro_pedido = getRandomNroPedido(pedido.id_cliente.ToString());
            if (pedidoDAL.create(pedido))
            {
                int nro_detalle = 0;
                int id_pedido = pedidoBL.getIdPedido(pedido.nro_pedido);
                foreach (Distar_EntidadesNegocio.DetallePedido item in pedido.detalle_items)
                {
                    item.nro_detalle = nro_detalle++;
                    item.id_pedido = id_pedido;
                }
                if (detallePedidoBL.agregarDetalleAPedido(pedido.detalle_items))
                {
                    foreach (Distar_EntidadesNegocio.DetallePedido item in pedido.detalle_items)
                    {
                        Distar_EntidadesNegocio.Producto producto = new Distar_EntidadesNegocio.Producto();
                        item.producto.stock = item.producto.stock - item.cantidad;
                        producto = item.producto;
                        productoBL.update(producto);
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean delete(Distar_EntidadesNegocio.Pedido pedido)
        {
            Distar_LogicaNegocio.DetallePedido detallePedidoBL = new Distar_LogicaNegocio.DetallePedido();
            Distar_AccesoDatos.Pedido pedidoDAL = new Distar_AccesoDatos.Pedido();
            if (pedidoDAL.delete(pedido.id_pedido))
            {
                detallePedidoBL.eliminarDetallesDePedido(pedido.id_pedido);
                return true;
            }
            else
            {
                return false;
            }
        }

        public int getRandomNroPedido(string id_cliente)
        {
            string randomStr;
            string substr1;
            string substr2;
            string s = "123456789";
            Random rd = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 10; i++)
            {
                int idx = rd.Next(0, 5);
                sb.Append(s.Substring(idx, 1));
            }
            randomStr = sb.ToString();
            substr1 = randomStr.Substring(0, 2);
            substr2 = randomStr.Substring(7, 2);
            randomStr = substr1 + substr2 + id_cliente;
            return Convert.ToInt32(randomStr);
        }

        public int getIdPedido(int nro_pedido)
        {
            Distar_AccesoDatos.Pedido pedidoDAL = new Distar_AccesoDatos.Pedido();
            return pedidoDAL.getIdPedido(nro_pedido);
        }

        public Boolean rechazarPedido(string descripcion, int id_pedido)
        {
            Distar_AccesoDatos.Pedido pedidoDAL = new Distar_AccesoDatos.Pedido();
            return pedidoDAL.rechazarPedido(descripcion, id_pedido);
        }
    }
}
