using System.Net;

namespace Intranet.Api.HttpClient.Exceptions
{
    public class ApiHttClientWithStatusCodeException : ApiHttpClientException
    {
        //public ValidationProblemDetails ProblemDetails { get; }

        public HttpStatusCode StatusCode { get; }

        public ApiHttClientWithStatusCodeException(string message) : base(message)
        {

        }

        public ApiHttClientWithStatusCodeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
