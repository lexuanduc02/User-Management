# User Management
 ## Chức năng :
 - Login.
 - Change Password.
 - User Registration.
 - User Management:
	 - List User
	 - Edit User
	 - Delete User
## Technical Stack:
- BE: .NET Core 8.0.
- UI: Razor Pages .
- Database : SQL Server.
## Cách chạy dự án:
- Tải và cài đặt **.NET 8.0**.
- Tải và cài đặt **SQL Server 2022**.
- Chạy câu lệnh ***dotnet ef database update*** trong thử mục **App.Infrastructure**.
 - Chạy dự án trong Visual Studio với thiết lập ***Multiple startup projects*** cho **`App.Api`** và **`App.WebApp`**
## Tương tác với WebServer bằng Postman:
- Import file Collection và Enviroment trong folder API Document vào Postman.