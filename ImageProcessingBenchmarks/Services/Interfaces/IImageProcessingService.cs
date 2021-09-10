using System.IO;
using System.Threading.Tasks;

namespace ImageProcessingBenchmarks.Services.Interfaces
{
	public interface IImageProcessingService
	{
		string LibraryName { get; }
		
		Stream ResizeImage(Stream stream, int width, int height);

		Task<Stream> ResizeImageAsync(Stream stream, int width, int height);
		
		Stream ResizeImage(byte[] bytes, int width, int height);

		Task<Stream> ResizeImageAsync(byte[] bytes, int width, int height);
		
		Stream ResizeImage(Stream stream, double ratio);

		Task<Stream> ResizeImageAsync(Stream stream, double ratio);
		
		Stream ResizeImage(byte[] bytes, double ratio);

		Task<Stream> ResizeImageAsync(byte[] bytes, double ratio);
	}
}