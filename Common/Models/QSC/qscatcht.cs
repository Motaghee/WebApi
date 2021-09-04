namespace Common.Models.QSC
{
    public class qscatcht
    {
        public int Srl { get; set; }
        public string attache { get; set; }
        public string title { get; set; }
        public int createdby { get; set; }
        public string extension { get; set; }
        public int qcsreqt_srl { get; set; }
        public int Inuse { get; set; }

        //select srl, attache, title, createdby, extension, qcsreqt_srl, inuse from qscatcht ; --جدول پیوست ها
    }
}