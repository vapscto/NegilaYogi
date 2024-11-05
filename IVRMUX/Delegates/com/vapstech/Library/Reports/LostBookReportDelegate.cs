using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library.Reports
{
    public class LostBookReportDelegate
    {

        CommonDelegate<LostBookReport_DTO, LostBookReport_DTO> _commnbranch = new CommonDelegate<LostBookReport_DTO, LostBookReport_DTO>();


        public LostBookReport_DTO getdetails(LostBookReport_DTO id)
        {
            return _commnbranch.PostLibrary(id, "LostBookReportFacade/getdetails/");
        }
        public LostBookReport_DTO get_report(LostBookReport_DTO data)
        {
            return _commnbranch.PostLibrary(data, "LostBookReportFacade/get_report/");
        }

    }
}
