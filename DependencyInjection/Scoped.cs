using System;

namespace SoftwareDesignPrinciples.DependencyInjection
{
    /*
     * BÀI 3. SCOPED LIFETIME
     *
     * 1. Nhu cầu thực tế (Bài toán đặt ra)
     * ------------------------------------
     * Trong một luồng xử lý nghiệp vụ hoặc một HTTP request, quy trình thường đi qua nhiều service
     * khác nhau (ví dụ: Controller -> Service -> Repository). Các thành phần này cần dùng chung một
     * ngữ cảnh (context), điển hình là phiên làm việc cơ sở dữ liệu (DbContext, database session)
     * hoặc một transaction duy nhất.
     *
     * - Nếu dùng Transient (tạo mới mỗi lần resolve): Mỗi service sẽ nhận một context/session riêng biệt,
     *   khiến các thao tác bị phân mảnh, không thể đồng bộ dữ liệu và phá vỡ mô hình Unit of Work.
     * - Nếu dùng Singleton (dùng chung một instance duy nhất): Toàn bộ các request độc lập chạy đồng thời
     *   sẽ tranh chấp cùng một context, dẫn đến xung đột dữ liệu, lỗi race condition và rò rỉ trạng thái.
     *
     * -> Khái niệm Scope (phạm vi) ra đời nhằm giải quyết vấn đề này bằng cách thiết lập một ranh giới
     *    vòng đời (lifecycle boundary). Trong ứng dụng web, mỗi HTTP request thường tương ứng với một
     *    scope riêng. Đối với tác vụ nền (background job/queue consumer), lập trình viên sẽ chủ động mở
     *    một scope riêng cho từng đơn vị công việc cần xử lý.
     *
     * 2. Định nghĩa và bản chất
     * -------------------------
     * Scoped lifetime quy định: trong phạm vi của một scope, container chỉ tạo tối đa một instance
     * duy nhất cho mỗi service được đăng ký.
     * - Mọi lần resolve service trong cùng một scope đều nhận về cùng một instance tham chiếu.
     * - Các scope khác nhau sẽ nhận các instance hoàn toàn độc lập và tách biệt.
     * - Khi một scope kết thúc (bị dispose), container sẽ tự động dọn dẹp và giải phóng (Dispose)
     *   toàn bộ các scoped service có triển khai IDisposable thuộc scope đó.
     *
     * Cú pháp đăng ký:
     *
     *     services.AddScoped<IScopedDatabaseSession, ScopedDatabaseSession>();
     *
     * 3. Cơ chế hoạt động
     * -------------------
     * Mỗi SimpleServiceProvider con (đại diện cho một scope) sở hữu một bộ nhớ đệm `_scopedCache` riêng:
     * - Lần resolve đầu tiên trong scope: Cache miss -> Container khởi tạo instance, lưu vào `_scopedCache`
     *   và đăng ký theo dõi giải phóng tài nguyên.
     * - Các lần resolve tiếp theo trong cùng scope: Cache hit -> Container trả về ngay instance đã lưu.
     * - Khởi tạo scope mới đồng nghĩa với việc tạo một cache mới độc lập; khi dispose scope, container
     *   sẽ tự động giải phóng toàn bộ tài nguyên/service mà scope đó theo dõi.
     *
     * 4. Trường hợp áp dụng
     * ---------------------
     * - Rất phù hợp cho: DbContext (như Entity Framework Core), Unit of Work, các Repository có trạng thái
     *   theo request, hoặc các service lưu giữ thông tin ngữ cảnh người dùng hiện tại (UserContext, TenantContext).
     * - Lưu ý về tính an toàn đa luồng (Thread-safety): Đối tượng scoped mặc định KHÔNG đảm bảo thread-safe.
     *   Tránh chia sẻ chung một scope cho nhiều luồng (threads) chạy song song nếu các service bên trong
     *   không hỗ trợ truy cập đồng thời (ví dụ kinh điển: DbContext sẽ ném ngoại lệ nếu bị gọi đồng thời).
     *
     * 5. Lỗi thường gặp (Common Pitfalls & Anti-patterns)
     * --------------------------------------------------
     * - Resolve Scoped service từ Root Provider: Khiến instance bị giữ lại suốt vòng đời ứng dụng (như Singleton),
     *   dễ dẫn đến rò rỉ bộ nhớ (Memory Leak) và sai lệch trạng thái. Trên .NET, hãy bật ValidateScopes để
     *   container tự động kiểm tra và ngăn chặn lỗi này.
     * - Captive Dependency (Tiêm Scoped vào Singleton): Singleton tồn tại xuyên suốt ứng dụng nên sẽ giữ chặt
     *   instance scoped đầu tiên được tiêm vào, khiến ranh giới của scope bị vô hiệu hóa hoàn toàn và có nguy
     *   cơ tiếp tục gọi vào một đối tượng đã bị dispose.
     * - Quên giải phóng Scope (Missing Dispose): Thường gặp khi tạo scope thủ công trong tác vụ nền mà không
     *   dùng khối `using`. Hậu quả là kết nối database, file stream hoặc unmanaged resources bị rò rỉ.
     * - Rò rỉ tham chiếu ra ngoài scope (Scope Escape): Lưu trữ hoặc chuyển tiếp service scoped ra ngoài scope
     *   của nó rồi tiếp tục gọi phương thức khi scope đã bị đóng, dẫn đến ngoại lệ ObjectDisposedException.
     */

