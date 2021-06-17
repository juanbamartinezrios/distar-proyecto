using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data.SqlClient;
using System.Data;

namespace Distar_AccesoDatos
{
    public class Distar_BD
    {
        static SqlConnection mConnection;
        private static string _connectionString;

        public static string Connectionstring
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    Distar_AccesoDatos.Services servicesDAL = new Distar_AccesoDatos.Services();
                    System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                    doc.Load(@"C:\\Distar\\Distar_connection.xml");
                    XmlNodeList xmlnode;
                    xmlnode = doc.GetElementsByTagName("conexionDB");
                    foreach (XmlNode nodo in xmlnode){
                        _connectionString = servicesDAL.DesencriptarASCII(nodo.SelectSingleNode("connectionString").InnerText);
                    }
                }
                return _connectionString;
            }
            set
            {
                _connectionString = value;
            }
        }

        public static Boolean ProbarConexion(string str)
        {
            try {
                mConnection = new SqlConnection(str);
                mConnection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Conexión a la BD fallida.");
                Console.WriteLine(ex.Message);
                mConnection.Close();
                mConnection.Dispose();
                return false;
            }

            if (mConnection.State == System.Data.ConnectionState.Open)
            {
                Console.WriteLine("Conexión a la BD exitosa.");
                mConnection.Close();
                mConnection.Dispose();
                return true;
            }
            else
            {
                mConnection.Close();
                mConnection.Dispose();
                return false;
            }
        }

        public static int ExecuteNonquerySec(string commandText)
        {
            try
            {
                mConnection = new SqlConnection(Connectionstring);
                SqlCommand sqlCommand = new SqlCommand(commandText, mConnection);
                mConnection.Open();
                return sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return 99;
            }
            finally
            {
                mConnection.Close();
                mConnection.Dispose();
            }
        }

        public static int ExecuteNonquery(string commandText)
        {
            try
            {
                mConnection = new SqlConnection(Connectionstring);
                SqlCommand sqlCommand = new SqlCommand(commandText, mConnection);
                mConnection.Open();
                return sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;
            }
            finally
            {
                mConnection.Close();
                mConnection.Dispose();
            }
        }

        public static SqlDataReader ExecuteReader(string commandText) {
            SqlDataReader myDataReader;
            try {
                mConnection = new SqlConnection(Connectionstring);
                SqlCommand sqlCommand = new SqlCommand(commandText, mConnection);
                mConnection.Open();
                myDataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                return myDataReader;
            } catch (SqlException ex) {
                string myStr = "";
                foreach(SqlError err in ex.Errors){
                    myStr += err.Message + "\r\n";
                }
                Console.WriteLine("Ocurrió un error en el acceso a la base de datos. Mensaje: "+ex.Message);
                Console.WriteLine("Errores del servidor SQL: "+myStr);
                return null;
            } catch (Exception ex) {
                Console.WriteLine(ex.Message+" Método: ExecuteReader - Clase: Distar_DB");
                return null;
            } finally {
                //mConnection.Close();
                //mConnection.Dispose();
            }
        }

        public static int ExecuteScalar(string commandText)
        {
            try
            {
                mConnection = new SqlConnection(Connectionstring);
                SqlCommand sqlCommand = new SqlCommand(commandText, mConnection);
                mConnection.Open();
                return Convert.ToInt32(sqlCommand.ExecuteScalar());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
            finally
            {
                mConnection.Close();
                mConnection.Dispose();
            }
        }
    }
}
