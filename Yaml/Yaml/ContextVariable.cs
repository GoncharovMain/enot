using YamlDotNet.Core.Tokens;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Yaml.UniNode;

namespace Yaml
{

	public class ContextVariable
	{

	}

	public class UniDataRequest
	{
		public Dictionary<string, UniRequest> Requests { get; set; }
	}

	public class UniRequest
	{
		public FactoryNode Header { get; set; }
	}



	public class DataRequest
	{
		public Dictionary<string, Request> Requests { get; set; }
	}

	public class Request
	{
		public string Header { get; set; }
	}

}
