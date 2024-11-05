using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library.Reports
{
    public class BookTypeReportDelegate
    {
        CommonDelegate<BookTypeReportDTO, BookTypeReportDTO> _commnbranch = new CommonDelegate<BookTypeReportDTO, BookTypeReportDTO>();
        public BookTypeReportDTO get_report(BookTypeReportDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookTypeReportFacade/get_report/");
        }
    }
}
