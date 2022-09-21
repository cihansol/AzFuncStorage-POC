using System;
using System.Runtime.Serialization;


namespace AzFuncStorage_POC
{
    public class OpenWeatherException : Exception
    {

        public int Cod { get; private set; }


        public OpenWeatherException(string message) : base(message)
        {
        }

        public OpenWeatherException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public OpenWeatherException(int cod, string message) : base(message)
        {
            Cod = cod;
        }

        protected OpenWeatherException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

}
