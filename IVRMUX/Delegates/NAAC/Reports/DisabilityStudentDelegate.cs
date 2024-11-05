using CommonLibrary;
using PreadmissionDTOs.NAAC.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Reports
{
    public class DisabilityStudentDelegate
    {

        CommonDelegate<Criteria2_DTO, Criteria2_DTO> _commnbranch = new CommonDelegate<Criteria2_DTO, Criteria2_DTO>();

        public Criteria2_DTO getdata(Criteria2_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "DisabilityStudentFacade/getdata/");
        }

        public Criteria2_DTO get_report(Criteria2_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "DisabilityStudentFacade/get_report/");
        }

        public Criteria2_DTO Demand_Ratio_212_Report(Criteria2_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "DisabilityStudentFacade/Demand_Ratio_212_Report/");
        }
        public Criteria2_DTO Exm_P_Stud_Report(Criteria2_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "DisabilityStudentFacade/Exm_P_Stud_Report/");
        }
        public Criteria2_DTO EMPLOYEE_AWARD_REPORT244(Criteria2_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "DisabilityStudentFacade/EMPLOYEE_AWARD_REPORT244/");
        }
        public Criteria2_DTO get_desination(Criteria2_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "DisabilityStudentFacade/get_desination/");
        }

        public Criteria2_DTO Teacher_Recognised_242_Report(Criteria2_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "DisabilityStudentFacade/Teacher_Recognised_242_Report/");
        }

        public Criteria2_DTO T_ProfileAndQuality_Report24(Criteria2_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "DisabilityStudentFacade/T_ProfileAndQuality_Report24/");
        }

        public Criteria2_DTO Student_Enrolment_Profile_Report21(Criteria2_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "DisabilityStudentFacade/Student_Enrolment_Profile_Report21/");
        }

        public Criteria2_DTO StudentSat_Survey_Report27(Criteria2_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "DisabilityStudentFacade/StudentSat_Survey_Report27/");
        }

        public Criteria2_DTO sanctioned_posts_Report245(Criteria2_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "DisabilityStudentFacade/sanctioned_posts_Report245/");
        }

        public Criteria2_DTO DeclrofResult_Report251(Criteria2_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "DisabilityStudentFacade/DeclrofResult_Report251/");
        }
        
    }
}
