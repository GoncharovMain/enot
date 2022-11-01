namespace RequestYaml
{
	public class Program
	{
		public static async Task Main()
		{
            RequestHandler requestHandler = new RequestHandler("C:\\Users\\yuriy.goncharov\\Desktop\\hu\\goncharov\\enot\\RequestYaml\\request.yaml");

			//RequestHandler requestHandler = new RequestHandler();
			//requestHandler.WriteYamlAsync();

            Response catalogue = await requestHandler.HandleRequestAsync("catalogue");
			
			Console.WriteLine(catalogue.Content.Substring(0, 150));



			requestHandler["catalogue"].Cookie = catalogue.Cookie;

            Response phoneBeeline = await requestHandler.HandleRequestAsync("phone-beeline");
			
			Console.WriteLine(phoneBeeline.Content.Substring(0, 150));

			Response paymentsInternal = await requestHandler.HandleRequestAsync("internal");

			string qwe = paymentsInternal?.ReturnFolders["sk"] ?? "Null";


			Console.WriteLine($"paymentsInternal : {paymentsInternal.Content.Substring(0, 200)}");



            Console.WriteLine();


			// requestHandler.HandleRequest(1);
			// requestHandler.Run();ghp_qZAMZeAefw2IoPmYnyc9TaJm4uIhwi37uuR9
		}
	}
}