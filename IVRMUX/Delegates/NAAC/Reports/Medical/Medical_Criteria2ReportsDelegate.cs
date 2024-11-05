using CommonLibrary;
using PreadmissionDTOs.NAAC.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Reports.Medical
{
    public class Medical_Criteria2ReportsDelegate
    {
        CommonDelegate<Medical_Criteria2Reports_DTO, Medical_Criteria2Reports_DTO> _commnbranch = new CommonDelegate<Medical_Criteria2Reports_DTO, Medical_Criteria2Reports_DTO>();

        public Medical_Criteria2Reports_DTO getdata(Medical_Criteria2Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria2ReportsFacade/getdata/");
        }
        public Medical_Criteria2Reports_DTO MC_221_Report(Medical_Criteria2Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria2ReportsFacade/MC_221_Report");
        }
        public Medical_Criteria2Reports_DTO MC_254_Report(Medical_Criteria2Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria2ReportsFacade/MC_254_Report");
        }
        public Medical_Criteria2Reports_DTO MC_232_Report(Medical_Criteria2Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria2ReportsFacade/MC_232_Report");
        }
        public Medical_Criteria2Reports_DTO MC_212_Report(Medical_Criteria2Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria2ReportsFacade/MC_212_Report");
        }
        public Medical_Criteria2Reports_DTO MC_213_report(Medical_Criteria2Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria2ReportsFacade/MC_213_report");
        }
        public Medical_Criteria2Reports_DTO MC_222_Report(Medical_Criteria2Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria2ReportsFacade/MC_222_Report");
        }
        public Medical_Criteria2Reports_DTO MC_234_Report(Medical_Criteria2Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria2ReportsFacade/MC_234_Report");
        }
        public Medical_Criteria2Reports_DTO MC_241_Report(Medical_Criteria2Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria2ReportsFacade/MC_241_Report");
        }
        public Medical_Criteria2Reports_DTO MC_242_Report(Medical_Criteria2Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria2ReportsFacade/MC_242_Report");
        }
        public Medical_Criteria2Reports_DTO MC_243_Report(Medical_Criteria2Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria2ReportsFacade/MC_243_Report");
        }
        public Medical_Criteria2Reports_DTO MC_244_Report(Medical_Criteria2Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria2ReportsFacade/MC_244_Report");
        }
        public Medical_Criteria2Reports_DTO MC_245_Report(Medical_Criteria2Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria2ReportsFacade/MC_245_Report");
        }
        public Medical_Criteria2Reports_DTO MC_262_Report(Medical_Criteria2Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria2ReportsFacade/MC_262_Report");
        }
        public Medical_Criteria2Reports_DTO MC_271_Report(Medical_Criteria2Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria2ReportsFacade/MC_271_Report");
        }

        
    }
}
