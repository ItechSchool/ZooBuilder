using System;
using System.Text;

namespace SharedNetwork
{
    public class MessageBuilder
    {
        private string _invocationType = null!;
        private string _methodName = null!;
        private object[] _parameter = null!;
    
        private MessageBuilder() {}

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
            string parameters = string.Join(":", _parameter);
            return $"{_invocationType}/{_methodName}:{parameters}";
        }
    }
}
