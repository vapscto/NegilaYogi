using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VidyaBharathi
{
    [Table("VBSC_Master_Events")]
    public class VBSC_Master_EventsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VBSCME_Id { get; set; }
        public string VBSCME_EventName { get; set; }
        public string VBSCME_EventNameDesc { get; set; }
        public bool VBSCME_ActiveFlag { get; set; }
        public DateTime? VBSCME_CreatedDate { get; set; }
        public DateTime? VBSCME_UpdatedDate { get; set; }
        public long MT_Id { get; set; }
        
    }
}
