using NAudio.Wave;
using System;

namespace ImageSonificationProject
{
	public class WaveFileGenerator
	{
		private string _fileName;
		private ProcessingMode _mode;
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

		public WaveFileGenerator(int sampleRate, string fileName, ProcessingMode _mode) : this(sampleRate)
		{
			this._fileName = fileName;
			this._mode = _mode;
		}

		public string SaveWavFile(AudioData[,] audioDataMap)
		{
			_sampleProvider.Data = audioDataMap;
			//string path = @"D:\UNIVERSITY\400\ENSE 479 Sound Art\test.wav";
			string path = _fileName.Substring(0, _fileName.LastIndexOf(".", StringComparison.Ordinal));

			switch (_mode)
			{
				case ProcessingMode.Brightness:
					path += "_Brightness.wav";
					break;
				case ProcessingMode.Darkness:
					path += "_Darkness.wav";
					break;
			}

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
