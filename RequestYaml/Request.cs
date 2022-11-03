using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using Newtonsoft.Json;
using System.Net;

namespace RequestYaml
{
	public class Request
	{
		private string _url;

        private HttpClient _client;
        public string Name { get; set; }
        public Method Method { get; set; }
		public Dictionary<string, string> QueryString { get; set; }

		public string Url
		{
			get
			{
				if (QueryString == null || QueryString.Count == 0)
					return _url;

				IEnumerable<string> @params = QueryString.Count > 0 ?
					QueryString.Select(value => $"{value.Key}={value.Value}") : null;

				return $"{_url}?{String.Join('&', @params)}".Replace(' ', '+');
			}
			set => _url = value;
		}

		public Header Headers { get; set; }
		public Body Body { get; set; }
		public Response Response { get; set; }
        public Dictionary<string, string> Cookie { get; set; }


		public Request()
		{
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.AutomaticDecompression = DecompressionMethods.All;
            
            _client = new HttpClient(httpClientHandler);
        }

        public void InitClient()
        {
            _client.DefaultRequestHeaders.Host = Headers?.Host;
            _client.DefaultRequestHeaders.Referrer = Headers.Referrer != null ? new Uri(Headers.Referrer) : null;
            
            _client.DefaultRequestHeaders.Accept.TryParseAdd(Headers.Accept);
            _client.DefaultRequestHeaders.AcceptLanguage.TryParseAdd(Headers.AcceptLanguage);

            Headers?.AcceptEncoding
                .Split(',', StringSplitOptions.TrimEntries)
                .ToList()
                .ForEach(encode => _client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue(encode)));

            _client.DefaultRequestHeaders.AcceptCharset.TryParseAdd(Headers.AcceptCharset);
            _client.DefaultRequestHeaders.UserAgent.TryParseAdd(Headers.UserAgent);
            
            foreach ((string name, string value) in Headers.Other)
                _client.DefaultRequestHeaders.TryAddWithoutValidation(name, value);
        }

		public async Task<Response> RequestAsync()
		{
            InitClient();

            HttpResponseMessage httpResponseMessage = await MethodAsync();

            Response = new Response
            {
                Status = (int)httpResponseMessage.StatusCode,
                StatusText = httpResponseMessage.StatusCode.ToString(),
                Content = await httpResponseMessage.Content.ReadAsStringAsync(),
            };

            if (ParseCookieHandler != null)
                Response.Cookie = ParseCookieHandler(httpResponseMessage.Headers);

            if (ParseExpectedFieldsHandler != null)
                Response.ExpectedFields = ParseExpectedFieldsHandler(Response.Content);

            return Response;
        }

        public async Task<HttpResponseMessage> MethodAsync()
            => Method switch
            {
                Method.Get => await _client.GetAsync(Url),
                Method.Post => Body.MimeType switch
				{
                    "application/x-www-form-urlencoded" => await _client.PostAsync(Url, Body.Format == "json" ? 
                        new StringContent(JsonConvert.SerializeObject(Body.PostData), Encoding.UTF8, "application/x-www-form-urlencoded") : 
                        new FormUrlEncodedContent(Body.PostData)
                        ),
                    "application/json; charset=UTF-8"   => await _client.PostAsync(Url, new StringContent(JsonConvert.SerializeObject(Body.PostData))),
                                                      _ => await _client.PostAsync(Url, new StringContent(Body.PostData["content"])),
                },
                Method.Delete => await _client.DeleteAsync(Url),
                _ => default
            };

        public delegate Dictionary<string, string> ParseToDictionary<in T>(T content);

		public ParseToDictionary<string> ParseExpectedFieldsHandler = null;

		public ParseToDictionary<HttpResponseHeaders> ParseCookieHandler;

    }
}
