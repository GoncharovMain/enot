using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Format
{
	public class Yaml
	{
		private string _src;

		public Yaml(string src)
		{
			_src = src;
		}

		public static void Print()
		{
			var any = new
			{
				url = "http://github.com",
				person = new {
					name = "Goncharov",
					age = 23
				}
			};

			var serializer = new SerializerBuilder()
				.WithNamingConvention(CamelCaseNamingConvention.Instance)
				.Build();

			var yaml = serializer.Serialize(any);
			
			Console.WriteLine(yaml);
		}
	}
}