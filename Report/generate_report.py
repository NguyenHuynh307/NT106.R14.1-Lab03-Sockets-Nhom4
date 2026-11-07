from reportlab.lib import colors
from reportlab.lib.enums import TA_CENTER, TA_LEFT
from reportlab.lib.pagesizes import A4
from reportlab.lib.styles import getSampleStyleSheet, ParagraphStyle
from reportlab.lib.units import mm
from reportlab.pdfbase import pdfmetrics
from reportlab.pdfbase.ttfonts import TTFont
from reportlab.platypus import (
    SimpleDocTemplate, Paragraph, Spacer, Table, TableStyle, PageBreak,
    KeepTogether, Flowable
)
from reportlab.graphics.shapes import Drawing, Rect, String, Line
from reportlab.lib.colors import HexColor
from pathlib import Path

OUT = Path(__file__).with_name("Bao-cao-Lab3-Socket-Nhom4.pdf")
FONT_DIR = Path("C:/Windows/Fonts")
pdfmetrics.registerFont(TTFont("ArialVN", str(FONT_DIR / "arial.ttf")))
pdfmetrics.registerFont(TTFont("ArialVN-Bold", str(FONT_DIR / "arialbd.ttf")))
pdfmetrics.registerFontFamily("ArialVN", normal="ArialVN", bold="ArialVN-Bold", italic="ArialVN", boldItalic="ArialVN-Bold")

NAVY = HexColor("#122B49")
BLUE = HexColor("#245B8F")
PALE = HexColor("#EEF4F8")
GOLD = HexColor("#D29A36")
INK = HexColor("#243447")
MUTED = HexColor("#657789")
GREEN = HexColor("#257A56")
AMBER = HexColor("#A66512")
RED = HexColor("#9C3D3D")

styles = getSampleStyleSheet()
styles.add(ParagraphStyle(name="VNBody", parent=styles["BodyText"], fontName="ArialVN", fontSize=9.4, leading=14.2, textColor=INK, spaceAfter=6))
styles.add(ParagraphStyle(name="VNBodySmall", parent=styles["BodyText"], fontName="ArialVN", fontSize=8.2, leading=11.5, textColor=INK, spaceAfter=4))
styles.add(ParagraphStyle(name="VNTitle", parent=styles["Title"], fontName="ArialVN-Bold", fontSize=25, leading=31, textColor=NAVY, alignment=TA_CENTER, spaceAfter=8))
styles.add(ParagraphStyle(name="VNSubtitle", parent=styles["BodyText"], fontName="ArialVN", fontSize=12, leading=17, textColor=BLUE, alignment=TA_CENTER, spaceAfter=7))
styles.add(ParagraphStyle(name="VNSection", parent=styles["Heading1"], fontName="ArialVN-Bold", fontSize=16, leading=20, textColor=NAVY, spaceBefore=4, spaceAfter=8, keepWithNext=True))
styles.add(ParagraphStyle(name="VNSubsection", parent=styles["Heading2"], fontName="ArialVN-Bold", fontSize=11.5, leading=15, textColor=BLUE, spaceBefore=8, spaceAfter=5, keepWithNext=True))
styles.add(ParagraphStyle(name="VNCallout", parent=styles["BodyText"], fontName="ArialVN-Bold", fontSize=9, leading=13.2, textColor=NAVY, backColor=PALE, borderColor=BLUE, borderWidth=0.5, borderPadding=8, spaceBefore=4, spaceAfter=8))
styles.add(ParagraphStyle(name="VNTiny", parent=styles["BodyText"], fontName="ArialVN", fontSize=7.4, leading=10, textColor=MUTED))
styles.add(ParagraphStyle(name="VNCoverLabel", parent=styles["BodyText"], fontName="ArialVN-Bold", fontSize=9, leading=14, textColor=GOLD, alignment=TA_CENTER, spaceAfter=15))
styles.add(ParagraphStyle(name="VNTableHead", parent=styles["BodyText"], fontName="ArialVN-Bold", fontSize=8.2, leading=11, textColor=colors.white))

def P(text, style="VNBody"):
    return Paragraph(text, styles[style])

def section(title):
    return P(title, "VNSection")

