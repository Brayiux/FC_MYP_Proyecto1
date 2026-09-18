using Controller.Definitions.Interfaces;
using System.Text;

namespace Controller.Resources
{
    /// <summary>
    /// Codifica y decodifica un arreglo de bytes en una cadena
    /// de caracteres (texto) bajo el formato UTF-8
    /// </summary>
    internal class EncoderUTF8 : IEncoder<string>
    {
        public string Decode(byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }

        public byte[] Encode(string data)
        {
            return Encoding.UTF8.GetBytes(data);
        }
    }
}
