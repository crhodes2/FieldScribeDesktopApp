using Fieldscribe_Windows_App.Controllers;
using Fieldscribe_Windows_App.Models;
using MaterialDesignThemes.Wpf;
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
using System.IO;
using System.Windows.Threading;
using System.ComponentModel;

namespace Fieldscribe_Windows_App
{
    /// <summary>
    /// Interaction logic for SetupUserControl.xaml
    /// </summary>
    public partial class SetupUserControl : UserControl
    {
        private string path = string.Empty;
        private enum DialogStatus { Edit, Create };
        private DialogStatus dialogStatus;

        public SetupUserControl()
        {
            InitializeComponent();

            MeasurementPicker.ItemsSource = new string[] { "English", "Metric" };
        }


        // Event Handlers

        #region Event Handlers

        private void SelectFolderBtn_Click(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.FolderBrowserDialog folderDialog =
                new System.Windows.Forms.FolderBrowserDialog();

            folderDialog.ShowDialog();

            path = folderDialog.SelectedPath;

            if (path != null && path != "")
            {
                SelectFolderText.Text = path;
                SelectFolderBtn.Background = Brushes.Green;
                SelectFolderBtn.BorderBrush = Brushes.Green;
                RaiseEvent(new RoutedEventArgs(FolderSelectionChanged));
            }    
        }

        private void DeleteMeetBtn_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DeleteBtnClicked));
        }

        private void EditMeetBtn_Click(object sender, RoutedEventArgs e)
        {
            dialogStatus = DialogStatus.Edit;
            CreateOrEditBtn.Content = "SAVE";
            RaiseEvent(new RoutedEventArgs(EditBtnClicked));
        }

        private void AddMeetBtn_Click(object sender, RoutedEventArgs e)
        {
            dialogStatus = DialogStatus.Create;
            CreateOrEditBtn.Content = "CREATE";
            ClearCreateMeetFields();
        }

        private void CreateOrEditBtnClick(object sender, RoutedEventArgs e)
        {
            if(ValidMeetEntry())
            {
                if(dialogStatus == DialogStatus.Create)
                {
                    RaiseEvent(new RoutedEventArgs(CreateMeetBtnClicked));
                }
                else if(dialogStatus == DialogStatus.Edit)
                {
                    RaiseEvent(new RoutedEventArgs(SaveMeetBtnClicked));
                }
            }
        }

        private void MeetPicker_DropDownClosed(object sender, EventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(MeetSelectionChanged));    
        }

        private void MeetPicker_DropDownOpen(object sender, EventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(MeetPickerOpen));
        }

        private void DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("Closing dialog with parameter: " + (eventArgs.Parameter ?? ""));
        }

        private void StartStopBtn_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(StartStopBtnClicked));
        }


        #endregion


        // Helper Functions

        #region Helper Functions

        private bool ValidMeetEntry()
        {
            return (
                CreatMeetDatePicker.SelectedDate != null &&
                MeetNameBox.Text != "" &&
                MeetLocationBox.Text != "" &&
                MeasurementPicker.SelectedItem != null);
        }

        private void ClearCreateMeetFields()
        {
            CreatMeetDatePicker.SelectedDate = null;
            MeetNameBox.Text = "";
            MeetLocationBox.Text = "";
            MeasurementPicker.SelectedItem = null;
        }

        #endregion


        // Routed Event Handlers

        #region Routed Event Handlers

        public static readonly RoutedEvent MeetSelectionChanged =
            EventManager.RegisterRoutedEvent("MeetPicker_SelectionChanged",
            RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(SetupUserControl));

        public event RoutedEventHandler MeetSelection_Changed
        {
            add { AddHandler(MeetSelectionChanged, value); }
            remove { RemoveHandler(MeetSelectionChanged, value); }
        }

        public static readonly RoutedEvent MeetPickerOpen =
            EventManager.RegisterRoutedEvent("MeetPicker_Open",
            RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(SetupUserControl));

        public event RoutedEventHandler MeetPicker_Open
        {
            add { AddHandler(MeetPickerOpen, value); }
            remove { RemoveHandler(MeetPickerOpen, value); }
        }

        public static readonly RoutedEvent FolderSelectionChanged =
            EventManager.RegisterRoutedEvent("SelectFolderBtn_Click",
            RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(SetupUserControl));

        public event RoutedEventHandler FolderSelection_Changed
        {
            add { AddHandler(FolderSelectionChanged, value); }
            remove { RemoveHandler(FolderSelectionChanged, value); }
        }

        public static readonly RoutedEvent StartStopBtnClicked =
            EventManager.RegisterRoutedEvent("StartStopBtn_Click",
            RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(SetupUserControl));

        public event RoutedEventHandler StartStopBtn_Clicked
        {
            add { AddHandler(StartStopBtnClicked, value); }
            remove { RemoveHandler(StartStopBtnClicked, value); }
        }

        public static readonly RoutedEvent DeleteBtnClicked =
            EventManager.RegisterRoutedEvent("DeleteBtn_Click",
            RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(SetupUserControl));

        public event RoutedEventHandler DeleteBtn_Clicked
        {
            add { AddHandler(DeleteBtnClicked, value); }
            remove { RemoveHandler(DeleteBtnClicked, value); }
        }

        public static readonly RoutedEvent EditBtnClicked =
            EventManager.RegisterRoutedEvent("EditBtn_Click",
            RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(SetupUserControl));

        public event RoutedEventHandler EditBtn_Clicked
        {
            add { AddHandler(EditBtnClicked, value); }
            remove { RemoveHandler(EditBtnClicked, value); }
        }

        public static readonly RoutedEvent CreateMeetBtnClicked =
            EventManager.RegisterRoutedEvent("CreateMeetBtn_Click",
            RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(SetupUserControl));

        public event RoutedEventHandler CreateMeetBtn_Clicked
        {
            add { AddHandler(CreateMeetBtnClicked, value); }
            remove { RemoveHandler(CreateMeetBtnClicked, value); }
        }

        public static readonly RoutedEvent SaveMeetBtnClicked =
            EventManager.RegisterRoutedEvent("SaveMeetBtn_Click",
            RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(SetupUserControl));

        public event RoutedEventHandler SaveMeetBtn_Clicked
        {
            add { AddHandler(SaveMeetBtnClicked, value); }
            remove { RemoveHandler(SaveMeetBtnClicked, value); }
        }

        #endregion
    }
}
