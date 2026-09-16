using System;

namespace SoftwareDesignPrinciples.DependencyInjection
{
    /*
     * BÀI 2. TRANSIENT LIFETIME
     *
     * 1. Nhu cầu
     * -----------
     * Trong kiến trúc phần mềm, nhiều service chỉ thực hiện các tác vụ xử lý ngắn hạn, phi trạng
     * thái (stateless) và có chi phí khởi tạo rất thấp. Việc tái sử dụng các service này không đem
     * lại lợi ích về mặt hiệu năng, thậm chí còn tiềm ẩn nguy cơ làm rò rỉ hoặc ô nhiễm trạng thái
     * (state pollution) giữa các lần xử lý khác nhau. Trong các trường hợp đó, mỗi khi có yêu cầu
     * phân giải (resolve), consumer nên nhận về một instance hoàn toàn độc lập.
     *
     * 2. Định nghĩa và bản chất
     * -------------------------
     * Transient lifetime (vòng đời tạm thời) là chính sách vòng đời mà trong đó container luôn
     * khởi tạo một instance mới ở mỗi lần service được resolve (phân giải). Khái niệm “mỗi lần” ở
     * đây được tính trên từng yêu cầu gửi tới container, bất kể lời gọi diễn ra tại root provider
     * hay bên trong một scope con.
     *
     * Cú pháp đăng ký tương ứng trong container mẫu:
     *
     *     services.AddTransient<ITransientService, TransientService>();
     *
     * Dòng lệnh trên đăng ký một ServiceDescriptor, ánh xạ abstraction ITransientService sang
     * implementation TransientService. Thao tác này mới chỉ khai báo cấu hình chứ chưa hề khởi tạo
     * đối tượng. Instance chỉ thực sự được tạo khi container tiến hành phân giải dependency (ví dụ
     * khi gọi GetRequiredService).
     *
     * 3. Cơ chế
     * ----------
     * Bên trong SimpleServiceProvider.Resolve, nhánh xử lý Transient luôn gọi trực tiếp CreateInstance
     * mà không lưu kết quả vào bất kỳ bộ nhớ đệm (cache) nào. Vì vậy, hai lần resolve liên tiếp sẽ
     * luôn kích hoạt constructor hai lần độc lập. Tương tự, nếu một consumer yêu cầu hai tham số có
     * cùng kiểu service trong constructor, container cũng sẽ resolve riêng rẽ từng tham số và cung cấp
     * hai instance tách biệt.
     *
     * 4. Trường hợp áp dụng
     * ---------------------
     * Phù hợp với: Các service phi trạng thái (stateless), chi phí khởi tạo nhẹ như bộ định dạng
     * (formatter), bộ chuyển đổi dữ liệu (mapper), bộ kiểm tra tính hợp lệ (validator), hoặc các
     * service xử lý nghiệp vụ độc lập không lưu vết trạng thái giữa các lần gọi.
     * 
     * Không nên dùng cho: Các đối tượng có chi phí cấp phát tốn kém (heavyweight objects), kết nối
     * cơ sở dữ liệu (Database Connection), hoặc các service cần chia sẻ chung một transaction/ngữ
     * cảnh xuyên suốt vòng đời của request (những trường hợp này nên ưu tiên Scoped hoặc Singleton).
     *
     * 5. Lỗi thường gặp
     * -----------------
     * - Lầm tưởng về thời điểm giải phóng đối tượng: Cho rằng "transient" đồng nghĩa với việc đối
     *   tượng sẽ tự động bị hủy ngay khi phương thức kết thúc. Thực tế, DI lifetime chỉ quyết định
     *   chiến lược khởi tạo và tái sử dụng instance của container. Thời điểm thu hồi bộ nhớ thực tế
     *   vẫn phụ thuộc vào việc còn tham chiếu hay không, cơ chế dọn rác của Garbage Collector (GC)
     *   và việc kích hoạt IDisposable.
     * - Captive Dependency (Phụ thuộc bị giam cầm): Tiêm trực tiếp một transient service vào một
     *   singleton service. Khi đó, instance transient sẽ bị singleton giữ tham chiếu suốt vòng đời
     *   ứng dụng, vô tình biến nó thành một singleton ngoài ý muốn và có thể gây lỗi trạng thái dữ liệu.
     * - Lãng phí tài nguyên và gây áp lực lên GC: Đăng ký Transient cho các đối tượng có chi phí tạo
     *   đắt đỏ hoặc quản lý tài nguyên unmanaged, dẫn đến việc cấp phát bộ nhớ liên tục không cần thiết.
     *   Ngoài ra, cần lưu ý container mẫu này cũng chưa tự động theo dõi và Dispose các transient instance.
     */

