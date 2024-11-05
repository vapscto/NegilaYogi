using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Sports
{
    [Table("SPCC_Master_House_Committe", Schema = "SPC")]
    public class SportMasterHouseCommitteDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SPCCMHC_Id { get; set; }
        public long MI_Id { get; set; }
        public long SPCCMHD_Id { get; set; }
        public long SPCCMH_Id { get; set; }
        public long AMST_Id { get; set; }
        public long? SPCCMHC_ContactNo { get; set; }
        public string SPCCMHC_EmailId { get; set; }
        public bool SPCCMHD_ActiveFlag { get; set; }
      
    }
}
