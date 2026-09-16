using System;
using System.Collections.Generic;
using System.Linq;

namespace SoftwareDesignPrinciples.DependencyInjection
{
    /*
     * BÀI 1. DEPENDENCY INJECTION VÀ CONTAINER TỐI GIẢN
     *
     * 1. Bài toán thực tế
     * ------------------
     * Trong thực tế, một lớp nghiệp vụ (chẳng hạn OrderProcessingHandler) thường phải phối hợp với
     * nhiều thành phần phụ thuộc (collaborators / dependencies) như: kết nối cơ sở dữ liệu, bộ kiểm
     * tra hợp lệ dữ liệu (validator) hay dịch vụ gửi thông báo.
     *
     * Nếu lớp nghiệp vụ tự khởi tạo trực tiếp các đối tượng này bằng toán tử `new`, nó sẽ bị gắn
     * chặt với các lớp triển khai cụ thể (concrete classes) và phải tự quản lý cách khởi tạo chúng.
     * Điều này dẫn đến sự phụ thuộc chặt chẽ (tight coupling), khiến mã nguồn khó thay thế triển khai,
     * khó mở rộng và đặc biệt là rất khó viết unit test (không thể mock các phụ thuộc).
     *
     * Dependency (phụ thuộc): Là bất kỳ dịch vụ hay đối tượng nào mà một lớp cần để thực hiện nhiệm vụ của mình.
     * Dependency Injection (DI - tiêm phụ thuộc): Là kỹ thuật cung cấp (inject) các phụ thuộc từ bên ngoài vào
     * đối tượng, thay vì để đối tượng tự khởi tạo chúng.
     * Trong kiến trúc phần mềm hiện đại (và trong mã nguồn này), các phụ thuộc chủ yếu được truyền qua hàm tạo;
     * kỹ thuật này được gọi là Constructor Injection - hình thức DI phổ biến và an toàn nhất.
     *
     * 2. Phân biệt các khái niệm then chốt
     * ------------------------------------
     * - Dependency Inversion Principle (DIP): Là một nguyên lý thiết kế hướng đối tượng (chữ 'D' trong SOLID).
     *   Nguyên lý này quy định: các module cấp cao không nên phụ thuộc vào các module cấp thấp; cả hai nên
     *   phụ thuộc vào abstraction (interface hoặc abstract class). Chi tiết triển khai cụ thể phải phụ thuộc vào
     *   abstraction, không có chiều ngược lại.
     * - Dependency Injection (DI): Là một design pattern / kỹ thuật triển khai cụ thể nhằm hiện thực hóa DIP.
     *   DI tách biệt việc "khởi tạo đối tượng" ra khỏi "sử dụng đối tượng".
     * - Inversion of Control (IoC - Đảo ngược điều khiển): Là nguyên lý kiến trúc mang tính khái quát rộng hơn.
     *   Thay vì mã nghiệp vụ chủ động điều phối luồng thực thi và vòng đời của đối tượng, quyền kiểm soát
     *   này được đảo ngược và ủy quyền lại cho một framework hoặc container bên ngoài.
     * - DI Container (IoC Container): Là công cụ/thư viện tự động hóa IoC. Container chịu trách nhiệm: lưu trữ
     *   thông tin đăng ký, giải quyết đồ thị phụ thuộc (dependency graph), quản lý vòng đời (lifetime) và
     *   khởi tạo các đối tượng hoàn chỉnh khi có yêu cầu.
     *
     * DI hoàn toàn có thể áp dụng thủ công (Pure DI / Manual DI) mà không cần đến container. Ví dụ:
     *
     *     IScopedDatabaseSession session = new ScopedDatabaseSession();
     *     var handler = new OrderProcessingHandler(session);
     *
     * Vai trò cốt lõi của DI Container là giải phóng lập trình viên khỏi việc khởi tạo thủ công khi hệ thống
     * có đồ thị phụ thuộc sâu và phức tạp.
     *
     * 3. Quy trình hoạt động của container trong ví dụ này
     * -----------------------------------------------------
     *     1. Đăng ký dịch vụ (Service Registration)
     *        -> Lưu thông tin cấu hình vào ServiceDescriptor
     *     2. Xây dựng Provider (BuildServiceProvider)
     *     3. Phân giải dịch vụ (Resolve / GetRequiredService<T>)
     *        -> Tra cứu ServiceDescriptor đã đăng ký
     *        -> Áp dụng cơ chế vòng đời (Lifetime: kiểm tra cache hit / cache miss)
     *        -> Khởi tạo đối tượng và đệ quy tiêm các phụ thuộc qua hàm tạo (Constructor Injection).
     *
     * Lưu ý: Đây là DI container tối giản phục vụ mục đích nghiên cứu và học tập để hiểu rõ bản chất của DI.
     * Phiên bản này chưa xử lý các trường hợp nâng cao như: đăng ký nhiều triển khai cho cùng một service (multiple
     * registrations), open generic types, factory delegate, phát hiện phụ thuộc vòng (circular dependency)
     * hay kiểm tra captive dependency.
     */

