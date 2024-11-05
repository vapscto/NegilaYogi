using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using corewebapi18072016.Delegates;
using PreadmissionDTOs.NAAC.Reports;

namespace IVRMUX.Delegates.NAAC.Reports
{
    public class NAACCriteria3ReportDelegate
    {
        CommonDelegate<NAACCriteria3ReportDTO, NAACCriteria3ReportDTO> _commnbranch = new CommonDelegate<NAACCriteria3ReportDTO, NAACCriteria3ReportDTO>();

        public NAACCriteria3ReportDTO getdata(NAACCriteria3ReportDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAACCriteria3ReportFacade/getdata/");
        }
        public NAACCriteria3ReportDTO get_report(NAACCriteria3ReportDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAACCriteria3ReportFacade/get_report/");
        }
       
        public NAACCriteria3ReportDTO get_report364(NAACCriteria3ReportDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAACCriteria3ReportFacade/get_report364/");
        }
        public NAACCriteria3ReportDTO reportIPR(NAACCriteria3ReportDTO data)
        {
            return _commnbranch.naacdetailsbypost(data, "NAACCriteria3ReportFacade/reportIPR/");
        }
       
        
    }
}
