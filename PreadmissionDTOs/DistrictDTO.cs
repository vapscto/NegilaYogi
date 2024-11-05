using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs
{
    public class DistrictDTO
    {
        public long IVRMMD_Id { get; set; }
        public string IVRMMD_Name { get; set; }

        public string IVRMMD_Code { get; set; }
        public long IVRMMS_Id { get; set; }
        public DateTime IVRMMD_CreatedDate { get; set; }
        public DateTime IVRMMD_UpdatedDate { get; set; }
        public long? IVRMMD_CreatedBy { get; set; }
        public long? IVRMMD_UpdatedBy { get; set; }
        public bool? IVRMMD_ActiveFlag { get; set; }
        public bool? IVRMMD_AllowScholashipFlg { get; set; }

        public Array districtDrpDown { get; set; }
    }
}
