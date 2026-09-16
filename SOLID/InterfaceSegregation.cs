using System;

namespace SoftwareDesignPrinciples.SOLID
{
    /// <summary>
    /// Minh họa Interface Segregation Principle (ISP - Nguyên lý phân tách giao diện)
    /// qua bài toán quản lý người lao động và robot.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <b>1. Bài toán thực tế.</b>
    /// Một hệ thống quản lý nhân sự ban đầu chỉ phục vụ con người, nên interface IWorker
    /// được thiết kế gộp chung các hành vi Work, Eat và Sleep. Khi đưa robot vào vận hành,
    /// robot chỉ có thể làm việc mà không cần ăn hay ngủ. Nếu ép RobotWorker phải implement
    /// toàn bộ IWorker, lớp này buộc phải viết các phương thức rỗng vô nghĩa hoặc ném ngoại lệ
    /// (NotSupportedException). Khi đó, interface đang được thiết kế nhằm phục vụ sự tiện lợi
    /// của bên cung cấp (provider) thay vì xuất phát từ nhu cầu thực tế của bên sử dụng (client).
    /// </para>
    /// <para>
    /// <b>2. Định nghĩa.</b>
    /// ISP phát biểu rằng: Client không nên bị ép buộc phải phụ thuộc vào những phương thức
    /// mà nó không sử dụng. Thay vì thiết kế một "fat interface" (giao diện cồng kềnh/quá tải)
    /// gom toàn bộ năng lực của một đối tượng, interface nên được phân rã thành các giao diện
    /// nhỏ, tinh gọn theo từng vai trò (role interface) hoặc theo nhu cầu cụ thể của từng client.
    /// </para>
    /// <para>
    /// <b>3. Bản chất.</b>
    /// Khái niệm "phụ thuộc" không chỉ dừng lại ở việc gọi phương thức. Khi một client tham chiếu
    /// đến một interface, bất kỳ thay đổi nào trên interface đó (dù client không hề dùng tới)
    /// đều có thể kéo theo việc client phải biên dịch, kiểm thử và triển khai lại (recompile/retest/redeploy).
    /// Bề mặt hợp đồng (contract surface) càng rộng thì bán kính ảnh hưởng khi có thay đổi càng lớn.
    /// ISP giúp giảm thiểu độ phụ thuộc (loose coupling) bằng cách chỉ để lộ đúng những năng lực (capabilities)
    /// mà client thực sự cần.
    /// </para>
    /// <para>
    /// <b>4. Cơ chế giải quyết.</b>
    /// Interface tổng hợp IWorker được tách thành ba role interface độc lập: IWorkable, IFeedable
    /// và ISleepable. Lớp HumanWorker implement cả ba interface vì con người có đầy đủ các năng lực này.
    /// Ngược lại, RobotWorker chỉ cần implement IWorkable. Phương thức ProcessShift chỉ yêu cầu tham số
    /// IWorkable, còn ServeLunch chỉ nhận IFeedable. Nhờ đó, chữ ký (signature) của mỗi phương thức client
    /// mô tả chính xác và tối thiểu ràng buộc mà nó đòi hỏi.
    /// </para>
    /// <para>
    /// <b>5. Cú pháp C#.</b>
    /// Trong C#, một lớp có thể implement nhiều interface bằng danh sách phân cách bởi dấu phẩy:
    /// “HumanWorker : IWorkable, IFeedable, ISleepable”. Mỗi interface ở đây đóng vai trò là một
    /// role interface. C# cũng cho phép một interface kế thừa các interface khác, nhưng chúng ta chỉ
    /// nên gộp hoặc kế thừa hợp đồng khi giữa chúng thực sự tồn tại mối quan hệ ngữ nghĩa chặt chẽ.
    /// </para>
    /// <para>
    /// <b>6. Phân biệt nguyên lý và kỹ thuật.</b>
    /// ISP là nguyên lý kiến trúc định hướng việc quản lý phụ thuộc, còn chia nhỏ interface chỉ là kỹ thuật
    /// thực hiện. Một interface chứa 10 phương thức vẫn hoàn toàn hợp lý nếu mọi client đều thực sự cần
    /// cả 10 phương thức đó (tính gắn kết cao - high cohesion). Ngược lại, một interface chỉ có 2 phương thức
    /// vẫn có thể vi phạm ISP nếu hai nhóm client khác nhau chỉ dùng riêng rẽ từng phương thức và chúng thay
    /// đổi độc lập với nhau.
    /// </para>
    /// <para>
    /// <b>7. Phân tích ví dụ.</b>
    /// BadRobotWorker biên dịch thành công vì cung cấp đủ chữ ký phương thức, nhưng các hành vi Eat và Sleep
    /// lại hoàn toàn phi ngữ nghĩa đối với robot. Điều này vừa vi phạm ISP, vừa gián tiếp làm suy yếu Liskov
    /// Substitution Principle (LSP): đối tượng không thể thay thế an toàn cho IBadWorker mà không gây lỗi runtime.
    /// Sau khi phân tách interface, hệ thống kiểu tĩnh (static typing) của C# sẽ ngăn ServeLunch nhận RobotWorker;
    /// lỗi thiết kế được phát hiện ngay từ thời điểm biên dịch (compile-time) thay vì chờ đến khi vận hành (runtime).
    /// </para>
    /// <para>
    /// <b>8. Trường hợp áp dụng.</b>
    /// ISP đặc biệt quan trọng khi thiết kế Public API, Service Interface, Repository, kiến trúc Plugin hoặc
    /// khi tích hợp các thiết bị/phần cứng có năng lực phân tầng. Mỗi use case chỉ nên phụ thuộc vào một
    /// cổng giao tiếp (interface) hẹp nhất vừa đủ dùng. Thiết kế này cũng giúp việc tạo mock/stub (test double)
    /// trong kiểm thử tự động (Unit Test) trở nên đơn giản hơn, vì bài test không phải giả lập những hành vi
    /// không liên quan.
    /// </para>
    /// <para>
    /// <b>9. Lỗi thường gặp.</b>
    /// Các sai lầm phổ biến gồm có: tạo interface "God Object" (ví dụ: IAppService ôm đồm toàn bộ nghiệp vụ);
    /// thêm phương thức mới vào interface dùng chung khiến nhiều implementation buộc phải ném NotImplementedException;
    /// truyền cả một đối tượng toàn năng (read/write/delete) khi client chỉ có nhu cầu đọc (read-only);
    /// hoặc ngược lại, chia cắt thái quá thành hàng loạt interface đơn lẻ 1 phương thức thiếu tính gắn kết.
    /// Cần nhóm phương thức dựa trên sự gắn kết nghiệp vụ (cohesion) và nhu cầu thực tế của client, không chạy
    /// theo số lượng phương thức một cách máy móc.
    /// </para>
    /// <para>
    /// <b>10. Quan hệ với SRP, LSP và DIP.</b>
    /// SRP xem xét lý do thay đổi của một module (Single Reason to Change); trong khi ISP xem xét bề mặt
    /// hợp đồng mà client phải phụ thuộc. Việc phân tách interface nhỏ gọn, đúng vai trò giúp các lớp triển khai
    /// dễ dàng tuân thủ LSP (không phải từ chối hành vi), đồng thời cung cấp các abstraction vừa vặn để áp dụng
    /// Dependency Inversion Principle (DIP) một cách hiệu quả.
    /// </para>
    /// <para>
    /// <b>11. Câu hỏi phỏng vấn và đáp án mẫu.</b>
    /// Câu hỏi: “ISP có bắt buộc mỗi interface chỉ được có một phương thức không?”
    /// Đáp án: Không. ISP không quy định số lượng phương thức, mà đòi hỏi tính gắn kết cao: mọi phương thức
    /// trong interface phải cùng phục vụ một vai trò cụ thể mà client mong đợi. Số lượng phương thức là hệ quả
    /// của mô hình nghiệp vụ, không phải tiêu chí đánh giá độc lập.
    /// </para>
    /// <para>
    /// Câu hỏi: “Ai là bên quyết định ranh giới của một interface?”
    /// Đáp án: Nhu cầu của client (bên tiêu thụ) quyết định. Thay vì xuất phát từ toàn bộ khả năng của lớp
    /// triển khai (provider), hãy tiếp cận theo hướng Client-Driven: xuất phát từ từng use case của client
    /// để công bố hợp đồng tối thiểu, vừa đủ để use case đó hoàn thành nhiệm vụ.
    /// </para>
    /// </remarks>
    public static class InterfaceSegregationDemo
    {
        /// <summary>
        /// Minh họa sự khác biệt giữa "fat interface" cồng kềnh và các "role interface" phân tách tinh gọn theo năng lực.
        /// </summary>
        public static void Run()
        {
            Console.WriteLine("=== I - NGUYÊN LÝ PHÂN TÁCH GIAO DIỆN (ISP) ===");

            IBadWorker badRobot = new BadRobotWorker();
            badRobot.Work();

            try
            {
                // Interface quá rộng cho phép gọi phương thức này dù đối tượng thực tế không hỗ trợ.
                badRobot.Eat();
            }
            catch (NotSupportedException exception)
            {
                Console.WriteLine($"[Chưa tốt] {exception.Message}");
            }

            Console.WriteLine();

            var human = new HumanWorker();
            var robot = new RobotWorker();

            // ProcessShift chỉ phụ thuộc vào IWorkable (năng lực làm việc) nên nhận được cả con người và robot.
            ProcessShift(human);
            ProcessShift(robot);

            // ServeLunch chỉ nhận đối tượng có hợp đồng ăn uống (IFeedable). Do RobotWorker không implement
            // IFeedable, trình biên dịch sẽ chặn ngay từ đầu, loại bỏ hoàn toàn khả năng lỗi lúc chạy
            // (make illegal states unrepresentable).
            ServeLunch(human);

            Console.WriteLine(
                "Kết luận: Mỗi client chỉ nên phụ thuộc vào những interface chứa năng lực mà nó thực sự sử dụng.");
            Console.WriteLine();
        }

