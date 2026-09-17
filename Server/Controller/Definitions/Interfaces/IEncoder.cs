namespace Controller.Definitions.Interfaces
{
    /// <summary>
    /// Provee métodos para codificar y decodificar un arreglo de
    /// bytes en cualquier tipo de dato que su implementación
    /// interprete de la información recibida.
    /// </summary>
    /// <typeparam name="T">Tipo de dato decodificado.</typeparam>
    public interface IEncoder<T>
    {
        /// <summary>
        /// Codifica un dato en un arreglo de bytes.
        /// </summary>
        /// <param name="data">Información a codificar.</param>
        /// <returns></returns>
        byte[] Encode(T data);
        /// <summary>
        /// Decodifica un arreglo de bytes.
        /// </summary>
        /// <param name="data">Información a decodificar.</param>
        /// <returns></returns>
        T Decode(byte[] data);
    }
}
