using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Portals.Chirman;


namespace PortalHub.com.vaps.Chairman.Interfaces
{
  public  interface ExamHomeInterface
    {
        
        ExamHomeDTO Getdetails(ExamHomeDTO data);
        ExamHomeDTO getcategory(ExamHomeDTO data);
        ExamHomeDTO getclassexam(ExamHomeDTO data);
        ExamHomeDTO showreport(ExamHomeDTO data);
        ExamHomeDTO showsectioncount(ExamHomeDTO data);

        
    }
}
