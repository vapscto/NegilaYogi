using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("Fee_Master_Staff_GroupHead")]
    public class Fee_Master_Staff_GroupHead
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMSTGH_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long FMG_Id { get; set; }
        public bool FMSTGH_ActiveFlag { get; set; }
        public List<Fee_Master_Staff_GroupHead_Installments> Fee_Master_Staff_GroupHead_Installments { get; set; }
    }
}
