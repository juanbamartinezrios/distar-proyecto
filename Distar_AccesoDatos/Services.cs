using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Web;

namespace Distar_AccesoDatos
{
    public class Services
    {
        //Llamada al proveedor de encriptados 3DES
        private TripleDESCryptoServiceProvider m_des = new TripleDESCryptoServiceProvider();

        //Define en controlador de cadenas de texto
        private UTF8Encoding m_utf8 = new UTF8Encoding();

        private Byte[] m_key;
        private Byte[] m_iv;

        //Llave local y vector de bytes
        private readonly Byte[] key = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };

        //Cambiar los valores numéricos por unos diferentes ya que es parte de la llave de codificación y decodificación
        private readonly Byte[] iv = { 43, 16, 44, 35, 56, 32, 41, 14 };

        public Services()
        {
            this.m_key = key;
            this.m_iv = iv;
        }

        public string EncriptarASCII(string input)
        {
            Byte [] IV = ASCIIEncoding.ASCII.GetBytes("qualityi");
            Byte [] EncryptionKey = Convert.FromBase64String("rpaSPvIvVLlrcmtzPU9/c67Gkj7yL1S5");
            Byte [] Buffer = Encoding.UTF8.GetBytes(input);
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            des.Key = EncryptionKey;
            des.IV = IV;
            return Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(Buffer, 0, Buffer.Length));
        }

        public string DesencriptarASCII(string input)
        {
            Byte [] IV = ASCIIEncoding.ASCII.GetBytes("qualityi"); // La clave debe ser de 8 caracteres
            Byte [] EncryptionKey = Convert.FromBase64String("rpaSPvIvVLlrcmtzPU9/c67Gkj7yL1S5"); // No se puede alterar la cantidad de caracteres pero si la clave
            Byte [] Buffer = Convert.FromBase64String(input);
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            des.Key = EncryptionKey;
            des.IV = IV;
            return Encoding.UTF8.GetString(des.CreateDecryptor().TransformFinalBlock(Buffer, 0, Buffer.Length));
        }

        public Boolean CrearCopiaDeSeguridad(string nombreBU, string rutaBU, int volumen)
        {
            string query = "BACKUP DATABASE Distar TO ";
            string splits = "DISK='" + rutaBU + @"\" + nombreBU;
            for (int i = 1; i <= volumen; i++)
            {
                query += splits + i + ".bak', ";
            }
            query = query.Substring(0, query.Length - 2);
            query += " WITH INIT, NOUNLOAD, NAME='Data backup', NOSKIP,  STATS=10,  NOFORMAT";
            try
            {
                // Por alguna razón, al tener los permisos, executeNonquery devuelve -1 pero el back-up de hace...
                return Distar_BD.ExecuteNonquerySec(query) != 99;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Services CrearCopiaDeSeguridad: " + ex.Message);
                return false;
            }
        }

        public string MD5EncriptPW(string pw)
        {
            MD5CryptoServiceProvider MD5;
            Byte [] byteValue;
            Byte [] byteHash;
            string encriptPW = "";
            int i;

            MD5 = new MD5CryptoServiceProvider();
            byteValue = System.Text.Encoding.UTF8.GetBytes(pw);
            byteHash = MD5.ComputeHash(byteValue);
            MD5.Clear();
            for (i = 0; i < byteHash.Length; i++)
            {
                encriptPW += byteHash[i].ToString("x").PadLeft(2, '0');
            }
            return encriptPW;
        }

        public Byte[] Encriptar(Byte[] input)
        {
            return transform(input, m_des.CreateEncryptor(m_key, m_iv));
        }

        public Byte[] Desencriptar(Byte[] input)
        {
            return transform(input, m_des.CreateDecryptor(m_key, m_iv));
        }

        public string Encriptar3D(string str)
        {
            Byte[] input = m_utf8.GetBytes(str);
            Byte[] output = transform(input, m_des.CreateEncryptor(m_key, m_iv));
            return Convert.ToBase64String(output);
        }

        public string Desencriptar3D(string str)
        {
            if (str != null)
            {
                try
                {
                    Byte[] input = Convert.FromBase64String(str);
                    Byte[] output = transform(input, m_des.CreateDecryptor(m_key, m_iv));
                    return m_utf8.GetString(output);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        public byte[] transform(byte[] input, ICryptoTransform CryptoTransformar)
        {
            using (var memory = new MemoryStream())
            {
                using (var stream = new CryptoStream(memory, CryptoTransformar, CryptoStreamMode.Write))
                {
                    stream.Write(input, 0, input.Length);
                    stream.FlushFinalBlock();
                    memory.Position = 0;
                    var result = new byte[memory.Length];
                    memory.Read(result, 0, result.Length);
                    return result;
                }
            }
        }

        public Boolean RestaurarCopiaDeSeguridad(string nombreBU, string pathFilesBU, int volumen)
        {
            string query = "use [Master] ALTER DATABASE [Distar] SET SINGLE_USER WITH ROLLBACK IMMEDIATE RESTORE DATABASE [Distar] FROM ";
            string splits = "DISK='" + pathFilesBU + @"\" + nombreBU;
            for (int i = 1; i <= volumen; i++)
            {
                if (i == 1) {
                    query += splits + i + ".bak', ";
                } else {
                    query += splits + i + ".bak', ";
                }
            }
            query = query.Substring(0, query.Length-2);
            query += " With REPLACE";
            try
            {
                return Distar_BD.ExecuteNonquerySec(query) != 99;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Services RestaurarCopiaDeSeguridad: " + ex.Message);
                return false;
            }
        }

        public Boolean ProbarConnectionString(string user, string pass, string nombre, string servidor, bool seguridadIntegrada)
        {
            string conncectionString = "Data Source=" + servidor + ";Initial Catalog=" + nombre + ";";
            if (seguridadIntegrada)
                conncectionString += "Integrated Security=True;";
            else
                conncectionString += "User ID=" + user + ";Password=" + pass+";";
            return Distar_BD.ProbarConexion(conncectionString);
        }


        public Boolean ProbarConnectionString(string str)
        {
            return Distar_BD.ProbarConexion(str);
        }
    }
}
