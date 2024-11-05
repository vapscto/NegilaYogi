using DomainModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("HR_Candidate_Family")]
    public class HR_Candidate_FamilyDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long HRCFAM_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRCD_Id { get; set; }
        public string HRCFAM_Name { get; set; }
        public string HRCFAM_Relationship { get; set; }
        public string HRCFAM_Occupation { get; set; }
        public string HRCFAM_CompanyName { get; set; }
        public long HRCFAM_Age { get; set; }
        public bool HRCFAM_ActiveFlag { get; set; }
        public long HRCFAM_CreatedBy { get; set; }
        public long HRCFAM_UpdatedBy { get; set; }
    }

}