using System.Net;

namespace Intranet.Api.HttpClient.Exceptions
{
    public class ApiHttpClientException : Exception
    {
        public ApiHttpClientException(string message) : base(message) { }

        public ApiHttpClientException(string message, Exception innerException) : base(message, innerException) { }
    }
}
