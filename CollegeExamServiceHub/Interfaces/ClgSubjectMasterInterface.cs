using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.Interfaces
{
    public interface ClgSubjectMasterInterface
    {
        MasterSubjectAllMDTO SaveMasterSubDetails(MasterSubjectAllMDTO master);
        MasterSubjectAllMDTO validateordernumber(MasterSubjectAllMDTO master);
        MasterSubjectAllMDTO GetMasterSubDetails(MasterSubjectAllMDTO master);
        MasterSubjectAllMDTO DeleteMasterSubDetails(int id);
        MasterSubjectAllMDTO EditMasterSubDetails(int id);
        MasterSubjectAllMDTO getalldetails(int id);
        MasterSubjectAllMDTO savedata2(MasterSubjectAllMDTO data);

    }
}
