using Newtonsoft.Json;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace EnergyDistribution.Client.Services
{
    public class Crypto
    {

        public static string EncriptarObjeto<T>(T objeto, string constraseña)
        {
            byte[] claveBytes = Encoding.UTF8.GetBytes(constraseña);
            KeyParameter clave = ParameterUtilities.CreateKeyParameter("AES", claveBytes);

            // Serializa el objeto a JSON
            string objetoSerializado = JsonConvert.SerializeObject(objeto);
            byte[] objetoBytes = Encoding.UTF8.GetBytes(objetoSerializado);

            // Configura el motor de encriptación
            IBufferedCipher cipher = CipherUtilities.GetCipher("AES/CFB/PKCS7Padding");
            cipher.Init(true, clave);

            // Encripta los datos del objeto
            byte[] datosEncriptados = cipher.DoFinal(objetoBytes);

            // Devuelve los datos encriptados como una cadena Base64
            return Convert.ToBase64String(datosEncriptados);
        }


        public static T DesencriptarObjeto<T>(string datosEncriptados, string constraseña)
        {
            byte[] claveBytes = Encoding.UTF8.GetBytes(constraseña);
            KeyParameter clave = ParameterUtilities.CreateKeyParameter("AES", claveBytes);

            // Convierte la cadena Base64 de datos encriptados a bytes
            byte[] datosEncriptadosBytes = Convert.FromBase64String(datosEncriptados);

            // Configura el motor de desencriptación
            IBufferedCipher cipher = CipherUtilities.GetCipher("AES/CFB/PKCS7Padding");
            cipher.Init(false, clave);

            // Desencripta los datos
            byte[] datosDesencriptados = cipher.DoFinal(datosEncriptadosBytes);
            string objetoSerializado = Encoding.UTF8.GetString(datosDesencriptados);

            // Deserializa el objeto a su tipo original
            return JsonConvert.DeserializeObject<T>(objetoSerializado)!;
        }


    }
}
