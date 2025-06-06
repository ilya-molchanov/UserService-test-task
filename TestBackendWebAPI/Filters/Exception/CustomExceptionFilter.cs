using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using BackendTest.Internal.Exceptions;
using BackendTest.Internal.Exceptions.Models;

namespace BackendTest.WebApi.Filters.Exception
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IModelMetadataProvider _modelMetadataProvider;

        public CustomExceptionFilter(
            IWebHostEnvironment hostingEnvironment,
            IModelMetadataProvider modelMetadataProvider)
        {
            _hostingEnvironment = hostingEnvironment;
            _modelMetadataProvider = modelMetadataProvider;
        }

        public void OnException(ExceptionContext context)
        {
            if (!_hostingEnvironment.IsDevelopment())
            {
                return;
            }

            // My web API client will throw a WebApiException if it doesn't produce a successful response
            if (context.Exception is InternalApiBusinessException webApiEx)
            {
                HttpStatusCode? statusCode = null;

                switch (webApiEx.InternalApiBusinessErrorCode)
                {
                    case InternalApiErrorCodes.ItemNotFound:
                        statusCode = HttpStatusCode.NotFound;
                        break;
                    case InternalApiErrorCodes.CannotUpdate:
                        statusCode = HttpStatusCode.UnprocessableContent;
                        break;
                    case InternalApiErrorCodes.EmptyName:
                    case InternalApiErrorCodes.EmptyEmail:
                    case InternalApiErrorCodes.EmptyPassword:
                    case InternalApiErrorCodes.InvalidEmail:
                    case InternalApiErrorCodes.BadRequest:
                        statusCode = HttpStatusCode.BadRequest;
                        break;
                    case InternalApiErrorCodes.UserWithGivenEmailAlreadyExists:
                        statusCode = HttpStatusCode.Conflict;
                        break;
                    default:
                        statusCode = HttpStatusCode.InternalServerError;
                        break;
                }

                var error = new ErrorModel
                (
                    typeof(InternalApiBusinessException).ToString(),
                    webApiEx.InternalApiBusinessErrorCode.ToString(),
                    webApiEx.Content
                );

                context.Result = new JsonResult(error)
                {
                    StatusCode = (int)statusCode
                };
            }
        }
    }
}
