namespace RequestYaml
{
	public class Body
	{
		private PostData _postData;
		public string MimeType { get; set; }
		public PostData PostData 
		{ 
			get => _postData; 
			set
			{
				switch(MimeType)
				{
					case "application/x-www-form-urlencoded": _postData = new FormUrlEncodedData(); break;
					case "application/json; charset=UTF-8": _postData = new JsonEncodedData(); break;
					case "string": _postData = new StringData(); break;
					default: _postData = new PostData(); break;
                }
			} 
		}
	}

	public class PostData
	{
		private string Content { get; set; }
    }

	public class FormUrlEncodedData : PostData
    {
		
	}

	public class JsonEncodedData : PostData
    {

	}

	public class StringData : PostData
    {

	}
}
