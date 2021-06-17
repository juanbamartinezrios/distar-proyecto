using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Distar_AccesoDatos
{
    public class CuentaCorriente
    {
        public string calcularDVHPorRegistro(int id_cta_cte)
        {
            string query = "SELECT id_cliente, nro_cta FROM CuentaCorriente WHERE id_cta_cte=" + id_cta_cte;
            string str = "";
            SqlDataReader dataReader;
            dataReader = Distar_BD.ExecuteReader(query);
            if (dataReader != null && dataReader.HasRows)
            {
                dataReader.Read();
                str = dataReader["id_cliente"].ToString() + dataReader["nro_cta"].ToString();
                dataReader.Close();
            }
            return str;
        }

        public Boolean actualizarDVH(string valorActualizado, int id_registro)
        {
            string query = "UPDATE CuentaCorriente SET DVH=" + valorActualizado + " WHERE id_cta_cte=" + id_registro;
            try
            {
                return Distar_BD.ExecuteNonquery(query) > -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL CuentaCorriente actualizarDVH: " + ex.Message);
                return false;
            }
        }

        public Boolean agregarCtaCteUsuario(Distar_EntidadesNegocio.CuentaCorriente cuentaCorriente)
        {
            string query = "INSERT INTO CuentaCorriente VALUES (" + cuentaCorriente.id_cliente + ", '" + cuentaCorriente.nro_cta + "' ," + cuentaCorriente .DVH + ")";
            try
            {
                return Distar_BD.ExecuteNonquery(query) > -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL CuentaCorriente agregarCtaCteUsuario: " + ex.Message);
                return false;
            }
        }

        public Distar_EntidadesNegocio.CuentaCorriente obtenerCtaCtePorId(int id_cta_cte)
        {
            string query = "SELECT * FROM CuentaCorriente WHERE id_cta_cte=" + id_cta_cte;
            Distar_EntidadesNegocio.CuentaCorriente cuentaCorriente = new Distar_EntidadesNegocio.CuentaCorriente();
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    cuentaCorriente.id_cliente = Convert.ToInt32(dataReader["id_cliente"]);
                    cuentaCorriente.id_cta_cte = Convert.ToInt32(dataReader["id_cta_cte"]);
                    cuentaCorriente.nro_cta = Convert.ToString(dataReader["nro_cta"]);
                    cuentaCorriente.DVH = Convert.ToInt32(dataReader["DVH"]);
                }
                dataReader.Close();
                return cuentaCorriente;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL CuentaCorriente obtenerCtaCtePorId: " + ex.Message);
                return cuentaCorriente;
            }
        }

        public Distar_EntidadesNegocio.CuentaCorriente cargarCuentaCorrienteDeusuario(int id_cliente)
        {
            string query = "SELECT * FROM CuentaCorriente WHERE id_cliente=" + id_cliente;
            Distar_EntidadesNegocio.CuentaCorriente cuentaCorriente = new Distar_EntidadesNegocio.CuentaCorriente();
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    cuentaCorriente.id_cliente = Convert.ToInt32(dataReader["id_cliente"]);
                    cuentaCorriente.id_cta_cte = Convert.ToInt32(dataReader["id_cta_cte"]);
                    cuentaCorriente.nro_cta = Convert.ToString(dataReader["nro_cta"]);
                    cuentaCorriente.DVH = Convert.ToInt32(dataReader["DVH"]);
                }
                dataReader.Close();
                return cuentaCorriente;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL CuentaCorriente cargarCuentaCorrienteDeusuario: " + ex.Message);
                return cuentaCorriente;
            }
        }

        public int getIdCtaCte(string nro_cta)
        {
            string query = "SELECT id_cta_cte FROM CuentaCorriente WHERE nro_cta='" + nro_cta + "'";
            Distar_EntidadesNegocio.CuentaCorriente cta = new Distar_EntidadesNegocio.CuentaCorriente();
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    cta.id_cta_cte = Convert.ToInt32(dataReader["id_cta_cte"]);
                }
                dataReader.Close();
                return cta.id_cta_cte;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL CuentaCorriente getIdCtaCte: " + ex.Message);
                return 0;
            }
        }

        public List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO> obtenerDTO_DV()
        {
            string query = "SELECT id_cta_cte, id_cliente, nro_cta, DVH FROM CuentaCorriente";
            List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO> lista_dv_dto = new List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO>();
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO dto = new Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO();
                    dto.entidad = "CuentaCorriente";
                    dto.valor_db = Convert.ToInt32(dataReader["DVH"]);
                    dto.id_registro = Convert.ToInt32(dataReader["id_cta_cte"]);
                    dto.txtstr = Convert.ToInt32(dataReader["id_cliente"]) + Convert.ToString(dataReader["nro_cta"]);
                    lista_dv_dto.Add(dto);
                }
                dataReader.Close();
                return lista_dv_dto;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL CuentaCorriente obtenerDTO_DV: " + ex.Message);
                return lista_dv_dto;
            }
        }
    }
}
