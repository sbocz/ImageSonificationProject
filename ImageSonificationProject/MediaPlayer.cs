using NAudio.Wave;
using System;
using System.IO;
using System.Linq;

namespace ImageSonificationProject
{
	public class MediaPlayer : IMediaPlayer
	{
		private IWavePlayer _soundPlayer;
		private AudioFileReader _audioFileReader;
		private string _mediaFilePath;

		public bool Loaded { get; set; }

		public MediaPlayer()
		{
			Loaded = InitializePlayer();
		}

		public void Play()
		{
			_soundPlayer.Play();
		}

		public void Pause()
		{
			_soundPlayer.Pause();
		}

		public void SkipBack()
		{
			_soundPlayer.Stop();
			InitializePlayer();
		}

		/// <summary>
		/// Initializes the sound player
		/// </summary>
		/// <returns>True is media is loaded. False if no media is loaded.</returns>
		public bool InitializePlayer()
		{

			_mediaFilePath = GetMediaFilePath();

			if (_mediaFilePath != null)
			{
				_soundPlayer = new WaveOut();
				_audioFileReader = new AudioFileReader((_mediaFilePath));
				_soundPlayer.Init(_audioFileReader);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Initializes the sound player
		/// </summary>
		/// <returns>True is media is loaded. False if no media is loaded.</returns>
		public bool InitializePlayer(string path)
		{

			_mediaFilePath = path;

			if (_mediaFilePath != null)
			{
				_soundPlayer = new WaveOut();
				_audioFileReader = new AudioFileReader((_mediaFilePath));
				_soundPlayer.Init(_audioFileReader);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Finds the first .wav file in the codebase directory and returns its path
		/// </summary>
		/// <returns></returns>
		private string GetMediaFilePath()
		{
			string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
			var directory = Path.GetDirectoryName(path);
			if (directory != null)
			{
				directory = directory.Substring(directory.IndexOf("c", StringComparison.Ordinal));
				return Directory.GetFiles(directory, "*.wav").FirstOrDefault();
			}
			return null;
		}
	}
}
