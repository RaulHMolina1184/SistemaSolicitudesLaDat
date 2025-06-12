using SistemaSolicitudesLaDat.Service.Abstract;
using System.Security.Cryptography;
using System.Text;

namespace SistemaSolicitudesLaDat.Service.Encriptado
{
    public class EncriptadoService : IEncriptadoService
    {
        private readonly ISeguridadService _seguridadService;
        private readonly byte[] _key;
        private const int NonceSize = 12; // Tamaño estándar para nonce en GCM
        private const int TagSize = 16;   // Tamaño estándar para etiqueta de autenticación (128 bits)

        public EncriptadoService(ISeguridadService seguridadService)
        {
            _seguridadService = seguridadService;
            _key = _seguridadService.GetLlaveEncriptacion(); // Inyección de la llave de encriptación desde el servicio de seguridad
        }

        // Encriptar texto plano (password) al guardar en base de datos 
        public (byte[] ciphertext, byte[] tag, byte[] nonce) Encriptar(byte[] plaintext)
        {
            byte[] nonce = new byte[NonceSize];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(nonce);

            byte[] ciphertext = new byte[plaintext.Length];
            byte[] tag = new byte[TagSize];

            using var aesGcm = new AesGcm(_key);
            aesGcm.Encrypt(nonce, plaintext, ciphertext, tag);

            return (ciphertext, tag, nonce);
        }

        // Encriptar inputContrasenia para inicio de sesión
        public (byte[] ciphertext, byte[] tag, byte[] nonce) EncriptarInput(byte[] plaintext)
        {
            byte[] nonce = new byte[NonceSize];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(nonce);

            byte[] ciphertext = new byte[plaintext.Length];
            byte[] tag = new byte[TagSize];

            using var aesGcm = new AesGcm(_key);
            aesGcm.Encrypt(nonce, plaintext, ciphertext, tag);

            return (ciphertext, tag, nonce);
        }

        // Desencriptar (no se necesita en teoria)
        public string Desencriptar(byte[] ciphertext, byte[] tag, byte[] nonce)
        {
            byte[] plaintext = new byte[ciphertext.Length];

            using var aesGcm = new AesGcm(_key);
            aesGcm.Decrypt(nonce, ciphertext, tag, plaintext);

            return Encoding.UTF8.GetString(plaintext);
        }

        // Verificación de contraseña
        public bool VerificarContrasenia(byte[] contraseniaCifrada, byte[] tagDb, byte[] nonceDb, string inputContrasenia)
        {
            try
            {
                byte[] inputTextoPlano = Encoding.UTF8.GetBytes(inputContrasenia);
                byte[] inputTextoCifrado = new byte[inputTextoPlano.Length];
                byte[] inputTag = new byte[16]; // Tamaño estándar del tag en AES-GCM

                using var aesGcm = new AesGcm(_key);
                aesGcm.Encrypt(nonceDb, inputTextoPlano, inputTextoCifrado, inputTag);

                // Comparar ciphertext y tag
                return CryptographicOperations.FixedTimeEquals(contraseniaCifrada, inputTextoCifrado) &&
                       CryptographicOperations.FixedTimeEquals(tagDb, inputTag);
            }
            catch
            {
                return false;
            }
        }
    }
}
