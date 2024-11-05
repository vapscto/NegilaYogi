using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("Fee_Master_Staff_GroupHead_Installments")]
    public class Fee_Master_Staff_GroupHead_Installments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMSTGHI_Id { get; set; }
        [ForeignKey("FMSTGH_Id")]
        public long FMSTGH_Id { get; set; }
        public long FMH_Id { get; set; }
        public long FTI_Id { get; set; }
    }
}
