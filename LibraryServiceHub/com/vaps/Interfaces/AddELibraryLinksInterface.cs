using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface AddELibraryLinksInterface
    {
        AddELibraryLinksDTO Savedata(AddELibraryLinksDTO data);
        AddELibraryLinksDTO getdetails(int id);
        AddELibraryLinksDTO GetELibrary(int id);
        AddELibraryLinksDTO deactiveY( AddELibraryLinksDTO data);
        AddELibraryLinksDTO geteditdata(AddELibraryLinksDTO data);
    }
}
