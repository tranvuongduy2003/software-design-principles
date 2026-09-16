using System;
using System.Collections.Generic;

namespace SoftwareDesignPrinciples.SOLID
{
    /// <summary>
    /// Minh họa Open/Closed Principle (OCP - Nguyên lý Mở/Đóng) thông qua bài toán
    /// tính tổng diện tích của nhiều loại hình học khác nhau.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <b>1. Bài toán thực tế.</b>
    /// Giả sử hệ thống ban đầu chỉ hỗ trợ tính diện tích hình tròn và hình chữ nhật.
    /// Khi có yêu cầu bổ sung hình tam giác, ta buộc phải can thiệp và sửa đổi chuỗi
    /// if/else bên trong bộ tính toán diện tích. Mỗi lần xuất hiện một loại hình mới,
    /// lớp xử lý đang hoạt động ổn định lại bị thay đổi, kéo theo nguy cơ phát sinh lỗi
    /// hồi quy (regression bugs) và toàn bộ kiểm thử liên quan đều phải chạy lại.
    /// </para>
    /// <para>
    /// <b>2. Định nghĩa.</b>
    /// OCP quy định rằng một software entity (thực thể phần mềm như lớp, mô-đun hoặc hàm)
    /// phải: "Open for extension (mở cho việc mở rộng), nhưng closed for modification
    /// (đóng đối với việc sửa đổi)". Khái niệm "đóng" ở đây không đồng nghĩa với việc
    /// đóng băng mã nguồn vĩnh viễn, mà là các thay đổi nghiệp vụ đã được dự trù nên
    /// được đáp ứng bằng cách bổ sung mã mới (viết thêm thành phần mới) thay vì sửa đổi
    /// trực tiếp vào mã nguồn lõi đang hoạt động ổn định.
    /// </para>
    /// <para>
    /// <b>3. Bản chất.</b>
    /// Bản chất của OCP là phân tách phần bất biến (ổn định) khỏi phần khả biến (thường
    /// xuyên biến đổi). Hệ thống định nghĩa một contract (hợp đồng trừu tượng) cố định,
    /// và mỗi biến thể nghiệp vụ sẽ hiện thực (implement) hợp đồng đó. Client code
    /// (mã tiêu thụ) chỉ phụ thuộc vào contract trừu tượng nên hoàn toàn độc lập với
    /// danh sách các biến thể cụ thể. OCP là nguyên lý định hướng; còn polymorphism
    /// (đa hình), Strategy pattern hay kiến trúc plugin là các kỹ thuật thực hiện.
    /// </para>
    /// <para>
    /// <b>4. Cơ chế hoạt động.</b>
    /// Interface IShape đóng vai trò là extension point (điểm mở rộng) thông qua
    /// phương thức CalculateArea. Lớp AreaCalculator chỉ cần duyệt qua danh sách
    /// IEnumerable&lt;IShape&gt; và gọi CalculateArea trên từng đối tượng. Cơ chế
    /// dynamic dispatch (đa hình tại thời điểm chạy) của C# sẽ tự động điều hướng
    /// đến đúng phương thức cài đặt của kiểu đối tượng thực tế. Nhờ đó, khi bổ sung
    /// Triangle, ta chỉ cần tạo mới lớp Triangle; thuật toán tính tổng diện tích trong
    /// AreaCalculator được giữ nguyên hoàn toàn.
    /// </para>
    /// <para>
    /// <b>5. Mô hình và cú pháp C#.</b>
    /// C# dùng từ khóa interface để khai báo contract trừu tượng; cú pháp kế thừa
    /// “Circle : IShape” biểu thị việc hiện thực contract; còn IEnumerable&lt;IShape&gt;
    /// cho phép quản lý một tập hợp đa hình (polymorphic collection) chứa các kiểu dẫn
    /// xuất cụ thể. Đây là kỹ thuật subtype polymorphism (đa hình theo kiểu con). Cần
    /// lưu ý, OCP còn có thể hiện thực thông qua delegate, generic, composition hoặc
    /// cấu hình động; interface chỉ là một trong những phương tiện phổ biến nhất.
    /// </para>
    /// <para>
    /// <b>6. Phân tích ví dụ.</b>
    /// Trong thiết kế chưa tốt, BadAreaCalculator nhận một đối tượng dữ liệu dùng chung
    /// rồi kiểm tra loại hình thông qua chuỗi Type. Cách làm này khiến bộ tính toán
    /// phải nắm giữ mọi công thức, vừa thiếu type safety (an toàn kiểu dữ liệu), vừa
    /// biến lớp này thành "điểm nóng" phải liên tục sửa đổi (modification hotspot).
    /// Ngược lại, thiết kế tốt đẩy công thức về cho từng loại hình tự quản lý.
    /// AreaCalculator chỉ còn trách nhiệm cộng dồn kết quả, vừa tuân thủ OCP vừa
    /// phối hợp hoàn hảo với Single Responsibility Principle (SRP).
    /// </para>
    /// <para>
    /// <b>7. Phạm vi áp dụng.</b>
    /// OCP nên được áp dụng tại các trục biến đổi (axes of change) có tần suất thay đổi
    /// cao hoặc đã được dự báo rõ ràng: phương thức thanh toán, chính sách định giá,
    /// định dạng xuất báo cáo, thuật toán xử lý hoặc cổng tích hợp dịch vụ bên thứ ba.
    /// Tuyệt đối tránh trừu tượng hóa sớm (over-engineering) cho những yêu cầu giả định
    /// chưa chắc đã xảy ra. Với các logic đơn giản, việc sửa đổi trực tiếp mã nguồn là
    /// hoàn toàn chấp nhận được cho đến khi một xu hướng thay đổi lặp lại xuất hiện.
    /// </para>
    /// <para>
    /// <b>8. Lỗi thường gặp.</b>
    /// Các sai lầm phổ biến bao gồm: lạm dụng chuỗi if/switch để kiểm tra kiểu dữ liệu;
    /// dùng "magic string" để phân biệt loại hình; trừu tượng hóa quá mức khi vội vàng
    /// tạo interface cho mọi lớp dù không có nhu cầu mở rộng; hoặc thiết kế dở dang
    /// khiến client code vẫn phải sửa đổi mỗi khi thêm lớp mới. Ngoài ra, không nên cực
    /// đoan xem mọi câu lệnh switch là vi phạm OCP: sử dụng switch/pattern matching tại
    /// composition root hoặc Factory để khởi tạo implementation từ cấu hình vẫn là
    /// cách tiếp cận hoàn toàn hợp lý.
    /// </para>
    /// <para>
    /// <b>9. Giới hạn.</b>
    /// Không có thiết kế nào có thể "đóng" tuyệt đối trước mọi loại thay đổi. Interface
    /// IShape giúp hệ thống đóng kín trước việc bổ sung các loại hình mới, nhưng nếu
    /// yêu cầu thay đổi theo chiều hướng khác (ví dụ: cần tính thêm chu vi, vẽ hình hay
    /// tính thể tích), interface IShape vẫn buộc phải sửa đổi hoặc cần đến các mẫu thiết
    /// kế phức tạp hơn như Visitor pattern. Do đó, OCP luôn gắn liền với một chiều thay
    /// đổi cụ thể và mang tính tương đối, không phải trạng thái tĩnh hay tuyệt đối.
    /// </para>
    /// <para>
    /// <b>10. Câu hỏi phỏng vấn và đáp án mẫu.</b>
    /// Câu hỏi: "Áp dụng OCP có đồng nghĩa với việc không bao giờ được sửa mã nguồn cũ không?"
    /// Đáp án: Không. Các hoạt động như sửa lỗi (bug fixes), tái cấu trúc (refactoring)
    /// hay điều chỉnh hợp đồng chung vẫn yêu cầu sửa mã cũ. OCP định hướng việc thiết
    /// lập các điểm mở rộng cho các biến thể đã dự đoán, nhờ đó tính năng mới có thể
    /// được bổ sung chủ yếu bằng cách thêm mã mới, giảm thiểu tối đa rủi ro gây lỗi cho
    /// phần lõi đang chạy ổn định.
    /// </para>
    /// <para>
    /// Câu hỏi: "Vì sao tính đa hình (polymorphism) lại là nền tảng cốt lõi của OCP?"
    /// Đáp án: Vì đa hình cho phép client code tương tác thông qua một contract chung,
    /// trong khi việc chọn hành vi cài đặt cụ thể được quyết định linh hoạt tại runtime.
    /// Nhờ đó, các biến thể mới có thể cắm vào hệ thống mà client code không cần biết
    /// kiểu dữ liệu cụ thể hay phải kiểm tra kiểu bằng các câu lệnh rẽ nhánh.
    /// </para>
    /// </remarks>
    public static class OpenClosedDemo
    {
        /// <summary>
        /// So sánh giữa cách tiếp cận phân nhánh theo kiểu (vi phạm OCP) và điều hướng đa hình (tuân thủ OCP).
        /// </summary>
        public static void Run()
        {
            Console.WriteLine("=== O - NGUYÊN LÝ MỞ/ĐÓNG (OCP) ===");

            var badShapes = new List<BadShape>
            {
                new() { Type = "Circle", Radius = 5 },
                new() { Type = "Rectangle", Width = 4, Height = 5 }
            };

            var badCalculator = new BadAreaCalculator();
            Console.WriteLine(
                $"[Chưa tốt] Tổng diện tích: " +
                $"{badCalculator.CalculateTotalArea(badShapes):F2}");

            // Bổ sung thêm Triangle mà không cần sửa đổi bất kỳ dòng mã nào trong AreaCalculator.
            // Đây chính là trục mở rộng (axis of change) mà OCP hướng tới bảo vệ.
            var shapes = new List<IShape>
            {
                new Circle(5),
                new Rectangle(4, 5),
                new Triangle(4, 5)
            };

            var calculator = new AreaCalculator();
            Console.WriteLine(
                $"[Tốt] Tổng diện tích: " +
                $"{calculator.CalculateTotalArea(shapes):F2}");
            Console.WriteLine(
                "Kết luận: Bổ sung loại hình mới mà không cần sửa đổi thuật toán tính tổng.");
            Console.WriteLine();
        }
    }

