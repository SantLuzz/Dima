using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dima.Core.Responses
{
    public class Response<TData>
    {
        
        private readonly int _code;
        
        public TData? Data { get; set; }
        public string? Message { get; set; } = string.Empty;
        [JsonIgnore]
        public bool IsSuccess => _code is >= 200 and <= 299;

        [JsonConstructor]
        public Response()
            => _code = Configuration.DefaultStatusCode;

        public Response(TData? data,
            int code = Configuration.DefaultStatusCode, 
            string? message = null)
        {
            Data = data;
            Message = message;
            _code = code;
        }
    }
}
