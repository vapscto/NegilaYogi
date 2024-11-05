using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.admission
{
  public  class ReligionCategory_MappingDTO
    {
        public long IRCC_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long IVRMMR_Id { get; set; }
        public long IMCC_Id { get; set; }
        public bool IRCC_ActiveFlg { get; set; }
        public long IRCC_CreatedBy { get; set; }
        public long IRCC_UpdatedBy { get; set; }
        public DateTime? IRCC_CreatedDate { get; set; }
        public DateTime? IRCC_UpdatedDate { get; set; }
        public Array religion_list { get; set; }
        public Array get_masterlist { get; set; }
        public Array caste_list { get; set; }
        public Array editlist { get; set; }
        public bool returnval { get; set; }
        public ReligionCategory_MappingDTO[] castid { get; set; }
        public bool duplicate { get; set; }
        public string IVRMMR_Name { get; set; }
        public string IMCC_CategoryName { get; set; }
        public string msg { get; set; }
    }
}
