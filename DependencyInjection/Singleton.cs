using System;
using System.Collections.Concurrent;

namespace SoftwareDesignPrinciples.DependencyInjection
{
    /*
     * BÀI 4. SINGLETON LIFETIME VÀ QUAN HỆ GIỮA CÁC LIFETIME
     *
     * 1. Đặt vấn đề & Nhu cầu
     * ------------------------
     * Trong ứng dụng, nhiều service đóng vai trò quản lý tài nguyên/cấu hình dùng chung, có chi phí
     * khởi tạo cao (heavy initialization) và không phụ thuộc vào bất kỳ request cụ thể nào. Việc tái tạo
     * instance mới ở mỗi lần phân giải (resolve) vừa gây lãng phí CPU/bộ nhớ, vừa làm phân mảnh hoặc
     * mất mát trạng thái cần chia sẻ. Với những service này, giải pháp tối ưu là duy trì duy nhất một
     * instance trong suốt vòng đời ứng dụng.
     *
     * 2. Định nghĩa và Bản chất
     * --------------------------
     * Singleton lifetime chỉ tạo tối đa một instance duy nhất cho mỗi đăng ký service (service descriptor)
     * trong phạm vi root provider. Cả root provider và mọi child scope đều phân giải về cùng một
     * tham chiếu (reference).
     *
     * Cần phân biệt rõ:
     * - Singleton trong DI (Lifetime Policy): Là chính sách quản lý vòng đời do DI container kiểm soát.
     *   Bản thân class vẫn là POCO thông thường với public constructor, không cần private constructor
     *   hay static instance property.
     * - Singleton Design Pattern (GoF): Ép buộc tính duy nhất ngay ở mức class bằng private constructor
     *   và static field/method (gây khó khăn khi viết unit test và vi phạm Dependency Inversion).
     *
     * Cú pháp đăng ký trong .NET:
     *
     *     services.AddSingleton<ISingletonCache, MemoryCacheSingleton>();
     *
     * 3. Cơ chế hoạt động
     * -------------------
     * Root provider khởi tạo và quản lý `_singletonCache`. Khi một child scope được tạo ra, nó được
     * chia sẻ trực tiếp tham chiếu của cache này từ root.
     * - Lần resolve đầu tiên (Cache Miss): Container khởi tạo instance theo cấu hình và lưu vào cache.
     * - Các lần resolve tiếp theo (Cache Hit): Bất kể được gọi từ root hay bất kỳ child scope nào,
     *   container đều trả về cùng một instance đã lưu trong cache.
     *
     * 4. Trường hợp áp dụng & Lưu ý kiến trúc
     * ----------------------------------------
     * - Thích hợp cho: In-memory cache, service đọc cấu hình ứng dụng, metadata registry, hoặc các
     *   service phi trạng thái (stateless) nhưng tốn chi phí khởi tạo lớn.
     * - An toàn đa luồng (Thread-Safety): Do một instance singleton được chia sẻ đồng thời giữa nhiều
     *   request/thread, implementation của nó BẮT BUỘC phải thread-safe. Nếu có trạng thái khả biến
     *   (mutable state), phải áp dụng các cơ chế đồng bộ (synchronization) hoặc các collection an toàn
     *   đa luồng (ví dụ: ConcurrentDictionary).
     *
     * 5. Các lỗi thường gặp (Antipatterns)
     * ------------------------------------
     * - Rò rỉ trạng thái (State Leaking): Lưu thông tin riêng của người dùng hoặc ngữ cảnh request trong
     *   singleton, dẫn đến nguy cơ xung đột dữ liệu và rò rỉ bảo mật giữa các request độc lập.
     * - Vi phạm an toàn đa luồng: Sử dụng collection thông thường (như Dictionary, List) rồi đọc/ghi
     *   từ nhiều luồng đồng thời mà không có khóa đồng bộ, gây race condition hoặc crash ứng dụng.
     * - Captive Dependency (Phụ thuộc bị cầm giữ): Inject trực tiếp một Scoped service vào Singleton.
     *   Vì Singleton sống suốt vòng đời ứng dụng, nó sẽ "giam giữ" instance Scoped đầu tiên vượt ra ngoài
     *   ranh giới vòng đời hợp lệ của nó, gây memory leak hoặc ObjectDisposedException khi dùng lại.
     * - Ngộ nhận về Thread-Safety: Lầm tưởng việc đăng ký AddSingleton sẽ tự động giúp class thread-safe.
     *   DI container chỉ đảm bảo tính duy nhất của instance, hoàn toàn không can thiệp hay khóa truy cập
     *   vào logic nội bộ của class.
     *
     * Khi một service Singleton cần thực thi một tác vụ đòi hỏi Scoped service (ví dụ: background worker
     * cần ghi log vào database qua DbContext), giải pháp chuẩn mực là inject IServiceScopeFactory (hoặc
     * IServiceProvider), chủ động tạo một scope ngắn hạn cho tác vụ đó, resolve scoped service bên trong
     * scope và giải phóng ngay khi hoàn tất. Phần cuối ví dụ mô phỏng kỹ thuật này từ Composition Root.
     */

