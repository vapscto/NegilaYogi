using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Portals.Chirman;


namespace PortalHub.com.vaps.Chairman.Interfaces
{
  public  interface ExamToppersListInterface
    {
        
        ExamToppersListDTO Getdetails(ExamToppersListDTO data);
        ExamToppersListDTO getcategory(ExamToppersListDTO data);
        ExamToppersListDTO getclassexam(ExamToppersListDTO data);
        ExamToppersListDTO showreport(ExamToppersListDTO data);
        ExamToppersListDTO getsection(ExamToppersListDTO data);

        
    }
}
