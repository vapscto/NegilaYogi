using CommonLibrary;
using PreadmissionDTOs.NAAC.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Reports.Medical
{
    public class Medical_Criteria3ReportsDelegate
    {
        CommonDelegate<Medical_Criteria3Reports_DTO, Medical_Criteria3Reports_DTO> _commnbranch = new CommonDelegate<Medical_Criteria3Reports_DTO, Medical_Criteria3Reports_DTO>();

        public Medical_Criteria3Reports_DTO getdata(Medical_Criteria3Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria3ReportsFacade/getdata/");
        }

        public Medical_Criteria3Reports_DTO MC_311_Report(Medical_Criteria3Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria3ReportsFacade/MC_311_Report");
        }
        public Medical_Criteria3Reports_DTO MC_312_Report(Medical_Criteria3Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria3ReportsFacade/MC_312_Report");
        }
        public Medical_Criteria3Reports_DTO MC_313_Report(Medical_Criteria3Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria3ReportsFacade/MC_313_Report");
        }
        public Medical_Criteria3Reports_DTO MC_322_Report(Medical_Criteria3Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria3ReportsFacade/MC_322_Report");
        }
        public Medical_Criteria3Reports_DTO MC_331_report(Medical_Criteria3Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria3ReportsFacade/MC_331_report");
        }
        public Medical_Criteria3Reports_DTO MC_332_Report(Medical_Criteria3Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria3ReportsFacade/MC_332_Report");
        }
        public Medical_Criteria3Reports_DTO MC_333_Report(Medical_Criteria3Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria3ReportsFacade/MC_333_Report");
        }
        public Medical_Criteria3Reports_DTO MC_334_Report(Medical_Criteria3Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria3ReportsFacade/MC_334_Report");
        }
        public Medical_Criteria3Reports_DTO MC_341_Report(Medical_Criteria3Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria3ReportsFacade/MC_341_Report");
        }
        public Medical_Criteria3Reports_DTO MC_342_Report(Medical_Criteria3Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria3ReportsFacade/MC_342_Report");
        }
        public Medical_Criteria3Reports_DTO MC_351_Report(Medical_Criteria3Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria3ReportsFacade/MC_351_Report");
        }
        public Medical_Criteria3Reports_DTO MC_352_Report(Medical_Criteria3Reports_DTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "Medical_Criteria3ReportsFacade/MC_352_Report");
        }
       
    }
}
