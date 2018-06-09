using System.Collections.Generic;
using System.ComponentModel;


namespace Fieldscribe_Windows_App.Models
{
    class ScribesPanelDataModel : INotifyPropertyChanged
    {
        private static ScribesPanelDataModel _instance = null;
        private static readonly object padlock = new object();

        public string _name;
        private IList<User> _assignedScribes;
        private IList<User> _scribes;
        private bool _scribesListSelected;
        private bool _assignedScribesListSelected;

        public IList<User> AssignedScribes
        {
            get { return _assignedScribes; }
            set
            {
                _assignedScribes = value;
                NotifyPropertyChanged();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged();
            }
        }

        public IList<User> Scribes
        {
            get { return _scribes; }
            set
            {
                _scribes = value;
                NotifyPropertyChanged();
            }
        }

        public bool ScribesListSelected
        {
            get { return _scribesListSelected; }
            set
            {
                _scribesListSelected = value;
                NotifyPropertyChanged();
                _assignedScribesListSelected = !_scribesListSelected;
            }
        }

        public bool AssignedScribesListSelected
        {
            get { return _assignedScribesListSelected; }
            set
            {
                _assignedScribesListSelected = value;
                NotifyPropertyChanged();
                _scribesListSelected = !_assignedScribesListSelected;
            }
        }

        public static ScribesPanelDataModel Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new ScribesPanelDataModel();
                    }
                    return _instance;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string property = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
