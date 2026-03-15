CREATE DATABASE QLSV
GO

USE QLSV
GO

-- Bảng Khoa
CREATE TABLE Khoa (
    MaKhoa VARCHAR(10) PRIMARY KEY,
    TenKhoa NVARCHAR(100)
)

-- Bảng Lớp
CREATE TABLE Lop (
    MaLop VARCHAR(10) PRIMARY KEY,
    TenLop NVARCHAR(100),
    MaKhoa VARCHAR(10),
    FOREIGN KEY (MaKhoa) REFERENCES Khoa(MaKhoa)
)

-- Bảng Sinh viên
CREATE TABLE SinhVien (
    MaSV VARCHAR(10) PRIMARY KEY,
    HoTen NVARCHAR(100),
    NgaySinh DATE,
    GioiTinh NVARCHAR(10),
    DiaChi NVARCHAR(200),
    MaLop VARCHAR(10),
    FOREIGN KEY (MaLop) REFERENCES Lop(MaLop)
)

-- Bảng Môn học
CREATE TABLE MonHoc (
    MaMH VARCHAR(10) PRIMARY KEY,
    TenMH NVARCHAR(100),
    SoTinChi INT
)

-- Bảng Kết quả
CREATE TABLE KetQua (
    MaSV VARCHAR(10),
    MaMH VARCHAR(10),
    Diem FLOAT,
    PRIMARY KEY (MaSV, MaMH),
    FOREIGN KEY (MaSV) REFERENCES SinhVien(MaSV),
    FOREIGN KEY (MaMH) REFERENCES MonHoc(MaMH)
)

-- Bảng Tài khoản
CREATE TABLE TaiKhoan (
    TenDangNhap VARCHAR(50) PRIMARY KEY,
    MatKhau VARCHAR(50)
)

-- Thêm dữ liệu bảng Khoa
INSERT INTO Khoa (MaKhoa, TenKhoa) VALUES
('CNTT', N'Công nghệ thông tin'),
('QTKD', N'Quản trị kinh doanh');

-- Thêm dữ liệu bảng Lop
INSERT INTO Lop (MaLop, TenLop, MaKhoa) VALUES
('CTK42', N'CNTT K42', 'CNTT'),
('QTKD1', N'QTKD K1', 'QTKD');

-- Thêm dữ liệu bảng SinhVien
INSERT INTO SinhVien (MaSV, HoTen, NgaySinh, GioiTinh, DiaChi, MaLop) VALUES
('SV01', N'Nguyen Van A', '2003-05-10', N'Nam', N'Ha Noi', 'CTK42'),
('SV02', N'Tran Thi B', '2003-08-12', N'Nu', N'Hai Phong', 'CTK42'),
('SV03', N'Le Van C', '2003-03-15', N'Nam', N'Nam Dinh', 'QTKD1');

-- Thêm dữ liệu bảng MonHoc
INSERT INTO MonHoc (MaMH, TenMH, SoTinChi) VALUES
('MH01', N'Lap trinh Java', 3),
('MH02', N'Co so du lieu', 3),
('MH03', N'Mang may tinh', 3);

-- Thêm dữ liệu bảng KetQua
INSERT INTO KetQua (MaSV, MaMH, Diem) VALUES
('SV01', 'MH01', 8.5),
('SV01', 'MH02', 7.0),
('SV02', 'MH01', 9.0),
('SV03', 'MH03', 6.5);

INSERT INTO TaiKhoan (TenDangNhap, MatKhau) VALUES
('admin','123');