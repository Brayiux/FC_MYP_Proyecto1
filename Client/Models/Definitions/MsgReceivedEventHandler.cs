using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models.Definitions
{
    /// <summary>
    /// Maneja los eventos de mensaje recibidos por la conexión.
    /// </summary>
    /// <param name="msg"></param>
    public delegate void MsgReceivedEventHandler(string msg);
}
