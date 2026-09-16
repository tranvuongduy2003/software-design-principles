using System;
using System.Collections.Generic;

namespace SoftwareDesignPrinciples.SOLID
{
    /// <summary>
    /// Minh họa Dependency Inversion Principle (DIP - Nguyên lý đảo ngược phụ thuộc)
    /// thông qua bài toán gửi thông báo đa kênh.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <b>1. Bài toán thực tế.</b>
    /// Lớp NotificationService đảm nhiệm chính sách nghiệp vụ cấp cao (high-level
    /// policy) là “gửi thông báo”. Nếu lớp này tự ý khởi tạo trực tiếp (new)
    /// EmailSender, logic nghiệp vụ sẽ bị liên kết chặt (tight coupling) với chi
    /// tiết kỹ thuật gửi email. Khi hệ thống cần mở rộng sang SMS, Push Notification,
    /// hay cần dùng test double (mock/stub) để viết unit test, lập trình viên buộc
    /// phải chỉnh sửa trực tiếp lớp nghiệp vụ này. Mọi thay đổi ở tầng hạ tầng
    /// (infrastructure) vì thế sẽ lan truyền ngược lên và làm mất ổn định tầng nghiệp vụ.
    /// </para>
    /// <para>
    /// <b>2. Định nghĩa.</b>
    /// DIP được đúc kết qua hai mệnh đề cốt lõi:
    /// (1) Các module cấp cao không nên phụ thuộc vào các module cấp thấp; cả hai đều
    /// phải phụ thuộc vào abstraction (lớp trừu tượng hoặc interface).
    /// (2) Abstraction không được phụ thuộc vào chi tiết triển khai (details); ngược
    /// lại, chi tiết triển khai phải phụ thuộc vào abstraction.
    /// Trong đó: “Module cấp cao” chứa quy tắc và mục tiêu nghiệp vụ cốt lõi (business
    /// logic/policies); “module cấp thấp” chứa các cơ chế kỹ thuật cụ thể (plumbing
    /// code) như gửi mail qua SMTP, gọi SMS gateway, truy vấn database hay đọc ghi file.
    /// </para>
    /// <para>
    /// <b>3. Bản chất của sự đảo ngược.</b>
    /// Về mặt luồng thực thi thời gian chạy (runtime control flow), NotificationService
    /// cuối cùng vẫn kích hoạt EmailSender để gửi thư. Tuy nhiên, chiều phụ thuộc mã
    /// nguồn (source code dependency) đã được đảo ngược hoàn toàn: thay vì module cấp
    /// cao phụ thuộc vào module cấp thấp, cả hai giờ đây cùng phụ thuộc vào interface
    /// IMessageSender do chính tầng nghiệp vụ làm chủ và định hình. Chi tiết triển khai
    /// kỹ thuật (EmailSender) buộc phải tuân theo bản hợp đồng của tầng nghiệp vụ, thay
    /// vì tầng nghiệp vụ phải tự thích ứng với API cụ thể của phía gửi email.
    /// </para>
    /// <para>
    /// <b>4. Cơ chế giải quyết.</b>
    /// Interface IMessageSender đặc tả chính xác năng lực mà NotificationService yêu
    /// cầu: phát một thông điệp đi. Các lớp EmailSender và SmsSender đóng vai trò là
    /// các triển khai cụ thể (implementations) đáp ứng hợp đồng này.
    /// Tại Composition Root (điểm lắp ghép phụ thuộc nằm ở tầng ngoài cùng của ứng
    /// dụng, chẳng hạn như Main hay IoC Container), ta lựa chọn implementation cụ thể,
    /// khởi tạo đối tượng rồi tiêm (inject) vào NotificationService. Lớp Service hoàn
    /// toàn chỉ giao tiếp và nắm giữ tham chiếu tới abstraction.
    /// </para>
    /// <para>
    /// <b>5. Phân biệt DIP, DI và IoC.</b>
    /// - DIP (Dependency Inversion Principle) là nguyên lý kiến trúc định hướng chiều
    /// phụ thuộc giữa các tầng module.
    /// - DI (Dependency Injection - tiêm phụ thuộc) là mẫu thiết kế / kỹ thuật cụ thể
    /// nhằm cung cấp các đối tượng phụ thuộc từ bên ngoài vào một class thay vì để nó
    /// tự khởi tạo (new).
    /// - IoC (Inversion of Control - đảo ngược điều khiển) là nguyên lý kiến trúc bao
    /// quát hơn, nơi luồng điều khiển của ứng dụng được nhượng lại cho framework hoặc
    /// một thành phần trung gian (theo quy tắc Hollywood: "Đừng gọi chúng tôi, chúng
    /// tôi sẽ gọi bạn").
    /// Constructor Injection trong ví dụ này là một kỹ thuật DI dùng để hiện thực hóa
    /// DIP. Cần lưu ý: Việc sử dụng DI container không đồng nghĩa với việc mã nguồn tự
    /// động tuân thủ DIP (nếu container tiêm trực tiếp concrete class thay vì abstraction).
    /// </para>
    /// <para>
    /// <b>6. Cú pháp C#.</b>
    /// Constructor "NotificationService(IMessageSender messageSender)" công bố tường
    /// minh các phụ thuộc bắt buộc (explicit dependency). Trường "readonly" bảo đảm
    /// tham chiếu này là bất biến (immutable) và không bị gán lại ngoài ý muốn sau khi
    /// khởi tạo.
    /// Từ khóa "interface" định nghĩa bản hợp đồng trừu tượng, còn thao tác "new
    /// EmailSender()" được cách ly hoàn toàn tại điểm lắp ghép (Composition Root). Kỹ
    /// thuật Constructor Injection thuần túy (Pure DI) này hoạt động độc lập mà không
    /// bắt buộc phải phụ thuộc vào bất kỳ thư viện hay DI container bên ngoài nào.
    /// </para>
    /// <para>
    /// <b>7. Phân tích ví dụ.</b>
    /// Trong thiết kế chưa tốt, BadNotificationService ẩn giấu phụ thuộc bên trong
    /// constructor và tự quyết định cứng kênh gửi cụ thể (new BadEmailSender). Client
    /// hoàn toàn mất quyền cấu hình kênh gửi, còn việc viết unit test trở nên khó khăn
    /// vì buộc phải kết nối tới dịch vụ email thật hoặc can thiệp sửa mã nguồn của lớp.
    /// Ngược lại, NotificationService nhận IMessageSender từ bên ngoài. Cùng một lớp
    /// nghiệp vụ giờ đây có thể cộng tác mượt mà với EmailSender, SmsSender hay
    /// RecordingMessageSender mà không cần bất kỳ câu lệnh rẽ nhánh if/switch nào và
    /// không cần sửa đổi dù chỉ một dòng code (kết hợp hoàn hảo cùng OCP).
    /// </para>
    /// <para>
    /// <b>8. Lợi ích kiểm thử.</b>
    /// RecordingMessageSender đóng vai trò là một test double (cụ thể là Fake/Spy). Lớp
    /// này ghi nhận các thông điệp vào bộ nhớ (in-memory) để kiểm tra trạng thái và
    /// hành vi mà không cần thực hiện I/O hay gọi mạng thực tế.
    /// Khả năng kiểm thử độc lập (testability) là một hệ quả tuyệt vời khi các thành
    /// phần liên kết lỏng (loose coupling) qua abstraction. Dẫu vậy, mục tiêu cốt lõi
    /// của DIP vẫn là giữ cho tầng chính sách nghiệp vụ độc lập, không bị xâm lấn bởi
    /// các chi tiết kỹ thuật hạ tầng.
    /// </para>
    /// <para>
    /// <b>9. Trường hợp áp dụng.</b>
    /// DIP đặc biệt cần thiết tại các ranh giới kiến trúc (architectural boundaries) —
    /// nơi tầng Domain/Application tương tác với Database, File System, System Clock,
    /// dịch vụ bên thứ ba (Third-party APIs), Message Broker hoặc giao diện người dùng
    /// (UI).
    /// Không nên áp dụng máy móc bằng cách đặt interface cho mọi class đơn giản (như DTO,
    /// Entity dữ liệu) hoặc các thuật toán nội bộ ổn định. Abstraction chỉ thực sự phát
    /// huy giá trị ở những nơi có ranh giới module rõ ràng, có nhu cầu thay thế triển
    /// khai hoặc cần cô lập để viết unit test.
    /// </para>
    /// <para>
    /// <b>10. Lỗi thường gặp.</b>
    /// Một số sai lầm phổ biến khi áp dụng DIP:
    /// - Đặt interface ở tầng hạ tầng hoặc để interface làm rò rỉ (leak) các kiểu dữ
    /// liệu đặc thù của thư viện/công nghệ cụ thể lên tầng trên.
    /// - Lạm dụng Service Locator khiến các phụ thuộc bị ẩn giấu (hidden dependencies),
    /// làm mất tính minh bạch của mã nguồn.
    /// - Constructor nhận quá nhiều dependency (Constructor Over-injection) do class ôm
    /// đồm nhiều việc mà không nhận ra vi phạm SRP.
    /// - Tạo "header interface" (sao chép thụ động toàn bộ public method của một class
    /// cụ thể thành interface) thay vì thiết kế abstraction dựa trên nhu cầu thực sự
    /// của client.
    /// - Đã cấu hình DI container nhưng trong logic nghiệp vụ vẫn tự ý gọi toán tử "new"
    /// để tạo các thành phần chi tiết cấp thấp.
    /// </para>
    /// <para>
    /// <b>11. Mối liên hệ tổng hợp SOLID.</b>
    /// Năm nguyên lý SOLID bổ trợ và hoàn thiện lẫn nhau trong ví dụ:
    /// - SRP: NotificationService chỉ chịu một trách nhiệm duy nhất là điều phối gửi thông báo.
    /// - OCP: Dễ dàng mở rộng kênh thông báo mới mà không cần sửa đổi mã nguồn hiện có.
    /// - LSP: Mọi sender đều tuân thủ hợp đồng và có thể thay thế lẫn nhau mà không phá
    /// vỡ tính đúng đắn của chương trình.
    /// - ISP: Interface IMessageSender tinh gọn, chỉ chứa đúng phương thức client cần.
    /// - DIP: Đảo ngược chiều phụ thuộc — module cấp thấp phụ thuộc vào abstraction do
    /// module cấp cao làm chủ.
    /// Năm nguyên lý cùng phối hợp để tạo nên một hệ thống linh hoạt, bền vững trước các
    /// đợt thay đổi yêu cầu.
    /// </para>
    /// <para>
    /// <b>12. Câu hỏi phỏng vấn và đáp án mẫu.</b>
    /// Câu hỏi: "Dependency Injection (DI) có đồng nghĩa với Dependency Inversion (DIP) không?"
    /// Đáp án: Hoàn toàn không. DIP là một nguyên lý kiến trúc định hướng chiều phụ thuộc
    /// (cả module cấp cao và cấp thấp cùng phụ thuộc vào abstraction). Trong khi đó, DI là
    /// một kỹ thuật / mẫu thiết kế cụ thể nhằm cung cấp các phụ thuộc từ bên ngoài. Nếu một
    /// class cấp cao nhận trực tiếp một concrete class hạ tầng qua constructor, đó vẫn là DI
    /// nhưng đã vi phạm DIP vì chiều phụ thuộc chưa hề được đảo ngược qua abstraction.
    /// </para>
    /// <para>
    /// Câu hỏi: "Áp dụng DIP có đồng nghĩa với việc loại bỏ hoàn toàn từ khóa new không?"
    /// Đáp án: Không. Trong một ứng dụng hướng đối tượng, các đối tượng cụ thể cuối cùng
    /// vẫn phải được khởi tạo ở một nơi nào đó. Mục tiêu đúng đắn là cô lập toàn bộ các thao
    /// tác "new" đối tượng cụ thể tại Composition Root (điểm lắp ghép của ứng dụng), sau đó
    /// tiêm chúng vào lõi nghiệp vụ. Điều DIP ngăn chặn là việc để các module nghiệp vụ tự
    /// ý khởi tạo và phụ thuộc cứng vào các chi tiết hạ tầng kỹ thuật.
    /// </para>
    /// </remarks>
    public static class DependencyInversionDemo
    {
        /// <summary>
        /// Minh họa hướng phụ thuộc trực tiếp và hướng phụ thuộc qua abstraction.
        /// </summary>
        public static void Run()
        {
            Console.WriteLine("=== D - NGUYÊN LÝ ĐẢO NGƯỢC PHỤ THUỘC (DIP) ===");

            var badService = new BadNotificationService();
            badService.Notify("Thông báo từ thiết kế phụ thuộc trực tiếp.");

            Console.WriteLine();

            // Composition Root: Nơi duy nhất quyết định và lắp ráp các triển khai
            // cụ thể vào module nghiệp vụ NotificationService.
            IMessageSender emailSender = new EmailSender();
            var emailService = new NotificationService(emailSender);
            emailService.Notify("Thông báo qua email.");

            IMessageSender smsSender = new SmsSender();
            var smsService = new NotificationService(smsSender);
            smsService.Notify("Thông báo qua SMS.");

            // Test Double (RecordingMessageSender) ghi nhận thông điệp trong bộ nhớ,
            // thay thế hạ tầng mạng thực tế khi chạy unit test.
            var recordingSender = new RecordingMessageSender();
            var testableService = new NotificationService(recordingSender);
            testableService.Notify("Thông điệp cần kiểm tra.");
            Console.WriteLine(
                $"[Kiểm thử] Đã ghi nhận {recordingSender.Messages.Count} thông điệp.");

            Console.WriteLine(
                "Kết luận: Tầng chính sách nghiệp vụ hoàn toàn không phụ thuộc vào chi tiết kênh gửi.");
            Console.WriteLine();
        }
    }

