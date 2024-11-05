using CommonLibrary;
using PreadmissionDTOs.NAAC.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Reports.Medical
{
    public class Medical_Criteria1ReportsDelegate
    {
        CommonDelegate<Medical_Criteria1Reports_DTO, Medical_Criteria1Reports_DTO> _commnbranch = new CommonDelegate<Medical_Criteria1Reports_DTO, Medical_Criteria1Reports_DTO>();

        public Medical_Criteria1Reports_DTO getdata(Medical_Criteria1Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria1ReportsFacade/getdata/");
        }
        public Medical_Criteria1Reports_DTO get_report_MC_112(Medical_Criteria1Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria1ReportsFacade/get_report_MC_112/");
        }
        public Medical_Criteria1Reports_DTO report_MC_141(Medical_Criteria1Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria1ReportsFacade/report_MC_141/");
        }
        public Medical_Criteria1Reports_DTO report_MC_142(Medical_Criteria1Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria1ReportsFacade/report_MC_142/");
        }
        public Medical_Criteria1Reports_DTO M_IDC121_Report(Medical_Criteria1Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria1ReportsFacade/M_IDC121_Report/");
        }
        public Medical_Criteria1Reports_DTO M_SRC122_Report(Medical_Criteria1Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria1ReportsFacade/M_SRC122_Report/");
        }

        public Medical_Criteria1Reports_DTO MC_VAC_report_132(Medical_Criteria1Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria1ReportsFacade/MC_VAC_report_132/");
        }

        public Medical_Criteria1Reports_DTO StudentsEnrolledInVAC133_report(Medical_Criteria1Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria1ReportsFacade/StudentsEnrolledInVAC133_report/");
        }

        public Medical_Criteria1Reports_DTO MC_StudentUTFV_134_Report(Medical_Criteria1Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria1ReportsFacade/MC_StudentUTFV_134_Report/");
        }
        
    }
}
