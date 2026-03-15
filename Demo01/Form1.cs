using System;
using System.Windows.Forms;
using Demo01.BLL;
using Demo01.Forms;

namespace Demo01
{
    public partial class Form1 : Form
    {
        private readonly AuthService _auth = new AuthService();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void txtUser_TextChanged(object sender, EventArgs e) { }
        private void txtPass_TextChanged(object sender, EventArgs e) { }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_auth.Login(txtUser.Text, txtPass.Text))
                {
                    MessageBox.Show("Sai username hoặc password.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPass.Focus();
                    txtPass.SelectAll();
                    return;
                }

                this.Hide();
                using (var main = new FrmMain(txtUser.Text.Trim()))
                {
                    main.ShowDialog();
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể đăng nhập.\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}