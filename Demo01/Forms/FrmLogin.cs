using System;
using System.Windows.Forms;
using Demo01.BLL;

namespace Demo01.Forms
{
    public class FrmLogin : Form
    {
        private readonly AuthService _auth = new AuthService();

        private TextBox _txtUser;
        private TextBox _txtPass;
        private Button _btnLogin;
        private Label _lblStatus;

        public FrmLogin()
        {
            Text = "Đăng nhập";
            Width = 420;
            Height = 260;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            BuildUi();
        }

        private void BuildUi()
        {
            var lblUser = new Label { Left = 30, Top = 35, Width = 90, Text = "Username" };
            var lblPass = new Label { Left = 30, Top = 75, Width = 90, Text = "Password" };

            _txtUser = new TextBox { Left = 130, Top = 32, Width = 230 };
            _txtPass = new TextBox { Left = 130, Top = 72, Width = 230, PasswordChar = '*' };

            _btnLogin = new Button { Left = 130, Top = 115, Width = 230, Height = 32, Text = "Đăng nhập" };
            _btnLogin.Click += (s, e) => DoLogin();

            _lblStatus = new Label { Left = 130, Top = 155, Width = 230, Height = 40, ForeColor = System.Drawing.Color.DarkRed };

            AcceptButton = _btnLogin;

            Controls.Add(lblUser);
            Controls.Add(lblPass);
            Controls.Add(_txtUser);
            Controls.Add(_txtPass);
            Controls.Add(_btnLogin);
            Controls.Add(_lblStatus);
        }

        private void DoLogin()
        {
            _lblStatus.Text = "";

            try
            {
                var ok = _auth.Login(_txtUser.Text, _txtPass.Text);
                if (!ok)
                {
                    _lblStatus.Text = "Sai username hoặc password.";
                    _txtPass.Focus();
                    _txtPass.SelectAll();
                    return;
                }

                Hide();
                using (var main = new FrmMain(_txtUser.Text.Trim()))
                {
                    main.ShowDialog();
                }
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể đăng nhập.\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}





