namespace Common.Models.General
{
    public class SMessage
    {
        public string Id { get; set; }
        public int SenderUserId { get; set; }
        public string SenderUserDesc { get; set; }
        public string CreatedDateTime { get; set; }
        public string CreatedDateTimeFa { get; set; }
        public string Message { get; set; }
        public int SendToUserId { get; set; }

    }
}