    #region Thiết kế vi phạm DIP

    /// <summary>
    /// Module cấp thấp (chi tiết kỹ thuật): Cài đặt cụ thể việc gửi email qua API riêng.
    /// </summary>
    public sealed class BadEmailSender
    {
        public void SendEmail(string message)
        {
            Console.WriteLine($"[Chưa tốt] Gửi email: {message}");
        }
    }

    /// <summary>
    /// Thiết kế vi phạm: Module cấp cao tự khởi tạo và phụ thuộc trực tiếp (tight coupling) vào module cấp thấp.
    /// </summary>
    public sealed class BadNotificationService
    {
        private readonly BadEmailSender _emailSender;

        public BadNotificationService()
        {
            // Phụ thuộc bị ẩn (hidden dependency) và liên kết cứng (hard-coded).
            // Mã client không thể thay thế kênh gửi và không thể viết unit test độc lập.
            _emailSender = new BadEmailSender();
        }

        public void Notify(string message)
        {
            _emailSender.SendEmail(message);
        }
    }

    #endregion

    #region Thiết kế tuân thủ DIP

    /// <summary>
    /// Abstraction đại diện cho bản hợp đồng do tầng nghiệp vụ (NotificationService) định nghĩa và làm chủ.
    /// </summary>
    public interface IMessageSender
    {
        void SendMessage(string message);
    }

