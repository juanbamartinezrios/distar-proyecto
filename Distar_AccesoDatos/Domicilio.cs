using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Distar_AccesoDatos
{
    public class Domicilio
    {
        public Boolean agregarDomicilioUsuario(Distar_EntidadesNegocio.Domicilio domicilio)
        {
            string query = "INSERT INTO Domicilio VALUES ('" + domicilio.cp+ "', '" + domicilio.direccion + "', " + domicilio.id_usuario + "," + domicilio.numero_dom + ")";
            try
            {
                return Distar_BD.ExecuteNonquery(query) > -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Direccion agregarDomicilioUsuario: " + ex.Message);
                return false;
            }
        }

        public Boolean update(Distar_EntidadesNegocio.Domicilio domicilio)
        {
            string query = "UPDATE Domicilio SET ";
            if (!string.IsNullOrEmpty(domicilio.direccion))
                query = query + " direccion='" + domicilio.direccion + "',";
            if (!string.IsNullOrEmpty(domicilio.cp))
                query = query + " cp='" + domicilio.cp + "',";
            if (!string.IsNullOrEmpty(domicilio.numero_dom))
                query = query + " numero_dom='" + domicilio.numero_dom + "',";
            query = query.Substring(0, query.Length - 1) + " WHERE id_domicilio=" + domicilio.id_domicilio + " AND id_usuario=" + domicilio.id_usuario;
            try
            {
                return Distar_BD.ExecuteNonquery(query) > -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Direccion update: " + ex.Message);
                return false;
            }
        }

        public Distar_EntidadesNegocio.Domicilio getDomicilioByIdUsuario(int id_usuario){
            string query = "SELECT * FROM Domicilio WHERE id_usuario="+Convert.ToString(id_usuario);
            Distar_EntidadesNegocio.Domicilio domicilio = new Distar_EntidadesNegocio.Domicilio();
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    domicilio.id_domicilio = Convert.ToInt32(dataReader["id_domicilio"]);
                    domicilio.cp = Convert.ToString(dataReader["cp"]);
                    domicilio.direccion = Convert.ToString(dataReader["direccion"]);
                    domicilio.numero_dom = Convert.ToString(dataReader["numero_dom"]);
                }
                dataReader.Close();
                return domicilio;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Direccion getDomicilioByIdUsuario: " + ex.Message);
                return domicilio;
            }
        }

        public Distar_EntidadesNegocio.Domicilio obtenerDomicilioPorId(int id_domicilio)
        {
            string query = "SELECT * FROM Domicilio WHERE id_domicilio=" + id_domicilio;
            Distar_EntidadesNegocio.Domicilio domicilio = new Distar_EntidadesNegocio.Domicilio();
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    domicilio.id_domicilio = Convert.ToInt32(dataReader["id_domicilio"]);
                    domicilio.cp = Convert.ToString(dataReader["cp"]);
                    domicilio.direccion = Convert.ToString(dataReader["direccion"]);
                    domicilio.numero_dom = Convert.ToString(dataReader["numero_dom"]);
                }
                dataReader.Close();
                return domicilio;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Direccion obtenerDomicilioPorId: " + ex.Message);
                return domicilio;
            }
        }
    }
}
