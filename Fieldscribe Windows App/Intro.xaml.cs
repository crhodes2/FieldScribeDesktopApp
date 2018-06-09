using System.Threading;
using System.Windows;

namespace Fieldscribe_Windows_App
{
    /// <summary>
    /// Interaction logic for Intro.xaml
    /// </summary>
    public partial class Intro : Window
    {
        public Intro()
        {
            InitializeComponent();
        }

        private void ImageBehavior_OnAnimationCompleted(object sender, RoutedEventArgs e)
        {
            //this.Close();
            //LoginScreen loginScreen = new LoginScreen();
            //loginScreen.Show();

        }

        public void Outro()
        {
            this.Close();
            //LoginScreen login = new LoginScreen();
            //login.Show();
        }
    }
}
