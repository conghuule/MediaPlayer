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

namespace MediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        class MediaFile
        {
            public string Name { get; set; } 
            public string Author { get; set; }
            public string Thumbnail { get; set; }
        }

        List<MediaFile> mediaFiles = new List<MediaFile>(); 
        public MainWindow()
        {
            InitializeComponent();

            mediaFiles.Add(new MediaFile() { 
                Name = "Waiting for you",
                Author = "Mono, Onionn",
                Thumbnail= "/Images/waiting_for_you.png"
            });
            mediaFiles.Add(new MediaFile()
            {
                Name = "Tai vi sao",
                Author = "MCK",
                Thumbnail = "/Images/waiting_for_you.png"
            });
            mediaFiles.Add(new MediaFile()
            {
                Name = "Waiting for you",
                Author = "Mono, Onionn",
                Thumbnail = "/Images/waiting_for_you.png"
            });
            mediaFiles.Add(new MediaFile()
            {
                Name = "Tai vi sao",
                Author = "MCK",
                Thumbnail = "/Images/waiting_for_you.png"
            }); mediaFiles.Add(new MediaFile()
            {
                Name = "Tai vi sao",
                Author = "MCK",
                Thumbnail = "/Images/waiting_for_you.png"
            }); mediaFiles.Add(new MediaFile()
            {
                Name = "Tai vi sao",
                Author = "MCK",
                Thumbnail = "/Images/waiting_for_you.png"
            }); mediaFiles.Add(new MediaFile()
            {
                Name = "Tai vi sao",
                Author = "MCK",
                Thumbnail = "/Images/waiting_for_you.png"
            }); mediaFiles.Add(new MediaFile()
            {
                Name = "Tai vi sao",
                Author = "MCK",
                Thumbnail = "/Images/waiting_for_you.png"
            }); mediaFiles.Add(new MediaFile()
            {
                Name = "Tai vi sao",
                Author = "MCK",
                Thumbnail = "/Images/waiting_for_you.png"
            }); mediaFiles.Add(new MediaFile()
            {
                Name = "Tai vi sao",
                Author = "MCK",
                Thumbnail = "/Images/waiting_for_you.png"
            }); mediaFiles.Add(new MediaFile()
            {
                Name = "Tai vi sao",
                Author = "MCK",
                Thumbnail = "/Images/waiting_for_you.png"
            }); mediaFiles.Add(new MediaFile()
            {
                Name = "Tai vi sao",
                Author = "MCK",
                Thumbnail = "/Images/waiting_for_you.png"
            }); mediaFiles.Add(new MediaFile()
            {
                Name = "Tai vi sao",
                Author = "MCK",
                Thumbnail = "/Images/waiting_for_you.png"
            }); mediaFiles.Add(new MediaFile()
            {
                Name = "Tai vi sao",
                Author = "MCK",
                Thumbnail = "/Images/waiting_for_you.png"
            }); mediaFiles.Add(new MediaFile()
            {
                Name = "Tai vi sao",
                Author = "MCK",
                Thumbnail = "/Images/waiting_for_you.png"
            }); mediaFiles.Add(new MediaFile()
            {
                Name = "Tai vi sao",
                Author = "MCK",
                Thumbnail = "/Images/waiting_for_you.png"
            }); mediaFiles.Add(new MediaFile()
            {
                Name = "Tai vi sao",
                Author = "MCK",
                Thumbnail = "/Images/waiting_for_you.png"
            }); mediaFiles.Add(new MediaFile()
            {
                Name = "Tai vi sao",
                Author = "MCK",
                Thumbnail = "/Images/waiting_for_you.png"
            }); mediaFiles.Add(new MediaFile()
            {
                Name = "Tai vi sao",
                Author = "MCK",
                Thumbnail = "/Images/waiting_for_you.png"
            }); mediaFiles.Add(new MediaFile()
            {
                Name = "Tai vi sao",
                Author = "MCK",
                Thumbnail = "/Images/waiting_for_you.png"
            }); mediaFiles.Add(new MediaFile()
            {
                Name = "Tai vi sao",
                Author = "MCK",
                Thumbnail = "/Images/waiting_for_you.png"
            }); mediaFiles.Add(new MediaFile()
            {
                Name = "Tai vi sao",
                Author = "MCK",
                Thumbnail = "/Images/waiting_for_you.png"
            }); mediaFiles.Add(new MediaFile()
            {
                Name = "Tai vi sao",
                Author = "MCK",
                Thumbnail = "/Images/waiting_for_you.png"
            }); mediaFiles.Add(new MediaFile()
            {
                Name = "Tai vi sao",
                Author = "MCK",
                Thumbnail = "/Images/waiting_for_you.png"
            }); mediaFiles.Add(new MediaFile()
            {
                Name = "Tai vi sao",
                Author = "MCK",
                Thumbnail = "/Images/waiting_for_you.png"
            }); mediaFiles.Add(new MediaFile()
            {
                Name = "Tai vi sao",
                Author = "MCK",
                Thumbnail = "/Images/waiting_for_you.png"
            }); mediaFiles.Add(new MediaFile()
            {
                Name = "Tai vi sao",
                Author = "MCK",
                Thumbnail = "/Images/waiting_for_you.png"
            }); mediaFiles.Add(new MediaFile()
            {
                Name = "Tai vi sao",
                Author = "MCK",
                Thumbnail = "/Images/waiting_for_you.png"
            }); mediaFiles.Add(new MediaFile()
            {
                Name = "Tai vi sao",
                Author = "MCK",
                Thumbnail = "/Images/waiting_for_you.png"
            }); mediaFiles.Add(new MediaFile()
            {
                Name = "Tai vi sao",
                Author = "MCK",
                Thumbnail = "/Images/waiting_for_you.png"
            }); mediaFiles.Add(new MediaFile()
            {
                Name = "Tai vi sao",
                Author = "MCK",
                Thumbnail = "/Images/waiting_for_you.png"
            });

            playlistListView.ItemsSource = mediaFiles;
        }
    }
}