    /// <summary>
    /// Minh họa 3 kịch bản kiểm chứng cơ chế khởi tạo mới của Transient lifetime.
    /// </summary>
    public static class TransientDemo
    {
        public static void Run()
        {
            Console.WriteLine("\n=== TRANSIENT: Mỗi lần phân giải (resolve) đều khởi tạo một instance mới ===\n");

            var services = new SimpleServiceCollection();

            // Đăng ký ServiceDescriptor vào container; constructor chưa được thực thi ở giai đoạn này.
            services.AddTransient<ITransientService, TransientService>();
            services.AddTransient<OrderWorkflowConsumer, OrderWorkflowConsumer>();

            using var provider = services.BuildServiceProvider();

            // Kịch bản 1: Hai lần resolve cùng một service từ cùng một provider sẽ tạo ra hai instance độc lập với OperationId khác nhau.
            Console.WriteLine("--- Kịch bản 1: Hai lần phân giải liên tiếp từ cùng một provider ---");
            var first = provider.GetRequiredService<ITransientService>();
            var second = provider.GetRequiredService<ITransientService>();

            Console.WriteLine($"  ID lần 1: {first.OperationId}");
            Console.WriteLine($"  ID lần 2: {second.OperationId}");
            Console.WriteLine(
                $"  Cùng instance? {ReferenceEquals(first, second)} (kỳ vọng: False)\n");

            // Kịch bản 2: Container resolve độc lập từng tham số trong constructor.
            // OrderWorkflowConsumer yêu cầu 2 dependency kiểu ITransientService nên sẽ nhận về 2 instance riêng biệt.
            Console.WriteLine("--- Kịch bản 2: Inject hai dependency cùng kiểu vào một consumer ---");
            var consumer = provider.GetRequiredService<OrderWorkflowConsumer>();
            consumer.Execute();

            // Kịch bản 3: Scope không làm thay đổi hành vi Transient vì container không cache instance loại này.
            Console.WriteLine("\n--- Kịch bản 3: Hai lần phân giải trong cùng một scope ---");
            using var scope = provider.CreateScope();
            var third = scope.ServiceProvider.GetRequiredService<ITransientService>();
            var fourth = scope.ServiceProvider.GetRequiredService<ITransientService>();
            Console.WriteLine(
                $"  Cùng instance? {ReferenceEquals(third, fourth)} (kỳ vọng: False)");

            Console.WriteLine(
                "\nKết luận: Transient luôn tạo mới instance ở mỗi lần phân giải, bất kể trong root provider hay trong cùng một scope.");
        }
    }

    /// <summary>
    /// Abstraction định nghĩa hợp đồng thao tác cho transient service.
    /// </summary>
    /// <remarks>
    /// Consumer chỉ phụ thuộc vào abstraction (interface) thay vì implementation cụ thể, tuân thủ nguyên lý Dependency Inversion (DIP).
    /// Việc container tự động tiêm đối tượng cài đặt phù hợp vào consumer chính là kỹ thuật Dependency Injection (DI).
    /// </remarks>
    public interface ITransientService
    {
        Guid OperationId { get; }

        void Execute(string caller);
    }

    /// <summary>
    /// Implementation cụ thể, xử lý nhẹ và hoàn toàn phi trạng thái (stateless).
    /// </summary>
    public sealed class TransientService : ITransientService
    {
        // Sử dụng Guid làm định danh duy nhất để quan sát mỗi lần instance được tạo mới.
        public Guid OperationId { get; } = Guid.NewGuid();

        public TransientService()
            => Console.WriteLine(
                $"  [NEW] Đã tạo TransientService - ID: {OperationId}");

        public void Execute(string caller)
            => Console.WriteLine($"  [{caller}] sử dụng ID: {OperationId}");
    }

    /// <summary>
    /// Minh họa Constructor Injection khi một consumer phụ thuộc vào nhiều tham số có cùng abstraction.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Sử dụng Primary Constructor của C# để tiếp nhận các dependency. Lớp consumer hoàn toàn không gọi <c>new</c>
    /// và không cần biết chi tiết cách <see cref="TransientService"/> được khởi tạo.
    /// </para>
    /// <para>
    /// Quá trình phân giải: Khi khởi tạo, container phát hiện constructor có hai tham số kiểu <see cref="ITransientService"/>;
    /// nó sẽ resolve độc lập tham số thứ nhất, tiếp tục resolve tham số thứ hai, rồi mới tạo instance cho <see cref="OrderWorkflowConsumer"/>.
    /// Vì service có lifetime là Transient, hai tham số sẽ nhận về hai instance hoàn toàn riêng biệt.
    /// </para>
    /// </remarks>
    public sealed class OrderWorkflowConsumer(
        ITransientService validator,
        ITransientService calculator)
    {
        public void Execute()
        {
            validator.Execute("Validator");
            calculator.Execute("Calculator");
            Console.WriteLine(
                $"  Cùng instance? {ReferenceEquals(validator, calculator)} " +
                "(kỳ vọng: False)");
        }
    }
}
