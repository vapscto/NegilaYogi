﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Portals.Chirman;


namespace PortalHub.com.vaps.Chairman.Interfaces
{
  public  interface ChairmanDashboardInterface
    {

        ChairmanDashboardDTO Getdetails(ChairmanDashboardDTO data);

    }
}