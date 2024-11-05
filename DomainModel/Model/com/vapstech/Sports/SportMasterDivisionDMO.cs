using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Sports
{
    [Table("SPCC_Master_Division", Schema = "SPC")]
    public class SportMasterDivisionDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SPCCMD_Id { get; set; }
        public long MI_Id { get; set; }
        public string SPCCMD_DivisionName { get; set; }
        public string SPCCMD_DivisionDescription { get; set; }
        public bool SPCCMD_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