def table(rows, widths, header=True, font_size=8.2):
    wrapped = []
    for ri, row in enumerate(rows):
        cell_style = "VNTableHead" if header and ri == 0 else "VNBodySmall"
        wrapped.append([cell if isinstance(cell, Flowable) else P(str(cell), cell_style) for cell in row])
    t = Table(wrapped, colWidths=widths, repeatRows=1 if header else 0, hAlign="LEFT")
    commands = [
        ("VALIGN", (0, 0), (-1, -1), "TOP"),
        ("GRID", (0, 0), (-1, -1), 0.35, HexColor("#CFD9E2")),
        ("LEFTPADDING", (0, 0), (-1, -1), 7), ("RIGHTPADDING", (0, 0), (-1, -1), 7),
        ("TOPPADDING", (0, 0), (-1, -1), 6), ("BOTTOMPADDING", (0, 0), (-1, -1), 6),
    ]
    if header:
        commands += [("BACKGROUND", (0, 0), (-1, 0), NAVY), ("TEXTCOLOR", (0, 0), (-1, 0), colors.white)]
        # Paragraphs in header cells use white text via table style text color.
    for ri in range(1 if header else 0, len(rows)):
        if ri % 2 == 0:
            commands.append(("BACKGROUND", (0, ri), (-1, ri), HexColor("#F6F8FA")))
    t.setStyle(TableStyle(commands))
    return t

class EvidencePlaceholder(Flowable):
    def __init__(self, width=174*mm, height=47*mm, caption="ẢNH DEMO LAN THỰC TẾ — bổ sung sau buổi chạy trên hai máy"):
        super().__init__()
        self.width, self.height, self.caption = width, height, caption
    def draw(self):
        self.canv.setStrokeColor(HexColor("#8798A8"))
        self.canv.setDash(4, 3)
        self.canv.roundRect(0, 0, self.width, self.height, 5*mm, stroke=1, fill=0)
        self.canv.setDash()
        self.canv.setFillColor(MUTED)
        self.canv.setFont("ArialVN-Bold", 10)
        self.canv.drawCentredString(self.width/2, self.height/2+3*mm, self.caption)
        self.canv.setFont("ArialVN", 8)
        self.canv.drawCentredString(self.width/2, self.height/2-4*mm, "Không chèn ảnh localhost thay cho bằng chứng chạy LAN.")

def network_diagram(label):
    d = Drawing(465, 92)
    def box(x, y, w, h, title, subtitle, fill):
        d.add(Rect(x, y, w, h, rx=8, ry=8, fillColor=fill, strokeColor=BLUE, strokeWidth=1))
        d.add(String(x+12, y+h-22, title, fontName="ArialVN-Bold", fontSize=9, fillColor=NAVY))
        d.add(String(x+12, y+14, subtitle, fontName="ArialVN", fontSize=7.6, fillColor=INK))
    box(12, 28, 130, 48, "Client 1", "WinForms · TCP/UTF-8", HexColor("#F3F7FA"))
    box(348, 28, 105, 48, "Client 2", "WinForms · TCP/UTF-8", HexColor("#F3F7FA"))
    box(175, 22, 115, 60, "Server · " + label, "0.0.0.0 · nhiều client", HexColor("#E8F2FA"))
    d.add(Line(142, 52, 175, 52, strokeColor=GOLD, strokeWidth=1.8, endArrow=True))
    d.add(Line(290, 52, 348, 52, strokeColor=GOLD, strokeWidth=1.8, endArrow=True))
    d.add(String(148, 62, "request", fontName="ArialVN", fontSize=7, fillColor=MUTED))
    d.add(String(301, 62, "snapshot", fontName="ArialVN", fontSize=7, fillColor=MUTED))
    d.add(String(168, 5, "" + label, fontName="ArialVN-Bold", fontSize=7.5, fillColor=BLUE))
    return d

def footer(canvas, doc):
    canvas.saveState()
    w, h = A4
    if doc.page > 1:
        canvas.setStrokeColor(HexColor("#DCE4EB")); canvas.line(18*mm, h-14*mm, w-18*mm, h-14*mm)
        canvas.setFont("ArialVN", 7.5); canvas.setFillColor(MUTED)
        canvas.drawString(18*mm, h-11*mm, "NT106.R14.1 · Thực hành Lab 3 · Nhóm 4")
    canvas.setStrokeColor(HexColor("#DCE4EB")); canvas.line(18*mm, 14*mm, w-18*mm, 14*mm)
    canvas.setFont("ArialVN", 7.5); canvas.setFillColor(MUTED)
    canvas.drawString(18*mm, 9*mm, "Bản làm việc — hoàn thiện ảnh demo LAN sau khi chạy tại lớp")
    canvas.drawRightString(w-18*mm, 9*mm, f"Trang {doc.page}")
    canvas.restoreState()