    /// <summary>
    /// Xác định vòng đời (lifetime) của service, quy định cách thức khởi tạo và phạm vi tái sử dụng đối tượng.
    /// </summary>
    /// <remarks>
    /// <para><see cref="Transient"/>: Luôn tạo instance mới ở mỗi lần phân giải (resolve).</para>
    /// <para><see cref="Scoped"/>: Dùng chung một instance duy nhất trong phạm vi của cùng một scope.</para>
    /// <para><see cref="Singleton"/>: Dùng chung một instance duy nhất trong toàn bộ vòng đời của ứng dụng (root provider).</para>
    /// </remarks>
    public enum ServiceLifetime
    {
        Transient,
        Scoped,
        Singleton
    }

    /// <summary>
    /// Chứa thông tin mô tả việc đăng ký service: kiểu dịch vụ (service type), lớp triển khai cụ thể (implementation type) và vòng đời (lifetime).
    /// </summary>
    /// <remarks>
    /// Ví dụ: Cấu hình <c>ICache -> MemoryCacheSingleton -> Singleton</c> chỉ dẫn cho container biết rằng:
    /// khi ứng dụng yêu cầu phân giải <c>ICache</c>, container sẽ tạo một instance của <c>MemoryCacheSingleton</c>
    /// và tái sử dụng instance đó cho các lần yêu cầu sau. Descriptor đóng vai trò là metadata cấu hình, không tự tạo đối tượng.
    /// </remarks>
    public sealed class ServiceDescriptor(
        Type serviceType,
        Type implType,
        ServiceLifetime lifetime)
    {
        public Type ServiceType { get; } = serviceType;
        public Type ImplementationType { get; } = implType;
        public ServiceLifetime Lifetime { get; } = lifetime;
    }

    /// <summary>
    /// Tập hợp các cấu hình đăng ký service (service descriptors) trước khi tiến hành khởi tạo provider.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Về bản chất, lớp này quản lý danh sách các <see cref="ServiceDescriptor"/>. Ba phương thức
    /// đăng ký có cấu trúc tương tự nhau và chỉ khác nhau ở giá trị thiết lập <see cref="ServiceLifetime"/>.
    /// </para>
    /// <para>
    /// Ràng buộc <c>where TImpl : class, TService</c> đảm bảo kiểm tra ngay tại thời điểm biên dịch (compile-time)
    /// rằng lớp triển khai phải là kiểu tham chiếu (reference type) và có quan hệ kế thừa/hiện thực hóa service tương ứng.
    /// Nhờ đó, các lỗi cấu hình sai kiểu dữ liệu được phát hiện từ sớm.
    /// </para>
    /// </remarks>
    public sealed class SimpleServiceCollection
    {
        internal readonly List<ServiceDescriptor> _descriptors = [];

        /// <summary>
        /// Đăng ký service với vòng đời Transient (khởi tạo mới ở mỗi lần phân giải).
        /// </summary>
        public SimpleServiceCollection AddTransient<TService, TImpl>()
            where TImpl : class, TService
        {
            _descriptors.Add(
                new(typeof(TService), typeof(TImpl), ServiceLifetime.Transient));
            return this;
        }

