# Bài 3 · TCP Client/Server

Project minh họa kết nối TCP giữa một Client và Server. Server cần bắt đầu lắng nghe trước khi Client kết nối.

## Chạy thử

1. Chạy `Bai3.csproj` và mở form **TCP Server**.
2. Chọn **Listen**. Server lắng nghe trên mọi card mạng tại cổng 8080.
3. Mở **TCP Client**. Trên cùng máy, dùng `127.0.0.1`; qua LAN, nhập IPv4 của máy Server. Cổng mặc định là 8080.
4. Chọn **Connect**, nhập thông điệp, chọn **Send** và kiểm tra nội dung ở Server.
5. Chọn **Disconnect** khi kết thúc.

Chỉ chạy một Server dùng cổng 8080 tại một thời điểm. Khi demo qua LAN, cho phép TCP/8080 qua tường lửa trên máy Server.
