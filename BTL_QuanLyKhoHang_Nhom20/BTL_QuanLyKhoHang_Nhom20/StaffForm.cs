using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient; // thu vien su dung sql
namespace BTL_QuanLyKhoHang_Nhom20
{
    public partial class StaffForm : Form
    {
        string chuoiKetNoi = @"Data Source=HAPZ06\SQLEXPRESS;Initial Catalog=QuanLyCuaHangQuanAo;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;";
        public StaffForm()
        {
            InitializeComponent();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        private void loadData()
        {
            using (SqlConnection conn = new SqlConnection(chuoiKetNoi))
            {
                try
                {
                    conn.Open();
                    // Lấy các cột gốc từ Database
                    string sql = "SELECT * FROM products";

                    SqlDataAdapter dtA = new SqlDataAdapter(sql, conn);
                    DataTable table = new DataTable();
                    dtA.Fill(table);
                    dataGridView1.Columns.Clear();
                    dataGridView1.AutoGenerateColumns = false;
                    DataGridViewCheckBoxColumn check_colum = new DataGridViewCheckBoxColumn();
                    check_colum.HeaderText = "Chọn";
                    check_colum.Name = "col_chon";
                    dataGridView1.Columns.Add(check_colum);
                    dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "product_id", HeaderText = "Mã SP", Name = "product_id", Width = 70 });
                    dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "product_name", HeaderText = "Tên sản phẩm", Name = "product_name", Width = 180 });
                    dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "product_categoryName", HeaderText = "Loại", Name = "product_categoryName" });
                    dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "product_size", HeaderText = "Size", Name = "product_size", Width = 50 });
                    dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "product_sellingPrice", HeaderText = "Giá bán", Name = "product_sellingPrice" });
                    dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "product_stockQuantity", HeaderText = "Tồn kho", Name = "product_stockQuantity" });
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
        private void StaffForm_Load(object sender, EventArgs e)
        {
            loadData();
        }

    }
}
