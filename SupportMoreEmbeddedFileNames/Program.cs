using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.FileProviders;

namespace SupportMoreEmbeddedFileNames
{
	public class Program
	{
		private static readonly string[] PleaseSupportThis =
		{
			// This case is already supported.
			"dir\\file -..123.txt",
			// Not supported.
			"dir -..123\\file -..123.txt",
			"dir\\..\\dir.txt"
		};

		public static void Main()
		{
			var assembly = typeof(Program).GetTypeInfo().Assembly;
			var fileProvider = new EmbeddedFileProvider(assembly);

			foreach (var path in PleaseSupportThis)
			{
				Console.WriteLine(path);
				var fileInfo = fileProvider.GetFileInfo(path);
				Console.WriteLine(fileInfo.Name);
				try
				{
					using (var stream = fileInfo.CreateReadStream())
					using (var reader = new StreamReader(stream))
						Console.WriteLine(reader.ReadToEnd());
				}
				catch (FileNotFoundException exception)
				{
					Console.WriteLine(exception.Message);
				}
				Console.WriteLine();
			}

			Console.WriteLine("Actual embedded file names:");
			foreach (var resourceName in assembly.GetManifestResourceNames())
				Console.WriteLine(resourceName);
			Console.ReadKey();
		}
	}
}
