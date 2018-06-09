using Fieldscribe_Windows_App.Controllers;
using Fieldscribe_Windows_App.Models;
using System;
using System.Windows.Media;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Net.Http;
using Fieldscribe_Windows_App.Infrastructure;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Net;
using MaterialDesignThemes.Wpf;
//using MaterialDesignThemes.Wpf;

namespace Fieldscribe_Windows_App
{
    /// <summary>
    /// Interaction logic for FieldScribeModernDesign.xaml
    /// </summary>
    public partial class FieldScribeModernDesign : Window
    {
        public AppDataModel _appDataModel;
        private IList<Meet> _meets;
        private bool _postLynxSuccess;
        private FolderWatcher _watcher;
        private static string _file = "lynx.evt";
        private TokenManager _tokenManager;

        public FieldScribeModernDesign()
        {
            InitializeComponent();

            _appDataModel = AppDataModel.Instance;
            this.DataContext = AppDataModel.Instance;

            _watcher = FolderWatcher.Instance;

            _tokenManager = TokenManager.Instance;

            _watcher.FSWatcher.Changed += File_Changed;

            // Add event handlers
            setupPanel.MeetSelection_Changed += MeetSelectionChanged;
            setupPanel.FolderSelection_Changed += FolderSelectionChanged;
            setupPanel.StartStopBtn_Clicked += StartStopBtnClicked;
            setupPanel.DeleteBtn_Clicked += DeleteBtnClicked;
            setupPanel.EditBtn_Clicked += EditBtnClicked;
            setupPanel.CreateMeetBtn_Clicked += CreateMeetBtnClicked;
            setupPanel.SaveMeetBtn_Clicked += SaveMeetBtnClicked;
            setupPanel.MeetPicker_Open += MeetPickerOpen;

            try { _meets = GetMeets(); }
            catch(Exception e)
            {
                // Will throw exception if not connected to internet
                // TODO: Handle exception
            }
            
        }

        private void File_Changed(object sender, FileSystemEventArgs e)
        {
            if (_appDataModel.MeetAndFolderSet && _appDataModel.UserReady)
                PostLynxFiles();
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OpenMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenMenuBtn.Visibility = Visibility.Collapsed;
            closeMenuBtn.Visibility = Visibility.Visible;
        }

        private void closeMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenMenuBtn.Visibility = Visibility.Visible;
            closeMenuBtn.Visibility = Visibility.Collapsed;
        }

        // Triggers when the user clicks on the Details item
        private void listViewItemDetails_Selected(object sender, RoutedEventArgs e)
        {
            ShowSelectedPanel(detailsPanel);
            detailsPanel.RefreshScreen();
        }

        // Triggers when the user clicks on the Setup item
        private void listViewItemSetup_Selected(object sender, RoutedEventArgs e)
        {
            ShowSelectedPanel(setupPanel);
        }

        // Triggers when the user clicks on the Scribes item
        private void listViewItemScribes_Selected(object sender, RoutedEventArgs e)
        {
            ShowSelectedPanel(scribesPanel);
        }

        private void ShowSelectedPanel(UserControl control)
        {
            setupPanel.Visibility = Visibility.Collapsed;
            detailsPanel.Visibility = Visibility.Collapsed;
            scribesPanel.Visibility = Visibility.Collapsed;
            control.Visibility = Visibility.Visible;
        }

        private void informationBtn_Click(object sender, RoutedEventArgs e)
        {
            if (HelpScreen.activeInstance == false)
            {
                HelpScreen dashBoard = new HelpScreen();
                dashBoard.Show();
            }
        }

        private void MeetPickerOpen(object sender, RoutedEventArgs e)
        {
            RefreshMeetPicker(_meets);
        }

        private void MeetSelectionChanged(object sender, RoutedEventArgs e)
        {
            ScribesPanelDataModel model = 
                ScribesPanelDataModel.Instance;

            model.AssignedScribes = null;

            if (setupPanel.MeetPicker.SelectedIndex >= 0)
            {
                _appDataModel.MeetSelected = true;
                _appDataModel.SelectedMeet = _meets[setupPanel.MeetPicker.SelectedIndex];

                model.AssignedScribes = GetScribes(_appDataModel.SelectedMeet.MeetId);

                // Filter Scribe from list
                if(model.AssignedScribes != null)
                    model.Scribes = model.Scribes.Where(x => !model.AssignedScribes
                    .Any(y => y.Id == x.Id)).ToList<User>();
            }
            else
            {
                _appDataModel.SelectedMeet = null;
            }

            RefreshScribesList(model.Scribes);
            RefreshAssignedScribesList(model.AssignedScribes);
            ResetStartStopBtn();
        }

