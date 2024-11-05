using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library.Reports
{
    public class BookCirculationReportDelegate
    {
        CommonDelegate<BookCirculationReportDTO, BookCirculationReportDTO> _commnbranch = new CommonDelegate<BookCirculationReportDTO, BookCirculationReportDTO>();


        public BookCirculationReportDTO getdetails(BookCirculationReportDTO id)
        {
            return _commnbranch.PostLibrary(id, "BookCirculationReportFacade/getdetails/");
        }

        public BookCirculationReportDTO getstuddetails(BookCirculationReportDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookCirculationReportFacade/getstuddetails/");
        }
        public BookCirculationReportDTO get_report(BookCirculationReportDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookCirculationReportFacade/get_report/");
        }
    }
}
