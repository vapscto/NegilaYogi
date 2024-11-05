using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Hostel
{
    [Table("HL_Master_Facility")]
    public class HL_Master_Facility_DMO:CommonParamDMO
    {
        [Key]        
        public long HLMFTY_Id { get; set; }
        public long MI_Id { get; set; }
        public string HLMFTY_FacilityName { get; set; }
        public string HLMFTY_FacilityDesc { get; set; }
        public string HLMFTY_FacilityFileName { get; set; }
        public string HLMFTY_FacilityFilePath { get; set; }
        public bool HLMFTY_ActiveFlag { get; set; }
        public long HLMFTY_CreatedBy { get; set; }
        public long HLMFTY_UpdatedBy { get; set; }
        
    }
}
