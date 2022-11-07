using System;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

#nullable disable

namespace Format
{
	public class Request
	{
		public string Header { get; set; }
		public string[] Body { get; set; }
		public Dictionary<string, string> Response { get; set; }

		public Field Other { get; set; }
	}

	public class Field
	{
		public object Value { get; set; }


		public static implicit operator String(Field field) => (string)field.Value;

		public string this[int index]
		{
			get
			{
				Console.WriteLine(Value.GetType());
				List<string> value = ((List<string>)Value);
				return value[index];
			}
		}

		public string this[string key]
		{
			get
			{
				Console.WriteLine(Value.GetType());
				Dictionary<string, string> dict = ((Dictionary<string, string>)Value);

				return dict[key];
			}
		}
	}

	public class Uni
	{
		private string _path;

		private string _yamlText;

		public Request Request { get; set; }

		public Uni()
		{
			_path = "universe.yaml";

			using (StreamReader reader = new StreamReader(_path))
			{
				_yamlText = reader.ReadToEnd();

				var deserializer = new DeserializerBuilder()
					.WithNamingConvention(CamelCaseNamingConvention.Instance)
					.Build();

				Request = deserializer.Deserialize<Request>(_yamlText);
			}
		}

	}

	public class Program
	{
		public static void Main()
		{
			Uni uni = new Uni();

			Console.WriteLine(uni.Request.Other["key1"]); // => a

		}
	}
}

/*

type string

field: string line



type list, array

field: [empty string]
  - a
  - b
  - c


type dictionary

field: [empty string]
  key1: value1
  key2: value2
  key3: value3



3 var
1. string -> after ':' is not empty
2. list -> after ':' is empty and second line have first symbol '-'
3. dictionary -> have ':' in second line

*/