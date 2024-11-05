using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Hostel
{
    [Table("HL_Hostel_Student_Transfer_College")]
    public class HL_Hostel_Student_Transfer_CollegeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HLHSTRSC_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime? HLHSALTC_TransferDate { get; set; }
        public long ASMAY_Id { get; set; }
        public long HLMH_Id { get; set; }
        public long HLMRCA_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long HRMRM_Id { get; set; }
        public decimal HLHSTRSC_RoomFee { get; set; }
        public long HLHSTRSC_To_HLMRCA_Id { get; set; }
        public long HLMRCAC_To_HRMRM_Id { get; set; }
        public bool HLHSTRSC_EntireRoomReqdFlg { get; set; }
        public decimal HLHSTRSC_NewRoomFee { get; set; }
        public string HLHSTRSC_AllotRemarks { get; set; }
        public string HLHSTRSC_VacateRemarks { get; set; }
        public bool HLHSTRSC_ActiveFlag { get; set; }
        public DateTime? HLHSTRSC_CreatedDate { get; set; }
        public DateTime ? HLHSTRSC_UpdatedDate    { get; set;}
        public long HLHSTRSC_UpdatedBy { get; set; }
        public long HLHSTRSC_CreatedBy { get; set; }

    }
}
