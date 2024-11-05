using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
   public interface MasterSubjectInterface
    {
        MasterSubjectDTO SaveMasterSubDetails(MasterSubjectDTO master);
        MasterSubjectDTO GetMasterSubDetails(MasterSubjectDTO master);
        MasterSubjectDTO DeleteMasterSubDetails(int id);
        MasterSubjectDTO EditMasterSubDetails(int id);
    }
}
