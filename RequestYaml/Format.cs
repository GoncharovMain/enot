using System.Net;
using System.Net.Http.Headers;


using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

#nullable disable

namespace RequestYaml
{
	public static class HeaderExtensions
	{
		public static void SetHeaders(this HttpRequestHeaders httpRequestHeaders, Header headers)
		{
			httpRequestHeaders.Host = headers?.Host;
			httpRequestHeaders.Referrer = new Uri(headers?.Referrer);
		}
	}

	public class DataRequest
	{
		public Dictionary<string, Request> Requests { get; set; }
	}

	public enum MethodRequest
	{
		Get = 0,
		Post = 1,
		Put = 2,
		Delete = 3,
		Patch = 4
	}

	public class Header 
	{
		public string Host { get; set; }
		public string Referrer { get; set; }
	}

	public class Body
	{
		public string MimeType { get; set; }
		public string PostData { get; set; }
	}

	public class Request
	{
		public MethodRequest Method { get; set; }
		public string Url { get; set; }
		public Header Headers { get; set; }
		public Body Body { get; set; }
		public Response Response { get; set; }
	}

	public class Response
	{

		public delegate Dictionary<string, string> ParseCookie(Func<bool> predicate);
	    public event ParseCookie ParseCookieHandler;

		public int Status { get; set; }
		public string StatusText { get; set; }
		public string Content { get; set; }

		public Dictionary<string, string> Cookie { get; set; }

		public static implicit operator Response(HttpResponseMessage response)
			=> new Response {
				Status = (int)response.StatusCode,
				StatusText = response.StatusCode.ToString(),
				Content = response.Content.ReadAsStringAsync().Result

				
			};
		
	}

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

			using (StreamWriter writer = new StreamWriter("request.yaml"))
			{
				writer.Write(yaml);
			}
		}


		public async Task<Response> HandleRequestAsync(string nameRequest)
		{
			Request request = _requests.Requests[nameRequest];
			return await RequestAsync(request);
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