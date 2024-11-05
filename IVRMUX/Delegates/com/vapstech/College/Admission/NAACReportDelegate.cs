using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Admission
{
    public class NAACReportDelegate
    {

        CommonDelegate<NAACReportDTO, NAACReportDTO> common = new CommonDelegate<NAACReportDTO, NAACReportDTO>();

        public NAACReportDTO getdetails(NAACReportDTO id)
        {
            return common.clgadmissionbypost(id, "NAACReportFacade/getdetails/");
        }
        public NAACReportDTO onreport(NAACReportDTO id)
        {
            return common.clgadmissionbypost(id, "NAACReportFacade/onreport/");
        }
    }
}
