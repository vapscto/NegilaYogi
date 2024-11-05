using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library.Reports
{
    public class BookRegisterReportDelegate
    {
        CommonDelegate<BookRegisterReportDTO, BookRegisterReportDTO> _commnbranch = new CommonDelegate<BookRegisterReportDTO, BookRegisterReportDTO>();


        public BookRegisterReportDTO getdetails(int id)
        {
            return _commnbranch.GETLibrary(id, "BookRegisterReportFacade/getdetails/");
        }
        public BookRegisterReportDTO get_report(BookRegisterReportDTO id)
        {
            return _commnbranch.PostLibrary(id, "BookRegisterReportFacade/get_report/");
        }
        //BarCode
        public BookRegisterReportDTO BarCode(BookRegisterReportDTO id)
        {
            return _commnbranch.PostLibrary(id, "BookRegisterReportFacade/BarCode/");
        }
    }
}
