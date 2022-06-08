using AdventureWorks.Api.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdventureWorks.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var errorResponse = new ErrorResponse();
            context.Response.ContentType = MediaTypeNames.Application.Json;
            var message = string.Empty;

            switch (exception.GetType().Name)
            {
                case nameof(ValidationException):
                    {
                        // Consistent validation error response
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        var validationException = (ValidationException)exception;
                        foreach (var error in validationException.Errors)
                        {
                            errorResponse.Errors.Add(new ErrorModel
                            {
                                FieldName = error.PropertyName,
                                Message = error.ErrorMessage
                            });
                        }
                        break;
                    }

                default:
                    {
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        errorResponse.Errors.Add(new ErrorModel
                        {
                            FieldName = "$",
                            Message = exception.Message
                        });
                        break;
                    }
            }

            message = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(message);
        }
    }
}