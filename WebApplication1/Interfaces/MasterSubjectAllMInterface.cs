using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
   public interface MasterSubjectAllMInterface
    {
        MasterSubjectAllMDTO SaveMasterSubDetails(MasterSubjectAllMDTO master);
        MasterSubjectAllMDTO validateordernumber(MasterSubjectAllMDTO master);
        MasterSubjectAllMDTO GetMasterSubDetails(MasterSubjectAllMDTO master);
        MasterSubjectAllMDTO DeleteMasterSubDetails(int id);
        MasterSubjectAllMDTO EditMasterSubDetails(int id);
        MasterSubjectAllMDTO getalldetails(int id);
    }
}
