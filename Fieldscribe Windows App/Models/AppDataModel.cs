using System.Collections.Generic;
using System.ComponentModel;

namespace Fieldscribe_Windows_App.Models
{
    public class AppDataModel : INotifyPropertyChanged
    {
        private static AppDataModel _instance = null;
        private static readonly object padlock = new object();

        private Meet _meet = null;
        private IList<Meet> _meetList;
        private bool _meetAndFolderSet = false;
        private bool _userReady = false;
        private bool _meetSelected = false;
        private bool _folderSet = false;
        private string _folderPath = "";

        public Meet SelectedMeet
        {
            get { return _meet; }
            set
            {
                _meet = value;
                NotifyPropertyChanged();

                MeetSelected = (_meet != null);
            }
        }

        public IList<Meet> MeetList
        {
            get { return _meetList; }
            set
            {
                _meetList = value;
                NotifyPropertyChanged();
            }
        }

        public string FolderPath // Use this to add results to the shared folder!!!
        {
            get { return _folderPath; }
            set
            {
                _folderPath = value;
                NotifyPropertyChanged();

                FolderSet = (_folderPath != ""
                    && _folderPath != null);
            }
        }

        public bool MeetSelected
        {
            get { return _meetSelected; }
            set
            {
                _meetSelected = value;
                NotifyPropertyChanged();

                MeetAndFolderSet = (_meetSelected && _folderSet);
            }
        }

        public bool FolderSet
        {
            get { return _folderSet; }
            set
            {
                _folderSet = value;
                NotifyPropertyChanged();

                MeetAndFolderSet = (_meetSelected && _folderSet);
            }
        }

        public bool MeetAndFolderSet
        {
            get { return _meetAndFolderSet; }
            set
            {
                _meetAndFolderSet = value;
                NotifyPropertyChanged();
            }
        }

        public bool UserReady
        {
            get { return _userReady; }
            set
            {
                _userReady = value;
                NotifyPropertyChanged();
            }
        }

        public static AppDataModel Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new AppDataModel();
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
