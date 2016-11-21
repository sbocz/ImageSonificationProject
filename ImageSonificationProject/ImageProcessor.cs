using System;
using System.Drawing;
using System.IO;
using System.Windows.Controls;

namespace ImageSonificationProject
{
	public class ImageProcessor
	{
		public const int ImageHeight = 300;
		public const int ImageWidth = 400;

		//Image stretched to ImageWidth x ImageHeight
		private readonly Bitmap _image;
		private readonly ProgressBar _progressBar;
		private readonly float[] _frequencies = new float[ImageHeight];
		private const float BaseFrequency = 440;

		public ImageProcessor(Stream stream, ProgressBar progressBar)
		{
			//Stretch it out
			_image = new Bitmap(new Bitmap(stream), new Size(ImageWidth, ImageHeight));

			//Math stuff http://www.phy.mtu.edu/~suits/NoteFreqCalcs.html
			//We want frequency change to sound linear as we move up the image
			var a = Math.Pow(2, (double)1 / 12);
			for (int i = 0; i < ImageHeight; i++)
			{
				float n = i * ((float)1 / 3) - 35;
				_frequencies[i] = (float)(BaseFrequency * Math.Pow(a, n)) - 50;
			}
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

			//Use frequency table
			int frequency = (int)_frequencies[y];

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
				case ProcessingMode.Darkness:
					return 1 - pixelColor.GetBrightness();
				case ProcessingMode.Mode3:
					break;
				case ProcessingMode.Mode4:
					break;
			}
			return 0;
		}
	}
}
