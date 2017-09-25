using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using Trifork;

namespace Problem.Tests
{
    [TestFixture]
    public class Test
    {
        [Test]
        public void Test1()
        {
            var foo = Trifork.Problem.From(StatusType.Ok)
                    .WithType(new Uri("about:blank"))
                    .WithDetail("some detail")
                    .WithInstance(new Uri("http://www.example.org/log/1"))
                    .With("balance", "0")
                ;

            Console.WriteLine(foo);

            var settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Include;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //settings.Converters.Add(new ComplexDictionaryConverter<string,object>());
            //settings.Converters.Add(new StatusTypeConverter());

            string json = JsonConvert.SerializeObject(foo, Formatting.Indented, settings);

            Console.WriteLine(json);

            var problem = JsonConvert.DeserializeObject<Trifork.Problem>(json);

            Assert.AreEqual(StatusType.Ok, problem.Status);
        }
    }
}