        private static void ProcessShift(IWorkable worker)
        {
            worker.Work();
        }

        private static void ServeLunch(IFeedable worker)
        {
            worker.Eat();
        }
    }

    #region Thiết kế vi phạm ISP

    /// <summary>
    /// Fat interface gom nhiều năng lực không liên quan, khiến không phải implementation nào cũng hiện thực được trọn vẹn.
    /// </summary>
    public interface IBadWorker
    {
        void Work();
        void Eat();
        void Sleep();
    }

    public sealed class BadHumanWorker : IBadWorker
    {
        public void Work()
        {
            Console.WriteLine("[Chưa tốt] Con người đang làm việc.");
        }

        public void Eat()
        {
            Console.WriteLine("[Chưa tốt] Con người đang dùng bữa.");
        }

        public void Sleep()
        {
            Console.WriteLine("[Chưa tốt] Con người đang ngủ.");
        }
    }

    /// <summary>
    /// Ví dụ vi phạm: Lớp này bị ép phải implement các phương thức mà bản thân không hỗ trợ chỉ để thỏa mãn interface.
    /// </summary>
    public sealed class BadRobotWorker : IBadWorker
    {
        public void Work()
        {
            Console.WriteLine("[Chưa tốt] Robot đang làm việc.");
        }

        public void Eat()
        {
            throw new NotSupportedException("Robot không có khả năng ăn uống.");
        }

