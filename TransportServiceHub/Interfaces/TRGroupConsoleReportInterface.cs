﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Transport;

namespace TransportServiceHub.Interfaces
{
 public   interface TRGroupConsoleReportInterface
    {
        TripReportDTO getdata(int id);
        TripReportDTO Getreportdetails(TripReportDTO data);
    }
}