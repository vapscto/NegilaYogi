using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library
{
    public class LibraryMonthEndReportDelegate
    {
        CommonDelegate<LibraryMonthEndReportDTO, LibraryMonthEndReportDTO> _commbranch = new CommonDelegate<LibraryMonthEndReportDTO, LibraryMonthEndReportDTO>();

        public LibraryMonthEndReportDTO getdetails(int id)
        {
            return _commbranch.GETLibrary(id, "LibraryMonthEndReportFacade/getdetails/");
        }
        public LibraryMonthEndReportDTO Savedata(LibraryMonthEndReportDTO data)
        {
            return _commbranch.PostLibrary(data, "LibraryMonthEndReportFacade/Savedata/");
        }
     
    }
}
