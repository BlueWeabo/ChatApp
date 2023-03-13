using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatApp
{
    public partial class LoginRegister : Form
    {
        public LoginRegister()
        {
            InitializeComponent();
            LoginRegisterSplit.Panel1.Show();
            LoginRegisterSplit.SplitterDistance = Width;
            LoginRegisterSplit.Panel2.Hide();
        }

        private void GoToLoginLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoginRegisterSplit.Panel1.Show();
            LoginRegisterSplit.SplitterDistance = Width;
            LoginRegisterSplit.Panel2.Hide();
        }

        private void GoToRegisterLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoginRegisterSplit.Panel2.Show();
            LoginRegisterSplit.SplitterDistance = 0;
            LoginRegisterSplit.Panel1.Hide();
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {

        }

        private void LoginButton_Click(object sender, EventArgs e)
        {

        }
    }
}
