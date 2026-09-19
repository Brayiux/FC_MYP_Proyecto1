using Model.Exceptions;

namespace Model.Entities
{
    /// <summary>
    /// Provee métodos para validar un texto dentro del chat.
    /// </summary>
    public static class TextRoomRules
    {
        /// <summary>
        /// Indica la longitud máxima de caracteres permitidos por texto/mensaje.
        /// </summary>
        public const int MaxLength = 1000;

        /// <summary>
        /// Valida que un texto tenga una longitud, valor y formato válidos
        /// para ser enviado dentro de la aplicación.
        /// </summary>
        /// <param name="text"></param>
        /// <exception cref="InvalidTextException"></exception>
        public static void ValidateText(string text)
        {
            if (text == null || text.Length is 0 or > MaxLength)
            {
                throw new InvalidTextException(
                    $"Error: El texto sale del rango de 1 a {MaxLength} caracteres.");
            }
        }
    }
}
