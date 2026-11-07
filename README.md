# NT106.R14.1 · Lab 3 · Nhóm 4

Repo thực hành Socket bằng C#. Mỗi bài nằm trong project riêng và được tập hợp bằng solution chung.

## Phân công

| Thành viên | Câu | Nhánh dự kiến |
|---|---:|---|---|
| Huỳnh Thanh Nguyên | 4, 5 | `feat/nguyen-bai04-bai05` |
| Nguyễn Quốc Nhật Minh | 1, 2 | `feat/minh-bai01-bai02` |
| Hoàng Anh | 3, 6 | `feat/hoang-anh-bai03-bai06` |

## Cấu trúc

- `Lab03_Bai04/`: giao diện đặt vé kế thừa Lab 2; TCP server quản lý danh sách `input5.txt` và trạng thái ghế dùng chung.
- `Lab03_Bai05/`: giao diện “Hôm nay ăn gì?” kế thừa Lab 2; SQLite và ảnh món được quản lý qua server TCP.
- `NT106.R14.1-Lab03-Sockets-Nhom4.sln`: solution chung của nhóm.
- `Report/`: báo cáo Lab 3.

## Build và chạy

Cần .NET 10 SDK và Windows (Windows Forms). Build toàn bộ solution:

```powershell
dotnet build NT106.R14.1-Lab03-Sockets-Nhom4.sln
```

Chạy từng bài:

```powershell
dotnet run --project Lab03_Bai04/Lab03_Bai04.csproj
dotnet run --project Lab03_Bai05/Lab03_Bai05.csproj
```

Trên máy chủ, mở chương trình và chọn **MỞ SERVER**. Trên máy client, nhập IPv4 LAN của máy chủ rồi chọn **MỞ CLIENT**. Mặc định Bài 4 nghe TCP cổng `8084`, Bài 5 cổng `8085`; cho phép các cổng TCP này qua Windows Firewall trên máy chủ. Máy khách và máy chủ cần cùng mạng LAN. Không nhập `127.0.0.1` trên máy khách LAN — địa chỉ đó chỉ dùng khi client và server đang trên cùng máy.

### Bài 4

Server nạp `input5.txt` trong thư mục chạy của server. Danh sách phim được quản lý tập trung. Khi nhiều client đặt vé, server kiểm tra toàn bộ ghế trong một vùng khóa; nếu có ghế vừa được client khác bán thì cả yêu cầu bị từ chối. Trạng thái và thống kê ghế được client làm mới tự động.

### Bài 5

Server lưu SQLite tại `Data/HomNayAnGi.db`; ảnh món client tải lên được lưu trong `Data/Images`. Dữ liệu món ăn, người đóng góp, yêu thích, lịch sử, quán ăn và các thao tác quay món đi qua server. Client kiểm tra phiên bản dữ liệu định kỳ để làm mới danh sách chung.

## Quy trình đóng góp

1. Tạo nhánh từ `main` theo bảng phân công; chỉ sửa project của câu được giao.
2. Kéo thay đổi mới nhất của `main` trước khi mở Pull Request.
3. PR vào `main`, ghi câu phụ trách, cách chạy và kết quả build. Không đẩy trực tiếp vào `main`.
4. Trưởng nhóm review và merge; xóa nhánh sau khi merge.

Thành viên chưa cần sửa project của người khác. Khi PR được merge, thêm project vào solution chung và cập nhật báo cáo tại mục câu tương ứng.

## Kiểm thử

Build bằng .NET 10 được thực hiện cục bộ. Kiểm thử trên các máy tính khác nhau trong cùng LAN cần hoàn tất tại lớp; báo cáo chỉ ghi bằng chứng đã quan sát được, không suy diễn kết quả LAN từ thử nghiệm localhost.
