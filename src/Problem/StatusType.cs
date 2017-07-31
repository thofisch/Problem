using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;

namespace Trifork
{
    [Serializable]
    [DebuggerDisplay("<{StatusCode} ({ReasonPhrase,nq})>")]
    public class StatusType : IComparable<StatusType>, IEquatable<StatusType>
    {
        public static readonly StatusType Continue = new StatusType(HttpStatusCode.Continue, "Continue");
        public static readonly StatusType SwitchingProtocols = new StatusType(HttpStatusCode.SwitchingProtocols, "Switching Protocols");
        public static readonly StatusType Processing = new StatusType(102, "Processing");
        public static readonly StatusType Checkpoint = new StatusType(103, "Checkpoint");

        public static readonly StatusType Ok = new StatusType(HttpStatusCode.OK, "OK");
        public static readonly StatusType Created = new StatusType(HttpStatusCode.Created, "Created");
        public static readonly StatusType Accepted = new StatusType(HttpStatusCode.Accepted, "Accepted");
        public static readonly StatusType NonAuthoritativeInformation = new StatusType(HttpStatusCode.NonAuthoritativeInformation, "Non-Authoritative Information");
        public static readonly StatusType NoContent = new StatusType(HttpStatusCode.NoContent, "No Content");
        public static readonly StatusType ResetContent = new StatusType(HttpStatusCode.ResetContent, "Reset Content");
        public static readonly StatusType PartialContent = new StatusType(HttpStatusCode.PartialContent, "Partial Content");
        public static readonly StatusType MultiStatus = new StatusType(207, "Multi-Status");
        public static readonly StatusType AlreadyReported = new StatusType(208, "Already Reported");
        public static readonly StatusType InstanceManipulationUsed = new StatusType(226, "IM Used");

        public static readonly StatusType MultipleChoices = new StatusType(HttpStatusCode.MultipleChoices, "Multiple Choices");
        public static readonly StatusType MovedPermanently = new StatusType(HttpStatusCode.MovedPermanently, "Moved Permanently");
        public static readonly StatusType Found = new StatusType(HttpStatusCode.Found, "Found");
        public static readonly StatusType SeeOther = new StatusType(HttpStatusCode.SeeOther, "See Other");
        public static readonly StatusType NotModified = new StatusType(HttpStatusCode.NotModified, "Not Modified");
        public static readonly StatusType UseProxy = new StatusType(HttpStatusCode.UseProxy, "Use Proxy");
        public static readonly StatusType TemporaryRedirect = new StatusType(HttpStatusCode.TemporaryRedirect, "Temporary Redirect");
        public static readonly StatusType PermanentRedirect = new StatusType(308, "Permanent Redirect");

