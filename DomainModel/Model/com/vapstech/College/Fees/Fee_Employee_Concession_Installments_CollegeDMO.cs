using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_Employee_Concession_Installments_College", Schema = "CLG")]
    public class Fee_Employee_Concession_Installments_CollegeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FECIC_Id { get; set; }
        public long FECC_Id { get; set; }
        public long FTI_Id { get; set; }
        public long FECIC_ConcessionAmount { get; set; }

        public DateTime FECIC_CreatedDate { get; set; }
        public DateTime FECIC_UpdatedDate { get; set; }
        public long FECIC_CreatedBy { get; set; }
        public long FECIC_UpdatedBy { get; set; }
    }
}
