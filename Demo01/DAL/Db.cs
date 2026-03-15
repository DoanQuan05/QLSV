using System.Configuration;
using System.Data.SqlClient;

namespace Demo01.DAL
{
    internal static class Db
    {
        internal static string ConnectionString
        {
            get
            {
                // Ưu tiên "QLSV", fallback sang connection string mặc định của project (nếu user chưa đổi)
                var cs = ConfigurationManager.ConnectionStrings["QLSV"]?.ConnectionString;
                if (!string.IsNullOrWhiteSpace(cs))
                    return cs;

                return ConfigurationManager.ConnectionStrings["Demo01.Properties.Settings.QLSVConnectionString"]?.ConnectionString;
            }
        }

        internal static SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}










