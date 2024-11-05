using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.Interfaces
{
  public  interface ClgmastersubsubjectInterface
    {
        mastersubsubjectDTO deactivate(mastersubsubjectDTO data);

        mastersubsubjectDTO editdeatils(int ID);
        mastersubsubjectDTO savedetails(mastersubsubjectDTO data);
        mastersubsubjectDTO validateordernumber(mastersubsubjectDTO data);

        mastersubsubjectDTO Getdetails(mastersubsubjectDTO data);
    }
}