story = []
story += [Spacer(1, 25*mm), P("BÁO CÁO THỰC HÀNH · NT106.R14.1", "VNCoverLabel"),
          P("LẬP TRÌNH SOCKET<br/>TRONG C#", "VNTitle"),
          P("LAB 3 · BÀI 4 VÀ BÀI 5", "VNSubtitle"),
          Spacer(1, 8*mm),
          table([["Lớp học phần", "NT106.R14.1"], ["Nhóm", "Nhóm 4"], ["Đại diện nhóm", "Huỳnh Thanh Nguyên · 25521233"], ["Giảng viên", "(Bổ sung theo lớp)"]], [48*mm, 112*mm], header=False),
          Spacer(1, 12*mm),
          P('<b>Mã nguồn:</b> <link href="https://github.com/NguyenHuynh307/NT106.R14.1-Lab03-Sockets-Nhom4" color="#245B8F">github.com/NguyenHuynh307/NT106.R14.1-Lab03-Sockets-Nhom4</link>', "VNSubtitle"),
          Spacer(1, 17*mm), P("Bản báo cáo nhóm đang thực hiện · các mục chưa có kết quả được ghi rõ để thành viên phụ trách bổ sung.", "VNCallout"), PageBreak()]

story += [section("1. Thông tin nhóm và phân công"),
          P("Bài làm theo nhóm. Mỗi thành viên thực hiện các câu được phân công trên nhánh riêng; trưởng nhóm review Pull Request trước khi gộp vào nhánh main."),
          table([["Thành viên", "MSSV", "Phần việc", "Trạng thái báo cáo"],
                 ["Huỳnh Thanh Nguyên", "25521233", "Câu 4, 5 · trưởng nhóm", "Đã triển khai, build và kiểm tra TCP localhost"],
                 ["Nguyễn Quốc Nhật Minh", "25521123", "Câu 1, 2", "Chờ thành viên bổ sung"],
                 ["Hoàng Anh", "25520048", "Câu 3, 6", "Chờ thành viên bổ sung"]], [45*mm, 24*mm, 43*mm, 52*mm]),
          Spacer(1, 6*mm), P("Repo nhóm: <link href=\"https://github.com/NguyenHuynh307/NT106.R14.1-Lab03-Sockets-Nhom4\" color=\"#245B8F\">NT106.R14.1-Lab03-Sockets-Nhom4</link>. Cấu trúc dự kiến mỗi câu là một project riêng trong solution chung. Repo hiện có project câu 4 và 5; câu 1, 2, 3, 6 chưa được nhập nên báo cáo không ghi kết quả thay cho thành viên."),
          P("Phạm vi của báo cáo này là câu 4 và câu 5. Bố cục các form bài tập kế thừa từ Lab 2; phần thêm mới là launcher cấu hình server/client và lớp giao tiếp mạng.", "VNCallout"),
          section("2. Môi trường và cách tổ chức"),
          table([["Thành phần", "Thiết lập"], ["Nền tảng", ".NET 10 · Windows Forms · C#"], ["Solution", "NT106.R14.1-Lab03-Sockets-Nhom4.sln"], ["Bài 4", "Project Lab03_Bai04 · TCP cổng mặc định 8084"], ["Bài 5", "Project Lab03_Bai05 · TCP cổng mặc định 8085 · Microsoft.Data.Sqlite"]], [42*mm, 122*mm]),
          P("Build solution: <font name=\"ArialVN-Bold\">dotnet build NT106.R14.1-Lab03-Sockets-Nhom4.sln</font>. Trên máy chủ chọn MỞ SERVER; trên máy client nhập IPv4 LAN của máy chủ rồi chọn MỞ CLIENT. Máy client và server phải cùng mạng LAN; cần cho phép TCP 8084 hoặc 8085 qua Windows Firewall.", "VNBody"), PageBreak()]

story += [section("3. Mô hình giao tiếp"),
          P("Hai bài dùng TCP với request/response UTF-8 phân khung bằng ký tự xuống dòng. Server bind IPAddress.Any để nhận kết nối từ địa chỉ IPv4 trong LAN. Mỗi request được xử lý độc lập; tác vụ server nhận kết nối chạy bất đồng bộ và nhiều kết nối có thể cùng tồn tại."),
          network_diagram("Bài 4 · 8084"), Spacer(1, 2*mm),
          P("Bài 4: server đọc input5.txt khi khởi chạy, giữ catalog phim, ghế đã bán và doanh thu trong bộ nhớ. Server khóa thao tác cập nhật ghế để kiểm tra và ghi nhận một đơn như một bước nguyên tử. Client làm mới snapshot để thấy ghế và thống kê được cập nhật.", "VNBodySmall"),
          network_diagram("Bài 5 · 8085"), Spacer(1, 2*mm),
          P("Bài 5: mọi thao tác CRUD và chọn món được gọi qua RPC TCP tới server. SQLite và ảnh tải lên nằm ở máy chủ. Server tuần tự hóa thao tác dữ liệu SQLite; client hỏi revision định kỳ và tải lại dữ liệu khi revision thay đổi.", "VNBodySmall"), PageBreak()]

