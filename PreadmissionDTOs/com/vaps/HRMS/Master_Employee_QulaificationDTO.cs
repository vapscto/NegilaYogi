using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class Master_Employee_QulaificationDTO :CommonParamDTO
    {
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
        public string retrunMsg { get; set; }
        public bool HRMEQ_PHDFlg { get; set; }
        public long HRMEQ_StateId { get; set; }
        public long HRMEQ_CountryId { get; set; }
        public string HRMEQ_ThesisTitle { get; set; }
        public long HRMEQ_RegistrationYear { get; set; }
        public string HRMEQ_GuideName { get; set; }
    }
}
