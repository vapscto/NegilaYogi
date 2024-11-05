using PreadmissionDTOs.com.vaps.VidyaBharathi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaBharathiServiceHub.com.vaps.Interfaces
{
   public interface VBSC_Events_CategoryInterface
    {
        VBSC_Events_CategoryDTO loaddata(VBSC_Events_CategoryDTO data);
        VBSC_Events_CategoryDTO savedata(VBSC_Events_CategoryDTO data);
        VBSC_Events_CategoryDTO deactive(VBSC_Events_CategoryDTO data);
  
    }
}
