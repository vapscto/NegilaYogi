using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
    public interface ConsolidatesRankReportInterface
    {
        WrittenTestMarksBindDataDTO Getdetails(WrittenTestMarksBindDataDTO mas);
        WrittenTestMarksBindDataDTO getclass(WrittenTestMarksBindDataDTO data);
        WrittenTestMarksBindDataDTO Getreport(WrittenTestMarksBindDataDTO data);
    }
}
