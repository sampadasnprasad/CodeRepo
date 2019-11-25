

using System;

namespace AzureStorageTest
{
	public class Program
	{
		static void Main(string[] args)
		{
			try
			{
				var connectionString = "UseDevelopmentStorage=true";
				var blob = new Blob(connectionString, "test", "test1.txt");

				blob.UploadTextAsync("sample text").Wait();
			}
			catch(AggregateException agr)
			{
			Console.WriteLine(agr);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex);
			}
		}
	}
}
