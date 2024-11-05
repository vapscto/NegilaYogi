using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface ClgyearlycoursemappingInterface
    {
        ClgyearlycoursemappingDTO getalldetails(ClgyearlycoursemappingDTO data);
        ClgyearlycoursemappingDTO getbranches(ClgyearlycoursemappingDTO data);
        ClgyearlycoursemappingDTO getsemisters(ClgyearlycoursemappingDTO data);
        ClgyearlycoursemappingDTO savedata(ClgyearlycoursemappingDTO data);
        ClgyearlycoursemappingDTO searchdata(ClgyearlycoursemappingDTO data);
        ClgyearlycoursemappingDTO viewrecordspopup(ClgyearlycoursemappingDTO data);
        

    }
}
