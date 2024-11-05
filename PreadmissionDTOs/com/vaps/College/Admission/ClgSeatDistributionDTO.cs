using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class ClgSeatDistributionDTO
    {
        public long AMB_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMB_BranchCode { get; set; }
        public string AMB_BranchInfo { get; set; }
        public string AMB_BranchType { get; set; }
        public int AMB_StudentCapacity { get; set; }
        public int AMB_Order { get; set; }
        public bool AMB_ActiveFlag { get; set; }
        public long AMCO_Id { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMCO_CourseCode { get; set; }
        public string AMB_AidedUnAided { get; set; }
        public long AMSE_Id { get; set; }
        public long ACQC_Id { get; set; }
        public string returnduplicatestatus { get; set; }
        public long ACQ_Id { get; set; }
        public string AMSE_SEMName { get; set; }
        public string AMSE_SEMCode { get; set; }
        public int AMSE_Year { get; set; }
        public string AMSE_EvenOdd { get; set; }
        public string ACQ_QuotaName { get; set; }
        public string ACQC_CategoryName { get; set; }

        public decimal ACSCD_SeatPer { get; set; }
        public long ACSCD_SeatNos { get; set; }
        public string ACSCD_Remarks { get; set; }
        public bool ACSCD_ActiveFlg { get; set; }
        public long ACSCD_Id { get; set; }


        public bool returnval { get; set; }
        public Array getYear { get; set; }
        public Array getCourse { get; set; }
        public Array getBranch { get; set; }
        public Array getBranchDetails { get; set; }

        public Array getSemester { get; set; }
        public Array getSeatCategory { get; set; }
        public Array getQuota { get; set; }
        public Array getSeatsdetails { get; set; }
        public Array getSeattotal { get; set; }
        
        public long[] AMSE_Sem { get; set; }
     
        public Array getSeatsdetails1 { get; set; }

        public Temp_SeatDTO[] seat_data { get; set; }
    }

    public class Temp_SeatDTO
    {
        public long ACQ_Id { get; set; }
        public long ACQC_Id { get; set; }
        public decimal ACSCD_SeatPer { get; set; }
        public long ACSCD_SeatNos { get; set; }
        public string ACSCD_Remarks { get; set; }

    }
}
