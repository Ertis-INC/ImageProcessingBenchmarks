namespace ImageProcessingBenchmarks
{
	public class BenchmarkResult
	{
		#region Properties

		public string LibraryName { get; set; }
		
		public string FileName { get; set; }
		
		public string FileFormat { get; set; }
		
		public long FileSize { get; set; }

		public double Ratio { get; set; }
		
		public double ElapsedTime { get; set; }
		
		public bool IsSuccess { get; set; }
		
		public string Message { get; set; }

		#endregion
	}
}