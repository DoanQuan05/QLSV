using System;
using System.Data;
using Demo01.DAL;

namespace Demo01.BLL
{
    public class StudentService
    {
        private readonly StudentDal _dal = new StudentDal();

        public DataTable GetAll()
        {
            return _dal.GetAllWithClass();
        }

        public DataTable Search(string keyword)
        {
            return _dal.Search(keyword);
        }

        public bool Add(string maSV, string hoTen, DateTime? ngaySinh, string gioiTinh, string diaChi, string maLop, out string error)
        {
            error = null;
            maSV = (maSV ?? string.Empty).Trim();
            hoTen = (hoTen ?? string.Empty).Trim();
            maLop = (maLop ?? string.Empty).Trim();

            if (maSV.Length == 0) { error = "Mã SV không được rỗng."; return false; }
            if (hoTen.Length == 0) { error = "Họ tên không được rỗng."; return false; }
            if (maLop.Length == 0) { error = "Vui lòng chọn lớp."; return false; }

            try
            {
                return _dal.Insert(maSV, hoTen, ngaySinh, gioiTinh, diaChi, maLop);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public bool Update(string maSV, string hoTen, DateTime? ngaySinh, string gioiTinh, string diaChi, string maLop, out string error)
        {
            error = null;
            maSV = (maSV ?? string.Empty).Trim();
            hoTen = (hoTen ?? string.Empty).Trim();
            maLop = (maLop ?? string.Empty).Trim();

            if (maSV.Length == 0) { error = "Mã SV không được rỗng."; return false; }
            if (hoTen.Length == 0) { error = "Họ tên không được rỗng."; return false; }
            if (maLop.Length == 0) { error = "Vui lòng chọn lớp."; return false; }

            try
            {
                return _dal.Update(maSV, hoTen, ngaySinh, gioiTinh, diaChi, maLop);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public bool Delete(string maSV, out string error)
        {
            error = null;
            maSV = (maSV ?? string.Empty).Trim();
            if (maSV.Length == 0) { error = "Mã SV không được rỗng."; return false; }

            try
            {
                return _dal.Delete(maSV);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }
    }
}










