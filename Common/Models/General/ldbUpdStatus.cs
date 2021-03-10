using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models.General
{
    public class ldbUpdStatus
    {
        public string Id { get; set; }
        public string ArchiveCompanyProductStatistics_UDate { get; set; }
        public string ArchiveGroupProductStatistics_UDate { get; set; }
        public string ArchiveQCStatistics_UDate { get; set; }
        public string ArchiveAuditStatistics_UDate { get; set; }
        public string ArchiveAuditStatisticsMDTrend_UDate { get; set; }
        public string ArchiveGenerateQCmdDPU { get; set; }
        public string ArchiveASPQCCASTT { get; set; }
    }
}
