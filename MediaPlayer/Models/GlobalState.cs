using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.Models
{
    class GlobalState:INotifyPropertyChanged
    {
        public bool IsRandom { get; set; } = false;
        public bool IsPlaying { get; set; } = false;

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
