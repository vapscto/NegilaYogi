﻿using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{
   public interface CLGTTCollegewiseConsolidatedReportInterface
    {
        CLGTTCollegewiseConsolidatedReportDTO loaddata(CLGTTCollegewiseConsolidatedReportDTO data);
        CLGTTCollegewiseConsolidatedReportDTO report(CLGTTCollegewiseConsolidatedReportDTO data);


    }
}
