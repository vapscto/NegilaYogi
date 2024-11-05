using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Sports
{
    [Table("SPCC_Master_Sponsor", Schema ="SPC")]
    public class MasterSponserDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SPCCMSP_Id { get; set; }
        public long MI_Id { get; set; }
        public string SPCCMSP_SponsorName { get; set; }
        public string SPCCMSP_ContactPerson { get; set; }
        public long SPCCMSP_ContactNo { get; set; }
        public string SPCCMSP_SponsorDetails { get; set; }
        public bool SPCCMSP_ActiveFlag { get; set; }

    }
}
