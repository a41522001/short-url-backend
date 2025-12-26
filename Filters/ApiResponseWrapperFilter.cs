using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShortUrlPJ.Models;

namespace ShortUrlPJ.Filters;

public class ApiResponseWrapperFilter : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is ObjectResult objectResult)
        {
            // 已經是 ApiResponse 就不要再包一次，保留 Controller 的自訂 message / code
            if (objectResult.Value is IApiResponse)
            {
                await next();
                return;
            }
            // 其他正常回傳物件 → 統一包成 ApiResponse
            var statusCode = objectResult.StatusCode ?? StatusCodes.Status200OK;
            var wrapped = new ApiResponse<object?>(
                data: objectResult.Value,
                message: "成功",
                code: statusCode
            );

            context.Result = new ObjectResult(wrapped)
            {
                StatusCode = statusCode
            };
        }
        await next();
    }
}