    /// <summary>
    /// Module cấp thấp (chi tiết kỹ thuật): Triển khai việc gửi email và phụ thuộc ngược vào abstraction IMessageSender.
    /// </summary>
    public sealed class EmailSender : IMessageSender
    {
        public void SendMessage(string message)
        {
            Console.WriteLine($"[Tốt] Gửi email: {message}");
        }
    }

    /// <summary>
    /// Module cấp thấp khác: Triển khai việc gửi SMS, dễ dàng hoán đổi nhờ cùng tuân thủ một hợp đồng.
    /// </summary>
    public sealed class SmsSender : IMessageSender
    {
        public void SendMessage(string message)
        {
            Console.WriteLine($"[Tốt] Gửi SMS: {message}");
        }
    }

    /// <summary>
    /// Test double (Fake/Spy) phục vụ kiểm thử: Lưu vết thông điệp trong bộ nhớ (in-memory), không thực hiện I/O mạng.
    /// </summary>
    public sealed class RecordingMessageSender : IMessageSender
    {
        private readonly List<string> _messages = new();

        public IReadOnlyList<string> Messages => _messages;

        public void SendMessage(string message)
        {
            _messages.Add(message);
        }
    }

    /// <summary>
    /// Module cấp cao: Chứa logic nghiệp vụ cốt lõi, chỉ phụ thuộc vào abstraction IMessageSender.
    /// </summary>
    public sealed class NotificationService
    {
        private readonly IMessageSender _messageSender;

        /// <summary>
        /// Constructor Injection: Khai báo tường minh và bắt buộc các phụ thuộc (explicit dependency).
        /// </summary>
        public NotificationService(IMessageSender messageSender)
        {
            ArgumentNullException.ThrowIfNull(messageSender);
            _messageSender = messageSender;
        }

        public void Notify(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException(
                    "Nội dung thông báo không được để trống.",
                    nameof(message));
            }

            _messageSender.SendMessage(message);
        }
    }

    #endregion
}
