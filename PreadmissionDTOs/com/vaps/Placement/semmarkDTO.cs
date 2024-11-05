using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Placement
{
   public class semmarkDTO
    {
        public string returnduplicatestatus;

        public long PLCISCHCOMJTSEM_Id { get; set; }
        public long PLCISCHCOMJT_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMCO_Id { get; set; }
        public decimal PLCISCHCOMJTSEM_CutOfMarks { get; set; }
        public string AMSE_SEMName { get; set; }

        public string PLCISCHCOMJT_JobTitle { get; set; }
        public string PLCISCHCOMJTSEM_OtherDetails { get; set; }
        public bool PLCISCHCOMJTSEM_ActiveFlag { get; set; }
        public DateTime? PLCISCHCOMJTSEM_CreatedDate { get; set; }
        public DateTime? PLCISCHCOMJTSEM_UpdatedDate { get; set; }
        public long PLCISCHCOMJTSEM_CreatedBy { get; set; }
        public long PLCISCHCOMJTSEM_UpdatedBy { get; set; }
        public Array getsavedata { get; set; }
        public Array jobtitlelist { get; set; }
        public Array save { get; set; }
        public string returnval { get; set; }
        public long User_Id { get; set; }
        public long MI_Id { get; set; }
        public Array editdata { get; set; }
    }
}