    /// <summary>
    /// Minh họa cơ chế chia sẻ instance duy nhất của Singleton giữa root provider và các child scope.
    /// </summary>
    public static class SingletonDemo
    {
        public static void Run()
        {
            Console.WriteLine(
                "\n=== SINGLETON: một instance dùng chung trong toàn provider ===\n");

            var services = new SimpleServiceCollection();
            services.AddSingleton<ISingletonCache, MemoryCacheSingleton>();
            services.AddScoped<IScopedWorker, ScopedWorker>();

            using var provider = services.BuildServiceProvider();

            // Lần resolve đầu tiên (Cache Miss): Container khởi tạo singleton và lưu vào cache.
            var rootCache = provider.GetRequiredService<ISingletonCache>();
            rootCache.Set("app", "DI Demo");

            // Scope 1 dùng chung singleton cache với root nên đọc được giá trị root đã ghi.
            using (var firstScope = provider.CreateScope())
            {
                var firstCache = firstScope.ServiceProvider
                    .GetRequiredService<ISingletonCache>();
                Console.WriteLine($"  Scope 1 đọc 'app': {firstCache.Get("app")}");
                Console.WriteLine(
                    $"  Cùng instance với root? {ReferenceEquals(rootCache, firstCache)} " +
                    "(kỳ vọng: True)");
            }

            // Scope 2 vẫn nhận cùng tham chiếu; dữ liệu ghi từ Scope 2 có hiệu lực toàn cục.
            using (var secondScope = provider.CreateScope())
            {
                var secondCache = secondScope.ServiceProvider
                    .GetRequiredService<ISingletonCache>();
                secondCache.Set("version", "1.0");
                Console.WriteLine(
                    $"\n  Scope 2 cùng instance? " +
                    $"{ReferenceEquals(rootCache, secondCache)} (kỳ vọng: True)");
            }

            Console.WriteLine(
                $"  Root đọc 'version' do Scope 2 ghi: {rootCache.Get("version")}");

            // Tránh Captive Dependency: Không inject ScopedWorker vào một Singleton sống lâu.
            // Thay vào đó, tạo scope ngắn hạn theo từng đơn vị công việc, resolve worker trong scope
            // và tự động giải phóng sau khi hoàn tất.
            Console.WriteLine("\n--- Sử dụng scoped service an toàn trong một scope riêng ---");
            using (var jobScope = provider.CreateScope())
            {
                var worker = jobScope.ServiceProvider.GetRequiredService<IScopedWorker>();
                worker.DoWork();
            }

            Console.WriteLine(
                "\nKết luận: Root provider và mọi child scope đều dùng chung một instance Singleton duy nhất.");
        }
    }

    /// <summary>
    /// Abstraction đại diện cho kho dữ liệu cache dùng chung toàn ứng dụng.
    /// </summary>
    public interface ISingletonCache
    {
        void Set(string key, object value);

        object? Get(string key);
    }

    /// <summary>
    /// Implementation của cache singleton, hỗ trợ truy cập an toàn trong môi trường đa luồng (thread-safe).
    /// </summary>
    /// <remarks>
    /// <see cref="ConcurrentDictionary{TKey,TValue}"/> đảm bảo tính an toàn cho từng thao tác riêng lẻ (atomic read/write).
    /// Nếu nghiệp vụ gồm chuỗi thao tác phức hợp (compound operations như kiểm tra rồi mới cập nhật),
    /// ta vẫn cần sử dụng các API nguyên tử chuyên dụng hoặc bổ sung cơ chế khóa (synchronization lock).
    /// </remarks>
    public sealed class MemoryCacheSingleton : ISingletonCache
    {
        private readonly ConcurrentDictionary<string, object> _cache = new();
        private readonly Guid _id = Guid.NewGuid();

        public MemoryCacheSingleton()
            => Console.WriteLine(
                $"  [NEW] Đã tạo MemoryCacheSingleton - ID: {_id} (chỉ một lần duy nhất)");

        public void Set(string key, object value)
        {
            _cache[key] = value;
            Console.WriteLine($"  [Cache] Gán '{key}' = '{value}'");
        }

        public object? Get(string key) => _cache.GetValueOrDefault(key);
    }

    /// <summary>
    /// Abstraction đại diện cho tác vụ chỉ có hiệu lực trong phạm vi một scope cụ thể.
    /// </summary>
    public interface IScopedWorker
    {
        void DoWork();
    }

    public sealed class ScopedWorker : IScopedWorker
    {
        public void DoWork()
            => Console.WriteLine("  [ScopedWorker] Thực hiện công việc trong scope ngắn hạn.");
    }
}

