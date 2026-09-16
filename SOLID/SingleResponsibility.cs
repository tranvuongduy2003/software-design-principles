using System;

namespace SoftwareDesignPrinciples.SOLID
{
    /// <summary>
    /// Minh họa Single Responsibility Principle (SRP - Nguyên lý Đơn trách nhiệm)
    /// thông qua bài toán xử lý hóa đơn trong thực tế.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <b>Tổng quan về SOLID.</b>
    /// SOLID là bộ 5 nguyên lý thiết kế hướng đối tượng (OOP) kinh điển: Single
    /// Responsibility (Đơn trách nhiệm), Open/Closed (Đóng/Mở), Liskov Substitution
    /// (Thay thế Liskov), Interface Segregation (Phân tách giao diện) và Dependency
    /// Inversion (Đảo ngược phụ thuộc). Đây là các nguyên lý mang tính định hướng kiến trúc,
    /// không phải design pattern cụ thể, thư viện hay quy tắc cú pháp bắt buộc. Mục tiêu
    /// cốt lõi của SOLID là giúp hệ thống dễ dàng thích ứng với thay đổi, phân định rõ ràng
    /// ranh giới giữa các module, hạn chế tối đa rủi ro phát sinh lỗi ngoài ý muốn (side-effects)
    /// và duy trì khả năng mở rộng lâu dài. Các bài học trong thư mục này được sắp xếp tuần tự
    /// để bài sau kế thừa và phát triển từ bài trước.
    /// </para>
    /// <para>
    /// <b>1. Bài toán thực tế.</b>
    /// Xét quy trình xử lý hóa đơn cơ bản gồm 3 bước: tính toán tổng tiền, định dạng
    /// hiển thị (in ấn) và lưu trữ dữ liệu. Sai lầm phổ biến khi mới thiết kế là gom toàn bộ
    /// 3 tác vụ này vào chung một class duy nhất với lý do "chúng đều xoay quanh hóa đơn".
    /// Vấn đề nảy sinh khi 3 tác vụ này chịu sự chi phối của 3 bên liên quan (stakeholders)
    /// hoàn toàn độc lập: bộ phận kinh doanh thay đổi công thức tính thuế/khuyến mãi, đội ngũ
    /// UI/UX thay đổi mẫu in/định dạng hiển thị, và nhóm hạ tầng thay đổi hệ quản trị CSDL.
    /// Khi gom chung vào một class, bất kỳ yêu cầu thay đổi nào từ một bên cũng có nguy cơ
    /// làm hỏng các phần còn lại và gây lỗi hồi quy (regression bugs).
    /// </para>
    /// <para>
    /// <b>2. Định nghĩa.</b>
    /// SRP phát biểu rằng: <i>"Một module (hoặc class) chỉ nên có một lý do duy nhất để
    /// thay đổi"</i> (A module should have one, and only one, reason to change).
    /// Theo giải thích chuẩn xác của Robert C. Martin (Uncle Bob): Một module chỉ nên chịu
    /// trách nhiệm trước duy nhất một actor (tác nhân) — tức một nhóm người dùng, một phòng ban
    /// hoặc một bên liên quan đại diện cho nguồn phát sinh yêu cầu thay đổi.
    /// Cần lưu ý: "Một trách nhiệm" không đồng nghĩa với "chỉ có một hàm duy nhất". Đó là một
    /// tập hợp các hành vi gắn kết chặt chẽ, cùng phục vụ một mục tiêu nghiệp vụ nhất quán
    /// và thay đổi cùng nhau vì cùng một lý do.
    /// </para>
    /// <para>
    /// <b>3. Bản chất.</b>
    /// Bản chất của SRP là hướng tới High Cohesion (độ gắn kết nội tại cao) và Low Coupling
    /// (độ phụ thuộc lỏng lẻo) trong kiến trúc phần mềm:
    /// - Những dữ liệu và hành vi cùng phục vụ một chính sách nghiệp vụ cần được gom lại gần
    /// nhau (tính đóng gói cao).
    /// - Các chính sách thay đổi theo những lý do độc lập cần được phân tách ranh giới rõ ràng.
    /// SRP là kim chỉ nam định hướng tư duy phân rã hệ thống; đây không phải quy tắc cú pháp
    /// của C# và tuyệt đối không đồng nghĩa với việc xé nhỏ mọi class một cách máy móc, cực đoan.
    /// </para>
    /// <para>
    /// <b>4. Cơ chế và mô hình giải quyết.</b>
    /// Để tái cấu trúc theo SRP, bước đầu tiên là nhận diện các nguồn gây thay đổi (actors).
    /// Sau đó, ta phân định ranh giới và bóc tách các trách nhiệm độc lập:
    /// - Giữ toàn bộ dữ liệu và logic tính toán nghiệp vụ trong domain entity <c>Invoice</c>.
    /// - Chuyển logic trình bày, định dạng in ấn sang service <c>InvoicePrinter</c>.
    /// - Chuyển logic lưu trữ dữ liệu xuống hạ tầng sang <c>InvoiceRepository</c>.
    /// Tầng điều phối (application/orchestration layer) bên ngoài sẽ kết hợp 3 đối tượng này
    /// để hoàn tất luồng nghiệp vụ. Việc tách class là phương tiện triển khai (mechanism),
    /// còn SRP là nguyên lý định hướng (policy) giúp đưa ra quyết định phân tách đúng đắn.
    /// </para>
    /// <para>
    /// <b>5. Cú pháp C# được sử dụng.</b>
    /// Ví dụ tận dụng các tính năng cơ bản của C# để hỗ trợ SRP:
    /// - Dùng class riêng biệt để đóng gói ranh giới từng trách nhiệm.
    /// - Dùng constructor để kiểm tra tính hợp lệ và bảo toàn tính toàn vẹn (invariants) ngay khi khởi tạo.
    /// - Dùng read-only property (<c>{ get; }</c>) để bảo vệ tính đóng gói, tránh việc bị sửa dữ liệu tùy tiện từ bên ngoài.
    /// Lưu ý rằng C# không có từ khóa chuyên biệt nào cho SRP. Interface chỉ thực sự cần thiết khi hệ thống
    /// cần hỗ trợ đa hình (polymorphism), trừu tượng hóa để phục vụ kiểm thử tự động (mocking/testing) hoặc áp dụng
    /// Dependency Injection (DIP); tránh lạm dụng tạo interface tràn lan chỉ để mang tính hình thức.
    /// </para>
    /// <para>
    /// <b>6. Phân tích ví dụ.</b>
    /// <c>BadInvoice</c> vi phạm SRP vì đóng vai trò "God Class" thu nhỏ: vừa tính tiền, vừa in ấn,
    /// vừa lưu trữ. Nếu muốn đổi mẫu in sang PDF hoặc chuyển CSDL sang dịch vụ đám mây (Cloud),
    /// ta buộc phải sửa đổi class chứa mã nguồn tính tiền cốt lõi.
    /// Ở thiết kế chuẩn hóa:
    /// - <c>Invoice</c> chỉ trả lời duy nhất câu hỏi: "Tổng giá trị hóa đơn là bao nhiêu?".
    /// - <c>InvoicePrinter</c> quyết định: "Hóa đơn được trình bày như thế nào?".
    /// - <c>InvoiceRepository</c> quyết định: "Hóa đơn được lưu trữ ở đâu và bằng cách nào?".
    /// Khi một khía cạnh thay đổi, các phần còn lại hoàn toàn không bị ảnh hưởng, không cần
    /// biên dịch lại (recompile) và không tiềm ẩn rủi ro hỏng hóc chéo (cross-breaking changes).
    /// </para>
    /// <para>
    /// <b>7. Trường hợp áp dụng.</b>
    /// Nên áp dụng SRP khi:
    /// - Một class pha trộn giữa logic nghiệp vụ (business logic) với truy xuất dữ liệu (data access/SQL).
    /// - Một service vừa xử lý nghiệp vụ, vừa gửi email thông báo, vừa ghi log kiểm toán (audit log).
    /// - Một module thường xuyên bị sửa đổi vì những lý do xuất phát từ nhiều bên liên quan khác nhau.
    /// Tuy nhiên, cần tránh bẫy tối ưu hóa sớm (premature optimization): Với các tiện ích nhỏ, script đơn giản
    /// hoặc các chính sách chưa có dấu hiệu biến đổi, việc phân tách quá sớm sẽ làm bùng nổ số lượng class
    /// (class explosion) và gia tăng độ phức tạp không cần thiết.
    /// </para>
    /// <para>
    /// <b>8. Lỗi thường gặp.</b>
    /// - Hiểu lầm SRP thành "mỗi class chỉ được phép chứa một hàm duy nhất".
    /// - Phân rã quá mức dẫn đến các class rỗng không hành vi (Anemic Domain Model) hoặc tách vụn vặt từng dòng code.
    /// - Tạo ra các class gom rác kiểu <c>CommonHelper</c>, <c>Utils</c> chứa đủ loại hàm tiện ích không liên quan.
    /// - Tách rời dữ liệu khỏi các hành vi vốn phải đi liền với nhau để bảo toàn tính bất biến (invariants).
    /// Dấu hiệu cảnh báo vi phạm (code smells): Tên class có chứa từ nối "And" (ví dụ: <c>InvoiceProcessorAndNotifier</c>),
    /// constructor nhận vào quá nhiều dependencies thuộc các tầng kỹ thuật không liên quan, hoặc unit test của class
    /// thường xuyên bị vỡ vì những lý do hoàn toàn không liên quan đến chức năng chính của nó.
    /// </para>
    /// <para>
    /// <b>9. Mối quan hệ với các nguyên lý SOLID tiếp theo.</b>
    /// SRP đóng vai trò định vị ranh giới trách nhiệm. Khi ranh giới giữa các thành phần đã được phân tách rõ ràng:
    /// - OCP (Open/Closed) cho phép mở rộng từng trách nhiệm mà không cần sửa đổi mã nguồn hiện có.
    /// - LSP (Liskov Substitution) đảm bảo các class con kế thừa đúng hành vi đã cam kết.
    /// - ISP (Interface Segregation) giúp tinh gọn các giao diện theo từng nhóm trách nhiệm cụ thể của client.
    /// - DIP (Dependency Inversion) giúp đảo ngược chiều phụ thuộc, tách rời nghiệp vụ cốt lõi khỏi chi tiết hạ tầng.
    /// Do đó, áp dụng đúng SRP là tiền đề quyết định để triển khai thành công 4 nguyên lý còn lại trong SOLID.
    /// </para>
    /// <para>
    /// <b>10. Câu hỏi phỏng vấn và đáp án mẫu.</b>
    /// <b>Câu hỏi 1:</b> "Một class chứa nhiều phương thức thì có chắc chắn vi phạm SRP không?"<br/>
    /// <b>Đáp án:</b> Không. Số lượng phương thức không phải là tiêu chí đánh giá SRP. Một class vẫn hoàn toàn
    /// tuân thủ SRP nếu tất cả phương thức của nó cùng hướng tới việc phục vụ một trách nhiệm duy nhất và chỉ chịu
    /// tác động bởi một actor. Thước đo cốt lõi là "nguồn gốc phát sinh thay đổi", không phải số dòng code hay số lượng hàm.
    /// </para>
    /// <para>
    /// <b>Câu hỏi 2:</b> "Cứ tách một class lớn thành nhiều class nhỏ thì mặc nhiên tuân thủ SRP?"<br/>
    /// <b>Đáp án:</b> Không. Nếu việc phân tách không dựa trên ranh giới thay đổi (reasons to change), ta chỉ đang
    /// tạo ra sự phân mảnh. Các class nhỏ khi đó vẫn có thể phụ thuộc vòng (circular dependencies) hoặc dẫn tới hiện tượng
    /// "Shotgun Surgery" (sửa một yêu cầu phải chỉnh sửa đồng loạt ở nhiều file khác nhau). Việc bóc tách chỉ thực sự có giá trị
    /// khi mỗi module đại diện cho một chính sách nghiệp vụ độc lập và nhất quán.
    /// </para>
    /// </remarks>
    public static class SingleResponsibilityDemo
    {
        /// <summary>
        /// Chạy ví dụ so sánh giữa thiết kế vi phạm và thiết kế tuân thủ SRP.
        /// </summary>
        public static void Run()
        {
            Console.WriteLine("=== S - NGUYÊN LÝ ĐƠN TRÁCH NHIỆM (SRP) ===");

            // 1. Thiết kế vi phạm SRP: Một đối tượng duy nhất phải ôm đồm cả 3 trách nhiệm độc lập.
            var badInvoice = new BadInvoice(100m, 0.10m);
            badInvoice.PrintInvoice();
            badInvoice.SaveToDatabase();

            Console.WriteLine();

            // 2. Thiết kế tuân thủ SRP: Phân tách ranh giới rõ ràng.
            // Dữ liệu và logic tính toán thuộc về domain entity Invoice.
            var invoice = new Invoice(100m, 0.10m);

            // Các service chuyên biệt nhận Invoice để thực thi trách nhiệm riêng của mình (hiển thị và lưu trữ).
            var printer = new InvoicePrinter();
            printer.Print(invoice);

            var repository = new InvoiceRepository();
            repository.Save(invoice);

            Console.WriteLine(
                "Kết luận: Mỗi class giờ đây chỉ có một lý do duy nhất để thay đổi.");
            Console.WriteLine();
        }
    }

