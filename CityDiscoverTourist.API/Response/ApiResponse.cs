using Microsoft.AspNetCore.Mvc;

namespace CityDiscoverTourist.API.Response;

/// <summary>
/// </summary>
/// <typeparam name="T"></typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// </summary>
    /// <param name="message"></param>
    /// <param name="data"></param>
    /// <param name="status"></param>
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

    public static ApiResponse<TData> Error<TData>(ProblemDetails message)
    {
        return new ApiResponse<TData>(message.Title, default, null, message.Status.ToString());
    }

    public string Message { get; }
    public T Data { get; }
    public object Pagination { get; }
    public string Status { get; }

    public static ApiResponse<TData> Ok<TData>(TData data) where TData : class
    {
        return new ApiResponse<TData>("", data, "success");
    }

    public static ApiResponse<TData> Success<TData>(TData data, object paging) where TData : class
    {
        return new ApiResponse<TData>("", data, paging, "success");
    }

    public static ApiResponse<TData> Created<TData>(TData data) where TData : class
    {
        return new ApiResponse<TData>("", data, "data modified");
    }
}