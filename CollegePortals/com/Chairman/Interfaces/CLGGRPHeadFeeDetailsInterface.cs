﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Portals;

namespace CollegePortals.com.Chairman.Interfaces
{
    public interface CLGGRPHeadFeeDetailsInterface
    {
        Task<CLGGRPHeadFeeDetailsDTO> Getdetails(CLGGRPHeadFeeDetailsDTO data);
        
    }

}
