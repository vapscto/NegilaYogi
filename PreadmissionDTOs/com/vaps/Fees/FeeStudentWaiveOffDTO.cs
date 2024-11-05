using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeStudentWaiveOffDTO
    {
        public long FSWO_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime FSWO_Date { get; set; }
        public long FMG_Id { get; set; }
        public long FMH_Id { get; set; }
        public long FTI_Id { get; set; }
        public long FMA_Id { get; set; }
        public long FSWO_WaivedOffAmount { get; set; }
        public bool FSWO_ActiveFlag { get; set; }
        
        public Array fillyear { get; set; }
        public Array fillstudent { get; set; }
        public Array fillclass { get; set; }
        public Array fillsection { get; set; }
        public Array fillgroup { get; set; }
        public Array fillhead { get; set; }
        public string FMG_GroupName { get; set; }
        public Array filldata { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public Array currentYear { get; set; }
        public string multiplegroup { get; set; }
        public long TotalTobepaid { get; set; }
        public long Tobepaid { get; set; }

        public string FMH_FeeName { get; set; }
        public string FTI_Name { get; set; }
        public string returnduplicatestatus { get; set; }
        public List<FeeStudentWaiveOffDTO> headlist { get; set; }
        public string searchType { get; set; }
        public string searchtext { get; set; }
        public string searchnumber { get; set; }
        public DateTime searchdate { get; set; }
        public long userid { get; set; }
        public string filterrefund { get; set; }
        public long FSS_ToBePaid { get; set; }
        public long FSS_PaidAmount { get; set; }

        public int finewaiveoff { get; set; }

        public int completefinewaiveoff { get; set; }

        public string FSWO_WaivedOffRemarks { get; set; }

        public string filepath { get; set; }
        public string filename { get; set; }

        public Array editamount { get; set; }
    }
}
