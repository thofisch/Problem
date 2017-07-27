using System;
using System.Collections.Generic;

namespace Trifork
{
    public class ProblemBuilder
    {
        public static ProblemBuilder Create(StatusType status)
        {
            return new ProblemBuilder()
                .WithTitle(status.ReasonPhrase)
                .WithStatus(status);
        }

        private static readonly ISet<string> ReservedProperties = new HashSet<string>(new[] {"type", "title", "status", "detail", "instance", "cause"});

        private readonly IDictionary<string, object> _parameters = new Dictionary<string, object>();

        private Uri _type;
        private string _title;
        private StatusType _status;
        private string _detail;
        private Uri _instance;
        private Problem _cause;

        private ProblemBuilder()
        {
        }

        public ProblemBuilder WithType(Uri type)
        {
            _type = type;
            return this;
        }

        public ProblemBuilder WithTitle(string title)
        {
            _title = title;
            return this;
        }

        public ProblemBuilder WithStatus(StatusType status)
        {
            _status = status;
            return this;
        }

        public ProblemBuilder WithDetail(string detail)
        {
            _detail = detail;
            return this;
        }

        public ProblemBuilder WithInstance(Uri instance)
        {
            _instance = instance;
            return this;
        }

        public ProblemBuilder WithCause(Problem cause)
        {
            _cause = cause;
            return this;
        }

        public ProblemBuilder With(string key, object value)
        {
            if (ReservedProperties.Contains(key))
            {
                throw new ArgumentException($"Property {key} is reserved", nameof(key));
            }
            _parameters.Add(key, value);
            return this;
        }

        public Problem Build()
        {
            return new Problem(_type, _title, _status, _detail, _instance, _cause, _parameters);
        }
    }
}