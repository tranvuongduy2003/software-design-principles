using System;

namespace SoftwareDesignPrinciples.SOLID
{
    /// <summary>
    /// Minh họa Liskov Substitution Principle (LSP - Nguyên lý thay thế Liskov)
    /// thông qua bài toán kinh điển về các loài chim có và không có khả năng bay.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <b>1. Bài toán thực tế.</b>
    /// Trong đời sống, quan niệm “chim thì biết bay” có vẻ hiển nhiên, nhưng trong kỹ thuật phần mềm
    /// thì đây là một giả định sai lầm. Nếu lớp cơ sở <c>Bird</c> định nghĩa phương thức <c>Fly</c>,
    /// nó ngầm áp đặt rằng mọi lớp con kế thừa từ nó đều phải có khả năng bay. Khi đó, lớp
    /// <c>Penguin</c> (chim cánh cụt) buộc phải ném <c>NotSupportedException</c>. Hệ quả là client code
    /// (mã tiêu thụ) dù đang làm việc với một đối tượng <c>Bird</c> hợp lệ nhưng vẫn có nguy cơ bị sập
    /// (crash) tại runtime khi gọi một hành vi mà lớp cha đã cam kết trong hợp đồng.
    /// </para>
    /// <para>
    /// <b>2. Định nghĩa.</b>
    /// LSP phát biểu rằng: Nếu S là một subtype (kiểu con) của T, thì các đối tượng kiểu T trong chương
    /// trình phải có thể được thay thế bằng các đối tượng kiểu S mà không làm thay đổi tính đúng đắn của
    /// chương trình. Nói cách khác, kiểu con phải kế thừa và bảo toàn trọn vẹn behavioral contract
    /// (hợp đồng hành vi) của kiểu cha, chứ không đơn thuần chỉ là tương thích về mặt cú pháp hay chữ ký
    /// phương thức (method signature).
    /// </para>
    /// <para>
    /// <b>3. Bản chất của hợp đồng hành vi (Design by Contract).</b>
    /// Hợp đồng của một kiểu bao gồm preconditions (tiền điều kiện), postconditions (hậu điều kiện),
    /// invariants (bất biến) và các side effects (tác dụng phụ) có thể quan sát được. Cụ thể:
    /// - Kiểu con không được thắt chặt tiền điều kiện (không được đòi hỏi khắt khe hơn kiểu cha).
    /// - Kiểu con không được làm suy yếu hậu điều kiện (phải bảo đảm ít nhất những gì kiểu cha đã cam kết).
    /// - Kiểu con phải duy trì toàn bộ tính bất biến của kiểu cha.
    /// - Kiểu con không được ném ra ngoại lệ mới đối với những trường hợp mà kiểu cha xử lý hợp lệ.
    /// </para>
    /// <para>
    /// <b>4. Dấu hiệu nhận diện vi phạm.</b>
    /// Vi phạm LSP thường bộc lộ rõ nhất ở client code: Nếu mã khách chỉ làm việc với kiểu cơ sở nhưng
    /// lại phải dùng các lệnh kiểm tra kiểu cụ thể (<c>is</c>, <c>as</c>, <c>switch</c>) để né tránh một lớp
    /// con nào đó, hoặc phải bọc khối <c>try-catch</c> để bắt <c>NotSupportedException</c>, thì mối quan hệ
    /// kế thừa đó chắc chắn đang vi phạm LSP.
    /// </para>
    /// <para>
    /// <b>5. Giải pháp thiết kế.</b>
    /// Tách abstraction dựa trên capability (năng lực): Lớp cơ sở <c>IBird</c> chỉ cam kết hành vi phổ
    /// quát chung cho mọi loài chim là <c>Move</c> (di chuyển). Năng lực bay (<c>Fly</c>) được tách thành
    /// một interface chuyên biệt hơn là <c>IFlyableBird</c>. <c>Eagle</c> (đại bàng) triển khai cả hai
    /// năng lực di chuyển và bay; trong khi <c>Penguin</c> chỉ triển khai di chuyển. Cách tiếp cận này
    /// giúp lớp cơ sở tránh bị áp đặt những hành vi cục bộ, bảo đảm tính phổ quát cho toàn bộ hệ thống phân cấp.
    /// </para>
    /// <para>
    /// <b>6. Cú pháp và cơ chế trong C#.</b>
    /// Khai báo <c>IFlyableBird : IBird</c> biểu thị một interface chuyên biệt kế thừa từ một hợp đồng
    /// tổng quát. Biến kiểu <c>IBird</c> có thể giữ tham chiếu tới cả <c>Eagle</c> lẫn <c>Penguin</c> và
    /// gọi <c>Move()</c> một cách an toàn; trong khi biến kiểu <c>IFlyableBird</c> chỉ chấp nhận những đối
    /// tượng thực sự có năng lực <c>Fly()</c>. Trình biên dịch sẽ ngăn chặn ngay lập tức lời gọi <c>Fly()</c>
    /// trên <c>IBird</c>, chuyển một lỗi tiềm ẩn nguy hiểm lúc runtime thành lỗi kiểm tra kiểu tại compile-time.
    /// </para>
    /// <para>
    /// <b>7. Phân tích ví dụ.</b>
    /// Trong mô hình chưa tối ưu, <c>BadEagle</c> thay thế <c>BadBird</c> thành công, nhưng <c>BadPenguin</c>
    /// lại khiến <c>Fly()</c> ném ngoại lệ. Vấn đề cốt lõi không nằm ở câu lệnh <c>throw</c>, mà nằm ở việc
    /// thiết kế abstraction sai lệch: <c>BadBird</c> đã đưa ra một lời hứa quá rộng so với thực tế. Ở thiết
    /// kế chuẩn hóa, phương thức <c>MakeBirdMove</c> nhận <c>IBird</c> và hoạt động chính xác với mọi loài
    /// chim; còn <c>MakeBirdFly</c> nhận <c>IFlyableBird</c> nên loại trừ hoàn toàn <c>Penguin</c> ngay từ đầu.
    /// </para>
    /// <para>
    /// <b>8. Mối liên hệ với OCP và ISP.</b>
    /// LSP là nền tảng sống còn của OCP (Open/Closed Principle): OCP chỉ phát huy tác dụng khi các lớp con
    /// mới bổ sung không phá vỡ hợp đồng hiện có. Nếu một subtype vi phạm LSP, điểm mở rộng sẽ trở nên bất
    /// ổn và khiến OCP sụp đổ. Đồng thời, việc tách riêng năng lực bay thành interface độc lập cũng là sự áp
    /// dụng trực tiếp của ISP (Interface Segregation Principle) — phân tách giao diện để bảo toàn khả năng thay
    /// thế hoàn hảo, thay vì ép buộc các lớp con phải gánh vác các phương thức dư thừa.
    /// </para>
    /// <para>
    /// <b>9. Phạm vi áp dụng thực tế.</b>
    /// Cần kiểm chứng LSP khi: thiết kế hệ thống phân cấp kế thừa (inheritance hierarchy), triển khai interface,
    /// thay thế repository thật (database) bằng repository giả lập (mock/in-memory) trong kiểm thử tự động,
    /// hoặc tích hợp nhiều nhà cung cấp dịch vụ (third-party providers). Cần lưu ý rằng hợp đồng không chỉ là
    /// chữ ký phương thức, mà phải bao gồm quy ước xử lý <c>null</c>, miền giá trị đầu vào/ra, thứ tự dữ liệu,
    /// các ngoại lệ dự kiến và hiệu ứng phụ (side effects).
    /// </para>
    /// <para>
    /// <b>10. Các lỗi vi phạm thường gặp (Code Smells).</b>
    /// - Lớp con ném <c>NotSupportedException</c> hoặc <c>NotImplementedException</c> đối với phương thức kế thừa từ lớp cha.
    /// - Ghi đè phương thức nhưng để trống (no-op) hoặc vô hiệu hóa hành vi vì lớp con không hỗ trợ.
    /// - Lớp con thắt chặt điều kiện đầu vào hoặc trả về kết quả vượt ngoài miền cam kết của lớp cha.
    /// - Làm thay đổi trạng thái trái với tính bất biến (invariants) mà lớp cha luôn duy trì.
    /// - Client code liên tục phải ép kiểu hoặc kiểm tra <c>if (x is SpecificType)</c> trước khi thao tác.
    /// - Lạm dụng kế thừa chỉ để tái sử dụng mã (code reuse) mà bỏ qua bản chất quan hệ “is-a” về mặt hành vi.
    /// </para>
    /// <para>
    /// <b>11. Câu hỏi phỏng vấn thường gặp.</b>
    /// <b>Câu hỏi 1:</b> Tương thích về chữ ký phương thức (method signature) đã đủ để đảm bảo tuân thủ LSP chưa?<br/>
    /// <b>Trả lời:</b> Chưa đủ. Chữ ký phương thức chỉ đảm bảo tương thích cú pháp (syntactic compatibility)
    /// tại thời điểm biên dịch. LSP đòi hỏi sự tương thích về mặt ngữ nghĩa hành vi (behavioral compatibility):
    /// tiền điều kiện, hậu điều kiện, tính bất biến, các ngoại lệ phát sinh và hiệu ứng phụ của lớp con đều
    /// phải nhất quán với những gì lớp cha đã cam kết.
    /// </para>
    /// <para>
    /// <b>Câu hỏi 2:</b> Việc ném <c>NotSupportedException</c> có phải lúc nào cũng vi phạm LSP không?<br/>
    /// <b>Trả lời:</b> Không hẳn. Nếu ngay từ đầu hợp đồng ở lớp cơ sở đã tuyên bố rõ ràng rằng thao tác đó
    /// là tùy chọn (optional) và có thể ném ngoại lệ (ví dụ: <see cref="System.IO.Stream.Write"/> đi kèm thuộc tính
    /// kiểm tra <see cref="System.IO.Stream.CanWrite"/>), thì ngoại lệ đó đã là một phần của hợp đồng. Ngược lại,
    /// nếu lớp cha ngầm định hành vi luôn luôn khả thi (như <c>Fly()</c> trong ví dụ), việc lớp con từ chối thực
    /// hiện và ném ngoại lệ chắc chắn là vi phạm LSP, chứng tỏ tầng trừu tượng đã bị thiết kế quá rộng.
    /// </para>
    /// </remarks>
    public static class LiskovSubstitutionDemo
    {
        /// <summary>
        /// Thực thi phản ví dụ vi phạm LSP và chứng minh tính thay thế an toàn trong thiết kế chuẩn.
        /// </summary>
        public static void Run()
        {
            Console.WriteLine("=== L - NGUYÊN LÝ THAY THẾ LISKOV (LSP) ===");

            BadBird eagleAsBadBird = new BadEagle();
            eagleAsBadBird.Fly();

            BadBird penguinAsBadBird = new BadPenguin();
            try
            {
                // Client code chỉ tương tác qua kiểu cha BadBird và kỳ vọng rằng phương thức
                // Fly() luôn thực thi hợp lệ. Tuy nhiên, lớp con đã phá vỡ hợp đồng lúc runtime.
                penguinAsBadBird.Fly();
            }
            catch (NotSupportedException exception)
            {
                Console.WriteLine($"[Chưa tốt] {exception.Message}");
            }

            Console.WriteLine();

            IBird eagle = new Eagle();
            IBird penguin = new Penguin();

            // Cùng một hàm MakeBirdMove hoạt động đồng nhất và an toàn với mọi subtype của IBird.
            MakeBirdMove(eagle);
            MakeBirdMove(penguin);

            // Đảm bảo an toàn kiểu tại compile-time: Chỉ kiểu nào thực sự cam kết năng lực bay mới được truyền vào.
            MakeBirdFly(new Eagle());

            Console.WriteLine(
                "Kết luận: Mỗi kiểu con đều bảo toàn trọn vẹn hợp đồng hành vi mà nó triển khai.");
            Console.WriteLine();
        }

