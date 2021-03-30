namespace Common.Models.General
{
    public class MessageCount
    {
        public string InsDateFa { get; set; }
        public double QCMsgCnt { get; set; } = 0;
        public double PTMsgCnt { get; set; } = 0;
        public double SPMsgCnt { get; set; } = 0;
        public double AuditMsgCnt { get; set; } = 0;
    }
}