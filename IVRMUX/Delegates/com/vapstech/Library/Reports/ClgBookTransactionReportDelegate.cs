using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library.Reports
{
    public class ClgBookTransactionReportDelegate
    {

        CommonDelegate<CLGBookTransactionDTO, CLGBookTransactionDTO> _commnbranch = new CommonDelegate<CLGBookTransactionDTO, CLGBookTransactionDTO>();


        public CLGBookTransactionDTO getdetails(CLGBookTransactionDTO id)
        {
            return _commnbranch.PostLibrary(id, "ClgBookTransactionReportFacade/getdetails/");
        }
        public CLGBookTransactionDTO get_report(CLGBookTransactionDTO data)
        {
            return _commnbranch.PostLibrary(data, "ClgBookTransactionReportFacade/get_report/");
        }

    }
}
