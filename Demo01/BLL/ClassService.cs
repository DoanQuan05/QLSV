using System;
using System.Data;
using Demo01.DAL;
using Demo01.DTO;

namespace Demo01.BLL
{
    public class ClassService
    {
        private readonly ClassDal _dal = new ClassDal();

        public DataTable GetAllTable()
        {
            return _dal.GetAllTable();
        }

        public bool Add(string maLop, string tenLop, out string error)
        {
            error = null;
            maLop = (maLop ?? string.Empty).Trim();
            tenLop = (tenLop ?? string.Empty).Trim();

            if (maLop.Length == 0) { error = "Mã lớp không được rỗng."; return false; }
            if (tenLop.Length == 0) { error = "Tên lớp không được rỗng."; return false; }

            try
            {
                return _dal.Insert(new ClassDto { MaLop = maLop, TenLop = tenLop });
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public bool Update(string maLop, string tenLop, out string error)
        {
            error = null;
            maLop = (maLop ?? string.Empty).Trim();
            tenLop = (tenLop ?? string.Empty).Trim();

            if (maLop.Length == 0) { error = "Mã lớp không được rỗng."; return false; }
            if (tenLop.Length == 0) { error = "Tên lớp không được rỗng."; return false; }

            try
            {
                return _dal.Update(new ClassDto { MaLop = maLop, TenLop = tenLop });
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public bool Delete(string maLop, out string error)
        {
            error = null;
            maLop = (maLop ?? string.Empty).Trim();
            if (maLop.Length == 0) { error = "Mã lớp không được rỗng."; return false; }

            try
            {
                return _dal.Delete(maLop);
            }
            catch (Exception ex)
            {
                // Thường gặp: đang có SV thuộc lớp -> FK
                error = ex.Message;
                return false;
            }
        }
    }
}





