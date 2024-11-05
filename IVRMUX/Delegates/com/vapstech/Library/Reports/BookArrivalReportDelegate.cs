using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library.Reports
{
    public class BookArrivalReportDelegate
    {
        CommonDelegate<BookArrivalReportDTO, BookArrivalReportDTO> _commnbranch = new CommonDelegate<BookArrivalReportDTO, BookArrivalReportDTO>();


        public BookArrivalReportDTO getdetails(BookArrivalReportDTO id)
        {
            return _commnbranch.PostLibrary(id, "BookArrivalReportFacade/getdetails/");
        }
        public BookArrivalReportDTO get_report(BookArrivalReportDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookArrivalReportFacade/get_report/");
        }
    }
}
