# Bài 1 · UDP Client/Server

Project WinForms minh họa gửi thông điệp UDP từ Client tới Server. Server lắng nghe trên cổng được nhập ở form; Client cần nhập đúng IPv4 và cùng số cổng.

## Chạy thử

1. Mở `Bai1_Lab3.csproj` hoặc chạy project từ solution chung.
2. Mở form **UDP Server**, nhập cổng rồi chọn **Listen**.
3. Mở form **UDP Client**, nhập địa chỉ IP của máy chạy Server, cùng cổng và nội dung cần gửi.
4. Chọn **Send**; kiểm tra thông điệp xuất hiện trong danh sách của Server.

Khi thử trên cùng máy, có thể dùng `127.0.0.1`. Khi thử giữa hai máy, dùng IPv4 LAN của máy Server và cho phép lưu lượng UDP qua tường lửa.
