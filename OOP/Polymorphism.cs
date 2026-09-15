using System;
using System.Collections.Generic;

namespace SoftwareDesignPrinciples.OOP
{
    /// <summary>
    /// Minh họa nguyên lý Polymorphism (tính đa hình) thông qua bài toán
    /// nạp chồng phương thức tính toán và tính diện tích hình học.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Đặt vấn đề: Giả sử hệ thống cần xử lý một danh sách gồm nhiều loại hình
    /// học khác nhau (hình tròn, hình chữ nhật, hình tam giác). Nếu client code
    /// duyệt danh sách và dùng câu lệnh kiểm tra kiểu (như if-else hoặc
    /// switch-case) để tự tính diện tích, thì mỗi khi bổ sung một loại hình
    /// mới, ta buộc phải tìm và sửa đổi tất cả các chuỗi điều kiện tương tự
    /// trong toàn bộ mã nguồn. Điều này khiến mã nguồn bị phân mảnh và vi phạm
    /// trực tiếp nguyên lý Mở/Đóng (Open/Closed Principle). Trách nhiệm tính
    /// diện tích (behavior) phải thuộc về chính từng hình cụ thể, còn phía gọi
    /// chỉ cần phát tín hiệu yêu cầu thông qua phương thức chung CalculateArea.
    /// </para>
    /// <para>
    /// Định nghĩa: Polymorphism (tính đa hình - bắt nguồn từ tiếng Hy Lạp nghĩa
    /// là "nhiều hình thái") là khả năng các đối tượng thuộc các kiểu dữ liệu
    /// khác nhau có thể phản ứng khác nhau trước cùng một thông điệp hoặc lời
    /// gọi phương thức. Tính đa hình cho phép client code tương tác với nhiều
    /// đối tượng khác nhau theo một cách thống nhất thông qua kiểu trừu tượng
    /// chung. Cần lưu ý: Polymorphism không đồng nhất với Kế thừa; Interface,
    /// Generic (Parametric Polymorphism), và Overloading (Ad-hoc
    /// Polymorphism) cũng tạo ra các hình thái đa hình mạnh mẽ mà không nhất
    /// thiết phụ thuộc vào cây kế thừa lớp (class hierarchy).
    /// </para>
    /// <para>
    /// Phân loại Đa hình trong C#:
    /// - Compile-time Polymorphism (Đa hình lúc biên dịch):
    ///   Được hiện thực hóa qua Method Overloading (nạp chồng phương thức).
    ///   Nhiều phương thức có cùng tên nhưng khác nhau về danh sách tham số
    ///   (số lượng, kiểu dữ liệu, thứ tự). Trình biên dịch (compiler) sẽ phân
    ///   giải và quyết định chính xác phương thức cần gọi ngay tại thời điểm
    ///   biên dịch dựa trên số lượng và kiểu tĩnh (static type) của đối số.
    ///   Lưu ý: Chỉ thay đổi kiểu trả về (return type) sẽ không tạo ra một
    ///   overload hợp lệ trong cùng một kiểu khai báo.
    /// - Runtime Polymorphism (Đa hình lúc chạy / Dynamic Polymorphism): Được
    ///   hiện thực hóa qua Method Overriding (ghi đè phương thức) kết hợp với
    ///   kế thừa hoặc interface. Lớp cơ sở Shape khai báo CalculateArea là
    ///   abstract, và các lớp cụ thể (concrete classes) sẽ dùng từ khóa
    ///   override để cung cấp công thức riêng. Khi thực thi, cơ chế
    ///   Điều phối động (Dynamic Dispatch) của CLR (thông qua vtable)
    ///   sẽ xác định và kích hoạt cài đặt của kiểu thực tế (runtime type) tại
    ///   thời điểm gọi, chứ không phụ thuộc vào kiểu khai báo tĩnh.
    /// </para>
    /// <para>
    /// Static Type và Runtime Type trong Type Casting:
    /// - Xét câu lệnh: Shape shape = new Circle(5);
    ///   + Static Type (kiểu tĩnh) là Shape: Được trình biên dịch sử dụng để
    ///     kiểm tra cú pháp và chỉ cho phép gọi thành viên do Shape công bố.
    ///   + Runtime Type (kiểu lúc chạy) là Circle: Kiểu thực tế của đối tượng
    ///     nằm trên bộ nhớ Heap, được runtime dùng cho Dynamic Dispatch.
    /// - Upcasting (chuyển kiểu con lên kiểu cha, ví dụ Circle -> Shape):
    ///   Diễn ra ngầm định (implicit), luôn an toàn và không gây mất dữ liệu.
    /// - Downcasting (chuyển kiểu cha xuống con, ví dụ Shape -> Circle):
    ///   Tiềm ẩn rủi ro ném InvalidCastException. Luôn phải kiểm tra kiểu an
    ///   toàn trước khi ép kiểu (bằng 'is', 'as' hoặc Pattern Matching).
    /// </para>
    /// <para>
    /// Luồng thực thi trong ví dụ:
    /// - Phần 1 (MathOperations): Thể hiện Compile-time Polymorphism khi
    ///   compiler tự động đối soát và phân giải ba overload của Add.
    /// - Phần 2 (Hệ thống hình học Shape): Thể hiện Runtime Polymorphism khi
    ///   mảng Shape chứa ba kiểu cụ thể khác nhau. Vòng lặp chỉ gọi duy nhất
    ///   phương thức shape.CalculateArea() nhưng nhận được ba kết quả với ba
    ///   thuật toán tính toán khác nhau mà hoàn toàn không cần bất kỳ câu lệnh
    ///   kiểm tra kiểu hay rẽ nhánh điều kiện nào ở phía client.
    /// </para>
    /// <para>
    /// Ứng dụng thực tế và Ranh giới kiến trúc:
    /// - Đa hình là nền tảng của hầu hết các mẫu thiết kế hướng đối tượng
    ///   (Strategy Pattern, State Pattern, Factory Pattern, Command Pattern).
    /// - Cho phép hoán đổi linh hoạt các thuật toán, thay thế các nhà cung cấp
    ///   dịch vụ (providers), xây dựng plugin, middleware hoặc pipeline.
    /// - Phân định phạm vi: Pattern Matching (hoặc switch theo type) phù hợp
    ///   tại các ranh giới chuyển đổi dữ liệu (data boundaries, DTO parsing,
    ///   thao tác trên cấu trúc dữ liệu thuần túy không có hành vi). Tuy
    ///   nhiên, nếu kiểm tra kiểu bị lặp lại trong tầng nghiệp vụ cốt lõi, đó
    ///   là dấu hiệu cảnh báo (code smell) cho thấy hành vi đang bị đặt sai
    ///   vị trí và cần được thay thế bằng Polymorphism.
    /// </para>
    /// <para>
    /// Lỗi thiết kế thường gặp (Code Smells &amp; Anti-patterns):
    /// - Trả về giá trị mặc định giả tạo (Magic/Dummy Value): Lớp cha trả về
    ///   0 hoặc null thay vì khai báo abstract/virtual, khiến lớp con quên
    ///   override mà compiler không thể cảnh báo, dẫn đến lỗi logic âm thầm.
    /// - Nhầm lẫn giữa Overloading và Overriding: Bối rối giữa thời điểm phân
    ///   giải của compiler (static) và runtime (dynamic), dẫn đến sai luồng.
    /// - Lạm dụng từ khóa 'new' để ẩn thành viên (Member Hiding): Làm đứt gãy
    ///   chuỗi đa hình vì lời gọi qua biến lớp cha sẽ không gọi hàm lớp con.
    /// - Ép kiểu ngược (Downcasting) ngay sau khi nhận abstraction: Phá vỡ bản
    ///   chất của đa hình; nếu client code liên tục ép kiểu về concrete type,
    ///   abstraction đó đã thất bại.
    /// - Vi phạm nguyên lý thay thế Liskov (LSP): Lớp con ghi đè phương thức
    ///   nhưng phá vỡ hành vi kỳ vọng của lớp cha (ném NotImplementedException,
    ///   giới hạn tham số bất hợp lý).
    /// - Nạp chồng quá mức kèm ép kiểu ngầm định (Overload Ambiguity): Tạo ra
    ///   quá nhiều overload với tham số có thể chuyển đổi ngầm, khiến compiler
    ///   không thể phân giải lời gọi hoặc chọn nhầm hàm.
    /// </para>
    /// <para>
    /// Câu hỏi phỏng vấn kinh điển:
    /// - Câu hỏi: "Phân biệt Method Overloading và Method Overriding?"
    ///   + Method Overloading (Đa hình tĩnh): Diễn ra trong cùng phạm vi lớp;
    ///     cùng tên nhưng khác danh sách tham số; được compiler phân giải
    ///     tại thời điểm biên dịch dựa trên static type.
    ///   + Method Overriding (Đa hình động): Diễn ra giữa lớp cha và lớp con;
    ///     cùng tên và cùng chữ ký phương thức; được môi trường thực thi phân
    ///     giải tại thời điểm chạy dựa trên runtime type (thông qua vtable).
    ///   + Góc nhìn chuyên sâu: Nếu một biến kiểu cha Shape chứa đối tượng con
    ///     Circle được truyền vào phương thức nạp chồng, compiler sẽ chọn phiên
    ///     bản nhận Shape (phân giải tĩnh). Nhưng nếu gọi một phương thức
    ///     virtual/abstract trên chính biến đó, runtime sẽ thực thi phiên bản
    ///     của Circle (phân giải động).
    /// - Câu hỏi: "Vì sao Shape.CalculateArea nên được khai báo là abstract
    ///   thay vì virtual với giá trị trả về mặc định là 0?"
    ///   + Trả lời: Không tồn tại một công thức tính diện tích mặc định có ý
    ///     nghĩa cho mọi hình học bất kỳ. Khai báo abstract biến trách nhiệm
    ///     cài đặt thành một hợp đồng bắt buộc: nếu một lớp con quên định nghĩa
    ///     công thức, trình biên dịch sẽ báo lỗi ngay lập tức (compile-time
    ///     error). Nếu cung cấp giá trị mặc định 0, chương trình vẫn biên dịch
    ///     thành công nhưng sẽ chạy với kết quả sai ngầm, gây rủi ro nghiêm
    ///     trọng và rất khó truy vết lỗi logic trong hệ thống.
    /// </para>
    /// </remarks>
    public static class PolymorphismDemo
    {
        public static void Run()
        {
            Console.WriteLine("--- POLYMORPHISM (TÍNH ĐA HÌNH) ---");

            // Phần 1: Compile-time Polymorphism - Trình biên dịch phân giải
            // overload dựa trên số lượng và kiểu tĩnh của đối số.
            Console.WriteLine(
                "Compile-time Polymorphism - Method Overloading:");
            var math = new MathOperations();
            Console.WriteLine($"Add(int, int): {math.Add(5, 10)}");
            Console.WriteLine($"Add(double, double): {math.Add(5.5, 10.5)}");
            Console.WriteLine($"Add(int, int, int): {math.Add(5, 10, 15)}");

            // Phần 2: Runtime Polymorphism - Cùng kiểu tĩnh Shape nhưng mỗi
            // phần tử mang một kiểu thực tế (runtime type) riêng biệt.
            Console.WriteLine(
                "\nRuntime Polymorphism - Method Overriding:");
            IReadOnlyList<Shape> shapes = new Shape[]
            {
                new Circle(5),
                new Rectangle(4, 6),
                new Triangle(4, 5)
            };

            foreach (var shape in shapes)
            {
                // Cơ chế Dynamic Dispatch tự động định tuyến và gọi phiên bản
                // CalculateArea tương ứng với kiểu thực tế tại runtime.
                Console.WriteLine(
                    $"{shape.Name,-12} | " +
                    $"Diện tích: {shape.CalculateArea(),6:F2}");
            }

            Console.WriteLine();
        }
    }

