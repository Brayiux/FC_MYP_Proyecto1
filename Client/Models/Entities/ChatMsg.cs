using System;

namespace Client.Models.Entities
{
    /// <summary>
    /// Contiene los datos de mensajes enviados por el chat.
    /// </summary>
    public class ChatMsg
    {
        #region Propiedades
        /// <summary>
        /// Obtiene al usuario que envía el mensaje.
        /// </summary>
        public ChatUser Sender { get; }

        /// <summary>
        /// Obtiene el contenido del mensaje.
        /// </summary>
        public string Text { get; }
        #endregion


        #region Construcción

        /// <summary>
        /// Construye un nuevo mensaje con un emisor y un contenido
        /// (texto)
        /// </summary>
        /// <param name="sender">Quien envía el mensaje.</param>
        /// <param name="text">El contenido del mensaje.</param>
        public ChatMsg(ChatUser sender, string text)
        {
            ValidateSender(sender);
            ValidateText(text);

            Sender = sender;
            Text = text;
        }

        #endregion

        #region Apoyo

        /// <summary>
        /// Comprueba que el emisor sea válido.
        /// </summary>
        /// <param name="sender">El emisor.</param>
        private void ValidateSender(ChatUser sender)
        {
            ArgumentNullException.ThrowIfNull(sender);
        }

        /// <summary>
        /// Comprueba que el texto sea válido.
        /// </summary>
        /// <param name="text">El texto</param>
        /// <exception cref="ArgumentException">Si la cadena
        /// <paramref name="text"/> es nula o vacía.</exception>
        private void ValidateText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException(nameof(text),
                    "Error: El texto no puede ser vacío.");
            }
        }

        #endregion
    }
}
