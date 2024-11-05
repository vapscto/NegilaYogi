using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Portals.IVRM
{
   [Table("IVRM_Gallery_Programs")]
   public class IVRM_Gallery_ProgramsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IGAPRG_Id { get; set; }
        public long IGA_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public bool IGAPRG_ActiveFlag { get; set; }
        public long IGAPRG_CreatedBy { get; set; }
        public long IGAPRG_UpdatedBy { get; set; }
        public DateTime IGAPRG_CreatedDate { get; set; }
        public DateTime IGAPRG_UpdatedDate { get; set; }
    }
}
