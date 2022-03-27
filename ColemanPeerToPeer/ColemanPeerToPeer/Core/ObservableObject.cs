/*
 This file is used to notify the ui when a property has been changed
 */

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

/* Maintenance History
 
 0.9: Added functionality
 1.0: Added Comments
 */
