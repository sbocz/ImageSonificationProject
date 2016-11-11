using System.IO;
using System.Windows.Controls;

namespace ImageSonificationProject
{
	public class ImageProcessor
	{
		private ProgressBar progressBar;
		private Stream stream;

		public ImageProcessor(Stream stream)
		{
			this.stream = stream;
		}

		public ImageProcessor(Stream stream, ProgressBar progressBar) : this(stream)
		{
			this.progressBar = progressBar;
		}

		public void Process(ProcessingMode mode)
		{
			throw new System.NotImplementedException();
		}
	}
}
