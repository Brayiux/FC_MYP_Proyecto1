using Controller.Definitions.Interfaces;

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
            throw new NotImplementedException();
        }

        public byte[] Encode(string data)
        {
            throw new NotImplementedException();
        }
    }
}
