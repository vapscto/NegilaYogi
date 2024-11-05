using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Placement
{
    [Table("PL_CI_Schedule_Company_JobTitle_Students_Status")]
    public class PL_CI_Schedule_Company_JobTitle_Students_StatusDMO
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PLCISCHCOMJTSTS_Id { get; set; }
        public long PLCISCHCOMJTST_Id { get; set; }
        public string PLCISCHCOMJTSTS_InterviewRound { get; set; }
        public string PLCISCHCOMJTSTS_Marks { get; set; }
        public string PLCISCHCOMJTSTS_TestType { get; set; } //
        public string PLCISCHCOMJTSTS_Remarks { get; set; }
        public bool PLCISCHCOMJTSTS_SelectedFlg { get; set; }
        public bool PLCISCHCOMJTSTS_ActiveFlag { get; set; }
        public DateTime PLCISCHCOMJTSTS_CreatedDate { get; set; }
        public long PLCISCHCOMJTSTS_CreatedBy { get; set; }
        public DateTime PLCISCHCOMJTSTS_UpdatedDate { get; set; }
        public long PLCISCHCOMJTSTS_UpdatedBy { get; set; }



    }
}
