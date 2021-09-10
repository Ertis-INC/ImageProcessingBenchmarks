using System.IO;
using System.Threading.Tasks;
using System.Linq;
using ImageProcessingBenchmarks.Services.Interfaces;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace ImageProcessingBenchmarks.Services
{
	public class SystemDrawingService : IImageProcessingService
	{
		#region Properties

		public string LibraryName => "System.Drawing";

		#endregion
		
		#region Methods

		public async Task<Stream> ResizeImageAsync(Stream stream, int width, int height)
		{
			return await Task.Run(() => this.ResizeImage(stream, width, height));
		}

		public Stream ResizeImage(Stream stream, int width, int height)
		{
			using (var image = new Bitmap(Image.FromStream(stream)))
			{
				return ResizeImage(image, width, height);
			}
		}

		private static Stream ResizeImage(Image image, int width, int height)
		{
			var resized = new Bitmap(width, height);
			using (var graphics = Graphics.FromImage(resized))
			{
				graphics.CompositingQuality = CompositingQuality.HighSpeed;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.DrawImage(image, 0, 0, width, height);
					
				var qualityParamId = Encoder.Quality;
				var encoderParameters = new EncoderParameters(1)
				{
					Param = {[0] = new EncoderParameter(qualityParamId, 75)}
				};
					
				var outStream = new MemoryStream();
				var decoders = ImageCodecInfo.GetImageDecoders();
				var codec = decoders.FirstOrDefault(decoder => decoder.FormatID == ImageFormat.Jpeg.Guid);
				if (codec != null)
				{
					resized.Save(outStream, codec, encoderParameters);
					outStream.Position = 0L;
					return outStream;
				}
				else
				{
					return null;
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
			using (var image = new Bitmap(Image.FromStream(stream)))
			{
				var originalWidth = image.Width;
				var originalHeight = image.Height;

				return ResizeImage(image, (int)(originalWidth * ratio), (int)(originalHeight * ratio));	
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