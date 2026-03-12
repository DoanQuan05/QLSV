using System;
using System.Data;
using System.Windows.Forms;
using Demo01.BLL;

namespace Demo01
{
    public partial class Form2 : Form
    {
        private readonly StudentService _svService = new StudentService();
        private readonly ClassService _lopService = new ClassService();

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                LoadClasses();
                LoadStudents(isSearch: false);
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải dữ liệu.\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadClasses()
        {
            var dt = _lopService.GetAllTable();
            cboLop.DisplayMember = "TenLop";
            cboLop.ValueMember = "MaLop";
            cboLop.DataSource = dt;

            cboGioiTinh.Items.Clear();
            cboGioiTinh.Items.AddRange(new object[] { "Nam", "Nữ", "Khác" });
            if (cboGioiTinh.Items.Count > 0) cboGioiTinh.SelectedIndex = 0;
        }

        private void LoadStudents(bool isSearch)
        {
            DataTable dt = isSearch ? _svService.Search(txtTimKiem.Text) : _svService.GetAll();
            dataGridView1.DataSource = dt;

            if (dataGridView1.Columns.Contains("MaSV")) dataGridView1.Columns["MaSV"].HeaderText = "MSSV";
            if (dataGridView1.Columns.Contains("HoTen")) dataGridView1.Columns["HoTen"].HeaderText = "Họ và tên";
            if (dataGridView1.Columns.Contains("NgaySinh")) dataGridView1.Columns["NgaySinh"].HeaderText = "Ngày sinh";
            if (dataGridView1.Columns.Contains("GioiTinh")) dataGridView1.Columns["GioiTinh"].HeaderText = "Giới tính";
            if (dataGridView1.Columns.Contains("DiaChi")) dataGridView1.Columns["DiaChi"].HeaderText = "Địa chỉ";
            if (dataGridView1.Columns.Contains("MaLop")) dataGridView1.Columns["MaLop"].HeaderText = "Mã lớp";
            if (dataGridView1.Columns.Contains("TenLop")) dataGridView1.Columns["TenLop"].HeaderText = "Tên lớp";
        }

        private void ClearForm()
        {
            txtMaSV.Enabled = true;
            txtMaSV.Text = "";
            txtHoTen.Text = "";
            dtpNgaySinh.Checked = false;
            if (cboGioiTinh.Items.Count > 0) cboGioiTinh.SelectedIndex = 0;
            txtDiaChi.Text = "";
            if (cboLop.Items.Count > 0) cboLop.SelectedIndex = 0;
            txtMaSV.Focus();
        }

        private DateTime? GetNgaySinh()
        {
            return dtpNgaySinh.Checked ? (DateTime?)dtpNgaySinh.Value.Date : null;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            var ns = GetNgaySinh();
            var gt = cboGioiTinh.SelectedItem?.ToString();
            var maLop = cboLop.SelectedValue?.ToString();

            if (!_svService.Add(txtMaSV.Text, txtHoTen.Text, ns, gt, txtDiaChi.Text, maLop, out var err))
            {
                MessageBox.Show(err ?? "Không thể thêm sinh viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadStudents(isSearch: false);
            ClearForm();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            var ns = GetNgaySinh();
            var gt = cboGioiTinh.SelectedItem?.ToString();
            var maLop = cboLop.SelectedValue?.ToString();

            if (!_svService.Update(txtMaSV.Text, txtHoTen.Text, ns, gt, txtDiaChi.Text, maLop, out var err))
            {
                MessageBox.Show(err ?? "Không thể sửa sinh viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadStudents(isSearch: false);
            ClearForm();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            var ma = (txtMaSV.Text ?? "").Trim();
            if (ma.Length == 0) { MessageBox.Show("Chọn sinh viên cần xóa."); return; }

            if (MessageBox.Show($"Xóa sinh viên {ma}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            if (!_svService.Delete(ma, out var err))
            {
                MessageBox.Show(err ?? "Không thể xóa sinh viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadStudents(isSearch: false);
            ClearForm();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            LoadStudents(isSearch: true);
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            txtTimKiem.Text = "";
            LoadClasses();
            LoadStudents(isSearch: false);
            ClearForm();
        }

        private void btnQuanLyLop_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Nếu bạn muốn form quản lý lớp riêng giống layout này, nói mình để mình tạo.", "Thông báo");
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;
            var r = dataGridView1.CurrentRow;

            txtMaSV.Text = r.Cells["MaSV"]?.Value?.ToString();
            txtHoTen.Text = r.Cells["HoTen"]?.Value?.ToString();

            var nsObj = r.Cells["NgaySinh"]?.Value;
            if (nsObj == null || nsObj == DBNull.Value)
            {
                dtpNgaySinh.Checked = false;
            }
            else
            {
                dtpNgaySinh.Checked = true;
                dtpNgaySinh.Value = Convert.ToDateTime(nsObj);
            }

            var gt = r.Cells["GioiTinh"]?.Value?.ToString();
            if (!string.IsNullOrWhiteSpace(gt) && cboGioiTinh.Items.Contains(gt))
                cboGioiTinh.SelectedItem = gt;

            txtDiaChi.Text = r.Cells["DiaChi"]?.Value?.ToString();

            var maLop = r.Cells["MaLop"]?.Value?.ToString();
            if (!string.IsNullOrWhiteSpace(maLop))
                cboLop.SelectedValue = maLop;

            txtMaSV.Enabled = false;
        }

        // Các handler Designer có gắn nhưng không cần xử lý gì
        private void dtpNgaySinh_ValueChanged(object sender, EventArgs e) { }
        private void panelLeft_Paint(object sender, PaintEventArgs e) { }
    }
}