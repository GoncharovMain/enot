using System.Net;
using System.Net.Http.Headers;
using AutoMapper;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace RequestYaml
{
	public class RequestCollection
	{
		protected DataRequest _requests;

        public Request this[string name] => _requests.Requests[name];
    }

	public class YamlRequestCollection : RequestCollection
	{
		public YamlRequestCollection() : base() { }

		public YamlRequestCollection(string src) : this()
		{
			using (StreamReader reader = new StreamReader(src))
			{
				var yml = reader.ReadToEnd();

				var deserializer = new DeserializerBuilder()
					.WithNamingConvention(CamelCaseNamingConvention.Instance)
					.Build();

				_requests = deserializer.Deserialize<DataRequest>(yml);
			}
		}

		public void WriteYamlAsync()
		{
			Console.WriteLine("****WRITE*************************");

			var request = new DataRequest
		 	{
		 		Requests = new Dictionary<string, Request> {
		 			{ 
		 				"catalogue", 
			 			new Request {
				 			Method = Method.Get,
							Url = "https://yoomoney.ru/catalogue/phone",
							Headers = new Header {
					 			Host = "yoomoney.ru",
					 			Referrer = "https://yoomoney.ru"
					 		},
							Response = new Response
							{
								ExpectedFields = new Dictionary<string, string>
								{
									{ "asdf", "asdf" }
								}
							}
				 		}
			 		},
				 	{	
				 		"phone-beeline",
				 		new Request {
				 			Method = Method.Get,
					 		Url = "https://yoomoney.ru/phone?sum=&netSum=&phone-prefix=&phone-number=&scid=&phone-operator=beeline-343",
					 		Headers = new Header {
					 			Host = "yoomoney.ru",
					 			Referrer = "https://yoomoney.ru"
					 		}
				 		}
			 		}
				}
		 	};

			var serializer = new SerializerBuilder()
			    .WithNamingConvention(CamelCaseNamingConvention.Instance)
			    .Build();

			var yaml = serializer.Serialize(request);

			Console.WriteLine(yaml);

			using (StreamWriter writer = new StreamWriter("request1.yaml"))
			{
				writer.Write(yaml);
			}
		}
	}
}
