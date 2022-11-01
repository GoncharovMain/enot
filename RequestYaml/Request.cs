namespace RequestYaml
{
	public class Request
	{
		public MethodRequest Method { get; set; }
		public string Url { get; set; }
		public Header Headers { get; set; }
		public Body Body { get; set; }
		public Response Response { get; set; }
	}
}
