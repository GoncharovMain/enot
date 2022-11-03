namespace RequestYaml
{
	public class Body
	{
		public string Format { get; set; }
		public string MimeType { get; set; }
		public Dictionary<string, string> PostData { get; set; }
	}
}
