using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Sports
{
    [Table("SPCC_Master_Compition_Level", Schema = "SPC")]
    public class SportMasterCompitionLevelDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SPCCMCL_Id { get; set; }
        public long MI_Id { get; set; }
        public string SPCCMCL_CompitionLevel { get; set; }
        public string SPCCMCL_CLDesc { get; set; }
        public bool SPCCMCL_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
