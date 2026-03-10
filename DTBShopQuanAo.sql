create database QuanLyCuaHangQuanAo;
go
USE QuanLyCuaHangQuanAo;
create table roles
(
	role_id int primary key identity(1,1),
	role_name nvarchar(100) not null
);
-- Tạo bảng Employees (Nhân sự)
create table employees 
(
    employee_id int primary key identity(1,1),
    E_name nvarchar(100),
    us_name varchar(50) not null unique,
    us_password varchar(255),
    RoleID int foreign key references Roles(role_id),
    sex varchar(30),
    PhoneNumber varchar(15) unique,
);
create table products
(
    product_id int primary key identity(1,1),
    product_pic VARBINARY(MAX), -- Cột ảnh chuẩn để lưu từ C#,
    product_name nvarchar(100),
    product_categoryName nvarchar(100), -- loại ha
    product_size varchar(10),
    product_sellingPrice int not null,
    product_importPrice int not null,
    product_stockQuantity int not null--so luong ton
)
create table customers
(
    customer_id int primary key identity(1,1),
    customer_name nvarchar(100),
    totalSpending int ,
    sex varchar(30),
    PhoneNumber varchar(15) unique,
)
create table importOrder
(
   import_id int primary key identity(1,1),
   date_Import datetime default getdate(), 
   SupplierName NVARCHAR(200),
   PhoneNumber varchar(15),
   employee_id int foreign key references employees(employee_id)
)
create table importDetails
(
    importdetail_id int primary key identity (1,1),
    import_id int not null foreign key references importOrder(import_id),                          -- Nối với bảng ImportOrders
    product_id int not null foreign key references products(product_id),                         -- Nối với sản phẩm cụ thể (Size, Màu)
    Quantity INT NOT NULL,                 -- Số lượng nhập về
    importPrice int not null,
)
create table orders
(
    order_id int primary key identity(1,1),
    order_date datetime default getdate(), -- Ngày giờ bán
    customer_id int foreign key references customers(customer_id), -- Khách nào mua
    employee_id int foreign key references employees(employee_id), -- Nhân viên nào bán
);
create table orderDetails
(
    orderdetail_id int primary key identity(1,1),
    order_id int not null foreign key references orders(order_id),
    product_id int not null foreign key references products(product_id),
    Quantity int not null, -- Số lượng khách mua
);



--- dữ liệu test
INSERT INTO roles (role_name) VALUES (N'Manager'), (N'Staff');

-- 2. Thêm 2 tài khoản Manager (Sẽ nhận ID 1 và 2)
INSERT INTO employees (E_name, us_name, us_password, RoleID, sex, PhoneNumber)
VALUES 
(N'Nguyễn Quản Lý 1', 'manager1', '123', 1, 'Male', '0911111111'),
(N'Trần Quản Lý 2', 'manager2', '123', 1, 'Female', '0922222222');

-- 3. Thêm 2 tài khoản Staff
INSERT INTO employees (E_name, us_name, us_password, RoleID, sex, PhoneNumber)
VALUES 
(N'Lê Nhân Viên 1', 'staff1', '123', 2, 'Male', '0933333333'),
(N'Phạm Nhân Viên 2', 'staff2', '123', 2, 'Female', '0944444444');
USE QuanLyCuaHangQuanAo;
GO

-- Chuyển kiểu dữ liệu sang VARBINARY(MAX) để chứa dữ liệu ảnh
drop table orderDetails;
drop table importDetails;

-- 2. Xóa dữ liệu trong bảng products
drop table products;
INSERT INTO products (product_name, product_categoryName, product_size, product_sellingPrice, product_importPrice, product_stockQuantity, product_pic)
VALUES 
(N'Áo Thun Cotton Basic White', N'Áo Thun', 'L', 150000, 85000, 100, NULL),
(N'Áo Polo Pique Navy', N'Áo Polo', 'XL', 280000, 150000, 60, NULL),
(N'Quần Jean Slim-fit Dark Blue', N'Quần Jean', '32', 450000, 250000, 45, NULL),
(N'Quần Short Kaki Beige', N'Quần Short', 'M', 220000, 120000, 80, NULL),
(N'Áo Hoodie Fleece Grey', N'Áo Khoác', 'L', 380000, 210000, 30, NULL);