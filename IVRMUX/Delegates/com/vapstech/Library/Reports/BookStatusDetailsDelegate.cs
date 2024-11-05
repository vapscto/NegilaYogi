using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library.Reports
{
    public class BookStatusDetailsDelegate
    {
        CommonDelegate<BookStatusDetailsDTO, BookStatusDetailsDTO> _commnbranch = new CommonDelegate<BookStatusDetailsDTO, BookStatusDetailsDTO>();

        public BookStatusDetailsDTO getdetails(BookStatusDetailsDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookStatusDetailsFacde/getdetails/");
        }
        public BookStatusDetailsDTO get_report(BookStatusDetailsDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookStatusDetailsFacde/get_report/");
        }
        public BookStatusDetailsDTO searchfilter(BookStatusDetailsDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookStatusDetailsFacde/searchfilter/");
        }
        public BookStatusDetailsDTO get_bookdetails(BookStatusDetailsDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookStatusDetailsFacde/get_bookdetails/");
        }
    }
}
