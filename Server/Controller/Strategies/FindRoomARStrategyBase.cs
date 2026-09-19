using Controller.Definitions.Abstracts;
using Controller.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace Controller.Strategies
{
    public abstract class FindRoomARStrategyBase : ARStrategyBase
    {
        /// <summary>
        /// Nombre de la sala que se desea encontrar.
        /// </summary>
        protected readonly string _roomname;

        /// <summary>
        /// El tipo de operación que la clase hija hace.
        /// </summary>
        protected readonly string _operation;

        protected FindRoomARStrategyBase(string roomname, string operation)
        {
            _roomname = roomname;
            _operation = operation;
        }

        /// <summary>
        /// Le responde al <paramref name="client"/> que la sala con el nombre
        /// indicado no existe.
        /// </summary>
        /// <param name="client">Cliente al que se le responde.</param>
        /// <param name="operation">Tipo de operación realizada</param>
        /// <returns></returns>
        protected async Task ReplyNoSuchRoomAsync(ClientConnection client)
        {
            MsgBuilder mb = new();
            string response = mb.WithType("RESPONSE")
                                .WithOperation(_operation)
                                .WithResult("NO_SUCH_ROOM")
                                .WithExtra(_roomname)
                                .Build();
            await SendMessageAsync(client, response);
        }
    }
}
