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
        private BindingSource bindingSourceProducts = new BindingSource();
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

        public void LoadActiveProducts()
        {
            // 1. Chỉ Select các cột cần thiết và luôn có WHERE is_deleted is null
            string query = @"SELECT product_id, product_name, product_categoryName, product_sellingPrice, product_stockQuantity 
                           FROM products WHERE is_deleted is null"; // Khóa chặt dữ liệu đã xóa ở đây

            // 2. Sử dụng 'using' để tự động đóng kết nối DB, tránh rò rỉ bộ nhớ
            using (SqlConnection conn = new SqlConnection(chuoiKetNoi))
            {
                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dataTable = new DataTable();

                    // Đổ dữ liệu từ DB vào DataTable
                    adapter.Fill(dataTable);

                    // 3. Gán qua BindingSource (Giúp bạn dễ dàng filter hoặc tìm kiếm sau này)
                    bindingSourceProducts.DataSource = dataTable;

                    // Gán lên DataGridView
                    dataGridView1.AutoGenerateColumns = false;
                    dataGridView1.DataSource = bindingSourceProducts;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void FormatProductGrid()
        {
            // Đổi tên Header
            if (dataGridView1.Columns.Contains("product_id"))
            {
                dataGridView1.Columns["product_id"].HeaderText = "Mã SP";
            }

            if(dataGridView1.Columns.Contains("product_name"))
            {
                dataGridView1.Columns["product_name"].HeaderText = "Tên Sản Phẩm";
            }
            
            if(dataGridView1.Columns.Contains("product_categoryName"))
            {
                dataGridView1.Columns["product_categoryName"].HeaderText = "Danh Mục";
            }
            
            if(dataGridView1.Columns.Contains("product_sellingPrice"))
            {
                dataGridView1.Columns["product_sellingPrice"].HeaderText = "Giá Bán";
            }
            
            if(dataGridView1.Columns.Contains("product_stockQuantity"))
            {
                dataGridView1.Columns["product_stockQuantity"].HeaderText = "Tồn Kho";
            }
            
            // Format cột Giá bán thành tiền tệ (VD: 100,000)
            dataGridView1.Columns["product_sellingPrice"].DefaultCellStyle.Format = "N0";

            // Chỉnh kích thước cột tự động
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Ngăn người dùng sửa trực tiếp trên Grid (nếu bạn có form nhập liệu riêng)
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
        private void ManagerForm_Load(object sender, EventArgs e)
        {
            LoadActiveProducts();
        }

        private void btTimKiem_Click(object sender, EventArgs e)
        {
            
        }

        private void btThemSanPham_Click(object sender, EventArgs e)
        {
            InsertProductForm insertProduct = new InsertProductForm();
            insertProduct.ShowDialog();
        }

        private void btXoaSanPham_Click(object sender, EventArgs e)
        {
            // 1. Chốt trạng thái chỉnh sửa của DataGridView (BẮT BUỘC ĐỂ LẤY ĐƯỢC CHECKBOX)
            dataGridView1.EndEdit();

            // 2. Tạo một danh sách để gom các mã sản phẩm (ID) cần xóa
            List<string> listIdCanXoa = new List<string>();

            // 3. Duyệt qua từng dòng trong bảng
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Bỏ qua dòng trống dưới cùng (nếu có)
                if (row.IsNewRow) continue;

                // Lấy giá trị của ô Checkbox (Cột đầu tiên có Index = 0)
                // Nếu bạn đặt Name cho cột Checkbox thì có thể dùng row.Cells["Tên_Cột_Checkbox"]
                DataGridViewCheckBoxCell chkCell = row.Cells[0] as DataGridViewCheckBoxCell;

                // Kiểm tra xem ô đó có được tích không
                if (chkCell != null && chkCell.Value != null && (bool)chkCell.Value == true)
                {
                    // Lấy Mã sản phẩm ở Cột số 2 (Index = 1) hoặc dùng tên Name của cột ID
                    // Thay "colProductID" bằng tên Name của cột Mã Sản Phẩm trong màn hình Design của bạn
                    string id = row.Cells[1].Value.ToString();
                    listIdCanXoa.Add(id);
                }
            }

            // 4. Nếu người dùng chưa tích vào ô nào mà bấm Xóa
            if (listIdCanXoa.Count == 0)
            {
                MessageBox.Show("Vui lòng tích chọn ít nhất một sản phẩm để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 5. Hiện bảng hỏi xác nhận cho chắc cú
            DialogResult confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa {listIdCanXoa.Count} sản phẩm đã chọn?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                // 6. Kết nối Database và thực hiện Xóa mềm (Soft Delete)
                using (SqlConnection conn = new SqlConnection(chuoiKetNoi))
                {
                    conn.Open();
                    try
                    {
                        // Chạy vòng lặp để Update từng ID
                        foreach (string id in listIdCanXoa)
                        {
                            // Lệnh UPDATE đánh dấu is_deleted = 1
                            string sqlUpdate = "UPDATE products SET is_deleted = 1, deleted_at = GETDATE() WHERE product_id = @id";

                            using (SqlCommand cmd = new SqlCommand(sqlUpdate, conn))
                            {
                                cmd.Parameters.AddWithValue("@id", id);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        MessageBox.Show("Đã xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // 7. Load lại dữ liệu để các dòng đã xóa biến mất khỏi DataGridView
                        LoadActiveProducts();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
