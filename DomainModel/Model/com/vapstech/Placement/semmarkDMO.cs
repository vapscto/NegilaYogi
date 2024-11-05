using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Placement
{
    [Table("PL_CI_Schedule_Company_JobTitle_SemMarks")]
    public class semmarkDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PLCISCHCOMJTSEM_Id { get; set; }
        public long PLCISCHCOMJT_Id { get; set; }
        public long AMSE_Id { get; set; }
        public decimal PLCISCHCOMJTSEM_CutOfMarks { get; set; }
        public string PLCISCHCOMJTSEM_OtherDetails { get; set; }
        public bool PLCISCHCOMJTSEM_ActiveFlag { get; set; }
        public DateTime? PLCISCHCOMJTSEM_CreatedDate { get; set; }
        public DateTime? PLCISCHCOMJTSEM_UpdatedDate { get; set; }
        public  long PLCISCHCOMJTSEM_CreatedBy { get; set; }
        public  long PLCISCHCOMJTSEM_UpdatedBy { get; set; }
    }
}