    /// <summary>
    /// Minh họa Compile-time Polymorphism qua kỹ thuật Method Overloading
    /// (nạp chồng phương thức).
    /// </summary>
    /// <remarks>
    /// Các phương thức có cùng tên nhưng khác nhau về chữ ký tham số. Trình
    /// biên dịch sẽ tự động chọn phiên bản phù hợp nhất dựa trên kiểu tĩnh của
    /// các đối số truyền vào. Việc chỉ thay đổi kiểu trả về (return type)
    /// không được coi là một overload hợp lệ trong C#.
    /// </remarks>
    public class MathOperations
    {
        public int Add(int a, int b) => a + b;
        public double Add(double a, double b) => a + b;
        public int Add(int a, int b, int c) => a + b + c;
    }

    /// <summary>
    /// Lớp cơ sở trừu tượng (abstract base class) định nghĩa hợp đồng chung
    /// cho mọi hình học.
    /// </summary>
    /// <remarks>
    /// Bắt buộc tất cả các lớp con cụ thể (concrete classes) phải tự cài đặt
    /// công thức tính diện tích. Tuyệt đối không cung cấp cài đặt mặc định giả
    /// tạo (như trả về 0) vì không có công thức chung nào đúng cho mọi hình.
    /// </remarks>
    public abstract class Shape
    {
        public string Name { get; }

