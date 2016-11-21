using NAudio.Wave;

namespace ImageSonificationProject
{
	public class WaveFileGenerator
	{
		private readonly ImageSampleProvider32 _sampleProvider;

		/// <summary>
		/// Creates a new Wave File Generator with the sampleRate in Hz
		/// </summary>
		/// <param name="sampleRate"></param>
		public WaveFileGenerator(int sampleRate)
		{
			_sampleProvider = new ImageSampleProvider32();

			//Monochannel
			_sampleProvider.SetWaveFormat(sampleRate, 1);
		}

		public string SaveWavFile(AudioData[,] audioDataMap)
		{
			_sampleProvider.Data = audioDataMap;
			string path = @"D:\UNIVERSITY\400\ENSE 479 Sound Art\test.wav";

			//Write each sample to file
			using (WaveFileWriter writer = new WaveFileWriter(path, _sampleProvider.WaveFormat))
			{
				var samples = _sampleProvider.GetSamples();
				foreach (float sample in samples)
				{
					writer.WriteSample(sample);
				}
			}

			return path;
		}
	}
}
