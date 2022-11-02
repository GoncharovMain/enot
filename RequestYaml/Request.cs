using System.Net.Http;
using System.Net.Http.Headers;

namespace RequestYaml
{
	public class Request
	{
		private string _url;
        private HttpClient _client;
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
			_client = new HttpClient();
		}

		public async Task<Response> RequestAsync()
		{
            HttpResponseMessage httpResponseMessage = await MethodAsync();

            Response = new Response
            {
                Status = (int)httpResponseMessage.StatusCode,
                StatusText = httpResponseMessage.StatusCode.ToString(),
                Content = await httpResponseMessage.Content.ReadAsStringAsync(),
            };

            if (ParseCookieHandler != null)
                Cookie = ParseCookieHandler(httpResponseMessage.Headers);

            if (ParseExpectedFieldsHandler != null)
                Response.ExpectedFields = ParseExpectedFieldsHandler(Response.Content);

            return Response;
        }

        public async Task<HttpResponseMessage> MethodAsync()
            => Method switch
            {
                Method.Get => await _client.GetAsync(Url),
                Method.Post => await _client.PostAsync(Url, new StringContent(Body.PostData)),
                Method.Delete => await _client.DeleteAsync(Url),
                _ => default
            };


        public delegate Dictionary<string, string> ParseToDictionary<in T>(T content);

		public ParseToDictionary<string> ParseExpectedFieldsHandler = null;

		public ParseToDictionary<HttpResponseHeaders> ParseCookieHandler;/* = headers => headers.GetValues("set-cookie")
                    .Select(cookie => cookie.Split(new char[] { '=', ';' }, 3))
                    .ToDictionary(value => value[0], value => value[1]);*/

    }
}
