using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ColemanPeerToPeer.Core
{ 
    //leverages reflection to update the UI
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string properyname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(properyname));
        }
    }
}
