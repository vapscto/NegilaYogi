using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("Master_VideoConferencing")]
    public class Master_VideoConferencingDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MVIDCON_Id { get; set; }  
        public string MVIDCON_VCNames { get; set; }
        public string MVIDCON_VCCode { get; set; } 
        public bool MVIDCON_ActiveFlg { get; set; }
        public DateTime MVIDCON_CreatedDate { get; set; }
        public DateTime MVIDCON_UpdatedDate { get; set; }
        public long MVIDCON_CreatedBy { get; set; }
        public long MVIDCON_UpdatedBy { get; set; }

    }
}
