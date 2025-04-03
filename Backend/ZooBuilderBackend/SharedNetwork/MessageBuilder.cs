using System;
using System.Text;

namespace SharedNetwork
{
    public class MessageBuilder
    {
        private string _invocationType;
        private string _methodName;
        private object[] _parameter;

        private MessageBuilder() { }

        public static MessageBuilder Call(string methodName)
        {
            var builder = new MessageBuilder
            {
                _invocationType = "CALL",
                _methodName = methodName
            };
            return builder;
        }

        public MessageBuilder AddParameter(params object[] parameter)
        {
            _parameter = parameter;
            return this;
        }

        public string Build()
        {
            string parameters = _parameter != null ? string.Join(":", _parameter) : string.Empty;
            return $"{_invocationType}/{_methodName}:{parameters}\\s";
        }
    }
}
