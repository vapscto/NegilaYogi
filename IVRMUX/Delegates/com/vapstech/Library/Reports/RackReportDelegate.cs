using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library.Reports
{
    public class RackReportDelegate
    {
        CommonDelegate<RackReportDTO, RackReportDTO> _commnbranch = new CommonDelegate<RackReportDTO, RackReportDTO>();

        public RackReportDTO getdetails(RackReportDTO data)
        {
            return _commnbranch.PostLibrary(data, "RackReportFacde/getdetails/");
        }
        public RackReportDTO get_report(RackReportDTO data)
        {
            return _commnbranch.PostLibrary(data, "RackReportFacde/get_report/");
        }
    }
}
