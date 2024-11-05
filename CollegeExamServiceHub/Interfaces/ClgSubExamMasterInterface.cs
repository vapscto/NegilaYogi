using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.Interfaces
{
  public interface ClgSubExamMasterInterface
    {
        mastersubexamDTO savedetails(mastersubexamDTO data);
        mastersubexamDTO validateordernumber(mastersubexamDTO data);

        mastersubexamDTO deactivate(mastersubexamDTO data);

        mastersubexamDTO editdetails(int ID);
        mastersubexamDTO Getdetails(mastersubexamDTO data);

    }
}
