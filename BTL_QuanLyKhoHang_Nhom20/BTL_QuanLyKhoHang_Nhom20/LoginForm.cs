using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient; // Dùng cái này nếu bạn vừa cài NuGet trên
// Để dùng các đối tượng như CommandType, DataTable
namespace BTL_QuanLyKhoHang_Nhom20
{
    public partial class LoginForm : Form
    {
        string chuoiKetNoi = @"Data Source=HAPZ06\SQLEXPRESS;Initial Catalog=QuanLyCuaHangQuanAo;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;"; SqlConnection conn = null;
        //hien-"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=QuanLyCuaHangQuanAo;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;"
        public LoginForm()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }
        private void login(object sender, EventArgs e)
        {
            string user = textBox1.Text.Trim();//lược khoảng trắng
            string pas = textBox2.Text.Trim();
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pas))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tài khoản và mật khẩu!");
                return;
            }
            using (SqlConnection conn = new SqlConnection(chuoiKetNoi))
            {
                try
                {
                    conn.Open();
                    // Truy vấn lấy RoleID và Tên nhân viên
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT RoleID, E_name FROM employees WHERE us_name = '"
                                      + user + "' AND us_password = '" + pas + "'";
                    SqlDataReader reader= cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        int role = Convert.ToInt32(reader["RoleID"]);
                        string name = reader["E_name"].ToString();
                        MessageBox.Show("Đăng nhập thành công. Xin chào " + name);
                        this.Hide();
                        if (role == 1)
                        {
                            ManagerForm form = new ManagerForm();
                            form.ShowDialog();
                        }
                        if(role==2)
                        {
                            StaffForm form = new StaffForm();
                            form.ShowDialog();
                        }

                    }
                    else
                    {
                        MessageBox.Show("Sai tài khoản hoặc mật khẩu");
                    }

                    reader.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối: " + ex.Message);
                }
            }
        }

        private void btnThoat(object sender, EventArgs e)
        {
            this.Close();
        }

        private void hidePassword(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                textBox2.UseSystemPasswordChar = false;
            else
                textBox2.UseSystemPasswordChar = true;
        }
    }
}
