using Client.Models.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models.Definitions
{
    /// <summary>
    /// Maneja los eventos de mensaje recibidos por la conexión.
    /// </summary>
    /// <param name="msgData"></param>
    public delegate void MsgReceivedEventHandler(MsgData msgData);
}
