using PreadmissionDTOs.com.vaps.VidyaBharathi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaBharathiServiceHub.com.vaps.Interfaces
{
   public interface VBSC_Events_Category_StudentsInterface
    {
        VBSC_Events_Category_StudentsDTO loaddata(VBSC_Events_Category_StudentsDTO data);
        VBSC_Events_Category_StudentsDTO savedata(VBSC_Events_Category_StudentsDTO data);
        VBSC_Events_Category_StudentsDTO deactive(VBSC_Events_Category_StudentsDTO data);
  
    }
}
