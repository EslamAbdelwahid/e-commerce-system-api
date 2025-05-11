namespace Store.Es.APIs.Errors
{
    public class ApiErrorResponse
    {
        public string? Message { get; set; }
        public int StatusCode { get; set; }
        public ApiErrorResponse(int statusCode, string? message = null)
        {
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
            StatusCode = statusCode;
        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            var message = statusCode switch
            {
                400 => "You Have Made A Bad Request",
                401 => "You are not Authorized",
                404 => "Resourse was not found",
                500 => "Server Error",
                _ => null
            };
            return message;
        }
    }
}
