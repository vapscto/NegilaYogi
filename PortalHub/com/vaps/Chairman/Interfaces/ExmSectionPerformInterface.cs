using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Portals.Chirman;


namespace PortalHub.com.vaps.Chairman.Interfaces
{
  public  interface ExmSectionPerformInterface
    {
        
        ExmSectionPerformDTO Getdetails(ExmSectionPerformDTO data);
        ExmSectionPerformDTO getcategory(ExmSectionPerformDTO data);
        ExmSectionPerformDTO getclassexam(ExmSectionPerformDTO data);
        ExmSectionPerformDTO showreport(ExmSectionPerformDTO data);
        ExmSectionPerformDTO getsubject(ExmSectionPerformDTO data);

        
    }
}
