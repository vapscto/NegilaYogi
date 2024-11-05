using PreadmissionDTOs.com.vaps.VidyaBharathi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaBharathiServiceHub.com.vaps.Interfaces
{
   public interface VBadminInterface
    {
        VBadminDTO LoadData(VBadminDTO data);
        VBadminDTO ViewCOEDetails(VBadminDTO data);

    }
}
