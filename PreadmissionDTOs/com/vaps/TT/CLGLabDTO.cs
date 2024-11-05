using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class CLGLabDTO
    {
        public string classname;
        public string sectionname;
        public object staffname;
        public object subjectname;

        public string returnduplicatestatus { get; set; }

        public bool returnval { get; set; }

        public long TTLAB_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long TTMC_Id { get; set; }
        public string TTLAB_LABLIBName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool TTLAB_ActiveFlag { get; set; }
        public Array catelist { get; set; }
        public Array academiclist { get; set; }
        public Array classDrpDwn { get; set; }
        public Array sectDrpDwn { get; set; }

        public Array staffDrpDwn { get; set; }
        public Array ttsujectslist { get; set; }
        public Array subjDrpDwn { get; set; }
        public Array labconsedit { get; set; }
        public Array labconsdetailsedit { get; set; }
        public Array labdetailsarray { get; set; }
        public Array sectionlist { get; set; }
        public CLGLabDTO[] TempararyArrayList { get; set; }
        public string ASMAYYear { get; set; }
        public string CategoryName { get; set; }
        public int TTLABD_Id { get; set; }
        public int ASMAY_Order { get; set; }
        public Array labdetilspopuparray { get; set; }
        public Array classbycategory { get; set; }
        public long ASMCL_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string TTMC_CategoryName { get; set; }
        public long HRME_Id { get; set; }
        public string staffNamelst { get; set; }
        public long ACMS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public string academicyr { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMSE_SEMName { get; set; }
        public string ASMAY_Year { get; set; }
    }
}
