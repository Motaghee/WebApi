namespace Common.Models.QSC
{
    public class qscbdmdt
    {
        public int Srl { get; set; }
        public string ModuleName { get; set; }
        public int Inuse { get; set; }

        //select srl, modulename, inuse from qscbdmdt for update; -- لیست قطعات بدنه و قطعات جانبی
    }
}