story += [section("4. Câu 4 · Đặt vé xem phim qua TCP"),
          P("Mục tiêu: đọc danh sách phim từ input5.txt phía server, cho client chọn phim/phòng/suất chiếu/ghế và đặt vé; server giữ trạng thái trung tâm để nhiều client không thể bán trùng một ghế."),
          P("Triển khai" , "VNSubsection"),
          P("• Dùng lại form đặt vé của Lab 2, không thay đổi bố cục điều khiển. Launcher riêng chọn mở server hoặc client và nhận IP/cổng server.<br/>• Server đọc và kiểm tra input5.txt; quản lý catalog phim, ghế bán, số vé và doanh thu.<br/>• Client gửi mã phim, phòng, suất, ghế qua TCP. Server tự xác thực phim/phòng/suất/mã ghế, tính doanh thu theo giá và hệ số ghế, kiểm tra ghế trong vùng khóa rồi chỉ commit nếu cả đơn còn trống.<br/>• Client tải snapshot định kỳ nên các ghế bán từ client khác được phản ánh tự động."),
          P("Kết quả kiểm tra cục bộ", "VNSubsection"),
          table([["Kiểm tra", "Kết quả quan sát"], ["Nạp dữ liệu", "Server đọc thành công 5 phim từ input5.txt."], ["Kết nối", "Hai kết nối client TCP localhost truy vấn được cùng snapshot server."], ["Đặt ghế", "Yêu cầu đầu cho Avatar · phòng 1 · suất 09:15 · ghế A1 được ghi nhận."], ["Đặt trùng", "Yêu cầu thứ hai cho cùng ghế bị từ chối với thông báo ghế vừa được client khác đặt."]], [40*mm, 124*mm]),
          Spacer(1, 5*mm), P("Ảnh giao diện và demo LAN", "VNSubsection"), EvidencePlaceholder(height=40*mm),
          Spacer(1, 4*mm), P("Ảnh cần chụp tại lớp: cửa sổ server trên máy 1 và hai client trên máy 1/máy 2; thể hiện client thứ hai thấy ghế A1 đã bán. Không dùng kết quả localhost làm bằng chứng LAN.", "VNBodySmall"), PageBreak()]

story += [section("5. Câu 5 · Hôm nay ăn gì? qua TCP"),
          P("Mục tiêu: nhiều client cùng thao tác trên một dữ liệu món ăn tập trung. Chức năng giao diện Lab 2 được giữ lại; lời gọi tới kho dữ liệu được chuyển thành RPC TCP."),
          P("Triển khai", "VNSubsection"),
          P("• SQLite của server nằm tại Data/HomNayAnGi.db; ảnh món được lưu tại Data/Images.<br/>• Client gọi qua TCP các thao tác thêm/xóa người đóng góp và món, sửa thông tin, xem danh sách, lọc và chọn ngẫu nhiên; yêu thích, lịch sử và thông tin quán cũng dùng kho chung.<br/>• Khi client thêm ảnh, ảnh được gửi trong request UTF-8/base64, lưu vào thư mục ảnh server và tên ảnh được ghi trong SQLite. Khi client hiển thị ảnh, nó tải nội dung ảnh từ server về cache cục bộ.<br/>• Server tăng revision sau thao tác ghi; client kiểm tra revision mỗi 2,2 giây để làm mới dữ liệu dùng chung."),
          P("Kết quả kiểm tra cục bộ", "VNSubsection"),
          table([["Kiểm tra", "Kết quả quan sát"], ["Thêm và xem chung", "Thêm món thử bằng một request; kết nối client thứ hai đọc được món từ server."], ["Truyền ảnh", "Server nhận ảnh PNG và trả lại 118 byte ảnh qua TCP."], ["Khởi động lại", "Món thử vẫn tồn tại sau khi dừng và khởi động lại server; xác nhận dữ liệu được lưu trong SQLite."], ["Dọn dữ liệu thử", "Đã xóa món/người đóng góp thử sau khi kiểm tra; ảnh thử được dọn khỏi thư mục dữ liệu." ]], [40*mm, 124*mm]),
          P("Mỗi thao tác chọn món ngẫu nhiên được thực thi trên kho dữ liệu server; các client cùng chọn trên dữ liệu SQLite chung. Kết quả giao diện và việc chọn theo cộng đồng cần chụp trong demo LAN để bổ sung vào bản nộp cuối.", "VNCallout"),
          P("Ảnh giao diện và demo LAN", "VNSubsection"), EvidencePlaceholder(height=40*mm), PageBreak()]

