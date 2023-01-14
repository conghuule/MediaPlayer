using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.Models
{
    internal class MediaFile : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public string Name { get; set; }
        public string Author { get; set; }
        public string Length { get; set; }
        public string Path { get; set; }
        public dynamic Thumbnail { get; set; }
    }
}
