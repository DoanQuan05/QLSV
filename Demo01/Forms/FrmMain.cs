using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Demo01.BLL;

namespace Demo01.Forms
{
    public class FrmMain : Form
    {
        private readonly StudentService _svService = new StudentService();
        private readonly ClassService _lopService = new ClassService();

        // Students tab controls
        private DataGridView _gvSv;
        private TextBox _txtMaSv;
        private TextBox _txtHoTen;
        private DateTimePicker _dtNgaySinh;
        private ComboBox _cboGioiTinh;
        private TextBox _txtDiaChi;
        private ComboBox _cboLop;
        private TextBox _txtSearch;

        // Classes tab controls
        private DataGridView _gvLop;
        private TextBox _txtMaLop;
        private TextBox _txtTenLop;

        public FrmMain(string loggedInUser)
        {
            Text = $"Quản lý sinh viên - Xin chào {loggedInUser}";
            Width = 1100;
            Height = 650;
            StartPosition = FormStartPosition.CenterScreen;

            BuildUi();
            LoadData();
        }

        private void BuildUi()
        {
            var tabs = new TabControl { Dock = DockStyle.Fill };
            tabs.TabPages.Add(BuildStudentsTab());
            tabs.TabPages.Add(BuildClassesTab());
            Controls.Add(tabs);
        }

        private TabPage BuildStudentsTab()
        {
            var page = new TabPage("Sinh viên");
            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                SplitterDistance = 360,
                FixedPanel = FixedPanel.Panel1
            };

            // Left: input + actions
            var panelLeft = new Panel { Dock = DockStyle.Fill, Padding = new Padding(12) };
            var title = new Label { Text = "Thông tin sinh viên", Font = new Font(Font, FontStyle.Bold), AutoSize = true };
            panelLeft.Controls.Add(title);

            int top = 35;
            panelLeft.Controls.Add(MkLabel("Mã SV", 0, top));
            _txtMaSv = MkTextBox(0, top); panelLeft.Controls.Add(_txtMaSv);
            top += 36;

            panelLeft.Controls.Add(MkLabel("Họ tên", 0, top));
            _txtHoTen = MkTextBox(0, top); panelLeft.Controls.Add(_txtHoTen);
            top += 36;

            panelLeft.Controls.Add(MkLabel("Ngày sinh", 0, top));
            _dtNgaySinh = new DateTimePicker { Left = 110, Top = top - 3, Width = 210, Format = DateTimePickerFormat.Short, ShowCheckBox = true };
            panelLeft.Controls.Add(_dtNgaySinh);
            top += 36;

            panelLeft.Controls.Add(MkLabel("Giới tính", 0, top));
            _cboGioiTinh = new ComboBox { Left = 110, Top = top - 3, Width = 210, DropDownStyle = ComboBoxStyle.DropDownList };
            _cboGioiTinh.Items.AddRange(new object[] { "Nam", "Nữ", "Khác" });
            _cboGioiTinh.SelectedIndex = 0;
            panelLeft.Controls.Add(_cboGioiTinh);
            top += 36;

            panelLeft.Controls.Add(MkLabel("Địa chỉ", 0, top));
            _txtDiaChi = MkTextBox(0, top); panelLeft.Controls.Add(_txtDiaChi);
            top += 36;

            panelLeft.Controls.Add(MkLabel("Lớp", 0, top));
            _cboLop = new ComboBox { Left = 110, Top = top - 3, Width = 210, DropDownStyle = ComboBoxStyle.DropDownList };
            panelLeft.Controls.Add(_cboLop);
            top += 44;

            var btnAdd = MkButton("Thêm", 0, top); btnAdd.Click += (s, e) => AddStudent();
            var btnUpdate = MkButton("Sửa", 110, top); btnUpdate.Click += (s, e) => UpdateStudent();
            var btnDelete = MkButton("Xóa", 220, top); btnDelete.Click += (s, e) => DeleteStudent();
            top += 40;
            var btnClear = MkButton("Làm mới", 0, top); btnClear.Width = 320; btnClear.Click += (s, e) => ClearStudentForm();

            panelLeft.Controls.Add(btnAdd);
            panelLeft.Controls.Add(btnUpdate);
            panelLeft.Controls.Add(btnDelete);
            panelLeft.Controls.Add(btnClear);

            split.Panel1.Controls.Add(panelLeft);

            // Right: search (Dock Top) + grid (Dock Fill) => không còn khoảng trống phía trên
            var panelRight = new Panel { Dock = DockStyle.Fill, Padding = new Padding(12) };

            var panelSearch = new Panel { Dock = DockStyle.Top, Height = 32 };
            var lblSearch = new Label { Left = 0, Top = 8, Width = 80, Text = "Tìm kiếm" };
            _txtSearch = new TextBox { Left = 80, Top = 5, Width = 700 };
            var btnSearch = new Button { Left = 790, Top = 3, Width = 90, Height = 26, Text = "Tìm" };
            var btnReload = new Button { Left = 885, Top = 3, Width = 90, Height = 26, Text = "Tải lại" };

