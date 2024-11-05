using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
  public  interface ClasswisestudentdetailsInterface
    {
        ClasswisestudentdetailsDTO getdetails(ClasswisestudentdetailsDTO id);
        Task<ClasswisestudentdetailsDTO> Getreportdetails(ClasswisestudentdetailsDTO data);
        ClasswisestudentdetailsDTO getsection(ClasswisestudentdetailsDTO id);
        ClasswisestudentdetailsDTO fetchclassbyYearId(ClasswisestudentdetailsDTO id);
        
    }
}
