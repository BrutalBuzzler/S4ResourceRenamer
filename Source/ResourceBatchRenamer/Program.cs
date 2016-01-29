using System;
using System.Globalization;
using System.IO;

namespace ResourceBatchRenamer
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			try
			{
                var renamer = new FileRenamer(new FileWrapper(), new FileNameConverter(), args);
			    renamer.Rename();
			}
			catch (Exception e)
			{
				
				WriteToErrorLog(e);
			}
		}

	    private static void WriteToErrorLog(Exception exception)
		{
			using (var stream = File.Open("ResourceBatchRenamer.log", FileMode.Append, FileAccess.Write, FileShare.Read))
			{
				var writer = new StreamWriter(stream);
				writer.WriteLine();
				writer.WriteLine();
				writer.WriteLine("**** {0} ****", DateTime.Now.ToString(CultureInfo.InvariantCulture));
				writer.WriteLine("An error occured during execution: {0} - {1}", exception.GetType().Name, exception.Message);
				writer.WriteLine(exception.StackTrace);
			}
		}
	}
}