    #region Thiết kế vi phạm SRP

    /// <summary>
    /// Phản ví dụ (Anti-pattern): Class hóa đơn ôm đồm cả logic nghiệp vụ, hiển thị và lưu trữ dữ liệu.
    /// </summary>
    /// <remarks>
    /// Việc các phương thức đều thao tác trên "hóa đơn" không đồng nghĩa với việc chúng thuộc cùng một trách nhiệm:
    /// - CalculateTotal: Thay đổi khi quy tắc nghiệp vụ/chính sách thuế thay đổi.
    /// - PrintInvoice: Thay đổi khi định dạng giao diện/mẫu in ấn thay đổi.
    /// - SaveToDatabase: Thay đổi khi công nghệ lưu trữ/cơ sở dữ liệu thay đổi.
    /// Khi bất kỳ khía cạnh nào thay đổi, class này đều bị ảnh hưởng và phải sửa đổi.
    /// </remarks>
    public sealed class BadInvoice
    {
        public decimal Amount { get; }
        public decimal TaxRate { get; }

        public BadInvoice(decimal amount, decimal taxRate)
        {
            Amount = amount;
            TaxRate = taxRate;
        }

        public decimal CalculateTotal()
        {
            return Amount + (Amount * TaxRate);
        }

        public void PrintInvoice()
        {
            Console.WriteLine($"[Chưa tốt] Tổng hóa đơn: {CalculateTotal():C}");
        }

