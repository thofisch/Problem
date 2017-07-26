using System;
using System.Collections.Generic;

namespace Trifork
{
    /**
         * @see <a href="https://tools.ietf.org/html/rfc7807">RFC 7807: Problem Details for HTTP APIs</a>
         */


    public class ProblemBuilder
    {
        private static readonly ISet<string> RESERVED_PROPERTIES =
            new HashSet<string>(new[] {"type", "title", "status", "detail", "instance", "cause"});

        private Uri type;
        private String title;
        private StatusType status;
        private String detail;
        private Uri instance;
        private Problem cause;
        private readonly IDictionary<string, object> parameters = new Dictionary<string, object>();

        /**
         * @see Problem#builder()
         */
        public ProblemBuilder()
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

        public ProblemBuilder withCause(Problem cause)
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

        public Problem build()
        {
            return new Problem(type, title, status, detail, instance, cause, parameters);
        }

        public static ProblemBuilder create(StatusType status)
        {
            return Problem.builder()
                .withTitle(status.ReasonPhrase)
                .withStatus(status);
        }
    }
}