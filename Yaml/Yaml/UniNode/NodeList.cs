using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yaml.UniNode
{
	public class NodeList : IUniNode
	{
		public List<string> List { get; set; }
		public string this[int index] { get => List[index]; set => List[index] = value; }

		#region Cannot convert for List<string>

		public string Value { get => null; set => throw new Exception("Cannot convert to String"); }
		public string this[string name] { get => null; set => throw new Exception("Cannot convert to Dictionary<string, string>"); }

		#endregion Cannot convert for List<string>
	}
}
