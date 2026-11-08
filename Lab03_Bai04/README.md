# Bài 4 · Quản lý vé xem phim

Bài thực hành nhiều Client kết nối một Server TCP. Server nạp danh sách phim từ `input5.txt`, quản lý trạng thái ghế và trả kết quả đặt vé.

## Chạy thử

1. Mở `Lab03_Bai04.csproj` hoặc chọn **Bài 4** từ menu Lab 3.
2. Trên máy chủ, chọn **MỞ SERVER**. Server mặc định dùng cổng 8084 và hiển thị IPv4 LAN.
3. Trên Client, nhập IP máy chủ và chọn **MỞ CLIENT**.
4. Chọn phim, phòng, suất chiếu, ghế và nhập thông tin khách; chọn **ĐẶT VÉ**.
5. Kết nối Client thứ hai và kiểm tra trạng thái ghế sau khi một Client đặt vé.

`input5.txt` cần nằm trong thư mục chạy của Server. Khi thử qua mạng, cho phép TCP/8084 qua tường lửa.
