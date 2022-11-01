using System.Net;
using System.Net.Http.Headers;
using AutoMapper;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace RequestYaml
{
	public class RequestHandler
	{
		private HttpClient _client;
		private DataRequest _requests;

		public RequestHandler()
		{
		 	_client = new HttpClient();
		}

		public RequestHandler(string src) : this()
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
				 			Method = MethodRequest.Get,
							Url = "https://yoomoney.ru/catalogue/phone",
							Headers = new Header {
					 			Host = "yoomoney.ru",
					 			Referrer = "https://yoomoney.ru"
					 		},
							Response = new Response
							{
								ReturnFolders = new Dictionary<string, string>
								{
									{ "asdf", "asdf" }
								}
							}
				 		}
			 		},
				 	{	
				 		"phone-beeline",
				 		new Request {
				 			Method = MethodRequest.Get,
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


		public async Task<Response> HandleRequestAsync(string nameRequest)
		{
			Request request = _requests.Requests[nameRequest];
            request.Response = await RequestAsync(request);

			return request.Response;
		}

		public Request this[string name]
		{
			get => _requests.Requests[name];
		}

        public async Task<HttpResponseMessage> RequestAsync(Request request)
			=> request.Method switch 
				{
					MethodRequest.Get => await _client.GetAsync(request.Url),
					MethodRequest.Post => await _client.PostAsync(request.Url, new StringContent(request.Body.PostData)),
					MethodRequest.Delete => await _client.DeleteAsync(request.Url),
					_ => default
                };
		
	}
}