        public void Sleep()
        {
            throw new NotSupportedException("Robot không có nhu cầu ngủ nghỉ.");
        }
    }

    #endregion

    #region Thiết kế tuân thủ ISP

    /// <summary>
    /// Role interface đại diện cho năng lực làm việc, phục vụ các client chỉ quan tâm đến tác vụ lao động.
    /// </summary>
    public interface IWorkable
    {
        void Work();
    }

    /// <summary>
    /// Role interface đại diện cho năng lực nạp năng lượng/ăn uống, phục vụ các client quản lý bữa ăn.
    /// </summary>
    public interface IFeedable
    {
        void Eat();
    }

    /// <summary>
    /// Role interface đại diện cho năng lực nghỉ ngơi, phục vụ các client quản lý thời gian hồi phục/nghỉ ngơi.
    /// </summary>
    public interface ISleepable
    {
        void Sleep();
    }

    /// <summary>
    /// Con người sở hữu cả ba năng lực nên implement đầy đủ cả ba role interface tương ứng.
    /// </summary>
    public sealed class HumanWorker : IWorkable, IFeedable, ISleepable
    {
        public void Work()
        {
            Console.WriteLine("[Tốt] Con người đang làm việc.");
        }

        public void Eat()
        {
            Console.WriteLine("[Tốt] Con người đang dùng bữa.");
        }

        public void Sleep()
        {
            Console.WriteLine("[Tốt] Con người đang ngủ.");
        }
    }

    /// <summary>
    /// Robot chỉ công bố năng lực mà nó thực sự hỗ trợ, không bị ép cài đặt các phương thức vô nghĩa.
    /// </summary>
    public sealed class RobotWorker : IWorkable
    {
        public void Work()
        {
            Console.WriteLine("[Tốt] Robot đang làm việc.");
        }
    }

    #endregion
}
