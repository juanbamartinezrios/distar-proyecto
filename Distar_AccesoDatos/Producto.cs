using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Distar_AccesoDatos
{
    public class Producto : Distar_EntidadesNegocio.ABMGenerico<Distar_EntidadesNegocio.Producto>
    {
        public string obtenerStrParaDVHPorProducto(string id_producto)
        {
            string query = "SELECT descripcion, estado, p_unitario, stock FROM Producto WHERE id_producto=" + id_producto;
            string str = null;
            SqlDataReader dataReader;
            dataReader = Distar_BD.ExecuteReader(query);
            if (dataReader != null && dataReader.HasRows)
            {
                dataReader.Read();
                str = dataReader["descripcion"].ToString() + dataReader["estado"].ToString() + dataReader["p_unitario"].ToString() + dataReader["stock"].ToString();
                dataReader.Close();
            }
            return str;
        }

        public Boolean actualizarDVH(string valorActualizado, int id_registro)
        {
            string query = "UPDATE Producto SET DVH=" + valorActualizado + " WHERE id_producto=" + id_registro.ToString();
            try
            {
                return Distar_BD.ExecuteNonquery(query) > -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Producto actualizarDVH: " + ex.Message);
                return false;
            }
        }

        public List<Distar_EntidadesNegocio.Producto> getAllProductos()
        {
            List<Distar_EntidadesNegocio.Producto> listaProductos = new List<Distar_EntidadesNegocio.Producto>();
            string query = "SELECT id_producto, descripcion, DVH, estado, p_unitario, stock FROM Producto";
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    Distar_EntidadesNegocio.Producto producto = new Distar_EntidadesNegocio.Producto();
                    producto.id_producto = Convert.ToInt32(dataReader["id_producto"]);
                    producto.descripcion = Convert.ToString(dataReader["descripcion"]);
                    producto.DVH = Convert.ToInt32(dataReader["DVH"]);
                    producto.estado = Convert.ToString(dataReader["estado"]);
                    producto.p_unitario = float.Parse(Convert.ToString(dataReader["p_unitario"]));
                    producto.stock = Convert.ToInt32(dataReader["stock"]);
                    listaProductos.Add(producto);
                }
                dataReader.Close();
                return listaProductos;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Producto getAllProductos: " + ex.Message);
                return listaProductos;
            }
        }

        public int getIdProducto(string descripcion, int DVH, string estado)
        {
            string query = "SELECT id_producto FROM Producto WHERE descripcion='" + descripcion + "' AND DVH="+ DVH +" AND estado='"+ estado+"'";
            Distar_EntidadesNegocio.Producto prod = new Distar_EntidadesNegocio.Producto();
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    prod.id_producto = Convert.ToInt32(dataReader["id_producto"]);
                }
                dataReader.Close();
                return prod.id_producto;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Producto getIdProducto: " + ex.Message);
                return 0;
            }
        }

        public Boolean create(Distar_EntidadesNegocio.Producto producto)
        {
            string query = "INSERT INTO Producto VALUES ('" + producto.descripcion + "', " + producto.DVH + ", '" + producto.estado + "', CONVERT(float, '" + Convert.ToString(producto.p_unitario).Replace(",", ".") + "'), " + producto.stock + ")";
            try
            {
                return Distar_BD.ExecuteNonquery(query) > -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Producto create: " + ex.Message);
                return false;
            }
        }

        public Boolean delete(Distar_EntidadesNegocio.Producto producto)
        {
            return false;
        }

        public Boolean deleteLogico(Distar_EntidadesNegocio.Producto producto)
        {
            string query = "UPDATE Producto SET estado='Inactivo' WHERE id_producto=" + producto.id_producto;
            try
            {
                return Distar_BD.ExecuteNonquery(query) > -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Producto deleteLogico: " + ex.Message);
                return false;
            }
        }

        public Boolean update(Distar_EntidadesNegocio.Producto producto)
        {
            string query = "UPDATE Producto SET descripcion ='" + producto.descripcion + "', DVH=" + producto.DVH + ", p_unitario=" + "CONVERT(float, '" + Convert.ToString(producto.p_unitario).Replace(",", ".") + "'), " + "stock=" + producto.stock + " WHERE id_producto=" + producto.id_producto;
            try
            {
                return Distar_BD.ExecuteNonquery(query) > -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Producto update: " + ex.Message);
                return false;
            }
        }

        public Distar_EntidadesNegocio.Producto obtenerProductoPorId(int id_producto)
        {
            Distar_EntidadesNegocio.Producto producto = new Distar_EntidadesNegocio.Producto();
            string query = "SELECT * FROM Producto WHERE id_producto=" + id_producto;
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    producto.id_producto = Convert.ToInt32(dataReader["id_producto"]);
                    producto.descripcion = Convert.ToString(dataReader["descripcion"]);
                    producto.estado = Convert.ToString(dataReader["estado"]);
                    producto.p_unitario = float.Parse(Convert.ToString(dataReader["p_unitario"]));
                    producto.stock = Convert.ToInt32(dataReader["stock"]);
                }
                dataReader.Close();
                return producto;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Producto obtenerProductoPorId: " + ex.Message);
                return producto;
            }
        }

        public List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO> obtenerDTO_DV()
        {
            string query = "SELECT id_producto, descripcion, estado, p_unitario, stock, DVH FROM Producto";
            List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO> lista_dv_dto = new List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO>();
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO dto = new Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO();
                    dto.entidad = "Producto";
                    dto.valor_db = Convert.ToInt32(dataReader["DVH"]);
                    dto.id_registro = Convert.ToInt32(dataReader["id_producto"]);
                    dto.txtstr = Convert.ToString(dataReader["descripcion"]) + Convert.ToString(dataReader["estado"]) + Convert.ToString(dataReader["p_unitario"]).Replace(',','.') + Convert.ToInt32(dataReader["stock"]).ToString();
                    lista_dv_dto.Add(dto);
                }
                dataReader.Close();
                return lista_dv_dto;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Producto obtenerDTO_DV: " + ex.Message);
                return lista_dv_dto;
            }
        }
    }
}
