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
/*
             * SqlConnection conn = new SqlConnection(
            "Data Source=.;Initial Catalog=QuanLyCuaHang;Integrated Security=True");
            conn.Open();
            con.Close();

            comand thuc hien lenh
                string sql = "INSERT INTO products(product_name, price) VALUES(@name,@price)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", tbName.Text);
                cmd.Parameters.AddWithValue("@price", tbPrice.Text);
                cmd.ExecuteNonQuery();

            adapter xuat datatable
                string sql = "SELECT * FROM products";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            
            reader doc 1 dong
                SqlCommand cmd = new SqlCommand("SELECT * FROM products", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    string name = reader["product_name"].ToString();
                }
 */

namespace BTL_QuanLyKhoHang_Nhom20
{
    public partial class StaffForm : Form
    {
        // da day form.tag la employee_id roi
        string staff_id = "";
        string chuoiKetNoi = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=QuanLyCuaHangQuanAo;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;"; public StaffForm()
        {
            InitializeComponent();
        }
        private void loadData(string lenh)
        {
            using (SqlConnection conn = new SqlConnection(chuoiKetNoi))
            {
                try
                {
                    conn.Open();
                    // Lấy các cột gốc từ Database
                    string sql = lenh;
                    SqlDataAdapter dtA = new SqlDataAdapter(sql, conn);
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
                    MessageBox.Show("Lỗi load dữ liệu: " + ex.Message);
                }
            }
        }
        private void StaffForm_Load(object sender, EventArgs e)
        {
            loadData("SELECT * FROM products");
            if (this.Tag != null)
            {
                staff_id = this.Tag.ToString();
            }
        }
        private void taoHoadon()
        {
            dataGridView2.RowHeadersVisible = false;//ko co tam giac
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.Rows.Clear();
            foreach (DataGridViewRow Row in dataGridView1.Rows)
            {
                bool chon = Convert.ToBoolean(Row.Cells[0].Value);
                if (chon)
                {
                    dataGridView2.Rows.Add(Row.Cells[1].Value,
                                            Row.Cells[2].Value,
                                            Row.Cells[3].Value,
                                            Row.Cells[4].Value,
                                            Row.Cells[5].Value,
                                            Row.Cells[6].Value);
                }
            }

        }
        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            taoHoadon();
            MessageBox.Show("Đã tạo hóa đơn");
            tabControl1.SelectedTab = tabPage2;
        }

        private void btTimKiem_Click(object sender, EventArgs e)
        {
            string loaitimkiem = comboBox3.SelectedItem.ToString();
            string tuTimkiem = tbTimKiem.Text;
            // Thêm dấu cách vào trước chữ LIKE
            string caulenh = "select * from products where " + loaitimkiem + " LIKE N'%" + tuTimkiem + "%'";
            loadData(caulenh);
        }

