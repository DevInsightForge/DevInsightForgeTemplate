using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DevInsightForge.WebAPI.Extensions;

public static class ExceptionHandlerAppExtension
{
    public static void UseAppExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.ContentType = "application/json";
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                ProblemDetails problemDetails = new()
                {
                    Instance = context.Request.Path
                };

                switch (contextFeature?.Error)
                {
                    //case BadRequestException exception:
                    //    problemDetails.Status = StatusCodes.Status400BadRequest;
                    //    problemDetails.Title = "Bad Request";
                    //    problemDetails.Detail = exception.Message.ToString();
                    //    break;

                    //case NotFoundException exception:
                    //    problemDetails.Status = StatusCodes.Status404NotFound;
                    //    problemDetails.Title = "Not Found";
                    //    problemDetails.Detail = exception.Message.ToString();
                    //    break;

                    case FluentValidation.ValidationException exception:
                        problemDetails.Status = StatusCodes.Status400BadRequest;
                        problemDetails.Title = "Validation Failed";
                        problemDetails.Detail = "One or more validation errors occurred.";

                        problemDetails.Extensions["errors"] = exception.Errors
                            .GroupBy(e => e.PropertyName)
                            .ToDictionary(
                                group => JsonNamingPolicy.CamelCase.ConvertName(group.Key) ?? group.Key,
                                group => group.Select(e => e.ErrorMessage).ToArray());
                        break;

                    default:
                        problemDetails.Status = StatusCodes.Status500InternalServerError;
                        problemDetails.Title = "Internal Server Error";
                        break;
                }

                context.Response.StatusCode = problemDetails.Status.Value;
                await context.Response.WriteAsJsonAsync(problemDetails);
            });
        });
    }
}
