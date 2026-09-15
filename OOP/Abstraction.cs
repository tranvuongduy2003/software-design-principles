using System;
using System.Collections.Generic;

namespace SoftwareDesignPrinciples.OOP
{
    /// <summary>
    /// Minh họa nguyên lý Abstraction (tính trừu tượng) thông qua bài toán
    /// điều khiển và vận hành phương tiện.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Đặt vấn đề: Phía điều khiển (client code) chỉ quan tâm đến các thao
    /// tác cốt lõi: khởi động, di chuyển và dừng xe. Phía gọi không cần biết
    /// ô tô phát ra âm thanh gì, mô tô truyền lực ra sao hay trạng thái
    /// động cơ được lưu trữ bằng biến nội bộ nào. Nếu các chi tiết cài đặt
    /// (implementation details) này bị phơi bày ra ngoài, client code sẽ
    /// bị gắn kết chặt chẽ (tight coupling) và mỗi khi bổ sung một loại xe
    /// mới, hệ thống sẽ trở nên phức tạp và khó mở rộng.
    /// </para>
    /// <para>
    /// Định nghĩa: Abstraction là quá trình chắt lọc và tập trung vào các
    /// đặc tính, hành vi cốt lõi của đối tượng trong ngữ cảnh bài toán,
    /// đồng thời ẩn đi những chi tiết kỹ thuật phức tạp không cần thiết đối
    /// với phía gọi. Abstraction định nghĩa hợp đồng công khai - "Đối tượng
    /// làm được gì?" (WHAT), trong khi Encapsulation (tính đóng gói) kiểm
    /// soát và bảo vệ dữ liệu nội tại cùng cách thức hoạt động bên trong -
    /// "Đối tượng làm việc đó như thế nào?" (HOW).
    /// </para>
    /// <para>
    /// Vai trò của các thành phần:
    /// - Interface IVehicle: Đóng vai trò là bản hợp đồng (contract), định
    ///   nghĩa tập hợp các thao tác (operations) công khai mà client code
    ///   được phép tương tác.
    /// - AbstractVehicle: Là lớp trừu tượng (abstract class) không thể khởi
    ///   tạo trực tiếp, dùng làm nền tảng để lưu giữ trạng thái chung (shared
    ///   state), cung cấp constructor kiểm thực dữ liệu và chứa các logic
    ///   dùng chung (shared implementation).
    /// - Abstract method: Đóng vai trò đặc tả khuôn mẫu hành vi; các lớp
    ///   cụ thể (concrete classes) bắt buộc phải ghi đè (override) để hiện
    ///   thực hóa chi tiết phù hợp với từng loại xe.
    /// </para>
    /// <para>
    /// Lựa chọn giữa Interface và Abstract Class:
    /// - Chọn Interface: Khi cần định nghĩa năng lực (capability/contract)
    ///   cho nhiều kiểu dữ liệu khác nhau, kể cả các lớp không thuộc cùng
    ///   một cây kế thừa (inheritance tree), hoặc khi muốn tách biệt hoàn
    ///   toàn client code khỏi chi tiết cài đặt (loose coupling).
    /// - Chọn Abstract Class: Khi các lớp có mối quan hệ bản chất mật thiết
    ///   ("is-a", cùng một họ/family), cần chia sẻ trạng thái nội tại, logic
    ///   khởi tạo hoặc quy trình xử lý dùng chung (shared workflow).
    /// - Trong C#, một class chỉ có thể kế thừa duy nhất một lớp cha (single
    ///   inheritance) nhưng có thể triển khai nhiều interface. Hai cơ chế này
    ///   bổ trợ cho nhau và thường được kết hợp nhịp nhàng như trong ví dụ.
    /// </para>
    /// <para>
    /// Luồng thực thi ví dụ: Danh sách phương tiện chỉ phơi bày kiểu hợp đồng
    /// IVehicle; AbstractVehicle quản lý trạng thái động cơ chung; Car và
    /// Motorcycle ghi đè Move và PlayEngineSound để thể hiện hành vi riêng.
    /// Vòng lặp duyệt danh sách hoàn toàn không cần kiểm tra kiểu cụ thể
    /// (concrete type checking), giúp client code tuân thủ nguyên lý Mở/Đóng
    /// (Open/Closed Principle) - sẵn sàng đón nhận loại xe mới mà không cần
    /// sửa đổi mã nguồn hiện có.
    /// </para>
    /// <para>
    /// Ứng dụng thực tế: Thường áp dụng tại các ranh giới kiến trúc
    /// (architectural boundaries) như giữa tầng nghiệp vụ (business logic)
    /// với cơ sở dữ liệu, cổng thanh toán, dịch vụ gửi thông báo hoặc thư
    /// viện bên thứ ba. Lưu ý kiến trúc: Chỉ tạo abstraction khi thực sự
    /// có nhu cầu cô lập phụ thuộc hoặc có điểm biến đổi (variation point)
    /// rõ ràng. Việc lạm dụng trừu tượng hóa (over-abstraction) - ví dụ tạo
    /// interface 1-1 cho mọi class - chỉ làm tăng số lượng kiểu dữ liệu và
    /// mức độ gián tiếp (indirection) mà không mang lại giá trị giảm coupling.
    /// </para>
    /// <para>
    /// Lỗi thiết kế thường gặp (Code Smells &amp; Anti-patterns):
    /// - Interface cồng kềnh (Fat Interface): Ép các lớp triển khai phải cài
    ///   đặt phương thức rỗng hoặc ném NotImplementedException.
    /// - Đặt tên chung chung, mơ hồ (như Process, Manager, Handler): Không
    ///   thể hiện rõ trách nhiệm (responsibility) nghiệp vụ.
    /// - Để rò rỉ kiểu dữ liệu tầng hạ tầng (infrastructure types) vào
    ///   interface trừu tượng của tầng nghiệp vụ.
    /// - Lớp trừu tượng chứa quá nhiều trạng thái dùng chung (protected state),
    ///   gây khó khăn cho việc bảo trì lớp con.
    /// - Tạo ra quá nhiều tầng trung gian (premature abstraction) trước khi
    ///   xác định rõ các điểm biến đổi thực sự.
    /// </para>
    /// <para>
    /// Câu hỏi phỏng vấn kinh điển: "Khi nào nên dùng Interface, khi nào nên
    /// dùng Abstract Class?"
    /// - Trả lời: Dùng interface để mô tả năng lực hành vi ("can-do") cho
    ///   các đối tượng đa dạng, hỗ trợ đa kế thừa và phân tách phụ thuộc
    ///   tối đa. Dùng abstract class khi các đối tượng có cùng bản chất cốt
    ///   lõi ("is-a"), cần chia sẻ cấu trúc trạng thái, logic khởi tạo hoặc
    ///   tái sử dụng quy trình xử lý chung (shared workflow).
    /// - Góc nhìn kiến trúc: Quyết định lựa chọn phải dựa trên bản chất mô
    ///   hình hóa miền nghiệp vụ (domain modeling) và mức độ phụ thuộc
    ///   (coupling), không đơn thuần dựa trên sự khác biệt về cú pháp.
    /// </para>
    /// </remarks>
    public static class AbstractionDemo
    {
        public static void Run()
        {
            Console.WriteLine("--- ABSTRACTION (TÍNH TRỪU TƯỢNG) ---");

            // Bước 1: Static type là IVehicle (hợp đồng trừu tượng);
            // runtime type là Car hoặc Motorcycle (kiểu cụ thể).
            IReadOnlyList<IVehicle> vehicles = new IVehicle[]
            {
                new Car("Toyota Camry"),
                new Motorcycle("Yamaha R1")
            };

            // Bước 2: Client code chỉ tương tác thông qua các thành viên
            // do hợp đồng IVehicle công bố.
            foreach (var vehicle in vehicles)
            {
                Console.WriteLine($"Vận hành {vehicle.Model}:");
                vehicle.StartEngine();
                vehicle.Move();
                vehicle.StopEngine();
                Console.WriteLine();
            }
        }
    }

