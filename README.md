# NT106.R14.1 · Thực hành Lab 3 · Nhóm 4

Bộ bài thực hành lập trình Socket bằng C#/.NET 10. Mỗi câu được phát triển độc lập theo nhánh phụ trách; solution ở thư mục gốc dùng để tích hợp và chạy các project.

## Phân công và nhánh

| Thành viên | Bài | Nhánh làm việc |
|---|---|---|
| Nguyễn Quốc Nhật Minh | 1, 2 | `feat/minh-bai01`, `feat/minh-bai02` |
| Hoàng Anh | 3, 6 | `feature/hanh-bai03-bai06` |
| Huỳnh Thanh Nguyên | 4, 5 | `feat/nguyen-bai04-bai05` |

Mỗi thành viên tiếp tục cập nhật bài của mình trên nhánh được phân công. Bản tích hợp nằm ở nhánh `integration/lab03-six-bai-menu`; các thay đổi được rà soát trước khi nhập vào `main`.

## Cấu trúc

- `Lab03_Bai01/` đến `Lab03_Bai06/`: sáu project tương ứng sáu bài thực hành.
- `Lab03_Menu/`: menu mở form điều hướng của từng bài.
- `NT106.R14.1-Lab03-Sockets-Nhom4.sln`: solution tích hợp.

## Yêu cầu chạy

- Windows và .NET 10 SDK.
- Mở solution bằng Visual Studio, chọn `Lab03_Menu` làm Startup Project, sau đó Build Solution và chạy bằng Ctrl+F5.
- Có thể mở trực tiếp project từng bài nếu cần kiểm tra riêng.

## Bài client–server trên cùng mạng

Chạy server trước. Trên máy client, nhập IPv4 LAN mà menu/server hiển thị trên máy chủ; không dùng `127.0.0.1` khi client ở máy khác. Hai máy cần cùng mạng và Windows Firewall trên máy chủ phải cho phép cổng TCP/UDP mà bài sử dụng.

- Bài 1: UDP, cổng do người dùng nhập (bản demo dùng 8080).
- Bài 2: TCP/Telnet, cổng 8080.
- Bài 3: TCP, cổng 8080.
- Bài 4: TCP, cổng mặc định 8084; server đọc `input5.txt`.
- Bài 5: TCP, cổng mặc định 8085; SQLite và ảnh món lưu tập trung phía server.
- Bài 6: TCP chat room, cổng 8080.

## Làm việc theo nhánh

1. Tạo hoặc checkout đúng nhánh bài được phân công.
2. Chỉ cập nhật project thuộc phạm vi bài đó; giữ nguyên project của thành viên khác.
3. Build và chạy thử bài, ghi rõ kết quả thực tế trong Pull Request.
4. Mở Pull Request vào `main` để trưởng nhóm review và tích hợp.

Báo cáo nộp riêng theo yêu cầu lớp; repo này tập trung vào mã nguồn và hướng dẫn chạy.
