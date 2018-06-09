using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Fieldscribe_Windows_App
{
    public class BootStrapper
    {
        Intro Splash = new Intro();
        LoginScreen Login = new LoginScreen();

        public void Start()
        {
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += (s, e) =>
            {
                Splash.Close();
                Login.Show();
                timer.Stop();
            };

            timer.Start();
            Splash.Show();
        }
    }
}
