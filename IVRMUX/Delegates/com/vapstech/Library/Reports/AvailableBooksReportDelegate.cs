using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library.Reports
{
    public class AvailableBooksReportDelegate
    {
        CommonDelegate<AvailableBooksReport_DTO, AvailableBooksReport_DTO> _commnbranch = new CommonDelegate<AvailableBooksReport_DTO, AvailableBooksReport_DTO>();


        public AvailableBooksReport_DTO getdetails(AvailableBooksReport_DTO id)
        {
            return _commnbranch.PostLibrary(id, "AvailableBooksReportFacade/getdetails/");
        }
        public AvailableBooksReport_DTO get_report(AvailableBooksReport_DTO id)
        {
            return _commnbranch.PostLibrary(id, "AvailableBooksReportFacade/get_report/");
        }

    }
}