        public void SaveToDatabase()
        {
            Console.WriteLine("[Chưa tốt] Đang lưu hóa đơn vào cơ sở dữ liệu...");
        }
    }

    #endregion

    #region Thiết kế tuân thủ SRP

    /// <summary>
    /// Domain Entity đại diện cho hóa đơn: Chỉ chịu trách nhiệm quản lý dữ liệu và tính toán tổng tiền.
    /// </summary>
    public sealed class Invoice
    {
        public decimal Amount { get; }
        public decimal TaxRate { get; }

        /// <summary>
        /// Khởi tạo hóa đơn và bảo đảm tính toàn vẹn dữ liệu (domain invariants).
        /// </summary>
        /// <param name="amount">Số tiền trước thuế (không được âm).</param>
        /// <param name="taxRate">Thuế suất hợp lệ (từ 0.0 đến 1.0, tương đương 0% đến 100%).</param>
        public Invoice(decimal amount, decimal taxRate)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(amount),
                    "Số tiền hóa đơn không được âm.");
            }

            if (taxRate is < 0 or > 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(taxRate),
                    "Thuế suất phải thuộc đoạn từ 0 đến 1.");
            }

            Amount = amount;
            TaxRate = taxRate;
        }

        /// <summary>
        /// Tính tổng tiền hóa đơn sau thuế theo quy tắc nghiệp vụ: tiền trước thuế + tiền thuế.
        /// </summary>
        public decimal CalculateTotal()
        {
            return Amount + (Amount * TaxRate);
        }
    }

    /// <summary>
    /// Service đảm nhiệm duy nhất trách nhiệm định dạng và in ấn/hiển thị hóa đơn.
    /// </summary>
    /// <remarks>
    /// Khi cần thay đổi định dạng in, ngôn ngữ hiển thị hoặc thiết bị xuất (Console, PDF, HTML),
    /// ta chỉ cần chỉnh sửa class này mà không gây bất kỳ tác động nào tới logic nghiệp vụ trong Invoice.
    /// </remarks>
    public sealed class InvoicePrinter
    {
        public void Print(Invoice invoice)
        {
            ArgumentNullException.ThrowIfNull(invoice);
            Console.WriteLine(
                $"[Tốt] In hóa đơn; tổng tiền: {invoice.CalculateTotal():C}");
        }
    }

    /// <summary>
    /// Repository đảm nhiệm duy nhất trách nhiệm lưu trữ (persistence) hóa đơn.
    /// </summary>
    /// <remarks>
    /// Trong hệ thống thực tế, class này sẽ tương tác trực tiếp với cơ sở dữ liệu (ORM, SQL, NoSQL).
    /// Mọi thay đổi về cấu trúc bảng hoặc công nghệ lưu trữ đều được cô lập tại đây, hoàn toàn độc lập
    /// với logic nghiệp vụ hay giao diện.
    /// </remarks>
    public sealed class InvoiceRepository
    {
        public void Save(Invoice invoice)
        {
            ArgumentNullException.ThrowIfNull(invoice);
            Console.WriteLine(
                $"[Tốt] Lưu hóa đơn {invoice.CalculateTotal():C} vào kho dữ liệu.");
        }
    }

    #endregion
}
