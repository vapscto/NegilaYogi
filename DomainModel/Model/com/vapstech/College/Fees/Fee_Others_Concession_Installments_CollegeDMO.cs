using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_Others_Concession_Installments_College", Schema = "CLG")]
    public class Fee_Others_Concession_Installments_CollegeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FOCIC_Id { get; set; }
        public long FOCC_Id { get; set; }
        public long FTI_Id { get; set; }
        public long FSCIC_ConcessionAmount { get; set; }
        public DateTime FSCIC_CreatedDate { get; set; }
        public DateTime FSCIC_UpdatedDate { get; set; }

        public long FOCIC_CreatedBy { get; set; }
        public long FOCIC_UpdatedBy { get; set; }
    }
}
