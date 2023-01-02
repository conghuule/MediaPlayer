using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
namespace MediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        class MediaFile
        {
            public string Name { get; set; }
            public string Author { get; set; }
            public string Thumbnail { get; set; }
            public string MediaPath { get; set; }
        }

        List<MediaFile> mediaFiles = new List<MediaFile>();
        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += new EventHandler(timer_Tick);
            void timer_Tick(object sender, EventArgs e)
            {
                Slider_Seek.Value = MediaPlayerEl.Position.TotalSeconds;
            }
            mediaFiles.Add(new MediaFile()
            {
                Name = "Waiting for you",
                Author = "Mono, Onionn",
                Thumbnail = "/Images/waiting_for_you.png",
                MediaPath = @"Images/MONO - Waiting For You (Album 22 - Track No.10).mp4"
            });
            mediaFiles.Add(new MediaFile()
            {
                Name = "Tai vi sao",
                Author = "MCK",
                Thumbnail = "/Images/waiting_for_you.png",
                MediaPath = @"Images/MONO - Waiting For You (Album 22 - Track No.10).mp4"
            });
            mediaFiles.Add(new MediaFile()
            {
                Name = "Waiting for you",
                Author = "Mono, Onionn",
                Thumbnail = "/Images/waiting_for_you.png",
                MediaPath = @"/Images/Waiting For You - MONO_ Onionn.mp3"
            });
            mediaFiles.Add(new MediaFile()
            {
                Name = "Tai vi sao",
                Author = "MCK",
                Thumbnail = "/Images/waiting_for_you.png",
                MediaPath = @"/Images/Waiting For You - MONO_ Onionn.mp3"
            });

            playlistListView.ItemsSource = mediaFiles;
        }

        private void playlistListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int id = playlistListView.SelectedIndex;
            MediaPlayerEl.Source = new Uri(mediaFiles[id].MediaPath, UriKind.Relative);
            MediaPlayerEl.LoadedBehavior = MediaState.Manual;
            MediaPlayerEl.UnloadedBehavior = MediaState.Manual;
            MediaPlayerEl.Volume = (double)Slider_Volume.Value;
            MediaPlayerEl.Play();
        }

        //private void ButtonPrevious_Click(object sender, RoutedEventArgs e)
        //{

        //}

        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            MediaPlayerEl.Play();
        }

        private void ButtonPause_Click(object sender, RoutedEventArgs e)
        {
            MediaPlayerEl.Pause();
        }

        private void slider_volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MediaPlayerEl.Volume = (double)Slider_Volume.Value;
        }

        private void slider_seek_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MediaPlayerEl.Position = TimeSpan.FromSeconds(Slider_Seek.Value);
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            string fileName = (string)((DataObject)e.Data).GetFileDropList()[0];
            MediaPlayerEl.Source = new Uri(fileName);
            MediaPlayerEl.LoadedBehavior = MediaState.Manual;
            MediaPlayerEl.UnloadedBehavior = MediaState.Manual;
            MediaPlayerEl.Volume = (double)Slider_Volume.Value;
            MediaPlayerEl.Play();
        }

        private void MediaPlayerEl_MediaOpened(object sender, RoutedEventArgs e)
        {
            TimeSpan ts = MediaPlayerEl.NaturalDuration.TimeSpan;
            Slider_Seek.Maximum = ts.TotalSeconds;
            timer.Start();
        }

    }
}