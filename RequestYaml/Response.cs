using AutoMapper;
using AutoMapper.Configuration;
using System.Net.Http;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


namespace RequestYaml
{
	public class Response
	{
		public int Status { get; set; }
		public string StatusText { get; set; }
		public string Content { get; set; }

		public Dictionary<string, string> Cookie { get; set; }

		public static implicit operator Response(HttpResponseMessage response)
			=> new Response {
				Status = (int)response.StatusCode,
				StatusText = response.StatusCode.ToString(),
				Content = response.Content.ReadAsStringAsync().Result,
				Cookie = response.Headers.GetValues("set-cookie")
					.Select(cookie => cookie.Split(new char[] { '=', ';' }, 3))
					.ToDictionary(value => value[0], value => value[1])
			};
	}

	public class MappingResponseProfile : Profile
    {
		public MappingResponseProfile()
		{
			CreateMap<HttpResponseMessage, Response>()
				.ForMember(
					response => response.Status,
					httpResponseMessage => httpResponseMessage.MapFrom(message => (int)message.StatusCode))
				.ForMember(
					response => response.StatusText,
					httpResponseMessage => httpResponseMessage.MapFrom(message => message.StatusCode));
		}
	}
}
