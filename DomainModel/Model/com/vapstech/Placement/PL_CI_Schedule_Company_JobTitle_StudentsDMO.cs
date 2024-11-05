using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Placement
{
    [Table("PL_CI_Schedule_Company_JobTitle_Students")]
    public class PL_CI_Schedule_Company_JobTitle_StudentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PLCISCHCOMJTST_Id{ get; set; }
        public long PLCISCHCOMJT_Id { get; set; }
        public long AMCST_Id { get; set; }
        public DateTime PLCISCHCOMJTST_Date { get; set; }
        public bool PLCISCHCOMJTST_ActiveFlag { get; set; } //
        public DateTime PLCISCHCOMJTST_CreatedDate { get; set; }
        public long PLCISCHCOMJTST_CreatedBy { get; set; }
        public DateTime PLCISCHCOMJTST_UpdatedDate { get; set; }//
        public long PLCISCHCOMJTST_UpdatedBy { get; set; }

    }
}
