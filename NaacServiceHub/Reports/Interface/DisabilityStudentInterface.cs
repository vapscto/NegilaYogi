using PreadmissionDTOs.NAAC.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Reports.Interface
{
   public interface DisabilityStudentInterface
    {
        Criteria2_DTO getdata(Criteria2_DTO data);
        Task<Criteria2_DTO> get_report(Criteria2_DTO data);
        Task<Criteria2_DTO> Demand_Ratio_212_Report(Criteria2_DTO data);
        Task<Criteria2_DTO> Exm_P_Stud_Report(Criteria2_DTO data);
        Task<Criteria2_DTO> EMPLOYEE_AWARD_REPORT244(Criteria2_DTO data);
        Criteria2_DTO get_desination(Criteria2_DTO data);
        Task<Criteria2_DTO> Teacher_Recognised_242_Report(Criteria2_DTO data);
        Task<Criteria2_DTO> T_ProfileAndQuality_Report24(Criteria2_DTO data);
        Task<Criteria2_DTO> Student_Enrolment_Profile_Report21(Criteria2_DTO data);
        Task<Criteria2_DTO> StudentSat_Survey_Report27(Criteria2_DTO data);
        Task<Criteria2_DTO> sanctioned_posts_Report245(Criteria2_DTO data);
        Task<Criteria2_DTO> DeclrofResult_Report251(Criteria2_DTO data);



    }
}
