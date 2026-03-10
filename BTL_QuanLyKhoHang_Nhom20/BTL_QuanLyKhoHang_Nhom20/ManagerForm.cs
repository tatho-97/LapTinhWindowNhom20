using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTL_QuanLyKhoHang_Nhom20
{
    public partial class ManagerForm : Form
    {
        public ManagerForm()
        {
            InitializeComponent();
        }

        private void ManagerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            LoginForm login = new LoginForm();
            login.Show();
  
            this.Hide();
        }
    }
}
