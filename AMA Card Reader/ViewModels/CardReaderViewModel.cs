using AMA_Card_Reader.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AMA_Card_Reader.ViewModels
{
    public class CardReaderViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Vehicle> _vehicles = new ObservableCollection<Vehicle>();
        public ObservableCollection<Vehicle> Vehicles
        {
            get { return _vehicles; }
            set { SetField(ref this._vehicles, value); }
        }

        private ObservableCollection<AMACardEntry> _entries = new ObservableCollection<AMACardEntry>();
        public ObservableCollection<AMACardEntry> Entries
        {
            get { return _entries; }
            set { SetField(ref this._entries, value); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            InternalPropertyChanged(propertyName);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected virtual void InternalPropertyChanged(string property)
        {
        }
    }
}
