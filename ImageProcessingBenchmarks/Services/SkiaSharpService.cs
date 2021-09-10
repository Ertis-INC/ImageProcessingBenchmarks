using System.IO;
using System.Threading.Tasks;
using ImageProcessingBenchmarks.Services.Interfaces;
using SkiaSharp;

namespace ImageProcessingBenchmarks.Services
{
	public class SkiaSharpService : IImageProcessingService
	{
		#region Properties

		public string LibraryName => "SkiaSharp";

		#endregion
		
		#region Methods

		public async Task<Stream> ResizeImageAsync(Stream stream, int width, int height)
		{
			return await Task.Run(() => this.ResizeImage(stream, width, height));
		}

		public Stream ResizeImage(Stream stream, int width, int height)
		{
			using (var sourceBitmap = SKBitmap.Decode(stream))
			{
				return ResizeImage(sourceBitmap, width, height);
			}
		}
		
		private static Stream ResizeImage(SKBitmap sourceBitmap, int width, int height)
		{
			using (var scaledBitmap = sourceBitmap.Resize(new SKImageInfo(width, height), SKFilterQuality.None))
			{
				using (var scaledImage = SKImage.FromBitmap(scaledBitmap))
				{
					using (var data = scaledImage.Encode())
					{
						return data.AsStream();
					}
				}
			}
		}
		
		public async Task<Stream> ResizeImageAsync(byte[] bytes, int width, int height)
		{
			return await Task.Run(() => this.ResizeImage(bytes, width, height));
		}

		public Stream ResizeImage(byte[] bytes, int width, int height)
		{
			using (var stream = new MemoryStream(bytes))
			{
				return this.ResizeImage(stream, width, height);
			}
		}
		
		public async Task<Stream> ResizeImageAsync(Stream stream, double ratio)
		{
			return await Task.Run(() => this.ResizeImage(stream, ratio));
		}

		public Stream ResizeImage(Stream stream, double ratio)
		{
			using (var sourceBitmap = SKBitmap.Decode(stream))
			{
				var originalWidth = sourceBitmap.Width;
				var originalHeight = sourceBitmap.Height;

				return ResizeImage(sourceBitmap, (int)(originalWidth * ratio), (int)(originalHeight * ratio));
			}
		}
		
		public async Task<Stream> ResizeImageAsync(byte[] bytes, double ratio)
		{
			return await Task.Run(() => this.ResizeImage(bytes, ratio));
		}

		public Stream ResizeImage(byte[] bytes, double ratio)
		{
			using (var stream = new MemoryStream(bytes))
			{
				return this.ResizeImage(stream, ratio);
			}
		}

		#endregion
	}
}