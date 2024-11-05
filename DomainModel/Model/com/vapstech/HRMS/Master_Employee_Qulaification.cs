using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Master_Employee_Qualification")]
    public class Master_Employee_Qulaification :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMEQ_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMC_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HRME_QualificationName { get; set; }
        public string HRMEQ_CollegeName { get; set; }
        public string HRMEQ_UniversityName { get; set; }
        public int? HRMEQ_YearOfPassing { get; set; }
        public string HRMEQ_RegistrationNo { get; set; }
        public string HRMEQ_Result { get; set; }
        public string HRMEQ_CGPAOrPerFlag { get; set; }
        public string HRMEQ_CGPA { get; set; }
        public string HRMEQ_Percentage { get; set; }
        public string HRMEQ_AreaOfSpecialisation { get; set; }
        public string HRMEQ_MedicalCouncil { get; set; }
        public DateTime? HRMEQ_Date { get; set; }
        public string HRMEQ_Hardcopy { get; set; }
        public bool? HRMEQ_PHDFlg { get; set; }
        public long? HRMEQ_StateId { get; set; }
        public long? HRMEQ_CountryId { get; set; }
        public string HRMEQ_ThesisTitle { get; set; }
        public long? HRMEQ_RegistrationYear { get; set; }
        public string HRMEQ_GuideName { get; set; }
    }
}