    /// <summary>
    /// Interface định nghĩa hợp đồng (contract) tối thiểu cho một phương
    /// tiện có động cơ.
    /// </summary>
    /// <remarks>
    /// Thiết kế theo ISP (Interface Segregation Principle): Nếu sau này cần
    /// hỗ trợ thêm phương tiện không động cơ (như xe đạp), ta nên tách năng
    /// lực di chuyển và năng lực dùng động cơ thành các interface riêng biệt;
    /// tránh ép xe đạp phải cài đặt các phương thức động cơ rỗng.
    /// </remarks>
    public interface IVehicle
    {
        string Model { get; }
        void StartEngine();
        void StopEngine();
        void Move();
    }

    /// <summary>
    /// Lớp trừu tượng (abstract class) quản lý trạng thái và cung cấp các
    /// hành vi dùng chung cho các loại phương tiện.
    /// </summary>
    /// <remarks>
    /// Trạng thái IsEngineRunning chỉ cho phép các lớp con (derived classes)
    /// đọc; việc thay đổi trạng thái hoàn toàn do lớp cha kiểm soát nhằm
    /// bảo vệ tính toàn vẹn (invariants) của động cơ.
    /// </remarks>
    public abstract class AbstractVehicle : IVehicle
    {
        // Trạng thái được thiết lập khi khởi tạo và ở trạng thái chỉ đọc
        // (read-only) trong suốt vòng đời của đối tượng.
        public string Model { get; }
        protected bool IsEngineRunning { get; private set; }

