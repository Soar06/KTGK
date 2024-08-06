using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace De02
{
    public partial class Form1 : Form
    {
        private string connectionString = "data source=DESKTOP-06K77T5;initial catalog=QLSanPham;integrated security=True;trustservercertificate=True;MultipleActiveResultSets=True;App=EntityFramework";
        public Form1()
        {   
            InitializeComponent();
            btLuu.Click += new EventHandler(btLuu_Click);
            btKLuu.Click += new EventHandler(btKLuu_Click);
            btTim.Click += new EventHandler(btTim_Click);
            btXoa.Click += new EventHandler(btXoa_Click);
            Thoat.Click += new EventHandler(Thoat_Click);

            
            btLuu.Enabled = false;
            btKLuu.Enabled = false;
        }

        private void dtNgayNhap_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtTenSP_TextChanged(object sender, EventArgs e)
        {

        }

        private void lvSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadDataIntoListView();
            LoadDataIntoComboBox();

        }
        private void LoadDataIntoListView()
        {
            
            string query = "SELECT MaSP, TenSP, Ngaynhap, MaLoai FROM Sanpham";

            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            lvSanPham.Items.Clear();
                            lvSanPham.Columns.Clear();
                            lvSanPham.Columns.Add("Mã SP", 60, HorizontalAlignment.Left);
                            lvSanPham.Columns.Add("Tên sản phẩm", 120, HorizontalAlignment.Left);
                            lvSanPham.Columns.Add("Ngày nhập", 120, HorizontalAlignment.Left);
                            lvSanPham.Columns.Add("Loại SP", 100, HorizontalAlignment.Left);

                            while (reader.Read())
                            {
                                ListViewItem item = new ListViewItem(reader["MaSP"].ToString());
                                item.SubItems.Add(reader["TenSP"].ToString());
                                item.SubItems.Add(Convert.ToDateTime(reader["Ngaynhap"]).ToString("dd/MM/yyyy HH:mm"));
                                item.SubItems.Add(reader["MaLoai"].ToString());

                                lvSanPham.Items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private void LoadDataIntoComboBox()
        {
            string query = "SELECT MaLoai, TenLoai FROM LoaiSP";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            cboLoaiSP.Items.Clear();

                            while (reader.Read())
                            {
                                string id = reader["MaLoai"].ToString();
                                string name = reader["TenLoai"].ToString();
                                string displayText = $"{id}: {name}";

                                cboLoaiSP.Items.Add(displayText);
                            }

                            foreach (var item in cboLoaiSP.Items)
                            {
                                if (item.ToString().StartsWith("1:"))
                                {
                                    cboLoaiSP.SelectedItem = item;
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btThem_Click(object sender, EventArgs e)
        {
            string maSP = txtMaSP.Text.Trim();
            string tenSP = txtTenSP.Text.Trim();
            DateTime ngayNhap = dtNgayNhap.Value;
            string selectedItem = cboLoaiSP.SelectedItem.ToString();
            string maLoai = selectedItem.Split(':')[0].Trim();

            string query = "INSERT INTO Sanpham (MaSP, TenSP, Ngaynhap, MaLoai) VALUES (@MaSP, @TenSP, @NgayNhap, @MaLoai)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {

                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@MaSP", maSP);
                        command.Parameters.AddWithValue("@TenSP", tenSP);
                        command.Parameters.AddWithValue("@NgayNhap", ngayNhap);
                        command.Parameters.AddWithValue("@MaLoai", maLoai);


                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Them san pham thanh cong");
                            LoadDataIntoListView();
                        }
                        else
                        {
                            MessageBox.Show("Loi: Khong them duoc.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void txtTimTheoId_TextChanged(object sender, EventArgs e)
        {

        }

        private void btTim_Click(object sender, EventArgs e)
        {
            // Retrieve the MaSP from the text box
            string maSPToFind = txtTimTheoId.Text.Trim();

            // SQL query to find the product by MaSP
            string query = "SELECT MaSP, TenSP, Ngaynhap, MaLoai FROM Sanpham WHERE MaSP = @MaSP";

            // Create a new SqlConnection
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Open the connection
                    connection.Open();

                    // Create a SqlCommand to execute the query
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameter to prevent SQL injection
                        command.Parameters.AddWithValue("@MaSP", maSPToFind);

                        // Execute the query and get a SqlDataReader
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Retrieve details from the reader
                                string maSP = reader["MaSP"].ToString();
                                string tenSP = reader["TenSP"].ToString();
                                string ngayNhapText = reader["Ngaynhap"].ToString();
                                string maLoai = reader["MaLoai"].ToString();

                                // Set values to the controls
                                txtMaSP.Text = maSP;
                                txtTenSP.Text = tenSP;
                                dtNgayNhap.Value = Convert.ToDateTime(ngayNhapText);

                                // Find and select the appropriate item in the ComboBox
                                foreach (var item in cboLoaiSP.Items)
                                {
                                    if (item.ToString().StartsWith(maLoai))
                                    {
                                        cboLoaiSP.SelectedItem = item;
                                        break;
                                    }
                                }
                                btLuu.Enabled = true;
                                btKLuu.Enabled = true;
                            }

                            else
                            {
                                MessageBox.Show("Khong tim thay");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btXoa_Click(object sender, EventArgs e)
        {
                // Retrieve the MaSP from the text box
                string maSPToDelete = txtMaSP.Text.Trim();

                if (string.IsNullOrEmpty(maSPToDelete))
                {
                    MessageBox.Show("Khong co san pham nao duoc chon");
                    return;
                }

                // SQL query to delete the product by MaSP
                string query = "DELETE FROM Sanpham WHERE MaSP = @MaSP";

                // Create a new SqlConnection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        // Open the connection
                        connection.Open();

                        // Create a SqlCommand to execute the query
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // Add parameter to prevent SQL injection
                            command.Parameters.AddWithValue("@MaSP", maSPToDelete);

                            // Execute the query
                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Xoa thanh cong");

                                // Clear the form controls
                                txtMaSP.Clear();
                                txtTenSP.Clear();
                                dtNgayNhap.Value = DateTime.Now;
                                cboLoaiSP.SelectedIndex = -1;

                                // Refresh the ListView
                                LoadDataIntoListView();
                            }
                            else
                            {
                                MessageBox.Show("Chua co san pham nao duoc chon");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            

        }

        private void Thoat_Click(object sender, EventArgs e)
        {
                Application.Exit();
        }

        private void btLuu_Click(object sender, EventArgs e)
        {
            // Retrieve the product details from the form controls
            string maSP = txtMaSP.Text.Trim();
            string tenSP = txtTenSP.Text.Trim();
            DateTime ngayNhap = dtNgayNhap.Value;
            string maLoai = cboLoaiSP.SelectedItem?.ToString().Split(':')[0].Trim();

            if (string.IsNullOrEmpty(maSP))
            {
                MessageBox.Show("TIm san pham de cap nhat");
                return;
            }

            // SQL query to update the product
            string query = "UPDATE Sanpham SET TenSP = @TenSP, Ngaynhap = @Ngaynhap, MaLoai = @MaLoai WHERE MaSP = @MaSP";

            // Create a new SqlConnection
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Open the connection
                    connection.Open();

                    // Create a SqlCommand to execute the query
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@MaSP", maSP);
                        command.Parameters.AddWithValue("@TenSP", tenSP);
                        command.Parameters.AddWithValue("@Ngaynhap", ngayNhap);
                        command.Parameters.AddWithValue("@MaLoai", maLoai);

                        // Execute the query
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Cap nhat thanh cong");

                            // Refresh the ListView
                            LoadDataIntoListView();
                        }
                        else
                        {
                            MessageBox.Show("Cap nhat that bai");
                        }

                        // Disable the buttons
                        btLuu.Enabled = false;
                        btKLuu.Enabled = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void btKLuu_Click(object sender, EventArgs e)
        {
            // Clear the form controls
            txtMaSP.Clear();
            txtTenSP.Clear();
            dtNgayNhap.Value = DateTime.Now;
            cboLoaiSP.SelectedIndex = -1;

            // Disable the buttons
            btLuu.Enabled = false;
            btKLuu.Enabled = false;

            // Show notification
            MessageBox.Show("Thay doi bi xoa");
        }
    }
}
