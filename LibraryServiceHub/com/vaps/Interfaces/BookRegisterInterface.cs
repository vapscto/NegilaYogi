using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface BookRegisterInterface
    {
        BookRegisterDTO getdetails(BookRegisterDTO id);
        BookRegisterDTO Ckeck_ISBNNO(BookRegisterDTO data);
        BookRegisterDTO chekAccno(BookRegisterDTO data);
        BookRegisterDTO Addaccnno(BookRegisterDTO data);
        BookRegisterDTO Savedata(BookRegisterDTO data);
        BookRegisterDTO Tab1Savedata(BookRegisterDTO data);
        BookRegisterDTO Tab2Savedata(BookRegisterDTO data);
        BookRegisterDTO Ckeck_LMBANO_AccessionNo(BookRegisterDTO data);
        Task<BookRegisterDTO> Editdata(BookRegisterDTO data);
        BookRegisterDTO deactiveY(BookRegisterDTO data);
        BookRegisterDTO searching(BookRegisterDTO data);
        BookRegisterDTO searchfilter(BookRegisterDTO data);
        Task<BookRegisterDTO> changelibrary(BookRegisterDTO data);

    }
}
