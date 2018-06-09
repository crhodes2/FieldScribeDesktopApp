using Fieldscribe_Windows_App.Controllers;
using Fieldscribe_Windows_App.Models;
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

namespace Fieldscribe_Windows_App
{
    /// <summary>
    /// Interaction logic for ScribesUserControl.xaml
    /// </summary>
    public partial class ScribesListUserControl : UserControl
    {
        public ScribesListUserControl()
        {
            InitializeComponent();

            Scribes.ItemsSource = PopulateList().Select(scribe => new Scribes
            {
                FirstName = scribe.FirstName,
                LastName = scribe.LastName,
                Email = scribe.Email
            });

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ScribesUserControl dashBoard = new ScribesUserControl();
            dashBoard.Show();
            this.Close();
        }
        private void Help_Click(object sender, RoutedEventArgs e)
        {
            if (HelpScreen.activeInstance == false)
            {
                HelpScreen dashBoard = new HelpScreen();
                dashBoard.Show();
            }
        }

        IList<Scribes> PopulateList()
        {
            ScribesController scribesControl = new ScribesController();

            // Retrieve an IList<Meet> that will include all meets
            return scribesControl.RetrieveAllScribes();
        }

    }
}
