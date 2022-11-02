namespace RequestYaml
{
    public static class StringExtensions
    {
        public static string GetStrBetweenTags(this string value, string startTag, string endTag, StringComparison stringComparison = StringComparison.CurrentCulture)
        {
            if (value.Contains(startTag) && value.Contains(endTag))
            {
                var startIndex = value.IndexOf(startTag, stringComparison) + startTag.Length;
                var endIndex = value.IndexOf(endTag, startIndex, stringComparison);

                if (startIndex >= endIndex) return "";

                return value[startIndex..endIndex];
            }

            return null;
        }
    }
    public class Program
	{
		
        public static async Task Main()
		{
            YamlRequestCollection requestHandler = new YamlRequestCollection("C:\\Users\\yuriy.goncharov\\Desktop\\hu\\goncharov\\enot\\RequestYaml\\request.yaml");

            Request requestCatalogue = requestHandler["catalogue"];

            Response responseCatalogue = await requestCatalogue.RequestAsync();






            #region Internal request

            Request requestInternal = requestHandler["internal"];

            requestInternal.ParseExpectedFieldsHandler = content =>
                new Dictionary<string, string>()
                {
                    { "sk", content.GetStrBetweenTags("sk=", "&amp;retpath") },
                    { "orderId", content.GetStrBetweenTags("orderId=", "&amp;preselectedPaymentType") }
                };

            requestInternal.ParseCookieHandler = header => header.GetValues("set-cookie")
				.Select(cookie => cookie.Split(new char[] { '=', ';' }, 3))
				.ToDictionary(cookie => cookie[0], cookie => cookie[1]);

            Response responseInternal = await requestHandler.RequestAsync("internal");

            #endregion Internal request


            Console.WriteLine();


			// requestHandler.HandleRequest(1);
			// requestHandler.Run();ghp_qZAMZeAefw2IoPmYnyc9TaJm4uIhwi37uuR9
		}
	}
}