        protected Shape(string name)
        {
            Name = name;
        }

        // Khai báo abstract chuyển lỗi thiếu công thức tính toán thành lỗi
        // biên dịch (compile-time error) thay vì lỗi lúc chạy.
        public abstract double CalculateArea();
    }

    /// <summary>
    /// Đại diện cho hình tròn; tự bảo vệ bất biến (invariant) về bán kính
    /// và hiện thực hóa công thức tính diện tích riêng.
    /// </summary>
    public sealed class Circle : Shape
    {
        public double Radius { get; }

        public Circle(double radius) : base("Hình tròn")
        {
            if (radius <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(radius),
                    "Bán kính phải lớn hơn 0.");
            }

            Radius = radius;
        }

        // CLR tự động kích hoạt phương thức này khi kiểu thực tế
        // (runtime type) của đối tượng là Circle.
        public override double CalculateArea() => Math.PI * Radius * Radius;
    }

    /// <summary>
    /// Đại diện cho hình chữ nhật; đảm bảo tính toàn vẹn của kích thước
    /// và hiện thực hóa công thức tính diện tích.
    /// </summary>
    public sealed class Rectangle : Shape
    {
        public double Width { get; }
        public double Height { get; }

        public Rectangle(double width, double height) : base("Hình chữ nhật")
        {
            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(width),
                    "Chiều rộng phải lớn hơn 0.");
            }

            if (height <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(height),
                    "Chiều cao phải lớn hơn 0.");
            }

            Width = width;
            Height = height;
        }

        public override double CalculateArea() => Width * Height;
    }

    /// <summary>
    /// Đại diện cho hình tam giác; quản lý kích thước cạnh đáy, chiều cao
    /// và hiện thực hóa công thức tính diện tích đặc thù.
    /// </summary>
    public sealed class Triangle : Shape
    {
        public double BaseLength { get; }
        public double Height { get; }

        public Triangle(double baseLength, double height)
            : base("Hình tam giác")
        {
            if (baseLength <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(baseLength),
                    "Độ dài đáy phải lớn hơn 0.");
            }

            if (height <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(height),
                    "Chiều cao phải lớn hơn 0.");
            }

            BaseLength = baseLength;
            Height = height;
        }

        public override double CalculateArea() => 0.5 * BaseLength * Height;
    }
}
