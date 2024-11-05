using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VidyaBharathi
{
    [Table("VBSC_Events_Category_Students")]
    public class VBSC_Events_Category_StudentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long VBSCECTSTU_Id { get; set; }

        public long VBSCECT_Id { get; set; }
        public long AMST_ID { get; set; }
        public string AMST_Name { get; set; }
        public long ASMCL_ID { get; set; }
        public long ASMS_Id { get; set; }
        public long VBSCECTSTU_Rank { get; set; }
        public decimal VBSCECTSTU_Points { get; set; }
        public bool VBSCECTSTU_RecordBrokenFlag { get; set; }
        public string VBSCECTSTU_Remarks { get; set; }
        public bool VBSCECTSTU_ActiveFlag { get; set; }
        public DateTime? VBSCECTSTU_CreatedDate { get; set; }
        public DateTime? VBSCECTSTU_UpdatedDate { get; set; }
        public long VBSCECTSTU_CreatedBy { get; set; }
        public long VBSCECTSTU_UpdatedBy { get; set; }

    }
}
