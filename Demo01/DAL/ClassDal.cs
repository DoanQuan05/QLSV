using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Demo01.DTO;

namespace Demo01.DAL
{
    public class ClassDal
    {
        public List<ClassDto> GetAll()
        {
            const string sql = @"SELECT MaLop, TenLop FROM dbo.Lop ORDER BY MaLop;";
            var list = new List<ClassDto>();

            using (var conn = Db.CreateConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new ClassDto
                        {
                            MaLop = rd["MaLop"].ToString(),
                            TenLop = rd["TenLop"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public DataTable GetAllTable()
        {
            const string sql = @"SELECT MaLop, TenLop FROM dbo.Lop ORDER BY MaLop;";
            using (var conn = Db.CreateConnection())
            using (var da = new SqlDataAdapter(sql, conn))
            {
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public bool Insert(ClassDto lop)
        {
            const string sql = @"INSERT INTO dbo.Lop(MaLop, TenLop) VALUES (@ma, @ten);";
            using (var conn = Db.CreateConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@ma", lop.MaLop);
                cmd.Parameters.AddWithValue("@ten", lop.TenLop);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Update(ClassDto lop)
        {
            const string sql = @"UPDATE dbo.Lop SET TenLop=@ten WHERE MaLop=@ma;";
            using (var conn = Db.CreateConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@ma", lop.MaLop);
                cmd.Parameters.AddWithValue("@ten", lop.TenLop);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(string maLop)
        {
            const string sql = @"DELETE FROM dbo.Lop WHERE MaLop=@ma;";
            using (var conn = Db.CreateConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@ma", maLop);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}





