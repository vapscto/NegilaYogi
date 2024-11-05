using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library
{
    public class NonBookTransactionReportDelegate
    {

        CommonDelegate<NonBookReport_DTO, NonBookReport_DTO> _commnbranch = new CommonDelegate<NonBookReport_DTO, NonBookReport_DTO>();


        public NonBookReport_DTO getdetails(NonBookReport_DTO id)
        {
            return _commnbranch.PostLibrary(id, "NonBookTransactionReportFacade/getdetails/");
        }

        public NonBookReport_DTO get_report(NonBookReport_DTO data)
        {
            return _commnbranch.PostLibrary(data, "NonBookTransactionReportFacade/get_report/");
        }


    }
}
