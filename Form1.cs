using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows.Forms;

namespace TrieuNgocHao_1150080050_BTTuan8
{
    public partial class Form1 : Form
    {

        private readonly string _cs = ConfigurationManager.ConnectionStrings["Pg"].ConnectionString;
        private SqlConnection sqlCon = null;
        private SqlDataAdapter adapter = null;
        private DataSet ds = null;
        private int vt = -1;

        public Form1()
        {
            InitializeComponent();
        }

        private void MoKetNoi()
        {
            if (sqlCon == null) sqlCon = new SqlConnection(_cs);
            if (sqlCon.State == ConnectionState.Closed) sqlCon.Open();
        }

        private void DongKetNoi()
        {
            if (sqlCon != null && sqlCon.State == ConnectionState.Open) sqlCon.Close();
        }
        private void XoaDuLieuForm()
        {
            txtMaXB.Text = txtTenXB.Text = txtDiaChi.Text = "";
        }
        private void HienThiDuLieu()
        {
            try
            {
                MoKetNoi();
                string query = "SELECT * FROM NhaXuatBan"; 
                adapter = new SqlDataAdapter(query, sqlCon);
                ds = new DataSet();
                adapter.Fill(ds, "tblNhaXuatBan");
                dgvDanhSach.AutoGenerateColumns = true;
                dgvDanhSach.DataSource = ds.Tables["tblNhaXuatBan"];
            }
            finally { DongKetNoi(); }
        }

        private void btnHienThi_Click(object sender, EventArgs e)
        {
            HienThiDuLieu();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            HienThiDuLieu();
            XoaDuLieuForm();
        }

        private void dgvDanhSach_Click(object sender, DataGridViewCellEventArgs e)
        {
            vt = e.RowIndex;
            if (vt < 0 || ds == null) return;
            var row = ds.Tables["tblNhaXuatBan"].Rows[vt];
            txtMaXB.Text = row["MaXB"].ToString().Trim();
            txtTenXB.Text = row["TenXB"].ToString().Trim();
            txtDiaChi.Text = row["DiaChi"].ToString().Trim();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (ds == null || adapter == null) { HienThiDuLieu(); }

            using (var builder = new SqlCommandBuilder(adapter))
            {
                var tbl = ds.Tables["tblNhaXuatBan"];
                var row = tbl.NewRow();
                row["MaXB"] = txtMaXB.Text.Trim();
                row["TenXB"] = txtTenXB.Text.Trim();
                row["DiaChi"] = txtDiaChi.Text.Trim();
                tbl.Rows.Add(row);

                MoKetNoi();
                int kq = adapter.Update(ds.Tables["tblNhaXuatBan"]);
                DongKetNoi();

                if (kq > 0)
                {
                    MessageBox.Show("Thêm dữ liệu thành công!");
                    HienThiDuLieu();
                    XoaDuLieuForm();
                }
                else MessageBox.Show("Thêm dữ liệu không thành công!");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (vt < 0 || ds == null || adapter == null)
            {
                MessageBox.Show("Bạn chưa chọn dữ liệu để xóa!");
                return;
            }

            if (MessageBox.Show("Bạn có thực sự muốn xóa hay không?",
                                "Cảnh báo",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning) != DialogResult.Yes) return;

            using (var builder = new SqlCommandBuilder(adapter))
            {
                ds.Tables["tblNhaXuatBan"].Rows[vt].Delete();

                MoKetNoi();
                int kq = adapter.Update(ds.Tables["tblNhaXuatBan"]);
                DongKetNoi();

                if (kq > 0)
                {
                    MessageBox.Show("Xóa dữ liệu thành công!");
                    HienThiDuLieu();
                    XoaDuLieuForm();
                }
                else MessageBox.Show("Xóa dữ liệu không thành công!");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (vt < 0 || ds == null || adapter == null)
            {
                MessageBox.Show("Bạn chưa chọn dữ liệu để chỉnh sửa!");
                return;
            }

            using (var builder = new SqlCommandBuilder(adapter))
            {
                var row = ds.Tables["tblNhaXuatBan"].Rows[vt];
                row.BeginEdit();
                row["MaXB"] = txtMaXB.Text.Trim();
                row["TenXB"] = txtTenXB.Text.Trim();
                row["DiaChi"] = txtDiaChi.Text.Trim();
                row.EndEdit();

                MoKetNoi();
                int kq = adapter.Update(ds.Tables["tblNhaXuatBan"]);
                DongKetNoi();

                if (kq > 0)
                {
                    MessageBox.Show("Chỉnh sửa dữ liệu thành công!");
                    HienThiDuLieu();
                    XoaDuLieuForm();
                }
                else MessageBox.Show("Chỉnh sửa dữ liệu không thành công!");
            }
        }
    }
}

