using System;

namespace VocabList.Service.Exceptions
{
    public class UserCreationException : Exception
    {
        public string StatusCode { get; }

        public UserCreationException(string message, string statusCode)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
