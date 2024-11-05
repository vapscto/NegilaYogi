using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Portals.Chirman;


namespace PortalHub.com.vaps.Chairman.Interfaces
{
  public  interface ADMCasteStrengthInterface
    {
        
        ADMCasteStrengthDTO Getdetails(ADMCasteStrengthDTO data);
        ADMCasteStrengthDTO getclass(ADMCasteStrengthDTO data);
        ADMCasteStrengthDTO Getsection(ADMCasteStrengthDTO data);
        ADMCasteStrengthDTO Getreport(ADMCasteStrengthDTO data);
        ADMCasteStrengthDTO Getstudentdetails(ADMCasteStrengthDTO data);
        
    }
    
}
