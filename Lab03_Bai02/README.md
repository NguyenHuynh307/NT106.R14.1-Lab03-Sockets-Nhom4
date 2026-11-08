# Bài 2 · Telnet Listener

Project dùng `Socket` để lắng nghe kết nối TCP trên cổng 8080 và hiển thị từng dòng nhận được trên form.

## Chạy thử

1. Chạy `Bai2_Lab3.csproj`.
2. Chọn **Listen** để mở cổng 8080.
3. Từ máy khác cùng mạng, kết nối tới IPv4 LAN của máy chạy Server, cổng 8080. Ví dụ: `telnet 192.168.1.20 8080`.
4. Gõ một dòng rồi nhấn Enter; nội dung sẽ hiện trong cửa sổ Server.

Nếu Windows chưa có Telnet Client, bật thành phần **Telnet Client** trong Windows Features. Cho phép TCP/8080 qua tường lửa máy Server khi kiểm tra qua LAN.
