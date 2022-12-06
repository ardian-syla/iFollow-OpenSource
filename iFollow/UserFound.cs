using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iFollow
{
    public partial class UserFound : Form
    {
        public UserFound()
        {
            InitializeComponent();
        }
        [DllImport("user32")]
        private static extern bool ReleaseCapture();

        [DllImport("user32")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wp, int lp);

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, 161, 2, 0);
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void loginBTN_Click(object sender, EventArgs e)
        {
        }

        private void UserFound_Load(object sender, EventArgs e)
        {
            idText.Text = iFollow.PublicId;
            UsernameText.Text = iFollow.PublicUsername;
            FollowersText.Text = iFollow.PublicFollowers;
            FollowingsText.Text = iFollow.PublicFollowings;
            Thread.Sleep(5000);
            UserFound user = new UserFound();
            user.Hide();
        }
    }
}
