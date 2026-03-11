using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTL_QuanLyKhoHang_Nhom20
{
    public partial class ManagerForm : Form
    {
        string chuoiKetNoi = @"Data Source=HAPZ06\SQLEXPRESS;Initial Catalog=QuanLyCuaHangQuanAo;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True";
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

        private void loadData()
        {
            using (SqlConnection conn = new SqlConnection(chuoiKetNoi))
            {
                try
                {
                    conn.Open();
                    // Lấy các cột gốc từ Database
                    string query = "SELECT * FROM products";

                    SqlDataAdapter dtA = new SqlDataAdapter(query, conn);
                    DataTable table = new DataTable();
                    dtA.Fill(table);
                    dataGridView1.AutoGenerateColumns = false;
                    if (table.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = table;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi load dữ liệu: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void ManagerForm_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void btTimKiem_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string duLieuTK = tbTimKiem.Text;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                bool checkedIN = false;
                    for (int i = 1; i < row.Cells.Count; i++)
                    {
                        string dlcheck = row.Cells[i].Value.ToString();
                        if(dlcheck.Contains(duLieuTK))
                        {
                            checkedIN = true;
                            break;
                        }
                    }
                    if(checkedIN)
                    {
                        dt.Rows.Add(row);
                    }
            }
            dataGridView1.DataSource = dt;
        }

        private void btThemSanPham_Click(object sender, EventArgs e)
        {
            InsertProductForm insertProduct = new InsertProductForm();
            insertProduct.ShowDialog();
        }
    }
}
