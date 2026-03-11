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
        private void taoHoadon()
        {
            dataGridView2.RowHeadersVisible = false;//ko co tam giac
            dataGridView2.AllowUserToAddRows = false;
            foreach (DataGridViewRow Row in dataGridView1.Rows)
            {
                bool chon = Convert.ToBoolean(Row.Cells[0].Value);
                if (chon)
                {
                    int n = dataGridView2.Rows.Add();

                    // Gán dữ liệu từ sourceRow (bảng 1) sang dòng mới ở dataGridView2
                    // Lưu ý: Chỉ số Cells[x] phải khớp với thứ tự cột bạn đã tạo ở trên
                    dataGridView2.Rows[n].Cells[0].Value = true; // Đã chọn
                    dataGridView2.Rows[n].Cells[1].Value = Row.Cells[1].Value; // Mã SP
                    dataGridView2.Rows[n].Cells[2].Value = Row.Cells[2].Value; // Tên SP
                    dataGridView2.Rows[n].Cells[3].Value = Row.Cells[3].Value; // Loại
                    dataGridView2.Rows[n].Cells[4].Value = Row.Cells[4].Value; // Size
                    dataGridView2.Rows[n].Cells[5].Value = Row.Cells[5].Value; // Giá bán
                    dataGridView2.Rows[n].Cells[6].Value = Row.Cells[6].Value; // Tồn kho
                }
            }
            
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
        private void StaffForm_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            taoHoadon();
            MessageBox.Show("Đã tạo hóa đơn");
            tabControl1.SelectedTab = tabPage2;
        }
    }
}
