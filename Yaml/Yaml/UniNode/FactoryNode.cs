using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yaml.UniNode
{
	public class FactoryNode
	{
		public object Value { get; set; }
		public IUniNode UniNode
		{
			get
			{
				if (Value.GetType() == typeof(String))
					return new NodeString { Value = (String)Value };

				if (Value.GetType() == typeof(List<object>))
					return new NodeList { List = ((List<object>)Value)
						.Select(value => (string)value).ToList() };

				if (Value.GetType() == typeof(Dictionary<object, object>))
					return new NodeDictonary { Dict = ((Dictionary<object, object>)Value)
						.ToDictionary(value => (string)value.Key, value => (string)value.Value) };

				return null;
			}
		}
	}
}
