using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeStreamGroupMappingDTO
    {
        public long PASL_ID { get; set; }
        public string PASL_Name { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long FMG_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMG_GroupName { get; set; }
        public string FMG_Remarks { get; set; }
        public string FMG_CompulsoryFlag { get; set; }
        public bool FMG_ActiceFlag { get; set; }
        public long user_id { get; set; }
        public Array yearlist { get; set; }
        public Array classdetails { get; set; }
        public Array stream { get; set; }
        public Array groupdetails { get; set; }
        public string ASMCL_ClassName { get; set; }
        public int ASMCL_MinAgeYear { get; set; }
        public int ASMCL_MinAgeMonth { get; set; }
        public int ASMCL_MinAgeDays { get; set; }
        public int ASMCL_MaxAgeYear { get; set; }
        public int ASMCL_MaxAgeMonth { get; set; }
        public int ASMCL_MaxAgeDays { get; set; }
        public int ASMCL_Order { get; set; }
        public string ASMCL_ClassCode { get; set; }
        public bool ASMCL_ActiveFlag { get; set; }
        public int ASMCL_MaxCapacity { get; set; }
        public int ASMCL_PreadmFlag { get; set; }

        public long FMSGM_Id { get; set; }
        public Array GroupData { get; set; }


     //   public long ASMCL_ID { get; set; }

        public long FMSGM_Active { get; set; }
        public string message { get; set; }
        public bool retrunval { get; set; }

        public FeeStreamGroupMappingDTO[] TempararyArrayList { get; set; }
        public FeeStreamGroupMappingDTO[] TempararyArrayList1 { get; set; }
        public string comparevlue { get; set; }
        public string returnduplicatestatus { get; set; }

        public Array getdeatils { get; set; }
        public string ASMAY_Year { get; set; }
        public bool returnval { get; set; }
    }
}
