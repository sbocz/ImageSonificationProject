using NAudio.Wave;
using System;

namespace ImageSonificationProject
{
	public class ImageSampleProvider32
	{
		//Length in seconds
		private const int ClipLength = 5;


		public ImageSampleProvider32()
		{
		}

		public AudioData[,] Data { get; set; }

		public WaveFormat WaveFormat { get; private set; }

		public int SampleCount
		{
			get { return ClipLength * WaveFormat.SampleRate; }
		}

		public void SetWaveFormat(int sampleRate, int channels)
		{
			WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channels);
		}

		public float[] GetSamples()
		{
			int sampleRate = WaveFormat.SampleRate;
			float[] samples = new float[sampleRate * ClipLength];
			int sample = 0;

			for (int i = 0; i < samples.Length; i++)
			{
				samples[i] = 0;
			}

			for (int i = 0; i < samples.Length; i++)
			{
				//Find relative index value for the current sample
				int x = (int)(i * ((float)ImageProcessor.ImageWidth / samples.Length));

				for (int y = 0; y < ImageProcessor.ImageHeight; y++)
				{
					//Sine wave with correct amplitude
					samples[i] += (float)(Data[x, y].Amplitude * Math.Sin((2 * Math.PI * sample * Data[x, y].Frequency) / sampleRate));

					//Keep sample value in range of samplerate
					sample++;
					if (sample >= sampleRate)
						sample = 0;
				}
			}
			return samples;
		}

	}
}
