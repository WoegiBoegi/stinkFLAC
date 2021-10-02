using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace stinkFLAC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string PLAY_SYMBOL = "▶️";
        private string PAUSE_SYMBOL = "⏸️";

        private bool isPlaying = false;

        public string ToPlayURI { get; set; }

        private DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += new EventHandler(UpdateSongProgress);
        }

        private void PlayMusic()
        {
            MediaElement.Play();
            PlayPauseButton.Content = PAUSE_SYMBOL;
            isPlaying = true;
            timer.Start();
        }

        private void PauseMusic()
        {
            MediaElement.Pause();
            PlayPauseButton.Content = PLAY_SYMBOL;
            isPlaying = false;
            timer.Stop();
        }

        private void StartPlayback()
        {
            MediaElement.Source = new Uri(ToPlayURI);
            PlayMusic();
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (MediaElement.Source != null)
            {
                if (isPlaying)
                {
                    PauseMusic();
                }
                else
                {
                    PlayMusic();
                }
            }
        }

        private void songChooseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "FLAC files (*.flac)|*.flac";
            openFile.ShowDialog();
            ToPlayURI = openFile.FileName;
            StartPlayback();
        }

        private void MediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            SongProgressBar.Maximum = MediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
            SongProgressBar.Minimum = 0;
            SongProgressSlider.Maximum = MediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
            SongProgressSlider.Minimum = 0;
        }

        private void UpdateSongProgress(object sender, EventArgs e)
        {
            SongProgressBar.Value = MediaElement.Position.TotalMilliseconds;
            double timeElapsed = MediaElement.Position.TotalSeconds;
            double timeLeft = SongProgressBar.Maximum / 1000 - MediaElement.Position.TotalSeconds;
            int minutesElapsed = (int)(timeElapsed / 60);
            int minutesLeft = (int)(timeLeft / 60);
            int secondsElapsed = (int)(timeElapsed - (minutesElapsed * 60));
            int secondsLeft = (int)(timeLeft - (minutesLeft * 60));

            string StimeElapsed = $"{minutesElapsed}:";
            if (secondsElapsed < 10)
                StimeElapsed += $"0{secondsElapsed}";
            else
                StimeElapsed += $"{secondsElapsed}";

            string StimeLeft = $"{minutesLeft}:";
            if (secondsLeft < 10)
                StimeLeft += $"0{secondsLeft}";
            else
                StimeLeft += $"{secondsLeft}";

            ElapsedTime.Content = StimeElapsed;
            RemainingTime.Content = StimeLeft;
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("A");
        }

        private void SongProgressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
