using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
   public interface BranchChangeInterface
    {
        BranchChangeDTO getdetails(BranchChangeDTO data);
        BranchChangeDTO Studentdetails(BranchChangeDTO data);
        BranchChangeDTO Savedetails(BranchChangeDTO data);
        BranchChangeDTO deactive(BranchChangeDTO data);
    }
}
