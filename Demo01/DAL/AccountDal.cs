using System.Data.SqlClient;

namespace Demo01.DAL
{
    public class AccountDal
    {
        public bool ValidateLogin(string username, string password)
        {
            const string sql = @"SELECT COUNT(1)
FROM dbo.TaiKhoan
WHERE TenDangNhap = @u AND MatKhau = @p;";

            using (var conn = Db.CreateConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@p", password);
                conn.Open();
                var result = (int)cmd.ExecuteScalar();
                return result > 0;
            }
        }
    }
}










