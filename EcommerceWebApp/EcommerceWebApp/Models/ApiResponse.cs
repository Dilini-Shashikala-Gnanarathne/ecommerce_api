
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; } // Allow null messages
        public T? Data { get; set; } // Allow null data

        public ApiResponse(bool success, string? message, T? data = default)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static ApiResponse<T> SuccessResponse(string message, T? data = default)
        {
            return new ApiResponse<T>(true, message, data);
        }

        public static ApiResponse<T> ErrorResponse(string message, T? data = default)
        {
            return new ApiResponse<T>(false, message, data);
        }

        internal static object? SuccessResponse(string v, Task<LoginResponse?> response)
        {
            throw new NotImplementedException();
        }

        internal static object? SuccessResponse(string v, Task<RegisterResponse?> response)
        {
            throw new NotImplementedException();
        }
    }