        /// <summary>
        /// Đăng ký service với vòng đời Scoped (dùng chung trong cùng một phạm vi scope).
        /// </summary>
        public SimpleServiceCollection AddScoped<TService, TImpl>()
            where TImpl : class, TService
        {
            _descriptors.Add(
                new(typeof(TService), typeof(TImpl), ServiceLifetime.Scoped));
            return this;
        }

        /// <summary>
        /// Đăng ký service với vòng đời Singleton (duy nhất trong toàn bộ ứng dụng).
        /// </summary>
        public SimpleServiceCollection AddSingleton<TService, TImpl>()
            where TImpl : class, TService
        {
            _descriptors.Add(
                new(typeof(TService), typeof(TImpl), ServiceLifetime.Singleton));
            return this;
        }

        /// <summary>
        /// Hoàn tất cấu hình đăng ký và khởi tạo root provider (Service Provider gốc).
        /// </summary>
        public SimpleServiceProvider BuildServiceProvider() => new(_descriptors);
    }

    /// <summary>
    /// Phân giải (resolve) service, dựng đồ thị phụ thuộc (object graph) và quản lý bộ nhớ đệm theo vòng đời (lifetime).
    /// </summary>
    /// <remarks>
    /// <para>Cơ chế cốt lõi của ba loại lifetime nằm ở vị trí lưu trữ (cache) instance:</para>
    /// <list type="bullet">
    /// <item><description><see cref="ServiceLifetime.Transient"/>: Không lưu vào cache; luôn khởi tạo instance mới ở mỗi lần phân giải.</description></item>
    /// <item><description><see cref="ServiceLifetime.Scoped"/>: Lưu vào cache cục bộ của từng scope.</description></item>
    /// <item><description><see cref="ServiceLifetime.Singleton"/>: Lưu vào cache dùng chung giữa root provider và toàn bộ scope con.</description></item>
    /// </list>
    /// <para>
    /// Trong các ứng dụng thực tế, bạn nên sử dụng thư viện tiêu chuẩn <c>Microsoft.Extensions.DependencyInjection</c> vì
    /// cung cấp khả năng xác thực cấu hình, xử lý đồng thời (concurrency), generic, factory delegate và
    /// quản lý giải phóng tài nguyên (disposal) toàn diện, an toàn hơn.
    /// </para>
    /// </remarks>
    public sealed class SimpleServiceProvider : IDisposable
    {
        private readonly List<ServiceDescriptor> _descriptors;

        // Mọi child provider (scope con) đều giữ tham chiếu về root provider. Nhờ đó, các singleton instance
        // luôn được root tạo, quản lý và giải phóng đồng bộ, kể cả khi được resolve lần đầu từ một scope con.
        private readonly SimpleServiceProvider _root;

        // Cache dùng chung cho Singleton giữa root provider và toàn bộ scope con.
        // Việc chia sẻ cùng một Dictionary đảm bảo mọi nơi đều truy xuất đúng một instance duy nhất.
        private readonly Dictionary<Type, object> _singletonCache;

        // Cache riêng cho Scoped service trên từng scope provider.
        // Hai scope khác nhau sở hữu hai cache riêng biệt, đảm bảo tính cô lập hoàn toàn giữa các phạm vi thực thi.
        private readonly Dictionary<Type, object> _scopedCache;

        // Danh sách các đối tượng IDisposable do provider hiện tại quản lý và chịu trách nhiệm giải phóng.
        // Scope provider quản lý các instance Scoped và Transient do chính nó tạo ra;
        // Root provider quản lý Singleton và các service được phân giải trực tiếp từ root.
        private readonly List<IDisposable> _disposables = [];

        private bool _disposed;

        /// <summary>
        /// Khởi tạo root provider với cache Singleton và cache Scoped ban đầu.
        /// </summary>
        internal SimpleServiceProvider(List<ServiceDescriptor> descriptors)
        {
            _descriptors = descriptors;
            _root = this;
            _singletonCache = new();
            _scopedCache = new();
        }

