using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VidyaBharathi
{
    [Table("VBSC_Events")]
    public class VBSC_EventsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VBSCE_Id { get; set; }
        public long MT_Id { get; set; }
        public long VBSCMCL_Id { get; set; }
        public long? IVRMMS_ID { get; set; }
        public long? IVRMMD_ID { get; set; }
        public long ASMAY_Id { get; set; }
        public long VBSCME_Id { get; set; }
        public string VBSCE_VenueName { get; set; }
        public DateTime VBSCE_StartDate { get; set; }
        public DateTime VBSCE_EndDate { get; set; }
        public string VBSCE_StartTime { get; set; }
        public string VBSCE_EndTime { get; set; }
        public string VBSCE_Remarks { get; set; }
        public bool VBSCE_ActiveFlag { get; set; }

        public DateTime? VBSCE_CreatedDate { get; set; }
        public DateTime? VBSCE_UpdatedDate { get; set; } 
        public long VBSCE_CreatedBy { get; set; } 
        public long VBSCE_UpdatedBy { get; set; } 
       
        
    }
}
