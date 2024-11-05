using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Portals;

namespace CollegePortals.com.Student.Interfaces
{
    public interface ClgFeeDetailsInterface
    {
       ClgPortalFeeDTO getloaddata(ClgPortalFeeDTO data);
        Task<ClgPortalFeeDTO> Getdetails(ClgPortalFeeDTO data);
    }
}
