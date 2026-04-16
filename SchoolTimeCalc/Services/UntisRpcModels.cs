using System.Text.Json.Serialization;

namespace SchoolTimeCalc.Services
{
    public class UntisRpcRequest
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = "1";

        [JsonPropertyName("method")]
        public string Method { get; set; } = string.Empty;

        [JsonPropertyName("params")]
        public object? Params { get; set; }

        [JsonPropertyName("jsonrpc")]
        public string JsonRpc { get; set; } = "2.0";
    }

    public class UntisAuthRequest
    {
        [JsonPropertyName("user")]
        public string User { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;

        [JsonPropertyName("client")]
        public string Client { get; set; } = "SchoolTimeCalc";
    }

    public class UntisAuthResponse
    {
        [JsonPropertyName("sessionId")]
        public string SessionId { get; set; } = string.Empty;
        
        [JsonPropertyName("personType")]
        public int PersonType { get; set; }

        [JsonPropertyName("personId")]
        public int PersonId { get; set; }

        [JsonPropertyName("classId")]
        public int ClassId { get; set; }
    }

    public class UntisRpcResponse<T>
    {
        [JsonPropertyName("jsonrpc")]
        public string JsonRpc { get; set; } = string.Empty;

        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("result")]
        public T? Result { get; set; }

        [JsonPropertyName("error")]
        public UntisRpcError? Error { get; set; }
    }

    public class UntisRpcError
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }

    public class UntisHolidayDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("longName")]
        public string LongName { get; set; } = string.Empty;

        [JsonPropertyName("startDate")]
        public int StartDate { get; set; } // yyyyMMdd format

        [JsonPropertyName("endDate")]
        public int EndDate { get; set; } // yyyyMMdd format
    }
}