story += [section("6. Kiểm tra LAN trước buổi chấm"),
          P("Danh sách thao tác cần thực hiện trên hai máy tính cùng mạng trước khi nộp báo cáo cuối:"),
          table([["Bước", "Máy server", "Máy client"],
                 ["1", "Chạy bài tương ứng; chọn MỞ SERVER; xác nhận đang nghe cổng 8084 hoặc 8085.", "Cùng Wi‑Fi/LAN; dùng IPv4 của máy server, không dùng 127.0.0.1."],
                 ["2", "Cho phép ứng dụng/cổng TCP qua Firewall của mạng Private.", "Chọn MỞ CLIENT và kiểm tra trạng thái đã kết nối."],
                 ["3 · Bài 4", "Giữ server hoạt động.", "Client 1 đặt ghế; client 2 quan sát ghế bán và thử đặt trùng để xác nhận bị từ chối."],
                 ["4 · Bài 5", "Kiểm tra Data/HomNayAnGi.db và Data/Images trên server.", "Client 1 thêm món có dấu tiếng Việt và ảnh; client 2 tải lại, xem món và chọn ngẫu nhiên."],
                 ["5", "Khởi động lại server để xác nhận dữ liệu món còn.", "Chụp ảnh màn hình hai máy, lưu tên thành viên/ngày/giờ để đưa vào báo cáo."]], [16*mm, 75*mm, 73*mm]),
          Spacer(1, 7*mm), P("Ảnh demo thực tế hai máy", "VNSubsection"), EvidencePlaceholder(height=44*mm),
          Spacer(1, 5*mm), P("Trạng thái hiện tại: chưa thực hiện buổi chạy trên hai máy riêng; nhóm cần thay các khung chờ bằng ảnh LAN và xác nhận các bước trên. Phần này không được ghi nhận là hoàn tất chỉ dựa vào kiểm tra localhost.", "VNCallout"), PageBreak()]

story += [section("7. Câu 1 · Nguyễn Quốc Nhật Minh"),
          P("CHỜ THÀNH VIÊN BỔ SUNG", "VNSubsection"),
          P("Chưa có mã nguồn, mô tả triển khai, ảnh giao diện hoặc kết quả chạy câu 1 trong repo tại thời điểm tạo bản báo cáo này. Minh cập nhật mục này bằng mô tả yêu cầu, thiết kế, cách chạy và bằng chứng kiểm tra sau khi mở Pull Request." , "VNCallout"),
          Spacer(1, 10*mm), section("8. Câu 2 · Nguyễn Quốc Nhật Minh"),
          P("CHỜ THÀNH VIÊN BỔ SUNG", "VNSubsection"),
          P("Chưa ghi kết quả thay cho thành viên phụ trách. Bổ sung mã nguồn, mô tả triển khai, cách chạy và ảnh kiểm chứng sau khi hoàn thành câu 2.", "VNCallout"),
          Spacer(1, 10*mm), section("9. Câu 3 · Hoàng Anh"),
          P("CHỜ THÀNH VIÊN BỔ SUNG", "VNSubsection"),
          P("Chưa có kết quả câu 3 trong repo tại thời điểm tạo báo cáo. Bổ sung mô tả triển khai và bằng chứng kiểm tra sau khi hoàn tất.", "VNCallout"),
          Spacer(1, 8*mm), section("10. Câu 6 · Hoàng Anh"),
          P("CHỜ THÀNH VIÊN BỔ SUNG", "VNSubsection"),
          P("Chưa có kết quả câu 6 trong repo tại thời điểm tạo báo cáo. Bổ sung mô tả triển khai và bằng chứng kiểm tra sau khi hoàn tất.", "VNCallout"),
          Spacer(1, 10*mm), P("Bản báo cáo nhóm sẽ được cập nhật sau khi các Pull Request được review/merge và nhóm hoàn tất demo client–server trên mạng LAN.", "VNBody"),
          ]

doc = SimpleDocTemplate(str(OUT), pagesize=A4, rightMargin=23*mm, leftMargin=23*mm, topMargin=21*mm, bottomMargin=22*mm, title="Báo cáo thực hành Lab 3 Socket C# - Nhóm 4", author="Nhóm 4 · NT106.R14.1")
doc.build(story, onFirstPage=footer, onLaterPages=footer)
print(str(OUT).encode("ascii", "backslashreplace").decode("ascii"))
