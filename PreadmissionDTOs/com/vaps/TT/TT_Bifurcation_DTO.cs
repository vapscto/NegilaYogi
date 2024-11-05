using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class TT_Bifurcation_DTO
    {
        public long TTB_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long TTMC_Id { get; set; }
        public string TTB_BifurcationName { get; set; }      
        public int TTB_NoOfPeriods { get; set; }
        public int TTB_AllotPeriods { get; set; }
        public int TTB_RemPeriods { get; set; }
        public int TTB_ConsecutiveFlag { get; set; }
        public int TTB_NoOfConPeriods { get; set; }
        public int TTB_NoOfConDays { get; set; }
        public int TTB_BefAftApplFlag { get; set; }
        public string TTB_BefAftFalg { get; set; }
        public int TTMP_Id { get; set; }
        public string TTB_AllotedFlag { get; set; }
        public bool TTB_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long userId { get; set; }
        public long roleId { get; set; }
        public Array categorylist { get; set; }
        public Array acdlist { get; set; }
        public Array detailslist { get; set; }
        public Array editdetailslist { get; set; }
        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
        public Array subjectlist { get; set; }
        public Array periodlist { get; set; }
        public Array stafflist { get; set; }
        public TT_Bifurcation_Details_DTO[] combinationlist { get; set; }
        public string returnMsg { get; set; }
        public string AcdYear { get; set; }
        public string periodname { get; set; }
        public string className { get; set; }
        public string categoryName { get; set; }
        public string sectioname { get; set; }
        public string staffname { get; set; }
        public string subjectname { get; set;}
        public string bifricationName { get; set; }
        public long TTBD_Id { get; set; }
        public Array viewdata { get; set; }
        public string returnduplicatestatus { get; set; }

    }
}
