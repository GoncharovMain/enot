namespace RequestYaml
{
	public class Program
	{
		public static async Task Main()
		{

            
            RequestHandler requestHandler = new RequestHandler("C:\\Users\\yuriy.goncharov\\Desktop\\hu\\goncharov\\enot\\RequestYaml\\request.yaml");
            //RequestHandler requestHandler = new RequestHandler("request.yaml");

			Response result = await requestHandler.HandleRequestAsync("catalogue");
			
			Console.WriteLine(result.Content.Substring(0, 150));


			result = await requestHandler.HandleRequestAsync("phone-beeline");
			
			Console.WriteLine(result.Content.Substring(0, 150));


			// requestHandler.HandleRequest(1);
			// requestHandler.Run();
		}
	}
}