using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.WindowsAPICodePack.Dialogs;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace MediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        class MediaFile : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler? PropertyChanged;

            public string Name { get; set; } 
            public string Author { get; set; }
            public string Length { get; set; }
            public string Path { get; set;  }
            public dynamic Thumbnail { get; set; }
        }

        private List<string> _fileAddedList = new List<string>();
        ObservableCollection<MediaFile> mediaFiles = new ObservableCollection<MediaFile>(); 

        public MainWindow()
        {
            InitializeComponent();

            //mediaFiles.Add(new MediaFile() { 
            //    Name = "Waiting for you",
            //    Author = "Mono, Onionn",
            //    Thumbnail= "/Images/waiting_for_you.png"
            //});
            //mediaFiles.Add(new MediaFile()
            //{
            //    Name = "Tai vi sao",
            //    Author = "MCK",
            //    Thumbnail = "/Images/waiting_for_you.png"
            //});
            //mediaFiles.Add(new MediaFile()
            //{
            //    Name = "Waiting for you",
            //    Author = "Mono, Onionn",
            //    Thumbnail = "/Images/waiting_for_you.png"
            //});

            playlistListView.ItemsSource = mediaFiles;
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
                            Author = f.Tag.FirstArtist != null ?  f.Tag.FirstArtist : "Unknown",
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
    }
}