    #region Thiết kế vi phạm OCP

    /// <summary>
    /// Mô hình dữ liệu thiếu an toàn kiểu (anemic model): dồn tất cả thuộc tính của mọi loại hình
    /// vào một lớp duy nhất, khiến một số thuộc tính trở nên vô nghĩa đối với loại hình tương ứng (ví dụ: hình tròn vẫn có Width/Height).
    /// </summary>
    public sealed class BadShape
    {
        public string Type { get; init; } = string.Empty;
        public double Radius { get; init; }
        public double Width { get; init; }
        public double Height { get; init; }
    }

    /// <summary>
    /// Thiết kế vi phạm OCP: Bộ tính toán phải nắm rõ chi tiết cụ thể và tự phân nhánh logic cho từng loại hình.
    /// </summary>
    public sealed class BadAreaCalculator
    {
        public double CalculateTotalArea(IEnumerable<BadShape> shapes)
        {
            ArgumentNullException.ThrowIfNull(shapes);

            double area = 0;
            foreach (var shape in shapes)
            {
                if (shape.Type == "Circle")
                {
                    area += Math.PI * shape.Radius * shape.Radius;
                }
                else if (shape.Type == "Rectangle")
                {
                    area += shape.Width * shape.Height;
                }

                // Vi phạm OCP: Muốn hỗ trợ thêm Triangle hay bất kỳ hình nào khác, ta bắt buộc phải can thiệp
                // và sửa đổi phương thức này. Ngoài ra, việc dựa vào magic string ("Circle", "Rectangle")
                // rất dễ gây lỗi runtime nếu sai chính tả mà trình biên dịch không thể phát hiện.
            }

            return area;
        }
    }

