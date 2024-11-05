using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;


namespace WebApplication1.Interfaces
{
    public interface enquiryreportInterface
    {
        // Enq getenqSearchedDetails(WrittenTestMarksBindDataDTO data);
        Task<WrittenTestMarksBindDataDTO> searchenquiry(WrittenTestMarksBindDataDTO dto);
        WrittenTestMarksBindDataDTO getenqyearlist(WrittenTestMarksBindDataDTO data);

    }
}
