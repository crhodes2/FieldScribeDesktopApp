using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fieldscribe_Windows_App
{
    public partial class FSSplashScreen : Form
    {
        public FSSplashScreen()
        {
            InitializeComponent();
        }

        private void FSSplashScreen_Load(object sender, EventArgs e)
        {
            staticLogo.Visible = false;
            pictureBox1.Visible = true;
            time.Start();
        }


        private void staticLogo_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void time_Tick(object sender, EventArgs e)
        {
            time.Stop();
            staticLogo.Visible = true;
            pictureBox1.Visible = false;
        }
    }
}
