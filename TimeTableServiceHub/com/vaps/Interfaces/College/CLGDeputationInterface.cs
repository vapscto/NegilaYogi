using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{
    public  interface CLGDeputationInterface
    {
        CLGDeputationDTO savedetails(CLGDeputationDTO data);
        CLGDeputationDTO get_period_alloted(CLGDeputationDTO data);
        CLGDeputationDTO get_free_stfdets(CLGDeputationDTO data);
        CLGDeputationDTO getalldetailsviewrecords2(CLGDeputationDTO data);
        CLGDeputationDTO viewdeputation(CLGDeputationDTO data);
        CLGDeputationDTO getdetails(CLGDeputationDTO data);
        CLGDeputationDTO viewabsent(CLGDeputationDTO data);
        CLGDeputationDTO getabsentstaff(CLGDeputationDTO data);
      

    }
}
