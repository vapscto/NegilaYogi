﻿using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
   public interface SlabWiseExamReportinterface
    {
        SlabWiseExamReportDTO getsubjects(SlabWiseExamReportDTO data);
        SlabWiseExamReportDTO getslabreport(SlabWiseExamReportDTO data);
    }
}