        private void TinhTien(object sender, EventArgs e)
        {
            int gia, soluongchon, thanhtien, soluongton, tong = 0;
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                gia = Convert.ToInt32(row.Cells[4].Value);
                soluongton = Convert.ToInt32(row.Cells[5].Value);
                soluongchon = Convert.ToInt32(row.Cells[6].Value);
                if (soluongchon > soluongton)
                {
                    MessageBox.Show("số lượng tồn không đủ đáp ứng nhu cầu");
                    return;
                }
                else
                {
                    thanhtien = gia * soluongchon;
                    row.Cells[7].Value = thanhtien.ToString();
                    tong += thanhtien;
                }
            }
            textBox3.Text = tong.ToString();
        }
        private string checkCustomer(string tenkhachhang, string sdt, int tongtien, SqlConnection con)
        {
            string cus_id = "";
            string sql = "select customer_id, totalSpending from customers " +
                         "where customer_name = N'" + tenkhachhang + "' and PhoneNumber = '" + sdt + "'";

            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataReader r = cmd.ExecuteReader();

            if (r.Read())
            {
                // TRƯỜNG HỢP KHÁCH CŨ
                cus_id = r["customer_id"].ToString();
                int spendingCu;
                if (r["totalSpending"] == DBNull.Value)
                {
                    spendingCu = 0;
                }
                else
                {
                    spendingCu = Convert.ToInt32(r["totalSpending"]);
                }
                r.Close(); // thực hiện lệnh UPDATE bên dưới
                cmd.CommandText = "UPDATE customers SET totalSpending = " + (spendingCu + tongtien) + " where customer_id = " + cus_id;
                cmd.ExecuteNonQuery();
            }
            else
            {
                // TRƯỜNG HỢP KHÁCH MỚI
                r.Close(); // Đóng reader của lệnh SELECT ban đầu

                // Thêm khách hàng mới
                cmd.CommandText = "insert into customers (customer_name, totalSpending, PhoneNumber) values "
                                + "(N'" + tenkhachhang + "', " + tongtien + ", '" + sdt + "')";
                cmd.ExecuteNonQuery();
                // Lấy lại ID 
                cmd.CommandText = "select customer_id from customers " +
                                  "where customer_name = N'" + tenkhachhang + "' and PhoneNumber = '" + sdt + "'";
                r = cmd.ExecuteReader();
                if (r.Read())
                {
                    cus_id = r["customer_id"].ToString();
                }
                r.Close(); // Đóng reader của lệnh SELECT lấy ID
            }
            return cus_id;
        }
        private void ThanhToan(object sender, EventArgs e)
        {
            TinhTien(sender, e);
            string tenkhachhang = textBox1.Text;
            string sdt = textBox2.Text;
            string cus_id="";
            int tongtien = Convert.ToInt32(textBox3.Text);
            int soluongton, soluongchon;
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection con = new SqlConnection(chuoiKetNoi))
            {
                cmd.Connection = con;
                try
                {
                    con.Open();
                    cus_id = checkCustomer(tenkhachhang, sdt, tongtien, con);
                    // cap nhat so luong ton
                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        int masp = Convert.ToInt32(row.Cells[0].Value);
                        soluongton = Convert.ToInt32(row.Cells[5].Value);
                        soluongchon = Convert.ToInt32(row.Cells[6].Value);
                        soluongton = soluongton - soluongchon;
                        cmd.CommandText = "update products set product_stockQuantity = " + soluongton + " where product_id = " + masp;
                        cmd.ExecuteNonQuery();
                    }
                    // Đảm bảo ép kiểu sang string rõ ràng và kiểm tra null
                    if (string.IsNullOrEmpty(cus_id) || string.IsNullOrEmpty(staff_id))
                    {
                        MessageBox.Show("Lỗi: Mã khách hàng hoặc mã nhân viên bị trống!");
                        return;
                    }

                    cmd.CommandText = "insert into orders (customer_id, employee_id) values (" + cus_id + ", " + staff_id + ")";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "select order_id from orders where customer_id = " + cus_id + " and employee_id = " + staff_id;
                    SqlDataReader r = cmd.ExecuteReader();
                    r.Read();
                    string order_id = r["order_id"].ToString();
                    r.Close();
                    // capnhat orderdetails
                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        // Lấy thông tin từ Grid
                        int masp = Convert.ToInt32(row.Cells[0].Value);
                        int slMua = Convert.ToInt32(row.Cells[6].Value);
                        // Lệnh chèn vào bảng orderDetails (Cộng chuỗi theo phong cách của bạn)
                        // Biến newOrderId là ID của hóa đơn chính vừa tạo ở bước trước
                        cmd.CommandText = "insert into orderDetails (order_id, product_id, Quantity) values (" +
                           order_id + ", " + masp + ", " + slMua + ")"; ;
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("đã thanh toán");
                    dataGridView2.Rows.Clear();
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi " + ex.ToString());
                }
            }
        }

        private void chuyentab(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                loadData("SELECT * FROM products");
            }
        }
    }
}
