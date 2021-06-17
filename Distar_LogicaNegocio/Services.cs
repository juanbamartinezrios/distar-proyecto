using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using System.Xml;

namespace Distar_LogicaNegocio
{
    public class Services
    {
        private List<Distar_EntidadesNegocio.Patente> lista_patentes_usuario = new List<Distar_EntidadesNegocio.Patente>();

        public bool existeConnectionstring()
        {
            bool isConnected = false;
            string str;
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.Load(@"C:\\Distar\\Distar_connection.xml");
            XmlNodeList xmlnode;
            xmlnode = doc.GetElementsByTagName("conexionDB");
            foreach (XmlNode nodo in xmlnode)
            {
                isConnected = bool.Parse(nodo.SelectSingleNode("isConnected").InnerText);
                str = string.Format(nodo.SelectSingleNode("connectionString").InnerText);
            }
            return isConnected;
        }

        public string getConnectionString()
        {
            string connectionString = null;
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.Load(@"C:\\Distar\\Distar_connection.xml");
            XmlNodeList xmlnode;
            xmlnode = doc.GetElementsByTagName("conexionDB");
            foreach (XmlNode nodo in xmlnode)
                connectionString = string.Format(nodo.SelectSingleNode("connectionString").InnerText);

            return connectionString;
        }

        public Boolean probarConexion(string userDB, string passDB, string nombreDB, string servidor, bool seguridadIntegrada) // Esto nos dice si las credenciales que ingresaron son correctas
        {
            Distar_AccesoDatos.Services servicesDAL = new Distar_AccesoDatos.Services();
            return servicesDAL.ProbarConnectionString(userDB, passDB, nombreDB, servidor, seguridadIntegrada);
        }

        public Boolean probarConexion(string connectionString) // Esto nos dice si las credenciales que ingresaron son correctas
        {
            Distar_AccesoDatos.Services servicesDAL = new Distar_AccesoDatos.Services();
            return servicesDAL.ProbarConnectionString(connectionString);
        }

        public void actualizarConexion(string userDB, string passDB, string nombreDB, string servidor)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"C:\\Distar\\Distar_connection.xml");
            XmlNodeList xmlnode;
            xmlnode = doc.GetElementsByTagName("conexionDB");
            foreach (XmlNode nodo in xmlnode)
                nodo.SelectSingleNode("isConnected").InnerText = "True";
            doc.Save(@"C:\\Distar\\Distar_connection.xml");
        }

        public void setConnextionString(string userDB, string passDB, string nombreDB, string servidor, bool seguridadIntegrada) {
            Distar_AccesoDatos.Services servicesDAL = new Distar_AccesoDatos.Services();
            string conncectionString = "Data Source=" + servidor + ";Initial Catalog=" + nombreDB + ";";
            if (seguridadIntegrada)
                conncectionString += "Integrated Security=True;";
            else
                conncectionString += "User ID=" + userDB + ";Password=" + passDB + ";";
            conncectionString = servicesDAL.EncriptarASCII(conncectionString);
            XmlDocument doc = new XmlDocument();
            doc.Load(@"C:\\Distar\\Distar_connection.xml");
            XmlNodeList xmlnode;
            xmlnode = doc.GetElementsByTagName("conexionDB");
            foreach(XmlNode nodo in xmlnode)
                nodo.SelectSingleNode("connectionString").InnerText = conncectionString;
            doc.Save(@"C:\\Distar\\Distar_connection.xml");
        }

        public string DesencriptarASCII(string str)
        {
            Distar_AccesoDatos.Services servicesDAL = new Distar_AccesoDatos.Services();
            return servicesDAL.DesencriptarASCII(str);
        }

        public Boolean CrearCopiaDeSeguridad(string path, string nombre, string cant_vol)
        {
            Distar_AccesoDatos.Services services = new Distar_AccesoDatos.Services();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (services.CrearCopiaDeSeguridad(nombre, path, Convert.ToInt32(cant_vol)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean RestaurarCopiaDeSeguridad(string path, string nombre, string cant_vol)
        {
            Distar_AccesoDatos.Services services = new Distar_AccesoDatos.Services();
            if (services.RestaurarCopiaDeSeguridad(nombre, path, Convert.ToInt32(cant_vol)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean validarPatente(int id_patente)
        {
            return lista_patentes_usuario.Exists(x => x.id_patente == id_patente) ? true : false;
        }

        public void setPatentesUsuarioLog(Distar_EntidadesNegocio.Usuario user)
        {
            lista_patentes_usuario = getPatentes(user);
        }

        private List<Distar_EntidadesNegocio.Patente> getPatentes(Distar_EntidadesNegocio.Usuario user)
        {
            List<Distar_EntidadesNegocio.Patente> lista_patentes = new List<Distar_EntidadesNegocio.Patente>();
            if (user.familias != null)
            {
                foreach (Distar_EntidadesNegocio.Familia familia in user.familias)
                {
                    foreach (Distar_EntidadesNegocio.Patente p in familia.patentes)
                    {
                        if (lista_patentes.Count == 0)
                        {
                            if (!p.negado)
                            {
                                lista_patentes.Add(p);
                            }
                        }
                        else
                        {
                            if (lista_patentes.Exists(x => x.id_patente != p.id_patente && !p.negado))
                            {
                                lista_patentes.Add(p);
                            }
                        }
                    }
                }
            }
            if (user.patentes != null)
            {
                foreach (Distar_EntidadesNegocio.Patente p in user.patentes)
                {
                    if (lista_patentes.Count == 0)
                    {
                        if (!p.negado)
                        {
                            lista_patentes.Add(p);
                        }
                    }
                    else
                    {
                        if (lista_patentes.Exists(x => x.id_patente != p.id_patente && !p.negado))
                        {
                            lista_patentes.Add(p);
                        }
                    }
                }
            }
            return lista_patentes;
        }
    }
}
