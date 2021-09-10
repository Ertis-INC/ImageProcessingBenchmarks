using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ImageProcessingBenchmarks.Services.Interfaces;

namespace ImageProcessingBenchmarks
{
	public class BenchmarkTest
	{
		#region Constants

		private static readonly string[] KnownImageExtensions =
		{
			".bmp",
			".jpg",
			".jpeg",
			".gif",
			".tiff",
			".png"
		};

		#endregion
		
		#region Services

		private readonly IImageProcessingService imageProcessingService;

		#endregion

		#region Properties

		public string LibraryName => this.imageProcessingService.LibraryName;
		
		public string ResourcesDirectoryPath { get; } = $"{Environment.CurrentDirectory}/../../../Resources";
		
		public List<BenchmarkResult> Results { get; }

		#endregion

		#region Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="imageProcessingService"></param>
		public BenchmarkTest(IImageProcessingService imageProcessingService)
		{
			this.imageProcessingService = imageProcessingService;
			this.Results = new List<BenchmarkResult>();
		}

		#endregion

		#region Methods

		public void Run()
		{
			Console.WriteLine($"{this.LibraryName} test running...");
			
			this.PrepareDirectories();
			
			var files = Directory.GetFiles(this.ResourcesDirectoryPath);
			foreach (var filePath in files)
			{
				var fileInfo = new FileInfo(filePath);
				if (KnownImageExtensions.Contains(fileInfo.Extension.ToLower()))
				{
					this.Results.Add(this.RunSingle(fileInfo, 0.5d));
					this.Results.Add(this.RunSingle(fileInfo, 2.0d));	
				}
			}
			
			Console.WriteLine($"{this.LibraryName} test completed");
		}

		private void PrepareDirectories(bool clearDestinationDirectory = false)
		{
			if (!Directory.Exists(this.ResourcesDirectoryPath))
			{
				throw new DirectoryNotFoundException("Resource directory is not exist!");
			}
			else if (!Directory.Exists($"{this.ResourcesDirectoryPath}/Results"))
			{
				Directory.CreateDirectory($"{this.ResourcesDirectoryPath}/Results");
			}
			else if (clearDestinationDirectory)
			{
				Directory.Delete($"{this.ResourcesDirectoryPath}/Results");
				Directory.CreateDirectory($"{this.ResourcesDirectoryPath}/Results");
			}
		}

		// ReSharper disable once UnusedMember.Local
		private BenchmarkResult RunSingle(string filePath, double ratio)
		{
			return this.RunSingle(new FileInfo(filePath), ratio);
		}
		
		private BenchmarkResult RunSingle(FileInfo fileInfo, double ratio)
		{
			var result = new BenchmarkResult
			{
				LibraryName = this.imageProcessingService.LibraryName,
				FileName = fileInfo.Name,
				FileFormat = fileInfo.Extension,
				FileSize = fileInfo.Length,
				Ratio = ratio,
			};
				
			Console.WriteLine($"{fileInfo.Name} resizing with {ratio} ratio");
			
			try
			{
				using (var fileStream = new FileStream(fileInfo.FullName, FileMode.Open))
				{
					Stream resizedStream = null; 
					var elapsedTime = CalculateExecutionTime(() =>
					{
						// ReSharper disable once AccessToDisposedClosure
						resizedStream = this.imageProcessingService.ResizeImage(fileStream, ratio);
					});

					result.ElapsedTime = elapsedTime.TotalMilliseconds;
					
					if (resizedStream != null)
					{
						SaveFile(resizedStream, $"{this.ResourcesDirectoryPath}/Results/{fileInfo.Name}");
						result.IsSuccess = true;
					}
					else
					{
						result.IsSuccess = false;
						result.Message = "Resize failed";
					}
				}
			}
			catch (Exception ex)
			{
				result.IsSuccess = false;
				result.Message = ex.Message;
			}

			if (result.IsSuccess)
			{
				Console.WriteLine($"{fileInfo.Name} resizing completed");
			}
			else
			{
				Console.WriteLine($"{fileInfo.Name} resizing failed");	
			}

			return result;
		}

		private static TimeSpan CalculateExecutionTime(Action action)
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();
			action();
			stopwatch.Stop();
			return stopwatch.Elapsed;
		}

		private static void SaveFile(Stream stream, string path)
		{
			using (var fileStream = File.Create(path, (int)stream.Length))
			{
				byte[] bytesInStream = new byte[stream.Length];
				stream.Read(bytesInStream, 0, bytesInStream.Length);
				fileStream.Write(bytesInStream, 0, bytesInStream.Length);
				fileStream.Close();
			}
		}
		
		#endregion
	}
}