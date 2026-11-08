# Bài 5 · Hôm nay ăn gì?

Bài thực hành client–server dùng TCP. Server quản lý dữ liệu món ăn bằng SQLite; các Client kết nối đến cùng máy chủ để xem và thao tác trên dữ liệu chung.

## Chạy thử

1. Mở `Lab03_Bai05.csproj` hoặc chọn **Bài 5** từ menu Lab 3.
2. Trên máy chủ, chọn **MỞ SERVER**. Cổng mặc định là 8085.
3. Trên Client, nhập IPv4 LAN của máy Server và chọn **MỞ CLIENT**.
4. Kiểm tra danh sách món; thử chức năng quay món trong chế độ cá nhân hoặc cộng đồng.
5. Khi có thay đổi dữ liệu, kết nối Client khác để kiểm tra dữ liệu dùng chung.

Cơ sở dữ liệu và ảnh được lưu ở phía Server. Khi thử qua LAN, cho phép TCP/8085 qua tường lửa máy chủ.
