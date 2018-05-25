using AMA_Card_Reader.Models;
using System.Collections.ObjectModel;

namespace AMA_Card_Reader.ViewModels
{
    public class CardReaderViewModel : Notifiable
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

        private string _filePath = string.Empty;
        public string FilePath
        {
            get { return _filePath; }
            set { SetField(ref this._filePath, value); }
        }

        private AMACardEntry _selectedEntry = null;
        public AMACardEntry SelectedEntry
        {
            get { return _selectedEntry; }
            set { SetField(ref this._selectedEntry, value); }
        }
    }
}
