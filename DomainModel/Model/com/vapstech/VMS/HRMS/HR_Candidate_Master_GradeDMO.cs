using DomainModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("HR_Candidate_Master_Grade")]
    public class HR_Candidate_Master_GradeDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRCMG_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRCMG_GradeName { get; set; }  
        public string HRCMG_MarksPerFlag { get; set; }
        public bool HRCMG_ActiveFlag { get; set; }
   
    }

}