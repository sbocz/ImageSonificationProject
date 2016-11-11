using System.Media;

namespace ImageSonificationProject
{
	public class MediaPlayer : IMediaPlayer
	{
		private SoundPlayer _soundPlayer;
		public MediaPlayer()
		{

		}

		public void Play()
		{
			_soundPlayer.Play();
		}

		public void Pause()
		{
			_soundPlayer.Stop();
		}

		public void SkipBack()
		{
			_soundPlayer.Stop();
		}

		public void Skip()
		{

		}
	}
}
