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

            #region Catalogue request

            Request requestCatalogue = requestHandler["catalogue"];

            Response responseCatalogue = await requestCatalogue.RequestAsync();

            #endregion Catalogue request


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

            Response responseInternal = await requestInternal.RequestAsync();

            Console.WriteLine(responseInternal.ToString());

            #endregion Internal request


            Request requestOrderId = requestHandler["orderId"];


            //"\"mimeType\": \"application/x-www-form-urlencoded\","
            requestOrderId.Body.PostData = 

            "extAuthFailUri: https://yoomoney.ru/payments/internal/land3ds?orderId=2aed9d53-000f-5000-8000-1913554af8f9&isPaymentShop=true&couldPayByWallet=false&notEnoughMoney=false&isBindCardCheckboxPreselected=false&canCardBindToWallet=false"

            "https://yoomoney.ru/payments/internal/land3ds?orderId=2aed9d53-000f-5000-8000-1913554af8f9&isPaymentShop=true&couldPayByWallet=false&notEnoughMoney=false&isBindCardCheckboxPreselected=false&canCardBindToWallet=false"
            "https://yoomoney.ru/payments/internal/land3ds?orderId=2aed9d53-000f-5000-8000-1913554af8f9&is3dsAuthPassed=true&isPaymentShop=true&couldPayByWallet=false&notEnoughMoney=false&isBindCardCheckboxPreselected=false&canCardBindToWallet=false"
            
            requestOrderId.QueryString["paymentCardSynonym"] = responseInternal.ExpectedFields["paymentCardSynonym"];
            requestOrderId.QueryString["orderId"] = responseInternal.ExpectedFields["orderId"];
            requestOrderId.QueryString["sk"] = responseInternal.ExpectedFields["sk"];

            Response responseOrderIs = await requestOrderId.RequestAsync();
            
            
            #region Request orderId



            #endregion Request orderId

            Console.WriteLine();


			// requestHandler.HandleRequest(1);
			// requestHandler.Run();ghp_qZAMZeAefw2IoPmYnyc9TaJm4uIhwi37uuR9
		}
	}
}