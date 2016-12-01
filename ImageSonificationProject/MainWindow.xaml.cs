using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageSonificationProject
{
	public enum ProcessingMode
	{
		Brightness,
		Darkness,
		Mode3,
		Mode4
	}

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		//44kHz
		public const int SampleRate = 44100;
		private readonly MediaPlayer _player;
		private ImageProcessor _imageProcessor;
		private ProcessingMode _mode;
		private string _audioPath;
		private WaveFileGenerator _waveGenerator;

		public MainWindow()
		{
			InitializeComponent();

			//Start with brightness as selection for mode
			BrightnessMode.IsChecked = true;
			_mode = ProcessingMode.Brightness;

			_player = new MediaPlayer();
			EnablePlaybackControls(_player.Loaded);
		}

		/// <summary>
		/// Upload the image selected by the user. Valid formats are .png and .jpeg files.
		/// Processes the image after upload then enables the playback controls once the processing is complete
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Upload_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.CheckFileExists = true;

			//Allow selection of .png and .jpeg files only.
			dialog.Filter = "png files (*.png)|*.png|jpeg files (*.jpeg)|*.jpeg";
			dialog.DefaultExt = ".png";

			if (dialog.ShowDialog() == true)
			{
				//Turn off controls during processing
				EnablePlaybackControls(false);

				//Display resized image to be processed
				DisplayedImage.Source = ImageSourceFromFileName(dialog.FileName);

				//Create processor and process the file
				Stream stream = dialog.OpenFile();
				_imageProcessor = new ImageProcessor(stream, ProgressBar);
				var imageAudioData = _imageProcessor.Process(_mode);
				_waveGenerator = new WaveFileGenerator(SampleRate, dialog.FileName, _mode);
				_audioPath = _waveGenerator.SaveWavFile(imageAudioData);
				_player.InitializePlayer(_audioPath);

				//Enable controls once processing is complete
				EnablePlaybackControls(true);
			}
		}

		private ImageSource ImageSourceFromFileName(string fileName)
		{
			var image = new BitmapImage();
			image.BeginInit();
			image.UriSource = new Uri(fileName);
			image.DecodePixelWidth = ImageProcessor.ImageWidth;
			image.DecodePixelHeight = ImageProcessor.ImageHeight;
			image.EndInit();
			return image;
		}

		private void Play_Click(object sender, RoutedEventArgs e)
		{
			_player.Play();
		}

		private void Pause_Click(object sender, RoutedEventArgs e)
		{
			_player.Pause();
		}

		private void Reset_Click(object sender, RoutedEventArgs e)
		{
			_player.SkipBack();
		}

		private void EnablePlaybackControls(bool enabled)
		{
			this.PlayButton.IsEnabled = enabled;
			this.PauseButton.IsEnabled = enabled;
			this.ResetButton.IsEnabled = enabled;
		}

		/// <summary>
		/// Sets the processing mode to the mode represented by the radio button that triggered this event.
		/// </summary>
		/// <param name="sender">Mode radio button</param>
		/// <param name="e"></param>
		private void ModeSelection_Click(object sender, RoutedEventArgs e)
		{
			if (sender.Equals(BrightnessMode))
				_mode = ProcessingMode.Brightness;
			else if (sender.Equals(DarknessMode))
				_mode = ProcessingMode.Darkness;
			else if (sender.Equals(Mode3))
				_mode = ProcessingMode.Mode3;
			else if (sender.Equals(Mode4))
				_mode = ProcessingMode.Mode4;
		}
	}
}
