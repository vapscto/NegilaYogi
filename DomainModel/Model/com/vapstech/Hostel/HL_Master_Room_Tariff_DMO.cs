using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Hostel
{
    [Table("HL_Master_Room_Tariff")]
    public class HL_Master_Room_Tariff_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HLMRTF_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long HRMRM_Id { get; set; }
        public decimal HLMRTF_SORate { get; set; }
        public decimal HLMRTF_RoomRate { get; set; }
        public bool HLMRTF_ActiveFlag { get; set; }
        public DateTime? HLMRTF_CreatedDate { get; set; }
        public long HLMRTF_CreatedBy { get; set; }
        public DateTime? HLMRTF_UpdatedDate { get; set; }
        public long HLMRTF_UpdatedBy { get; set; }
    }
}
