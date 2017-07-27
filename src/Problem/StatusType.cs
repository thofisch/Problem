using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using Newtonsoft.Json;

namespace Trifork
{
    /// <summary>
    ///     The {@link Status} class in JAX-RS doesn't list of official HTTP status codes. The purpose
    ///     of this class is to provide easy access to the missing ones.
    ///     @see Status
    /// </summary>
    [Serializable]
    [DebuggerDisplay("<{StatusCode} ({ReasonPhrase,nq})>")]
    public class StatusType : IComparable<StatusType>, IEquatable<StatusType>
    {
        private static readonly string[][] HttpStatusDescriptions =
        {
            null,
            // 100
            new[]
            {
                "Continue",
                "Switching Protocols",
                "Processing"
            },
            // 200
            new[]
            {
                "OK",
                "Created",
                "Accepted",
                "Non-Authoritative Information",
                "No Content",
                "Reset Content",
                "Partial Content",
                "Multi-Status"
            },
            // 300
            new[]
            {
                "Multiple Choices",
                "Moved Permanently",
                "Found",
                "See Other",
                "Not Modified",
                "Use Proxy",
                null,
                "Temporary Redirect"
            },
            // 400
            new[]
            {
                "Bad Request",
                "Unauthorized",
                "Payment Required",
                "Forbidden",
                "Not Found",
                "Method Not Allowed",
                "Not Acceptable",
                "Proxy Authentication Required",
                "Request Timeout",
                "Conflict",
                "Gone",
                "Length Required",
                "Precondition Failed",
                "Request Entity Too Large",
                "Request-Uri Too Long",
                "Unsupported Media Type",
                "Requested Range Not Satisfiable",
                "Expectation Failed",
                null,
                null,
                null,
                null,
                "Unprocessable Entity",
                "Locked",
                "Failed Dependency",
                null,
                "Upgrade Required"
            },
            // 500
            new[]
            {
                "Internal Server Error",
                "Not Implemented",
                "Bad Gateway",
                "Service Unavailable",
                "Gateway Timeout",
                "Http Version Not Supported",
                null,
                "Insufficient Storage"
            }
        };

        public static readonly StatusType Ok = new StatusType(HttpStatusCode.OK, "OK");

        ///// <summary>
        // * @see <a href="http://tools.ietf.org/html/rfc7231#section-6.2.1">HTTP/1.1: Semantics and Content, section 6.2.1</a>
        // /// </summary>
        //CONTINUE(100, "Continue"),
        ///// <summary>
        // * @see <a href="http://tools.ietf.org/html/rfc7231#section-6.2.2">HTTP/1.1: Semantics and Content, section 6.2.2</a>
        // /// </summary>
        //SWITCHING_PROTOCOLS(101, "Switching Protocols"),
        ///// <summary>
        // * @see <a href="http://tools.ietf.org/html/rfc2518#section-10.1">WebDAV</a>
        // /// </summary>
        //PROCESSING(102, "Processing"),
        ///// <summary>
        // * @see <a href="http://code.google.com/p/gears/wiki/ResumableHttpRequestsProposal">A proposal for supporting
        // * resumable POST/PUT HTTP requests in HTTP/1.0</a>
        // /// </summary>
        //CHECKPOINT(103, "Checkpoint"),
        ///// <summary>
        // * @see <a href="http://tools.ietf.org/html/rfc7231#section-6.3.4">HTTP/1.1: Semantics and Content, section 6.3.4</a>
        // /// </summary>
        //NON_AUTHORITATIVE_INFORMATION(203, "Non-Authoritative Information"),
        ///// <summary>
        // * @see <a href="http://tools.ietf.org/html/rfc4918#section-13">WebDAV</a>
        // /// </summary>
        //MULTI_STATUS(207, "Multi-Status"),
        ///// <summary>
        // * @see <a href="http://tools.ietf.org/html/rfc5842#section-7.1">WebDAV Binding Extensions</a>
        // /// </summary>
        //ALREADY_REPORTED(208, "Already Reported"),
        ///// <summary>
        // * @see <a href="http://tools.ietf.org/html/rfc3229#section-10.4.1">Delta encoding in HTTP</a>
        // /// </summary>
        //IM_USED(226, "IM Used"),
        ///// <summary>
        // * @see <a href="http://tools.ietf.org/html/rfc7231#section-6.4.1">HTTP/1.1: Semantics and Content, section 6.4.1</a>
        // /// </summary>
        //MULTIPLE_CHOICES(300, "Multiple Choices"),
        ///// <summary>
        // * @see <a href="http://tools.ietf.org/html/rfc7238">RFC 7238</a>
        // /// </summary>
        //PERMANENT_REDIRECT(308, "Permanent Redirect"),
        ///// <summary>
        // * @see <a href="http://tools.ietf.org/html/rfc2324#section-2.3.2">HTCPCP/1.0</a>
        // /// </summary>
        //I_AM_A_TEAPOT(418, "I'm a teapot"),
        ///// <summary>
        // * @see <a href="http://tools.ietf.org/html/rfc4918#section-11.2">WebDAV</a>
        // /// </summary>
        //UNPROCESSABLE_ENTITY(422, "Unprocessable Entity"),
        ///// <summary>
        // * @see <a href="http://tools.ietf.org/html/rfc4918#section-11.3">WebDAV</a>
        // /// </summary>
        //LOCKED(423, "Locked"),
        ///// <summary>
        // * @see <a href="http://tools.ietf.org/html/rfc4918#section-11.4">WebDAV</a>
        // /// </summary>
        //FAILED_DEPENDENCY(424, "Failed Dependency"),
        ///// <summary>
        // * @see <a href="http://tools.ietf.org/html/rfc2817#section-6">Upgrading to TLS Within HTTP/1.1</a>
        // /// </summary>
        //UPGRADE_REQUIRED(426, "Upgrade Required"),
        ///// <summary>
        // * @see <a href="http://tools.ietf.org/html/rfc6585#section-3">Additional HTTP Status Codes</a>
        // /// </summary>
        //PRECONDITION_REQUIRED(428, "Precondition Required"),
        ///// <summary>
        // * @see <a href="http://tools.ietf.org/html/rfc6585#section-4">Additional HTTP Status Codes</a>
        // /// </summary>
        //TOO_MANY_REQUESTS(429, "Too Many Requests"),
        ///// <summary>
        // * @see <a href="http://tools.ietf.org/html/rfc6585#section-5">Additional HTTP Status Codes</a>
        // /// </summary>
        //REQUEST_HEADER_FIELDS_TOO_LARGE(431, "Request Header Fields Too Large"),
        ///// <summary>
        // * @see <a href="http://tools.ietf.org/html/rfc2295#section-8.1">Transparent Content Negotiation</a>
        // /// </summary>
        //VARIANT_ALSO_NEGOTIATES(506, "Variant Also Negotiates"),
        ///// <summary>
        // * @see <a href="http://tools.ietf.org/html/rfc4918#section-11.5">WebDAV</a>
        // /// </summary>
        //INSUFFICIENT_STORAGE(507, "Insufficient Storage"),
        ///// <summary>
        // * @see <a href="http://tools.ietf.org/html/rfc5842#section-7.2">WebDAV Binding Extensions</a>
        // /// </summary>
        //LOOP_DETECTED(508, "Loop Detected"),
        ///// <summary>
        // * {@code 509 Bandwidth Limit Exceeded}
        // /// </summary>
        //BANDWIDTH_LIMIT_EXCEEDED(509, "Bandwidth Limit Exceeded"),
        ///// <summary>
        // * @see <a href="http://tools.ietf.org/html/rfc2774#section-7">HTTP Extension Framework</a>
        // /// </summary>
        //NOT_EXTENDED(510, "Not Extended"),
        ///// <summary>
        // * @see <a href="http://tools.ietf.org/html/rfc6585#section-6">Additional HTTP Status Codes</a>
        // /// </summary>
        //NETWORK_AUTHENTICATION_REQUIRED(511, "Network Authentication Required");

