﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json;

namespace Trifork
{
    /// <summary>
    ///     See <a href="https://tools.ietf.org/html/rfc7807">RFC 7807: Problem Details for HTTP APIs</a>
    /// </summary>
    public class Problem
    {
        private readonly IDictionary<string, object> _parameters = new Dictionary<string, object>();

        [JsonConstructor]
        private Problem()
        {
        }

        public Problem(Uri type) : this(type, null)
        {
        }

        public Problem(Uri type, string title) : this(type, title, null)
        {
        }

        public Problem(Uri type, string title, StatusType status) : this(type, title, status, null)
        {
        }

        public Problem(Uri type, string title, StatusType status, string detail) : this(type, title, status, detail,
            null)
        {
        }

        public Problem(Uri type, string title, StatusType status, string detail, Uri instance) : this(type, title,
            status, detail, instance, null)
        {
        }

        public Problem(Uri type, string title, StatusType status, string detail, Uri instance, Problem cause) : this(type, title, status, detail, instance, cause, null)
        {
        }

        public Problem(Uri type, string title, StatusType status, string detail, Uri instance, Problem cause, IDictionary<string, object> parameters)
        {
            Type = type;
            Title = title;
            Status = status;
            Detail = detail;
            Instance = instance;
            Cause = cause;
            _parameters = parameters ?? new Dictionary<string, object>();
        }

        /// <summary>
        ///     "type" (string) - A URI reference[RFC3986] that identifies the problem type.This specification encourages that,
        ///     when dereferenced,
        ///     it provide human-readable documentation for the problem type (e.g., using HTML [W3C.REC-html5-20141028]).
        ///     When this member is not present, its value is assumed to be "about:blank"
        ///     Consumers MUST use the "type" string as the primary identifier forthe problem type
        ///     When "about:blank" is used, the title SHOULD be the same as the recommended HTTP status phrase for that code(e.g.,
        ///     "Not Found" for
        ///     404, and so on), although it MAY be localized to suit client preferences(expressed with the Accept-Language request
        ///     header)
        /// </summary>
        public Uri Type { get; set; }

        /// <summary>
        ///     "title" (string) - A short, human-readable summary of the problem type.It SHOULD NOT change from occurrence to
        ///     occurrence of the
        ///     problem, except for purposes of localization(e.g., using proactive content negotiation; see[RFC7231], Section 3.4)
        ///     the "title" string is advisory and included only for users who are not aware of the semantics of the URI and do not
        ///     have the ability to discover them (e.g., offline log analysis). Consumers SHOULD NOT automatically dereference the
        ///     type URI.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     "status" (number) - The HTTP status code ([RFC7231], Section 6) generated by the origin server for this occurrence
        ///     of the problem.
        ///     The "status" member, if present, is only advisory; it conveys the HTTP status code used for the convenience of the
        ///     consumer.
        ///     Generators MUST use the same status code in the actual HTTP response, to assure that generic HTTP software that
        ///     does not understand this
        ///     format still behaves correctly.  See Section 5 for further caveats regarding its use.
        /// </summary>
        [JsonConverter(typeof(StatusTypeConverter))]
        public StatusType Status { get; set; }

        /// <summary>
        ///     "detail" (string) - A human-readable explanation specific to this occurrence of the problem.
        ///     The "detail" member, if present, ought to focus on helping the client correct the problem, rather than giving
        ///     debugging information.
        ///     Consumers SHOULD NOT parse the "detail" member for information; extensions are more suitable and less error-prone
        ///     ways to obtain such information.
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        ///     "instance" (string) - A URI reference that identifies the specific occurrence of the problem.
        ///     It may or may not yield further information if dereferenced.
        /// </summary>
        public Uri Instance { get; set; }

        public Problem Cause { get; set; }

        /// <summary>
        ///     Optional, additional attributes of the problem. Implementations can choose to ignore this in favor of concrete,
        ///     typed fields.
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, object> Parameters => _parameters;

        public static Problem ValueOf(StatusType status)
        {
            return ProblemBuilder.Create(status).Build();
        }

        public static Problem ValueOf(StatusType status, string detail)
        {
            return ProblemBuilder.Create(status).WithDetail(detail).Build();
        }

        public static Problem ValueOf(StatusType status, Uri instance)
        {
            return ProblemBuilder.Create(status).WithInstance(instance).Build();
        }

        public static Problem ValueOf(StatusType status, string detail, Uri instance)
        {
            return ProblemBuilder.Create(status).WithDetail(detail).WithInstance(instance).Build();
        }

        public override string ToString()
        {
            var parts = new[]
            {
                Status == null ? null : Status.StatusCode.ToString(),
                Title,
                Detail,
                Instance == null ? null : "instance=" + Instance,
                //        problem.getParameters()
                //            .entrySet().stream()
                //            .map(Map.Entry::toString))
            }.Where(x => x != null);

            return $"{GetType().Name}{{{string.Join(", ", parts)}}}";
        }
    }

    public class StatusTypeConverter : JsonConverter
    {
        //public override bool CanRead { get; } = false;

        private static readonly ConcurrentDictionary<Type, TypeConverter> TypeConverters = new ConcurrentDictionary<Type, TypeConverter>();

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var statusType = (StatusType) value;

            writer.WriteValue(statusType.StatusCode);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                throw new InvalidOperationException("Cannot accept <null> as a StatusType value");
            }

            var typeConverter = TypeConverters.GetOrAdd(reader.ValueType, TypeDescriptor.GetConverter);

            if (!typeConverter.CanConvertTo(typeof(int)))
            {
                throw new InvalidOperationException($"Cannot accept StatusType of type '{reader.ValueType}' with value '{reader.Value}'.");
            }

            return StatusType.FromValue((int) typeConverter.ConvertTo(reader.Value, typeof(int)));
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(StatusType).IsAssignableFrom(objectType);
        }
    }
}