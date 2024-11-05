using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface BookStatusDetailsInterface
    {
        BookStatusDetailsDTO getdetails(BookStatusDetailsDTO id);
      
        Task<BookStatusDetailsDTO> searchfilter(BookStatusDetailsDTO data);
        Task<BookStatusDetailsDTO> get_bookdetails(BookStatusDetailsDTO data);
    }
}
