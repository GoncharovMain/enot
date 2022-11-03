using System.Net.Mime;
using System.Reflection.PortableExecutable;

namespace RequestYaml
{
	public class Header 
	{
		public string Host { get; set; }
		public string Referrer { get; set; }
		public string Origin { get; set; }

        public string UserAgent { get;set;}
		public string Accept { get;set;}
		public string AcceptLanguage { get;set;}
		public string AcceptEncoding { get;set;}
		public string AcceptCharset { get; set; }

        public string ContentType { get;set;}
		public string Connection { get; set; }

        public Dictionary<string, string> Other { get; set; }
	}
}
