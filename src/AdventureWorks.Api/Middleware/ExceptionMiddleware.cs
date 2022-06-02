using AdventureWorks.Api.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
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
            context.Response.ContentType = MediaTypeNames.Application.Json; ; 
            var message = string.Empty;

            switch (exception.GetType().Name)
            {
                case nameof(ValidationException):
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    var validationException = (ValidationException)exception;
                    var errorResponse = new ErrorResponse();
                    foreach (var error in validationException.Errors)
                    {
                        errorResponse.Errors.Add(new ErrorModel
                        {
                            FieldName = error.PropertyName,
                            Message = error.ErrorMessage
                        });
                    }
                    message = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                    break;

                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    message = "Internal Server Error.";
                    break;
            }

            await context.Response.WriteAsync(message);
        }
    }
}