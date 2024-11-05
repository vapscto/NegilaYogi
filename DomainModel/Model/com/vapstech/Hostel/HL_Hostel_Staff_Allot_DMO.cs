using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Hostel
{
    [Table("HL_Hostel_Staff_Allot")]
    public class HL_Hostel_Staff_Allot_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HLHSTALT_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime HLHSTALT_AllotmentDate { get; set; }
        public long HLMH_Id { get; set; }
        public long HLMRCA_Id { get; set; }
        public long HRME_Id { get; set; }
        public long HRMRM_Id { get; set; }
        public long HLHSTALT_NoOfBeds { get; set; }
        public string HLHSTALT_AllotRemarks { get; set; }
        public bool HLHSTALT_VacateFlg { get; set; }
        public DateTime? HLHSTALT_VacatedDate { get; set; }
        public string HLHSTALT_VacateRemarks { get; set; }
        public bool HLHSTALT_ActiveFlag { get; set; }
        public DateTime? HLHSTALT_CreatedDate { get; set; }
        public DateTime? HLHSTALT_UpdatedDate { get; set; }
        public long HLHSTALT_UpdatedBy { get; set; }
        public long HLHSTALT_CreatedBy { get; set; }
    }
}
