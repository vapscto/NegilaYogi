using CommonLibrary;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Library
{
    public class BookDetailsImportDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<BookDetailsImportDTO, BookDetailsImportDTO> _commnbranch = new CommonDelegate<BookDetailsImportDTO, BookDetailsImportDTO>();

        public BookDetailsImportDTO save_excel_data(BookDetailsImportDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookDetailsImportFacade/save_excel_data/");
        }

        public BookDetailsImportDTO checkvalidation(BookDetailsImportDTO data)
        {
            return _commnbranch.PostLibrary(data, "BookDetailsImportFacade/checkvalidation/");
        }
    }
}
