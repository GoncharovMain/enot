using System.Net.Http.Headers;
using System.Reflection.PortableExecutable;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Reflection;

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

        public static string TryReplacePseudo(this string current, string variable, string replacement)
        {
            int leftIndex = current.IndexOf('{');
            int rightIndex = current.IndexOf('}') + 1;

            string substringReplace = current.Substring(leftIndex, rightIndex - leftIndex);

            string store = "replace";

            if (substringReplace.Contains(variable))
                return current.Replace($"{substringReplace}", replacement);

            return current;
        }

        public static string FirstUpper(this string str)
        {
            return str.Substring(0, 1).ToUpper() + (str.Length > 1 ? str.Substring(1) : "");
        }
    }
    public class Program
	{
        public static class FieldsParser
        {
            public static Dictionary<string, string> ParseExpectedFieldsInternal(string content)
            => new Dictionary<string, string>()
                {
                    { "sk", content.GetStrBetweenTags("sk=", "&amp;retpath") },
                    { "orderId", content.GetStrBetweenTags("orderId=", "&amp;preselectedPaymentType") }
                };

            public static Dictionary<string, string> ParseCookieInternal(HttpResponseHeaders headers)
                => headers.GetValues("set-cookie")
                    .Select(cookie => cookie.Split(new char[] { '=', ';' }, 3))
                    .ToDictionary(cookie => cookie[0], cookie => cookie[1]);

            public static Dictionary<string, string> ParseExpectedFieldsStorecard(string content)
            {
                Console.WriteLine(content);
                var jsonContent = JsonConvert.DeserializeObject<JObject>(content);

                return new Dictionary<string, string>
                {
                    { "status", jsonContent["status"].ToString() },
                    { "cardSynonym", jsonContent["result"]["cardSynonym"].ToString() },
                };
            }
        }

        public static async void REQUEST()
        {
            YamlRequestCollection yamlRequestCollection = new YamlRequestCollection("C:\\Users\\yuriy.goncharov\\Desktop\\hu\\goncharov\\enot\\RequestYaml\\request.yaml");

            #region Internal request
            Request internalRequest = yamlRequestCollection["internal"];

            internalRequest.ParseCookieHandler = FieldsParser.ParseCookieInternal;
            internalRequest.ParseExpectedFieldsHandler = FieldsParser.ParseExpectedFieldsInternal;

            Response internalResponse = await internalRequest.RequestAsync();
            #endregion Internal request


            #region Storecard request
            Request storecardRequest = yamlRequestCollection["storecard"];

            storecardRequest.ParseExpectedFieldsHandler = FieldsParser.ParseExpectedFieldsStorecard;

            Response storecardResponse = await storecardRequest.RequestAsync();
            #endregion Storecard request

            //`internal.headers.origin`
            #region OrderId request

            Request orderIdRequest = yamlRequestCollection["orderId"];

            orderIdRequest.Body.PostData["sk"] = internalResponse.ExpectedFields["sk"];
            orderIdRequest.Body.PostData["paymentCardSynonym"] = storecardResponse.ExpectedFields["cardSynonym"];
            orderIdRequest.Body.PostData["extAuthFailUri"] = $"https://yoomoney.ru/payments/internal/land3ds?orderId={internalResponse.ExpectedFields["orderId"]}&isPaymentShop=true&couldPayByWallet=false&notEnoughMoney=false&isBindCardCheckboxPreselected=false&canCardBindToWallet=false";
            orderIdRequest.Body.PostData["extAuthSuccessUri"] = $"https://yoomoney.ru/payments/internal/land3ds?orderId={internalResponse.ExpectedFields["orderId"]}&is3dsAuthPassed=true&isPaymentShop=true&couldPayByWallet=false&notEnoughMoney=false&isBindCardCheckboxPreselected=false&canCardBindToWallet=false";

            Response orderIdResponse = await orderIdRequest.RequestAsync();

            #endregion OrderId request

            Console.WriteLine();
        }

        public static object GetNextObject(object obj, string nameProperty)
        {
            Type type = obj.GetType();

            if (type == typeof(Dictionary<string, string>))
            {
                return ((Dictionary<string, string>)obj)[nameProperty]);
            }

            PropertyInfo[] propertyInfos = type.GetProperties();



            return "";
        }

        public static bool CompareStrings(string left, string right)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;

            if (left != right)
                Console.ForegroundColor = ConsoleColor.DarkRed;

            Console.WriteLine($"'{left}' == '{right}' => {left == right}");

            Console.ForegroundColor = ConsoleColor.Gray;

            return left == right;
        }
        public static async Task Main()
        {

            CompareStrings("orderId={internal}".TryReplacePseudo("internal", "IU243-4234O-32434"), "orderId=IU243-4234O-32434");
            CompareStrings("sk={sk}".TryReplacePseudo("sk", "5ui9g35847h6uuytr..."), "sk=5ui9g35847h6uuytr...");
            CompareStrings("synonym={synonym} qwerqtrw".TryReplacePseudo("synonym", "124098"), "synonym=124098 qwerqtrw");

            CompareStrings("asdf", "asdf");





            CompareStrings("asdf", "asdf");




            YamlRequestCollection yamlRequestCollection = new YamlRequestCollection("C:\\Users\\yuriy.goncharov\\Desktop\\hu\\goncharov\\enot\\RequestYaml\\request.yaml");

            string sequence = "internal.response.expectedField.sk";

            string[] objectCollection = sequence.Split('.');

            string nameRequest = objectCollection[0];


            Request request = yamlRequestCollection[nameRequest];

            // get list properties

            PropertyInfo[] propertyInfos = request.GetType().GetProperties();

            foreach(PropertyInfo propertyInfo in propertyInfos)
            {
                Console.WriteLine(propertyInfo.Name.ToLower());
            }

            string nameProperty = objectCollection[1].FirstUpper();

            int indexTotalProperty = propertyInfos.Select(property => property.Name).ToList().IndexOf(nameProperty);

            object second = propertyInfos[indexTotalProperty].GetValue(request);

            Type typeProperty = second.GetType();


            Console.WriteLine(second);

            Console.WriteLine(second);
        }
	}
}