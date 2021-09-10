using System.IO;
using System.Threading.Tasks;
using ImageProcessingBenchmarks.Services.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ImageProcessingBenchmarks.Services
{
	public class ImageSharpService : IImageProcessingService
	{
		#region Properties

		public string LibraryName => "ImageSharp";

		#endregion
		
		#region Methods

		public Stream ResizeImage(Stream stream, int width, int height) =>
			this.ResizeImageAsync(stream, width, height).ConfigureAwait(false).GetAwaiter().GetResult();

		public async Task<Stream> ResizeImageAsync(Stream stream, int width, int height)
		{
			var outStream = new MemoryStream();
			using (var image = Image.Load(stream, out var format))
			{
				var clone = image.Clone(i => i.Resize(width, height));
				await clone.SaveAsync(outStream, format);
			}

			outStream.Position = 0L;
			return outStream;
		}
		
		public Stream ResizeImage(byte[] bytes, int width, int height) =>
			this.ResizeImageAsync(bytes, width, height).ConfigureAwait(false).GetAwaiter().GetResult();

		public async Task<Stream> ResizeImageAsync(byte[] bytes, int width, int height)
		{
			await using (var stream = new MemoryStream(bytes))
			{
				return await this.ResizeImageAsync(stream, width, height);
			}
		}
		
		public Stream ResizeImage(Stream stream, double ratio) =>
			this.ResizeImageAsync(stream, ratio).ConfigureAwait(false).GetAwaiter().GetResult();

		public async Task<Stream> ResizeImageAsync(Stream stream, double ratio)
		{
			using (var image = Image.Load(stream, out _))
			{
				var originalWidth = image.Width;
				var originalHeight = image.Height;

				stream.Position = 0L;
				return await this.ResizeImageAsync(stream, (int)(originalWidth * ratio), (int)(originalHeight * ratio));
			}
		}
		
		public Stream ResizeImage(byte[] bytes, double ratio) =>
			this.ResizeImageAsync(bytes, ratio).ConfigureAwait(false).GetAwaiter().GetResult();

		public async Task<Stream> ResizeImageAsync(byte[] bytes, double ratio)
		{
			await using (var stream = new MemoryStream(bytes))
			{
				return await this.ResizeImageAsync(stream, ratio);
			}	
		}

		#endregion
	}
}