        private void CreateMeetBtnClicked(object sender, RoutedEventArgs e)
        {
            if (CreateMeet(
                new Meet
                {
                    MeetName = setupPanel.MeetNameBox.Text,
                    MeetDate = (DateTime)setupPanel.CreatMeetDatePicker.SelectedDate,
                    MeetLocation = setupPanel.MeetLocationBox.Text,
                    MeasurementType = setupPanel.MeasurementPicker.SelectedItem.ToString()
                }
            ))
            { _meets = GetMeets(); } // Refresh list of meets   

            //RootDialogHost.IsOpen = false;
        }

        private void SaveMeetBtnClicked(object sender, RoutedEventArgs e)
        {
            if(EditMeet(
                new Meet
                {
                    MeetId = _appDataModel.SelectedMeet.MeetId,
                    MeetName = setupPanel.MeetNameBox.Text,
                    MeetDate = (DateTime)setupPanel.CreatMeetDatePicker.SelectedDate,
                    MeetLocation = setupPanel.MeetLocationBox.Text,
                    MeasurementType = setupPanel.MeasurementPicker.SelectedItem.ToString()
                }
              ))
            {
                int index = setupPanel.MeetPicker.SelectedIndex;
                _meets = GetMeets();
                RefreshMeetPicker(_meets);
                setupPanel.MeetPicker.SelectedIndex = index;
                _appDataModel.SelectedMeet = _meets[setupPanel.MeetPicker.SelectedIndex];
            }
        }

        private void FolderSelectionChanged(object sender, RoutedEventArgs e)
        {
            _appDataModel.FolderPath = setupPanel.SelectFolderText.Text;
            _watcher.WatchDirectory(_appDataModel.FolderPath, _file);
        }

        private void DeleteBtnClicked(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = 
                MessageBox.Show("Are you sure?", "Confirm Meet Deletion",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if(result == MessageBoxResult.Yes)
            {
                if (DeleteMeet(_appDataModel.SelectedMeet))
                {
                    _meets = GetMeets();
                    RefreshMeetPicker(_meets);
                    _appDataModel.SelectedMeet = null;
                } 
            }
        }

        private void EditBtnClicked(object sender, RoutedEventArgs e)
        {
            Meet m = _appDataModel.SelectedMeet;
            // Index of selected meet?
            setupPanel.MeetNameBox.Text = m.MeetName;
            setupPanel.MeetLocationBox.Text = m.MeetLocation;
            setupPanel.CreatMeetDatePicker.SelectedDate = m.MeetDate;
            setupPanel.MeasurementPicker.SelectedItem = m.MeasurementType;
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            // close main menu and go back to login screen
            LoginScreen backToLogin = new LoginScreen();
            backToLogin.Show();
            this.Close();
        }

        private void StartStopBtnClicked(object sender, RoutedEventArgs e)
        {
            if(!_appDataModel.UserReady)
            {
                System.Threading.Thread.Sleep(100);

                LynxFileService lfs = new LynxFileService(_appDataModel.FolderPath);

                if (!lfs.LynxFilesExist())
                {
                    MessageBox.Show("Could not find Lynx .evt, .sch, or .ppl files. " +
                        "Make sure to update start lists in shared folder.", "Lynx Files Not Found",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    setupPanel.StartProgressBar.Visibility = Visibility.Visible;
                    setupPanel.InfoText.Text = "Posting Lynx Files to Server";
                    setupPanel.StartStopBtn.IsEnabled = false;

                    bool success = PostLynxFiles();

                    if(success)
                    {

                    }
                }
            }
            else
            {
                ResetStartStopBtn();
            }  
        }


        private bool PostLynxFiles()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_PostLynxFiles;
            worker.RunWorkerCompleted += worker_PostLynxFilesComplete;
            worker.RunWorkerAsync();

            return _postLynxSuccess;
        }

        private void worker_PostLynxFiles(object sender, DoWorkEventArgs e)
        {
            LynxFileService lfs = new LynxFileService(_appDataModel.FolderPath);

            FieldScribeFileDataController fdc = new FieldScribeFileDataController();

            if(_tokenManager.Token != "")

                _postLynxSuccess = fdc.AddMeetFiles(_appDataModel.SelectedMeet.MeetId,
                    lfs, _tokenManager.Token);

            else
            _postLynxSuccess = false;
        }


        private void worker_PostLynxFilesComplete(object sender, RunWorkerCompletedEventArgs e)
        {  
            setupPanel.StartProgressBar.Visibility = Visibility.Hidden;

            if(_postLynxSuccess)
            {
                setupPanel.InfoText.Text = "Lynx files successfully posted.";
                setupPanel.StartStopBtn.Background = Brushes.DarkRed;
                setupPanel.StartStopBtn.BorderBrush = Brushes.DarkRed;
                setupPanel.StartStopBtn.Content = "Stop";
                _appDataModel.UserReady = true;
            }
            else
            {
                setupPanel.InfoText.Text = "Error posting Lynx files. Try again.";
            }

            setupPanel.StartStopBtn.IsEnabled = true;
        }


        private bool CreateMeet(Meet newMeet)
        {
            MeetsController meetController = new MeetsController();

            var response = meetController.AddMeet(newMeet);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                // Success!
                MessageBox.Show(newMeet.MeetName + " successfully added", "Success",
                MessageBoxButton.OK, MessageBoxImage.None);
                return true;
            }
            else
            {
                MessageBox.Show("Failed to add " + newMeet.MeetName + ": "
                    + response.StatusCode.ToString(), "Failure",
                MessageBoxButton.OK, MessageBoxImage.None);
                return false;
            }
        }

