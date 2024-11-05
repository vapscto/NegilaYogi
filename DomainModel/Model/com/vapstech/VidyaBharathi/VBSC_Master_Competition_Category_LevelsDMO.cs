using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VidyaBharathi
{

    [Table("VBSC_Master_Competition_Category_Levels")]
    public class VBSC_Master_Competition_Category_LevelsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VBSCMCCCLE_Id { get; set; }
        public long VBSCMCC_Id { get; set; }
        public long VBSCMCL_Id { get; set; }
        public bool VBSCMCCCLE_ActiveFlag { get; set; }
        public DateTime? VBSCMCCCLE_CreatedDate { get; set; }
        public DateTime? VBSCMCCCLE_UpdatedDate { get; set; }
        public long VBSCMCCCLE_CreatedBy { get; set; }
        public long VBSCMCCCLE_UpdatedBy { get; set; }
    }
}