        /// <summary>
        /// Khởi tạo provider cho một scope con mới.
        /// </summary>
        /// <remarks>
        /// Scope con nhận lại đúng tham chiếu cache Singleton của root provider, đồng thời khởi tạo một cache Scoped độc lập.
        /// Chi tiết thiết kế này hiện thực hóa chính xác cơ chế: "Singleton dùng chung toàn cục, Scoped tách biệt theo từng phạm vi".
        /// </remarks>
        private SimpleServiceProvider(
            List<ServiceDescriptor> descriptors,
            SimpleServiceProvider root)
        {
            _descriptors = descriptors;
            _root = root;
            _singletonCache = root._singletonCache;
            _scopedCache = new();
        }

        /// <summary>
        /// Phân giải service kiểu <typeparamref name="T"/>, ném ngoại lệ nếu service chưa được đăng ký.
        /// </summary>
        /// <typeparam name="T">Abstraction (interface) hoặc concrete class đã đăng ký.</typeparam>
        /// <returns>Instance hoàn chỉnh đã được tiêm đầy đủ các phụ thuộc qua hàm tạo.</returns>
        public T GetRequiredService<T>() where T : class
            => (T)ResolveRequired(typeof(T));

        /// <summary>
        /// Phân giải service theo kiểu dữ liệu (non-generic), phục vụ quá trình phân giải đệ quy.
        /// </summary>
        private object ResolveRequired(Type serviceType)
            => Resolve(serviceType)
               ?? throw new InvalidOperationException(
                   $"Service '{serviceType.Name}' chưa được đăng ký.");

        /// <summary>
        /// Tra cứu ServiceDescriptor và áp dụng chiến lược khởi tạo hoặc tái sử dụng đối tượng theo Lifetime.
        /// </summary>
        private object? Resolve(Type serviceType)
        {
            // FirstOrDefault khiến container mẫu chỉ chọn đăng ký đầu tiên nếu một service
            // được đăng ký nhiều lần. Trong container chính thức của .NET, danh sách đăng ký được xử lý linh hoạt hơn.
            var descriptor = _descriptors.FirstOrDefault(
                item => item.ServiceType == serviceType);

            if (descriptor is null)
            {
                return null;
            }

