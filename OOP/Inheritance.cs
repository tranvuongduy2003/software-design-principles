using System;

namespace SoftwareDesignPrinciples.OOP
{
    /// <summary>
    /// Minh họa nguyên lý Inheritance (tính kế thừa) trong lập trình hướng
    /// đối tượng và so sánh với Object Composition thông qua bài toán
    /// quản lý nhân sự.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Đặt vấn đề: Trong hệ thống quản lý nhân sự, mọi nhân viên đều có các
    /// thuộc tính cơ bản như tên, mức lương và hành vi làm việc (Work).
    /// Tuy nhiên, mỗi vị trí lại có trách nhiệm chuyên biệt: Manager đảm nhận
    /// quản lý nhóm, trong khi Developer thực hiện viết mã nguồn. Nếu lặp lại
    /// toàn bộ các trường dữ liệu, logic kiểm thực (validation) và hành vi
    /// chung ở từng lớp riêng biệt, mã nguồn sẽ bị trùng lặp nghiêm trọng
    /// (code duplication), gây khó khăn cho việc bảo trì và làm mất tính nhất
    /// quán của hệ thống.
    /// </para>
    /// <para>
    /// Khái niệm cốt lõi: Inheritance cho phép một derived class (lớp dẫn
    /// xuất / lớp con) kế thừa các trường, thuộc tính và phương thức có mức
    /// truy cập phù hợp từ base class (lớp cơ sở / lớp cha), đồng thời có thể
    /// bổ sung dữ liệu mới hoặc ghi đè (override) hành vi theo nhu cầu. Về mặt
    /// ngữ nghĩa, kế thừa bắt buộc phải thỏa mãn mối quan hệ "is-a" (là một
    /// loại): ví dụ, Manager là một Employee. Kế thừa vừa giúp tái sử dụng mã
    /// nguồn (code reuse), vừa thiết lập quan hệ phân cấp kiểu (subtyping).
    /// Trong đó, tính đúng đắn của phân cấp kiểu và khả năng thay thế
    /// (substitutability) luôn giữ vai trò tiên quyết, quan trọng hơn mục
    /// tiêu giảm mã lặp thuần túy.
    /// </para>
    /// <para>
    /// Cơ chế hiện thực trong C#:
    /// - Toán tử dấu hai chấm (:): Khai báo lớp cơ sở được kế thừa.
    /// - Từ khóa base(...): Gọi constructor của lớp cơ sở để khởi tạo trạng
    ///   thái cha (constructor chaining).
    /// - Từ khóa virtual: Đánh dấu phương thức tại lớp cơ sở cho phép lớp con
    ///   ghi đè (extension point).
    /// - Từ khóa override: Cung cấp cách hiện thực mới hoặc mở rộng phương
    ///   thức virtual từ lớp cơ sở.
    /// - Lệnh gọi base.Work(): Tái sử dụng logic sẵn có của lớp cơ sở bên trong
    ///   phương thức override của lớp con.
    /// - Từ khóa sealed: Đóng cây kế thừa, ngăn chặn các lớp khác tiếp tục
    ///   dẫn xuất từ lớp này.
    /// - Cơ chế đơn kế thừa (Single Inheritance): Mỗi lớp trong C# chỉ có duy
    ///   nhất một direct base class, nhưng có thể hiện thực (implement) đồng
    ///   thời nhiều interface khác nhau.
    /// </para>
    /// <para>
    /// Thứ tự khởi tạo và quy tắc Constructor:
    /// Khi khởi tạo một đối tượng Manager, constructor của Employee luôn được
    /// thực thi hoàn tất trước khi thân constructor của Manager chạy.
    /// Constructor không được kế thừa tự động; mỗi lớp dẫn xuất phải tường
    /// minh định nghĩa constructor riêng và ủy quyền cho base constructor phù
    /// hợp. Cảnh báo kiến trúc: Tuyệt đối tránh gọi virtual method trong
    /// constructor, bởi vì khi phương thức override thực thi, các trường dữ
    /// liệu và trạng thái nội tại của derived class có thể chưa được khởi tạo.
    /// </para>
    /// <para>
    /// Khả năng thay thế theo nguyên lý Liskov (LSP):
    /// Lớp dẫn xuất phải có khả năng thay thế hoàn toàn cho lớp cơ sở tại mọi
    /// vị trí mong đợi base type mà không làm thay đổi tính đúng đắn của
    /// chương trình. Lớp con không được thắt chặt tiền điều kiện
    /// (preconditions), không nới lỏng hậu điều kiện (postconditions), không
    /// tự ý vô hiệu hóa hành vi hợp lệ hay phá vỡ các bất biến (invariants)
    /// mà lớp cha đã xác lập. Đây là nền tảng cốt lõi của tính đa hình trong
    /// hệ thống kiểu hướng đối tượng.
    /// </para>
    /// <para>
    /// Khi nào nên áp dụng:
    /// Chỉ sử dụng kế thừa khi tồn tại mối quan hệ phân cấp tổng quát hóa -
    /// chuyên biệt hóa (generalization - specialization) thực sự rõ ràng, ổn
    /// định và lớp cơ sở được chủ đích thiết kế để mở rộng. Nếu quan hệ thực
    /// chất là "has-a" (có / chứa một) hoặc chỉ đơn thuần cần tái sử dụng
    /// một phần hành vi, hãy ưu tiên Object Composition để giảm mức độ phụ
    /// thuộc (tight coupling).
    /// </para>
    /// <para>
    /// Object Composition (Hợp thành đối tượng):
    /// Là kỹ thuật kiến trúc xây dựng một đối tượng phức hợp bằng cách ghép
    /// nối các đối tượng thành phần độc lập. Đối tượng chính nắm giữ tham
    /// chiếu đến component và ủy quyền (delegate) công việc cho component đó.
    /// Trong ví dụ, RoleBasedEmployee duy trì quan hệ "has-a" với giao diện
    /// IWorkRole và gọi Perform để hoàn thành công việc. Nhờ đó, vai trò làm
    /// việc có thể được hoán đổi linh hoạt ngay tại runtime mà không cần thay
    /// đổi kiểu lớp của nhân viên hay tạo ra sự bùng nổ các nhánh kế thừa.
    /// </para>
    /// <para>
    /// So sánh cốt lõi giữa Inheritance và Object Composition:
    /// - Inheritance: Biểu diễn quan hệ "is-a", liên kết chặt chẽ ngay từ
    ///   thời điểm biên dịch (compile-time), chia sẻ trực tiếp trạng thái và
    ///   mã triển khai từ lớp cha (white-box reuse), bị ràng buộc bởi luật đơn
    ///   kế thừa. Kế thừa tạo mức độ kết dính cao (tight coupling) và có nguy
    ///   cơ phá vỡ tính đóng gói nếu để lộ dữ liệu nội bộ cho lớp con.
    /// - Object Composition: Biểu diễn quan hệ "has-a", liên kết lỏng lẻo
    ///   thông qua cơ chế ủy quyền (delegation), cho phép thay đổi component
    ///   linh hoạt tại runtime (black-box reuse) và chỉ phụ thuộc vào
    ///   abstraction (interface).
    /// </para>
    /// <para>
    /// Phân biệt thuật ngữ quan hệ trong thiết kế phần mềm:
    /// - is-a: Biểu thị quan hệ kế thừa hoặc phân cấp kiểu (Inheritance /
    ///   Subtyping).
    /// - has-a: Biểu thị một đối tượng nắm giữ tham chiếu lâu dài đến đối
    ///   tượng khác (Association, Aggregation hoặc Composition).
    /// - uses-a: Biểu thị quan hệ phụ thuộc tạm thời (Dependency), trong đó
    ///   đối tượng chỉ tương tác với đối tượng khác thông qua tham số phương
    ///   thức, biến cục bộ hoặc lệnh gọi hàm nhất thời.
    /// </para>
    /// <para>
    /// Ghi chú về mô hình hóa UML:
    /// Trong đặc tả UML, Composition mang ngữ nghĩa hẹp là mối quan hệ
    /// whole-part với quyền sở hữu tuyệt đối (strong ownership): vòng đời của
    /// thành phần con (part) phụ thuộc hoàn toàn vào đối tượng chứa nó (whole).
    /// Trong khi đó, việc truyền IWorkRole từ bên ngoài vào (Dependency
    /// Injection) có thể thay thế độc lập minh họa cho nguyên lý Object
    /// Composition tổng quát trong lập trình; xét theo chuẩn UML, liên kết
    /// này tương đồng với Aggregation hoặc Association hơn là Composition.
    /// </para>
    /// <para>
    /// Thực tiễn kiến trúc hiện đại (Best Practices):
    /// Nguyên lý "Favor Composition over Inheritance" (Ưu tiên hợp thành hơn
    /// kế thừa) khuyến nghị luôn cân nhắc Composition trước tiên khi mục tiêu
    /// là tái sử dụng mã hoặc thay đổi hành vi. Cách tiếp cận này đem lại tính
    /// linh hoạt cao, thuận tiện viết Unit Test (dễ mock) và ngăn chặn bùng nổ
    /// cây kế thừa (class explosion). Inheritance vẫn là lựa chọn chính xác
    /// khi quan hệ phân loại mang tính bản chất, bền vững, lớp con bảo đảm
    /// khả năng thay thế hoàn toàn cho lớp cha và các điểm mở rộng được kiến
    /// trúc minh bạch (ví dụ: Template Method Pattern).
    /// </para>
    /// <para>
    /// Các lỗi thiết kế thường gặp (Code Smells &amp; Anti-patterns):
    /// - Lạm dụng kế thừa chỉ nhằm mục đích dùng lại vài dòng mã mà không thỏa
    ///   mãn ngữ nghĩa "is-a".
    /// - Cây kế thừa quá sâu (deep inheritance tree), dẫn đến vấn đề lớp cơ
    ///   sở mỏng manh (Fragile Base Class Problem).
    /// - Xâm phạm tính đóng gói: chuyển đổi private field thành protected để
    ///   lớp con can thiệp tùy tiện vào dữ liệu nội bộ.
    /// - Sử dụng từ khóa new (member hiding) thay vì override, làm vô hiệu hóa
    ///   cơ chế đa hình (polymorphism).
    /// - Vi phạm LSP: Lớp con ném NotSupportedException hoặc bỏ qua các phương
    ///   thức hợp lệ của lớp cha.
    /// - Lạm dụng kiểm tra kiểu thủ công (is, as) rải rác thay vì tận dụng
    ///   dynamic dispatch.
    /// </para>
    /// <para>
    /// Câu hỏi phỏng vấn kinh điển:
    /// 1. Constructor có được kế thừa trong C# không?
    ///    - Trả lời: Không. Constructor không bao giờ được kế thừa. Lớp dẫn
    ///      xuất phải tự định nghĩa constructor riêng và có thể gọi
    ///      constructor của lớp cha bằng cú pháp base(...). Base constructor
    ///      luôn được thực thi trước.
    ///    - Bản chất: Constructor chịu trách nhiệm thiết lập các bất biến
    ///      (invariants) cho chính kiểu dữ liệu khai báo nó (declaring type),
    ///      do đó mỗi lớp phải chủ động kiểm soát quy trình khởi tạo của mình.
    /// 2. Từ khóa override khác từ khóa new (member hiding) như thế nào?
    ///    - Trả lời: override tham gia vào cơ chế Dynamic Dispatch (phân phối
    ///      động tại runtime), phương thức được gọi sẽ dựa trên kiểu thực tế
    ///      của đối tượng tại thời điểm chạy. Trong khi đó, new chỉ ẩn thành
    ///      viên của lớp cha tại thời điểm biên dịch (compile-time resolution),
    ///      phương thức được gọi sẽ phụ thuộc vào kiểu của biến tham chiếu
    ///      (static type). Vì vậy, new không thể thay thế override trong thiết
    ///      kế đa hình.
    /// 3. Khi nào nên chọn Inheritance, khi nào nên chọn Composition?
    ///    - Trả lời: Ưu tiên Composition khi cần ghép nối, hoán đổi linh hoạt
    ///      hoặc kiểm thử hành vi độc lập tại runtime; sử dụng Inheritance khi
    ///      quan hệ "is-a" thực sự đúng đắn, mô hình phân cấp bền vững và lớp
    ///      dẫn xuất cam kết bảo toàn khả năng thay thế theo LSP.
    /// </para>
    /// </remarks>
    public static class InheritanceDemo
    {
        public static void Run()
        {
            Console.WriteLine("--- INHERITANCE (TÍNH KẾ THỪA) ---");

            // Bước 1: Khởi tạo đối tượng qua kế thừa - constructor của
            // Employee (base class) luôn thực thi trước constructor của
            // Manager (derived class).
            var manager = new Manager("Nguyễn An", 80_000m, 5);
            manager.Work();
            manager.ConductMeeting();

            Console.WriteLine();

            // Bước 2: Khởi tạo Developer - kế thừa thuộc tính chung và mở
            // rộng phương thức Work() bằng cách override.
            var developer = new Developer("Trần Bình", 60_000m, "C#");
            developer.Work();
            developer.WriteCode();

            Console.WriteLine();

            Console.WriteLine("--- OBJECT COMPOSITION ---");

            // Bước 3: Áp dụng Object Composition - đóng gói hành vi (behavior)
            // vào component IWorkRole thay vì gắn cứng vào cấu trúc lớp.
            var roleBasedEmployee = new RoleBasedEmployee(
                "Lê Chi",
                new DeveloperWorkRole("C#"));
            roleBasedEmployee.Work();

            // Bước 4: Hoán đổi component linh hoạt ngay tại runtime mà không
            // làm biến đổi định danh hay kiểu dữ liệu của đối tượng.
            roleBasedEmployee.ChangeRole(new ManagerWorkRole(3));
            roleBasedEmployee.Work();

            Console.WriteLine();
        }
    }

