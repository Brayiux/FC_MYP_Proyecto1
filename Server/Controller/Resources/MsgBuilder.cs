using Model.Definitions.Enums;

namespace Controller.Resources
{
    /// <summary>
    /// Permite construir cualquier mensaje que se especifica
    /// en el protocolo, para responder a clientes.
    /// </summary>
    public class MsgBuilder
    {
        /// <summary>
        /// Mensaje que se construye a través de este <i>builder</i>.
        /// </summary>
        private string _msg;

        /// <summary>
        /// Indica si el mensaje está vacío.
        /// </summary>
        private bool _isEmpty;

        #region Construcción

        public MsgBuilder()
        {
            throw new NotImplementedException();
        }

        public MsgBuilder WithType(string type)
        {
            throw new NotImplementedException();
        }

        public MsgBuilder WithOperation(string op)
        {
            throw new NotImplementedException();
        }

        public MsgBuilder WithResult(string result)
        {
            throw new NotImplementedException();
        }

        public MsgBuilder WithExtra(string extra)
        {
            throw new NotImplementedException();
        }

        public MsgBuilder WithUsername(string username)
        {
            throw new NotImplementedException();
        }

        public MsgBuilder WithStatus(UserStatus status)
        {
            throw new NotImplementedException();
        }

        public MsgBuilder WithUsers(Dictionary<string, UserStatus> users)
        {
            throw new NotImplementedException();
        }

        public MsgBuilder WithText(string text)
        {
            throw new NotImplementedException();
        }

        public MsgBuilder WithRoomname(string rn)
        {
            throw new NotImplementedException();
        }
        public string Build()
        {
            throw new NotImplementedException();
        }
        public void Reset()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
