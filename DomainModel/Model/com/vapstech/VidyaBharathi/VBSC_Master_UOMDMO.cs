using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VidyaBharathi
{
    [Table("VBSC_Master_UOM")]
    public class VBSC_Master_UOMDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VBCCMUOM_Id { get; set; }
      
        public string VBCCMUOM_UOMName { get; set; }
        public bool VBCCMUOM_ActiveFlag { get; set; }
        public DateTime? VBCCMUOM_CreatedDate { get; set; }
        
        public DateTime? VBCCMUOM_UpdatedDate { get; set; }
    
    }
  
}
