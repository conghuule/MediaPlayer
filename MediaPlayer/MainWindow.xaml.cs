using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.WindowsAPICodePack.Dialogs;
using System.ComponentModel;
using System.Collections.ObjectModel;

using System.Windows.Threading;
namespace MediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        private int currentIndex = 0;
        private bool isSuffle = false;
        private static Random rng = new Random();

        class MediaFile : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler? PropertyChanged;

            public string Name { get; set; }
            public string Author { get; set; }
            public string Length { get; set; }
            public string Path { get; set; }
            public dynamic Thumbnail { get; set; }
        }

        private List<string> _fileAddedList = new List<string>();
        ObservableCollection<MediaFile> mediaFiles = new ObservableCollection<MediaFile>();

        public MainWindow()
        {
            InitializeComponent();

            setButtonState();

            mediaFiles.CollectionChanged += MediaFiles_CollectionChanged;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += new EventHandler(timer_Tick);
            void timer_Tick(object sender, EventArgs e)
            {
                Slider_Seek.Value = MediaPlayerEl.Position.TotalSeconds;
                if (IsPlaying())
                {
                    playButton.Visibility = Visibility.Collapsed;
                    pauseButton.Visibility = Visibility.Visible;
                }
                else
                {
                    playButton.Visibility = Visibility.Visible;
                    pauseButton.Visibility = Visibility.Collapsed;
                }
            }

            playlistListView.ItemsSource = mediaFiles;
        }

        private void MediaFiles_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            setButtonState();
        }

        private void AddFiles_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "All Media Files|*.wav;*.aac;*.wma;*.wmv;*.avi;*.mpg;*.mpeg;*.m1v;*.mp2;*.mp3;*.mpa;*.mpe;*.m3u;*.mp4;*.mov;*.3g2;*.3gp2;*.3gp;*.3gpp;*.m4a;*.cda;*.aif;*.aifc;*.aiff;*.mid;*.midi;*.rmi;*.mkv;*.WAV;*.AAC;*.WMA;*.WMV;*.AVI;*.MPG;*.MPEG;*.M1V;*.MP2;*.MP3;*.MPA;*.MPE;*.M3U;*.MP4;*.MOV;*.3G2;*.3GP2;*.3GP;*.3GPP;*.M4A;*.CDA;*.AIF;*.AIFC;*.AIFF;*.MID;*.MIDI;*.RMI;*.MKV";

            if (openFileDialog.ShowDialog() == true)
            {

                foreach (var path in openFileDialog.FileNames)
                {
                    if (!_fileAddedList.Contains(path))
                    {
                        _fileAddedList.Add(path);
                        string filename = System.IO.Path.GetFileName(path);
                        TagLib.File f = TagLib.File.Create(path);

                        dynamic imgData;
                        if (f.Tag.Pictures.Length != 0)
                        {
                            MemoryStream ms = new MemoryStream(f.Tag.Pictures[0].Data.Data);
                            ms.Seek(0, SeekOrigin.Begin);
                            imgData = new BitmapImage();
                            imgData.BeginInit();
                            imgData.StreamSource = ms;
                            imgData.EndInit();
                        }
                        else
                        {
                            //Default thumbnail
                            imgData = "/Images/waiting_for_you.png";
                        }

                        MediaFile file = new MediaFile()
                        {
                            Name = f.Tag.Title != null ? f.Tag.Title : filename,
                            Author = f.Tag.FirstArtist != null ? f.Tag.FirstArtist : "Unknown",
                            Length = f.Tag.Length != null ? f.Tag.Length : "Unknown",
                            Path = path,
                            Thumbnail = imgData,
                        };

                        mediaFiles.Add(file);
                    }
                }
            }
        }

        private void AddFolder_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.Multiselect = true;
            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                HandleImportFiles(dialog.FileNames.ToList());
            }
        }
        private void handleFolder(string path)
        {
            string[] fileEntries = Directory.GetFiles(path);
            string[] mediaExtensions = {
                ".WAV", ".MID", ".MIDI", ".WMA", ".MP3", ".OGG", ".RMA", //etc
                ".AVI", ".MP4", ".DIVX", ".WMV", //etc
            };

            foreach (string fileName in fileEntries)
            {
                if (Array.IndexOf(mediaExtensions, Path.GetExtension(fileName).ToUpperInvariant()) != -1)
                {
                    if (!_fileAddedList.Contains(fileName))
                        _fileAddedList.Add(fileName);
                }
            }
            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(path);
            foreach (string subdirectory in subdirectoryEntries)
                handleFolder(subdirectory);
        }
        private void HandleImportFiles(List<string> FileNames)
        {
            Dispatcher.BeginInvoke(() =>
            {
                //Save old files
                List<string> lastFileList = new List<string>(_fileAddedList);
                //New files
                List<string> arrAllFiles = new List<string>(FileNames);

                foreach (var file in arrAllFiles)
                {
                    if (!_fileAddedList.Contains(file))
                    {
                        if (File.Exists(file))
                        {

                            if (!_fileAddedList.Contains(file))
                                _fileAddedList.Add(file);

                            // This path is a file
                        }
                        else if (Directory.Exists(file))
                        {
                            // This path is a directory
                            handleFolder(file);
                        }
                    }
                }

                // To store
                foreach (var path in _fileAddedList)
                {
                    if (!lastFileList.Contains(path))
                    {
                        string filename = Path.GetFileName(path);
                        TagLib.File f = TagLib.File.Create(path);

                        dynamic imgData;
                        if (f.Tag.Pictures.Length != 0)
                        {
                            MemoryStream ms = new MemoryStream(f.Tag.Pictures[0].Data.Data);
                            ms.Seek(0, SeekOrigin.Begin);
                            imgData = new BitmapImage();
                            imgData.BeginInit();
                            imgData.StreamSource = ms;
                            imgData.EndInit();
                        }
                        else
                        {
                            //Default thumbnail
                            imgData = "/Images/waiting_for_you.png";
                        }

                        MediaFile file = new MediaFile()
                        {
                            Name = f.Tag.Title != null ? f.Tag.Title : filename,
                            Author = f.Tag.FirstArtist != null ? f.Tag.FirstArtist : "Unknown",
                            Length = f.Tag.Length != null ? f.Tag.Length : "Unknown",
                            Path = path,
                            Thumbnail = imgData,
                        };

                        mediaFiles.Add(file);
                    }
                }
            });
        }
        private void RemoveFiles_Click(object sender, RoutedEventArgs e)
        {
            while (playlistListView.SelectedItems.Count > 0)
            {
                int index = playlistListView.SelectedIndex;
                mediaFiles.RemoveAt(index);
                _fileAddedList.RemoveAt(index);
            }
        }

        private void SavePlaylistClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void playlistListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (playlistListView.Items.Count == 0)
            {
                return;
            }
            int id = playlistListView.SelectedIndex;

            int lastIndex = mediaFiles[id].Name.LastIndexOf('.');
            string name = mediaFiles[id].Name.Substring(0, lastIndex);
            fileName.Text = name;
            mediaName.Text = name;

            playMedia(id);
        }

        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            MediaPlayerEl?.Play();
        }

        private void ButtonPause_Click(object sender, RoutedEventArgs e)
        {
            MediaPlayerEl?.Pause();
        }

        private void ButtonPrev_Click(object sender, RoutedEventArgs e)
        {
            playMedia(currentIndex - 1);
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            playMedia(currentIndex + 1);
        }

        private void slider_volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MediaPlayerEl.Volume = (double)Slider_Volume.Value / 100;
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
            MediaPlayerEl.Volume = (double)Slider_Volume.Value / 100;
            MediaPlayerEl.Play();
        }

        private void MediaPlayerEl_MediaOpened(object sender, RoutedEventArgs e)
        {
            TimeSpan ts = MediaPlayerEl.NaturalDuration.TimeSpan;
            Slider_Seek.Maximum = ts.TotalSeconds;
            timer.Start();
        }

        bool IsPlaying()
        {
            var pos1 = MediaPlayerEl.Position;
            System.Threading.Thread.Sleep(1);
            var pos2 = MediaPlayerEl.Position;

            return pos2 != pos1;
        }

        void playMedia(int id)
        {
            MediaPlayerEl.Source = new Uri(mediaFiles[id].Path, UriKind.Relative);
            MediaPlayerEl.LoadedBehavior = MediaState.Manual;
            MediaPlayerEl.UnloadedBehavior = MediaState.Manual;
            MediaPlayerEl.Volume = (double)Slider_Volume.Value / 100;
            MediaPlayerEl.Play();

            currentIndex = id;
            playlistListView.SelectedIndex = currentIndex;
            setButtonState();
        }

        void setButtonState()
        {
            if (mediaFiles.Count() == 0)
            {
                playButton.IsEnabled = false;
            }
            else
            {
                playButton.IsEnabled = true;
            }

            if (currentIndex <= 0)
            {
                prevButton.IsEnabled = false;
            }
            else
            {
                prevButton.IsEnabled = true;
            }

            if (currentIndex >= mediaFiles.Count() - 1)
            {
                nextButton.IsEnabled = false;
            }
            else
            {
                nextButton.IsEnabled = true;
            }
        }

        private void media_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (currentIndex >= mediaFiles.Count() - 1)
            {
                return;
            }

            playMedia(currentIndex + 1);
        }

        private void ButtonSuffle_Click(object sender, RoutedEventArgs e)
        {
            isSuffle = !isSuffle;
            if (!isSuffle)
            {
                var uriSource = new Uri(@"/Images/random.png", UriKind.Relative);
                suffleIcon.Source = new BitmapImage(uriSource);
                playlistListView.ItemsSource = mediaFiles;
            }
            else
            {
                var uriSource = new Uri(@"/Images/random-active.png", UriKind.Relative);
                suffleIcon.Source = new BitmapImage(uriSource);
                playlistListView.ItemsSource = mediaFiles.OrderBy(a => rng.Next()).ToList();
            }
            currentIndex = playlistListView.SelectedIndex;
            setButtonState();
        }
    }
} 