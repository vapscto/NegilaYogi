using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.IVRM
{
    public class PortalMonthEndReportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long IVRMRT_Id { get; set; }
        public int month { get; set; }
        public string year { get; set; }
        public string Role_flag { get; set; }
        public string countflag { get; set; }
        public string countflaguc { get; set; }
        public string countflaglc { get; set; }
        public string Moduleflag { get; set; }
        public string roleflag { get; set; }
        public Array roletype { get; set; }
        public Array acayear { get; set; }
        public Array Month_array { get; set; }
        public Array get_monthendreport_uc { get; set; }
        public Array get_monthendreport_lc { get; set; }
        public Array get_monthendreport { get; set; }
        public string Allcontent { get; set; }
        public string allcount { get; set; }
        public string studentflg { get; set; }
        public string staffflg { get; set; }
        public string principalflg { get; set; }
        public string chairmanflg { get; set; }
        public string Managerflg { get; set; }
        public string portalflg { get; set; }
        public string mobileappflg { get; set; }
        public string usercountflg { get; set; }
        public string logincountflg { get; set; }
        public string kioskflg { get; set; }

    }
}