using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yaml.UniNode
{
	public class NodeDictonary : IUniNode
	{
		public Dictionary<string, string> Dict { get; set; }
		public string this[string name] { get => Dict[name]; set => Dict[name] = value; }

		#region Cannot convert for Dictionary<string, string>

		public string Value { get => null; set => throw new Exception("Cannot convert to String"); }
		public string this[int index] { get => null; set => throw new Exception("Cannot convert to List<string>"); }

		#endregion Cannot convert for Dictionary<string, string>

	}
}
