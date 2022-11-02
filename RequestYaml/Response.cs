using AutoMapper;
using AutoMapper.Configuration;
using System.Net.Http;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace RequestYaml
{
	public class Response
	{
		public int Status { get; set; }
		public string StatusText { get; set; }
		public string Content { get; set; }
        public Dictionary<string, string> Cookie { get; set; }
        public Dictionary<string, string> ExpectedFields { get; set; }
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
