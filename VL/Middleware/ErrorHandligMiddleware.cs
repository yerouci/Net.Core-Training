﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using VL.Exceptions;

namespace VL.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IOptions<ErrorDefinition> _errorSettings;
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger, IOptions<ErrorDefinition> errorSettings)
        {
            _logger = logger;
            _next = next;
            _errorSettings = errorSettings;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _logger);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex, ILogger<ErrorHandlingMiddleware> logger)
        {            
            object errors = null;

            switch (ex)
            {
                //For Custom Status Code Error
                case RestException re:
                    logger.LogError(ex, $"REST ERROR: {(ex as RestException).Errors}");
                    errors = re.Errors;
                    context.Response.StatusCode = (int)re.Code;
                    break;
                //Code 500 as requested
                case Exception e:
                    logger.LogError(ex, "SERVER ERROR");
                    errors = string.IsNullOrWhiteSpace(e.Message) ? "Error" : e.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.ContentType = "application/json";
            if (errors != null)
            {
                var result = JsonConvert.SerializeObject(new
                {
                    _errorSettings.Value.Message,
                    errors
                });

                await context.Response.WriteAsync(result);
            }
        }
    }
}