        private static readonly Lazy<StatusType[]> Enumerations = new Lazy<StatusType[]>(GetEnumerations);

        private StatusType(int statusCode, string reasonPhrase)
        {
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
        }

        private StatusType(HttpStatusCode httpStatusCode, string reasonPhrase) : this((int) httpStatusCode, reasonPhrase)
        {
        }

        public int StatusCode { get; }
        public string ReasonPhrase { get; }

        public int CompareTo(StatusType other)
        {
            return StatusCode.CompareTo(other.StatusCode);
        }

        public sealed override string ToString()
        {
            return ReasonPhrase;
        }

        public static StatusType[] GetAll()
        {
            return Enumerations.Value;
        }

        private static StatusType[] GetEnumerations()
        {
            return typeof(StatusType)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Where(info => typeof(StatusType).IsAssignableFrom(info.FieldType))
                .Select(info => info.GetValue(null))
                .Cast<StatusType>()
                .ToArray();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as StatusType);
        }

        public bool Equals(StatusType other)
        {
            return other != null && StatusCode.Equals(other.StatusCode);
        }

        public override int GetHashCode()
        {
            return StatusCode.GetHashCode();
        }

        public static bool operator ==(StatusType left, StatusType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(StatusType left, StatusType right)
        {
            return !Equals(left, right);
        }

        public static StatusType FromValue(int statusCode)
        {
            return Parse(statusCode, "statusCode", x => x.StatusCode == statusCode);
        }

        public static StatusType FromValue(HttpStatusCode httpStatusCode)
        {
            return Parse(httpStatusCode, "statusCode", x => x.StatusCode == (int) httpStatusCode);
        }

        private static StatusType Parse<T>(T value, string description, Func<StatusType, bool> predicate)
        {
            StatusType result;

            if (!TryParse(predicate, out result))
            {
                var message = $"'{value}' is not a valid {description} in {typeof(StatusType)}";
                throw new ArgumentException(message, nameof(value));
            }
            return result;
        }

        private static bool TryParse(Func<StatusType, bool> predicate, out StatusType result)
        {
            result = GetAll().FirstOrDefault(predicate);
            return result != null;
        }

        public static bool TryParse(int value, out StatusType result)
        {
            return TryParse(e => e.StatusCode == value, out result);
        }

        public static bool TryParse(HttpStatusCode httpStatusCode, out StatusType result)
        {
            return TryParse(e => e.StatusCode == (int) httpStatusCode, out result);
        }
    }
}