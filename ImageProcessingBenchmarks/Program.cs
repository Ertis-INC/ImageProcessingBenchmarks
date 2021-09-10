using System;
using System.Collections.Generic;
using System.IO;
using ImageProcessingBenchmarks.Services;
using Newtonsoft.Json;

namespace ImageProcessingBenchmarks
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			var benchmarks = new[]
			{
				new BenchmarkTest(new SkiaSharpService()),
				new BenchmarkTest(new ImageSharpService()),
				new BenchmarkTest(new SystemDrawingService())
			};

			var results = new List<object>();
			foreach (var benchmarkTest in benchmarks)
			{
				benchmarkTest.Run();
				results.Add(new
				{
					library = benchmarkTest.LibraryName,
					results = benchmarkTest.Results
				});
			}

			var json = JsonConvert.SerializeObject(results, Formatting.Indented);
			File.WriteAllText($"{Environment.CurrentDirectory}/../../../benchmark_result_{DateTime.Now:ddMMYYYYHHmm}.json", json);
			
			Console.WriteLine("Finish");
		}
	}
}