            btnSearch.Click += (s, e) => LoadStudents(isSearch: true);
            btnReload.Click += (s, e) => { _txtSearch.Text = ""; LoadStudents(isSearch: false); };

            panelSearch.Controls.Add(lblSearch);
            panelSearch.Controls.Add(_txtSearch);
            panelSearch.Controls.Add(btnSearch);
            panelSearch.Controls.Add(btnReload);

            _gvSv = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            _gvSv.CellClick += (s, e) => FillStudentFormFromGrid();

            panelRight.Controls.Add(_gvSv);
            panelRight.Controls.Add(panelSearch);
            split.Panel2.Controls.Add(panelRight);

            page.Controls.Add(split);
            return page;
        }

        private TabPage BuildClassesTab()
        {
            var page = new TabPage("Lớp");
            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                SplitterDistance = 360,
                FixedPanel = FixedPanel.Panel1
            };

            var left = new Panel { Dock = DockStyle.Fill, Padding = new Padding(12) };
            left.Controls.Add(new Label { Text = "Quản lý lớp", Font = new Font(Font, FontStyle.Bold), AutoSize = true });

            int top = 35;
            left.Controls.Add(MkLabel("Mã lớp", 0, top));
            _txtMaLop = MkTextBox(0, top); left.Controls.Add(_txtMaLop);
            top += 36;

            left.Controls.Add(MkLabel("Tên lớp", 0, top));
            _txtTenLop = MkTextBox(0, top); left.Controls.Add(_txtTenLop);
            top += 44;

            var btnAdd = MkButton("Thêm", 0, top); btnAdd.Click += (s, e) => AddClass();
            var btnUpdate = MkButton("Sửa", 110, top); btnUpdate.Click += (s, e) => UpdateClass();
            var btnDelete = MkButton("Xóa", 220, top); btnDelete.Click += (s, e) => DeleteClass();
            top += 40;
            var btnClear = MkButton("Làm mới", 0, top); btnClear.Width = 320; btnClear.Click += (s, e) => ClearClassForm();

            left.Controls.Add(btnAdd);
            left.Controls.Add(btnUpdate);
            left.Controls.Add(btnDelete);
            left.Controls.Add(btnClear);

            split.Panel1.Controls.Add(left);

            _gvLop = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            _gvLop.CellClick += (s, e) => FillClassFormFromGrid();
            split.Panel2.Controls.Add(_gvLop);