        public static readonly StatusType BadRequest = new StatusType(HttpStatusCode.BadRequest, "Bad Request");
        public static readonly StatusType Unauthorized = new StatusType(HttpStatusCode.Unauthorized, "Unauthorized");
        public static readonly StatusType PaymentRequired = new StatusType(HttpStatusCode.PaymentRequired, "Payment Required");
        public static readonly StatusType Forbidden = new StatusType(HttpStatusCode.Forbidden, "Forbidden");
        public static readonly StatusType NotFound = new StatusType(HttpStatusCode.NotFound, "Not Found");
        public static readonly StatusType MethodNotAllowed = new StatusType(HttpStatusCode.MethodNotAllowed, "Method Not Allowed");
        public static readonly StatusType NotAcceptable = new StatusType(HttpStatusCode.NotAcceptable, "Not Acceptable");
        public static readonly StatusType ProxyAuthenticationRequired = new StatusType(HttpStatusCode.ProxyAuthenticationRequired, "Proxy Authentication Required");
        public static readonly StatusType RequestTimeout = new StatusType(HttpStatusCode.RequestTimeout, "Request Timeout");
        public static readonly StatusType Conflict = new StatusType(HttpStatusCode.Conflict, "Conflict");
        public static readonly StatusType Gone = new StatusType(HttpStatusCode.Gone, "Gone");
        public static readonly StatusType LengthRequired = new StatusType(HttpStatusCode.LengthRequired, "Length Required");
        public static readonly StatusType PreconditionFailed = new StatusType(HttpStatusCode.PreconditionFailed, "Precondition Failed");
        public static readonly StatusType RequestEntityTooLarge = new StatusType(HttpStatusCode.RequestEntityTooLarge, "Request Entity Too Large");
        public static readonly StatusType RequestUriTooLong = new StatusType(HttpStatusCode.RequestUriTooLong, "Request-Uri Too Long");
        public static readonly StatusType UnsupportedMediaType = new StatusType(HttpStatusCode.UnsupportedMediaType, "Unsupported Media Type");
        public static readonly StatusType RequestedRangeNotSatisfiable = new StatusType(HttpStatusCode.RequestedRangeNotSatisfiable, "Requested Range Not Satisfiable");
        public static readonly StatusType ExpectationFailed = new StatusType(HttpStatusCode.ExpectationFailed, "Expectation Failed");
        public static readonly StatusType ImATeapot = new StatusType(418, "I'm a teapot");
        public static readonly StatusType MisdirectedRequest = new StatusType(421, "Misdirected Request");
        public static readonly StatusType UnprocessableEntity = new StatusType(422, "Unprocessable Entity");
        public static readonly StatusType Locked = new StatusType(423, "Locked");
        public static readonly StatusType FailedDependency = new StatusType(424, "Failed Dependency");
        public static readonly StatusType UpgradeRequired = new StatusType(HttpStatusCode.UpgradeRequired, "Upgrade Required");
        public static readonly StatusType PreconditionRequired = new StatusType(428, "Precondition Required");
        public static readonly StatusType TooManyRequests = new StatusType(429, "Too Many Requests");
        public static readonly StatusType RequestHeaderFieldsTooLarge = new StatusType(431, "Request Header Fields Too Large");
        public static readonly StatusType ConnectionClosedWithoutResponse = new StatusType(444, "Connection Closed Without Response");
        public static readonly StatusType UnavailableForLegalReasons = new StatusType(451, "Unavailable For Legal Reasons");
        public static readonly StatusType ClientClosedRequest = new StatusType(499, "Client Closed Request");

        public static readonly StatusType InternalServerError = new StatusType(HttpStatusCode.InternalServerError, "Internal Server Error");
        public static readonly StatusType NotImplemented = new StatusType(HttpStatusCode.NotImplemented, "Not Implemented");
        public static readonly StatusType BadGateway = new StatusType(HttpStatusCode.BadGateway, "Bad Gateway");
        public static readonly StatusType ServiceUnavailable = new StatusType(HttpStatusCode.ServiceUnavailable, "Service Unavailable");
        public static readonly StatusType GatewayTimeout = new StatusType(HttpStatusCode.GatewayTimeout, "Gateway Timeout");
        public static readonly StatusType HttpVersionNotSupported = new StatusType(HttpStatusCode.HttpVersionNotSupported, "Http Version Not Supported");
        public static readonly StatusType VariantAlsoNegotiates = new StatusType(506, "Variant Also Negotiates");
        public static readonly StatusType InsufficientStorage = new StatusType(507, "Insufficient Storage");
        public static readonly StatusType LoopDetected = new StatusType(508, "Loop Detected");
        public static readonly StatusType BandwidthLimitExceeded = new StatusType(509, "Bandwidth Limit Exceeded");
        public static readonly StatusType NotExtended = new StatusType(510, "Not Extended");
        public static readonly StatusType NetworkAuthenticationRequired = new StatusType(511, "Network Authentication Required");
        public static readonly StatusType NetworkConnectTimeoutError = new StatusType(599, "Network Connect Timeout Error");

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