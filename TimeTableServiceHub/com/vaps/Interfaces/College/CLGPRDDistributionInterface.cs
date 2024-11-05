using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.Interfaces
{
    public interface CLGPRDDistributionInterface
    {
      
        CLGPRDDistributionDTO editprddestr(CLGPRDDistributionDTO data);
        CLGPRDDistributionDTO deactivate(CLGPRDDistributionDTO data);
        CLGPRDDistributionDTO deactivecrsday(CLGPRDDistributionDTO data);
        CLGPRDDistributionDTO getalldetails(CLGPRDDistributionDTO data);
        CLGPRDDistributionDTO viewperiods(CLGPRDDistributionDTO data);
        CLGPRDDistributionDTO getBranch(CLGPRDDistributionDTO data);
        CLGPRDDistributionDTO savedetail(CLGPRDDistributionDTO data);
       
    }
}