    #endregion

    #region Thiết kế tuân thủ OCP

    /// <summary>
    /// Abstraction đóng vai trò là contract chung, định nghĩa khả năng tính diện tích cho mọi loại hình.
    /// </summary>
    public interface IShape
    {
        double CalculateArea();
    }

    /// <summary>
    /// Biến thể hình tròn tự đóng gói dữ liệu kích thước và công thức tính diện tích của riêng mình.
    /// </summary>
    public sealed class Circle : IShape
    {
        public double Radius { get; }

        public Circle(double radius)
        {
            if (radius < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(radius));
            }

            Radius = radius;
        }

        public double CalculateArea()
        {
            return Math.PI * Radius * Radius;
        }
    }

    /// <summary>
    /// Biến thể hình chữ nhật hiện thực (implement) contract IShape.
    /// </summary>
    public sealed class Rectangle : IShape
    {
        public double Width { get; }
        public double Height { get; }

        public Rectangle(double width, double height)
        {
            if (width < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width));
            }

            if (height < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(height));
            }

            Width = width;
            Height = height;
        }

        public double CalculateArea()
        {
            return Width * Height;
        }
    }

    /// <summary>
    /// Lớp mở rộng mới: Được bổ sung độc lập để đáp ứng yêu cầu nghiệp vụ mà không làm thay đổi AreaCalculator.
    /// </summary>
    public sealed class Triangle : IShape
    {
        public double BaseLength { get; }
        public double Height { get; }

        public Triangle(double baseLength, double height)
        {
            if (baseLength < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(baseLength));
            }

            if (height < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(height));
            }

            BaseLength = baseLength;
            Height = height;
        }

        public double CalculateArea()
        {
            return BaseLength * Height / 2;
        }
    }

    /// <summary>
    /// Mô-đun ổn định (closed for modification): Chỉ phụ thuộc và tổng hợp kết quả thông qua abstraction IShape.
    /// </summary>
    public sealed class AreaCalculator
    {
        public double CalculateTotalArea(IEnumerable<IShape> shapes)
        {
            ArgumentNullException.ThrowIfNull(shapes);

            double total = 0;
            foreach (var shape in shapes)
            {
                // Tuân thủ OCP: Không cần if/else, switch hay kiểm tra kiểu dữ liệu cụ thể.
                // Cơ chế đa hình tại runtime sẽ tự động điều hướng đến đúng phương thức CalculateArea của từng đối tượng.
                total += shape.CalculateArea();
            }

            return total;
        }
    }

    #endregion
}
