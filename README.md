# NT106.R14.1 · Lab 3: Lập trình Socket trong C#

Repo lưu mã nguồn thực hành Lab 3 của Nhóm 5. Sáu bài được tách thành project riêng; solution và menu chung giúp mở từng bài để chạy hoặc trình bày.

## Nội dung

- **Bài 1 · UDP Client/Server** — gửi thông điệp UTF-8 từ client và hiển thị dữ liệu nhận được ở server.
- **Bài 2 · Telnet Listener** — lắng nghe kết nối TCP trên cổng 8080 và đưa dữ liệu nhận được lên giao diện.
- **Bài 3 · TCP Client/Server** — kết nối một client với server để gửi và nhận thông điệp.
- **Bài 4 · Quản lý vé xem phim** — server đọc danh sách phim từ `input5.txt`; các client cùng xem trạng thái ghế và gửi yêu cầu đặt vé.
- **Bài 5 · Hôm nay ăn gì?** — client làm việc với dữ liệu món ăn được lưu tập trung ở server bằng SQLite.
- **Bài 6 · Chat Room** — nhiều client chat chung, nhắn riêng, gửi ảnh hoặc tệp và xem danh sách người đang kết nối.

## Mở và chạy

Cần Windows và .NET 10 SDK (workload **.NET Desktop Development** trong Visual Studio).

1. Mở `NT106.R14.1-Lab03-Sockets-Nhom5.sln`.
2. Chọn `Lab03_Menu` làm Startup Project.
3. Chọn **Build > Build Solution**, sau đó nhấn **Ctrl+F5**.
4. Trong menu, chọn bài cần chạy. Đóng cửa sổ bài hiện tại trước khi mở bài khác nếu hai bài cùng dùng một cổng.

Có thể build hoặc chạy riêng từng project trong `Lab03_Bai01`–`Lab03_Bai06`.

## Chạy Server và Client qua mạng

Với bài có giao tiếp client–server, khởi động Server trước. Trên máy Client, nhập địa chỉ IPv4 LAN hiển thị ở máy Server; `127.0.0.1` chỉ dùng khi Client và Server chạy trên cùng máy. Hai máy cần kết nối cùng mạng và tường lửa máy Server phải cho phép lưu lượng đến cổng của bài.

| Bài | Giao thức | Cổng mặc định |
|---|---|---:|
| 1 | UDP | Nhập trên form (thường dùng 8080) |
| 2 | TCP | 8080 |
| 3 | TCP | 8080 |
| 4 | TCP | 8084 |
| 5 | TCP | 8085 |
| 6 | TCP | 8080 |

## Phân công và nhánh làm việc

| Thành viên | Phụ trách | Nhánh |
|---|---|---|
| Nguyễn Quốc Nhật Minh | Bài 1, 2 | `feat/minh-bai01`, `feat/minh-bai02` |
| Hoàng Anh | Bài 3, 6 | `feature/hanh-bai03-bai06` |
| Huỳnh Thanh Nguyên | Bài 4, 5 | `feat/nguyen-bai04-bai05` |

Mỗi người phát triển phần được giao trên nhánh riêng. Khi một phần sẵn sàng, mở Pull Request vào `main` để nhóm xem lại và tích hợp. Thư mục các project trong solution chung là bản dùng để chạy toàn bộ bài; nội dung báo cáo được nộp riêng theo yêu cầu lớp.
