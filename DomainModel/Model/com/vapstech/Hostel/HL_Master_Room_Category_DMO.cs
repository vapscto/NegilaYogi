using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.Hostel
{
    [Table("HL_Master_Room_Category")]
    public class HL_Master_Room_Category_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HLMRCA_Id { get; set; }
        public long MI_Id { get; set; }
        public long? HLMH_Id { get; set; }
        public long? FMG_Id { get; set; }
        public string HLMRCA_RoomCategory { get; set; }
        public string HLMRCA_Description { get; set; }
        public long HLMRCA_MaxCapacity { get; set; }
        public bool HLMRCA_ActiveFlag { get; set; }
        public bool? HLMRCA_ACFlg { get; set; }
        public bool? HLMRCA_SharingFlg { get; set; }
        public decimal? HLMRCA_SORate { get; set; }
        public decimal? HLMRCA_RoomRate { get; set; }
        public DateTime? HLMRCA_CreatedDate { get; set; }
        public long HLMRCA_CreatedBy { get; set; }
        public DateTime? HLMRCA_UpdatedDate { get; set; }
        public long HLMRCA_UpdatedBy { get; set; }
    }
}
