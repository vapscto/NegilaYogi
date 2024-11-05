using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library.Reports
{
    public class LibTransactionReportDelegate
    {
        CommonDelegate<LibTransactionReportDTO, LibTransactionReportDTO> _commnbranch = new CommonDelegate<LibTransactionReportDTO, LibTransactionReportDTO>();


        public LibTransactionReportDTO getdetails(LibTransactionReportDTO id)
        {
            return _commnbranch.PostLibrary(id, "LibTransactionReportFacade/getdetails/");
        }
        public LibTransactionReportDTO get_report(LibTransactionReportDTO data)
        {
            return _commnbranch.PostLibrary(data, "LibTransactionReportFacade/get_report/");
        }
        public LibTransactionReportDTO CLGget_report(LibTransactionReportDTO data)
        {
            return _commnbranch.PostLibrary(data, "LibTransactionReportFacade/CLGget_report/");
        }
    }
}
