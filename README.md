# QLSV - Quản lý sinh viên (WinForms + SQL Server)

## 1) Tạo database

- Mở SQL Server Management Studio (SSMS)
- New Query
- Chạy file: `Demo01/database/QLSV.sql`

Tài khoản mẫu:
- Username: `admin`
- Password: `123`

## 2) Cấu hình connection string

Mở `Demo01/App.config` và sửa connection string cho đúng SQL Server instance của bạn.

Code sẽ ưu tiên đọc connection string tên **`QLSV`**, nếu chưa có thì sẽ fallback sang:
`Demo01.Properties.Settings.QLSVConnectionString`.

## 3) Chạy project

Mở `Demo01.sln` bằng Visual Studio (khuyến nghị VS 2019/2022) và Run.

## 4) Luồng xử lý (UI → BLL → DAL → DB)

- UI:
  - `Demo01/Forms/FrmLogin.cs`: đăng nhập
  - `Demo01/Forms/FrmMain.cs`: quản lý sinh viên + lớp
- BLL:
  - `Demo01/BLL/*Service.cs`: validate dữ liệu + gọi DAL
- DAL:
  - `Demo01/DAL/*.cs`: truy vấn SQL bằng ADO.NET (parameterized queries)
- DTO:
  - `Demo01/DTO/*.cs`










