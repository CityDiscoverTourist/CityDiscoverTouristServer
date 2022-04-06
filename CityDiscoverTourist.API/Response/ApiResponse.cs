namespace CityDiscoverTourist.API.Response;

    public class ApiResponse<T>
    {
        public ApiResponse(string message, T data, string status)
        {
            Message = message;
            Data = data;
            Status = status;
        }

        private ApiResponse(string message, T data, object paging, string status)
        {
            Message = message;
            Data = data;
            Pagination = paging;
            Status = status;
        }

        public string Message { get; }
        public T Data { get; }
        public object Pagination { get; }
        public string Status { get; }

        public static ApiResponse<TData> Ok<TData>(TData data) where TData : class
        {
            return new ApiResponse<TData>("", data, "success");
        }

        public static ApiResponse<TData> Ok2<TData>(TData data, object paging) where TData : class
        {
            return new ApiResponse<TData>("", data, paging, "success");
        }

        public static ApiResponse<TData> Created<TData>(TData data) where TData : class
        {
            return new ApiResponse<TData>("", data, "success");
        }

    }