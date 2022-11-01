namespace RequestYaml
{
	public class Program
	{
		public static async Task Main()
		{
            RequestHandler requestHandler = new RequestHandler("C:\\Users\\yuriy.goncharov\\Desktop\\hu\\goncharov\\enot\\RequestYaml\\request.yaml");

			Response result = await requestHandler.HandleRequestAsync("catalogue");
			
			Console.WriteLine(result.Content.Substring(0, 150));

			result = await requestHandler.HandleRequestAsync("phone-beeline");
			
			Console.WriteLine(result.Content.Substring(0, 150));


            Console.WriteLine();


			// requestHandler.HandleRequest(1);
			// requestHandler.Run();ghp_qZAMZeAefw2IoPmYnyc9TaJm4uIhwi37uuR9
		}
	}
}