using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
    public interface MasterNonBookInterface
    {
        Task<MatserNonBook_DTO> getdetails(MatserNonBook_DTO id);
        MatserNonBook_DTO Savedata(MatserNonBook_DTO data);
        Task<MatserNonBook_DTO> Editdata(MatserNonBook_DTO data);
        MatserNonBook_DTO deactiveY(MatserNonBook_DTO data);
        MatserNonBook_DTO searching(MatserNonBook_DTO data);
        Task<MatserNonBook_DTO> changelibrary(MatserNonBook_DTO data);

    }
}
