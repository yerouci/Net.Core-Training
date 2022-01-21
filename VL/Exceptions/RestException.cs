using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace VL.Exceptions
{
    public class RestException : Exception
    {
        public HttpStatusCode Code { get; set; }
        public object Errors { get; set; }

        public RestException(HttpStatusCode statusCode, object errors = null)
        {
            Code = statusCode;
            Errors = errors;
        }
    }
}
