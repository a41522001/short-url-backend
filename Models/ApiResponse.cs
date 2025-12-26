namespace ShortUrlPJ.Models;
public interface IApiResponse { }
public class ApiResponse<T> : IApiResponse
{
    private static readonly TimeZoneInfo TaipeiTimeZone =
        TimeZoneInfo.FindSystemTimeZoneById("Asia/Taipei");
    public T? Data { get; set; }
    public string Message { get; set; } = "成功";
    public int Code { get; set; } = 200;
    public string Time { get; set; } = TimeZoneInfo
        .ConvertTimeFromUtc(DateTime.UtcNow, TaipeiTimeZone)
        .ToString("yyyy-MM-dd HH:mm:ss");

    public ApiResponse(T? data, string? message = "成功", int? code = 200) {
        Data = data;
        Message = message ?? "成功";
        Code = code ?? 200;
    }
}
public class ApiResponse : ApiResponse<object>
{
    public ApiResponse(string message = "成功", int statusCode = 200) : base(null, message, statusCode) { }
}
