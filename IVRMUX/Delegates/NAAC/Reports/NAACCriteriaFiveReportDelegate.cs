using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using corewebapi18072016.Delegates;
using PreadmissionDTOs.NAAC.Reports;

namespace IVRMUX.Delegates.NAAC.Reports
{
    public class NAACCriteriaFiveReportDelegate
    {
        CommonDelegate<NAACCriteriaFiveReportDTO, NAACCriteriaFiveReportDTO> _commnbranch = new CommonDelegate<NAACCriteriaFiveReportDTO, NAACCriteriaFiveReportDTO>();

        public NAACCriteriaFiveReportDTO getdata(NAACCriteriaFiveReportDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAACCriteriaFiveReportFacade/getdata/");
        }
        public NAACCriteriaFiveReportDTO get_report(NAACCriteriaFiveReportDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAACCriteriaFiveReportFacade/get_report/");
        }
        public NAACCriteriaFiveReportDTO HSU511(NAACCriteriaFiveReportDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAACCriteriaFiveReportFacade/HSU511/");
        }
        public NAACCriteriaFiveReportDTO get_report513(NAACCriteriaFiveReportDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAACCriteriaFiveReportFacade/get_report513/");
        }
        public NAACCriteriaFiveReportDTO get_report514(NAACCriteriaFiveReportDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAACCriteriaFiveReportFacade/get_report514/");
        }
        public NAACCriteriaFiveReportDTO get_report513med(NAACCriteriaFiveReportDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAACCriteriaFiveReportFacade/get_report513med/");
        }
        public NAACCriteriaFiveReportDTO get_report516(NAACCriteriaFiveReportDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAACCriteriaFiveReportFacade/get_report516/");
        }
        public NAACCriteriaFiveReportDTO get_report515med(NAACCriteriaFiveReportDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAACCriteriaFiveReportFacade/get_report515med/");
        }
          public NAACCriteriaFiveReportDTO get_report521(NAACCriteriaFiveReportDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAACCriteriaFiveReportFacade/get_report521/");
        }
        public NAACCriteriaFiveReportDTO get_report522(NAACCriteriaFiveReportDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAACCriteriaFiveReportFacade/get_report522/");
        }
        public NAACCriteriaFiveReportDTO get_report531(NAACCriteriaFiveReportDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAACCriteriaFiveReportFacade/get_report531/");
        }
        public NAACCriteriaFiveReportDTO get_report533(NAACCriteriaFiveReportDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAACCriteriaFiveReportFacade/get_report533/");
        }
        public NAACCriteriaFiveReportDTO get_report542(NAACCriteriaFiveReportDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAACCriteriaFiveReportFacade/get_report542/");
        }
        public NAACCriteriaFiveReportDTO get_report542HSU(NAACCriteriaFiveReportDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAACCriteriaFiveReportFacade/get_report542HSU/");
        }
        public NAACCriteriaFiveReportDTO get_report543(NAACCriteriaFiveReportDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAACCriteriaFiveReportFacade/get_report543/");
        }
        public NAACCriteriaFiveReportDTO get_report523(NAACCriteriaFiveReportDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAACCriteriaFiveReportFacade/get_report523/");
        }
         public NAACCriteriaFiveReportDTO get_report515(NAACCriteriaFiveReportDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAACCriteriaFiveReportFacade/get_report515/");
        }
       
    }
}
