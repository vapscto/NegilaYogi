using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("Master_VideoConferencing_Instituition")]
    public class Master_VideoConferencing_InstituitionDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
         
        public long MVIDCONINT_Id { get; set; }
        public long MI_Id { get; set; }
        public long MVIDCON_Id { get; set; }
        public string MVIDCONINT_HostedURL { get; set; }
        public bool MVIDCONINT_ActiveFlg { get; set; }
        public long MVIDCONINT_CreatedBy { get; set; }
        public DateTime MVIDCONINT_CreatedDate { get; set; }
        public long MVIDCONINT_UpdatedBy { get; set; }
        public DateTime MVIDCONINT_UpdatedDate { get; set; }
        
    }
}
