using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yaml.UniNode
{
	public class NodeString : IUniNode
	{
		public string Value { get; set; }

		public static implicit operator string(NodeString nodeString)
			=> nodeString.Value;

		#region Cannot convert for String

		public string this[int index] { get => null; set => throw new Exception("Cannot convert to List<string>"); }
		public string this[string name] { get => null; set => throw new Exception("Cannot convert to Dictionary<string, string>"); }

		#endregion Cannot convert for String
	}
}