    /// <summary>
    /// Chạy demo mô phỏng hai scope độc lập để kiểm chứng quy tắc:
    /// "Cùng scope nhận cùng một instance, khác scope nhận instance riêng biệt".
    /// </summary>
    public static class ScopedDemo
    {
        public static void Run()
        {
            Console.WriteLine("\n=== SCOPED: Một instance duy nhất dùng chung trong cùng một phạm vi (Scope) ===\n");

            var services = new SimpleServiceCollection();
            services.AddScoped<IScopedDatabaseSession, ScopedDatabaseSession>();
            services.AddScoped<OrderProcessingHandler, OrderProcessingHandler>();

            using var provider = services.BuildServiceProvider();

            // Scope 1: Mô phỏng HTTP Request thứ nhất. Hai lần resolve session liên tiếp đều lấy từ cùng một cache.
            Console.WriteLine("--- Scope 1 (Request 1) ---");
            using (var firstScope = provider.CreateScope())
            {
                var firstSession = firstScope.ServiceProvider
                    .GetRequiredService<IScopedDatabaseSession>();
                var secondSession = firstScope.ServiceProvider
                    .GetRequiredService<IScopedDatabaseSession>();

                firstSession.Execute("SELECT * FROM Users");
                secondSession.Execute("SELECT * FROM Orders");

                Console.WriteLine(
                    $"  Cùng instance? {ReferenceEquals(firstSession, secondSession)} " +
                    "(kỳ vọng: True)");

                // Handler được resolve sau đó nhưng vẫn nhận đúng session đang nằm trong cache của Scope 1.
                var handler = firstScope.ServiceProvider
                    .GetRequiredService<OrderProcessingHandler>();
                handler.HandleOrder();
            }
            // Ra khỏi khối using: firstScope.Dispose() được gọi, tự động giải phóng ScopedDatabaseSession của Scope 1.

            // Scope 2: Mô phỏng HTTP Request thứ hai. Do sở hữu cache độc lập, lần resolve đầu tiên tạo session mới hoàn toàn.
            Console.WriteLine("\n--- Scope 2 (Request 2) ---");
            using (var secondScope = provider.CreateScope())
            {
                var thirdSession = secondScope.ServiceProvider
                    .GetRequiredService<IScopedDatabaseSession>();
                thirdSession.Execute("SELECT * FROM Products");
                Console.WriteLine("  Instance này hoàn toàn độc lập với instance của Scope 1.");
            }

            Console.WriteLine(
                "\n[Kết luận] Cùng scope dùng chung một instance duy nhất; sang scope khác sẽ tạo mới; " +
                "khi scope kết thúc toàn bộ tài nguyên sẽ được tự động giải phóng.");
        }
    }

    /// <summary>
    /// Abstraction đại diện cho phiên làm việc với cơ sở dữ liệu (Database Session) theo Scoped lifetime.
    /// </summary>
    public interface IScopedDatabaseSession : IDisposable
    {
        Guid SessionId { get; }

        void Execute(string sql);
    }

    /// <summary>
    /// Mô phỏng tài nguyên cần chia sẻ xuyên suốt một request và phải được giải phóng khi kết thúc request.
    /// </summary>
    public sealed class ScopedDatabaseSession : IScopedDatabaseSession
    {
        public Guid SessionId { get; } = Guid.NewGuid();

        private bool _disposed;

        public ScopedDatabaseSession()
            => Console.WriteLine(
                $"  [NEW] Đã tạo ScopedDatabaseSession - ID: {SessionId}");

        public void Execute(string sql)
        {
            // Ngăn chặn việc tái sử dụng đối tượng sau khi đã bị giải phóng bằng ngoại lệ rõ ràng.
            ObjectDisposedException.ThrowIf(_disposed, this);
            Console.WriteLine($"  [Session {SessionId.ToString()[..8]}] SQL: {sql}");
        }

        public void Dispose()
        {
            // Đảm bảo tính idempotent cho Dispose(): có thể gọi nhiều lần an toàn nhưng chỉ thực thi dọn dẹp một lần.
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            Console.WriteLine($"  [DISPOSE] Session {SessionId}");
        }
    }

    /// <summary>
    /// Service tiêu thụ (Consumer) nhận database session thông qua Constructor Injection.
    /// </summary>
    /// <remarks>
    /// Handler chỉ tiêu thụ mà không sở hữu (ownership) và không tự dispose dbSession.
    /// Scope (thông qua DI container) là bên khởi tạo nên sẽ chịu trách nhiệm gọi Dispose().
    /// Nguyên tắc "Bên nào khởi tạo tài nguyên thì bên đó dọn dẹp" giúp tránh việc một consumer
    /// tự ý hủy nhầm dependency mà các consumer khác trong cùng scope vẫn đang cần sử dụng.
    /// </remarks>
    public sealed class OrderProcessingHandler(IScopedDatabaseSession dbSession)
    {
        public void HandleOrder()
        {
            dbSession.Execute("INSERT INTO Orders VALUES (1, 'Pending')");
            Console.WriteLine($"  Handler sử dụng SessionId: {dbSession.SessionId}");
        }
    }
}
