using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.Chirman
{
    public class Ch_feedbackDTO
    {
        public long ASGFE_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public string ASGFE_FeedBack { get; set; }
        public DateTime? ASGFE_FeedbackDate { get; set; }
        public bool ASGFE_ActiveFlag { get; set; }
        public long ASGFE_CreatedBy { get; set; }
        public long ASGFE_UpdatedBy { get; set; }
        public string ASMAY_Year { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public DateTime AMST_Date { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public string AMST_AdmNo { get; set; }
        public Array feedbackdetails { get; set; }
        public Array academicyearlst { get; set; }

    }
}