    // ========================================================================
    // INHERITANCE (IS-A): Manager và Developer kế thừa từ Employee.
    // Thể hiện mối quan hệ phân cấp kiểu (type hierarchy) và tái sử dụng mã.
    // ========================================================================

    /// <summary>
    /// Lớp cơ sở (Base Class) định nghĩa các thuộc tính, quy tắc toàn vẹn
    /// (invariants) và hành vi chung cho mọi nhân viên trong hệ thống.
    /// </summary>
    public class Employee
    {
        // private set bảo vệ tính đóng gói: chỉ nội bộ Employee mới có quyền
        // cập nhật mức lương.
        public string Name { get; }
        public decimal Salary { get; private set; }

        /// <summary>
        /// Khởi tạo trạng thái cơ sở của Employee; bắt buộc thực thi trước
        /// constructor của lớp dẫn xuất.
        /// </summary>
        public Employee(string name, decimal salary)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(
                    "Tên nhân viên không được để trống.",
                    nameof(name));
            }

            if (salary < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(salary),
                    "Mức lương không được âm.");
            }

            Name = name;
            Salary = salary;
            Console.WriteLine($"Khởi tạo phần Employee cho {Name}.");
        }

        /// <summary>
        /// Triển khai hành vi mặc định. Từ khóa 'virtual' thiết lập điểm mở
        /// rộng cho phép các lớp dẫn xuất ghi đè (override) khi cần thiết.
        /// </summary>
        public virtual void Work()
        {
            Console.WriteLine(
                $"{Name} thực hiện công việc chung của nhân viên.");
        }

        /// <summary>
        /// Nghiệp vụ tăng lương có kiểm thực tiền điều kiện, thay vì mở public
        /// setter gây rủi ro sai lệch dữ liệu nội bộ.
        /// </summary>
        public void GiveRaise(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(amount),
                    "Mức tăng lương phải lớn hơn 0.");
            }

            Salary += amount;
        }
    }

    /// <summary>
    /// Lớp dẫn xuất (Derived Class) kế thừa từ Employee, thể hiện mối quan hệ
    /// ngữ nghĩa 'is-a' (Manager là một Employee).
    /// </summary>
    public class Manager : Employee
    {
        public int TeamSize { get; }

        /// <summary>
        /// Khởi tạo Manager. Cú pháp : base(...) bảo đảm constructor lớp cha
        /// hoàn tất trước khi thân constructor của Manager được thực thi.
        /// </summary>
        public Manager(string name, decimal salary, int teamSize)
            : base(name, salary)
        {
            if (teamSize < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(teamSize),
                    "Quy mô nhóm không được âm.");
            }

            TeamSize = teamSize;
            Console.WriteLine($"Khởi tạo phần Manager cho {Name}.");
        }

        /// <summary>
        /// Override phương thức Work để thay thế hành vi mặc định bằng nghiệp
        /// vụ quản lý đặc thù.
        /// </summary>
        public override void Work()
        {
            Console.WriteLine($"{Name} quản lý một nhóm gồm {TeamSize} người.");
        }

        /// <summary>
        /// Hành vi chuyên biệt của Manager, không tồn tại ở lớp cơ sở Employee.
        /// </summary>
        public void ConductMeeting()
        {
            Console.WriteLine($"{Name} điều hành cuộc họp nhóm.");
        }
    }

    /// <summary>
    /// Lớp dẫn xuất từ Employee. Từ khóa 'sealed' ngăn chặn việc tiếp tục kế
    /// thừa, cố định hành vi và đóng cây kế thừa tại lớp Developer.
    /// </summary>
    public sealed class Developer : Employee
    {
        public string ProgrammingLanguage { get; }

        /// <summary>
        /// Khởi tạo Developer: ủy quyền khởi tạo thông tin chung cho base
        /// class trước, sau đó kiểm thực và gán trạng thái riêng của Developer.
        /// </summary>
        public Developer(
            string name,
            decimal salary,
            string programmingLanguage)
            : base(name, salary)
        {
            if (string.IsNullOrWhiteSpace(programmingLanguage))
            {
                throw new ArgumentException(
                    "Ngôn ngữ lập trình không được để trống.",
                    nameof(programmingLanguage));
            }

            ProgrammingLanguage = programmingLanguage;
            Console.WriteLine($"Khởi tạo phần Developer cho {Name}.");
        }

        /// <summary>
        /// Override phương thức Work: vừa tái sử dụng logic gốc qua
        /// 'base.Work()', vừa mở rộng thêm hành vi lập trình chuyên biệt.
        /// </summary>
        public override void Work()
        {
            // Tái sử dụng triển khai của base class trước khi thực thi nghiệp
            // vụ mở rộng.
            base.Work();
            Console.WriteLine(
                $"{Name} phát triển phần mềm bằng {ProgrammingLanguage}.");
        }

        /// <summary>
        /// Hành vi chuyên biệt của Developer, không tồn tại ở lớp Employee.
        /// </summary>
        public void WriteCode()
        {
            Console.WriteLine($"{Name} đang viết mã nguồn.");
        }
    }

    // ========================================================================
    // OBJECT COMPOSITION (HAS-A): RoleBasedEmployee ủy quyền cho component
    // IWorkRole thay vì kế thừa hành vi từ nhiều lớp con khác nhau.
    // ========================================================================

    /// <summary>
    /// Abstraction định nghĩa năng lực (capability) thực thi một vai trò công
    /// việc cụ thể.
    /// </summary>
    public interface IWorkRole
    {
        void Perform(string employeeName);
    }

    /// <summary>
    /// Component độc lập hiện thực hóa vai trò phát triển phần mềm trong mô
    /// hình Composition.
    /// </summary>
    public sealed class DeveloperWorkRole : IWorkRole
    {
        public string ProgrammingLanguage { get; }

        public DeveloperWorkRole(string programmingLanguage)
        {
            if (string.IsNullOrWhiteSpace(programmingLanguage))
            {
                throw new ArgumentException(
                    "Ngôn ngữ lập trình không được để trống.",
                    nameof(programmingLanguage));
            }

            ProgrammingLanguage = programmingLanguage;
        }

        public void Perform(string employeeName)
        {
            Console.WriteLine(
                $"{employeeName} phát triển phần mềm bằng " +
                $"{ProgrammingLanguage}.");
        }
    }

    /// <summary>
    /// Component độc lập hiện thực hóa vai trò quản lý nhóm trong mô hình
    /// Composition.
    /// </summary>
    public sealed class ManagerWorkRole : IWorkRole
    {
        public int TeamSize { get; }

        public ManagerWorkRole(int teamSize)
        {
            if (teamSize < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(teamSize),
                    "Quy mô nhóm không được âm.");
            }

            TeamSize = teamSize;
        }

        public void Perform(string employeeName)
        {
            Console.WriteLine(
                $"{employeeName} quản lý một nhóm gồm {TeamSize} người.");
        }
    }

    /// <summary>
    /// Minh họa Object Composition: RoleBasedEmployee thiết lập quan hệ
    /// 'has-a' (sở hữu/chứa đựng) với IWorkRole.
    /// </summary>
    /// <remarks>
    /// Khác với cấu trúc kế thừa tĩnh (compile-time), IWorkRole được ủy quyền
    /// xử lý và có thể thay thế linh hoạt tại runtime. Thiết kế này giúp đối
    /// tượng thích ứng nhanh với sự thay đổi vai trò mà không cần tạo mới
    /// nhân viên hay mở rộng cây kế thừa phức tạp.
    /// </remarks>
    public sealed class RoleBasedEmployee
    {
        private IWorkRole _workRole;

        public string Name { get; }

        public RoleBasedEmployee(string name, IWorkRole workRole)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(
                    "Tên nhân viên không được để trống.",
                    nameof(name));
            }

            ArgumentNullException.ThrowIfNull(workRole);

            Name = name;
            _workRole = workRole;
        }

        /// <summary>
        /// Hoán đổi component vai trò linh hoạt ngay tại runtime mà không làm
        /// biến đổi kiểu của đối tượng.
        /// </summary>
        public void ChangeRole(IWorkRole workRole)
        {
            ArgumentNullException.ThrowIfNull(workRole);
            _workRole = workRole;
        }

        /// <summary>
        /// Ủy quyền (delegate) xử lý công việc cho component IWorkRole
        /// hiện tại.
        /// </summary>
        public void Work()
        {
            _workRole.Perform(Name);
        }
    }
}
