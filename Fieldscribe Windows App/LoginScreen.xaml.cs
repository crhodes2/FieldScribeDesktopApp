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
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Threading;
using Fieldscribe_Windows_App.Infrastructure;
using System.ComponentModel;
using Fieldscribe_Windows_App.Controllers;
using Fieldscribe_Windows_App.Models;

namespace Fieldscribe_Windows_App
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        private TokenManager _tokenManager;
        private bool _loginSuccess;
        private AppDataModel _dataModel;
        //FSSplashScreen splash = new FSSplashScreen();

        public void StartApp()
        {
            _dataModel = AppDataModel.Instance;

            //System.Windows.Forms.Application.Run(splash);
        }

        public LoginScreen()
        {  
            /*
            Thread splashScreenThread = new Thread(new ThreadStart(StartApp));
            splashScreenThread.Start();
            Thread.Sleep(4500);

            splashScreenThread.Interrupt();
            */
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            Topmost = true;
            InitializeComponent();

            _tokenManager = TokenManager.Instance;

            /*
            try
            {
                splash.Close();
            }
            catch (InvalidOperationException)
            {

            }
            */
        }

        private void OpenMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            _loginSuccess = false;

            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_AuthenticateUser;
            worker.RunWorkerCompleted += worker_AuthenticationComplete;
            worker.RunWorkerAsync();

            _tokenManager.UserCredentials = new Credentials
            {
                Username = UsernameTextBox.Text,
                Password = PasswordTextBox.Password
            };

            InvalidLoginText.Visibility = Visibility.Hidden;
            LoginProgressBar.Visibility = Visibility.Visible;
        }

        private void worker_AuthenticateUser(object sender, DoWorkEventArgs e)
        {
            // Username + Password check
            (bool tokenSuccess, string Error) = _tokenManager.GetToken();

            if(tokenSuccess)
            {
                UsersController uc = new UsersController();

                (bool roleSuccess, User loggedInUser) = uc.GetLoggedInUser(
                    _tokenManager.Token);

                // Role check
                if(loggedInUser.Roles.Contains("Admin") ||
                    loggedInUser.Roles.Contains("Timer"))
                {
                    _loginSuccess = true;
                }
            }
        }

        private void worker_AuthenticationComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if(!_loginSuccess)
            {
                LoginProgressBar.Visibility = Visibility.Hidden;
                InvalidLoginText.Visibility = Visibility.Visible;
            }
            else
            {
                // Program Opens
                FieldScribeModernDesign dashBoard = new FieldScribeModernDesign();
                dashBoard.Show();
                this.Close();
            }
        }
    }
}
