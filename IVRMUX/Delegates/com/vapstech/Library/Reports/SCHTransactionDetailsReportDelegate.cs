using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library.Reports
{
    public class SCHTransactionDetailsReportDelegate
    {
        CommonDelegate<SCHTransactionDetailsReportDTO, SCHTransactionDetailsReportDTO> _commnbranch = new CommonDelegate<SCHTransactionDetailsReportDTO, SCHTransactionDetailsReportDTO>();


        public SCHTransactionDetailsReportDTO getdetails(SCHTransactionDetailsReportDTO id)
        {
            return _commnbranch.PostLibrary(id, "SCHTransactionDetailsReportFacade/getdetails/");
        }
        public SCHTransactionDetailsReportDTO get_report(SCHTransactionDetailsReportDTO id)
        {
            return _commnbranch.PostLibrary(id, "SCHTransactionDetailsReportFacade/get_report/");
        }

    }
}
