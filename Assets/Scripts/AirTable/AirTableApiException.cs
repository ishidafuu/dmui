using System;
using System.Net;

namespace DM
{
    public abstract class AirTableApiException : Exception
    {
        protected AirTableApiException(HttpStatusCode errorCode, string errorName, string errorMessage) : base(
            $"{errorName} - {errorCode}: {errorMessage}")
        {
            ErrorCode = errorCode;
            ErrorName = errorName;
            ErrorMessage = errorMessage;
        }

        public HttpStatusCode ErrorCode { get; }
        public string ErrorName { get; }
        public string ErrorMessage { get; }
    }

    public class AirTableUnrecognizedException : AirTableApiException
    {
        public AirTableUnrecognizedException(HttpStatusCode statusCode) : base(statusCode, "Unrecognized Error",
            $"AirTable returned HTTP status code {statusCode}") { }
    }

    public class AirTableBadRequestException : AirTableApiException
    {
        public AirTableBadRequestException() : base(HttpStatusCode.BadRequest, "Bad Request",
            "The request encoding is invalid; the request can't be parsed as a valid JSON.") { }
    }

    public class AirTableUnauthorizedException : AirTableApiException
    {
        public AirTableUnauthorizedException() : base(HttpStatusCode.Unauthorized, "Unauthorized",
            "Accessing a protected resource without authorization or with invalid credentials.") { }
    }

    public class AirTablePaymentRequiredException : AirTableApiException
    {
        public AirTablePaymentRequiredException() : base(
            HttpStatusCode.PaymentRequired,
            "Payment Required",
            "The account associated with the API key making requests hits a quota that can be increased by upgrading the AirTable account plan.") { }
    }

    public class AirTableForbiddenException : AirTableApiException
    {
        public AirTableForbiddenException() : base(
            HttpStatusCode.Forbidden,
            "Forbidden",
            "Accessing a protected resource with API credentials that don't have access to that resource.") { }
    }

    public class AirTableNotFoundException : AirTableApiException
    {
        public AirTableNotFoundException() : base(
            HttpStatusCode.NotFound,
            "Not Found",
            "Route or resource is not found. This error is returned when the request hits an undefined route, or if the resource doesn't exist (e.g. has been deleted).") { }
    }

    public class AirTableRequestEntityTooLargeException : AirTableApiException
    {
        public AirTableRequestEntityTooLargeException() : base(
            HttpStatusCode.RequestEntityTooLarge,
            "Request Entity Too Large",
            "The request exceeded the maximum allowed payload size. You shouldn't encounter this under normal use.") { }
    }

    public class AirTableInvalidRequestException : AirTableApiException
    {
        public string DetailedErrorMessage { get; }

        public AirTableInvalidRequestException(string errorMessage = null) : base(
            (HttpStatusCode)422,
            "Invalid Request",
            "The request data is invalid. This includes most of the base-specific validations. The DetailedErrorMessage property contains the detailed error message string.")
        {
            DetailedErrorMessage = errorMessage;
        }
    }

    public class AirTableTooManyRequestsException : AirTableApiException
    {
        public AirTableTooManyRequestsException() : base(
            (HttpStatusCode)429,
            "Too Many Requests",
            "The user has sent too many requests in a given amount of time (rate limiting).") { }
    }
}