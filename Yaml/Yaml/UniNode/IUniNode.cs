using System.Collections;

namespace Yaml.UniNode
{
	public interface IUniNode
	{
		public string Value { get; set; }
		public string this[int index] { get; set; }
		public string this[string name] { get; set; }
	}
}