        /// <summary>
        /// Constructor của lớp cha: Khởi tạo và kiểm thực tính hợp lệ của
        /// trạng thái dùng chung trước khi lớp con hoàn tất khởi tạo.
        /// </summary>
        protected AbstractVehicle(string model)
        {
            if (string.IsNullOrWhiteSpace(model))
            {
                throw new ArgumentException(
                    "Tên mẫu xe không được để trống.",
                    nameof(model));
            }

            Model = model;
        }

        /// <summary>
        /// Cung cấp quy trình khởi động dùng chung (Template Workflow), đồng
        /// thời kích hoạt điểm biến đổi (variation point) PlayEngineSound.
        /// </summary>
        public void StartEngine()
        {
            if (IsEngineRunning)
            {
                Console.WriteLine($"Động cơ {Model} đã hoạt động.");
                return;
            }

            IsEngineRunning = true;
            Console.WriteLine($"Khởi động động cơ {Model}.");
            PlayEngineSound();
        }

        /// <summary>
        /// Đưa trạng thái động cơ về trạng thái tắt/dừng.
        /// </summary>
        public void StopEngine()
        {
            IsEngineRunning = false;
            Console.WriteLine($"Tắt động cơ {Model}.");
        }

        // Điểm biến đổi (variation point): Lớp cha xác định phương tiện phải
        // di chuyển được, nhưng nhường chi tiết thực thi cho từng loại xe.
        public abstract void Move();

        // Thành viên nội bộ: Chỉ dành cho cây kế thừa hiện thực hóa,
        // không phơi bày ra public API của client.
        protected abstract void PlayEngineSound();

        /// <summary>
        /// Phương thức bảo vệ (guard method): Đảm bảo quy tắc nghiệp vụ -
        /// chỉ cho phép di chuyển khi động cơ đang hoạt động.
        /// </summary>
        protected void EnsureEngineIsRunning()
        {
            if (!IsEngineRunning)
            {
                throw new InvalidOperationException(
                    $"Không thể di chuyển {Model} khi động cơ đang tắt.");
            }
        }
    }

    /// <summary>
    /// Lớp cụ thể (concrete class) cho xe ô tô; sử dụng từ khóa sealed để
    /// ngăn ngừa việc mở rộng kế thừa không mong muốn.
    /// </summary>
    public sealed class Car : AbstractVehicle
    {
        public Car(string model) : base(model)
        {
        }

        /// <summary>
        /// Hiện thực hóa chi tiết cách thức di chuyển đặc thù của xe ô tô
        /// (bằng 4 bánh).
        /// </summary>
        public override void Move()
        {
            EnsureEngineIsRunning();
            Console.WriteLine($"{Model} di chuyển bằng bốn bánh.");
        }

        protected override void PlayEngineSound()
        {
            Console.WriteLine("Âm thanh động cơ ô tô: vroom!");
        }
    }

    /// <summary>
    /// Lớp cụ thể (concrete class) cho xe mô tô; sử dụng từ khóa sealed để
    /// ngăn ngừa việc kế thừa tiếp.
    /// </summary>
    public sealed class Motorcycle : AbstractVehicle
    {
        public Motorcycle(string model) : base(model)
        {
        }

        /// <summary>
        /// Hiện thực hóa chi tiết cách thức di chuyển đặc thù của xe mô tô
        /// (bằng 2 bánh).
        /// </summary>
        public override void Move()
        {
            EnsureEngineIsRunning();
            Console.WriteLine($"{Model} di chuyển bằng hai bánh.");
        }

        protected override void PlayEngineSound()
        {
            Console.WriteLine("Âm thanh động cơ mô tô: brap!");
        }
    }
}
