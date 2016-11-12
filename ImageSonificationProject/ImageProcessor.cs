using System.Drawing;
using System.IO;
using System.Windows.Controls;

namespace ImageSonificationProject
{
	public class ImageProcessor
	{
		public const int ImageHeight = 300;
		public const int ImageWidth = 400;
		public const int MinimumFrequency = 50;
		public const int MaximumFrequency = 20000;

		//Image stretched to ImageWidth x ImageHeight
		private readonly Bitmap _image;
		private readonly ProgressBar _progressBar;

		public ImageProcessor(Stream stream, ProgressBar progressBar)
		{
			//Stretch it out
			_image = new Bitmap(new Bitmap(stream), new Size(ImageWidth, ImageHeight));

			//Flip for processing so that 0 means bottom of y axis
			//Turns image upside down with pixels in the same column as before flip
			_image.RotateFlip(RotateFlipType.Rotate180FlipX);

			_progressBar = progressBar;
		}

		/// <summary>
		/// Iterates through each pixel and processes the data into a map of the resulting frequency and amplitude for each pixel
		/// </summary>
		/// <param name="mode"></param>
		/// <returns>2D array of audio data that matches the image pixels</returns>
		public AudioData[,] Process(ProcessingMode mode)
		{
			_progressBar.Value = 0;
			_progressBar.Maximum = ImageWidth;

			var data = new AudioData[ImageWidth, ImageHeight];

			for (int i = 0; i < ImageWidth; i++)
			{
				for (int j = 0; j < ImageHeight; j++)
				{
					//Data from each pixel mapped to output
					data[i, j] = ProcessPixel(i, j, mode);
				}
				_progressBar.Value++;
			}

			return data;
		}

		/// <summary>
		/// Calculates the corresponding amplitude and frequency of a pixel from _image.
		/// </summary>
		/// <param name="x">x-coordinate of pixel</param>
		/// <param name="y">y-coordinate of pixel</param>
		/// <param name="mode"></param>
		/// <returns>Data object with amplitude and frequency</returns>
		private AudioData ProcessPixel(int x, int y, ProcessingMode mode)
		{
			var pixelColor = _image.GetPixel(x, y);
			float amplitudeFactor = GetAmplitudeForColorAndMode(pixelColor, mode);
			float heightFactor = (float)y / (float)ImageHeight;

			//FOR LINEAR CHANGE IN FREQUENCY WITH HEIGHT(likely will change)
			int frequency = (int)(heightFactor * (MaximumFrequency - MinimumFrequency) + MinimumFrequency);

			return new AudioData()
			{
				Frequency = frequency,
				Amplitude = amplitudeFactor
			};

		}

		/// <summary>
		/// Gets the percentage of the maximum amplitude a color should output
		/// </summary>
		/// <param name="pixelColor"></param>
		/// <param name="mode"></param>
		/// <returns></returns>
		private float GetAmplitudeForColorAndMode(Color pixelColor, ProcessingMode mode)
		{
			switch (mode)
			{
				case ProcessingMode.Brightness:
					return pixelColor.GetBrightness();
				case ProcessingMode.Mode2:
					break;
				case ProcessingMode.Mode3:
					break;
				case ProcessingMode.Mode4:
					break;
			}
			return 0;
		}
	}
}
