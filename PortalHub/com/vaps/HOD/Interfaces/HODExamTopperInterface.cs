using PreadmissionDTOs.com.vaps.Portals.HOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.HOD.Interfaces
{
   public interface HODExamTopperInterface
    {
        HODExamTopper_DTO Getdetails(HODExamTopper_DTO data);
        HODExamTopper_DTO getcategory(HODExamTopper_DTO data);
        HODExamTopper_DTO getclassexam(HODExamTopper_DTO data);
        HODExamTopper_DTO showreport(HODExamTopper_DTO data);
        HODExamTopper_DTO getsection(HODExamTopper_DTO data);

    }
}
