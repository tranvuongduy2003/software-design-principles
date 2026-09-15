using System;

namespace SoftwareDesignPrinciples.OOP
{
    /// <summary>
    /// Minh họa nguyên lý Encapsulation (tính đóng gói) thông qua bài toán
    /// quản lý tài khoản ngân hàng.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Đặt vấn đề: Trong lập trình hướng đối tượng (OOP), đối tượng (object)
    /// không chỉ đơn thuần là cấu trúc gom cụm dữ liệu. Mỗi đối tượng là một
    /// thực thể hoàn chỉnh sở hữu định danh (identity), trạng thái nội tại
    /// (state) và các hành vi (behavior) tương ứng. Trong bài toán tài khoản
    /// ngân hàng, hệ thống có các quy tắc nghiệp vụ (business rules) bất di
    /// bất dịch: tài khoản phải gắn liền với tên chủ sở hữu hợp lệ, số tiền
    /// giao dịch phải luôn là số dương, và số dư không bao giờ được phép âm.
    /// Nếu số dư được khai báo dưới dạng biến công khai (public field), bất
    /// kỳ mã ngoại vi nào (client code) cũng có thể can thiệp trực tiếp và
    /// gán giá trị tùy tiện (chẳng hạn số dư âm), khiến đối tượng rơi vào
    /// trạng thái không hợp lệ (invalid state) và phá vỡ tính đúng đắn của
    /// toàn bộ hệ thống.
    /// </para>
    /// <para>
    /// Định nghĩa: Encapsulation (tính đóng gói) là kỹ thuật gom nhóm dữ liệu
    /// (state) cùng các hành vi (behavior) thao tác trực tiếp trên dữ liệu đó
    /// vào trong cùng một thực thể (class), đồng thời thiết lập ranh giới bảo
    /// vệ bằng cách che giấu chi tiết cấu trúc nội bộ (information hiding)
    /// và kiểm soát chặt chẽ quyền đọc/ghi từ bên ngoài. Mục tiêu cốt lõi
    /// của Encapsulation là bảo vệ tính bất biến (invariant) - tức các điều
    /// kiện hoặc quy tắc nghiệp vụ tiên quyết phải luôn được thỏa mãn trong
    /// suốt vòng đời của một đối tượng hợp lệ.
    /// </para>
    /// <para>
    /// Cơ chế hiện thực trong C#:
    /// - Private backing field (_balance): Ẩn giấu hoàn toàn dữ liệu thô khỏi
    ///   thế giới bên ngoài, ngăn chặn mọi can thiệp trái phép từ client code.
    /// - Read-only property (Balance, OwnerName): Chỉ công bố dữ liệu ra
    ///   ngoài dưới dạng đọc khi có nhu cầu quan sát, không mở public setter.
    /// - Constructor có kiểm thực (validation): Đảm bảo đối tượng luôn được
    ///   khởi tạo ở trạng thái hợp lệ ngay từ đầu (valid initial state); từ
    ///   chối và ném exception nếu dữ liệu đầu vào không hợp lệ (Fail-fast).
    /// - Public business methods (Deposit, Withdraw): Đóng vai trò là các lối
    ///   vào có kiểm soát (controlled entry points). Mọi biến đổi trạng thái
    ///   đều phải thông qua các phương thức mang ý nghĩa nghiệp vụ rõ ràng,
    ///   đi kèm kiểm tra tiền điều kiện (preconditions) để đảm bảo tính toàn
    ///   vẹn luôn được bảo toàn.
    /// - Lưu ý: Từ khóa 'private' chỉ là công cụ kiểm soát phạm vi truy cập
    ///   (access modifier). Một lớp chỉ thực sự đạt được tính đóng gói khi
    ///   mọi điểm truy cập công khai đều duy trì và bảo vệ tính bất biến.
    /// </para>
    /// <para>
    /// Phân tích luồng thực thi trong ví dụ:
    /// - BadBankAccount: Thể hiện thiết kế vi phạm tính đóng gói. Việc dùng
    ///   public field cho phép client code tùy ý gán số dư âm (-500), làm
    ///   hỏng quy tắc nghiệp vụ mà không có bất kỳ rào cản nào ngăn chặn.
    /// - BankAccount: Khắc phục triệt để vấn đề trên bằng cách chuyển trạng
    ///   thái số dư thành _balance với phạm vi private. Trạng thái chỉ được
    ///   đọc qua thuộc tính Balance và chỉ được thay đổi thông qua hai thao
    ///   tác nghiệp vụ Deposit hoặc Withdraw. Khi giao dịch rút tiền vượt quá
    ///   số dư hiện có, hệ thống chủ động từ chối giao dịch trước khi thực
    ///   hiện phép trừ, đảm bảo tài khoản luôn duy trì trạng thái hợp lệ.
    /// </para>
    /// <para>
    /// Ngữ cảnh áp dụng và lưu ý kiến trúc:
    /// - Áp dụng khi dữ liệu có miền giá trị hợp lệ (valid range), khi nhiều
    ///   trường dữ liệu cần duy trì sự nhất quán đồng bộ (ví dụ: ngày kết
    ///   thúc phải sau ngày bắt đầu), hoặc khi các thao tác biến đổi trạng
    ///   thái cần kiểm tra quyền hạn, ghi log và phát sinh sự kiện.
    /// - Phân biệt với DTO (Data Transfer Object): Các lớp DTO chỉ thuần túy
    ///   dùng để vận chuyển dữ liệu qua mạng hoặc giữa các tầng kiến trúc, do
    ///   đó việc sử dụng public getter/setter tự do là hoàn toàn phù hợp.
    ///   Tuy nhiên, DTO không thay thế cho Domain Model mang logic nghiệp vụ.
    ///   Trong thiết kế kiến trúc (như DDD), Domain Entity luôn phải là
    ///   Rich Domain Model (được đóng gói chặt chẽ, giàu hành vi), tránh biến
    ///   thành Anemic Domain Model (mô hình nghèo nàn chỉ chứa getter/setter).
    /// </para>
    /// <para>
    /// Lỗi thiết kế thường gặp (Code Smells &amp; Anti-patterns):
    /// - Lạm dụng getter/setter tự động (Auto-property syndrome): Mở public
    ///   setter cho mọi property theo thói quen, vô tình biến đối tượng thành
    ///   cấu trúc dữ liệu bị phơi bày hoàn toàn.
    /// - Chỉ kiểm thực ở tầng giao diện người dùng (UI-only validation): Phó
    ///   mặc việc kiểm tra dữ liệu cho Frontend hoặc Controller mà bỏ qua
    ///   tầng Domain, khiến nghiệp vụ dễ bị tổn thương khi có các luồng truy
    ///   cập khác (như API, Batch Job, Message Queue).
    /// - Nuốt lỗi ngầm (Silent failure): Tự ý điều chỉnh dữ liệu sai về giá
    ///   trị mặc định thay vì ném exception hoặc thông báo lỗi tường minh, dẫn
    ///   đến lỗi sai lệch logic ngầm rất khó gỡ lỗi (debug).
    /// - Rò rỉ tham chiếu đối tượng có thể thay đổi (Exposing mutable state):
    ///   Trả về trực tiếp tham chiếu của collection nội bộ (như List&lt;T&gt;)
    ///   thay vì bản sao hoặc giao diện chỉ đọc (IReadOnlyCollection&lt;T&gt;),
    ///   cho phép bên ngoài thêm/xóa phần tử ngoài tầm kiểm soát của đối tượng.
    /// - Lạm dụng trường dữ liệu protected: Cho phép các lớp con (derived
    ///   classes) can thiệp trực tiếp vào dữ liệu của lớp cha, làm suy yếu
    ///   khả năng tự bảo vệ tính bất biến của lớp cơ sở.
    /// </para>
    /// <para>
    /// Câu hỏi phỏng vấn kinh điển: "Khai báo trường dữ liệu là private có
    /// đồng nghĩa với việc đã đạt được Encapsulation không?"
    /// - Trả lời: Không. Từ khóa private chỉ đơn thuần là công cụ giới hạn
    ///   phạm vi truy cập (access modifier) do ngôn ngữ cung cấp.
    ///   Encapsulation là nguyên lý thiết kế yêu cầu đối tượng phải bảo vệ
    ///   tính toàn vẹn (invariant) tại mọi thời điểm thông qua các hành vi
    ///   nghiệp vụ có kiểm soát. Nếu một lớp có trường dữ liệu là private
    ///   nhưng lại đi kèm với public setter không có logic kiểm tra, hoặc cung
    ///   cấp phương thức thay đổi trạng thái mà không thẩm định dữ liệu, thì
    ///   đối tượng đó vẫn bị phá vỡ tính đóng gói và có thể rơi vào trạng
    ///   thái không hợp lệ bất kỳ lúc nào.
    /// </para>
    /// </remarks>
    public static class EncapsulationDemo
    {
        public static void Run()
        {
            Console.WriteLine("--- ENCAPSULATION (TÍNH ĐÓNG GÓI) ---");

            // Bước 1: Thiết kế vi phạm Encapsulation - public field không
            // có cơ chế kiểm soát truy cập ghi (write access).
            Console.WriteLine("Không áp dụng Encapsulation: public field");
            var badAccount = new BadBankAccount
            {
                Balance = 100m
            };
            // Dữ liệu vi phạm quy tắc nghiệp vụ (số dư âm) vẫn bị ghi đè
            // tùy tiện do thiếu ranh giới bảo vệ.
            badAccount.Balance = -500m;
            Console.WriteLine($"Số dư không hợp lệ: {badAccount.Balance:N0}");

            // Bước 2: Thiết kế áp dụng Encapsulation - khởi tạo đối tượng
            // với trạng thái ban đầu hợp lệ thông qua constructor có
            // kiểm thực (validation).
            Console.WriteLine(
                "\nÁp dụng Encapsulation: thay đổi qua business method");
            var account = new BankAccount("Nguyễn An", 1_000m);
            Console.WriteLine($"Số dư ban đầu: {account.Balance:N0}");

            // Bước 3: Thực hiện thay đổi trạng thái thông qua phương thức
            // nghiệp vụ thay vì can thiệp trực tiếp vào biến nội bộ.
            account.Deposit(500m);
            Console.WriteLine($"Sau khi gửi 500: {account.Balance:N0}");

            account.Withdraw(200m);
            Console.WriteLine($"Sau khi rút 200: {account.Balance:N0}");

            // Bước 4: Kiểm chứng cơ chế bảo vệ tính bất biến (invariants) -
            // giao dịch vi phạm quy tắc nghiệp vụ sẽ bị từ chối an toàn.
            try
            {
                account.Withdraw(5_000m);
            }
            catch (InvalidOperationException exception)
            {
                Console.WriteLine($"Từ chối giao dịch: {exception.Message}");
            }

            Console.WriteLine();
        }
    }

