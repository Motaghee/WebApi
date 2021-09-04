namespace Common.Models.QSC
{
    public class qscordet
    {
        public int Srl { get; set; }
        public string OriginDesc { get; set; }
        public int Inuse { get; set; }

        //select srl, origindesc, inuse from qscordet ;--جدول علل شناسايي در زمان بررسي کيفيت
    }
}