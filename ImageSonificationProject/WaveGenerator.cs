namespace ImageSonificationProject
{
	public class WaveGenerator
	{
		/*
		 * Input: Peak amplitude (A), Frequency (f)
		 * Output: Amplitude value (y)
		 * 
		 * y = A * sin(phase)
		 * phase = phase + ((2 * pi * f) / samplerate)
		 * if phase > (2 * pi) then 
		 *		phase = phase - (2 * pi)
		 */
		private int _sampleRate;

		public WaveGenerator(int sampleRate)
		{
			_sampleRate = sampleRate;
		}

		public string SaveWavFile(AudioData[,] audioDataMap)
		{
			throw new System.NotImplementedException();
		}
	}
}
