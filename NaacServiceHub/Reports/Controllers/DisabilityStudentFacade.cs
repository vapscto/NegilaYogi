using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Reports.Interface;
using PreadmissionDTOs.NAAC.Reports;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Reports.Controllers
{
    [Route("api/[controller]")]
    public class DisabilityStudentFacade : Controller
    {
        public DisabilityStudentInterface _Interface;
        public DisabilityStudentFacade(DisabilityStudentInterface para)
        {
            _Interface = para;
        }

        [Route("getdata")]
       public Criteria2_DTO getdata([FromBody]Criteria2_DTO data)
        {
            return _Interface.getdata(data);
        }

        [Route("get_report")]
        public Task<Criteria2_DTO> get_report([FromBody]Criteria2_DTO data)
        {
            return _Interface.get_report(data);
        }

        [Route("Demand_Ratio_212_Report")]
        public Task<Criteria2_DTO> Demand_Ratio_212_Report([FromBody]Criteria2_DTO data)
        {
            return _Interface.Demand_Ratio_212_Report(data);
        }

        [Route("Exm_P_Stud_Report")]
        public Task<Criteria2_DTO> Exm_P_Stud_Report([FromBody]Criteria2_DTO data)
        {
            return _Interface.Exm_P_Stud_Report(data);
        }

        [Route("EMPLOYEE_AWARD_REPORT244")]
        public Task<Criteria2_DTO> EMPLOYEE_AWARD_REPORT244([FromBody]Criteria2_DTO data)
        {
            return _Interface.EMPLOYEE_AWARD_REPORT244(data);
        }

        [Route("get_desination")]
        public Criteria2_DTO get_desination([FromBody]Criteria2_DTO data)
        {
            return _Interface.get_desination(data);
        }

        [Route("Teacher_Recognised_242_Report")]
        public Task<Criteria2_DTO> Teacher_Recognised_242_Report([FromBody]Criteria2_DTO data)
        {
            return _Interface.Teacher_Recognised_242_Report(data);
        }

        [Route("T_ProfileAndQuality_Report24")]
        public Task<Criteria2_DTO> T_ProfileAndQuality_Report24([FromBody]Criteria2_DTO data)
        {
            return _Interface.T_ProfileAndQuality_Report24(data);
        }

        [Route("Student_Enrolment_Profile_Report21")]
        public Task<Criteria2_DTO> Student_Enrolment_Profile_Report21([FromBody]Criteria2_DTO data)
        {
            return _Interface.Student_Enrolment_Profile_Report21(data);
        }

        [Route("StudentSat_Survey_Report27")]
        public Task<Criteria2_DTO> StudentSat_Survey_Report27([FromBody]Criteria2_DTO data)
        {
            return _Interface.StudentSat_Survey_Report27(data);
        }

        [Route("sanctioned_posts_Report245")]
        public Task<Criteria2_DTO> sanctioned_posts_Report245([FromBody]Criteria2_DTO data)
        {
            return _Interface.sanctioned_posts_Report245(data);
        }


        [Route("DeclrofResult_Report251")]
        public Task<Criteria2_DTO> DeclrofResult_Report251([FromBody] Criteria2_DTO data)
        {
            return _Interface.DeclrofResult_Report251(data);
        }


    }
}