        private static void MakeBirdMove(IBird bird)
        {
            bird.Move();
        }

        private static void MakeBirdFly(IFlyableBird bird)
        {
            bird.Fly();
        }
    }

    #region Thiết kế vi phạm LSP

    /// <summary>
    /// Lớp cơ sở thiết kế sai khi ngầm giả định mọi loài chim đều có khả năng bay.
    /// </summary>
    public class BadBird
    {
        public virtual void Fly()
        {
            Console.WriteLine("Chim đang bay.");
        }
    }

    /// <summary>
    /// Kiểu con này ngẫu nhiên tương thích với giả định của BadBird vì đại bàng thực sự bay được.
    /// </summary>
    public sealed class BadEagle : BadBird
    {
        public override void Fly()
        {
            Console.WriteLine("Đại bàng bay cao.");
        }
    }

    /// <summary>
    /// Kiểu con phá vỡ hợp đồng của BadBird do từ chối thực hiện một hành vi mà lớp cha đã cam kết.
    /// </summary>
    public sealed class BadPenguin : BadBird
    {
        public override void Fly()
        {
            throw new NotSupportedException("Chim cánh cụt không thể bay.");
        }
    }

    #endregion

    #region Thiết kế tuân thủ LSP

    /// <summary>
    /// Hợp đồng cơ sở định nghĩa hành vi phổ quát, luôn đúng và an toàn cho mọi loài chim.
    /// </summary>
    public interface IBird
    {
        void Move();
    }

    /// <summary>
    /// Hợp đồng chuyên biệt hóa dành riêng cho các loài chim có năng lực bay.
    /// </summary>
    public interface IFlyableBird : IBird
    {
        void Fly();
    }

    /// <summary>
    /// Đại bàng đáp ứng trọn vẹn cả hợp đồng di chuyển (IBird) và hợp đồng bay (IFlyableBird).
    /// </summary>
    public sealed class Eagle : IFlyableBird
    {
        public void Move()
        {
            Console.WriteLine("[Tốt] Đại bàng di chuyển bằng chân.");
        }

        public void Fly()
        {
            Console.WriteLine("[Tốt] Đại bàng bay cao.");
        }
    }

    /// <summary>
    /// Chim cánh cụt chỉ triển khai hợp đồng IBird mà nó thực sự có thể đáp ứng.
    /// </summary>
    public sealed class Penguin : IBird
    {
        public void Move()
        {
            Console.WriteLine("[Tốt] Chim cánh cụt đi lạch bạch hoặc bơi.");
        }
    }

    #endregion
}