        private bool DeleteMeet(Meet meet)
        {
            bool success = false;
            MeetsController mc = new MeetsController();
            var response = mc.DeleteMeet(meet);

            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Success!
                MessageBox.Show(meet.MeetName + " successfully deleted", "Success",
                MessageBoxButton.OK, MessageBoxImage.None);
                success = true;
            }
            else
            {
                MessageBox.Show("Deletion failed: " + 
                    response.StatusCode.ToString(), "Failure",
                MessageBoxButton.OK, MessageBoxImage.None);
            }

            return success;
        }

        private bool EditMeet(Meet meet)
        {
            bool success = false;
            MeetsController mc = new MeetsController();
            var response = mc.EditMeet(meet);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Success!
                MessageBox.Show("Edit succeeded", "Success",
                MessageBoxButton.OK, MessageBoxImage.None);
                success = true;
            }
            else
            {
                MessageBox.Show("Edit failed" + 
                    response.StatusCode.ToString(), "Failure",
                MessageBoxButton.OK, MessageBoxImage.None);
            }

            return success;
        }

        private void ResetStartStopBtn()
        {
            _appDataModel.UserReady = false;
            setupPanel.StartStopBtn.Background = Brushes.Green;
            setupPanel.StartStopBtn.BorderBrush = Brushes.Green;
            setupPanel.StartStopBtn.Content = "Start";
        }

        private void RefreshMeetPicker(IList<Meet> meets)
        {
            if(meets != null)
            setupPanel.MeetPicker.ItemsSource = meets.Select(meet => new Meet
            {
                MeetDate = meet.MeetDate.Date,
                MeetName = meet.MeetName,
                MeetLocation = meet.MeetLocation
            });
        }

        IList<Meet> GetMeets()
        {
            return new MeetsController().RetrieveAllMeets();
        }


        private void RefreshAssignedScribesList(IList<User> scribes)
        {
            if(scribes == null)
            {
                scribesPanel.AssignedScribesList.ItemsSource = null;
            }
            else
            {
                scribesPanel.AssignedScribesList.ItemsSource =
                scribes.Select(scribe => new User
                {
                    Id = scribe.Id,
                    FirstName = scribe.FirstName,
                    LastName = scribe.LastName,
                    Email = scribe.Email,
                    CreatedAt = scribe.CreatedAt,
                    Roles = scribe.Roles
                });
            }
        }

        private void RefreshScribesList(IList<User> scribes)
        {
            if(scribes == null)
            {
                scribesPanel.AssignedScribesList.ItemsSource = null;
            }
            else
            {
                scribesPanel.ScribesList.ItemsSource =
                scribes.Select(scribe => new User
                {
                    Id = scribe.Id,
                    FirstName = scribe.FirstName,
                    LastName = scribe.LastName,
                    Email = scribe.Email,
                    CreatedAt = scribe.CreatedAt,
                    Roles = scribe.Roles
                });
            }
        }


        IList<User> GetScribes(int meetId)
        {
            (bool success, IList<User> scribes) =
                new UsersController().GetScribesForMeet(
                meetId, _tokenManager.Token);

            if (success)
                return scribes;

            return null;
        }
    }
}