            return descriptor.Lifetime switch
            {
                ServiceLifetime.Transient =>
                    CreateTransient(descriptor.ImplementationType),
                ServiceLifetime.Scoped =>
                    GetOrCreate(_scopedCache, descriptor),
                ServiceLifetime.Singleton =>
                    _root.GetOrCreate(_singletonCache, descriptor),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        /// <summary>
        /// Khởi tạo instance Transient mới và đăng ký theo dõi giải phóng tài nguyên với provider hiện tại.
        /// </summary>
        private object CreateTransient(Type implementationType)
        {
            var instance = CreateInstance(implementationType);
            TrackDisposable(instance);
            return instance;
        }

        /// <summary>
        /// Truy xuất instance từ cache; nếu chưa có (cache miss) thì khởi tạo, lưu vào cache và theo dõi để giải phóng tài nguyên.
        /// </summary>
        /// <param name="cache">Cache Singleton dùng chung hoặc cache Scoped của phạm vi hiện tại.</param>
        /// <param name="descriptor">Thông tin đăng ký service cần phân giải.</param>
        private object GetOrCreate(
            Dictionary<Type, object> cache,
            ServiceDescriptor descriptor)
        {
            // Sử dụng lock trên cache để thao tác "kiểm tra và khởi tạo" (check-then-act) mang tính nguyên tử (atomic / thread-safe).
            // Nếu nhiều luồng yêu cầu đồng thời, chỉ một luồng thực thi constructor và tất cả các luồng đều nhận cùng một instance.
            lock (cache)
            {
                if (cache.TryGetValue(descriptor.ServiceType, out var existing))
                {
                    // Cache hit: Đối tượng đã tồn tại trong cache, tái sử dụng mà không cần gọi constructor.
                    return existing;
                }

                // Cache miss: Khởi tạo đối tượng mới và lưu vào cache với key là ServiceType.
                var instance = CreateInstance(descriptor.ImplementationType);
                cache[descriptor.ServiceType] = instance;
                TrackDisposable(instance);
                return instance;
            }
        }

        /// <summary>
        /// Thực hiện Constructor Injection thông qua Reflection (cơ chế soi chiếu kiểu dữ liệu tại runtime).
        /// </summary>
        /// <remarks>
        /// <para>Thuật toán khởi tạo gồm bốn bước:</para>
        /// <list type="number">
        /// <item><description>Quét danh sách các public constructor của lớp triển khai.</description></item>
        /// <item><description>Chọn constructor có nhiều tham số nhất (greedy constructor selection).</description></item>
        /// <item><description>Phân giải đệ quy (recursive resolution) service tương ứng với từng tham số.</description></item>
        /// <item><description>Kích hoạt constructor (invoke) với mảng đối số vừa thu được.</description></item>
        /// </list>
        /// <para>
        /// Ví dụ: Khi khởi tạo <c>OrderProcessingHandler(IScopedDatabaseSession dbSession)</c>,
        /// container sẽ phân giải <c>IScopedDatabaseSession</c> trước, sau đó truyền instance thu được vào
        /// constructor của handler. Quá trình đệ quy này sẽ dựng nên toàn bộ đồ thị đối tượng (object graph).
        /// </para>
        /// <para>
        /// Tiêu chí chọn "constructor có nhiều tham số nhất" là một quy ước đơn giản nhằm mục đích minh họa.
        /// Nếu lớp không có public constructor, thiếu đăng ký cho phụ thuộc hoặc đồ thị bị phụ thuộc vòng (circular dependency),
        /// đoạn mã này sẽ ném ngoại lệ thô từ runtime. Một container hoàn chỉnh trong thực tế sẽ kiểm tra đồ thị và phát sinh thông báo lỗi rõ ràng hơn.
        /// </para>
        /// </remarks>
        private object CreateInstance(Type type)
        {
            var constructor = type
                .GetConstructors()
                .OrderByDescending(item => item.GetParameters().Length)
                .First();

            var arguments = constructor
                .GetParameters()
                .Select(parameter => ResolveRequired(parameter.ParameterType))
                .ToArray();

            return constructor.Invoke(arguments);
        }

        /// <summary>
        /// Tạo một scope mới để giới hạn phạm vi dùng chung và kiểm soát thời điểm giải phóng tài nguyên của các Scoped service.
        /// </summary>
        public SimpleServiceScope CreateScope()
            => new(new SimpleServiceProvider(_descriptors, _root));

        private void TrackDisposable(object instance)
        {
            if (instance is IDisposable disposable)
            {
                _disposables.Add(disposable);
            }
        }

        /// <summary>
        /// Giải phóng các tài nguyên (IDisposable) do provider hiện tại quản lý theo thứ tự ngược với lúc khởi tạo (LIFO).
        /// </summary>
        /// <remarks>
        /// Thứ tự giải phóng ngược (LIFO) đảm bảo đối tượng cấp cao được dọn dẹp trước các đối tượng phụ thuộc mà nó đang sử dụng.
        /// Cờ <c>_disposed</c> đảm bảo tính chất lũy kế (idempotent): việc gọi Dispose nhiều lần chỉ thực thi dọn dẹp đúng một lần duy nhất.
        /// </remarks>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            for (var index = _disposables.Count - 1; index >= 0; index--)
            {
                _disposables[index].Dispose();
            }
        }
    }

    /// <summary>
    /// Đại diện cho một phạm vi vòng đời (lifetime scope), chuyển tiếp trách nhiệm giải phóng tài nguyên cho ServiceProvider của scope đó.
    /// </summary>
    /// <remarks>
    /// Cú pháp <c>using var scope = provider.CreateScope();</c> bảo đảm phương thức <c>Dispose</c> được gọi tự động
    /// khi ra khỏi phạm vi khối lệnh, qua đó giải phóng an toàn các Scoped service (chẳng hạn như database session, DbContext hay transaction).
    /// </remarks>
    public sealed class SimpleServiceScope(SimpleServiceProvider provider) : IDisposable
    {
        public SimpleServiceProvider ServiceProvider { get; } = provider;

        public void Dispose() => ServiceProvider.Dispose();
    }
}
