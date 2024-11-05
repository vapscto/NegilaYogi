using DomainModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("HR_Candidate_Languages")]
    public class HR_Candidate_LanguagesDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long HRCLAN_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRCD_Id { get; set; }
        public string HRCLAN_ToRead { get; set; }
        public string HRCLAN_ToWrite { get; set; }
        public string HRCLAN_ToSpeak { get; set; }
        public bool HRCLAN_ActiveFlag { get; set; }
        public long HRCLAN_CreatedBy { get; set; }
        public long HRCLAN_UpdatedBy { get; set; }
    }
}