
using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface MasterSubjectGroupInterface
    {
        MasterSubjectGroupDTO savedetail(MasterSubjectGroupDTO objcategory);
        MasterSubjectGroupDTO deactivate(MasterSubjectGroupDTO data);
        MasterSubjectGroupDTO getdetails(int id);
        MasterSubjectGroupDTO getpageedit(int id);
        MasterSubjectGroupDTO getalldetailsviewrecords(int id);
        MasterSubjectGroupDTO deleterec(int id);


    }
}
