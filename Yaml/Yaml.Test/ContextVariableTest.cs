using Xunit;
using Yaml.UniNode;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

using YamlDotNet.Core;
using System.Security.Cryptography;

namespace Yaml.Test
{
	public class ContextVariableTest
	{
		public string _directoryResources;

		public Dictionary<string, string> Directories { get; set; }
		public Dictionary<string, string> YamlText { get; set; }

		private IDeserializer _deserializer {  get; set; }

		public ContextVariableTest()
		{
			_directoryResources = Directory.GetCurrentDirectory()
											.Replace("bin\\Debug\\net6.0", "Resources");

			_deserializer = new DeserializerBuilder()
					.WithNamingConvention(CamelCaseNamingConvention.Instance)
					.Build();
			
			Directories = new Dictionary<string, string>()
			{
				{ "request-simple", $"{_directoryResources}/request-simple.yaml" },
				{ "request-not-valid-symbols", $"{_directoryResources}/request-not-valid-symbols.yaml" },
				{ "request-double-brace", $"{_directoryResources}/request-double-brace.yaml" },
				{ "request-context-variable-all", $"{_directoryResources}/request-context-variable-all.yaml" }
			};

			YamlText = Directories.ToDictionary(
				directory => directory.Key, 
				directory => File.ReadAllText(directory.Value));
		}

		[Fact]
		public void StaticValues()
		{
			DataRequest requests = _deserializer.Deserialize<DataRequest>(YamlText["request-simple"]);
			
			Assert.Equal("4", requests.Requests["google"].Header);
			Assert.Equal("6", requests.Requests["yandex"].Header);
			Assert.Equal("8", requests.Requests["vk"].Header);
		}

		[Fact]
		public void NotValidSymbols()
			=> Assert.Throws<YamlException>(
				new Action(() =>
				{
					_deserializer.Deserialize<DataRequest>(YamlText["request-not-valid-symbols"]);
				}));

		[Fact]
		public void DoubleBrace()
			=> Assert.Throws<YamlException>(
				new Action(() =>
				{
					_deserializer.Deserialize<DataRequest>(YamlText["request-double-brace"]);
				}));
		
		[Fact]
		public void DynamicVariableType()
		{
			UniDataRequest uniDataRequest = _deserializer.Deserialize<UniDataRequest>(YamlText["request-context-variable-all"]);

			Assert.IsType<NodeString>(uniDataRequest.Requests["google"].Header.UniNode);
			Assert.IsType<NodeList>(uniDataRequest.Requests["yandex"].Header.UniNode);
			Assert.IsType<NodeDictonary>(uniDataRequest.Requests["vk"].Header.UniNode);
		}

		[Fact]
		public void DynamicVariableValues()
		{	
			UniDataRequest uniDataRequest = _deserializer.Deserialize<UniDataRequest>(YamlText["request-context-variable-all"]);

			Assert.Equal("dynamic", uniDataRequest.Requests["google"].Header.UniNode.Value);

			for (int i = 0; i < 3; i++)
				Assert.Equal($"value {i + 1}", uniDataRequest.Requests["yandex"].Header.UniNode[i]);


			Assert.Equal("John", uniDataRequest.Requests["vk"].Header.UniNode["name"]);
			Assert.Equal("33", uniDataRequest.Requests["vk"].Header.UniNode["age"]);
			Assert.Equal("male", uniDataRequest.Requests["vk"].Header.UniNode["sex"]);
		}

		[Fact]
		public void ExceptionsVariableValues()
		{
			UniDataRequest uniDataRequest = _deserializer.Deserialize<UniDataRequest>(YamlText["request-context-variable-all"]);

			#region for String

			for (int i = 0; i < 3; i++)
				Assert.Null(uniDataRequest.Requests["google"].Header.UniNode[i]);

			new List<string> { "name", "age", "sex" }
				.ForEach(name => Assert.Null(uniDataRequest.Requests["google"].Header.UniNode[name]));

			#endregion for String


			#region for List

			Assert.Null(uniDataRequest.Requests["yandex"].Header.UniNode.Value);

			new List<string> { "name", "age", "sex" }
				.ForEach(name => Assert.Null(uniDataRequest.Requests["yandex"].Header.UniNode[name]));

			#endregion for List


			#region for Dictionary

			Assert.Null(uniDataRequest.Requests["vk"].Header.UniNode.Value);

			for (int i = 0; i < 3; i++)
				Assert.Null(uniDataRequest.Requests["vk"].Header.UniNode[i]);

			#endregion for Dictionary
		}

		[Fact]
		public void ExceptionTryChange()
		{
			UniDataRequest uniDataRequest = _deserializer.Deserialize<UniDataRequest>(YamlText["request-context-variable-all"]);

			for (int i = 0; i < 3; i++)
				Assert.Throws<Exception>(new Action(() => 
					{ 
						uniDataRequest.Requests["google"].Header.UniNode[i] = "Not empty"; 
					}));


			Assert.Throws<Exception>(new Action(() =>
			{
				uniDataRequest.Requests["google"].Header.UniNode["name"] = "Not Empty";
			}));

			Assert.Throws<Exception>(new Action(() =>
			{
				uniDataRequest.Requests["google"].Header.UniNode["age"] = "Not Empty";
			}));

			Assert.Throws<Exception>(new Action(() =>
			{
				uniDataRequest.Requests["google"].Header.UniNode["sex"] = "Not Empty";
			}));
		}
	}
}