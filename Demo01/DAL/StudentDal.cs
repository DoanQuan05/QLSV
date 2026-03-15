using System;
using System.Data;
using System.Data.SqlClient;

namespace Demo01.DAL
{
    public class StudentDal
    {
        public DataTable GetAllWithClass()
        {
            const string sql = @"
SELECT sv.MaSV, sv.HoTen, sv.NgaySinh, sv.GioiTinh, sv.DiaChi, sv.MaLop, l.TenLop
FROM dbo.SinhVien sv
INNER JOIN dbo.Lop l ON l.MaLop = sv.MaLop
ORDER BY sv.MaSV;";

            using (var conn = Db.CreateConnection())
            using (var da = new SqlDataAdapter(sql, conn))
            {
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable Search(string keyword)
        {
            const string sql = @"
SELECT sv.MaSV, sv.HoTen, sv.NgaySinh, sv.GioiTinh, sv.DiaChi, sv.MaLop, l.TenLop
FROM dbo.SinhVien sv
INNER JOIN dbo.Lop l ON l.MaLop = sv.MaLop
WHERE sv.MaSV LIKE @kw OR sv.HoTen LIKE @kw OR l.TenLop LIKE @kw
ORDER BY sv.MaSV;";

            using (var conn = Db.CreateConnection())
            using (var cmd = new SqlCommand(sql, conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@kw", "%" + (keyword ?? string.Empty) + "%");
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public bool Insert(string maSV, string hoTen, DateTime? ngaySinh, string gioiTinh, string diaChi, string maLop)
        {
            const string sql = @"
INSERT INTO dbo.SinhVien(MaSV, HoTen, NgaySinh, GioiTinh, DiaChi, MaLop)
VALUES (@ma, @ten, @ns, @gt, @dc, @lop);";

            using (var conn = Db.CreateConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@ma", maSV);
                cmd.Parameters.AddWithValue("@ten", hoTen);
                cmd.Parameters.AddWithValue("@ns", (object)ngaySinh ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@gt", (object)gioiTinh ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@dc", (object)diaChi ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@lop", maLop);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Update(string maSV, string hoTen, DateTime? ngaySinh, string gioiTinh, string diaChi, string maLop)
        {
            const string sql = @"
UPDATE dbo.SinhVien
SET HoTen=@ten, NgaySinh=@ns, GioiTinh=@gt, DiaChi=@dc, MaLop=@lop
WHERE MaSV=@ma;";

            using (var conn = Db.CreateConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@ma", maSV);
                cmd.Parameters.AddWithValue("@ten", hoTen);
                cmd.Parameters.AddWithValue("@ns", (object)ngaySinh ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@gt", (object)gioiTinh ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@dc", (object)diaChi ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@lop", maLop);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(string maSV)
        {
            const string sql = @"DELETE FROM dbo.SinhVien WHERE MaSV=@ma;";
            using (var conn = Db.CreateConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@ma", maSV);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}