    /// <summary>
    /// Thiết kế vi phạm tính đóng gói: Trạng thái nội bộ bị phơi bày công khai,
    /// cho phép client code can thiệp tùy tiện.
    /// </summary>
    public class BadBankAccount
    {
        // Public field làm mất hoàn toàn quyền kiểm soát tính hợp lệ dữ liệu.
        public decimal Balance;
    }

    /// <summary>
    /// Thiết kế tuân thủ tính đóng gói: Dữ liệu nội bộ được bảo vệ chặt chẽ
    /// và duy trì hai bất biến (invariants): tên chủ tài khoản không được để
    /// trống và số dư không bao giờ được âm.
    /// </summary>
    public class BankAccount
    {
        // Trạng thái nội bộ (internal state) chỉ được phép đọc và ghi bên
        // trong phạm vi của lớp BankAccount.
        private decimal _balance;

        // Thuộc tính chỉ đọc (read-only property); giá trị được thiết lập
        // duy nhất một lần khi khởi tạo đối tượng.
        public string OwnerName { get; }

        // Phơi bày số dư ra bên ngoài dưới dạng chỉ đọc; không cung cấp
        // public setter nhằm ngăn client code sửa đổi trực tiếp.
        public decimal Balance => _balance;

        /// <summary>
        /// Khởi tạo tài khoản với trạng thái ban đầu hợp lệ; ném ngoại lệ ngay
        /// lập tức nếu dữ liệu đầu vào vi phạm quy tắc nghiệp vụ (Fail-fast).
        /// </summary>
        public BankAccount(string ownerName, decimal initialBalance)
        {
            if (string.IsNullOrWhiteSpace(ownerName))
            {
                throw new ArgumentException(
                    "Tên chủ tài khoản không được để trống.",
                    nameof(ownerName));
            }

            if (initialBalance < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(initialBalance),
                    "Số dư ban đầu không được âm.");
            }

            OwnerName = ownerName;
            _balance = initialBalance;
        }

        /// <summary>
        /// Nạp tiền vào tài khoản thông qua nghiệp vụ có kiểm soát; từ chối
        /// số tiền không hợp lệ để bảo toàn tính toàn vẹn dữ liệu.
        /// </summary>
        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(amount),
                    "Số tiền gửi phải lớn hơn 0.");
            }

            _balance += amount;
        }

        /// <summary>
        /// Rút tiền khỏi tài khoản; kiểm tra tiền điều kiện (số tiền hợp lệ
        /// và không vượt quá số dư hiện tại) trước khi biến đổi trạng thái.
        /// </summary>
        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(amount),
                    "Số tiền rút phải lớn hơn 0.");
            }

            if (amount > _balance)
            {
                throw new InvalidOperationException(
                    "Số dư không đủ để thực hiện giao dịch rút tiền.");
            }

            _balance -= amount;
        }
    }
}
