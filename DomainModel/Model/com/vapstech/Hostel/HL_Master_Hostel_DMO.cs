using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModel.Model.com.vapstech.HRMS;

namespace DomainModel.Model.com.vapstech.Hostel
{
    [Table("HL_Master_Hostel")]
    public class HL_Master_Hostel_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HLMH_Id { get; set; }
        public long MI_Id { get; set; }
        public string HLMH_Name { get; set; }
        public string HLMH_BoysGirlsFlg { get; set; }
        public string HLMH_Address { get; set; }
        public long IVRMMC_Id { get; set; }
        public long IVRMMS_Id { get; set; }
        public string HLMH_City { get; set; }
        public long HLMH_Pincode { get; set; }
        public long HLMH_ContactNo { get; set; }
        public long HLMH_TotalFloor { get; set; }
        public long HLMH_TotalRoom { get; set; }
        public long HLMH_TotalCapacity { get; set; }
        public string HLMH_Desc { get; set; }
        public bool HLMH_ActiveFlag { get; set; }

        public List<HL_Master_Hostel_Mess_DMO> HL_Master_Hostel_Mess_DMO { get; set; }
        public List<HL_Master_Hostel_Photos_DMO> HL_Master_Hostel_Photos_DMO { get; set; }
        public List<HL_Master_Hostel_Facilities_DMO> HL_Master_Hostel_Facilities_DMO { get; set; }
        public List<HL_Master_Hostel_Warden_DMO> HL_Master_Hostel_Warden_DMO { get; set; }

    }
}
