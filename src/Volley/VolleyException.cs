using System;

namespace Volley
{
    /// <summary>
    /// Exception thrown when a Volley API request fails.
    /// </summary>
    public class VolleyException : Exception
    {
        /// <summary>
        /// HTTP status code of the failed request.
        /// </summary>
        public int StatusCode { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VolleyException"/> class.
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="statusCode">HTTP status code</param>
        public VolleyException(string message, int statusCode = 0) : base(message)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VolleyException"/> class.
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="statusCode">HTTP status code</param>
        /// <param name="innerException">Inner exception</param>
        public VolleyException(string message, int statusCode, Exception innerException) 
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Checks if the exception represents an unauthorized error (401).
        /// </summary>
        public bool IsUnauthorized() => StatusCode == 401;

        /// <summary>
        /// Checks if the exception represents a forbidden error (403).
        /// </summary>
        public bool IsForbidden() => StatusCode == 403;

        /// <summary>
        /// Checks if the exception represents a not found error (404).
        /// </summary>
        public bool IsNotFound() => StatusCode == 404;

        /// <summary>
        /// Checks if the exception represents a rate limit error (429).
        /// </summary>
        public bool IsRateLimited() => StatusCode == 429;

        /// <summary>
        /// Checks if the exception represents a server error (5xx).
        /// </summary>
        public bool IsServerError() => StatusCode >= 500 && StatusCode < 600;
    }
}

