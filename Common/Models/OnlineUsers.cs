namespace Common.Models
{
    public class OnlineUsers
    {
        public int Id { get; set; }
        public double UserSRL { get; set; } //double
        public int UserId { get; set; }
        public string DateFa { get; set; }
        public string DateTimeFa { get; set; }
        public string Time { get; set; }
        public string Message { get; set; }
        public int DataType { get; set; } //0-login //1-GetTime //2-chat
        public string LoginDateTimeFa { get; set; }
    }
}