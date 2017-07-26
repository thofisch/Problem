using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Problem
{
    /**
     * @see <a href="https://tools.ietf.org/html/rfc7807">RFC 7807: Problem Details for HTTP APIs</a>
     */


    public class ProblemBuilder
    {
        private static readonly ISet<String> RESERVED_PROPERTIES =
            new HashSet<string>(new[] {"type", "title", "status", "detail", "instance", "cause"});

        private Uri type;
        private String title;
        private StatusType status;
        private String detail;
        private Uri instance;
        private ThrowableProblem cause;
        private readonly IDictionary<String, Object> parameters = new Dictionary<String, Object>();

        /**
         * @see Problem#builder()
         */
        ProblemBuilder()
        {
        }

        public ProblemBuilder withType(Uri type)
        {
            this.type = type;
            return this;
        }

        public ProblemBuilder withTitle(String title)
        {
            this.title = title;
            return this;
        }

        public ProblemBuilder withStatus(StatusType status)
        {
            this.status = status;
            return this;
        }

        public ProblemBuilder withDetail(String detail)
        {
            this.detail = detail;
            return this;
        }

        public ProblemBuilder withInstance(Uri instance)
        {
            this.instance = instance;
            return this;
        }

        public ProblemBuilder withCause(ThrowableProblem cause)
        {
            this.cause = cause;
            return this;
        }

        /**
         *
         * @param key
         * @param value
         * @return
         * @throws IllegalArgumentException if key is any of type, title, status, detail or instance
         */
        public ProblemBuilder with(String key, Object value)
        {
            //if (RESERVED_PROPERTIES.contains(key))
            //{
            //    throw new ArgumentException("Property " + key + " is reserved");
            //}
            //parameters.put(key, value);
            return this;
        }

        public ThrowableProblem build()
        {
            return new DefaultProblem(type, title, status, detail, instance, cause, new LinkedHashMap<>(parameters));
        }
    }

    public abstract class ThrowableProblem /*extends RuntimeException implements Problem, Exceptional*/
    {
        //protected ThrowableProblem()
        //{
        //}

        //protected ThrowableProblem(ThrowableProblem cause)
        //{
        //    super(cause);

        //    readonly Collection<StackTraceElement> stackTrace = COMPOUND.process(asList(getStackTrace()));
        //    setStackTrace(stackTrace.toArray(new StackTraceElement[stackTrace.size()]));
        //}

        //@Override
        //public String getMessage()
        //{
        //    return Stream.of(getTitle(), getDetail())
        //        .filter(Objects::nonNull)
        //        .collect(joining(": "));
        //}

        //@Override
        //public ThrowableProblem getCause()
        //{
        //    // cast is safe, since the only way to set this is our constructor
        //    return (ThrowableProblem)super.getCause();
        //}

        //@Override
        //public String toString()
        //{
        //    return Problem.toString(this);
        //}
    }

    public class DefaultProblem : AbstractThrowableProblem
    {
        // TODO needed for jackson
        DefaultProblem(Uri type, String title, StatusType status, String detail, Uri instance, ThrowableProblem cause)
        {
            //super(type, title, status, detail, instance, cause);
        }

        DefaultProblem(Uri type, String title, StatusType status, String detail, Uri instance, ThrowableProblem cause,
            IDictionary<String, Object> parameters)
        {
            //super(type, title, status, detail, instance, cause, parameters);
        }
    }

    public abstract class AbstractThrowableProblem : ThrowableProblem
    {
        private readonly Uri type;
        private readonly String title;
        private readonly StatusType status;
        private readonly String detail;
        private readonly Uri instance;
        private readonly IDictionary<String, Object> parameters;

        protected AbstractThrowableProblem()
        {
            this(null);
        }

        protected AbstractThrowableProblem(Uri type)
        {
            this(type, null);
        }

        protected AbstractThrowableProblem(Uri type,
            String title)
        {
            this(type, title, null);
        }

        protected AbstractThrowableProblem(Uri type,
            String title,
            Response.StatusType status)
        {
            this(type, title, status, null);
        }

        protected AbstractThrowableProblem(Uri type,
            String title,
            Response.StatusType status,
            String detail)
        {
            this(type, title, status, detail, null);
        }

        protected AbstractThrowableProblem(Uri type,
            String title,
            Response.StatusType status,
            String detail,
            Uri instance)
        {
            this(type, title, status, detail, instance, null);
        }

        protected AbstractThrowableProblem(Uri type,
            String title,
            Response.StatusType status,
            String detail,
            Uri instance,
            ThrowableProblem cause)
        {
            this(type, title, status, detail, instance, cause, null);
        }

        protected AbstractThrowableProblem(Uri type,
            String title,
            Response.StatusType status,
            String detail,
            Uri instance,
            ThrowableProblem cause,
            Map<String, Object> parameters)
        {
            super(cause);
            this.type = Optional.ofNullable(type).orElse(DEFAULT_TYPE);
            this.title = title;
            this.status = status;
            this.detail = detail;
            this.instance = instance;
            this.parameters = Optional.ofNullable(parameters).orElseGet(LinkedHashMap::new);
        }

        @Override

        public Uri getType()
        {
            return type;
        }

        @Override

        public String getTitle()
        {
            return title;
        }

        @Override

        public Response.StatusType getStatus()
        {
            return status;
        }

        @Override

        public String getDetail()
        {
            return detail;
        }

        @Override

        public Uri getInstance()
        {
            return instance;
        }

        @Override

        public Map<String, Object> getParameters()
        {
            return Collections.unmodifiableMap(parameters);
        }

        /**
         * This is required to workaround missing support for {@link com.fasterxml.jackson.annotation.JsonAnySetter} on
         * constructors annotated with {@link com.fasterxml.jackson.annotation.JsonCreator}.
         *
         * @param key   the custom key
         * @param value the custom value
         * @see <a href="https://github.com/FasterXML/jackson-databind/issues/562">Jackson Issue 562</a>
         */
        @Hack
            @OhNoYouDidnt

        void set(readonly String key, readonly Object value)
        {
            parameters.put(key, value);
        }
    }

    public abstract class Problem
    {
        Uri DEFAULT_TYPE = new Uri("about:blank");

/**
     * An absolute Uri that identifies the problem type. When dereferenced,
     * it SHOULD provide human-readable documentation for the problem type
     * (e.g., using HTML). When this member is not present, its value is
     * assumed to be "about:blank".
     *
     * @return an absolute Uri that identifies this problem's type
     */
        public virtual Uri getType()
        {
            return DEFAULT_TYPE;
        }

/**
 * A short, human-readable summary of the problem type. It SHOULD NOT
 * change from occurrence to occurrence of the problem, except for
 * purposes of localisation.
 *
 * @return a short, human-readable summary of this problem
 */
        public virtual String getTitle()
        {
            return null;
        }

/**
 * The HTTP status code generated by the origin server for this
 * occurrence of the problem.
 *
 * @return the HTTP status code
 */
        public virtual StatusType getStatus()
        {
            return null;
        }

/**
 * A human readable explanation specific to this occurrence of the problem.
 *
 * @return A human readable explaination of this problem
 */
        public virtual String getDetail()
        {
            return null;
        }

/**
 * An absolute Uri that identifies the specific occurrence of the problem.
 * It may or may not yield further information if dereferenced.
 *
 * @return an absolute Uri that identifies this specific problem
 */
        public virtual Uri getInstance()
        {
            return null;
        }
/**
 * Optional, additional attributes of the problem. Implementations can choose to ignore this in favor of concrete,
 * typed fields.
 *
 * @return additional parameters
 */
//public virtual  Map<String, Object> getParameters()
//{
//    return Collections.emptyMap();
//}

//static ProblemBuilder builder()
//{
//    return new ProblemBuilder();
//}

//static ThrowableProblem valueOf(readonly StatusType status)
//{
//    return GenericProblems.create(status).build();
//}

//static ThrowableProblem valueOf(readonly StatusType status, readonly String detail)
//{
//    return GenericProblems.create(status).withDetail(detail).build();
//}

//static ThrowableProblem valueOf(readonly StatusType status, readonly Uri instance)
//{
//    return GenericProblems.create(status).withInstance(instance).build();
//}

//static ThrowableProblem valueOf(readonly StatusType status, readonly String detail, readonly Uri instance)
//{
//    return GenericProblems.create(status).withDetail(detail).withInstance(instance).build();
//}
/**
 * Specification by example:
 * <p>
 * <pre>{@code
 *   // Returns "about:blank{404, Not Found}"
 *   Problem.valueOf(NOT_FOUND).toString();
 *
 *   // Returns "about:blank{404, Not Found, Order 123}"
 *   Problem.valueOf(NOT_FOUND, "Order 123").toString();
 *
 *   // Returns "about:blank{404, Not Found, instance=https://example.org/}"
 *   Problem.valueOf(NOT_FOUND, Uri.create("https://example.org/")).toString();
 *
 *   // Returns "about:blank{404, Not Found, Order 123, instance=https://example.org/"}
 *   Problem.valueOf(NOT_FOUND, "Order 123", Uri.create("https://example.org/")).toString();
 *
 *   // Returns "https://example.org/problem{422, Oh, oh!, Crap., instance=https://example.org/problem/123}
 *   Problem.builder()
 *       .withType(Uri.create("https://example.org/problem"))
 *       .withTitle("Oh, oh!")
 *       .withStatus(UNPROCESSABLE_ENTITY)
 *       .withDetail("Crap.")
 *       .withInstance(Uri.create("https://example.org/problem/123"))
 *       .build()
 *       .toString();
 * }</pre>
 *
 * @param problem the problem
 * @return a string representation of the problem
 * @see Problem#valueOf(StatusType)
 * @see Problem#valueOf(StatusType, String)
 * @see Problem#valueOf(StatusType, Uri)
 * @see Problem#valueOf(StatusType, String, Uri)
 */
//static String toString(readonly Problem problem)
//{
//    readonly Stream<String>
//    parts = Stream.concat(
//            Stream.of(
//                problem.getStatus() == null ? null : String.valueOf(problem.getStatus().getStatusCode()),
//                problem.getTitle(),
//                problem.getDetail(),
//                problem.getInstance() == null ? null : "instance=" + problem.getInstance()),
//            problem.getParameters()
//                .entrySet().stream()
//                .map(Map.Entry::toString))
//        .filter(Objects::nonNull);

//    return problem.getType().toString() + "{" + parts.collect(joining(", ")) + "}";
//}
    }

/**
 * The {@link Status} class in JAX-RS doesn't list of official HTTP status codes. The purpose
 * of this class is to provide easy access to the missing ones.
 *
 * @see Status
 */
    public class StatusType
    {
        ///**
// * @see <a href="http://tools.ietf.org/html/rfc7231#section-6.2.1">HTTP/1.1: Semantics and Content, section 6.2.1</a>
// */
//CONTINUE(100, "Continue"),
        ///**
// * @see <a href="http://tools.ietf.org/html/rfc7231#section-6.2.2">HTTP/1.1: Semantics and Content, section 6.2.2</a>
// */
//SWITCHING_PROTOCOLS(101, "Switching Protocols"),
        ///**
// * @see <a href="http://tools.ietf.org/html/rfc2518#section-10.1">WebDAV</a>
// */
//PROCESSING(102, "Processing"),
        ///**
// * @see <a href="http://code.google.com/p/gears/wiki/ResumableHttpRequestsProposal">A proposal for supporting
// * resumable POST/PUT HTTP requests in HTTP/1.0</a>
// */
//CHECKPOINT(103, "Checkpoint"),
        ///**
// * @see <a href="http://tools.ietf.org/html/rfc7231#section-6.3.4">HTTP/1.1: Semantics and Content, section 6.3.4</a>
// */
//NON_AUTHORITATIVE_INFORMATION(203, "Non-Authoritative Information"),
        ///**
// * @see <a href="http://tools.ietf.org/html/rfc4918#section-13">WebDAV</a>
// */
//MULTI_STATUS(207, "Multi-Status"),
        ///**
// * @see <a href="http://tools.ietf.org/html/rfc5842#section-7.1">WebDAV Binding Extensions</a>
// */
//ALREADY_REPORTED(208, "Already Reported"),
        ///**
// * @see <a href="http://tools.ietf.org/html/rfc3229#section-10.4.1">Delta encoding in HTTP</a>
// */
//IM_USED(226, "IM Used"),
        ///**
// * @see <a href="http://tools.ietf.org/html/rfc7231#section-6.4.1">HTTP/1.1: Semantics and Content, section 6.4.1</a>
// */
//MULTIPLE_CHOICES(300, "Multiple Choices"),
        ///**
// * @see <a href="http://tools.ietf.org/html/rfc7238">RFC 7238</a>
// */
//PERMANENT_REDIRECT(308, "Permanent Redirect"),
        ///**
// * @see <a href="http://tools.ietf.org/html/rfc2324#section-2.3.2">HTCPCP/1.0</a>
// */
//I_AM_A_TEAPOT(418, "I'm a teapot"),
        ///**
// * @see <a href="http://tools.ietf.org/html/rfc4918#section-11.2">WebDAV</a>
// */
//UNPROCESSABLE_ENTITY(422, "Unprocessable Entity"),
        ///**
// * @see <a href="http://tools.ietf.org/html/rfc4918#section-11.3">WebDAV</a>
// */
//LOCKED(423, "Locked"),
        ///**
// * @see <a href="http://tools.ietf.org/html/rfc4918#section-11.4">WebDAV</a>
// */
//FAILED_DEPENDENCY(424, "Failed Dependency"),
        ///**
// * @see <a href="http://tools.ietf.org/html/rfc2817#section-6">Upgrading to TLS Within HTTP/1.1</a>
// */
//UPGRADE_REQUIRED(426, "Upgrade Required"),
        ///**
// * @see <a href="http://tools.ietf.org/html/rfc6585#section-3">Additional HTTP Status Codes</a>
// */
//PRECONDITION_REQUIRED(428, "Precondition Required"),
        ///**
// * @see <a href="http://tools.ietf.org/html/rfc6585#section-4">Additional HTTP Status Codes</a>
// */
//TOO_MANY_REQUESTS(429, "Too Many Requests"),
        ///**
// * @see <a href="http://tools.ietf.org/html/rfc6585#section-5">Additional HTTP Status Codes</a>
// */
//REQUEST_HEADER_FIELDS_TOO_LARGE(431, "Request Header Fields Too Large"),
        ///**
// * @see <a href="http://tools.ietf.org/html/rfc2295#section-8.1">Transparent Content Negotiation</a>
// */
//VARIANT_ALSO_NEGOTIATES(506, "Variant Also Negotiates"),
        ///**
// * @see <a href="http://tools.ietf.org/html/rfc4918#section-11.5">WebDAV</a>
// */
//INSUFFICIENT_STORAGE(507, "Insufficient Storage"),
        ///**
// * @see <a href="http://tools.ietf.org/html/rfc5842#section-7.2">WebDAV Binding Extensions</a>
// */
//LOOP_DETECTED(508, "Loop Detected"),
        ///**
// * {@code 509 Bandwidth Limit Exceeded}
// */
//BANDWIDTH_LIMIT_EXCEEDED(509, "Bandwidth Limit Exceeded"),
        ///**
// * @see <a href="http://tools.ietf.org/html/rfc2774#section-7">HTTP Extension Framework</a>
// */
//NOT_EXTENDED(510, "Not Extended"),
        ///**
// * @see <a href="http://tools.ietf.org/html/rfc6585#section-6">Additional HTTP Status Codes</a>
// */
//NETWORK_AUTHENTICATION_REQUIRED(511, "Network Authentication Required");

//private readonly int statusCode;
//private readonly Family family;
//private readonly String reasonPhrase;

//MoreStatus(readonly int statusCode, readonly String reasonPhrase)
//{
//    this.statusCode = statusCode;
//    this.family = Family.familyOf(statusCode);
//    this.reasonPhrase = reasonPhrase;
//}

//@Override
//public int getStatusCode()
//{
//    return statusCode;
//}

//@Override
//public Family getFamily()
//{
//    return family;
//}

//@Override
//public String getReasonPhrase()
//{
//    return reasonPhrase;
//}
        ///**
// * Convert a numerical status code into the corresponding Status.
// *
// * @param statusCode the numerical status code.
// * @return the matching Status or null is no matching Status is defined.
// */
//@Nullable
//public static StatusType fromStatusCode(readonly int statusCode)
//{
//    for (readonly StatusType status : values())
//    {
//        if (status.getStatusCode() == statusCode)
//        {
//            return status;
//        }
//    }
//    return null;
//}
    }
}