            page.Controls.Add(split);
            return page;
        }

        private void LoadData()
        {
            LoadClasses();
            LoadStudents(isSearch: false);
        }

        private void LoadClasses()
        {
            var dt = _lopService.GetAllTable();
            _gvLop.DataSource = dt;

            _cboLop.DisplayMember = "TenLop";
            _cboLop.ValueMember = "MaLop";
            _cboLop.DataSource = dt.Copy();
        }

        private void LoadStudents(bool isSearch)
        {
            DataTable dt = isSearch ? _svService.Search(_txtSearch.Text) : _svService.GetAll();
            _gvSv.DataSource = dt;

            if (_gvSv.Columns.Contains("MaSV")) _gvSv.Columns["MaSV"].HeaderText = "Mã SV";
            if (_gvSv.Columns.Contains("HoTen")) _gvSv.Columns["HoTen"].HeaderText = "Họ và tên";
            if (_gvSv.Columns.Contains("NgaySinh")) _gvSv.Columns["NgaySinh"].HeaderText = "Ngày sinh";
            if (_gvSv.Columns.Contains("GioiTinh")) _gvSv.Columns["GioiTinh"].HeaderText = "Giới tính";
            if (_gvSv.Columns.Contains("DiaChi")) _gvSv.Columns["DiaChi"].HeaderText = "Địa chỉ";
            if (_gvSv.Columns.Contains("MaLop")) _gvSv.Columns["MaLop"].HeaderText = "Mã lớp";
            if (_gvSv.Columns.Contains("TenLop")) _gvSv.Columns["TenLop"].HeaderText = "Tên lớp";
        }

        private void FillStudentFormFromGrid()
        {
            if (_gvSv.CurrentRow == null) return;
            var r = _gvSv.CurrentRow;

            _txtMaSv.Text = r.Cells["MaSV"]?.Value?.ToString();
            _txtHoTen.Text = r.Cells["HoTen"]?.Value?.ToString();

            var nsObj = r.Cells["NgaySinh"]?.Value;
            if (nsObj == null || nsObj == DBNull.Value)
            {
                _dtNgaySinh.Checked = false;
            }
            else
            {
                _dtNgaySinh.Checked = true;
                _dtNgaySinh.Value = Convert.ToDateTime(nsObj);
            }

            var gt = r.Cells["GioiTinh"]?.Value?.ToString();
            if (!string.IsNullOrWhiteSpace(gt) && _cboGioiTinh.Items.Contains(gt))
                _cboGioiTinh.SelectedItem = gt;

            _txtDiaChi.Text = r.Cells["DiaChi"]?.Value?.ToString();

            var maLop = r.Cells["MaLop"]?.Value?.ToString();
            if (!string.IsNullOrWhiteSpace(maLop))
                _cboLop.SelectedValue = maLop;

            // không cho sửa MaSV khi đang chọn dòng
            _txtMaSv.Enabled = false;
        }

        private void ClearStudentForm()
        {
            _txtMaSv.Enabled = true;
            _txtMaSv.Text = "";
            _txtHoTen.Text = "";
            _dtNgaySinh.Checked = false;
            _cboGioiTinh.SelectedIndex = 0;
            _txtDiaChi.Text = "";
            if (_cboLop.Items.Count > 0) _cboLop.SelectedIndex = 0;
            _txtMaSv.Focus();
        }

        private void AddStudent()
        {
            var ngaySinh = _dtNgaySinh.Checked ? (DateTime?)_dtNgaySinh.Value.Date : null;
            var gioiTinh = _cboGioiTinh.SelectedItem?.ToString();
            var maLop = _cboLop.SelectedValue?.ToString();

            if (!_svService.Add(_txtMaSv.Text, _txtHoTen.Text, ngaySinh, gioiTinh, _txtDiaChi.Text, maLop, out var err))
            {
                MessageBox.Show(err ?? "Không thể thêm sinh viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadStudents(isSearch: false);
            ClearStudentForm();
        }

        private void UpdateStudent()
        {
            var ngaySinh = _dtNgaySinh.Checked ? (DateTime?)_dtNgaySinh.Value.Date : null;
            var gioiTinh = _cboGioiTinh.SelectedItem?.ToString();
            var maLop = _cboLop.SelectedValue?.ToString();

            if (!_svService.Update(_txtMaSv.Text, _txtHoTen.Text, ngaySinh, gioiTinh, _txtDiaChi.Text, maLop, out var err))
            {
                MessageBox.Show(err ?? "Không thể sửa sinh viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadStudents(isSearch: false);
            ClearStudentForm();
        }

        private void DeleteStudent()
        {
            var ma = (_txtMaSv.Text ?? "").Trim();
            if (ma.Length == 0) { MessageBox.Show("Chọn sinh viên cần xóa."); return; }

            if (MessageBox.Show($"Xóa sinh viên {ma}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            if (!_svService.Delete(ma, out var err))
            {
                MessageBox.Show(err ?? "Không thể xóa sinh viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadStudents(isSearch: false);
            ClearStudentForm();
        }

        private void FillClassFormFromGrid()
        {
            if (_gvLop.CurrentRow == null) return;
            var r = _gvLop.CurrentRow;
            _txtMaLop.Text = r.Cells["MaLop"]?.Value?.ToString();
            _txtTenLop.Text = r.Cells["TenLop"]?.Value?.ToString();
            _txtMaLop.Enabled = false;
        }

        private void ClearClassForm()
        {
            _txtMaLop.Enabled = true;
            _txtMaLop.Text = "";
            _txtTenLop.Text = "";
            _txtMaLop.Focus();
        }

        private void AddClass()
        {
            if (!_lopService.Add(_txtMaLop.Text, _txtTenLop.Text, out var err))
            {
                MessageBox.Show(err ?? "Không thể thêm lớp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            LoadClasses();
            LoadStudents(isSearch: false);
            ClearClassForm();
        }

        private void UpdateClass()
        {
            if (!_lopService.Update(_txtMaLop.Text, _txtTenLop.Text, out var err))
            {
                MessageBox.Show(err ?? "Không thể sửa lớp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            LoadClasses();
            LoadStudents(isSearch: false);
            ClearClassForm();
        }

        private void DeleteClass()
        {
            var ma = (_txtMaLop.Text ?? "").Trim();
            if (ma.Length == 0) { MessageBox.Show("Chọn lớp cần xóa."); return; }

            if (MessageBox.Show($"Xóa lớp {ma}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            if (!_lopService.Delete(ma, out var err))
            {
                MessageBox.Show(err ?? "Không thể xóa lớp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadClasses();
            LoadStudents(isSearch: false);
            ClearClassForm();
        }

        private static Label MkLabel(string text, int left, int top)
        {
            return new Label { Left = left, Top = top + 3, Width = 100, Text = text };
        }

        private static TextBox MkTextBox(int left, int top)
        {
            return new TextBox { Left = 110 + left, Top = top, Width = 210 };
        }

        private static Button MkButton(string text, int left, int top)
        {
            return new Button { Left = left, Top = top, Width = 100, Height = 30, Text = text };
        }
    }
}





