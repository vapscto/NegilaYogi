using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library.Reports
{
    public class NonBookReportDelegate
    {
        CommonDelegate<NonBookReport_DTO, NonBookReport_DTO> _commnbranch = new CommonDelegate<NonBookReport_DTO, NonBookReport_DTO>();

        public NonBookReport_DTO getdetails(NonBookReport_DTO obj)
        {
            return _commnbranch.PostLibrary(obj, "NonBookReportFacade/getdetails/");
        }
        public NonBookReport_DTO get_report(NonBookReport_DTO data)
        {
            return _commnbranch.PostLibrary(data, "NonBookReportFacade/get_report/");
        }
    }
}
