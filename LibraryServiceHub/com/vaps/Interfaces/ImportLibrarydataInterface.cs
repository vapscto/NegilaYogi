using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface ImportLibrarydataInterface
    {
        ImportLibrarydataDTO Savedata(ImportLibrarydataDTO data);
        ImportLibrarydataDTO getdetails(int id);
        ImportLibrarydataDTO deactiveY(ImportLibrarydataDTO data);
    }
}
