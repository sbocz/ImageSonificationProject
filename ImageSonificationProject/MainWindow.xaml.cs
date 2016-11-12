using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace ImageSonificationProject
{
	public enum ProcessingMode
	{
		Mode1,
		Mode2,
		Mode3,
		Mode4
	}

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly MediaPlayer _player;
		private ImageProcessor _imageProcessor;
		private ProcessingMode _mode;

		public MainWindow()
		{
			InitializeComponent();

			Mode1.IsChecked = true;
			_mode = ProcessingMode.Mode1;

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

				//Create processor and process the file
				Stream stream = dialog.OpenFile();
				_imageProcessor = new ImageProcessor(stream, ProgressBar);
				_imageProcessor.Process(_mode);

				//Enable controls once processing is complete
				EnablePlaybackControls(true);
			}
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
			if (sender.Equals(Mode1))
				_mode = ProcessingMode.Mode1;
			else if (sender.Equals(Mode2))
				_mode = ProcessingMode.Mode2;
			else if (sender.Equals(Mode3))
				_mode = ProcessingMode.Mode3;
			else if (sender.Equals(Mode4))
				_mode = ProcessingMode.Mode4;
		}
	}
}
