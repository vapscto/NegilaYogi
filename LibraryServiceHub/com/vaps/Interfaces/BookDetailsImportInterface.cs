using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface BookDetailsImportInterface
    {
        Task<BookDetailsImportDTO> save_excel_data(BookDetailsImportDTO data);
        Task<BookDetailsImportDTO> checkvalidation(BookDetailsImportDTO data);

    }
}
